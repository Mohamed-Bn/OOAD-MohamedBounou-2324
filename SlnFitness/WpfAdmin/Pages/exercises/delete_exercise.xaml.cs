using CLFitness.WpfAdmin;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WpfAdmin.Pages.exercises
{
    public partial class delete_exercise : Page
    {
        // Eigenschap om de te verwijderen oefening op te slaan.
        public Exercise Exercise { get; set; }

        // Constructor die wordt aangeroepen wanneer een oefening wordt verwijderd.
        public delete_exercise(Exercise exercise)
        {
            InitializeComponent();
            DataContext = this;
            Exercise = exercise; 
            name.Text = exercise.Name;
        }

        public delete_exercise()
        {
            InitializeComponent();
            DataContext = this;
        }

        // Event handler voor de 'Annuleren' knop.
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack(); 
        }

        // Event handler voor de 'Verwijderen' knop.
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string deleteResult = Exercise.Delete();

            if (deleteResult == "true")
            {
                MessageBox.Show($"Exercise '{Exercise.Name}' deleted successfully.");
            }
            else
            {
                MessageBox.Show($"Failed to delete exercise: {deleteResult}");
            }

            // Navigeert terug naar het overzicht van oefeningen na het verwijderen.
            exercises_overview temp = new exercises_overview();
            NavigationService.Navigate(temp);
        }
    }

    // https://stackoverflow.com/questions/10262178/how-to-delete-a-button-run-time
    // https://stackoverflow.com/questions/1769951/c-sharp-cancelbutton-closes-dialog
}
