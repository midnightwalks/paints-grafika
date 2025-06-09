using PaintCeunah.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace PaintCeunah
{
    public partial class Form1 : Form
    {
        private Point startPoint; // Titik awal
        private Point endPoint; // Titik akhir
        private bool isDrawing;
        private EnumShape currentActiveShape= EnumShape.NONE;
        private List<Shape> tumpukanGambar;
        private Shape tempShape;
        private Color fillColor;
        private Color strokeColor = Color.Black;
        private Color currentFillColor = Color.White;
        private Color currentBorderColor = Color.White;
        private float rotationAngle = 0;
        private Bitmap tempBitmap;
        private int translationX = 0;
        private int translationY = 0;

        private bool isCircle = false; //Untuk toggle ellipse dan circle
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            tumpukanGambar = new List<Shape>();
            canvasPanel.MouseDown += CanvasPanel_MouseDown;
            canvasPanel.MouseMove += CanvasPanel_MouseMove;
            canvasPanel.MouseUp += CanvasPanel_MouseUp;
            canvasPanel.Paint += CanvasPanel_Paint;
            panel1.Paint += panel1_Paint;
            btnColor.BackColor = currentFillColor;
            btnBorderColor.BackColor = currentBorderColor;
            tempBitmap = new Bitmap(canvasPanel.Width, canvasPanel.Height);
            canvasPanel.Image = tempBitmap;
        }


        private void CanvasPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentActiveShape != EnumShape.NONE)
            {
                startPoint = e.Location;
                isDrawing = true;

                switch (currentActiveShape)
                {
                    case EnumShape.CIRCLE:
                        tempShape = new Circle(currentActiveShape, startPoint, startPoint,
                            currentFillColor, currentBorderColor, new Pen(currentBorderColor,
                            (tbBorderWidth.Text.Length > 0) ? int.Parse(tbBorderWidth.Text.ToString()) : 5));
                        isCircle = (e.Button == MouseButtons.Right) ? false : true;
                        break;

                    case EnumShape.SQUARE:
                        tempShape = new Square(currentActiveShape, startPoint, startPoint,
                            currentFillColor, currentBorderColor, new Pen(currentBorderColor,
                            (tbBorderWidth.Text.Length > 0) ? int.Parse(tbBorderWidth.Text.ToString()) : 5));
                        break;

                    case EnumShape.RECTANGLE:
                        tempShape = new RectangleDrawer(currentActiveShape, startPoint, startPoint,
                            currentFillColor, currentBorderColor, new Pen(currentBorderColor,
                            (tbBorderWidth.Text.Length > 0) ? int.Parse(tbBorderWidth.Text.ToString()) : 5));
                        break;

                    case EnumShape.LINE:
                        tempShape = new LineDrawer(currentActiveShape, startPoint, startPoint,
                            currentFillColor, currentBorderColor, new Pen(currentBorderColor,
                            (tbBorderWidth.Text.Length > 0) ? int.Parse(tbBorderWidth.Text.ToString()) : 5));
                        break;

                    case EnumShape.PENCIL:
                        tempShape = new Pencil(currentActiveShape, startPoint, startPoint,
                            currentFillColor, currentBorderColor, new Pen(currentBorderColor,
                            (tbBorderWidth.Text.Length > 0) ? int.Parse(tbBorderWidth.Text.ToString()) : 5));
                        break;

                    case EnumShape.ERASER:
                        tempShape = new Pencil(currentActiveShape, startPoint, startPoint,
                            Color.White, Color.White, new Pen(Color.White,
                            (tbBorderWidth.Text.Length > 0) ? int.Parse(tbBorderWidth.Text.ToString()) : 5));
                        break;

                    case EnumShape.TRIANGLE:
                        tempShape = new Triangle(currentActiveShape, startPoint, startPoint,
                            currentFillColor, currentBorderColor, new Pen(currentBorderColor,
                            (tbBorderWidth.Text.Length > 0) ? int.Parse(tbBorderWidth.Text.ToString()) : 5));
                        break;

                    case EnumShape.HEXAGON:
                        tempShape = new Hexagon(currentActiveShape, startPoint, startPoint,
                            currentFillColor, currentBorderColor, new Pen(currentBorderColor,
                            (tbBorderWidth.Text.Length > 0) ? int.Parse(tbBorderWidth.Text.ToString()) : 5));
                        break;

                    case EnumShape.STAR:
                        tempShape = new Star(currentActiveShape, startPoint, startPoint,
                            currentFillColor, currentBorderColor, new Pen(currentBorderColor,
                            (tbBorderWidth.Text.Length > 0) ? int.Parse(tbBorderWidth.Text.ToString()) : 5));
                        break;

                    default:
                        // Tidak ada aksi
                        break;
                }
            }
        }

        private void CanvasPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing && currentActiveShape != EnumShape.NONE)
            {

                if (currentActiveShape == EnumShape.PENCIL && e.Button == MouseButtons.Left)
                {
                    tempShape.AddPoint(e.Location);
                    using(Graphics g = Graphics.FromImage(tempBitmap))
                    {
                        tempShape.Draw(g);
                        endPoint = e.Location;
                    }
                    canvasPanel.Refresh();
                }
                else if (currentActiveShape == EnumShape.ERASER && e.Button == MouseButtons.Left)
                {
                    tempShape.AddPoint(e.Location);
                    using (Graphics g = Graphics.FromImage(tempBitmap))
                    {
                        tempShape.Draw(g);
                        endPoint = e.Location;
                    }
                    canvasPanel.Refresh();
                }
                else
                {
                    endPoint = e.Location;
                    canvasPanel.Refresh();// Meminta panel untuk digambar ulang
                }
            }
        }

        private void CanvasPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentActiveShape != EnumShape.NONE)
            {
                endPoint = e.Location;
                isDrawing = false;
                tumpukanGambar.Add(tempShape);
                using(Graphics g = Graphics.FromImage(tempBitmap))
                {
                    tempShape.Draw(g);
                }
                canvasPanel.Refresh();
            }
        }

        private void CanvasPanel_Paint(object sender, PaintEventArgs e)
        {
            if (tempShape != null)
            {
                tempShape.EndPoint = endPoint;

                if (currentActiveShape == EnumShape.CIRCLE)
                {
                    (tempShape as Circle).SetDrawingCircle(isCircle);
                }
                tempShape.Draw(e.Graphics);
            }
        }

        private void btnCircle_Click(object sender, EventArgs e)
        {
            currentActiveShape = EnumShape.CIRCLE;
            panel1.Invalidate();
            panel1.Refresh();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (currentActiveShape != EnumShape.NONE)
            {
                foreach (Control c in panel1.Controls)
                {
                    c.BackColor = SystemColors.Control;
                }
            }
            if(currentActiveShape == EnumShape.CIRCLE)
            {
                btnCircle.BackColor = Color.Aqua;
            }
            else if(currentActiveShape == EnumShape.SQUARE)
            {
                btnSquare.BackColor = Color.Aqua;
            }
            else if (currentActiveShape == EnumShape.RECTANGLE)
            {
                btnRectangle.BackColor = Color.Aqua;
            }
            else if (currentActiveShape == EnumShape.LINE)
            {
                btnLine.BackColor = Color.Aqua;
            }
            else if (currentActiveShape == EnumShape.PENCIL)
            {
                btnPencil.BackColor = Color.Aqua;
            }
            else if(currentActiveShape == EnumShape.ERASER)
            {
                btnEraser.BackColor = Color.Aqua;
            }
            else if (currentActiveShape == EnumShape.TRIANGLE)
            {
                btnTriangle.BackColor = Color.Aqua;
            }
            else if (currentActiveShape == EnumShape.HEXAGON)
            {
                btnHexagon.BackColor = Color.Aqua;
            }
            else if (currentActiveShape == EnumShape.STAR)
            {
                btnStar.BackColor = Color.Aqua;
            }
        }

        private void btnSquare_Click(object sender, EventArgs e)
        {
            currentActiveShape = EnumShape.SQUARE;
            panel1.Invalidate();
            panel1.Refresh();
        }

        private void btnRectangle_Click(object sender, EventArgs e)
        {
            currentActiveShape = EnumShape.RECTANGLE;
            panel1.Invalidate();
            panel1.Refresh();
        }

        private void btnLine_Click(object sender, EventArgs e)
        {
            currentActiveShape = EnumShape.LINE;
            panel1.Invalidate();
            panel1.Refresh();
        }

        private void btnPencil_Click(object sender, EventArgs e)
        {
            currentActiveShape = EnumShape.PENCIL;
            panel1.Invalidate();
            panel1.Refresh();
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            Thread colorThread = new Thread(() =>
            {
                using (ColorDialog cd = new ColorDialog())
                {
                    if (cd.ShowDialog() == DialogResult.OK)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            currentFillColor = cd.Color;
                            fillColor = currentFillColor;
                            btnColor.BackColor = currentFillColor; // update warna tombol
                        });
                    }
                }
            });
            colorThread.SetApartmentState(ApartmentState.STA);
            colorThread.Start();
        }


        private void tbBorderWidth_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {// Hanya izinkan angka (0-9), tombol backspace, dan tombol delete
                e.Handled = true; // Mengabaikan karakter selain angka
            }
        }

        private void btnBorderColor_Click(object sender, EventArgs e)
        {
            Thread colorThread = new Thread(() =>
            {
                using (ColorDialog cd = new ColorDialog())
                {
                    if (cd.ShowDialog() == DialogResult.OK)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            currentBorderColor = cd.Color;
                            strokeColor = currentBorderColor;
                            btnBorderColor.BackColor = currentBorderColor; // update warna tombol
                        });
                    }
                }
            });
            colorThread.SetApartmentState(ApartmentState.STA);
            colorThread.Start();
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            tumpukanGambar.Clear();
            tempShape = null;
            using(Graphics g = Graphics.FromImage(tempBitmap))
            {
                g.Clear(Color.White);
            }
            canvasPanel.Refresh();
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "JPEG files (*.jpg)|*.jpg";
                sfd.FilterIndex = 1;
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // Buat sebuah Bitmap dari canvasPanel
                    Bitmap bmp = new Bitmap(canvasPanel.Width, canvasPanel.Height);
                    canvasPanel.DrawToBitmap(bmp, new Rectangle(0, 0, canvasPanel.Width, canvasPanel.Height));

                    // Simpan bitmap sebagai file JPEG
                    bmp.Save(sfd.FileName, ImageFormat.Jpeg);
                }
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (canvasPanel.Width <= 0 || canvasPanel.Height <= 0)
                return;

            Bitmap newBitmap = new Bitmap(canvasPanel.Width, canvasPanel.Height);
            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                g.Clear(Color.White);
                if (tempBitmap != null)
                {
                    g.DrawImage(tempBitmap, 0, 0);
                }
            }

            tempBitmap?.Dispose(); 
            tempBitmap = newBitmap;

            canvasPanel.Image?.Dispose(); 
            canvasPanel.Image = tempBitmap;
        }

        private void btnEraser_Click(object sender, EventArgs e)
        {
            currentActiveShape = EnumShape.ERASER;
            panel1.Invalidate();
            panel1.Refresh();
        }

        private void btnRotate_Click(object sender, EventArgs e)
        {
            rotationAngle = float.Parse(tbRotate.Text);
            tumpukanGambar.Last().SetRotationAngle(rotationAngle);
            drawAll();
        }
        private void drawAll()
        {

            using(Graphics g = Graphics.FromImage(tempBitmap))
            {
                g.Clear(Color.White);
                foreach (Shape item in tumpukanGambar)
                {
                    item.Draw(g);
                }
            }
            canvasPanel.Refresh();
        }

        private void btnMoveX_Click(object sender, EventArgs e)
        {
            if (int.TryParse(translationX.ToString(), out translationX) && int.TryParse(translationY.ToString(), out translationY))
            {
                tumpukanGambar.Last().SetTranslation(new Point(translationX, translationY));
                drawAll();
            }
        }

        private void btnMoveY_Click(object sender, EventArgs e)
        {
            if (int.TryParse(translationX.ToString(), out translationX) && int.TryParse(translationY.ToString(), out translationY))
            {
                tumpukanGambar.Last().SetTranslation(new Point(translationX, translationY));
                drawAll();
            }
        }

        private void tbMoveX_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Hanya izinkan angka, tanda minus, dan kontrol karakter
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-')
            {
                e.Handled = true;
                translationX = int.Parse(tbMoveX.Text);
            }

            // Izinkan tanda minus hanya di posisi pertama
            if (e.KeyChar == '-' && (sender as TextBox).SelectionStart != 0)
            {
                e.Handled = true;
                translationX = int.Parse(tbMoveX.Text);
            }

            // Tidak izinkan lebih dari satu tanda minus
            if (e.KeyChar == '-' && (sender as TextBox).Text.IndexOf('-') > -1)
            {
                e.Handled = true;
                translationX = int.Parse(tbMoveX.Text);
            }
        }

        private void tbMoveY_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Hanya izinkan angka, tanda minus, dan kontrol karakter
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-')
            {
                e.Handled = true;
                translationY = int.Parse(tbMoveY.Text);
            }

            // Izinkan tanda minus hanya di posisi pertama
            if (e.KeyChar == '-' && (sender as TextBox).SelectionStart != 0)
            {
                e.Handled = true;
                translationY = int.Parse(tbMoveY.Text);
            }

            // Tidak izinkan lebih dari satu tanda minus
            if (e.KeyChar == '-' && (sender as TextBox).Text.IndexOf('-') > -1)
            {
                e.Handled = true;
                translationY = int.Parse(tbMoveY.Text);
            }
        }

        private void tbMoveX_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                translationX = int.Parse(tbMoveX.Text);
            }catch(Exception ex)
            {
                translationX = 0;
            }
        }

        private void tbMoveY_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                translationY = int.Parse(tbMoveY.Text);
            }
            catch (Exception ex)
            {
                translationY = 0;
            }
        }

        private void btnTriangle_Click(object sender, EventArgs e)
        {
            currentActiveShape = EnumShape.TRIANGLE;
            panel1.Invalidate();
            panel1.Refresh();
        }

        private void btnScale_Click(object sender, EventArgs e)
        {
            if (tumpukanGambar.Count > 0)
            {
                if (float.TryParse(tbScale.Text, out float scaleFactor) && scaleFactor > 0)
                {
                    tumpukanGambar.Last().SetScaleFactor(scaleFactor);
                    drawAll();
                }
                else
                {
                    MessageBox.Show("Please enter a valid scale factor.");
                }
            }
        }

        private void tbScale_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Hanya izinkan angka, kontrol karakter, dan titik untuk desimal
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if(e.KeyChar == '.' && (sender as TextBox).Text.Contains('.'))
            {
                e.Handled = true;
            }
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void btnHexagon_Click(object sender, EventArgs e)
        {
            currentActiveShape = EnumShape.HEXAGON;
            panel1.Invalidate();
            panel1.Refresh();
        }

        private void btnStar_Click(object sender, EventArgs e)
        {
            currentActiveShape = EnumShape.STAR;
            panel1.Invalidate();
            panel1.Refresh();
        }
    }
}
