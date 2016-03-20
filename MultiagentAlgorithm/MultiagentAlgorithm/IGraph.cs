using System.Collections.Generic;

namespace MultiagentAlgorithm
{
    public interface IGraph
    {
        /// <summary>
        /// The array of weights for each vertex in the graph.
        /// </summary>
        Vertex[] Vertices { get; }

        /// <summary>
        /// Number of edges read from files.
        /// This is not used in the application.
        /// </summary>
        int NumberOfEdges { get; set; }

        int[,] EdgesWeights { get; set; }

        void InitializeGraph();

        /// <summary>
        /// Color each vertex of the graph at random forming k balanced sets.
        /// </summary>
        /// <param name="numberOfColors">The number of colors/ants/partitions.</param>
        void ColorVerticesRandomly(int numberOfColors);

        /// <summary>
        /// Put each ant on a randomly chosen vertex.
        /// </summary>
        /// <param name="numberOfAnts">The number of ants.</param>
        void InitializeAnts(int numberOfAnts);

        /// <summary>
        /// For all vertices initialize local cost function.
        /// The ratio between the number of neighbors that have different 
        /// colors to the total number of neighbors.
        /// </summary>
        void CalculateLocalCostFunction();

        IEnumerable<Vertex> GetConnectedVertices(int vertexId);

        /// <summary>
        /// Counts the number of times that an edge joins vertices of different colors.
        /// </summary>
        /// <returns>The value of global cost function.</returns>
        double CalculateGlobalCostFunction();
    }
}