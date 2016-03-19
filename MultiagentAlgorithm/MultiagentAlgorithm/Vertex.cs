using System;
using System.Collections.Generic;

namespace MultiagentAlgorithm
{
    public class Vertex
    {
        /// <summary>
        /// The ID of the vertex.
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// The weight of the vertex.
        /// </summary>
        public int Weight { get; }

        /// <summary>
        /// The color set for the vertex.
        /// This value represent the partition.
        /// </summary>
        public int Color { get; set; }

        private List<int> _ants;
        public List<int> Ants
        {
            get { return _ants ?? (_ants = new List<int>()); }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _ants = value;
            }
        }

        /// <summary>
        /// The ratio between the number of neighbors that have different 
        /// colors to the total number of neighbors.
        /// </summary>
        public double LocalCost { get; set; }

        public Vertex(int id, int weight)
        {
            ID = id;
            Weight = weight;
        }
    }
}