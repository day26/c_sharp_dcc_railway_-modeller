using System.Windows;

namespace DccController.Models.Track
{
    public enum PointsDirection { Left, Right }
    public enum PointsState { Straight, Diverging }

    public class PointsTrack : TrackPiece
    {
        public PointsDirection PointsDirection { get; set; }
        public PointsState State { get; set; } = PointsState.Straight;
        public bool IsCurved { get; set; } = false;

        private double LengthPx =>
            TrackConstants.StandardPointLengthMm * TrackConstants.PixelsPerMm;

        public PointsTrack()
        {
            PieceType = TrackPieceType.PointsLeftHand;
        }

        // The straight-ahead end point
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

        // The diverging end point
        public Point DivergingEndPoint
        {
            get
            {
                double divergeAngle = PointsDirection == PointsDirection.Left
                    ? StartAngle - TrackConstants.StandardPointAngle
                    : StartAngle + TrackConstants.StandardPointAngle;

                double rad = divergeAngle * Math.PI / 180.0;
                return new Point(
                    StartPoint.X + LengthPx * Math.Cos(rad),
                    StartPoint.Y + LengthPx * Math.Sin(rad));
            }
        }

        public double DivergingEndAngle =>
            PointsDirection == PointsDirection.Left
                ? StartAngle - TrackConstants.StandardPointAngle
                : StartAngle + TrackConstants.StandardPointAngle;
    }
}