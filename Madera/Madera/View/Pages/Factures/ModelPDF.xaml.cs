using iTextSharp.text;
using iTextSharp.text.pdf;
using Madera.Model;
using Madera.View.Pages.Tdb;
using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        public ModelPDF(int id_maison) {
            InitializeComponent();
            GetInfosClient(id_maison);
            getAllModuleMaison(id_maison);
        }

        private void Click_btn_retour(object sender, RoutedEventArgs e) {
            Factures.Index listing_facture = new Factures.Index();
            ((MetroWindow)this.Parent).Content = listing_facture;
        }

        // Récupère les infos du client
        private void GetInfosClient(int id_maison) {
            DBEntities db = new DBEntities();

            Projet ProjMaison = new Projet();
            Client clientMaison = new Client();
            ProjMaison = db.Projet.Where(i => i.idMaison == id_maison).FirstOrDefault();
            clientMaison = db.Client.Where(i => i.idClient == ProjMaison.idClient).FirstOrDefault();

            nom_client.Content = clientMaison.nom + " " + clientMaison.prenom;
            adresse_client.Text = clientMaison.adresse;
        }

        private void getAllModuleMaison(int id_maison) {
            DBEntities db = new DBEntities();

            var liste_module = from module_maison in db.Module_Maison
                               join module in db.Module on module_maison.idModule equals module.idModule
                               join type_module in db.TypeModule on module.idType equals type_module.idType
                               join gamme in db.Gamme on module.idGamme equals gamme.idGamme
                               where module_maison.idMaison == id_maison
                               select new {
                                   nom_module = module.nom,
                                   prix_module = module.prix,
                                   image_module = module.imgUrl,
                                   type_module = type_module.nomType,
                                   nom_gamme = gamme.nom,
                               };

            listing_modules.ItemsSource = liste_module.ToList();
            var prix = 0;
            foreach(var module in liste_module) {
                prix += (int)module.prix_module;
            }
            prix_total.Content = "Prix total :"+ prix +" €";
        }

        #region pdf
        private void btn_print_Click(object sender, RoutedEventArgs e) {            
            var date = DateTime.Now.ToString("hh mm ss tt");
            string sPDFFileName = System.IO.Path.GetTempPath() + "Facture_" + date + ".pdf";
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