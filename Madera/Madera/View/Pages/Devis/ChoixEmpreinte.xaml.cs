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
        public ChoixEmpreinte(Nullable<int> id_client)
        {
            InitializeComponent();
            ChargerEmpreinte();
            if (null != id_client) {
                var client = GetInfosClient(Convert.ToInt32(id_client));
                lblNumClient.Content = "#"+client.idClient;
                lblNomClient.Content = client.nom;
                lblPrenomClient.Content = client.prenom;
                lblMail.Content = client.mail;
            }
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
            int idEmpreinte = lstV.SelectedIndex + 1;

            Vue2D vue2D = new Vue2D(idEmpreinte);
            ((MetroWindow)this.Parent).Content = vue2D;
        }

        private Client GetInfosClient(int id_client) {
            DBEntities db = new DBEntities();
            Client client = db.Client.Where(i => i.idClient == id_client).FirstOrDefault();
            return client;
        }
    }
}
