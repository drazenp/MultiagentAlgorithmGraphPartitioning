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

            var graph = new Graph(loaderMock.Object);

            graph.InitializeGraph();

            Assert.AreEqual(7, graph.VerticesWeights.Length, "The number of vertices weights is not correct.");
            Assert.AreEqual(11, graph.NumberOfEdges, "The number of edges is not correct.");
        }

        [TestMethod]
        public void Graph_VerticesWeights_Initalized()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);

            var graph = new Graph(loaderMock.Object);

            graph.InitializeGraph();

            Assert.AreEqual(4, graph.VerticesWeights[0]);
            Assert.AreEqual(2, graph.VerticesWeights[1]);
            Assert.AreEqual(5, graph.VerticesWeights[2]);
            Assert.AreEqual(3, graph.VerticesWeights[3]);
            Assert.AreEqual(1, graph.VerticesWeights[4]);
            Assert.AreEqual(6, graph.VerticesWeights[5]);
            Assert.AreEqual(2, graph.VerticesWeights[6]);
        }
    }
}
