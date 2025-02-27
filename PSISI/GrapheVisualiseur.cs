using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

public class GrapheVisualiseur : Form
{
    private Graphe _graphe;
    private PictureBox _pictureBox;
    private Bitmap? _bitmap;
    private Dictionary<int, Point> _positions = new Dictionary<int, Point>();

    // Paramètres de visualisation
    private const int NODE_RADIUS = 20;
    private Point _center;
    private int _circleRadius;

    public GrapheVisualiseur(Graphe graphe)
    {
        _graphe = graphe;
        InitializeComponent();
        GenererPositionsCirculaires();
        RedessinerGraphe();
    }

    private void InitializeComponent()
    {
        this.ClientSize = new Size(800, 600);
        _center = new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2);
        _circleRadius = Math.Min(this.ClientSize.Width, this.ClientSize.Height) / 2 - 50;

        _pictureBox = new PictureBox { Dock = DockStyle.Fill };
        this.Controls.Add(_pictureBox);
        this.Resize += (s, e) => {
            _center = new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2);
            _circleRadius = Math.Min(this.ClientSize.Width, this.ClientSize.Height) / 2 - 50;
            GenererPositionsCirculaires();
            RedessinerGraphe();
        };
    }

    private void GenererPositionsCirculaires()
    {
        _positions.Clear();

        // Placer le nœud 1 au centre
        _positions[1] = _center;

        // Placer les autres nœuds sur le cercle
        var autresNoeuds = _graphe.ListeAdjacence.Keys
            .Where(n => n != 1)
            .OrderBy(n => n)
            .ToList();

        double angleStep = 2 * Math.PI / autresNoeuds.Count;
        double currentAngle = 0;

        foreach (var noeud in autresNoeuds)
        {
            int x = _center.X + (int)(_circleRadius * Math.Cos(currentAngle));
            int y = _center.Y + (int)(_circleRadius * Math.Sin(currentAngle));
            _positions[noeud] = new Point(x, y);
            currentAngle += angleStep;
        }
    }

    private void RedessinerGraphe()
    {
        _bitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
        using (Graphics g = Graphics.FromImage(_bitmap))
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            // Dessiner les liens
            foreach (var (source, destinations) in _graphe.ListeAdjacence)
            {
                foreach (var dest in destinations)
                {
                    if (_positions.TryGetValue(source, out var p1) &&
                        _positions.TryGetValue(dest, out var p2))
                    {
                        g.DrawLine(Pens.LightGray, p1, p2);
                    }
                }
            }

            // Dessiner les nœuds
            foreach (var (id, pos) in _positions)
            {
                g.FillEllipse(id == 1 ? Brushes.Red : Brushes.LightBlue,
                    pos.X - NODE_RADIUS,
                    pos.Y - NODE_RADIUS,
                    NODE_RADIUS * 2,
                    NODE_RADIUS * 2);

                g.DrawString(id.ToString(),
                    new Font("Arial", 10, FontStyle.Bold),
                    Brushes.Black,
                    pos.X - 10,
                    pos.Y - 10);
            }
        }
        _pictureBox.Image = _bitmap;
    }
}