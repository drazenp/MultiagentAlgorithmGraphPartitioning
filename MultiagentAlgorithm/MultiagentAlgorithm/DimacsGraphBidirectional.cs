using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiagentAlgorithm
{
    public class DimacsGraphBidirectional : DimacsGraph
    {
        public DimacsGraphBidirectional(IDataLoader dataLoader, Random rnd) 
            : base(dataLoader, rnd)
        {
        }

        public override void InitializeGraph()
        {
            foreach (var line in DataLoader.LoadData())
            {
                var fileData = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (fileData[0] == "c")
                {
                    continue;
                }
                if (fileData[0] == "p")
                {
                    // TODO: Check if number in description represent corrent number of vertices and edges.
                    var numberOfVertices = int.Parse(fileData[2]);
                    Vertices = new List<Vertex>(numberOfVertices);
                    NumberOfEdges = int.Parse(fileData[3]);
                }
                else if (fileData[0] == "e")
                {
                    var vertexID = int.Parse(fileData[1]) - 1;
                    var connectedVertexID = int.Parse(fileData[2]) - 1;

                    var vertex = Vertices.FirstOrDefault(v => v.ID == vertexID);
                    if (vertex == null)
                    {
                        vertex = new Vertex(vertexID, VertexWeight);
                        Vertices.Add(vertex);
                    }
                    vertex.ConnectedEdges.Add(connectedVertexID, EdgeWeight);
                }
            }
        }
    }
}
