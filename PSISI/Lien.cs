public class Lien<T>
{
    public Noeud<T> Depart { get; }
    public Noeud<T> Arrivee { get; }
    public double Poids { get; }
    public string LineId { get; } 

    public Lien(Noeud<T> depart, Noeud<T> arrivee, double poids, string lineId)
    {
        Depart = depart;
        Arrivee = arrivee;
        Poids = poids;
        LineId = lineId;
    }
}
