using CLFitness.WpfCustomer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLFitness.WpfAdmin
{
    public class DumbbellExercise : Exercise
    {
        // Eigenschappen voor de instructie en het lichaamsdeel dat getraind wordt.

        public string Instruction { get; set; }
        public string BodyPart { get; set; }

        // Een 'override' methode om de instructie op te halen.

        protected override string GetInstruction() => Instruction;

        // Een 'override' methode om het lichaamsdeel op te halen.

        protected override string GetBodyPart() => BodyPart;
    }
}
