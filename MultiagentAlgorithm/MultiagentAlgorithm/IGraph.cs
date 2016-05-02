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
        
        void InitializeGraph();

        /// <summary>
        /// Color each vertex of the graph at random forming k balanced sets.
        /// </summary>
        /// <param name="numberOfColors">The number of colors/partitions.</param>
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

        /// <summary>
        /// Counts the number of times that an edge joins vertices of different colors.
        /// </summary>
        /// <returns>The value of global cost function.</returns>
        int GetGlobalCostFunction();

        /// <summary>
        /// Find the worst adjacent vertex and move ant to it.
        /// </summary>
        /// <param name="ant">The ID of the ant.</param>
        /// <returns>The vertex on which ant moved.</returns>
        Vertex MoveAntToVertexWithLowestCost(int ant);

        /// <summary>
        /// Randomly choose an adjacent vertex and move on to it.
        /// </summary>
        /// <param name="ant">The ID of the ant.</param>
        /// <returns>The vertex on which ant moved.</returns>
        Vertex MoveAntToAnyAdjacentVertex(int ant);

        /// <summary>
        /// Fint best color for the ant's vertex and replace the old color with the new color.
        /// The best color is the color which increases the local cost.
        /// </summary>
        /// <param name="ant">The ID of the ant.</param>
        /// <returns>The vertex with the new color.</returns>
        Vertex ColorVertexWithBestColor(int ant);

        /// <summary>
        /// Randomly choose a color and set the new color for the ant's vertex.
        /// </summary>
        /// <param name="ant">The ID of the ant.</param>
        /// <param name="numberOfColors">The number of colors/partitions.</param>
        /// <returns>The vertex with the new color.</returns>
        Vertex ColorVertexWithRandomColor(int ant, int numberOfColors);

        /// <summary>
        /// To keep the balance, the algorithm chooses, from
        /// a set of s random vertices, one with the lowest value 
        /// of the local cost function -from those which have the new color- 
        /// and changes its color to the old color.
        /// </summary>
        /// <param name="numberOfRandomVertices">The number of vertices set to keep balance.</param>
        /// <param name="vertexWithAntID">The ID of the vertex on wich ant moved.</param>
        /// <param name="oldColor">The changed color of the vertex.</param>
        /// <param name="newColor">The new color of the vertex.</param>
        /// <returns>The vertex which has been changed to keep balance.</returns>
        Vertex KeepBalance(int numberOfRandomVertices, int vertexWithAntID, int oldColor, int newColor);

        /// <summary>
        /// Update local cost function for all chosen vertices 
        /// which has new color and for all adjacent vertices.
        /// </summary>
        void UpdateLocalCostFunction(Vertex vertexWithOldColor, Vertex vertexWithNewColor);
    }
}