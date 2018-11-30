using AOLadderer.UI.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace AOLadderer.UI.ViewModels
{
    public class ImplantConfigurationViewModel : ViewModelBase
    {
        public ImplantConfigurationViewModel(ImplantSlot implantSlot)
        {
            ImplantSlot = implantSlot;
            ShinyClusterOptions = implantSlot.ShinyClusterTemplates
                .Select(t => new ClusterOption(t.Stat))
                .Prepend(new ClusterOption())
                .ToArray();
            BrightClusterOptions = implantSlot.BrightClusterTemplates
                .Select(t => new ClusterOption(t.Stat))
                .Prepend(new ClusterOption())
                .ToArray();
            FadedClusterOptions = implantSlot.FadedClusterTemplates
                .Select(t => new ClusterOption(t.Stat))
                .Prepend(new ClusterOption())
                .ToArray();
        }

        public ImplantSlot ImplantSlot { get; }
        public IReadOnlyList<ClusterOption> ShinyClusterOptions { get; }
        public IReadOnlyList<ClusterOption> BrightClusterOptions { get; }
        public IReadOnlyList<ClusterOption> FadedClusterOptions { get; }

        private ClusterOption _selectedShinyClusterOption;
        public ClusterOption SelectedShinyClusterOption
        {
            get => _selectedShinyClusterOption;
            set
            {
                if (Set(ref _selectedShinyClusterOption, value))
                {
                    RaisePropertyChanged(nameof(AbiSlotDescription));
                }
            }
        }

        private ClusterOption _selectedBrightClusterOption;
        public ClusterOption SelectedBrightClusterOption
        {
            get => _selectedBrightClusterOption;
            set
            {
                if (Set(ref _selectedBrightClusterOption, value))
                {
                    RaisePropertyChanged(nameof(AbiSlotDescription));
                }
            }
        }

        private ClusterOption _selectedFadedClusterOption;
        public ClusterOption SelectedFadedClusterOption
        {
            get => _selectedFadedClusterOption;
            set
            {
                if (Set(ref _selectedFadedClusterOption, value))
                {
                    RaisePropertyChanged(nameof(AbiSlotDescription));
                }
            }
        }

        private bool _isImplantSlotAvailable = true;
        public bool IsImplantSlotAvailable
        {
            get => _isImplantSlotAvailable;
            set => Set(ref _isImplantSlotAvailable, value);
        }

        public bool IsEmpty
            => (SelectedShinyClusterOption?.IsEmpty ?? true)
            && (SelectedBrightClusterOption?.IsEmpty ?? true)
            && (SelectedFadedClusterOption?.IsEmpty ?? true);

        public Stat ShinyStat => SelectedShinyClusterOption?.Stat;
        public Stat BrightStat => SelectedBrightClusterOption?.Stat;
        public Stat FadedStat => SelectedFadedClusterOption?.Stat;

        public ImplantTemplate GetImplantTemplate()
            => IsEmpty ? null : ImplantTemplate.GetImplantTemplate(ImplantSlot, ShinyStat, BrightStat, FadedStat);

        public void SetImplantTemplate(ImplantTemplate implantTemplate)
        {
            SelectedShinyClusterOption = ShinyClusterOptions.Single(o => o.Stat == implantTemplate?.ShinyStat);
            SelectedBrightClusterOption = BrightClusterOptions.Single(o => o.Stat == implantTemplate?.BrightStat);
            SelectedFadedClusterOption = FadedClusterOptions.Single(o => o.Stat == implantTemplate?.FadedStat);
        }

        public string AbiSlotDescription
            => IsEmpty ? ImplantSlot.ShortName : $"{GetImplantTemplate().RequiredAbility.ShortName} {ImplantSlot.ShortName}";

        public void ToggleImplantSlotAvailability()
            => IsImplantSlotAvailable = !IsImplantSlotAvailable;

        public class ClusterOption
        {
            public ClusterOption(Stat stat = null)
                => Stat = stat;

            public Stat Stat { get; }
            public bool IsEmpty => Stat == null;

            public override string ToString()
                => Stat?.Name;
        }
    }
}
