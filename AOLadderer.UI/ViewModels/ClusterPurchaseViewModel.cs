namespace AOLadderer.UI.ViewModels
{
    public class ClusterPurchaseViewModel : ViewModelBase
    {
        public ClusterPurchaseViewModel(ClusterTemplate clusterTemplate, int minimumClusterQL)
        {
            ClusterTemplate = clusterTemplate;
            MinimumClusterQL = minimumClusterQL;
        }

        public ClusterTemplate ClusterTemplate { get; }
        public int MinimumClusterQL { get; }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set => Set(ref _isChecked, value);
        }

        public Stat Stat => ClusterTemplate.Stat;
    }
}
