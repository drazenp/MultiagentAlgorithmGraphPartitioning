using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MultiagentAlgorithm
{
    public class DimacsGraph : BaseGraph
    {
        private const int VertexWeight = 1;
        private const int EdgeWeight = 1;

        private enum FileDataType : byte
        {
            [Description("c")]
            Comment,
            [Description("p")]
            Definition,
            [Description("e")]
            Edges
        }

        public DimacsGraph(IDataLoader dataLoader, Random rnd)
        {
            DataLoader = dataLoader;
            Rnd = rnd;
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
                    var vertexID = int.Parse(fileData[1]);
                    var connectedVertexID = int.Parse(fileData[2]);

                    var vertex = Vertices.FirstOrDefault(v => v.ID == vertexID);
                    if (vertex == null)
                    {
                        vertex = new Vertex(vertexID, VertexWeight);
                        Vertices.Add(vertex);
                    }
                    vertex.ConnectedEdges.Add(connectedVertexID, EdgeWeight);

                    var connectedVertex = Vertices.FirstOrDefault(v => v.ID == connectedVertexID);
                    if (connectedVertex == null)
                    {
                        connectedVertex = new Vertex(connectedVertexID, VertexWeight);
                        Vertices.Add(connectedVertex);
                    }
                    connectedVertex.ConnectedEdges.Add(vertexID, EdgeWeight);
                }
            }
        }
    }
}
