using System.Windows;

namespace DccController.Models.Track
{
    // Covers uncoupling track and power track — both behave like straights
    public class AccessoryTrack : TrackPiece
    {
        public double LengthMm { get; set; } =
            TrackConstants.UncouplingTrackLengthMm;

        private double LengthPx => LengthMm * TrackConstants.PixelsPerMm;

        public AccessoryTrack()
        {
            PieceType = TrackPieceType.UncouplingTrack;
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