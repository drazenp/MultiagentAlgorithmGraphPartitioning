using System;

namespace MultiAgentAlgorithmAnalyze
{
   partial class AnalyzeResult
   {
      private static readonly DateTime DateTime1970 = new DateTime(1970, 1, 1);

      public int GetIntegerFromDate(DateTime date)
      {
         // Integer should be enough.
         return (int)(date - DateTime1970).TotalSeconds;
      }

      public DateTime GetDateTimeFromInteger(int seconds)
      {
         return DateTime1970.AddSeconds(seconds);
      }

      public override string ToString()
      {
         return $@"AnalyzeID {AnalyzeID}
                   BestCost {BestCost}
                   StartDate {StartDate}
                   EndDate {EndDate}
                   Duration {Duration}
                   BestCostIteration {BestCostIteration}";
      }
   }
}
