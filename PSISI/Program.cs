namespace PSISI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graphe graphe = new Graphe("soc-karate.mtx");
            Console.WriteLine("Parcours BFS : " + string.Join(", ", graphe.BFS(1)));
            Console.WriteLine("Graphe connexe : " + graphe.EstConnexe());
            Console.WriteLine("Présence de cycles : " + graphe.ContientCycle());
            Application.Run(new GrapheVisualiseur(graphe));
        }
    }
 }
