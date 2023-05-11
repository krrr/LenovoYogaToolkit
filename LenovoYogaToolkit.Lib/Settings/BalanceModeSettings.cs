using static LenovoYogaToolkit.Lib.Settings.BalanceModeSettings;

namespace LenovoYogaToolkit.Lib.Settings;

public class BalanceModeSettings : AbstractSettings<BalanceModeSettingsStore>
{
    public class BalanceModeSettingsStore
    {
        public bool AIModeEnabled { get; set; }
    }

    public BalanceModeSettings() : base("balancemode.json") { }
}