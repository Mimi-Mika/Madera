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
        public ChoixEmpreinte()
        {
            InitializeComponent();
            ChargerEmpreinte();
        }

        private void Click_btn_retour(object sender, RoutedEventArgs e)
        {
            Tableau_de_bord tdb = new Tableau_de_bord();
            ((MetroWindow)this.Parent).Content = tdb;
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
            //int test = lstV.SelectedIndex;
            int col = 0;
            int lig = 0;
            //int i = 0;
            int idZoneMorte = 0;


            DBEntities DB = new DBEntities();
            //foreach (var item in DB.Empreinte)
            //{
            //    if (i == test)
            //    {
            //        col = (int)item.longueur;
            //        lig = (int)item.largeur;
            //        idZoneMorte = (int)item.idZoneMorte;
            //        break;
            //    }
            //    i++;
            //}

            int bb = lstV.SelectedIndex + 1;
            Empreinte empreinteSelection = new Empreinte();
            empreinteSelection = DB.Empreinte.Where(i => i.idEmpreinte == bb).FirstOrDefault();

            col = (int)empreinteSelection.longueur;
            lig = (int)empreinteSelection.largeur;
            if (empreinteSelection.idZoneMorte != null)
            {
                idZoneMorte = (int)empreinteSelection.idZoneMorte;
            }


            Vue2D vue2D = new Vue2D(col, lig, idZoneMorte);
            ((MetroWindow)this.Parent).Content = vue2D;
        }
    }
}
