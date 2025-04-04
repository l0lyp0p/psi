using PSISI2;
using System;

namespace PSISI2
{
    public class Lien<T>
    {
        public Noeud<T> De { get; }
        public Noeud<T> Vers { get; }
        public double Poids { get; }
        public int LineId { get; } 

        public Lien(Noeud<T> de, Noeud<T> vers, double poids, int lineId)
        {
            De = de;
            Vers = vers;
            Poids = poids;
            LineId = lineId;
        }
    }
}
