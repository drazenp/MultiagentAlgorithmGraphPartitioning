using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MultiagentAlgorithm
{
    public class DimacsGraph : BaseGraph
    {
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
                var fileData = line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                if (fileData[0] == "c")
                {
                    continue;
                } else if (fileData[0] == "p")
                {
                    // TODO: Check if number in description represent corrent number of vertices and edges.
                    var numberOfVertices = int.Parse(fileData[2]);
                    Vertices = new List<Vertex>(numberOfVertices);
                    NumberOfEdges = int.Parse(fileData[3]);
                } else if (fileData[0] == "e")
                {
                    
                }
            }
        }
    }
}
