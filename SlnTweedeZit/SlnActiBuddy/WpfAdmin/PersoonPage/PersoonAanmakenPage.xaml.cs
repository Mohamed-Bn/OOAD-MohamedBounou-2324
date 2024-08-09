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
    /// Interaction logic for PersoonAanmakenPage.xaml
    /// </summary>
    public partial class PersoonAanmakenPage : Page
    {
        public PersoonAanmakenPage()
        {
            InitializeComponent();
        }

        Persoon nieuwePersoon = new ();

        private void BtnKiesFoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new ()
            {
                Multiselect = false,
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"
            };

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                try
                {
                    Uri fileUri = new (openFileDialog.FileName);
                    BitmapImage bitmapImage = new ();

                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = fileUri;
                    bitmapImage.DecodePixelHeight = 200;
                    bitmapImage.EndInit();

                    ImgPersoonFoto.Source = bitmapImage;
                    nieuwePersoon.Profielfoto = File.ReadAllBytes(openFileDialog.FileName);
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
                string.IsNullOrEmpty(login) ||
                string.IsNullOrEmpty(paswoord))
            {
                LblError.Content = "Alle velden zijn verplicht!";
                return;
            }
            if (nieuwePersoon.Profielfoto == null)
            {
                LblError.Content = "Selecteer een profielfoto!";
                return;
            }

            nieuwePersoon.Voornaam = voornaam;
            nieuwePersoon.Achternaam = achternaam;
            nieuwePersoon.Login = login;
            nieuwePersoon.Paswoord = PasswordHashService.QuickHash(paswoord);
            nieuwePersoon.IsAdmin = isAdmin;
            nieuwePersoon.RegDatum = DateTime.Now;

            try
            {
                nieuwePersoon.InsertInDb();
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

