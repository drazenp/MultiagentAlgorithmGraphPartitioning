using System;

namespace MultiagentAlgorithm
{
    public static class Algorithm
    {
        public static void Run(Options options, Random rnd)
        {
            var loader = new FileLoader(options.GraphFilePath);
            var graph = new Graph(loader, rnd);
            graph.InitializeGraph();
            graph.InitializeAnts(options.NumberOfAnts);
            graph.ColorVerticesRandomly(options.NumberOfPartitions);
            graph.CalculateLocalCostFunction();

            var bestCost = graph.GetGlobalCostFunction();

            // While (best cost > 0) do
            for (var i = 0; i < 10; i++)
            {
                // At a given iteration each ant moves from the current position 
                // to the adjacent vertex with the lowest local cost, 
                // i.e.the vertex with the greatest number of constraints (neighbors of a
                // different color) and replaces its color with a new color 
                // which increases the local cost.
                foreach (var ant in graph.Ants.Keys)
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
                }
            }
        }
    }
}
