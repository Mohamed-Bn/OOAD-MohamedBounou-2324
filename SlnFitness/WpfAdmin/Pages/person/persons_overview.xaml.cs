using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;

namespace WpfAdmin.Pages.person
{

    public partial class persons_overview : Page
    {
        public persons_overview()
        {
            InitializeComponent();
            AddDynamicContent();
        }



        private void AddDynamicContent()
        {
            for (int i = 1; i <= 16; i++)
            {
                Button newTextBlock = new Button();
                newTextBlock.Content = "Content Line " + i;
                newTextBlock.Margin = new Thickness(10);
                newTextBlock.Background = Brushes.White;
                newTextBlock.Click +=show_person_info;
                stackPanel.Children.Add(newTextBlock); 
                
            }
        }



        private void show_person_info(object sender, RoutedEventArgs e)
        {
            name.Content = "Abdullah";
            reg_no.Content = "Nov-22 2022";
            admin.Content = "No";
            img_place.Source = new BitmapImage(new Uri("https://www.indiafilings.com/learn/wp-content/uploads/2023/03/Can-a-single-person-own-a-firm-in-India.jpg"));
        }


        

        private void btn_click_add_person(object sender, RoutedEventArgs e)
        {
            new_person temp = new new_person();
            this.Content = temp;
        }

        private void btn_click_edit_person(object sender, RoutedEventArgs e)
        {
            Frame frame = FindFrame(this);
            if (frame != null)
            {
                edit_person temp = new edit_person();
                frame.NavigationService.Navigate(temp);
            }
        }

        private Frame FindFrame(DependencyObject parent)
        {
            if (parent == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is Frame frame)
                {
                    return frame;
                }
                else
                {
                    var result = FindFrame(child);
                    if (result != null) return result;
                }
            }

            return null;
        }


        private void btn_click_remove_person(object sender, RoutedEventArgs e)
        {

        }
    }
}
