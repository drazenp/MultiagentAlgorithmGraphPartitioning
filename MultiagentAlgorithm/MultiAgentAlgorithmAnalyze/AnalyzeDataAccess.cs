using System.Configuration;
using System.Data;
using System.Data.SQLite;
using MultiagentAlgorithm;

namespace MultiAgentAlgorithmAnalyze
{
    class AnalyzeDataAccess
    {
        private readonly static string ConnectionString = ConfigurationManager.ConnectionStrings["AnalyzeConnectionString"].ConnectionString;

        public static AnalyzeData GetAnalyzeData()
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                const string sqlAnalyzeData = @"select ad.ID, ad.GraphFilePath, ad.InputFileType, ad.NumberOfAnts, ad.NumberOfPartitions, ad.ColoringProbability,
                                                ad.MovingProbability, ad.NumberOfVerticesForBalance, ad.NumberOfIterations
                                                from AnalyzeData ad
                                                left join AnalyzeResults ar on ar.AnalyzeID=ad.ID
                                                where ar.AnalyzeID is null";

                using (SQLiteCommand cmd = new SQLiteCommand(sqlAnalyzeData, conn))
                {
                    conn.Open();
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();

                        var analyzeData = new AnalyzeData();
                        analyzeData.ID = reader.GetInt32(0);
                        analyzeData.GraphFilePath = reader.GetString(1);
                        analyzeData.GraphFileType = (GraphFileType)reader.GetInt32(2);
                        analyzeData.NumberOfAnts = reader.GetInt32(3);
                        analyzeData.NumberOfPartitions = reader.GetInt32(4);
                        analyzeData.ColoringProbability = reader.GetDouble(5);
                        analyzeData.MovingProbability = reader.GetDouble(6);
                        analyzeData.NumberOfVerticesForBalance = reader.GetInt32(7);
                        analyzeData.NumberOfIterations = reader.GetInt32(8);

                        return analyzeData;
                    }
                }
            }
        }

        public static void SaveAnalyzeResult(int analyzeID, ResultData resultData)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                const string sqlSaveAnalyzeResult = @"";

                using (SQLiteCommand cmd = new SQLiteCommand(sqlSaveAnalyzeResult, conn))
                {
                    cmd.Parameters.Add("AnalyzeID", DbType.Int32).Value = analyzeID;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
