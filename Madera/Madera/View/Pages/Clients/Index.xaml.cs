using Madera.Model;
using Madera.View.Pages.Tdb;
using MahApps.Metro.Controls;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Madera.View.Pages.Clients
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
        }

        private void Click_btn_retour(object sender, RoutedEventArgs e)
        {
            Tableau_de_bord tdb = new Tableau_de_bord(Master);
            ((MetroWindow)this.Parent).Content = tdb;
        }

        private void btn_add_client(object sender, RoutedEventArgs e)
        {
            
            Create add_client = new Create(Master);
            ((MetroWindow)this.Parent).Content = add_client;
        }

        private void btn_edit_client(object sender, RoutedEventArgs e)
        {
            Client client = (Client)ListeClient.SelectedItem;
            Master.NewClient = client;
            Edit edit_client = new Edit(Master);
            ((MetroWindow)this.Parent).Content = edit_client;
        }

        private void loadClient()
        {
            DBEntities DB = new DBEntities();
            ListeClient.ItemsSource = DB.Client.Select(i => i).ToList();
        }
    }
}