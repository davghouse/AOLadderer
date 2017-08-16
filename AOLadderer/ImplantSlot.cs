using AOLadderer.ClusterTemplates;
using System.Collections.Generic;
using System.Linq;

namespace AOLadderer
{
    public class ImplantSlot
    {
        private ImplantSlot(string name)
            => Name = name;

        public string Name { get; }

        private IReadOnlyList<ShinyClusterTemplate> _shinyClusterTemplates;
        public IReadOnlyList<ShinyClusterTemplate> ShinyClusterTemplates => _shinyClusterTemplates
            ?? (_shinyClusterTemplates = ShinyClusterTemplate.ShinyClusterTemplates.Where(t => t.ImplantSlot == this).ToArray());

        private IReadOnlyList<BrightClusterTemplate> _brightClusterTemplates;
        public IReadOnlyList<BrightClusterTemplate> BrightClusterTemplates => _brightClusterTemplates
            ?? (_brightClusterTemplates = BrightClusterTemplate.BrightClusterTemplates.Where(t => t.ImplantSlot == this).ToArray());

        private IReadOnlyList<FadedClusterTemplate> _fadedClusterTemplates;
        public IReadOnlyList<FadedClusterTemplate> FadedClusterTemplates => _fadedClusterTemplates
            ?? (_fadedClusterTemplates = FadedClusterTemplate.FadedClusterTemplates.Where(t => t.ImplantSlot == this).ToArray());

        private IReadOnlyList<ImplantTemplate> _implantTemplates;
        public IReadOnlyList<ImplantTemplate> ImplantTemplates => _implantTemplates
            ?? (_implantTemplates = ImplantTemplate.ImplantTemplates.Where(t => t.ImplantSlot == this).ToArray());

        private IReadOnlyList<LadderImplantGroup> _ladderImplantGroups;
        public IReadOnlyList<LadderImplantGroup> LadderImplantGroups => _ladderImplantGroups
            ?? (_ladderImplantGroups = LadderImplantGroup.LadderImplantGroups.Where(t => t.ImplantSlot == this).ToArray());

        public override string ToString()
            => Name;

        public static readonly ImplantSlot
            Head = new ImplantSlot("Head"),
            Eye = new ImplantSlot("Eye"),
            Ear = new ImplantSlot("Ear"),
            Chest = new ImplantSlot("Chest"),
            RightArm = new ImplantSlot("Right-Arm"),
            LeftArm = new ImplantSlot("Left-Arm"),
            Waist = new ImplantSlot("Waist"),
            RightWrist = new ImplantSlot("Right-Wrist"),
            LeftWrist = new ImplantSlot("Left-Wrist"),
            Leg = new ImplantSlot("Leg"),
            RightHand = new ImplantSlot("Right-Hand"),
            LeftHand = new ImplantSlot("Left-Hand"),
            Feet = new ImplantSlot("Feet");

        public static readonly IReadOnlyList<ImplantSlot> ImplantSlots = new[] { Head, Eye, Ear, Chest, RightArm, LeftArm, Waist, RightWrist, LeftWrist, Leg, RightHand, LeftHand, Feet };
        public static readonly IReadOnlyList<ImplantSlot> LadderImplantSlots = new[] { Head, Eye, Ear, Chest, RightArm, LeftArm, Waist, Leg, RightHand, Feet };
    }
}
