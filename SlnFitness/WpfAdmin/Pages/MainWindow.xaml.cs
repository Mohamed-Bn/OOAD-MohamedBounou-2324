using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAdmin.Pages.person;
using WpfAdmin.Pages.exercises;
using CLFitness.WpfAdmin;
using CLFitness.WpfCustomer;


namespace WpfAdmin.Pages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            default_load();
        }

        private void Main_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }


        private void default_load()
        {
            persons_overview person = new persons_overview();
            frame.NavigationService.Navigate(person);
        }

        private void Personeon_btn_Click(object sender, RoutedEventArgs e)
        {
            persons_overview person=new persons_overview();
            frame.NavigationService.Navigate(person);
        }



        private void exercise_btn_Click(object sender, RoutedEventArgs e)
        {
             exercises_overview exercises = new exercises_overview();
            frame.NavigationService.Navigate(exercises);

        }

        private void back_btn_Click(object sender , RoutedEventArgs e)
        {
            LoginWindow login_window = new LoginWindow();
            login_window.Show();
            this.Close();
        }
}
}
