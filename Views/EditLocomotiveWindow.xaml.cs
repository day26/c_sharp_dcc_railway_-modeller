using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using DccController.Models;
using Microsoft.Win32;

namespace DccController.Views
{
    public partial class EditLocomotiveWindow : Window
    {
        // The locomotive being edited
        private readonly Locomotive _locomotive;

        public EditLocomotiveWindow(Locomotive locomotive)
        {
            InitializeComponent();
            _locomotive = locomotive;
            PopulateFields();
        }

        // ── Pre-populate fields with existing data ───────────────────
        private void PopulateFields()
        {
            NameTextBox.Text = _locomotive.Name;
            DccAddressTextBox.Text = _locomotive.DccAddress.ToString();
            ClassTextBox.Text = _locomotive.LocoClass;
            ImagePathTextBox.Text = _locomotive.ImagePath;
            NotesTextBox.Text = _locomotive.Notes;

            // Set the manufacturer combobox
            foreach (var item in ManufacturerComboBox.Items)
            {
                if (item is System.Windows.Controls.ComboBoxItem comboItem
                    && comboItem.Content.ToString() == _locomotive.Manufacturer)
                {
                    ManufacturerComboBox.SelectedItem = comboItem;
                    break;
                }
            }

            // Load the image preview if one exists
            if (!string.IsNullOrWhiteSpace(_locomotive.ImagePath)
                && File.Exists(_locomotive.ImagePath))
            {
                LoadImagePreview(_locomotive.ImagePath);
            }
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

        // ── Save Button ──────────────────────────────────────────────
        private void SaveButton_Click(object sender, RoutedEventArgs e)
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

            // Update the locomotive object directly
            _locomotive.Name = NameTextBox.Text.Trim();
            _locomotive.DccAddress = dccAddress;
            _locomotive.Manufacturer = (ManufacturerComboBox.SelectedItem
                as System.Windows.Controls.ComboBoxItem)
                ?.Content?.ToString() ?? "Other";
            _locomotive.LocoClass = ClassTextBox.Text.Trim();
            _locomotive.ImagePath = ImagePathTextBox.Text.Trim();
            _locomotive.Notes = NotesTextBox.Text.Trim();

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