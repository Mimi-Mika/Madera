﻿using Madera.Model;
using Madera.View.Pages.Tdb;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logique d'interaction pour Create.xaml
    /// </summary>
    public partial class Create : Page
    {
        public Create()
        {
            InitializeComponent();
            RemplirListeClient();
        }

        private void Click_btn_retour(object sender, RoutedEventArgs e)
        {
            Tableau_de_bord tdb = new Tableau_de_bord();
            ((MetroWindow)this.Parent).Content = tdb;
        }

        private void btn_add(object sender, RoutedEventArgs e)
        {
            int id_client = Convert.ToInt32(ListeClient.SelectedValue.ToString());
            if(id_client == 0) {
                MessageBox.Show("Merci de sélectionner un client dans la liste.");
            }
            else {
                ChoixEmpreinte tdb = new ChoixEmpreinte(id_client);
                ((MetroWindow)this.Parent).Content = tdb;
            }
        }

        private void RemplirListeClient()
        {
            DBEntities DB = new DBEntities();
            ListeClient.ItemsSource   = DB.Client.ToList();
            ListeClient.DisplayMemberPath = "nom";
            ListeClient.SelectedValuePath = "idClient";
        }
    }
}
