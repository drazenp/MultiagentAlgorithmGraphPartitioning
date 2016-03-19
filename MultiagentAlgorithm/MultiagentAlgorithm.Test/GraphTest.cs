using System;
using System.Collections.Generic;
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
            var randomMock = new System.Fakes.StubRandom();

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
            var randomMock = new System.Fakes.StubRandom();

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
            var randomMock = new System.Fakes.StubRandom();

            var graph = new Graph(loaderMock.Object, randomMock);

            graph.InitializeGraph();

            Assert.AreEqual(7, graph.Vertices.Length, "Not all vertices are initialized.");
        }

        [TestMethod]
        public void Graph_RandomColorEachVertex_Success()
        {
            var numberOfColors = 3;

            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);
            
            var randomMock = new System.Fakes.StubRandom()
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
    }
}
