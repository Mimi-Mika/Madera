using Madera.Model;
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
using System.Windows.Shapes;

namespace Madera.View.Pages.MessageBoxCustom
{
    /// <summary>
    /// Logique d'interaction pour msgbox.xaml
    /// </summary>
    public partial class msgbox : Window
    {
        public msgbox(MasterClasse Master, Module_Maison ModMaison)
        {
            InitializeComponent();
            Module Mod = Master.LockModule.Where(i => i.idModule == ModMaison.idModule).FirstOrDefault();
            string test = ModMaison.Module.imgUrl.Substring(18);

            imgModule.Source = new BitmapImage(new Uri(@"../../../" + test, UriKind.Relative)); // ModMaison.Module.imgUrl;
            lblNomModule.Content = ModMaison.Module.nom;
            lblPrixModule.Content = ModMaison.historiquePrixModule + "€";
            lblCouleur.Content = Master.LockCouleur.Where(i => i.idCouleur == ModMaison.idCouleur).FirstOrDefault().nom;
            lblPrixCouleur.Content = ModMaison.historiquePrixCouleur + "€";
            lblFinition.Content = Master.LockGamme.Where(i => i.idGamme == Mod.idGamme).FirstOrDefault().finition;
            lblHuisserie.Content = Master.LockGamme.Where(i => i.idGamme == Mod.idGamme).FirstOrDefault().isolation;
        }
    }
}
