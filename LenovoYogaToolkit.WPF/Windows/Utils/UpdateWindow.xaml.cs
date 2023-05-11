﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;
using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Utils;
using LenovoYogaToolkit.WPF.Extensions;
using LenovoYogaToolkit.WPF.Resources;

namespace LenovoYogaToolkit.WPF.Windows.Utils;

public partial class UpdateWindow : IProgress<float>
{
    private readonly UpdateChecker _updateChecker = IoCContainer.Resolve<UpdateChecker>();

    private CancellationTokenSource? _downloadCancellationTokenSource;

    public UpdateWindow() => InitializeComponent();

    private async void UpdateWindow_Loaded(object sender, RoutedEventArgs e)
    {
        var updates = await _updateChecker.GetUpdates();

        var stringBuilder = new StringBuilder();
        foreach (var update in updates)
        {
            stringBuilder.AppendLine("**" + update.Title + "**   _(" + update.Date.ToString("D") + ")_")
                .AppendLine()
                .AppendLine(update.Description)
                .AppendLine();
        }

        _markdownViewer.Markdown = stringBuilder.ToString();

        _downloadButton.IsEnabled = true;
    }

    private void UpdateWindow_Closing(object? sender, CancelEventArgs e) => _downloadCancellationTokenSource?.Cancel();

    private async void DownloadButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            _downloadCancellationTokenSource?.Cancel();
            _downloadCancellationTokenSource = new CancellationTokenSource();

            SetDownloading(true);

            var path = await _updateChecker.DownloadLatestUpdate(this, _downloadCancellationTokenSource.Token);

            _downloadCancellationTokenSource = null;

            Process.Start(path, $"/SILENT /RESTARTAPPLICATIONS /LANG={Resource.Culture.Name.Replace("-", "")}");
            await App.Current.ShutdownAsync();
        }
        catch (OperationCanceledException)
        {
            SetDownloading(false);
        }
        catch
        {
            SetDownloading(false);
        }
    }

    private void CancelDownloadButton_Click(object sender, RoutedEventArgs e) => _downloadCancellationTokenSource?.Cancel();

    private void SetDownloading(bool isDownloading)
    {
        if (isDownloading)
        {
            _downloadProgressBar.Visibility = Visibility.Visible;

            _downloadButton.Visibility = Visibility.Collapsed;
            _downloadButton.IsEnabled = false;

            _cancelDownloadButton.Visibility = Visibility.Visible;
            _cancelDownloadButton.IsEnabled = true;
        }
        else
        {
            _downloadProgressBar.Value = 0;
            _downloadProgressBar.IsIndeterminate = true;
            _downloadProgressBar.Visibility = Visibility.Hidden;

            _downloadButton.Visibility = Visibility.Visible;
            _downloadButton.IsEnabled = true;

            _cancelDownloadButton.Visibility = Visibility.Collapsed;
            _cancelDownloadButton.IsEnabled = false;
        }
    }

    public void Report(float value) => Dispatcher.Invoke(() =>
    {
        _downloadProgressBar.IsIndeterminate = !(value > 0);
        _downloadProgressBar.Value = value;
    });
}