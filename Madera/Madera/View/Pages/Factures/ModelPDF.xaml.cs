using iTextSharp.text;
using iTextSharp.text.pdf;
using Madera.View.Pages.Tdb;
using MahApps.Metro.Controls;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Madera.View.Pages.Factures
{
    /// <summary>
    /// Logique d'interaction pour ModelPDF.xaml
    /// </summary>
    public partial class ModelPDF : Page
    {
        public ModelPDF() {
            InitializeComponent();
        }

        private void Click_btn_retour(object sender, RoutedEventArgs e) {
            Factures.Index listing_facture = new Factures.Index();
            ((MetroWindow)this.Parent).Content = listing_facture;
        }

#region pdf
        private void btn_print_Click(object sender, RoutedEventArgs e) {
            var client = "BRAZZALOTTO CAZES";
            string sPDFFileName = System.IO.Path.GetTempPath() + "Facture_"+ client + ".pdf";
            string sImagePath = System.IO.Path.GetTempPath() + "window.png";

            SaveAsPng(GetImage(vue_pdf_facture), sImagePath);
            createPdfFromImage(sImagePath, sPDFFileName);
        }

        public static RenderTargetBitmap GetImage(UIElement view) {
            Size size = new Size(view.RenderSize.Width, view.RenderSize.Height);
            if(size.IsEmpty)
                return null;

            RenderTargetBitmap result = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);

            DrawingVisual drawingvisual = new DrawingVisual();
            using(DrawingContext context = drawingvisual.RenderOpen()) {
                context.DrawRectangle(new VisualBrush(view), null, new Rect(0, 0, (int)size.Width, (int)size.Height));
                context.Close();
            }

            result.Render(drawingvisual);
            return result;
        }

        public static void SaveAsPng(RenderTargetBitmap src, string targetFile) {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(src));

            using(var stm = System.IO.File.Create(targetFile)) {
                encoder.Save(stm);
            }
        }

        public static void createPdfFromImage(string imageFile, string pdfFile) {
            using(var ms = new MemoryStream()) {
                var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER.Rotate(), 0, 0, 0, 0);
                PdfWriter.GetInstance(document, new FileStream(pdfFile, FileMode.Create));
                iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms).SetFullCompression();
                document.Open();

                FileStream fs = new FileStream(imageFile, FileMode.Open);
                var image = iTextSharp.text.Image.GetInstance(fs);
                image.ScaleToFit(document.PageSize.Width, document.PageSize.Height);
                document.Add(image);
                document.Close();

                //open pdf file
                Process.Start("explorer.exe", pdfFile);
            }
        }

#endregion

    }
}