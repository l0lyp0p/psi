using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class GrapheVisualiseur : Form
{
    private Graphe _graphe;
    private PictureBox _pictureBox;
    private Bitmap _bitmap;
    private Dictionary<int, Point> _positions = new Dictionary<int, Point>();
    private const int NODE_RADIUS = 20;

    public GrapheVisualiseur(Graphe graphe)
    {
        _graphe = graphe;
        InitializeComponent();
        GenererPositionsAleatoires();
        RedessinerGraphe();
    }

    private void InitializeComponent()
    {
        this.Size = new Size(800, 600);
        _pictureBox = new PictureBox { Dock = DockStyle.Fill };
        this.Controls.Add(_pictureBox);
        this.Resize += (s, e) => RedessinerGraphe();
    }

    private void GenererPositionsAleatoires()
    {
        Random rand = new Random();
        foreach (var noeud in _graphe.ListeAdjacence.Keys)
        {
            _positions[noeud] = new Point(
                rand.Next(NODE_RADIUS, _pictureBox.Width - NODE_RADIUS),
                rand.Next(NODE_RADIUS, _pictureBox.Height - NODE_RADIUS)
            );
        }
    }

    private void RedessinerGraphe()
    {
        _bitmap = new Bitmap(_pictureBox.Width, _pictureBox.Height);
        using (Graphics g = Graphics.FromImage(_bitmap))
        {
            g.Clear(Color.White);
            DessinerLiens(g);
            DessinerNoeuds(g);
        }
        _pictureBox.Image = _bitmap;
    }

    private void DessinerLiens(Graphics g)
    {
        foreach (var (source, destinations) in _graphe.ListeAdjacence)
        {
            foreach (var dest in destinations)
            {
                g.DrawLine(Pens.Gray, _positions[source], _positions[dest]);
            }
        }
    }

    private void DessinerNoeuds(Graphics g)
    {
        foreach (var (id, pos) in _positions)
        {
            g.FillEllipse(Brushes.LightBlue,
                pos.X - NODE_RADIUS,
                pos.Y - NODE_RADIUS,
                NODE_RADIUS * 2,
                NODE_RADIUS * 2
            );
            g.DrawString(id.ToString(),
                new Font("Arial", 10),
                Brushes.Black,
                pos.X - 8,
                pos.Y - 8
            );
        }
    }
}