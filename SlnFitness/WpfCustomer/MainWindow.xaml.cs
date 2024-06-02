using CLFitness.WpfCustomer;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace WpfCustomer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Person loggedInPerson;

        public MainWindow(Person loggedInVal)
        {
            InitializeComponent();
            loggedInPerson = loggedInVal;
            SetProfileImage(loggedInPerson.ProfilePhoto);
        }

        private void SetProfileImage(byte[] profilePhoto)
        {
            if (profilePhoto == null || profilePhoto.Length == 0)
            {
                return;
            }

            BitmapImage bitmap = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(profilePhoto))
            {
                stream.Seek(0, SeekOrigin.Begin);
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
            }

            foto_abdel.Source = bitmap;
        }

        private void StatistiekenButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Pages.Statistics_Customer(loggedInPerson));
        }

        private void WorkoutsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Pages.Workout_Customer(loggedInPerson));
        }

        private void logout(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
