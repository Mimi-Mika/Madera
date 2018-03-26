using Madera.View.Pages.Tdb;
using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace Madera.View
{
    /// <summary>
    /// Logique d'interaction pour Home.xaml
    /// </summary>
    public partial class Home : MetroWindow
    {
        public Home() {
            InitializeComponent();
            //home.Source = new Uri("Pages/Tdb/Tdb.xaml", UriKind.Relative);

            this.Content = new Tableau_de_bord();
        }

        private void Click_btn_deconnexion(object sender, RoutedEventArgs e) {
            new MainWindow().Show();
            Close();
        }
    }
}
