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
            Console.Error.WriteLine("Usage: Rmg.PdfPrinting.DemoApp.exe example.pdf \"Printer Name\"");
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
        try
        {
            var pdfPrinter = new PdfPrinter();
            if (!printerName.EndsWith(".xps", StringComparison.OrdinalIgnoreCase))
            {
                await pdfPrinter.Print(printerName, pdfPath);
            }
            else
            {
                await pdfPrinter.ConvertToXps(pdfPath, printerName);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());
        }
    }
}
