using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleKaartspel1
{
    internal class Deck
    {
        public List<Kaart> Kaarten { get; set; }

        public Deck()
        {
            Kaarten = new List<Kaart>();

            // ik vind het beter om alles appart te doen want dan is het net zoals een echte nieuwe deck kaarten
            for (int i = 1; i <= 13; i++)
            {
                Kaart kaartC = new Kaart(i, 'C');
                Kaarten.Add(kaartC);
            }
            for (int i = 1; i <= 13; i++)
            {
                Kaart kaartS = new Kaart(i, 'S');
                Kaarten.Add(kaartS);
            }
            for (int i = 1; i <= 13; i++)
            {
                Kaart kaartH = new Kaart(i, 'H');
                Kaarten.Add(kaartH);
            }
            for (int i = 1; i <= 13; i++)
            {
                Kaart kaartD = new Kaart(i, 'D');
                Kaarten.Add(kaartD);
            }
        }

        public void Schudden()
        {
            Random rnd = new Random();
            int lastCard = Kaarten.Count;
            while (lastCard > 1)
            {
                lastCard--;
                int randomCard = rnd.Next(lastCard + 1);
                Kaart oldCard = Kaarten[randomCard];
                Kaarten[randomCard] = Kaarten[lastCard];
                Kaarten[lastCard] = oldCard;
            }
        }

        public Kaart NeemKaart()
        {
            if (Kaarten.Count > 0)
            {
                Kaart card = Kaarten[0];
                Kaarten.RemoveAt(0);
                return card;
            }
            else
            {
                return null;
            }
        }
    }
}
