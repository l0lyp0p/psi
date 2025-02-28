using PSISI;

public class Graphe
{
    List<Noeud> noeuds;
    Dictionary<int, List<int>> listeAdjacence;
    int[,] matriceAdjacence;

    public Dictionary<int, List<int>> ListeAdjacence => listeAdjacence;

    /// <summary>
    /// Constructeur qui charge un graphe depuis un fichier.
    /// </summary>
    /// <param name="filename">Nom du fichier contenant le graphe.</param>
    public Graphe(string filename)
    {
        noeuds = new List<Noeud>();
        listeAdjacence = new Dictionary<int, List<int>>();
        matriceAdjacence = new int[0, 0];
        ChargerGrapheDepuisFichier(filename);
    }

    /// <summary>
    /// Effectue un parcours en largeur (BFS) du graphe.
    /// </summary>
    /// <param name="depart">Noeud de départ.</param>
    /// <returns>Liste des noeuds visités dans l'ordre.</returns>
    public List<int> BFS(int depart)
    {
        depart--;
        HashSet<int> visite = new HashSet<int>();
        Queue<int> file = new Queue<int>();
        List<int> ordreVisite = new List<int>();

        file.Enqueue(depart);
        visite.Add(depart);

        while (file.Count > 0)
        {
            int noeud = file.Dequeue();
            ordreVisite.Add(noeud + 1);

            foreach (int voisin in listeAdjacence[noeud + 1])
            {
                int voisinIndex = voisin - 1;
                if (!visite.Contains(voisinIndex))
                {
                    visite.Add(voisinIndex);
                    file.Enqueue(voisinIndex);
                }
            }
        }
        return ordreVisite;
    }

    /// <summary>
    /// Effectue un parcours en profondeur (DFS) du graphe.
    /// </summary>
    /// <param name="depart">Noeud de départ.</param>
    /// <returns>Liste des noeuds visités dans l'ordre.</returns>
    public List<int> DFS(int depart)
    {
        depart--;
        HashSet<int> visite = new HashSet<int>();
        Stack<int> pile = new Stack<int>();
        List<int> ordreVisite = new List<int>();

        pile.Push(depart);
        visite.Add(depart);

        while (pile.Count > 0)
        {
            int noeud = pile.Pop();
            ordreVisite.Add(noeud + 1);

            List<int> voisins = new List<int>(listeAdjacence[noeud + 1]);
            voisins.Reverse();

            foreach (int voisin in voisins)
            {
                int voisinIndex = voisin - 1;
                if (!visite.Contains(voisinIndex))
                {
                    visite.Add(voisinIndex);
                    pile.Push(voisinIndex);
                }
            }
        }
        return ordreVisite;
    }

    /// <summary>
    /// Vérifie si le graphe est connexe.
    /// </summary>
    /// <returns>Vrai si le graphe est connexe, faux sinon.</returns>
    public bool EstConnexe()
    {
        return noeuds.Count == 0 || BFS(1).Count == noeuds.Count;
    }

    /// <summary>
    /// Vérifie si le graphe contient un cycle.
    /// </summary>
    /// <returns>Vrai si le graphe contient un cycle, faux sinon.</returns>
    public bool ContientCycle()
    {
        HashSet<int> visite = new HashSet<int>();
        for (int i = 0; i < noeuds.Count; i++)
        {
            if (!visite.Contains(i) && DFSRecursifCycle(i, -1, visite))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Charge un graphe depuis un fichier.
    /// </summary>
    /// <param name="cheminFichier">Chemin du fichier contenant le graphe.</param>
    private void ChargerGrapheDepuisFichier(string cheminFichier)
    {
        string[] lignes = File.ReadAllLines(cheminFichier);
        List<string> donnees = new List<string>();

        foreach (string ligne in lignes)
        {
            if (!ligne.StartsWith("%") && !string.IsNullOrWhiteSpace(ligne))
            {
                donnees.Add(ligne);
            }
        }

        string[] enTete = donnees[0].Split(' ');
        int taille = int.Parse(enTete[0]);
        int aretes = int.Parse(enTete[2]);

        matriceAdjacence = new int[taille, taille];
        for (int i = 1; i <= taille; i++)
        {
            noeuds.Add(new Noeud { Id = i });
            listeAdjacence[i] = new List<int>();
        }

        for (int i = 1; i <= aretes; i++)
        {
            string[] arete = donnees[i].Split(' ');
            int u = int.Parse(arete[0]);
            int v = int.Parse(arete[1]);

            AjouterArete(u, v);
        }
    }

    /// <summary>
    /// Ajoute une arête au graphe.
    /// </summary>
    /// <param name="u">Premier noeud.</param>
    /// <param name="v">Deuxième noeud.</param>
    private void AjouterArete(int u, int v)
    {
        if (!listeAdjacence[u].Contains(v))
        {
            listeAdjacence[u].Add(v);
        }
        if (!listeAdjacence[v].Contains(u))
        {
            listeAdjacence[v].Add(u);
        }
        matriceAdjacence[u - 1, v - 1] = 1;
    }

    /// <summary>
    /// Recherche récursive pour détecter un cycle dans le graphe.
    /// </summary>
    /// <param name="noeud">Noeud actuel.</param>
    /// <param name="parent">Noeud parent.</param>
    /// <param name="visite">Ensemble des noeuds visités.</param>
    /// <returns>Vrai si un cycle est trouvé, faux sinon.</returns>
    private bool DFSRecursifCycle(int noeud, int parent, HashSet<int> visite)
    {
        visite.Add(noeud);
        foreach (int voisin in listeAdjacence[noeud + 1])
        {
            int voisinIndex = voisin - 1;
            if (!visite.Contains(voisinIndex))
            {
                if (DFSRecursifCycle(voisinIndex, noeud, visite))
                    return true;
            }
            else if (voisinIndex != parent)
            {
                return true;
            }
        }
        return false;
    }
}