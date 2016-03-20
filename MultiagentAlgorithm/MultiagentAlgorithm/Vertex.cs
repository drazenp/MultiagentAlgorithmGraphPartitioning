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

        private int _color;
        /// <summary>
        /// The color set for the vertex.
        /// This value represent the partition.
        /// </summary>
        public int Color
        {
            get { return _color; }
            set
            {
                // Save the current color as old on each change.                                                   
                OldColor = _color;
                _color = value;
            }
        }

        /// <summary>
        /// The color which was set before the current color (if changed);
        /// </summary>
        public int? OldColor { get; private set; }

        // TODO: Remove this property with refactoring.
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

        public Dictionary<int, int> ConnectedEdges { get; }

        /// <summary>
        /// If vertices was marked as with lowest cost and ant moved to it as that.
        /// </summary>
        public bool LowestCost { get; set; }

        public Vertex(int id, int weight, Dictionary<int, int> connectedEdges)
        {
            ID = id;
            Weight = weight;
            ConnectedEdges = connectedEdges;
        }

        /// <summary>
        /// Reset old color and lowest cost.
        /// </summary>
        public void Reset()
        {
            LowestCost = false;
            OldColor = null;
        }
    }
}