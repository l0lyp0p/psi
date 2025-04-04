using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace PSISI2
{
    /// <summary>
    /// Représente une structure de graphe avec des noeuds et des liens.
    /// </summary>
    /// <typeparam name="T">Le type de données stockées dans les noeuds.</typeparam>
    public class Graphe<T>
    {
        /// <summary>
        /// Dictionnaire des noeuds dans le graphe.
        /// </summary>
        public Dictionary<int, Noeud<T>> Noeuds { get; } = new Dictionary<int, Noeud<T>>();

        /// <summary>
        /// Liste des liens dans le graphe.
        /// </summary>
        public List<Lien<T>> Liens { get; } = new List<Lien<T>>();

        /// <summary>
        /// Initialise une nouvelle instance de la classe Graphe en lisant les noeuds et les liens à partir de fichiers CSV.
        /// </summary>
        /// <param name="cheminNoeuds">Chemin vers le fichier CSV des noeuds.</param>
        /// <param name="cheminArcs">Chemin vers le fichier CSV des liens.</param>
        public Graphe(string cheminNoeuds, string cheminArcs)
        {
            Dictionary<string, List<Noeud<T>>> noeudsParNom = new Dictionary<string, List<Noeud<T>>>();
            Dictionary<int, string> ligneParId = new Dictionary<int, string>();

            // Lecture du fichier des noeuds
            foreach (string ligne in File.ReadAllLines(cheminNoeuds).Skip(1))
            {
                string[] colonnes = ligne.Split(';');
                if (colonnes.Length < 5)
                {
                    continue;
                }

                bool hasId = int.TryParse(colonnes[0], out int idStation);
                if (!hasId)
                {
                    continue;
                }

                T nomStation = (T)Convert.ChangeType(colonnes[2], typeof(T));
                string ligneId = colonnes[1];
                double longitude = ConvertirEnDouble(colonnes[3]);
                double latitude = ConvertirEnDouble(colonnes[4]);

                Noeud<T> noeud = new Noeud<T>(idStation, nomStation)
                {
                    Longitude = longitude,
                    Latitude = latitude
                };

                AjouterNoeud(noeud);
                ligneParId[idStation] = ligneId;

                if (!noeudsParNom.ContainsKey(nomStation.ToString()))
                {
                    noeudsParNom[nomStation.ToString()] = new List<Noeud<T>>();
                }
                noeudsParNom[nomStation.ToString()].Add(noeud);
            }

            // Lecture du fichier des liens
            foreach (string ligne in File.ReadAllLines(cheminArcs).Skip(1))
            {
                string[] colonnes = ligne.Split(';');
                if (colonnes.Length < 6)
                {
                    continue;
                }

                bool isIdValid = int.TryParse(colonnes[0], out int id);
                bool hasPrecedent = int.TryParse(colonnes[2], out int precedentId);
                bool hasSuivant = int.TryParse(colonnes[3], out int suivantId);
                double poids = ConvertirEnDouble(colonnes[4]);

                if (!isIdValid || !Noeuds.ContainsKey(id))
                {
                    continue;
                }
                Noeud<T> stationActuelle = Noeuds[id];
                string ligneId;
                if (ligneParId.ContainsKey(id))
                {
                    ligneId = ligneParId[id];
                }
                else
                {
                    ligneId = "inconnu";
                }

                if (hasPrecedent && Noeuds.ContainsKey(precedentId))
                {
                    Noeud<T> precedent = Noeuds[precedentId];
                    string lignePrec;
                    if (ligneParId.ContainsKey(precedentId))
                    {
                        lignePrec = ligneParId[precedentId];
                    }
                    else
                    {
                        lignePrec = ligneId;
                    }

                    AjouterLien(new Lien<T>(precedent, stationActuelle, poids, lignePrec));
                    AjouterLien(new Lien<T>(stationActuelle, precedent, poids, lignePrec));
                }

                if (hasSuivant && Noeuds.ContainsKey(suivantId))
                {
                    Noeud<T> suivant = Noeuds[suivantId];
                    string ligneSuiv;
                    if (ligneParId.ContainsKey(suivantId))
                    {
                        ligneSuiv = ligneParId[suivantId];
                    }
                    else
                    {
                        ligneSuiv = ligneId;
                    }

                    AjouterLien(new Lien<T>(stationActuelle, suivant, poids, ligneSuiv));
                    AjouterLien(new Lien<T>(suivant, stationActuelle, poids, ligneSuiv));
                }
            }

            // Correspondances entre les stations de même nom
            const double tempsCorrespondance = 5.0;
            foreach (KeyValuePair<string, List<Noeud<T>>> kvp in noeudsParNom)
            {
                List<Noeud<T>> liste = kvp.Value;
                for (int i = 0; i < liste.Count; i++)
                {
                    for (int j = i + 1; j < liste.Count; j++)
                    {
                        AjouterLien(new Lien<T>(liste[i], liste[j], tempsCorrespondance, "corres"));
                        AjouterLien(new Lien<T>(liste[j], liste[i], tempsCorrespondance, "corres"));
                    }
                }
            }
        }

        /// <summary>
        /// Ajoute un noeud au graphe.
        /// </summary>
        /// <param name="noeud">Le noeud à ajouter.</param>
        public void AjouterNoeud(Noeud<T> noeud)
        {
            if (!Noeuds.ContainsKey(noeud.Id))
            {
                Noeuds[noeud.Id] = noeud;
            }
        }

        /// <summary>
        /// Ajoute un lien au graphe.
        /// </summary>
        /// <param name="lien">Le lien à ajouter.</param>
        public void AjouterLien(Lien<T> lien)
        {
            Liens.Add(lien);
        }

        /// <summary>
        /// Convertit une chaîne en double, en gérant le formatage spécifique à la culture.
        /// </summary>
        /// <param name="valeur">La valeur chaîne à convertir.</param>
        /// <returns>La valeur double convertie.</returns>
        private double ConvertirEnDouble(string valeur)
        {
            double resultat;
            if (double.TryParse(valeur.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out resultat))
            {
                return resultat;
            }
            return 0.0;
        }

        /// <summary>
        /// Reconstruit le chemin à partir des prédécesseurs.
        /// </summary>
        /// <param name="predecessors">Dictionnaire des prédécesseurs.</param>
        /// <param name="start">Noeud de départ.</param>
        /// <param name="end">Noeud de fin.</param>
        /// <returns>La liste des noeuds formant le chemin.</returns>
        private List<Noeud<T>> ReconstruireChemin(Dictionary<Noeud<T>, Noeud<T>> predecessors, Noeud<T> start, Noeud<T> end)
        {
            List<Noeud<T>> path = new List<Noeud<T>>();
            for (Noeud<T> at = end; at != null; at = predecessors.GetValueOrDefault(at))
            {
                path.Insert(0, at);
            }

            if (path.Count > 0 && path[0] == start)
            {
                return path;
            }
            else
            {
                return new List<Noeud<T>>(); // Chemin non trouvé
            }
        }

        /// <summary>
        /// Trouve le plus court chemin en utilisant l'algorithme de Dijkstra.
        /// </summary>
        /// <param name="start">Noeud de départ.</param>
        /// <param name="end">Noeud de fin.</param>
        /// <returns>Le chemin et le temps total.</returns>
        public (List<Noeud<T>> Path, double TotalTime) Dijkstra(Noeud<T> start, Noeud<T> end)
        {
            Dictionary<Noeud<T>, double> distances = new Dictionary<Noeud<T>, double>();
            Dictionary<Noeud<T>, Noeud<T>> predecessors = new Dictionary<Noeud<T>, Noeud<T>>();
            PriorityQueue<Noeud<T>, double> priorityQueue = new PriorityQueue<Noeud<T>, double>();

            foreach (Noeud<T> node in Noeuds.Values)
            {
                distances[node] = double.PositiveInfinity;
            }
            distances[start] = 0;
            priorityQueue.Enqueue(start, 0);

            while (priorityQueue.Count > 0)
            {
                Noeud<T> current = priorityQueue.Dequeue();
                if (current == end)
                {
                    break;
                }

                foreach (Lien<T> lien in Liens.Where(l => l.Depart == current))
                {
                    Noeud<T> neighbor = lien.Arrivee;
                    double tentative = distances[current] + lien.Poids;
                    if (tentative < distances[neighbor])
                    {
                        distances[neighbor] = tentative;
                        predecessors[neighbor] = current;
                        priorityQueue.Enqueue(neighbor, tentative);
                    }
                }
            }

            // Reconstruction du chemin
            List<Noeud<T>> path = ReconstruireChemin(predecessors, start, end);
            return (path, distances[end]);
        }

        /// <summary>
        /// Trouve le plus court chemin en utilisant l'algorithme de Bellman-Ford.
        /// </summary>
        /// <param name="start">Noeud de départ.</param>
        /// <param name="end">Noeud de fin.</param>
        /// <returns>Le chemin et le temps total.</returns>
        public (List<Noeud<T>> Path, double TotalTime) BellmanFord(Noeud<T> start, Noeud<T> end)
        {
            Dictionary<Noeud<T>, double> distances = new Dictionary<Noeud<T>, double>();
            Dictionary<Noeud<T>, Noeud<T>> predecessors = new Dictionary<Noeud<T>, Noeud<T>>();

            foreach (Noeud<T> node in Noeuds.Values)
            {
                distances[node] = double.PositiveInfinity;
            }
            distances[start] = 0;

            for (int i = 0; i < Noeuds.Count - 1; i++)
            {
                foreach (Lien<T> lien in Liens)
                {
                    if (distances[lien.Depart] + lien.Poids < distances[lien.Arrivee])
                    {
                        distances[lien.Arrivee] = distances[lien.Depart] + lien.Poids;
                        predecessors[lien.Arrivee] = lien.Depart;
                    }
                }
            }

            // Vérification des cycles négatifs (optionnel)
            foreach (Lien<T> lien in Liens)
            {
                if (distances[lien.Depart] + lien.Poids < distances[lien.Arrivee])
                {
                    throw new InvalidOperationException("Cycle négatif détecté");
                }
            }

            return (ReconstruireChemin(predecessors, start, end), distances[end]);
        }

        /// <summary>
        /// Trouve le plus court chemin en utilisant l'algorithme de Floyd-Warshall.
        /// </summary>
        /// <param name="start">Noeud de départ.</param>
        /// <param name="end">Noeud de fin.</param>
        /// <returns>Le chemin et le temps total.</returns>
        public (List<Noeud<T>> Path, double TotalTime) FloydWarshall(Noeud<T> start, Noeud<T> end)
        {
            List<Noeud<T>> nodes = Noeuds.Values.ToList();
            int n = nodes.Count;
            double[,] dist = new double[n, n];
            int[,] next = new int[n, n];

            // Initialisation
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    dist[i, j] = i == j ? 0 : double.PositiveInfinity;
                    next[i, j] = -1;
                }
            }

            Dictionary<Noeud<T>, int> nodeIndex = nodes.Select((node, idx) => new { node, idx }).ToDictionary(x => x.node, x => x.idx);

            foreach (Lien<T> lien in Liens)
            {
                int u = nodeIndex[lien.Depart];
                int v = nodeIndex[lien.Arrivee];
                if (dist[u, v] > lien.Poids)
                {
                    dist[u, v] = lien.Poids;
                    next[u, v] = v;
                }
            }

            // Algorithme
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (dist[i, j] > dist[i, k] + dist[k, j])
                        {
                            dist[i, j] = dist[i, k] + dist[k, j];
                            next[i, j] = next[i, k];
                        }
                    }
                }
            }

            // Reconstruction
            int startIdx = nodeIndex[start];
            int endIdx = nodeIndex[end];
            if (next[startIdx, endIdx] == -1)
            {
                return (new List<Noeud<T>>(), dist[startIdx, endIdx]);
            }

            List<Noeud<T>> path = new List<Noeud<T>>();
            int current = startIdx;
            while (current != endIdx)
            {
                path.Add(nodes[current]);
                current = next[current, endIdx];
            }
            path.Add(nodes[endIdx]);

            return (path, dist[startIdx, endIdx]);
        }

        /// <summary>
        /// Visualise le chemin trouvé.
        /// </summary>
        /// <param name="chemin">Le chemin à visualiser.</param>
        public void VisualiserChemin(List<Noeud<T>> chemin)
        {
            Console.WriteLine("Chemin parcouru :");
            for (int i = 0; i < chemin.Count; i++)
            {
                Noeud<T> station = chemin[i];
                Console.WriteLine($"{i + 1}. {station.Valeur} (ID: {station.Id})");
                if (i < chemin.Count - 1)
                {
                    Lien<T> lien = Liens.First(l => l.Depart == station && l.Arrivee == chemin[i + 1]);
                    Console.WriteLine($"   ↓ Ligne {lien.LineId} ({lien.Poids} minutes)");
                }
            }
        }
    }
}
