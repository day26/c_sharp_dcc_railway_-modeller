namespace DccController.Models.Track
{
    public enum TrackManufacturer
    {
        Hornby,
        Peco,
        Bachmann
    }

    public enum TrackPieceType
    {
        // ── Straights ────────────────────────────────────────────────
        ThirdStraight,
        HalfStraight,
        FullStraight,
        DoubleStraight,

        // ── Curves ───────────────────────────────────────────────────
        Radius1Curve,
        Radius1DoubleCurve,
        Radius2Curve,
        Radius2DoubleCurve,
        Radius3Curve,
        Radius3DoubleCurve,
        Radius4Curve,
        Radius4DoubleCurve,

        // ── Points ───────────────────────────────────────────────────
        PointsLeftHand,
        PointsRightHand,
        CurvedPointsLeftHand,
        CurvedPointsRightHand,

        // ── Crossings ────────────────────────────────────────────────
        DiamondCrossing,
        SingleSlipLeft,
        SingleSlipRight,
        DoubleSlip,

        // ── Accessories ──────────────────────────────────────────────
        BufferStop,
        UncouplingTrack,
        PowerTrack,

        Unknown
    }

    public enum TrackCategory
    {
        Straights,
        Curves,
        Points,
        Crossings,
        Accessories
    }

    public class TrackReference
    {
        public TrackManufacturer Manufacturer { get; set; }
        public TrackPieceType PieceType { get; set; }
        public TrackCategory Category { get; set; }
        public string ReferenceCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DetailedDescription { get; set; } = string.Empty;
    }
}