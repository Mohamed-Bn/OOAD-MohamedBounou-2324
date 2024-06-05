using CLFitness.WpfCustomer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLFitness.WpfAdmin
{
    public class YogaExercise : Exercise
    {
        // Eigenschappen specifiek voor yoga-oefeningen.

        public string Instruction { get; set; }
        public string Pose { get; set; }
        public string Nickname { get; set; }

        // 'Override' methoden om de instructie, pose en bijnaam te verkrijgen.

        protected override string GetInstruction() => Instruction;
        protected override string GetPose() => Pose;
        protected override string GetNickname() => Nickname;
    }
}