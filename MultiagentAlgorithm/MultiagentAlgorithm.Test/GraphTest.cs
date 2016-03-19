using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MultiagentAlgorithm.Test
{
    [TestClass]
    public class GraphTest
    {
        [TestMethod]
        public void Graph_FirstLineRead_Sucess()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(new List<string>() { "7 11 011" } );

            var graph = new Graph(loaderMock.Object);

            graph.InitializeGraph();

            Assert.AreEqual(7, graph.VerticesWeights.Length, "The number of vertices weights is not correct.");
            Assert.AreEqual(11, graph.NumberOfEdges, "The number of edges is not correct.");
        }
    }
}
