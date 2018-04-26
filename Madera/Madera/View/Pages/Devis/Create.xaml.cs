using Madera.Model;
using Madera.View.Pages.Tdb;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Madera.View.Pages.Devis
{
    /// <summary>
    /// Logique d'interaction pour Create.xaml
    /// </summary>
    public partial class Create : Page
    {
        MasterClasse Master = new MasterClasse();
        public Create(MasterClasse _Master)
        {
            InitializeComponent();
            RemplirListeClient();
        }

        private void Click_btn_retour(object sender, RoutedEventArgs e)
        {
            Tableau_de_bord tdb = new Tableau_de_bord(Master);
            ((MetroWindow)this.Parent).Content = tdb;
        }

        private void btn_add(object sender, RoutedEventArgs e)
        {
            DBEntities DB = new DBEntities();
            Master.NewClient = DB.Client.Where(i => i.idClient == Convert.ToInt32(ListeClient.SelectedValue.ToString())).FirstOrDefault();
            ChoixEmpreinte tdb = new ChoixEmpreinte(Master);
            ((MetroWindow)this.Parent).Content = tdb;
        }

        private void RemplirListeClient()
        {
            DBEntities DB = new DBEntities();
            ListeClient.ItemsSource   = DB.Client.ToList();
            ListeClient.DisplayMemberPath = "nom";
            ListeClient.SelectedValuePath = "idClient";
        }
    }
}
