using System;
using System.Windows;

namespace WpfComplexiteit
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BerekenComplexiteit_Click(object sender, RoutedEventArgs e)
        {
            string woord = woordTextBox.Text;

            if (string.IsNullOrEmpty(woord))
            {
                resultaatTextBlock.Text = "Voer eerst een woord in.";
                return;
            }

            int aantalKarakters = woord.Length;
            int aantalLettergrepen = AantalLettergrepen(woord);
            double complexiteit = Complexiteit(woord);

            resultaatTextBlock.Text = $@"Aantal karakters: {aantalKarakters}
Aantal lettergrepen: {aantalLettergrepen}
Complexiteit: {complexiteit:F1}";
        }

        private bool IsKlinker(char karakter)
        {
            char[] klinkers = { 'a', 'e', 'i', 'o', 'u' };
            return Array.IndexOf(klinkers, char.ToLower(karakter)) != -1;
        }

        private int AantalLettergrepen(string woord)
        {
            int aantalLettergrepen = 0;

            for (int i = 0; i < woord.Length; i++)
            {
                if (IsKlinker(woord[i]) && (i == 0 || !IsKlinker(woord[i - 1])))
                {
                    aantalLettergrepen++;
                }
            }

            return aantalLettergrepen;
        }

        private double Complexiteit(string woord)
        {
            int aantalLetters = woord.Length;
            int aantalKlinkers = 0;
            int aantalMedeklinkers = 0;

            foreach (char karakter in woord)
            {
                if (IsKlinker(karakter))
                {
                    aantalKlinkers++;
                }
                else
                {
                    aantalMedeklinkers++;
                }
            }

            int aantalLettergrepen = AantalLettergrepen(woord);

            double complexiteit = aantalLetters / 3.0 + aantalLettergrepen;

            if (woord.Contains("x") || woord.Contains("y") || woord.Contains("q"))
            {
                complexiteit += 1;
            }

            return complexiteit;
        }
    }
}
