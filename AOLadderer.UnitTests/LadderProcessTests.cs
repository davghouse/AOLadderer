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
            Assert.AreEqual(0, LadderProcess.GetAllPossibleEquipOrders(implantTemplates).ToArray().Length);
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
    }
}
