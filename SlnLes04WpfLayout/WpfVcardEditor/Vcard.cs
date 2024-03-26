using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfVcardEditor
{
    internal class Vcard
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public char? Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string? PrivateEmail { get; set; }
        public string? WorkEmail { get; set; }
        public BitmapImage? Image { get; set; }
        public string? PrivatePhone { get; set; }
        public string? WorkPhone { get; set; }
        public string? JobTitle { get; set; }
        public string? Company { get; set; }
        public string? Facebook { get; set; }
        public string? LinkedIn { get; set; }
        public string? Instagram { get; set; }
        public string? Youtube { get; set; }

        public Vcard()
        {
            Firstname = null;
            Lastname = null;
            Gender = null;
            Birthday = default;
            PrivateEmail = null;
            WorkEmail = null;
            Image = null;
            PrivatePhone = null;
            WorkPhone = null;
            JobTitle = null;
            Company = null;
            Facebook = null;
            LinkedIn = null;
            Instagram = null;
            Youtube = null;
        }

        public Vcard(string chosenFileName)
        {
            string[] lines;
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

            ResetVcard();
            foreach (string line in lines)
            {
                if (line.StartsWith("FN;") || line.StartsWith("N;"))
                {
                    if (line.StartsWith("FN;"))
                    {
                        string[] idk = line.Split(':');
                        string[] name = idk[1].Split(' ');
                        Firstname = name[0];
                        Lastname = name[1];
                    }
                    else if (line.StartsWith("N;"))
                    {
                        string[] idk = line.Split(':');
                        string[] name = idk[1].Split(';');
                        Lastname = name[0];
                        Firstname = name[1];
                    }
                }
                else if (line.StartsWith("GENDER:"))
                {
                    string[] idk = line.Split(':');
                    Gender = idk[1].ToCharArray().First();
                }
                else if (line.StartsWith("BDAY:"))
                {
                    string[] idk = line.Split(':');
                    DateTime birthday;

                    // bron: classmate
                    DateTime.TryParseExact(idk[1], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out birthday);

                    Birthday = birthday;
                }
                else if (line.StartsWith("EMAIL;CHARSET=UTF-8;type=HOME,"))
                {
                    string[] idk = line.Split(':');
                    PrivateEmail = idk[1];
                }
                else if (line.StartsWith("EMAIL;CHARSET=UTF-8;type=WORK,"))
                {
                    string[] idk = line.Split(':');
                    WorkEmail = idk[1];
                }
                else if (line.StartsWith("PHOTO;ENCODING=") && (line.Contains("jpeg") || line.Contains("JPEG")))
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

                                Image = image;
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
                    PrivatePhone = idk[1];
                }
                else if (line.StartsWith("TEL;TYPE=WORK,"))
                {
                    string[] idk = line.Split(':');
                    WorkPhone = idk[1];
                }
                else if (line.StartsWith("TITLE;"))
                {
                    string[] idk = line.Split(':');
                    JobTitle = idk[1];
                }
                else if (line.StartsWith("ORG;"))
                {
                    string[] idk = line.Split(':');
                    Company = idk[1];
                }
                else if (line.StartsWith("X-SOCIALPROFILE;TYPE=facebook:"))
                {
                    string[] idk = line.Split(':');
                    if (idk[2] != null)
                    {
                        Facebook = idk[1] + ":" + idk[2];
                    }
                    else
                    {
                        Facebook = idk[1];
                    }
                }
                else if (line.StartsWith("X-SOCIALPROFILE;TYPE=linkedin:"))
                {
                    string[] idk = line.Split(':');
                    if (idk[2] != null)
                    {
                        LinkedIn = idk[1] + ":" + idk[2];
                    }
                    else
                    {
                        LinkedIn = idk[1];
                    }
                }
                else if (line.StartsWith("X-SOCIALPROFILE;TYPE=instagram:"))
                {
                    string[] idk = line.Split(':');
                    if (idk[2] != null)
                    {
                        Instagram = idk[1] + ":" + idk[2];
                    }
                    else
                    {
                        Instagram = idk[1];
                    }
                }
                else if (line.StartsWith("X-SOCIALPROFILE;TYPE=youtube:"))
                {
                    string[] idk = line.Split(':');
                    if (idk[2] != null)
                    {
                        Youtube = idk[1] + ":" + idk[2];
                    }
                    else
                    {
                        Youtube = idk[1];
                    }
                }
            }
        }
        public void SaveAsLines(string chosenFileName)
        {
            List<string> saveLines = new List<string>
            {
                "BEGIN:VCARD",
                "VERSION:3.0",
                $"FN;CHARSET=UTF-8:{Firstname} {Lastname}",
                $"N;CHARSET=UTF-8:{Lastname};{Firstname};;;"
            };
            if (Gender != null)
            {
                saveLines.Add($"GENDER:{Gender}");
            }
            if (Birthday.ToString("yyyyMMdd") != "00010101")
            {
                saveLines.Add($"BDAY:{Birthday.ToString("yyyyMMdd")}");
            }
            if (!string.IsNullOrEmpty(PrivateEmail))
            {
                saveLines.Add($"EMAIL;CHARSET=UTF-8;type=HOME,INTERNET:{PrivateEmail}");
            }
            if (!string.IsNullOrEmpty(WorkEmail))
            {
                saveLines.Add($"EMAIL;CHARSET=UTF-8;type=WORK,INTERNET:{WorkEmail}");
            }
            if (Image != null)
            {
                string base64String = null;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    BitmapEncoder encoder = new JpegBitmapEncoder();
                    try
                    {
                        encoder.Frames.Add(BitmapFrame.Create(Image));
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
                if (!string.IsNullOrEmpty(base64String))
                {
                    saveLines.Add($"PHOTO;ENCODING=BASE64;TYPE=image/jpeg:{base64String}");
                }
            }
            if (!string.IsNullOrEmpty(PrivatePhone))
            {
                saveLines.Add($"TEL;TYPE=HOME,VOICE:{PrivatePhone}");
            }
            if (!string.IsNullOrEmpty(WorkPhone))
            {
                saveLines.Add($"TEL;TYPE=WORK,VOICE:{WorkPhone}");
            }
            if (!string.IsNullOrEmpty(JobTitle))
            {
                saveLines.Add($"TITLE;CHARSET=UTF-8:{JobTitle}");
            }
            if (!string.IsNullOrEmpty(Company))
            {
                saveLines.Add($"ORG;CHARSET=UTF-8:{Company}");
            }
            if (!string.IsNullOrEmpty(Facebook))
            {
                saveLines.Add($"X-SOCIALPROFILE;TYPE=facebook:{Facebook}");
            }
            if (!string.IsNullOrEmpty(LinkedIn))
            {
                saveLines.Add($"X-SOCIALPROFILE;TYPE=linkedin:{LinkedIn}");
            }
            if (!string.IsNullOrEmpty(Instagram))
            {
                saveLines.Add($"X-SOCIALPROFILE;TYPE=instagram:{Instagram}");
            }
            if (!string.IsNullOrEmpty(Youtube))
            {
                saveLines.Add($"X-SOCIALPROFILE;TYPE=youtube:{Youtube}");
            }
            saveLines.Add($"REV:{DateTime.UtcNow.ToString("yyyy-MM-dd")}T{DateTime.UtcNow.ToString("HH:mm:ss.fff")}Z");
            saveLines.Add($"END:VCARD");

            try
            {
                File.WriteAllLines(chosenFileName, saveLines);
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
        }
        public void ResetVcard()
        {
            Firstname = null;
            Lastname = null;
            Gender = null;
            Birthday = default;
            PrivateEmail = null;
            WorkEmail = null;
            Image = null;
            PrivatePhone = null;
            WorkPhone = null;
            JobTitle = null;
            Company = null;
            Facebook = null;
            LinkedIn = null;
            Instagram = null;
            Youtube = null;
        }
    }
}
