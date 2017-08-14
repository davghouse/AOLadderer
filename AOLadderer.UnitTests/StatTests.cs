using AOLadderer.ClusterTemplates;
using AOLadderer.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AOLadderer.UnitTests
{
    [TestClass]
    public class StatTests
    {
        [TestMethod]
        public void LadderStatCountsAgree()
        {
            Assert.AreEqual(7, Stat.Stats.Where(s => s.IsLadderStat).Count());
            Assert.AreEqual(7, Stat.LadderStats.Count);
            Assert.AreEqual(7, ShinyClusterTemplate.ShinyClusterTemplates.Where(t => t.RaisesLadderStat).Count());
            Assert.AreEqual(7, BrightClusterTemplate.BrightClusterTemplates.Where(t => t.RaisesLadderStat).Count());
            Assert.AreEqual(7, FadedClusterTemplate.FadedClusterTemplates.Where(t => t.RaisesLadderStat).Count());
        }

        [TestMethod]
        public void AbilityCountsAgree()
        {
            Assert.AreEqual(6, Ability.Abilities.Count);
            Assert.AreEqual(6, ShinyClusterTemplate.ShinyClusterTemplates.Where(t => t.RaisesAbility).Count());
            Assert.AreEqual(6, BrightClusterTemplate.BrightClusterTemplates.Where(t => t.RaisesAbility).Count());
            Assert.AreEqual(6, FadedClusterTemplate.FadedClusterTemplates.Where(t => t.RaisesAbility).Count());
        }
    }
}
