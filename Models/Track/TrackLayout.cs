namespace DccController.Models.Track
{
    public class TrackLayout
    {
        public string Name { get; set; } = "My Layout";
        public List<TrackPiece> Pieces { get; set; } = new List<TrackPiece>();
    }
}