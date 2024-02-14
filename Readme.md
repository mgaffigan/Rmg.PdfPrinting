# C# PDF Printing

API which uses [Windows.Data.Pdf](https://learn.microsoft.com/en-us/uwp/api/windows.data.pdf?view=winrt-22621) from C# to print a PDF file.

No third party apps required - no additional licenses.  Only Windows.

PDF's are printed as vector.  Text and fonts are preserved.  Desktop and WinRT apps.

## Key steps

1. Ensure project specifies a windows version in `csproj`: `<TargetFramework>net7.0-windows10.0.22621.0</TargetFramework>`
1. Add NuGet package `Rmg.WinRTPdfPrinter`
1. Create a	`PdfPrinter` and call `Print`
1. There is no step 4

Example ([Program.cs](Rmg.WinRTPdfPrinter.DemoApp)):

```csharp
var pdfPrinter = new PdfPrinter();
await pdfPrinter.Print(printerName, pdfPath);
```

## Future possibilites
- WPF viewer control
- PDF to XPS
- PDF to PNG/TIFF/any WIC bitmap
- netframework support
- Earlier revisions of netcore