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
using Windows.ApplicationModel.Background;
using System.CodeDom;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Rmg.PdfPrinting;

public class PdfPrinter
{
    private ID3D11Device d3dDevice;
    private IDXGIDevice dxgiDevice;
    private ID2D1Factory1 d2dFactory;
    private IWICImagingFactory2 pWic;
    private ID2D1Device d2dDevice;
    private ID2D1DeviceContext d2dContextForPrint;

    public unsafe PdfPrinter()
    {
        // d3d
        PInvoke.D3D11CreateDevice(null, D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_HARDWARE, HMODULE.Null,
            D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_BGRA_SUPPORT,
            null, 0, 7 /* D3D11_SDK_VERSION */, out d3dDevice, null, out var d3dContext)
            .ThrowOnFailure();
        dxgiDevice = (IDXGIDevice)d3dDevice;

        // d2d
        var options = new D2D1_FACTORY_OPTIONS();
#if DEBUG
        options.debugLevel = D2D1_DEBUG_LEVEL.D2D1_DEBUG_LEVEL_INFORMATION;
#endif
        PInvoke.D2D1CreateFactory(D2D1_FACTORY_TYPE.D2D1_FACTORY_TYPE_MULTI_THREADED, typeof(ID2D1Factory1).GUID, options, out var od2dFactory).ThrowOnFailure();
        d2dFactory = (ID2D1Factory1)od2dFactory;
        d2dFactory.CreateDevice(dxgiDevice, out d2dDevice);
        d2dDevice.CreateDeviceContext(D2D1_DEVICE_CONTEXT_OPTIONS.D2D1_DEVICE_CONTEXT_OPTIONS_NONE, out d2dContextForPrint);

        // wic
        pWic = (IWICImagingFactory2)new WICImagingFactory();
    }


    [Guid("cacaf262-9370-4615-a13b-9f5539da4c0a"), ComImport]
    private class WICImagingFactory
    {
    }

    public async Task Print(string printerName, string fileName, string jobName = null!, PrintTicket? ticket = null)
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
    {
        // Initialize the job
        ID2D1PrintControl printControl;
        PrintCompletionSource listener;
        var tcs = new TaskCompletionSource();
        {
            // Create a factory for document print job.
            var documentTargetFactory = (IPrintDocumentPackageTargetFactory)new PrintDocumentPackageTargetFactory();
            var ticketStream = ArrayToIStream(ticket?.GetXmlStream().ToArray());
            documentTargetFactory.CreateDocumentPackageTargetForPrintJob(
                printerName, jobName, null, ticketStream, out var docTarget);

            // wait for completion
            var cpc = (IConnectionPointContainer)docTarget;
            cpc.FindConnectionPoint(typeof(IPrintDocumentPackageStatusEvent).GUID, out var cp);
            listener = new PrintCompletionSource(cp);

            // Create a new print control linked to the package target.
            d2dDevice.CreatePrintControl(pWic, docTarget, null, out printControl);
        }

        // Open the PDF Document
        PInvoke.PdfCreateRenderer(dxgiDevice, out var pPdfRendererNative).ThrowOnFailure();
        var renderParams = new PDF_RENDER_PARAMS();

        // Write pages
        for (uint pageIndex = 0; pageIndex < pdfDoc.PageCount; pageIndex++)
        {
            var pdfPage = pdfDoc.GetPage(pageIndex);

            d2dContextForPrint.CreateCommandList(out var commandList);
            d2dContextForPrint.SetTarget(commandList);

            d2dContextForPrint.BeginDraw();
            pPdfRendererNative.RenderPageToDeviceContext(pdfPage, d2dContextForPrint, &renderParams);
            d2dContextForPrint.EndDraw();

            commandList.Close();

            var pdfSize = pdfPage.Size;
            var dxSize = new D2D_SIZE_F() { width = (float)pdfSize.Width, height = (float)pdfSize.Height };
            printControl.AddPage(commandList, dxSize, null);
        }

        printControl.Close();

        return listener.Task;
    }

    [return: NotNullIfNotNull(nameof(data))]
    private unsafe IStream? ArrayToIStream(byte[]? data)
    {
        if (data == null) return null;

        PInvoke.CreateStreamOnHGlobal((HGLOBAL)null, true, out var stm).ThrowOnFailure();
        uint sz = (uint)data.Length;
        stm.SetSize(sz);
        fixed (byte* pData = data)
        {
            uint cbWritten;
            stm.Write(pData, sz, &cbWritten).ThrowOnFailure();
            if (cbWritten != sz) throw new InvalidOperationException();
        }
        return stm;
    }
}

internal class PrintCompletionSource : IPrintDocumentPackageStatusEvent
{
    private readonly TaskCompletionSource Source = new();
    private IConnectionPoint ConnectionPoint;
    private readonly uint cookie;

    public Task Task => Source.Task;

    public PrintCompletionSource(IConnectionPoint cp)
    {
        this.ConnectionPoint = cp;
        cp.Advise(this, out cookie);
    }

    public unsafe void PackageStatusUpdated(PrintDocumentPackageStatus* packageStatus)
    {
        bool ended = false;
        switch (packageStatus->Completion)
        {
            case PrintDocumentPackageCompletion.PrintDocumentPackageCompletion_InProgress:
                break;
            case PrintDocumentPackageCompletion.PrintDocumentPackageCompletion_Completed:
                ended = Source.TrySetResult();
                break;
            case PrintDocumentPackageCompletion.PrintDocumentPackageCompletion_Canceled:
                ended = Source.TrySetCanceled();
                break;
            case PrintDocumentPackageCompletion.PrintDocumentPackageCompletion_Failed:
                ended = Source.TrySetException(Marshal.GetExceptionForHR(HRESULT.E_FAIL)!);
                break;
        }

        if (ended)
        {
            ConnectionPoint.Unadvise(cookie);
        }
    }
}