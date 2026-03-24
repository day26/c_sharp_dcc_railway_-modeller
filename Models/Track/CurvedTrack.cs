using System.Windows;

namespace DccController.Models.Track
{
    public enum CurveDirection { Left, Right }

    public class CurvedTrack : TrackPiece
    {
        public double RadiusMm { get; set; } = TrackConstants.Radius2Mm;
        public double AngleDegrees { get; set; } = 45.0;
        public CurveDirection Direction { get; set; } = CurveDirection.Right;

        private double RadiusPx => RadiusMm * TrackConstants.PixelsPerMm;

        public CurvedTrack()
        {
            // Default to radius 2 — can be overridden
            PieceType = TrackPieceType.Radius2Curve;
        }

        public Point Center
        {
            get
            {
                double h = StartAngle * Math.PI / 180.0;
                if (Direction == CurveDirection.Right)
                {
                    return new Point(
                        StartPoint.X + RadiusPx * (-Math.Sin(h)),
                        StartPoint.Y + RadiusPx * Math.Cos(h));
                }
                else
                {
                    return new Point(
                        StartPoint.X + RadiusPx * Math.Sin(h),
                        StartPoint.Y + RadiusPx * (-Math.Cos(h)));
                }
            }
        }

        public override Point EndPoint
        {
            get
            {
                var c = Center;
                double startAngleFromCenter = Math.Atan2(
                    StartPoint.Y - c.Y,
                    StartPoint.X - c.X);
                double sign = Direction == CurveDirection.Right ? 1 : -1;
                double endAngleFromCenter = startAngleFromCenter +
                    sign * AngleDegrees * Math.PI / 180.0;
                return new Point(
                    c.X + RadiusPx * Math.Cos(endAngleFromCenter),
                    c.Y + RadiusPx * Math.Sin(endAngleFromCenter));
            }
        }

        public override double EndAngle
        {
            get
            {
                if (Direction == CurveDirection.Right)
                    return StartAngle + AngleDegrees;
                else
                    return StartAngle - AngleDegrees;
            }
        }
    }
}