using AOLadderer.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AOLadderer.UnitTests.Stats
{
    [TestClass]
    public class SkillTests
    {
        [TestMethod]
        public void GetShinySkillIncreases()
        {
            Assert.AreEqual(6, Skill.Rifle.GetShinyStatIncrease(1));
            Assert.AreEqual(55, Skill.Rifle.GetShinyStatIncrease(99));
            Assert.AreEqual(55, Skill.Rifle.GetShinyStatIncrease(100));
            Assert.AreEqual(56, Skill.Rifle.GetShinyStatIncrease(101));
            Assert.AreEqual(105, Skill.Rifle.GetShinyStatIncrease(200));
        }

        [TestMethod]
        public void GetBrightSkillIncreases()
        {
            Assert.AreEqual(3, Skill.Rifle.GetBrightStatIncrease(1));
            Assert.AreEqual(30, Skill.Rifle.GetBrightStatIncrease(89));
            Assert.AreEqual(30, Skill.Rifle.GetBrightStatIncrease(90));
            Assert.AreEqual(30, Skill.Rifle.GetBrightStatIncrease(91));
            Assert.AreEqual(30, Skill.Rifle.GetBrightStatIncrease(92));
            Assert.AreEqual(31, Skill.Rifle.GetBrightStatIncrease(93));
            Assert.AreEqual(63, Skill.Rifle.GetBrightStatIncrease(200));
        }

        [TestMethod]
        public void GetFadedSkillIncreases()
        {
            Assert.AreEqual(2, Skill.Rifle.GetFadedStatIncrease(1));
            Assert.AreEqual(38, Skill.Rifle.GetFadedStatIncrease(179));
            Assert.AreEqual(38, Skill.Rifle.GetFadedStatIncrease(180));
            Assert.AreEqual(38, Skill.Rifle.GetFadedStatIncrease(181));
            Assert.AreEqual(38, Skill.Rifle.GetFadedStatIncrease(182));
            Assert.AreEqual(39, Skill.Rifle.GetFadedStatIncrease(183));
            Assert.AreEqual(42, Skill.Rifle.GetFadedStatIncrease(200));
        }

        [TestMethod]
        public void SkillCount()
            => Assert.AreEqual(67, Skill.Skills.Count);
    }
}
