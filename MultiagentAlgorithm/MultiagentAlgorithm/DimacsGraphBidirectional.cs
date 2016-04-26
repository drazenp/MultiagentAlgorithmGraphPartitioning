using System;
using System.Linq;

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
                    var numberOfVertices = int.Parse(fileData[2]);
                    Vertices = new Vertex[numberOfVertices];
                    for (var i = 0; i < numberOfVertices; i++)
                    {
                        Vertices[i] = new Vertex(i, VertexWeight);
                    }

                    NumberOfEdges = int.Parse(fileData[3]);
                }
                else if (fileData[0] == "e")
                {
                    var vertexID = int.Parse(fileData[1]) - 1;
                    var connectedVertexID = int.Parse(fileData[2]) - 1;
                    
                    Vertices[vertexID].ConnectedEdges.Add(connectedVertexID, EdgeWeight);
                }
            }
            
            MaxNumberOfAdjacentVertices = Vertices.Max(verex => verex.ConnectedEdges.Count);
        }
    }
}
