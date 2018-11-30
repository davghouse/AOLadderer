using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AOLadderer.Blazor.Models
{
    public class LadderModel : IReadOnlyCollection<LadderModel.StepModel>
    {
        public class StepModel
        {
            public StepModel(Implant implant)
            {
                ImplantQL = implant.QL;
                ImplantSlot = implant.ImplantSlot?.Name;
                ShinyStat = implant.ShinyStat?.Name;
                BrightStat = implant.BrightStat?.Name;
                FadedStat = implant.FadedStat?.Name;
            }

            public int ImplantQL { get; }
            public string ImplantSlot { get; }
            public string ShinyStat { get; }
            public string BrightStat { get; }
            public string FadedStat { get; }
            public bool IsChecked { get; set; }
        }

        private readonly IReadOnlyCollection<StepModel> steps;

        public LadderModel(LadderProcess ladder)
        {
            steps = ladder.OrderedLadderImplants
                .Concat(ladder.OrderedFinalImplants)
                .Select(i => new StepModel(i))
                .ToArray();
            AverageFinalImplantQL = ladder.AverageFinalImplantQL;
        }

        public double AverageFinalImplantQL { get; }

        public int Count => steps.Count;
        public IEnumerator<StepModel> GetEnumerator() => steps.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => steps.GetEnumerator();
    }
}
