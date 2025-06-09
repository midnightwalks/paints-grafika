using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintCeunah.models
{
    public class Circle : Shape
    {
        private bool isDrawingCircle = false; // Menandai apakah sedang menggambar lingkaran
        public Circle(EnumShape shapeType, Point startPoint, Point endPoint, Color fillColor, Color borderColor, Pen borderWidth, float rotationAngle = 0)
            : base(shapeType, startPoint, endPoint, fillColor, borderColor, borderWidth, rotationAngle)
        {
        }

        public override void Draw(Graphics graphics)
        {
            // Menggambar lingkaran atau elips
            Rectangle rec = isDrawingCircle ? GetCircleRectangle() : GetEllipseRectangle();
            // get midpoint
            Point center = new Point(rec.X + rec.Width / 2, rec.Y + rec.Height / 2);
            // transformation, rotation and translation.
            Matrix transform = new Matrix();
            transform.Translate(Translation.X, Translation.Y);
            transform.RotateAt(RotationAngle, center);
            graphics.Transform = transform;
            ApplyScaleTransform(graphics, center);
            // draw
            graphics.DrawEllipse(BorderWidth, rec);
            graphics.FillEllipse(BrushColor, rec);

            // reset
            graphics.ResetTransform();
        }

        // Method untuk menetapkan apakah sedang menggambar lingkaran atau tidak
        public void SetDrawingCircle(bool isDrawing)
        {
            isDrawingCircle = isDrawing;
        }

        private Rectangle GetCircleRectangle()
        {
            int diameter = Math.Max(Math.Abs(EndPoint.X - StartPoint.X), Math.Abs(EndPoint.Y - StartPoint.Y));
            int x = Math.Min(StartPoint.X, EndPoint.X) + Translation.X;
            int y = Math.Min(StartPoint.Y, EndPoint.Y) + Translation.Y;
            return new Rectangle(x, y, diameter, diameter);
        }
        private Rectangle GetEllipseRectangle()
        {
            int x = Math.Min(StartPoint.X, EndPoint.X) + Translation.X;
            int y = Math.Min(StartPoint.Y, EndPoint.Y) + Translation.Y;
            int width = Math.Abs(EndPoint.X - StartPoint.X);
            int height = Math.Abs(EndPoint.Y - StartPoint.Y);
            return new Rectangle(x, y, width, height);
        }
    }
}
