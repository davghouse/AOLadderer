using System.Collections.Generic;
using System.Linq;

namespace AOLadderer.UI.ViewModels
{
    public class LadderViewModel : ViewModelBase
    {
        private LadderProcess _ladderProcess;
        public LadderProcess LadderProcess
        {
            get => _ladderProcess;
            set
            {
                if (Set(ref _ladderProcess, value))
                {
                    LadderStepViewModels = LadderProcess.OrderedLadderImplants
                            .Select(i => new LadderStepViewModel(i))
                        .Concat(LadderProcess.OrderedFinalImplants
                            .Select(i => new LadderStepViewModel(i, isFinalImplant: true)))
                        .ToArray();
                    AverageFinalImplantQL = LadderProcess.AverageFinalImplantQL;
                }
            }
        }

        private IReadOnlyList<LadderStepViewModel> _ladderStepViewModels = new List<LadderStepViewModel>();
        public IReadOnlyList<LadderStepViewModel> LadderStepViewModels
        {
            get => _ladderStepViewModels;
            set => Set(ref _ladderStepViewModels, value);
        }

        private double? _averageFinalImplantQL;
        public double? AverageFinalImplantQL
        {
            get => _averageFinalImplantQL;
            set => Set(ref _averageFinalImplantQL, value);
        }
    }
}
