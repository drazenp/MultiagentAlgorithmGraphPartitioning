using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MultiagentAlgorithm
{
    public class DimacsGraph : BaseGraph
    {
        protected const int VertexWeight = 1;
        protected const int EdgeWeight = 1;

        protected enum FileDataType : byte
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

                    var connectedVertex = Vertices.FirstOrDefault(v => v.ID == connectedVertexID);
                    if (connectedVertex == null)
                    {
                        connectedVertex = new Vertex(connectedVertexID, VertexWeight);
                        Vertices.Add(connectedVertex);
                    }
                    connectedVertex.ConnectedEdges.Add(vertexID, EdgeWeight);
                }
            }

            Vertices = Vertices.OrderBy(v => v.ID).ToList();

            MaxNumberOfAdjacentVertices = Vertices.Max(verex => verex.ConnectedEdges.Count);
        }
    }
}
