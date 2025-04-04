using System;
using System.Collections.Generic;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace PSISI2
{
    public class GrapheVisualiseur : Form
    {
        private readonly Graphe<string> _graphe;
        private const int StationRadius = 4;
        private float _scale = 10000f;
        private float _offsetX = 0;
        private float _offsetY = 0;
        private float _minLon = float.MaxValue;
        private float _maxLon = float.MinValue;
        private float _minLat = float.MaxValue;
        private float _maxLat = float.MinValue;
        private List<Noeud<string>> _highlightedPath = new List<Noeud<string>>();

        // Dictionnaire des couleurs par ligne
        private static readonly Dictionary<string, Color> LineColors = new Dictionary<string, Color>
        {
            { "1", Color.Red },
            { "2", Color.Blue },
            { "3", Color.Green },
            { "4", Color.Orange },
            { "5", Color.Purple },
            { "6", Color.Brown },
            { "7", Color.Cyan },
            { "8", Color.Magenta },
            { "9", Color.DarkBlue },
            { "10", Color.DarkGreen },
            { "11", Color.Teal },
            { "12", Color.Maroon },
            { "13", Color.Olive },
            { "14", Color.Pink },
            { "corres", Color.Gray } // correspondances
        };

        public GrapheVisualiseur(Graphe<string> graphe)
        {
            _graphe = graphe;

            Text = "Visualisation du Métro de Paris";
            Width = 1200;
            Height = 800;
            DoubleBuffered = true;

            CalculerBornesGeo();
            Resize += (s, e) => { Invalidate(); };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            DrawLinks(g);
            DrawStations(g);
            DrawLegend(g);
        }

        private void DrawLinks(Graphics g)
        {
            foreach (var lien in _graphe.Liens)
            {
                var p1 = ConvertCoords(lien.Depart.Longitude, lien.Depart.Latitude);
                var p2 = ConvertCoords(lien.Arrivee.Longitude, lien.Arrivee.Latitude);

                Color color = LineColors.ContainsKey(lien.LineId) ? LineColors[lien.LineId] : Color.Black;
                using Pen pen = new Pen(color, lien.LineId == "corres" ? 1f : 2f);

                if (lien.LineId == "corres")
                {
                    pen.DashStyle = DashStyle.Dot;
                }

                g.DrawLine(pen, p1, p2);
            }
        }

        private void DrawStations(Graphics g)
        {
            foreach (var noeud in _graphe.Noeuds.Values)
            {
                var pos = ConvertCoords(noeud.Longitude, noeud.Latitude);
                Brush brush = _highlightedPath.Contains(noeud) ? Brushes.Red : Brushes.Black;
                g.FillEllipse(brush, pos.X - StationRadius, pos.Y - StationRadius, StationRadius * 2, StationRadius * 2);

                var nameSize = g.MeasureString(noeud.Valeur, Font);
                g.DrawString(noeud.Valeur, Font, Brushes.Black, pos.X + 5, pos.Y - nameSize.Height / 2);
            }
        }

        private void DrawLegend(Graphics g)
        {
            int x = 10;
            int y = 10;
            var font = new Font("Arial", 8);

            foreach (var kvp in LineColors.OrderBy(k => k.Key))
            {
                using Brush brush = new SolidBrush(kvp.Value);
                g.FillRectangle(brush, x, y, 20, 10);
                g.DrawRectangle(Pens.Black, x, y, 20, 10);
                g.DrawString(kvp.Key == "corres" ? "Correspondance" : $"Ligne {kvp.Key}", font, Brushes.Black, x + 25, y - 2);
                y += 15;
            }
        }

        private PointF ConvertCoords(double lon, double lat)
        {
            // Normalisation dans la fenêtre
            float normX = (float)((lon - _minLon) / (_maxLon - _minLon));
            float normY = (float)((lat - _minLat) / (_maxLat - _minLat));

            float canvasWidth = ClientSize.Width - 40;
            float canvasHeight = ClientSize.Height - 40;

            return new PointF(
                20 + normX * canvasWidth,
                20 + (1 - normY) * canvasHeight // inversé car Y descend à l’écran
            );
        }

        private void CalculerBornesGeo()
        {
            foreach (var noeud in _graphe.Noeuds.Values)
            {
                float lon = (float)noeud.Longitude;
                float lat = (float)noeud.Latitude;

                _minLon = Math.Min(_minLon, lon);
                _maxLon = Math.Max(_maxLon, lon);
                _minLat = Math.Min(_minLat, lat);
                _maxLat = Math.Max(_maxLat, lat);
            }
        }

        public void HighlightPath(List<Noeud<string>> path)
        {
            _highlightedPath = path;
            Invalidate(); // Forcer le redessin du graphe
        }
    }
}
