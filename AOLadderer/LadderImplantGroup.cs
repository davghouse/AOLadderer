using System.Collections.Generic;
using System.Linq;

namespace AOLadderer
{
    // See notes in LadderImplantGroups.txt.
    public class LadderImplantGroup
    {
        private LadderImplantGroup(params int[] implantTemplateIndexes)
        {
            LadderImplantTemplates = implantTemplateIndexes
                .Select(i => ImplantTemplate.ImplantTemplates[i])
                .ToArray();
            ImplantSlot = LadderImplantTemplates.First().ImplantSlot;
            RaisedLadderStats = LadderImplantTemplates.First().RaisedLadderStats;
        }

        public IReadOnlyList<ImplantTemplate> LadderImplantTemplates { get; }
        public ImplantSlot ImplantSlot { get; }
        public IReadOnlyList<Stat> RaisedLadderStats { get; }

        public static readonly IReadOnlyList<LadderImplantGroup> LadderImplantGroups = new[]
        {
            new LadderImplantGroup(263),
            new LadderImplantGroup(253, 259),
            new LadderImplantGroup(617),
            new LadderImplantGroup(613, 619),
            new LadderImplantGroup(223, 187),
            new LadderImplantGroup(869),
            new LadderImplantGroup(865, 871),
            new LadderImplantGroup(1030, 1948, 1036, 2176),
            new LadderImplantGroup(1162, 1168, 2308),
            new LadderImplantGroup(2344, 2356, 2338, 2362),
            new LadderImplantGroup(2345, 2339, 2363),
            new LadderImplantGroup(2436),
            new LadderImplantGroup(2445),
            new LadderImplantGroup(2746),
            new LadderImplantGroup(2786),
            new LadderImplantGroup(2795),
            new LadderImplantGroup(2755),
            new LadderImplantGroup(2796),
            new LadderImplantGroup(2836),
            new LadderImplantGroup(2845),
            new LadderImplantGroup(2805),
            new LadderImplantGroup(3247, 3229, 3236, 3235),
            new LadderImplantGroup(3280, 3281, 3313),
            new LadderImplantGroup(3365),
            new LadderImplantGroup(3427),
            new LadderImplantGroup(3429, 3557, 3685),
            new LadderImplantGroup(3442, 3570, 3698),
            new LadderImplantGroup(3378),
            new LadderImplantGroup(4422),
            new LadderImplantGroup(4432),
            new LadderImplantGroup(4496),
            new LadderImplantGroup(4685, 4541, 4621, 4589),
            new LadderImplantGroup(5388, 5389)
        };
    }
}
