using AOLadderer.LadderProcesses;
using AOLadderer.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace AOLadderer.UnitTests
{
    [TestClass]
    public class LadderProcessTests
    {
        [TestMethod]
        public void CountsAllPossibleEquipOrdersForZeroImplantTemplates()
        {
            var implantTemplates = new ImplantTemplate[0];
            Assert.AreEqual(1, LadderProcess.GetAllPossibleEquipOrders(implantTemplates).ToArray().Length);
        }

        [TestMethod]
        public void CountsAllPossibleEquipOrdersForOneImplantTemplate()
        {
            var implantTemplates = new[]
            {
                ImplantSlot.Head.ImplantTemplates.First()
            };
            Assert.AreEqual(1, LadderProcess.GetAllPossibleEquipOrders(implantTemplates).ToArray().Length);
        }

        [TestMethod]
        public void CountsAllPossibleEquipOrdersForFourImplantTemplates()
        {
            var implantTemplates = new[]
            {
                ImplantSlot.Head.ImplantTemplates.First(),
                ImplantSlot.Eye.ImplantTemplates.First(),
                ImplantSlot.Ear.ImplantTemplates.First(),
                ImplantSlot.Chest.ImplantTemplates.First()
            };
            Assert.AreEqual(24, LadderProcess.GetAllPossibleEquipOrders(implantTemplates).ToArray().Length);
        }

        [TestMethod]
        public void VerifiesAllPossibleEquipOrders()
        {
            var equipOrders = new[]
            {
                new[]
                {
                    ImplantSlot.Head.ImplantTemplates.First(),
                    ImplantSlot.Eye.ImplantTemplates.First(),
                    ImplantSlot.Ear.ImplantTemplates.First(),
                },
                new[]
                {
                    ImplantSlot.Head.ImplantTemplates.First(),
                    ImplantSlot.Ear.ImplantTemplates.First(),
                    ImplantSlot.Eye.ImplantTemplates.First(),
                },
                new[]
                {
                    ImplantSlot.Ear.ImplantTemplates.First(),
                    ImplantSlot.Head.ImplantTemplates.First(),
                    ImplantSlot.Eye.ImplantTemplates.First(),
                },
                new[]
                {
                    ImplantSlot.Ear.ImplantTemplates.First(),
                    ImplantSlot.Eye.ImplantTemplates.First(),
                    ImplantSlot.Head.ImplantTemplates.First(),
                },
                new[]
                {
                    ImplantSlot.Eye.ImplantTemplates.First(),
                    ImplantSlot.Ear.ImplantTemplates.First(),
                    ImplantSlot.Head.ImplantTemplates.First(),
                },
                new[]
                {
                    ImplantSlot.Eye.ImplantTemplates.First(),
                    ImplantSlot.Head.ImplantTemplates.First(),
                    ImplantSlot.Ear.ImplantTemplates.First(),
                }
            };

            var matchedEquipOrderIndexes = new HashSet<int>();
            foreach (var equipOrder in LadderProcess.GetAllPossibleEquipOrders(new[]
                {
                    ImplantSlot.Eye.ImplantTemplates.First(),
                    ImplantSlot.Head.ImplantTemplates.First(),
                    ImplantSlot.Ear.ImplantTemplates.First(),
                }))
            {
                for (int i = 0; i < equipOrders.Length; ++i)
                {
                    if (equipOrders[i].SequenceEqual(equipOrder))
                    {
                        Assert.IsTrue(matchedEquipOrderIndexes.Add(i));
                    }
                }
            }

            Assert.AreEqual(6, matchedEquipOrderIndexes.Count);
        }

        [TestMethod]
        public void AllLadderProcessesAreEquivalentWhenNoLadderSlotsAreAvailable()
        {
            var character = new Character(
                agilityValue: 281, intelligenceValue: 315, psychicValue: 305,
                senseValue: 213, staminaValue: 301, strengthValue: 316, treatmentValue: 662.6250);
            var finalImplantTemplates = new[]
            {
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightWrist, Skill.RunSpeed, Skill.NanoResist, Skill.MultMelee),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftWrist, Skill.MultMelee, Skill.RunSpeed, Skill.NanoResist),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftHand, null, ArmorClass.FireAC, Skill.MartialArts),
            };

            var degenerateLadderProcess = new DegenerateLadderProcess(character, finalImplantTemplates);
            var basicLadderProcess = new BasicLadderProcess(character, finalImplantTemplates,
                unavailableImplantSlots: ImplantSlot.LadderImplantSlots);
            var advancedLadderProcess = new AdvancedLadderProcess(character, finalImplantTemplates,
                unavailableImplantSlots: ImplantSlot.LadderImplantSlots);

            CollectionAssert.AreEquivalent(
                degenerateLadderProcess.OrderedFinalImplants.ToArray(),
                basicLadderProcess.OrderedFinalImplants.ToArray());
            CollectionAssert.AreEquivalent(
                basicLadderProcess.OrderedFinalImplants.ToArray(),
                advancedLadderProcess.OrderedFinalImplants.ToArray());
            CollectionAssert.AreEquivalent(
                degenerateLadderProcess.OrderedLadderImplants.ToArray(),
                basicLadderProcess.OrderedLadderImplants.ToArray());
            CollectionAssert.AreEquivalent(
                basicLadderProcess.OrderedLadderImplants.ToArray(),
                advancedLadderProcess.OrderedLadderImplants.ToArray());
            Assert.AreEqual(138, degenerateLadderProcess.AverageFinalImplantQL);
            Assert.AreEqual(138, basicLadderProcess.AverageFinalImplantQL);
            Assert.AreEqual(138, advancedLadderProcess.AverageFinalImplantQL);
        }

        [TestMethod]
        public void AllLadderProcessesAreEquivalentWhenTheOnlyLadderSlotsAvailableArentUseful()
        {
            var character = new Character(
                agilityValue: 281, intelligenceValue: 315, psychicValue: 305,
                senseValue: 213, staminaValue: 301, strengthValue: 380, treatmentValue: 662.6250);
            var finalImplantTemplates = new[]
            {
                ImplantTemplate.GetImplantTemplate(ImplantSlot.RightWrist, Skill.RunSpeed, Skill.NanoResist, Skill.MultMelee),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftWrist, Skill.MultMelee, Skill.RunSpeed, Skill.NanoResist),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftHand, null, ArmorClass.FireAC, Skill.MartialArts),
                ImplantTemplate.GetImplantTemplate(ImplantSlot.LeftArm, Skill.Brawling, Skill.TwoHandBlunt, null),
            };

            // Right-arm and left-arm can only buff strength, which doesn't trickle to treatment, so it's not useful.
            var unavailableImplantSlots = ImplantSlot.LadderImplantSlots
                .Where(s => s != ImplantSlot.RightArm && s != ImplantSlot.LeftArm);

            var degenerateLadderProcess = new DegenerateLadderProcess(character, finalImplantTemplates);
            var basicLadderProcess = new BasicLadderProcess(character, finalImplantTemplates, unavailableImplantSlots);
            var advancedLadderProcess = new AdvancedLadderProcess(character, finalImplantTemplates, unavailableImplantSlots);

            CollectionAssert.AreEquivalent(
                degenerateLadderProcess.OrderedFinalImplants.ToArray(),
                basicLadderProcess.OrderedFinalImplants.ToArray());
            CollectionAssert.AreEquivalent(
                basicLadderProcess.OrderedFinalImplants.ToArray(),
                advancedLadderProcess.OrderedFinalImplants.ToArray());
            CollectionAssert.AreEquivalent(
                degenerateLadderProcess.OrderedLadderImplants.ToArray(),
                basicLadderProcess.OrderedLadderImplants.ToArray());
            CollectionAssert.AreEquivalent(
                basicLadderProcess.OrderedLadderImplants.ToArray(),
                advancedLadderProcess.OrderedLadderImplants.ToArray());
            Assert.AreEqual(138, degenerateLadderProcess.AverageFinalImplantQL);
            Assert.AreEqual(138, basicLadderProcess.AverageFinalImplantQL);
            Assert.AreEqual(138, advancedLadderProcess.AverageFinalImplantQL);
        }
    }
}
