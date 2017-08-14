using AOLadderer.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AOLadderer.UnitTests.Stats
{
    [TestClass]
    public class AbilityTests
    {
        [TestMethod]
        public void GetShinyAbilityIncreases()
        {
            Assert.AreEqual(5, Ability.Agility.GetShinyStatIncrease(1));
            Assert.AreEqual(30, Ability.Agility.GetShinyStatIncrease(99));
            Assert.AreEqual(30, Ability.Agility.GetShinyStatIncrease(101));
            Assert.AreEqual(30, Ability.Agility.GetShinyStatIncrease(102));
            Assert.AreEqual(31, Ability.Agility.GetShinyStatIncrease(103));
            Assert.AreEqual(55, Ability.Agility.GetShinyStatIncrease(200));
        }

        [TestMethod]
        public void GetBrightAbilityIncreases()
        {
            Assert.AreEqual(3, Ability.Agility.GetBrightStatIncrease(1));
            Assert.AreEqual(16, Ability.Agility.GetBrightStatIncrease(89));
            Assert.AreEqual(16, Ability.Agility.GetBrightStatIncrease(90));
            Assert.AreEqual(17, Ability.Agility.GetBrightStatIncrease(91));
            Assert.AreEqual(17, Ability.Agility.GetBrightStatIncrease(92));
            Assert.AreEqual(33, Ability.Agility.GetBrightStatIncrease(200));
        }

        [TestMethod]
        public void GetFadedAbilityIncreases()
        {
            Assert.AreEqual(2, Ability.Agility.GetFadedStatIncrease(1));
            Assert.AreEqual(20, Ability.Agility.GetFadedStatIncrease(179));
            Assert.AreEqual(20, Ability.Agility.GetFadedStatIncrease(185));
            Assert.AreEqual(21, Ability.Agility.GetFadedStatIncrease(186));
            Assert.AreEqual(22, Ability.Agility.GetFadedStatIncrease(200));
        }

        [TestMethod]
        public void AbilityCount()
            => Assert.AreEqual(6, Ability.Abilities.Count);
    }
}
