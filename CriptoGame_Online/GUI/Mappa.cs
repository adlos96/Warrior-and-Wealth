using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CriptoGame_Online.GUI
{
    public partial class Mappa : Form
    {
        // ===== VARIABILI DI STATO =====
        private MapCell[,] map;
        private int cellSize = 35;
        private Point offset = new Point(0, 0);

        // Per gestire il drag della mappa
        private Point lastMousePos;
        private bool isDragging = false;

        // Dimensioni della mappa in celle
        private const int MAP_WIDTH = 100;
        private const int MAP_HEIGHT = 100;

        // ===== ZOOM E LIMITI =====
        private const int MIN_CELL_SIZE = 10;
        private const int MAX_CELL_SIZE = 60;
        private const int ZOOM_STEP = 3;

        // ===== RISORSE GRAFICHE (create una volta sola) =====
        private Dictionary<TerrainType, Color> terrainColors;
        private Pen gridPen;
        private Font coordinateFont;
        private Font villageFont;

        // ===== UI CONTROLS =====
        private Label lblCoords;
        private Label lblZoom;
        private Button btnCenter;
        private Button btnRefresh;

        // ===== AUTO-REFRESH =====
        private System.Windows.Forms.Timer refreshTimer;
        private const int REFRESH_INTERVAL = 5000; // 5 secondi

        public Mappa()
        {
            InitializeComponent();
            InitializeDoubleBuffering();
            InitializeResources();
            InitializeMap(MAP_WIDTH, MAP_HEIGHT);
            InitializeUI();
            InitializeAutoRefresh();
        }

        // ===== INIZIALIZZAZIONE DOUBLE BUFFERING =====
        private void InitializeDoubleBuffering()
        {
            Panel_Mappa.GetType().GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic)
                ?.SetValue(Panel_Mappa, true, null);

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
        }

        // ===== INIZIALIZZAZIONE RISORSE GRAFICHE =====
        private void InitializeResources()
        {
            terrainColors = new Dictionary<TerrainType, Color>
            {
                { TerrainType.Grass, Color.FromArgb(34, 139, 34) },
                { TerrainType.Forest, Color.FromArgb(0, 100, 0) },
                { TerrainType.Mountain, Color.FromArgb(139, 137, 137) },
                { TerrainType.Water, Color.FromArgb(65, 105, 225) }
            };

            gridPen = new Pen(Color.FromArgb(80, 255, 255, 255), 1);
            coordinateFont = new Font("Arial", 8);
            villageFont = new Font("Arial", 9, FontStyle.Bold);
        }

        // ===== INIZIALIZZAZIONE AUTO-REFRESH =====
        private void InitializeAutoRefresh()
        {
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = REFRESH_INTERVAL;
            refreshTimer.Tick += RefreshTimer_Tick;
            refreshTimer.Start();
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            // Qui chiamerai il server per ottenere gli aggiornamenti
            RefreshMapData();
        }

        // ===== REFRESH DATI MAPPA DAL SERVER =====
        private void RefreshMapData()
        {
            try
            {
                // TODO: Implementa la chiamata al server per ottenere villaggi aggiornati
                // Esempio:
                // var updatedVillages = ServerAPI.GetAllVillages();
                // UpdateVillagesOnMap(updatedVillages);

                // Per ora simula un aggiornamento casuale (RIMUOVI IN PRODUZIONE)
                SimulateMapUpdate();

                Panel_Mappa.Invalidate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore refresh mappa: {ex.Message}");
            }
        }

        // ===== SIMULAZIONE AGGIORNAMENTO (SOLO PER TEST) =====
        private void SimulateMapUpdate()
        {
            Random random = new Random();

            // Aggiungi casualmente un nuovo villaggio (5% di probabilità)
            if (random.Next(100) < 5)
            {
                int x = random.Next(MAP_WIDTH);
                int y = random.Next(MAP_HEIGHT);

                if (map[x, y].Village == null)
                {
                    map[x, y].Village = new Village
                    {
                        PlayerName = $"Player{random.Next(100, 999)}",
                        Level = random.Next(1, 10)
                    };
                }
            }
        }

        // ===== METODO PUBBLICO: AGGIORNA VILLAGGI DALLA LISTA =====
        public void UpdateVillagesOnMap(List<VillageData> villages)
        {
            // Pulisci tutti i villaggi esistenti
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    map[x, y].Village = null;
                }
            }

            // Aggiungi i nuovi villaggi
            foreach (var villageData in villages)
            {
                if (villageData.X >= 0 && villageData.X < MAP_WIDTH &&
                    villageData.Y >= 0 && villageData.Y < MAP_HEIGHT)
                {
                    map[villageData.X, villageData.Y].Village = new Village
                    {
                        PlayerName = villageData.PlayerName,
                        Level = villageData.Level,
                        Population = villageData.Population,
                        Resources = villageData.Resources
                    };
                }
            }

            Panel_Mappa.Invalidate();
        }

        // ===== INIZIALIZZAZIONE UI =====
        private void InitializeUI()
        {
            // Label coordinate
            lblCoords = new Label
            {
                Location = new Point(10, 10),
                AutoSize = true,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(180, 0, 0, 0),
                Padding = new Padding(5),
                Text = "Coordinate: (0, 0)"
            };
            this.Controls.Add(lblCoords);
            lblCoords.BringToFront();

            // Label zoom
            lblZoom = new Label
            {
                Location = new Point(10, 40),
                AutoSize = true,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(180, 0, 0, 0),
                Padding = new Padding(5),
                Text = $"Zoom: {cellSize}px"
            };
            this.Controls.Add(lblZoom);
            lblZoom.BringToFront();

            // Pulsante centra villaggio
            btnCenter = new Button
            {
                Text = "🏠 Il Mio Villaggio",
                Location = new Point(10, 70),
                AutoSize = true,
                BackColor = Color.FromArgb(200, 70, 130, 180),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCenter.FlatAppearance.BorderSize = 0;
            btnCenter.Click += BtnCenter_Click;
            this.Controls.Add(btnCenter);
            btnCenter.BringToFront();

            // Pulsante refresh manuale
            btnRefresh = new Button
            {
                Text = "🔄 Aggiorna",
                Location = new Point(10, 105),
                AutoSize = true,
                BackColor = Color.FromArgb(200, 34, 139, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => RefreshMapData();
            this.Controls.Add(btnRefresh);
            btnRefresh.BringToFront();
        }

        private void BtnCenter_Click(object sender, EventArgs e)
        {
            var pos = FindPlayerVillage("Player1");
            if (pos.HasValue)
            {
                CenterOnCell(pos.Value.X, pos.Value.Y);
            }
            else
            {
                MessageBox.Show("Villaggio non trovato!", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Mappa_Load(object sender, EventArgs e)
        {
            // ===== REGISTRA EVENTI DEL PANEL =====
            Panel_Mappa.Paint += Panel_Paint;
            Panel_Mappa.MouseDown += Panel_MouseDown;
            Panel_Mappa.MouseMove += Panel_MouseMove;
            Panel_Mappa.MouseUp += Panel_MouseUp;
            Panel_Mappa.MouseWheel += Panel_MouseWheel;
            Panel_Mappa.MouseClick += Panel_MouseClick;

            Panel_Mappa.BackColor = Color.Black;
            PlaceVillageRandomly("Player1");
        }

        // ===== INIZIALIZZAZIONE MAPPA CON TERRENO PROCEDURALE =====
        private void InitializeMap(int width, int height)
        {
            map = new MapCell[width, height];
            Random random = new Random();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Genera terreno pseudo-casuale
                    double noise = random.NextDouble();
                    TerrainType terrain;

                    if (noise < 0.1)
                        terrain = TerrainType.Water;
                    else if (noise < 0.25)
                        terrain = TerrainType.Forest;
                    else if (noise < 0.35)
                        terrain = TerrainType.Mountain;
                    else
                        terrain = TerrainType.Grass;

                    map[x, y] = new MapCell
                    {
                        X = x,
                        Y = y,
                        Terrain = terrain,
                        Village = null
                    };
                }
            }
        }

        // ===== DISEGNO PRINCIPALE =====
        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int startX = Math.Max(0, -offset.X / cellSize);
            int startY = Math.Max(0, -offset.Y / cellSize);
            int endX = Math.Min(map.GetLength(0), startX + Panel_Mappa.Width / cellSize + 2);
            int endY = Math.Min(map.GetLength(1), startY + Panel_Mappa.Height / cellSize + 2);

            for (int x = startX; x < endX; x++)
                for (int y = startY; y < endY; y++)
                    DrawCell(g, map[x, y]);

            DrawGrid(g, startX, startY, endX, endY);
            DrawCoordinates(g, startX, startY, endX, endY);
            DrawMinimap(g);
        }

        // ===== DISEGNO SINGOLA CELLA =====
        private void DrawCell(Graphics g, MapCell cell)
        {
            int screenX = cell.X * cellSize + offset.X;
            int screenY = cell.Y * cellSize + offset.Y;

            Color cellColor = terrainColors.ContainsKey(cell.Terrain)
                ? terrainColors[cell.Terrain]
                : Color.ForestGreen;

            using (SolidBrush brush = new SolidBrush(cellColor))
            {
                g.FillRectangle(brush, screenX + 1, screenY + 1, cellSize - 1, cellSize - 1);
            }

            if (cell.Village != null)
            {
                using (SolidBrush villageBrush = new SolidBrush(Color.SaddleBrown))
                {
                    g.FillEllipse(villageBrush,
                        screenX + cellSize / 4,
                        screenY + cellSize / 4,
                        cellSize / 2,
                        cellSize / 2);
                }

                using (Pen villagePen = new Pen(Color.Yellow, 2))
                {
                    g.DrawEllipse(villagePen,
                        screenX + cellSize / 4,
                        screenY + cellSize / 4,
                        cellSize / 2,
                        cellSize / 2);
                }

                if (cellSize > 30)
                {
                    string displayName = cell.Village.PlayerName.Length > 8
                        ? cell.Village.PlayerName.Substring(0, 8)
                        : cell.Village.PlayerName;

                    SizeF textSize = g.MeasureString(displayName, villageFont);
                    g.DrawString(displayName, villageFont, Brushes.White,
                        screenX + (cellSize - textSize.Width) / 2,
                        screenY + cellSize - textSize.Height - 2);
                }
            }
        }

        // ===== DISEGNA GRIGLIA =====
        private void DrawGrid(Graphics g, int startX, int startY, int endX, int endY)
        {
            int mapStartX = 0 * cellSize + offset.X;
            int mapStartY = 0 * cellSize + offset.Y;
            int mapEndX = map.GetLength(0) * cellSize + offset.X;
            int mapEndY = map.GetLength(1) * cellSize + offset.Y;

            for (int x = startX; x <= endX; x++)
            {
                int screenX = x * cellSize + offset.X;

                if (screenX >= mapStartX && screenX <= mapEndX)
                {
                    int lineStartY = Math.Max(0, mapStartY);
                    int lineEndY = Math.Min(Panel_Mappa.Height, mapEndY);
                    g.DrawLine(gridPen, screenX, lineStartY, screenX, lineEndY);
                }
            }

            for (int y = startY; y <= endY; y++)
            {
                int screenY = y * cellSize + offset.Y;

                if (screenY >= mapStartY && screenY <= mapEndY)
                {
                    int lineStartX = Math.Max(0, mapStartX);
                    int lineEndX = Math.Min(Panel_Mappa.Width, mapEndX);
                    g.DrawLine(gridPen, lineStartX, screenY, lineEndX, screenY);
                }
            }
        }

        // ===== DISEGNA COORDINATE =====
        private void DrawCoordinates(Graphics g, int startX, int startY, int endX, int endY)
        {
            if (cellSize < 20) return;

            using (SolidBrush brush = new SolidBrush(Color.FromArgb(150, 255, 255, 255)))
            {
                for (int x = startX; x < endX; x += 10)
                {
                    int screenX = x * cellSize + offset.X;
                    g.DrawString(x.ToString(), coordinateFont, brush, screenX + 2, 2);
                }

                for (int y = startY; y < endY; y += 10)
                {
                    int screenY = y * cellSize + offset.Y;
                    g.DrawString(y.ToString(), coordinateFont, brush, 2, screenY + 2);
                }
            }
        }

        // ===== DISEGNA MINIMAP =====
        private void DrawMinimap(Graphics g)
        {
            int minimapSize = 150;
            int minimapX = Panel_Mappa.Width - minimapSize - 10;
            int minimapY = 10;

            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(180, 0, 0, 0)))
            {
                g.FillRectangle(bgBrush, minimapX, minimapY, minimapSize, minimapSize);
            }

            g.DrawRectangle(Pens.Gray, minimapX, minimapY, minimapSize, minimapSize);

            float scale = (float)minimapSize / MAP_WIDTH;

            // Disegna villaggi sulla minimap
            for (int x = 0; x < MAP_WIDTH; x += 2)
            {
                for (int y = 0; y < MAP_HEIGHT; y += 2)
                {
                    if (map[x, y].Village != null)
                    {
                        Color villageColor = map[x, y].Village.PlayerName == "Player1"
                            ? Color.Lime
                            : Color.Yellow;

                        using (SolidBrush villageBrush = new SolidBrush(villageColor))
                        {
                            g.FillRectangle(villageBrush,
                                minimapX + x * scale,
                                minimapY + y * scale,
                                scale * 2, scale * 2);
                        }
                    }
                }
            }

            // Mostra viewport corrente
            float viewX = -offset.X / cellSize * scale;
            float viewY = -offset.Y / cellSize * scale;
            float viewW = Panel_Mappa.Width / cellSize * scale;
            float viewH = Panel_Mappa.Height / cellSize * scale;

            using (Pen viewportPen = new Pen(Color.Red, 2))
            {
                g.DrawRectangle(viewportPen,
                    minimapX + viewX,
                    minimapY + viewY,
                    viewW, viewH);
            }
        }

        // ===== GESTIONE MOUSE: INIZIO DRAG =====
        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastMousePos = e.Location;
                Panel_Mappa.Cursor = Cursors.Hand;
            }
        }

        // ===== GESTIONE MOUSE: DRAGGING =====
        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                int deltaX = e.X - lastMousePos.X;
                int deltaY = e.Y - lastMousePos.Y;

                offset.X += deltaX;
                offset.Y += deltaY;

                ClampOffset();
                lastMousePos = e.Location;
                Panel_Mappa.Invalidate();
            }

            int mapX = (e.X - offset.X) / cellSize;
            int mapY = (e.Y - offset.Y) / cellSize;

            if (mapX >= 0 && mapX < map.GetLength(0) && mapY >= 0 && mapY < map.GetLength(1))
            {
                lblCoords.Text = $"Coordinate: ({mapX}, {mapY})";
                Panel_Mappa.Cursor = isDragging ? Cursors.Hand : Cursors.Cross;
            }
        }

        // ===== LIMITA OFFSET PER NON FAR USCIRE LA MAPPA =====
        private void ClampOffset()
        {
            int maxOffsetX = 0;
            int minOffsetX = Panel_Mappa.Width - (map.GetLength(0) * cellSize);
            int maxOffsetY = 0;
            int minOffsetY = Panel_Mappa.Height - (map.GetLength(1) * cellSize);

            offset.X = Math.Max(minOffsetX, Math.Min(maxOffsetX, offset.X));
            offset.Y = Math.Max(minOffsetY, Math.Min(maxOffsetY, offset.Y));
        }

        // ===== GESTIONE MOUSE: FINE DRAG =====
        private void Panel_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            Panel_Mappa.Cursor = Cursors.Cross;
        }

        // ===== GESTIONE ZOOM =====
        private void Panel_MouseWheel(object sender, MouseEventArgs e)
        {
            int oldCellSize = cellSize;

            if (e.Delta > 0)
                cellSize += ZOOM_STEP;
            else
                cellSize -= ZOOM_STEP;

            cellSize = Math.Max(MIN_CELL_SIZE, Math.Min(MAX_CELL_SIZE, cellSize));

            float scale = (float)cellSize / oldCellSize;
            offset.X = (int)((offset.X - e.X) * scale + e.X);
            offset.Y = (int)((offset.Y - e.Y) * scale + e.Y);

            ClampOffset();
            lblZoom.Text = $"Zoom: {cellSize}px";
            Panel_Mappa.Invalidate();
        }

        // ===== GESTIONE CLICK SU CELLA =====
        private void Panel_MouseClick(object sender, MouseEventArgs e)
        {
            int mapX = (e.X - offset.X) / cellSize;
            int mapY = (e.Y - offset.Y) / cellSize;

            if (mapX >= 0 && mapX < map.GetLength(0) && mapY >= 0 && mapY < map.GetLength(1))
            {
                MapCell cell = map[mapX, mapY];

                if (cell.Village != null)
                {
                    MessageBox.Show(
                        $"Villaggio: {cell.Village.PlayerName}\n" +
                        $"Livello: {cell.Village.Level}\n" +
                        $"Popolazione: {cell.Village.Population}\n" +
                        $"Risorse: {cell.Village.Resources}\n" +
                        $"Coordinate: ({mapX}, {mapY})",
                        "Informazioni Villaggio",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        $"Cella vuota\n" +
                        $"Coordinate: ({mapX}, {mapY})\n" +
                        $"Terreno: {cell.Terrain}",
                        "Informazioni Cella",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        // ===== POSIZIONAMENTO VILLAGGIO RANDOM =====
        public void PlaceVillageRandomly(string playerName)
        {
            Random random = new Random();
            int x, y;
            int attempts = 0;

            do
            {
                x = random.Next(0, map.GetLength(0));
                y = random.Next(0, map.GetLength(1));
                attempts++;

                if (attempts > 1000)
                {
                    MessageBox.Show("Impossibile trovare una posizione libera!");
                    return;
                }
            } while (map[x, y].Village != null);

            map[x, y].Village = new Village
            {
                PlayerName = playerName,
                Level = 1,
                Population = 100,
                Resources = 1000
            };

            CenterOnCell(x, y);
        }

        // ===== CENTRA MAPPA SU UNA CELLA =====
        private void CenterOnCell(int x, int y)
        {
            offset.X = Panel_Mappa.Width / 2 - x * cellSize - cellSize / 2;
            offset.Y = Panel_Mappa.Height / 2 - y * cellSize - cellSize / 2;

            ClampOffset();
            Panel_Mappa.Invalidate();
        }

        // ===== TROVA VILLAGGIO DEL GIOCATORE =====
        public Point? FindPlayerVillage(string playerName)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y].Village != null &&
                        map[x, y].Village.PlayerName == playerName)
                    {
                        return new Point(x, y);
                    }
                }
            }
            return null;
        }
    }

    // ===== CLASSI DI SUPPORTO =====

    public class MapCell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Village? Village { get; set; }
        public TerrainType Terrain { get; set; }
    }

    public class Village
    {
        public string PlayerName { get; set; }
        public int Level { get; set; }
        public int Population { get; set; }
        public int Resources { get; set; }
    }

    // Classe per ricevere dati dal server
    public class VillageData
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string PlayerName { get; set; }
        public int Level { get; set; }
        public int Population { get; set; }
        public int Resources { get; set; }
    }

    public enum TerrainType
    {
        Grass,
        Forest,
        Mountain,
        Water
    }
}