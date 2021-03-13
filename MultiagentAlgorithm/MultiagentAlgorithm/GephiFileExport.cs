﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiagentAlgorithm
{
   public class GephiFileExport : IExportGraph
   {
      private readonly IDataWriter _dataWriter;
      private readonly string _fileName;

      public GephiFileExport(IDataWriter dataWriter, string fileName)
      {
         _dataWriter = dataWriter;
         _fileName = fileName;
      }

      public void ExportGraph(IList<Vertex> vertices)
      {
         var sb = new StringBuilder(128);
         sb.Append("Id;Degree;Label");
         sb.AppendLine();

         var nodes = string.Join(Environment.NewLine, vertices.Select(vertex => (vertex.ID + 1) + ";" + vertex.Color + ";" + (vertex.ID + 1)));
         sb.Append(nodes);
         _dataWriter.WriteData(sb.ToString(), $"{_fileName}.vrtx.csv");

         sb = new StringBuilder(128);
         sb.Append("Source;Target");
         sb.AppendLine();

         var edgesSet = (from vertex in vertices
                         from edge in vertex.ConnectedEdges
                         select Tuple.Create(vertex.ID + 1, edge.Key + 1)).ToList();

         var uniqueEdges = new List<Tuple<int, int>>();
         foreach (var edge in edgesSet)
         {
            if (
                !uniqueEdges.Exists(
                    t =>
                        (t.Item1 == edge.Item1 && t.Item2 == edge.Item2) ||
                        (t.Item1 == edge.Item2 && t.Item2 == edge.Item1)))
            {
               uniqueEdges.Add(Tuple.Create(edge.Item1, edge.Item2));
            }
         }

         var edges = from vertex in uniqueEdges
                     select vertex.Item1 + ";" + vertex.Item2;

         var links = string.Join(Environment.NewLine, edges.ToList());
         sb.Append(links);

         _dataWriter.WriteData(sb.ToString(), $"{_fileName}.edges.csv");
      }
   }
}
