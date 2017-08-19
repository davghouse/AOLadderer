using AOLadderer.LadderProcesses;
using AOLadderer.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AOLadderer.UnitTests.LadderProcesses
{
    [TestClass]
    public class BasicLadderProcessTests
    {
        private Character _character;
        private IReadOnlyList<ImplantTemplate> _finalImplantTemplates;

        [TestInitialize]
        public void Initialize()
        {
            _character = new Character(
                agilityValue: 281, intelligenceValue: 315, psychicValue: 305,
                senseValue: 213, staminaValue: 301, strengthValue: 316, treatmentValue: 662.6250);
            _finalImplantTemplates = new[]
            {
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Head, Skill.BioMetamor, Skill.NanoPool, Ability.Sense),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Eye, Skill.AimedShot, Skill.SensoryImpr, Skill.TimeAndSpace),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Ear, null, Skill.Concealment, Skill.PsychoModi),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Chest, Skill.BodyDev, Skill.BioMetamor, Skill.SensoryImpr),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Waist, ArmorClass.FireAC, ArmorClass.MeleeMaAC, Skill.EvadeClsC),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightWrist, Skill.RunSpeed, Skill.NanoResist, Skill.MultMelee),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftWrist, Skill.MultMelee, Skill.RunSpeed, Skill.NanoResist),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Leg, Ability.Agility, Skill.EvadeClsC, ArmorClass.MeleeMaAC),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightHand, Skill.MartialArts, Skill.TimeAndSpace, Skill.Treatment),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftHand, null, ArmorClass.FireAC, Skill.MartialArts),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Feet, Skill.EvadeClsC, Skill.MartialArts, Skill.DuckExp),
            };
        }

        [TestMethod]
        public void BasicLadderProcessIsPossible()
        {
            var ladderProcess = new BasicLadderProcess(_character, _finalImplantTemplates);

            foreach (var ladderImplant in ladderProcess.OrderedLadderImplants)
            {
                Assert.IsTrue(_character.TryEquipImplant(ladderImplant));
            }

            foreach (var finalImplant in ladderProcess.OrderedFinalImplants)
            {
                Assert.IsTrue(_character.TryEquipImplant(finalImplant));
            }

            var finalImplantSlots = _finalImplantTemplates
                .Select(t => t.ImplantSlot)
                .ToArray();

            Assert.AreEqual(_character.GetTotalImplantQL(finalImplantSlots), ladderProcess.TotalFinalImplantQL);
            Assert.AreEqual(_character.GetAverageImplantQL(finalImplantSlots), ladderProcess.AverageFinalImplantQL);
        }

        [TestMethod]
        public void BasicLadderProcessLeavesCharacterInOriginalState()
        {
            var ladderProcess = new BasicLadderProcess(_character, _finalImplantTemplates);
            Assert.AreEqual(0, _character.GetTotalImplantQL());
            Assert.AreEqual(662.6250, _character.TreatmentValue);
            Assert.AreEqual(281, _character.GetAbilityValue(Ability.Agility));
        }

        [TestMethod]
        public void BasicLadderProcessIsDeterministicWhenOldApplicationIsnt()
        {
            var ladderProcesses = new[]
            {
                new BasicLadderProcess(_character, _finalImplantTemplates),
                new BasicLadderProcess(_character, _finalImplantTemplates),
                new BasicLadderProcess(_character, _finalImplantTemplates),
                new BasicLadderProcess(_character, _finalImplantTemplates),
                new BasicLadderProcess(_character, _finalImplantTemplates)
            };

            Assert.IsTrue(ladderProcesses.All(p => p.TotalFinalImplantQL == ladderProcesses[0].TotalFinalImplantQL));
            Assert.IsTrue(ladderProcesses.All(p => p.OrderedLadderImplants.SequenceEqual(ladderProcesses[0].OrderedLadderImplants)));
            Assert.IsTrue(ladderProcesses.All(p => p.OrderedFinalImplants.SequenceEqual(ladderProcesses[0].OrderedFinalImplants)));
        }

        [TestMethod]
        public void BasicLadderProcessIsAtLeastAsGoodAsOldApplications()
        {
            // For 282 the old one seems deterministic, unlike for 281.
            _character.IncreaseAbilityValue(Ability.Agility, 1);
            Assert.AreEqual(282, _character.GetAbilityValue(Ability.Agility));

            var ladderProcess = new BasicLadderProcess(_character, _finalImplantTemplates);

            Assert.IsTrue(164.55 <= ladderProcess.AverageFinalImplantQL);
            // Throw in a regression test, hopefully won't ever drop below this.
            Assert.AreEqual(166.72727272727272, ladderProcess.AverageFinalImplantQL);
        }

        [TestMethod]
        public void BasicLadderProcessBehaviorWhenHugeCharacterStats()
        {
            _character = new Character(
                agilityValue: 981, intelligenceValue: 915, psychicValue: 905,
                senseValue: 913, staminaValue: 901, strengthValue: 916, treatmentValue: 962.6250);

            var ladderProcess = new BasicLadderProcess(_character, _finalImplantTemplates);

            // First group supersedes second in this case, so it should be chosen. We're not worrying
            // about detecting when no ladder implants will help, so at least once always gets chosen.
            Assert.AreEqual(
                LadderImplantGroup.LadderImplantGroups[1].LadderImplantTemplates[0],
                ladderProcess.OrderedLadderImplants[0].ImplantTemplate);
            Assert.AreEqual(1, ladderProcess.OrderedLadderImplants.Count);
            Assert.AreEqual(200, ladderProcess.AverageFinalImplantQL);
            Assert.AreEqual(200 * 11, ladderProcess.TotalFinalImplantQL);
        }

        [TestMethod]
        public void BasicLadderProcessBehaviorWhenTinyCharacterStats()
        {
            _character = new Character(
                agilityValue: 1, intelligenceValue: 1, psychicValue: 1,
                senseValue: 1, staminaValue: 1, strengthValue: 1, treatmentValue: 1);

            var ladderProcess = new BasicLadderProcess(_character, _finalImplantTemplates);

            Assert.AreEqual(0, ladderProcess.TotalFinalImplantQL);
            Assert.AreEqual(0, ladderProcess.OrderedLadderImplants.Count);
            Assert.AreEqual(0, ladderProcess.OrderedFinalImplants.Count);
        }

        [TestMethod, Timeout(60000)]
        public void BasicLadderDoesntTakeForeverEvenWhenHighNumberOfFinalImplantsAsLadderImplants()
        {
            // If we try 9 or 10 ladder implants it's going take around a minute.
            _finalImplantTemplates = new[]
            {
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Head, Skill.TimeAndSpace, Skill.NanoPool, Ability.Sense),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Eye, Skill.AimedShot, Skill.SensoryImpr, Skill.TimeAndSpace),
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
            Assert.AreEqual(8, _finalImplantTemplates.Count(t => t.RaisesLadderStats));

            var ladderProcess = new BasicLadderProcess(_character, _finalImplantTemplates);
        }

        [TestMethod]
        public void BasicLadderProcessHandlesEmptyFinalImplantTemplates()
        {
            var ladderProcess = new BasicLadderProcess(_character, new ImplantTemplate[0]);
            Assert.AreEqual(0, ladderProcess.TotalFinalImplantQL);
            Assert.AreEqual(0, ladderProcess.AverageFinalImplantQL);
            Assert.AreEqual(0, ladderProcess.OrderedLadderImplants.Count);
            Assert.AreEqual(0, ladderProcess.OrderedFinalImplants.Count);
        }

        [TestMethod]
        public void BasicLadderProcessIsAtLeastAsGoodAsOldApplicationWhenNoInitialLimitingFactor()
        {
            // There's no limiting factor here, they have the exact stats required to equip a QL 100 left-arm.
            _character = new Character(
                agilityValue: 204, intelligenceValue: 204, psychicValue: 204,
                senseValue: 204, staminaValue: 204, strengthValue: 204, treatmentValue: 479);
            _finalImplantTemplates = new[]
            {
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftArm, Skill.Brawling, Skill.TwoHandBlunt, ArmorClass.ChemicalAC)
            };

            var ladderProcess = new BasicLadderProcess(_character, _finalImplantTemplates);

            // Make sure it's at least as good as the old ladderer. I wanted to document this behavior because it depends
            // upon something non-obvious, the order we've defined the ladder implant groups. If there's no limiting factor
            // the first non-superseded group is used to find the first ladder implant. So it's important that that implant
            // is a good one. The most obvious choice is a shiny treatment implant since treatment benefits every implant.
            // And so we've defined the ladder implants groups such that head groups adding to treatment come first.
            Assert.IsTrue(121 <= ladderProcess.AverageFinalImplantQL);
        }
    }
}
