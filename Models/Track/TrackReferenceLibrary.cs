namespace DccController.Models.Track
{
    public static class TrackReferenceLibrary
    {
        // Master list of all known reference codes
        // Add new manufacturers and codes here as needed
        private static readonly List<TrackReference> _references = new()
        {
            // ── Hornby ───────────────────────────────────────────────
            new TrackReference
            {
                Manufacturer = TrackManufacturer.Hornby,
                PieceType = TrackPieceType.FullStraight,
                ReferenceCode = "R603",
                Description = "Long Straight"
            },
            new TrackReference
            {
                Manufacturer = TrackManufacturer.Hornby,
                PieceType = TrackPieceType.HalfStraight,
                ReferenceCode = "R600",
                Description = "Short Straight"
            },
            new TrackReference
            {
                Manufacturer = TrackManufacturer.Hornby,
                PieceType = TrackPieceType.Radius1Curve,
                ReferenceCode = "R605",
                Description = "1st Radius Double Curve"
            },
            new TrackReference
            {
                Manufacturer = TrackManufacturer.Hornby,
                PieceType = TrackPieceType.Radius2Curve,
                ReferenceCode = "R607",
                Description = "2nd Radius Double Curve"
            },

            // ── Peco (placeholders for future use) ───────────────────
            new TrackReference
            {
                Manufacturer = TrackManufacturer.Peco,
                PieceType = TrackPieceType.FullStraight,
                ReferenceCode = "SL-100",
                Description = "Standard Straight"
            },
        };

        // ── Look up a reference code ─────────────────────────────────
        public static TrackReference? GetReference(
            TrackManufacturer manufacturer,
            TrackPieceType pieceType)
        {
            return _references.FirstOrDefault(r =>
                r.Manufacturer == manufacturer &&
                r.PieceType == pieceType);
        }

        // ── Get all references for a manufacturer ────────────────────
        public static List<TrackReference> GetAllForManufacturer(
            TrackManufacturer manufacturer)
        {
            return _references
                .Where(r => r.Manufacturer == manufacturer)
                .ToList();
        }
    }
}