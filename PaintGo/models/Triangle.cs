using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PaintGo.models
{
    public class Triangle : Shape
    {
        public Triangle(EnumShape shapeType, Point startPoint, Point endPoint, Color fillColor, Color borderColor, Pen borderWidth, float rotationAngle = 0)
            : base(shapeType, startPoint, endPoint, fillColor, borderColor, borderWidth, rotationAngle)
        {
        }

        public override void Draw(Graphics graphics)
        {
            // Dapatkan titik-titik segitiga sama kaki
            Point[] trianglePoints = GetEquilateralTrianglePoints(StartPoint, EndPoint);

            // Hitung titik pusat segitiga
            Point centerPoint = new Point(
                (trianglePoints[0].X + trianglePoints[1].X + trianglePoints[2].X) / 3,
                (trianglePoints[0].Y + trianglePoints[1].Y + trianglePoints[2].Y) / 3
            );

            // Buat matrix transformasi kumulatif: Translate → Rotate → Scale
            Matrix transform = new Matrix();
            transform.Translate(Translation.X, Translation.Y);
            transform.RotateAt(RotationAngle, centerPoint);
            transform.Translate(centerPoint.X, centerPoint.Y);
            transform.Scale(ScaleFactor, ScaleFactor);
            transform.Translate(-centerPoint.X, -centerPoint.Y);

            // Terapkan transformasi
            graphics.Transform = transform;

            // Gambar segitiga
            graphics.FillPolygon(BrushColor, trianglePoints);
            graphics.DrawPolygon(BorderWidth, trianglePoints);

            // Reset transformasi agar tidak memengaruhi objek lain
            graphics.ResetTransform();
        }

        private Point[] GetEquilateralTrianglePoints(Point startPoint, Point endPoint)
        {
            Point[] points = new Point[3];
            points[0] = new Point((startPoint.X + endPoint.X) / 2, startPoint.Y); // atas (puncak)
            points[1] = new Point(endPoint.X, endPoint.Y);                        // kanan bawah
            points[2] = new Point(startPoint.X, endPoint.Y);                      // kiri bawah
            return points;
        }
    }
}
