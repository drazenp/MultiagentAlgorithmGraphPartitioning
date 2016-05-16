using System;
using System.ComponentModel;
using System.Linq;

namespace MultiagentAlgorithm
{
    public class DimacsGraph : BaseGraph
    {
        private const int VertexWeight = 1;
        private const int EdgeWeight = 1;

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

                switch (fileData[0])
                {
                    case "p":
                        var numberOfVertices = int.Parse(fileData[2]);
                        Vertices = new Vertex[numberOfVertices];
                        for (var i = 0; i < numberOfVertices; i++)
                        {
                            Vertices[i] = new Vertex(i, VertexWeight);
                        }

                        NumberOfEdges = int.Parse(fileData[3]);
                        break;
                    case "e":
                        var vertexID = int.Parse(fileData[1]) - 1;
                        var connectedVertexID = int.Parse(fileData[2]) - 1;

                        if (!Vertices[vertexID].ConnectedEdges.ContainsKey(connectedVertexID))
                        {
                            Vertices[vertexID].ConnectedEdges.Add(connectedVertexID, EdgeWeight);
                        }
                        if (!Vertices[connectedVertexID].ConnectedEdges.ContainsKey(vertexID))
                        {
                            Vertices[connectedVertexID].ConnectedEdges.Add(vertexID, EdgeWeight);
                        }
                        break;
                }
            }

            MaxNumberOfAdjacentVertices = Vertices.Max(verex => verex.ConnectedEdges.Count);
        }
    }
}
