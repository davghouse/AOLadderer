using AOLadderer.LadderProcesses;
using AOLadderer.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            var finalImplantTemplates = new[]
            {
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Head, Skill.MatterCrea, Skill.NanoPool, Skill.Tutoring), // Intelligence
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftWrist, Skill.MultMelee, Skill.RunSpeed, Skill.NanoResist), // Agility
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightArm, Skill.OneHandBlunt, ArmorClass.ChemicalAC, ArmorClass.RadiationAC), // Strength
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Waist, ArmorClass.ChemicalAC, MaxHealthOrNano.MaxNano, Skill.BioMetamor) // Sense
            };

            var ladderProcess = new DegenerateLadderProcess(_character, finalImplantTemplates);

            Assert.AreEqual(
                Implant.GetMaxImplantQL(_character.GetAbilityValue(Ability.Intelligence), _character.TreatmentValue),
                ladderProcess.OrderedFinalImplants.Single(i => i.ImplantTemplate == finalImplantTemplates[0]).QL);
            Assert.AreEqual(
                Implant.GetMaxImplantQL(_character.GetAbilityValue(Ability.Agility), _character.TreatmentValue),
                ladderProcess.OrderedFinalImplants.Single(i => i.ImplantTemplate == finalImplantTemplates[1]).QL);
            Assert.AreEqual(
                Implant.GetMaxImplantQL(_character.GetAbilityValue(Ability.Strength), _character.TreatmentValue),
                ladderProcess.OrderedFinalImplants.Single(i => i.ImplantTemplate == finalImplantTemplates[2]).QL);
            Assert.AreEqual(
                Implant.GetMaxImplantQL(_character.GetAbilityValue(Ability.Sense), _character.TreatmentValue),
                ladderProcess.OrderedFinalImplants.Single(i => i.ImplantTemplate == finalImplantTemplates[3]).QL);

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
            var finalImplantTemplates = new[]
            {
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Head, Skill.QuantumFT, null, Skill.Tutoring), // Agility
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftWrist, Skill.MultMelee, Skill.RunSpeed, Skill.NanoResist), // Agility
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightArm, Skill.OneHandBlunt, ArmorClass.ChemicalAC, ArmorClass.RadiationAC), // Strength
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Leg, Ability.Agility, Ability.Stamina, null), // Stamina
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Waist, ArmorClass.ChemicalAC, null, Skill.BioMetamor) // Stamina
            };

            var ladderProcess = new DegenerateLadderProcess(_character, finalImplantTemplates);

            Assert.AreEqual(
                Implant.GetMaxImplantQL(_character.GetAbilityValue(Ability.Stamina), _character.TreatmentValue),
                ladderProcess.OrderedFinalImplants[0].QL);
            Assert.AreEqual(finalImplantTemplates[3], ladderProcess.OrderedFinalImplants[0].ImplantTemplate);
            Assert.AreEqual(
                Implant.GetMaxImplantQL(_character.GetAbilityValue(Ability.Agility) + ladderProcess.OrderedFinalImplants[0].ShinyStatIncrease.Value, _character.TreatmentValue),
                ladderProcess.OrderedFinalImplants.Single(i => i.ImplantTemplate == finalImplantTemplates[0]).QL);
            Assert.AreEqual(51, ladderProcess.OrderedFinalImplants.Single(i => i.ImplantTemplate == finalImplantTemplates[0]).QL);
            Assert.AreEqual(
                Implant.GetMaxImplantQL(_character.GetAbilityValue(Ability.Agility) + ladderProcess.OrderedFinalImplants[0].ShinyStatIncrease.Value, _character.TreatmentValue),
                ladderProcess.OrderedFinalImplants.Single(i => i.ImplantTemplate == finalImplantTemplates[1]).QL);
            Assert.AreEqual(
                Implant.GetMaxImplantQL(_character.GetAbilityValue(Ability.Strength), _character.TreatmentValue),
                ladderProcess.OrderedFinalImplants.Single(i => i.ImplantTemplate == finalImplantTemplates[2]).QL);
            Assert.AreEqual(
                Implant.GetMaxImplantQL(_character.GetAbilityValue(Ability.Stamina) + ladderProcess.OrderedFinalImplants[0].ShinyStatIncrease.Value, _character.TreatmentValue),
                Implant.GetMaxImplantQL(_character.GetAbilityValue(Ability.Sense), _character.TreatmentValue),
                ladderProcess.OrderedFinalImplants.Single(i => i.ImplantTemplate == finalImplantTemplates[4]).QL);

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
            var finalImplantTemplates = new[]
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

            var ladderProcess = new DegenerateLadderProcess(_character, finalImplantTemplates);

            // Implants should be unequipped right now and stats should be the same as they were before laddering.
            Assert.AreEqual(0, _character.GetAverageImplantQL());
            Assert.AreEqual(0, _character.GetTotalImplantQL());
            Assert.AreEqual(91, _character.GetAbilityValue(Ability.Agility));
            Assert.AreEqual(91, _character.GetAbilityValue(Ability.Stamina));
            Assert.AreEqual(281.4, _character.TreatmentValue);

            // The final QL should be greater than what we get by w/o any laddered equips.
            int unladderedTotalImplantQL = 0;
            foreach (var implantTemplate in finalImplantTemplates)
            {
                unladderedTotalImplantQL += _character.GetMaxImplant(implantTemplate).QL;
            }
            Assert.IsTrue(unladderedTotalImplantQL < ladderProcess.TotalFinalImplantQL);

            // The final QL should be greater than what we get by laddering in the arbitrary order declared above.
            foreach (var implantTemplate in finalImplantTemplates)
            {
                _character.EquipMaxImplant(implantTemplate);
            }
            Assert.IsTrue(unladderedTotalImplantQL < _character.GetTotalImplantQL());
            Assert.IsTrue(_character.GetTotalImplantQL() < ladderProcess.TotalFinalImplantQL);

            Assert.AreEqual(0, ladderProcess.OrderedLadderImplants.Count);

            // Some regression tests, I don't know for sure that 480 is what to expect, hopefully won't ever drop below this.
            Assert.AreEqual(480, ladderProcess.TotalFinalImplantQL);
            Assert.AreEqual(480 / (double)9, ladderProcess.AverageFinalImplantQL);
        }

        [TestMethod]
        public void DegenerateLadderProcessLeavesAlreadyEquippedImplantsAlone()
        {
            var alreadyEquippedImplants = new[]
            {
                Implant.GetImplant(ImplantSlot.Head, Skill.Treatment, null, Ability.Sense, 200),
                Implant.GetImplant(ImplantSlot.Leg, Ability.Agility, Ability.Stamina, null, 200)
            };
            _character.SetImplants(alreadyEquippedImplants);
            Assert.AreEqual(146, _character.GetAbilityValue(Ability.Agility));
            Assert.AreEqual(124, _character.GetAbilityValue(Ability.Stamina));
            Assert.AreEqual(116, _character.GetAbilityValue(Ability.Sense));
            Assert.AreEqual(391.625, _character.TreatmentValue);

            var finalImplantTemplates = new[]
            {
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Chest, Ability.Sense, Ability.Psychic, Ability.Strength), // Psychic
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightArm, Ability.Strength, null, null), // Sense
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Waist, null, Ability.Sense, Ability.Stamina), // Psychic
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Eye, null, Skill.Treatment, null), // Agility
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightHand, null, null, Skill.Treatment), // Intelligence
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Feet, null, Ability.Agility, null), // Agility
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Ear, null, null, Ability.Intelligence) // Psychic
            };
            var finalImplantSlots = finalImplantTemplates.Select(t => t.ImplantSlot).ToArray();

            var ladderProcess = new DegenerateLadderProcess(_character, finalImplantTemplates);

            Assert.AreEqual(146, _character.GetAbilityValue(Ability.Agility));
            Assert.AreEqual(124, _character.GetAbilityValue(Ability.Stamina));
            Assert.AreEqual(116, _character.GetAbilityValue(Ability.Sense));
            Assert.AreEqual(391.625, _character.TreatmentValue);
            Assert.AreEqual(_character.GetImplant(ImplantSlot.Head), alreadyEquippedImplants[0]);
            Assert.AreEqual(_character.GetImplant(ImplantSlot.Leg), alreadyEquippedImplants[1]);
            Assert.AreEqual(400, _character.GetTotalImplantQL());

            // The final QL should be greater than what we get by w/o any laddered equips.
            int unladderedTotalFinalImplantQL = 0;
            foreach (var implantTemplate in finalImplantTemplates)
            {
                unladderedTotalFinalImplantQL += _character.GetMaxImplant(implantTemplate).QL;
            }
            Assert.IsTrue(unladderedTotalFinalImplantQL < ladderProcess.TotalFinalImplantQL);

            // The final QL should be greater than what we get by laddering in the arbitrary order declared above.
            foreach (var implantTemplate in finalImplantTemplates)
            {
                _character.EquipMaxImplant(implantTemplate);
            }
            Assert.IsTrue(unladderedTotalFinalImplantQL < _character.GetTotalImplantQL(finalImplantSlots));
            Assert.IsTrue(_character.GetTotalImplantQL(finalImplantSlots) < ladderProcess.TotalFinalImplantQL);

            Assert.AreEqual(0, ladderProcess.OrderedLadderImplants.Count);

            // Some regression tests, I don't know for sure that 423 is what to expect, hopefully won't ever drop below this.
            Assert.AreEqual(388, unladderedTotalFinalImplantQL);
            Assert.AreEqual(406, _character.GetTotalImplantQL(finalImplantSlots));
            Assert.AreEqual(406 / (double)7, _character.GetAverageImplantQL(finalImplantSlots));
            Assert.AreEqual(423, ladderProcess.TotalFinalImplantQL);
            Assert.AreEqual(423 / (double)7, ladderProcess.AverageFinalImplantQL);
        }

        [TestMethod]
        public void DegenerateLadderProcessEquipsImplantsInTheProperThreePhases()
        {
            // The three phases are
            //  1. the final ladder implants in initially empty slots,
            //  2. the final non-ladder implants in initially empty slots,
            //  3. the final implants in initially full slots.

            var alreadyEquippedImplants = new[]
            {
                Implant.GetImplant(ImplantSlot.Head, Skill.Treatment, null, Ability.Sense, 200),
                Implant.GetImplant(ImplantSlot.Leg, Ability.Agility, Ability.Stamina, null, 200),
                Implant.GetImplant(ImplantSlot.Ear, Skill.Perception, null, Ability.Intelligence, 200)
            };
            _character.SetImplants(alreadyEquippedImplants);

            var finalImplantTemplates = new[]
            {
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Chest, Ability.Sense, Ability.Psychic, Ability.Strength), // 1
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightArm, Skill.Burst, null, null), // 2
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Head, Skill.Treatment, null, null), // 3
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Eye, null, Skill.Treatment, null), // 1
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightHand, null, null, Skill.Grenade), // 2
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftArm, Skill.Brawling, null, null), // 2
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Feet, null, Ability.Agility, null), // 1
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Ear, null, null, Skill.PsychoModi) // 3
            };

            var ladderProcess = new DegenerateLadderProcess(_character, finalImplantTemplates);

            CollectionAssert.AreEquivalent(
                new[] { finalImplantTemplates[0], finalImplantTemplates[3], finalImplantTemplates[6] },
                ladderProcess.OrderedFinalImplants.Take(3).Select(i => i.ImplantTemplate).ToArray());
            CollectionAssert.AreEquivalent(
                new[] { finalImplantTemplates[1], finalImplantTemplates[4], finalImplantTemplates[5] },
                ladderProcess.OrderedFinalImplants.Skip(3).Take(3).Select(i => i.ImplantTemplate).ToArray());
            CollectionAssert.AreEquivalent(
                new[] { finalImplantTemplates[2], finalImplantTemplates[7] },
                ladderProcess.OrderedFinalImplants.Skip(6).Take(2).Select(i => i.ImplantTemplate).ToArray());
        }

        [TestMethod]
        public void DegenerateLadderProcessUnequipsWeakLadderImplantsFirst()
        {
            // The head and ear are strong ladder implants and shouldn't be equipped before the weaker leg, eye, and feet.
            var alreadyEquippedImplants = new[]
            {
                Implant.GetImplant(ImplantSlot.Head, Skill.Treatment, null, Ability.Sense, 200),
                Implant.GetImplant(ImplantSlot.Leg, Ability.Agility, Ability.Stamina, null, 1),
                Implant.GetImplant(ImplantSlot.Ear, Skill.Perception, null, Ability.Intelligence, 200),
                Implant.GetImplant(ImplantSlot.Eye, null, Skill.Treatment, null, 1),
                Implant.GetImplant(ImplantSlot.Feet, null, Ability.Agility, null, 1)
            };
            _character.SetImplants(alreadyEquippedImplants);

            var finalImplantTemplates = new[]
            {
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Chest, Ability.Sense, Ability.Psychic, Ability.Strength), // 1
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightArm, Skill.Burst, null, null), // 2
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Head, Skill.Treatment, null, null), // 3
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Eye, null, Skill.Treatment, Skill.Bow), // 1
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightHand, null, null, Skill.Grenade), // 2
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftArm, Skill.Brawling, null, null), // 2
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Feet, null, Ability.Agility, null), // 1
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Ear, null, null, Skill.PsychoModi) // 3
            };

            var ladderProcess = new DegenerateLadderProcess(_character, finalImplantTemplates);

            var orderedFinalImplantSlots = ladderProcess.OrderedFinalImplants
                .Select(i => i.ImplantSlot)
                .ToArray();

            Assert.IsTrue(Array.IndexOf(orderedFinalImplantSlots, ImplantSlot.Head) > Array.IndexOf(orderedFinalImplantSlots, ImplantSlot.Eye));
            Assert.IsTrue(Array.IndexOf(orderedFinalImplantSlots, ImplantSlot.Head) > Array.IndexOf(orderedFinalImplantSlots, ImplantSlot.Feet));
            Assert.IsTrue(Array.IndexOf(orderedFinalImplantSlots, ImplantSlot.Ear) > Array.IndexOf(orderedFinalImplantSlots, ImplantSlot.Eye));
            Assert.IsTrue(Array.IndexOf(orderedFinalImplantSlots, ImplantSlot.Ear) > Array.IndexOf(orderedFinalImplantSlots, ImplantSlot.Feet));
        }

        [TestMethod]
        public void DegenerateLadderProcessHandlesEmptyFinalImplantTemplates()
        {
            var ladderProcess = new DegenerateLadderProcess(_character, new ImplantTemplate[0]);
            Assert.AreEqual(0, ladderProcess.TotalFinalImplantQL);
            Assert.AreEqual(0, ladderProcess.AverageFinalImplantQL);
            Assert.AreEqual(0, ladderProcess.OrderedLadderImplants.Count);
            Assert.AreEqual(0, ladderProcess.OrderedFinalImplants.Count);
        }

        [TestMethod, Timeout(60000)]
        public void DegenerateLadderDoesntTakeForeverEvenWhenHighNumberOfFinalImplantsAsLadderImplants()
        {
            var finalImplantTemplates = new[]
            {
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Head, Skill.TimeAndSpace, Skill.NanoPool, Ability.Sense),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Eye, Skill.AimedShot, Skill.Treatment, Skill.TimeAndSpace),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Ear, null, Skill.Concealment, Ability.Intelligence),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Chest, Ability.Stamina, Skill.BioMetamor, Skill.SensoryImpr),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightArm, Ability.Strength, null, null),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftArm, null, Ability.Strength, null),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Waist, ArmorClass.FireAC, MaxHealthOrNano.MaxHealth, Skill.EvadeClsC),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightWrist, Skill.RunSpeed, Skill.NanoResist, Skill.MultMelee),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftWrist, Skill.MultMelee, Skill.RunSpeed, Skill.NanoResist),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Leg, Ability.Agility, Skill.EvadeClsC, ArmorClass.MeleeMaAC),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightHand, Skill.MartialArts, Skill.TimeAndSpace, Skill.Treatment),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftHand, null, ArmorClass.FireAC, Skill.MartialArts),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Feet, Skill.EvadeClsC, Ability.Agility, Skill.DuckExp),
            };
            Assert.AreEqual(9, finalImplantTemplates.Count(t => t.RaisesLadderStats));

            var ladderProcess = new DegenerateLadderProcess(_character, finalImplantTemplates);
        }
    }
}
