using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PaintGo.models
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

            // Hitung pusat dan ukuran shape
            float centerX = (StartPoint.X + EndPoint.X) / 2f;
            float centerY = (StartPoint.Y + EndPoint.Y) / 2f;
            float halfWidth = Math.Abs(EndPoint.X - StartPoint.X) / 2f;
            float halfHeight = Math.Abs(EndPoint.Y - StartPoint.Y) / 2f;

            if (halfWidth <= 0 || halfHeight <= 0) return;

            // Buat titik-titik diamond
            PointF[] diamondPoints = CreateDiamondPoints(centerX, centerY, halfWidth, halfHeight);

            // Simpan state grafik
            GraphicsState state = g.Save();

            // Buat matrix transformasi kumulatif
            Matrix matrix = new Matrix();

            // 1. Skala di pusat
            matrix.Translate(-centerX, -centerY);
            matrix.Scale(ScaleFactor, ScaleFactor);
            matrix.Translate(centerX, centerY);

            // 2. Rotasi terhadap pusat
            matrix.RotateAt(RotationAngle, new PointF(centerX, centerY));

            // 3. Translasi terakhir
            matrix.Translate(Translation.X, Translation.Y);

            // Transformasi titik
            matrix.TransformPoints(diamondPoints);

            // Mengisi diamond
            using (SolidBrush fillBrush = new SolidBrush(FillColor))
            {
                g.FillPolygon(fillBrush, diamondPoints);
            }

            // Menggambar outline
            using (Pen outlinePen = new Pen(BorderColor, BorderWidth.Width))
            {
                g.DrawPolygon(outlinePen, diamondPoints);
            }

            // Pulihkan state
            g.Restore(state);
            matrix.Dispose();
        }

        private PointF[] CreateDiamondPoints(float centerX, float centerY, float halfWidth, float halfHeight)
        {
            return new PointF[]
            {
                new PointF(centerX, centerY - halfHeight), // Atas
                new PointF(centerX + halfWidth, centerY),  // Kanan
                new PointF(centerX, centerY + halfHeight), // Bawah
                new PointF(centerX - halfWidth, centerY)   // Kiri
            };
        }
    }
}
