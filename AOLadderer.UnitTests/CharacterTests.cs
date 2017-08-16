using AOLadderer.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AOLadderer.UnitTests
{
    [TestClass]
    public class CharacterTests
    {
        private Character _character;

        [TestInitialize]
        public void Initialize()
        {
            _character = new Character(
                agilityValue: 91, intelligenceValue: 103, psychicValue: 98,
                senseValue: 94, staminaValue: 91, strengthValue: 91, treatmentValue: 281.4);
        }

        [TestMethod]
        public void AllImplantSlotsStartEmpty()
            => Assert.IsTrue(ImplantSlot.ImplantSlots.All(_character.IsImplantSlotEmpty));

        [TestMethod]
        public void CannotDirectlyEquipQL50StoreBoughtEyeImplant()
        {
            var eye = Implant.GetImplant(ImplantSlot.Eye, Skill.Tutoring, Skill.PsychoModi, Skill.TimeAndSpace, 50);
            Assert.IsFalse(_character.CanEquipImplant(eye));
        }

        [TestMethod]
        public void CanEquipQL50StoreBoughtEyeImplantAfterEquippingIntelligenceImplant()
        {
            var eye = Implant.GetImplant(ImplantSlot.Eye, Skill.Tutoring, Skill.PsychoModi, Skill.TimeAndSpace, 50);
            var head = Implant.GetImplant(ImplantSlot.Head, Ability.Intelligence, null, null, 20);

            Assert.IsTrue(_character.TryEquipImplant(head));
            Assert.IsTrue(_character.TryEquipImplant(eye));
        }

        [TestMethod]
        public void EquipsAndUnequipsSomeRandomImplants()
        {
            var head = Implant.GetImplant(ImplantSlot.Head, Skill.Treatment, null, Ability.Sense, 20);
            var ear = Implant.GetImplant(ImplantSlot.Ear, null, null, Ability.Intelligence, 20);
            var chest = Implant.GetImplant(ImplantSlot.Chest, Ability.Stamina, Ability.Psychic, Ability.Strength, 20);
            var eye = Implant.GetImplant(ImplantSlot.Eye, null, Skill.Treatment, null, 20);
            var leg = Implant.GetImplant(ImplantSlot.Leg, Ability.Agility, null, null, 20);
            var feet = Implant.GetImplant(ImplantSlot.Feet, null, Ability.Agility, null, 20);

            Assert.IsTrue(_character.TryEquipImplant(head));
            Assert.AreEqual(296, (int)_character.TreatmentValue);
            Assert.AreEqual(98, _character.GetAbilityValue(Ability.Sense));
            Assert.IsTrue(_character.TryEquipImplant(ear));
            Assert.AreEqual(297, (int)_character.TreatmentValue);
            Assert.AreEqual(107, _character.GetAbilityValue(Ability.Intelligence));
            Assert.AreEqual(head, _character.UnequipImplant(ImplantSlot.Head));
            Assert.AreEqual(281, (int)_character.TreatmentValue);
            Assert.AreEqual(94, _character.GetAbilityValue(Ability.Sense));
            Assert.IsTrue(_character.TryEquipImplant(head));
            Assert.AreEqual(297, (int)_character.TreatmentValue);
            Assert.AreEqual(107, _character.GetAbilityValue(Ability.Intelligence));
            Assert.IsTrue(_character.TryEquipImplant(chest));
            Assert.IsTrue(_character.TryEquipImplant(eye));
            Assert.IsTrue(_character.TryEquipImplant(leg));
            Assert.IsTrue(_character.TryEquipImplant(feet));
            Assert.AreEqual(307, (int)_character.TreatmentValue);
            Assert.AreEqual(107, _character.GetAbilityValue(Ability.Agility));
            Assert.AreEqual(107, _character.GetAbilityValue(Ability.Intelligence));
            Assert.AreEqual(104, _character.GetAbilityValue(Ability.Psychic));
            Assert.AreEqual(98, _character.GetAbilityValue(Ability.Sense));
            Assert.AreEqual(101, _character.GetAbilityValue(Ability.Stamina));
            Assert.AreEqual(95, _character.GetAbilityValue(Ability.Strength));

            var ql50Eye = Implant.GetImplant(ImplantSlot.Eye, null, Skill.SensoryImpr, Skill.MatterCrea, 50);

            Assert.AreEqual(50, Implant.GetMaxImplantQLForAbilityValue(_character.GetAbilityValue(Ability.Psychic)));
            Assert.IsTrue(_character.TryEquipImplant(ql50Eye));
            Assert.AreEqual(298, (int)_character.TreatmentValue);
        }

        [TestMethod]
        public void SetImplantIgnoresRequirementsButUnequipsFirst()
        {
            var lowHead = Implant.GetImplant(ImplantSlot.Head, Skill.Treatment, null, Ability.Sense, 20);
            var highHead = Implant.GetImplant(ImplantSlot.Head, Skill.Treatment, null, Ability.Sense, 200);

            Assert.IsTrue(_character.TryEquipImplant(lowHead));
            Assert.IsFalse(_character.TryEquipImplant(highHead));
            Assert.AreEqual(296, (int)_character.TreatmentValue);
            Assert.AreEqual(98, _character.GetAbilityValue(Ability.Sense));
            Assert.IsTrue(_character.IsImplantSlotFull(ImplantSlot.Head));
            Assert.IsFalse(_character.IsImplantSlotFull(ImplantSlot.LeftArm));

            _character.SetImplant(highHead);

            Assert.AreEqual(highHead, _character.GetImplant(ImplantSlot.Head));
            Assert.AreEqual(387.5, _character.TreatmentValue);
            Assert.AreEqual(116, _character.GetAbilityValue(Ability.Sense));
            Assert.IsTrue(_character.IsImplantSlotFull(ImplantSlot.Head));
            Assert.IsFalse(_character.IsImplantSlotFull(ImplantSlot.LeftArm));
        }

        [TestMethod]
        public void EquipMaxImplantUnequipsFirst()
        {
            _character.SetAbilityValue(Ability.Agility, 300); // Max sure it's not the limiting factor.
            var lowHead = Implant.GetImplant(ImplantSlot.Head, Skill.Treatment, null, Ability.Sense, 20);
            var maxHead1 = _character.GetMaxImplant(lowHead.ImplantTemplate);
            _character.SetImplant(lowHead);
            var maxHead2 = _character.GetMaxImplant(lowHead.ImplantTemplate);
            var impossibleHotswapQL = Implant.GetMaxImplantQL(_character.GetAbilityValue(Ability.Agility), _character.TreatmentValue);

            Assert.AreEqual(maxHead1.QL, maxHead2.QL);
            Assert.IsTrue(impossibleHotswapQL > maxHead1.QL);
        }

        [TestMethod]
        public void EquipsAndVerifiesSomeMaxImplants()
        {
            Assert.AreEqual(0, _character.GetTotalImplantQL());
            Assert.AreEqual(0, _character.GetAverageImplantQL());

            var maxHead = _character.EquipMaxImplant(ImplantTemplate.GetImplantTemplate(ImplantSlot.Head, Skill.Treatment, null, Ability.Sense));
            Assert.AreEqual(43, maxHead.QL);
            Assert.AreEqual(308.7, _character.TreatmentValue);
            Assert.AreEqual(100, _character.GetAbilityValue(Ability.Sense));
            Assert.AreEqual(91, _character.GetAbilityValue(Ability.Strength));
            Assert.AreEqual(43, _character.GetTotalImplantQL());
            Assert.AreEqual(43, _character.GetAverageImplantQL());

            var maxRightArm1 = _character.EquipMaxImplant(ImplantTemplate.GetImplantTemplate(ImplantSlot.RightArm, Ability.Strength, Skill.Brawling, null));
            Assert.AreEqual(43, maxRightArm1.QL);
            Assert.AreEqual(308.7, _character.TreatmentValue);
            Assert.AreEqual(100, _character.GetAbilityValue(Ability.Sense));
            Assert.AreEqual(107, _character.GetAbilityValue(Ability.Strength));

            var maxLeftArm = _character.EquipMaxImplant(ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftArm, Skill.Brawling, Ability.Strength, ArmorClass.ChemicalAC));
            Assert.AreEqual(51, maxLeftArm.QL);
            Assert.AreEqual(308.7, _character.TreatmentValue);
            Assert.AreEqual(100, _character.GetAbilityValue(Ability.Sense));
            Assert.AreEqual(118, _character.GetAbilityValue(Ability.Strength));

            var maxRightArm2 = _character.EquipMaxImplant(ImplantTemplate.GetImplantTemplate(ImplantSlot.RightArm, Ability.Strength, Skill.Brawling, null));
            Assert.AreEqual(49, maxRightArm2.QL);
            Assert.AreEqual(308.7, _character.TreatmentValue);
            Assert.AreEqual(100, _character.GetAbilityValue(Ability.Sense));
            Assert.AreEqual(119, _character.GetAbilityValue(Ability.Strength));

            var maxLeg = _character.EquipMaxImplant(ImplantTemplate.GetImplantTemplate(ImplantSlot.Leg, Ability.Agility, Ability.Stamina, null));
            Assert.AreEqual(43, maxLeg.QL);
            Assert.AreEqual(309.9, _character.TreatmentValue);
            Assert.AreEqual(100, _character.GetAbilityValue(Ability.Sense));
            Assert.AreEqual(119, _character.GetAbilityValue(Ability.Strength));
            Assert.AreEqual(107, _character.GetAbilityValue(Ability.Agility));
            Assert.AreEqual(100, _character.GetAbilityValue(Ability.Stamina));
            Assert.AreEqual(186, _character.GetTotalImplantQL());
            Assert.AreEqual(46.5, _character.GetAverageImplantQL());
        }
    }
}
