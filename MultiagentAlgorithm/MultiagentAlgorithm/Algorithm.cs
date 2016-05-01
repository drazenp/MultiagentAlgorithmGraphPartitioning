using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;

namespace MultiagentAlgorithm
{
    public static class Algorithm
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static int Run(BaseGraph graph, Options options, Random rnd, IExportGraph graphExport)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            graph.InitializeGraph();
            graph.InitializeAnts(options.NumberOfAnts);
            graph.ColorVerticesRandomly(options.NumberOfPartitions);
            graph.CalculateLocalCostFunction();

            var bestCost = graph.GetGlobalCostFunction();
            var bestDistribution = graph.Vertices;
            Log.Info($"Initial global cost: {bestCost}");

            var iteration = 0;
            while(bestCost > 0 && iteration < options.NumberOfIterations)
            { 
                // At a given iteration each ant moves from the current position 
                // to the adjacent vertex with the lowest local cost, 
                // i.e. the vertex with the greatest number of constraints (neighbors of a different color).
                // which increases the local cost.
                foreach (var ant in graph.Ants.Keys.ToArray())
                {
                    // The agent or ant moves to the worst adjacent vertex with a
                    // probability pm (it moves randomly to any other adjacent vertex with 
                    // probability 1 − pm)
                    int oldColor;
                    if (rnd.NextDouble() < options.MovingProbability)
                    {
                        // Move the ant to the worst adjacent vertex.
                        oldColor = graph.MoveAntToVertexWithLowestCost(ant);
                    }
                    else
                    {
                        // Move randomly to any adjacent vertex.
                        oldColor = graph.MoveAntToAnyAdjacentVertex(ant);
                    }

                    Vertex vertexWithNewColor;
                    // Replaces the color which belong to ant with a new color.
                    if (rnd.NextDouble() < options.ColoringProbability)
                    {
                        // Change vertex color to the best possible color.
                        vertexWithNewColor = graph.ColorVertexWithBestColor(ant);
                    }
                    else
                    {
                        // Change to a randomly chosen color.
                        vertexWithNewColor = graph.ColorVertexWithRandomColor(ant, options.NumberOfPartitions);
                    }

                    // Keep balance (Change a randomly chosen vertex with low local cost
                    // from the new to the old color).
                    Vertex vertexWhichKeepBalance = graph.KeepBalance(options.NumberVerticesForBalance, oldColor, vertexWithNewColor.Color);

                    // For the chosen vertices and all adjacent vertices update local cost function.
                    graph.UpdateLocalCostFunction(vertexWhichKeepBalance, vertexWithNewColor);

                    // Get best global cost.
                    var globalCost = graph.GetGlobalCostFunction();
                    if (globalCost < bestCost)
                    {
                        bestCost = globalCost;
                        Log.Debug($"Best cost: {bestCost}");
                        bestDistribution = graph.Vertices;
                    }
                    Log.Info($"Iteration [{iteration}] | Ant {ant} | Global cost: {globalCost} | Best cost: {bestCost}");
                }
                iteration++;
            }
            stopwatch.Stop();
            Log.Info($"The algorithm running time: {stopwatch.ElapsedMilliseconds}");

            Log.Info($"Best cost at the end: {bestCost}");
            if (Log.IsDebugEnabled)
            {
                foreach (var partition in Enumerable.Range(1, options.NumberOfPartitions))
                {
                    var numberOfVerticesWithinPartition = bestDistribution.Count(vertex => vertex.Color == partition);
                    Log.Debug($"Partition [{partition}]: {numberOfVerticesWithinPartition}");
                }
            }
            LoggerHelper.LogVertices(bestDistribution);
            LoggerHelper.LogVerticesOneLine(bestDistribution);
            graphExport.ExportGraph(bestDistribution);
            LoggerHelper.LogChangesOnVertices(graph.changes);

            return bestCost;
        }
    }
}
