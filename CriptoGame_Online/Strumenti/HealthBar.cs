using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Warrior_and_Wealth.Strumenti
{
    public class StatBar : Control
    {
        private int _value = 100;
        private int _maxValue = 100;

        [Category("Behavior")]
        [Description("Valore corrente della barra")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Value
        {
            get => _value;
            set
            {
                _value = Math.Clamp(value, 0, _maxValue);
                Invalidate();
            }
        }

        [Category("Behavior")]
        [Description("Valore massimo della barra")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int MaxValue
        {
            get => _maxValue;
            set
            {
                if (value > 0)
                {
                    _maxValue = value;
                    _value = Math.Clamp(_value, 0, _maxValue);
                    Invalidate();
                }
            }
        }

        [Category("Appearance")]
        [Description("Colore della barra principale")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color BarColor { get; set; } = Color.LimeGreen;

        [Category("Appearance")]
        [Description("Colore dello sfondo della barra")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color BackColorBar { get; set; } = Color.DarkGray;

        [Category("Appearance")]
        [Description("Colore del bordo della barra")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color BorderColor { get; set; } = Color.Black;

        [Category("Appearance")]
        [Description("Etichetta da mostrare sulla barra (es. HP, DEF)")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Label { get; set; } = "HP";

        [Category("Appearance")]
        [Description("Mostra o nasconde il testo della percentuale/valore")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowText { get; set; } = true;

        [Category("Appearance")]
        [Description("Raggio degli angoli della barra esterna")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Radius_Border { get; set; } = 12;

        [Category("Appearance")]
        [Description("Raggio degli angoli della barra interna")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Radius_Internal { get; set; } = 10;

        public StatBar()
        {
            DoubleBuffered = true;
            Size = new Size(200, 25);
            Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }

        private GraphicsPath GetRoundedRect(Rectangle rect, int radius)
        {
            int r = Math.Max(2, radius); // Radius minimo 2 per evitare eccezioni
            int diameter = r * 2;

            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        public void DrawOn(Graphics g, Rectangle rect)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int padding = 2;

            // Sfondo
            using (var backBrush = new SolidBrush(BackColorBar))
            using (var path = GetRoundedRect(rect, Radius_Border))
                g.FillPath(backBrush, path);

            // Percentuale
            float percent = _maxValue > 0 ? (float)_value / _maxValue : 0f;
            int barWidth = (int)((rect.Width - 2 * padding) * percent);
            int barHeight = rect.Height - 2 * padding;

            barWidth = Math.Max(barWidth, 2);
            barHeight = Math.Max(barHeight, 2);
            int effectiveRadius = Math.Min(Radius_Internal, Math.Min(barWidth / 2, barHeight / 2));

            Rectangle barRect = new Rectangle(rect.X + padding, rect.Y + padding, barWidth, barHeight);

            using (var barBrush = new LinearGradientBrush(barRect, ControlPaint.Dark(BarColor, 0.2f), BarColor, LinearGradientMode.Horizontal))
            using (var path = GetRoundedRect(barRect, effectiveRadius))
                g.FillPath(barBrush, path);

            // Bordo
            using (var pen = new Pen(ControlPaint.Dark(BorderColor, 0.3f), 2))
            using (var path = GetRoundedRect(rect, Radius_Border))
                g.DrawPath(pen, path);

            // Testo
            if (ShowText)
            {
                string text = $"{Label}: {_value}/{_maxValue}";
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                using var textBrush = new SolidBrush(Color.WhiteSmoke);
                g.DrawString(text, Font, textBrush, rect, sf);
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int padding = 2;

            // Sfondo barra
            using (var backBrush = new SolidBrush(BackColorBar))
            using (var path = GetRoundedRect(ClientRectangle, Radius_Border))
            {
                g.FillPath(backBrush, path);
            }

            // Percentuale
            float percent = _maxValue > 0 ? (float)_value / _maxValue : 0f;
            int barWidth = (int)((Width - 2 * padding) * percent);
            int barHeight = Height - 2 * padding;

            // Evita larghezze/altezza troppo piccole
            barWidth = Math.Max(barWidth, 2);
            barHeight = Math.Max(barHeight, 2);

            // Radius interno adattivo
            int effectiveRadius = Math.Min(Radius_Internal, Math.Min(barWidth / 2, barHeight / 2));
            effectiveRadius = Math.Max(effectiveRadius, 2);

            // Rettangolo interno della barra
            Rectangle barRect = new Rectangle(padding, padding, barWidth, barHeight);

            // Riempimento con gradiente orizzontale
            using (var barBrush = new LinearGradientBrush(
                barRect,
                ControlPaint.Dark(BarColor, 0.2f),
                BarColor,
                LinearGradientMode.Horizontal))
            using (var path = GetRoundedRect(barRect, effectiveRadius))
            {
                g.FillPath(barBrush, path);
            }

            // Bordo esterno
            using (var pen = new Pen(ControlPaint.Dark(BorderColor, 0.3f), 2))
            using (var path = GetRoundedRect(ClientRectangle, Radius_Border))
            {
                g.DrawPath(pen, path);
            }

            // Testo centrale
            if (ShowText)
            {
                string text = $"{Label}: {_value}/{_maxValue}";
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                using var textBrush = new SolidBrush(Color.WhiteSmoke);
                g.DrawString(text, Font, textBrush, ClientRectangle, sf);
            }
        }
    }
}
