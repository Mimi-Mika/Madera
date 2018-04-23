using Madera.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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

namespace Madera.View.Pages.PlanVues
{
    /// <summary>
    /// Logique d'interaction pour Vue2D.xaml
    /// </summary>
    public partial class Vue2D : Page
    {
        public Vue2D()
        {
            InitializeComponent();
            DBEntities DB = new DBEntities();
            this.DataContext = this;

            // Gamme premiere Liste
            List<Gamme> GammeList = new List<Gamme>();
            GammeList = DB.Gamme.ToList();
            listGamme.ItemsSource = GammeList;
            listGamme.SelectedValuePath = "idGamme";
            listGamme.SelectedIndex = 0; // Selection premier champ de la liste

            //

            List<TypeModule> TypeModuleList = new List<TypeModule>();
            TypeModuleList = DB.TypeModule.ToList();
            listTypeModule.ItemsSource = TypeModuleList;
            listTypeModule.SelectedIndex = 0;


            List<Module> ModuleList = new List<Module>();
            ModuleList = DB.Module.Where(i=>i.TypeModule.idType == listTypeModule.SelectedIndex+1).ToList();
            listModule.ItemsSource = ModuleList;


            // Couleur liste indépendante
            List<Couleur> CouleurList = new List<Couleur>();
            CouleurList = DB.Couleur.ToList();
            listCouleur.ItemsSource = CouleurList;


            


            List<Finition> FinitionList = new List<Finition>();
            FinitionList = DB.Finition.ToList();
            listFinition.ItemsSource = FinitionList;
        }





        private void listTypeModule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DBEntities DB = new DBEntities();
            List<Module> ModuleList = new List<Module>();
            ModuleList = DB.Module.Where(i => i.TypeModule.idType == listTypeModule.SelectedIndex+1).ToList();
            listModule.ItemsSource = ModuleList;

        }

        private void listGamme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // MessageBox.Show(listGamme.SelectedValue.ToString());
        }




        //private ObservableCollection<Module> moduleList;
        //public ObservableCollection<Module> ModuleList
        //{
        //    get { return moduleList; }
        //    set
        //    {
        //        if (value != moduleList)
        //        {
        //            moduleList = value;
        //        }
        //    }
        //}
    }
    
}
