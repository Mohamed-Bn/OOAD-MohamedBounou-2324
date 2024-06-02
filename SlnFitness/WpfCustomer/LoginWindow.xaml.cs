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
using CLFitness.WpfCustomer;
using System.Linq;
using System.Security.Cryptography;

namespace WpfCustomer
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        List<Person> persons;

        public LoginWindow()
        {
            InitializeComponent();
            persons = Person.GetAllPerson();
        }

        private void RemoveText(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == "Username")
            {
                textBox.Text = "";
            }

            if (sender is PasswordBox passwordBox && passwordBox.Tag.ToString() == "Password")
            {
                passwordBox.Tag = "";
                passwordBox.Password = "";
            }
        }

        private void AddText(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Username";
            }

            if (sender is PasswordBox passwordBox && string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                passwordBox.Tag = "Password";
                passwordBox.Password = "";
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                if (string.IsNullOrWhiteSpace(passwordBox.Password))
                {
                    passwordBox.Tag = "Password";
                }
                else
                {
                    passwordBox.Tag = "";
                }
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            Person loggedInPerson = IsLoginValid(username, password);
            if (loggedInPerson != null)
            {
                LoginMessageTextBlock.Foreground = Brushes.Green;
                LoginMessageTextBlock.Text = "Login successful!";
                MainWindow mainWindow = new MainWindow(loggedInPerson);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                LoginMessageTextBlock.Foreground = Brushes.Red;
                LoginMessageTextBlock.Text = "Invalid username or password.";
            }
        }

        private Person IsLoginValid(string username, string password)
        {
            string hashedPassword = GetSHA256Hash(password);

            for (int i = 0; i < persons.Count; i++)
            {
                if (persons[i].Login == username && persons[i].Password == hashedPassword)
                {
                    return persons[i];
                }
            }

            return null;
        }

        private string GetSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}

