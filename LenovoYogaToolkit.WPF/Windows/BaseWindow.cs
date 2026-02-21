using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Extensions;
using LenovoYogaToolkit.WPF.Utils;
using NvAPIWrapper.Native;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace LenovoYogaToolkit.WPF.Windows;

public class BaseWindow : UiWindow
{
    private readonly ThemeManager _themeManager = IoCContainer.Resolve<ThemeManager>();

    public BaseWindow()
    {
        SnapsToDevicePixels = true;
        ExtendsContentIntoTitleBar = true;
        WindowBackdropType = _themeManager.GetBackgroundType();
        DpiChanged += BaseWindow_DpiChanged;

    }

    private void BaseWindow_DpiChanged(object sender, DpiChangedEventArgs e) => VisualTreeHelper.SetRootDpi(this, e.NewDpi);

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        // 即使用了ExtendsContentIntoTitleBar并且应用了Acrylic效果，Windows依然在逻辑上保留了一个“标题栏区域”，DWM依然会绘制一个主题色的矩形。
        // 使用DWMWA_CAPTION_COLOR，win11限定，可以取消这个效果。win10中需要复杂的 HTTRANSPARENT 或彻底移除 WS_CAPTION 样式（这会丢失阴影）。
        if (OSExtensions.GetCurrent() == OS.Windows11)
        {
            var windowHandle = new WindowInteropHelper(this).Handle;
            int captionColor = unchecked((int)0xFFFFFFFE);
            ThemeManager.DwmSetWindowAttribute(
                windowHandle,
                ThemeManager.DWMWA_CAPTION_COLOR,
                ref captionColor,
                Marshal.SizeOf(typeof(int)));
        }
    }
}