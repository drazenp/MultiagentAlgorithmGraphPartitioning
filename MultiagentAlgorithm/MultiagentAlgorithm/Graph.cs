﻿using System;
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

                    firstLine = false;
                }
                else
                {
                    var vertexWeight = int.Parse(fileData[0]);

                    // Initilize edges. Get list of vertices as the each even element in the file line;
                    // The edges are the each odd element in the file line except first - the first is vertex weight.
                    var fileDataList = fileData.ToList();
                    var edges = fileDataList.Where((x, y) => y % 2 == 0).Skip(1).ToList();
                    var vertices = fileDataList.Where((x, y) => y % 2 != 0).ToList();

                    var connectedEdges = new Dictionary<int, int>();
                    for (var i = 0; i < vertices.Count(); i++)
                    {
                        var edgeVertex = int.Parse(vertices.ElementAt(i)) - 1;
                        var edgeWeight = int.Parse(edges.ElementAt(i));
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
            for (var i = 0; i < Vertices.Length;)
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
            var globalCost = 0;

            foreach (var vertex in Vertices)
            {
                var differentColorCount = vertex.ConnectedEdges.Select(connectedEdge => Vertices[connectedEdge.Key]).Count(x => x.Color == vertex.Color);
                globalCost += differentColorCount;
            }

            return globalCost / 2;
        }

        /// <summary>
        /// Find the worst adjacent vertex and move ant to it.
        /// </summary>
        /// <param name="ant">The ID of the ant.</param>
        public void MoveAntToVertexWithLowestCost(int ant)
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
            worstVertex.LowestCost = true;
            Ants[ant] = worstAdjacentVertex;
        }

        /// <summary>
        /// Randomly choose an adjacent vertex and move on to it.
        /// </summary>
        /// <param name="ant">The ID of the ant.</param>
        public void MoveAntToAnyAdjacentVertex(int ant)
        {
            var vertex = Vertices[Ants[ant]];
            var randomAdjacentVertex = vertex.ConnectedEdges.Keys.Shuffle(_rnd).First();
            // Move ant to the random adjacent vertex.
            Ants[ant] = randomAdjacentVertex;
        }

        /// <summary>
        /// Fint best color for the ant's vertex and replace the old color with the new color.
        /// The best color is the color which increases the local cost.
        /// </summary>
        /// <param name="ant">The ID of the ant.</param>
        public void ColorVertexWithBestColor(int ant)
        {
            var vertex = Vertices[Ants[ant]];
            var bestColor = vertex.ConnectedEdges.Select(connectedEdge => Vertices[connectedEdge.Key]).GroupBy(v => v.Color, (color, group) => new { color, count = group.Count() }).ToDictionary(tuple => tuple.color, tuple => tuple.count).OrderByDescending(x => x.Value).First();
            ChosenVertices.Add(vertex);
            vertex.Color = bestColor.Key;
        }

        /// <summary>
        /// Randomly choose a color and set the new color for the ant's vertex.
        /// </summary>
        /// <param name="ant">The ID of the ant.</param>
        /// <param name="numberOfColors">The number of colors/ants/partitions.</param>
        public void ColorVertexWithRandomColor(int ant, int numberOfColors)
        {
            var vertex = Vertices[Ants[ant]];
            var randomColor = Enumerable.Range(1, numberOfColors).Shuffle(_rnd).First();
            ChosenVertices.Add(vertex);
            vertex.Color = randomColor;
        }

        /// <summary>
        /// To keep the balance, the algorithm chooses, from
        /// a set of s random vertices, one with the lowest value 
        /// of the local cost function -from those which have the new color- 
        /// and changes its color to the old color.
        /// </summary>
        public void KeepBalance(int numberOfRandomVertices)
        {
            var random = Vertices.Shuffle(_rnd).Take(numberOfRandomVertices);
            var withChangedColor = random.Where(vertex => vertex.OldColor != null).OrderBy(vertex => vertex.LocalCost);
            if (withChangedColor.Any())
            {
                var oldColor = withChangedColor.First().OldColor;
                if (oldColor != null)
                {
                    withChangedColor.First().Color = oldColor.Value;
                }
            }
        }

        /// <summary>
        /// Reset the all vertices history states. 
        /// </summary>
        public void ResetVerticesState()
        {
            foreach (var vertex in Vertices)
            {
                vertex.Reset();
            }
        }

        /// <summary>
        /// Update local cost function for all chosen vertices 
        /// which has new color and for all adjacent vertices.
        /// </summary>
        public void UpdateLocalCostFunction()
        {
            List<int> vertices = new List<int>();
            var changedVertices = Vertices.Select(vertex => vertex.OldColor != null);
            foreach (var changedVertex in ChosenVertices)
            {
                vertices.AddRange(changedVertex.ConnectedEdges.Keys);
            }

            foreach (var vertex in vertices.Distinct())
            {
                CalculateLocalCostFunctionForVertex(Vertices[vertex]);
            }
        }
    }
}