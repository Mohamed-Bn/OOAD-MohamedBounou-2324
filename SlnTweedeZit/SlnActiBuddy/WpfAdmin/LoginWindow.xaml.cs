using System.Windows;
using CLActiBuddy;

namespace WpfAdmin
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            try
            {
                DataInitializerService.InitializeData();
            }
            catch (Exception ex)
            {
                LblErrorMessage.Content = $"Er ging iets mis bij het initialiseren van de data. Error: {ex.Message}";
                throw;
            }
        }

        private void BtnInloggen_Click(object sender, RoutedEventArgs e)
        {
            string gebruikersnaam = TxtGebruikersnaam.Text;
            string paswoord = TxtWachtwoord.Password;

            if (string.IsNullOrEmpty(gebruikersnaam))
            {
                LblErrorMessage.Content = "Vul gebruikersnaam in!";
                return;
            }
            if (string.IsNullOrEmpty(paswoord))
            {
                LblErrorMessage.Content = "Vul paswoord in!";
                return;
            }

            paswoord = PasswordHashService.QuickHash(paswoord);

            try
            {
                Persoon persoon = Persoon.GetByGebruikersnaamEnPaswoord(gebruikersnaam, paswoord);
                if (persoon == null || !persoon.IsAdmin)
                {
                    LblErrorMessage.Content = "Incorrecte login.";
                }
                else
                {
                    Application.Current.Properties.Add("persoon", persoon);

                    if (Application.Current.Properties.Contains("loginPagina"))
                    {
                        Application.Current.Properties.Remove("loginPagina");
                    }

                    MainWindow mainWindow = new ()
                    {
                        Title = $"WPF Admin - welkom {persoon.Voornaam} {persoon.Achternaam}"
                    };
                    mainWindow.Show();

                    Close();
                }
            }
            catch (Exception ex)
            {
                LblErrorMessage.Content = $"Error: {ex.Message}";
            }
        }
    }
}
