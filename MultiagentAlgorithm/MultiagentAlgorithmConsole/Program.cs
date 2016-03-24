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

        private enum AlgortithamTest : byte
        {
            TestMetis,
            TestDimacs,
            Queen55
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
                    var optionsDimacs = new Options(numberOfAnts: 1, numberOfPartitions: 2, coloringProbability: 0.9,
                        movingProbability: 0.95, graphFilePath: Myciel3GraphFilePath, numberVerticesForBalance: 3, numberOfIterations: 100);
                    var loaderDimacs = new FileLoader(optionsDimacs.GraphFilePath);
                    var graphDimacs = new DimacsGraph(loaderDimacs, rnd);
                    Algorithm.Run(graphDimacs, optionsDimacs, rnd);
                    break;
                case AlgortithamTest.Queen55:
                    var optionsQueen55 = new Options(numberOfAnts: 1, numberOfPartitions: 5, coloringProbability: 0.9,
                        movingProbability: 0.95, graphFilePath: Queen55GraphFilePath, numberVerticesForBalance: 5, numberOfIterations: 100);
                    var loaderQueen55 = new FileLoader(optionsQueen55.GraphFilePath);
                    var graphQueen55 = new DimacsGraphBidirectional(loaderQueen55, rnd);
                    Algorithm.Run(graphQueen55, optionsQueen55, rnd);
                    break;
                default:
                    return;
            }
        }
    }
}
