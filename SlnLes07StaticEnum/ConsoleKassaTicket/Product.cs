using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleKassaTicket
{
    internal class Product
    {
        private static string _rexCode = @"^P[0-9]{5}";

        private string _name;
        private decimal _uniteprice;
        private string _code;

        public string Naam
        { 
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Naam mag niet leeg zijn.");
                _name = value;
            }
        }
        public decimal Eenheidsprijs
        {
            get { return _uniteprice; }
            set
            {
                if (value < 0) throw new ArgumentException("Eenheidsprijs mag niet negatief zijn.");
                _uniteprice = value;
            }
        }
        public string Code
        {
            get { return _code; }
            set
            {
                if (!ValideerCode(value)) throw new ArgumentOutOfRangeException("Code moet uit 6 tekens bestaan en beginnen met ‘P’.");
                _code = value;
            }
        }

        public Product(string naam, decimal eenheidsprijs, string code)
        {
            if (string.IsNullOrEmpty(naam)) throw new ArgumentException("Naam mag niet leeg zijn.");
            if (eenheidsprijs < 0) throw new ArgumentException("Eenheidsprijs mag niet negatief zijn.");
            if (!ValideerCode(code)) throw new ArgumentOutOfRangeException("Code moet uit 6 tekens bestaan en beginnen met ‘P’.");

            Naam = naam;
            Eenheidsprijs = eenheidsprijs;
            Code = code;
        }

        public static bool ValideerCode(string code)
        {
            return Regex.Match(code, _rexCode).Success;
        }

        public override string ToString()
        {
            return $"({Code}) {Naam}: {Eenheidsprijs}";
        }
    }
}
