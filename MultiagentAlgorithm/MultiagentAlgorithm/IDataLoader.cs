using System.Collections.Generic;

namespace MultiagentAlgorithm
{
    public interface IDataLoader
    {
        IEnumerable<string> LoadData();
    }
}
