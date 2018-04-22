using Madera.View.Pages.Clients;
using Madera.View.Pages.Devis;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Madera.View.Pages.Tdb
{
    /// <summary>
    /// Logique d'interaction pour Tdb.xaml
    /// </summary>
    public partial class Tableau_de_bord : Page
    {
        public Tableau_de_bord() {
            InitializeComponent();
        }

        private void Click_btn_clients(object sender, RoutedEventArgs e) {

            Clients.Index listing_clients = new Clients.Index();
            ((MetroWindow)this.Parent).Content = listing_clients;
        }

        private void btn_add_client(object sender, RoutedEventArgs e) {
            Clients.Create add_client = new Clients.Create();
            ((MetroWindow)this.Parent).Content = add_client;
        }

        private void btn_add_devis(object sender, RoutedEventArgs e)
        {
            Devis.Create add_devis = new Devis.Create();
            ((MetroWindow)this.Parent).Content = add_devis;
        }

        private void Click_btn_factures(object sender, RoutedEventArgs e)
        {
            Factures.Index listing_factures = new Factures.Index();
            ((MetroWindow)this.Parent).Content = listing_factures;
        }
    }
}
