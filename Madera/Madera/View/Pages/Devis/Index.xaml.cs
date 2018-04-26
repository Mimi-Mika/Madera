using Madera.Model;
using Madera.View.Pages.PlanVues;
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

namespace Madera.View.Pages.Devis
{
    
    /// <summary>
    /// Logique d'interaction pour Index.xaml
    /// </summary>
    public partial class Index : Page
    {
        MasterClasse Master = new MasterClasse();
        int IdClient;
        int IdEmpreinte;

        public Index()
        {
            InitializeComponent();
            loadClient();
            loadDevis();
        }

        private void loadClient()
        {
            DBEntities DB = new DBEntities();
            cmbClient.ItemsSource = DB.Client.ToList();
            cmbClient.DisplayMemberPath = "nom";
            cmbClient.SelectedValuePath = "idClient";
        }

        private void Click_btn_retour(object sender, RoutedEventArgs e)
        {
            Tableau_de_bord tdb = new Tableau_de_bord(Master);
            ((MetroWindow)this.Parent).Content = tdb;
        }

        private void btn_ajout(object sender, RoutedEventArgs e)
        {
            Devis.Create tdb = new Devis.Create(Master);
            ((MetroWindow)this.Parent).Content = tdb;
        }

        private void btnChoisirDevis_Click(object sender, RoutedEventArgs e)
        {
            if (ListeDevis.SelectedItem != null)
            {
                DBEntities DB = new DBEntities();

                Projet projet = new Projet();
                projet = (Projet)ListeDevis.SelectedItem;

                IdClient = (int)projet.idClient.Value;

                var truc = DB.Maison.Where(i => i.idMaison == (int)projet.idMaison).FirstOrDefault();
                IdEmpreinte = (int)truc.idEmpreinte;

                Vue2D vue2d = new Vue2D(Master);
                ((MetroWindow)this.Parent).Content = vue2d;
                
            }
            else
            {
                MessageBox.Show("Merci de selectionner un Devis");
            }
            
        }

        private void loadDevis()
        {
            DBEntities DB = new DBEntities();
            ListeDevis.ItemsSource = DB.Projet.Select(i => i).ToList();
            ListeDevis.SelectedValuePath = "idClient";
        }

        private void cmbClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id_client = Convert.ToInt32(cmbClient.SelectedValue.ToString());
            DBEntities DB = new DBEntities();
            ListeDevis.ItemsSource = DB.Projet.Where(i => i.idClient == id_client).ToList();
            ListeDevis.SelectedValuePath = "idClient";
        }
    }
}
