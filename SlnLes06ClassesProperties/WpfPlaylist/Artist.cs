using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPlaylist
{
    public class Artist
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Photo { get; set; }

        public Artist(string name, string bio, string photo)
        {
            Name = name;
            Bio = bio;
            Photo = photo;
        }
    }
}
