using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using CLActiBuddy;

namespace WpfUser
{
    /// <summary>
    /// Interaction logic for ActiviteitDetailPage.xaml
    /// </summary>
    public partial class ActiviteitDetailPage : Page
    {
        public ActiviteitDetailPage(Activiteit activiteit)
        {
            InitializeComponent();

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

        private void BtnTerug_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrganiseerPage());
        }
    }
}
