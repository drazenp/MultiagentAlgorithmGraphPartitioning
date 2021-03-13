using System;
using System.Diagnostics;
using System.Linq;

namespace MultiagentAlgorithm
{
   public static class Algorithm
   {
      public static ResultData Run(BaseGraph graph, Options options, Random rnd, IExportGraph graphExport)
      {
         var stopwatch = new Stopwatch();
         stopwatch.Start();

         graph.InitializeGraph();
         graph.InitializeAnts(options.NumberOfAnts);
         graph.ColorVerticesRandomly(options.NumberOfPartitions);
         graph.CalculateLocalCostFunction();

         var bestCost = graph.GetGlobalCostFunction();
         var bestCostIteration = 0;
         var bestDistribution = (Vertex[])graph.Vertices.Clone();
         var iteration = 0;

         while (bestCost > 0 && iteration < options.NumberOfIterations)
         {
            // At a given iteration each ant moves from the current position 
            // to the adjacent vertex with the lowest local cost, 
            // i.e. the vertex with the greatest number of constraints (neighbors of a different color).
            // which increases the local cost.
            foreach (var ant in graph.Ants.Keys.ToArray())
            {
               // The agent or ant moves to the worst adjacent vertex with a
               // probability pm (it moves randomly to any other adjacent vertex with probability 1 − pm)
               Vertex vertexWithAnt;
               if (rnd.NextDouble() < options.MovingProbability)
               {
                  // Move the ant to the worst adjacent vertex.
                  vertexWithAnt = graph.MoveAntToVertexWithLowestCost(ant);
               }
               else
               {
                  // Move randomly to any adjacent vertex.
                  vertexWithAnt = graph.MoveAntToAnyAdjacentVertex(ant);
               }

               // Save the data of the vertex on witch ant moved.
               int oldColor = vertexWithAnt.Color;
               int vertexWithAntID = vertexWithAnt.ID;

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
               Vertex vertexWhichKeepBalance = graph.KeepBalance(options.NumberOfVerticesForBalance, vertexWithAntID, oldColor, vertexWithNewColor.Color);

               // For the chosen vertices and all adjacent vertices update local cost function.
               graph.UpdateLocalCostFunction(vertexWhichKeepBalance, vertexWithNewColor);

               // Get best global cost.
               var globalCost = graph.GetGlobalCostFunction();
               if (globalCost < bestCost)
               {
                  bestCost = globalCost;
                  bestCostIteration = iteration;
                  bestDistribution = (Vertex[])graph.Vertices.Clone();
               }
            }
            iteration++;
         }
         stopwatch.Stop();

         graphExport.ExportGraph(bestDistribution);

         var result = new ResultData(bestCost, bestCostIteration, stopwatch.ElapsedMilliseconds);

         return result;
      }
   }
}
