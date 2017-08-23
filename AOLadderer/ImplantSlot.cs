using AOLadderer.ClusterTemplates;
using System.Collections.Generic;
using System.Linq;

namespace AOLadderer
{
    public class ImplantSlot
    {
        private ImplantSlot(string name, int aunoSlotID)
        {
            Name = name;
            ShortName = name.Replace("Right", "R").Replace("Left", "L");
            AunoSlotID = aunoSlotID;
        }

        public string Name { get; }
        public string ShortName { get; }
        public int AunoSlotID { get; }

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
            Head = new ImplantSlot("Head", 4),
            Eye = new ImplantSlot("Eye", 2),
            Ear = new ImplantSlot("Ear", 8),
            Chest = new ImplantSlot("Chest", 32),
            RightArm = new ImplantSlot("Right-Arm", 16),
            LeftArm = new ImplantSlot("Left-Arm", 64),
            Waist = new ImplantSlot("Waist", 256),
            RightWrist = new ImplantSlot("Right-Wrist", 128),
            LeftWrist = new ImplantSlot("Left-Wrist", 512),
            Leg = new ImplantSlot("Leg", 2048),
            RightHand = new ImplantSlot("Right-Hand", 1024),
            LeftHand = new ImplantSlot("Left-Hand", 4096),
            Feet = new ImplantSlot("Feet", 8192);

        public static readonly IReadOnlyList<ImplantSlot> ImplantSlots = new[] { Head, Eye, Ear, Chest, RightArm, LeftArm, Waist, RightWrist, LeftWrist, Leg, RightHand, LeftHand, Feet };
        public static readonly IReadOnlyList<ImplantSlot> LadderImplantSlots = new[] { Head, Eye, Ear, Chest, RightArm, LeftArm, Waist, Leg, RightHand, Feet };
    }
}
