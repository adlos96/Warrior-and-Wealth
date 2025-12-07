namespace CriptoGame_Online.Strumenti
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class GameTextBox : Panel
    {
        public class Segment
        {
            public string Text { get; set; }
            public Color Color { get; set; }
            public Image? Icon { get; set; }
            public bool IsIcon { get; set; }
        }

        private class Line
        {
            public List<Segment> Segments { get; set; } = new();
            public int RenderedHeight { get; set; }
        }

        private readonly List<Line> lines = new();
        private int lineHeight;
        private int totalContentHeight = 0;
        private const int iconSize = 17;

        public GameTextBox()
        {
            DoubleBuffered = true;
            AutoScroll = true;
            BackColor = Color.FromArgb(32, 26, 14);
            ForeColor = Color.White;
            Font = new Font("Consolas", 8.5f, FontStyle.Bold);
            lineHeight = (int)Font.GetHeight() + 5;
        }

        // NUOVO metodo principale
        public void AddLine(List<Segment> segments)
        {
            var line = new Line();
            line.Segments.AddRange(segments);
            lines.Add(line);

            CalculateLineHeight(line);
            totalContentHeight += line.RenderedHeight;
            AutoScrollMinSize = new Size(0, totalContentHeight);

            this.BeginInvoke(new Action(() =>
            {
                if (VerticalScroll.Visible)
                {
                    VerticalScroll.Value = VerticalScroll.Maximum;
                }
            }));

            Invalidate();
        }

        // Metodo per server (usa il parser)
        public void AddLineFromServer(string serverMessage)
        {
            var segments = LogSupport.Parse(serverMessage);
            AddLine(segments);
        }

        // Vecchio metodo per compatibilità
        public void AddLine(string text, Color color)
        {
            var segments = new List<Segment>
            {
                new Segment { Text = text, Color = color, IsIcon = false }
            };
            AddLine(segments);
        }

        private void CalculateLineHeight(Line line)
        {
            int maxWidth = ClientSize.Width - 25;
            float x = 5f;
            int wrappedLines = 1;

            using (var g = CreateGraphics())
            {
                foreach (var seg in line.Segments)
                {
                    if (seg.IsIcon && seg.Icon != null)
                    {
                        float iconWidth = iconSize + 4;
                        if (x + iconWidth > maxWidth)
                        {
                            wrappedLines++;
                            x = 5f;
                        }
                        x += iconWidth;
                    }
                    else if (!string.IsNullOrEmpty(seg.Text))
                    {
                        string[] words = seg.Text.Split(' ');
                        foreach (var word in words)
                        {
                            if (string.IsNullOrWhiteSpace(word)) continue;

                            string drawWord = word + " ";
                            float wordWidth = g.MeasureString(drawWord, Font).Width;

                            if (x + wordWidth > maxWidth)
                            {
                                wrappedLines++;
                                x = 5f;
                            }
                            x += wordWidth;
                        }
                    }
                }
            }

            line.RenderedHeight = wrappedLines * lineHeight;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.TranslateTransform(AutoScrollPosition.X, AutoScrollPosition.Y);

            int y = 0;
            int maxWidth = ClientSize.Width - 25;

            foreach (var line in lines)
            {
                float x = 5f;

                foreach (var seg in line.Segments)
                {
                    if (seg.IsIcon && seg.Icon != null)
                    {
                        float iconWidth = iconSize + 0;

                        if (x + iconWidth > maxWidth)
                        {
                            y += lineHeight;
                            x = 5f;
                        }

                        int iconY = y + (lineHeight - iconSize) / 2 - 2;
                        e.Graphics.DrawImage(seg.Icon, new Rectangle((int)x, iconY, iconSize, iconSize));
                        x += iconWidth;
                    }
                    else if (!string.IsNullOrEmpty(seg.Text))
                    {
                        string[] words = seg.Text.Split(' ');
                        using var brush = new SolidBrush(seg.Color);

                        foreach (var word in words)
                        {
                            if (string.IsNullOrWhiteSpace(word)) continue;

                            string drawWord = word + " ";
                            float wordWidth = e.Graphics.MeasureString(drawWord, Font).Width;

                            if (x + wordWidth > maxWidth)
                            {
                                y += lineHeight;
                                x = 5f;
                            }

                            e.Graphics.DrawString(drawWord, Font, brush, new PointF(x, y));
                            x += wordWidth;
                        }
                    }
                }

                y += lineHeight;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            totalContentHeight = 0;
            foreach (var line in lines)
            {
                CalculateLineHeight(line);
                totalContentHeight += line.RenderedHeight;
            }

            AutoScrollMinSize = new Size(0, totalContentHeight);
            Invalidate();
        }

        public void Clear()
        {
            lines.Clear();
            totalContentHeight = 0;
            AutoScrollMinSize = new Size(0, 0);
            Invalidate();
        }
    }
}