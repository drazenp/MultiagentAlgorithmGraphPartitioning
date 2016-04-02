using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiagentAlgorithm
{
    public abstract class BaseGraph : IGraph
    {
        protected IDataLoader DataLoader;

        protected Random Rnd;

        /// <summary>
        /// The list of weights for each vertex in the graph.
        /// </summary>
        public IList<Vertex> Vertices { get; protected set; }

        /// <summary>
        /// The list of all chosen vertices - the vertices which the 
        /// changed color in the current iteration.
        /// </summary>
        private IList<Vertex> _chosenVertices;
        private IList<Vertex> ChosenVertices
        {
            get
            {
                if (_chosenVertices == null)
                {
                    _chosenVertices = new List<Vertex>();
                }
                return _chosenVertices;
            }
        }

        /// <summary>
        /// Number of edges read from files.
        /// This is not used in the application.
        /// </summary>
        public int NumberOfEdges { get; set; }

        public Dictionary<int, int> Ants;

        public int MaxNumberOfAdjacentVertices;

        public abstract void InitializeGraph();

        /// <summary>
        /// Color each vertex of the graph at random forming k balanced sets.
        /// </summary>
        /// <param name="numberOfColors">The number of colors/partitions.</param>
        public void ColorVerticesRandomly(int numberOfColors)
        {
            for (var i = 0; i < Vertices.Count;)
            {
                var randomColors = Enumerable.Range(1, numberOfColors).Shuffle(Rnd);
                foreach (var color in randomColors)
                {
                    Vertices[i].Color = color;
                    i++;
                    if (i >= Vertices.Count)
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Put each ant on a randomly chosen vertex.
        /// </summary>
        /// <param name="numberOfAnts">The number of ants.</param>
        public void InitializeAnts(int numberOfAnts)
        {
            Ants = new Dictionary<int, int>();
            var counter = 0;
            foreach (var vertex in Vertices.Shuffle(Rnd))
            {
                Ants.Add(counter, vertex.ID);
                counter++;
                if (counter >= numberOfAnts)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// For all vertices initialize local cost function.
        /// The ratio between the number of neighbors that have different 
        /// colors to the total number of neighbors.
        /// </summary>
        public void CalculateLocalCostFunction()
        {
            foreach (var vertex in Vertices)
            {
                CalculateLocalCostFunctionForVertex(vertex);
            }
        }

        /// <summary>
        /// Calculate local cost function for the vertex.
        /// </summary>
        /// <param name="vertex">The vertex to calculate local cost function.</param>
        private void CalculateLocalCostFunctionForVertex(Vertex vertex)
        {
            var connectedVertices = vertex.ConnectedEdges.Select(connectedEdge => Vertices[connectedEdge.Key]).ToList();

            var verticesCount = connectedVertices.Count();
            var differentColorCount = connectedVertices.Count(x => x.Color != vertex.Color);

            if (verticesCount == differentColorCount)
            {
                vertex.LocalCost = 0;
            }
            else if (differentColorCount == 0)
            {
                vertex.LocalCost = 1;
            }
            else
            {
                vertex.LocalCost = differentColorCount / (double)verticesCount;
            }
        }

        /// <summary>
        /// Counts the number of times that an edge joins vertices of different colors.
        /// </summary>
        /// <returns>The value of global cost function.</returns>
        public int GetGlobalCostFunction()
        {
            LoggerHelper.LogVertices(Vertices);
            var globalCost = 0;

            foreach (var vertex in Vertices)
            {
                var differentColorCount = vertex.ConnectedEdges.Select(connectedEdge => Vertices[connectedEdge.Key]).Count(x => x.Color != vertex.Color);
                globalCost += differentColorCount;
            }

            return globalCost;
        }

        /// <summary>
        /// Find the worst adjacent vertex and move ant to it.
        /// </summary>
        /// <param name="ant">The ID of the ant.</param>
        /// <returns>The color of the vertex on which ant moved.</returns>
        public int MoveAntToVertexWithLowestCost(int ant)
        {
            var vertex = Vertices[Ants[ant]];
            var worstAdjacentVertex = vertex.ConnectedEdges.First().Key;
            var lowestLocalCost = Vertices[worstAdjacentVertex].LocalCost;
            foreach (var connectedVertex in vertex.ConnectedEdges.Keys.Skip(1))
            {
                var localCost = Vertices[connectedVertex].LocalCost;
                if (localCost < lowestLocalCost)
                {
                    lowestLocalCost = localCost;
                    worstAdjacentVertex = connectedVertex;
                }
            }

            // Move ant to the worst adjacent vertex.
            var worstVertex = Vertices[worstAdjacentVertex];
            Ants[ant] = worstAdjacentVertex;

            return worstVertex.Color;
        }

        /// <summary>
        /// Randomly choose an adjacent vertex and move on to it.
        /// </summary>
        /// <param name="ant">The ID of the ant.</param>
        /// <returns>The color of the vertex on which ant moved.</returns>
        public int MoveAntToAnyAdjacentVertex(int ant)
        {
            var vertex = Vertices[Ants[ant]];
            var randomAdjacentVertex = vertex.ConnectedEdges.Keys.Shuffle(Rnd).First();
            // Move ant to the random adjacent vertex.
            Ants[ant] = randomAdjacentVertex;

            return Vertices[randomAdjacentVertex].Color;
        }

        /// <summary>
        /// Fint best color for the ant's vertex and replace the old color with the new color.
        /// The best color is the color which increases the local cost.
        /// </summary>
        /// <param name="ant">The ID of the ant.</param>
        /// <returns>The vertex with the new color.</returns>
        public Vertex ColorVertexWithBestColor(int ant)
        {
            var vertex = Vertices[Ants[ant]];
            var bestColor = vertex.ConnectedEdges.Select(connectedEdge => Vertices[connectedEdge.Key]).GroupBy(v => v.Color, (color, group) => new { color, count = group.Count() }).ToDictionary(tuple => tuple.color, tuple => tuple.count).OrderByDescending(x => x.Value).First();
            ChosenVertices.Add(vertex);
            vertex.Color = bestColor.Key;

            return vertex;
        }

        /// <summary>
        /// Randomly choose a color and set the new color for the ant's vertex.
        /// </summary>
        /// <param name="ant">The ID of the ant.</param>
        /// <param name="numberOfColors">The number of colors/partitions.</param>
        /// <returns>The vertex with the new color.</returns>
        public Vertex ColorVertexWithRandomColor(int ant, int numberOfColors)
        {
            var vertex = Vertices[Ants[ant]];
            var randomColor = Enumerable.Range(1, numberOfColors).Shuffle(Rnd).First();
            ChosenVertices.Add(vertex);
            vertex.Color = randomColor;

            return vertex;
        }

        /// <summary>
        /// To keep the balance, the algorithm chooses, from
        /// a set of s random vertices, one with the lowest value 
        /// of the local cost function -from those which have the new color- 
        /// and changes it's color to the old color.
        /// </summary>
        /// <param name="numberOfRandomVertices">The number of vertices set to keep balance.</param>
        /// <param name="oldColor">The changed color of the vertex.</param>
        /// <param name="newColor">The new color of the vertex.</param>
        /// <returns>The vertex which has been changed to keep balance.</returns>
        public Vertex KeepBalance(int numberOfRandomVertices, int oldColor, int newColor)
        {
            var random = Vertices.Shuffle(Rnd).Take(numberOfRandomVertices);
            var vertexChangedColor = random.Where(vertex => vertex.Color == newColor).OrderBy(vertex => vertex.LocalCost).FirstOrDefault();

            // TODO: Probably this function to return renturn random vertices should be run in recursion until the one is found.
            Debug.Assert(vertexChangedColor != null, "vertexChangedColor != null");
            vertexChangedColor.Color = oldColor;

            return vertexChangedColor;
        }
        
        /// <summary>
        /// Update local cost function for all chosen vertices 
        /// which has new color and for all adjacent vertices.
        /// </summary>
        public void UpdateLocalCostFunction(Vertex vertexWithOldColor, Vertex vertexWithNewColor)
        {
            CalculateLocalCostFunctionForVertex(vertexWithOldColor);
            CalculateLocalCostFunctionForVertex(vertexWithNewColor);
        }
    }
}
