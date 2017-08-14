using AOLadderer.ClusterTemplates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AOLadderer.UnitTests.ClusterTemplates
{
    [TestClass]
    public class FadedClusterTemplateTests
    {
        [TestMethod]
        public void HasFadedClusterTemplateForEachStat()
            => Assert.AreEqual(Stat.Stats.Count, FadedClusterTemplate.FadedClusterTemplates.Count);
    }
}
