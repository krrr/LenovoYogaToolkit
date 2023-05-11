using System;
using System.Threading.Tasks;
using LenovoYogaToolkit.Lib.System;

namespace LenovoYogaToolkit.Lib.Features;

public class OneLevelWhiteKeyboardBacklightFeature : AbstractDriverFeature<OneLevelWhiteKeyboardBacklightState>
{
    public OneLevelWhiteKeyboardBacklightFeature() : base(Drivers.GetEnergy, Drivers.IOCTL_ENERGY_SETTINGS) { }

    public override async Task<bool> IsSupportedAsync()
    {
        try
        {
            var outBuffer = SendCode(DriverHandle(), ControlCode, GetInBufferValue());
            var result = ((int)outBuffer & 16) == 16;
            return result;
        }
        catch
        {
            return false;
        }
    }

    protected override uint GetInBufferValue() => 0x2;

    protected override Task<uint[]> ToInternalAsync(OneLevelWhiteKeyboardBacklightState state)
    {
        var result = state switch
        {
            OneLevelWhiteKeyboardBacklightState.Off => new uint[] { 0x9 },
            OneLevelWhiteKeyboardBacklightState.On => new uint[] { 0x8 },
            _ => throw new InvalidOperationException("Invalid state"),
        };
        return Task.FromResult(result);
    }

    protected override Task<OneLevelWhiteKeyboardBacklightState> FromInternalAsync(uint state)
    {
        var isOn = ((int)state & 32) == 32;
        var result = isOn ? OneLevelWhiteKeyboardBacklightState.On : OneLevelWhiteKeyboardBacklightState.Off;
        return Task.FromResult(result);
    }
}