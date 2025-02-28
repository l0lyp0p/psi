using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using PSISI;
namespace TestPsi
{
    [TestClass]
    public class GrapheTests
    {
        private const string TestFile = "testGraphe.txt";

        [TestInitialize]
        public void Setup()
        {
            File.WriteAllLines(TestFile, new string[]
            {
                "5 5 4",
                "1 2",
                "2 3",
                "3 4",
                "4 5",
                "5 1"
            });
        }

        [TestMethod]
        public void TestChargementGraphe()
        {
            Graphe graphe = new Graphe(TestFile);
            Assert.AreEqual(5, graphe.ListeAdjacence.Count);
        }

        [TestMethod]
        public void TestBFS()
        {
            Graphe graphe = new Graphe(TestFile);
            List<int> ordre = graphe.BFS(1);
            Assert.IsNotNull(ordre);
            Assert.AreEqual(5, ordre.Count);
        }

        [TestMethod]
        public void TestDFS()
        {
            Graphe graphe = new Graphe(TestFile);
            List<int> ordre = graphe.DFS(1);
            Assert.IsNotNull(ordre);
            Assert.AreEqual(5, ordre.Count);
        }

        [TestMethod]
        public void TestEstConnexe()
        {
            Graphe graphe = new Graphe(TestFile);
            Assert.IsTrue(graphe.EstConnexe());
        }

        [TestMethod]
        public void TestContientCycle()
        {
            Graphe graphe = new Graphe(TestFile);
            Assert.IsTrue(graphe.ContientCycle());
        }
    }
}
