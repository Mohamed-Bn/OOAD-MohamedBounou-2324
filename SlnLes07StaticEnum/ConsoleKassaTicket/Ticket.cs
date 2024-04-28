using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleKassaTicket
{
    internal class Ticket
    {
        private readonly decimal visakosten = 0.12m;

        public List<Product> Products { get; set; }
        public Betaalwijze PaidWith { get; set; } = Betaalwijze.Cash;
        public string Kassier { get; set; }
        public decimal Totalprice
        {
            get 
            {
                decimal totaal = 0;
                if (Products != null)
                {
                    foreach (Product product in Products)
                    {
                        totaal += product.Eenheidsprijs;
                    }
                    if (PaidWith == Betaalwijze.Visa) totaal += visakosten;
                }
                return totaal;
            }
        }

        public Ticket(string kassier, Betaalwijze betaaldMet)
        {
            if (string.IsNullOrEmpty(kassier)) throw new ArgumentException("Kassier mag niet leeg zijn.");
            Kassier = kassier;
            PaidWith = betaaldMet;
            Products = new List<Product>();
        }

        public void DrukTicket()
        {
            Console.WriteLine("KASSATICKET");
            Console.WriteLine("===========");
            Console.WriteLine($"Uw kassier: {Kassier}");
            Console.WriteLine();
            foreach (Product product in Products)
            {
                Console.WriteLine($"{product}");
            }
            Console.WriteLine("-----------");
            if (PaidWith == Betaalwijze.Visa)
            {
                Console.WriteLine($"Visa kosten: {visakosten}");
            }
            Console.WriteLine($"Totaal: {Totalprice}");
        }
    }
}
