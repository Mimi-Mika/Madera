using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using MahApps.Metro.Controls;
using Madera.Model;
using System.Linq;

namespace Madera.View.Pages.PlanVues
{
    /// <summary>
    /// Logique d'interaction pour Vue3D.xaml
    /// </summary>
    public partial class Vue3D : Page
    {
        public static Maison m3daffiche;
        public Vue3D(Maison _m3daffiche = null)
        {
            m3daffiche = _m3daffiche;
            InitializeComponent();

        }

        // The main object model group.
        private Model3DGroup MainModel3Dgroup = new Model3DGroup();

        // The camera.
        private PerspectiveCamera TheCamera;

        // The camera's current location.
        private double CameraPhi = Math.PI / 6.0;       // 30 degrees
        private double CameraTheta = Math.PI / 6.0;     // 30 degrees

        //taille de l'image au debut
        private double CameraR = 10.0;
        // The change in CameraPhi when you press the up and down arrows.
        private const double CameraDPhi = 0.1;

        // The change in CameraTheta when you press the left and right arrows.
        private const double CameraDTheta = 0.1;

        // The change in CameraR when you press + or -.
        private const double CameraDR = 0.1;

        private int CameraXCentre = 0;
        private int CameraYCentre = 0;

        // Create the scene.
        // MainViewport is the Viewport3D defined
        // in the XAML code that displays everything.
        private void MainViewport_Loaded(object sender, RoutedEventArgs e)
        {
            // Give the camera its initial position.
            TheCamera = new PerspectiveCamera();
            TheCamera.FieldOfView = 70;
            MainViewport.Camera = TheCamera;
            PositionCamera();

            // Define lights.
            DefineLights();

            DBEntities DB = new DBEntities();
            if (m3daffiche == null)
            {
                int idM = 1;
                m3daffiche = DB.Maison.Where(i => i.idMaison == idM).FirstOrDefault();
            }

            List<Module_Maison> listModulesMaison = new List<Module_Maison>();

            listModulesMaison = DB.Module_Maison.Where(i => i.idMaison == m3daffiche.idMaison).ToList();

            // Create the model.
            DefineModel(MainModel3Dgroup, listModulesMaison);

            // Add the group of models to a ModelVisual3D.
            ModelVisual3D model_visual = new ModelVisual3D();
            model_visual.Content = MainModel3Dgroup;

            // Display the main visual to the viewportt.
            MainViewport.Children.Add(model_visual);
        }

        // Define the lights.
        private void DefineLights()
        {
            AmbientLight ambient_light = new AmbientLight(Colors.Gray);
            DirectionalLight directional_light =
                new DirectionalLight(Colors.Gray, new Vector3D(-1.0, -3.0, -2.0));
            MainModel3Dgroup.Children.Add(ambient_light);
            MainModel3Dgroup.Children.Add(directional_light);
        }

        // Add the model to the Model3DGroup.
        private void DefineModel(Model3DGroup model_group, List<Module_Maison> _listModulesMaison)
        {
            MeshGeometry3D meshGround = new MeshGeometry3D();
            DiffuseMaterial surface_materialGround = new DiffuseMaterial(Brushes.LightGray);
            GeometryModel3D surface_modelGround = new GeometryModel3D(meshGround, surface_materialGround);
            surface_modelGround.BackMaterial = surface_materialGround;
            model_group.Children.Add(surface_modelGround);

            CameraXCentre = 15;
            CameraYCentre = 20;

            dessiner(meshGround, 0 - CameraXCentre, 31 - CameraXCentre, 0 - CameraYCentre, 41 - CameraYCentre, -2, -0.01);    // Plancher

            foreach (var item in _listModulesMaison)
            {

                printPanneau(model_group, (int)item.Module.TypeModule.idType, (int)item.posXDebut, (int)item.posYDebut, (int)item.posXFin, (int)item.posYFin);

            }

        }

        private void printPanneau(Model3DGroup model_group, int _type, int _xD, int _yD, int _xF, int _yF)
        {

            // Make a mesh to hold the surface.
            MeshGeometry3D meshWall = new MeshGeometry3D();
            MeshGeometry3D meshInt = new MeshGeometry3D();


            // Make the surface's material using a solid green brush.
            //ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri("View/Pages/PlanVues/MurExt.jpg", UriKind.Relative)));
            //ImageBrush imgBrushInt = new ImageBrush(new BitmapImage(new Uri("View/Pages/PlanVues/MurInt.jpg", UriKind.Relative)));
            DiffuseMaterial surface_material = new DiffuseMaterial(Brushes.LightGreen);
            DiffuseMaterial surface_materialInt = new DiffuseMaterial(Brushes.LightBlue);

            // Make the mesh's model.
            GeometryModel3D surface_model = new GeometryModel3D(meshWall, surface_material);
            GeometryModel3D surface_modelInt = new GeometryModel3D(meshInt, surface_materialInt);

            // Make the surface visible from both sides.
            surface_model.BackMaterial = surface_material;
            surface_modelInt.BackMaterial = surface_materialInt;

            // Add the model to the model groups.
            model_group.Children.Add(surface_model);
            model_group.Children.Add(surface_modelInt);

            //dessiner(meshInt, 0, 29, 20, 30, 0, 25);    // 1er bout cloison
            int xD = (_xD * 10) - CameraXCentre;
            int xF = (_xF * 10) - CameraXCentre;
            int yD = (_yD * 10) - CameraYCentre;
            int yF = (_yF * 10) - CameraYCentre;

            if (_xD == _xF)
            {
                xF = xF + 1;
            }
            if (_yD == _yF)
            {
                yF = yF + 1;
            }

            switch (_type)
            {
                case 1:     //Porte ext
                    if (xF - xD == 1) // porte en Y
                    {
                        dessiner(meshWall, xD, xF, yD + 0.01, yF - 8, 0, 20); //collision
                        dessiner(meshWall, xD, xF, yD + 2, yF - 2, 18, 20);
                        dessiner(meshWall, xD, xF, yD + 8, yF, 0, 20);
                    }
                    else
                    {
                        dessiner(meshWall, xD + 0.01, xF - 8, yD, yF, 0, 20);   //collision
                        dessiner(meshWall, xD + 2, xF - 2, yD, yF, 18, 20);
                        dessiner(meshWall, xD + 8, xF, yD, yF, 0, 20);
                    }
                    break;

                case 2:     //Porte int
                    if (xF - xD == 1) // porte en Y
                    {
                        dessiner(meshInt, xD, xF, yD + 0.01, yF - 8, 0, 20); //collision
                        dessiner(meshInt, xD, xF, yD + 2, yF - 2, 18, 20);
                        dessiner(meshInt, xD, xF, yD + 8, yF, 0, 20);
                    }
                    else
                    {
                        dessiner(meshInt, xD + 0.01, xF - 8, yD, yF, 0, 20);   //collision
                        dessiner(meshInt, xD + 2, xF - 2, yD, yF, 18, 20);
                        dessiner(meshInt, xD + 8, xF, yD, yF, 0, 20);
                    }
                    break;

                case 3:     //Mur ext     
                    if (xD < 2 && yD < 2)
                    {
                        if (xF - xD == 1) // Fenetre en Y
                        {
                            dessiner(meshWall, xD, xF, yD + 1.01, yF, 0, 20);
                        }
                        else
                        {
                            dessiner(meshWall, xD, xF, yD, yF, 0, 20);
                        }
                    }
                    else
                    {
                        dessiner(meshWall, xD + 0.01, xF, yD + 0.01, yF, 0, 20);   //collision
                    }

                    break;

                case 4:     //Mur int     
                    if (xD < 2 && yD < 2)
                    {
                        if (xF - xD == 1) // Fenetre en Y
                        {
                            dessiner(meshInt, xD, xF, yD + 1.01, yF, 0, 20);
                        }
                        else
                        {
                            dessiner(meshInt, xD + 1.01, xF, yD, yF, 0, 20);
                        }
                    }
                    else
                    {
                        dessiner(meshInt, xD + 0.01, xF, yD + 0.01, yF, 0, 20);   //collision
                    }

                    break;

                case 5:     //Fenetre ext
                    if (xF - xD == 1) // Fenetre en Y
                    {
                        dessiner(meshWall, xD, xF, yD + 0.01, yF - 8, 0, 20); //collision 
                        dessiner(meshWall, xD, xF, yD + 2, yF - 2, 16, 20);
                        dessiner(meshWall, xD, xF, yD + 2, yF - 2, 0, 6);
                        dessiner(meshWall, xD, xF, yD + 8, yF, 0, 20);
                    }
                    else
                    {
                        dessiner(meshWall, xD + 0.01, xF - 8, yD, yF, 0, 20);//collision
                        dessiner(meshWall, xD + 2, xF - 2, yD, yF, 16, 20);
                        dessiner(meshWall, xD + 2, xF - 2, yD, yF, 0, 6);
                        dessiner(meshWall, xD + 8, xF, yD, yF, 0, 20);
                    }
                    break;

                case 6:     //Fenetre int
                    if (xF - xD == 1) // Fenetre en Y
                    {
                        dessiner(meshInt, xD, xF, yD + 0.01, yF - 8, 0, 20); //collision 
                        dessiner(meshInt, xD, xF, yD + 2, yF - 2, 16, 20);
                        dessiner(meshInt, xD, xF, yD + 2, yF - 2, 0, 6);
                        dessiner(meshInt, xD, xF, yD + 8, yF, 0, 20);
                    }
                    else
                    {
                        dessiner(meshInt, xD + 0.01, xF - 8, yD, yF, 0, 20);//collision
                        dessiner(meshInt, xD + 2, xF - 2, yD, yF, 16, 20);
                        dessiner(meshInt, xD + 2, xF - 2, yD, yF, 0, 6);
                        dessiner(meshInt, xD + 8, xF, yD, yF, 0, 20);
                    }
                    break;

            }
        }

        //private void defin(Model3DGroup model_group, string type, double xdeb, double zdeb, double xfin, double zfin, double hdeb, double hfin) 
        //{
        //    // Make a mesh to hold the surface.
        //    MeshGeometry3D mesh = new MeshGeometry3D();

        //    dessiner(mesh, 0, 0, 100, 70, -0.2, -0.0001);

        //    // Make the surface's material using a solid green brush.
        //    Brush brush;
        //    switch (type)
        //    {
        //        case "MurExt":
        //            brush = Brushes.LightGreen;
        //            break;
        //        case "MurInt":
        //            brush = Brushes.LightBlue;
        //            break;
        //        case "Porte":
        //            brush = Brushes.LightGray;
        //            break;
        //        case "Fenetre":
        //            brush = Brushes.LightGray;
        //            break;
        //        case "Sol":
        //            brush = Brushes.LightGray;
        //            break;
        //        default:
        //            break;
        //    }
        //    DiffuseMaterial surface_material = new DiffuseMaterial(Brushes.LightGreen);

        //    //test image ??? ne marche pas...
        //    //ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri(@"test.jpg", UriKind.Relative)));
        //    //imgBrush.Stretch = Stretch.Fill;
        //    //imgBrush.Opacity = 1;
        //    //DiffuseMaterial surface_materialInt = new DiffuseMaterial(imgBrush);
        //    //DiffuseMaterial surface_materialGround = new DiffuseMaterial(imgBrush);

        //    // Make the mesh's model.
        //    GeometryModel3D surface_model = new GeometryModel3D(mesh, surface_material);

        //    // Make the surface visible from both sides.
        //    surface_model.BackMaterial = surface_material;

        //    // Add the model to the model groups.
        //    model_group.Children.Add(surface_model);

        //}


        private void dessiner(MeshGeometry3D mesh, double xdeb, double xfin, double zdeb, double zfin, double hdeb, double hfin)
        {

            double xDeb = xdeb / 10;
            double zDeb = zdeb / 10;
            double xFin = xfin / 10;
            double zFin = zfin / 10;
            double hDeb = hdeb / 10;
            double hFin = hfin / 10;

            Point3D p00 = new Point3D(xDeb, hDeb, zDeb);
            Point3D p01 = new Point3D(xFin, hDeb, zDeb);
            Point3D p02 = new Point3D(xFin, hFin, zDeb);
            Point3D p03 = new Point3D(xFin, hFin, zFin);
            Point3D p04 = new Point3D(xDeb, hFin, zDeb);
            Point3D p05 = new Point3D(xDeb, hFin, zFin);
            Point3D p06 = new Point3D(xDeb, hDeb, zFin);
            Point3D p07 = new Point3D(xFin, hDeb, zFin);

            AddTriangle(mesh, p00, p02, p01);
            AddTriangle(mesh, p00, p04, p02);
            AddTriangle(mesh, p00, p01, p07);
            AddTriangle(mesh, p00, p06, p07);
            AddTriangle(mesh, p00, p06, p05);
            AddTriangle(mesh, p00, p04, p05);
            AddTriangle(mesh, p03, p02, p01);
            AddTriangle(mesh, p03, p04, p02);
            AddTriangle(mesh, p03, p01, p07);
            AddTriangle(mesh, p03, p06, p07);
            AddTriangle(mesh, p03, p06, p05);
            AddTriangle(mesh, p03, p04, p05);

        }

        // Add a triangle to the indicated mesh.
        // If the triangle's points already exist, reuse them.
        private void AddTriangle(MeshGeometry3D mesh, Point3D point1, Point3D point2, Point3D point3)
        {
            // Get the points' indices.
            int index1 = AddPoint(mesh.Positions, point1);
            int index2 = AddPoint(mesh.Positions, point2);
            int index3 = AddPoint(mesh.Positions, point3);

            // Create the triangle.
            mesh.TriangleIndices.Add(index1);
            mesh.TriangleIndices.Add(index2);
            mesh.TriangleIndices.Add(index3);
        }

        // A dictionary to hold points for fast lookup.
        private Dictionary<Point3D, int> PointDictionary = new Dictionary<Point3D, int>();

        // If the point already exists, return its index.
        // Otherwise create the point and return its new index.
        private int AddPoint(Point3DCollection points, Point3D point)
        {
            // If the point is in the point dictionary,
            // return its saved index.
            if (PointDictionary.ContainsKey(point))
                return PointDictionary[point];

            // We didn't find the point. Create it.
            points.Add(point);
            PointDictionary.Add(point, points.Count - 1);
            return points.Count - 1;
        }

        // Adjust the camera's position.
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {

                case Key.Up:
                    CameraPhi += CameraDPhi;
                    if (CameraPhi > Math.PI / 2.0) CameraPhi = Math.PI / 2.0;
                    break;
                case Key.Down:
                    CameraPhi -= CameraDPhi;
                    if (CameraPhi < -Math.PI / 2.0) CameraPhi = -Math.PI / 2.0;
                    break;

                case Key.Left:
                    CameraTheta += CameraDTheta;
                    break;
                case Key.Right:
                    CameraTheta -= CameraDTheta;
                    break;

                case Key.Add:
                case Key.OemPlus:
                    CameraR -= CameraDR;
                    if (CameraR < CameraDR) CameraR = CameraDR;

                    break;
                case Key.Subtract:
                case Key.OemMinus:
                    CameraR += CameraDR;
                    break;
            }

            // Update the camera's position.
            PositionCamera();
        }


        private void btnGauche_Click(object sender, RoutedEventArgs e)
        {
            CameraTheta += CameraDTheta;
            PositionCamera();
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            CameraPhi -= CameraDPhi;
            if (CameraPhi < -Math.PI / 2.0) CameraPhi = -Math.PI / 2.0;
            PositionCamera();
        }

        private void btnDroite_Click(object sender, RoutedEventArgs e)
        {
            CameraTheta -= CameraDTheta;
            PositionCamera();
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            CameraPhi += CameraDPhi;
            if (CameraPhi > Math.PI / 2.0) CameraPhi = Math.PI / 2.0;
            PositionCamera();
        }

        private void btnMoins_Click(object sender, RoutedEventArgs e)
        {
            CameraR += CameraDR;
            PositionCamera();
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            CameraR -= CameraDR;
            if (CameraR < CameraDR) CameraR = CameraDR;
            PositionCamera();
        }

        // Position the camera.
        private void PositionCamera()
        {
            // Calculate the camera's position in Cartesian coordinates.
            double y = CameraR * Math.Sin(CameraPhi);
            double hyp = CameraR * Math.Cos(CameraPhi);
            double x = hyp * Math.Cos(CameraTheta);
            double z = hyp * Math.Sin(CameraTheta);
            TheCamera.Position = new Point3D(x, y, z);

            // Look toward the origin.
            TheCamera.LookDirection = new Vector3D(-x, -y, -z);

            // Set the Up direction.
            TheCamera.UpDirection = new Vector3D(0, 1, 0);
            // Console.WriteLine("Camera.Position: (" + x + ", " + y + ", " + z + ")");
        }


        #region pdf


        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            string sPDFFileName = System.IO.Path.GetTempPath() + "PDFFile.pdf";
            string sImagePath = System.IO.Path.GetTempPath() + "window.png";

            SaveAsPng(GetImage(MainViewport), sImagePath);
            createPdfFromImage(sImagePath, sPDFFileName);
        }

        public static RenderTargetBitmap GetImage(UIElement view)
        {
            Size size = new Size(view.RenderSize.Width, view.RenderSize.Height);
            if (size.IsEmpty)
                return null;

            RenderTargetBitmap result = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
            //RenderTargetBitmap result = new RenderTargetBitmap((int)size.Width, (int)size.Height, 200, 200, PixelFormats.Pbgra32);

            DrawingVisual drawingvisual = new DrawingVisual();
            using (DrawingContext context = drawingvisual.RenderOpen())
            {
                context.DrawRectangle(new VisualBrush(view), null, new Rect(0, 0, (int)size.Width, (int)size.Height));
                context.Close();
            }

            result.Render(drawingvisual);
            return result;
        }

        public static void SaveAsPng(RenderTargetBitmap src, string targetFile)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(src));

            using (var stm = System.IO.File.Create(targetFile))
            {
                encoder.Save(stm);
            }
        }

        public static void createPdfFromImage(string imageFile, string pdfFile)
        {
            using (var ms = new MemoryStream())
            {
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Vue2D vue2d = new Vue2D(1 , 1);
            ((MetroWindow)this.Parent).Content = vue2d;
        }
    }
}