namespace MultiagentAlgorithm
{
    public struct Options
    {
        /// <summary>
        /// The number of ants.
        /// </summary>
        public int NumberOfAnts { get; }

        /// <summary>
        /// The probability with agent or ant moves to the worst adjacent vertex.
        /// </summary>
        public double MovingProbability { get; }

        /// <summary>
        /// The probability with agent or ant assign the best color.
        /// </summary>
        public double ColoringProbability { get; }

        /// <summary>
        /// The path to the file with graph definition.
        /// </summary>
        public string GraphFilePath { get; }

        public Options(int numberOfAnts, double coloringProbability, double movingProbability, string graphFilePath)
        {
            NumberOfAnts = numberOfAnts;
            ColoringProbability = coloringProbability;
            MovingProbability = movingProbability;
            GraphFilePath = graphFilePath;
        }
    }
}
