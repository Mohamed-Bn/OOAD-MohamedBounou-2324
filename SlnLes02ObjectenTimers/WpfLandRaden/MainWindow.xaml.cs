using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfLandRaden
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string[] landen = { "Brazilië", "Canada", "Finland", "Argentina", "India" };
        private int huidigLandIndex = 0;
        private DateTime startTijd;
        private int juisteAntwoorden = 0;
        private int kansen = 5; 

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            startText.Visibility = Visibility.Hidden;

            startTijd = DateTime.Now;
            ToonVolgendLand();
        }

        private void ToonVolgendLand()
        {
            if (huidigLandIndex < landen.Length && kansen > 0)
            {
                resultLabel.Content = landen[huidigLandIndex];
                huidigLandIndex++;
            }
            else
            {
                TimeSpan totaleTijd = DateTime.Now - startTijd;
                double gemiddeldeTijd = juisteAntwoorden > 0 ? totaleTijd.TotalSeconds / juisteAntwoorden : 0;
                resultLabel.Content = $"Je had er {juisteAntwoorden}/{landen.Length} juist. Je gemiddelde tijd is {gemiddeldeTijd:F2} seconden";
            }
        }

        private void Image_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Image geklikteAfbeelding && geklikteAfbeelding.Tag.ToString() == landen[huidigLandIndex - 1])
            {
                juisteAntwoorden++;
                resultLabel.Content = "Juist!";
                geklikteAfbeelding.Opacity = 0.5;
                ToonVolgendLand();
            }
            else
            {
                resultLabel.Content = "Fout!";
                kansen--; 
                if (kansen == 0)
                {
                    ToonVolgendLand();
                }
            }
        }
    }
}
