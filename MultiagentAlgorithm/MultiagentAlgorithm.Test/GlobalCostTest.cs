using System.Collections.Generic;
using System.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MultiagentAlgorithm.Test
{
    [TestClass]
    public class GlobalCostTest
    {
        private readonly List<string> _myciel4 = new List<string>() { "p edges 23 71",
                                                                      "e 1 2",
                                                                      "e 1 4",
                                                                      "e 1 7",
                                                                      "e 1 9",
                                                                      "e 1 13",
                                                                      "e 1 15",
                                                                      "e 1 18",
                                                                      "e 1 20",
                                                                      "e 2 3",
                                                                      "e 2 6",
                                                                      "e 2 8",
                                                                      "e 2 12",
                                                                      "e 2 14",
                                                                      "e 2 17",
                                                                      "e 2 19",
                                                                      "e 3 5",
                                                                      "e 3 7",
                                                                      "e 3 10",
                                                                      "e 3 13",
                                                                      "e 3 16",
                                                                      "e 3 18",
                                                                      "e 3 21",
                                                                      "e 4 5",
                                                                      "e 4 6",
                                                                      "e 4 10",
                                                                      "e 4 12",
                                                                      "e 4 16",
                                                                      "e 4 17",
                                                                      "e 4 21",
                                                                      "e 5 8",
                                                                      "e 5 9",
                                                                      "e 5 14",
                                                                      "e 5 15",
                                                                      "e 5 19",
                                                                      "e 5 20",
                                                                      "e 6 11",
                                                                      "e 6 13",
                                                                      "e 6 15",
                                                                      "e 6 22",
                                                                      "e 7 11",
                                                                      "e 7 12",
                                                                      "e 7 14",
                                                                      "e 7 22",
                                                                      "e 8 11",
                                                                      "e 8 13",
                                                                      "e 8 16",
                                                                      "e 8 22",
                                                                      "e 9 11",
                                                                      "e 9 12",
                                                                      "e 9 16",
                                                                      "e 9 22",
                                                                      "e 10 11",
                                                                      "e 10 14",
                                                                      "e 10 15",
                                                                      "e 10 22",
                                                                      "e 11 17",
                                                                      "e 11 18",
                                                                      "e 11 19",
                                                                      "e 11 20",
                                                                      "e 11 21",
                                                                      "e 12 23",
                                                                      "e 13 23",
                                                                      "e 14 23",
                                                                      "e 15 23",
                                                                      "e 16 23",
                                                                      "e 17 23",
                                                                      "e 18 23",
                                                                      "e 19 23",
                                                                      "e 20 23",
                                                                      "e 21 23",
                                                                      "e 22 23"};

        private readonly List<string> _metisTest = new List<string>() {"7 11 011",
                                                                       "4 5 1 3 2 2 1",
                                                                       "2 1 1 3 2 4 1",
                                                                       "5 5 3 4 2 2 2 1 2",
                                                                       "3 2 1 3 2 6 2 7 5",
                                                                       "1 1 1 3 3 6 2",
                                                                       "6 5 2 4 2 7 6",
                                                                       "2 6 6 4 5"};

        // Uncomment if needed.
        //private readonly Options _optionMyciel4 = new Options(numberOfAnts: 2, numberOfPartitions: 2, coloringProbability: 0.9,
        //        movingProbability: 0.85, graphFilePath: string.Empty, numberOfVerticesForBalance: 12, numberOfIterations: 100);

        //private readonly Options _optionMetisTest = new Options(numberOfAnts: 2, numberOfPartitions: 2, coloringProbability: 0.9,
        //        movingProbability: 0.85, graphFilePath: string.Empty, numberOfVerticesForBalance: 4, numberOfIterations: 5);


        [TestMethod]
        public void GraphMyciel4_CalculateGlobalCostFunction_Success()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_myciel4);

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new DimacsGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();

            graph.Vertices[0].Color = 1;
            graph.Vertices[1].Color = 1;
            graph.Vertices[2].Color = 1;
            graph.Vertices[3].Color = 1;
            graph.Vertices[4].Color = 0;
            graph.Vertices[5].Color = 1;
            graph.Vertices[6].Color = 1;
            graph.Vertices[7].Color = 1;
            graph.Vertices[8].Color = 0;
            graph.Vertices[9].Color = 0;
            graph.Vertices[10].Color = 1;
            graph.Vertices[11].Color = 0;
            graph.Vertices[12].Color = 0;
            graph.Vertices[13].Color = 1;
            graph.Vertices[14].Color = 0;
            graph.Vertices[15].Color = 1;
            graph.Vertices[16].Color = 1;
            graph.Vertices[17].Color = 0;
            graph.Vertices[18].Color = 0;
            graph.Vertices[19].Color = 0;
            graph.Vertices[20].Color = 1;
            graph.Vertices[21].Color = 0;
            graph.Vertices[22].Color = 0;

            var globalCost = graph.GetGlobalCostFunction();

            Assert.AreEqual(68, globalCost);
        }

        [TestMethod]
        public void GraphMetisTest_CalculateGlobalCostFunction_Success()
        {
            var loaderMock = new Mock<IDataLoader>();
            loaderMock.Setup(m => m.LoadData()).Returns(_metisTest);

            var randomMock = new StubRandom()
            {
                NextInt32Int32 = (a, b) => 1
            };

            var graph = new MetisGraph(loaderMock.Object, randomMock);
            graph.InitializeGraph();

            graph.Vertices[0].Color = 2;
            graph.Vertices[1].Color = 2;
            graph.Vertices[2].Color = 2;
            graph.Vertices[3].Color = 1;
            graph.Vertices[4].Color = 2;
            graph.Vertices[5].Color = 1;
            graph.Vertices[6].Color = 1;

            var globalCost = graph.GetGlobalCostFunction();

            Assert.AreEqual(6, globalCost);
        }
    }
}
