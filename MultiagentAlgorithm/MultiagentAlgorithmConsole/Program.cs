using System;
using System.Diagnostics;
using System.Reflection;
using CommandLine;
using log4net;
using log4net.Config;
using MultiagentAlgorithm;

namespace MultiagentAlgorithmConsole
{
    static class Program
    {
        private static ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // part -t 0 -i "Graphs\DIMACS\myciel3.col" -a 1 -p 2 -c 0.9 -m 0.85 -s 6 -d 100
        static int Main(string[] args)
        {
            XmlConfigurator.Configure();

            Func<PartitionGraphOptions, int> partitionFunc = options =>
                {
                    var rnd = new Random(Environment.TickCount);
                    //var rnd = new Random(1);
                    var fileWriter = new FileWriter("graph.json");
                    var exportGraph = new GephiFileExport(fileWriter);

                    var fileLoader = new FileLoader(options.InputGraphFilePath);
                    BaseGraph graph;
                    switch (options.ImportGraphFileType)
                    {
                        case GraphInputFileType.Dimacs:
                            graph = new DimacsGraph(fileLoader, rnd);
                            break;
                        case GraphInputFileType.Metis:
                            graph = new DimacsGraph(fileLoader, rnd);
                            break;
                        default:
                            throw new ApplicationException("Graph file format is not suppoted.");
                    }

                    var graphOptions = new Options(options.NumberOfAnts, options.NumberOfPartitions, options.ColoringProbability,
                                                   options.MovingProbability, options.NumberOfVerticesForBalance, options.NumberOfIterations);

                    ResultData resultData = Algorithm.Run(graph, graphOptions, rnd, exportGraph);
                    return resultData.BestCost;
                };

            var result = Parser.Default.ParseArguments<PartitionGraphOptions>(args);

            var exitCode = result.MapResult(options =>
                {
                    if (options.Verbose)
                    {
                        Console.WriteLine("Write additional information.");
                    }
                    else
                    {
                        Console.WriteLine("Processing...");

                        var stopwatch = new Stopwatch();
                        stopwatch.Start();

                        var bestCost = partitionFunc(options);

                        stopwatch.Stop();
                        Log.Warn($"Algoritham run time: {stopwatch.ElapsedMilliseconds}");
                        Log.Warn($"Best cost: {bestCost}");

                        Console.WriteLine($"{args[0]} | {bestCost}");
                    }
                    return 0;
                }, errors => 1);

            return exitCode;
        }
    }
}
