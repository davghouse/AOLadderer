using AOLadderer.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AOLadderer.UnitTests
{
    [TestClass]
    public class ImplantTemplateTests
    {
        [TestMethod]
        public void ImplantTemplateCount()
        {
            int expectedCount = 0;
            foreach (var implantSlot in ImplantSlot.ImplantSlots)
            {
                expectedCount += (implantSlot.ShinyClusterTemplates.Count + 1)
                    * (implantSlot.BrightClusterTemplates.Count + 1)
                    * (implantSlot.FadedClusterTemplates.Count + 1) - 1;
            }

            Assert.AreEqual(expectedCount, ImplantTemplate.ImplantTemplates.Count);
        }

        [TestMethod]
        public void VerifiesAOIDsAndRequiredAbilities()
        {
            var waist = ImplantTemplate.GetImplantTemplate(ImplantSlot.Waist, ArmorClass.ColdAC, Skill.BodyDev, Skill.EvadeClsC);
            Assert.AreEqual(111104, waist.AOID);
            Assert.AreEqual(Ability.Stamina, waist.RequiredAbility);

            var leftWrist = ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftWrist, Skill.MultMelee, Skill.Parry, Skill.NanoResist);
            Assert.AreEqual(111199, leftWrist.AOID);
            Assert.AreEqual(Ability.Agility, leftWrist.RequiredAbility);

            var rightHand = ImplantTemplate.GetImplantTemplate(ImplantSlot.RightHand, Skill.MartialArts, Skill.SharpObj, Skill.OneHandBlunt);
            Assert.AreEqual(111688, rightHand.AOID);
            Assert.AreEqual(Ability.Agility, rightHand.RequiredAbility);
        }
    }
}
