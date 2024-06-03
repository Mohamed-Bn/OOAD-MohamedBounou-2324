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
        public string Instruction { get; set; }
        public string BodyPart { get; set; }

        protected override string GetInstruction() => Instruction;
        protected override string GetBodyPart() => BodyPart;
    }
}
