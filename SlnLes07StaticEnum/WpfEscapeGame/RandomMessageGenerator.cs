using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    static class RandomMessageGenerator
    {
        private static readonly string[] NotWorking = { "That doesn't seem to work. ", "Well that doesn't work. ", "I should try something else. " };
        private static readonly string[] NotPickupable = { "I can't pick that up. ", "I don't think I can pick that up. ", "That may be a bit too much for me to be able to pickup. " };
        private static readonly string[] Dropped = { "I'll just drop that ", "I don't think I need that ", "I just dropped the " };

        public static string GetRandomMessage(MainWindow.MessageType t)
        {
            switch (t)
            {
                case MainWindow.MessageType.NotWorking: return NotWorking[new Random().Next(0, NotWorking.Length)];
                case MainWindow.MessageType.NotPickupable: return NotPickupable[new Random().Next(0, NotPickupable.Length)];
                case MainWindow.MessageType.Dropped: return Dropped[new Random().Next(0, Dropped.Length)];

                default: return "error not available";
            }
        }
    }
}