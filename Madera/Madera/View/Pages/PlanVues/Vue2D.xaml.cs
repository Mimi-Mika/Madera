using Madera.Model;
using Madera.View.Pages.Devis;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Madera.View.Pages.PlanVues
{
    public static class GridExtensions
    {
        public static Button GetXYChild(this Grid instance, int x, int y)
        {
            if (null == instance)
            {
                throw new ArgumentNullException("instance");
            }
            Button btn = new Button();
            foreach (Button fe in instance.Children)
            {
                if (Grid.GetRow(fe) == y && Grid.GetColumn(fe) == x)
                {
                    btn = fe;
                }
            }
            return btn;
        }
    }
    /// <summary>
    /// Logique d'interaction pour Vue2D.xaml
    /// </summary>
    public partial class Vue2D : Page
    {
        bool isNew = new bool();
        MasterClasse _Master = new MasterClasse();

        public Vue2D(MasterClasse Master)
        {
            DBEntities DB = new DBEntities();
            _Master = Master;

            if (_Master.NewModuleMaison == null)
            {
                isNew = true;
            }
            else
            {
                isNew = false;
                _Master.NewModuleMaison = DB.Module_Maison.Where(i => i.idMaison == _Master.NewMaison.idMaison).ToList();
            }
            InitializeComponent();
            RemplirLabel();
            RemplirLesListe();
            CreeLaGridVide();
            TailleDesButtons();

            AfficherLesBoutons();
        }

        #region Initialisation

        /// <summary>
        /// Remplir les listes
        /// </summary>
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

        /// <summary>
        /// Remplir les labels
        /// </summary>
        private void RemplirLabel()
        {
            DBEntities DB = new DBEntities();
            lblNom.Content = DB.Client.Where(i => i.idClient == _Master.LockClient.idClient).FirstOrDefault();
            lblNumClient.Content = _Master.LockClient.idClient;
        }

        ///// <summary>
        ///// Creation de la Gris avec les boutons
        ///// </summary>
        //private void CreateEmptyFloorPlan()
        //{
        //    //Done: Modifier le if pour si on arrive de "Devis créé" ou de "Devis Editer" (Test liste module maison == vide ou pas)
        //    if (_Master.NewModuleMaison == null)
        //    {
        //        //Ajoute les boutons pour un devis a créer
        //        AfficherLesBoutons();
        //    }
        //    else
        //    {
        //        //Ajouter les boutons pour un devis a éditer
        //        AjouterLesBoutons();
        //    }
        //}

        /// <summary>
        /// Crée la Grid
        /// </summary>
        private void CreeLaGridVide()
        {
            int col = (int)_Master.LockEmpreinte.longueur * 2 + 1;
            int lig = (int)_Master.LockEmpreinte.largeur * 2 + 1;
            // Crée le Grid vide
            // Fois 2 + 1 pour mur (ext et interieur)
            for (int i = 0; i < lig; i++)
            {
                RowDefinition myRow = new RowDefinition()
                {
                    Height = new GridLength(1, GridUnitType.Star)
                };
                grid2D.RowDefinitions.Add(myRow);
            }

            for (int i = 0; i < col; i++)
            {
                ColumnDefinition myCol = new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                };
                grid2D.ColumnDefinitions.Add(myCol);
            }
        }

        /// <summary>
        /// Créer les boutons sur la grille
        /// </summary>
        private void AfficherLesBoutons()
        {
            int col = (int)_Master.LockEmpreinte.longueur * 2 + 1;
            int lig = (int)_Master.LockEmpreinte.largeur * 2 + 1;

            int zoneMorteTailleX = 0;
            int zoneMorteTailleY = 0;
            int zoneMorteCoordX = 0;
            int zoneMorteCoordY = 0;

            int idZoneMorte = 0;
            long idEmpreinte = _Master.LockEmpreinte.idEmpreinte;

            DBEntities DB = new DBEntities();
            ZoneMorte zoneMorteSelection = new ZoneMorte();
            

            Empreinte empreinteSelection = new Empreinte();
            empreinteSelection = DB.Empreinte.Where(i => i.idEmpreinte == idEmpreinte).FirstOrDefault();

            if (empreinteSelection.idZoneMorte != null)
            {
                idZoneMorte = (int)empreinteSelection.idZoneMorte;
            }

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



            for (int y = 0; y < grid2D.RowDefinitions.Count; y++)
            {
                for (int x = 0; x < grid2D.ColumnDefinitions.Count; x++)
                {
                    //ne tracer que les murs
                    if ((((y % 2) == 0) && ((x % 2) != 0)) || (((y % 2) != 0) && ((x % 2) == 0)))
                    {
                        Module_Maison ModMaison = new Module_Maison();
                        //ne pas tracer la zone morte
                        if ((col - zoneMorteCoordX - zoneMorteTailleX + 1 != 0 && x == zoneMorteCoordX + zoneMorteTailleX - 2) ||
                            (lig - zoneMorteCoordY - zoneMorteTailleY + 1 != 0 && y == zoneMorteCoordY + zoneMorteTailleY - 2) ||
                            y < zoneMorteCoordY ||
                            x < zoneMorteCoordX ||
                            y > zoneMorteCoordY + zoneMorteTailleY - 2 ||
                            x > zoneMorteCoordX + zoneMorteTailleX - 2)
                        {

                            Button MyControl1 = new Button() { Content = "" }; // ("x" + (x + 1).ToString() + " y" + (y + 1).ToString());
                            Grid.SetColumn(MyControl1, x);
                            Grid.SetRow(MyControl1, y);

                            if (isNew)
                            {
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

                                    var brush = new ImageBrush() { ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/imgMurExt.jpg", UriKind.Relative)) };
                                    MyControl1.Background = brush;
                                    //Ajouter evenement pour mur exterieur
                                    MyControl1.Click += new RoutedEventHandler(BtnClickMurExt);


                                    Module module = DB.Module.Where(i => i.idModule == 1).FirstOrDefault();
                                    ModMaison.distanceSol = 0;
                                    ModMaison.idModule = module.idModule;
                                    ModMaison.idMaison = _Master.NewMaison.idMaison;


                                    //test si horizotal ou vertical
                                    if ((x % 2) == 0)
                                    {
                                        switch (x)
                                        {
                                            case 0:
                                                ModMaison.posXDebut = x;
                                                break;
                                            default:
                                                ModMaison.posXDebut = (x) / 2;
                                                break;
                                        }
                                        switch (y)
                                        {
                                            case 1:
                                                ModMaison.posYDebut = y - 1;
                                                break;
                                            default:
                                                ModMaison.posYDebut = (y - 1) / 2;
                                                break;
                                        }
                                        ModMaison.posXFin = ModMaison.posXDebut;
                                        ModMaison.posYFin = ModMaison.posYDebut + 1;
                                    }
                                    else
                                    {
                                        switch (x)
                                        {
                                            case 1:
                                                ModMaison.posXDebut = x - 1;
                                                break;
                                            default:
                                                ModMaison.posXDebut = (x - 1) / 2;
                                                break;
                                        }
                                        switch (y)
                                        {
                                            case 0:
                                                ModMaison.posYDebut = y;
                                                break;
                                            default:
                                                ModMaison.posYDebut = (y) / 2;
                                                break;
                                        }
                                        ModMaison.posXFin = ModMaison.posXDebut + 1;
                                        ModMaison.posYFin = ModMaison.posYDebut;
                                    }

                                    MyControl1.SetValue(FrameworkElement.TagProperty, ModMaison);
                                    //Done: Enregistrer Mur en base en brouillon
                                    DB.Module_Maison.Add(ModMaison);
                                    DB.SaveChanges();

                                    //Done: Enregistrer sur le master
                                    if (_Master.NewModuleMaison == null)
                                    {
                                        _Master.NewModuleMaison = new List<Module_Maison>();
                                    }
                                    _Master.NewModuleMaison.Add(ModMaison);
                                }
                                else
                                {
                                    //Ajouter evenement pour mur interieur
                                    MyControl1.Click += new RoutedEventHandler(BtnClickMurInt);
                                }  
                            }
                            grid2D.Children.Add(MyControl1);
                        }
                    }
                }
            }
            if (isNew != true)
            {
                //Done:Placer les btn de la base
                PlacerLesObjectsSurLesBoutons();
            }

        }

        private void PlacerLesObjectsSurLesBoutons()
        {
            //Done:Placer les btn de la base
            foreach (Module_Maison moduleMaison in _Master.NewModuleMaison)
            {
                int x = 0;
                int y = 0;

                //Done: calculer si hori ou vertic
                if (moduleMaison.posXDebut - moduleMaison.posXFin == 0)
                {
                    y = (int)moduleMaison.posYDebut * 2 + 1;
                    x = (int)moduleMaison.posXDebut * 2;
                }
                //si hori
                else
                {
                    x = (int)moduleMaison.posXDebut * 2 + 1;
                    y = (int)moduleMaison.posYDebut * 2;
                }


                Button MyControl1 = GridExtensions.GetXYChild(grid2D, x, y);
                //Done: Choisir l'image
                Function2D F2D = new Function2D();
                MyControl1.Background = F2D.ChoisirLeBrush(moduleMaison.Module.TypeModule.nomType);
                //Done: Affecter l'object au bouton
                MyControl1.SetValue(FrameworkElement.TagProperty, moduleMaison);
                //Done: Affecter les bon evenement
                if (moduleMaison.Module.TypeModule.nomType.Contains("Exterieur"))
                {
                    MyControl1.Click += new RoutedEventHandler(BtnClickMurExt);
                    MyControl1.Content = "Ok";
                }
                else
                {
                    MyControl1.Click += new RoutedEventHandler(BtnClickMurInt);
                    MyControl1.Content = "Ok";
                }
            }

            foreach (Button item in grid2D.Children)
            {
                if (item.Content.ToString() != "Ok")
                {
                    item.Click += new RoutedEventHandler(BtnClickMurInt);
                    item.Content = "";
                }
                else
                {
                    item.Content = "";
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
                    grid2D.ColumnDefinitions[i].Width = new GridLength(25);
            }

            for (int i = 0; i < grid2D.RowDefinitions.Count; i++)
            {
                if ((i % 2) == 0)
                    grid2D.RowDefinitions[i].Height = new GridLength(25);
            }
        }

        /// <summary>
        /// Ajouter les Murs existant
        /// </summary>
        //private void AjouterLesBoutons()
        //{
        //    //: Ajouter les boutons pour un devis a éditer
        //    foreach (Module_Maison moduleMaison in _Master.NewModuleMaison)
        //    {
        //        Button MyControl1 = new Button() { Content = "" };
        //        int x = 0;
        //        int y = 0;


        //        //si verticale


        //        MyControl1.Name = "ExtButton" + ("x" + (x + 1).ToString() + "y" + (y + 1).ToString());
        //        var brush = new ImageBrush();
        //        //: Récuperer les X et Y créer les boutons et associer l'object 

        //        //Si mur exterieur
        //        if (moduleMaison.Module.TypeModule.nomType.Contains("Exterieur"))
        //        {
        //            //Done: calculer si hori ou vertic
        //            if (moduleMaison.posXDebut - moduleMaison.posXFin == 0)
        //            {
        //                y = (int)moduleMaison.posYDebut * 2 + 1;
        //                if (moduleMaison.posXDebut == 0)
        //                {
        //                    x = 0;
        //                }
        //                else
        //                {
        //                    x = (int)moduleMaison.posXDebut * 2 + 1;
        //                }
        //            }
        //            //si hori
        //            else
        //            {
        //                x = (int)moduleMaison.posXDebut * 2 + 1;
        //                if ((int)moduleMaison.posYDebut == 0)
        //                {
        //                    y = 0;
        //                }
        //                else
        //                {
        //                    y = (int)moduleMaison.posYDebut * 2 + 1;
        //                }
        //            }

        //            //Ajouter evenement pour mur exterieur
        //            MyControl1.Click += new RoutedEventHandler(BtnClickMurExt);

        //            if (moduleMaison.Module.TypeModule.nomType.Contains("Porte"))
        //            {
        //                brush.ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/PorteHorizontal.png", UriKind.Relative));
        //                //: Modifier object pour avoir coordonnées type etc...
        //                //ModMaison.idModule = 1;
        //            }

        //            if (moduleMaison.Module.TypeModule.nomType.Contains("Fenetre"))
        //            {
        //                brush.ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/FenetreHorizontal.png", UriKind.Relative));
        //                //: Modifier object pour avoir coordonnées type etc...
        //            }

        //            if (moduleMaison.Module.TypeModule.nomType.Contains("Mur"))
        //            {
        //                brush.ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/imgMurExt.jpg", UriKind.Relative));
        //                //: Modifier object pour avoir coordonnées type etc...
        //            }


        //        }
        //        else
        //        {
        //            //Ajouter evenement pour mur exterieur
        //            MyControl1.Click += new RoutedEventHandler(BtnClickMurInt);

        //            //Done: calculer si hori ou vertic
        //            if (moduleMaison.posXDebut - moduleMaison.posXFin == 0)
        //            {
        //                y = (int)moduleMaison.posYDebut * 2;
        //                if (moduleMaison.posXDebut == 0)
        //                {
        //                    x = 0;
        //                }
        //                else
        //                {
        //                    x = (int)moduleMaison.posXDebut * 2 + 1;
        //                }
        //            }
        //            //si hori
        //            else
        //            {
        //                x = (int)moduleMaison.posXDebut * 2;
        //                if ((int)moduleMaison.posYDebut == 0)
        //                {
        //                    y = 0;
        //                }
        //                else
        //                {
        //                    y = (int)moduleMaison.posYDebut * 2 + 1;
        //                }
        //            }

        //            if (moduleMaison.Module.TypeModule.nomType.Contains("Porte"))
        //            {
        //                brush.ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/PorteHorizontal.png", UriKind.Relative));
        //                //: Modifier object pour avoir coordonnées type etc...
        //                //ModMaison.idModule = 1;
        //            }

        //            if (moduleMaison.Module.TypeModule.nomType.Contains("Fenetre"))
        //            {
        //                brush.ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/FenetreHorizontal.png", UriKind.Relative));
        //                //: Modifier object pour avoir coordonnées type etc...
        //            }

        //            if (moduleMaison.Module.TypeModule.nomType.Contains("Mur"))
        //            {
        //                brush.ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/imgMurInt.jpg", UriKind.Relative));
        //                //: Modifier object pour avoir coordonnées type etc...
        //            }

        //        }

        //        Grid.SetColumn(MyControl1, x);
        //        Grid.SetRow(MyControl1, y);

        //        MyControl1.Background = brush;
        //        MyControl1.SetValue(FrameworkElement.TagProperty, moduleMaison);
        //        grid2D.Children.Add(MyControl1);
        //    }
        //}
        #endregion

        #region click
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
        /// appuis sur un mur exterieur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClickMurExt(object sender, RoutedEventArgs e)
        {
            if (listModule.SelectedItem != null)
            {
                //Done: Récupérer l'object dans le boutton et le modifier
                DBEntities DB = new DBEntities();
                Button btn = ((Button)sender);
                //Grid grid = (Grid)btn.Parent;
                int row = Grid.GetRow(btn);
                ImageBrush brush = new ImageBrush();
                int col = Grid.GetColumn(btn);
                Module_Maison ModMaison = (Module_Maison)btn.GetValue(FrameworkElement.TagProperty);

                //MessageBox.Show(((Button)sender).Background.GetType().ToString());

                if (rbAjout.IsChecked == true)
                {
                    //Done: Modifier le btn existant
                    string test = DB.TypeModule.Where(i => i.idType == ((long)listTypeModule.SelectedValue)).FirstOrDefault().nomType;
                    if (test.Contains("Exterieur"))
                    {
                        Function2D F2D = new Function2D();
                        brush = F2D.ChoisirLeBrush(test);
                        ModMaison.distanceSol = F2D.ChoisirLaHauteur(test, _Master, (Module)listModule.SelectedItem);
                        ModMaison.idModule = ((Module)listModule.SelectedItem).idModule;
                    }
                    else
                    {
                        MessageBox.Show("Bug impossible !!!!");
                    }
                }
                else
                {
                    //Done: Reset le button
                    brush.ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/imgMurExt.jpg", UriKind.Relative));

                    //Done: Changer le id module en murExterieur basique
                    ModMaison.distanceSol = 0;
                    ModMaison.idModule = 1; //Module mur Ext classic
                }

                ((Button)sender).Background = brush;
                //Done: Enregistrer les modif modules
                //Module
                (from x in DB.Module_Maison
                 where x.idModule_maison == ModMaison.idModule_maison
                 select x).ToList().ForEach(xx => xx.idModule = ModMaison.idModule);

                //Hauteur
                (from x in DB.Module_Maison
                 where x.idModule_maison == ModMaison.idModule_maison
                 select x).ToList().ForEach(xx => xx.distanceSol = ModMaison.distanceSol);

                //TODO: Voir table des couleurs finition....

                btn.SetValue(FrameworkElement.TagProperty, ModMaison);

                DB.SaveChanges();

                //TODO: Modifier dans le master
                (from x in _Master.NewModuleMaison
                 where x.idModule_maison == ModMaison.idModule_maison
                 select x).ToList().ForEach(xx => xx.idModule = ModMaison.idModule);
                //Hauteur
                (from x in _Master.NewModuleMaison
                 where x.idModule_maison == ModMaison.idModule_maison
                 select x).ToList().ForEach(xx => xx.distanceSol = ModMaison.distanceSol);
            }
            else
            {
                MessageBox.Show("Choisir un module");
            }

        }

        /// <summary>
        /// Appuis sur un mur interieur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClickMurInt(object sender, RoutedEventArgs e)
        {
            if (listModule.SelectedItem != null)
            {
                //TODO: Récupérer l'object dans le boutton et le modifier
                bool IsConnectedWithTheRestOfTheWorld = false;
                DBEntities DB = new DBEntities();
                Button btn = ((Button)sender);
                //Grid grid = (Grid)btn.Parent;
                int row = Grid.GetRow(btn);
                ImageBrush brush = new ImageBrush();
                int col = Grid.GetColumn(btn);
                Module_Maison ModMaison = new Module_Maison();

                //Done: Changer algo ==> verif si bouton rb checked en 1er
                if (rbAjout.IsChecked == true)
                {
                    try
                    {
                        ModMaison = (Module_Maison)btn.GetValue(FrameworkElement.TagProperty);
                        IsConnectedWithTheRestOfTheWorld = true;
                    }
                    catch (Exception)
                    {
                        ModMaison.idMaison = _Master.NewMaison.idMaison;
                        if ((col % 2) == 0)
                        {
                            switch (col)
                            {
                                case 0:
                                    ModMaison.posXDebut = col;
                                    break;
                                default:
                                    ModMaison.posXDebut = (col) / 2;
                                    break;
                            }
                            switch (row)
                            {
                                case 1:
                                    ModMaison.posYDebut = row - 1;
                                    break;
                                default:
                                    ModMaison.posYDebut = (row - 1) / 2;
                                    break;
                            }
                            ModMaison.posXFin = ModMaison.posXDebut;
                            ModMaison.posYFin = ModMaison.posYDebut + 1;
                        }
                        else
                        {
                            switch (col)
                            {
                                case 1:
                                    ModMaison.posXDebut = col - 1;
                                    break;
                                default:
                                    ModMaison.posXDebut = (col - 1) / 2;
                                    break;
                            }
                            switch (row)
                            {
                                case 0:
                                    ModMaison.posYDebut = row;
                                    break;
                                default:
                                    ModMaison.posYDebut = (row) / 2;
                                    break;
                            }
                            ModMaison.posXFin = ModMaison.posXDebut + 1;
                            ModMaison.posYFin = ModMaison.posYDebut;
                        }

                        Grid.SetColumn(btn, col);
                        Grid.SetRow(btn, row);

                        btn.SetValue(FrameworkElement.TagProperty, ModMaison);
                        throw;
                    }

                    //Test si vertical ou horizontal
                    if (col % 2 == 0)
                    {
                        //Bouton en ligne
                        if (((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row - 1 && Grid.GetColumn(i) == col - 1)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush" ||
                            ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row - 1 && Grid.GetColumn(i) == col + 1)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush" ||
                            ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row + 1 && Grid.GetColumn(i) == col - 1)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush" ||
                            ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row + 1 && Grid.GetColumn(i) == col + 1)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush" ||
                            ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row + 2 && Grid.GetColumn(i) == col + 0)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush" ||
                            ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row - 2 && Grid.GetColumn(i) == col + 0)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush")
                        {
                            string test = DB.TypeModule.Where(i => i.idType == ((long)listTypeModule.SelectedValue)).FirstOrDefault().nomType;
                            if (test.Contains("Interieur"))
                            {
                                Function2D F2D = new Function2D();
                                brush = F2D.ChoisirLeBrush(test);
                                ModMaison.distanceSol = F2D.ChoisirLaHauteur(test, _Master, (Module)listModule.SelectedItem);
                                ModMaison.idModule = ((Module)listModule.SelectedItem).idModule;
                                //Done: Enregistrer les modif modules
                                //Module
                                (from x in DB.Module_Maison
                                 where x.idModule_maison == ModMaison.idModule_maison
                                 select x).ToList().ForEach(xx => xx.idModule = ModMaison.idModule);

                                //hauteur
                                (from x in DB.Module_Maison
                                 where x.idModule_maison == ModMaison.idModule_maison
                                 select x).ToList().ForEach(xx => xx.distanceSol = ModMaison.distanceSol);
                            }
                            else
                            {
                                MessageBox.Show("Choisir un module pour mur interieur !!!!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Un item interieur doit etre accrocher a un autre", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        //Bouton en colonne
                        if (((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row - 1 && Grid.GetColumn(i) == col - 1)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush" ||
                            ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row - 1 && Grid.GetColumn(i) == col + 1)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush" ||
                            ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row + 1 && Grid.GetColumn(i) == col - 1)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush" ||
                            ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row + 1 && Grid.GetColumn(i) == col + 1)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush" ||
                            ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row + 0 && Grid.GetColumn(i) == col + 2)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush" ||
                            ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row - 0 && Grid.GetColumn(i) == col - 2)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush")
                        {
                            string test = DB.TypeModule.Where(i => i.idType == ((long)listTypeModule.SelectedValue)).FirstOrDefault().nomType;
                            if (test.Contains("Interieur"))
                            {
                                Function2D F2D = new Function2D();
                                brush = F2D.ChoisirLeBrush(test);
                                ModMaison.distanceSol = F2D.ChoisirLaHauteur(test, _Master, (Module)listModule.SelectedItem);
                                ModMaison.idModule = ((Module)listModule.SelectedItem).idModule;
                                //Done: Enregistrer les modif modules
                                //Module
                                (from x in DB.Module_Maison
                                 where x.idModule_maison == ModMaison.idModule_maison
                                 select x).ToList().ForEach(xx => xx.idModule = ModMaison.idModule);

                                //hauteur
                                (from x in DB.Module_Maison
                                 where x.idModule_maison == ModMaison.idModule_maison
                                 select x).ToList().ForEach(xx => xx.distanceSol = ModMaison.distanceSol);
                            }
                            else
                            {
                                MessageBox.Show("Choisir un module pour mur interieur !!!!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Un item interieur doit etre accrocher a un autre", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    
                }
                else
                {
                    //Done: Verifier la présence d'un object lier si oui ==> supprimer l'object du Master + BDD
                    if (IsConnectedWithTheRestOfTheWorld == true)
                    {
                        //Recup l'object
                        ModMaison = (Module_Maison)btn.GetValue(FrameworkElement.TagProperty);

                        //supprimer objet sur le button
                        btn.SetValue(FrameworkElement.TagProperty, null);

                        //supprimer l'objet en base
                        var delet = DB.Module_Maison.Where(i => i.idModule_maison == ModMaison.idModule_maison).FirstOrDefault();
                        DB.Module_Maison.Remove(delet);

                        //supprimer en master
                        _Master.NewModuleMaison.Remove(ModMaison);

                        //supprimer l'image
                        ((Button)sender).Background = null;
                    }

                    DB.SaveChanges();
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
            Vue3D windows3D = new Vue3D(_Master);
            ((MetroWindow)this.Parent).Content = windows3D;
        }

        /// <summary>
        /// Appuis du bouton retour
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRet_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Vider la Classe Master (Garder Client et Commercial)
            Index emp = new Index(_Master);
            ((MetroWindow)this.Parent).Content = emp;
        }

        private void BtnSav_Click(object sender, RoutedEventArgs e)
        {
            //Done: Passer l'état du projet de Brouillon à Devis
            DBEntities db = new DBEntities();
            Projet_EtatCommande addProjetEtatCommande = new Projet_EtatCommande()
            {
                idEtatCommande = 2, // Brouillon
                idProjet = _Master.NewProjet.idProjet,
                dates = 1254488,
                prix = 0,
                paiementValide = 0
            };
            db.Projet_EtatCommande.Add(addProjetEtatCommande);
            db.SaveChanges();
        }
        #endregion
    }
}
