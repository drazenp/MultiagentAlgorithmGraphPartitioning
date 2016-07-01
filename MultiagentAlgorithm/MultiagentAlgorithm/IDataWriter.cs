namespace MultiagentAlgorithm
{
    public interface IDataWriter
    {
        void WriteData(string data);
        void WriteData(string data, string fileName);
    }
}
