using Madera.View.Pages.Tdb;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;

namespace Madera.View.Pages.Clients
{
    /// <summary>
    /// Logique d'interaction pour Index.xaml
    /// </summary>
    public partial class Index : Page
    {
        public Index() {
            InitializeComponent();
        }

        private void Click_btn_retour(object sender, RoutedEventArgs e) {
            Tableau_de_bord tdb = new Tableau_de_bord();
            ((MetroWindow)this.Parent).Content = tdb;
        }

        private void btn_add_client(object sender, RoutedEventArgs e) {
            Create add_client = new Create();
            ((MetroWindow)this.Parent).Content = add_client;
        }

        private void btn_edit_client(object sender, RoutedEventArgs e) {
            Edit edit_client = new Edit();
            ((MetroWindow)this.Parent).Content = edit_client;
        }
    }
}
