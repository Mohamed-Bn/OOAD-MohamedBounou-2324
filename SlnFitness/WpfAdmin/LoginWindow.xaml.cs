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
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void EmailTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (email_textbox.Text == "eg: ab@gmail.com")
            {
                email_textbox.Text = "";
            }
        }
        private void login_button_click( object sender , RoutedEventArgs e )
        {
            MainWindow mainWindow = new MainWindow();   
            mainWindow.Show();
            this.Close();


        }


        private void EmailTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(email_textbox.Text))
            {
                email_textbox.Text = "eg: ab@gmail.com";
            }
        }

        private void PasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (password_textbox.Password == "password")
            {
                password_textbox.Password = "";
            }
        }

        private void PasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(password_textbox.Password))
            {
                password_textbox.Password = "password";
            }
        }

    }
}
