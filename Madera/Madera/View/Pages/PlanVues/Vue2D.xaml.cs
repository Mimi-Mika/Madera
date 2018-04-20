using Madera.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

            //List<Module> ModuleList = new List<Module>();
            ObservableCollection<Module> ModuleList = new ObservableCollection<Module>();
            foreach (var item in DB.Module.ToList())
            {
                ModuleList.Add(item);
            }

            listModule.ItemsSource = ModuleList;

        }

        private ObservableCollection<Module> moduleList;
        public ObservableCollection<Module> ModuleList
        {
            get { return moduleList; }
            set
            {
                if (value != moduleList)
                {
                    moduleList = value;
                }
            }
        }
    }
    
}
