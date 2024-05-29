using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLFitness.WpfCustomer
{

    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; } // Note: Passwords should be handled securely in real applications
        public byte[] ProfilePhoto { get; set; }
        public DateTime RegDate { get; set; }
        public bool IsAdmin { get; set; }
    }


}
