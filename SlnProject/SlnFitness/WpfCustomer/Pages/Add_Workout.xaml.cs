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

        // Constructor die de oefeningen laadt en de ingelogde persoon initialiseert.

        public Add_Workout(Person loggedInVal)
        {
            InitializeComponent();
            LoadExercises();
            loggedInPerson = loggedInVal;
        }

        // Methode om alle oefeningen te laden en weer te geven in de ComboBox.
        private void LoadExercises()
        {
            exercises = Exercise.GetAllExercises();
            ExerciseComboBox.ItemsSource = exercises;
        }

        // Event handler die wordt aangeroepen wanneer een oefening wordt geselecteerd.
        private void ExerciseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Toont details van de geselecteerde oefening en past de zichtbaarheid van de cardio details aan.
            if (ExerciseComboBox.SelectedItem is Exercise selectedExercise)
            {
                DisplayExerciseDetails(selectedExercise);

                CardioDetailsPanel.Visibility = selectedExercise.TypeNum == 1 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        // Hulpfunctie om de details van een oefening weer te geven.
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

        // Hulpfunctie om een byte array om te zetten naar een BitmapImage.
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

        // Event handler voor de 'Workout Toevoegen' knop.
        private void AddWorkoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Voegt een nieuwe workout toe met de geselecteerde oefening en afstand.
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

    // https://stackoverflow.com/questions/75621952/c-sharp-code-for-moqs-setup-and-its-return-in-regards-to-mocking-a-dynamic-pro
    // https://stackoverflow.com/questions/1769951/c-sharp-cancelbutton-closes-dialog
    // https://stackoverflow.com/questions/9531270/change-button-image-after-clicking-it
    // https://www.codeproject.com/Questions/5301504/How-to-make-a-save-and-load-buttons-to-save-and-lo
    // https://stackoverflow.com/questions/13082007/how-should-i-clear-fields-in-generic-static-class
    // chatgpt
}
