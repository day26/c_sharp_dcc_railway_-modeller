namespace DccController.Models
{
    public class LayoutSaveFile
    {
        // File format version — useful for future migrations
        public string Version { get; set; } = "1.0";

        // When this file was saved
        public DateTime SavedAt { get; set; } = DateTime.Now;

        // The track layout name
        public string LayoutName { get; set; } = string.Empty;

        // The track pieces — stored as a list of base type
        // TypeNameHandling in the serialiser handles the polymorphism
        public List<TrackPieceSaveData> TrackPieces { get; set; } = new();

        // Locomotives assigned to this layout
        public List<Locomotive> AssignedLocomotives { get; set; } = new();
    }

    // A flat serialisable version of a track piece
    // We use this rather than serialising the abstract class directly
    public class TrackPieceSaveData
    {
        // Which concrete type this is
        public string PieceTypeName { get; set; } = string.Empty;

        // Common properties
        public double StartX { get; set; }
        public double StartY { get; set; }
        public double StartAngle { get; set; }
        public int BlockId { get; set; }
        public bool IsOccupied { get; set; }
        public string PieceType { get; set; } = string.Empty;

        // Straight / accessory specific
        public double LengthMm { get; set; }

        // Curve specific
        public double RadiusMm { get; set; }
        public double AngleDegrees { get; set; }
        public string CurveDirection { get; set; } = string.Empty;

        // Points specific
        public string PointsDirection { get; set; } = string.Empty;
        public string PointsState { get; set; } = string.Empty;
        public bool IsCurved { get; set; }
    }
}