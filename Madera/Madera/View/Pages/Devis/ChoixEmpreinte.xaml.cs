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
    /// Logique d'interaction pour ChoixEmpreinte.xaml
    /// </summary>
    public partial class ChoixEmpreinte : Page
    {
        int IdClient;
        public ChoixEmpreinte(int id_client)
        {
            InitializeComponent();
            ChargerEmpreinte();
            ChargerTypeDalle();

            var client = GetInfosClient(Convert.ToInt32(id_client));
            IdClient = id_client;
            lblNumClient.Content = "#"+client.idClient;
            lblNomClient.Content = client.nom;
            lblPrenomClient.Content = client.prenom;
            lblMail.Content = client.mail;
        }

        private void ChargerTypeDalle()
        {
            DBEntities DB = new DBEntities();

            // Gamme premiere Liste
            List<TypeDalle> typeDalle = new List<TypeDalle>();
            typeDalle = DB.TypeDalle.ToList();
            listTypeDalle.ItemsSource = typeDalle;
            listTypeDalle.SelectedValuePath = "idTypeDalle";
            listTypeDalle.SelectedIndex = 0; // Selection premier champ de la liste

        }

        private void Click_btn_retour(object sender, RoutedEventArgs e)
        {
            Devis.Create choix_client = new Devis.Create();
            ((MetroWindow)this.Parent).Content = choix_client;
        }

        private void ChargerEmpreinte()
        {
            //lsvEmpreinte.View = ListView.ViewProperty.
            DBEntities DB = new DBEntities();
            foreach (var item in DB.Empreinte)
            {
                //ListViewItem myItem = new ListViewItem();
                //Image img = new Image();
                //img.Source = new BitmapImage(new Uri("../../../Pictures/home.png", UriKind.Relative));
                //myItem.Content = img;
                //this.lstV.Items.Add(myItem);
                this.lstV.Items.Add(new Image() { Source = new BitmapImage(new Uri("../../../Pictures/" + item.nom + ".png", UriKind.Relative)) });

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DBEntities db = new DBEntities();
            Projet projet = new Projet();

            projet.

            if (lstV.SelectedItem != null)
            {
                int idEmpreinte = lstV.SelectedIndex + 1;

                Vue2D vue2D = new Vue2D(idEmpreinte, IdClient);
                ((MetroWindow)this.Parent).Content = vue2D;
            }
            
        }

        private Client GetInfosClient(int id_client) {
            DBEntities db = new DBEntities();
            Client client = db.Client.Where(i => i.idClient == id_client).FirstOrDefault();
            return client;
        }
    }
}
