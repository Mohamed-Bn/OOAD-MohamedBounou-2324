using CLFitness.WpfCustomer;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfCustomer.Pages
{
    public partial class Statistics_Customer : Page
    {
        private List<Workout> workouts;
        private Person loggedInPerson;

        // Constructor die de workouts laadt voor de ingelogde persoon.
        public Statistics_Customer(Person loggedInVal)
        {
            InitializeComponent();
            loggedInPerson = loggedInVal;
            workouts = Workout.GetPersonWorkout(loggedInPerson.Id);
        }

        // Event handler voor de 'Toon Statistieken' knop.
        private void ShowStatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            // Filtert de workouts op basis van de geselecteerde datums en toont de statistieken.
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;

            if (startDate == null || endDate == null)
            {
                MessageBox.Show("Selecteer een geldige periode.");
                return;
            }

            var filteredWorkouts = workouts
                .Where(w => w.Date >= startDate.Value && w.Date <= endDate.Value)
                .OrderBy(w => w.Date)
                .ToList();

            if (filteredWorkouts.Count == 0)
            {
                MessageBox.Show("Geen workouts gevonden in de geselecteerde periode.");
                return;
            }

            var weeklyPoints = CalculateWeeklyPoints(filteredWorkouts);
            DisplayChart(weeklyPoints);
            DisplayStatistics(filteredWorkouts, weeklyPoints);
        }

        // Hulpfunctie om de wekelijkse punten te berekenen.
        private Dictionary<DateTime, int> CalculateWeeklyPoints(List<Workout> workouts)
        {
            Dictionary<DateTime, int> weeklyPoints = new Dictionary<DateTime, int>();

            foreach (var workout in workouts)
            {
                var weekStart = StartOfWeek(workout.Date, DayOfWeek.Monday);
                if (weeklyPoints.ContainsKey(weekStart))
                {
                    weeklyPoints[weekStart] += workout.Exercise.Points;
                }
                else
                {
                    weeklyPoints[weekStart] = workout.Exercise.Points;
                }
            }

            return weeklyPoints;
        }

        // Hulpfunctie om het begin van de week te bepalen.
        private DateTime StartOfWeek(DateTime date, DayOfWeek startOfWeek)
        {
            int diff = (7 + (date.DayOfWeek - startOfWeek)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        // Hulpfunctie om de grafiek weer te geven.
        private void DisplayChart(Dictionary<DateTime, int> weeklyPoints)
        {
            var values = weeklyPoints.Values.Select(v => (double)v).ToArray();
            var labels = weeklyPoints.Keys.Select(d => d.ToString("dd-MM-yyyy")).ToArray();

            var seriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Punten",
                    Values = new ChartValues<double>(values)
                }
            };

            CustomerBarChart.Series = seriesCollection;
            CustomerBarChart.AxisX[0].Labels = labels;
        }

        // Hulpfunctie om de statistieken weer te geven
        private void DisplayStatistics(List<Workout> workouts, Dictionary<DateTime, int> weeklyPoints)
        {
            int totalPoints = workouts.Sum(w => w.Exercise.Points);
            double averagePointsPerWeek = weeklyPoints.Values.Average();
            int longestStreak = CalculateLongestStreak(workouts);

            StatisticsTextBlock.Text = $"Gefeliciteerd! Je behaalde al {totalPoints} punten\n" +
                                        $"Gemiddeld per week: {averagePointsPerWeek:F2}\n" +
                                        $"Langste streak: {longestStreak} weken\n";
        }

        // Hulpfunctie om de langste streak te berekenen
        private int CalculateLongestStreak(List<Workout> workouts)
        {
            var weeks = workouts
                .Select(w => StartOfWeek(w.Date, DayOfWeek.Monday))
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            int longestStreak = 0;
            int currentStreak = 1;

            for (int i = 1; i < weeks.Count; i++)
            {
                if (weeks[i] == weeks[i - 1].AddDays(7))
                {
                    currentStreak++;
                }
                else
                {
                    if (currentStreak > longestStreak)
                    {
                        longestStreak = currentStreak;
                    }
                    currentStreak = 1;
                }
            }

            return Math.Max(longestStreak, currentStreak);
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
