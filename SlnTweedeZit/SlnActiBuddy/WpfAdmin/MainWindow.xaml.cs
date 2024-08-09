using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CLActiBuddy;
using WpfAdmin.ActiviteitPage;
using WpfAdmin.PersoonPage;

namespace WpfAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Persoon? persoon = (Persoon?)Application.Current.Properties["persoon"];
            if (persoon != null)
            {
                imgFoto.Source = ByteToImage(persoon.Profielfoto);
                MainFrame.Content = new PersonenOverzichtPage();
            }
        }

        // https://stackoverflow.com/questions/22065815/how-to-convert-byte-array-to-imagesource-for-windows-8-0-store-application
        public static ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage biImg = new ();
            MemoryStream ms = new (imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg;

            return imgSrc;
        }

        private void BtnPersonen_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new PersonenOverzichtPage();
        }

        private void BtnActiviteiten_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ActiviteitenOverzichtPage();
        }

        private void BtnUitloggen_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
