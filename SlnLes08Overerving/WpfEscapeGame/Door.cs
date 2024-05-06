using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WpfEscapeGame
{
    internal class Door : LockableItem
    {
        public Room ToRoom { get; set; }

        public Door(string name, string desc, Room toRoom)
            : base(name, desc)
        {
            ToRoom = toRoom;
        }
        public Door(string name, string desc, Item key, Room toRoom)
            : this(name, desc, toRoom)
        {
            Key = key;
        }
        public Door(string name, string desc, bool isLocked, Room toRoom)
            : this(name, desc, toRoom)
        {
            IsLocked = isLocked;
        }
    }
}
