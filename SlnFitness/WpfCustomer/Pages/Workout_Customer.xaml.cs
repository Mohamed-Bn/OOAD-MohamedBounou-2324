using CLFitness.WpfCustomer;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfCustomer.Pages
{
    public partial class Workout_Customer : Page
    {
        private Person loggedInPerson;
        private List<Workout> workouts;
        private List<Workout> filterWorkouts;

        // Constructor die de workouts laadt voor de ingelogde persoon.
        public Workout_Customer(Person loggedInVal)
        {
            InitializeComponent();
            loggedInPerson = loggedInVal;
            workouts = Workout.GetPersonWorkout(loggedInPerson.Id);
            DisplayWorkouts(DateTime.Today);
        }

        // Event handler die wordt aangeroepen wanneer een datum wordt geselecteerd
        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            // Toont de workouts voor de geselecteerde datum
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is DateTime selectedDate)
            {
                DisplayWorkouts(selectedDate);
                date.Text = selectedDate.ToString("yyyy-MM-dd");
            }
        }

        // Methode om de workouts weer te geven voor een specifieke datum.
        private void DisplayWorkouts(DateTime selectedDate)
        {
            double topMargin = 268;
            double leftMargin = 10;
            double rightMargin = 385;
            double bottomMargin = 93;

            MainGrid.Children.Clear();

            filterWorkouts = Workout.GetPersonWorkoutsByDate(loggedInPerson.Id, selectedDate);

            foreach (var workout in filterWorkouts)
            {
                Canvas workoutCanvas = CreateWorkoutCanvas(workout);
                workoutCanvas.Margin = new Thickness(leftMargin, topMargin, rightMargin, bottomMargin);
                MainGrid.Children.Add(workoutCanvas);

                topMargin += 110;
            }
        }

        // Hulpfunctie om een byte array om te zetten naar een BitmapImage
        private BitmapImage GetImage(byte[] profilePhoto)
        {
            if (profilePhoto == null || profilePhoto.Length == 0)
            {
                return null;
            }

            BitmapImage bitmap = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(profilePhoto))
            {
                stream.Seek(0, SeekOrigin.Begin);
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
            }

            return bitmap;
        }

        // Hulpfunctie om een Canvas te creëren voor elke workout.
        private Canvas CreateWorkoutCanvas(Workout workout)
        {
            Canvas canvas = new Canvas
            {
                Background = new SolidColorBrush(Colors.LavenderBlush)
            };

            Image exerciseImage = new Image
            {
                Height = 70,
                Width = 68,
                Source = GetImage(workout.Exercise.Photo)
            };
            Canvas.SetLeft(exerciseImage, 10);
            Canvas.SetTop(exerciseImage, 10);
            canvas.Children.Add(exerciseImage);

            Label exerciseLabel = new Label
            {
                Content = workout.Exercise.Name,
                FontSize = 20,
                Width = 200
            };
            Canvas.SetLeft(exerciseLabel, 86);
            canvas.Children.Add(exerciseLabel);

            Label distanceLabel = new Label
            {
                Content = $"{workout.Distance} km",
            };
            Canvas.SetLeft(distanceLabel, 86);
            Canvas.SetTop(distanceLabel, 31);
            canvas.Children.Add(distanceLabel);

            Label pointsLabel = new Label
            {
                Content = $"{workout.Exercise.Points} punten",
            };
            Canvas.SetLeft(pointsLabel, 86);
            Canvas.SetTop(pointsLabel, 53);
            canvas.Children.Add(pointsLabel);

            Image deleteImage = new Image
            {
                Height = 36,
                Width = 30,
                Source = new BitmapImage(new Uri("/Pages/trash.png", UriKind.Relative))
            };
            deleteImage.MouseLeftButtonDown += (s, e) => OnDeleteImageClick(workout);
            Canvas.SetLeft(deleteImage, 349);
            Canvas.SetTop(deleteImage, 43);
            canvas.Children.Add(deleteImage);

            return canvas;
        }

        // Event handler die wordt aangeroepen wanneer op de verwijderafbeelding wordt geklikt
        private void OnDeleteImageClick(Workout workout)
        {
            // Navigeert naar de pagina om de workout te verwijderen
            Remove_Workout removeWorkoutPage = new Remove_Workout(workout);
            NavigationService.Navigate(removeWorkoutPage);
        }

        // Event handler voor de 'Toevoegen' knop
        private void OnAddButtonClicked(object sender, RoutedEventArgs e)
        {
            // Navigeert naar de pagina om een nieuwe workout toe te voegen
            Add_Workout addWorkoutPage = new Add_Workout(loggedInPerson);
            NavigationService.Navigate(addWorkoutPage);
        }
    }

    // https://stackoverflow.com/questions/75621952/c-sharp-code-for-moqs-setup-and-its-return-in-regards-to-mocking-a-dynamic-pro
    // https://stackoverflow.com/questions/1769951/c-sharp-cancelbutton-closes-dialog
    // https://stackoverflow.com/questions/9531270/change-button-image-after-clicking-it
    // https://www.codeproject.com/Questions/5301504/How-to-make-a-save-and-load-buttons-to-save-and-lo
    // https://stackoverflow.com/questions/13082007/how-should-i-clear-fields-in-generic-static-class
    // https://stackoverflow.com/questions/662379/calculate-date-from-week-number
    // https://stackoverflow.com/questions/10622674/chart-creating-dynamically-in-net-c-sharp
    // https://stackoverflow.com/questions/12912873/how-can-i-use-linq-to-calculate-the-longest-streak
    // chatgpt
}
