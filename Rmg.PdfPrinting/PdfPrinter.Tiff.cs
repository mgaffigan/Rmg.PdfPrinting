using System.IO;
using Windows.Data.Pdf;
using Windows.Storage;
using Windows.Win32;
using Windows.Win32.Graphics.Direct2D;
using Windows.Win32.Graphics.Direct2D.Common;
using Windows.Win32.Graphics.Imaging;
using Windows.Win32.Graphics.Dxgi.Common;
using Windows.Win32.System.Com.StructuredStorage;
using WinRTSize = Windows.Foundation.Size;

namespace Rmg.PdfPrinting;

public partial class PdfPrinter
{
    public async Task ConvertToTiff(string pdfPath, string tiffPath)
    {
        var pdfFile = await StorageFile.GetFileFromPathAsync(pdfPath);
        var pdfDoc = await PdfDocument.LoadFromFileAsync(pdfFile);
        using var tiffStream = File.Open(tiffPath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);

        ConvertToTiff(pdfDoc, tiffStream);
    }

    public unsafe void ConvertToTiff(PdfDocument pdfDoc, Stream tiffStream)
    {
        var encoder = CreateEncoder(tiffStream, PInvoke.GUID_ContainerFormatTiff);

        // Open the PDF Document
        PInvoke.PdfCreateRenderer(dxgiDevice, out var pPdfRendererNative).ThrowOnFailure();

        // Write pages
        for (uint pageIndex = 0; pageIndex < pdfDoc.PageCount; pageIndex++)
        {
            var page = pdfDoc.GetPage(pageIndex);
            var cl = RenderPageToCommandList(pPdfRendererNative, page);
            RenderCommandListToFrame(cl, page.Size, encoder);
        }

        encoder.Commit();
    }

    private unsafe void RenderCommandListToFrame(
        ID2D1Image img, WinRTSize size,
        IWICBitmapEncoder encoder)
    {
        // binarize if needed
        if (printOpts.IsBlackAndWhite)
        {
            img = Binarize(img);
        }

        // render to bitmap
        IPropertyBag2? frameOpts = null;
        encoder.CreateNewFrame(out var frame, ref frameOpts);
        frameOpts.SetProperty("TiffCompressionMethod", printOpts.IsBlackAndWhite
            ? (byte)WICTiffCompressionOption.WICTiffCompressionCCITT4
            : (byte)WICTiffCompressionOption.WICTiffCompressionZIP);
        frame.Initialize(frameOpts);
        pWic.CreateImageEncoder(d2dDevice, out var imageEncoder);

        var imageParams = new WICImageParameters();
        var dpi = printOpts.OutputDpi;
        imageParams.DpiX = imageParams.DpiY = (float)dpi;
        imageParams.PixelWidth = (uint)Math.Round(size.Width * dpi / 96f);
        imageParams.PixelHeight = (uint)Math.Round(size.Height * dpi / 96f);
        imageParams.PixelFormat.format = DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM;
        imageParams.PixelFormat.alphaMode = D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_PREMULTIPLIED;
        imageEncoder.WriteFrame(img, frame, imageParams);
        frame.Commit();
    }

    private ID2D1Image Binarize(ID2D1Image img)
    {
        // Is this needlessly stupid? Yes.  Can I find a better way without writing a custom
        // effect? No.

        // downconvert to grayscale
        d2dContextForPrint.CreateEffect(PInvoke.CLSID_D2D1Grayscale, out var colorMatrix);
        colorMatrix.SetInput(0, img, false);
        colorMatrix.GetOutput(out img);

        // then binarize with a threshold of approximately 0.5
        d2dContextForPrint.CreateEffect(PInvoke.CLSID_D2D1DiscreteTransfer, out var effect);
        effect.SetInput(0, img, false);
        var table = new float[] { 0f, 1f };
        effect.SetValue(D2D1_DISCRETETRANSFER_PROP.D2D1_DISCRETETRANSFER_PROP_RED_TABLE, table);
        effect.SetValue(D2D1_DISCRETETRANSFER_PROP.D2D1_DISCRETETRANSFER_PROP_GREEN_TABLE, table);
        effect.SetValue(D2D1_DISCRETETRANSFER_PROP.D2D1_DISCRETETRANSFER_PROP_BLUE_TABLE, table);
        effect.GetOutput(out img);
        
        return img;
    }

    private unsafe IWICBitmapEncoder CreateEncoder(Stream stream, Guid format)
    {
        var encoder = this.pWic.CreateEncoder(&format, null);
        encoder.Initialize(new ManagedIStream(stream), WICBitmapEncoderCacheOption.WICBitmapEncoderNoCache);
        return encoder;
    }
}
