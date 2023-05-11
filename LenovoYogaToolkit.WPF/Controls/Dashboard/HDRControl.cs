﻿using System;
using System.Threading.Tasks;
using System.Windows;
using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Features;
using LenovoYogaToolkit.Lib.Listeners;
using LenovoYogaToolkit.WPF.Resources;
using Wpf.Ui.Common;

namespace LenovoYogaToolkit.WPF.Controls.Dashboard;

public class HDRControl : AbstractToggleFeatureCardControl<HDRState>
{
    private readonly DisplayConfigurationListener _listener = IoCContainer.Resolve<DisplayConfigurationListener>();

    protected override HDRState OnState => HDRState.On;

    protected override HDRState OffState => HDRState.Off;

    public HDRControl()
    {
        Icon = SymbolRegular.Hdr24;
        Title = Resource.HDRControl_Title;
        Subtitle = Resource.HDRControl_Message;

        _listener.Changed += Listener_Changed;
    }

    protected override async Task OnRefreshAsync()
    {
        await base.OnRefreshAsync();

        try
        {
            var isHdrBlocked = await ((HDRFeature)Feature).IsHdrBlockedAsync();

            IsToggleEnabled = !isHdrBlocked;
            Warning = isHdrBlocked ? Resource.HDRControl_Warning : string.Empty;
        }
        catch
        {
            IsToggleEnabled = true;
            Warning = string.Empty;
        }

        Visibility = Visibility.Visible;
    }

    private void Listener_Changed(object? sender, EventArgs e) => Dispatcher.Invoke(async () =>
    {
        if (IsLoaded)
            await RefreshAsync();
    });
}