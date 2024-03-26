using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfPlaylist
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private List<Song> songs = new List<Song>();
        private int currentSongIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
            PlaySong();
            UpdateUI();
        }

        private void PlaySong()
        {
            songs.Add(new Song("Jul_Sort_le_cross_volé", new Artist("Jul", "Jul est un rappeur français.", "521.png"), "3:30", "Mp3/5 second song.mp3"));
            songs.Add(new Song("JuL_Love_de_toi", new Artist("Jul", "Jul est un rappeur français.", "521.png"), "3:45", "Mp3/JuL_Love_de_toi.mp3"));
            songs.Add(new Song("13_Block_Fuck_le_17", new Artist("13 Block", "13 Block est un groupe de rap français.", "777.png"), "4:00", "Mp3/13_Block_Fuck_le_17.mp3"));

            foreach (var song in songs)
            {
                lstSongs.Items.Add(song.Name);
            }

            if (songs.Count > 0)
            {
                Song currentSong = songs[currentSongIndex];
                mediaPlayer.Open(new Uri(currentSong.Uri, UriKind.Relative));
                mediaPlayer.Play();
            }
        }

        private void UpdateUI()
        {
            if (songs.Count > 0)
            {
                Song currentSong = songs[currentSongIndex];
                lblSong.Content = currentSong.Name;
                lblArtist.Content = currentSong.Artist.Name;
                imgArtist.Source = new BitmapImage(new Uri($"Photos/{currentSong.Artist.Photo}", UriKind.Relative));
            }
        }

        private void lstSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentSongIndex = lstSongs.SelectedIndex;
            UpdateUI();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            Play();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Next();
        }

        private void Play()
        {
            if (songs.Count > 0)
            {
                Song currentSong = songs[currentSongIndex];
                mediaPlayer.Open(new Uri(currentSong.Uri, UriKind.Relative));
                mediaPlayer.Play();
            }
        }

        private void Stop()
        {
            mediaPlayer.Stop();
        }

        private void Next()
        {
            if (songs.Count > 0)
            {
                currentSongIndex = (currentSongIndex + 1) % songs.Count;
                Stop();
                UpdateUI();
                Play();
            }
        }
    }
}