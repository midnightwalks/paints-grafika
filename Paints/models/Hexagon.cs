using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PaintCeunah.models
{
    public class Hexagon : Shape
    {
        public Hexagon(EnumShape shapeType, Point startPoint, Point endPoint, Color fillColor, Color borderColor, Pen borderWidth, float rotationAngle = 0)
            : base(shapeType, startPoint, endPoint, fillColor, borderColor, borderWidth, rotationAngle)
        {
        }

        public override void Draw(Graphics graphics)
        {
            Point[] points = GetHexagonPoints();

            // get midpoint
            Point center = new Point(
                (points[0].X + points[3].X) / 2,
                (points[0].Y + points[3].Y) / 2
            );

            // transformasi: translate + rotasi
            Matrix transform = new Matrix();
            transform.Translate(Translation.X, Translation.Y);
            transform.RotateAt(RotationAngle, center);
            graphics.Transform = transform;

            // skala
            ApplyScaleTransform(graphics, center);

            // gambar garis dan isi hexagon
            graphics.DrawPolygon(BorderWidth, points);
            graphics.FillPolygon(BrushColor, points);

            graphics.ResetTransform();
        }

        private Point[] GetHexagonPoints()
        {
            int width = Math.Abs(EndPoint.X - StartPoint.X);
            int height = Math.Abs(EndPoint.Y - StartPoint.Y);

            int centerX = Math.Min(StartPoint.X, EndPoint.X) + width / 2 + Translation.X;
            int centerY = Math.Min(StartPoint.Y, EndPoint.Y) + height / 2 + Translation.Y;

            float radius = Math.Min(width, height) / 2f;

            Point[] points = new Point[6];
            for (int i = 0; i < 6; i++)
            {
                double angleDeg = 60 * i - 30; // rotasi agar posisi bawah rata
                double angleRad = Math.PI / 180 * angleDeg;
                points[i] = new Point(
                    centerX + (int)(radius * Math.Cos(angleRad)),
                    centerY + (int)(radius * Math.Sin(angleRad))
                );
            }

            return points;
        }
    }
}
