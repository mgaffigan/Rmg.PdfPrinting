﻿using System.IO;
using Windows.Data.Pdf;
using Windows.Storage;
using Windows.Win32.Graphics.Direct2D;

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
        var printControlOptions = GetPrintControlProperties();
        d2dDevice.CreatePrintControl(pWic, target, &printControlOptions, out var printControl);

        RenderDocToPrintControl(pdfDoc, printControl);
    }
}
