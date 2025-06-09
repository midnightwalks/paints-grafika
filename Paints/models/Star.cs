using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PaintCeunah.models
{
    public class Star : Shape
    {
        private int numberOfPoints = 5;

        public Star(EnumShape shapeType, Point startPoint, Point endPoint, Color fillColor, Color borderColor, Pen borderWidth, float rotationAngle = 0)
            : base(shapeType, startPoint, endPoint, fillColor, borderColor, borderWidth, rotationAngle)
        {
        }

        public override void Draw(Graphics graphics)
        {
            Point[] starPoints = GetStarPoints();

            // Calculate center point
            Point centerPoint = new Point(
                (StartPoint.X + EndPoint.X) / 2 + Translation.X,
                (StartPoint.Y + EndPoint.Y) / 2 + Translation.Y
            );

            // Apply transformations
            Matrix transformationMatrix = new Matrix();
            transformationMatrix.Translate(Translation.X, Translation.Y);
            transformationMatrix.RotateAt(RotationAngle, centerPoint);
            graphics.Transform = transformationMatrix;

            // Apply scaling
            ApplyScaleTransform(graphics, centerPoint);

            // Draw star
            graphics.DrawPolygon(BorderWidth, starPoints);
            graphics.FillPolygon(BrushColor, starPoints);

            graphics.ResetTransform();
        }

        private Point[] GetStarPoints()
        {
            int width = Math.Abs(EndPoint.X - StartPoint.X);
            int height = Math.Abs(EndPoint.Y - StartPoint.Y);

            int centerX = Math.Min(StartPoint.X, EndPoint.X) + width / 2;
            int centerY = Math.Min(StartPoint.Y, EndPoint.Y) + height / 2;

            float outerRadius = Math.Min(width, height) / 2f;
            float innerRadius = outerRadius * 0.4f;

            Point[] points = new Point[numberOfPoints * 2];

            for (int i = 0; i < numberOfPoints * 2; i++)
            {
                double angleDeg = (360.0 / (numberOfPoints * 2)) * i - 90; // Start from top
                double angleRad = Math.PI / 180 * angleDeg;

                float radius = (i % 2 == 0) ? outerRadius : innerRadius;

                points[i] = new Point(
                    centerX + (int)(radius * Math.Cos(angleRad)),
                    centerY + (int)(radius * Math.Sin(angleRad))
                );
            }

            return points;
        }
    }
}