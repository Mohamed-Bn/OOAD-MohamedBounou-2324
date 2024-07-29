using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using CLFitness.WpfAdmin;

namespace WpfAdmin.Pages.exercises
{
    public partial class view : Page
    {
        private Exercise currentExercise;

        // Constructor die de huidige oefening initialiseert en de UI-elementen invult
        public view(Exercise exercise)
        {
            // Vult de UI-elementen met de gegevens van de huidige oefening.
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
            else if (currentExercise is YogaExercise yoga_)
            {
                instruction_box.Text = yoga_.Instruction;
                nickname_box.Text = yoga_.Nickname;
            }
            else
            {
                instruction_box.Text = string.Empty;
            }

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
            instruction_box.Visibility = Visibility.Collapsed;
            instruction_label.Visibility = Visibility.Collapsed;
            body_part.Visibility = Visibility.Collapsed;
            body_part_l.Visibility = Visibility.Collapsed;
            pose_label.Visibility = Visibility.Collapsed;
            pose_box.Visibility = Visibility.Collapsed;
            nickname.Visibility = Visibility.Collapsed;
            nickname_box.Visibility = Visibility.Collapsed;

            switch (type)
            {
                case 1: 
                    break;
                case 2: 
                    instruction_box.Visibility = Visibility.Visible;
                    instruction_label.Visibility = Visibility.Visible;
                    body_part.Visibility = Visibility.Visible;
                    body_part_l.Visibility = Visibility.Visible;
                    break;
                case 3:
                    instruction_box.Visibility = Visibility.Visible;
                    instruction_label.Visibility = Visibility.Visible;
                    pose_box.Visibility = Visibility.Visible;
                    pose_label.Visibility = Visibility.Visible;
                    nickname.Visibility = Visibility.Visible;
                    nickname_box.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        // Hulpfunctie om een byte array om te zetten naar een BitmapImage.
        private BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
        {
            // Converteert een byte array naar een BitmapImage voor weergave in de UI
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

        // Event handler voor de 'Terug' knop.
        private void move_back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }

    // https://stackoverflow.com/questions/70004518/c-sharp-back-button-on-visual-studio
}
