using System;
using System.Diagnostics.Eventing.Reader;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CLFitness.WpfAdmin; 

using Microsoft.Win32;
using System.IO;
using System.Data.SqlClient;

namespace WpfAdmin.Pages.exercises
{
    public partial class edit_exercise : Page
    {
        private Exercise currentExercise;

        public edit_exercise(Exercise exercise)
        {
            InitializeComponent();
            currentExercise = exercise;

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
            }
            else if (exercise is DumbbellExercise dumbbell)
            {
                body_part.Text = dumbbell.BodyPart;
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

        

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }


        private void ChangePhotoButton_Click(object sender, RoutedEventArgs e)
        {
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
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
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

                    string mess = newExercise.Save();
                    if (mess == "true")
                    {
                        MessageBox.Show("Exercise updated");
                        NavigationService.GoBack();
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





        private BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
        {
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

        private void nickname_box_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
