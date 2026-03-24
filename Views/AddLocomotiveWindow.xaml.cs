using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using DccController.Models;
using Microsoft.Win32;

namespace DccController.Views
{
    public partial class AddLocomotiveWindow : Window
    {
        // This will hold the new locomotive when the user clicks Add
        public Locomotive? NewLocomotive { get; private set; }

        public AddLocomotiveWindow()
        {
            InitializeComponent();
        }

        // ── Browse for Image ─────────────────────────────────────────
        private void BrowseImageButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select Locomotive Photo",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                InitialDirectory = Environment.GetFolderPath(
                    Environment.SpecialFolder.MyPictures)
            };

            if (dialog.ShowDialog() == true)
            {
                ImagePathTextBox.Text = dialog.FileName;
                LoadImagePreview(dialog.FileName);
            }
        }

        private void LoadImagePreview(string imagePath)
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                LocoImagePreview.Source = bitmap;
            }
            catch
            {
                LocoImagePreview.Source = null;
            }
        }

        // ── Add Button ───────────────────────────────────────────────
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate Name
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Please enter a locomotive name.",
                    "Validation", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // Validate DCC Address
            if (!int.TryParse(DccAddressTextBox.Text, out int dccAddress)
                || dccAddress < 1 || dccAddress > 9999)
            {
                MessageBox.Show("Please enter a valid DCC address (1-9999).",
                    "Validation", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // Build the locomotive object
            NewLocomotive = new Locomotive
            {
                Name = NameTextBox.Text.Trim(),
                DccAddress = dccAddress,
                Manufacturer = (ManufacturerComboBox.SelectedItem
                    as System.Windows.Controls.ComboBoxItem)
                    ?.Content?.ToString() ?? "Other",
                LocoClass = ClassTextBox.Text.Trim(),
                ImagePath = ImagePathTextBox.Text.Trim(),
                Notes = NotesTextBox.Text.Trim()
            };

            DialogResult = true;
            Close();
        }

        // ── Cancel Button ────────────────────────────────────────────
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}