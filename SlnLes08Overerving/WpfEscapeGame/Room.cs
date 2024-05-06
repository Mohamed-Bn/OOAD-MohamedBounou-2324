using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEscapeGame
{
    internal class Room : Actor
    {
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Door> Doors { get; set; } = new List<Door>();
        public string Image { get; }

        public Room(string name, string desc, string image)
            : base(name, desc)
        {
            Image = image;
        }
    }
}