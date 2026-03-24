namespace DccController.Models.Track
{
    public static class TrackConstants
    {
        // ── Scale ────────────────────────────────────────────────────
        public const double PixelsPerMm = 0.667;

        // ── OO Gauge ─────────────────────────────────────────────────
        public const double GaugeMm = 16.5;
        public static readonly double GaugePx = GaugeMm * PixelsPerMm;

        // ── Sleepers ─────────────────────────────────────────────────
        public const double SleeperSpacingMm = 12.0;
        public static readonly double SleeperSpacingPx =
            SleeperSpacingMm * PixelsPerMm;
        public const double SleeperOverhangMm = 5.0;
        public static readonly double SleeperOverhangPx =
            SleeperOverhangMm * PixelsPerMm;
        public const double RailWidthPx = 1.5;

        // ── Hornby Straight Lengths (mm) ─────────────────────────────
        public const double ThirdStraightMm = 55.0;
        public const double HalfStraightMm = 83.0;
        public const double FullStraightMm = 168.0;
        public const double DoubleStraightMm = 336.0;

        // ── Hornby Curve Radii (mm) ───────────────────────────────────
        public const double Radius1Mm = 371.0;
        public const double Radius2Mm = 438.0;
        public const double Radius3Mm = 505.0;
        public const double Radius4Mm = 572.0;

        // ── Hornby Curve Angles (degrees) ────────────────────────────
        public const double StandardCurveAngle = 22.5;
        public const double DoubleCurveAngle = 45.0;

        // ── Points (mm) ──────────────────────────────────────────────
        // Hornby standard points are approx 228mm long
        public const double StandardPointLengthMm = 228.0;
        // Diverging angle for standard points
        public const double StandardPointAngle = 15.0;
        // Curved points follow radius 1 geometry
        public const double CurvedPointRadiusMm = Radius1Mm;
        public const double CurvedPointAngle = 22.5;

        // ── Crossings (mm) ───────────────────────────────────────────
        public const double DiamondCrossingLengthMm = 168.0;
        public const double DiamondCrossingAngle = 45.0;

        // ── Accessories (mm) ─────────────────────────────────────────
        public const double BufferStopLengthMm = 59.0;
        public const double UncouplingTrackLengthMm = 83.0;
        public const double PowerTrackLengthMm = 168.0;

        // ── Canvas ───────────────────────────────────────────────────
        public const double CanvasWidth = 5000.0;
        public const double CanvasHeight = 4000.0;
        public const double CanvasCentreX = CanvasWidth / 2.0;
        public const double CanvasCentreY = CanvasHeight / 2.0;
    }
}