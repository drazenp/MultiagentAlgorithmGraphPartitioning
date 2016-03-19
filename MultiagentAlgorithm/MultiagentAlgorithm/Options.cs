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

        public Options(int numberOfAnts, double coloringProbability, double movingProbability)
        {
            NumberOfAnts = numberOfAnts;
            ColoringProbability = coloringProbability;
            MovingProbability = movingProbability;
        }
    }
}
