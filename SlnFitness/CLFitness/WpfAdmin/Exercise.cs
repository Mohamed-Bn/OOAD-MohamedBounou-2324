using CLFitness.Central;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLFitness.WpfAdmin
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ExerciseType Type { get; set; }
        public string Instruction { get; set; } // Nullable
        public string BodyPart { get; set; } // Nullable
        public string Pose { get; set; } // Nullable
        public string Nickname { get; set; } // Nullable
        public byte[] Photo { get; set; }
        public int Points { get; set; }
    }


}
