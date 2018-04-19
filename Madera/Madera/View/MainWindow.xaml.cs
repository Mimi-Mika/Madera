using Madera.Model;
using Madera.View;
using MahApps.Metro.Controls;
using System;
using System.Linq;
using System.Windows;


namespace Madera
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow() {
            InitializeComponent();

            //Chemin relatif pour la connection a la BDD
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            path = path.Remove(path.Length - 10);
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
        }

        private void btn_connexion_Click(object sender, RoutedEventArgs e) {

            DBEntities DB = new DBEntities();

            var testLogin = DB.Commercial.FirstOrDefault(u => u.nom == login.Text
                     && u.mdp == password.Password);


            DB.Client.Add(new Client {
                nom = "Asticot",
                prenom ="Joe",
                adresse ="20 rue des Pommiers",
                tel="0561856235",
                mail="astiJ@gogo.com"
            });

            DB.Client.ToList();
            int bb= 2;
            Empreinte empreinteSelection= new Empreinte();
            empreinteSelection=DB.Empreinte.Where(i => i.idEmpreinte == bb).FirstOrDefault();
            

            //var Maison=DB.Maison.Include("Maison_TypeDalle").Include("TypeDalle").Include("Empreinte");
            var Maison = DB.Maison.Where(s => s.idMaison == 1).FirstOrDefault();
            
            var bob=Maison.Maison_TypeDalle.FirstOrDefault().TypeDalle.prixM2;
            
            Maison_TypeDalle testajout = new Maison_TypeDalle() {historiquePrixM2=25, idMaison=Maison.idMaison,idTypeDalle=2 };

            DB.Maison_TypeDalle.Add(testajout);
            DB.SaveChanges();
            

            if (testLogin !=null) {
                Home home = new Home();
                home.Show();
                Close();
            }
            else {
                error_message.Content = "Login ou mot de passe incorrect !";
            }



        }
    }
}
