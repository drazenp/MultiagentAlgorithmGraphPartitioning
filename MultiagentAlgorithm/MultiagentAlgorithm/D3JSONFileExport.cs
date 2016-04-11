using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiagentAlgorithm
{
    public class D3JSONFileExport : IExportGraph
    {
        private readonly IDataWriter _dataWriter;

        public D3JSONFileExport(IDataWriter dataWriter)
        {
            _dataWriter = dataWriter;
        }

        public void ExportGraph(IList<Vertex> vertices)
        {
            var sb = new StringBuilder(128);
            sb.Append("\"nodes\":[");
            var nodes = string.Join(",", vertices.Select(vertex => "{\"name\":'" + (vertex.ID + 1) + "','group':" + vertex.Color + "}"));
            sb.Append(nodes);

            sb.Append("],'links':[");

            var edges = new List<string>();
            foreach (var vertex in vertices)
            {
                foreach (var edge in vertex.ConnectedEdges)
                {
                    edges.Add("{'source':" + vertex.ID + ",'target':" + edge.Key + ",'value':1}");
                }
            }

            var links = string.Join(",", edges);
            sb.Append(links);

            sb.Append("]");
            _dataWriter.WriteData(sb.ToString());
        }
    }
}
