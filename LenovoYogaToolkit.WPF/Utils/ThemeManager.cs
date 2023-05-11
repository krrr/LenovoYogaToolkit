﻿using System;
using System.Windows;
using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Listeners;
using LenovoYogaToolkit.Lib.Settings;
using LenovoYogaToolkit.Lib.System;
using LenovoYogaToolkit.Lib.Utils;
using LenovoYogaToolkit.WPF.Extensions;

namespace LenovoYogaToolkit.WPF.Utils;

public class ThemeManager {
    private static readonly RGBColor DefaultAccentColor = new(255, 33, 33);

    private readonly ApplicationSettings _settings;
    private readonly SystemThemeListener _listener;

    public event EventHandler? ThemeApplied;

    public ThemeManager(SystemThemeListener systemThemeListener, ApplicationSettings settings) {
        _listener = systemThemeListener;
        _settings = settings;

        _listener.Changed += (_, _) => Application.Current.Dispatcher.Invoke(Apply);
    }

    public void Apply() {
        SetBackdropType();
        SetTheme();
        SetColor();

        ThemeApplied?.Invoke(this, EventArgs.Empty);
    }

    public bool IsDarkMode() {
        var theme = _settings.Store.Theme;

        switch (theme) {
            case Theme.Dark:
                return true;
            case Theme.Light:
                return false;
            case Theme.System:
                try {
                    return SystemTheme.IsDarkMode();
                } catch (Exception ex) {
                    if (Log.Instance.IsTraceEnabled)
                        Log.Instance.Trace($"Couldn't check system theme; assuming Dark Mode.", ex);

                    return true;
                }
            default:
                return true;
        }
    }

    public RGBColor GetAccentColor() {
        switch (_settings.Store.AccentColorSource) {
            case AccentColorSource.Custom:
                return _settings.Store.AccentColor ?? DefaultAccentColor;
            case AccentColorSource.System:
                try {
                    return SystemTheme.GetAccentColor();
                } catch (Exception ex) {
                    if (Log.Instance.IsTraceEnabled)
                        Log.Instance.Trace($"Couldn't check system accent color; using default.", ex);

                    return DefaultAccentColor;
                }
            default:
                return DefaultAccentColor;
        }
    }

    private void SetTheme() {
        var theme = IsDarkMode() ? Wpf.Ui.Appearance.ThemeType.Dark : Wpf.Ui.Appearance.ThemeType.Light;
        Wpf.Ui.Appearance.Theme.Apply(theme, GetBackgroundType(), false);
    }
    private void SetBackdropType() {
        ((Wpf.Ui.Controls.UiWindow)Application.Current.MainWindow).WindowBackdropType = GetBackgroundType();
    }

    private Wpf.Ui.Appearance.BackgroundType GetBackgroundType() {
        return _settings.Store.AppBackground switch {
            AppBackground.Acrylic => Wpf.Ui.Appearance.BackgroundType.Acrylic,
            AppBackground.Mica => Wpf.Ui.Appearance.BackgroundType.Mica,
            _ => Wpf.Ui.Appearance.BackgroundType.Mica,
        };
    }

    private void SetColor() {
        var accentColor = GetAccentColor().ToColor();
        Wpf.Ui.Appearance.Accent.Apply(systemAccent: accentColor,
            primaryAccent: accentColor,
            secondaryAccent: accentColor,
            tertiaryAccent: accentColor);
    }
}