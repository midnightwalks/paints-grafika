using System;
using System.Drawing;

namespace PaintGo.models
{
    public abstract class Shape
    {
        public EnumShape ShapeType { get; set; }
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public Color FillColor { get; set; }
        public Color BorderColor { get; set; }
        public Pen BorderWidth { get; set; }
        public Brush BrushColor;

        public float RotationAngle { get; set; }
        public Point Translation { get; set; } = new Point(0, 0);
        public float ScaleFactor { get; set; } = 1.0f;

        public Shape(EnumShape shapeType, Point startPoint, Point endPoint, Color fillColor, Color borderColor,
            Pen borderWidth, float rotationAngle = 0)
        {
            ShapeType = shapeType;
            StartPoint = startPoint;
            EndPoint = endPoint;
            FillColor = fillColor.IsEmpty ? Color.Blue : fillColor;
            BorderColor = borderColor;
            BorderWidth = borderWidth;
            BrushColor = new SolidBrush(FillColor);
            RotationAngle = rotationAngle;
        }

        public abstract void Draw(Graphics graphics); // Abstract method untuk menggambar shape

        // Tambah point untuk shape dinamis seperti polygon (jika diperlukan)
        public virtual void AddPoint(Point point) { }

        // Translasi kumulatif
        public void Translate(Point offset)
        {
            Translation = new Point(Translation.X + offset.X, Translation.Y + offset.Y);
        }

        // Rotasi kumulatif
        public void Rotate(float angle)
        {
            RotationAngle += angle;
        }

        // Untuk rotasi dari UI (diganti agar tetap kumulatif)
        public void SetRotationAngle(float angle)
        {
            RotationAngle += angle;
        }

        // Translasi dari UI (diganti agar tetap kumulatif)
        public void SetTranslation(Point offset)
        {
            offset.Y *= -1; // jika ingin sumbu Y dibalik (tergantung sistem koordinat)
            Translation = new Point(Translation.X + offset.X, Translation.Y + offset.Y);
        }

        // Skala tetap overwrite
        public void SetScaleFactor(float scaleFactor)
        {
            ScaleFactor = scaleFactor;
        }

        // Terapkan transformasi skala terhadap titik tengah
        protected void ApplyScaleTransform(Graphics graphics, Point midPoint)
        {
            graphics.TranslateTransform(midPoint.X, midPoint.Y);
            graphics.ScaleTransform(ScaleFactor, ScaleFactor);
            graphics.TranslateTransform(-midPoint.X, -midPoint.Y);
        }

        // Dapatkan titik tengah shape
        protected Point GetMidPoint()
        {
            int midX = (StartPoint.X + EndPoint.X) / 2;
            int midY = (StartPoint.Y + EndPoint.Y) / 2;
            return new Point(midX, midY);
        }

        // Untuk debugging atau tampilan info
        public string GetTransformStatus()
        {
            return $"Translation: ({Translation.X}, {Translation.Y}), Rotation: {RotationAngle}°, Scale: {ScaleFactor}";
        }
    }
}
