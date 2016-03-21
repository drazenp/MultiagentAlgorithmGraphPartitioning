using System;
using System.Linq;
using System.Reflection;
using log4net;

namespace MultiagentAlgorithm
{
    public static class Algorithm
    {
        private static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static int Run(Options options, Random rnd)
        {
            var loader = new FileLoader(options.GraphFilePath);
            var graph = new Graph(loader, rnd);
            graph.InitializeGraph();
            graph.InitializeAnts(options.NumberOfAnts);
            graph.ColorVerticesRandomly(options.NumberOfPartitions);
            graph.CalculateLocalCostFunction();

            var bestCost = graph.GetGlobalCostFunction();
            Log.DebugFormat($"Initiale global cost function: {bestCost}");

            // While (best cost > 0) do
            for (var i = 0; i < 10; i++)
            {
                // Reset all history data of vertices so it can be tracked
                // in the new interation.
                graph.ResetVerticesState();

                // At a given iteration each ant moves from the current position 
                // to the adjacent vertex with the lowest local cost, 
                // i.e. the vertex with the greatest number of constraints (neighbors of a different color).
                // which increases the local cost.
                foreach (var ant in graph.Ants.Keys.ToArray())
                {
                    // The agent or ant moves to the worst adjacent vertex with a
                    // probability pm (it moves randomly to any other adjacent vertex with 
                    // probability 1 − pm)
                    if (rnd.NextDouble() < options.MovingProbability)
                    {
                        // Move the ant to the worst adjacent vertex.
                        graph.MoveAntToVertexWithLowestCost(ant);
                    }
                    else
                    {
                        // Move randomly to any adjacent vertex.
                        graph.MoveAntToAnyAdjacentVertex(ant);
                    }

                    // Replaces the color which belong to ant with a new color.
                    if (rnd.NextDouble() < options.ColoringProbability)
                    {
                        // Change vertex color to the best possible color.
                        graph.ColorVertexWithBestColor(ant);
                    }
                    else
                    {
                        // Change to a randomly chosen color.
                        graph.ColorVertexWithRandomColor(ant, options.NumberOfPartitions);
                    }

                    // Keep balance (Change a randomly chosen vertex with low local cost
                    // from the new to the old color).
                    graph.KeepBalance(options.NumberVerticesForBalance);

                    // For the chosen vertices and all adjacent vertices update local cost function.
                    graph.UpdateLocalCostFunction();

                    // Get best global cost.
                    var globalCostFunction = graph.GetGlobalCostFunction();
                    Log.DebugFormat($"New global cost function: {globalCostFunction}");
                    if (globalCostFunction < bestCost)
                    {
                        bestCost = globalCostFunction;
                    }
                }
            }

            return bestCost;
        }
    }
}
