using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using CLActiBuddy;

namespace WpfAdmin.ActiviteitPage
{
    /// <summary>
    /// Interaction logic for ActiviteitenOverzichtPage.xaml
    /// </summary>
    public partial class ActiviteitenOverzichtPage : Page
    {
        List<Activiteit> allActiviteiten = new ();
        List<Activiteit> filteredActiviteiten = new ();
        private bool alreadyFiltered = false;
        public ActiviteitenOverzichtPage()
        {
            InitializeComponent();
            allActiviteiten = Activiteit.GetAllActiviteiten();

            foreach (Activiteit activiteit in allActiviteiten)
            {
                AddBorder(activiteit);
            }
        }

        // zie link in ChatGPT dumps 
        private void AddBorder(Activiteit activiteit)
        {
            // Create Border
            Border border = new ()
            {
                Background = Brushes.PaleGoldenrod,
                Width = 250,
                Height = 125,
                BorderThickness = new Thickness(2),
                BorderBrush = Brushes.Black,
                Margin = new Thickness(5),
            };

            // Create Grid
            Grid grid = new ();
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition());

            // Create Inner Grid
            Grid innerGrid = new ();
            innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.2, GridUnitType.Star) });
            innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Create Image
            Image imgActiviteitIcon = new ();
            if (activiteit.Icoon != null)
            {
                imgActiviteitIcon.Source = MainWindow.ByteToImage(activiteit.Icoon);
            }
            innerGrid.Children.Add(imgActiviteitIcon);

            // Create StackPanel for Labels
            StackPanel stackPanel1 = new ();
            Grid.SetColumn(stackPanel1, 1);
            try
            {
                Label lblDatum = new () { Name = "LblDatum", Padding = new Thickness(0), Content = activiteit.DatumTijd.ToLongDateString() };
                Label lblOrganiser = new () { Name = "LblOrganiser", Padding = new Thickness(0), Content = $"georganiseerd door {activiteit.Organisator.Voornaam}" };
                Label lblDeelnemers = new () { Name = "LblDeelnemers", Padding = new Thickness(0), Content = $"{activiteit.Deelnemers.Count}/{activiteit.MaxPersonen}" };
                stackPanel1.Children.Add(lblDatum);
                stackPanel1.Children.Add(lblOrganiser);
                stackPanel1.Children.Add(lblDeelnemers);
                innerGrid.Children.Add(stackPanel1);
            }
            catch (Exception ex)
            {
                LblError.Content = $"Error: {ex.Message}";
                throw;
            }

            // Create StackPanel for Buttons
            StackPanel stackPanel2 = new ();
            Grid.SetColumn(stackPanel2, 2);
            Button btnDetails = new ()
            { 
                Content = "📑", 
                Width = 25, 
                Margin = new Thickness(0, 0, 0, 5), 
                Background = Brushes.Transparent, 
                BorderBrush = Brushes.Black, 
                BorderThickness = new Thickness(1),
                Tag = activiteit
            };
            Button btnDelete = new ()
            { 
                Content = "🗑️", 
                Width = 25, 
                Background = Brushes.Transparent, 
                BorderBrush = Brushes.Black, 
                BorderThickness = new Thickness(1),
                Tag = activiteit
            };

            btnDetails.Click += BtnDetails_Click;
            btnDelete.Click += BtnDelete_Click;

            stackPanel2.Children.Add(btnDetails);
            stackPanel2.Children.Add(btnDelete);
            innerGrid.Children.Add(stackPanel2);

            // Add Inner Grid to Main Grid
            grid.Children.Add(innerGrid);

            // Create and Add Title Label
            Label lblTitel = new () { Name = "LblTitel", Content = activiteit.Titel };
            Grid.SetRow(lblTitel, 1);
            grid.Children.Add(lblTitel);

            // Create and Add Description TextBlock
            TextBlock txtBeschrijving = new () { Name = "TxtBeschrijving", Text = activiteit.Beschrijving, TextWrapping = TextWrapping.Wrap, IsEnabled = false };
            Grid.SetRow(txtBeschrijving, 2);
            grid.Children.Add(txtBeschrijving);

            // Add Grid to Border
            border.Child = grid;

            // Add Border to WrapActiviteiten
            WrapActiviteiten.Children.Add(border);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ActiviteitVerwijderenPage((Activiteit)((Button)sender).Tag));
        }

        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ActiviteitDetailPage((Activiteit)((Button)sender).Tag));
        }

        private void ApplyFilters()
        {
            filteredActiviteiten.Clear();

            string zoekTerm = TxtZoeken.Text.ToLower();
            bool isHobby = ChkHobby.IsChecked == true;
            bool isCultuur = ChkCultuur.IsChecked == true;
            bool isSport = ChkSport.IsChecked == true;
            DateTime? filterDatum = DteDatumFilter.SelectedDate;

            foreach (Activiteit activiteit in allActiviteiten)
            {
                if (activiteit.Titel.ToLower().Contains(zoekTerm))
                {
                    if (!isHobby && !isCultuur && !isSport)
                    {
                        if (filterDatum == null)
                        {
                            filteredActiviteiten.Add(activiteit);
                        }
                        else if (filterDatum.Value.Date == activiteit.DatumTijd.Date)
                        {
                            filteredActiviteiten.Add(activiteit);
                        }
                    }
                    else
                    {
                        if (isHobby && activiteit.Soort == ActiviteitSoort.Hobby)
                        {
                            if (filterDatum == null)
                            {
                                filteredActiviteiten.Add(activiteit);
                            }
                            else if (filterDatum.Value.Date == activiteit.DatumTijd.Date)
                            {
                                filteredActiviteiten.Add(activiteit);
                            }
                        }
                        if (isCultuur && activiteit.Soort == ActiviteitSoort.Cultuur)
                        {
                            if (filterDatum == null)
                            {
                                filteredActiviteiten.Add(activiteit);
                            }
                            else if (filterDatum.Value.Date == activiteit.DatumTijd.Date)
                            {
                                filteredActiviteiten.Add(activiteit);
                            }
                        }
                        if (isSport && activiteit.Soort == ActiviteitSoort.Sport)
                        {
                            if (filterDatum == null)
                            {
                                filteredActiviteiten.Add(activiteit);
                            }
                            else if (filterDatum.Value.Date == activiteit.DatumTijd.Date)
                            {
                                filteredActiviteiten.Add(activiteit);
                            }
                        }
                    }
                }                
            }

            WrapActiviteiten.Children.Clear();
            foreach (var activiteit in filteredActiviteiten)
            {
                AddBorder(activiteit);
            }

            alreadyFiltered = true;
        }

        private void TxtZoeken_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void DteDatumFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void BtnWisFilters_Click(object sender, RoutedEventArgs e)
        {
            TxtZoeken.Text = "";
            ChkHobby.IsChecked = false;
            ChkCultuur.IsChecked = false;
            ChkSport.IsChecked = false;
            DteDatumFilter.SelectedDate = null;

            // dit om te voorkomen dat we dubbel filteren. want de bovenstaande aanpassing vuren al de changed events af, behalve de checkboxes. waardoor er al gefilterd wordt.
            // Bij checkboxes werkte dit niet, omdat de checkbox een 'click' event heeft. Dus als je enkel Checkbox aanduid en wis filter, dan gebeurde er niets.
            // vandaar nog eens ApplyFilters() oproepen voor de zekerheid. (maar niet zomaar, als er al gefilterd is dan wordt het hier niet nog eens gefilterd)
            if (!alreadyFiltered) 
            {
                ApplyFilters();
            }
            else
            {
                alreadyFiltered = false;
            }
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
    }
}
