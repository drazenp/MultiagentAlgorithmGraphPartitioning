using System.Collections.Generic;
using System.Fakes;
using System.Reflection;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MultiagentAlgorithm.Test
{
    [TestClass]
    public class DimacsGraphTest
    {
        public static ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<string> _dummyFile = new List<string>() { "c FILE: myciel3.col",
                                                                        "c SOURCE: Michael Trick (trick@cmu.edu)",
                                                                        "c DESCRIPTION: Graph based on Mycielski transformation.",
                                                                        "c              Triangle free (clique number 2) but increasing",
                                                                        "c              coloring number",
                                                                        "p edge 11 20",
                                                                        "e 1 2",
                                                                        "e 1 4",
                                                                        "e 1 7",
                                                                        "e 1 9",
                                                                        "e 2 3",
                                                                        "e 2 6",
                                                                        "e 2 8",
                                                                        "e 3 5",
                                                                        "e 3 7",
                                                                        "e 3 10",
                                                                        "e 4 5",
                                                                        "e 4 6",
                                                                        "e 4 10",
                                                                        "e 5 8",
                                                                        "e 5 9",
                                                                        "e 6 11",
                                                                        "e 7 11",
                                                                        "e 8 11",
                                                                        "e 9 11",
                                                                        "e 10 11" };

        private readonly Options _options = new Options(numberOfAnts: 2, numberOfPartitions: 2, coloringProbability: 0.9,
                movingProbability: 0.95, graphFilePath: string.Empty, numberOfVerticesForBalance: 1, numberOfIterations: 100);

        [TestMethod]
        public void DimacsGraph_FirstLineRead_Success()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);
            var randomMock = new StubRandom();

            var graph = new DimacsGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();

            Assert.AreEqual(11, graph.Vertices.Length, "The number of vertices weights is not correct.");
            Assert.AreEqual(20, graph.NumberOfEdges, "The number of edges is not correct.");
        }

        [TestMethod]
        public void DimacsGraph_Vertices_Initialized()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);
            var randomMock = new StubRandom();

            var graph = new DimacsGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();

            Assert.AreEqual(11, graph.Vertices.Length, "Not all vertices are initialized.");
        }

        [TestMethod]
        public void DimacsGraph_MaxNumberOfAdjacentVertices_Correct()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);
            var randomMock = new StubRandom();

            var graph = new DimacsGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();

            Assert.AreEqual(5, graph.MaxNumberOfAdjacentVertices, "The number of maximum adjacent vertices of one vertex is not calculatef correctly.");
        }
    }
}
