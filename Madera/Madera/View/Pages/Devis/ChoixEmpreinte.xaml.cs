using Madera.Model;
using Madera.View.Pages.PlanVues;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Madera.View.Pages.Devis
{
    /// <summary>
    /// Logique d'interaction pour ChoixEmpreinte.xaml
    /// </summary>
    public partial class ChoixEmpreinte : Page
    {
        MasterClasse Master = new MasterClasse();

        //Done: Supprimer la variable et passer par la classe master
        //long IdClient;
        public ChoixEmpreinte(MasterClasse _Master)
        {
            Master = _Master;
            InitializeComponent();
            ChargerEmpreinte();
            ChargerTypeDalle();
            GetInfosClient();


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
            Devis.Create choix_client = new Devis.Create(Master);
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

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (lstV.SelectedItem != null)
            {
                DBEntities db = new DBEntities();
                int monte = 0;
                if (RbMontee.IsChecked == true)
                {
                    monte = 1;
                }

                long idEmpreinte = lstV.SelectedIndex + 1;
                long idTypeDalle = Convert.ToInt64(listTypeDalle.SelectedValue.ToString());

                //**********************************************************
                //************* création d'un devis brouillon **************
                //**********************************************************

                //Done: Enregistrer en base le brouillon (Attention a l'ordre d'enregistrement des tables)

                //dans l'ordre:
                //-------------------------------------------------------------------------------
                //         Enregistrer    |     EN BDD     |  EN MASTER  |  COMMENTAIRE
                //------------------------|----------------|-------------|-----------------------
                //1 - TypeDalle           |       NON      |     OUI     |
                //2 - Empreinte           |       NON      |     OUI     |
                //3 - ZoneMorte           |       NON      |     OUI     |
                //4 - Maison              |       OUI      |     OUI     |
                //5 - Maison_TypeDalle    |       OUI      |     OUI     | 
                //6 - Projet              |       OUI      |     OUI     |
                //7 - EtatCommande        |       NON      |     OUI     |
                //8 - Projet_EtatCommande |       OUI      |     OUI     |

                //db.Maison.Add(addMaison);
                //db.SaveChanges();

                //addMaisonTypeDalle.idMaison = addMaison.idMaison;
                //db.Maison_TypeDalle.Add(addMaisonTypeDalle);
                //db.SaveChanges();

                //addProjet.idMaison = addMaison.idMaison;
                //db.Projet.Add(addProjet);
                //db.SaveChanges();


                //Enregistrement des classes Empreinte et TypeDalle en MASTER et ZoneMorte
                Master.LockTypeDalle = db.TypeDalle.Where(i => i.idTypeDalle == idTypeDalle).FirstOrDefault();
                Master.LockEmpreinte = db.Empreinte.Where(i => i.idEmpreinte == idEmpreinte).FirstOrDefault();

                //Enregistrement de la maison en BDD
                Maison addMaison = new Maison()
                {
                    nomMaison = TxtNomMaison.Text,
                    idEmpreinte = Master.LockEmpreinte.idEmpreinte
                };
                db.Maison.Add(addMaison);
                db.SaveChanges(); // (Générer un idMaison)

                //Enregistrer la Maison en Master
                Master.NewMaison = addMaison;

                //Enregistrement Maison_TypeDalle en BDD
                //Done: Prix de la dalle
                double? zoneMorteMoins = 0;
                if (Master.LockZoneMorte != null)
                {
                    zoneMorteMoins = Master.LockTypeDalle.prixM2 * Master.LockZoneMorte.longueur * Master.LockZoneMorte.largeur;
                }

                Maison_TypeDalle addMaisonTypeDalle = new Maison_TypeDalle()
                {
                    historiquePrixM2 = Master.LockTypeDalle.prixM2 * Master.LockEmpreinte.largeur * Master.LockEmpreinte.longueur - zoneMorteMoins,
                    idTypeDalle = Master.LockTypeDalle.idTypeDalle,
                    idMaison = Master.NewMaison.idMaison,
                };
                db.Maison_TypeDalle.Add(addMaisonTypeDalle);
                db.SaveChanges();

                //Enregistrer la Maison_TypeDalle en Master
                Master.NewMaisonTypeDalle = addMaisonTypeDalle;


                //Done: BDD Enregistrer le Commercial
                //Done: Renseigner vrais prix
                //Done: Renseigner vrais nom Devis
                //Enregistrement Projet en BDD
                Projet addProjet = new Projet()
                {
                    nom = TxtNomMaison.Text + "",
                    kitMonte = monte,
                    idClient = Master.LockClient.idClient,
                    idMaison = Master.NewMaison.idMaison,
                    prixFabrication = addMaisonTypeDalle.historiquePrixM2,
                    prixComposant = addMaisonTypeDalle.historiquePrixM2 * 0.3, // pas de compo pour la dalle
                    prixInstallation = addMaisonTypeDalle.historiquePrixM2 * 0.8,
                    prixFinal = addMaisonTypeDalle.historiquePrixM2 * (1 + 0.3 + 0.8),
                    numDevis = "DevCliNo" + "" + (db.Projet.Where(i => i.idClient == Master.LockClient.idClient).Count() + 1),
                    numFacture = "",
                    numOF = "",
                    numOM = "",
                    reductionMarge = 0,
                    devisSynchro = 0,
                    idCommercial = Master.LockCommercial.idCommercial,
                    DernierEtatCommande = 1
                };
                db.Projet.Add(addProjet);
                db.SaveChanges();

                //Enregistrer le Projet en Master
                Master.NewProjet = addProjet;

                //Enregistrer EtatCommande en Master (Brouillon)
                Master.LockEtatCommande = db.EtatCommande.ToList();

                Projet_EtatCommande addProjetEtatCommande = new Projet_EtatCommande()
                {
                    idEtatCommande = 1, // Brouillon
                    idProjet = addProjet.idProjet,
                    dates = DateTime.Now.ToString(),
                    prix = 0,
                    paiementValide = 0
                };
                db.Projet_EtatCommande.Add(addProjetEtatCommande);
                db.SaveChanges();

                //Enregistrer EtatCommande en Master
                Master.NewProjetEtatCommande = new List<Projet_EtatCommande>();
                Master.NewProjetEtatCommande.Add(addProjetEtatCommande);



                Vue2D vue2D = new Vue2D(Master);
                ((MetroWindow)this.Parent).Content = vue2D;
            }
            else
            {
                MessageBox.Show("Choisisez une empreinte");
            }
        }

        private void GetInfosClient()
        {
            //Done: A mettre dans le module "GetinfosClient"
            Client client = Master.LockClient;
            //IdClient = Master.NewClient.idClient;
            lblNumClient.Content = "#" + client.idClient;
            lblNomClient.Content = client.nom;
            lblPrenomClient.Content = client.prenom;
            lblMail.Content = client.mail;

            //Done: Faire qu'un radio bouton soit cocher au lancement
            RbMontee.IsChecked = true;
            //Done: Définir un nom de maison générique
            DateTime dateTime = DateTime.UtcNow.Date;
            TxtNomMaison.Text = "Maison " + client.nom + " " + dateTime.ToString("dd/MM/yyyy");
        }
    }
}
