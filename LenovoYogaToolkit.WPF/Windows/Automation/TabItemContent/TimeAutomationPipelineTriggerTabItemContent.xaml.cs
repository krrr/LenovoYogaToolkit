using System;
using System.Windows;
using LenovoYogaToolkit.Lib;
using LenovoYogaToolkit.Lib.Automation.Pipeline.Triggers;
using LenovoYogaToolkit.Lib.Extensions;

namespace LenovoYogaToolkit.WPF.Windows.Automation.TabItemContent;

public partial class TimeAutomationPipelineTriggerTabItemContent : IAutomationPipelineTriggerTabItemContent<ITimeAutomationPipelineTrigger>
{
    private readonly ITimeAutomationPipelineTrigger _trigger;
    private readonly bool _isSunrise;
    private readonly bool _isSunset;
    private readonly Time? _time;

    public TimeAutomationPipelineTriggerTabItemContent(ITimeAutomationPipelineTrigger trigger)
    {
        _trigger = trigger;
        _isSunrise = trigger.IsSunrise;
        _isSunset = trigger.IsSunset;
        _time = trigger.Time;

        InitializeComponent();
    }

    private void TimeTabItem_Initialized(object? sender, EventArgs e)
    {
        _sunriseRadioButton.IsChecked = _isSunrise;
        _sunsetRadioButton.IsChecked = _isSunset;

        var local = _time is not null
            ? DateTimeExtensions.UtcFrom(_time.Value.Hour, _time.Value.Minute).ToLocalTime()
            : DateTime.Now;

        _timePickerHours.Value = local.Hour;
        _timePickerMinutes.Value = local.Minute;

        _timeRadioButton.IsChecked = _time is not null;
        _timePickerPanel.IsEnabled = _time is not null;
    }

    public ITimeAutomationPipelineTrigger GetTrigger()
    {
        var isSunrise = _sunriseRadioButton.IsChecked ?? false;
        var isSunset = _sunsetRadioButton.IsChecked ?? false;
        var utc = DateTimeExtensions.LocalFrom((int)_timePickerHours.Value, (int)_timePickerMinutes.Value).ToUniversalTime();
        Time? time = _timePickerPanel.IsEnabled ? new()
        {
            Hour = utc.Hour,
            Minute = utc.Minute,
        } : null;
        return _trigger.DeepCopy(isSunrise, isSunset, time);
    }

    private void RadioButton_Click(object sender, RoutedEventArgs e)
    {
        _timePickerPanel.IsEnabled = _timeRadioButton.IsChecked ?? false;
    }
}
