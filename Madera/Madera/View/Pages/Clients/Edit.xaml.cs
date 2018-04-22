using Madera.Model;
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
        private Client client { get; set; }
        private Commercial comConnect { get; set; }
        private Projet_EtatCommande projet_EtatCommande { get; set; }
        private Projet projet { get; set; }

        public Edit(Client cli, Projet proj, Commercial comCon, Projet_EtatCommande projEtatCmd)
        {
            InitializeComponent();
            this.client = cli;
            this.projet = proj;
            this.comConnect = comCon;
            this.projet_EtatCommande = projEtatCmd;
            numero_client.Content = this.client.idClient;
            nom.Text = this.client.nom;
            mail.Text = this.client.mail;
            prenom.Text = this.client.prenom;
            telephone.Text = this.client.tel;
            adresse.Text = this.client.adresse;
            if (this.projet_EtatCommande.idEtatCommande.Equals(this.projet.idProjet))
            {
                date_ajout.Content = this.projet_EtatCommande.dates;
            }

            // init select
            DBEntities DB = new DBEntities();
            List<Commercial> listCommercials = new List<Commercial>();
            listCommercials = DB.Commercial.Select(i => i).ToList();
            commercial.Items.Add(listCommercials);
            commercial.SelectedItem(comConnect.idCommercial);

            if (this.projet.idClient.Equals(this.client.idClient))
            {
                numero_dossier.Content = this.projet.idProjet;
            }
        }

        private void Click_btn_annuler(object sender, RoutedEventArgs e)
        {
            Tableau_de_bord tdb = new Tableau_de_bord();
            ((MetroWindow)this.Parent).Content = tdb;
        }

        private void Click_btn_delete(object sender, RoutedEventArgs e)
        {
            Index listing_users = new Index();
            DBEntities DB = new DBEntities();

            Client.ItemsSource = DB.Client.Remove(this.client);
            ((MetroWindow)this.Parent).Content = listing_users;
        }

        private void Click_btn_edit(object sender, RoutedEventArgs e)
        {
            Index listing_users = new Index();
            DBEntities DB = new DBEntities();
            this.client.nom = nom.Text;
            this.client.mail = mail.Text;
            this.client.prenom = prenom.Text;
            this.client.tel = telephone.Text;
            this.client.adresse = adresse.Text;
            ((MetroWindow)this.Parent).Content = listing_users;
        }

        private void Click_btn_retour(object sender, RoutedEventArgs e)
        {
            Index listing_users = new Index();
            ((MetroWindow)this.Parent).Content = listing_users;
        }
    }
}