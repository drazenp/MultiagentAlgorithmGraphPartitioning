using System.Reflection;
using log4net;
using log4net.Config;

namespace MultiagentAlgorithmConsole
{
    static class Program
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

        }
    }
}
