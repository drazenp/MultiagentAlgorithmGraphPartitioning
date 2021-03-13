using CommandLine;

namespace MultiagentAlgorithmConsole
{
   [Verb("part", HelpText = "Partition graph based on imput paramenters.")]
   public class PartitionGraphOptions
   {
      [Option('t', HelpText = "Type of the graph in input file - graph to be parted.", Required = true)]
      public GraphInputFileType ImportGraphFileType { get; set; }

      [Option('i', HelpText = "Input file path with graph to be processed.", Required = true)]
      public string InputGraphFilePath { get; set; }

      [Option('a', HelpText = "Represent the number of ants used for partitioning the graph.", Required = true)]
      public int NumberOfAnts { get; set; }

      [Option('p', HelpText = "The number of partition.", Required = true)]
      public int NumberOfPartitions { get; set; }

      [Option('c', HelpText = "The ant assigns the best color under probability pc.", Required = true)]
      public double ColoringProbability { get; set; }

      [Option('m', HelpText = "The ant moves to the worst adjacent vertex with a probability pm.", Required = true)]
      public double MovingProbability { get; set; }

      [Option('s', HelpText = "The number of random vertices to keep balance.", Required = true)]
      public int NumberOfVerticesForBalance { get; set; }

      [Option('d', HelpText = "The number of iteration.", Required = true)]
      public int NumberOfIterations { get; set; }

      // Omitting long name, default --verbose
      [Option(HelpText = "Prints all messages to standard output.")]
      public bool Verbose { get; set; }
   }
}
