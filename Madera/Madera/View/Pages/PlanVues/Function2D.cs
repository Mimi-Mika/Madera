using Madera.Model;
using Madera.View.Pages.Devis;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Madera.View.Pages.PlanVues
{
    class Function2D
    {
        public ImageBrush ChoisirLeBrush(String nom)
        {
            var brush = new ImageBrush();
            //Si mur exterieur

            if (nom.Contains("Exterieur"))
            {
                if (nom.Contains("Porte"))
                {
                    brush.ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/PorteHorizontal.png", UriKind.Relative));
                }

                if (nom.Contains("Fenetre"))
                {
                    brush.ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/FenetreHorizontal.png", UriKind.Relative));
                }

                if (nom.Contains("Mur"))
                {
                    brush.ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/imgMurExt.jpg", UriKind.Relative));
                }
            }
            //Si mur Interieur
            else
            {
                if (nom.Contains("Porte"))
                {
                    brush.ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/PorteHorizontal.png", UriKind.Relative));
                }

                if (nom.Contains("Fenetre"))
                {
                    brush.ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/FenetreHorizontal.png", UriKind.Relative));
                }

                if (nom.Contains("Mur"))
                {
                    brush.ImageSource = new BitmapImage(new Uri("../../Pictures/Vue2D/imgMurInt.jpg", UriKind.Relative));
                }
            }
            return brush;
        }

        public long? ChoisirLaHauteur(String nom, MasterClasse Master, Module module)
        {
            long? Hauteur = 0;
            if (nom.Contains("Fenetre"))
            {
                Hauteur = 250 - 30 - Master.LockModule.Where(i => i.idModule == module.idModule).FirstOrDefault().hauteur;
            }
            return Hauteur;
        }
    }
}
