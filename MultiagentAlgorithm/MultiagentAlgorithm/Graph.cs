using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiagentAlgorithm
{
    public class Graph : IGraph
    {
        private readonly IDataLoader _dataLoader;

        private readonly Random _rnd;

        /// <summary>
        /// The array of weights for each vertex in the graph.
        /// </summary>
        public Vertex[] Vertices { get; private set; }
        
        /// <summary>
        /// Number of edges read from files.
        /// This is not used in the application.
        /// </summary>
        public int NumberOfEdges { get; set; }

        public int[,] EdgesWeights { get; set; }

        public Dictionary<int, int> Ants;

        public Graph(IDataLoader dataLoader, Random rnd)
        {
            _dataLoader = dataLoader;
            _rnd = rnd;
        }

        public void InitializeGraph()
        {
            // We need first line of the file just for initialization of the graph.
            bool firstLine = true;
            int counter = 0;

            foreach (var line in _dataLoader.LoadData())
            {
                var fileData = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (firstLine)
                {
                    NumberOfEdges = int.Parse(fileData[1]);
                    Vertices = new Vertex[int.Parse(fileData[0])];
                    EdgesWeights = new int[Vertices.Length, Vertices.Length];

                    firstLine = false;
                }
                else
                {
                    var vertexWeight = int.Parse(fileData[0]);
                    
                    // Initilize edges. Get list of vertices as the each even element in the file line;
                    // The edges are the each odd element in the file line except first - the first is vertex weight.
                    var fileDataList = fileData.ToList();
                    var edges = fileDataList.Where((x, y) => y%2 == 0).Skip(1).ToList();
                    var vertices = fileDataList.Where((x, y) => y%2 != 0).ToList();

                    var connectedEdges = new Dictionary<int, int>();
                    for (var i = 0; i < vertices.Count(); i++)
                    {
                        var edgeVertex = int.Parse(vertices.ElementAt(i)) - 1;
                        var edgeWeight = int.Parse(edges.ElementAt(i));
                        EdgesWeights[edgeVertex, counter] = edgeWeight;

                        // Separately!
                        connectedEdges.Add(edgeVertex, edgeWeight);
                    }

                    Vertices[counter] = new Vertex(counter, vertexWeight, connectedEdges);

                    counter++;
                }
            }
        }

        /// <summary>
        /// Color each vertex of the graph at random forming k balanced sets.
        /// </summary>
        /// <param name="numberOfColors">The number of colors/ants/partitions.</param>
        public void ColorVerticesRandomly(int numberOfColors)
        {
            for (int i = 0; i < Vertices.Length;)
            {
                var randomColors = Enumerable.Range(1, numberOfColors).Shuffle(_rnd);
                foreach (var color in randomColors)
                {
                    Vertices[i].Color = color;
                    i++;
                    if (i >= Vertices.Length)
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
            foreach (var vertex in Vertices.Shuffle(_rnd))
            {
                Ants.Add(counter, vertex.ID);
                vertex.Ants.Add(counter);
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
                var connectedVertices = GetConnectedVertices(vertex.ID).ToList();
                var verticesCount = connectedVertices.Count;
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
                    vertex.LocalCost = differentColorCount / (double)connectedVertices.Count;
                }
            }
        }

        public IEnumerable<Vertex> GetConnectedVertices(int vertexId)
        {
            for (var i = 0; i < EdgesWeights.GetLength(0); i++)
            {
                if (EdgesWeights[vertexId, i] != 0)
                {
                    yield return Vertices.Single(x => x.ID == i);
                }
            }
        }

        /// <summary>
        /// Counts the number of times that an edge joins vertices of different colors.
        /// </summary>
        /// <returns>The value of global cost function.</returns>
        public double CalculateGlobalCostFunction()
        {
            var globalCost = 0.0D;

            foreach (var vertex in Vertices)
            {
                var differentColorCount = GetConnectedVertices(vertex.ID).Count(x => x.Color == vertex.Color);
                globalCost += differentColorCount;
            }

            return globalCost / 2;
        }
    }
}
