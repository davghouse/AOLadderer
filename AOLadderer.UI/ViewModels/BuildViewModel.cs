using AOLadderer.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace AOLadderer.UI.ViewModels
{
    public class BuildViewModel : ViewModelBase
    {
        public class ClusterOption
        {
            public ClusterOption(Stat stat = null)
                => Stat = stat;

            public Stat Stat { get; }

            public override string ToString()
                => Stat?.Name;
        }

        public IReadOnlyList<ClusterOption> ShinyHeadClusterOptions { get; } = GetClusterOptions(ImplantSlot.Head.ShinyClusterTemplates);
        private ClusterOption _selectedShinyHeadClusterOption;
        public ClusterOption SelectedShinyHeadClusterOption
        {
            get => _selectedShinyHeadClusterOption;
            set => Set(ref _selectedShinyHeadClusterOption, value);
        }

        public IReadOnlyList<ClusterOption> BrightHeadClusterOptions { get; } = GetClusterOptions(ImplantSlot.Head.ShinyClusterTemplates);
        public IReadOnlyList<ClusterOption> FadedHeadClusterOptions { get; } = GetClusterOptions(ImplantSlot.Head.ShinyClusterTemplates);

        private static IReadOnlyList<ClusterOption> GetClusterOptions(IEnumerable<ClusterTemplate> clusterTemplates)
            => clusterTemplates
            .Select(t => new ClusterOption(t.Stat))
            .Prepend(new ClusterOption())
            .ToArray();
    }

}
