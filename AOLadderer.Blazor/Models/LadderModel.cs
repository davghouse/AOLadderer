using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AOLadderer.Blazor.Models
{
    public class LadderStepModel
    {
        public LadderStepModel(Implant implant)
        {
            ImplantQL = implant.QL;
            ImplantSlot = implant.ImplantSlot;
            ShinyStat = implant.ShinyStat;
            BrightStat = implant.BrightStat;
            FadedStat = implant.FadedStat;
        }

        public bool IsCompleted { get; set; }
        public int ImplantQL { get; set; }
        public ImplantSlot ImplantSlot { get; set; }
        public Stat ShinyStat { get; set; }
        public Stat BrightStat { get; set; }
        public Stat FadedStat { get; set; }
    }

    public class LadderModel : IReadOnlyCollection<LadderStepModel>
    {
        private readonly IReadOnlyCollection<LadderStepModel> steps;

        public LadderModel(LadderProcess process)
        {
            steps = process.OrderedLadderImplants
                .Concat(process.OrderedFinalImplants)
                .Select(i => new LadderStepModel(i))
                .ToArray();
            AverageFinalImplantQL = process.AverageFinalImplantQL;
        }

        public double AverageFinalImplantQL { get; }

        public int Count => steps.Count;
        public IEnumerator<LadderStepModel> GetEnumerator() => steps.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => steps.GetEnumerator();
    }
}
