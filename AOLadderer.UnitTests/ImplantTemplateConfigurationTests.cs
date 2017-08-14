using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AOLadderer.UnitTests
{
    [TestClass]
    public class ImplantTemplateConfigurationTests
    {
        [TestMethod]
        public void SetsAndRemovesFromImplantTemplateConfiguration()
        {
            var implantTemplates = new[]
            {
                ImplantSlot.Head.ImplantTemplates.First(),
                ImplantSlot.Leg.ImplantTemplates.First(),
                ImplantSlot.RightHand.ImplantTemplates.First()
            };
            var implantTemplateConfiguration = new ImplantTemplateConfiguration(implantTemplates);

            Assert.IsTrue(implantTemplateConfiguration.IsImplantSlotEmpty(ImplantSlot.Eye));
            Assert.IsTrue(implantTemplateConfiguration.IsImplantSlotFull(ImplantSlot.Head));
            Assert.IsTrue(implantTemplateConfiguration.IsImplantSlotEmpty(ImplantSlot.LeftHand));
            Assert.IsTrue(implantTemplateConfiguration.IsImplantSlotFull(ImplantSlot.RightHand));

            implantTemplateConfiguration.RemoveImplantTemplate(ImplantSlot.LeftHand);
            implantTemplateConfiguration.RemoveImplantTemplate(ImplantSlot.RightHand);

            Assert.IsTrue(implantTemplateConfiguration.IsImplantSlotEmpty(ImplantSlot.Eye));
            Assert.IsTrue(implantTemplateConfiguration.IsImplantSlotFull(ImplantSlot.Head));
            Assert.IsTrue(implantTemplateConfiguration.IsImplantSlotEmpty(ImplantSlot.LeftHand));
            Assert.IsTrue(implantTemplateConfiguration.IsImplantSlotEmpty(ImplantSlot.RightHand));
        }
    }
}
