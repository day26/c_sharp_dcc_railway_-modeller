using Microsoft.Win32;
using System.IO;
using DccController.Models;
using DccController.Models.Track;
using DccController.Services;
using DccController.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace DccController
{
    public partial class MainWindow : Window
    {
        // Track whether power is on
        private bool _trackPowerOn = false;

        // Track current direction
        private string _currentDirection = "FWD";

        // Roster and service
        private List<Locomotive> _locomotives = new List<Locomotive>();
        private RosterService _rosterService = new RosterService();
        private Locomotive? _selectedLocomotive = null;

        // Track layout
        private TrackLayout? _trackLayout = null;
        private readonly TrackLayoutService _trackLayoutService = new TrackLayoutService();
        private readonly TrackRenderer _trackRenderer = new TrackRenderer();

        // Zoom and pan
        private double _zoomLevel = 1.0;
        private const double ZoomStep = 0.1;
        private const double ZoomMin = 0.3;
        private const double ZoomMax = 3.0;
        private Point _panStartPoint;
        private bool _isPanning = false;

        // WASD pan speed in pixels per key press
        private const double KeyPanStep = 40.0;

        // Layout Save Service
        private readonly LayoutSaveService _layoutSaveService =
            new LayoutSaveService();

        public MainWindow()
        {
            InitializeComponent();
            InitialiseControls();
            LoadRoster();
            LoadExampleLayout();
            InitialiseCanvasControls();
        }

        // ── Initialise ───────────────────────────────────────────────
        private void InitialiseControls()
        {
            // Wire up the speed slider change event
            SpeedSlider.ValueChanged += SpeedSlider_ValueChanged;

            // Wire up the buttons
            ForwardButton.Click += ForwardButton_Click;
            ReverseButton.Click += ReverseButton_Click;
            StopButton.Click += StopButton_Click;
            PowerButton.Click += PowerButton_Click;

            // Wire up roster list selection
            LocomotiveRoster.SelectionChanged += LocomotiveRoster_SelectionChanged;

            // Wire up menu items
            WireUpMenuItems();
        }

        private void WireUpMenuItems()
        {
            var menu = (Menu)((DockPanel)Content).Children[0];

            // File menu — find items by header text
            var fileMenu = (MenuItem)menu.Items[0];
            foreach (var item in fileMenu.Items)
            {
                if (item is MenuItem menuItem)
                {
                    switch (menuItem.Header.ToString())
                    {
                        case "New Layout":
                            menuItem.Click += NewLayout_Click;
                            break;
                        case "Open Layout":
                            menuItem.Click += OpenLayout_Click;
                            break;
                        case "Save Layout":
                            menuItem.Click += SaveLayout_Click;
                            break;
                        case "Exit":
                            menuItem.Click += (s, e) => Close();
                            break;
                    }
                }
            }

            // Locomotives menu — find items by header text
            var locoMenu = (MenuItem)menu.Items[1];
            foreach (var item in locoMenu.Items)
            {
                if (item is MenuItem menuItem)
                {
                    switch (menuItem.Header.ToString())
                    {
                        case "Add Locomotive":
                            menuItem.Click += AddLocomotive_Click;
                            break;
                        case "Manage Roster":
                            menuItem.Click += ManageRoster_Click;
                            break;
                    }
                }
            }
        }

        // ── Roster ───────────────────────────────────────────────────
        private void LoadRoster()
        {
            try
            {
                _locomotives = _rosterService.LoadRoster();
                RefreshRosterDisplay();
                UpdateStatus($"Roster loaded — {_locomotives.Count} locomotive(s)");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load roster:\n{ex.Message}",
                    "Load Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // ── Track Layout ─────────────────────────────────────────────
        private void LoadExampleLayout()
        {
            try
            {
                _trackLayout = _trackLayoutService.CreateExampleLayout();
                _trackRenderer.RenderLayout(TrackCanvas, _trackLayout);
                UpdateStatus($"Layout loaded: {_trackLayout.Name}");

                // ContentRendered fires after the window is fully drawn
                ContentRendered += (s, e) => ScrollToCentre();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load layout:\n{ex.Message}",
                    "Layout Error", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        // ── New Layout ────────────────────────────────────────────────
        private void NewLayout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Start a new layout? Any unsaved changes will be lost.",
                "New Layout",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _trackLayout = new TrackLayout { Name = "New Layout" };
                _trackRenderer.RenderLayout(TrackCanvas, _trackLayout);
                UpdateStatus("New layout created");
                ScrollToCentre();
            }
        }

        // ── Save Layout ───────────────────────────────────────────────
        private void SaveLayout_Click(object sender, RoutedEventArgs e)
        {
            if (_trackLayout == null)
            {
                MessageBox.Show("No layout to save.",
                    "Save Layout", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            var dialog = new SaveFileDialog
            {
                Title = "Save Layout",
                Filter = "DCC Layout Files (*.dcclayout)|*.dcclayout|All Files (*.*)|*.*",
                DefaultExt = "dcclayout",
                InitialDirectory = Environment.GetFolderPath(
                    Environment.SpecialFolder.MyDocuments)
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _layoutSaveService.SaveLayout(
                        dialog.FileName,
                        _trackLayout,
                        _locomotives);

                    UpdateStatus($"Layout saved: " +
                        $"{Path.GetFileName(dialog.FileName)}");

                    MessageBox.Show("Layout saved successfully!",
                        "Saved", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not save layout:\n{ex.Message}",
                        "Save Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        // ── Open Layout ───────────────────────────────────────────────
        private void OpenLayout_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Open Layout",
                Filter = "DCC Layout Files (*.dcclayout)|*.dcclayout|All Files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(
                    Environment.SpecialFolder.MyDocuments)
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var (layout, locomotives) =
                        _layoutSaveService.LoadLayout(dialog.FileName);

                    _trackLayout = layout;
                    _trackRenderer.RenderLayout(TrackCanvas, _trackLayout);

                    // Merge loaded locomotives into roster
                    foreach (var loco in locomotives)
                    {
                        if (!_locomotives.Any(l => l.Id == loco.Id))
                        {
                            _locomotives.Add(loco);
                        }
                    }

                    RefreshRosterDisplay();
                    ScrollToCentre();

                    UpdateStatus($"Layout loaded: {layout.Name} — " +
                        $"{layout.Pieces.Count} piece(s)");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not open layout:\n{ex.Message}",
                        "Load Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void ScrollToCentre()
        {
            // Scroll so the canvas centre is in the middle of the viewport
            double targetX = TrackConstants.CanvasCentreX * _zoomLevel
                - TrackScrollViewer.ViewportWidth / 2.0;
            double targetY = TrackConstants.CanvasCentreY * _zoomLevel
                - TrackScrollViewer.ViewportHeight / 2.0;

            TrackScrollViewer.ScrollToHorizontalOffset(Math.Max(0, targetX));
            TrackScrollViewer.ScrollToVerticalOffset(Math.Max(0, targetY));
        }

        // ── WASD Key Navigation ───────────────────────────────────────
        private void TrackCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            double panStep = KeyPanStep;

            // Hold shift to pan faster
            if (Keyboard.IsKeyDown(Key.LeftShift) ||
                Keyboard.IsKeyDown(Key.RightShift))
                panStep *= 3;

            switch (e.Key)
            {
                case Key.W:
                case Key.Up:
                    TrackScrollViewer.ScrollToVerticalOffset(
                        TrackScrollViewer.VerticalOffset - panStep);
                    e.Handled = true;
                    break;

                case Key.S:
                case Key.Down:
                    TrackScrollViewer.ScrollToVerticalOffset(
                        TrackScrollViewer.VerticalOffset + panStep);
                    e.Handled = true;
                    break;

                case Key.A:
                case Key.Left:
                    TrackScrollViewer.ScrollToHorizontalOffset(
                        TrackScrollViewer.HorizontalOffset - panStep);
                    e.Handled = true;
                    break;

                case Key.D:
                case Key.Right:
                    TrackScrollViewer.ScrollToHorizontalOffset(
                        TrackScrollViewer.HorizontalOffset + panStep);
                    e.Handled = true;
                    break;

                // Ctrl + = to zoom in, Ctrl + - to zoom out
                case Key.OemPlus:
                case Key.Add:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl) ||
                        Keyboard.IsKeyDown(Key.RightCtrl))
                        AdjustZoom(ZoomStep);
                    e.Handled = true;
                    break;

                case Key.OemMinus:
                case Key.Subtract:
                    if (Keyboard.IsKeyDown(Key.LeftCtrl) ||
                        Keyboard.IsKeyDown(Key.RightCtrl))
                        AdjustZoom(-ZoomStep);
                    e.Handled = true;
                    break;

                // Home key to snap back to centre
                case Key.Home:
                    ScrollToCentre();
                    e.Handled = true;
                    break;
            }
        }

        // ── Zoom and Pan Setup ────────────────────────────────────────
        private void InitialiseCanvasControls()
        {
            // Zoom buttons
            // Zoom buttons (no mouse position — zoom from centre)
            ZoomInButton.Click += (s, e) => AdjustZoom(ZoomStep);
            ZoomOutButton.Click += (s, e) => AdjustZoom(-ZoomStep);
            ZoomResetButton.Click += (s, e) => ResetZoom();

            // Mouse wheel zoom toward cursor position
            TrackScrollViewer.PreviewMouseWheel += (s, e) =>
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) ||
                    Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    var mousePos = e.GetPosition(TrackScrollViewer);
                    AdjustZoom(e.Delta > 0 ? ZoomStep : -ZoomStep, mousePos);
                    e.Handled = true;
                }
            };

            // Pan with middle mouse button
            TrackScrollViewer.PreviewMouseDown += (s, e) =>
            {
                if (e.MiddleButton == MouseButtonState.Pressed)
                {
                    _isPanning = true;
                    _panStartPoint = e.GetPosition(TrackScrollViewer);
                    TrackScrollViewer.Cursor = Cursors.Hand;
                    e.Handled = true;
                }
            };

            TrackScrollViewer.PreviewMouseMove += (s, e) =>
            {
                if (_isPanning)
                {
                    var current = e.GetPosition(TrackScrollViewer);
                    double dx = current.X - _panStartPoint.X;
                    double dy = current.Y - _panStartPoint.Y;
                    TrackScrollViewer.ScrollToHorizontalOffset(
                        TrackScrollViewer.HorizontalOffset - dx);
                    TrackScrollViewer.ScrollToVerticalOffset(
                        TrackScrollViewer.VerticalOffset - dy);
                    _panStartPoint = current;
                }
            };

            TrackScrollViewer.PreviewMouseUp += (s, e) =>
            {
                if (e.MiddleButton == MouseButtonState.Released)
                {
                    _isPanning = false;
                    TrackScrollViewer.Cursor = Cursors.Arrow;
                }
            };

            // Overlay checkboxes
            ShowEndPointsCheckBox.Checked += (s, e) => RefreshCanvas();
            ShowEndPointsCheckBox.Unchecked += (s, e) => RefreshCanvas();
            ShowRefOverlayCheckBox.Checked += (s, e) => RefreshCanvas();
            ShowRefOverlayCheckBox.Unchecked += (s, e) => RefreshCanvas();

            // Manufacturer selector
            ManufacturerOverlayComboBox.SelectionChanged += (s, e) =>
                RefreshCanvas();

            // WASD keyboard navigation
            // Make sure the window captures key events
            this.KeyDown += TrackCanvas_KeyDown;
            TrackScrollViewer.Focusable = true;
        }

        private void AdjustZoom(double delta, Point? mousePosition = null)
        {
            // Remember where the mouse is on the canvas before zooming
            double mouseCanvasX = 0;
            double mouseCanvasY = 0;

            if (mousePosition.HasValue)
            {
                mouseCanvasX = (TrackScrollViewer.HorizontalOffset +
                    mousePosition.Value.X) / _zoomLevel;
                mouseCanvasY = (TrackScrollViewer.VerticalOffset +
                    mousePosition.Value.Y) / _zoomLevel;
            }

            double oldZoom = _zoomLevel;
            _zoomLevel = Math.Clamp(_zoomLevel + delta, ZoomMin, ZoomMax);
            ApplyZoom();

            // After zooming scroll so the point under the mouse stays fixed
            if (mousePosition.HasValue && _zoomLevel != oldZoom)
            {
                double newOffsetX = mouseCanvasX * _zoomLevel -
                    mousePosition.Value.X;
                double newOffsetY = mouseCanvasY * _zoomLevel -
                    mousePosition.Value.Y;

                TrackScrollViewer.ScrollToHorizontalOffset(
                    Math.Max(0, newOffsetX));
                TrackScrollViewer.ScrollToVerticalOffset(
                    Math.Max(0, newOffsetY));
            }
        }

        private void ResetZoom()
        {
            _zoomLevel = 1.0;
            ApplyZoom();
        }

        private void ApplyZoom()
        {
            CanvasScale.ScaleX = _zoomLevel;
            CanvasScale.ScaleY = _zoomLevel;

            TrackCanvasContainer.Width =
                TrackConstants.CanvasWidth * _zoomLevel;
            TrackCanvasContainer.Height =
                TrackConstants.CanvasHeight * _zoomLevel;

            int percent = (int)(_zoomLevel * 100);
            ZoomLevelText.Text = $"{percent}%";
        }

        private void RefreshCanvas()
        {
            if (_trackLayout == null) return;

            // Update renderer settings from UI controls
            _trackRenderer.ShowStartEndPoints =
                ShowEndPointsCheckBox.IsChecked == true;
            _trackRenderer.ShowReferenceOverlay =
                ShowRefOverlayCheckBox.IsChecked == true;

            // Set manufacturer from combobox
            _trackRenderer.OverlayManufacturer =
                (ManufacturerOverlayComboBox.SelectedItem
                    as ComboBoxItem)?.Content?.ToString() switch
                {
                    "Peco" => TrackManufacturer.Peco,
                    "Bachmann" => TrackManufacturer.Bachmann,
                    _ => TrackManufacturer.Hornby
                };

            _trackRenderer.RenderLayout(TrackCanvas, _trackLayout);
        }

        private void RefreshRosterDisplay()
        {
            LocomotiveRoster.Items.Clear();

            foreach (var loco in _locomotives)
            {
                var item = new ListBoxItem
                {
                    Content = loco.DisplayName,
                    Tag = loco,
                    Foreground = new SolidColorBrush(Colors.White),
                    Background = new SolidColorBrush(Color.FromRgb(30, 30, 46)),
                    Padding = new Thickness(5)
                };
                LocomotiveRoster.Items.Add(item);
            }
        }

        private void LocomotiveRoster_SelectionChanged(object sender,
    SelectionChangedEventArgs e)
        {
            if (LocomotiveRoster.SelectedItem is ListBoxItem item
                && item.Tag is Locomotive loco)
            {
                _selectedLocomotive = loco;

                // Reset speed slider to this loco's current speed
                SpeedSlider.Value = loco.CurrentSpeed;

                // Update the details panel
                UpdateDetailsPanel(loco);

                UpdateStatus($"Selected: {loco.Name} " +
                    $"(Address {loco.DccAddress})");
            }
        }

        private void UpdateDetailsPanel(Locomotive loco)
        {
            // Update text fields
            DetailNameText.Text = loco.Name;
            DetailAddressText.Text = loco.DccAddress.ToString();
            DetailManufacturerText.Text = loco.Manufacturer;
            DetailClassText.Text = string.IsNullOrWhiteSpace(loco.LocoClass)
                ? "—" : loco.LocoClass;
            DetailNotesText.Text = string.IsNullOrWhiteSpace(loco.Notes)
                ? "—" : loco.Notes;

            // Update the photo
            if (!string.IsNullOrWhiteSpace(loco.ImagePath)
                && System.IO.File.Exists(loco.ImagePath))
            {
                try
                {
                    var bitmap = new System.Windows.Media.Imaging.BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(loco.ImagePath);
                    bitmap.CacheOption =
                        System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    LocoDetailImage.Source = bitmap;
                    NoImageText.Visibility = Visibility.Collapsed;
                }
                catch
                {
                    LocoDetailImage.Source = null;
                    NoImageText.Visibility = Visibility.Visible;
                }
            }
            else
            {
                LocoDetailImage.Source = null;
                NoImageText.Visibility = Visibility.Visible;
            }
        }

        // ── Add Locomotive ───────────────────────────────────────────
        private void AddLocomotive_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddLocomotiveWindow
            {
                Owner = this
            };

            if (dialog.ShowDialog() == true && dialog.NewLocomotive != null)
            {
                var loco = dialog.NewLocomotive;
                loco.Id = _rosterService.GetNextId(_locomotives);

                _locomotives.Add(loco);

                try
                {
                    _rosterService.SaveRoster(_locomotives);
                    RefreshRosterDisplay();
                    UpdateStatus($"Added: {loco.Name}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not save roster:\n{ex.Message}",
                        "Save Error", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void ManageRoster_Click(object sender, RoutedEventArgs e)
        {
            // We will build this in a later step
            MessageBox.Show("Manage Roster coming soon!",
                "Coming Soon", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        // ── Edit Locomotive ──────────────────────────────────────────
        private void EditLocoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (LocomotiveRoster.SelectedItem is ListBoxItem item
                && item.Tag is Locomotive loco)
            {
                var dialog = new EditLocomotiveWindow(loco)
                {
                    Owner = this
                };

                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        _rosterService.SaveRoster(_locomotives);
                        RefreshRosterDisplay();
                        UpdateDetailsPanel(loco);
                        UpdateStatus($"Updated: {loco.Name}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Could not save roster:\n{ex.Message}",
                            "Save Error", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a locomotive to edit.",
                    "No Selection", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        // ── Delete Locomotive ────────────────────────────────────────
        private void DeleteLocoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (LocomotiveRoster.SelectedItem is ListBoxItem item
                && item.Tag is Locomotive loco)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete '{loco.Name}'?\n\n" +
                    "This cannot be undone.",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _rosterService.DeleteLocomotive(_locomotives, loco.Id);
                        RefreshRosterDisplay();

                        // Clear the details panel
                        ClearDetailsPanel();

                        _selectedLocomotive = null;
                        UpdateStatus($"Deleted: {loco.Name}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Could not delete locomotive:\n{ex.Message}",
                            "Delete Error", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a locomotive to delete.",
                    "No Selection", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        // ── Clear Details Panel ──────────────────────────────────────
        private void ClearDetailsPanel()
        {
            DetailNameText.Text = "—";
            DetailAddressText.Text = "—";
            DetailManufacturerText.Text = "—";
            DetailClassText.Text = "—";
            DetailNotesText.Text = "—";
            LocoDetailImage.Source = null;
            NoImageText.Visibility = Visibility.Visible;
        }

        // ── Speed Slider ─────────────────────────────────────────────
        private void SpeedSlider_ValueChanged(object sender,
            RoutedPropertyChangedEventArgs<double> e)
        {
            int speed = (int)SpeedSlider.Value;

            if (SpeedValueText != null)
                SpeedValueText.Text = $"Speed: {speed}";

            if (_selectedLocomotive != null)
                _selectedLocomotive.CurrentSpeed = speed;

            // TODO: Send speed command to DCC controller
            UpdateStatus($"Speed set to {speed}");
        }

        // ── Direction Buttons ────────────────────────────────────────
        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            _currentDirection = "FWD";
            ForwardButton.Background = new SolidColorBrush(
                Color.FromRgb(39, 174, 96));
            ReverseButton.Background = new SolidColorBrush(
                Color.FromRgb(100, 60, 60));

            if (_selectedLocomotive != null)
                _selectedLocomotive.IsForward = true;

            // TODO: Send direction command to DCC controller
            UpdateStatus("Direction: Forward");
        }

        private void ReverseButton_Click(object sender, RoutedEventArgs e)
        {
            _currentDirection = "REV";
            ReverseButton.Background = new SolidColorBrush(
                Color.FromRgb(231, 76, 60));
            ForwardButton.Background = new SolidColorBrush(
                Color.FromRgb(39, 100, 60));

            if (_selectedLocomotive != null)
                _selectedLocomotive.IsForward = false;

            // TODO: Send direction command to DCC controller
            UpdateStatus("Direction: Reverse");
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            SpeedSlider.Value = 0;

            if (_selectedLocomotive != null)
                _selectedLocomotive.CurrentSpeed = 0;

            // TODO: Send emergency stop command to DCC controller
            UpdateStatus("STOP issued");
        }

        // ── Power Button ─────────────────────────────────────────────
        private void PowerButton_Click(object sender, RoutedEventArgs e)
        {
            _trackPowerOn = !_trackPowerOn;

            if (_trackPowerOn)
            {
                PowerStatusText.Text = "Track Power: ON";
                PowerStatusText.Foreground =
                    new SolidColorBrush(Colors.LimeGreen);
                PowerButton.Background = new SolidColorBrush(
                    Color.FromRgb(142, 68, 173));
                UpdateStatus("Track power ON");
            }
            else
            {
                PowerStatusText.Text = "Track Power: OFF";
                PowerStatusText.Foreground =
                    new SolidColorBrush(Colors.Red);
                PowerButton.Background = new SolidColorBrush(
                    Color.FromRgb(80, 40, 100));
                UpdateStatus("Track power OFF");
            }
        }

        // ── Helper Methods ───────────────────────────────────────────
        private void UpdateStatus(string message)
        {
            StatusText.Text = $"Status: {message}";
        }
    }
}