﻿using System;
using System.Linq;
using System.Threading.Tasks;
using LenovoYogaToolkit.Lib.Controllers;
using LenovoYogaToolkit.Lib.Listeners;
using LenovoYogaToolkit.Lib.System;
using LenovoYogaToolkit.Lib.Utils;

namespace LenovoYogaToolkit.Lib.Features;

public class PowerModeFeature : AbstractLenovoGamezoneWmiFeature<PowerModeState>
{
    private readonly AIModeController _aiModeController;
    private readonly PowerPlanController _powerPlanController;
    private readonly ThermalModeListener _thermalModeListener;
    private readonly PowerModeListener _powerModeListener;

    public bool AllowAllPowerModesOnBattery { get; set; }

    public PowerModeFeature(
        AIModeController aiModeController,
        PowerPlanController powerPlanController,
        ThermalModeListener thermalModeListener,
        PowerModeListener powerModeListener
        ) : base("SmartFanMode", 1, "IsSupportSmartFan")
    {
        _aiModeController = aiModeController ?? throw new ArgumentNullException(nameof(aiModeController));
        _powerPlanController = powerPlanController ?? throw new ArgumentNullException(nameof(powerPlanController));
        _thermalModeListener = thermalModeListener ?? throw new ArgumentNullException(nameof(thermalModeListener));
        _powerModeListener = powerModeListener ?? throw new ArgumentNullException(nameof(powerModeListener));
    }

    public override async Task<PowerModeState[]> GetAllStatesAsync() {
        return new[] { PowerModeState.Quiet, PowerModeState.Balance, PowerModeState.Performance };
    }

    public override async Task SetStateAsync(PowerModeState state)
    {
        var allStates = await GetAllStatesAsync().ConfigureAwait(false);
        if (!allStates.Contains(state))
            throw new InvalidOperationException($"Unsupported power mode {state}.");

        if (state is PowerModeState.Performance 
            && !AllowAllPowerModesOnBattery
            && await Power.IsPowerAdapterConnectedAsync() is PowerAdapterStatus.Disconnected)
            throw new InvalidOperationException($"Can't switch to {state} power mode on battery."); ;

        var currentState = await GetStateAsync().ConfigureAwait(false);

        await _aiModeController.StopAsync(currentState).ConfigureAwait(false);

        var mi = await Compatibility.GetMachineInformationAsync().ConfigureAwait(false);

        if (mi.Properties.HasQuietToPerformanceModeSwitchingBug && currentState == PowerModeState.Quiet && state == PowerModeState.Performance)
        {
            _thermalModeListener.SuppressNext();
            await base.SetStateAsync(PowerModeState.Balance).ConfigureAwait(false);
            await Task.Delay(TimeSpan.FromMilliseconds(500)).ConfigureAwait(false);
        }

        _thermalModeListener.SuppressNext();
        await base.SetStateAsync(state).ConfigureAwait(false);

        await _powerModeListener.NotifyAsync(state).ConfigureAwait(false);
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