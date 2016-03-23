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

        private enum AlgortithamTest : byte
        {
            TestMetis,
            TestDimacs
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
                    var optionsDimacs = new Options(numberOfAnts: 2, numberOfPartitions: 2, coloringProbability: 0.9,
                        movingProbability: 0.95, graphFilePath: Myciel3GraphFilePath, numberVerticesForBalance: 3, numberOfIterations: 10000);
                    var loaderDimacs = new FileLoader(optionsDimacs.GraphFilePath);
                    var graphDimacs = new DimacsGraph(loaderDimacs, rnd);
                    Algorithm.Run(graphDimacs, optionsDimacs, rnd);
                    break;
                default:
                    return;
            }
        }
    }
}
