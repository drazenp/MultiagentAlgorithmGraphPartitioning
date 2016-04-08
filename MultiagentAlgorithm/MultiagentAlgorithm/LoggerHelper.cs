using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using log4net;

namespace MultiagentAlgorithm
{
    public static class LoggerHelper
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogVertices(IList<Vertex> vertices)
        {
            var output = new StringBuilder(64);
            foreach (var vertex in vertices)
            {
                output.AppendFormat($"{Environment.NewLine}Vertex [{vertex.ID}] has Color [{vertex.Color}].");
            }

            Log.Info(output.ToString());
        }
    }
}
