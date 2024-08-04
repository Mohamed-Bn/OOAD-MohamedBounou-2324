using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using CLActiBuddy;
using Microsoft.Win32;

namespace WpfUser
{
    /// <summary>
    /// Interaction logic for ActiviteitAanmakenPage.xaml
    /// </summary>
    public partial class ActiviteitAanmakenPage : Page
    {
        ActiviteitSoort activiteitSoort = ActiviteitSoort.Sport;

        // ik heb surpress gedaan want []? zegt dat ik spatie moet zetten, en [] ? zegt dat er geen spatie mag
#pragma warning disable SA1011 // Closing square brackets should be spaced correctly
        byte[]? fotoBytes = null;
#pragma warning restore SA1011 // Closing square brackets should be spaced correctly
        Persoon persoon;

        public ActiviteitAanmakenPage()
        {
            InitializeComponent();
            persoon = (Persoon?)Application.Current.Properties["persoon"];

            // bron: chatGPT dumps
            // Haal de waarden van de enum op
            var enumValues = Enum.GetValues(typeof(ActiviteitSoort));

            // Voeg elke enum waarde toe aan de ComboBox
            foreach (var value in enumValues)
            {
                CboSoort.Items.Add(value);
            }

            // Optioneel: Stel de standaard geselecteerde index in
            CboSoort.SelectedIndex = 0;
        }

        private void BtnTerug_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrganiseerPage());
        }

        private void CboSoort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // bron: chatGPT dumps (enkel de if gevraagd om Activiteitsoort uit de CBO te lezen. Inhoud zelf geschreven.)
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ActiviteitSoort selectedSoort)
            {
                // selectedSoort bevat nu de geselecteerde enum-waarde
                activiteitSoort = selectedSoort;
                CboSpecifiek.Items.Clear();

                if (activiteitSoort == ActiviteitSoort.Sport)
                {
                    LblSpecifiek.Content = "Moeilijkheid";

                    // bron: chatGPT dumps
                    // Haal de waarden van de enum op
                    var enumValues = Enum.GetValues(typeof(ActiviteitMoeilijkheid));

                    // Voeg elke enum waarde toe aan de ComboBox
                    foreach (var value in enumValues)
                    {
                        CboSpecifiek.Items.Add(value);
                    }

                    // Optioneel: Stel de standaard geselecteerde index in
                    CboSpecifiek.SelectedIndex = 0;
                }
                else if (activiteitSoort == ActiviteitSoort.Cultuur)
                {
                    LblSpecifiek.Content = "Niveau";

                    // bron: chatGPT dumps
                    // Haal de waarden van de enum op
                    var enumValues = Enum.GetValues(typeof(ActiviteitSector));

                    // Voeg elke enum waarde toe aan de ComboBox
                    foreach (var value in enumValues)
                    {
                        CboSpecifiek.Items.Add(value);
                    }

                    // Optioneel: Stel de standaard geselecteerde index in
                    CboSpecifiek.SelectedIndex = 0;
                }
                else if (activiteitSoort == ActiviteitSoort.Hobby)
                {
                    LblSpecifiek.Content = "Sector";

                    // bron: chatGPT dumps
                    // Haal de waarden van de enum op
                    var enumValues = Enum.GetValues(typeof(ActiviteitNiveau));

                    // Voeg elke enum waarde toe aan de ComboBox
                    foreach (var value in enumValues)
                    {
                        CboSpecifiek.Items.Add(value);
                    }

                    // Optioneel: Stel de standaard geselecteerde index in
                    CboSpecifiek.SelectedIndex = 0;
                }
            }
        }

        private void BtnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            string titel = TxtTitel.Text;
            string beschrijving = TxtBeschrijving.Text;
            DateTime? datum = DteDatum.SelectedDate;
            string longitudeText = TxtLongitude.Text;
            string latitudeText = TxtLaltitude.Text;
            string maxPersonenText = TxtMaxPersonen.Text;
            string leeftijdsGroepText = TxtLeeftijdsgroep.Text;

            decimal longitude;
            decimal latitude;
            int maxPersonen;
            int leeftijdsGroep;

            if (string.IsNullOrEmpty(titel))
            {
                LblError.Content = "Titel verplicht";
                return;
            }
            if (string.IsNullOrEmpty(beschrijving))
            {
                LblError.Content = "Beschrijving verplicht";
                return;
            }
            if (datum == null)
            {
                LblError.Content = "Datum verplicht";
                return;
            }

            // Gebruik CultureInfo met de juiste decimalen scheidingstekens
            var culture = new CultureInfo("nl-NL");

            if (!decimal.TryParse(longitudeText, NumberStyles.Any, culture, out longitude))
            {
                LblError.Content = "Longitude verplicht en moet een getal zijn!";
                return;
            }
            if (!decimal.TryParse(latitudeText, NumberStyles.Any, culture, out latitude))
            {
                LblError.Content = "Latitude verplicht en moet een getal zijn!";
                return;
            }

            // Check of de waarde binnen het bereik valt
            if (longitude < -180 || longitude > 180)
            {
                LblError.Content = "Longitude moet tussen -180 en 180 liggen.";
                return;
            }

            if (latitude < -90 || latitude > 90)
            {
                LblError.Content = "Latitude moet tussen -90 en 90 liggen.";
                return;
            }

            if (!int.TryParse(maxPersonenText, out maxPersonen))
            {
                LblError.Content = "Max personen verplicht en moet een getal zijn!";
                return;
            }
            if (!int.TryParse(leeftijdsGroepText, out leeftijdsGroep))
            {
                LblError.Content = "LeeftijdsGroep verplicht en moet een getal zijn!";
                return;
            }

            // omdat Activiteit abstract is kan ik geen = new Activiteit() doen. daarom afhankelijk van de huidige activiteitsoort, maak ik juiste object aan.
            Activiteit newActiviteit =
                activiteitSoort == ActiviteitSoort.Sport ? new SportActiviteit() :
                activiteitSoort == ActiviteitSoort.Hobby ? new HobbyActiviteit() :
                new CultuurActiviteit();

            if (activiteitSoort == ActiviteitSoort.Sport)
            {
                newActiviteit = new SportActiviteit();

                if (CboSpecifiek.SelectedValue != null)
                {
                    if (Enum.IsDefined(typeof(ActiviteitMoeilijkheid), CboSpecifiek.SelectedValue))
                    {
                        ((SportActiviteit)newActiviteit).Moeilijkheid = (ActiviteitMoeilijkheid)CboSpecifiek.SelectedValue;
                    }
                }
            }
            else if (activiteitSoort == ActiviteitSoort.Hobby)
            {
                newActiviteit = new HobbyActiviteit();
                if (CboSpecifiek.SelectedValue != null)
                {
                    if (Enum.IsDefined(typeof(ActiviteitNiveau), CboSpecifiek.SelectedValue))
                    {
                        ((HobbyActiviteit)newActiviteit).Niveau = (ActiviteitNiveau)CboSpecifiek.SelectedValue;
                    }
                }
            }
            else if (activiteitSoort == ActiviteitSoort.Cultuur)
            {
                newActiviteit = new CultuurActiviteit();
                if (CboSpecifiek.SelectedValue != null)
                {
                    if (Enum.IsDefined(typeof(ActiviteitSector), CboSpecifiek.SelectedValue))
                    {
                        ((CultuurActiviteit)newActiviteit).Sector = (ActiviteitSector)CboSpecifiek.SelectedValue;
                    }
                }
            }

            newActiviteit.Titel = titel;
            newActiviteit.Beschrijving = beschrijving;
            newActiviteit.DatumTijd = (DateTime)datum;
            newActiviteit.Longitude = longitude;
            newActiviteit.Latitude = latitude;
            newActiviteit.MaxPersonen = maxPersonen;
            newActiviteit.Leeftijdsgroep = leeftijdsGroep;
            newActiviteit.Soort = activiteitSoort;
            newActiviteit.Icoon = fotoBytes;
            newActiviteit.OrganisatorId = persoon.Id;
            try
            {
                newActiviteit.InsertInDb();
                NavigationService.Navigate(new OrganiseerPage());
            }
            catch (Exception ex)
            {
                LblError.Content = $"Error: {ex.Message}";
                throw;
            }
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

                    ImgIcoon.Source = bitmapImage;
                    fotoBytes = File.ReadAllBytes(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    LblError.Content = $"Error: {ex.Message}";
                }
            }
        }
    }
}
