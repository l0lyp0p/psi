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

        public Noeud(int id, T value)
        {
            Id = id;
            Value = value;
        }
    }
}
﻿
