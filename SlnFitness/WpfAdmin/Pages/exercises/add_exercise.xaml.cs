using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using CLFitness.WpfAdmin;
using System.IO;

namespace WpfAdmin.Pages.exercises
{
    public partial class add_exercise : Page
    {
        public add_exercise()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            type_exercise.Items.Add("Cardio");
            type_exercise.Items.Add("Dumbbell");
            type_exercise.Items.Add("Yoga");

            ToggleFieldsVisibility(false, false, false);
        }

        private void TypeExercise_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedType = type_exercise.SelectedItem?.ToString();
            if (selectedType != null)
            {
                switch (selectedType)
                {
                    case "Cardio":
                        ToggleFieldsVisibility(false, false, false);
                        break;
                    case "Dumbbell":
                        ToggleFieldsVisibility(true, false, true);
                        break;
                    case "Yoga":
                        ToggleFieldsVisibility(true, true, true);
                        break;
                    default:
                        ToggleFieldsVisibility(false, false, false);
                        break;
                }
            }
        }

        private void ToggleFieldsVisibility(bool bodyPartVisible, bool poseVisible, bool instructionVisible)
        {
            body_part.Visibility = bodyPartVisible ? Visibility.Visible : Visibility.Collapsed;
            pose_box.Visibility = poseVisible ? Visibility.Visible : Visibility.Collapsed;
            instruction_box.Visibility = instructionVisible ? Visibility.Visible : Visibility.Collapsed;
            nickname_box.Visibility = poseVisible ? Visibility.Visible : Visibility.Collapsed;

            body_part.IsEnabled = bodyPartVisible;
            pose_box.IsEnabled = poseVisible;
            instruction_box.IsEnabled = instructionVisible;
            nickname_box.IsEnabled = poseVisible;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Exercise newExercise = null;
            string selectedType = type_exercise.SelectedItem?.ToString();

            if (selectedType != null)
            {
                ExerciseType exerciseType;
                if (Enum.TryParse(selectedType, out exerciseType))
                {
                    switch (exerciseType)
                    {
                        case ExerciseType.Dumbbell:
                            newExercise = new DumbbellExercise
                            {
                                Instruction = instruction_box.Text,
                                BodyPart = body_part.Text
                            };
                            break;
                        case ExerciseType.Yoga:
                            newExercise = new YogaExercise
                            {
                                Pose = pose_box.Text,
                                Nickname = nickname_box.Text,
                                Instruction = instruction_box.Text
                            };
                            break;
                        case ExerciseType.Cardio:
                            newExercise = new CardioExercise();
                            break;
                        default:
                            MessageBox.Show("Unknown exercise type selected.");
                            return;
                    }

                    if (newExercise != null)
                    {

                        if (exercise_name.Text == null)
                            MessageBox.Show("Name must require");
                        else if (description_box.Text == null)
                            MessageBox.Show("Description must require");
                        else if (points_box.Text == null)
                            MessageBox.Show("Points must require");

                        else if (exercise_name.Text == null)
                            MessageBox.Show("Name must require");


                        newExercise.Name = exercise_name.Text;
                        newExercise.Description = description_box.Text;
                        newExercise.Points = int.Parse(points_box.Text);
                        newExercise.Type = exerciseType;
                        byte[] imageData;
                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)img.Source));

                        using (MemoryStream ms = new MemoryStream())
                        {
                            encoder.Save(ms);
                            imageData = ms.ToArray();
                        }

                        newExercise.Photo = imageData;



                        if (newExercise.Photo== null)
                        {
                            MessageBox.Show("Photo must required");
                            return;
                        }
                        string mess = newExercise.SaveNew();
                        if (mess == "true")
                        {
                            MessageBox.Show("Exercise added");
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
                else
                {
                    MessageBox.Show("Failed to parse the selected exercise type.");
                }
            }
            else
            {
                MessageBox.Show("Please select exercise type");
            }
        }

        private void nickname_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void ChangePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                img.Source = new BitmapImage(new Uri(openFileDialog.FileName));

                MessageBox.Show("Photo changed successfully.");
            }
        }
    }
}
