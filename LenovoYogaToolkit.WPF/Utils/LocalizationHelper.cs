﻿using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using LenovoYogaToolkit.Lib.Utils;
using LenovoYogaToolkit.WPF.Resources;
using LenovoYogaToolkit.WPF.Windows.Utils;
using Windows.Win32;
using Windows.Win32.Foundation;

namespace LenovoYogaToolkit.WPF.Utils;

public static class LocalizationHelper
{
    private static readonly string LanguagePath = Path.Combine(Folders.AppData, "lang");

    private static readonly CultureInfo DefaultLanguage = new("en");

    public static readonly CultureInfo[] Languages = {
        DefaultLanguage,
        new("zh-hans"),
    };

    public static FlowDirection Direction => Resource.Culture?.TextInfo.IsRightToLeft ?? false
        ? FlowDirection.RightToLeft
        : FlowDirection.LeftToRight;

    private static string? _dateFormat = null;

    public static string ShortDateFormat
    {
        get
        {
            if (_dateFormat is not null)
                return _dateFormat;

            _dateFormat = GetSystemShortDateFormat() ?? "dd/M/yyyy";
            return _dateFormat;
        }
    }

    public static string ForceLeftToRight(string str)
    {
        if (Resource.Culture?.TextInfo.IsRightToLeft ?? false)
            return "\u200e" + str + "\u200e";
        return str;
    }

    public static async Task SetLanguageAsync(bool interactive = false)
    {
        CultureInfo? cultureInfo = null;

        if (interactive && await GetLanguageFromFile() is null)
        {
            var window = new LanguageSelectorWindow(Languages, DefaultLanguage);
            window.Show();
            cultureInfo = await window.ShouldContinue;
            if (cultureInfo is not null)
                await SaveLanguageToFileAsync(cultureInfo);
        }

        cultureInfo ??= await GetLanguageAsync();

        SetLanguageInternal(cultureInfo);
    }

    public static async Task SetLanguageAsync(CultureInfo cultureInfo)
    {
        await SaveLanguageToFileAsync(cultureInfo);
        SetLanguageInternal(cultureInfo);
    }

    public static async Task<CultureInfo> GetLanguageAsync()
    {
        var cultureInfo = await GetLanguageFromFile();
        if (cultureInfo is null)
        {
            cultureInfo = DefaultLanguage;
            await SaveLanguageToFileAsync(cultureInfo);
        }
        return cultureInfo;
    }

    private static async Task<CultureInfo?> GetLanguageFromFile()
    {
        try
        {
            var name = await File.ReadAllTextAsync(LanguagePath);
            var cultureInfo = new CultureInfo(name);
            if (!Languages.Contains(cultureInfo))
                throw new InvalidOperationException("Unknown language.");
            return cultureInfo;
        }
        catch
        {
            return null;
        }
    }

    private static Task SaveLanguageToFileAsync(CultureInfo cultureInfo) => File.WriteAllTextAsync(LanguagePath, cultureInfo.Name);

    private static void SetLanguageInternal(CultureInfo cultureInfo)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en");
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en");

        Thread.CurrentThread.CurrentUICulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

        Resource.Culture = cultureInfo;
        Lib.Resources.Resource.Culture = cultureInfo;
        Lib.Automation.Resources.Resource.Culture = cultureInfo;
    }

    private static unsafe string? GetSystemShortDateFormat()
    {
        var ptr = IntPtr.Zero;
        try
        {
            var length = PInvoke.GetLocaleInfoEx((string?)null, PInvoke.LOCALE_SSHORTDATE, null, 0);
            if (length == 0)
                return null;

            ptr = Marshal.AllocHGlobal(sizeof(char) * length);
            var charPtr = new PWSTR((char*)ptr.ToPointer());

            length = PInvoke.GetLocaleInfoEx((string?)null, PInvoke.LOCALE_SSHORTDATE, charPtr, length);
            if (length == 0)
                return null;

            return charPtr.ToString();
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
    }
}