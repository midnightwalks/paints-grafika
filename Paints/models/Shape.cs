using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintCeunah.models
{
    public abstract class Shape
    {
        public EnumShape ShapeType { get; set; }
        public Point StartPoint { get; set; }
        public float RotationAngle { get; set; }
        public Point Translation { get; set; }
        public Point EndPoint { get; set; }
        public Color FillColor { get; set; }
        public Color BorderColor { get; set; }
        public Pen BorderWidth { get; set; }
        public Brush BrushColor;

        public float ScaleFactor { get; set; } = 1.0f;

        public Shape(EnumShape shapeType, Point startPoint, Point endPoint, Color fillColor, Color borderColor,
            Pen borderWidth,
            float rotationAngle = 0)
        {
            ShapeType = shapeType;
            StartPoint = startPoint;
            EndPoint = endPoint;

            // Set default fill color ke biru jika belum ditentukan
            FillColor = fillColor.IsEmpty ? Color.Blue : fillColor;
            BorderColor = borderColor;
            BorderWidth = borderWidth;
            BrushColor = new SolidBrush(FillColor);
            RotationAngle = rotationAngle;
            Translation = new Point(0, 0);
        }



        public abstract void Draw(Graphics graphics); // Metode abstract untuk menggambar bentuk


        public virtual void AddPoint(Point point)
        {

        }
        public void SetRotationAngle(float angle)
        {
            RotationAngle = -1 * angle;
        }
        public void SetTranslation(Point translation)
        {
            translation.Y = translation.Y * -1;
            Translation = translation;
        }

        public void SetScaleFactor(float scaleFactor)
        {
            ScaleFactor = scaleFactor;
        }
        protected void ApplyScaleTransform(Graphics graphics, Point midPoint)
        {
            graphics.TranslateTransform(midPoint.X, midPoint.Y);
            graphics.ScaleTransform(ScaleFactor, ScaleFactor);
            graphics.TranslateTransform(-midPoint.X, -midPoint.Y);
        }
        protected Point GetMidPoint()
        {
            int midX = (StartPoint.X + EndPoint.X) / 2;
            int midY = (StartPoint.Y + EndPoint.Y) / 2;
            return new Point(midX, midY);
        }
    }
}
