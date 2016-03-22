using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiagentAlgorithm
{
    public class MetisGraph : BaseGraph
    {
        public MetisGraph(IDataLoader dataLoader, Random rnd)
        {
            DataLoader = dataLoader;
            Rnd = rnd;
        }

        public override void InitializeGraph()
        {
            // We need first line of the file just for initialization of the graph.
            bool firstLine = true;
            int counter = 0;

            foreach (var line in DataLoader.LoadData())
            {
                var fileData = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (firstLine)
                {
                    NumberOfEdges = int.Parse(fileData[1]);
                    Vertices = new List<Vertex>(int.Parse(fileData[0]));

                    firstLine = false;
                }
                else
                {
                    var vertexWeight = int.Parse(fileData[0]);

                    // Initilize edges. Get list of vertices as the each even element in the file line;
                    // The edges are the each odd element in the file line except first - the first is vertex weight.
                    var fileDataList = fileData.ToList();
                    var edges = fileDataList.Where((x, y) => y % 2 == 0).Skip(1).ToList();
                    var vertices = fileDataList.Where((x, y) => y % 2 != 0).ToList();

                    var connectedEdges = new Dictionary<int, int>();
                    for (var i = 0; i < vertices.Count(); i++)
                    {
                        var edgeVertex = int.Parse(vertices.ElementAt(i)) - 1;
                        var edgeWeight = int.Parse(edges.ElementAt(i));
                        connectedEdges.Add(edgeVertex, edgeWeight);
                    }

                    Vertices.Add(new Vertex(counter, vertexWeight, connectedEdges));

                    counter++;
                }
            }
        }
    }
}
