using System;
using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Automation.Steps;
using LenovoYogaToolkit.Lib.Listeners;
using LenovoYogaToolkit.WPF.Resources;
using LenovoYogaToolkit.WPF.Utils;
using Wpf.Ui.Common;

namespace LenovoYogaToolkit.WPF.Controls.Automation.Steps;

public class RefreshRateAutomationStepControl : AbstractComboBoxAutomationStepCardControl<RefreshRate>
{
    private readonly DisplayConfigurationListener _listener = IoCContainer.Resolve<DisplayConfigurationListener>();

    public RefreshRateAutomationStepControl(IAutomationStep<RefreshRate> step) : base(step)
    {
        Icon = SymbolRegular.DesktopPulse24;
        Title = Resource.RefreshRateAutomationStepControl_Title;
        Subtitle = Resource.RefreshRateAutomationStepControl_Message;

        _listener.Changed += Listener_Changed;
    }

    protected override string ComboBoxItemDisplayName(RefreshRate value)
    {
        var str = base.ComboBoxItemDisplayName(value);
        return LocalizationHelper.ForceLeftToRight(str);
    }

    private void Listener_Changed(object? sender, EventArgs e) => Dispatcher.Invoke(async () =>
    {
        if (IsLoaded)
            await RefreshAsync();
    });
}