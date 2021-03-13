using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiagentAlgorithm
{
   public class MetisUnweightedGraph : BaseGraph
   {
      private const int VertexWeight = 1;
      private const int EdgeWeight = 1;

      public MetisUnweightedGraph(IDataLoader dataLoader, Random rnd)
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
               Vertices = new Vertex[int.Parse(fileData[0])];

               firstLine = false;
            }
            else
            {
               Vertices[counter] = new Vertex(counter, VertexWeight);

               var connectedEdges = new Dictionary<int, int>();
               foreach (var strEdge in fileData)
               {
                  connectedEdges.Add(int.Parse(strEdge) - 1, EdgeWeight);
               }
               Vertices[counter].ConnectedEdges = connectedEdges;

               counter++;
            }
         }

         MaxNumberOfAdjacentVertices = Vertices.Max(verex => verex.ConnectedEdges.Count);
      }
   }
}
