using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using Microsoft.Win32;

namespace WpfMinifier
{/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
    public partial class MainWindow : Window
    {
        private string selectedFolderPath = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                InitialDirectory = string.IsNullOrEmpty(pathTextBox.Text) ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) : pathTextBox.Text,
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Folder Selection."
            };

            if (dialog.ShowDialog() == true)
            {
                pathTextBox.Text = Path.GetDirectoryName(dialog.FileName);
                CreateFiles(pathTextBox.Text); 
                UpdateFileList();
            }
        }

        private void Minify_Click(object sender, RoutedEventArgs e)
        {
            MinifyFiles(pathTextBox.Text);
        }

        private void MinifyAs_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                ValidateNames = false,
                CheckPathExists = true,
                FileName = "Folder Selection."
            };

            if (dialog.ShowDialog() == true)
            {
                MinifyFiles(Path.GetDirectoryName(dialog.FileName));
            }
        }

        private void UpdateFileList()
        {
            fileListBox.Items.Clear();
            var files = Directory.GetFiles(pathTextBox.Text, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".css") || s.EndsWith(".html") || s.EndsWith(".js"));
            foreach (var file in files)
            {
                fileListBox.Items.Add(file);
            }
        }

        private void MinifyFiles(string outputPath)
        {
            var selectedFile = fileListBox.SelectedItem.ToString();
            var extension = Path.GetExtension(selectedFile);
            var contents = File.ReadAllText(selectedFile);
            var minified = MinifyFile(contents, extension);
            var fileName = Path.GetFileNameWithoutExtension(selectedFile);
            var minifiedFileName = $"{fileName}.min{extension}";
            var minifiedFilePath = Path.Combine(outputPath, minifiedFileName);
            File.WriteAllText(minifiedFilePath, minified);
            fileListBox.Items.Add(minifiedFilePath);
        }

        private string MinifyFile(string input, string fileExtension)
        {
            return fileExtension switch
            {
                ".css" => MinifyCss(input),
                ".html" => MinifyHtml(input),
                ".js" => MinifyJavaScript(input),
                _ => input,
            };
        }

        private string MinifyCss(string input)
        {
            return Regex.Replace(input, @"\s+", " ");
        }

        private string MinifyHtml(string input)
        {
            return Regex.Replace(input, @"\s+", " ");
        }

        private string MinifyJavaScript(string input)
        {
            return Regex.Replace(input, @"\s+", " ");
        }

        private void CreateFiles(string folderPath)
        {
            File.WriteAllText(Path.Combine(folderPath, "test.css"), "/* css */");
            File.WriteAllText(Path.Combine(folderPath, "test.html"), "<!-- html -->");
            File.WriteAllText(Path.Combine(folderPath, "test.js"), "// javascript");
        }
    }
}
