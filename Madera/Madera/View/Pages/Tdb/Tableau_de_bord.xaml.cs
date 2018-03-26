using Madera.View.Pages.Clients;
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

            Index listing_clients = new Index();
            ((MetroWindow)this.Parent).Content = listing_clients;
        }

        private void btn_add_client(object sender, RoutedEventArgs e) {
            Create add_client = new Create();
            ((MetroWindow)this.Parent).Content = add_client;
        }
    }
}
