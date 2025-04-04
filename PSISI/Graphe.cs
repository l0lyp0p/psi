using System;
using System.Collections.Generic;
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
            if (double.TryParse(valeur.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out resultat))
            {
                return resultat;
            }
            return 0.0;
        }
    }
}
