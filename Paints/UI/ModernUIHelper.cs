using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PaintCeunah.UI
{
    public static class ModernUIHelper
    {
        // Modern Color Palette
        public static readonly Color PrimaryBlue = Color.FromArgb(37, 99, 235);
        public static readonly Color SecondaryGreen = Color.FromArgb(16, 185, 129);
        public static readonly Color AccentOrange = Color.FromArgb(245, 158, 11);
        public static readonly Color BackgroundLight = Color.FromArgb(248, 250, 252);
        public static readonly Color SurfaceWhite = Color.FromArgb(255, 255, 255);
        public static readonly Color TextDark = Color.FromArgb(31, 41, 55);
        public static readonly Color BorderLight = Color.FromArgb(229, 231, 235);

        public static void ApplyModernButtonStyle(Button button, Color backgroundColor, Color textColor)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = backgroundColor;
            button.ForeColor = textColor;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            button.Cursor = Cursors.Hand;
            
            // Add rounded corners effect
            button.Region = CreateRoundedRegion(button.Size, 8);
        }

        public static void ApplyModernPanelStyle(Panel panel)
        {
            panel.BackColor = SurfaceWhite;
            panel.Padding = new Padding(10);
        }

        private static Region CreateRoundedRegion(Size size, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(size.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(size.Width - radius, size.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, size.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            return new Region(path);
        }
    }
}