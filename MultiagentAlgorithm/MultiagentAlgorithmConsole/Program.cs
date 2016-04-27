using System;
using System.Reflection;
using log4net;
using log4net.Config;
using MultiagentAlgorithm;

namespace MultiagentAlgorithmConsole
{
    static class Program
    {
        public static ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static readonly string TestGraphFilePath = @"Graphs/test.txt";
        static readonly string Myciel3GraphFilePath = @"Graphs/DIMACS/myciel3.col";
        private static readonly string Queen55GraphFilePath = @"Graphs/DIMACS/queen5_5.col";
        private static readonly string Queen88GraphFilePath = @"Graphs/DIMACS/queen8_8.col";
        private static readonly string Queen1212GraphFilePath = @"Graphs/DIMACS/queen12_12.col";
        private static readonly string Queen1616GraphFilePath = @"Graphs/DIMACS/queen16_16.col";
        private static readonly string Myciel4GraphFilePath = @"Graphs/DIMACS/myciel4.col";
        private static readonly string JeanGraphFilePath = @"Graphs/DIMACS/jean.col";
        private static readonly string Miles500GraphFilePath = @"Graphs/DIMACS/miles500.col";
        private static readonly string Le45015bGraphFilePath = @"Graphs/DIMACS/le450_15b.col";

        private enum AlgortithamTest : byte
        {
            TestMetis,
            Myciel3,
            Queen55,
            Queen88,
            Queen1212,
            Queen1616,
            Myciel4,
            Miles500,
            Jean,
            Le45015B
        }

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            var algortithamTest = (AlgortithamTest)Enum.Parse(typeof(AlgortithamTest), args[0]);
            var rnd = new Random(Environment.TickCount);
            var fileWriter = new FileWriter("graph.json");
            var exportGraph = new GephiFileExport(fileWriter);

            switch (algortithamTest)
            {
                case AlgortithamTest.TestMetis:
                    var optionsMetis = new Options(numberOfAnts: 3, numberOfPartitions: 2, coloringProbability: 0.9,
                        movingProbability: 0.85, graphFilePath: TestGraphFilePath, numberVerticesForBalance: 4, numberOfIterations: 1000);
                    var loaderMetis = new FileLoader(optionsMetis.GraphFilePath);
                    var graphMetis = new MetisGraph(loaderMetis, rnd);
                    Algorithm.Run(graphMetis, optionsMetis, rnd, exportGraph);
                    break;
                case AlgortithamTest.Myciel3:
                    var optionsMyciel3 = new Options(numberOfAnts: 3, numberOfPartitions: 2, coloringProbability: 0.9,
                        movingProbability: 0.85, graphFilePath: Myciel3GraphFilePath, numberVerticesForBalance: 6, numberOfIterations: 1000);
                    var loaderMyciel3 = new FileLoader(optionsMyciel3.GraphFilePath);
                    var graphDimacs = new DimacsGraph(loaderMyciel3, rnd);
                    Algorithm.Run(graphDimacs, optionsMyciel3, rnd, exportGraph);
                    break;
                case AlgortithamTest.Queen55:
                    var optionsQueen55 = new Options(numberOfAnts: 3, numberOfPartitions: 2, coloringProbability: 0.9,
                        movingProbability: 0.85, graphFilePath: Queen55GraphFilePath, numberVerticesForBalance: 10, numberOfIterations: 1000);
                    var loaderQueen55 = new FileLoader(optionsQueen55.GraphFilePath);
                    var graphQueen55 = new DimacsGraphBidirectional(loaderQueen55, rnd);
                    Algorithm.Run(graphQueen55, optionsQueen55, rnd, exportGraph);
                    break;
                case AlgortithamTest.Queen88:
                    var optionsQueen88 = new Options(numberOfAnts: 4, numberOfPartitions: 4, coloringProbability: 0.9,
                        movingProbability: 0.85, graphFilePath: Queen88GraphFilePath, numberVerticesForBalance: 30, numberOfIterations: 1000);
                    var loaderQueen88 = new FileLoader(optionsQueen88.GraphFilePath);
                    var graphQueen88 = new DimacsGraphBidirectional(loaderQueen88, rnd);
                    Algorithm.Run(graphQueen88, optionsQueen88, rnd, exportGraph);
                    break;
                case AlgortithamTest.Queen1212:
                    var optionsQueen1212 = new Options(numberOfAnts: 6, numberOfPartitions: 9, coloringProbability: 0.9,
                        movingProbability: 0.85, graphFilePath: Queen1212GraphFilePath, numberVerticesForBalance: 70, numberOfIterations: 1000);
                    var loaderQueen1212 = new FileLoader(optionsQueen1212.GraphFilePath);
                    var graphQueen1212 = new DimacsGraphBidirectional(loaderQueen1212, rnd);
                    Algorithm.Run(graphQueen1212, optionsQueen1212, rnd, exportGraph);
                    break;
                case AlgortithamTest.Queen1616:
                    var optionsQueen1616 = new Options(numberOfAnts: 7, numberOfPartitions: 16, coloringProbability: 0.9,
                        movingProbability: 0.85, graphFilePath: Queen1616GraphFilePath, numberVerticesForBalance: 150, numberOfIterations: 1000);
                    var loaderQueen1616 = new FileLoader(optionsQueen1616.GraphFilePath);
                    var graphQueen1616 = new DimacsGraphBidirectional(loaderQueen1616, rnd);
                    Algorithm.Run(graphQueen1616, optionsQueen1616, rnd, exportGraph);
                    break;
                case AlgortithamTest.Myciel4:
                    var optionsMyciel4 = new Options(numberOfAnts: 2, numberOfPartitions: 2, coloringProbability: 0.9,
                        movingProbability: 0.85, graphFilePath: Myciel4GraphFilePath, numberVerticesForBalance: 12, numberOfIterations: 1000);
                    var loaderMyciel4 = new FileLoader(optionsMyciel4.GraphFilePath);
                    var graphMyciel4 = new DimacsGraph(loaderMyciel4, rnd);
                    Algorithm.Run(graphMyciel4, optionsMyciel4, rnd, exportGraph);
                    break;
                case AlgortithamTest.Jean:
                    var optionsJean = new Options(numberOfAnts: 3, numberOfPartitions: 5, coloringProbability: 0.9,
                        movingProbability: 0.85, graphFilePath: JeanGraphFilePath, numberVerticesForBalance: 35, numberOfIterations: 1000);
                    var loaderJean = new FileLoader(optionsJean.GraphFilePath);
                    var graphJean = new DimacsGraphBidirectional(loaderJean, rnd);
                    Algorithm.Run(graphJean, optionsJean, rnd, exportGraph);
                    break;
                case AlgortithamTest.Miles500:
                    var optionsMiles500 = new Options(numberOfAnts: 7, numberOfPartitions: 8, coloringProbability: 0.9,
                        movingProbability: 0.85, graphFilePath: Miles500GraphFilePath, numberVerticesForBalance: 60, numberOfIterations: 1000);
                    var loaderMiles500 = new FileLoader(optionsMiles500.GraphFilePath);
                    var graphMiles500 = new DimacsGraphBidirectional(loaderMiles500, rnd);
                    Algorithm.Run(graphMiles500, optionsMiles500, rnd, exportGraph);
                    break;
                case AlgortithamTest.Le45015B:
                    var optionsLe45015B = new Options(numberOfAnts: 8, numberOfPartitions: 29, coloringProbability: 0.9,
                        movingProbability: 0.85, graphFilePath: Le45015bGraphFilePath, numberVerticesForBalance: 200, numberOfIterations: 1000);
                    var loaderLe45015B = new FileLoader(optionsLe45015B.GraphFilePath);
                    var graphLe45015B = new DimacsGraph(loaderLe45015B, rnd);
                    Algorithm.Run(graphLe45015B, optionsLe45015B, rnd, exportGraph);
                    break;
                default:
                    return;
            }
        }
    }
}
