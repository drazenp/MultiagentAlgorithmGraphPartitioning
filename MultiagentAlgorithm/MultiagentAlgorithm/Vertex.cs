namespace MultiagentAlgorithm
{
    public class Vertex
    {
        /// <summary>
        /// The ID of the vertex.
        /// </summary>
        private int ID { get; }

        /// <summary>
        /// The weight of the vertex.
        /// </summary>
        public int Weight { get;}

        /// <summary>
        /// The color set for the vertex.
        /// This value represent the partition.
        /// </summary>
        public int Color { get; set; }

        public Vertex(int id, int weight)
        {
            ID = id;
            Weight = weight;
        }
    }
}