using System.Collections.Generic;
using System.Linq;

namespace AOLadderer.Blazor.Models
{
    public class ShoppingModel
    {
        public class ClusterModel
        {
            public ClusterModel(int minimumQL, Stat stat)
            {
                MinimumQL = minimumQL;
                Stat = stat.Name;
            }

            public int MinimumQL { get; }
            public string Stat { get; }
            public bool IsChecked { get; set; }
        }

        public ShoppingModel(LadderProcess ladder)
        {
            ShinyClusters = ladder.OrderedLadderImplants
                .Concat(ladder.OrderedFinalImplants)
                .Where(i => i.ShinyClusterTemplate != null)
                .Select(i => new ClusterModel(i.MinimumShinyClusterQL.Value, i.ShinyStat))
                .OrderBy(c => c.Stat).ThenBy(c => c.MinimumQL)
                .ToArray();
            BrightClusters = ladder.OrderedLadderImplants
                .Concat(ladder.OrderedFinalImplants)
                .Where(i => i.BrightClusterTemplate != null)
                .Select(i => new ClusterModel(i.MinimumBrightClusterQL.Value, i.BrightStat))
                .OrderBy(c => c.Stat).ThenBy(c => c.MinimumQL)
                .ToArray();
            FadedClusters = ladder.OrderedLadderImplants
                .Concat(ladder.OrderedFinalImplants)
                .Where(i => i.FadedClusterTemplate != null)
                .Select(i => new ClusterModel(i.MinimumFadedClusterQL.Value, i.FadedStat))
                .OrderBy(c => c.Stat).ThenBy(c => c.MinimumQL)
                .ToArray();
        }

        public IReadOnlyCollection<ClusterModel> ShinyClusters { get; }
        public IReadOnlyCollection<ClusterModel> BrightClusters { get; }
        public IReadOnlyCollection<ClusterModel> FadedClusters { get; }
    }
}
