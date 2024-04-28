using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleKassaTicket
{
    public enum Betaalwijze
    {
        Visa,
        Cash,
        Bancontact
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Product bananen = new Product("bananen", 1.75m, "P02384");
            Product brood = new Product("brood", 2.10m, "P01820");
            Product kaas = new Product("kaas", 3.99m, "P45612");
            Product kofie = new Product("kofie", 4.10m, "P98754");
            Ticket ticket1 = new Ticket("Annie", Betaalwijze.Visa);

            ticket1.Products.Add(bananen);
            ticket1.Products.Add(brood);
            ticket1.Products.Add(kaas);
            ticket1.Products.Add(kofie);

            ticket1.DrukTicket();
            Console.ReadKey();
        }
    }
}