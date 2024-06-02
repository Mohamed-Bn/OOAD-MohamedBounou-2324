using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Linq;
using CLFitness.WpfAdmin;
using CLFitness.WpfCustomer;
using System.IO;

namespace WpfCustomer.Pages
{
    public partial class Add_Workout : Page
    {
        private List<Exercise> exercises;
        private Person loggedInPerson;

        public Add_Workout(Person loggedInVal)
        {
            InitializeComponent();
            LoadExercises();
            loggedInPerson = loggedInVal;
        }

        private void LoadExercises()
        {
            exercises = Exercise.GetAllExercises();
            ExerciseComboBox.ItemsSource = exercises;
        }

        private void ExerciseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ExerciseComboBox.SelectedItem is Exercise selectedExercise)
            {
                DisplayExerciseDetails(selectedExercise);

                CardioDetailsPanel.Visibility = selectedExercise.TypeNum == 1 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void DisplayExerciseDetails(Exercise exercise)
        {
            ExerciseImage.Source = GetImage(exercise.Photo);
            ExerciseTypeText.Text = $"Type: {exercise.Type}";
            ExerciseDescriptionText.Text = $"Beschrijving: {exercise.Description}";
            ExerciseInstructionsText.Text = $"Instructies: {exercise.Instruction}";
            ExerciseBodyPartText.Text = $"Lichaamsdeel: {exercise.BodyPart}";
            ExercisePoseText.Text = $"Houding: {exercise.Pose}";
            ExerciseNicknameText.Text = $"Bijnaam: {exercise.Nickname}";
        }

        private BitmapImage GetImage(byte[] photo)
        {
            if (photo == null || photo.Length == 0) return null;

            BitmapImage bitmap = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(photo))
            {
                stream.Seek(0, SeekOrigin.Begin);
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
            }
            return bitmap;
        }

        private void AddWorkoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExerciseComboBox.SelectedItem is Exercise selectedExercise)
            {
                float? distance = null;
                if (selectedExercise.TypeNum == 1)
                {
                    if (float.TryParse(DistanceTextBox.Text, out float parsedDistance))
                    {
                        distance = parsedDistance;
                    }
                    else
                    {
                        MessageBox.Show("Voer een geldige afstand in.");
                        return;
                    }
                }

                Workout newWorkout = new Workout
                {
                    Date = DateTime.Today,
                    CustomerId = loggedInPerson.Id,
                    ExerciseId = selectedExercise.Id,
                    Distance = distance,
                };

                Workout.AddWorkout(newWorkout);
                MessageBox.Show("Workout succesvol toegevoegd!");
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Selecteer een oefening.");
            }
        }

        
    }
}
