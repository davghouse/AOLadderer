using AOLadderer.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AOLadderer.UnitTests
{
    [TestClass]
    public class LadderImplantGroupTests
    {
        [TestMethod]
        public void VerifiesSomeLadderImplantGroups()
        {
            CollectionAssert.AreEquivalent(
                new[] { Ability.Agility, Ability.Intelligence, Ability.Sense, Ability.Stamina },
                ImplantSlot.RightHand.LadderImplantGroups.Single().RequiredAbilityOptions.ToArray());
            Assert.AreEqual(
                Skill.Treatment, ImplantSlot.RightHand.LadderImplantGroups.Single().RaisedLadderStats.Single());

            CollectionAssert.AreEqual(
                new[] { Ability.Sense, Ability.Stamina },
                ImplantSlot.Waist.LadderImplantGroups[3].RaisedLadderStats.ToArray());

            CollectionAssert.AreEquivalent(
                new[] { Ability.Stamina, Ability.Sense, Ability.Strength },
                ImplantSlot.LeftArm.LadderImplantGroups.Single().RequiredAbilityOptions.ToArray());
        }

        [TestMethod]
        public void VerifiesSomeLadderImplantGroupsWithSupersedingLadderStats()
        {
            // Head group adding to Psychic has its ladder stats superseded by group adding to Psychic and Sense.
            Assert.AreEqual(
                LadderImplantGroup.LadderImplantGroups[2].LadderImplantGroupsWithSupersedingLadderStats.Single(),
                LadderImplantGroup.LadderImplantGroups[3]);

            // Head group adding to Treatment has its ladder stats superseded by group adding to Treatment and Sense.
            Assert.AreEqual(
                LadderImplantGroup.LadderImplantGroups[5].LadderImplantGroupsWithSupersedingLadderStats.Single(),
                LadderImplantGroup.LadderImplantGroups[6]);

            // Eye group adding to Intelligence has its ladder stats superseded by no other groups.
            Assert.IsFalse(LadderImplantGroup.LadderImplantGroups[7].LadderImplantGroupsWithSupersedingLadderStats.Any());

            // Leg group adding to Stamina has its ladder stats superseded by group adding to Agility and Stamina.
            Assert.AreEqual(
                ImplantSlot.Leg.LadderImplantGroups[2].LadderImplantGroupsWithSupersedingLadderStats.Single(),
                ImplantSlot.Leg.LadderImplantGroups[1]);
        }
    }
}
