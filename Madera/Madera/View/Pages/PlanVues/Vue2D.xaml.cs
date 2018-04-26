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
        int IdClient;
        public Vue2D(int idEmpreinte, int idClient)
        {
            IdClient = idClient;

            InitializeComponent();
            RemplirLabel(idClient);

            CreateEmptyFloorPlan(idEmpreinte);
            TailleDesButtons();

            RemplirLesListe();
        }

        public Vue2D(Projet projet)
        {


        }

        private void RemplirLesListe()
        {
            DBEntities DB = new DBEntities();

            // Gamme premiere Liste
            List<Gamme> GammeList = new List<Gamme>();
            GammeList = DB.Gamme.ToList();
            listGamme.ItemsSource = GammeList;
            listGamme.SelectedValuePath = "idGamme";
            listGamme.SelectedIndex = 0; // Selection premier champ de la liste

            //Type Module, liste avec critère de gamme
            List<TypeModule> TypeModuleList = new List<TypeModule>();
            TypeModuleList = DB.TypeModule.ToList();
            listTypeModule.ItemsSource = TypeModuleList;
            listTypeModule.SelectedValuePath = "idType";
            listTypeModule.SelectedIndex = 0;

            //Finition, liste avec critère de gamme
            List<Finition> FinitionList = new List<Finition>();
            FinitionList = DB.Finition.ToList();
            listFinition.ItemsSource = FinitionList;
            listFinition.SelectedValuePath = "idFinition";
            listFinition.SelectedIndex = 0;

            //Liste des modules ayant la gammme, le type module et la finition selectionnée
            List<Module> ModuleList = new List<Module>();
            ModuleList = DB.Module.Where(i => i.TypeModule.idType == (long)listTypeModule.SelectedValue
            && i.Gamme.idGamme == (long)listGamme.SelectedValue).ToList();
            listModule.ItemsSource = ModuleList;
            listFinition.SelectedValuePath = "idModule";

            // Couleur liste indépendante
            List<Couleur> CouleurList = new List<Couleur>();
            CouleurList = DB.Couleur.ToList();
            listCouleur.ItemsSource = CouleurList;
            listCouleur.SelectedValuePath = "idCouleur";

            // Selection radio bouton "Ajouter" par défaut
            rbAjout.IsChecked = true;
        }

        private void RemplirLabel(int idClient)
        {
            DBEntities DB = new DBEntities();
            lblNom.Content = DB.Client.Where(i => i.idClient == IdClient).FirstOrDefault();
            lblNumClient.Content = idClient;
        }


        /// <summary>
        /// Creation de la Gris avec les boutons
        /// </summary>
        /// <param name="idEmpreinte"></param>
        private void CreateEmptyFloorPlan(int idEmpreinte)
        {
            int idZoneMorte = 0;
            DBEntities DB = new DBEntities();

            this.DataContext = this;

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
            AjouterLesBoutons(col, lig, zoneMorteCoordX, zoneMorteTailleX, zoneMorteCoordY, zoneMorteTailleY);

        }

        private void ListTypeModule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DBEntities DB = new DBEntities();
            var b = listGamme.SelectedValue;
            var bc = listTypeModule.SelectedValue;
            List<Module> ModuleList = new List<Module>();
            ModuleList = DB.Module.Where(i => i.TypeModule.idType == (long)listTypeModule.SelectedValue
             && i.Gamme.idGamme == (long)listGamme.SelectedValue).ToList();
            listModule.ItemsSource = ModuleList;
        }

        private void ListGamme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listTypeModule.SelectedValue != null && listGamme.SelectedValue != null)
            {
                DBEntities DB = new DBEntities();
                List<Module> ModuleList = new List<Module>();
                ModuleList = DB.Module.Where(i => i.TypeModule.idType == (long)listTypeModule.SelectedValue
                 && (long)i.Gamme.idGamme == (long)listGamme.SelectedValue).ToList();
                listModule.ItemsSource = ModuleList;
            }
        }

        /// <summary>
        /// Crée la Grid
        /// </summary>
        /// <param name="col"></param>
        /// <param name="lig"></param>
        private void CreeLaGridVide(int col, int lig)
        {
            // Crée le Grid vide
            // Fois 2 + 1 pour mur (ext et interieur)
            for (int i = 0; i < lig; i++)
            {
                RowDefinition myRow = new RowDefinition()
                {
                    Height = new GridLength(1, GridUnitType.Star)};
                grid2D.RowDefinitions.Add(myRow);
            }

            //After: HttpClient client = new HttpClient(); client.BaseAddress = new Uri(url);

            //Before: HttpClient client = new HttpClient() { BaseAddress = new Uri(url) };

            for (int i = 0; i < col; i++)
            {
                ColumnDefinition myCol = new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)};
                grid2D.ColumnDefinitions.Add(myCol);
            }
        }

        /// <summary>
        /// Ajouter les boutons sur la grille
        /// </summary>
        /// <param name="col"></param>
        /// <param name="lig"></param>
        /// <param name="zoneMorteCoordX"></param>
        /// <param name="zoneMorteTailleX"></param>
        /// <param name="zoneMorteCoordY"></param>
        /// <param name="zoneMorteTailleY"></param>
        private void AjouterLesBoutons(int col, int lig, int zoneMorteCoordX, int zoneMorteTailleX, int zoneMorteCoordY, int zoneMorteTailleY)
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
                            Button MyControl1 = new Button() { Content = "" }; // ("x" + (x + 1).ToString() + " y" + (y + 1).ToString());
                            
                            Grid.SetColumn(MyControl1, x);
                            Grid.SetRow(MyControl1, y);

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

                            //Chercher les murs exterieurs
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
                                MyControl1.Name = "ExtButton" + ("x" + (x + 1).ToString() + "y" + (y + 1).ToString());
                                var brush = new ImageBrush() { ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/imgMurExt.jpg", UriKind.Relative))  };
                                MyControl1.Background = brush;
                                //Ajouter evenement pour mur exterieur
                                MyControl1.Click += new RoutedEventHandler(BtnClickMurExt);
                            }
                            //Murs interieurs
                            else
                            {
                                MyControl1.Name = "IntButton" + ("x" + (x + 1).ToString() + "y" + (y + 1).ToString());
                                //Ajouter evenement pour mur interieur
                                MyControl1.Click += new RoutedEventHandler(BtnClickMurInt);
                            }
                            grid2D.Children.Add(MyControl1);
                            count++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set la taille des boutons sur la grid
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

        /// <summary>
        /// appuis sur un mur exterieur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClickMurExt(object sender, RoutedEventArgs e)
        {
            Button btn = ((Button)sender);
            Grid grid = (Grid)btn.Parent;
            int row = 0;
            int col = 0;
            row = Grid.GetRow(btn);
            col = Grid.GetColumn(btn);

            //MessageBox.Show(((Button)sender).Background.GetType().ToString());
            DBEntities DB = new DBEntities();
            string test = DB.TypeModule.Where(i => i.idType == ((long)listTypeModule.SelectedValue)).FirstOrDefault().nomType;

            var brush = new ImageBrush();

            if (grid2D.ColumnDefinitions[col].ActualWidth < grid2D.RowDefinitions[row].ActualHeight)
            {
                //button en ligne
                if (rbAjout.IsChecked == true)
                {
                    if (test.Contains("Exterieur"))
                    {
                        if (test.Contains("Porte"))
                        {
                            brush.ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/PorteHorizontal.png", UriKind.Relative));
                            ((Button)sender).Background = brush;
                        }

                        if (test.Contains("Fenetre"))
                        {
                            brush.ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/FenetreHorizontal.png", UriKind.Relative));
                            ((Button)sender).Background = brush;
                        }
                    }
                }
                else
                {
                    brush.ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/imgMurExt.jpg", UriKind.Relative));
                    ((Button)sender).Background = brush;
                }
            }
            else
            {
                //bouton en colone
                if (rbAjout.IsChecked == true)
                {
                    if (test.Contains("Exterieur"))
                    {
                        if (test.Contains("Porte"))
                        {
                            brush.ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/PorteVertical.png", UriKind.Relative));
                            ((Button)sender).Background = brush;
                        }

                        if (test.Contains("Fenetre"))
                        {
                            brush.ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/FenetreVertical.png", UriKind.Relative));
                            ((Button)sender).Background = brush;
                        }

                        if (test.Contains("Mur"))
                        {
                            brush.ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/imgMurInt.jpg", UriKind.Relative));
                            ((Button)sender).Background = brush;
                        }
                    }
                }
                else
                {
                    brush.ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/imgMurExt.jpg", UriKind.Relative));
                    ((Button)sender).Background = brush;
                }
            }
        }
        /// <summary>
        /// Appuis sur un mur interieur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClickMurInt(object sender, RoutedEventArgs e)
        {
            Button btn = ((Button)sender);
            Grid grid = (Grid)btn.Parent;
            int row = 0;
            int col = 0;
            row = Grid.GetRow(btn);
            col = Grid.GetColumn(btn);

            //MessageBox.Show(((Button)sender).Background.GetType().ToString());

            if (grid2D.ColumnDefinitions[col].ActualWidth < grid2D.RowDefinitions[row].ActualHeight)
            {
                //button en ligne
                if (((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row - 1 && Grid.GetColumn(i) == col - 1)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush" ||
                    ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row - 1 && Grid.GetColumn(i) == col + 1)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush" ||
                    ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row + 1 && Grid.GetColumn(i) == col - 1)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush" ||
                    ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row + 1 && Grid.GetColumn(i) == col + 1)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush" ||
                    ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row + 2 && Grid.GetColumn(i) == col + 0)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush" ||
                    ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row - 2 && Grid.GetColumn(i) == col + 0)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush")
                {
                    if (rbAjout.IsChecked == true)
                    {


                        DBEntities DB = new DBEntities();
                        string test = DB.TypeModule.Where(i => i.idType == ((long)listTypeModule.SelectedValue)).FirstOrDefault().nomType;

                        if (test.Contains("Interieur"))
                        {
                            if (test.Contains("Porte"))
                            {
                                var brush = new ImageBrush() { ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/PorteHorizontal.png", UriKind.Relative)) };
                                ((Button)sender).Background = brush;
                            }

                            if (test.Contains("Fenetre"))
                            {
                                var brush = new ImageBrush() { ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/FenetreHorizontal.png", UriKind.Relative))  };
                                ((Button)sender).Background = brush;
                            }

                            if (test.Contains("Mur"))
                            {
                                var brush = new ImageBrush() { ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/imgMurInt.jpg", UriKind.Relative)) };
                                ((Button)sender).Background = brush;
                            }
                        }
                    }
                    else
                    {
                        if (((Button)sender).Background != null)
                        {
                            ((Button)sender).Background = null;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Un item interieur doit etre accrocher a un autre", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                //bouton en colone
                if (((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row - 1 && Grid.GetColumn(i) == col - 1)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush" ||
                    ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row - 1 && Grid.GetColumn(i) == col + 1)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush" ||
                    ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row + 1 && Grid.GetColumn(i) == col - 1)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush" ||
                    ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row + 1 && Grid.GetColumn(i) == col + 1)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush" ||
                    ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row + 0 && Grid.GetColumn(i) == col + 2)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush" ||
                    ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row - 0 && Grid.GetColumn(i) == col - 2)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush")
                {
                    if (rbAjout.IsChecked == true)
                    {


                        DBEntities DB = new DBEntities();
                        string test = DB.TypeModule.Where(i => i.idType == ((long)listTypeModule.SelectedValue)).FirstOrDefault().nomType;

                        if (test.Contains("Interieur"))
                        {
                            if (test.Contains("Porte"))
                            {
                                var brush = new ImageBrush() { ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/PorteVertical.png", UriKind.Relative)) };
                                ((Button)sender).Background = brush;
                            }

                            if (test.Contains("Fenetre"))
                            {
                                var brush = new ImageBrush() { ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/FenetreVertical.png", UriKind.Relative)) };
                                ((Button)sender).Background = brush;
                            }

                            if (test.Contains("Mur"))
                            {
                                var brush = new ImageBrush() { ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/imgMurInt.jpg", UriKind.Relative)) };
                                ((Button)sender).Background = brush;
                            }
                        }
                    }
                    else
                    {
                        if (((Button)sender).Background != null)
                        {
                            ((Button)sender).Background = null;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Un item interieur doit etre accrocher a un autre","Erreur",MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Appuis sur le bouton Vue3D
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn3D_Click(object sender, RoutedEventArgs e)
        {
            Vue3D windows3D = new Vue3D();
            ((MetroWindow)this.Parent).Content = windows3D;
        }

        /// <summary>
        /// Appuis du bouton retour
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRet_Click(object sender, RoutedEventArgs e)
        {
            Index emp = new Index();
            ((MetroWindow)this.Parent).Content = emp;
        }

        private void BtnSav_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
