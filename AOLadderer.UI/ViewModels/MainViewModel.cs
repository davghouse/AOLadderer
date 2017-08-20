namespace AOLadderer.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public BuildViewModel BuildViewModel { get; } = new BuildViewModel();
        public LadderViewModel BasicLadderViewModel { get; } = new LadderViewModel();
        public ShoppingViewModel BasicShoppingViewModel = new ShoppingViewModel();
        public LadderViewModel AdvancedLadderViewModel { get; } = new LadderViewModel();
        public ShoppingViewModel AdvancedShoppingViewModel = new ShoppingViewModel();
    }
}
