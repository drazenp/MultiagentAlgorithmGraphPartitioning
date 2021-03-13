using System;

namespace MultiAgentAlgorithmAnalyze
{
   partial class AnalyzeResult
   {
      public int AnalyzeID { get; set; }
      public int BestCost { get; set; }
      public DateTime StartDate { get; set; }
      public DateTime EndDate { get; set; }
      public long Duration { get; set; }
      public int BestCostIteration { get; set; }
   }
}
