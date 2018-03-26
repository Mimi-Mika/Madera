using Madera.View;
using MahApps.Metro.Controls;
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
        }

        private void btn_connexion_Click(object sender, RoutedEventArgs e) {
            if(login.Text == "madera" && password.Password == "madera") {
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
