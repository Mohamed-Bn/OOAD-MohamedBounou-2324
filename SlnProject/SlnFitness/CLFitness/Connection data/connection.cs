using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLFitness.Connection_data
{
    public static class Connection
    {
        // Deze string wordt gebruikt om een verbinding met de SQL Server-database tot stand te brengen
        private static readonly string connString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=FitnessDB;Integrated Security=True\r\n";

        public static string GetConnectionString()
        {
            return connString;
        }
    }
    // https://youtu.be/b2ikv9KtAKY
}
