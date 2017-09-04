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

            var finalImplantSlots = _finalImplantTemplates
                .Select(t => t.ImplantSlot)
                .ToArray();

            Assert.AreEqual(_character.GetTotalImplantQL(finalImplantSlots), ladderProcess.TotalFinalImplantQL);
            Assert.AreEqual(_character.GetAverageImplantQL(finalImplantSlots), ladderProcess.AverageFinalImplantQL);
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

            Assert.AreEqual(0, ladderProcess.OrderedLadderImplants.Count);
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

        [TestMethod, Timeout(1000)]
        public void AdvancedLadderIsFastWhenMaxNumberOfLadderImplantsButVeryHighStartingStats()
        {
            _character = new Character(
                agilityValue: 500, intelligenceValue: 500, psychicValue: 500,
                senseValue: 500, staminaValue: 500, strengthValue: 500, treatmentValue: 1000);

            _finalImplantTemplates = new[]
            {
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Head, Skill.TimeAndSpace, Skill.NanoPool, Ability.Sense),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Eye, Skill.AimedShot, Skill.Treatment, Skill.TimeAndSpace),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Ear, null, Skill.Concealment, Ability.Intelligence),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Chest, Ability.Stamina, Skill.BioMetamor, Skill.SensoryImpr),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightArm, Ability.Strength, null, null),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftArm, null, Ability.Strength, null),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Waist, ArmorClass.FireAC, MaxHealthOrNano.MaxHealth, Ability.Stamina),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightWrist, Skill.RunSpeed, Skill.NanoResist, Skill.MultMelee),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftWrist, Skill.MultMelee, Skill.RunSpeed, Skill.NanoResist),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Leg, Ability.Agility, Skill.EvadeClsC, ArmorClass.MeleeMaAC),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightHand, Skill.MartialArts, Skill.TimeAndSpace, Skill.Treatment),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftHand, null, ArmorClass.FireAC, Skill.MartialArts),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.Feet, Skill.EvadeClsC, Ability.Agility, Skill.DuckExp),
            };
            Assert.AreEqual(10, _finalImplantTemplates.Count(t => t.RaisesLadderStats));

            var ladderProcess = new AdvancedLadderProcess(_character, _finalImplantTemplates);

            Assert.AreEqual(200, ladderProcess.AverageFinalImplantQL);
        }

        [TestMethod]
        public void AdvancedLadderDoesntUseUnavailableImplantSlots()
        {
            _finalImplantTemplates = _finalImplantTemplates.Where(t => t.ImplantSlot != ImplantSlot.Head).ToArray();
            var fullLadderProcess = new AdvancedLadderProcess(_character, _finalImplantTemplates);
            var restrictedLadderProcess = new AdvancedLadderProcess(_character, _finalImplantTemplates, new[] { ImplantSlot.Head } );

            Assert.IsTrue(fullLadderProcess.OrderedLadderImplants.Any(i => i.ImplantSlot == ImplantSlot.Head));
            Assert.IsFalse(restrictedLadderProcess.OrderedLadderImplants.Any(i => i.ImplantSlot == ImplantSlot.Head));
            Assert.IsTrue(fullLadderProcess.AverageFinalImplantQL > restrictedLadderProcess.AverageFinalImplantQL);
            // Throw in a regression test, hopefully won't ever drop below this.
            Assert.AreEqual(170.5, fullLadderProcess.AverageFinalImplantQL);
            Assert.AreEqual(154.2, restrictedLadderProcess.AverageFinalImplantQL);
        }

        [TestMethod]
        public void AdvancedLadderUsesOneLadderImplantWhenTheresOnlyOneAvailable()
        {
            _finalImplantTemplates = new[]
            {
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightWrist, Skill.RunSpeed, Skill.NanoResist, Skill.MultMelee),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftWrist, Skill.MultMelee, Skill.RunSpeed, Skill.NanoResist),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftHand, null, ArmorClass.FireAC, Skill.MartialArts),
            };

            var advancedLadderProcess = new AdvancedLadderProcess(_character, _finalImplantTemplates,
                unavailableImplantSlots: ImplantSlot.LadderImplantSlots.Where(s => s != ImplantSlot.RightHand));

            Assert.AreEqual(ImplantSlot.RightHand, advancedLadderProcess.OrderedLadderImplants.Single().ImplantSlot);
            Assert.IsTrue(advancedLadderProcess.AverageFinalImplantQL > 138);
        }

        [TestMethod]
        public void AdvancedLadderUsesASingleLadderImplantIfThatsAllThatsNecessary()
        {
            _character = new Character(
                agilityValue: 410, intelligenceValue: 410, psychicValue: 410,
                senseValue: 410, staminaValue: 410, strengthValue: 410, treatmentValue: 907);

            var advancedLadderProcess = new AdvancedLadderProcess(_character, _finalImplantTemplates);

            Assert.AreEqual(ImplantSlot.Head, advancedLadderProcess.OrderedLadderImplants.Single().ImplantSlot);
            Assert.AreEqual(200, advancedLadderProcess.AverageFinalImplantQL);
        }

        [TestMethod]
        public void AdvancedLadderProcessUsesAFewLadderImplantsIfThatsAllThatsAvailable()
        {
            var availableImplantSlots = new[]
            {
                ImplantSlot.Head, ImplantSlot.RightHand, ImplantSlot.Leg,
                ImplantSlot.RightWrist, ImplantSlot.LeftWrist, ImplantSlot.LeftHand
            };

            _finalImplantTemplates = _finalImplantTemplates
                .Where(t => availableImplantSlots.Contains(t.ImplantSlot))
                .ToArray();

            var advancedLadderProcess = new AdvancedLadderProcess(_character, _finalImplantTemplates,
                unavailableImplantSlots: ImplantSlot.ImplantSlots.Except(availableImplantSlots));

            Assert.IsTrue(advancedLadderProcess.OrderedLadderImplants.All(i => availableImplantSlots.Contains(i.ImplantSlot)));
            Assert.IsTrue(advancedLadderProcess.OrderedFinalImplants.All(i => availableImplantSlots.Contains(i.ImplantSlot)));
            // Throw in a regression test, hopefully won't ever drop below this.
            Assert.AreEqual(929, advancedLadderProcess.TotalFinalImplantQL);
        }
    }
}
