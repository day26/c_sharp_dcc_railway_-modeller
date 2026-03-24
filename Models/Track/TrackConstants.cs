namespace DccController.Models.Track
{
    public static class TrackConstants
    {
        // Scale: pixels per mm
        public const double PixelsPerMm = 0.667;

        // OO Gauge track measurements
        public const double GaugeMm = 16.5;
        public static readonly double GaugePx = GaugeMm * PixelsPerMm;

        // Sleeper spacing and size
        public const double SleeperSpacingMm = 12.0;
        public static readonly double SleeperSpacingPx =
            SleeperSpacingMm * PixelsPerMm;
        public const double SleeperOverhangMm = 5.0;
        public static readonly double SleeperOverhangPx =
            SleeperOverhangMm * PixelsPerMm;

        // Rail drawing width in pixels
        public const double RailWidthPx = 1.5;

        // Hornby standard track piece dimensions (mm)
        public const double FullStraightMm = 168.0;
        public const double HalfStraightMm = 84.0;
        public const double Radius1Mm = 371.0;
        public const double Radius2Mm = 438.0;
        public const double Radius3Mm = 505.0;
        public const double Radius4Mm = 572.0;
        public const double StandardCurveAngle = 22.5;

        // Canvas dimensions — large so the user can work outwards
        public const double CanvasWidth = 5000.0;
        public const double CanvasHeight = 4000.0;

        // Centre of the canvas — this is the reference origin point
        public const double CanvasCentreX = CanvasWidth / 2.0;
        public const double CanvasCentreY = CanvasHeight / 2.0;
    }
}