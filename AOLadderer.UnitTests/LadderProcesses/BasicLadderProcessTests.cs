using AOLadderer.LadderProcesses;
using AOLadderer.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
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

            Assert.AreEqual(0, ladderProcess.OrderedLadderImplants.Count);
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

            Assert.IsTrue(121 <= ladderProcess.AverageFinalImplantQL);
        }

        [TestMethod, Timeout(1000)]
        public void BasicLadderIsFastWhenMaxNumberOfLadderImplantsButVeryHighStartingStats()
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

            var ladderProcess = new BasicLadderProcess(_character, _finalImplantTemplates);

            Assert.AreEqual(200, ladderProcess.AverageFinalImplantQL);
        }

        [TestMethod]
        public void BasicLadderDoesntUseUnavailableImplantSlots()
        {
            _finalImplantTemplates = _finalImplantTemplates.Where(t => t.ImplantSlot != ImplantSlot.Head).ToArray();
            var fullLadderProcess = new BasicLadderProcess(_character, _finalImplantTemplates);
            var restrictedLadderProcess = new BasicLadderProcess(_character, _finalImplantTemplates, new[] { ImplantSlot.Head } );

            Assert.IsTrue(fullLadderProcess.OrderedLadderImplants.Any(i => i.ImplantSlot == ImplantSlot.Head));
            Assert.IsFalse(restrictedLadderProcess.OrderedLadderImplants.Any(i => i.ImplantSlot == ImplantSlot.Head));
            Assert.IsTrue(fullLadderProcess.AverageFinalImplantQL > restrictedLadderProcess.AverageFinalImplantQL);
            // Throw in a regression test, hopefully won't ever drop below this.
            Assert.AreEqual(168.6, fullLadderProcess.AverageFinalImplantQL);
            Assert.AreEqual(153.4, restrictedLadderProcess.AverageFinalImplantQL);
        }

        [TestMethod]
        public void BasicLadderUsesOneLadderImplantWhenTheresOnlyOneAvailable()
        {
            _finalImplantTemplates = new[]
            {
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightWrist, Skill.RunSpeed, Skill.NanoResist, Skill.MultMelee),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftWrist, Skill.MultMelee, Skill.RunSpeed, Skill.NanoResist),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftHand, null, ArmorClass.FireAC, Skill.MartialArts),
            };

            var basicLadderProcess = new BasicLadderProcess(_character, _finalImplantTemplates,
                unavailableImplantSlots: ImplantSlot.LadderImplantSlots.Where(s => s != ImplantSlot.RightHand));

            Assert.AreEqual(ImplantSlot.RightHand, basicLadderProcess.OrderedLadderImplants.Single().ImplantSlot);
            Assert.IsTrue(basicLadderProcess.AverageFinalImplantQL > 138);
        }

        [TestMethod]
        public void BasicLadderUsesASingleLadderImplantIfThatsAllThatsNecessary()
        {
            _character = new Character(
                agilityValue: 410, intelligenceValue: 410, psychicValue: 410,
                senseValue: 410, staminaValue: 410, strengthValue: 410, treatmentValue: 907);

            var basicLadderProcess = new BasicLadderProcess(_character, _finalImplantTemplates);

            Assert.AreEqual(ImplantSlot.Head, basicLadderProcess.OrderedLadderImplants.Single().ImplantSlot);
            Assert.AreEqual(200, basicLadderProcess.AverageFinalImplantQL);
        }

        [TestMethod]
        public void BasicLadderProcessUsesAFewLadderImplantsIfThatsAllThatsAvailable()
        {
            var availableImplantSlots = new[]
            {
                ImplantSlot.Head, ImplantSlot.RightHand, ImplantSlot.Leg,
                ImplantSlot.RightWrist, ImplantSlot.LeftWrist, ImplantSlot.LeftHand
            };

            _finalImplantTemplates = _finalImplantTemplates
                .Where(t => availableImplantSlots.Contains(t.ImplantSlot))
                .ToArray();

            var basicLadderProcess = new BasicLadderProcess(_character, _finalImplantTemplates,
                unavailableImplantSlots: ImplantSlot.ImplantSlots.Except(availableImplantSlots));

            Assert.IsTrue(basicLadderProcess.OrderedLadderImplants.All(i => availableImplantSlots.Contains(i.ImplantSlot)));
            Assert.IsTrue(basicLadderProcess.OrderedFinalImplants.All(i => availableImplantSlots.Contains(i.ImplantSlot)));
            // Throw in a regression test, hopefully won't ever drop below this.
            Assert.AreEqual(928, basicLadderProcess.TotalFinalImplantQL);
        }
    }
}
