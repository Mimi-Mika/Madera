using Madera.Model;
using Madera.View.Pages.Devis;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Madera.View.Pages.PlanVues
{
    /// <summary>
    /// Logique d'interaction pour Vue2D.xaml
    /// </summary>
    public partial class Vue2D : Page
    {
        public Vue2D(int idEmpreinte)
        {
            InitializeComponent();
            //test maj github
            CreateEmptyFloorPlan(idEmpreinte);

            TailleDesButtons();
        }

        private void CreateEmptyFloorPlan(int idEmpreinte)
        {
            int idZoneMorte = 0;
            DBEntities DB = new DBEntities();
            Empreinte empreinteSelection = new Empreinte();
            empreinteSelection = DB.Empreinte.Where(i => i.idEmpreinte == idEmpreinte).FirstOrDefault();

            if (empreinteSelection.idZoneMorte != null)
            {
                idZoneMorte = (int)empreinteSelection.idZoneMorte;
            }
            int col = (int)empreinteSelection.longueur * 2 + 1;
            int lig = (int)empreinteSelection.largeur * 2 + 1;

            int zoneMorteTailleX = 0;
            int zoneMorteTailleY = 0;
            int zoneMorteCoordX = 0;
            int zoneMorteCoordY = 0;

            ZoneMorte zoneMorteSelection = new ZoneMorte();
            zoneMorteSelection = DB.ZoneMorte.Where(i => i.idZoneMorte == idZoneMorte).FirstOrDefault();
            if ((int)zoneMorteSelection.coordonneeX != 0)
            {
                zoneMorteCoordX = ((int)zoneMorteSelection.coordonneeX * 2 + 1);
                zoneMorteTailleX = ((int)zoneMorteSelection.longueur * 2 + 1);
            }
            else
            {
                zoneMorteTailleX = ((int)zoneMorteSelection.longueur * 2 + 1) + 1;
            }
            if ((int)zoneMorteSelection.coordonneeY != 0)
            {
                zoneMorteCoordY = ((int)zoneMorteSelection.coordonneeY * 2 + 1);
                zoneMorteTailleY = ((int)zoneMorteSelection.largeur * 2 + 1);
            }
            else
            {
                zoneMorteTailleY = ((int)zoneMorteSelection.largeur * 2 + 1) + 1;
            }

            CreeLaGridVide(col, lig);
            AjouterLesButtons(col, lig, zoneMorteCoordX, zoneMorteTailleX, zoneMorteCoordY, zoneMorteTailleY);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="col"></param>
        /// <param name="lig"></param>
        private void CreeLaGridVide(int col, int lig)
        {
            // Crée le Grid vide
            // Fois 2 + 1 pour mur (ext et interieur)
            for (int i = 0; i < lig; i++)
            {
                RowDefinition myRow = new RowDefinition();
                myRow.Height = new GridLength(1, GridUnitType.Star);
                grid2D.RowDefinitions.Add(myRow);
            }

            for (int i = 0; i < col; i++)
            {
                ColumnDefinition myCol = new ColumnDefinition();
                myCol.Width = new GridLength(1, GridUnitType.Star);
                grid2D.ColumnDefinitions.Add(myCol);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="col"></param>
        /// <param name="lig"></param>
        /// <param name="zoneMorteCoordX"></param>
        /// <param name="zoneMorteTailleX"></param>
        /// <param name="zoneMorteCoordY"></param>
        /// <param name="zoneMorteTailleY"></param>
        private void AjouterLesButtons(int col, int lig, int zoneMorteCoordX, int zoneMorteTailleX, int zoneMorteCoordY, int zoneMorteTailleY)
        {
            int count = 1;
            for (int y = 0; y < grid2D.RowDefinitions.Count; y++)
            {
                for (int x = 0; x < grid2D.ColumnDefinitions.Count; x++)
                {
                    //ne tracer que les murs
                    if ((((y % 2) == 0) && ((x % 2) != 0)) || (((y % 2) != 0) && ((x % 2) == 0)))
                    {
                        //ne pas tracer la zone morte
                        if ((col - zoneMorteCoordX - zoneMorteTailleX + 1 != 0 && x == zoneMorteCoordX + zoneMorteTailleX - 2) ||
                            (lig - zoneMorteCoordY - zoneMorteTailleY + 1 != 0 && y == zoneMorteCoordY + zoneMorteTailleY - 2) ||
                            y < zoneMorteCoordY ||
                            x < zoneMorteCoordX ||
                            y > zoneMorteCoordY + zoneMorteTailleY - 2 ||
                            x > zoneMorteCoordX + zoneMorteTailleX - 2)
                        {

                            //TODO faire des templates de button
                            Button MyControl1 = new Button();
                            MyControl1.Content = ("x" + (x + 1).ToString() + " y" + (y + 1).ToString());
                            MyControl1.Name = "Button" + count.ToString();
                            Grid.SetColumn(MyControl1, x);
                            Grid.SetRow(MyControl1, y);

                            //TODO chercher les murs ext
                            // tracer mur si zone morte coller en haut a droite
                            // tracer mur si zone morte coller en haut a droite
                            // tracer mur ext de haut avec meme y que la zone morte si zone morte pas coller a haut
                            // tracer mur ext de gauche avec meme x que la zone morte si zone morte pas coller a gauche
                            // tracer mur si zone morte coller en haut a droite
                            // tracer mur si zone morte coller en haut a droite
                            // tracer mur ext de bas avec meme y que la zone morte si zone morte pas coller a bas
                            // tracer mur ext de droite avec meme x que la zone morte si zone morte pas coller a droite
                            // tracer haut zone morte
                            // tracer gauche zone morte
                            // tracer bas zone morte
                            // tracer droite zone morte

                            if ((y == 0 && (x < zoneMorteCoordX || x > zoneMorteCoordX + zoneMorteTailleX - 2)) ||
                                (x == 0 && (y < zoneMorteCoordY || y > zoneMorteCoordY + zoneMorteTailleY - 2)) ||
                                (y == 0 && zoneMorteCoordY != 0) ||
                                (x == 0 && zoneMorteCoordX != 0) ||
                                (y == lig - 1 && (x < zoneMorteCoordX || x > zoneMorteCoordX + zoneMorteTailleX - 2)) ||
                                (x == col - 1 && (y < zoneMorteCoordY || y > zoneMorteCoordY + zoneMorteTailleY - 2)) ||
                                (y == lig - 1 && zoneMorteCoordY != lig - 1) ||
                                (x == col - 1 && zoneMorteCoordX != col - 1) ||
                                (y == zoneMorteCoordY - 1 && !(x < zoneMorteCoordX || x > zoneMorteCoordX + zoneMorteTailleX - 2)) ||
                                (x == zoneMorteCoordX - 1 && !(y < zoneMorteCoordY || y > zoneMorteCoordY + zoneMorteTailleY - 2)) ||
                                 y == zoneMorteCoordY + zoneMorteTailleY - 2 && !(x < zoneMorteCoordX || x > zoneMorteCoordX + zoneMorteTailleX - 2) ||
                                 x == zoneMorteCoordX + zoneMorteTailleX - 2 && !(y < zoneMorteCoordY || y > zoneMorteCoordY + zoneMorteTailleY - 2))
                            {
                                var brush = new ImageBrush();
                                brush.ImageSource = new BitmapImage(new Uri("../../Pictures/imgMurExt.jpg", UriKind.Relative));
                                MyControl1.Background = brush;
                                MyControl1.Click += new RoutedEventHandler(btnClickMurExt);
                            }
                            else
                            {
                                MyControl1.Click += new RoutedEventHandler(btnClickMurInt);
                            }
                            grid2D.Children.Add(MyControl1);
                            count++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set la taille des bouton sur la grid
        /// </summary>
        private void TailleDesButtons()
        {
            for (int i = 0; i < grid2D.ColumnDefinitions.Count; i++)
            {
                if ((i % 2) == 0)
                    grid2D.ColumnDefinitions[i].Width = new GridLength(15);
            }

            for (int i = 0; i < grid2D.RowDefinitions.Count; i++)
            {
                if ((i % 2) == 0)
                    grid2D.RowDefinitions[i].Height = new GridLength(15);

            }
        }

        private void btnClickMurExt(object sender, RoutedEventArgs e)
        {
            //TODO savoir si mur dans un angle?
            Button btn = ((Button)sender);
            Grid grid = (Grid)btn.Parent;

            int row = 0;
            int col = 0;
            row = Grid.GetRow(btn);
            col = Grid.GetColumn(btn);

            Button buttonHautGauche = new Button();
            try
            {
                buttonHautGauche = (Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row - 1 && Grid.GetColumn(i) == col - 1);
            }
            catch (Exception) { }

            Button buttonHautDroite = new Button();
            try
            {
                buttonHautDroite = (Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row - 1 && Grid.GetColumn(i) == col + 1);
            }
            catch (Exception) { }

            Button buttonBasGauche = new Button();
            try
            {
                buttonBasGauche = (Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row + 1 && Grid.GetColumn(i) == col - 1);
            }
            catch (Exception) { }

            Button buttonBasDroite = new Button();
            try
            {
                buttonBasDroite = (Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row + 1 && Grid.GetColumn(i) == col + 1);
            }
            catch (Exception) { }

            if (buttonHautGauche != null)
            {
                MessageBox.Show("haut G " + buttonHautGauche.Content.ToString()
                    + "haut D " + buttonHautDroite.Content.ToString()
                    + "bas G " + buttonBasGauche.Content.ToString()
                    + "bas D " + buttonBasDroite.Content.ToString());
            }
        }

        private void btnClickMurInt(object sender, RoutedEventArgs e)
        {
            //if (rbMurInt.IsChecked == true)
            //{
            //    ((Button)sender).Background = btnMurInt.Background;
            //}
            //else if (rbPorte.IsChecked == true)
            //{
            //    ((Button)sender).Background = btnPorte.Background;
            //}
            //else if (rbFenetre.IsChecked == true)
            //{
            //    ((Button)sender).Background = btnFenetre.Background;
            //}
            //else if (rbLibre.IsChecked == true)
            //{
            //    ((Button)sender).Background = btnLibre.Background;
            //}

            //TODO savoir si mur dans un angle?
            Button btn = ((Button)sender);
            Grid grid = (Grid)btn.Parent;

            int row = 0;
            int col = 0;
            row = Grid.GetRow(btn);
            col = Grid.GetColumn(btn);

            Button buttonHautGauche = new Button();
            try
            {
                buttonHautGauche = (Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row - 1 && Grid.GetColumn(i) == col - 1);
            }
            catch (Exception) { }

            Button buttonHautDroite = new Button();
            try
            {
                buttonHautDroite = (Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row - 1 && Grid.GetColumn(i) == col + 1);
            }
            catch (Exception) { }

            Button buttonBasGauche = new Button();
            try
            {
                buttonBasGauche = (Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row + 1 && Grid.GetColumn(i) == col - 1);
            }
            catch (Exception) { }

            Button buttonBasDroite = new Button();
            try
            {
                buttonBasDroite = (Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row + 1 && Grid.GetColumn(i) == col + 1);
            }
            catch (Exception) { }






            if (buttonHautGauche != null)
            {
                MessageBox.Show("haut G " + buttonHautGauche.Content.ToString()
                    + " haut D " + buttonHautDroite.Content.ToString()
                    + " bas G " + buttonBasGauche.Content.ToString()
                    + " bas D " + buttonBasDroite.Content.ToString());
            }
        }

        private void btn3D_Click(object sender, RoutedEventArgs e)
        {
            //Apercu3D windows3D = new Apercu3D();
            // ((MetroWindow)this.Parent).Content = windows3D;
        }

        private void btnRet_Click(object sender, RoutedEventArgs e)
        {
            ChoixEmpreinte emp = new ChoixEmpreinte();
            ((MetroWindow)this.Parent).Content = emp;
        }
    }
}
