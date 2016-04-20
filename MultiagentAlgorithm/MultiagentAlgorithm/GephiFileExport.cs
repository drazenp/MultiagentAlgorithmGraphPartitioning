using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiagentAlgorithm
{
    public class GephiFileExport : IExportGraph
    {
        private readonly IDataWriter _dataWriter;

        public GephiFileExport(IDataWriter dataWriter)
        {
            _dataWriter = dataWriter;
        }

        public void ExportGraph(IList<Vertex> vertices)
        {
            var sb = new StringBuilder(128);
            sb.Append("Id;Degree");
            sb.AppendLine();

            var nodes = string.Join("", vertices.Select(vertex => (vertex.ID + 1) + ";" + vertex.Color + Environment.NewLine));
            sb.Append(nodes);
            _dataWriter.WriteData(sb.ToString());

            sb = new StringBuilder(128);
            sb.Append("Source;Target");
            sb.AppendLine();

            var edges = from vertex in vertices
                        from edge in vertex.ConnectedEdges
                        select (vertex.ID + 1) + ";" + edge.Key + Environment.NewLine;

            var links = string.Join("", edges.ToList());
            sb.Append(links);

            _dataWriter.WriteData(sb.ToString());
        }
    }
}
