using CLFitness.WpfCustomer;
using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfAdmin.Pages.person
{
    public partial class delete_person : Page
    {
        // Eigenschap om de te verwijderen persoon op te slaan.
        public Person Person { get; set; }

        // Constructor die de huidige persoon initialiseert en de UI-elementen invult
        public delete_person(Person person)
        {
            InitializeComponent();
            Person = person;
            DataContext = Person;
            name.Text=person.FirstName+" "+person.LastName;
        }

        // Event handler voor de 'Annuleren' knop.
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        // Event handler voor de 'Verwijderen' knop
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Verwijdert de workouts van de persoon en vervolgens de persoon zelf
            string deleteWorkoutResult = Person.DeleteWorkoutsForPerson(Person.Id);
            if (deleteWorkoutResult != "true")
            {
                MessageBox.Show($"Failed to delete workouts for person: {deleteWorkoutResult}");
                return;
            }

            string deletePersonResult = Person.DeletePerson(Person);
            if (deletePersonResult == "true")
            {
                MessageBox.Show("Person deleted successfully.");
                persons_overview temp = new persons_overview();
                NavigationService.Navigate(temp);
            }
            else
            {
                MessageBox.Show($"Failed to delete person: {deletePersonResult}");
            }
        }
    }
    // https://stackoverflow.com/questions/75621952/c-sharp-code-for-moqs-setup-and-its-return-in-regards-to-mocking-a-dynamic-pro
    // https://stackoverflow.com/questions/1769951/c-sharp-cancelbutton-closes-dialog
    // https://stackoverflow.com/questions/9531270/change-button-image-after-clicking-it
    // https://www.codeproject.com/Questions/5301504/How-to-make-a-save-and-load-buttons-to-save-and-lo
}
