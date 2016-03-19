﻿using System.Collections.Generic;
using System.Fakes;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MultiagentAlgorithm.Test
{
    [TestClass]
    public class GraphTest
    {
        private readonly List<string> _dummyFile = new List<string>() { "7 11 011",
                                                                        "4 5 1 3 2 2 1",
                                                                        "2 1 1 3 2 4 1",
                                                                        "5 5 3 4 2 2 2 1 2",
                                                                        "3 2 1 3 2 6 2 7 5",
                                                                        "1 1 1 3 3 6 2",
                                                                        "6 5 2 4 2 7 6",
                                                                        "2 6 6 4 5" };
            
        [TestMethod]
        public void Graph_FirstLineRead_Sucess()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);
            var randomMock = new StubRandom();

            var graph = new Graph(loaderMock.Object, randomMock);
            graph.InitializeGraph();

            Assert.AreEqual(7, graph.Vertices.Length, "The number of vertices weights is not correct.");
            Assert.AreEqual(11, graph.NumberOfEdges, "The number of edges is not correct.");
        }

        [TestMethod]
        public void Graph_VerticesWeights_Initalized()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);
            var randomMock = new StubRandom();

            var graph = new Graph(loaderMock.Object, randomMock);
            graph.InitializeGraph();

            Assert.AreEqual(4, graph.Vertices[0].Weight);
            Assert.AreEqual(2, graph.Vertices[1].Weight);
            Assert.AreEqual(5, graph.Vertices[2].Weight);
            Assert.AreEqual(3, graph.Vertices[3].Weight);
            Assert.AreEqual(1, graph.Vertices[4].Weight);
            Assert.AreEqual(6, graph.Vertices[5].Weight);
            Assert.AreEqual(2, graph.Vertices[6].Weight);
        }

        [TestMethod]
        public void Graph_Vertices_Initialized()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);
            var randomMock = new StubRandom();

            var graph = new Graph(loaderMock.Object, randomMock);
            graph.InitializeGraph();

            Assert.AreEqual(7, graph.Vertices.Length, "Not all vertices are initialized.");
        }

        [TestMethod]
        public void Graph_RandomColorEachVertex_Success()
        {
            const int numberOfColors = 3;

            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);
            
            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new Graph(loaderMock.Object, randomMock);
            graph.InitializeGraph();
            graph.ColorVerticesRandomly(numberOfColors);

            Assert.AreEqual(1, graph.Vertices[0].Color);
            Assert.AreEqual(3, graph.Vertices[1].Color);
            Assert.AreEqual(2, graph.Vertices[2].Color);
            Assert.AreEqual(1, graph.Vertices[3].Color);
            Assert.AreEqual(3, graph.Vertices[4].Color);
            Assert.AreEqual(2, graph.Vertices[5].Color);
            Assert.AreEqual(1, graph.Vertices[6].Color);
        }

        [TestMethod]
        public void Graph_InitializeAnts_Success()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);
            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };
            const int numberOfAnts = 2;

            var graph = new Graph(loaderMock.Object, randomMock);
            graph.InitializeGraph();
            graph.InitializeAnts(numberOfAnts);

            Assert.AreEqual(0, graph.Vertices[0].Ants[0]);
            Assert.AreEqual(1, graph.Vertices[6].Ants[0]);
        }

        [TestMethod]
        public void Graph_InitializeEdges_Success()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);
            var randomMock = new StubRandom();

            var graph = new Graph(loaderMock.Object, randomMock);
            graph.InitializeGraph();

            // [0] 5 1 3 2 2 1
            Assert.AreEqual(1, graph.EdgesWeights[0, 4]);
            Assert.AreEqual(2, graph.EdgesWeights[0, 2]);
            Assert.AreEqual(1, graph.EdgesWeights[0, 1]);
            // [1] 1 1 3 2 4 1
            Assert.AreEqual(1, graph.EdgesWeights[1, 0]);
            Assert.AreEqual(2, graph.EdgesWeights[1, 2]);
            Assert.AreEqual(1, graph.EdgesWeights[1, 3]);
            // [2] 5 3 4 2 2 2 1 2
            Assert.AreEqual(3, graph.EdgesWeights[2, 4]);
            Assert.AreEqual(2, graph.EdgesWeights[2, 3]);
            Assert.AreEqual(2, graph.EdgesWeights[2, 1]);
            Assert.AreEqual(2, graph.EdgesWeights[2, 0]);
            // [3] 2 1 3 2 6 2 7 5
            Assert.AreEqual(1, graph.EdgesWeights[3, 1]);
            Assert.AreEqual(2, graph.EdgesWeights[3, 2]);
            Assert.AreEqual(2, graph.EdgesWeights[3, 5]);
            Assert.AreEqual(5, graph.EdgesWeights[3, 6]);
            // [4] 1 1 3 3 6 2
            Assert.AreEqual(1, graph.EdgesWeights[4, 0]);
            Assert.AreEqual(3, graph.EdgesWeights[4, 2]);
            Assert.AreEqual(2, graph.EdgesWeights[4, 5]);
            // [5] 5 2 4 2 7 6
            Assert.AreEqual(2, graph.EdgesWeights[5, 4]);
            Assert.AreEqual(2, graph.EdgesWeights[5, 3]);
            Assert.AreEqual(6, graph.EdgesWeights[5, 6]);
            // [6] 6 6 4 5
            Assert.AreEqual(6, graph.EdgesWeights[6, 5]);
            Assert.AreEqual(5, graph.EdgesWeights[6, 3]);
        }

        [TestMethod]
        public void Graph_GetConnectedVertcies_Success()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new Graph(loaderMock.Object, randomMock);
            graph.InitializeGraph();
            var connectedVertices = graph.GetConnectedVertices(0).ToList();

            Assert.AreEqual(1, connectedVertices[0].ID);
            Assert.AreEqual(2, connectedVertices[1].ID);
            Assert.AreEqual(4, connectedVertices[2].ID);
        }

        [TestMethod]
        public void Graph_LocalCostFunction_ThreeColors()
        {
            const int numberOfColors = 3;

            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new Graph(loaderMock.Object, randomMock);
            graph.InitializeGraph();
            graph.ColorVerticesRandomly(numberOfColors);
            graph.CalculateLocalCostFunction();

            Assert.AreEqual(0D, graph.Vertices[0].LocalCost);
            Assert.AreEqual(0D, graph.Vertices[1].LocalCost);
            Assert.AreEqual(0D, graph.Vertices[2].LocalCost);
            Assert.AreEqual(0.75D, graph.Vertices[3].LocalCost);
            Assert.AreEqual(0D, graph.Vertices[4].LocalCost);
            Assert.AreEqual(0D, graph.Vertices[5].LocalCost);
            Assert.AreEqual(0.5D, graph.Vertices[6].LocalCost);
        }

        [TestMethod]
        public void Graph_LocalCostFunction_TwoColors()
        {
            const int numberOfColors = 2;

            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new Graph(loaderMock.Object, randomMock);
            graph.InitializeGraph();
            graph.ColorVerticesRandomly(numberOfColors);
            graph.CalculateLocalCostFunction();

            Assert.AreEqual(1/3D, graph.Vertices[0].LocalCost);
            Assert.AreEqual(2/3D, graph.Vertices[1].LocalCost);
            Assert.AreEqual(0.5D, graph.Vertices[2].LocalCost);
            Assert.AreEqual(0.5D, graph.Vertices[3].LocalCost);
            Assert.AreEqual(1/3D, graph.Vertices[4].LocalCost);
            Assert.AreEqual(2/3D, graph.Vertices[5].LocalCost);
            Assert.AreEqual(0D, graph.Vertices[6].LocalCost);
        }

        [TestMethod]
        public void Graph_GlobalCostFunction_Initialized()
        {
            const int numberOfColors = 2;

            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new Graph(loaderMock.Object, randomMock);
            graph.InitializeGraph();
            graph.ColorVerticesRandomly(numberOfColors);
            var globalCost = graph.CalculateGlobalCostFunction();

            Assert.AreEqual(5, globalCost);
        }
    }
}
