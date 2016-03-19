﻿using System;
using System.Linq;

namespace MultiagentAlgorithm
{
    public class Graph
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
                    Vertices[counter] = new Vertex(counter, vertexWeight);

                    // Initilize edges. Get list of vertices as the each even element in the file line;
                    // The edges are the each odd element in the file line except first - the first is vertex weight.
                    var fileDataList = fileData.ToList();
                    var edges = fileDataList.Where((x, y) => y%2 == 0).Skip(1).ToList();
                    var vertices = fileDataList.Where((x, y) => y%2 != 0).ToList();

                    for (var i = 0; i < vertices.Count(); i++)
                    {
                        var edgeVertex = int.Parse(vertices.ElementAt(i)) - 1;
                        var edgeWeight = int.Parse(edges.ElementAt(i));
                        //EdgesWeights[counter, edgeVertex] = edgeWeight;
                        EdgesWeights[edgeVertex, counter] = edgeWeight;
                    }

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
            var counter = 0;
            foreach (var vertex in Vertices.Shuffle(_rnd))
            {
                vertex.Ants.Add(counter);
                counter++;
                if (counter >= numberOfAnts)
                {
                    return;
                }
            }
        }
    }
}
