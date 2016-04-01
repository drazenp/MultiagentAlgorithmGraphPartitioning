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

        private enum AlgortithamTest : byte
        {
            TestMetis,
            TestDimacs,
            Queen55,
            Queen88,
            Queen1212,
            Queen1616
        }

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            var algortithamTest = (AlgortithamTest)Enum.Parse(typeof(AlgortithamTest), args[0]);
            var rnd = new Random(Environment.TickCount);

            switch (algortithamTest)
            {
                case AlgortithamTest.TestMetis:
                    var optionsMetis = new Options(numberOfAnts: 2, numberOfPartitions: 2, coloringProbability: 0.9,
                        movingProbability: 0.95, graphFilePath: TestGraphFilePath, numberVerticesForBalance: 3, numberOfIterations: 100);
                    var loaderMetis = new FileLoader(optionsMetis.GraphFilePath);
                    var graphMetis = new MetisGraph(loaderMetis, rnd);
                    Algorithm.Run(graphMetis, optionsMetis, rnd);
                    break;
                case AlgortithamTest.TestDimacs:
                    var optionsDimacs = new Options(numberOfAnts: 1, numberOfPartitions: 3, coloringProbability: 0.9,
                        movingProbability: 0.95, graphFilePath: Myciel3GraphFilePath, numberVerticesForBalance: 3, numberOfIterations: 100);
                    var loaderDimacs = new FileLoader(optionsDimacs.GraphFilePath);
                    var graphDimacs = new DimacsGraph(loaderDimacs, rnd);
                    Algorithm.Run(graphDimacs, optionsDimacs, rnd);
                    break;
                case AlgortithamTest.Queen55:
                    var optionsQueen55 = new Options(numberOfAnts: 5, numberOfPartitions: 2, coloringProbability: 0.9,
                        movingProbability: 0.95, graphFilePath: Queen55GraphFilePath, numberVerticesForBalance: 10, numberOfIterations: 300);
                    var loaderQueen55 = new FileLoader(optionsQueen55.GraphFilePath);
                    var graphQueen55 = new DimacsGraphBidirectional(loaderQueen55, rnd);
                    Algorithm.Run(graphQueen55, optionsQueen55, rnd);
                    break;
                case AlgortithamTest.Queen88:
                    var optionsQueen88 = new Options(numberOfAnts: 8, numberOfPartitions: 4, coloringProbability: 0.9,
                        movingProbability: 0.95, graphFilePath: Queen88GraphFilePath, numberVerticesForBalance: 30, numberOfIterations: 100);
                    var loaderQueen88 = new FileLoader(optionsQueen88.GraphFilePath);
                    var graphQueen88 = new DimacsGraphBidirectional(loaderQueen88, rnd);
                    Algorithm.Run(graphQueen88, optionsQueen88, rnd);
                    break;
                case AlgortithamTest.Queen1212:
                    var optionsQueen1212 = new Options(numberOfAnts: 12, numberOfPartitions: 9, coloringProbability: 0.9,
                        movingProbability: 0.95, graphFilePath: Queen1212GraphFilePath, numberVerticesForBalance: 70, numberOfIterations: 100);
                    var loaderQueen1212 = new FileLoader(optionsQueen1212.GraphFilePath);
                    var graphQueen1212 = new DimacsGraphBidirectional(loaderQueen1212, rnd);
                    Algorithm.Run(graphQueen1212, optionsQueen1212, rnd);
                    break;
                case AlgortithamTest.Queen1616:
                    var optionsQueen1616 = new Options(numberOfAnts: 16, numberOfPartitions: 16, coloringProbability: 0.9,
                        movingProbability: 0.95, graphFilePath: Queen1616GraphFilePath, numberVerticesForBalance: 120, numberOfIterations: 100);
                    var loaderQueen1616 = new FileLoader(optionsQueen1616.GraphFilePath);
                    var graphQueen1616 = new DimacsGraphBidirectional(loaderQueen1616, rnd);
                    Algorithm.Run(graphQueen1616, optionsQueen1616, rnd);
                    break;
                default:
                    return;
            }
        }
    }
}
