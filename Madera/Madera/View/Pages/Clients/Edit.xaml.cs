using Madera.View.Pages.Tdb;
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

namespace Madera.View.Pages.Clients
{
    /// <summary>
    /// Logique d'interaction pour Edit.xaml
    /// </summary>
    public partial class Edit : Page
    {
        public Edit()
        {
            InitializeComponent();

            numero_client.Content = 2018030720;
            date_ajout.Content = "07 / 03 / 2018";
            nom_commercial.Content = "Bertrand RENARD";
            numero_dossier.Content = 2018307201;
        }

        private void Click_btn_annuler(object sender, RoutedEventArgs e) {
            Tableau_de_bord tdb = new Tableau_de_bord();
            ((MetroWindow)this.Parent).Content = tdb;
        }

        private void Click_btn_delete(object sender, RoutedEventArgs e) {
            Index listing_users = new Index();
            ((MetroWindow)this.Parent).Content = listing_users;
        }

        private void Click_btn_retour(object sender, RoutedEventArgs e) {
            Index listing_users = new Index();
            ((MetroWindow)this.Parent).Content = listing_users;
        }
    }
}
