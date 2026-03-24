using System.Windows;

namespace DccController.Models.Track
{
    public class BufferStopTrack : TrackPiece
    {
        private double LengthPx =>
            TrackConstants.BufferStopLengthMm * TrackConstants.PixelsPerMm;

        public BufferStopTrack()
        {
            PieceType = TrackPieceType.BufferStop;
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