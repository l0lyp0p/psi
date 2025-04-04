using System;

namespace PSISI2
{
    /// <summary>
    /// Classe générique représentant un nœud.
    /// </summary>
    public class Noeud<T>
    {
        public int Id { get; }
        public T Value { get; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    
        public Noeud(int id, T value)
        {
            Id = id;
            Value = value;
        }
    }
}
﻿
