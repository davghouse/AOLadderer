using AOLadderer.ClusterTemplates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AOLadderer.UnitTests.ClusterTemplates
{
    [TestClass]
    public class BrightClusterTemplateTests
    {
        [TestMethod]
        public void HasBrightClusterTemplateForEachStat()
            => Assert.AreEqual(Stat.Stats.Count, BrightClusterTemplate.BrightClusterTemplates.Count);
    }
}
