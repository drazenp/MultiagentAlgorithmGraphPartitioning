using System;
using System.Reflection;
using log4net;
using log4net.Config;
using MultiagentAlgorithm;

namespace MultiagentAlgorithmConsole
{
    static class Program
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static readonly string TestGraphFilePath = @"Graphs/test.txt";
        static readonly string Myciel3GraphFilePath = @"Graphs/DIMACS/myciel3.col";

        private enum AlgortithamTest : byte
        {
            None,
            TestMetis
        }

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            var algortithamTest = (AlgortithamTest)Enum.Parse(typeof (AlgortithamTest), args[0]);

            switch (algortithamTest)
            {
                case AlgortithamTest.TestMetis:
                    var options = new Options(numberOfAnts: 5, numberOfPartitions: 3, coloringProbability: 0.9,
                        movingProbability: 0.95, graphFilePath: TestGraphFilePath, numberVerticesForBalance: 3, numberOfIterations: 100);
                    var rnd = new Random(Environment.TickCount);
                    var loader = new FileLoader(options.GraphFilePath);
                    var graph = new MetisGraph(loader, rnd);
                    Algorithm.Run(graph, options, rnd);
                break;
            }
        }
    }
}
