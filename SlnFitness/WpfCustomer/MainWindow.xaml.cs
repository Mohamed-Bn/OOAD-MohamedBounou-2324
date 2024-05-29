using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfCustomer
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

        private void StatistiekenButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Statistics_Customer page
            MainFrame.Navigate(new Pages.Statistics_Customer());
        }

        private void WorkoutsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Workout_Customer page
            MainFrame.Navigate(new Pages.Workout_Customer());
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}