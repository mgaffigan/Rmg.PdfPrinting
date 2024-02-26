# C# PDF Printing

[![Nuget](https://img.shields.io/nuget/v/Rmg.PdfPrinting)](https://www.nuget.org/packages/Rmg.PdfPrinting)

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

## Convert to XPS

You can also use this to convert a PDF to XPS:

```csharp
var pdfPrinter = new PdfPrinter();
await pdfPrinter.ConvertToXps("input.pdf", "output.xps");
```

## Convert to Tiff

You can also use this to convert a PDF to Tiff:

```csharp
var pdfPrinter = new PdfPrinter();
await pdfPrinter.ConvertToTiff("input.pdf", "output.tiff");
```

By default, a color, opaque, 300dpi, ZIP compressed multipage tiff will result.  These settings can be 
adjusted with `TiffConversionOptions`:

```csharp
// Produce a fax-style 1bpp TIFF
await pdfPrinter.ConvertToTiff("input.pdf", "output.tiff", new (Dpi: 150, ColorMode: TiffColorMode.BlackAndWhite);

// Produce a transparent background RGBA tiff
await pdfPrinter.ConvertToTiff("input.pdf", "output.tiff, new (FillBackground: false));
```

## Convert to PDF/JPEG/GIF/etc...

You don't need this library if you are only working with a single page at a time.  The 
[`PdfPage.RenderToStreamAsync`](https://learn.microsoft.com/en-us/uwp/api/windows.data.pdf.pdfpage.rendertostreamasync?view=winrt-22621#windows-data-pdf-pdfpage-rendertostreamasync(windows-storage-streams-irandomaccessstream-windows-data-pdf-pdfpagerenderoptions))
method allows you to specify a bitmap encoder and rects:

```csharp
var pdfFile = await StorageFile.GetFileFromPathAsync(@"C:\drop\example.pdf");
var pdfDoc = await PdfDocument.LoadFromFileAsync(pdfFile);
var outDir = await StorageFolder.GetFolderFromPathAsync(@"C:\drop\");

for (uint pageIndex = 0; pageIndex < pdfDoc.PageCount; pageIndex++)
{
    var pdfPage = pdfDoc.GetPage(pageIndex);
    
    var pngFile = await outDir.CreateFileAsync($"page-{pageIndex}.png", CreationCollisionOption.ReplaceExisting);
    using var pngStream = await pngFile.OpenAsync(FileAccessMode.ReadWrite);
    var options = new PdfPageRenderOptions();
    options.BitmapEncoderId = BitmapEncoder.PngEncoderId;
    // optionally specify source rectangle or dimensions
    await pdfPage.RenderToStreamAsync(pngStream, options);
}
```


## Future possibilites
- WPF viewer control
- netframework support
- Earlier revisions of netcore