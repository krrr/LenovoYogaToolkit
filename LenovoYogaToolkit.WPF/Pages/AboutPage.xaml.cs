using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows;
using LenovoYogaToolkit.Lib.Extensions;
using LenovoYogaToolkit.Lib.Utils;
using LenovoYogaToolkit.WPF.Extensions;
using LenovoYogaToolkit.WPF.Resources;

namespace LenovoYogaToolkit.WPF.Pages;

public partial class AboutPage
{
    private string VersionText
    {
        get
        {
            var version = Assembly.GetEntryAssembly()?.GetName().Version;
            return version?.ToString(3) ?? "";
        }
    }
    private string BuildText => Assembly.GetEntryAssembly()?.GetBuildDateTime()?.ToString("yyyyMMddHHmmss") ?? "";

    private string CopyrightText
    {
        get
        {
            var ret = "Original author: Bartosz Cichecki";
            var location = Assembly.GetEntryAssembly()?.Location;
            if (location is null)
                return ret;
            var versionInfo = FileVersionInfo.GetVersionInfo(location);
            if (versionInfo.LegalCopyright != null) {
                ret += "\nModified by: " + versionInfo.LegalCopyright;

            }
            return ret;
        }
    }

    public AboutPage()
    {
        InitializeComponent();

        _version.Text += $" {VersionText}";
        _build.Text += $" {BuildText}";
        _copyright.Text = CopyrightText;

        _translationCredit.Visibility = Resource.Culture.Equals(new CultureInfo("en")) ? Visibility.Collapsed : Visibility.Visible;
    }

    private void OpenApplicationDataFolder_Click(object sender, RoutedEventArgs e) => new Uri(Folders.AppData).Open();

    private void OpenApplicationTempFolder_Click(object sender, RoutedEventArgs e) => new Uri(Folders.Temp).Open();
}