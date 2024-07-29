using System;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CLFitness.WpfCustomer;
using Microsoft.Win32;

namespace WpfAdmin.Pages.person
{
    public partial class new_person : Page
    {
        BitmapImage image;
        public new_person()
        {
            InitializeComponent();
        }

        private void Opslaan_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            Person newPerson = new Person
            {
                FirstName = voorname_box.Text,
                LastName = AchterName_box.Text,
                Login = login_box.Text,
                Password = HashPassword(password_box.Text),
                IsAdmin = is_admin_chk_box.IsChecked ?? false,
                ProfilePhoto = ConvertBitmapImageToByteArray(image)
            };

            try
            {
                string message = Person.InsertPersonIntoDatabase(newPerson);
                if (message == "true")
                {
                    MessageBox.Show("Person added successfully.");
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Error: " + message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void kies_btn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                image = new BitmapImage(new Uri(openFileDialog.FileName));
                person_image.Source = image;
            }
        }

        private byte[] ConvertBitmapImageToByteArray(BitmapImage image)
        {
            byte[] imageData;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                imageData = ms.ToArray();
            }
            return imageData;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(voorname_box.Text) ||
                string.IsNullOrWhiteSpace(AchterName_box.Text) ||
                string.IsNullOrWhiteSpace(password_box.Text) ||
                image == null)
            {
                MessageBox.Show("All fields must be filled.");
                return false;
            }

            if (password_box.Text.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long.");
                return false;
            }

            return true;
        }

        // Hulpfunctie om te controleren of een e-mailadres geldig is.
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

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

        // Hulpfunctie om de invoervelden te wissen.
        private void ClearFields()
        {
            voorname_box.Clear();
            AchterName_box.Clear();
            login_box.Clear();
            password_box.Clear();
            is_admin_chk_box.IsChecked = false;
            person_image.Source = null;
        }

        private void Annuleren_btn_Click(object sender, RoutedEventArgs e)
        {
            persons_overview temp = new persons_overview();
            NavigationService.Navigate(temp);
        }
    }

    // https://stackoverflow.com/questions/75621952/c-sharp-code-for-moqs-setup-and-its-return-in-regards-to-mocking-a-dynamic-pro
    // https://stackoverflow.com/questions/1769951/c-sharp-cancelbutton-closes-dialog
    // https://stackoverflow.com/questions/9531270/change-button-image-after-clicking-it
    // https://www.codeproject.com/Questions/5301504/How-to-make-a-save-and-load-buttons-to-save-and-lo
    // https://stackoverflow.com/questions/13082007/how-should-i-clear-fields-in-generic-static-class
}
