using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CLFitness.WpfCustomer;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;

namespace WpfAdmin.Pages.person
{
    public partial class new_person : Page
    {
        public new_person()
        {
            InitializeComponent();
        }
        BitmapImage image;
        private void Opslaan_btn_Click(object sender, RoutedEventArgs e)
        {
            Person newPerson = new Person
            {
                FirstName = voorname_box.Text,
                LastName = AchterName_box.Text,
                Login = login_box.Text,
                Password = password_box.Text,
                IsAdmin = is_admin_chk_box.IsChecked ?? false,
                ProfilePhoto = ConvertBitmapImageToByteArray(image)

        };

            if (voorname_box.Text == null || AchterName_box.Text==null || password_box.Text==null || image==null)
                MessageBox.Show("All field must be required to fill");
            string mess = Person.InsertPersonIntoDatabase(newPerson);
            if (mess=="true")
            {
                MessageBox.Show("Person added successfully.");
                voorname_box.Clear();
                AchterName_box.Clear();
                login_box.Clear();
                password_box.Clear();
                is_admin_chk_box.IsChecked = false;
            }
            else
            {
                MessageBox.Show(mess);
            }
        }

        private void kies_btn_Click(object sender, RoutedEventArgs e)
         {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == true) {
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

    }
}
