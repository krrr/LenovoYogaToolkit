using System;
using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Automation.Steps;
using LenovoYogaToolkit.Lib.Listeners;
using LenovoYogaToolkit.WPF.Resources;
using Wpf.Ui.Common;

namespace LenovoYogaToolkit.WPF.Controls.Automation.Steps;

public class ResolutionAutomationStepControl : AbstractComboBoxAutomationStepCardControl<Resolution>
{
    private readonly DisplayConfigurationListener _listener = IoCContainer.Resolve<DisplayConfigurationListener>();

    public ResolutionAutomationStepControl(IAutomationStep<Resolution> step) : base(step)
    {
        Icon = SymbolRegular.ScaleFill24;
        Title = Resource.ResolutionAutomationStepControl_Title;
        Subtitle = Resource.ResolutionAutomationStepControl_Message;

        _listener.Changed += Listener_Changed;
    }

    private void Listener_Changed(object? sender, EventArgs e) => Dispatcher.Invoke(async () =>
    {
        if (IsLoaded)
            await RefreshAsync();
    });
}