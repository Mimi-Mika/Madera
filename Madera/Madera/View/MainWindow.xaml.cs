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
        MasterClasse Master = new MasterClasse();
        public MainWindow() {
            InitializeComponent();

            //Chemin relatif pour la connection a la BDD
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            path = path.Remove(path.Length - 10);
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
        }

        private void Btn_connexion_Click(object sender, RoutedEventArgs e) {

            DBEntities DB = new DBEntities();

            Commercial testLogin = DB.Commercial.FirstOrDefault(u => u.nom == login.Text && u.mdp == password.Password);

            if (testLogin !=null) {
                Master.NewCommercial = testLogin;
                Home home = new Home(Master);
                home.Show();
                Close();
                
            }
            else {
                error_message.Content = "Login ou mot de passe incorrect !";
            }



        }
    }
}
