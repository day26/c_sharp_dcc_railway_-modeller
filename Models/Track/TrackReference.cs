namespace DccController.Models.Track
{
    // Add new manufacturers here as needed in the future
    public enum TrackManufacturer
    {
        Hornby,
        Peco,
        Bachmann
    }

    // Describes what type of track piece this is
    public enum TrackPieceType
    {
        FullStraight,
        HalfStraight,
        Radius1Curve,
        Radius2Curve,
        Radius3Curve,
        Radius4Curve,
        Unknown
    }

    public class TrackReference
    {
        public TrackManufacturer Manufacturer { get; set; }
        public TrackPieceType PieceType { get; set; }
        public string ReferenceCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}