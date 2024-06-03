using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLFitness.Connection_data
{
    public static class Connection
    {
        private static readonly string connString = "Data Source=DESKTOP-0G52L1T\\SQLEXPRESS;Initial Catalog=FitnessDB;Integrated Security=True;";

        public static string GetConnectionString()
        {
            return connString;
        }
    }

}
