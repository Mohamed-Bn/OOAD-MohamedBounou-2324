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
using WpfAdmin.Pages.exercises;
using WpfAdmin.Pages.person;


namespace WpfAdmin.Pages.exercises
{
    /// <summary>
    /// Interaction logic for exercises_overview.xaml
    /// </summary>
    public partial class exercises_overview : Page
    {
        Button[] exercise;

        public exercises_overview()
        {
            InitializeComponent();
            exercise = new Button[4];
            add_exercise_name();
            AddRectangles();

        }


        private void add_exercise_name()
        {

            string[] exerciseNames = { "Cardio", "Dumbell", "Yoga" };

            for (int i = 0; i < exerciseNames.Length; i++)
            {
                exercise[i] = new Button();
                exercise[i].Content = exerciseNames[i];
                exercise[i].Margin = new Thickness(10);
                exercise_name_panel.Children.Add(exercise[i]);
            }
        }

        private void AddRectangles()
        {
            int numberOfRectangles = 7;
            int columnsPerRow = 3;
            int numRows = (int)Math.Ceiling((double)numberOfRectangles / columnsPerRow);

            // Clear any existing row and column definitions and children
            rectangleGrid.RowDefinitions.Clear();
            rectangleGrid.ColumnDefinitions.Clear();
            rectangleGrid.Children.Clear();

            // Create the necessary RowDefinitions
            for (int i = 0; i < numRows; i++)
            {
                rectangleGrid.RowDefinitions.Add(new RowDefinition());
            }

            // Create the necessary ColumnDefinitions
            for (int j = 0; j < columnsPerRow; j++)
            {
                rectangleGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < numberOfRectangles; i++)
            {
                // Create a Border to mimic a Rectangle with inner content
                Border border = new Border
                {
                    Width = 194,
                    Height = 168,
                    Margin = new Thickness(10),
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    Background = Brushes.Beige,
                    CornerRadius = new CornerRadius(5)
                };

                // Create the inner Grid for the content
                Grid contentGrid = new Grid();
                contentGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                contentGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                contentGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                contentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                contentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                contentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                // Add Image
                Image image = new Image
                {
                    Source = new BitmapImage(new Uri("exercise.PNG", UriKind.Relative)), // Change to your image path
                    Width = 50,
                    Height = 50,
                    Margin = new Thickness(5)
                };
                Grid.SetRow(image, 0);
                Grid.SetRowSpan(image, 3);
                Grid.SetColumn(image, 0);
                contentGrid.Children.Add(image);

                // Add Exercise Name
                TextBlock exerciseName = new TextBlock
                {
                    Text = "Exercise Name",
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                    Margin = new Thickness(5, 0, 0, 0)
                };
                Grid.SetRow(exerciseName, 0);
                Grid.SetColumn(exerciseName, 1);
                contentGrid.Children.Add(exerciseName);

                // Add Points
                TextBlock points = new TextBlock
                {
                    Text = "10 points",
                    FontSize = 14,
                    Margin = new Thickness(5, 5, 0, 0)
                };
                Grid.SetRow(points, 1);
                Grid.SetColumn(points, 1);
                contentGrid.Children.Add(points);

                // Add Description
                TextBlock description = new TextBlock
                {
                    Text = "The sumo squat is a variation of the traditional squat that focuses on a wider stance and different toe positioning.",
                    FontSize = 12,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(5, 5, 0, 0)
                };
                Grid.SetRow(description, 2);
                Grid.SetColumn(description, 1);
                contentGrid.Children.Add(description);

                // Add Icon Buttons
                StackPanel iconsPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(0, 5, 5, 5)
                };

                Button editButton = new Button { Content = "✏️", Width = 30, Height = 30, Margin = new Thickness(2) };
                editButton.Click += load_edit_page;

                Button deleteButton = new Button { Content = "🗑️", Width = 30, Height = 30, Margin = new Thickness(2) };
              
                Button otherButton = new Button { Content = "📄", Width = 30, Height = 30, Margin = new Thickness(2) };
                otherButton.Click += new_edit_page;

                iconsPanel.Children.Add(editButton);
                iconsPanel.Children.Add(deleteButton);
                iconsPanel.Children.Add(otherButton);

                Grid.SetRow(iconsPanel, 0);
                Grid.SetRowSpan(iconsPanel, 3);
                Grid.SetColumn(iconsPanel, 2);
                contentGrid.Children.Add(iconsPanel);

                border.Child = contentGrid;

                int row = i / columnsPerRow;
                int column = i % columnsPerRow;

                rectangleGrid.Children.Add(border);
                Grid.SetRow(border, row);
                Grid.SetColumn(border, column);
            }
        }


        

        private void load_edit_page(object sender, EventArgs e)
        {
            edit_exercise temp_edit_exercise = new edit_exercise();
            temp_edit_exercise.ShowsNavigationUI = true;
            
        }

        private void new_edit_page(object sender, EventArgs e)
        {
            add_exercise temp=new add_exercise();
            temp.ShowsNavigationUI = true;
            this.Content = temp;

        }

    }

}
