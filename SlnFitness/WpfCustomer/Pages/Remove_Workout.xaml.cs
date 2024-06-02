using CLFitness.WpfCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfCustomer.Pages
{
    /// <summary>
    /// Interaction logic for Remove_Workout.xaml
    /// </summary>
    public partial class Remove_Workout : Page
    {
        private Workout workoutToRemove;
        public Remove_Workout(Workout workout)
        {
            InitializeComponent();
            workoutToRemove = workout;

            WorkoutNumberText.Text = $"Workout Number: {workoutToRemove.Id}";
            WorkoutDateText.Text = $"Date: {workoutToRemove.Date.ToString("yyyy-MM-dd")}";
            ExerciseNameText.Text = $"Exercise Name: {workoutToRemove.Exercise.Name}";
            DistanceText.Text = $"Distance: {workoutToRemove.Distance} km";
            PointsText.Text = $"Points: {workoutToRemove.Exercise.Points}";
        }

        private void RemoveWorkoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (Workout.RemoveWorkout(workoutToRemove))
            {

                MessageBox.Show("Workout successfully removed!");

                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }

            }
            else
            {
                MessageBox.Show("An error occurred while removing the workout!");
            }
        }

    }
}
