using Madera.View.Pages.Tdb;
using MahApps.Metro.Controls;
using System.Windows.Controls;

namespace Madera.View.Pages.Clients
{
    /// <summary>
    /// Logique d'interaction pour Create.xaml
    /// </summary>
    public partial class Create : Page
    {
        public Create()
        {
            InitializeComponent();
        }

        private void Click_btn_retour(object sender, System.Windows.RoutedEventArgs e) {
            Index index = new Index();
            ((MetroWindow)this.Parent).Content = index;
        }

        private void Click_btn_annuler(object sender, System.Windows.RoutedEventArgs e) {
            Tableau_de_bord tdb = new Tableau_de_bord();
            ((MetroWindow)this.Parent).Content = tdb;
        }
    }
}
