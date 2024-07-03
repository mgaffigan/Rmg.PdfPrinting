using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;

namespace Rmg.PdfPrinting.WpfDemoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TiffBitmapDecoder? tiff;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void dv_Drop(object sender, DragEventArgs e)
        {
            var pdfPath = ((string[])e.Data.GetData(DataFormats.FileDrop)).Single();

            if (sender == btOpenXps)
            {
                var printer = new PdfPrinter();
                await printer.ConvertToXps(pdfPath, pdfPath + ".xps");
                dv.Document = new XpsDocument(pdfPath + ".xps", FileAccess.Read).GetFixedDocumentSequence();
            }
            else if (sender == btOpenImage)
            {
                await Task.Run(async () =>
                {
                    var printer = new PdfPrinter();
                    await printer.ConvertToTiff(pdfPath, pdfPath + ".tiff");
                });

                tiff = new TiffBitmapDecoder(new FileStream(pdfPath + ".tiff", FileMode.Open, FileAccess.Read, FileShare.Read),
                    BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

                var fd = new FixedDocument();
                for (var i = 0; i < tiff.Frames.Count; i++)
                {
                    var src = tiff.Frames[i];
                    // Weird things happen if the BitmapFrame is used directly as the Image source
                    var imgSrc = new TransformedBitmap(src, Transform.Identity);

                    var img = new Image();
                    img.Source = imgSrc;
                    var fixedPage = new FixedPage();
                    fixedPage.Children.Add(img);
                    var page = new PageContent();
                    page.Child = fixedPage;
                    fd.Pages.Add(page);
                }
                dv.Document = fd;
            }
            else throw new NotSupportedException();
        }

        private void dv_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
        }

        private void btOpen_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}