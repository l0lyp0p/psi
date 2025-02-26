using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSISI
{
    public class Lien
    {
        public Noeud De { get; set; } = new Noeud(); // Initialisation
        public Noeud Vers { get; set; } = new Noeud();
    }
}
