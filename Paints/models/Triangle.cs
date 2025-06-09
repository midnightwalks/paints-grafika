using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PaintCeunah.models
{
    public class Triangle : Shape
    {
        public Triangle(EnumShape shapeType, Point startPoint, Point endPoint, Color fillColor, Color borderColor, Pen borderWidth, float rotationAngle = 0)
            : base(shapeType, startPoint, endPoint, fillColor, borderColor, borderWidth, rotationAngle)
        {
        }

        public override void Draw(Graphics graphics)
        {
            Point[] trianglePoints = GetEquilateralTrianglePoints(StartPoint, EndPoint);

            // Calculate the center point of the triangle
            Point centerPoint = new Point(
                (trianglePoints[0].X + trianglePoints[1].X + trianglePoints[2].X) / 3,
                (trianglePoints[0].Y + trianglePoints[1].Y + trianglePoints[2].Y) / 3
            );

            // Apply rotation and translation using matrix
            Matrix transformationMatrix = new Matrix();
            transformationMatrix.Translate(Translation.X, Translation.Y);
            transformationMatrix.RotateAt(RotationAngle, centerPoint);
            graphics.Transform = transformationMatrix;

            //for scalling
            ApplyScaleTransform(graphics, centerPoint);

            graphics.DrawPolygon(BorderWidth, trianglePoints);
            graphics.FillPolygon(BrushColor, trianglePoints);

            graphics.ResetTransform();
        }

        // calculate triangle points according to center and base length
        private Point[] GetEquilateralTrianglePoints(Point startPoint, Point endPoint)
        {
            Point[] points = new Point[3];
            points[0] = new Point((startPoint.X + endPoint.X) / 2, startPoint.Y); // top 
            points[1] = new Point(endPoint.X, endPoint.Y); // base right 
            points[2] = new Point(startPoint.X, endPoint.Y); // base left 

            return points;
        }
    }
}
