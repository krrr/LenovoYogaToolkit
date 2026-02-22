using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Listeners;
using LenovoYogaToolkit.Lib.Settings;
using LenovoYogaToolkit.Lib.System;
using LenovoYogaToolkit.Lib.Utils;
using LenovoYogaToolkit.WPF.Extensions;
using NvAPIWrapper.Native;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;

namespace LenovoYogaToolkit.WPF.Utils;

public class ThemeManager {
    private static readonly RGBColor DefaultAccentColor = new(255, 33, 33);

    private readonly ApplicationSettings _settings;
    private readonly SystemThemeListener _listener;
    public static readonly int DWMWA_CAPTION_COLOR = 35;

    public event EventHandler? ThemeApplied;

    public ThemeManager(SystemThemeListener systemThemeListener, ApplicationSettings settings) {
        _listener = systemThemeListener;
        _settings = settings;

        _listener.Changed += (_, _) => Application.Current.Dispatcher.Invoke(Apply);
    }

    public void Apply() {
        SetTheme();
        SetColor();
        ((Wpf.Ui.Controls.UiWindow)Application.Current.MainWindow).WindowBackdropType = GetBackgroundType();

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

        // 默认的文字颜色太淡需要修改
        // 可能只要启动后改一次就够，先不优化
        var res = "pack://application:,,,/Wpf.Ui;component/Styles/Theme/Light.xaml";
        var textFillColorPrimary = Color.FromRgb(0x1b, 0x1b, 0x1a);
        Console.WriteLine(Application.Current.Resources.MergedDictionaries);
        SetThemeOverride(res, "TextFillColorPrimary", textFillColorPrimary);
        SetThemeOverride(res, "TextFillColorPrimaryBrush", new SolidColorBrush(textFillColorPrimary));
        var textFillColorSecondary = Color.FromRgb(0x60, 0x60, 0x60);
        SetThemeOverride(res, "TextFillColorSecondary", textFillColorSecondary);
        SetThemeOverride(res, "TextFillColorSecondaryBrush", new SolidColorBrush(textFillColorSecondary));
    }

    public Wpf.Ui.Appearance.BackgroundType GetBackgroundType() {
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

    [DllImport("dwmapi.dll")]
    public static extern int DwmSetWindowAttribute([In] IntPtr hWnd, [In] int dwAttribute, [In] ref int pvAttribute, [In] int cbAttribute);

    public void SetThemeOverride(string resourceLookup, string key, object value)
    {
        var mergedDicts = Application.Current.Resources.MergedDictionaries;

        foreach (var dict in mergedDicts)
        {
            if (dict.Source != null && dict.Source.ToString().Equals(resourceLookup)) // 只在这个特定的字典里覆盖
            {
                dict[key] = value;
                return;
            }
        }
    }
}