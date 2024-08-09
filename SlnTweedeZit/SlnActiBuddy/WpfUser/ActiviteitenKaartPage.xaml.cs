using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CLActiBuddy;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;

namespace WpfUser
{
    /// <summary>
    /// Interaction logic for ActiviteitenKaartPage.xaml
    /// </summary>
    public partial class ActiviteitenKaartPage : Page
    {
        List<Activiteit> activiteiten = new List<Activiteit>();
        List<Activiteit> filteredActiviteiten = new List<Activiteit>();
        Activiteit? selectedActiviteit = null;
        Persoon? persoon = null;
        bool isIngeschreven = false;

        public ActiviteitenKaartPage()
        {
            InitializeComponent();
            try
            {
                activiteiten = Activiteit.GetAllActiviteiten();
            }
            catch (Exception ex)
            {
                LblError.Content = $"Error: {ex.Message}";
                throw;
            }
            InitializeMap();
            persoon = (Persoon)Application.Current.Properties["persoon"];

            List<int> leeftijden = new List<int>();

            foreach (Activiteit activiteit in activiteiten)
            {
                if (!leeftijden.Contains(activiteit.Leeftijdsgroep))
                {
                    leeftijden.Add(activiteit.Leeftijdsgroep);
                }
            }

            leeftijden.Sort();

            CboLeeftijdscategorie.Items.Add("All");

            foreach (int leeftijd in leeftijden)
            {
                CboLeeftijdscategorie.Items.Add(leeftijd);
            }
        }

        private static byte[] LoadImage(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    return File.ReadAllBytes(filePath);
                }
                else
                {
                    Console.WriteLine($"File not found: {filePath}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image from {filePath}: {ex.Message}");
                return null;
            }
        }

        // ChatGPT (Geen GMaps documentatie online)
        private void InitializeMap()
        {
            // Basisinstellingen voor de kaart
            MainMap.MapProvider = GMapProviders.GoogleMap;
            GMaps.Instance.Mode = AccessMode.ServerAndCache; // ServerOnly mode, you can also use CacheOnly or ServerAndCache
            MainMap.Position = new PointLatLng(50.8503, 4.3517); // Brussels coordinates
            MainMap.MinZoom = 2;
            MainMap.MaxZoom = 25;
            MainMap.Zoom = 15;
            MainMap.MouseWheelZoomEnabled = true;

            // Extra instellingen
            MainMap.CanDragMap = true;
            MainMap.DragButton = MouseButton.Left;
            MainMap.ShowCenter = false; // Verberg het kruis in het midden

            foreach (var activiteit in activiteiten)
            {
                AddMarker(activiteit);
            }
        }

        private void AddMarker(Activiteit activiteit)
        {
            // Maak een nieuwe marker
            GMapMarker marker = new GMapMarker(new PointLatLng((double)activiteit.Latitude, (double)activiteit.Longitude));

            // Creëer een image element voor het icoon
            Image icon = new Image
            {
                Width = 30,
                Height = 30,
                Source = MainWindow.ByteToImage(activiteit.Icoon) ?? MainWindow.ByteToImage(LoadImage("Assets/noIcon.png")),
                Tag = activiteit
            };

            icon.MouseLeftButtonUp += Icon_MouseLeftButtonUp;

            // Voeg de image toe aan de marker
            marker.Shape = icon;

            // Voeg de marker toe aan de kaart
            MainMap.Markers.Add(marker);
        }

        private void Icon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isIngeschreven = false;

            selectedActiviteit = (Activiteit)((Image)sender).Tag;
            LblTitel.Content = selectedActiviteit.Titel;
            TxtBeschrijving.Text = selectedActiviteit.Beschrijving;
            LblDatum.Content = selectedActiviteit.DatumTijd.ToLongDateString() + " " + selectedActiviteit.DatumTijd.ToShortTimeString();
            if (selectedActiviteit.Icoon != null)
            {
                ImgIcoon.Source = MainWindow.ByteToImage(selectedActiviteit.Icoon);
            }
            else
            {
                ImgIcoon.Source = MainWindow.ByteToImage(LoadImage("Assets/noIcon.png"));
            }

            try
            {
                LblOrganisator.Content = selectedActiviteit.Organisator.Voornaam;
                LblMaxDeelnemers.Content = selectedActiviteit.MaxPersonen;

                List<Persoon> deelnemers = Deelname.GetDeelnemersByActiviteitId(selectedActiviteit.Id);
                if (deelnemers.Count() >= selectedActiviteit.MaxPersonen)
                {
                    LblPlaatsenOver.Content = $"Activiteit volzet!";
                    BtnInschrijven.IsEnabled = false;
                    BtnInschrijven.Content = "Volzet!";
                }
                else
                {
                    LblPlaatsenOver.Content = $"Nog {selectedActiviteit.MaxPersonen - deelnemers.Count()} van {selectedActiviteit.MaxPersonen} plaatsen over";
                    BtnInschrijven.IsEnabled = true;

                    foreach (Persoon persoon in deelnemers)
                    {
                        if (persoon.Id == this.persoon.Id)
                        {
                            isIngeschreven = true;
                            BtnInschrijven.Content = "Uitschrijven";
                            break;
                        }
                    }

                    if (!isIngeschreven)
                    {
                        BtnInschrijven.Content = "Inschrijven";
                    }
                }
            }
            catch (Exception ex)
            {
                LblError.Content = $"Error: {ex.Message}";
                throw;
            }
        }

        private void ApplyFilters()
        {
            filteredActiviteiten.Clear();

            string zoekTerm = TxtZoeken.Text.ToLower();
            bool isHobby = ChkHobby.IsChecked == true;
            bool isCultuur = ChkCultuur.IsChecked == true;
            bool isSport = ChkSport.IsChecked == true;
            int leeftijdsgroep = -1;
            if (CboLeeftijdscategorie.SelectedItem != null && CboLeeftijdscategorie.SelectedIndex != 0)
            {
                leeftijdsgroep = (int)CboLeeftijdscategorie.SelectedItem;
            }

            foreach (Activiteit activiteit in activiteiten)
            {
                if (activiteit.Titel.ToLower().Contains(zoekTerm))
                {
                    if (!isHobby && !isCultuur && !isSport)
                    {
                        if (leeftijdsgroep == -1)
                        {
                            filteredActiviteiten.Add(activiteit);
                        }
                        else if (leeftijdsgroep == activiteit.Leeftijdsgroep)
                        {
                            filteredActiviteiten.Add(activiteit);
                        }
                    }
                    else
                    {
                        if (isHobby && activiteit.Soort == ActiviteitSoort.Hobby)
                        {
                            if (leeftijdsgroep == -1)
                            {
                                filteredActiviteiten.Add(activiteit);
                            }
                            else if (leeftijdsgroep == activiteit.Leeftijdsgroep)
                            {
                                filteredActiviteiten.Add(activiteit);
                            }
                        }
                        if (isCultuur && activiteit.Soort == ActiviteitSoort.Cultuur)
                        {
                            if (leeftijdsgroep == -1)
                            {
                                filteredActiviteiten.Add(activiteit);
                            }
                            else if (leeftijdsgroep == activiteit.Leeftijdsgroep)
                            {
                                filteredActiviteiten.Add(activiteit);
                            }
                        }
                        if (isSport && activiteit.Soort == ActiviteitSoort.Sport)
                        {
                            if (leeftijdsgroep == -1)
                            {
                                filteredActiviteiten.Add(activiteit);
                            }
                            else if (leeftijdsgroep == activiteit.Leeftijdsgroep)
                            {
                                filteredActiviteiten.Add(activiteit);
                            }
                        }
                    }
                }
            }

            MainMap.Markers.Clear();
            foreach (var activiteit in filteredActiviteiten)
            {
                AddMarker(activiteit);
            }
        }

        private void CboLeeftijdscategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ChkHobby_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void ChkCultuur_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void ChkSport_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void BtnInschrijven_Click(object sender, RoutedEventArgs e)
        {
            if (selectedActiviteit != null)
            {
                Deelname deelname = new Deelname()
                {
                    ActiviteitId = selectedActiviteit.Id,
                    PersoonId = persoon.Id,
                };
                if (isIngeschreven)
                {
                    deelname.DeleteFromDb();
                    BtnInschrijven.Content = "Inschrijven";
                }
                else
                {
                    deelname.InsertInDb();
                    BtnInschrijven.Content = "Uitschrijven";
                }
                isIngeschreven = !isIngeschreven;
                try
                {
                    List<Persoon> deelnemers = Deelname.GetDeelnemersByActiviteitId(selectedActiviteit.Id);
                    LblPlaatsenOver.Content = $"Nog {selectedActiviteit.MaxPersonen - deelnemers.Count()} van {selectedActiviteit.MaxPersonen} plaatsen over";
                }
                catch (Exception ex)
                {
                    LblError.Content = $"Error: {ex.Message}";
                    throw;
                }

                ApplyFilters();
            }
        }
    }
}
