using Madera.Model;
using Madera.View.Pages.Tdb;
using MahApps.Metro.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Madera.View.Pages.Factures
{
    /// <summary>
    /// Logique d'interaction pour Index.xaml
    /// </summary>
    public partial class Index : Page
    {
        public Index() {
            InitializeComponent();
            GetAllFactures();
        }

        private void Click_btn_retour(object sender, RoutedEventArgs e) {
            Tableau_de_bord tdb = new Tableau_de_bord();
            ((MetroWindow)this.Parent).Content = tdb;
        }

        private void Click_btn_view_facture(object sender, RoutedEventArgs e) {
            int id_maison = Convert.ToInt32(liste_factures.SelectedValue);
            if(id_maison == 0) {
                MessageBox.Show("Merci de sélectionner un client.");
            }
            else {
                ModelPDF modele_facture = new ModelPDF(id_maison);
                ((MetroWindow)this.Parent).Content = modele_facture;
            }
        }

        private void GetAllFactures() {
            DBEntities db = new DBEntities();

            var listing_facture = from maison in db.Maison
                                  join projet in db.Projet on maison.idMaison equals projet.idMaison
                                  join client in db.Client on projet.idClient equals client.idClient
                                  select new {
                                      id_maison = maison.idMaison,
                                      nom_maison = maison.nomMaison,
                                      nom_client = client.nom +" "+ client.prenom
                                  };
            liste_factures.ItemsSource = listing_facture.ToList();
        }
    }
}
