using Windows.Win32.Foundation;
using Windows.Win32.System.Com;
using Windows.Win32.Storage.Xps.Printing;
using System.Runtime.InteropServices;

namespace Rmg.PdfPrinting;

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