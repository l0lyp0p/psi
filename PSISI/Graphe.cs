using PSISI;

public class Graphe
{
    
    List<Noeud> noeuds;
    Dictionary<int, List<int>> listeAdjacence;
    int[,] matriceAdjacence;
    bool estSymetrique;

    public Dictionary<int, List<int>> ListeAdjacence { get;}
    public int[,] MatriceAdjacence { get;}


    // Constructeur 
    public Graphe(string filename)
    {
        noeuds = new List<Noeud>();
        listeAdjacence = new Dictionary<int, List<int>>();
        matriceAdjacence = new int[0, 0];
        estSymetrique = false;
        ChargerGrapheDepuisFichier(filename);
    }

    public List<int> BFS(int depart)
    {
        depart--; // 
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

    public bool EstConnexe()
    {
        return noeuds.Count == 0 || BFS(1).Count == noeuds.Count;
    }

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

        
        foreach (string ligne in lignes)
        {
            if (ligne.Contains("symmetric"))
            {
                estSymetrique = true;
                break;
            }
        }

        
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
            if (estSymetrique && u != v)
            {
                AjouterArete(v, u);
            }
        }
    }

    private void AjouterArete(int u, int v)
    {
        
        if (!listeAdjacence[u].Contains(v))
        {
            listeAdjacence[u].Add(v);
        }

        
        matriceAdjacence[u - 1, v - 1] = 1;
    }

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
