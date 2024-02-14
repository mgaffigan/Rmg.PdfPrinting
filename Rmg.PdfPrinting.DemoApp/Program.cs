using System.Windows;
using System.Windows.Threading;

namespace Rmg.PdfPrinting.DemoApp;

internal class Program
{
    [STAThread]
    public static int Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.Error.WriteLine("Usage: Rmg.WinRTPdfPrinter.DemoApp.exe example.pdf \"Printer Name\"");
            return -1;
        }

        var app = new Application();
        SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(app.Dispatcher));
        app.ShutdownMode = ShutdownMode.OnExplicitShutdown;

        _ = app.Dispatcher.BeginInvoke(async () =>
        {
            await RunAsync(args[0], args[1]);
            app.Shutdown();
        });

        app.Run();
        return 0;
    }

    private static async Task RunAsync(string pdfPath, string printerName)
    {
        var pdfPrinter = new PdfPrinter();
        await pdfPrinter.Print(printerName, pdfPath);
    }
}
