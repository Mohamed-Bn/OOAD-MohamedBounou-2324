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
                CreateFiles(pathTextBox.Text);  // Créez les fichiers ici
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
            try
            {
                var files = Directory.GetFiles(pathTextBox.Text, "*.*", SearchOption.AllDirectories)
                    .Where(s => s.EndsWith(".css") || s.EndsWith(".html") || s.EndsWith(".js"));
                foreach (var file in files)
                {
                    fileListBox.Items.Add(file);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la lecture du répertoire : {ex.Message}");
            }
        }

        private void MinifyFiles(string outputPath)
        {
            try
            {
                var selectedFile = fileListBox.SelectedItem.ToString();
                var extension = Path.GetExtension(selectedFile);
                var contents = File.ReadAllText(selectedFile);
                var minified = MinifyFile(contents, extension);
                var fileName = Path.GetFileNameWithoutExtension(selectedFile);
                var minifiedFileName = $"{fileName}.min{extension}";
                var minifiedFilePath = Path.Combine(outputPath, minifiedFileName);
                File.WriteAllText(minifiedFilePath, minified);
                fileListBox.Items.Add(minifiedFilePath); // Ajoutez le fichier minifié à la liste
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la minification des fichiers : {ex.Message}");
            }
        }

        private string MinifyFile(string input, string fileExtension)
        {
            switch (fileExtension)
            {
                case ".css":
                    return MinifyCss(input);
                case ".html":
                    return MinifyHtml(input);
                case ".js":
                    return MinifyJavaScript(input);
                default:
                    return input;
            }
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
            // Créez un fichier .css, .html et .js dans le dossier spécifié
            File.WriteAllText(Path.Combine(folderPath, "test.css"), "/* Ceci est un fichier CSS de test */");
            File.WriteAllText(Path.Combine(folderPath, "test.html"), "<!-- Ceci est un fichier HTML de test -->");
            File.WriteAllText(Path.Combine(folderPath, "test.js"), "// Ceci est un fichier JavaScript de test");
        }
    }
}
