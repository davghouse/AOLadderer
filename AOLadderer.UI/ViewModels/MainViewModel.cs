using System.ComponentModel;

namespace AOLadderer.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
            => BuildViewModel.PropertyChanged += BuildViewModel_PropertyChanged;

        public BuildViewModel BuildViewModel { get; } = new BuildViewModel();
        public LadderViewModel BasicLadderViewModel { get; } = new LadderViewModel();
        public ShoppingViewModel BasicShoppingViewModel = new ShoppingViewModel();
        public LadderViewModel AdvancedLadderViewModel { get; } = new LadderViewModel();
        public ShoppingViewModel AdvancedShoppingViewModel = new ShoppingViewModel();

        private void BuildViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BuildViewModel.BasicLadderProcess))
            {

            }
            else if (e.PropertyName == nameof(BuildViewModel.AdvancedLadderProcess))
            {

            }
        }
    }
}
