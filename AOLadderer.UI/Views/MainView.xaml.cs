using AOLadderer.UI.Properties;
using AOLadderer.UI.ViewModels;
using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace AOLadderer.UI.Views
{
    public partial class MainView : Window
    {
        private readonly MainViewModel _mainViewModel;

        public MainView()
        {
            InitializeComponent();
            DataContext = _mainViewModel = new MainViewModel();
        }

        private void HeaderRow_MouseDown_Drag(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void OpenMenuItem_Click_LoadFromFile(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Ladder Files (*.ladd)|*.ladd|All Files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                _mainViewModel.LoadFromFile(dialog.FileName);
            }
        }

        private void SaveAsMenuItem_Click_SaveToFile(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Ladder Files (*.ladd)|*.ladd|All Files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                _mainViewModel.SaveToFile(dialog.FileName);
            }
        }

        private void MinimizeButton_Click_MinimizeApplication(object sender, RoutedEventArgs e)
            => WindowState = WindowState.Minimized;

        private void CloseButton_Click_CloseApplication(object sender, RoutedEventArgs e)
            => Close();

        protected override void OnClosing(CancelEventArgs e)
        {
            Settings.Default.Save();

            base.OnClosing(e);
        }
    }
}
