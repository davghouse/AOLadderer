using AOLadderer.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AOLadderer.UnitTests
{
    [TestClass]
    public class CharacterTests
    {
        private Character _level25MP;

        [TestInitialize]
        public void Initialize()
        {
            _level25MP = new Character(
                ladderStatValues: new LadderStatValues(agilityValue: 91, intelligenceValue: 103, psychicValue: 98,
                    senseValue: 94, staminaValue: 91, strengthValue: 91, treatmentValue: 281.4),
                implantConfiguration: null);
        }

        [TestMethod]
        public void AllImplantSlotsStartEmpty()
            => Assert.IsTrue(ImplantSlot.ImplantSlots.All(_level25MP.IsImplantSlotEmpty));

        [TestMethod]
        public void CannotDirectlyEquipQL50StoreBoughtEyeImplant()
        {
            var eye = Implant.BuildImplant(ImplantSlot.Eye, Skill.Tutoring, Skill.PsychoModi, Skill.TimeAndSpace, 50);
            Assert.IsFalse(_level25MP.CanEquipImplant(eye));
        }

        [TestMethod]
        public void CanEquipQL50StoreBoughtEyeImplantAfterEquippingIntelligenceImplant()
        {
            var eye = Implant.BuildImplant(ImplantSlot.Eye, Skill.Tutoring, Skill.PsychoModi, Skill.TimeAndSpace, 50);
            var head = Implant.BuildImplant(ImplantSlot.Head, Ability.Intelligence, null, null, 20);

            Assert.IsTrue(_level25MP.TryEquipImplant(head));
            Assert.IsTrue(_level25MP.TryEquipImplant(eye));
        }

        [TestMethod]
        public void EquipsAndUnequipsSomeRandomImplants()
        {
            var head = Implant.BuildImplant(ImplantSlot.Head, Skill.Treatment, null, Ability.Sense, 20);
            var ear = Implant.BuildImplant(ImplantSlot.Ear, null, null, Ability.Intelligence, 20);
            var chest = Implant.BuildImplant(ImplantSlot.Chest, Ability.Stamina, Ability.Psychic, Ability.Strength, 20);
            var eye = Implant.BuildImplant(ImplantSlot.Eye, null, Skill.Treatment, null, 20);
            var leg = Implant.BuildImplant(ImplantSlot.Leg, Ability.Agility, null, null, 20);
            var feet = Implant.BuildImplant(ImplantSlot.Feet, null, Ability.Agility, null, 20);

            Assert.IsTrue(_level25MP.TryEquipImplant(head));
            Assert.AreEqual(296, (int)_level25MP.TreatmentValue);
            Assert.AreEqual(98, _level25MP.GetAbilityValue(Ability.Sense));
            Assert.IsTrue(_level25MP.TryEquipImplant(ear));
            Assert.AreEqual(297, (int)_level25MP.TreatmentValue);
            Assert.AreEqual(107, _level25MP.GetAbilityValue(Ability.Intelligence));
            Assert.AreEqual(head, _level25MP.UnequipImplant(ImplantSlot.Head));
            Assert.AreEqual(281, (int)_level25MP.TreatmentValue);
            Assert.AreEqual(94, _level25MP.GetAbilityValue(Ability.Sense));
            Assert.IsTrue(_level25MP.TryEquipImplant(head));
            Assert.AreEqual(297, (int)_level25MP.TreatmentValue);
            Assert.AreEqual(107, _level25MP.GetAbilityValue(Ability.Intelligence));
            Assert.IsTrue(_level25MP.TryEquipImplant(chest));
            Assert.IsTrue(_level25MP.TryEquipImplant(eye));
            Assert.IsTrue(_level25MP.TryEquipImplant(leg));
            Assert.IsTrue(_level25MP.TryEquipImplant(feet));
            Assert.AreEqual(307, (int)_level25MP.TreatmentValue);
            Assert.AreEqual(107, _level25MP.GetAbilityValue(Ability.Agility));
            Assert.AreEqual(107, _level25MP.GetAbilityValue(Ability.Intelligence));
            Assert.AreEqual(104, _level25MP.GetAbilityValue(Ability.Psychic));
            Assert.AreEqual(98, _level25MP.GetAbilityValue(Ability.Sense));
            Assert.AreEqual(101, _level25MP.GetAbilityValue(Ability.Stamina));
            Assert.AreEqual(95, _level25MP.GetAbilityValue(Ability.Strength));

            var ql50Eye = Implant.BuildImplant(ImplantSlot.Eye, null, Skill.SensoryImpr, Skill.MatterCrea, 50);

            Assert.AreEqual(50, Implant.GetMaxImplantQLForAbilityValue(_level25MP.GetAbilityValue(Ability.Psychic)));
            Assert.IsTrue(_level25MP.TryEquipImplant(ql50Eye));
            Assert.AreEqual(298, (int)_level25MP.TreatmentValue);
        }

        [TestMethod]
        public void SetImplantIgnoresRequirementsButUnequipsFirst()
        {
            var lowHead = Implant.BuildImplant(ImplantSlot.Head, Skill.Treatment, null, Ability.Sense, 20);
            var highHead = Implant.BuildImplant(ImplantSlot.Head, Skill.Treatment, null, Ability.Sense, 200);

            Assert.IsTrue(_level25MP.TryEquipImplant(lowHead));
            Assert.IsFalse(_level25MP.TryEquipImplant(highHead));
            Assert.AreEqual(296, (int)_level25MP.TreatmentValue);
            Assert.AreEqual(98, _level25MP.GetAbilityValue(Ability.Sense));
            Assert.IsTrue(_level25MP.IsImplantSlotFull(ImplantSlot.Head));
            Assert.IsFalse(_level25MP.IsImplantSlotFull(ImplantSlot.LeftArm));

            _level25MP.SetImplant(highHead);

            Assert.AreEqual(highHead, _level25MP.GetImplant(ImplantSlot.Head));
            Assert.AreEqual(387.5, _level25MP.TreatmentValue);
            Assert.AreEqual(116, _level25MP.GetAbilityValue(Ability.Sense));
            Assert.IsTrue(_level25MP.IsImplantSlotFull(ImplantSlot.Head));
            Assert.IsFalse(_level25MP.IsImplantSlotFull(ImplantSlot.LeftArm));
        }

        [TestMethod]
        public void EquipMaxImplantUnequipsFirst()
        {
            _level25MP.SetAbilityValue(Ability.Agility, 300); // Max sure it's not the limiting factor.
            var lowHead = Implant.BuildImplant(ImplantSlot.Head, Skill.Treatment, null, Ability.Sense, 20);
            var maxHead1 = _level25MP.GetMaxImplant(lowHead.ImplantTemplate);
            _level25MP.SetImplant(lowHead);
            var maxHead2 = _level25MP.GetMaxImplant(lowHead.ImplantTemplate);
            var impossibleHotswapQL = Implant.GetMaxImplantQL(_level25MP.GetAbilityValue(Ability.Agility), _level25MP.TreatmentValue);

            Assert.AreEqual(maxHead1.ImplantQL, maxHead2.ImplantQL);
            Assert.IsTrue(impossibleHotswapQL > maxHead1.ImplantQL);
        }

        [TestMethod]
        public void EquipsAndVerifiesSomeMaxImplants()
        {
            var maxHead = _level25MP.EquipMaxImplant(ImplantTemplate.GetImplantTemplate(ImplantSlot.Head, Skill.Treatment, null, Ability.Sense));
            Assert.AreEqual(43, maxHead.ImplantQL);
            Assert.AreEqual(308.7, _level25MP.TreatmentValue);
            Assert.AreEqual(100, _level25MP.GetAbilityValue(Ability.Sense));
            Assert.AreEqual(91, _level25MP.GetAbilityValue(Ability.Strength));

            var maxRightArm1 = _level25MP.EquipMaxImplant(ImplantTemplate.GetImplantTemplate(ImplantSlot.RightArm, Ability.Strength, Skill.Brawling, null));
            Assert.AreEqual(43, maxRightArm1.ImplantQL);
            Assert.AreEqual(308.7, _level25MP.TreatmentValue);
            Assert.AreEqual(100, _level25MP.GetAbilityValue(Ability.Sense));
            Assert.AreEqual(107, _level25MP.GetAbilityValue(Ability.Strength));

            var maxLeftArm = _level25MP.EquipMaxImplant(ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftArm, Skill.Brawling, Ability.Strength, ArmorClass.ChemicalAC));
            Assert.AreEqual(51, maxLeftArm.ImplantQL);
            Assert.AreEqual(308.7, _level25MP.TreatmentValue);
            Assert.AreEqual(100, _level25MP.GetAbilityValue(Ability.Sense));
            Assert.AreEqual(118, _level25MP.GetAbilityValue(Ability.Strength));

            var maxRightArm2 = _level25MP.EquipMaxImplant(ImplantTemplate.GetImplantTemplate(ImplantSlot.RightArm, Ability.Strength, Skill.Brawling, null));
            Assert.AreEqual(49, maxRightArm2.ImplantQL);
            Assert.AreEqual(308.7, _level25MP.TreatmentValue);
            Assert.AreEqual(100, _level25MP.GetAbilityValue(Ability.Sense));
            Assert.AreEqual(119, _level25MP.GetAbilityValue(Ability.Strength));

            var maxLeg = _level25MP.EquipMaxImplant(ImplantTemplate.GetImplantTemplate(ImplantSlot.Leg, Ability.Agility, Ability.Stamina, null));
            Assert.AreEqual(43, maxLeg.ImplantQL);
            Assert.AreEqual(309.9, _level25MP.TreatmentValue);
            Assert.AreEqual(100, _level25MP.GetAbilityValue(Ability.Sense));
            Assert.AreEqual(119, _level25MP.GetAbilityValue(Ability.Strength));
            Assert.AreEqual(107, _level25MP.GetAbilityValue(Ability.Agility));
            Assert.AreEqual(100, _level25MP.GetAbilityValue(Ability.Stamina));
        }
    }
}
