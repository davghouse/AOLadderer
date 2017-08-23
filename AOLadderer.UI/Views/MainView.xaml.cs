using AOLadderer.UI.Properties;
using AOLadderer.UI.ViewModels;
using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
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

        private string _currentFilePath;
        private string CurrentFilePath
        {
            get => _currentFilePath;
            set
            {
                _currentFilePath = value;
                Title = CurrentFilePath == null ? "AO Ladderer"
                    : $"{Path.GetFileName(CurrentFilePath)} - AO Ladderer";
                TitleTextBlock.Text = Title;
            }
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
                CurrentFilePath = dialog.FileName;
            }
        }

        private void SaveMenuItem_Click_SaveToFile(object sender, RoutedEventArgs e)
        {
            if (CurrentFilePath != null)
            {
                _mainViewModel.SaveToFile(CurrentFilePath);
            }
            else
            {
                SaveAsMenuItem_Click_SaveToFile(sender, e);
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
                CurrentFilePath = dialog.FileName;
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
