using System;
using MultiagentAlgorithm;

namespace MultiAgentAlgorithmAnalyze
{
    class Program
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
            //var startDate = 
            var inputGraph = graphFunc(analyzeData);

            var fileWriter = new FileWriter("graph.json");
            var exportGraph = new GephiFileExport(fileWriter);

            var graphOptions = new Options(analyzeData.NumberOfAnts, analyzeData.NumberOfPartitions, analyzeData.ColoringProbability,
                                                   analyzeData.MovingProbability, analyzeData.NumberOfVerticesForBalance, analyzeData.NumberOfIterations);

            ResultData resultData = Algorithm.Run(inputGraph, graphOptions, rnd, exportGraph);

            AnalyzeDataAccess.SaveAnalyzeResult(analyzeData.ID, resultData);
        }
    }
}
