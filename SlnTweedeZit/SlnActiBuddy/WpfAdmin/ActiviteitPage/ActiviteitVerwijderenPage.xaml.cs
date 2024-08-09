using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using CLActiBuddy;

namespace WpfAdmin.ActiviteitPage
{
    /// <summary>
    /// Interaction logic for ActiviteitVerwijderenPage.xaml
    /// </summary>
    public partial class ActiviteitVerwijderenPage : Page
    {
        Activiteit? activiteit = null;
        public ActiviteitVerwijderenPage(Activiteit activiteit)
        {
            InitializeComponent();
            this.activiteit = activiteit;

            TxtTitel.Text = activiteit.Titel;
            TxtBeschrijving.Text = activiteit.Beschrijving;
            TxtDatum.Text = activiteit.DatumTijd.ToLongDateString() + " " + activiteit.DatumTijd.ToShortTimeString();
            if (activiteit.Icoon != null)
            {
                ImgIcoon.Source = MainWindow.ByteToImage(activiteit.Icoon);
            }
            TxtLongitude.Text = activiteit.Longitude.ToString();
            TxtLaltitude.Text = activiteit.Latitude.ToString();
            TxtMaxPersonen.Text = activiteit.MaxPersonen.ToString();
            TxtSoort.Text = activiteit.Soort.ToString();
            TxtLeeftijdsgroep.Text = activiteit.Leeftijdsgroep.ToString();
            if (activiteit.Soort == ActiviteitSoort.Hobby)
            {
                LblSpecifiek.Content = "Niveau";
                TxtSpecifiek.Text = ((HobbyActiviteit)activiteit).Niveau?.ToString() ?? "";
            }
            else if (activiteit.Soort == ActiviteitSoort.Cultuur)
            {
                LblSpecifiek.Content = "Sector";
                TxtSpecifiek.Text = ((CultuurActiviteit)activiteit).Sector?.ToString() ?? "";
            }
            else if (activiteit.Soort == ActiviteitSoort.Sport)
            {
                LblSpecifiek.Content = "Moeilijkheid";
                TxtSpecifiek.Text = ((SportActiviteit)activiteit).Moeilijkheid?.ToString() ?? "";
            }
        }

        private void BtnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ActiviteitenOverzichtPage());
        }

        private void BtnVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                activiteit.DeleteFromDb();
                NavigationService.Navigate(new ActiviteitenOverzichtPage());
            }
            catch (Exception ex)
            {
                LblError.Content = $"Error: {ex.Message}";
            }
        }
    }
}
