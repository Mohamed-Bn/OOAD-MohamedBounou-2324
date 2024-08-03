using System.Windows.Controls;
using CLActiBuddy;

namespace WpfAdmin.PersoonPage
{
    /// <summary>
    /// Interaction logic for PersonenOverzichtPage.xaml
    /// </summary>
    public partial class PersonenOverzichtPage : Page
    {
        private List<Persoon> personen = new List<Persoon>();
        public PersonenOverzichtPage()
        {
            InitializeComponent();
            personen = Persoon.GetAllPersonen();

            LstPersonen.Items.Clear();
            foreach (Persoon persoon in personen)
            {
                LstPersonen.Items.Add(persoon);
            }
        }

        private void LstPersonen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstPersonen.SelectedItem == null)
            {
                LblLogin.Text = "";
                LblRegDatum.Text = "";
                LblIsAdmin.Text = "";
                ImgPersoonFoto.Source = null;
            }
            else
            {
                Persoon person = (Persoon)LstPersonen.SelectedItem;
                LblLogin.Text = person.Login;
                LblRegDatum.Text = person.RegDatum.ToLongDateString();
                LblIsAdmin.Text = person.IsAdmin ? "Ja" : "Nee";
                ImgPersoonFoto.Source = MainWindow.ByteToImage(person.Profielfoto);
            }
        }

        private void NieuwButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new PersoonAanmakenPage());
        }

        private void VerwijderenButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (LstPersonen.SelectedItem == null)
            {
                LblError.Content = "Selecteer een persoon om te verwijderen.";
            }
            else
            {
                Persoon persoon = (Persoon)LstPersonen.SelectedItem;
                NavigationService.Navigate(new PersoonVerwijderenPage(persoon));
            }
        }

        private void BewerkenButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (LstPersonen.SelectedItem == null)
            {
                LblError.Content = "Selecteer een persoon om te bewerken.";
            }
            else
            {
                Persoon persoon = (Persoon)LstPersonen.SelectedItem;
                NavigationService.Navigate(new PersoonBewerkenPage(persoon));
            }
        }
    }
}
