using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleKaartspel1
{
    internal class Speler
    {
        public string Naam { get; set; }
        public List<Kaart> Kaarten { get; set; }
        public bool HeeftNogKaarten
        {
            get
            {
                bool result = false;
                if (Kaarten.Count > 0)
                {
                    result = true;
                }
                return result;
            }
        }

        public Speler(string naam)
        {
            Naam = naam;
            Kaarten = new List<Kaart>();
        }

        public Speler(string naam, List<Kaart> kaarten)
            : this(naam)
        {
            Kaarten.AddRange(kaarten);
        }

        public Kaart LegKaart()
        {
            if (Kaarten.Count > 0)
            {
                int rnd = new Random().Next(0, Kaarten.Count);
                Kaart card = Kaarten[rnd];
                Kaarten.RemoveAt(rnd);
                return card;
            }
            else
            {
                return null;
            }
        }
    }
}
