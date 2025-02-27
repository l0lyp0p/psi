using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class GrapheVisualiseur : Form
{
    private Graphe _graphe;
    private PictureBox _pictureBox;
    private Bitmap _bitmap;
    private Dictionary<int, PointF> _positions = new Dictionary<int, PointF>();
    private const int NODE_RADIUS = 20;

    public GrapheVisualiseur(Graphe graphe)
    {
        _graphe = graphe;
        InitializeComponent();
        CalculerPositionsEnGrille();
        RedessinerGraphe();
    }

    private void InitializeComponent()
    {
        this.Size = new Size(800, 600);
        _pictureBox = new PictureBox { Dock = DockStyle.Fill };
        this.Controls.Add(_pictureBox);
        this.Resize += (s, e) => RedessinerGraphe();
    }

    private void CalculerPositionsEnGrille()
    {
        int width = _pictureBox.Width;
        int height = _pictureBox.Height;
        int cols = (int)Math.Ceiling(Math.Sqrt(_graphe.ListeAdjacence.Count));
        int rows = (int)Math.Ceiling((double)_graphe.ListeAdjacence.Count / cols);
        int cellWidth = width / (cols + 1);
        int cellHeight = height / (rows + 1);
        int index = 0;

        foreach (var noeud in _graphe.ListeAdjacence.Keys)
        {
            int col = index % cols;
            int row = index / cols;
            _positions[noeud] = new PointF((col + 1) * cellWidth, (row + 1) * cellHeight);
            index++;
        }
    }

    private void RedessinerGraphe()
    {
        _bitmap = new Bitmap(_pictureBox.Width, _pictureBox.Height);
        using (Graphics g = Graphics.FromImage(_bitmap))
        {
            g.Clear(Color.WhiteSmoke);
            g.SmoothingMode = SmoothingMode.HighQuality;
            DessinerLiens(g);
            DessinerNoeuds(g);
        }
        _pictureBox.Image = _bitmap;
    }

    private void DessinerLiens(Graphics g)
    {
        using (Pen pen = new Pen(Color.FromArgb(220, 30, 30, 30), 2))
        {
            foreach (var (source, destinations) in _graphe.ListeAdjacence)
            {
                foreach (var dest in destinations)
                {
                    g.DrawLine(pen, _positions[source], _positions[dest]);
                }
            }
        }
    }

    private void DessinerNoeuds(Graphics g)
    {
        using (var nodeBrush = new SolidBrush(Color.FromArgb(240, 230, 140)))
        using (var textBrush = new SolidBrush(Color.Navy))
        {
            foreach (var (id, pos) in _positions)
            {
                g.FillEllipse(nodeBrush, pos.X - NODE_RADIUS, pos.Y - NODE_RADIUS, NODE_RADIUS * 2, NODE_RADIUS * 2);
                g.DrawEllipse(new Pen(Color.DarkSlateGray, 2), pos.X - NODE_RADIUS, pos.Y - NODE_RADIUS, NODE_RADIUS * 2, NODE_RADIUS * 2);
                g.DrawString(id.ToString(), new Font("Segoe UI", 12, FontStyle.Bold), textBrush, pos, new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
            }
        }
    }
}
