using System;

namespace ConsoleComplexiteit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            while (true)
            {
                Console.Write("Geef een woord (enter om te stoppen): ");
                string woord = Console.ReadLine();

                if (string.IsNullOrEmpty(woord))
                {
                    Console.WriteLine(' ');
                    Console.Write("Bedankt en tot ziens.");
                    break;
                }

                int aantalKarakters = woord.Length;
                int aantalLettergrepen = AantalLettergrepen(woord);
                double complexiteit = Complexiteit(woord);

                Console.WriteLine($"aantal karakters: {aantalKarakters}");
                Console.WriteLine($"aantal lettergrepen: {aantalLettergrepen}");
                Console.WriteLine($"complexiteit: {complexiteit:F1}\n");
            }
            Console.ReadLine();
        }

        static bool IsKlinker(char karakter)
        {
            char[] klinkers = { 'a', 'e', 'i', 'o', 'u' };
            return Array.IndexOf(klinkers, char.ToLower(karakter)) != -1;
        }

        static int AantalLettergrepen(string woord)
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

        static double Complexiteit(string woord)
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

            // Condition spécifique pour certains mots
            if (woord.Contains("x") || woord.Contains("y") || woord.Contains("q"))
            {
                complexiteit += 1;
            }

            return complexiteit;
        }
    }
}
