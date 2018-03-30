using Madera.View.Pages.Tdb;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows;

namespace Madera.View.Pages.Clients
{
    /// <summary>
    /// Logique d'interaction pour Create.xaml
    /// </summary>
    public partial class Create : Page
    {
        public Create()
        {
            InitializeComponent();
            bool valid = ControleFormEmpty();
            if (valid)
            {
                AddCustomer();
            }
            else
            {
                DisplayMsgError();
            }
        }

        private void AddCustomer()
        {
        }

        private void Click_btn_annuler(object sender, System.Windows.RoutedEventArgs e)
        {
            Tableau_de_bord tdb = new Tableau_de_bord();
            ((MetroWindow)this.Parent).Content = tdb;
        }

        private void Click_btn_retour(object sender, System.Windows.RoutedEventArgs e)
        {
            Index index = new Index();
            ((MetroWindow)this.Parent).Content = index;
        }

        private void Click_btn_valid(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void DatePicker_DateChanged(object sender, SelectionChangedEventArgs e)
        {
            var datePicker = sender as DatePicker;
            if (datePicker.SelectedDate == null)
            {
            }
            else
            {
            }
        }

        private void Combobox_SelectChanged(object sender, SelectionChangedEventArgs e)
        {
            var combobox = sender as ComboBox;
            if (combobox.SelectedItem == null)
            {
            }
            else
            {
                MessageBox.Show("Tous les champs en rouge sont obligatoire !");
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            try
            {
                // Get control that raised this event.
                // Change Window Title.
                //this.Title = textBox.Text + "[Length = " + textBox.Text.Length.ToString() + "]";

                if (textBox.Text == null)
                {
                    // textBox.ForeColor = Color.Red;
                }
                else
                {
                    // textBox.ForeColor = Color.White;
                }
            }
            catch
            {
                // If there is an error, display the text using the system colors.
                //textBox.ForeColor = SystemColors.ControlTextColor;
            }
        }

        private bool ControleFormEmpty()
        {
            bool valid = false;
            // we test if the fields of the form are filled.
            if (!(new Regex(@"^[A-Za-z]+$")).IsMatch(nom.Text))
            {
                valid = false;
            }
            else if (!(new Regex(@"^[A-Za-z]+$")).IsMatch(prenom.Text))
            {
                valid = false;
            }
            else if (!(new Regex(@"^[_a-z0-9-]+(.[_a-z0-9-]+)*@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})+$")).IsMatch(mail.Text) && mail.Text == null)
            {
                valid = false;
            }
            else if (!(new Regex(@"^[0-9]+$")).IsMatch(telephone.Text))
            {
                valid = false;
            }
            else if (!(new Regex(@"^((0[1-9])|([1-8][0-9])|(9[0-8])|(2A)|(2B))[0-9]{3}+$")).IsMatch(code_postal.Text) && code_postal.Text == null)
            {
                valid = false;
            }
            else if (!(new Regex((@"^[A-Za-z]+$")).IsMatch(ville.Text)) && ville.Text == null)
            {
                valid = false;
            }
            else if (!(new Regex((@"^[A-Za-z]+$")).IsMatch(adresse.Text)) && adresse.Text == null)
            {
                valid = false;
            }
            else if (!(new Regex((@"^[0-9]+$")).IsMatch(budget.Text)) && budget.Text == null)
            {
                valid = false;
            }
            else
            {
                valid = true;
            }

            return valid;
        }

        private void DisplayMsgError()
        {
        }
    }
}