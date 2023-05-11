using System.Windows;

namespace LenovoYogaToolkit.WPF.Windows.Dashboard;

public partial class ExtendedHybridModeInfoWindow
{
    public ExtendedHybridModeInfoWindow() => InitializeComponent();

    private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
}