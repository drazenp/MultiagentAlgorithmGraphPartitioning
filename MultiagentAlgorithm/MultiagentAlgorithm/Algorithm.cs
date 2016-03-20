using System;

namespace MultiagentAlgorithm
{
    public class Algorithm
    {
        public void Run(IGraph graph, Options options, Random rnd)
        {
            // While (best cost > 0) do
            for (var i = 0; i < 10; i++)
            {
                for (var ant = 0; ant < options.NumberOfAnts; ant++)
                {
                    if (rnd.NextDouble() < options.MovingProbability)
                    {
                        // Move the ant to the worst adjacent vertex.
                    }
                    else
                    {
                        // Move randomly to any adjacent vertex.
                    }
                }
            }
        }
    }
}
