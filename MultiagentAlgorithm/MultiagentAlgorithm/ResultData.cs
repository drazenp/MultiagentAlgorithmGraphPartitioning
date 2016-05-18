using System;

namespace MultiagentAlgorithm
{
    public class ResultData
    {
        public int BestCost { get; }

        public int BestCostIteration { get; }

        public long ElapsedMilliseconds { get; }

        public ResultData(int bestCost, int bestCostIteration, long elapsedMilliseconds)
        {
            BestCost = bestCost;
            BestCostIteration = bestCostIteration;
            ElapsedMilliseconds = elapsedMilliseconds;
        }

        public override string ToString()
        {
            return $@"Best cost: {BestCost} {Environment.NewLine}
                      Best cost iteration: {BestCostIteration} {Environment.NewLine}
                      Elapsed milliseconds: {ElapsedMilliseconds}";
        }
    }
}
