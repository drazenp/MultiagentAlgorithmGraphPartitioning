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

        static readonly string GraphFilePath = @"Graphs/test.txt";

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            var option = new Options(numberOfAnts: 5, numberOfPartitions: 3, coloringProbability: 0.9, 
                movingProbability: 0.95, graphFilePath: GraphFilePath, numberVerticesForBalance: 1, numberOfIterations: 100);
            var rnd = new Random(Environment.TickCount);
            Algorithm.Run(option, rnd);
        }
    }
}
