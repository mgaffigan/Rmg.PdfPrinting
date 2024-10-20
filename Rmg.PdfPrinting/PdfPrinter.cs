﻿using System.IO;
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
using Windows.Win32.Graphics.DirectWrite;
using Windows.Win32.Graphics.Direct2D.Common;

namespace Rmg.PdfPrinting;

public partial class PdfPrinter
{
    private readonly ID3D11Device d3dDevice;
    private readonly IDXGIDevice dxgiDevice;
    private readonly ID2D1Factory1 d2dFactory;
    private readonly IWICImagingFactory2 pWic;
    private readonly IDWriteFactory dwrite;
    private readonly ID2D1Device d2dDevice;
    private readonly ID2D1DeviceContext d2dContextForPrint;
    private readonly IDWriteRenderingParams? textParams;

    private PdfPrinterOptions printOpts;

    public PdfPrinter()
        : this(new PdfPrinterOptions())
    {
        // nop
    }

    public unsafe PdfPrinter(PdfPrinterOptions printOpts)
    {
        this.printOpts = printOpts;

        // d3d
        PInvoke.D3D11CreateDevice(null, D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_HARDWARE, HMODULE.Null,
            D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_BGRA_SUPPORT,
            null, 0, 7 /* D3D11_SDK_VERSION */, out d3dDevice, null, out var d3dContext)
            .ThrowOnFailure();
        dxgiDevice = (IDXGIDevice)d3dDevice;

        // d2d
        var factoryOptions = new D2D1_FACTORY_OPTIONS();
#if DEBUG
        factoryOptions.debugLevel = D2D1_DEBUG_LEVEL.D2D1_DEBUG_LEVEL_INFORMATION;
#endif
        PInvoke.D2D1CreateFactory(D2D1_FACTORY_TYPE.D2D1_FACTORY_TYPE_MULTI_THREADED, typeof(ID2D1Factory1).GUID, factoryOptions, out var od2dFactory).ThrowOnFailure();
        d2dFactory = (ID2D1Factory1)od2dFactory;
        d2dFactory.CreateDevice(dxgiDevice, out d2dDevice);
        d2dDevice.CreateDeviceContext(D2D1_DEVICE_CONTEXT_OPTIONS.D2D1_DEVICE_CONTEXT_OPTIONS_NONE, out d2dContextForPrint);

        // wic
        pWic = (IWICImagingFactory2)new WICImagingFactory();

        // dwrite
        PInvoke.DWriteCreateFactory(DWRITE_FACTORY_TYPE.DWRITE_FACTORY_TYPE_SHARED, typeof(IDWriteFactory).GUID, out var oDwrite).ThrowOnFailure();
        dwrite = (IDWriteFactory)oDwrite;

        // text rendering params
        if (printOpts.IsBlackAndWhite)
        {
            dwrite.CreateCustomRenderingParams(1, 0, 0,
                DWRITE_PIXEL_GEOMETRY.DWRITE_PIXEL_GEOMETRY_FLAT,
                DWRITE_RENDERING_MODE.DWRITE_RENDERING_MODE_ALIASED,
                out textParams);
        }
    }

    public Task Print(string printerName, string fileName, string? jobName = null, PrintTicket? ticket = null)
        => Print(printerName, fileName, jobName, ticket?.GetXmlStream());
    public async Task Print(string printerName, string fileName, string? jobName, Stream? ticket)
    {
        if (string.IsNullOrWhiteSpace(jobName))
        {
            jobName = Path.GetFileNameWithoutExtension(fileName);
        }
        var pdfFile = await StorageFile.GetFileFromPathAsync(fileName);
        var pdfDoc = await PdfDocument.LoadFromFileAsync(pdfFile);

        await Print(printerName, pdfDoc, jobName, ticket);
    }

    public unsafe Task Print(string printerName, PdfDocument pdfDoc, string jobName = "PDF", PrintTicket? ticket = null)
        => Print(printerName, pdfDoc, jobName, ticket?.GetXmlStream());
    public unsafe Task Print(string printerName, PdfDocument pdfDoc, string jobName, Stream? ticket)
    {
        // Initialize the job
        ID2D1PrintControl printControl;
        PrintCompletionSource listener;
        {
            // Create a factory for document print job.
            var documentTargetFactory = (IPrintDocumentPackageTargetFactory)new PrintDocumentPackageTargetFactory();
            var ticketStream = ticket is null ? null : new ManagedIStream(ticket);
            documentTargetFactory.CreateDocumentPackageTargetForPrintJob(
                printerName, jobName, null, ticketStream, out var docTarget);

            // wait for completion
            var cpc = (IConnectionPointContainer)docTarget;
            cpc.FindConnectionPoint(typeof(IPrintDocumentPackageStatusEvent).GUID, out var cp);
            listener = new PrintCompletionSource(cp);

            // Create a new print control linked to the package target.
            var printControlOptions = new D2D1_PRINT_CONTROL_PROPERTIES();
            printControlOptions.rasterDPI = (float)printOpts.RasterDpi;
            d2dDevice.CreatePrintControl(pWic, docTarget, printControlOptions, out printControl);
        }

        RenderDocToPrintControl(pdfDoc, printControl);

        return listener.Task;
    }

    private unsafe void RenderDocToPrintControl(PdfDocument pdfDoc, ID2D1PrintControl printControl)
    {
        // Open the PDF Document
        PInvoke.PdfCreateRenderer(dxgiDevice, out var pPdfRendererNative).ThrowOnFailure();

        // Write pages
        for (uint pageIndex = 0; pageIndex < pdfDoc.PageCount; pageIndex++)
        {
            var page = pdfDoc.GetPage(pageIndex);
            var cl = RenderPageToCommandList(pPdfRendererNative, page);
            printControl.AddPage(cl, page.Size, null);
        }

        printControl.Close();
    }

    private unsafe ID2D1CommandList RenderPageToCommandList(
        IPdfRendererNative pPdfRendererNative, PdfPage pdfPage)
    {
        d2dContextForPrint.CreateCommandList(out var commandList);
        d2dContextForPrint.SetTarget(commandList);
        if (printOpts.IsBlackAndWhite)
        {
            d2dContextForPrint.SetAntialiasMode(D2D1_ANTIALIAS_MODE.D2D1_ANTIALIAS_MODE_ALIASED);
            d2dContextForPrint.SetTextRenderingParams(textParams ?? throw new InvalidOperationException("No textParams found"));
        }

        d2dContextForPrint.BeginDraw();
        DrawPage(d2dContextForPrint, pPdfRendererNative, pdfPage);
        d2dContextForPrint.EndDraw();

        commandList.Close();
        return commandList;
    }

    private unsafe void DrawPage(ID2D1DeviceContext d2dContextForPrint, IPdfRendererNative pPdfRendererNative, PdfPage pdfPage)
    {
        if (printOpts.FillBackground || printOpts.IsBlackAndWhite)
        {
            d2dContextForPrint.Clear(new D2D1_COLOR_F() { r = 1f, g = 1f, b = 1f, a = 1f });
        }

        var renderParams = new PDF_RENDER_PARAMS();
        renderParams.IgnoreHighContrast = true;

        // This seems to be ignored by the PDF renderer
        d2dContextForPrint.SetDpi((float)printOpts.RasterDpi, (float)printOpts.RasterDpi);
        // Instead we'll transform so that 1 unit = 1px
        d2dContextForPrint.SetTransform(D2D_MATRIX_3X2_F.Scale((float)(96d / printOpts.RasterDpi), (float)(96 / printOpts.RasterDpi)));
        renderParams.DestinationWidth = (uint)(pdfPage.Size.Width * (printOpts.RasterDpi / 96d));
        renderParams.DestinationHeight = (uint)(pdfPage.Size.Height * (printOpts.RasterDpi / 96d));

        var pdfContext = !printOpts.IsBlackAndWhite ? (ID2D1DeviceContext)d2dContextForPrint
            : new OneBppDeviceContext(d2dContextForPrint, FilterOneBppImage);
        pPdfRendererNative.RenderPageToDeviceContext(pdfPage, pdfContext, &renderParams);

        // revert transform
        d2dContextForPrint.SetTransform(D2D_MATRIX_3X2_F.Identity);
    }

    private unsafe ID2D1CommandList FilterOneBppImage(ID2D1CommandList cl)
    {
        d2dDevice.CreateDeviceContext(D2D1_DEVICE_CONTEXT_OPTIONS.D2D1_DEVICE_CONTEXT_OPTIONS_NONE, out var newDc);
        newDc.CreateCommandList(out var newCl);
        newDc.SetTarget(newCl);
        if (printOpts.IsBlackAndWhite)
        {
            newDc.SetAntialiasMode(D2D1_ANTIALIAS_MODE.D2D1_ANTIALIAS_MODE_ALIASED);
            newDc.SetTextRenderingParams(textParams ?? throw new InvalidOperationException("No textParams found"));
        }

        newDc.BeginDraw();
        cl.Stream(new OneBppCommandSinkProxy(newDc));
        newDc.EndDraw();

        newCl.Close();
        return newCl;
    }
}

public class PdfPrinterOptions
{
    public double RasterDpi { get; set; } = 300;

    private double? outputDpi;
    public double OutputDpi
    {
        get => outputDpi ?? RasterDpi;
        set => outputDpi = double.IsFinite(value) ? value : null;
    }

    public bool FillBackground { get; set; }

    public PdfPrinterColorMode ColorMode { get; set; } = PdfPrinterColorMode.Default;

    internal bool IsBlackAndWhite => ColorMode == PdfPrinterColorMode.BlackAndWhite;
}

public enum PdfPrinterColorMode
{
    Default,
    BlackAndWhite
}
