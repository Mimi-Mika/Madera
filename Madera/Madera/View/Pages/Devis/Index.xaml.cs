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
            //TODO: Charger les types d'avancement (tous, brouillon, devis, facture)
        }

        private void loadClient()
        {
            DBEntities DB = new DBEntities();
            cmbClient.ItemsSource = DB.Client.ToList();
            cmbClient.DisplayMemberPath = "nom";
            cmbClient.SelectedValuePath = "idClient";
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
                //DBEntities DB = new DBEntities();

                //Projet projet = new Projet();
                //projet = (Projet)ListeDevis.SelectedItem;

                //IdClient = (int)projet.idClient.Value;

                //var truc = DB.Maison.Where(i => i.idMaison == (int)projet.idMaison).FirstOrDefault();
                //IdEmpreinte = (int)truc.idEmpreinte;

                //TODO: Vider les tables du Master (sauf commercial)
                //TODO: Remplir les tables du master avec le projet selectionner

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
            ListeDevis.ItemsSource = DB.Projet.Select(i => i).ToList();
            ListeDevis.SelectedValuePath = "idClient";
        }

        private void cmbClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filter();
        }

        private void CmbEtat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filter();
        }

        private void filter()
        {
            //TODO: Filter avec cmbClient + cmbEtat (il manque un jeu de donnée "Projet EtatCommande" en base pour le faire)
            int id_client = Convert.ToInt32(cmbClient.SelectedValue.ToString());
            DBEntities DB = new DBEntities();
            ListeDevis.ItemsSource = DB.Projet.Where(i => i.idClient == id_client).ToList();
            ListeDevis.SelectedValuePath = "idClient";
        }
    }
}
