using System.Windows;
using DccController.Models.Track;

namespace DccController.Services
{
    public class TrackLayoutService
    {
        // ── Build a simple oval example layout ───────────────────────
        public TrackLayout CreateExampleLayout()
        {
            var layout = new TrackLayout { Name = "Example Oval Layout" };

            // We'll build a simple oval:
            // - Two straight sections on top and bottom
            // - Four curves at each end (4 x 45 degrees = 180 degrees)
            // Starting at top left, heading right (angle = 0)

            // Centre the oval layout on the canvas
            // Oval width  = 3 x full straight in pixels
            // Oval height = 2 x Radius2 in pixels
            double straightPx = TrackConstants.FullStraightMm *
                TrackConstants.PixelsPerMm;
            double radiusPx = TrackConstants.Radius2Mm *
                TrackConstants.PixelsPerMm;

            double ovalWidth = straightPx * 3;
            double ovalHeight = radiusPx * 2;

            double startX = TrackConstants.CanvasCentreX - ovalWidth / 2.0;
            double startY = TrackConstants.CanvasCentreY - ovalHeight / 2.0;

            // ── Top straight sections ────────────────────────────────
            var topStraight1 = new StraightTrack
            {
                StartPoint = new Point(startX, startY),
                StartAngle = 0,
                LengthMm = TrackConstants.FullStraightMm
            };
            layout.Pieces.Add(topStraight1);

            var topStraight2 = new StraightTrack
            {
                StartPoint = topStraight1.EndPoint,
                StartAngle = topStraight1.EndAngle,
                LengthMm = TrackConstants.FullStraightMm
            };
            layout.Pieces.Add(topStraight2);

            var topStraight3 = new StraightTrack
            {
                StartPoint = topStraight2.EndPoint,
                StartAngle = topStraight2.EndAngle,
                LengthMm = TrackConstants.FullStraightMm
            };
            layout.Pieces.Add(topStraight3);

            // ── Right hand curves (4 x 45 degrees) ──────────────────
            var curve1 = new CurvedTrack
            {
                StartPoint = topStraight3.EndPoint,
                StartAngle = topStraight3.EndAngle,
                RadiusMm = TrackConstants.Radius2Mm,
                AngleDegrees = 45.0,
                Direction = CurveDirection.Right
            };
            layout.Pieces.Add(curve1);

            var curve2 = new CurvedTrack
            {
                StartPoint = curve1.EndPoint,
                StartAngle = curve1.EndAngle,
                RadiusMm = TrackConstants.Radius2Mm,
                AngleDegrees = 45.0,
                Direction = CurveDirection.Right
            };
            layout.Pieces.Add(curve2);

            var curve3 = new CurvedTrack
            {
                StartPoint = curve2.EndPoint,
                StartAngle = curve2.EndAngle,
                RadiusMm = TrackConstants.Radius2Mm,
                AngleDegrees = 45.0,
                Direction = CurveDirection.Right
            };
            layout.Pieces.Add(curve3);

            var curve4 = new CurvedTrack
            {
                StartPoint = curve3.EndPoint,
                StartAngle = curve3.EndAngle,
                RadiusMm = TrackConstants.Radius2Mm,
                AngleDegrees = 45.0,
                Direction = CurveDirection.Right
            };
            layout.Pieces.Add(curve4);

            // ── Bottom straight sections ─────────────────────────────
            var bottomStraight1 = new StraightTrack
            {
                StartPoint = curve4.EndPoint,
                StartAngle = curve4.EndAngle,
                LengthMm = TrackConstants.FullStraightMm
            };
            layout.Pieces.Add(bottomStraight1);

            var bottomStraight2 = new StraightTrack
            {
                StartPoint = bottomStraight1.EndPoint,
                StartAngle = bottomStraight1.EndAngle,
                LengthMm = TrackConstants.FullStraightMm
            };
            layout.Pieces.Add(bottomStraight2);

            var bottomStraight3 = new StraightTrack
            {
                StartPoint = bottomStraight2.EndPoint,
                StartAngle = bottomStraight2.EndAngle,
                LengthMm = TrackConstants.FullStraightMm
            };
            layout.Pieces.Add(bottomStraight3);

            // ── Left hand curves (4 x 45 degrees) ───────────────────
            var curve5 = new CurvedTrack
            {
                StartPoint = bottomStraight3.EndPoint,
                StartAngle = bottomStraight3.EndAngle,
                RadiusMm = TrackConstants.Radius2Mm,
                AngleDegrees = 45.0,
                Direction = CurveDirection.Right
            };
            layout.Pieces.Add(curve5);

            var curve6 = new CurvedTrack
            {
                StartPoint = curve5.EndPoint,
                StartAngle = curve5.EndAngle,
                RadiusMm = TrackConstants.Radius2Mm,
                AngleDegrees = 45.0,
                Direction = CurveDirection.Right
            };
            layout.Pieces.Add(curve6);

            var curve7 = new CurvedTrack
            {
                StartPoint = curve6.EndPoint,
                StartAngle = curve6.EndAngle,
                RadiusMm = TrackConstants.Radius2Mm,
                AngleDegrees = 45.0,
                Direction = CurveDirection.Right
            };
            layout.Pieces.Add(curve7);

            var curve8 = new CurvedTrack
            {
                StartPoint = curve7.EndPoint,
                StartAngle = curve7.EndAngle,
                RadiusMm = TrackConstants.Radius2Mm,
                AngleDegrees = 45.0,
                Direction = CurveDirection.Right
            };
            layout.Pieces.Add(curve8);

            return layout;
        }
    }
}