using System.Collections.Generic;
using System.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MultiagentAlgorithm.Test
{
    [TestClass]
    public class MetisGraphTest
    {
        private readonly List<string> _dummyFile = new List<string>() {"7 11 011",
                                                                       "4 5 1 3 2 2 1",
                                                                       "2 1 1 3 2 4 1",
                                                                       "5 5 3 4 2 2 2 1 2",
                                                                       "3 2 1 3 2 6 2 7 5",
                                                                       "1 1 1 3 3 6 2",
                                                                       "6 5 2 4 2 7 6",
                                                                       "2 6 6 4 5"};

        private readonly Options _optionTwoColors = new Options(numberOfAnts: 2, numberOfPartitions: 2, coloringProbability: 0.9,
           movingProbability: 0.85, graphFilePath: string.Empty, numberOfVerticesForBalance: 4, numberOfIterations: 5);

        [TestMethod]
        public void MetisGraph_FirstLineRead_Success()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_dummyFile);
            var randomMock = new StubRandom();

            var graph = new MetisGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();

            Assert.AreEqual(7, graph.Vertices.Length, "The number of vertices weights is not correct.");
            Assert.AreEqual(11, graph.NumberOfEdges, "The number of edges is not correct.");
        }

        [TestMethod]
        public void MetisGraph_FirstIteration_Success()
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

            // First ant.
            Vertex vertexWithFirstAnt = graph.MoveAntToVertexWithLowestCost(0);
            int oldColorFirstAnt = vertexWithFirstAnt.Color;
            int vertexWithAntIDFirstAnt = vertexWithFirstAnt.ID;
            Vertex vertexWithNewColorFirstAnt = graph.ColorVertexWithBestColor(0);

            Vertex vertexWhichKeepBalanceFirstAnt = graph.KeepBalance(_optionTwoColors.NumberOfVerticesForBalance,
                                        vertexWithAntIDFirstAnt, oldColorFirstAnt, vertexWithNewColorFirstAnt.Color);
            graph.UpdateLocalCostFunction(vertexWhichKeepBalanceFirstAnt, vertexWithNewColorFirstAnt);

            // Second ant.
            Vertex vertexWithSecondAnt = graph.MoveAntToVertexWithLowestCost(1);
            int oldColorSecondAnt = vertexWithSecondAnt.Color;
            int vertexWithAntIDSecondAnt = vertexWithSecondAnt.ID;
            Vertex vertexWithNewColorSecondAnt = graph.ColorVertexWithBestColor(1);

            Vertex vertexWhichKeepBalanceSecondAnt = graph.KeepBalance(_optionTwoColors.NumberOfVerticesForBalance,
                                        vertexWithAntIDSecondAnt, oldColorSecondAnt, vertexWithNewColorSecondAnt.Color);
            graph.UpdateLocalCostFunction(vertexWhichKeepBalanceSecondAnt, vertexWithNewColorSecondAnt);

            Assert.AreEqual(3 / 4D, graph.Vertices[0].LocalCost);
            Assert.AreEqual(1D, graph.Vertices[1].LocalCost);
            Assert.AreEqual(3 / 4D, graph.Vertices[2].LocalCost);
            Assert.AreEqual(0.5, graph.Vertices[3].LocalCost);
            Assert.AreEqual(0.5, graph.Vertices[4].LocalCost);                                                                                                                                                         
            Assert.AreEqual(3 / 4D, graph.Vertices[5].LocalCost);
            Assert.AreEqual(3 / 4D, graph.Vertices[6].LocalCost);
            LoggerHelper.LogChangesOnVertices(graph.changes);
        }

        [TestMethod]
        public void MetisGraph_SecondIteration_Success()
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

            #region First Iteration
            // First ant.
            Vertex vertexWithFirstAnt = graph.MoveAntToVertexWithLowestCost(0);
            int oldColorFirstAnt = vertexWithFirstAnt.Color;
            int vertexWithAntIDFirstAnt = vertexWithFirstAnt.ID;
            Vertex vertexWithNewColorFirstAnt = graph.ColorVertexWithBestColor(0);

            Vertex vertexWhichKeepBalanceFirstAnt = graph.KeepBalance(_optionTwoColors.NumberOfVerticesForBalance,
                                        vertexWithAntIDFirstAnt, oldColorFirstAnt, vertexWithNewColorFirstAnt.Color);
            graph.UpdateLocalCostFunction(vertexWhichKeepBalanceFirstAnt, vertexWithNewColorFirstAnt);

            // Second ant.
            Vertex vertexWithSecondAnt = graph.MoveAntToVertexWithLowestCost(1);
            int oldColorSecondAnt = vertexWithSecondAnt.Color;
            int vertexWithAntIDSecondAnt = vertexWithSecondAnt.ID;
            Vertex vertexWithNewColorSecondAnt = graph.ColorVertexWithBestColor(1);

            Vertex vertexWhichKeepBalanceSecondAnt = graph.KeepBalance(_optionTwoColors.NumberOfVerticesForBalance,
                                        vertexWithAntIDSecondAnt, oldColorSecondAnt, vertexWithNewColorSecondAnt.Color);
            graph.UpdateLocalCostFunction(vertexWhichKeepBalanceSecondAnt, vertexWithNewColorSecondAnt);

            Assert.AreEqual(3 / 4D, graph.Vertices[0].LocalCost);
            Assert.AreEqual(1D, graph.Vertices[1].LocalCost);
            Assert.AreEqual(3 / 4D, graph.Vertices[2].LocalCost);
            Assert.AreEqual(0.5, graph.Vertices[3].LocalCost);
            Assert.AreEqual(0.5, graph.Vertices[4].LocalCost);
            Assert.AreEqual(3 / 4D, graph.Vertices[5].LocalCost);
            Assert.AreEqual(3 / 4D, graph.Vertices[6].LocalCost);
            #endregion

            #region Second Iteration
            // First ant.
            vertexWithFirstAnt = graph.MoveAntToVertexWithLowestCost(0);
            oldColorFirstAnt = vertexWithFirstAnt.Color;
            vertexWithAntIDFirstAnt = vertexWithFirstAnt.ID;
            vertexWithNewColorFirstAnt = graph.ColorVertexWithBestColor(0);

            vertexWhichKeepBalanceFirstAnt = graph.KeepBalance(_optionTwoColors.NumberOfVerticesForBalance,
                            vertexWithAntIDFirstAnt, oldColorFirstAnt, vertexWithNewColorFirstAnt.Color);
            graph.UpdateLocalCostFunction(vertexWhichKeepBalanceFirstAnt, vertexWithNewColorFirstAnt);

            // Second ant.
            vertexWithSecondAnt = graph.MoveAntToVertexWithLowestCost(1);
            oldColorSecondAnt = vertexWithSecondAnt.Color;
            vertexWithAntIDSecondAnt = vertexWithSecondAnt.ID;
            vertexWithNewColorSecondAnt = graph.ColorVertexWithBestColor(1);

            vertexWhichKeepBalanceSecondAnt = graph.KeepBalance(_optionTwoColors.NumberOfVerticesForBalance,
                            vertexWithAntIDSecondAnt, oldColorSecondAnt, vertexWithNewColorSecondAnt.Color);
            graph.UpdateLocalCostFunction(vertexWhichKeepBalanceSecondAnt, vertexWithNewColorSecondAnt);

            Assert.AreEqual(3 / 4D, graph.Vertices[0].LocalCost);
            Assert.AreEqual(1D, graph.Vertices[1].LocalCost);
            Assert.AreEqual(3 / 4D, graph.Vertices[2].LocalCost);
            Assert.AreEqual(0.5, graph.Vertices[3].LocalCost);
            Assert.AreEqual(0.5, graph.Vertices[4].LocalCost);
            Assert.AreEqual(3 / 4D, graph.Vertices[5].LocalCost);
            Assert.AreEqual(3 / 4D, graph.Vertices[6].LocalCost);
            LoggerHelper.LogChangesOnVertices(graph.changes);
            #endregion
        }
    }
}
