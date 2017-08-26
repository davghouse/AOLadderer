using AOLadderer.UI.Properties;
using AOLadderer.UI.ViewModels;
using Microsoft.Win32;
using System;
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

        private string _currentSavePath;
        private string CurrentSavePath
        {
            get => _currentSavePath;
            set
            {
                _currentSavePath = value;
                Title = CurrentSavePath == null ? "AO Ladderer"
                    : $"{Path.GetFileName(CurrentSavePath)} - AO Ladderer";
                TitleTextBlock.Text = Title;
            }
        }

        private string CurrentSave { get; set; }

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

                try
                {
                    string save = File.ReadAllText(dialog.FileName);
                    _mainViewModel.LoadSave(save);

                    CurrentSavePath = dialog.FileName;
                    CurrentSave = save;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
$@"Error loading file ""{dialog.FileName}"". Old .lad files aren't supported, only new .ladd files are.

{ex.ToString()}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveMenuItem_Click_SaveToFile(object sender, RoutedEventArgs e)
        {
            if (CurrentSavePath != null)
            {
                string save = _mainViewModel.GetSave();
                File.WriteAllText(CurrentSavePath, save);

                CurrentSave = save;
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
                string save = _mainViewModel.GetSave();
                File.WriteAllText(dialog.FileName, save);

                CurrentSavePath = dialog.FileName;
                CurrentSave = save;
            }
        }

        private void MinimizeButton_Click_MinimizeApplication(object sender, RoutedEventArgs e)
            => WindowState = WindowState.Minimized;

        private void CloseButton_Click_CloseApplication(object sender, RoutedEventArgs e)
        {
            if (CurrentSavePath != null)
            {
                string save = _mainViewModel.GetSave();

                if (save != CurrentSave)
                {
                    var result = MessageBox.Show(
                        $"Save ladder file \"{CurrentSavePath}\"?",
                        "Save",
                        button: MessageBoxButton.YesNoCancel,
                        icon: MessageBoxImage.Question,
                        defaultResult: MessageBoxResult.Cancel);

                    if (result == MessageBoxResult.Cancel)
                        return;

                    if (result == MessageBoxResult.Yes)
                    {
                        File.WriteAllText(CurrentSavePath, save);
                    }
                }
            }

            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Settings.Default.Save();

            base.OnClosing(e);
        }
    }
}
