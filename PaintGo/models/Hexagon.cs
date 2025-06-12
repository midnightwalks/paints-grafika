using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PaintGo.models
{
    public class Hexagon : Shape
    {
        public Hexagon(EnumShape shapeType, Point startPoint, Point endPoint, Color fillColor, Color borderColor, Pen borderWidth, float rotationAngle = 0)
            : base(shapeType, startPoint, endPoint, fillColor, borderColor, borderWidth, rotationAngle)
        {
        }

        public override void Draw(Graphics graphics)
        {
            // Hitung titik-titik hexagon
            Point[] points = GetHexagonPoints();

            // Hitung titik tengah untuk rotasi dan skala
            Point center = GetMidPoint();

            // Buat transformasi kumulatif: Translate → Rotate → Scale
            Matrix transform = new Matrix();

            // Translasi dari pengguna (kumulatif)
            transform.Translate(Translation.X, Translation.Y);

            // Rotasi dan skala terhadap titik tengah
            transform.Translate(center.X, center.Y);
            transform.Rotate(-RotationAngle);
            transform.Scale(ScaleFactor, ScaleFactor);
            transform.Translate(-center.X, -center.Y);

            // Terapkan transformasi ke grafik
            graphics.Transform = transform;

            // Gambar hexagon
            graphics.FillPolygon(BrushColor, points);
            graphics.DrawPolygon(BorderWidth, points);

            // Reset transformasi agar tidak mengganggu shape lain
            graphics.ResetTransform();
        }

        private Point[] GetHexagonPoints()
        {
            int width = Math.Abs(EndPoint.X - StartPoint.X);
            int height = Math.Abs(EndPoint.Y - StartPoint.Y);

            int centerX = Math.Min(StartPoint.X, EndPoint.X) + width / 2;
            int centerY = Math.Min(StartPoint.Y, EndPoint.Y) + height / 2;

            float radius = Math.Min(width, height) / 2f;

            Point[] points = new Point[6];
            for (int i = 0; i < 6; i++)
            {
                double angleDeg = 60 * i - 30; // agar bagian bawah rata
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
