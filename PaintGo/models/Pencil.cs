using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PaintGo.models
{
    public class Pencil : Shape
    {
        private List<Point> points = new List<Point>();
        public Pencil(EnumShape shapeType, Point startPoint, Point endPoint, Color fillColor, Color borderColor, Pen borderWidth, float rotationAngle = 0) : 
            base(shapeType, startPoint, endPoint, fillColor, borderColor, borderWidth, rotationAngle)
        {
            points.Add(startPoint);
        }

        public override void Draw(Graphics graphics)
        {
            for (int i = 1; i < points.Count; i++)
            {
                graphics.DrawLine(BorderWidth, points[i - 1], points[i]);
            }
        }

        public override void AddPoint(Point point)
        {
            points.Add(point);
        }

    }
}
