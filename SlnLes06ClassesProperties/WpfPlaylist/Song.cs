using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPlaylist
{
    public class Song
    {
        public string Name { get; set; }
        public Artist Artist { get; set; }
        public string Duration { get; set; }
        public string Uri { get; set; }

        public Song(string name, Artist artist, string duration, string uri)
        {
            Name = name;
            Artist = artist;
            Duration = duration;
            Uri = uri;
        }
    }
}
