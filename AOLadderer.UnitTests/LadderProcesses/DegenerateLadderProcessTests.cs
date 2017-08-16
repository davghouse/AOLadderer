using AOLadderer.LadderProcesses;
using AOLadderer.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AOLadderer.UnitTests.LadderProcesses
{
    [TestClass]
    public class DegenerateLadderProcessTests
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
        public void DegenerateLadderProcessWhenNoFinalLadderImplants()
        {
            var implantTemplates = new[]
            {
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Head, Skill.MatterCrea, Skill.NanoPool, Skill.Tutoring), // Intelligence
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftWrist, Skill.MultMelee, Skill.RunSpeed, Skill.NanoResist), // Agility
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightArm, Skill.OneHandBlunt, ArmorClass.ChemicalAC, ArmorClass.RadiationAC), // Strength
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Waist, ArmorClass.ChemicalAC, MaxHealthOrNano.MaxNano, Skill.BioMetamor) // Sense
            };

            var ladderProcess = new DegenerateLadderProcess(_character, implantTemplates);

            Assert.AreEqual(
                Implant.GetMaxImplantQL(_character.GetAbilityValue(Ability.Intelligence), _character.TreatmentValue),
                ladderProcess.OrderedFinalImplants.Single(i => i.ImplantTemplate == implantTemplates[0]).QL);
            Assert.AreEqual(
                Implant.GetMaxImplantQL(_character.GetAbilityValue(Ability.Agility), _character.TreatmentValue),
                ladderProcess.OrderedFinalImplants.Single(i => i.ImplantTemplate == implantTemplates[1]).QL);
            Assert.AreEqual(
                Implant.GetMaxImplantQL(_character.GetAbilityValue(Ability.Strength), _character.TreatmentValue),
                ladderProcess.OrderedFinalImplants.Single(i => i.ImplantTemplate == implantTemplates[2]).QL);
            Assert.AreEqual(
                Implant.GetMaxImplantQL(_character.GetAbilityValue(Ability.Sense), _character.TreatmentValue),
                ladderProcess.OrderedFinalImplants.Single(i => i.ImplantTemplate == implantTemplates[3]).QL);

            // Implants should be unequipped right now and stats should be the same as they were before laddering.
            Assert.AreEqual(0, _character.GetAverageImplantQL());
            Assert.AreEqual(0, _character.GetTotalImplantQL());
            Assert.AreEqual(91, _character.GetAbilityValue(Ability.Agility));
            Assert.AreEqual(91, _character.GetAbilityValue(Ability.Stamina));
            Assert.AreEqual(281.4, _character.TreatmentValue);
        }

        [TestMethod]
        public void DegenerateLadderProcessWhenASingleFinalLadderImplant()
        {
            var implantTemplates = new[]
            {
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Head, Skill.QuantumFT, null, Skill.Tutoring), // Agility
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftWrist, Skill.MultMelee, Skill.RunSpeed, Skill.NanoResist), // Agility
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightArm, Skill.OneHandBlunt, ArmorClass.ChemicalAC, ArmorClass.RadiationAC), // Strength
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Leg, Ability.Agility, Ability.Stamina, null), // Stamina
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Waist, ArmorClass.ChemicalAC, null, Skill.BioMetamor) // Stamina
            };

            var ladderProcess = new DegenerateLadderProcess(_character, implantTemplates);

            Assert.AreEqual(
                Implant.GetMaxImplantQL(_character.GetAbilityValue(Ability.Stamina), _character.TreatmentValue),
                ladderProcess.OrderedFinalImplants[0].QL);
            Assert.AreEqual(implantTemplates[3], ladderProcess.OrderedFinalImplants[0].ImplantTemplate);
            Assert.AreEqual(
                Implant.GetMaxImplantQL(_character.GetAbilityValue(Ability.Agility) + ladderProcess.OrderedFinalImplants[0].ShinyStatIncrease.Value, _character.TreatmentValue),
                ladderProcess.OrderedFinalImplants.Single(i => i.ImplantTemplate == implantTemplates[0]).QL);
            Assert.AreEqual(51, ladderProcess.OrderedFinalImplants.Single(i => i.ImplantTemplate == implantTemplates[0]).QL);
            Assert.AreEqual(
                Implant.GetMaxImplantQL(_character.GetAbilityValue(Ability.Agility) + ladderProcess.OrderedFinalImplants[0].ShinyStatIncrease.Value, _character.TreatmentValue),
                ladderProcess.OrderedFinalImplants.Single(i => i.ImplantTemplate == implantTemplates[1]).QL);
            Assert.AreEqual(
                Implant.GetMaxImplantQL(_character.GetAbilityValue(Ability.Strength), _character.TreatmentValue),
                ladderProcess.OrderedFinalImplants.Single(i => i.ImplantTemplate == implantTemplates[2]).QL);
            Assert.AreEqual(
                Implant.GetMaxImplantQL(_character.GetAbilityValue(Ability.Stamina) + ladderProcess.OrderedFinalImplants[0].ShinyStatIncrease.Value, _character.TreatmentValue),
                Implant.GetMaxImplantQL(_character.GetAbilityValue(Ability.Sense), _character.TreatmentValue),
                ladderProcess.OrderedFinalImplants.Single(i => i.ImplantTemplate == implantTemplates[4]).QL);

            // Implants should be unequipped right now and stats should be the same as they were before laddering.
            Assert.AreEqual(0, _character.GetAverageImplantQL());
            Assert.AreEqual(0, _character.GetTotalImplantQL());
            Assert.AreEqual(91, _character.GetAbilityValue(Ability.Agility));
            Assert.AreEqual(91, _character.GetAbilityValue(Ability.Stamina));
            Assert.AreEqual(281.4, _character.TreatmentValue);

            // Using the order the ladder process outlines, all equips should be possible and maximal.
            foreach (var finalImplant in ladderProcess.OrderedFinalImplants)
            {
                Assert.IsTrue(_character.CanEquipImplant(finalImplant));
                var maxImplant = _character.EquipMaxImplant(finalImplant.ImplantTemplate);
                Assert.AreEqual(maxImplant, finalImplant);
            }
        }

        [TestMethod]
        public void DegenerateLadderProcessWhenManyFinalLadderImplants()
        {
            var implantTemplates = new[]
            {
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Head, Skill.Treatment, null, Ability.Sense), // Agility
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Chest, Ability.Sense, Ability.Psychic, Ability.Strength), // Psychic
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightArm, Ability.Strength, null, null), // Sense
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Leg, Ability.Agility, Ability.Stamina, null), // Stamina
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Waist, null, Ability.Sense, Ability.Stamina), // Psychic
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Eye, null, Skill.Treatment, null), // Agility
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightHand, null, null, Skill.Treatment), // Intelligence
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Feet, null, Ability.Agility, null), // Agility
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Ear, null, null, Ability.Intelligence) // Psychic
            };

            var ladderProcess = new DegenerateLadderProcess(_character, implantTemplates);

            // Implants should be unequipped right now and stats should be the same as they were before laddering.
            Assert.AreEqual(0, _character.GetAverageImplantQL());
            Assert.AreEqual(0, _character.GetTotalImplantQL());
            Assert.AreEqual(91, _character.GetAbilityValue(Ability.Agility));
            Assert.AreEqual(91, _character.GetAbilityValue(Ability.Stamina));
            Assert.AreEqual(281.4, _character.TreatmentValue);

            // The final QL should be greater than what we get by w/o any laddered equips.
            int unladderedTotalImplantQL = 0;
            foreach (var implantTemplate in implantTemplates)
            {
                unladderedTotalImplantQL += _character.GetMaxImplant(implantTemplate).QL;
            }
            Assert.IsTrue(unladderedTotalImplantQL < ladderProcess.TotalImplantQL);

            // The final QL should be greater than what we get by laddering in the order above.
            foreach (var implantTemplate in implantTemplates)
            {
                _character.EquipMaxImplant(implantTemplate);
            }
            Assert.IsTrue(unladderedTotalImplantQL < _character.GetTotalImplantQL());
            Assert.IsTrue(_character.GetTotalImplantQL() < ladderProcess.TotalImplantQL);

            Assert.AreEqual(0, ladderProcess.OrderedLadderImplants.Count);

            // Some regression tests, I don't know for sure that 470 is what to expect, but certainly at least 470.
            Assert.AreEqual(480, ladderProcess.TotalImplantQL);
            Assert.AreEqual(480 / (double)9, ladderProcess.AverageImplantQL);
        }
    }
}
