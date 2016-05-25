using System;
using MultiagentAlgorithm;

namespace MultiAgentAlgorithmAnalyze
{
    static class Program
    {
        static void Main()
        {
            var rnd = new Random(Environment.TickCount);

            Func<AnalyzeData, BaseGraph> graphFunc = data =>
            {
                var fileLoader = new FileLoader(data.GraphFilePath);

                BaseGraph graph;
                switch (data.GraphFileType)
                {
                    case GraphFileType.Dimacs:
                        graph = new DimacsGraph(fileLoader, rnd);
                        break;
                    case GraphFileType.Metis:
                        graph = new DimacsGraph(fileLoader, rnd);
                        break;
                    default:
                        throw new ApplicationException("Graph file format is not suppoted.");
                }
                return graph;
            };

            var analyzeData = AnalyzeDataAccess.GetAnalyzeData();

            while (analyzeData != null)
            {
                var startDate = DateTime.UtcNow;
                var inputGraph = graphFunc(analyzeData);

                var fileWriter = new FileWriter("graph.json");
                var exportGraph = new GephiFileExport(fileWriter);

                var graphOptions = new Options(analyzeData.NumberOfAnts, analyzeData.NumberOfPartitions, analyzeData.ColoringProbability,
                                               analyzeData.MovingProbability, analyzeData.NumberOfVerticesForBalance, analyzeData.NumberOfIterations);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(graphOptions);

                ResultData resultData = Algorithm.Run(inputGraph, graphOptions, rnd, exportGraph);

                var analyzeResult = new AnalyzeResult
                {
                    AnalyzeID = analyzeData.ID,
                    BestCost = resultData.BestCost,
                    BestCostIteration = resultData.BestCostIteration,
                    Duration = resultData.ElapsedMilliseconds,
                    StartDate = startDate,
                    EndDate = DateTime.UtcNow
                };

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(analyzeResult);

                AnalyzeDataAccess.SaveAnalyzeResult(analyzeResult);

                analyzeData = AnalyzeDataAccess.GetAnalyzeData();
            }
        }
    }
}
