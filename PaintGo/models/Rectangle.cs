using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PaintGo.models
{
    public class RectangleDrawer : Shape
    {
        public RectangleDrawer(EnumShape shapeType, Point startPoint, Point endPoint, Color fillColor, Color borderColor, Pen borderWidth, float rotationAngle = 0)
            : base(shapeType, startPoint, endPoint, fillColor, borderColor, borderWidth, rotationAngle)
        {
        }

        public override void Draw(Graphics graphics)
        {
            // Hitung posisi dan ukuran rectangle
            int x = Math.Min(StartPoint.X, EndPoint.X);
            int y = Math.Min(StartPoint.Y, EndPoint.Y);
            int width = Math.Abs(StartPoint.X - EndPoint.X);
            int height = Math.Abs(StartPoint.Y - EndPoint.Y);
            Rectangle rect = new Rectangle(x, y, width, height);

            // Hitung midpoint
            Point center = new Point(x + width / 2, y + height / 2);

            // Gabungkan semua transformasi dalam satu matrix
            Matrix transform = new Matrix();
            transform.Translate(Translation.X, Translation.Y);
            transform.RotateAt(RotationAngle, center);
            transform.Translate(center.X, center.Y);
            transform.Scale(ScaleFactor, ScaleFactor);
            transform.Translate(-center.X, -center.Y);

            // Terapkan transformasi ke graphics
            graphics.Transform = transform;

            // Gambar rectangle
            graphics.FillRectangle(BrushColor, rect);
            graphics.DrawRectangle(BorderWidth, rect);

            // Reset transformasi
            graphics.ResetTransform();
        }
    }
}
