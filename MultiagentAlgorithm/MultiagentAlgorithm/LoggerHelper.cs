using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;

namespace MultiagentAlgorithm
{
    public static class LoggerHelper
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogVertices(IEnumerable<Vertex> vertices)
        {
            var output = new StringBuilder(64);
            foreach (var vertex in vertices)
            {
                output.AppendFormat($"{Environment.NewLine}Vertex [{vertex.ID}] has Color [{vertex.Color}].");
            }

            Log.Info(output.ToString());
        }

        public static void LogVerticesOneLine(IEnumerable<Vertex> vertices)
        {
            Log.Info(string.Join(" ", vertices.Select(v => v.Color)));
        }

        public static void LogVertexWithState(IEnumerable<Vertex> vertices)
        {
            var output = new StringBuilder(256);
            foreach (var vertex in vertices)
            {
                output.AppendFormat($"{Environment.NewLine}Vertex [{vertex.ID}] has Color [{vertex.Color} and LocalCost [{vertex.LocalCost}]].");
            }

            Log.Warn(output.ToString());
        }

        public static void LogChangesOnVertices(Dictionary<int, List<string>> changes)
        {
            var output = new StringBuilder(256);
            foreach (var vertex in changes.OrderBy(c => c.Key))
            {
                output.AppendFormat($"{Environment.NewLine}Vertex [{vertex.Key}] had Colors [{string.Join(" ", vertex.Value)}].");
            }

            Log.Warn(output.ToString());
        }
    }
}
