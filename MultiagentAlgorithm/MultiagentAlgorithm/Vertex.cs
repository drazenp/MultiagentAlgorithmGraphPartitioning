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

        /// <summary>
        /// The ratio between the number of neighbors that have different 
        /// colors to the total number of neighbors.
        /// </summary>
        public double LocalCost { get; set; }

        private Dictionary<int, int> _connectedEdges;
        /// <summary>
        /// The all other connected vertices with the vertex.
        /// </summary>
        public Dictionary<int, int> ConnectedEdges
        {
            get
            {
                if (_connectedEdges == null)
                {
                    _connectedEdges = new Dictionary<int, int>();
                }
                return _connectedEdges;
            }
            set { _connectedEdges = value; }
        }

        /// <summary>
        /// If vertices was marked as with lowest cost and ant moved to it as that.
        /// </summary>
        public bool LowestCost { get; set; }

        public Vertex(int id, int weight)
        {
            ID = id;
            Weight = weight;
        }
    }
}