using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AOLadderer.UI
{
    public partial class App : Application
    {
        public App()
        {
            TextOptions.TextFormattingModeProperty.OverrideMetadata(
                typeof(DependencyObject),
                new FrameworkPropertyMetadata(TextFormattingMode.Display, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

            ToolTipService.ShowDurationProperty.OverrideMetadata(
                typeof(DependencyObject),
                new FrameworkPropertyMetadata(Int32.MaxValue));
        }
    }
}
