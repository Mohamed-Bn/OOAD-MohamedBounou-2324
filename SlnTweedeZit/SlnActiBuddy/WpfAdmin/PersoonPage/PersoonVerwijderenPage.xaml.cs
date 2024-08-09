using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using CLActiBuddy;

namespace WpfAdmin.PersoonPage
{
    /// <summary>
    /// Interaction logic for PersoonVerwijderenPage.xaml
    /// </summary>
    public partial class PersoonVerwijderenPage : Page
    {
        Persoon? persoon = null;
        public PersoonVerwijderenPage(Persoon persoon)
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

        private void BtnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PersonenOverzichtPage());
        }

        private void BtnVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                persoon.DeleteFromDb();
                NavigationService.Navigate(new PersonenOverzichtPage());
            }
            catch (Exception ex)
            {
                LblError.Content = $"Error: {ex.Message}";
            }
        }
    }
}
