using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MultiagentAlgorithm.Test
{
   [TestClass]
   public static class TestAssemblyInitialize
   {
      [AssemblyInitialize]
      public static void Configure(TestContext tc)
      {
         log4net.Config.XmlConfigurator.Configure();
      }
   }
}
