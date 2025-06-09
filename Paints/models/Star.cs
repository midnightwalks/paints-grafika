using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintCeunah.models
{
    public class Star : Shape
    {
        public Star(EnumShape shape, Point startPoint, Point endPoint, Color fillColor, Color strokeColor, Pen pen) 
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

            // Create star points (5-pointed star)
            PointF[] starPoints = CreateStarPoints(centerX, centerY, radius, radius / 2, 5);

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
            
            // Apply scaling (manual implementation since ScaleAt doesn't exist in .NET Framework 4.8)
            if (ScaleFactor != 1.0f)
            {
                // Translate to origin, scale, then translate back
                matrix.Translate(-centerX, -centerY);
                matrix.Scale(ScaleFactor, ScaleFactor);
                matrix.Translate(centerX, centerY);
            }

            // Transform points
            matrix.TransformPoints(starPoints);

            // Save graphics state
            GraphicsState state = g.Save();

            try
            {
                // Fill the star
                using (SolidBrush fillBrush = new SolidBrush(FillColor))
                {
                    g.FillPolygon(fillBrush, starPoints);
                }

                // Draw the star outline
                g.DrawPolygon(BorderPen, starPoints);
            }
            finally
            {
                // Restore graphics state
                g.Restore(state);
                matrix.Dispose();
            }
        }

        private PointF[] CreateStarPoints(float centerX, float centerY, float outerRadius, float innerRadius, int points)
        {
            PointF[] starPoints = new PointF[points * 2];
            double angleStep = Math.PI / points;
            double angle = -Math.PI / 2; // Start from top

            for (int i = 0; i < points * 2; i++)
            {
                float radius = (i % 2 == 0) ? outerRadius : innerRadius;
                starPoints[i] = new PointF(
                    centerX + (float)(Math.Cos(angle) * radius),
                    centerY + (float)(Math.Sin(angle) * radius)
                );
                angle += angleStep;
            }

            return starPoints;
        }
    }
}
