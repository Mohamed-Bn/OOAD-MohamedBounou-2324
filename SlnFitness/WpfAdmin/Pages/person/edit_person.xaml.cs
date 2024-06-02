using CLFitness.WpfCustomer;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfAdmin.Pages.person
{
    public partial class edit_person : Page
    {
        private Person person;

        public edit_person(Person person)
        {
            InitializeComponent();
            this.person = person;
            PopulateUI();
        }

        private void PopulateUI()
        {
            voorname_box.Text = person.FirstName;
            AchterName_box.Text = person.LastName;
            login_box.Text = person.Login;
            password_box.Text = person.Password;
            is_admin_chk_box.IsChecked = person.IsAdmin;

            if (person.ProfilePhoto != null)
            {
                BitmapImage imageSource = new BitmapImage();
                using (MemoryStream stream = new MemoryStream(person.ProfilePhoto))
                {
                    imageSource.BeginInit();
                    imageSource.StreamSource = stream;
                    imageSource.CacheOption = BitmapCacheOption.OnLoad;
                    imageSource.EndInit();
                }
                person_image.Source = imageSource;
            }
        }

        private void Opslaan_btn_Click(object sender, RoutedEventArgs e)
        {
            person.FirstName = voorname_box.Text;
            person.LastName = AchterName_box.Text;
            person.Login = login_box.Text;
            person.Password = password_box.Text;
            person.IsAdmin = is_admin_chk_box.IsChecked ?? false; 

            string result = Person.UpdatePersonInDatabase(person);
            if (result == "true")
            {
                MessageBox.Show("Person updated successfully.");
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show($"Failed to update person: {result}");
            }
        }

       

        private void Annuleren_btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void kies_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

