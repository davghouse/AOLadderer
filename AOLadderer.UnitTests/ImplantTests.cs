using AOLadderer.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AOLadderer.UnitTests
{
    [TestClass]
    public class ImplantTests
    {
        [TestMethod]
        public void GetRequiredAbilityValues()
        {
            Assert.AreEqual(6, Implant.GetRequiredAbilityValue(1));
            Assert.AreEqual(30, Implant.GetRequiredAbilityValue(13));
            Assert.AreEqual(112, Implant.GetRequiredAbilityValue(54));
            Assert.AreEqual(114, Implant.GetRequiredAbilityValue(55));
            Assert.AreEqual(116, Implant.GetRequiredAbilityValue(56));
            Assert.AreEqual(202, Implant.GetRequiredAbilityValue(99));
            Assert.AreEqual(404, Implant.GetRequiredAbilityValue(200));
        }

        [TestMethod]
        public void GetRequiredTreatmentValues()
        {
            Assert.AreEqual(11, Implant.GetRequiredTreatmentValue(1));
            Assert.AreEqual(35, Implant.GetRequiredTreatmentValue(6));
            Assert.AreEqual(162, Implant.GetRequiredTreatmentValue(33));
            Assert.AreEqual(460, Implant.GetRequiredTreatmentValue(96));
            Assert.AreEqual(549, Implant.GetRequiredTreatmentValue(115));
            Assert.AreEqual(554, Implant.GetRequiredTreatmentValue(116));
            Assert.AreEqual(559, Implant.GetRequiredTreatmentValue(117));
            Assert.AreEqual(564, Implant.GetRequiredTreatmentValue(118));
            Assert.AreEqual(568, Implant.GetRequiredTreatmentValue(119));
            Assert.AreEqual(573, Implant.GetRequiredTreatmentValue(120));
            Assert.AreEqual(946, Implant.GetRequiredTreatmentValue(199));
            Assert.AreEqual(951, Implant.GetRequiredTreatmentValue(200));
        }

        [TestMethod]
        public void GetMaxImplantQLForAbilityValue()
        {
            Assert.AreEqual(0, Implant.GetMaxImplantQLForAbilityValue(5));
            Assert.AreEqual(1, Implant.GetMaxImplantQLForAbilityValue(7));
            Assert.AreEqual(4, Implant.GetMaxImplantQLForAbilityValue(13));
            Assert.AreEqual(7, Implant.GetMaxImplantQLForAbilityValue(19));
            Assert.AreEqual(33, Implant.GetMaxImplantQLForAbilityValue(70));
            Assert.AreEqual(33, Implant.GetMaxImplantQLForAbilityValue(71));
            Assert.AreEqual(34, Implant.GetMaxImplantQLForAbilityValue(72));
            Assert.AreEqual(34, Implant.GetMaxImplantQLForAbilityValue(73));
            Assert.AreEqual(35, Implant.GetMaxImplantQLForAbilityValue(74));
            Assert.AreEqual(35, Implant.GetMaxImplantQLForAbilityValue(75));
            Assert.AreEqual(36, Implant.GetMaxImplantQLForAbilityValue(76));
            Assert.AreEqual(36, Implant.GetMaxImplantQLForAbilityValue(77));
            Assert.AreEqual(178, Implant.GetMaxImplantQLForAbilityValue(361));
            Assert.AreEqual(197, Implant.GetMaxImplantQLForAbilityValue(399));
            Assert.AreEqual(199, Implant.GetMaxImplantQLForAbilityValue(403));
            Assert.AreEqual(200, Implant.GetMaxImplantQLForAbilityValue(404));
            Assert.AreEqual(200, Implant.GetMaxImplantQLForAbilityValue(450));
        }

        [TestMethod]
        public void GetMaxImplantQLForTreatmentValue()
        {
            Assert.AreEqual(0, Implant.GetMaxImplantQLForTreatmentValue(10));
            Assert.AreEqual(1, Implant.GetMaxImplantQLForTreatmentValue(11));
            Assert.AreEqual(1, Implant.GetMaxImplantQLForTreatmentValue(13));
            Assert.AreEqual(1, Implant.GetMaxImplantQLForTreatmentValue(15));
            Assert.AreEqual(2, Implant.GetMaxImplantQLForTreatmentValue(16));
            Assert.AreEqual(19, Implant.GetMaxImplantQLForTreatmentValue(100.9));
            Assert.AreEqual(20, Implant.GetMaxImplantQLForTreatmentValue(101));
            Assert.AreEqual(21, Implant.GetMaxImplantQLForTreatmentValue(109));
            Assert.AreEqual(22, Implant.GetMaxImplantQLForTreatmentValue(110.9));
            Assert.AreEqual(22, Implant.GetMaxImplantQLForTreatmentValue(114.5));
            Assert.AreEqual(23, Implant.GetMaxImplantQLForTreatmentValue(115.5));
            Assert.AreEqual(158, Implant.GetMaxImplantQLForTreatmentValue(756.4));
            Assert.AreEqual(159, Implant.GetMaxImplantQLForTreatmentValue(757.9));
            Assert.AreEqual(159, Implant.GetMaxImplantQLForTreatmentValue(761));
            Assert.AreEqual(160, Implant.GetMaxImplantQLForTreatmentValue(762));
            Assert.AreEqual(188, Implant.GetMaxImplantQLForTreatmentValue(898.9));
            Assert.AreEqual(189, Implant.GetMaxImplantQLForTreatmentValue(899));
            Assert.AreEqual(189, Implant.GetMaxImplantQLForTreatmentValue(903.9));
            Assert.AreEqual(190, Implant.GetMaxImplantQLForTreatmentValue(904.9));
            Assert.AreEqual(191, Implant.GetMaxImplantQLForTreatmentValue(912.9));
            Assert.AreEqual(192, Implant.GetMaxImplantQLForTreatmentValue(915));
            Assert.AreEqual(192, Implant.GetMaxImplantQLForTreatmentValue(917.9));
            Assert.AreEqual(193, Implant.GetMaxImplantQLForTreatmentValue(918));
            Assert.AreEqual(199, Implant.GetMaxImplantQLForTreatmentValue(950));
            Assert.AreEqual(200, Implant.GetMaxImplantQLForTreatmentValue(951));
            Assert.AreEqual(200, Implant.GetMaxImplantQLForTreatmentValue(1001));
        }

        [TestMethod]
        public void VerifiesAbilityAndTreatmentRequirements()
        {
            var leftHand = Implant.GetImplant(ImplantSlot.LeftHand, Skill.FastAttack, ArmorClass.FireAC, Skill.MartialArts, 155);
            Assert.AreEqual(Ability.Agility, leftHand.RequiredAbility);
            Assert.AreEqual(314, leftHand.RequiredAbilityValue);
            Assert.AreEqual(738, leftHand.RequiredTreatmentValue);

            var feet = Implant.GetImplant(ImplantSlot.Feet, Skill.EvadeClsC, Skill.MartialArts, Skill.DuckExp, 144);
            Assert.AreEqual(Ability.Agility, feet.RequiredAbility);
            Assert.AreEqual(292, feet.RequiredAbilityValue);
            Assert.AreEqual(686, feet.RequiredTreatmentValue);

            var eye = Implant.GetImplant(ImplantSlot.Eye, Skill.AimedShot, Skill.SensoryImpr, Skill.SharpObj, 132);
            Assert.AreEqual(Ability.Agility, eye.RequiredAbility);
            Assert.AreEqual(268, eye.RequiredAbilityValue);
            Assert.AreEqual(630, eye.RequiredTreatmentValue);
        }

        [TestMethod]
        public void GetsTotalTreatmentIncrease()
        {
            var leftHand = Implant.GetImplant(ImplantSlot.LeftHand, Skill.FastAttack, ArmorClass.FireAC, Skill.MartialArts, 155);
            Assert.AreEqual(0, leftHand.TotalTreatmentIncrease);

            var head1 = Implant.GetImplant(ImplantSlot.Head, Skill.Treatment, null, Ability.Sense, 200);
            Assert.AreEqual(106.1, head1.TotalTreatmentIncrease);

            var head2 = Implant.GetImplant(ImplantSlot.Head, Ability.Intelligence, null, Ability.Sense, 200);
            Assert.AreEqual(7.975, head2.TotalTreatmentIncrease);
        }
    }
}
