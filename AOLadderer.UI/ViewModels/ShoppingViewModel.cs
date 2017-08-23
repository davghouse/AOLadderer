using System.Collections.Generic;
using System.Linq;

namespace AOLadderer.UI.ViewModels
{
    public class ShoppingViewModel : ViewModelBase
    {
        private LadderProcess _ladderProcess;
        public LadderProcess LadderProcess
        {
            get => _ladderProcess;
            set
            {
                if (Set(ref _ladderProcess, value))
                {
                    ShinyClusterPurchases = _ladderProcess.OrderedLadderImplants
                        .Concat(_ladderProcess.OrderedFinalImplants)
                        .Where(i => i.ShinyClusterTemplate != null)
                        .Select(i => new ClusterPurchaseViewModel(i.ShinyClusterTemplate, i.MinimumShinyClusterQL.Value))
                        .OrderBy(p => p.Stat.Name)
                        .ThenBy(p => p.MinimumClusterQL)
                        .ToArray();
                    BrightClusterPurchases = _ladderProcess.OrderedLadderImplants
                        .Concat(_ladderProcess.OrderedFinalImplants)
                        .Where(i => i.BrightClusterTemplate != null)
                        .Select(i => new ClusterPurchaseViewModel(i.BrightClusterTemplate, i.MinimumBrightClusterQL.Value))
                        .OrderBy(p => p.Stat.Name)
                        .ThenBy(p => p.MinimumClusterQL)
                        .ToArray();
                    FadedClusterPurchases = _ladderProcess.OrderedLadderImplants
                        .Concat(_ladderProcess.OrderedFinalImplants)
                        .Where(i => i.FadedClusterTemplate != null)
                        .Select(i => new ClusterPurchaseViewModel(i.FadedClusterTemplate, i.MinimumFadedClusterQL.Value))
                        .OrderBy(p => p.Stat.Name)
                        .ThenBy(p => p.MinimumClusterQL)
                        .ToArray();
                }
            }
        }

        private IReadOnlyList<ClusterPurchaseViewModel> _shinyClusterPurchases = new List<ClusterPurchaseViewModel>();
        public IReadOnlyList<ClusterPurchaseViewModel> ShinyClusterPurchases
        {
            get => _shinyClusterPurchases;
            set => Set(ref _shinyClusterPurchases, value);
        }

        private IReadOnlyList<ClusterPurchaseViewModel> _brightClusterPurchases = new List<ClusterPurchaseViewModel>();
        public IReadOnlyList<ClusterPurchaseViewModel> BrightClusterPurchases
        {
            get => _brightClusterPurchases;
            set => Set(ref _brightClusterPurchases, value);
        }

        private IReadOnlyList<ClusterPurchaseViewModel> _fadedClusterPurchases = new List<ClusterPurchaseViewModel>();
        public IReadOnlyList<ClusterPurchaseViewModel> FadedClusterPurchases
        {
            get => _fadedClusterPurchases;
            set => Set(ref _fadedClusterPurchases, value);
        }
    }
}
