﻿using CommandLine;
using log4net;
using log4net.Config;
using MultiagentAlgorithm;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MultiagentAlgorithmConsole
{
   static class Program
   {
      private static ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

      // part -t 0 -i "Graphs\DIMACS\myciel3.col" -a 1 -p 2 -c 0.9 -m 0.85 -s 6 -d 100
      // part -t 0 -i "Graphs\DIMACS\le450_15b.col" -a 5 -p 29 -c 0.9 -m 0.85 -s 6 -d 1
      // part -t 1 -i "Graphs\manual.txt" -a 1 -p 2 -c 0.9 -m 0.85 -s 4 -d 100
      static int Main(string[] args)
      {
         XmlConfigurator.Configure();

         Func<PartitionGraphOptions, int> partitionFunc = options =>
             {
                var rnd = new Random(Environment.TickCount);
                var fileWriter = new FileWriter("graph.json");
                var exportGraph = new GephiFileExport(fileWriter, Path.GetFileNameWithoutExtension(options.InputGraphFilePath));

                var fileLoader = new FileLoader(options.InputGraphFilePath);
                BaseGraph graph;
                switch (options.ImportGraphFileType)
                {
                   case GraphInputFileType.Dimacs:
                      graph = new DimacsGraph(fileLoader, rnd);
                      break;
                   case GraphInputFileType.Metis:
                      graph = new MetisGraph(fileLoader, rnd);
                      break;
                   case GraphInputFileType.MetisUnweighted:
                      graph = new MetisUnweightedGraph(fileLoader, rnd);
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
