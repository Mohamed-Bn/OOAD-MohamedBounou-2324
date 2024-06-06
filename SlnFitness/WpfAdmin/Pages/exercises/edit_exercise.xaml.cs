using System;
using System.Diagnostics.Eventing.Reader;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CLFitness.WpfAdmin; 

using Microsoft.Win32;
using System.IO;
using System.Data.SqlClient;
using WpfAdmin.Pages.person;

namespace WpfAdmin.Pages.exercises
{
    public partial class edit_exercise : Page
    {
        private Exercise currentExercise;

        // Constructor die de huidige oefening initialiseert en de UI-elementen invult.
        public edit_exercise(Exercise exercise)
        {
            InitializeComponent();
            currentExercise = exercise;

            // Vult de UI-elementen met de gegevens van de huidige oefening.
            exercise_name.Text = exercise.Name;
            points_box.Text = exercise.Points.ToString();
            description_box.Text = exercise.Description;

            if (exercise.Photo != null)
            {
                img.Source = ByteArrayToBitmapImage(exercise.Photo);
            }

            if (exercise is YogaExercise yoga)
            {
                pose_box.Text = yoga.Pose;
                setUI(3);
            }
            else if (exercise is DumbbellExercise dumbbell)
            {
                body_part.Text = dumbbell.BodyPart;
                setUI(2);
            }
            else
            {
                setUI(1);
            }

            type_exercise.Items.Clear();

            type_exercise.Items.Add("Cardio");
            type_exercise.Items.Add("Dumbbell");
            type_exercise.Items.Add("Yoga");


            if (currentExercise is DumbbellExercise dumbbell_)
            {
                instruction_box.Text = dumbbell_.Instruction;
            }
            else if (currentExercise is YogaExercise yoga_ )
            {
                instruction_box.Text = yoga_.Instruction;
                nickname_box.Text = yoga_.Nickname;
            }
            else
            {
                instruction_box.Text = string.Empty;
            }

            type_exercise.Items.Clear();
            type_exercise.Items.Add("Cardio");
            type_exercise.Items.Add("Dumbbell");
            type_exercise.Items.Add("Yoga");

            switch (currentExercise.Type)
            {
                case ExerciseType.Cardio:
                    type_exercise.SelectedItem = "Cardio";
                    break;
                case ExerciseType.Dumbbell:
                    type_exercise.SelectedItem = "Dumbbell";
                    break;
                case ExerciseType.Yoga:
                    type_exercise.SelectedItem = "Yoga";
                    break;
                default:
                    type_exercise.SelectedItem = null;
                    break;
            }

        }

        // Hulpfunctie om de UI aan te passen op basis van het type oefening.
        private void setUI(int type)
        {
            // Verbergt of toont UI-elementen afhankelijk van het type oefening.
            switch (type)
            {
                case 1:
                    instruction_box.Visibility = Visibility.Collapsed;
                    instruction_label.Visibility = Visibility.Collapsed;

                    body_part.Visibility = Visibility.Collapsed;
                    body_part_l.Visibility = Visibility.Collapsed;

                    pose_label.Visibility = Visibility.Collapsed;
                    pose_box.Visibility = Visibility.Collapsed;
                    nickname.Visibility = Visibility.Collapsed;
                    nickname_box.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    pose_label.Visibility = Visibility.Collapsed;
                    pose_box.Visibility = Visibility.Collapsed;
                    nickname.Visibility = Visibility.Collapsed;
                    nickname_box.Visibility = Visibility.Collapsed;
                    break;
                case 3: 
                    body_part.Visibility = Visibility.Collapsed;
                    body_part_l.Visibility = Visibility.Collapsed;
                    break;
                default:
                    break;
            }
        }

        // Event handler voor de 'Annuleren' knop.
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }


        // Event handler voor de 'Foto wijzigen' knop.
        private void ChangePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            // Stelt de gebruiker in staat om een nieuwe foto te kiezen en bij te werken.
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                Title = "Select an image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFileName = openFileDialog.FileName;
                img.Source = new BitmapImage(new Uri(selectedFileName));

                currentExercise.Photo = File.ReadAllBytes(selectedFileName);
            }
        }

        // Event handler voor de 'Opslaan' knop.
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Creëert een nieuw Exercise object en slaat de wijzigingen op.
            Exercise newExercise = null;
            string selectedType = type_exercise.SelectedItem?.ToString();
            ExerciseType exerciseType;

            if (selectedType != null)
            {
                switch (selectedType)
                {
                    case "Dumbbell":
                        newExercise = new DumbbellExercise
                        {
                            Instruction = instruction_box.Text,
                            BodyPart = body_part.Text
                        };
                        exerciseType = ExerciseType.Dumbbell;
                        break;
                    case "Yoga":
                        newExercise = new YogaExercise
                        {
                            Pose = pose_box.Text,
                            Nickname = nickname_box.Text,
                            Instruction = instruction_box.Text
                        };
                        exerciseType = ExerciseType.Yoga;
                        break;
                    case "Cardio":
                        newExercise = new CardioExercise();
                        exerciseType = ExerciseType.Cardio;
                        break;
                    default:
                        MessageBox.Show("Unknown exercise type selected.");
                        return;
                }

                if (newExercise != null)
                {
                    newExercise.Id = currentExercise.Id;
                    newExercise.Name = exercise_name.Text;
                    newExercise.Description = description_box.Text;
                    newExercise.Points = int.Parse(points_box.Text);
                    newExercise.Type = exerciseType;
                    newExercise.Photo = currentExercise.Photo;

                    if (int.TryParse(points_box.Text, out int points))
                    {
                        newExercise.Points = points;
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid integer for points.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    string mess = newExercise.Save();
                    if (mess == "true")
                    {
                        MessageBox.Show("Exercise updated");
                        exercises_overview temp = new exercises_overview();
                        NavigationService.Navigate(temp);
                    }
                    else
                    {
                        MessageBox.Show(mess);
                    }
                }
                else
                {
                    MessageBox.Show("Failed to create the new exercise instance.");
                }
            }
        }

        // Hulpfunctie om een byte array om te zetten naar een BitmapImage.
        private BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
        {
            // Converteert een byte array naar een BitmapImage voor weergave in de UI.
            using (var stream = new System.IO.MemoryStream(byteArray))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                return image;
            }
        }

        // Event handler voor tekstwijzigingen in de 'nickname' box.
        private void nickname_box_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }

    // https://stackoverflow.com/questions/75621952/c-sharp-code-for-moqs-setup-and-its-return-in-regards-to-mocking-a-dynamic-pro
    // https://stackoverflow.com/questions/1769951/c-sharp-cancelbutton-closes-dialog
    // https://stackoverflow.com/questions/9531270/change-button-image-after-clicking-it
    // https://www.codeproject.com/Questions/5301504/How-to-make-a-save-and-load-buttons-to-save-and-lo
}
