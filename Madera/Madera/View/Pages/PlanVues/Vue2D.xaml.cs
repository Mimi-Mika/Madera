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
                    break;
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
            RemplirLesListe();
            CreeLaGridVide();
            TailleDesButtons();
            RemplirLabel();

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

            // Couleur liste indépendante
            List<Couleur> CouleurList = new List<Couleur>();
            CouleurList = DB.Couleur.ToList();
            listCouleur.ItemsSource = CouleurList;
            listCouleur.SelectedValuePath = "idCouleur";
            listCouleur.SelectedIndex = 0;

            //// Module liste indépendante
            //List<Module> ModuleList = new List<Module>();
            //ModuleList = DB.Module.ToList();
            //listModule.ItemsSource = ModuleList;
            
            listModule.SelectedIndex = 0;

            // Selection radio bouton "Ajouter" par défaut
            rbAjout.IsChecked = true;
        }

        /// <summary>
        /// Remplir les labels
        /// </summary>
        private void RemplirLabel()
        {
            DBEntities DB = new DBEntities();
            lblNom.Content = _Master.LockClient.nom.ToString() + " " + _Master.LockClient.prenom.ToString();
            lblNumClient.Content = _Master.LockClient.idClient;

            if (listGamme.Items.Count != 0 && listGamme.SelectedValue != null)
            {
                _Master.LockGamme = DB.Gamme.ToList();
                lblCouverture.Content = _Master.LockGamme.Where(i => i.idGamme == (long)listGamme.SelectedValue).FirstOrDefault().couverture;
                lblFinition.Content = _Master.LockGamme.Where(i => i.idGamme == (long)listGamme.SelectedValue).FirstOrDefault().finition;
                LblHuisserie.Content = _Master.LockGamme.Where(i => i.idGamme == (long)listGamme.SelectedValue).FirstOrDefault().huisserie;
                lblIsolation.Content = _Master.LockGamme.Where(i => i.idGamme == (long)listGamme.SelectedValue).FirstOrDefault().isolation;
            }
            

            if (listModule.Items.Count != 0 && listModule.SelectedValue != null)
            {
                lblModule.Content = _Master.LockModule.Where(i => i.idModule == (long)listModule.SelectedValue).FirstOrDefault().nom;
                lblPrixModule.Content = _Master.LockModule.Where(i => i.idModule == (long)listModule.SelectedValue).FirstOrDefault().prix + "€";
                if (listCouleur.Items.Count != 0 && listCouleur.SelectedValue != null)
                {
                    lblCouleur.Content = _Master.LockCouleur.Where(i => i.idCouleur == (long)listCouleur.SelectedValue).FirstOrDefault().nom;
                    lblPrixCouleur.Content = _Master.LockCouleur.Where(i => i.idCouleur == (long)listCouleur.SelectedValue).FirstOrDefault().surCout * _Master.LockModule.Where(i => i.idModule == (long)listModule.SelectedValue).FirstOrDefault().prix / 100 + "€";
                }
            }
        }

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

            DBEntities DB = new DBEntities();
            Empreinte empreinteSelection = DB.Empreinte.Where(i => i.idEmpreinte == _Master.LockEmpreinte.idEmpreinte).FirstOrDefault();
            ZoneMorte zoneMorteSelection = DB.ZoneMorte.Where(i => i.idZoneMorte == (int)empreinteSelection.idZoneMorte).FirstOrDefault();
            Zone zone = new Zone();
            zone.ZoneMorteCoordonee(zoneMorteSelection);

            int zoneMorteTailleX = zone.zoneMorteTailleX;
            int zoneMorteTailleY = zone.zoneMorteTailleY;
            int zoneMorteCoordX = zone.zoneMorteCoordX;
            int zoneMorteCoordY = zone.zoneMorteCoordY;

            //Balayer les lignes
            for (int y = 0; y < grid2D.RowDefinitions.Count; y++)
            {
                //Balayer les colonnes
                for (int x = 0; x < grid2D.ColumnDefinitions.Count; x++)
                {
                    //Ne tracer que les murs
                    if ((((y % 2) == 0) && ((x % 2) != 0)) || (((y % 2) != 0) && ((x % 2) == 0)))
                    {
                        Module_Maison ModMaison = new Module_Maison();
                        //Ne pas tracer la zone morte
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

                                    //Ajouter evenement pour click sur mur exterieur
                                    MyControl1.Click += new RoutedEventHandler(BtnClickMurExt);
                                    //Calcule de la position du module
                                    ModMaisonPos MMP = new ModMaisonPos();
                                    MMP.position(x, y);

                                    ModMaison.idMaison = _Master.NewMaison.idMaison;
                                    ModMaison.idModule = 1;                             //id module = 1 (mur de base le moins cher)
                                    ModMaison.distanceSol = 0;                          //Hauteur a 0
                                    ModMaison.historiquePrixModule = 100;               //Mur exterieur par defaut
                                    ModMaison.historiquePrixCouleur = 0;                //Pas de surplus
                                    ModMaison.idCouleur = 1;                            //Couleur par defaut
                                    ModMaison.posXDebut = MMP.posXDebut ;
                                    ModMaison.posYDebut = MMP.posYDebut;
                                    ModMaison.posXFin = MMP.posXFin;
                                    ModMaison.posYFin = MMP.posYFin;

                                    //Done: Lier moduleMaison au bouton
                                    MyControl1.SetValue(FrameworkElement.TagProperty, ModMaison);
                                    //Done: Enregistrer Mur en base
                                    DB.Module_Maison.Add(ModMaison);

                                    //Done: Maj prix projet 
                                    (from proj in DB.Projet
                                     where proj.idProjet == _Master.NewProjet.idProjet
                                     select proj).ToList().ForEach(xx => xx.prixFabrication = xx.prixFabrication + ModMaison.historiquePrixModule + ModMaison.historiquePrixCouleur);

                                    //Done: Enregistrer sur le master
                                    if (_Master.NewModuleMaison == null)
                                    {
                                        _Master.NewModuleMaison = new List<Module_Maison>();
                                    }
                                    _Master.NewModuleMaison.Add(ModMaison);
                                }
                                else
                                {
                                    //Done: Ajouter evenement pour mur interieur
                                    MyControl1.Click += new RoutedEventHandler(BtnClickMurInt);
                                }
                            }
                            grid2D.Children.Add(MyControl1);
                        }
                    }
                }
            }
            if (isNew == true)
            {
                var Proj = DB.Projet.Where(i => i.idProjet == _Master.NewProjet.idProjet).ToList();
                Proj.ForEach(xx => xx.prixComposant = xx.prixFabrication * 0.3);
                Proj.ForEach(xx => xx.prixInstallation = xx.prixFabrication * 0.8);
                Proj.ForEach(xx => xx.prixFinal = xx.prixFabrication + xx.prixInstallation + xx.prixComposant);

                DB.SaveChanges();
            }
            else
            {
                //Done:Placer les btn de la base
                PlacerLesObjectsSurLesBoutons();
            }

        }

        /// <summary>
        /// Lier les objects ModuleMaison au bouton
        /// </summary>
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
        #endregion

        #region SelectionChange
        private void ListTypeModule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DBEntities DB = new DBEntities();
            var b = listGamme.SelectedValue;
            var bc = listTypeModule.SelectedValue;
            List<Module> ModuleList = new List<Module>();
            ModuleList = DB.Module.Where(i => i.TypeModule.idType == (long)listTypeModule.SelectedValue
             && i.Gamme.idGamme == (long)listGamme.SelectedValue).ToList();
            listModule.ItemsSource = ModuleList;
            listModule.SelectedValuePath = "idModule";

            RemplirLabel();
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
                listModule.SelectedValuePath = "idModule";
            }

            RemplirLabel();
        }

        private void listModule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RemplirLabel();
        }

        private void listCouleur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RemplirLabel();
        }

        #endregion

        #region RBchange
        private void rbSelectionner_Click(object sender, RoutedEventArgs e)
        {
            listCouleur.IsEnabled = false;
            listGamme.IsEnabled = false;
            listModule.IsEnabled = false;
            listTypeModule.IsEnabled = false;
        }

        private void rbVider_Click(object sender, RoutedEventArgs e)
        {
            listCouleur.IsEnabled = false;
            listGamme.IsEnabled = false;
            listModule.IsEnabled = false;
            listTypeModule.IsEnabled = false;
        }

        private void rbAjout_Click(object sender, RoutedEventArgs e)
        {
            listCouleur.IsEnabled = true;
            listGamme.IsEnabled = true;
            listModule.IsEnabled = true;
            listTypeModule.IsEnabled = true;
        }
        #endregion

        #region Click

        /// <summary>
        /// appuis sur un mur exterieur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClickMurExt(object sender, RoutedEventArgs e)
        {
            if (rbSelectionner.IsChecked == true)
            {
                info((Button)sender);
            }
            else
            {
                if (listModule.SelectedItem != null && listCouleur.SelectedItem != null)
                {
                    //Done: Récupérer l'object dans le boutton et le modifier
                    DBEntities DB = new DBEntities();
                    Button btn = ((Button)sender);
                    //Grid grid = (Grid)btn.Parent;
                    int row = Grid.GetRow(btn);
                    double? lastPrixModule = 0;
                    double? lastPrixCouleur = 0;
                    ImageBrush brush = new ImageBrush();
                    int col = Grid.GetColumn(btn);
                    Module_Maison ModMaison = (Module_Maison)btn.GetValue(FrameworkElement.TagProperty);
                    lastPrixModule = ModMaison.historiquePrixModule;
                    lastPrixCouleur = ModMaison.historiquePrixCouleur;
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
                            ModMaison.historiquePrixModule = _Master.LockModule.Where(i => i.idModule == ModMaison.idModule).FirstOrDefault().prix;
                            ModMaison.idCouleur = ((Couleur)listCouleur.SelectedItem).idCouleur;
                            ModMaison.historiquePrixCouleur = ModMaison.historiquePrixModule * (_Master.LockCouleur.Where(i => i.idCouleur == ModMaison.idCouleur).FirstOrDefault().surCout / 100);
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
                        ModMaison.historiquePrixModule = 100;
                        ModMaison.idCouleur = 1; //Couleur classic
                        ModMaison.historiquePrixCouleur = 0;

                    }

                    ((Button)sender).Background = brush;
                    //Done: Enregistrer les modif modules
                    //Module
                    (from x in DB.Module_Maison
                     where x.idModule_maison == ModMaison.idModule_maison
                     select x).ToList().ForEach(xx => xx.idModule = ModMaison.idModule);
                    //PrixModule
                    (from x in DB.Module_Maison
                     where x.idModule_maison == ModMaison.idModule_maison
                     select x).ToList().ForEach(xx => xx.historiquePrixModule = ModMaison.historiquePrixModule);
                    //Couleur
                    (from x in DB.Module_Maison
                     where x.idModule_maison == ModMaison.idModule_maison
                     select x).ToList().ForEach(xx => xx.idCouleur = ModMaison.idCouleur);
                    //PrixCouleur
                    (from x in DB.Module_Maison
                     where x.idModule_maison == ModMaison.idModule_maison
                     select x).ToList().ForEach(xx => xx.historiquePrixCouleur = ModMaison.historiquePrixCouleur);
                    //Hauteur
                    (from x in DB.Module_Maison
                     where x.idModule_maison == ModMaison.idModule_maison
                     select x).ToList().ForEach(xx => xx.distanceSol = ModMaison.distanceSol);


                    //Maj prix Projet
                    (from proj in DB.Projet
                     where proj.idProjet == _Master.NewProjet.idProjet
                     select proj).ToList().ForEach(xx => xx.prixFabrication = xx.prixFabrication - lastPrixModule - lastPrixCouleur + ModMaison.historiquePrixModule + ModMaison.historiquePrixCouleur);

                    (from proj in DB.Projet
                     where proj.idProjet == _Master.NewProjet.idProjet
                     select proj).ToList().ForEach(xx => xx.prixComposant = xx.prixFabrication * 0.3);

                    (from proj in DB.Projet
                     where proj.idProjet == _Master.NewProjet.idProjet
                     select proj).ToList().ForEach(xx => xx.prixInstallation = xx.prixFabrication * 0.8);

                    (from proj in DB.Projet
                     where proj.idProjet == _Master.NewProjet.idProjet
                     select proj).ToList().ForEach(xx => xx.prixFinal = xx.prixFabrication + xx.prixInstallation + xx.prixComposant);


                    btn.SetValue(FrameworkElement.TagProperty, ModMaison);

                    DB.SaveChanges();

                    //Done: Remplacer l'object d'un coup
                    //Done: Modifier dans le master

                    var Mast = _Master.NewModuleMaison.Where(i => i.idModule_maison == ModMaison.idModule_maison).ToList();
                    Mast.ForEach(xx => xx.idModule = ModMaison.idModule);
                    Mast.ForEach(xx => xx.historiquePrixModule = ModMaison.historiquePrixModule);
                    Mast.ForEach(xx => xx.distanceSol = ModMaison.distanceSol);
                    Mast.ForEach(xx => xx.idCouleur = ModMaison.idCouleur); 
                    Mast.ForEach(xx => xx.historiquePrixCouleur = ModMaison.historiquePrixCouleur);
                    
                    //Update projet
                    _Master.NewProjet = DB.Projet.Where(i => i.idProjet == _Master.NewProjet.idProjet).FirstOrDefault();

                }
                else
                {
                    MessageBox.Show("Choisir un module et une couleur");
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
            if (rbSelectionner.IsChecked == true)
            {
                info((Button)sender);
            }
            else
            {
                if (listModule.SelectedItem != null || rbVider.IsChecked == true)
                {
                    //Done: Récupérer l'object dans le boutton et le modifier
                    DBEntities DB = new DBEntities();
                    Button btn = ((Button)sender);
                    //Grid grid = (Grid)btn.Parent;
                    int row = Grid.GetRow(btn);
                    ImageBrush brush = new ImageBrush();
                    int col = Grid.GetColumn(btn);
                    Module_Maison ModMaison = new Module_Maison();
                    double? lastPrixModule = 0;
                    double? lastPrixCouleur = 0;

                    //Done: Changer algo ==> verif si bouton rb checked en 1er
                    if (rbAjout.IsChecked == true)
                    {
                        ModMaison = (Module_Maison)btn.GetValue(FrameworkElement.TagProperty);
                        if (ModMaison == null)
                        {
                            ModMaisonPos MMP = new ModMaisonPos();
                            MMP.position(col, row);

                            ModMaison = new Module_Maison();
                            ModMaison.idMaison = _Master.NewMaison.idMaison;
                            ModMaison.posXDebut = MMP.posXDebut;
                            ModMaison.posYDebut = MMP.posYDebut;
                            ModMaison.posXFin = MMP.posXFin;
                            ModMaison.posYFin = MMP.posYFin;


                            //if ((col % 2) == 0)
                            //{
                            //    switch (col)
                            //    {
                            //        case 0:
                            //            ModMaison.posXDebut = col;
                            //            break;
                            //        default:
                            //            ModMaison.posXDebut = (col) / 2;
                            //            break;
                            //    }
                            //    switch (row)
                            //    {
                            //        case 1:
                            //            ModMaison.posYDebut = row - 1;
                            //            break;
                            //        default:
                            //            ModMaison.posYDebut = (row - 1) / 2;
                            //            break;
                            //    }
                            //    ModMaison.posXFin = ModMaison.posXDebut;
                            //    ModMaison.posYFin = ModMaison.posYDebut + 1;
                            //}
                            //else
                            //{
                            //    switch (col)
                            //    {
                            //        case 1:
                            //            ModMaison.posXDebut = col - 1;
                            //            break;
                            //        default:
                            //            ModMaison.posXDebut = (col - 1) / 2;
                            //            break;
                            //    }
                            //    switch (row)
                            //    {
                            //        case 0:
                            //            ModMaison.posYDebut = row;
                            //            break;
                            //        default:
                            //            ModMaison.posYDebut = (row) / 2;
                            //            break;
                            //    }
                            //    ModMaison.posXFin = ModMaison.posXDebut + 1;
                            //    ModMaison.posYFin = ModMaison.posYDebut;
                            //}

                            Grid.SetColumn(btn, col);
                            Grid.SetRow(btn, row);

                            btn.SetValue(FrameworkElement.TagProperty, ModMaison);
                        }
                        else
                        {
                            lastPrixModule = ModMaison.historiquePrixModule;
                            lastPrixCouleur = ModMaison.historiquePrixCouleur;
                        }

                        //Test si vertical ou horizontal
                        if (row % 2 == 0)
                        {
                            //Bouton en ligne
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
                                    ModMaison.historiquePrixModule = _Master.LockModule.Where(i => i.idModule == ModMaison.idModule).FirstOrDefault().prix;
                                    ModMaison.idCouleur = ((Couleur)listCouleur.SelectedItem).idCouleur;
                                    ModMaison.historiquePrixCouleur = ModMaison.historiquePrixModule * (_Master.LockCouleur.Where(i => i.idCouleur == ModMaison.idCouleur).FirstOrDefault().surCout / 100);

                                    //Done: Enregistrer les modif modules
                                    //Création
                                    if (ModMaison.idModule_maison == 0)
                                    {
                                        DB.Module_Maison.Add(ModMaison);
                                    }
                                    //MAJ
                                    else
                                    {
                                        //Done: Enregistrer les modif modules
                                        //Module
                                        (from x in DB.Module_Maison
                                         where x.idModule_maison == ModMaison.idModule_maison
                                         select x).ToList().ForEach(xx => xx.idModule = ModMaison.idModule);
                                        //PrixModule
                                        (from x in DB.Module_Maison
                                         where x.idModule_maison == ModMaison.idModule_maison
                                         select x).ToList().ForEach(xx => xx.historiquePrixModule = ModMaison.historiquePrixModule);
                                        //Couleur
                                        (from x in DB.Module_Maison
                                         where x.idModule_maison == ModMaison.idModule_maison
                                         select x).ToList().ForEach(xx => xx.idCouleur = ModMaison.idCouleur);
                                        //PrixCouleur
                                        (from x in DB.Module_Maison
                                         where x.idModule_maison == ModMaison.idModule_maison
                                         select x).ToList().ForEach(xx => xx.historiquePrixCouleur = ModMaison.historiquePrixCouleur);
                                        //Hauteur
                                        (from x in DB.Module_Maison
                                         where x.idModule_maison == ModMaison.idModule_maison
                                         select x).ToList().ForEach(xx => xx.distanceSol = ModMaison.distanceSol);

                                        //Done: Modifier dans le master
                                        //Module
                                        (from x in _Master.NewModuleMaison
                                         where x.idModule_maison == ModMaison.idModule_maison
                                         select x).ToList().ForEach(xx => xx.idModule = ModMaison.idModule);
                                        //PrixModule
                                        (from x in _Master.NewModuleMaison
                                         where x.idModule_maison == ModMaison.idModule_maison
                                         select x).ToList().ForEach(xx => xx.historiquePrixModule = ModMaison.historiquePrixModule);
                                        //Hauteur
                                        (from x in _Master.NewModuleMaison
                                         where x.idModule_maison == ModMaison.idModule_maison
                                         select x).ToList().ForEach(xx => xx.distanceSol = ModMaison.distanceSol);
                                        //Couleur
                                        (from x in _Master.NewModuleMaison
                                         where x.idModule_maison == ModMaison.idModule_maison
                                         select x).ToList().ForEach(xx => xx.idCouleur = ModMaison.idCouleur);
                                        //PrixCouleur
                                        (from x in _Master.NewModuleMaison
                                         where x.idModule_maison == ModMaison.idModule_maison
                                         select x).ToList().ForEach(xx => xx.historiquePrixCouleur = ModMaison.historiquePrixCouleur);
                                    }

                                    //Maj prix Projet
                                    (from proj in DB.Projet
                                     where proj.idProjet == _Master.NewProjet.idProjet
                                     select proj).ToList().ForEach(xx => xx.prixFabrication = xx.prixFabrication - lastPrixModule - lastPrixCouleur + ModMaison.historiquePrixModule + ModMaison.historiquePrixCouleur);

                                    (from proj in DB.Projet
                                     where proj.idProjet == _Master.NewProjet.idProjet
                                     select proj).ToList().ForEach(xx => xx.prixComposant = xx.prixFabrication * 0.3);

                                    (from proj in DB.Projet
                                     where proj.idProjet == _Master.NewProjet.idProjet
                                     select proj).ToList().ForEach(xx => xx.prixInstallation = xx.prixFabrication * 0.8);

                                    (from proj in DB.Projet
                                     where proj.idProjet == _Master.NewProjet.idProjet
                                     select proj).ToList().ForEach(xx => xx.prixFinal = xx.prixFabrication + xx.prixInstallation + xx.prixComposant);

                                    DB.SaveChanges();
                                    btn.Background = brush;
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
                                ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row + 2 && Grid.GetColumn(i) == col + 0)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush" ||
                                ((Button)grid2D.Children.Cast<UIElement>().First(i => Grid.GetRow(i) == row - 2 && Grid.GetColumn(i) == col - 0)).Background.GetType().ToString() == "System.Windows.Media.ImageBrush")
                            {
                                string test = DB.TypeModule.Where(i => i.idType == ((long)listTypeModule.SelectedValue)).FirstOrDefault().nomType;
                                if (test.Contains("Interieur"))
                                {
                                    Function2D F2D = new Function2D();
                                    brush = F2D.ChoisirLeBrush(test);
                                    ModMaison.distanceSol = F2D.ChoisirLaHauteur(test, _Master, (Module)listModule.SelectedItem);
                                    ModMaison.idModule = ((Module)listModule.SelectedItem).idModule;
                                    ModMaison.historiquePrixModule = _Master.LockModule.Where(i => i.idModule == ModMaison.idModule).FirstOrDefault().prix;
                                    ModMaison.idCouleur = ((Couleur)listCouleur.SelectedItem).idCouleur;
                                    ModMaison.historiquePrixCouleur = ModMaison.historiquePrixModule * (_Master.LockCouleur.Where(i => i.idCouleur == ModMaison.idCouleur).FirstOrDefault().surCout / 100);
                                    //Done: Enregistrer les modif modules
                                    //Création
                                    if (ModMaison.idModule_maison == 0)
                                    {
                                        DB.Module_Maison.Add(ModMaison);
                                    }
                                    //MAJ
                                    else
                                    {
                                        //Done: Enregistrer les modif modules
                                        //Module
                                        (from x in DB.Module_Maison
                                         where x.idModule_maison == ModMaison.idModule_maison
                                         select x).ToList().ForEach(xx => xx.idModule = ModMaison.idModule);
                                        //PrixModule
                                        (from x in DB.Module_Maison
                                         where x.idModule_maison == ModMaison.idModule_maison
                                         select x).ToList().ForEach(xx => xx.historiquePrixModule = ModMaison.historiquePrixModule);
                                        //Couleur
                                        (from x in DB.Module_Maison
                                         where x.idModule_maison == ModMaison.idModule_maison
                                         select x).ToList().ForEach(xx => xx.idCouleur = ModMaison.idCouleur);
                                        //PrixCouleur
                                        (from x in DB.Module_Maison
                                         where x.idModule_maison == ModMaison.idModule_maison
                                         select x).ToList().ForEach(xx => xx.historiquePrixCouleur = ModMaison.historiquePrixCouleur);
                                        //Hauteur
                                        (from x in DB.Module_Maison
                                         where x.idModule_maison == ModMaison.idModule_maison
                                         select x).ToList().ForEach(xx => xx.distanceSol = ModMaison.distanceSol);

                                        //Done: Modifier dans le master
                                        //Module
                                        (from x in _Master.NewModuleMaison
                                         where x.idModule_maison == ModMaison.idModule_maison
                                         select x).ToList().ForEach(xx => xx.idModule = ModMaison.idModule);
                                        //PrixModule
                                        (from x in _Master.NewModuleMaison
                                         where x.idModule_maison == ModMaison.idModule_maison
                                         select x).ToList().ForEach(xx => xx.historiquePrixModule = ModMaison.historiquePrixModule);
                                        //Hauteur
                                        (from x in _Master.NewModuleMaison
                                         where x.idModule_maison == ModMaison.idModule_maison
                                         select x).ToList().ForEach(xx => xx.distanceSol = ModMaison.distanceSol);
                                        //Couleur
                                        (from x in _Master.NewModuleMaison
                                         where x.idModule_maison == ModMaison.idModule_maison
                                         select x).ToList().ForEach(xx => xx.idCouleur = ModMaison.idCouleur);
                                        //PrixCouleur
                                        (from x in _Master.NewModuleMaison
                                         where x.idModule_maison == ModMaison.idModule_maison
                                         select x).ToList().ForEach(xx => xx.historiquePrixCouleur = ModMaison.historiquePrixCouleur);
                                    }

                                    //Maj prix Projet
                                    (from proj in DB.Projet
                                     where proj.idProjet == _Master.NewProjet.idProjet
                                     select proj).ToList().ForEach(xx => xx.prixFabrication = xx.prixFabrication - lastPrixModule - lastPrixCouleur + ModMaison.historiquePrixModule + ModMaison.historiquePrixCouleur);

                                    (from proj in DB.Projet
                                     where proj.idProjet == _Master.NewProjet.idProjet
                                     select proj).ToList().ForEach(xx => xx.prixComposant = xx.prixFabrication * 0.3);

                                    (from proj in DB.Projet
                                     where proj.idProjet == _Master.NewProjet.idProjet
                                     select proj).ToList().ForEach(xx => xx.prixInstallation = xx.prixFabrication * 0.8);

                                    (from proj in DB.Projet
                                     where proj.idProjet == _Master.NewProjet.idProjet
                                     select proj).ToList().ForEach(xx => xx.prixFinal = xx.prixFabrication + xx.prixInstallation + xx.prixComposant);

                                    DB.SaveChanges();

                                    //MAJ visuel
                                    btn.Background = brush;
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
                        ModMaison = (Module_Maison)btn.GetValue(FrameworkElement.TagProperty);
                        if (ModMaison != null)
                        {
                            //Recup l'object
                            ModMaison = (Module_Maison)btn.GetValue(FrameworkElement.TagProperty);
                            lastPrixModule = ModMaison.historiquePrixModule;
                            lastPrixCouleur = ModMaison.historiquePrixCouleur;
                            //supprimer objet sur le button
                            btn.SetValue(FrameworkElement.TagProperty, null);

                            //supprimer l'objet en base
                            var delet = DB.Module_Maison.Where(i => i.idModule_maison == ModMaison.idModule_maison).FirstOrDefault();
                            DB.Module_Maison.Remove(delet);

                            //supprimer en master
                            _Master.NewModuleMaison.Remove(ModMaison);

                            //Maj prix Projet
                            (from proj in DB.Projet
                             where proj.idProjet == _Master.NewProjet.idProjet
                             select proj).ToList().ForEach(xx => xx.prixFabrication = xx.prixFabrication - lastPrixModule - lastPrixCouleur);

                            (from proj in DB.Projet
                             where proj.idProjet == _Master.NewProjet.idProjet
                             select proj).ToList().ForEach(xx => xx.prixComposant = xx.prixFabrication * 0.3);

                            (from proj in DB.Projet
                             where proj.idProjet == _Master.NewProjet.idProjet
                             select proj).ToList().ForEach(xx => xx.prixInstallation = xx.prixFabrication * 0.8);

                            (from proj in DB.Projet
                             where proj.idProjet == _Master.NewProjet.idProjet
                             select proj).ToList().ForEach(xx => xx.prixFinal = xx.prixFabrication + xx.prixInstallation + xx.prixComposant);

                            //Maj Master Projet
                            _Master.NewProjet = DB.Projet.Where(i => i.idProjet == _Master.NewProjet.idProjet).FirstOrDefault();

                            //supprimer l'image
                            ((Button)sender).Background = null;
                        }

                        DB.SaveChanges();
                    }
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
            //UseLess: Vider la Classe Master (Garder Client et Commercial)
            Index emp = new Index(_Master);
            ((MetroWindow)this.Parent).Content = emp;
        }

        /// <summary>
        /// Sauvegarder le brouillon en devis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSav_Click(object sender, RoutedEventArgs e)
        {
            //Done: test si déja enregistre ==> MAJ
            //Done: Passer l'état du projet de Brouillon à Devis
            DBEntities db = new DBEntities();

            if (_Master.NewProjet.DernierEtatCommande == 2)
            {
                //Done: Mettre a jour la date + prix
                Projet_EtatCommande PEC = _Master.NewProjetEtatCommande.Where(i => i.idProjet == _Master.NewProjet.idProjet && i.idEtatCommande == 2).FirstOrDefault();
                PEC.prix = _Master.NewProjet.prixFinal;
                PEC.paiementValide = 0;
                PEC.dates = DateTime.Now.ToString();
            }
                //Done: Mettre a jour Projet.DernierEtatCommande
            else
            {
                Projet_EtatCommande addProjetEtatCommande = new Projet_EtatCommande()
                {
                    idEtatCommande = 2, // Devis
                    idProjet = _Master.NewProjet.idProjet,
                    prix = _Master.NewProjet.prixFinal,
                    paiementValide = 0,
                    dates = DateTime.Now.ToString()
                };
                db.Projet_EtatCommande.Add(addProjetEtatCommande);
                _Master.NewProjet.DernierEtatCommande = 2; //Devis
                var Proj = db.Projet.Where(i => i.idProjet == _Master.NewProjet.idProjet).ToList();
                Proj.ForEach(xx => xx.DernierEtatCommande = 2); //Devis
            }
            db.SaveChanges();
        }
        #endregion

        /// <summary>
        /// Mise a jour des informations lors d'un click sur un mur et RBselection == true
        /// </summary>
        /// <param name="btn"></param>
        private void info(Button btn) 
        {
            Module_Maison ModMaison = new Module_Maison();
            ModMaison = (Module_Maison)btn.GetValue(FrameworkElement.TagProperty);
            if (ModMaison != null)
            {
                Module Mod = _Master.LockModule.Where(i => i.idModule == ModMaison.idModule).FirstOrDefault();

                lblCouverture.Content = _Master.LockGamme.Where(i => i.idGamme == Mod.idGamme).FirstOrDefault().couverture;
                lblFinition.Content = _Master.LockGamme.Where(i => i.idGamme == Mod.idGamme).FirstOrDefault().finition;
                LblHuisserie.Content = _Master.LockGamme.Where(i => i.idGamme == Mod.idGamme).FirstOrDefault().huisserie;
                lblIsolation.Content = _Master.LockGamme.Where(i => i.idGamme == Mod.idGamme).FirstOrDefault().isolation;

                lblModule.Content = Mod.nom;
                lblCouleur.Content = _Master.LockCouleur.Where(i => i.idCouleur == ModMaison.idCouleur).FirstOrDefault().nom;
                lblPrixCouleur.Content = ModMaison.historiquePrixCouleur + "€";
                lblPrixModule.Content = ModMaison.historiquePrixModule + "€";

                DBEntities DB = new DBEntities();

                //Selectionner le TypeModule
                listTypeModule.SelectedIndex = (int)DB.Module.Where(i => i.idModule == ModMaison.idModule).FirstOrDefault().idType - 1;

                //Selectionner la Gamme
                listGamme.SelectedIndex = (int)DB.Module.Where(i => i.idModule == ModMaison.idModule).FirstOrDefault().idGamme - 1;

                //Selectionner la Couleur
                listCouleur.SelectedIndex = (int)ModMaison.idCouleur - 1;

                //Selectionner le Module
                int sel = 0;

                for (int i = 0; i < listModule.Items.Count; i++)
                {
                    Module test = (Module)listModule.Items[i];
                    if (test.idModule == ModMaison.idModule)
                    {
                        sel = i;
                        break;
                    }
                }
                listModule.SelectedIndex = sel;
            }
        }
    }
}
