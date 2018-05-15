using Madera.Model;
using Madera.View.Pages.Tdb;
using MahApps.Metro.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            Master = _Master;
            InitializeComponent();
            RemplirListeClient();
        }

        private void Click_btn_retour(object sender, RoutedEventArgs e)
        {
            Tableau_de_bord tdb = new Tableau_de_bord(Master);
            ((MetroWindow)this.Parent).Content = tdb;
        }

        private void Btn_add(object sender, RoutedEventArgs e)
        {
            DBEntities DB = new DBEntities();

            //Done: Reinitialiser le master (sauf le commercial) <== sur la page qui appelle


            long test = Convert.ToInt64(ListeClient.SelectedValue.ToString());
            Master.LockClient = DB.Client.Where(i => i.idClient == test).FirstOrDefault();
            ChoixEmpreinte tdb = new ChoixEmpreinte(Master);
            ((MetroWindow)this.Parent).Content = tdb;
        }

        private void RemplirListeClient()
        {
            DBEntities DB = new DBEntities();
            ListeClient.ItemsSource = DB.Client.ToList();
            ListeClient.DisplayMemberPath = "nom";
            ListeClient.SelectedValuePath = "idClient";
        }
    }
}
