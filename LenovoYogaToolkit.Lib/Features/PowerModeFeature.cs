using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LenovoYogaToolkit.Lib.Controllers;
using LenovoYogaToolkit.Lib.Listeners;

namespace LenovoYogaToolkit.Lib.Features;

public class PowerModeFeature : IFeature<PowerModeState> {
    private const string REG_KEY = "SYSTEM\\CurrentControlSet\\Services\\LITSSVC\\LNBITS\\IC\\MMC";

    private readonly AIModeController _aiModeController;
    private readonly PowerPlanController _powerPlanController;
    private readonly PowerModeListener _powerModeListener;

    public PowerModeFeature(
        AIModeController aiModeController,
        PowerPlanController powerPlanController,
        PowerModeListener powerModeListener
        ) {
        _aiModeController = aiModeController ?? throw new ArgumentNullException(nameof(aiModeController));
        _powerPlanController = powerPlanController ?? throw new ArgumentNullException(nameof(powerPlanController));
        _powerModeListener = powerModeListener ?? throw new ArgumentNullException(nameof(powerModeListener));
    }

    public async Task<bool> IsSupportedAsync() => ITSService.IsSupported() && (await GetAllStatesAsync().ConfigureAwait(false)).Length > 1;

    public async Task<PowerModeState[]> GetAllStatesAsync() {
        var modes = new List<PowerModeState>();
        try {
            using var hkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(REG_KEY);
            var capability = (int)hkey.GetValue("Capability")!;
            if ((capability & 1) == 0)
                modes.Add(PowerModeState.Balance);
            if ((capability & 2) != 0)
                modes.Add(PowerModeState.Quiet);
            if ((capability & 8) != 0)
                modes.Add(PowerModeState.Performance);
        } catch {
        }
        return modes.ToArray();
    }

    public Task<PowerModeState> GetStateAsync() {
        using var hkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(REG_KEY);
        if (hkey == null) throw new Exception("ITSService registry key not exists");
        var automode = (int)hkey.GetValue("AutomaticModeSetting")!;
        if (automode == 2) {
            return Task.FromResult(PowerModeState.Balance);
        } else if (automode == 1) {
            var current = (int)hkey.GetValue("CurrentSetting")!;
            return Task.FromResult(current switch {
                1 => PowerModeState.Quiet,
                3 => PowerModeState.Performance,
                _ => throw new Exception("unknown mode"),
            });
        } else {
            throw new Exception("unknown auto mode value");
        }
    }

    public async Task SetStateAsync(PowerModeState mode) {
        var allStates = await GetAllStatesAsync().ConfigureAwait(false);
        if (!allStates.Contains(mode))
            throw new InvalidOperationException($"Unsupported power mode {mode}.");

        //var currentState = await GetStateAsync().ConfigureAwait(false);
        //await _aiModeController.StopAsync(currentState).ConfigureAwait(false);

        var controlcode = mode switch {
            //PowerModeState.None => 0x86,
            PowerModeState.Balance => 0x87,
            PowerModeState.Quiet => 0x92,
            PowerModeState.Performance => 0x94,
            _ => throw new ArgumentException("invalid mode", nameof(mode))
        };
        using var svc = ITSService.OpenService();
        if (svc == null) {
            throw new Exception("failed to get ITSService");
        }
        svc.ExecuteCommand(controlcode);

        await _powerModeListener.NotifyAsync(mode).ConfigureAwait(false);
    }

    public async Task EnsureCorrectPowerPlanIsSetAsync()
    {
        var state = await GetStateAsync().ConfigureAwait(false);
        await _powerPlanController.ActivatePowerPlanAsync(state, true).ConfigureAwait(false);
    }

    public async Task EnsureAiModeIsSetAsync()
    {
        var state = await GetStateAsync().ConfigureAwait(false);
        await _aiModeController.StartAsync(state).ConfigureAwait(false);
    }

    public async Task EnsureAiModeIsOffAsync()
    {
        var state = await GetStateAsync().ConfigureAwait(false);
        await _aiModeController.StopAsync(state).ConfigureAwait(false);
    }
}