﻿using Madera.View.Pages.Tdb;
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

namespace Madera.View.Pages.Factures
{
    /// <summary>
    /// Logique d'interaction pour Index.xaml
    /// </summary>
    public partial class Index : Page
    {
        public Index() {
            InitializeComponent();
        }

        private void Click_btn_retour(object sender, RoutedEventArgs e) {
            Tableau_de_bord tdb = new Tableau_de_bord();
            ((MetroWindow)this.Parent).Content = tdb;
        }

        private void Click_btn_view_facture(object sender, RoutedEventArgs e) {
            ModelPDF modele_facture = new ModelPDF();
            ((MetroWindow)this.Parent).Content = modele_facture;
        }
    }
}