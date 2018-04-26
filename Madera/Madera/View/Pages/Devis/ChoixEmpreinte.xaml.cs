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
            string addNomMaison = string.Empty;
            string nomProjet = string.Empty;
            int monte = 0;
            long idEmpreinte = lstV.SelectedIndex + 1;
            long idTypeDalle = 0;
            long idClient = 0;


            DBEntities db = new DBEntities();

            //Création des classes Empreinte et TypeDalle
            Empreinte addEmpreinte = new Empreinte();
            TypeDalle addTypeDalle = new TypeDalle();
            addEmpreinte = db.Empreinte.Where(i => i.idEmpreinte == idEmpreinte).FirstOrDefault();
            addTypeDalle = db.TypeDalle.Where(i => i.idTypeDalle == idTypeDalle).FirstOrDefault();

            
            //Création maison type dalle
            Maison_TypeDalle addMaisonTypeDalle = new Maison_TypeDalle()
            {
                historiquePrixM2 = addTypeDalle.prixM2,
                idTypeDalle= idTypeDalle,
                TypeDalle =addTypeDalle
                // Manque liaison Maison
                // Manque idMaison
            };

            List<Maison_TypeDalle> lstMaiTypDal = new List<Maison_TypeDalle>() { addMaisonTypeDalle };
            //lstMaiTypDal.Add(addMaisonTypeDalle);

            //Client
            Client addClient = new Client();
            addClient = db.Client.Where(i => i.idClient == idClient).FirstOrDefault();

            //Création projet
            Projet addProjet = new Projet();
            List<Projet> lstPrj = new List<Projet>() { addProjet };
            //lstPrj.Add(addProjet);

            
            Maison addMaison = new Maison()
            {
                nomMaison = addNomMaison,
                idEmpreinte = idEmpreinte,
                Empreinte = addEmpreinte,
                Maison_TypeDalle = lstMaiTypDal,
                Projet= lstPrj
            };

            addProjet.idMaison = addMaison.idMaison;
            addProjet.nom = nomProjet;
            addProjet.kitMonte = monte;
            addProjet.Maison = addMaison;
            addProjet.Client = addClient;
            addProjet.idClient = idClient;

            //Enregistrer
            //db.Maison_TypeDalle.Add(addMaisonTypeDalle);
            //db.Maison.Add(addMaison);
            //db.Projet.Add(addProjet);
            //db.SaveChanges();





            if (lstV.SelectedItem != null)
            {
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
