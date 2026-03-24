namespace DccController.Models.Track
{
    public static class TrackReferenceLibrary
    {
        private static readonly List<TrackReference> _references = new()
        {
            // ── HORNBY STRAIGHTS ─────────────────────────────────────
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Straights,
                PieceType        = TrackPieceType.ThirdStraight,
                ReferenceCode    = "R601",
                Description      = "1/3rd Straight",
                DetailedDescription = "55mm short filler straight"
            },
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Straights,
                PieceType        = TrackPieceType.HalfStraight,
                ReferenceCode    = "R600",
                Description      = "Short Straight",
                DetailedDescription = "83mm half straight"
            },
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Straights,
                PieceType        = TrackPieceType.FullStraight,
                ReferenceCode    = "R603",
                Description      = "Standard Straight",
                DetailedDescription = "168mm full straight"
            },
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Straights,
                PieceType        = TrackPieceType.DoubleStraight,
                ReferenceCode    = "R604",
                Description      = "Double Straight",
                DetailedDescription = "336mm double length straight"
            },

            // ── HORNBY CURVES ────────────────────────────────────────
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Curves,
                PieceType        = TrackPieceType.Radius1Curve,
                ReferenceCode    = "R606",
                Description      = "1st Radius Curve",
                DetailedDescription = "371mm radius, 22.5° arc"
            },
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Curves,
                PieceType        = TrackPieceType.Radius1DoubleCurve,
                ReferenceCode    = "R605",
                Description      = "1st Radius Double Curve",
                DetailedDescription = "371mm radius, 45° arc"
            },
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Curves,
                PieceType        = TrackPieceType.Radius2Curve,
                ReferenceCode    = "R608",
                Description      = "2nd Radius Curve",
                DetailedDescription = "438mm radius, 22.5° arc"
            },
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Curves,
                PieceType        = TrackPieceType.Radius2DoubleCurve,
                ReferenceCode    = "R607",
                Description      = "2nd Radius Double Curve",
                DetailedDescription = "438mm radius, 45° arc"
            },
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Curves,
                PieceType        = TrackPieceType.Radius3Curve,
                ReferenceCode    = "R609",
                Description      = "3rd Radius Curve",
                DetailedDescription = "505mm radius, 22.5° arc"
            },
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Curves,
                PieceType        = TrackPieceType.Radius3DoubleCurve,
                ReferenceCode    = "R8262",
                Description      = "3rd Radius Double Curve",
                DetailedDescription = "505mm radius, 45° arc"
            },
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Curves,
                PieceType        = TrackPieceType.Radius4Curve,
                ReferenceCode    = "R610",
                Description      = "4th Radius Curve",
                DetailedDescription = "572mm radius, 22.5° arc"
            },
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Curves,
                PieceType        = TrackPieceType.Radius4DoubleCurve,
                ReferenceCode    = "R8261",
                Description      = "4th Radius Double Curve",
                DetailedDescription = "572mm radius, 45° arc"
            },

            // ── HORNBY POINTS ────────────────────────────────────────
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Points,
                PieceType        = TrackPieceType.PointsLeftHand,
                ReferenceCode    = "R8077",
                Description      = "Left Hand Point",
                DetailedDescription = "Standard left hand turnout"
            },
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Points,
                PieceType        = TrackPieceType.PointsRightHand,
                ReferenceCode    = "R8078",
                Description      = "Right Hand Point",
                DetailedDescription = "Standard right hand turnout"
            },
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Points,
                PieceType        = TrackPieceType.CurvedPointsLeftHand,
                ReferenceCode    = "R8072",
                Description      = "Left Hand Curved Point",
                DetailedDescription = "Curved left hand turnout, R1 geometry"
            },
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Points,
                PieceType        = TrackPieceType.CurvedPointsRightHand,
                ReferenceCode    = "R8073",
                Description      = "Right Hand Curved Point",
                DetailedDescription = "Curved right hand turnout, R1 geometry"
            },

            // ── HORNBY CROSSINGS ─────────────────────────────────────
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Crossings,
                PieceType        = TrackPieceType.DiamondCrossing,
                ReferenceCode    = "R615",
                Description      = "Diamond Crossing",
                DetailedDescription = "45° diamond crossing"
            },
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Crossings,
                PieceType        = TrackPieceType.SingleSlipLeft,
                ReferenceCode    = "R616",
                Description      = "Single Slip Left",
                DetailedDescription = "Left hand single slip"
            },
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Crossings,
                PieceType        = TrackPieceType.SingleSlipRight,
                ReferenceCode    = "R617",
                Description      = "Single Slip Right",
                DetailedDescription = "Right hand single slip"
            },
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Crossings,
                PieceType        = TrackPieceType.DoubleSlip,
                ReferenceCode    = "R618",
                Description      = "Double Slip",
                DetailedDescription = "Full double slip crossing"
            },

            // ── HORNBY ACCESSORIES ───────────────────────────────────
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Accessories,
                PieceType        = TrackPieceType.BufferStop,
                ReferenceCode    = "R619",
                Description      = "Buffer Stop",
                DetailedDescription = "Standard buffer stop, 59mm"
            },
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Accessories,
                PieceType        = TrackPieceType.UncouplingTrack,
                ReferenceCode    = "R8241",
                Description      = "Uncoupling Track",
                DetailedDescription = "Electromagnetic uncoupler track"
            },
            new TrackReference
            {
                Manufacturer     = TrackManufacturer.Hornby,
                Category         = TrackCategory.Accessories,
                PieceType        = TrackPieceType.PowerTrack,
                ReferenceCode    = "R8206",
                Description      = "Power Connecting Track",
                DetailedDescription = "Track with power feed connections"
            },
        };

        // ── Lookup methods ───────────────────────────────────────────
        public static TrackReference? GetReference(
            TrackManufacturer manufacturer, TrackPieceType pieceType)
        {
            return _references.FirstOrDefault(r =>
                r.Manufacturer == manufacturer &&
                r.PieceType == pieceType);
        }

        public static List<TrackReference> GetAllForManufacturer(
            TrackManufacturer manufacturer)
        {
            return _references
                .Where(r => r.Manufacturer == manufacturer)
                .ToList();
        }

        public static List<TrackReference> GetByCategory(
            TrackManufacturer manufacturer, TrackCategory category)
        {
            return _references
                .Where(r => r.Manufacturer == manufacturer &&
                            r.Category == category)
                .ToList();
        }

        public static List<TrackCategory> GetCategories(
            TrackManufacturer manufacturer)
        {
            return _references
                .Where(r => r.Manufacturer == manufacturer)
                .Select(r => r.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
        }
    }
}