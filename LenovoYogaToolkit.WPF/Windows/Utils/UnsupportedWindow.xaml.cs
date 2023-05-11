using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Utils;

namespace LenovoYogaToolkit.WPF.Windows.Utils;

public partial class UnsupportedWindow
{
    private readonly TaskCompletionSource<bool> _taskCompletionSource = new();

    public Task<bool> ShouldContinue => _taskCompletionSource.Task;

    public UnsupportedWindow(MachineInformation mi)
    {
        InitializeComponent();

        _vendorText.Text = mi.Vendor;
        _modelText.Text = mi.Model;
        _machineTypeText.Text = mi.MachineType;
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e) {
        var originContent = _continueButton.Content;
        for (var i = 5; i > 0; i--) {
            _continueButton.Content = $"{originContent} ({i})";
            await Task.Delay(TimeSpan.FromSeconds(1));
        }

        _continueButton.Content = originContent;
        _continueButton.IsEnabled = true;
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        _taskCompletionSource.TrySetResult(false);
    }

    private void Logs_Click(object sender, RoutedEventArgs e)
    {
        var logsDirectory = Path.Combine(Folders.AppData, "log");
        Directory.CreateDirectory(logsDirectory);
        Process.Start("explorer.exe", logsDirectory);
    }

    private void Continue_Click(object sender, RoutedEventArgs e)
    {
        _taskCompletionSource.TrySetResult(true);
        Close();
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        _taskCompletionSource.TrySetResult(false);
        Close();
    }
}