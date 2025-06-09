using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintCeunah.models
{
    public class LineDrawer : Shape
    {
        public LineDrawer(EnumShape shapeType, Point startPoint, Point endPoint, Color fillColor, Color borderColor, Pen borderWidth, float rotationAngle = 0) : 
            base(shapeType, startPoint, endPoint, fillColor, borderColor, borderWidth, rotationAngle)
        {
        }

        public override void Draw(Graphics graphics)
        {
            //get mid point
            Point midPoint = GetMidPoint();
            // Hitung titik tengah garis
            float midX = (StartPoint.X + EndPoint.X) / 2;
            float midY = (StartPoint.Y + EndPoint.Y) / 2;

            // Simpan state transformasi sebelumnya
            var oldTransform = graphics.Transform;

            // Lakukan rotasi di sekitar titik tengah garis
            graphics.TranslateTransform(midX, midY);
            graphics.RotateTransform(RotationAngle);
            graphics.TranslateTransform(-midX, -midY);
            ApplyScaleTransform(graphics, midPoint);

            // Gambar garis dengan rotasi
            graphics.DrawLine(BorderWidth, StartPoint, EndPoint);

            // Reset transformasi ke state sebelumnya
            graphics.Transform = oldTransform;
            //graphics.DrawLine(BorderWidth,StartPoint,EndPoint);
        }
    }
}
