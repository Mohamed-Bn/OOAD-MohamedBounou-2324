using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfTaken
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isFormValid;
        private Stack<object> _memory = new();
        private int _count;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnToevoegen_Click(object sender, RoutedEventArgs e)
        {
            ValidateForm(true);
            if (string.IsNullOrEmpty(txtFout.Text))
            {
                string door = rdbAdam.IsChecked == true ? "Adam" :
                              rdbBilal.IsChecked == true ? "Bilal" :
                              rdbChelsey.IsChecked == true ? "Chelsey" : "error";

                ListBoxItem item = new()
                {
                    Content = $"{txbTaak.Text} (deadline: {dtpDeadline.Text}; door: {door})",
                    Background = cmbPrioriteit.SelectedIndex switch
                    {
                        0 => Brushes.LightCoral,
                        1 => Brushes.Yellow,
                        2 => Brushes.LightGreen,
                        _ => Brushes.Black,
                    }
                };

                lsbLijst.Items.Add(item);
            }
        }

        private void ValidateForm(bool shouldValidate = false)
        {
            if (shouldValidate)
            {
                _isFormValid = true;
            }

            if (_isFormValid)
            {
                var errorMessages = new List<string>();

                if (string.IsNullOrEmpty(txbTaak.Text))
                {
                    errorMessages.Add("gelieve een taak te kiezen");
                }

                if (cmbPrioriteit.SelectedIndex == -1)
                {
                    errorMessages.Add("gelieve een prioriteit te kiezen");
                }

                if (dtpDeadline.SelectedDate == null)
                {
                    errorMessages.Add("gelieve een deadline te kiezen");
                }

                if (rdbAdam.IsChecked == false && rdbBilal.IsChecked == false && rdbChelsey.IsChecked == false)
                {
                    errorMessages.Add("gelieve een uitvoerder te kiezen");
                }

                txtFout.Text = string.Join("\n", errorMessages);
            }
        }

        private void SelectionChanged(object sender, EventArgs e)
        {
            ValidateForm();
        }

        private void LsbLijst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnVerwijderen.IsEnabled = lsbLijst.SelectedIndex >= 0;
        }

        private void BtnVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            PushToMemory();
        }

        private void PushToMemory()
        {
            _memory.Push(lsbLijst.SelectedItem);
            lsbLijst.Items.Remove(lsbLijst.SelectedItem);
            _count++;
            btnTerugzetten.IsEnabled = true;
        }

        private void BtnTerugzetten_Click(object sender, RoutedEventArgs e)
        {
            PopFromMemory();
        }

        private void PopFromMemory()
        {
            lsbLijst.Items.Add(_memory.Pop());
            _count--;
            btnTerugzetten.IsEnabled = _count > 0;
        }
    }
}
