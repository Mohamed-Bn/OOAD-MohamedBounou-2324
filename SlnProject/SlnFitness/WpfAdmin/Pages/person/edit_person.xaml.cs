using CLFitness.WpfCustomer;
using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfAdmin.Pages.person
{
    public partial class edit_person : Page
    {
        private Person person;

        // Constructor die de huidige persoon initialiseert en de UI-elementen invult.
        public edit_person(Person person)
        {
            InitializeComponent();
            this.person = person;
            PopulateUI();
        }

        // Methode om de UI-elementen te vullen met de gegevens van de huidige persoon.
        private void PopulateUI()
        {
            voorname_box.Text = person.FirstName;
            AchterName_box.Text = person.LastName;
            login_box.Text = person.Login;
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

        // Event handler voor de 'Opslaan' knop
        private void Opslaan_btn_Click(object sender, RoutedEventArgs e)
        {
            person.FirstName = voorname_box.Text;
            person.LastName = AchterName_box.Text;
            person.Login = login_box.Text;
            person.IsAdmin = is_admin_chk_box.IsChecked ?? false;

            if (!string.IsNullOrWhiteSpace(password_box.Text))
            {
                if (password_box.Text.Length < 8)
                {
                    MessageBox.Show("Password must be at least 8 characters long.");
                    return;
                }

                person.Password = HashPassword(password_box.Text);
            }

            string result = Person.UpdatePersonInDatabase(person);
            if (result == "true")
            {
                MessageBox.Show("Person updated successfully.");
                persons_overview temp = new persons_overview();
                NavigationService.Navigate(temp);
            }
            else
            {
                MessageBox.Show($"Failed to update person: {result}");
            }
        }

        // Hulpfunctie om een wachtwoord te hashen.
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Event handler voor de 'Annuleren' knop.
        private void Annuleren_btn_Click(object sender, RoutedEventArgs e)
        {
            persons_overview temp = new persons_overview();
            NavigationService.Navigate(temp);
        }

        // Event handler voor de 'Kies' knop.
        private void kies_btn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (Stream fileStream = openFileDialog.OpenFile())
                    {
                        BitmapImage newImage = new BitmapImage();
                        newImage.BeginInit();
                        newImage.StreamSource = fileStream;
                        newImage.CacheOption = BitmapCacheOption.OnLoad;
                        newImage.EndInit();

                        person_image.Source = newImage;

                        using (MemoryStream ms = new MemoryStream())
                        {
                            BitmapEncoder encoder = new PngBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(newImage));
                            encoder.Save(ms);
                            person.ProfilePhoto = ms.ToArray();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
    }

    // https://stackoverflow.com/questions/75621952/c-sharp-code-for-moqs-setup-and-its-return-in-regards-to-mocking-a-dynamic-pro
    // https://stackoverflow.com/questions/1769951/c-sharp-cancelbutton-closes-dialog
    // https://stackoverflow.com/questions/9531270/change-button-image-after-clicking-it
    // https://www.codeproject.com/Questions/5301504/How-to-make-a-save-and-load-buttons-to-save-and-lo
}
