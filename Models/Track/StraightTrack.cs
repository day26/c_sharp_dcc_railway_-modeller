using System.Windows;

namespace DccController.Models.Track
{
    public class StraightTrack : TrackPiece
    {
        public double LengthMm { get; set; } = TrackConstants.FullStraightMm;

        private double LengthPx => LengthMm * TrackConstants.PixelsPerMm;

        public StraightTrack()
        {
            // Default to full straight — can be overridden
            PieceType = TrackPieceType.FullStraight;
        }

        public override Point EndPoint
        {
            get
            {
                double rad = StartAngle * Math.PI / 180.0;
                return new Point(
                    StartPoint.X + LengthPx * Math.Cos(rad),
                    StartPoint.Y + LengthPx * Math.Sin(rad));
            }
        }

        public override double EndAngle => StartAngle;
    }
}