namespace DccController.Models
{
    public class Locomotive
    {
        // Core DCC Properties
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DccAddress { get; set; }

        // Manufacturer Details
        public string Manufacturer { get; set; } = string.Empty;
        public string LocoClass { get; set; } = string.Empty;

        // Photo
        public string ImagePath { get; set; } = string.Empty;

        // Notes
        public string Notes { get; set; } = string.Empty;

        // Current State (not saved - runtime only)
        public int CurrentSpeed { get; set; } = 0;
        public bool IsForward { get; set; } = true;
        public bool IsSelected { get; set; } = false;

        // Display helper for the roster list
        public string DisplayName =>
            $"[{DccAddress}] {Name} ({Manufacturer} {LocoClass})";
    }
}