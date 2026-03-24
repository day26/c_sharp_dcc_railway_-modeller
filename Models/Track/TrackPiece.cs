using System.Windows;
using DccController.Models.Track;

namespace DccController.Models.Track
{
    public abstract class TrackPiece
    {
        // Where this piece starts and what direction it faces
        public Point StartPoint { get; set; }
        public double StartAngle { get; set; }

        // Calculated end point and angle
        public abstract Point EndPoint { get; }
        public abstract double EndAngle { get; }

        // Block detection
        public int BlockId { get; set; } = 0;
        public bool IsOccupied { get; set; } = false;

        // Reference code info
        public TrackPieceType PieceType { get; set; } = TrackPieceType.Unknown;

        // Get the reference code for a given manufacturer
        public string GetReferenceCode(TrackManufacturer manufacturer)
        {
            var reference = TrackReferenceLibrary.GetReference(
                manufacturer, PieceType);
            return reference?.ReferenceCode ?? "—";
        }

        // Get the description for a given manufacturer
        public string GetDescription(TrackManufacturer manufacturer)
        {
            var reference = TrackReferenceLibrary.GetReference(
                manufacturer, PieceType);
            return reference?.Description ?? "Unknown";
        }
    }
}