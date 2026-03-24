using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using DccController.Models.Track;

namespace DccController.Services
{
    public class TrackRenderer
    {
        // ── Settings ─────────────────────────────────────────────────
        public bool ShowStartEndPoints { get; set; } = true;
        public bool ShowReferenceOverlay { get; set; } = false;
        public TrackManufacturer OverlayManufacturer { get; set; } =
            TrackManufacturer.Hornby;

        // ── Colours ───────────────────────────────────────────────────
        private static readonly Brush RailBrush =
            new SolidColorBrush(Color.FromRgb(180, 180, 180));
        private static readonly Brush SleeperBrush =
            new SolidColorBrush(Color.FromRgb(101, 67, 33));
        private static readonly Brush OccupiedBrush =
            new SolidColorBrush(Color.FromRgb(231, 76, 60));
        private static readonly Brush StartPointBrush =
            new SolidColorBrush(Color.FromRgb(46, 204, 113));
        private static readonly Brush EndPointBrush =
            new SolidColorBrush(Color.FromRgb(52, 152, 219));
        private static readonly Brush OverlayBrush =
            new SolidColorBrush(Color.FromRgb(255, 220, 50));

        // ── Main render entry point ──────────────────────────────────
        public void RenderLayout(Canvas canvas, TrackLayout layout)
        {
            canvas.Children.Clear();

            // Always draw the centre crosshair first
            DrawCentreMarker(canvas);

            foreach (var piece in layout.Pieces)
            {
                if (piece is StraightTrack straight)
                    DrawStraight(canvas, straight);
                else if (piece is CurvedTrack curve)
                    DrawCurve(canvas, curve);

                if (ShowStartEndPoints)
                    DrawStartEndPoints(canvas, piece);

                if (ShowReferenceOverlay)
                    DrawReferenceOverlay(canvas, piece);
            }
        }

        // ── Draw the canvas centre marker ────────────────────────────
        private void DrawCentreMarker(Canvas canvas)
        {
            double cx = TrackConstants.CanvasCentreX;
            double cy = TrackConstants.CanvasCentreY;
            const double armLength = 20.0;
            const double gapSize = 4.0;

            var brush = new SolidColorBrush(
                Color.FromArgb(80, 255, 255, 255));

            // Horizontal arms (left and right of centre gap)
            DrawLine(canvas,
                new Point(cx - armLength, cy),
                new Point(cx - gapSize, cy),
                brush, 1.0);
            DrawLine(canvas,
                new Point(cx + gapSize, cy),
                new Point(cx + armLength, cy),
                brush, 1.0);

            // Vertical arms (above and below centre gap)
            DrawLine(canvas,
                new Point(cx, cy - armLength),
                new Point(cx, cy - gapSize),
                brush, 1.0);
            DrawLine(canvas,
                new Point(cx, cy + gapSize),
                new Point(cx, cy + armLength),
                brush, 1.0);

            // Small centre dot
            var dot = new System.Windows.Shapes.Ellipse
            {
                Width = 4,
                Height = 4,
                Fill = new SolidColorBrush(
                    Color.FromArgb(80, 255, 255, 255))
            };
            Canvas.SetLeft(dot, cx - 2);
            Canvas.SetTop(dot, cy - 2);
            canvas.Children.Add(dot);

            // "Centre" label
            var label = new System.Windows.Controls.TextBlock
            {
                Text = "centre",
                Foreground = new SolidColorBrush(
                    Color.FromArgb(60, 255, 255, 255)),
                FontSize = 9
            };
            Canvas.SetLeft(label, cx + armLength + 4);
            Canvas.SetTop(label, cy - 7);
            canvas.Children.Add(label);
        }

        // ── Draw a straight piece ────────────────────────────────────
        private void DrawStraight(Canvas canvas, StraightTrack track)
        {
            double angleRad = track.StartAngle * Math.PI / 180.0;
            double halfGauge = TrackConstants.GaugePx / 2.0;

            double perpX = -Math.Sin(angleRad);
            double perpY = Math.Cos(angleRad);

            var leftStart = new Point(
                track.StartPoint.X + perpX * halfGauge,
                track.StartPoint.Y + perpY * halfGauge);
            var leftEnd = new Point(
                track.EndPoint.X + perpX * halfGauge,
                track.EndPoint.Y + perpY * halfGauge);
            var rightStart = new Point(
                track.StartPoint.X - perpX * halfGauge,
                track.StartPoint.Y - perpY * halfGauge);
            var rightEnd = new Point(
                track.EndPoint.X - perpX * halfGauge,
                track.EndPoint.Y - perpY * halfGauge);

            var railBrush = track.IsOccupied ? OccupiedBrush : RailBrush;

            DrawSleepers(canvas, track.StartPoint, track.EndPoint,
                angleRad, track.IsOccupied);
            DrawLine(canvas, leftStart, leftEnd, railBrush,
                TrackConstants.RailWidthPx);
            DrawLine(canvas, rightStart, rightEnd, railBrush,
                TrackConstants.RailWidthPx);
        }

        // ── Draw a curved piece ──────────────────────────────────────
        private void DrawCurve(Canvas canvas, CurvedTrack track)
        {
            var center = track.Center;
            double radiusPx = track.RadiusMm * TrackConstants.PixelsPerMm;
            double halfGauge = TrackConstants.GaugePx / 2.0;

            var railBrush = track.IsOccupied ? OccupiedBrush : RailBrush;

            DrawCurveSleepers(canvas, track);

            DrawArc(canvas, center, radiusPx - halfGauge,
                track.StartPoint, track.AngleDegrees,
                track.Direction, railBrush);
            DrawArc(canvas, center, radiusPx + halfGauge,
                track.StartPoint, track.AngleDegrees,
                track.Direction, railBrush);
        }

        // ── Draw start and end point indicators ──────────────────────
        private void DrawStartEndPoints(Canvas canvas, TrackPiece piece)
        {
            const double dotRadius = 3.0;

            // Green dot at start
            DrawDot(canvas, piece.StartPoint, dotRadius, StartPointBrush);

            // Blue dot at end
            DrawDot(canvas, piece.EndPoint, dotRadius, EndPointBrush);
        }

        private void DrawDot(Canvas canvas, Point centre,
            double radius, Brush brush)
        {
            var ellipse = new Ellipse
            {
                Width = radius * 2,
                Height = radius * 2,
                Fill = brush,
                Opacity = 0.85
            };
            Canvas.SetLeft(ellipse, centre.X - radius);
            Canvas.SetTop(ellipse, centre.Y - radius);
            canvas.Children.Add(ellipse);
        }

        // ── Draw reference code overlay ──────────────────────────────
        private void DrawReferenceOverlay(Canvas canvas, TrackPiece piece)
        {
            string code = piece.GetReferenceCode(OverlayManufacturer);
            if (code == "—") return;

            // Position the label at the midpoint of the piece
            Point midPoint = GetMidPoint(piece);

            var border = new Border
            {
                Background = new SolidColorBrush(
                    Color.FromArgb(200, 30, 30, 46)),
                BorderBrush = OverlayBrush,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(3),
                Padding = new Thickness(3, 1, 3, 1)
            };

            var text = new TextBlock
            {
                Text = code,
                Foreground = OverlayBrush,
                FontSize = 8,
                FontWeight = FontWeights.Bold
            };

            border.Child = text;

            // Measure so we can centre it
            border.Measure(new Size(double.PositiveInfinity,
                double.PositiveInfinity));
            double w = border.DesiredSize.Width;
            double h = border.DesiredSize.Height;

            Canvas.SetLeft(border, midPoint.X - w / 2);
            Canvas.SetTop(border, midPoint.Y - h / 2);
            canvas.Children.Add(border);
        }

        // ── Get the visual midpoint of a track piece ─────────────────
        private Point GetMidPoint(TrackPiece piece)
        {
            if (piece is StraightTrack straight)
            {
                return new Point(
                    (straight.StartPoint.X + straight.EndPoint.X) / 2,
                    (straight.StartPoint.Y + straight.EndPoint.Y) / 2);
            }
            else if (piece is CurvedTrack curve)
            {
                var center = curve.Center;
                double radiusPx = curve.RadiusMm * TrackConstants.PixelsPerMm;
                double startAngleRad = Math.Atan2(
                    curve.StartPoint.Y - center.Y,
                    curve.StartPoint.X - center.X);
                double sign = curve.Direction == CurveDirection.Right ? 1 : -1;
                double midAngleRad = startAngleRad +
                    sign * (curve.AngleDegrees / 2.0) * Math.PI / 180.0;
                return new Point(
                    center.X + radiusPx * Math.Cos(midAngleRad),
                    center.Y + radiusPx * Math.Sin(midAngleRad));
            }
            return piece.StartPoint;
        }

        // ── Draw sleepers along a straight ───────────────────────────
        private void DrawSleepers(Canvas canvas, Point start, Point end,
            double angleRad, bool occupied)
        {
            double dx = end.X - start.X;
            double dy = end.Y - start.Y;
            double length = Math.Sqrt(dx * dx + dy * dy);

            double perpX = -Math.Sin(angleRad);
            double perpY = Math.Cos(angleRad);

            double spacing = TrackConstants.SleeperSpacingPx;
            int count = (int)(length / spacing);

            var brush = occupied
                ? new SolidColorBrush(Color.FromRgb(180, 60, 40))
                : SleeperBrush;

            for (int i = 0; i <= count; i++)
            {
                double t = i * spacing;
                double cx = start.X + (dx / length) * t;
                double cy = start.Y + (dy / length) * t;

                double overhang = TrackConstants.SleeperOverhangPx;
                var sleeperStart = new Point(
                    cx + perpX * (TrackConstants.GaugePx / 2 + overhang),
                    cy + perpY * (TrackConstants.GaugePx / 2 + overhang));
                var sleeperEnd = new Point(
                    cx - perpX * (TrackConstants.GaugePx / 2 + overhang),
                    cy - perpY * (TrackConstants.GaugePx / 2 + overhang));

                DrawLine(canvas, sleeperStart, sleeperEnd, brush, 2.0);
            }
        }

        // ── Draw sleepers along a curve ───────────────────────────────
        private void DrawCurveSleepers(Canvas canvas, CurvedTrack track)
        {
            var center = track.Center;
            double radiusPx = track.RadiusMm * TrackConstants.PixelsPerMm;
            double halfGauge = TrackConstants.GaugePx / 2.0;
            double overhang = TrackConstants.SleeperOverhangPx;

            double startAngleRad = Math.Atan2(
                track.StartPoint.Y - center.Y,
                track.StartPoint.X - center.X);

            double sign = track.Direction == CurveDirection.Right ? 1 : -1;
            double totalAngleRad = sign * track.AngleDegrees * Math.PI / 180.0;

            double spacingRad = TrackConstants.SleeperSpacingPx / radiusPx;
            int count = (int)(Math.Abs(totalAngleRad) / spacingRad);

            var brush = track.IsOccupied
                ? new SolidColorBrush(Color.FromRgb(180, 60, 40))
                : SleeperBrush;

            for (int i = 0; i <= count; i++)
            {
                double angle = startAngleRad + i * spacingRad * sign;

                double cx = center.X + radiusPx * Math.Cos(angle);
                double cy = center.Y + radiusPx * Math.Sin(angle);

                double radialX = Math.Cos(angle);
                double radialY = Math.Sin(angle);

                var sleeperStart = new Point(
                    cx + radialX * (halfGauge + overhang),
                    cy + radialY * (halfGauge + overhang));
                var sleeperEnd = new Point(
                    cx - radialX * (halfGauge + overhang),
                    cy - radialY * (halfGauge + overhang));

                DrawLine(canvas, sleeperStart, sleeperEnd, brush, 2.0);
            }
        }

        // ── Draw an arc ───────────────────────────────────────────────
        private void DrawArc(Canvas canvas, Point center, double radius,
            Point startPoint, double angleDegrees,
            CurveDirection direction, Brush brush)
        {
            double startAngleRad = Math.Atan2(
                startPoint.Y - center.Y,
                startPoint.X - center.X);

            double sign = direction == CurveDirection.Right ? 1 : -1;
            double endAngleRad = startAngleRad +
                sign * angleDegrees * Math.PI / 180.0;

            var arcStart = new Point(
                center.X + radius * Math.Cos(startAngleRad),
                center.Y + radius * Math.Sin(startAngleRad));
            var arcEnd = new Point(
                center.X + radius * Math.Cos(endAngleRad),
                center.Y + radius * Math.Sin(endAngleRad));

            bool isLargeArc = angleDegrees > 180.0;
            var sweepDirection = direction == CurveDirection.Right
                ? SweepDirection.Clockwise
                : SweepDirection.Counterclockwise;

            var segment = new ArcSegment(
                arcEnd,
                new Size(radius, radius),
                0,
                isLargeArc,
                sweepDirection,
                true);

            var figure = new PathFigure
            {
                StartPoint = arcStart,
                IsClosed = false
            };
            figure.Segments.Add(segment);

            var geometry = new PathGeometry();
            geometry.Figures.Add(figure);

            var path = new Path
            {
                Stroke = brush,
                StrokeThickness = TrackConstants.RailWidthPx,
                Data = geometry
            };

            canvas.Children.Add(path);
        }

        // ── Helper — draw a simple line ───────────────────────────────
        private void DrawLine(Canvas canvas, Point start, Point end,
            Brush brush, double thickness)
        {
            var line = new Line
            {
                X1 = start.X,
                Y1 = start.Y,
                X2 = end.X,
                Y2 = end.Y,
                Stroke = brush,
                StrokeThickness = thickness
            };
            canvas.Children.Add(line);
        }
    }
}