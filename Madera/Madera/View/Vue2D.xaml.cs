﻿using MahApps.Metro.Controls;
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

namespace Madera.View
{
    /// <summary>
    /// Logique d'interaction pour Vue2D.xaml
    /// </summary>
    public partial class Vue2D : Page
    {
        public Vue2D()
        {
            InitializeComponent();
            //test maj github
            CreateEmptyFloorPlan();
        }

        private void CreateEmptyFloorPlan()
        {
            int col = 20;
            int lig = 15;


            // Fois 2 + 1 pour mur (ext et interieur)
            for (int i = 0; i < lig; i++)
            {
                RowDefinition myRow = new RowDefinition();
                myRow.Height = new GridLength(1, GridUnitType.Star);
                grid2D.RowDefinitions.Add(myRow);
            }

            for (int i = 0; i < col; i++)
            {
                ColumnDefinition myCol = new ColumnDefinition();
                myCol.Width = new GridLength(1, GridUnitType.Star);
                grid2D.ColumnDefinitions.Add(myCol);
            }

            //test remplissage de la grille avec des bouttons
            int count = 1;
            for (int i = 0; i < grid2D.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < grid2D.ColumnDefinitions.Count; j++)
                {
                    //TODO faire des templates de button
                    Button MyControl1 = new Button();
                    MyControl1.Content = ("x" + (j + 1).ToString() + " y" + (i + 1).ToString());
                    MyControl1.Name = "Button" + count.ToString();

                    Grid.SetColumn(MyControl1, j);
                    Grid.SetRow(MyControl1, i);
                    MyControl1.Click += new RoutedEventHandler(btnClick);
                    MyControl1.PreviewMouseDown += new MouseButtonEventHandler(btnDown);
                    MyControl1.PreviewMouseUp += new MouseButtonEventHandler(btnUp);
                    //MyControl1.MouseEnter += new System.Windows.Input.MouseEventArgs(mouseEnter);
                    grid2D.Children.Add(MyControl1);

                    count++;
                }
            }

            //for (int i = 0; i < grid2D.ColumnDefinitions.Count; i++)
            //{
            //    if ((i % 2) == 0)
            //        grid2D.ColumnDefinitions[i].Width = new GridLength(15);
            //}

            //for (int i = 0; i < grid2D.RowDefinitions.Count; i++)
            //{
            //    if ((i % 2) == 0)
            //        grid2D.RowDefinitions[i].Height = new GridLength(15);

            //} 
        }


        private void btnClick(object sender, RoutedEventArgs e)
        {
            //Button obj = ((FrameworkElement)sender).DataContext as Button;

            //MessageBox.Show(sender.ToString());
            if (rbMurExt.IsChecked == true)
            {
                ((Button)sender).Background = btnMurExt.Background;
            }
            else if (rbMurInt.IsChecked == true)
            {
                ((Button)sender).Background = btnMurInt.Background;
            }
            else if (rbPorte.IsChecked == true)
            {
                ((Button)sender).Background = btnPorte.Background;
            }
            else if (rbFenetre.IsChecked == true)
            {
                ((Button)sender).Background = btnFenetre.Background;
            }
            else if (rbLibre.IsChecked == true)
            {
                ((Button)sender).Background = btnLibre.Background;
            }

        }

        private void mouseEnter(object sender, MouseEventArgs e)
        {
            //if ((Mouse)e.Button == MouseButtons.Left)
            //{

            //}
        }

        private void btnUp(object sender, MouseButtonEventArgs e)
        {
            
            int row = Grid.GetRow(sender as Button);
            int column = Grid.GetColumn(sender as Button);
            MessageBox.Show("row " + row + " column " + column);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDown(object sender, MouseButtonEventArgs e)
        {
            //enregistre position en x et y du bouton
            //int row = Grid.GetRow(sender as Button);
            //int column = Grid.GetColumn(sender as Button);
            //MessageBox.Show("row " + row + " column " + column);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Apercu3D windows3D = new Apercu3D();
            ((MetroWindow)this.Parent).Content = windows3D;
        }
    }
}
