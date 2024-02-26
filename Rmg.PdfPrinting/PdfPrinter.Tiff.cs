using System;
using System.IO;
using System.Printing;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using Windows.Data.Pdf;
using Windows.Devices.HumanInterfaceDevice;
using Windows.Devices.I2c;
using Windows.Storage;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;
using Windows.Win32.Graphics.Direct2D;
using Windows.Win32.Graphics.Direct3D;
using Windows.Win32.Graphics.Direct3D11;
using Windows.Win32.Graphics.Dxgi;
using Windows.Win32.Graphics.Imaging.D2D;
using Windows.Win32.Storage.Xps.Printing;
using Windows.Win32.System.WinRT.Pdf;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using Windows.Win32.Graphics.Direct2D.Common;
using Windows.Win32.Graphics.Imaging;
using Windows.Win32.Graphics.DirectWrite;
using Windows.Win32.Graphics.Dxgi.Common;
using Windows.Win32.System.Com.StructuredStorage;
using WinRTSize = Windows.Foundation.Size;

namespace Rmg.PdfPrinting;

public partial class PdfPrinter
{
    public async Task ConvertToTiff(string pdfPath, string tiffPath, TiffConversionOptions? options = null)
    {
        var pdfFile = await StorageFile.GetFileFromPathAsync(pdfPath);
        var pdfDoc = await PdfDocument.LoadFromFileAsync(pdfFile);
        using var tiffStream = File.Open(tiffPath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);

        ConvertToTiff(pdfDoc, tiffStream, options);
    }

    public unsafe void ConvertToTiff(PdfDocument pdfDoc, Stream tiffStream, TiffConversionOptions? options = null)
    {
        options ??= new TiffConversionOptions();
        var encoder = CreateEncoder(tiffStream, PInvoke.GUID_ContainerFormatTiff);

        IDWriteRenderingParams? textParams = null;
        if (options.BW)
        {
            dwrite.CreateCustomRenderingParams(1, 0, 0,
                DWRITE_PIXEL_GEOMETRY.DWRITE_PIXEL_GEOMETRY_FLAT,
                DWRITE_RENDERING_MODE.DWRITE_RENDERING_MODE_ALIASED,
                out textParams);
        }

        // Open the PDF Document
        PInvoke.PdfCreateRenderer(dxgiDevice, out var pPdfRendererNative).ThrowOnFailure();

        // Write pages
        for (uint pageIndex = 0; pageIndex < pdfDoc.PageCount; pageIndex++)
        {
            var page = pdfDoc.GetPage(pageIndex);
            var (cl, size) = RenderPageToCommandList(pPdfRendererNative, page, textParams, options);
            RenderCommandListToFrame(cl, size, encoder, options);
        }

        encoder.Commit();
    }

    private unsafe (ID2D1CommandList CommandList, WinRTSize Size) RenderPageToCommandList(
        IPdfRendererNative pPdfRendererNative, PdfPage pdfPage,
        IDWriteRenderingParams? textParams, TiffConversionOptions options)
    {
        d2dContextForPrint.CreateCommandList(out var commandList);
        d2dContextForPrint.SetTarget(commandList);
        if (options.BW)
        {
            d2dContextForPrint.SetAntialiasMode(D2D1_ANTIALIAS_MODE.D2D1_ANTIALIAS_MODE_ALIASED);
            d2dContextForPrint.SetTextRenderingParams(textParams ?? throw new ArgumentNullException(nameof(textParams)));
        }

        d2dContextForPrint.BeginDraw();
        {
            if (options.FillBackground)
            {
                d2dContextForPrint.Clear(new D2D1_COLOR_F() { r = 1f, g = 1f, b = 1f, a = 1f });
            }
            var pdfContext = !options.BW ? d2dContextForPrint : d2dContextForPrint;

            var renderParams = new PDF_RENDER_PARAMS();
            renderParams.IgnoreHighContrast = true;
            pPdfRendererNative.RenderPageToDeviceContext(pdfPage, pdfContext, &renderParams);
        }
        d2dContextForPrint.EndDraw();

        commandList.Close();
        return (commandList, pdfPage.Size);
    }

    private unsafe void RenderCommandListToFrame(
        ID2D1CommandList commandList, WinRTSize size,
        IWICBitmapEncoder encoder, TiffConversionOptions options)
    {
        // render to bitmap
        IPropertyBag2? frameOpts = null;
        encoder.CreateNewFrame(out var frame, ref frameOpts);
        frameOpts.SetProperty("TiffCompressionMethod", options.BW
            ? (byte)WICTiffCompressionOption.WICTiffCompressionCCITT4
            : (byte)WICTiffCompressionOption.WICTiffCompressionZIP);
        frame.Initialize(frameOpts);
        pWic.CreateImageEncoder(d2dDevice, out var imageEncoder);

        var imageParams = new WICImageParameters();
        var dpi = options.Dpi;
        imageParams.DpiX = imageParams.DpiY = dpi;
        imageParams.PixelWidth = (uint)Math.Round(size.Width * dpi / 96f);
        imageParams.PixelHeight = (uint)Math.Round(size.Height * dpi / 96f);
        imageParams.PixelFormat.format = DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM;
        imageParams.PixelFormat.alphaMode = D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_PREMULTIPLIED;
        imageEncoder.WriteFrame(commandList, frame, imageParams);
        frame.Commit();
    }

    private unsafe IWICBitmapEncoder CreateEncoder(Stream stream, Guid format)
    {
        var encoder = this.pWic.CreateEncoder(&format, null);
        encoder.Initialize(new ManagedIStream(stream), WICBitmapEncoderCacheOption.WICBitmapEncoderNoCache);
        return encoder;
    }
}

public record TiffConversionOptions(bool FillBackground = true, float Dpi = 300f, TiffColorMode ColorMode = TiffColorMode.Default)
{
    internal bool BW => ColorMode == TiffColorMode.BlackAndWhite;
}
public enum TiffColorMode
{
    Default,
    BlackAndWhite
}
