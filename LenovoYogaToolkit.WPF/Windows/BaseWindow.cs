﻿using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace LenovoYogaToolkit.WPF.Windows;

public class BaseWindow : UiWindow
{
    public BaseWindow()
    {
        SnapsToDevicePixels = true;
        ExtendsContentIntoTitleBar = true;

        //WindowBackdropType = BackgroundType.Acrylic;

        DpiChanged += BaseWindow_DpiChanged;
    }

    private void BaseWindow_DpiChanged(object sender, DpiChangedEventArgs e) => VisualTreeHelper.SetRootDpi(this, e.NewDpi);
}