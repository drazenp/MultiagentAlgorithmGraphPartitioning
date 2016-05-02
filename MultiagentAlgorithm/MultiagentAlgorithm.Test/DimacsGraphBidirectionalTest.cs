using System.Collections.Generic;
using System.Fakes;
using System.Reflection;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MultiagentAlgorithm.Test
{
    [TestClass]
    public class DimacsGraphBidirectionalTest
    {
        public static ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<string> _dummyFile = new List<string>() { "c FILE: myciel3bi.col",
                                                                        "c SOURCE: Michael Trick (trick@cmu.edu)",
                                                                        "c DESCRIPTION: Graph based on Mycielski transformation.",
                                                                        "c              Triangle free (clique number 2) but increasing",
                                                                        "c              coloring number",
                                                                        "p edge 11 20",
                                                                        "e 1 2",
                                                                        "e 1 4",
                                                                        "e 1 7",
                                                                        "e 1 9",
                                                                        "e 2 1",
                                                                        "e 2 3",
                                                                        "e 2 6",
                                                                        "e 2 8",
                                                                        "e 3 2",
                                                                        "e 3 5",
                                                                        "e 3 7",
                                                                        "e 3 10",
                                                                        "e 4 1",
                                                                        "e 4 5",
                                                                        "e 4 6",
                                                                        "e 4 10",
                                                                        "e 5 3",
                                                                        "e 5 4",
                                                                        "e 5 8",
                                                                        "e 5 9",
                                                                        "e 6 2",
                                                                        "e 6 4",
                                                                        "e 6 11",
                                                                        "e 7 1",
                                                                        "e 7 3",
                                                                        "e 7 11",
                                                                        "e 8 2",
                                                                        "e 8 5",
                                                                        "e 8 11",
                                                                        "e 9 1",
                                                                        "e 9 5",
                                                                        "e 9 11",
                                                                        "e 10 3",
                                                                        "e 10 4",
                                                                        "e 10 11",
                                                                        "e 11 6",
                                                                        "e 11 7",
                                                                        "e 11 8",
                                                                        "e 11 9",
                                                                        "e 11 10"};

        #region Queen5_5
        private readonly List<string> _queen5_5Dimacs = new List<string>() {"p edge 25 320",
                                                                          "e 1 7",
                                                                          "e 1 13",
                                                                          "e 1 19",
                                                                          "e 1 25",
                                                                          "e 1 2",
                                                                          "e 1 3",
                                                                          "e 1 4",
                                                                          "e 1 5",
                                                                          "e 1 6",
                                                                          "e 1 11",
                                                                          "e 1 16",
                                                                          "e 1 21",
                                                                          "e 2 8",
                                                                          "e 2 14",
                                                                          "e 2 20",
                                                                          "e 2 6",
                                                                          "e 2 3",
                                                                          "e 2 4",
                                                                          "e 2 5",
                                                                          "e 2 7",
                                                                          "e 2 12",
                                                                          "e 2 17",
                                                                          "e 2 22",
                                                                          "e 2 1",
                                                                          "e 3 9",
                                                                          "e 3 15",
                                                                          "e 3 7",
                                                                          "e 3 11",
                                                                          "e 3 4",
                                                                          "e 3 5",
                                                                          "e 3 8",
                                                                          "e 3 13",
                                                                          "e 3 18",
                                                                          "e 3 23",
                                                                          "e 3 2",
                                                                          "e 3 1",
                                                                          "e 4 10",
                                                                          "e 4 8",
                                                                          "e 4 12",
                                                                          "e 4 16",
                                                                          "e 4 5",
                                                                          "e 4 9",
                                                                          "e 4 14",
                                                                          "e 4 19",
                                                                          "e 4 24",
                                                                          "e 4 3",
                                                                          "e 4 2",
                                                                          "e 4 1",
                                                                          "e 5 9",
                                                                          "e 5 13",
                                                                          "e 5 17",
                                                                          "e 5 21",
                                                                          "e 5 10",
                                                                          "e 5 15",
                                                                          "e 5 20",
                                                                          "e 5 25",
                                                                          "e 5 4",
                                                                          "e 5 3",
                                                                          "e 5 2",
                                                                          "e 5 1",
                                                                          "e 6 12",
                                                                          "e 6 18",
                                                                          "e 6 24",
                                                                          "e 6 7",
                                                                          "e 6 8",
                                                                          "e 6 9",
                                                                          "e 6 10",
                                                                          "e 6 11",
                                                                          "e 6 16",
                                                                          "e 6 21",
                                                                          "e 6 2",
                                                                          "e 6 1",
                                                                          "e 7 13",
                                                                          "e 7 19",
                                                                          "e 7 25",
                                                                          "e 7 11",
                                                                          "e 7 8",
                                                                          "e 7 9",
                                                                          "e 7 10",
                                                                          "e 7 12",
                                                                          "e 7 17",
                                                                          "e 7 22",
                                                                          "e 7 6",
                                                                          "e 7 3",
                                                                          "e 7 2",
                                                                          "e 7 1",
                                                                          "e 8 14",
                                                                          "e 8 20",
                                                                          "e 8 12",
                                                                          "e 8 16",
                                                                          "e 8 9",
                                                                          "e 8 10",
                                                                          "e 8 13",
                                                                          "e 8 18",
                                                                          "e 8 23",
                                                                          "e 8 7",
                                                                          "e 8 6",
                                                                          "e 8 4",
                                                                          "e 8 3",
                                                                          "e 8 2",
                                                                          "e 9 15",
                                                                          "e 9 13",
                                                                          "e 9 17",
                                                                          "e 9 21",
                                                                          "e 9 10",
                                                                          "e 9 14",
                                                                          "e 9 19",
                                                                          "e 9 24",
                                                                          "e 9 8",
                                                                          "e 9 7",
                                                                          "e 9 6",
                                                                          "e 9 5",
                                                                          "e 9 4",
                                                                          "e 9 3",
                                                                          "e 10 14",
                                                                          "e 10 18",
                                                                          "e 10 22",
                                                                          "e 10 15",
                                                                          "e 10 20",
                                                                          "e 10 25",
                                                                          "e 10 9",
                                                                          "e 10 8",
                                                                          "e 10 7",
                                                                          "e 10 6",
                                                                          "e 10 5",
                                                                          "e 10 4",
                                                                          "e 11 17",
                                                                          "e 11 23",
                                                                          "e 11 12",
                                                                          "e 11 13",
                                                                          "e 11 14",
                                                                          "e 11 15",
                                                                          "e 11 16",
                                                                          "e 11 21",
                                                                          "e 11 7",
                                                                          "e 11 6",
                                                                          "e 11 3",
                                                                          "e 11 1",
                                                                          "e 12 18",
                                                                          "e 12 24",
                                                                          "e 12 16",
                                                                          "e 12 13",
                                                                          "e 12 14",
                                                                          "e 12 15",
                                                                          "e 12 17",
                                                                          "e 12 22",
                                                                          "e 12 11",
                                                                          "e 12 8",
                                                                          "e 12 7",
                                                                          "e 12 6",
                                                                          "e 12 4",
                                                                          "e 12 2",
                                                                          "e 13 19",
                                                                          "e 13 25",
                                                                          "e 13 17",
                                                                          "e 13 21",
                                                                          "e 13 14",
                                                                          "e 13 15",
                                                                          "e 13 18",
                                                                          "e 13 23",
                                                                          "e 13 12",
                                                                          "e 13 11",
                                                                          "e 13 9",
                                                                          "e 13 8",
                                                                          "e 13 7",
                                                                          "e 13 5",
                                                                          "e 13 3",
                                                                          "e 13 1",
                                                                          "e 14 20",
                                                                          "e 14 18",
                                                                          "e 14 22",
                                                                          "e 14 15",
                                                                          "e 14 19",
                                                                          "e 14 24",
                                                                          "e 14 13",
                                                                          "e 14 12",
                                                                          "e 14 11",
                                                                          "e 14 10",
                                                                          "e 14 9",
                                                                          "e 14 8",
                                                                          "e 14 4",
                                                                          "e 14 2",
                                                                          "e 15 19",
                                                                          "e 15 23",
                                                                          "e 15 20",
                                                                          "e 15 25",
                                                                          "e 15 14",
                                                                          "e 15 13",
                                                                          "e 15 12",
                                                                          "e 15 11",
                                                                          "e 15 10",
                                                                          "e 15 9",
                                                                          "e 15 5",
                                                                          "e 15 3",
                                                                          "e 16 22",
                                                                          "e 16 17",
                                                                          "e 16 18",
                                                                          "e 16 19",
                                                                          "e 16 20",
                                                                          "e 16 21",
                                                                          "e 16 12",
                                                                          "e 16 11",
                                                                          "e 16 8",
                                                                          "e 16 6",
                                                                          "e 16 4",
                                                                          "e 16 1",
                                                                          "e 17 23",
                                                                          "e 17 21",
                                                                          "e 17 18",
                                                                          "e 17 19",
                                                                          "e 17 20",
                                                                          "e 17 22",
                                                                          "e 17 16",
                                                                          "e 17 13",
                                                                          "e 17 12",
                                                                          "e 17 11",
                                                                          "e 17 9",
                                                                          "e 17 7",
                                                                          "e 17 5",
                                                                          "e 17 2",
                                                                          "e 18 24",
                                                                          "e 18 22",
                                                                          "e 18 19",
                                                                          "e 18 20",
                                                                          "e 18 23",
                                                                          "e 18 17",
                                                                          "e 18 16",
                                                                          "e 18 14",
                                                                          "e 18 13",
                                                                          "e 18 12",
                                                                          "e 18 10",
                                                                          "e 18 8",
                                                                          "e 18 6",
                                                                          "e 18 3",
                                                                          "e 19 25",
                                                                          "e 19 23",
                                                                          "e 19 20",
                                                                          "e 19 24",
                                                                          "e 19 18",
                                                                          "e 19 17",
                                                                          "e 19 16",
                                                                          "e 19 15",
                                                                          "e 19 14",
                                                                          "e 19 13",
                                                                          "e 19 9",
                                                                          "e 19 7",
                                                                          "e 19 4",
                                                                          "e 19 1",
                                                                          "e 20 24",
                                                                          "e 20 25",
                                                                          "e 20 19",
                                                                          "e 20 18",
                                                                          "e 20 17",
                                                                          "e 20 16",
                                                                          "e 20 15",
                                                                          "e 20 14",
                                                                          "e 20 10",
                                                                          "e 20 8",
                                                                          "e 20 5",
                                                                          "e 20 2",
                                                                          "e 21 22",
                                                                          "e 21 23",
                                                                          "e 21 24",
                                                                          "e 21 25",
                                                                          "e 21 17",
                                                                          "e 21 16",
                                                                          "e 21 13",
                                                                          "e 21 11",
                                                                          "e 21 9",
                                                                          "e 21 6",
                                                                          "e 21 5",
                                                                          "e 21 1",
                                                                          "e 22 23",
                                                                          "e 22 24",
                                                                          "e 22 25",
                                                                          "e 22 21",
                                                                          "e 22 18",
                                                                          "e 22 17",
                                                                          "e 22 16",
                                                                          "e 22 14",
                                                                          "e 22 12",
                                                                          "e 22 10",
                                                                          "e 22 7",
                                                                          "e 22 2",
                                                                          "e 23 24",
                                                                          "e 23 25",
                                                                          "e 23 22",
                                                                          "e 23 21",
                                                                          "e 23 19",
                                                                          "e 23 18",
                                                                          "e 23 17",
                                                                          "e 23 15",
                                                                          "e 23 13",
                                                                          "e 23 11",
                                                                          "e 23 8",
                                                                          "e 23 3",
                                                                          "e 24 25",
                                                                          "e 24 23",
                                                                          "e 24 22",
                                                                          "e 24 21",
                                                                          "e 24 20",
                                                                          "e 24 19",
                                                                          "e 24 18",
                                                                          "e 24 14",
                                                                          "e 24 12",
                                                                          "e 24 9",
                                                                          "e 24 6",
                                                                          "e 24 4",
                                                                          "e 25 24",
                                                                          "e 25 23",
                                                                          "e 25 22",
                                                                          "e 25 21",
                                                                          "e 25 20",
                                                                          "e 25 19",
                                                                          "e 25 15",
                                                                          "e 25 13",
                                                                          "e 25 10",
                                                                          "e 25 7",
                                                                          "e 25 5",
                                                                          "e 25 1"};

        #endregion

        private readonly Options _options = new Options(numberOfAnts: 2, numberOfPartitions: 2, coloringProbability: 0.9,
                                                            movingProbability: 0.95, graphFilePath: string.Empty, numberOfVerticesForBalance: 1, numberOfIterations: 100);

        [TestMethod]
        public void DimacsGraphBidirectionalGraph_FirstLineRead_Sucess()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);
            var randomMock = new StubRandom();

            var graph = new DimacsGraphBidirectional(loaderMock.Object, randomMock);
            graph.InitializeGraph();

            Assert.AreEqual(11, graph.Vertices.Length, "The number of vertices weights is not correct.");
            Assert.AreEqual(20, graph.NumberOfEdges, "The number of edges is not correct.");
        }

        [TestMethod]
        public void DimacsGraphBidirectionalGraph_Vertices_Initialized()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);
            var randomMock = new StubRandom();

            var graph = new DimacsGraphBidirectional(loaderMock.Object, randomMock);
            graph.InitializeGraph();

            Assert.AreEqual(11, graph.Vertices.Length, "Not all vertices are initialized.");
        }

        [TestMethod]
        public void DimacsGraphBidirectionalGraph_MaxNumberOfAdjacentVertices_Correct()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);
            var randomMock = new StubRandom();

            var graph = new DimacsGraphBidirectional(loaderMock.Object, randomMock);
            graph.InitializeGraph();

            Assert.AreEqual(5, graph.MaxNumberOfAdjacentVertices, "The number of maximum adjacent vertices of one vertex is not calculatef correctly.");
        }

        [TestMethod]
        public void DimacsGraphBidirectionalGraph_GraphQueen5_5_CalculateGlobalCostFunction_Success()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_queen5_5Dimacs);

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new DimacsGraphBidirectional(loaderMock.Object, randomMock);
            graph.InitializeGraph();

            graph.Vertices[0].Color = 2;
            graph.Vertices[1].Color = 2;
            graph.Vertices[2].Color = 1;
            graph.Vertices[3].Color = 1;
            graph.Vertices[4].Color = 1;
            graph.Vertices[5].Color = 2;
            graph.Vertices[6].Color = 2;
            graph.Vertices[7].Color = 1;
            graph.Vertices[8].Color = 2;
            graph.Vertices[9].Color = 2;
            graph.Vertices[10].Color = 1;
            graph.Vertices[11].Color = 1;
            graph.Vertices[12].Color = 2;
            graph.Vertices[13].Color = 2;
            graph.Vertices[14].Color = 2;
            graph.Vertices[15].Color = 2;
            graph.Vertices[16].Color = 1;
            graph.Vertices[17].Color = 1;
            graph.Vertices[18].Color = 2;
            graph.Vertices[19].Color = 2;
            graph.Vertices[20].Color = 2;
            graph.Vertices[21].Color = 1;
            graph.Vertices[22].Color = 1;
            graph.Vertices[23].Color = 1;
            graph.Vertices[24].Color = 1;

            var globalCost = graph.GetGlobalCostFunction();

            Assert.AreEqual(168, globalCost);
        }
    }
}
