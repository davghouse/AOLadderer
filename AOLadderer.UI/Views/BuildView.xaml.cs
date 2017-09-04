using AOLadderer.UI.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace AOLadderer.UI.Views
{
    public partial class BuildView : UserControl
    {
        public BuildView()
            => InitializeComponent();

        private BuildViewModel BuildViewModel => (BuildViewModel)DataContext;

        private void TextBlock_MouseLeftButtonDown_ToggleImplantSlotAvailability(object sender, MouseButtonEventArgs e)
        {
            if (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl))
                return;

            int textBlockRowIndex = Grid.GetRow((TextBlock)sender);
            BuildViewModel.ImplantConfigurationViewModels[textBlockRowIndex].ToggleImplantSlotAvailability();
        }
    }
}
