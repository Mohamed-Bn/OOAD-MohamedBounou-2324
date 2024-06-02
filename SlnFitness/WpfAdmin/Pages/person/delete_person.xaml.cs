using CLFitness.WpfCustomer;
using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfAdmin.Pages.person
{
    public partial class delete_person : Page
    {
        public Person Person { get; set; }

        public delete_person(Person person)
        {
            InitializeComponent();
            Person = person;
            DataContext = Person;
            name.Text=person.FirstName+" "+person.LastName;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
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
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show($"Failed to delete person: {deletePersonResult}");
            }
        }
    }
}
