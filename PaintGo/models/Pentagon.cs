using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PaintGo.models
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

            // Hitung pusat dan radius
            float centerX = (StartPoint.X + EndPoint.X) / 2f;
            float centerY = (StartPoint.Y + EndPoint.Y) / 2f;
            float radius = Math.Max(Math.Abs(EndPoint.X - StartPoint.X), Math.Abs(EndPoint.Y - StartPoint.Y)) / 2f;
            if (radius <= 0) return;

            // Buat titik-titik pentagon
            PointF[] pentagonPoints = CreatePentagonPoints(centerX, centerY, radius);

            // Simpan state grafik
            GraphicsState state = g.Save();

            // Buat matrix transformasi kumulatif
            Matrix matrix = new Matrix();

            // Skala terhadap titik pusat
            matrix.Translate(-centerX, -centerY);
            matrix.Scale(ScaleFactor, ScaleFactor);
            matrix.Translate(centerX, centerY);

            // Rotasi terhadap titik pusat
            matrix.RotateAt(RotationAngle, new PointF(centerX, centerY));

            // Translasi akhir (dipindahkan seluruh shape)
            matrix.Translate(Translation.X, Translation.Y);

            // Terapkan transformasi ke semua titik
            matrix.TransformPoints(pentagonPoints);

            // Gambar isi
            using (SolidBrush brush = new SolidBrush(FillColor))
            {
                g.FillPolygon(brush, pentagonPoints);
            }

            // Gambar garis tepi
            using (Pen outline = new Pen(BorderColor, BorderWidth.Width))
            {
                g.DrawPolygon(outline, pentagonPoints);
            }

            // Kembalikan ke state awal
            g.Restore(state);
            matrix.Dispose();
        }

        private PointF[] CreatePentagonPoints(float centerX, float centerY, float radius)
        {
            PointF[] pentagonPoints = new PointF[5];
            double angleStep = 2 * Math.PI / 5; // 72 derajat
            double angle = -Math.PI / 2; // Mulai dari atas

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
