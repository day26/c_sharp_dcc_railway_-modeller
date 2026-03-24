using System.IO;
using System.Windows;
using DccController.Models;
using DccController.Models.Track;
using Newtonsoft.Json;

namespace DccController.Services
{
    public class LayoutSaveService
    {
        // ── Save Layout ──────────────────────────────────────────────
        public void SaveLayout(string filePath,
            TrackLayout layout,
            List<Locomotive> assignedLocomotives)
        {
            var saveFile = new LayoutSaveFile
            {
                LayoutName = layout.Name,
                SavedAt = DateTime.Now,
                AssignedLocomotives = assignedLocomotives,
                TrackPieces = layout.Pieces
                    .Select(p => SerialiseTrackPiece(p))
                    .ToList()
            };

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };

            string json = JsonConvert.SerializeObject(saveFile, settings);
            File.WriteAllText(filePath, json);
        }

        // ── Load Layout ──────────────────────────────────────────────
        public (TrackLayout layout, List<Locomotive> locomotives)
            LoadLayout(string filePath)
        {
            string json = File.ReadAllText(filePath);

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };

            var saveFile = JsonConvert.DeserializeObject<LayoutSaveFile>(
                json, settings);

            if (saveFile == null)
                throw new Exception("Failed to deserialise layout file.");

            var layout = new TrackLayout
            {
                Name = saveFile.LayoutName,
                Pieces = saveFile.TrackPieces
                    .Select(p => DeserialiseTrackPiece(p))
                    .Where(p => p != null)
                    .Cast<TrackPiece>()
                    .ToList()
            };

            return (layout, saveFile.AssignedLocomotives ?? new());
        }

        // ── Serialise a single track piece ───────────────────────────
        private TrackPieceSaveData SerialiseTrackPiece(TrackPiece piece)
        {
            var data = new TrackPieceSaveData
            {
                PieceTypeName = piece.GetType().Name,
                StartX = piece.StartPoint.X,
                StartY = piece.StartPoint.Y,
                StartAngle = piece.StartAngle,
                BlockId = piece.BlockId,
                IsOccupied = piece.IsOccupied,
                PieceType = piece.PieceType.ToString()
            };

            switch (piece)
            {
                case StraightTrack straight:
                    data.LengthMm = straight.LengthMm;
                    break;

                case CurvedTrack curve:
                    data.RadiusMm = curve.RadiusMm;
                    data.AngleDegrees = curve.AngleDegrees;
                    data.CurveDirection = curve.Direction.ToString();
                    break;

                case PointsTrack points:
                    data.PointsDirection = points.PointsDirection.ToString();
                    data.PointsState = points.State.ToString();
                    data.IsCurved = points.IsCurved;
                    break;

                case BufferStopTrack _:
                    // No extra properties needed
                    break;

                case AccessoryTrack accessory:
                    data.LengthMm = accessory.LengthMm;
                    break;
            }

            return data;
        }

        // ── Deserialise a single track piece ─────────────────────────
        private TrackPiece? DeserialiseTrackPiece(TrackPieceSaveData data)
        {
            var origin = new Point(data.StartX, data.StartY);

            Enum.TryParse<TrackPieceType>(data.PieceType,
                out TrackPieceType pieceType);

            switch (data.PieceTypeName)
            {
                case nameof(StraightTrack):
                    return new StraightTrack
                    {
                        StartPoint = origin,
                        StartAngle = data.StartAngle,
                        BlockId = data.BlockId,
                        IsOccupied = data.IsOccupied,
                        PieceType = pieceType,
                        LengthMm = data.LengthMm
                    };

                case nameof(CurvedTrack):
                    Enum.TryParse<CurveDirection>(data.CurveDirection,
                        out CurveDirection curveDir);
                    return new CurvedTrack
                    {
                        StartPoint = origin,
                        StartAngle = data.StartAngle,
                        BlockId = data.BlockId,
                        IsOccupied = data.IsOccupied,
                        PieceType = pieceType,
                        RadiusMm = data.RadiusMm,
                        AngleDegrees = data.AngleDegrees,
                        Direction = curveDir
                    };

                case nameof(PointsTrack):
                    Enum.TryParse<PointsDirection>(data.PointsDirection,
                        out PointsDirection pointsDir);
                    Enum.TryParse<PointsState>(data.PointsState,
                        out PointsState pointsState);
                    return new PointsTrack
                    {
                        StartPoint = origin,
                        StartAngle = data.StartAngle,
                        BlockId = data.BlockId,
                        IsOccupied = data.IsOccupied,
                        PieceType = pieceType,
                        PointsDirection = pointsDir,
                        State = pointsState,
                        IsCurved = data.IsCurved
                    };

                case nameof(BufferStopTrack):
                    return new BufferStopTrack
                    {
                        StartPoint = origin,
                        StartAngle = data.StartAngle,
                        BlockId = data.BlockId,
                        IsOccupied = data.IsOccupied
                    };

                case nameof(AccessoryTrack):
                    return new AccessoryTrack
                    {
                        StartPoint = origin,
                        StartAngle = data.StartAngle,
                        BlockId = data.BlockId,
                        IsOccupied = data.IsOccupied,
                        PieceType = pieceType,
                        LengthMm = data.LengthMm
                    };

                default:
                    return null;
            }
        }
    }
}