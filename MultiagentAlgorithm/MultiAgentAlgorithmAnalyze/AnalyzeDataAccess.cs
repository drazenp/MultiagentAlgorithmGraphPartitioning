using System.Collections.Generic;
using System.Data.SQLite;

namespace MultiAgentAlgorithmAnalyze
{
    class AnalyzeDataAccess
    {
        public IEnumerable<AnalyzeData> GetAnalyzeData()
        {
            using (SQLiteConnection conn = new SQLiteConnection("data source"))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();


                    yield return new AnalyzeData();
                }
            }
        }
    }
}
