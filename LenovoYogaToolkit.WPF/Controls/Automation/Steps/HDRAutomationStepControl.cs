using System;
using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Automation.Steps;
using LenovoYogaToolkit.Lib.Listeners;
using LenovoYogaToolkit.WPF.Resources;
using Wpf.Ui.Common;

namespace LenovoYogaToolkit.WPF.Controls.Automation.Steps;

public class HDRAutomationStepControl : AbstractComboBoxAutomationStepCardControl<HDRState>
{
    private readonly DisplayConfigurationListener _listener = IoCContainer.Resolve<DisplayConfigurationListener>();

    public HDRAutomationStepControl(IAutomationStep<HDRState> step) : base(step)
    {
        Icon = SymbolRegular.Hdr24;
        Title = Resource.HDRAutomationStepControl_Title;
        Subtitle = Resource.HDRAutomationStepControl_Message;

        _listener.Changed += Listener_Changed;
    }

    private void Listener_Changed(object? sender, EventArgs e) => Dispatcher.Invoke(async () =>
    {
        if (IsLoaded)
            await RefreshAsync();
    });
}