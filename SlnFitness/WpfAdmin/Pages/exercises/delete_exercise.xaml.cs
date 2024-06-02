using CLFitness.WpfAdmin;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WpfAdmin.Pages.exercises
{
    public partial class delete_exercise : Page
    {
        public Exercise Exercise { get; set; }

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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack(); 
        }

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

            NavigationService.GoBack();
        }
    }
}
