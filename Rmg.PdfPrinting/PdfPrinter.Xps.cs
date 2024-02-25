using System.IO;
using System.Printing;
using Windows.Data.Pdf;
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
using Windows.Win32.Graphics.Direct2D.Common;

namespace Rmg.PdfPrinting;

public partial class PdfPrinter
{
    public async Task ConvertToXps(string pdfPath, string xpsPath)
    {
        var pdfFile = await StorageFile.GetFileFromPathAsync(pdfPath);
        var pdfDoc = await PdfDocument.LoadFromFileAsync(pdfFile);
        using var xpsFile = File.Open(xpsPath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);

        ConvertToXps(pdfDoc, xpsFile);
    }

    public unsafe void ConvertToXps(PdfDocument pdfDoc, Stream xpsStream)
    {
        var target = new XpsPrintDocumentPackageTarget(new ManagedIStream(xpsStream));
        d2dDevice.CreatePrintControl(pWic, target, null, out var printControl);

        RenderDocToPrintControl(pdfDoc, printControl);
    }
}
