using Madera.Model;
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
        public Vue2D(int col, int lig, int idZoneMorte)
        {
            InitializeComponent();
            //test maj github
            CreateEmptyFloorPlan(col, lig, idZoneMorte);
        }

        private void CreateEmptyFloorPlan(int col, int lig, int idZoneMorte)
        {
            col = (col * 2 + 1);
            lig = (lig * 2 + 1);

            int zoneMorteTailleX = 0;
            int zoneMorteTailleY = 0;
            int zoneMorteCoordX = 0;
            int zoneMorteCoordY = 0;

            DBEntities DB = new DBEntities();
            foreach (var item in DB.ZoneMorte)
            {
                if (idZoneMorte == item.idZoneMorte)
                {
                    if ((int)item.coordonneeX != 0)
                    {
                        zoneMorteCoordX = ((int)item.coordonneeX * 2 + 1);
                        zoneMorteTailleX = ((int)item.longueur * 2 + 1);
                    }
                    else
                    {
                        zoneMorteTailleX = ((int)item.longueur * 2 + 1) + 1;
                    }
                    if ((int)item.coordonneeY != 0)
                    {
                        zoneMorteCoordY = ((int)item.coordonneeY * 2 + 1);
                        zoneMorteTailleY = ((int)item.largeur * 2 + 1);
                    }
                    else
                    {
                        zoneMorteTailleY = ((int)item.largeur * 2 + 1) + 1;
                    }

                }
            }

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

            //test remplissage de la grille avec des bouttons
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
                            if ((y == 0 && (x < zoneMorteCoordX || x > zoneMorteCoordX + zoneMorteTailleX - 2)) || (x == 0 && (y < zoneMorteCoordY || y > zoneMorteCoordY + zoneMorteTailleY - 2)))
                            {
                                MyControl1.Background = btnMurExt.Background;
                                MyControl1.Click += new RoutedEventHandler(btnClickMurExt);
                            }
                            else
                            {
                                MyControl1.Click += new RoutedEventHandler(btnClick);
                            }

                            //if ((y == lig - 1 && (x < zoneMorteCoordX || x > zoneMorteCoordX + zoneMorteTailleX - 2)) || (x == col - 1 && (y < zoneMorteCoordY || y > zoneMorteCoordY + zoneMorteTailleY - 2)))
                            //{
                            //    MyControl1.Background = btnMurExt.Background;
                            //}

                            //MyControl1.Click += new RoutedEventHandler(btnClick);
                            //MyControl1.PreviewMouseDown += new MouseButtonEventHandler(btnDown);
                            //MyControl1.PreviewMouseUp += new MouseButtonEventHandler(btnUp);
                            //MyControl1.MouseEnter += new System.Windows.Input.MouseEventArgs(mouseEnter);
                            grid2D.Children.Add(MyControl1);

                            count++;

                        }
                        else
                        {

                        }

                    }
                }
            }

            //Taille des boutons
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

            //Couleur des murs Exterieurs
            //foreach (Control ctrl in grid2D.Children)
            //{
            //    if (ctrl.GetType() == typeof(Button))
            //    {

            //        Button btn = (Button)ctrl;

            //        if (Grid.GetRow(ctrl) == 0)
            //        {
            //            btn.Background = btnMurExt.Background;
            //        }

            //        if (Grid.GetColumn(ctrl) == 0)
            //        {
            //            btn.Background = btnMurExt.Background;
            //        }

            //        if (Grid.GetRow(ctrl) == (lig - 1))
            //        {
            //            if (Grid.GetRow(ctrl) != zoneMorteCoordX * 2)
            //            {
            //                btn.Background = btnMurExt.Background;
            //            }

            //        }

            //        if (Grid.GetColumn(ctrl) == (col - 1))
            //        {
            //            btn.Background = btnMurExt.Background;
            //        }
            //    }
            //}

            //for (int y = 0; y < grid2D.RowDefinitions.Count; y = y + 2)
            //{
            //    var test = grid2D.RowDefinitions[y + 2];

            //    //MessageBox.Show(grid2D.RowDefinitions[y + 2].GetChildObjects);
            //    if (grid2D.RowDefinitions[y + 2] != null)
            //    {

            //    }
            //}

            //for (int x = 0; x < grid2D.ColumnDefinitions.Count; x = x + 2)
            //{

            //}



        }

        private void btnClickMurExt(object sender, RoutedEventArgs e)
        {

        }

        private void btnClick(object sender, RoutedEventArgs e)
        {
            //Button obj = ((FrameworkElement)sender).DataContext as Button;

            //MessageBox.Show(sender.ToString());
            if (rbMurExt.IsChecked == true)
            {
                ((Button)sender).Background = btnMurExt.Background;
            }
            else if (rbMurInt.IsChecked == true)
            {
                ((Button)sender).Background = btnMurInt.Background;
            }
            else if (rbPorte.IsChecked == true)
            {
                ((Button)sender).Background = btnPorte.Background;
            }
            else if (rbFenetre.IsChecked == true)
            {
                ((Button)sender).Background = btnFenetre.Background;
            }
            else if (rbLibre.IsChecked == true)
            {
                ((Button)sender).Background = btnLibre.Background;
            }

        }

        private void mouseEnter(object sender, MouseEventArgs e)
        {
            //if ((Mouse)e.Button == MouseButtons.Left)
            //{

            //}
        }

        private void btnUp(object sender, MouseButtonEventArgs e)
        {

            int row = Grid.GetRow(sender as Button);
            int column = Grid.GetColumn(sender as Button);
            MessageBox.Show("row " + row + " column " + column);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDown(object sender, MouseButtonEventArgs e)
        {
            //enregistre position en x et y du bouton
            //int row = Grid.GetRow(sender as Button);
            //int column = Grid.GetColumn(sender as Button);
            //MessageBox.Show("row " + row + " column " + column);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Apercu3D windows3D = new Apercu3D();
            // ((MetroWindow)this.Parent).Content = windows3D;
        }
    }
}
