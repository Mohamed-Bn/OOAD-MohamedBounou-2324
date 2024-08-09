using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using CLActiBuddy;
using Microsoft.Win32;

namespace WpfAdmin.PersoonPage
{
    /// <summary>
    /// Interaction logic for PersoonBewerkenPage.xaml
    /// </summary>
    public partial class PersoonBewerkenPage : Page
    {
        Persoon? persoon = null;
        public PersoonBewerkenPage(Persoon persoon)
        {
            InitializeComponent();
            if (persoon == null)
            {
                NavigationService.Navigate(new PersonenOverzichtPage());
                return;
            }

            this.persoon = persoon;
            TxtVoornaam.Text = persoon.Voornaam;
            TxtAchternaam.Text = persoon.Achternaam;
            TxtLogin.Text = persoon.Login;
            ChkIsAdmin.IsChecked = persoon.IsAdmin;
            ImgPersoonFoto.Source = MainWindow.ByteToImage(persoon.Profielfoto);
        }

        private void BtnKiesFoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"
            };

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                try
                {
                    Uri fileUri = new Uri(openFileDialog.FileName);
                    BitmapImage bitmapImage = new BitmapImage();

                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = fileUri;
                    bitmapImage.DecodePixelHeight = 200;
                    bitmapImage.EndInit();

                    ImgPersoonFoto.Source = bitmapImage;
                    persoon.Profielfoto = File.ReadAllBytes(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    LblError.Content = $"Error: {ex.Message}";
                }
            }
        }

        private void BtnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            string voornaam = TxtVoornaam.Text;
            string achternaam = TxtAchternaam.Text;
            string login = TxtLogin.Text;
            string paswoord = TxtPasswordBox.Password;
            bool isAdmin = ChkIsAdmin.IsChecked == true;

            if (string.IsNullOrEmpty(voornaam) ||
                string.IsNullOrEmpty(achternaam) ||
                string.IsNullOrEmpty(login))
            {
                LblError.Content = "Alle velden zijn verplicht!";
                return;
            }
            if (persoon.Profielfoto == null)
            {
                LblError.Content = "Selecteer een profielfoto!";
                return;
            }

            persoon.Voornaam = voornaam;
            persoon.Achternaam = achternaam;
            persoon.Login = login;
            if (!string.IsNullOrEmpty(paswoord))
            {
                persoon.Paswoord = PasswordHashService.QuickHash(paswoord);
            }
            persoon.IsAdmin = isAdmin;
            persoon.RegDatum = DateTime.Now;

            try
            {
                persoon.UpdateInDb();
                NavigationService.Navigate(new PersonenOverzichtPage());
            }
            catch (Exception ex)
            {
                LblError.Content = $"Error: {ex.Message}";
            }
        }

        private void BtnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PersonenOverzichtPage());
        }
    }
}
