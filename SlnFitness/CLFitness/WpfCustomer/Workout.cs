using CLFitness.WpfAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLFitness.WpfCustomer
{
    public class Workout
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public float? Distance { get; set; } // Nullable
        public int CustomerId { get; set; }
        public int ExerciseId { get; set; }

        // Navigation properties
        public Person Customer { get; set; }
        public Exercise Exercise { get; set; }
    }

}
