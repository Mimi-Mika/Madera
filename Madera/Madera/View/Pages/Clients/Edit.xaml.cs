using Madera.Model;
using Madera.View.Pages.Tdb;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace Madera.View.Pages.Clients
{
    /// <summary>
    /// Logique d'interaction pour Edit.xaml
    /// </summary>
    public partial class Edit : Page
    {
        private Client client { get; set; }

        public Edit(Client cli)
        {
            InitializeComponent();
            this.client = cli;
            num_client.Content = this.client.idClient.ToString();
            nom.Text = this.client.nom;
            mail.Text = this.client.mail;
            prenom.Text = this.client.prenom;
            telephone.Text = this.client.tel;
            adresse.Text = this.client.adresse;
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
            DB.SaveChanges();
            ((MetroWindow)this.Parent).Content = listing_users;
        }

        private void Click_btn_edit(object sender, RoutedEventArgs e)
        {
            if (ControleFormEmpty())
            {
                DBEntities DB = new DBEntities();
                this.client.nom = nom.Text;
                this.client.mail = mail.Text;
                this.client.prenom = prenom.Text;
                this.client.tel = telephone.Text;
                this.client.adresse = adresse.Text;
                DB.Entry(this.client).CurrentValues.SetValues(this.client);
                DB.SaveChanges();
                Index listing_users = new Index();
                ((MetroWindow)this.Parent).Content = listing_users;
            }
            else
            {
                MessageBox.Show("Vous devez remplir tous les champs et respecter les formats attendus");
            }
        }

        private void Click_btn_retour(object sender, RoutedEventArgs e)
        {
            Index listing_users = new Index();
            ((MetroWindow)this.Parent).Content = listing_users;
        }

        private bool ControleFormEmpty()
        {
            bool valid = false;
            // we test if the fields of the form are filled.
            if (!(new Regex(@"^[A-Za-z]+$")).IsMatch(nom.Text))
            {
                valid = false;
            }
            else if (!(new Regex(@"^[A-Za-z]+$")).IsMatch(prenom.Text))
            {
                valid = false;
            }
            else if (!(new Regex(@"^[_a-z0-9-]+(.[_a-z0-9-]+)*@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})+$")).IsMatch(mail.Text) && mail.Text == null)
            {
                valid = false;
            }
            else if (!(new Regex(@"^[0-9]+$")).IsMatch(telephone.Text))
            {
                valid = false;
            }
            else if (!(new Regex((@"^[A-Za-z]+$")).IsMatch(adresse.Text)) && adresse.Text == null)
            {
                valid = false;
            }
            else
            {
                valid = true;
            }
            return valid;
        }
    }
}