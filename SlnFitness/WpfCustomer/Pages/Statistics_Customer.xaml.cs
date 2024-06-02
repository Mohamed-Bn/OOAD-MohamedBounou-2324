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

        public Statistics_Customer(Person loggedInVal)
        {
            InitializeComponent();
            loggedInPerson = loggedInVal;
            workouts = Workout.GetPersonWorkout(loggedInPerson.Id);
        }

        private void ShowStatisticsButton_Click(object sender, RoutedEventArgs e)
        {
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

        private DateTime StartOfWeek(DateTime date, DayOfWeek startOfWeek)
        {
            int diff = (7 + (date.DayOfWeek - startOfWeek)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

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

        private void DisplayStatistics(List<Workout> workouts, Dictionary<DateTime, int> weeklyPoints)
        {
            int totalPoints = workouts.Sum(w => w.Exercise.Points);
            double averagePointsPerWeek = weeklyPoints.Values.Average();
            int longestStreak = CalculateLongestStreak(workouts);

            StatisticsTextBlock.Text = $"Gefeliciteerd! Je behaalde al {totalPoints} punten\n" +
                                        $"Gemiddeld per week: {averagePointsPerWeek:F2}\n" +
                                        $"Langste streak: {longestStreak} weken\n";
        }

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
}
