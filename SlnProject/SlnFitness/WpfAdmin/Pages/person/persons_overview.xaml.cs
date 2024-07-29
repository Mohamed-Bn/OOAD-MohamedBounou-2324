using CLFitness.WpfCustomer;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Drawing;
using System.IO;
using System.Xml.Linq;
using System.Drawing.Imaging;

namespace WpfAdmin.Pages.person
{
    public partial class persons_overview : Page
    {
        List<Person_name> persons;
        private Person selectedPerson;

        public persons_overview()
        {
            InitializeComponent();
            persons = Person_name.GetAllPerson();
            AddDynamicContent();  
        }

        private void AddDynamicContent()
        {
            for (int i = 0; i < persons.Count; i++)
            {
                TextBlock newTextBlock = new TextBlock();
                newTextBlock.Text = i + 1 + ".  " + persons[i].FirstName + " " + persons[i].LastName;
                newTextBlock.Margin = new Thickness(2);
                newTextBlock.MouseLeftButtonUp += show_person_info;
                newTextBlock.Tag = persons[i].Id;
                stackPanel.Children.Add(newTextBlock);
            }
        }

        private void show_person_info(object sender, RoutedEventArgs e)
        {
            int id = (int)((TextBlock)sender).Tag;
            Person person = Person.GetPerson(id);
            selectedPerson = Person.GetPerson(id);

            if (person == null)
            {
                MessageBox.Show("Person not found.");
                return;
            }

            name.Content = person.FirstName;
            reg_no.Content = person.RegDate.ToString("MMM-dd yyyy");
            admin.Content = person.IsAdmin ? "Yes" : "No";

            if (person.ProfilePhoto != null && person.ProfilePhoto.Length > 0)
            {
                BitmapImage bitmapImage = ByteArrayToBitmapImage(person.ProfilePhoto);
                img_place.Source = bitmapImage;
            }
            else
            {
                img_place.Source = new BitmapImage(new Uri("default_profile_image.png", UriKind.Relative));
            }
        }

        public static BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                throw new ArgumentException("Byte array is empty or null");

            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        private void btn_click_add_person(object sender, RoutedEventArgs e)
        {
            new_person temp = new new_person();
            NavigationService.Navigate(temp);
        }

        private void btn_click_edit_person(object sender, RoutedEventArgs e)
        {
            if (selectedPerson == null)
            {
                MessageBox.Show("Please select a person.");
                return;
            }

            edit_person temp = new edit_person(selectedPerson);
            NavigationService.Navigate(temp);
        }

        private Frame FindFrame(DependencyObject parent)
        {
            if (parent == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is Frame frame)
                {
                    return frame;
                }
                else
                {
                    var result = FindFrame(child);
                    if (result != null) return result;
                }
            }

            return null;
        }

        private void btn_click_remove_person(object sender, RoutedEventArgs e)
        {
            if (selectedPerson == null)
            {
                MessageBox.Show("Please select a person.");
                return;
            }

            delete_person temp = new delete_person(selectedPerson);
            NavigationService.Navigate(temp);
        }
    }

    // https://stackoverflow.com/questions/75621952/c-sharp-code-for-moqs-setup-and-its-return-in-regards-to-mocking-a-dynamic-pro
    // https://stackoverflow.com/questions/1769951/c-sharp-cancelbutton-closes-dialog
    // https://stackoverflow.com/questions/9531270/change-button-image-after-clicking-it
    // https://www.codeproject.com/Questions/5301504/How-to-make-a-save-and-load-buttons-to-save-and-lo
    // https://stackoverflow.com/questions/13082007/how-should-i-clear-fields-in-generic-static-class
    // chatgpt
}
