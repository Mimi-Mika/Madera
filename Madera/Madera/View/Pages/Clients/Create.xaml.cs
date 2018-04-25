using Madera.View.Pages.Tdb;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows;
using Madera.Model;

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

        private void Click_btn_annuler(object sender, System.Windows.RoutedEventArgs e)
        {
            Tableau_de_bord tdb = new Tableau_de_bord();
            ((MetroWindow)this.Parent).Content = tdb;
        }

        private void Click_btn_retour(object sender, System.Windows.RoutedEventArgs e)
        {
            Index index = new Index();
            ((MetroWindow)this.Parent).Content = index;
        }

        private void Click_btn_valid(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ControleFormEmpty())
            {
                DBEntities DB = new DBEntities();
                Client client = new Client();
                client.nom = nom.Text;
                client.mail = mail.Text;
                client.prenom = prenom.Text;
                client.tel = telephone.Text;
                client.adresse = adresse.Text;
                DB.Client.Add(client);
                DB.SaveChanges();
                Index listing_users = new Index();
                ((MetroWindow)this.Parent).Content = listing_users;
            }
            else
            {
                MessageBox.Show("Vous devez remplir tous les champs et respecter les formats attendus");
            }
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