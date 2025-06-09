using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintCeunah.models
{
    public class Diamond : Shape
    {
        public Diamond(EnumShape shape, Point startPoint, Point endPoint, Color fillColor, Color strokeColor, Pen pen) 
            : base(shape, startPoint, endPoint, fillColor, strokeColor, pen)
        {
        }

        public override void Draw(Graphics g)
        {
            if (StartPoint == null || EndPoint == null) return;

            // Calculate center and dimensions
            int centerX = (StartPoint.X + EndPoint.X) / 2;
            int centerY = (StartPoint.Y + EndPoint.Y) / 2;
            int width = Math.Abs(EndPoint.X - StartPoint.X);
            int height = Math.Abs(EndPoint.Y - StartPoint.Y);

            if (width <= 0 || height <= 0) return;

            // Create diamond points
            PointF[] diamondPoints = CreateDiamondPoints(centerX, centerY, width / 2, height / 2);

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
            matrix.TransformPoints(diamondPoints);

            // Save graphics state
            GraphicsState state = g.Save();

            try
            {
                // Fill the diamond
                using (SolidBrush fillBrush = new SolidBrush(FillColor))
                {
                    g.FillPolygon(fillBrush, diamondPoints);
                }

                // Draw the diamond outline
                using (Pen outlinePen = new Pen(Color.Black, 2))
                {
                    g.DrawPolygon(outlinePen, diamondPoints);
                }
            }
            finally
            {
                // Restore graphics state
                g.Restore(state);
                matrix.Dispose();
            }
        }

        private PointF[] CreateDiamondPoints(float centerX, float centerY, float halfWidth, float halfHeight)
        {
            PointF[] diamondPoints = new PointF[4];
            
            // Top point
            diamondPoints[0] = new PointF(centerX, centerY - halfHeight);
            // Right point
            diamondPoints[1] = new PointF(centerX + halfWidth, centerY);
            // Bottom point
            diamondPoints[2] = new PointF(centerX, centerY + halfHeight);
            // Left point
            diamondPoints[3] = new PointF(centerX - halfWidth, centerY);

            return diamondPoints;
        }
    }
}
