using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace WpfVcardEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string? chosenFileName;
        string fileName;
        bool changed = false;
        double percentage = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        // Displays the "About" window
        private void About_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().Show();
        }

        // Closes the application
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (changed)
            {
                MessageBoxResult results = MessageBox.Show("Ben je zeker dat je de applicatie wil afsluiten?\nEr zijn nog onopgeslagen wijzigingen", "Toepassing sluiten", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.Yes);
                switch (results)
                {
                    case MessageBoxResult.Yes: Environment.Exit(0); break;
                }
            }
            else if (!changed)
            {
                Environment.Exit(0);
            }
        }

        // Opens a file
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Filter = "vCard|*.VCF";
            bool? dialogResult = dialog.ShowDialog();
            if (!dialogResult == true) return;

            chosenFileName = dialog.FileName;
            fileName = dialog.SafeFileName;
            string[] lines = null;
            try
            {
                lines = File.ReadAllLines(chosenFileName);
            }
            catch (FileNotFoundException)
            { // file not found
                MessageBox.Show($"File {chosenFileName} not found", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (IOException)
            { // unable to open for reading
                MessageBox.Show($"Unable to open {chosenFileName}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (Exception)
            { // use general Exception as fallback
                MessageBox.Show($"Unknown error reading {chosenFileName}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            New_vCard();
            foreach (string line in lines)
            {
                if (line.StartsWith("FN;"))
                {
                    string[] idk = line.Split(':');
                    string[] name = idk[1].Split(' ');
                    txtFirstname.Text = name[0];
                }
                else if (line.StartsWith("N;"))
                {
                    string[] idk = line.Split(':');
                    string[] name = idk[1].Split(';');
                    txtLastname.Text = name[0];
                }
                else if (line.StartsWith("GENDER:"))
                {
                    string[] idk = line.Split(':');
                    if (idk[1] == "M")
                    {
                        rbtMan.IsChecked = true;
                    }
                    else if (idk[1] == "F")
                    {
                        rbtWomen.IsChecked = true;
                    }
                    else
                    {
                        rbtUndifined.IsChecked = true;
                    }
                }
                else if (line.StartsWith("BDAY:"))
                {
                    string[] idk = line.Split(':');
                    DateTime birthday;

                    // bron: classmate
                    DateTime.TryParseExact(idk[1], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out birthday);

                    datBirthday.SelectedDate = birthday;
                }
                else if (line.StartsWith("EMAIL;CHARSET=UTF-8;type=HOME,"))
                {
                    string[] idk = line.Split(':');
                    txtPrivateEmail.Text = idk[1];
                }
                else if (line.StartsWith("EMAIL;CHARSET=UTF-8;type=WORK,"))
                {
                    string[] idk = line.Split(':');
                    txtWorkEmail.Text = idk[1];
                }
                else if (line.StartsWith("PHOTO;ENCODING=b;TYPE=JPEG:"))
                {
                    string[] idk = line.Split(':');
                    byte[] imageBytes = null;
                    try
                    {
                        imageBytes = Convert.FromBase64String(idk[1]);
                    }
                    catch (FormatException ex)
                    {
                        MessageBox.Show($"There was a FormatException trying to convert the image: {ex.Message}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    { // use general Exception as fallback
                        MessageBox.Show($"Unknown error converting the image: {ex.Message}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    if (imageBytes != null)
                    {
                        try
                        {
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                BitmapImage image = new BitmapImage();
                                image.BeginInit();
                                image.StreamSource = ms;
                                image.CacheOption = BitmapCacheOption.OnLoad;
                                image.EndInit();

                                imgPicture.Source = image;
                            }
                        }
                        catch (FormatException ex)
                        {
                            MessageBox.Show($"There was a FormatException trying to convert the image: {ex.Message}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        catch (Exception ex)
                        { // use general Exception as fallback
                            MessageBox.Show($"Unknown error converting the image: {ex.Message}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else if (line.StartsWith("TEL;TYPE=HOME,"))
                {
                    string[] idk = line.Split(':');
                    txtPrivatePhone.Text = idk[1];
                }
                else if (line.StartsWith("TEL;TYPE=WORK,"))
                {
                    string[] idk = line.Split(':');
                    txtWorkPhone.Text = idk[1];
                }
                else if (line.StartsWith("TITLE;"))
                {
                    string[] idk = line.Split(':');
                    txtJobTitle.Text = idk[1];
                }
                else if (line.StartsWith("ORG;"))
                {
                    string[] idk = line.Split(':');
                    txtCompany.Text = idk[1];
                }
                else if (line.StartsWith("X-SOCIALPROFILE;TYPE=facebook:"))
                {
                    string[] idk = line.Split(':');
                    txtFacebook.Text = idk[1];
                }
                else if (line.StartsWith("X-SOCIALPROFILE;TYPE=linkedin:"))
                {
                    string[] idk = line.Split(':');
                    txtLinkedIn.Text = idk[1];
                }
                else if (line.StartsWith("X-SOCIALPROFILE;TYPE=instagram:"))
                {
                    string[] idk = line.Split(':');
                    txtInstagram.Text = idk[1];
                }
                else if (line.StartsWith("X-SOCIALPROFILE;TYPE=youtube:"))
                {
                    string[] idk = line.Split(':');
                    txtYoutube.Text = idk[1];
                }
            }
            lblPicture.Content = "(geen geselecteerd)";
            sbiCurrentCard.Content = $"huidige kaart: {chosenFileName}";
            PercentageFull();
            mitSave.IsEnabled = true;
            changed = false;
        }

        private List<string> SaveFile()
        {
            string gender = "";
            if (rbtMan.IsChecked == true)
            {
                gender = "M";
            }
            else if (rbtWomen.IsChecked == true)
            {
                gender = "F";
            }
            else if (rbtUndifined.IsChecked == true)
            {
                gender = "O";
            }
            DateTime dateTime = default(DateTime);
            try
            {
                dateTime = Convert.ToDateTime(datBirthday.SelectedDate);
            }
            catch (FormatException)
            {
                MessageBox.Show($"File {datBirthday.SelectedDate} not found", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            { // use general Exception as fallback
                MessageBox.Show($"Unknown error converting {datBirthday.SelectedDate}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            List<string> saveLines = new List<string>();
            saveLines.Add("BEGIN:VCARD");
            saveLines.Add("VERSION:3.0");
            saveLines.Add($"FN;CHARSET=UTF-8:{txtFirstname.Text} {txtLastname.Text}");
            saveLines.Add($"N;CHARSET=UTF-8:{txtLastname.Text};{txtFirstname.Text};;;");
            if (gender != "")
            {
                saveLines.Add($"GENDER:{gender}");
            }
            if (dateTime.ToString("yyyyMMdd") != "00010101")
            {
                saveLines.Add($"BDAY:{dateTime.ToString("yyyyMMdd")}");
            }
            if (txtPrivateEmail.Text != "")
            {
                saveLines.Add($"EMAIL;CHARSET=UTF-8;type=HOME,INTERNET:{txtPrivateEmail.Text}");
            }
            if (txtWorkEmail.Text != "")
            {
                saveLines.Add($"EMAIL;CHARSET=UTF-8;type=WORK,INTERNET:{txtWorkEmail.Text}");
            }
            if (imgPicture.Source != null)
            {
                BitmapImage bitmapImage = null;
                try
                {
                    bitmapImage = (BitmapImage)imgPicture.Source;
                }
                catch (FormatException ex)
                {
                    MessageBox.Show($"There was a FormatException trying to convert the image: {ex.Message}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                { // use general Exception as fallback
                    MessageBox.Show($"Unknown error converting the image: {ex.Message}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                string base64String = null;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    BitmapEncoder encoder = new JpegBitmapEncoder();
                    try
                    {
                        encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show($"Failed to create a bitmap frame for the image: {ex.Message}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    { // use general Exception as fallback
                        MessageBox.Show($"Unknown error creating a bitmap frame for the image: {ex.Message}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    encoder.Save(memoryStream);

                    memoryStream.Position = 0;

                    byte[] imageBytes = memoryStream.ToArray();
                    try
                    {
                        base64String = Convert.ToBase64String(imageBytes);
                    }
                    catch (FormatException ex)
                    {
                        MessageBox.Show($"There was a FormatException trying to convert the image: {ex.Message}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    { // use general Exception as fallback
                        MessageBox.Show($"Unknown error converting the image: {ex.Message}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                if (base64String != null)
                {
                    saveLines.Add($"PHOTO;ENCODING=b;TYPE=JPEG:{base64String}");
                }
            }
            if (txtPrivatePhone.Text != "")
            {
                saveLines.Add($"TEL;TYPE=HOME,VOICE:{txtPrivatePhone.Text}");
            }
            if (txtWorkPhone.Text != "")
            {
                saveLines.Add($"TEL;TYPE=WORK,VOICE:{txtWorkPhone.Text}");
            }
            if (txtJobTitle.Text != "")
            {
                saveLines.Add($"TITLE;CHARSET=UTF-8:{txtJobTitle.Text}");
            }
            if (txtCompany.Text != "")
            {
                saveLines.Add($"ORG;CHARSET=UTF-8:{txtCompany.Text}");
            }
            if (txtFacebook.Text != "")
            {
                saveLines.Add($"X-SOCIALPROFILE;TYPE=facebook:{txtFacebook.Text}");
            }
            if (txtLinkedIn.Text != "")
            {
                saveLines.Add($"X-SOCIALPROFILE;TYPE=linkedin:{txtLinkedIn.Text}");
            }
            if (txtInstagram.Text != "")
            {
                saveLines.Add($"X-SOCIALPROFILE;TYPE=instagram:{txtInstagram.Text}");
            }
            if (txtYoutube.Text != "")
            {
                saveLines.Add($"X-SOCIALPROFILE;TYPE=youtube:{txtYoutube.Text}");
            }
            saveLines.Add($"REV:{DateTime.UtcNow.ToString("yyyy-MM-dd")}T{DateTime.UtcNow.ToString("HH:mm:ss.fff")}Z");
            saveLines.Add($"END:VCARD");

            return saveLines;
        }

        // Saves the information to a file
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            List<string> savedLines = SaveFile();

            try
            {
                File.WriteAllLines(chosenFileName, savedLines);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show($"You don't have access to {chosenFileName}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (PathTooLongException)
            {
                MessageBox.Show($"The path to {chosenFileName} is too long", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (IOException)
            { // unable to open for reading
                MessageBox.Show($"Unable to open {chosenFileName}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (Exception)
            { // use general Exception as fallback
                MessageBox.Show($"Unknown error reading {chosenFileName}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show($"File has been succesfully saved at:\n{chosenFileName}", "Saved succesfully", MessageBoxButton.OK, MessageBoxImage.None);
            changed = false;
        }

        // Saves the information to a new file
        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Filter = "vCard|*.VCF";
            dialog.FileName = fileName;
            if (dialog.ShowDialog() != true) return;

            chosenFileName = dialog.FileName;

            List<string> savedLines = SaveFile();

            try
            {
                File.WriteAllLines(chosenFileName, savedLines);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show($"You don't have access to {chosenFileName}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (PathTooLongException)
            {
                MessageBox.Show($"The path to {chosenFileName} is too long", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (IOException)
            { // unable to open for reading
                MessageBox.Show($"Unable to open {chosenFileName}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (Exception)
            { // use general Exception as fallback
                MessageBox.Show($"Unknown error saving {chosenFileName}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            sbiCurrentCard.Content = $"huidige kaart: {chosenFileName}";
            mitSave.IsEnabled = true;
            changed = false;
        }

        private void Card_Changed(object sender, EventArgs e)
        {
            if (sender != mitNew)
            {
                PercentageFull();
                changed = true;
            }
            if (changed && sender == mitNew)
            {
                MessageBoxResult results = MessageBox.Show("Er zijn nog onopgeslagen wijzigingen, wil je alles toch wissen?", "Nog onopgeslagen wijzigingen!", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.Yes);
                if (results == MessageBoxResult.Yes)
                {
                    New_vCard();
                }
            }
            else if (!changed && sender == mitNew)
            {
                New_vCard();
            }
        }

        // Resets the form to create a new vCard
        private void New_vCard()
        {
            txtFirstname.Text = null;
            txtLastname.Text = null;
            datBirthday.SelectedDate = null;
            rbtMan.IsChecked = false;
            rbtWomen.IsChecked = false;
            rbtUndifined.IsChecked = false;
            txtPrivateEmail.Text = null;
            txtPrivatePhone.Text = null;
            imgPicture.Source = null;
            txtCompany.Text = null;
            txtJobTitle.Text = null;
            txtWorkEmail.Text = null;
            txtWorkPhone.Text = null;
            txtLinkedIn.Text = null;
            txtFacebook.Text = null;
            txtInstagram.Text = null;
            txtYoutube.Text = null;
            lblPicture.Content = "(geen geselecteerd)";
            sbiCurrentCard.Content = "huidige kaart: (geen geopend)";
            sbiPercentageFull.Content = "percentage ingevuld: n.a.";
            chosenFileName = null;
            mitSave.IsEnabled = false;
            changed = false;
        }

        // Calculates the percentage of form completion
        private void PercentageFull()
        {
            double count = 0;
            if (!string.IsNullOrEmpty(txtFirstname.Text))
            {
                count += 1;
            }
            if (!string.IsNullOrEmpty(txtLastname.Text))
            {
                count += 1;
            }
            if (datBirthday.SelectedDate != null)
            {
                count += 1;
            }
            if (rbtMan.IsChecked != false || rbtWomen.IsChecked != false || rbtUndifined.IsChecked != false)
            {
                count += 1;
            }
            if (!string.IsNullOrEmpty(txtPrivateEmail.Text))
            {
                count += 1;
            }
            if (!string.IsNullOrEmpty(txtPrivatePhone.Text))
            {
                count += 1;
            }
            if (imgPicture.Source != null)
            {
                count += 1;
            }
            if (!string.IsNullOrEmpty(txtCompany.Text))
            {
                count += 1;
            }
            if (!string.IsNullOrEmpty(txtJobTitle.Text))
            {
                count += 1;
            }
            if (!string.IsNullOrEmpty(txtWorkEmail.Text))
            {
                count += 1;
            }
            if (!string.IsNullOrEmpty(txtWorkPhone.Text))
            {
                count += 1;
            }
            if (!string.IsNullOrEmpty(txtLinkedIn.Text))
            {
                count += 1;
            }
            if (!string.IsNullOrEmpty(txtFacebook.Text))
            {
                count += 1;
            }
            if (!string.IsNullOrEmpty(txtInstagram.Text))
            {
                count += 1;
            }
            if (!string.IsNullOrEmpty(txtYoutube.Text))
            {
                count += 1;
            }
            if (count > 0)
            {
                percentage = (count / 15) * 100;
            }
            sbiPercentageFull.Content = $"percentage ingevuld: {percentage.ToString("0.00")}%";
            percentage = 0;
        }

        // Selects an image to use as a profile picture
        private void BtnPicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Filter = "Image|*.JPEG;*.JPG";
            bool? dialogResult = dialog.ShowDialog();
            if (!dialogResult == true) return;

            string filePath = dialog.FileName;
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = stream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();

                    imgPicture.Source = image;
                }
            }
            catch (FileNotFoundException ex)
            { // file not found
                MessageBox.Show($"File {filePath} not found: {ex.Message}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (IOException ex)
            { // unable to open for reading
                MessageBox.Show($"Unable to open {filePath}: {ex.Message}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (Exception ex)
            { // use general Exception as fallback
                MessageBox.Show($"Unknown error reading {filePath}: {ex.Message}", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            lblPicture.Content = dialog.SafeFileName;
            PercentageFull();
        }
    }
}