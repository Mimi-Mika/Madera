using Madera.Model;
using Madera.View.Pages.PlanVues;
using Madera.View.Pages.Tdb;
using MahApps.Metro.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Madera.View.Pages.Devis
{

    /// <summary>
    /// Logique d'interaction pour Index.xaml
    /// </summary>
    public partial class Index : Page
    {
        MasterClasse Master = new MasterClasse();

        public Index(MasterClasse _Master)
        {
            Master = _Master;
            InitializeComponent();
            loadClient();
            loadDevis();
            //Done: Charger les types d'avancement
            loadEtat();

        }

        private void loadClient()
        {
            DBEntities DB = new DBEntities();
            CmbClient.ItemsSource = DB.Client.ToList();
            CmbClient.DisplayMemberPath = "nom";
            CmbClient.SelectedValuePath = "idClient";
        }

        private void Click_btn_retour(object sender, RoutedEventArgs e)
        {
            Tableau_de_bord tdb = new Tableau_de_bord(Master);
            ((MetroWindow)this.Parent).Content = tdb;
        }

        private void btn_ajout(object sender, RoutedEventArgs e)
        {
            Devis.Create tdb = new Devis.Create(Master);
            ((MetroWindow)this.Parent).Content = tdb;
        }

        private void btnChoisirDevis_Click(object sender, RoutedEventArgs e)
        {
            if (ListeDevis.SelectedItem != null)
            {
                //

                //Projet projet = new Projet();
                //projet = (Projet)ListeDevis.SelectedItem;

                //IdClient = (int)projet.idClient.Value;

                //var truc = DB.Maison.Where(i => i.idMaison == (int)projet.idMaison).FirstOrDefault();
                //IdEmpreinte = (int)truc.idEmpreinte;

                //Done: Vider les tables du Master (sauf commercial)
                Master.LockClient = null;
                Master.LockEmpreinte = null;
                Master.LockEtatCommande = null;
                Master.LockTypeDalle = null;
                Master.LockZoneMorte = null;
                Master.NewProjetEtatCommande = null;
                Master.NewProjet = null;
                Master.NewModuleMaison = null;
                Master.LockModule = null;
                Master.NewMaisonTypeDalle = null;
                Master.NewMaison = null;
                Master.LockFinition = null;
                Master.LockCouleur = null;
                Master.LockTypeModule = null;
                Master.LockGamme = null;
                Master.NewCouleurModule = null;
                Master.NewFavori = null;
                Master.NewModuleFavori = null;

                //Done: Remplir les tables du master avec le projet selectionner
                //TODO: Toutes les listes "Lock" sans condition de where peuvent etre charger en début de prog et ne plus etre reset
                DBEntities DB = new DBEntities();

                string test666 = ListeDevis.SelectedValue.ToString();
                int test999 = Convert.ToInt32(test666);

                Master.NewProjet = DB.Projet.Where(i => i.idProjet == test999).FirstOrDefault();
                Master.LockClient = DB.Client.Where(i => i.idClient == Master.NewProjet.idClient).FirstOrDefault();
                Master.NewMaison = DB.Maison.Where(i => i.idMaison == Master.NewProjet.idMaison).FirstOrDefault();
                Master.LockEmpreinte = DB.Empreinte.Where(i => i.idEmpreinte == Master.NewMaison.idEmpreinte).FirstOrDefault();
                Master.LockZoneMorte = DB.ZoneMorte.Where(i => i.idZoneMorte == Master.LockEmpreinte.idZoneMorte).FirstOrDefault();
                Master.NewMaisonTypeDalle = DB.Maison_TypeDalle.Where(i => i.idMaison == Master.NewMaison.idMaison).FirstOrDefault();
                Master.LockTypeDalle = DB.TypeDalle.Where(i => i.idTypeDalle == Master.NewMaisonTypeDalle.idTypeDalle).FirstOrDefault();
                Master.NewProjetEtatCommande = Master.NewProjet.Projet_EtatCommande.ToList();
                Master.LockEtatCommande = DB.EtatCommande.ToList();
                Master.NewModuleMaison = DB.Module_Maison.Where(i => i.idMaison == Master.NewMaison.idMaison).ToList();
                Master.LockModule = new System.Collections.Generic.List<Module>();
                foreach (Module_Maison item in Master.NewModuleMaison)
                {
                    Master.LockModule.Add(item.Module);
                }
                Master.LockTypeModule = (DB.TypeModule.ToList());
                Master.LockGamme = (DB.Gamme.ToList());
                Master.LockFinition = (DB.Finition.ToList());
                Master.LockCouleur = (DB.Couleur.ToList());
                Master.NewCouleurModule = new System.Collections.Generic.List<Couleur_Module>();
                foreach (var item in Master.LockModule)
                {
                    Master.NewCouleurModule.AddRange(DB.Couleur_Module.Where(i => i.idModule == item.idModule).ToList());
                }
                Master.NewFavori = (DB.Favori.ToList());
                Master.NewModuleFavori = (DB.Module_Favori.ToList());

                Vue2D vue2d = new Vue2D(Master);
                ((MetroWindow)this.Parent).Content = vue2d;

            }
            else
            {
                MessageBox.Show("Merci de selectionner un Devis");
            }

        }

        private void loadDevis()
        {
            DBEntities DB = new DBEntities();
            ListeDevis.ItemsSource = DB.Projet.ToList();
            ListeDevis.DisplayMemberPath = "nom";
            ListeDevis.DisplayMemberPath = "numOF";
            ListeDevis.SelectedValuePath = "idClient";
        }

        private void loadEtat()
        {
            DBEntities DB = new DBEntities();
            CmbEtat.ItemsSource = DB.EtatCommande.ToList();
            CmbEtat.DisplayMemberPath = "nom";
            CmbEtat.SelectedValuePath = "idEtatCommande";
        }

        private void CmbClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filter();
        }

        private void CmbEtat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //filter();
        }

        private void filter()
        {
            //TODO: Filter avec cmbClient + cmbEtat (il manque un jeu de donnée "Projet EtatCommande" en base pour le faire)
            int id_client = Convert.ToInt32(CmbClient.SelectedValue.ToString());
            //int id_etat = Convert.ToInt32(CmbEtat.SelectedValue.ToString());  .Where(i => i.idProjet == id_etat)
            DBEntities DB = new DBEntities();
            ListeDevis.ItemsSource = DB.Projet.Where(i => i.idClient == id_client).ToList();
            ListeDevis.SelectedValuePath = "idProjet";
        }
    }
}
