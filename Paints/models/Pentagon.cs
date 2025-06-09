using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintCeunah.models
{
    public class Pentagon : Shape
    {
        public Pentagon(EnumShape shape, Point startPoint, Point endPoint, Color fillColor, Color strokeColor, Pen pen) 
            : base(shape, startPoint, endPoint, fillColor, strokeColor, pen)
        {
        }

        public override void Draw(Graphics g)
        {
            if (StartPoint == null || EndPoint == null) return;

            // Calculate center and radius
            int centerX = (StartPoint.X + EndPoint.X) / 2;
            int centerY = (StartPoint.Y + EndPoint.Y) / 2;
            int radius = Math.Max(Math.Abs(EndPoint.X - StartPoint.X), Math.Abs(EndPoint.Y - StartPoint.Y)) / 2;

            if (radius <= 0) return;

            // Create pentagon points (5-sided polygon)
            PointF[] pentagonPoints = CreatePentagonPoints(centerX, centerY, radius);

            // Apply transformations
            Matrix matrix = new Matrix();
            
            // Apply translation
            if (Translation.X != 0 || Translation.Y != 0)
            {
                matrix.Translate(Translation.X, Translation.Y);
            }
            
            // Apply rotation around center
            if (RotationAngle != 0)
            {
                matrix.RotateAt(RotationAngle, new PointF(centerX, centerY));
            }
            
            // Apply scaling
            if (ScaleFactor != 1.0f)
            {
                matrix.Translate(-centerX, -centerY);
                matrix.Scale(ScaleFactor, ScaleFactor);
                matrix.Translate(centerX, centerY);
            }

            // Transform points
            matrix.TransformPoints(pentagonPoints);

            // Save graphics state
            GraphicsState state = g.Save();

            try
            {
                // Fill the pentagon
                using (SolidBrush fillBrush = new SolidBrush(FillColor))
                {
                    g.FillPolygon(fillBrush, pentagonPoints);
                }

                // Draw the pentagon outline - buat pen baru untuk menghindari error
                using (Pen outlinePen = new Pen(Color.Black, 2))
                {
                    g.DrawPolygon(outlinePen, pentagonPoints);
                }
            }
            finally
            {
                // Restore graphics state
                g.Restore(state);
                matrix.Dispose();
            }
        }

        private PointF[] CreatePentagonPoints(float centerX, float centerY, float radius)
        {
            PointF[] pentagonPoints = new PointF[5];
            double angleStep = 2 * Math.PI / 5; // 72 degrees in radians
            double angle = -Math.PI / 2; // Start from top

            for (int i = 0; i < 5; i++)
            {
                pentagonPoints[i] = new PointF(
                    centerX + (float)(Math.Cos(angle) * radius),
                    centerY + (float)(Math.Sin(angle) * radius)
                );
                angle += angleStep;
            }

            return pentagonPoints;
        }
    }
}
