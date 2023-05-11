using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LenovoYogaToolkit.Lib.Automation.Steps;
using LenovoYogaToolkit.WPF.Resources;
using Wpf.Ui.Common;
using NumberBox = Wpf.Ui.Controls.NumberBox;

namespace LenovoYogaToolkit.WPF.Controls.Automation.Steps;

public class DisplayBrightnessAutomationStepControl : AbstractAutomationStepControl<DisplayBrightnessAutomationStep>
{
    private readonly NumberBox _brightness = new()
    {
        Width = 150,
        IntegersOnly = true,
        ClearButtonEnabled = false,
        Min = 0,
        Max = 100,
        Step = 5,
    };

    private readonly Grid _grid = new();

    public DisplayBrightnessAutomationStepControl(DisplayBrightnessAutomationStep step) : base(step)
    {
        Icon = SymbolRegular.BrightnessHigh48;
        Title = Resource.DisplayBrightnessAutomationStepControl_Title;
        Subtitle = Resource.DisplayBrightnessAutomationStepControl_Message;
    }

    public override IAutomationStep CreateAutomationStep() => new DisplayBrightnessAutomationStep((int)_brightness.Value);

    protected override UIElement GetCustomControl()
    {
        _brightness.TextChanged += (_, _) => RaiseChanged();
        _grid.Children.Add(_brightness);
        return _grid;
    }

    protected override void OnFinishedLoading() { }

    protected override Task RefreshAsync()
    {
        _brightness.Text = $"{AutomationStep.Brightness}";
        return Task.CompletedTask;
    }
}