using AOLadderer.ClusterTemplates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AOLadderer.UnitTests.ClusterTemplates
{
    [TestClass]
    public class ShinyClusterTemplateTests
    {
        [TestMethod]
        public void HasShinyClusterTemplateForEachStat()
            => Assert.AreEqual(Stat.Stats.Count, ShinyClusterTemplate.ShinyClusterTemplates.Count);
    }
}
