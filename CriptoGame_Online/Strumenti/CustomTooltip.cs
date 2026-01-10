using System.Drawing.Drawing2D;
using CriptoGame_Online.Strumenti;

public class CustomToolTip
{
    private readonly Dictionary<Control, string> _tips = new();
    private TooltipForm? _activeTip;
    private int _currentTooltipId;

    public int InitialDelay { get; set; } = 300;
    public int AutoPopDelay { get; set; } = 5000;

    public void SetToolTip(Control control, string text)
    {
        if (!_tips.ContainsKey(control))
        {
            _tips[control] = text;

            control.MouseEnter += async (s, e) => await ShowTooltipAsync(control);
            control.MouseLeave += (s, e) => HideTooltip();
            control.MouseMove += (s, e) => MoveTooltip();
        }
        else
        {
            _tips[control] = text;
        }
    }

    private async Task ShowTooltipAsync(Control ctl)
    {
        var tooltipId = ++_currentTooltipId;

        await Task.Delay(InitialDelay);

        // Verifica se è ancora valido mostrare il tooltip
        if (tooltipId != _currentTooltipId || !ctl.ClientRectangle.Contains(ctl.PointToClient(Cursor.Position)))
            return;

        HideTooltip();

        _activeTip = new TooltipForm(_tips[ctl])
        {
            Location = Cursor.Position + new Size(12, 12)
        };

        _activeTip.Show();
        _ = AutoHideAsync(tooltipId);
    }

    private async Task AutoHideAsync(int tooltipId)
    {
        await Task.Delay(AutoPopDelay);

        // Nascondi solo se è ancora lo stesso tooltip
        if (tooltipId == _currentTooltipId)
            HideTooltip();
    }

    private void MoveTooltip()
    {
        if (_activeTip != null)
        {
            _activeTip.Location = Cursor.Position + new Size(12, 12);
        }
    }

    private void HideTooltip()
    {
        _currentTooltipId++; // Invalida tutti i tooltip pendenti
        if (_activeTip != null)
        {
            _activeTip.Close();
            _activeTip = null;
        }
    }

    // -------------------------------------------------------------
    // INTERNAL FORM: Medieval Tooltip con colori e icone
    // -------------------------------------------------------------
    private class TooltipForm : Form
    {
        private readonly List<GameTextBox.Segment> _segments;
        private const int iconSize = 18;
        private static readonly Font TooltipFont = new Font("Georgia", 11, FontStyle.Regular);

        public TooltipForm(string text)
        {
            this.AutoScaleMode = AutoScaleMode.None;
            // PARSING del testo con LogSupport
            _segments = LogSupport.Parse(text);

            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            DoubleBuffered = true;
            TopMost = true;
            BackColor = Color.White;

            // Auto-size basato sul contenuto
            CalculateSize();

            Opacity = 0;
            _ = FadeInAsync();
        }

        private void CalculateSize()
        {
            using (Bitmap bmp = new Bitmap(1, 1))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                int maxLineWidth = 400;
                float currentX = 0;
                float totalHeight = TooltipFont.GetHeight(g);
                float maxWidth = 0;
                float lineHeight = TooltipFont.GetHeight(g);

                foreach (var seg in _segments)
                {
                    if (seg.IsIcon && seg.Icon != null)
                    {
                        float iconWidth = iconSize + 4;

                        if (currentX + iconWidth > maxLineWidth)
                        {
                            maxWidth = Math.Max(maxWidth, currentX);
                            currentX = 0;
                            totalHeight += lineHeight;
                        }

                        currentX += iconWidth;
                    }
                    else if (!string.IsNullOrEmpty(seg.Text))
                    {
                        // Gestisci \n per andare a capo
                        string[] lines = seg.Text.Split('\n');

                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (i > 0)
                            {
                                maxWidth = Math.Max(maxWidth, currentX);
                                currentX = 0;
                                totalHeight += lineHeight;
                            }

                            string line = lines[i];
                            string[] words = line.Split(' ');

                            foreach (var word in words)
                            {
                                if (string.IsNullOrWhiteSpace(word)) continue;

                                string drawWord = word + " ";
                                SizeF wordSize = g.MeasureString(drawWord, TooltipFont);

                                if (currentX + wordSize.Width > maxLineWidth)
                                {
                                    maxWidth = Math.Max(maxWidth, currentX);
                                    currentX = 0;
                                    totalHeight += lineHeight;
                                }

                                currentX += wordSize.Width;
                            }
                        }
                    }
                }

                maxWidth = Math.Max(maxWidth, currentX);

                // AGGIUNGI UN PADDING MINIMO
                Width = Math.Max(190, (int)maxWidth + 30); //Largezza
                Height = Math.Max(30, (int)totalHeight + 30); //Altezza
            }
        }

        private async Task FadeInAsync()
        {
            for (int i = 0; i <= 10; i++)
            {
                if (IsDisposed) break;
                try
                {
                    Opacity = i / 10.0;
                }
                catch (ObjectDisposedException) { break; }
                await Task.Delay(15);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new(0, 0, Width - 1, Height - 1);

            // Pergamena style background
            using (LinearGradientBrush bg = new(rect,
                Color.FromArgb(240, 225, 200),
                Color.FromArgb(225, 210, 185),
                90f))
            {
                g.FillRoundedRectangle(bg, rect, 12);
            }

            // Medieval border
            using (Pen p = new(Color.FromArgb(110, 70, 40), 2))
            {
                g.DrawRoundedRectangle(p, rect, 12);
            }

            // Renderizza segmenti con colori e icone
            float x = 15f;
            float y = 12f;
            float lineHeight = TooltipFont.GetHeight(g);
            int maxWidth = Width - 30;

            foreach (var seg in _segments)
            {
                if (seg.IsIcon && seg.Icon != null)
                {
                    float iconWidth = iconSize + 0; //Spazio tra icona e testo successivo

                    if (x + iconWidth > maxWidth)
                    {
                        x = 15f;
                        y += lineHeight;
                    }

                    int iconY = (int)(y + (lineHeight - iconSize) / 2 + 2); //Posizione altezza icona
                    g.DrawImage(seg.Icon, new Rectangle((int)x, iconY, iconSize, iconSize));
                    x += iconWidth;
                }
                else if (!string.IsNullOrEmpty(seg.Text))
                {
                    // Gestisci \n
                    string[] lines = seg.Text.Split('\n');

                    // Usa il colore del segmento invece del colore fisso
                    using (Brush brush = new SolidBrush(seg.Color))
                    {
                        foreach (var line in lines)
                        {
                            if (line != lines[0])
                            {
                                // Nuova riga
                                x = 15f;
                                y += lineHeight;
                            }

                            string[] words = line.Split(' ');
                            foreach (var word in words)
                            {
                                if (string.IsNullOrWhiteSpace(word)) continue;

                                string drawWord = word + " ";
                                SizeF wordSize = g.MeasureString(drawWord, TooltipFont);

                                if (x + wordSize.Width > maxWidth)
                                {
                                    x = 15f;
                                    y += lineHeight;
                                }

                                g.DrawString(drawWord, TooltipFont, brush, new PointF(x, y));
                                x += wordSize.Width;
                            }
                        }
                    }
                }
            }
        }
    }
}

// -------------------------------------------------------------
// EXTENSION METHOD per bordi arrotondati
// -------------------------------------------------------------
public static class GraphicsExtensions
{
    public static void FillRoundedRectangle(this Graphics g, Brush brush, Rectangle bounds, int radius)
    {
        using GraphicsPath path = RoundedRect(bounds, radius);
        g.FillPath(brush, path);
    }

    public static void DrawRoundedRectangle(this Graphics g, Pen pen, Rectangle bounds, int radius)
    {
        using GraphicsPath path = RoundedRect(bounds, radius);
        g.DrawPath(pen, path);
    }

    private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
    {
        int d = radius * 2;
        GraphicsPath path = new();
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}