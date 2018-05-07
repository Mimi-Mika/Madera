using Madera.Model;
using Madera.View.Pages.Tdb;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;


namespace Madera.View.Pages.Devis
{
    /// <summary>
    /// Logique d'interaction pour Edit.xaml
    /// </summary>
    public partial class Edit : Page
    {
        //TODO: Supprimer la page ==> passage direct sur vue 2D


        MasterClasse Master = new MasterClasse();
        public Edit(MasterClasse _Master)
        {
            Master = _Master;
            InitializeComponent();
        }

        private void Click_btn_retour(object sender, RoutedEventArgs e)
        {

            Tableau_de_bord tdb = new Tableau_de_bord(Master);
            ((MetroWindow)this.Parent).Content = tdb;
        }
    }
}
