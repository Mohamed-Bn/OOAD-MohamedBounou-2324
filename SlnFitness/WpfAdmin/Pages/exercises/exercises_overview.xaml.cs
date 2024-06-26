﻿using CLFitness.WpfAdmin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfAdmin.Pages.exercises
{
    public partial class exercises_overview : Page
    {
        private Button[] exerciseButtons;

        // Constructor die de componenten initialiseert en knoppen toevoegt.
        public exercises_overview()
        {
            InitializeComponent();
            exerciseButtons = new Button[4];
            AddExerciseTypeButtons();
        }

        // Methode om knoppen voor verschillende oefeningstypes toe te voegen.
        private void AddExerciseTypeButtons()
        {
            string[] exerciseTypes = { "Cardio", "Dumbbell", "Yoga" , "Add exercise" };

            for (int i = 0; i < exerciseTypes.Length; i++)
            {
                exerciseButtons[i] = new Button
                {
                    Content = exerciseTypes[i],
                    Margin = new Thickness(10),
                    Tag = exerciseTypes[i],
                    Padding = new Thickness(20, 10, 20, 10),
                    BorderThickness = new Thickness(1), 
                    BorderBrush = Brushes.Black 
                };

                if (i == 0) 
                    exerciseButtons[i].Margin = new Thickness(0, 10, 10, 10); 

                if (i == exerciseTypes.Length - 1) 
                    exerciseButtons[i].Margin = new Thickness(10, 10, 0, 10); 

                if (i == 3)
                {
                    exerciseButtons[i].Click += add_exercise;
                }
                else
                {
                    exerciseButtons[i].Click += ExerciseTypeButton_Click;
                }
                exercise_name_panel.Children.Add(exerciseButtons[i]);
            }
        }

        // Event handler voor het klikken op een oefeningstype knop.
        // chatgpt
        private void ExerciseTypeButton_Click(object sender, RoutedEventArgs e)
        {
            // Toont oefeningen op basis van het geselecteerde type.
            Button clickedButton = sender as Button;
            string exerciseType = clickedButton.Tag.ToString();

            if (exerciseType == "Cardio")
                DisplayExercises(1);
            else if (exerciseType == "Dumbbell")
                DisplayExercises(2);
            else if (exerciseType == "Yoga")
                DisplayExercises(3);
        }

        // Event handler voor het toevoegen van een nieuwe oefening
        private void add_exercise(object sender, RoutedEventArgs e)
        {
            // Navigeert naar de pagina om een nieuwe oefening toe te voegen.
            add_exercise temp = new add_exercise();
            NavigationService.Navigate(temp);
        }

        // Methode om oefeningen weer te geven op basis van het type.
        // chatgpt
        private void DisplayExercises(int type)
        {
            // Haalt oefeningen op en toont deze in een grid
            List<Exercise> exercises = Exercise.GetExercise(type);

            int numberOfRectangles = exercises.Count;
            int columnsPerRow = 3;
            int numRows = (int)Math.Ceiling((double)numberOfRectangles / columnsPerRow);

            exercisesGrid.RowDefinitions.Clear();
            exercisesGrid.ColumnDefinitions.Clear();
            exercisesGrid.Children.Clear();

            for (int i = 0; i < numRows; i++)
            {
                exercisesGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int j = 0; j < columnsPerRow; j++)
            {
                exercisesGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < numberOfRectangles; i++)
            {
                Exercise exercise = exercises[i];

                Border border = new Border
                {
                    Width = 200,
                    Height = 290, 
                    Margin = new Thickness(5),
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    Background = Brushes.Beige,
                    CornerRadius = new CornerRadius(5)
                };
                border.Tag = exercise;

                Grid contentGrid = new Grid();
                contentGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                contentGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                contentGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                contentGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                contentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                contentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

                Image image = new Image
                {
                    Source = ByteArrayToBitmapImage(exercise.Photo),
                    Width = 90,
                    Height = 100,
                    Margin = new Thickness(3, 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                Grid.SetRow(image, 0);
                Grid.SetColumn(image, 0);
                Grid.SetColumnSpan(image, 2); 
                contentGrid.Children.Add(image);

                TextBlock exerciseName = new TextBlock
                {
                    Text = exercise.Name,
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                    Margin = new Thickness(5, 3, 0, 0),
                    TextWrapping = TextWrapping.Wrap
                };
                Grid.SetRow(exerciseName, 1);
                Grid.SetColumn(exerciseName, 0);
                Grid.SetColumnSpan(exerciseName, 2); 
                contentGrid.Children.Add(exerciseName);

                TextBlock points = new TextBlock
                {
                    Text = $"{exercise.Points} points",
                    FontSize = 14,
                    Margin = new Thickness(5, 3, 0, 0),
                };
                Grid.SetRow(points, 2); 
                Grid.SetColumn(points, 0);
                Grid.SetColumnSpan(points, 2);
                contentGrid.Children.Add(points);

                TextBlock description = new TextBlock
                {
                    Text = exercise.Description,
                    FontSize = 12,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(5, 3, 0, 0)
                };
                Grid.SetRow(description, 3);
                Grid.SetColumn(description, 0);
                Grid.SetColumnSpan(description, 2); 
                contentGrid.Children.Add(description);

                StackPanel iconsPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(0, 5, 5, 0), 
                };

                Button editButton = new Button { Content = "✏️", Width = 30, Height = 30, Margin = new Thickness(2) };
                editButton.Click += load_edit_page;

                Button deleteButton = new Button { Content = "🗑️", Width = 30, Height = 30, Margin = new Thickness(2) };
                deleteButton.Click += delete_edit_page;
                Button otherButton = new Button { Content = "📄", Width = 30, Height = 30, Margin = new Thickness(2) };
                otherButton.Click += view_page;

                iconsPanel.Children.Add(editButton);
                iconsPanel.Children.Add(deleteButton);
                iconsPanel.Children.Add(otherButton);

                Grid.SetRow(iconsPanel, 0);
                Grid.SetColumn(iconsPanel, 1);
                contentGrid.Children.Add(iconsPanel);

                border.Child = contentGrid;

                int row = i / columnsPerRow;
                int column = i % columnsPerRow;

                exercisesGrid.Children.Add(border);
                Grid.SetRow(border, row);
                Grid.SetColumn(border, column);
            }
        }

        // Hulpfunctie om een byte array om te zetten naar een BitmapImage.
        public static BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                throw new ArgumentException("Byte array is empty or null");

            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        // Hulpfunctie om een bovenliggend element van een bepaald type te vinden.
        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
            {
                return null;
            }

            if (parentObject is T parent)
            {
                return parent;
            }
            else
            {
                return FindParent<T>(parentObject);
            }
        }

        // Event handler voor het verwijderen van een oefening
        private void delete_edit_page(object sender, RoutedEventArgs e)
        {
            // Navigeert naar de 'delete_exercise' pagina met de geselecteerde oefening
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                Border parentBorder = FindParent<Border>(clickedButton);
                if (parentBorder != null)
                {
                    if (parentBorder.Tag != null)
                    {
                        Exercise selectedExercise = parentBorder.Tag as Exercise;
                        if (selectedExercise != null)
                        {
                            delete_exercise temp = new delete_exercise(selectedExercise);
                            NavigationService.Navigate(temp);
                        }
                        else
                        {
                            MessageBox.Show("Selected exercise is null.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tag property of the parent border is null.");
                    }
                }
                else
                {
                    MessageBox.Show("Parent border not found.");
                }
            }
            else
            {
                MessageBox.Show("Clicked element is not a button.");
            }
        }

        // Event handler voor het laden van de bewerkingspagina van een oefening
        private void load_edit_page(object sender, RoutedEventArgs e)
        {
            // Navigeert naar de 'edit_exercise' pagina met de geselecteerde oefening.
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                Border parentBorder = FindParent<Border>(clickedButton);
                if (parentBorder != null)
                {
                    if (parentBorder.Tag != null)
                    {
                        Exercise selectedExercise = parentBorder.Tag as Exercise;
                        if (selectedExercise != null)
                        {
                            edit_exercise editPage = new edit_exercise(selectedExercise);
                            NavigationService.Navigate(editPage);
                        }
                        else
                        {
                            MessageBox.Show("Selected exercise is null.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tag property of the parent border is null.");
                    }
                }
                else
                {
                    MessageBox.Show("Parent border not found.");
                }
            }
            else
            {
                MessageBox.Show("Clicked element is not a button.");
            }
        }

        // Event handler voor het bekijken van een oefening.
        private void view_page(object sender, RoutedEventArgs e)
        {
            // Navigeert naar een pagina om de geselecteerde oefening te bekijken
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                Border parentBorder = FindParent<Border>(clickedButton);
                if (parentBorder != null)
                {
                    if (parentBorder.Tag != null)
                    {
                        Exercise selectedExercise = parentBorder.Tag as Exercise;
                        if (selectedExercise != null)
                        {
                            view temp = new view(selectedExercise);
                            NavigationService.Navigate(temp);
                        }
                        else
                        {
                            MessageBox.Show("Selected exercise is null.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tag property of the parent border is null.");
                    }
                }
                else
                {
                    MessageBox.Show("Parent border not found.");
                }
            }
            else
            {
                MessageBox.Show("Clicked element is not a button.");
            }
        }
    }

    // https://stackoverflow.com/questions/71694403/c-sharp-how-to-add-button-to-array-and-set-text-of-all-buttons
    // https://stackoverflow.com/questions/2096488/how-to-practically-use-events
}
