using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;

namespace MultiagentAlgorithm
{
    public abstract class BaseGraph : IGraph
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected IDataLoader DataLoader;

        protected Random Rnd;

        /// <summary>
        /// The array of weights for each vertex in the graph.
        /// </summary>
        public Vertex[] Vertices { get; protected set; }

        /// <summary>
        /// Number of edges read from files.
        /// This is not used in the application.
        /// </summary>
        public int NumberOfEdges { get; set; }

        public Dictionary<int, int> Ants;

        public int MaxNumberOfAdjacentVertices;

        public Dictionary<int, List<string>> changes = new Dictionary<int, List<string>>();

        private void AddVertex(string type, Vertex vertex)
        {
            if (changes.Keys.All(key => key != vertex.ID))
            {
                changes.Add(vertex.ID, new List<string>());
            }
            changes[vertex.ID].Add(type + " " + vertex.Color);
        }

        public abstract void InitializeGraph();

        /// <summary>
        /// Color each vertex of the graph at random forming k balanced sets.
        /// </summary>
        /// <param name="numberOfColors">The number of colors/partitions.</param>
        public void ColorVerticesRandomly(int numberOfColors)
        {
            var shuffleVertices = Vertices.Shuffle(Rnd).ToList();
            for (var i = 0; i < Vertices.Length; i++)
            {
                shuffleVertices[i].Color = i % numberOfColors + 1;
                AddVertex("i", shuffleVertices[i]);
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
            var differentColorCount = connectedVertices.Count(x => x.Color != vertex.Color);

            if (differentColorCount == 0)
            {
                vertex.LocalCost = 1;
            }
            else
            {
                vertex.LocalCost = 1 - differentColorCount / (double)MaxNumberOfAdjacentVertices;
            }
        }

        /// <summary>
        /// Counts the number of times that an edge joins vertices of different colors.
        /// </summary>
        /// <returns>The value of global cost function.</returns>
        public int GetGlobalCostFunction()
        {
            if (Log.IsDebugEnabled)
            {
                LoggerHelper.LogVertices(Vertices);
            }
            
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
        /// <returns>The vertex on which ant moved.</returns>
        public Vertex MoveAntToVertexWithLowestCost(int ant)
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

            return worstVertex;
        }

        /// <summary>
        /// Randomly choose an adjacent vertex and move on to it.
        /// </summary>
        /// <param name="ant">The ID of the ant.</param>
        /// <returns>The vertex on which ant moved.</returns>
        public Vertex MoveAntToAnyAdjacentVertex(int ant)
        {
            var vertex = Vertices[Ants[ant]];
            var randomAdjacentVertex = vertex.ConnectedEdges.Keys.Shuffle(Rnd).First();
            // Move ant to the random adjacent vertex.
            Ants[ant] = randomAdjacentVertex;

            return Vertices[randomAdjacentVertex];
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
            var bestColor = vertex.ConnectedEdges
                              .Select(connectedEdge => Vertices[connectedEdge.Key])
                              .GroupBy(v => v.Color, (color, group) => new { color, count = group.Count() })
                              .ToDictionary(tuple => tuple.color, tuple => tuple.count)
                              .OrderByDescending(x => x.Value).First();

            vertex.Color = bestColor.Key;
            
            AddVertex("b", vertex);

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
            vertex.Color = randomColor;

            AddVertex("r", vertex);

            return vertex;
        }

        /// <summary>
        /// To keep the balance, the algorithm chooses, from
        /// a set of s random vertices, one with the lowest value 
        /// of the local cost function -from those which have the new color- 
        /// and changes it's color to the old color.
        /// </summary>
        /// <param name="numberOfRandomVertices">The number of vertices set to keep balance.</param>
        /// <param name="vertexWithAntID">The ID of the vertex on wich ant moved.</param>
        /// <param name="oldColor">The changed color of the vertex.</param>
        /// <param name="newColor">The new color of the vertex.</param>
        /// <returns>The vertex which has been changed to keep balance.</returns>
        public Vertex KeepBalance(int numberOfRandomVertices, int vertexWithAntID, int oldColor, int newColor)
        {
            var random = Vertices.Where(v => v.ID != vertexWithAntID).Shuffle(Rnd).Take(numberOfRandomVertices).ToList();
            var vertexChangedColor = random.Where(vertex => vertex.Color == newColor).OrderBy(vertex => vertex.LocalCost).FirstOrDefault();
            //if (vertexChangedColor == null)
            //{
            //    Log.Warn($"New color: {newColor}");
            //    LoggerHelper.LogVertexWithState(random);
            //    Log.Warn("------------------------------------------------------------------");
            //    LoggerHelper.LogVertexWithState(Vertices);
            //}

            // TODO: Probably the function to return random vertices should be run in recursion until the one is found.
            Debug.Assert(vertexChangedColor != null, "vertexChangedColor != null");
            vertexChangedColor.Color = oldColor;

            AddVertex("k", vertexChangedColor);

            return vertexChangedColor;
        }

        /// <summary>
        /// Update local cost function for all chosen vertices 
        /// which has new color and for all adjacent vertices.
        /// </summary>
        public void UpdateLocalCostFunction(Vertex vertexWithOldColor, Vertex vertexWithNewColor)
        {
            var neighbors = new HashSet<int>(vertexWithOldColor.ConnectedEdges.Keys);
            CalculateLocalCostFunctionForVertex(vertexWithOldColor);
            neighbors.UnionWith(vertexWithNewColor.ConnectedEdges.Keys);
            CalculateLocalCostFunctionForVertex(vertexWithNewColor);
            neighbors.Remove(vertexWithNewColor.ID);
            neighbors.Remove(vertexWithOldColor.ID);

            foreach (var neighbor in neighbors)
            {
                CalculateLocalCostFunctionForVertex(Vertices.Single(vertex => vertex.ID == neighbor));
            }
        }
    }
}
