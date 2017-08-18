using AOLadderer.LadderProcesses;
using AOLadderer.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AOLadderer.UnitTests.LadderProcesses
{
    [TestClass]
    public class AdvancedLadderProcessTests
    {
        private Character _character;
        private IReadOnlyList<ImplantTemplate> _finalImplantTemplates;
        private IReadOnlyList<ImplantSlot> _finalImplantSlots;

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
            _finalImplantSlots = _finalImplantTemplates
                .Select(t => t.ImplantSlot)
                .ToArray();
        }

        [TestMethod]
        public void AdvancedLadderProcessIsPossible()
        {
            var ladderProcess = new AdvancedLadderProcess(_character, _finalImplantTemplates);

            foreach (var ladderImplant in ladderProcess.OrderedLadderImplants)
            {
                Assert.IsTrue(_character.TryEquipImplant(ladderImplant));
            }

            foreach (var finalImplant in ladderProcess.OrderedFinalImplants)
            {
                Assert.IsTrue(_character.TryEquipImplant(finalImplant));
            }

            Assert.AreEqual(_character.GetTotalImplantQL(_finalImplantSlots), ladderProcess.TotalFinalImplantQL);
            Assert.AreEqual(_character.GetAverageImplantQL(_finalImplantSlots), ladderProcess.AverageFinalImplantQL);
        }

        [TestMethod]
        public void AdvancedLadderProcessLeavesCharacterInOriginalState()
        {
            var ladderProcess = new AdvancedLadderProcess(_character, _finalImplantTemplates);
            Assert.AreEqual(0, _character.GetTotalImplantQL());
            Assert.AreEqual(662.6250, _character.TreatmentValue);
            Assert.AreEqual(281, _character.GetAbilityValue(Ability.Agility));
        }

        [TestMethod]
        public void AdvancedLadderProcessIsDeterministicWhenOldApplicationIsnt()
        {
            var ladderProcesses = new[]
            {
                new AdvancedLadderProcess(_character, _finalImplantTemplates),
                new AdvancedLadderProcess(_character, _finalImplantTemplates),
                new AdvancedLadderProcess(_character, _finalImplantTemplates),
                new AdvancedLadderProcess(_character, _finalImplantTemplates),
                new AdvancedLadderProcess(_character, _finalImplantTemplates)
            };

            Assert.IsTrue(ladderProcesses.All(p => p.TotalFinalImplantQL == ladderProcesses[0].TotalFinalImplantQL));
            Assert.IsTrue(ladderProcesses.All(p => p.OrderedLadderImplants.SequenceEqual(ladderProcesses[0].OrderedLadderImplants)));
            Assert.IsTrue(ladderProcesses.All(p => p.OrderedFinalImplants.SequenceEqual(ladderProcesses[0].OrderedFinalImplants)));
        }

        [TestMethod]
        public void AdvancedLadderProcessIsBetterThanBasic()
        {
            var basicLadderProcess = new BasicLadderProcess(_character, _finalImplantTemplates);
            var advancedLadderProcess = new AdvancedLadderProcess(_character, _finalImplantTemplates);
            Assert.IsTrue(basicLadderProcess.AverageFinalImplantQL < advancedLadderProcess.AverageFinalImplantQL);
        }

        [TestMethod]
        public void AdvancedLadderProcessIsAtLeastAsGoodAsOldApplications()
        {
            // To be consistent w/ other test for basic ladder process.
            _character.IncreaseAbilityValue(Ability.Agility, 1);
            Assert.AreEqual(282, _character.GetAbilityValue(Ability.Agility));

            var ladderProcess = new AdvancedLadderProcess(_character, _finalImplantTemplates);

            Assert.IsTrue(167 <= ladderProcess.AverageFinalImplantQL);
            // Throw in a regression test, hopefully won't ever drop below this.
            Assert.AreEqual(168.54545454545453, ladderProcess.AverageFinalImplantQL);
        }

        [TestMethod]
        public void AdvancedLadderProcessBehaviorWhenHugeCharacterStats()
        {
            _character = new Character(
                agilityValue: 981, intelligenceValue: 915, psychicValue: 905,
                senseValue: 913, staminaValue: 901, strengthValue: 916, treatmentValue: 962.6250);

            var ladderProcess = new AdvancedLadderProcess(_character, _finalImplantTemplates);

            // First group supersedes second in this case, so it should be chosen. We're not worrying
            // about detecting when no ladder implants will help, so at least once always gets chosen.
            CollectionAssert.AreEquivalent(
                new[]
                {
                    LadderImplantGroup.LadderImplantGroups[1].LadderImplantTemplates[0],
                    LadderImplantGroup.LadderImplantGroups[1].LadderImplantTemplates[0]
                },
                ladderProcess.OrderedLadderImplants.Select(i => i.ImplantTemplate).ToArray());
            Assert.AreEqual(2, ladderProcess.OrderedLadderImplants.Count);
            Assert.AreEqual(200, ladderProcess.AverageFinalImplantQL);
            Assert.AreEqual(200 * 11, ladderProcess.TotalFinalImplantQL);
        }

        [TestMethod]
        public void AdvancedLadderProcessBehaviorWhenTinyCharacterStats()
        {
            _character = new Character(
                agilityValue: 1, intelligenceValue: 1, psychicValue: 1,
                senseValue: 1, staminaValue: 1, strengthValue: 1, treatmentValue: 1);

            var ladderProcess = new AdvancedLadderProcess(_character, _finalImplantTemplates);

            Assert.AreEqual(0, ladderProcess.TotalFinalImplantQL);
            Assert.AreEqual(0, ladderProcess.OrderedLadderImplants.Count);
            Assert.AreEqual(0, ladderProcess.OrderedFinalImplants.Count);
        }

        [TestMethod]
        public void AdvancedLadderProcessHandlesEmptyFinalImplantTemplates()
        {
            var ladderProcess = new AdvancedLadderProcess(_character, new ImplantTemplate[0]);
            Assert.AreEqual(0, ladderProcess.TotalFinalImplantQL);
            Assert.AreEqual(0, ladderProcess.AverageFinalImplantQL);
            Assert.AreEqual(0, ladderProcess.OrderedLadderImplants.Count);
            Assert.AreEqual(0, ladderProcess.OrderedFinalImplants.Count);
        }
    }
}
