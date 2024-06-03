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
using WpfAdmin.Pages;

namespace WpfAdmin
{

    public partial class LoginWindow : Window
    {
        private bool isPasswordSet;
        public LoginWindow()
        {
            InitializeComponent();
            SetInitialPassword("admin123");
        }

        private void SetInitialPassword(string password)
        {
            password_textbox.Password = password;
            isPasswordSet = true;
        }
     
        private void login_button_click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();


        }



    }
}
