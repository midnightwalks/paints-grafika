using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintCeunah.models
{
    public class RectangleDrawer : Shape
    {
        public RectangleDrawer(EnumShape shapeType, Point startPoint, Point endPoint, Color fillColor, Color borderColor, Pen borderWidth, float rotationAngle = 0)
            : base(shapeType, startPoint, endPoint, fillColor, borderColor, borderWidth,rotationAngle)
        {
        }

        public override void Draw(Graphics graphics)
        {
            // Menggunakan method Math.Min dan Math.Max untuk menentukan koordinat dan dimensi persegi panjang
            int x = Math.Min(StartPoint.X, EndPoint.X) + Translation.X;
            int y = Math.Min(StartPoint.Y, EndPoint.Y) + Translation.Y;
            int width = Math.Abs(StartPoint.X - EndPoint.X);
            int height = Math.Abs(StartPoint.Y - EndPoint.Y);
            // get midpoint
            Point midPoint = GetMidPoint();

            //apply scaling
            ApplyScaleTransform(graphics, midPoint);

            //for rotation
            graphics.TranslateTransform((float)(x + width / 2), (float)(y + height / 2));
            graphics.RotateTransform(RotationAngle);
            graphics.TranslateTransform(-(float)(x + width / 2), -(float)(y + height / 2));



            // Menggambar persegi panjang
            graphics.DrawRectangle(BorderWidth, x, y, width, height);
            graphics.FillRectangle(BrushColor, x, y, width, height);
            graphics.ResetTransform();
        }
        // Method for calculate the midpoint
    }
}
