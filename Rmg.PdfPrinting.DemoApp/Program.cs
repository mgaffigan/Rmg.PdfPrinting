using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace Rmg.PdfPrinting.DemoApp;

internal class Program
{
    [STAThread]
    public static int Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.Error.WriteLine(@"Usage: Rmg.PdfPrinting.DemoApp.exe example.pdf ""Printer Name"" [options]

    Options:
        /rasterDpi:300      Sets the DPI of images and other rastered items
        /outputDpi:300      Sets the DPI of output images (for TIFF output only)
        /bw                 Sets binary image output
        /fill               Fill the background (do not leave transparent)");
            return -1;
        }

        var app = new Application();
        SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(app.Dispatcher));
        app.ShutdownMode = ShutdownMode.OnExplicitShutdown;

        _ = app.Dispatcher.BeginInvoke(async () =>
        {
            await RunAsync(args[0], args[1], args.Skip(2));
            app.Shutdown(0);
        });

        app.Run();
        return 0;
    }

    private static async Task RunAsync(string pdfPath, string printerName, IEnumerable<string> args)
    {
        pdfPath = Path.GetFullPath(pdfPath);

        try
        {
            var opts = new PdfPrinterOptions();
            var dpiArg = args.FirstOrDefault(a => a.StartsWith("/outputDpi:", StringComparison.OrdinalIgnoreCase));
            if (dpiArg is not null)
            {
                opts.OutputDpi = int.Parse(dpiArg["/outputDpi:".Length..], CultureInfo.InvariantCulture);
            }
            var intermediateDpiArg = args.FirstOrDefault(a => a.StartsWith("/rasterDpi:", StringComparison.OrdinalIgnoreCase));
            if (intermediateDpiArg is not null)
            {
                opts.RasterDpi = int.Parse(intermediateDpiArg["/rasterDpi:".Length..], CultureInfo.InvariantCulture);
            }
            if (args.Any(a => a.Equals("/bw", StringComparison.OrdinalIgnoreCase)))
            {
                opts.ColorMode = PdfPrinterColorMode.BlackAndWhite;
            }
            if (args.Any(a => a.Equals("/fill", StringComparison.OrdinalIgnoreCase)))
            {
                opts.FillBackground = true;
            }

            var pdfPrinter = new PdfPrinter(opts);
            if (printerName.EndsWith(".xps", StringComparison.OrdinalIgnoreCase))
            {
                await pdfPrinter.ConvertToXps(pdfPath, Path.GetFullPath(printerName));
            }
            else if (printerName.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase))
            {
                await pdfPrinter.ConvertToTiff(pdfPath, Path.GetFullPath(printerName));
            }
            else
            {
                await pdfPrinter.Print(printerName, pdfPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());
        }
    }
}
