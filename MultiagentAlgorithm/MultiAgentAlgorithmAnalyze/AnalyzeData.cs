namespace MultiAgentAlgorithmAnalyze
{
    class AnalyzeData
    {
        public int ID { get; set; }

        public string GraphFilePath { get; set; }

        public int NumberOfAnts { get; set; }

        public int NumberOfPartitions { get; set; }

        public double ColoringProbability { get; set; }

        public double MovingProbability { get; set; }

        public int NumberOfVerticesForBalance { get; set; }

        public int NumberOfIterations { get; set; }

        public GraphFileType GraphFileType { get; set; }
    }
}
