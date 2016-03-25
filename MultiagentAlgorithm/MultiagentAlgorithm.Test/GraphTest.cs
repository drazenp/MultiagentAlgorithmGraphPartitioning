using System.Collections.Generic;
using System.Fakes;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MultiagentAlgorithm.Test
{
    [TestClass]
    public class GraphTest
    {
        public static ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<string> _dummyFile = new List<string>() { "7 11 011",
                                                                        "4 5 1 3 2 2 1",
                                                                        "2 1 1 3 2 4 1",
                                                                        "5 5 3 4 2 2 2 1 2",
                                                                        "3 2 1 3 2 6 2 7 5",
                                                                        "1 1 1 3 3 6 2",
                                                                        "6 5 2 4 2 7 6",
                                                                        "2 6 6 4 5" };

        private readonly Options _optionOneColors = new Options(numberOfAnts: 2, numberOfPartitions: 1, coloringProbability: 0.9,
            movingProbability: 0.95, graphFilePath: string.Empty, numberVerticesForBalance: 1, numberOfIterations: 100);
        private readonly Options _optionTwoColors = new Options(numberOfAnts: 2, numberOfPartitions: 2, coloringProbability: 0.9,
            movingProbability: 0.95, graphFilePath: string.Empty, numberVerticesForBalance: 1, numberOfIterations: 100);
        private readonly Options _optionThreeColors = new Options(numberOfAnts: 2, numberOfPartitions: 3, coloringProbability: 0.9,
            movingProbability: 0.95, graphFilePath: string.Empty, numberVerticesForBalance: 1, numberOfIterations: 100);

        [TestMethod]
        public void Graph_FirstLineRead_Sucess()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);
            var randomMock = new StubRandom();

            var graph = new MetisGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();

            Assert.AreEqual(7, graph.Vertices.Count, "The number of vertices weights is not correct.");
            Assert.AreEqual(11, graph.NumberOfEdges, "The number of edges is not correct.");
        }

        [TestMethod]
        public void Graph_VerticesWeights_Initalized()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);
            var randomMock = new StubRandom();

            var graph = new MetisGraph(loaderMock.Object, randomMock);
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

            var graph = new MetisGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();

            Assert.AreEqual(7, graph.Vertices.Count, "Not all vertices are initialized.");
        }

        [TestMethod]
        public void Graph_RandomColorEachVertex_ThreeColors()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new MetisGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();
            graph.ColorVerticesRandomly(_optionThreeColors.NumberOfPartitions);

            Assert.AreEqual(1, graph.Vertices[0].Color);
            Assert.AreEqual(3, graph.Vertices[1].Color);
            Assert.AreEqual(2, graph.Vertices[2].Color);
            Assert.AreEqual(1, graph.Vertices[3].Color);
            Assert.AreEqual(3, graph.Vertices[4].Color);
            Assert.AreEqual(2, graph.Vertices[5].Color);
            Assert.AreEqual(1, graph.Vertices[6].Color);
        }

        [TestMethod]
        public void Graph_RandomColorEachVertex_TwoColors()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new MetisGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();
            graph.ColorVerticesRandomly(_optionTwoColors.NumberOfPartitions);

            Assert.AreEqual(1, graph.Vertices[0].Color);
            Assert.AreEqual(2, graph.Vertices[1].Color);
            Assert.AreEqual(1, graph.Vertices[2].Color);
            Assert.AreEqual(2, graph.Vertices[3].Color);
            Assert.AreEqual(1, graph.Vertices[4].Color);
            Assert.AreEqual(2, graph.Vertices[5].Color);
            Assert.AreEqual(1, graph.Vertices[6].Color);
        }

        [TestMethod]
        public void Graph_InitializeAntsSeparately_Success()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);
            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new MetisGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();
            graph.InitializeAnts(_optionTwoColors.NumberOfAnts);

            Assert.AreEqual(0, graph.Ants[0]);
            Assert.AreEqual(6, graph.Ants[1]);
        }

        [TestMethod]
        public void Graph_InitializeEdgesSeparately_Success()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);
            var randomMock = new StubRandom();

            var graph = new MetisGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();

            // [0] 5 1 3 2 2 1
            Assert.AreEqual(1, graph.Vertices[0].ConnectedEdges.ElementAt(0).Value);
            Assert.AreEqual(2, graph.Vertices[0].ConnectedEdges.ElementAt(1).Value);
            Assert.AreEqual(1, graph.Vertices[0].ConnectedEdges.ElementAt(2).Value);
            // [1] 1 1 3 2 4 1
            Assert.AreEqual(1, graph.Vertices[1].ConnectedEdges.ElementAt(0).Value);
            Assert.AreEqual(2, graph.Vertices[1].ConnectedEdges.ElementAt(1).Value);
            Assert.AreEqual(1, graph.Vertices[1].ConnectedEdges.ElementAt(2).Value);
            // [2] 5 3 4 2 2 2 1 2
            Assert.AreEqual(3, graph.Vertices[2].ConnectedEdges.ElementAt(0).Value);
            Assert.AreEqual(2, graph.Vertices[2].ConnectedEdges.ElementAt(1).Value);
            Assert.AreEqual(2, graph.Vertices[2].ConnectedEdges.ElementAt(2).Value);
            Assert.AreEqual(2, graph.Vertices[2].ConnectedEdges.ElementAt(3).Value);
            // [3] 2 1 3 2 6 2 7 5
            Assert.AreEqual(1, graph.Vertices[3].ConnectedEdges.ElementAt(0).Value);
            Assert.AreEqual(2, graph.Vertices[3].ConnectedEdges.ElementAt(1).Value);
            Assert.AreEqual(2, graph.Vertices[3].ConnectedEdges.ElementAt(2).Value);
            Assert.AreEqual(5, graph.Vertices[3].ConnectedEdges.ElementAt(3).Value);
            // [4] 1 1 3 3 6 2
            Assert.AreEqual(1, graph.Vertices[4].ConnectedEdges.ElementAt(0).Value);
            Assert.AreEqual(3, graph.Vertices[4].ConnectedEdges.ElementAt(1).Value);
            Assert.AreEqual(2, graph.Vertices[4].ConnectedEdges.ElementAt(2).Value);
            // [5] 5 2 4 2 7 6
            Assert.AreEqual(2, graph.Vertices[5].ConnectedEdges.ElementAt(0).Value);
            Assert.AreEqual(2, graph.Vertices[5].ConnectedEdges.ElementAt(1).Value);
            Assert.AreEqual(6, graph.Vertices[5].ConnectedEdges.ElementAt(2).Value);
            // [6] 6 6 4 5
            Assert.AreEqual(6, graph.Vertices[6].ConnectedEdges.ElementAt(0).Value);
            Assert.AreEqual(5, graph.Vertices[6].ConnectedEdges.ElementAt(1).Value);
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

            var graph = new MetisGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();

            // [0] 5 1 3 2 2 1
            Assert.AreEqual(4, graph.Vertices[0].ConnectedEdges.ElementAt(0).Key);
            Assert.AreEqual(2, graph.Vertices[0].ConnectedEdges.ElementAt(1).Key);
            Assert.AreEqual(1, graph.Vertices[0].ConnectedEdges.ElementAt(2).Key);
            // [1] 1 1 3 2 4 1
            Assert.AreEqual(0, graph.Vertices[1].ConnectedEdges.ElementAt(0).Key);
            Assert.AreEqual(2, graph.Vertices[1].ConnectedEdges.ElementAt(1).Key);
            Assert.AreEqual(3, graph.Vertices[1].ConnectedEdges.ElementAt(2).Key);
            // [2] 5 3 4 2 2 2 1 2
            Assert.AreEqual(4, graph.Vertices[2].ConnectedEdges.ElementAt(0).Key);
            Assert.AreEqual(3, graph.Vertices[2].ConnectedEdges.ElementAt(1).Key);
            Assert.AreEqual(1, graph.Vertices[2].ConnectedEdges.ElementAt(2).Key);
            Assert.AreEqual(0, graph.Vertices[2].ConnectedEdges.ElementAt(3).Key);
            // [3] 2 1 3 2 6 2 7 5
            Assert.AreEqual(1, graph.Vertices[3].ConnectedEdges.ElementAt(0).Key);
            Assert.AreEqual(2, graph.Vertices[3].ConnectedEdges.ElementAt(1).Key);
            Assert.AreEqual(5, graph.Vertices[3].ConnectedEdges.ElementAt(2).Key);
            Assert.AreEqual(6, graph.Vertices[3].ConnectedEdges.ElementAt(3).Key);
            // [4] 1 1 3 3 6 2
            Assert.AreEqual(0, graph.Vertices[4].ConnectedEdges.ElementAt(0).Key);
            Assert.AreEqual(2, graph.Vertices[4].ConnectedEdges.ElementAt(1).Key);
            Assert.AreEqual(5, graph.Vertices[4].ConnectedEdges.ElementAt(2).Key);
            // [5] 5 2 4 2 7 6
            Assert.AreEqual(4, graph.Vertices[5].ConnectedEdges.ElementAt(0).Key);
            Assert.AreEqual(3, graph.Vertices[5].ConnectedEdges.ElementAt(1).Key);
            Assert.AreEqual(6, graph.Vertices[5].ConnectedEdges.ElementAt(2).Key);
            // [6] 6 6 4 5
            Assert.AreEqual(5, graph.Vertices[6].ConnectedEdges.ElementAt(0).Key);
            Assert.AreEqual(3, graph.Vertices[6].ConnectedEdges.ElementAt(1).Key);
        }

        [TestMethod]
        public void Graph_LocalCostFunction_ThreeColors()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new MetisGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();
            graph.ColorVerticesRandomly(_optionThreeColors.NumberOfPartitions);
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
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new MetisGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();
            graph.ColorVerticesRandomly(_optionTwoColors.NumberOfPartitions);
            graph.CalculateLocalCostFunction();

            Assert.AreEqual(1 / 3D, graph.Vertices[0].LocalCost);
            Assert.AreEqual(2 / 3D, graph.Vertices[1].LocalCost);
            Assert.AreEqual(0.5D, graph.Vertices[2].LocalCost);
            Assert.AreEqual(0.5D, graph.Vertices[3].LocalCost);
            Assert.AreEqual(1 / 3D, graph.Vertices[4].LocalCost);
            Assert.AreEqual(2 / 3D, graph.Vertices[5].LocalCost);
            Assert.AreEqual(0D, graph.Vertices[6].LocalCost);
        }

        [TestMethod]
        public void Graph_GlobalCostFunction_Initialized()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new MetisGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();
            graph.ColorVerticesRandomly(_optionTwoColors.NumberOfPartitions);
            var globalCost = graph.GetGlobalCostFunction();

            Assert.AreEqual(6, globalCost);
        }

        [TestMethod]
        public void Graph_GlobalCostFunction_OneColor()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new MetisGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();
            graph.ColorVerticesRandomly(_optionOneColors.NumberOfPartitions);
            var globalCost = graph.GetGlobalCostFunction();

            Assert.AreEqual(0, globalCost);
        }

        [TestMethod]
        public void Graph_MoveAntToVertexWithLowestCost_Success()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new MetisGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();
            graph.InitializeAnts(_optionTwoColors.NumberOfAnts);
            graph.ColorVerticesRandomly(_optionTwoColors.NumberOfPartitions);
            graph.CalculateLocalCostFunction();

            graph.MoveAntToVertexWithLowestCost(0);

            Assert.AreEqual(4, graph.Vertices[graph.Ants[0]].ID);

            graph.MoveAntToVertexWithLowestCost(1);

            Assert.AreEqual(3, graph.Vertices[graph.Ants[1]].ID);
        }

        [TestMethod]
        public void Graph_MoveAntToAnyAdjacentVertex_Success()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new MetisGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();
            graph.InitializeAnts(_optionTwoColors.NumberOfAnts);
            graph.ColorVerticesRandomly(_optionTwoColors.NumberOfPartitions);
            graph.CalculateLocalCostFunction();

            graph.MoveAntToAnyAdjacentVertex(0);

            Assert.AreEqual(4, graph.Vertices[graph.Ants[0]].ID);

            graph.MoveAntToAnyAdjacentVertex(1);

            Assert.AreEqual(5, graph.Vertices[graph.Ants[1]].ID);
        }

        [TestMethod]
        public void Grap_ChnageVertexColorWithBestColor_Success()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new MetisGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();
            graph.InitializeAnts(_optionTwoColors.NumberOfAnts);
            graph.ColorVerticesRandomly(_optionTwoColors.NumberOfPartitions);

            graph.ColorVertexWithBestColor(0);

            Assert.AreEqual(1, graph.Vertices[graph.Ants[0]].Color);

            graph.ColorVertexWithBestColor(1);

            Assert.AreEqual(2, graph.Vertices[graph.Ants[1]].Color);
        }

        [TestMethod]
        public void Grap_ChnageVertexColorWithRandomColor_Success()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new MetisGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();
            graph.InitializeAnts(_optionTwoColors.NumberOfAnts);
            graph.ColorVerticesRandomly(_optionTwoColors.NumberOfPartitions);

            graph.ColorVertexWithRandomColor(0, _optionTwoColors.NumberOfPartitions);

            Assert.AreEqual(1, graph.Vertices[graph.Ants[0]].Color);

            graph.ColorVertexWithRandomColor(1, _optionTwoColors.NumberOfPartitions);

            Assert.AreEqual(1, graph.Vertices[graph.Ants[1]].Color);
        }

        [TestMethod]
        public void Graph_ResetVerticeState_Success()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new MetisGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();
            graph.InitializeAnts(_optionTwoColors.NumberOfAnts);
            graph.ColorVerticesRandomly(_optionTwoColors.NumberOfPartitions);

            graph.ResetVerticesState();

            Assert.IsFalse(graph.Vertices.Any(vertex => vertex.LowestCost));
            Assert.IsTrue(graph.Vertices.All(vertex => vertex.OldColor == null));
        }

        [TestMethod]
        public void Graph_KeepBalance_Success()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new MetisGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();
            graph.InitializeAnts(_optionTwoColors.NumberOfAnts);
            graph.ColorVerticesRandomly(_optionTwoColors.NumberOfPartitions);
            graph.MoveAntToAnyAdjacentVertex(0);
            graph.ColorVertexWithBestColor(0);
            graph.MoveAntToAnyAdjacentVertex(1);
            graph.ColorVertexWithBestColor(1);

            graph.KeepBalance(_optionTwoColors.NumberVerticesForBalance);

            Assert.AreEqual(1, graph.Vertices[0].Color);
            Assert.AreEqual(2, graph.Vertices[1].Color);
            Assert.AreEqual(1, graph.Vertices[2].Color);
            Assert.AreEqual(2, graph.Vertices[3].Color);
            Assert.AreEqual(1, graph.Vertices[4].Color);
            Assert.AreEqual(1, graph.Vertices[5].Color);
            Assert.AreEqual(1, graph.Vertices[6].Color);
        }

        [TestMethod]
        public void Graph_UpdateLocalCostFunction_Success()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new MetisGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();
            graph.InitializeAnts(_optionTwoColors.NumberOfAnts);
            graph.ColorVerticesRandomly(_optionTwoColors.NumberOfPartitions);
            graph.MoveAntToAnyAdjacentVertex(0);
            graph.ColorVertexWithBestColor(0);
            graph.MoveAntToAnyAdjacentVertex(1);
            graph.ColorVertexWithBestColor(1);

            graph.UpdateLocalCostFunction();
            
            Assert.AreEqual(1/3D, graph.Vertices[0].LocalCost);
            Assert.AreEqual(0, graph.Vertices[1].LocalCost);
            Assert.AreEqual(0.5, graph.Vertices[2].LocalCost);
            Assert.AreEqual(0.75, graph.Vertices[3].LocalCost);
            Assert.AreEqual(1, graph.Vertices[4].LocalCost);
            Assert.AreEqual(1/3D, graph.Vertices[5].LocalCost);
            Assert.AreEqual(0.5, graph.Vertices[6].LocalCost);
        }
    }
}
