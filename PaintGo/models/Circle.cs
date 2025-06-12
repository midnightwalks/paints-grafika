using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PaintGo.models
{
    public class Circle : Shape
    {
        private bool isDrawingCircle = false;

        public Circle(EnumShape shapeType, Point startPoint, Point endPoint, Color fillColor, Color borderColor, Pen borderWidth, float rotationAngle = 0)
            : base(shapeType, startPoint, endPoint, fillColor, borderColor, borderWidth, rotationAngle)
        {
        }

        public override void Draw(Graphics graphics)
        {
            Rectangle rec = isDrawingCircle ? GetCircleRectangle() : GetEllipseRectangle();
            Point center = new Point(rec.X + rec.Width / 2, rec.Y + rec.Height / 2);

            // Matriks transformasi
            Matrix transform = new Matrix();
            transform.Translate(Translation.X, Translation.Y); // Translasi dulu
            transform.RotateAt(RotationAngle, center);         // Rotasi di titik tengah
            graphics.Transform = transform;

            ApplyScaleTransform(graphics, center); // (Opsional) Scaling

            // Gambar isi dan border lingkaran/ellipse
            graphics.FillEllipse(BrushColor, rec);
            graphics.DrawEllipse(BorderWidth, rec);

            graphics.ResetTransform(); // Reset agar tidak mengganggu objek lain
        }

        public void SetDrawingCircle(bool isDrawing)
        {
            isDrawingCircle = isDrawing;
        }

        private Rectangle GetCircleRectangle()
        {
            int diameter = Math.Max(Math.Abs(EndPoint.X - StartPoint.X), Math.Abs(EndPoint.Y - StartPoint.Y));
            int x = Math.Min(StartPoint.X, EndPoint.X);
            int y = Math.Min(StartPoint.Y, EndPoint.Y);
            return new Rectangle(x, y, diameter, diameter);
        }

        private Rectangle GetEllipseRectangle()
        {
            int x = Math.Min(StartPoint.X, EndPoint.X);
            int y = Math.Min(StartPoint.Y, EndPoint.Y);
            int width = Math.Abs(EndPoint.X - StartPoint.X);
            int height = Math.Abs(EndPoint.Y - StartPoint.Y);
            return new Rectangle(x, y, width, height);
        }
    }
}
