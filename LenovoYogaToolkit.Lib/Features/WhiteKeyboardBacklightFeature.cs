using System;
using System.Threading.Tasks;
using LenovoYogaToolkit.Lib.System;

namespace LenovoYogaToolkit.Lib.Features;

// 参考联想智能引擎UIKeypadBacklight.dll的UIKeypadBacklight.Commons.kbdBacklightHelper


public class WhiteKeyboardBacklightFeature : AbstractDriverFeature<WhiteKeyboardBacklightState> {
    private enum BacklightType {
        Unsupported,
        Level2,
        Level2Auto,
    }

    private BacklightType backlightType;

    public WhiteKeyboardBacklightFeature() : base(Drivers.GetEnergy, Drivers.IOCTL_ENERGY_KEYBOARD) { }

    public override async Task<bool> IsSupportedAsync() {
        backlightType = GetBacklightType();
        return backlightType != BacklightType.Unsupported;
    }

    private BacklightType GetBacklightType() {
        try {
            var outBuffer = SendCode(DriverHandle(), ControlCode, 0x1);

            return (outBuffer >> 1) switch {
                // 0x1 => BacklightType.Level1,
                0x2 => BacklightType.Level2,
                0x3 => BacklightType.Level2Auto,
                _ => BacklightType.Unsupported
            };
        } catch {
            return BacklightType.Unsupported;
        }
    }
    public override Task<WhiteKeyboardBacklightState[]> GetAllStatesAsync() {
        return Task.FromResult(backlightType switch {
            BacklightType.Level2 => new WhiteKeyboardBacklightState[] {WhiteKeyboardBacklightState.Off, WhiteKeyboardBacklightState.Low, WhiteKeyboardBacklightState.High},
            BacklightType.Level2Auto => new WhiteKeyboardBacklightState[] {WhiteKeyboardBacklightState.Off, WhiteKeyboardBacklightState.Low, WhiteKeyboardBacklightState.High, WhiteKeyboardBacklightState.Auto},
            _ => throw new InvalidOperationException("Invalid backlight type")
        });
    }

    protected override uint GetInBufferValue() {
        return backlightType switch {
            BacklightType.Level2 => 0x22,
            BacklightType.Level2Auto => 0x32,
            _ => throw new InvalidOperationException("Invalid backlight type")
        };
    }

    protected override Task<uint[]> ToInternalAsync(WhiteKeyboardBacklightState state) {
        var result = state switch
        {
            WhiteKeyboardBacklightState.Off => new uint[] { 0x00000 },
            WhiteKeyboardBacklightState.Low => new uint[] { 0x10000 },
            WhiteKeyboardBacklightState.High => new uint[] { 0x20000 },
            WhiteKeyboardBacklightState.Auto => new uint[] { 0x30000 },
            _ => throw new InvalidOperationException("Invalid state"),
        };
        result[0] += GetInBufferValue() + 0x1;
        return Task.FromResult(result);
    }

    protected override Task<WhiteKeyboardBacklightState> FromInternalAsync(uint state) {
        if ((state & 0x1) != 0x1) {
            return Task.FromResult(WhiteKeyboardBacklightState.Off);  // failed
        } else if (backlightType == BacklightType.Level2Auto && (state & 0x8000) == 0x8000) {
            return Task.FromResult(WhiteKeyboardBacklightState.Off);  // disabled off??
        }

        state = (state >> 1) & 0x7FFF;
        var result = state switch {
            0x0 => WhiteKeyboardBacklightState.Off,
            0x1 => WhiteKeyboardBacklightState.Low,
            0x2 => WhiteKeyboardBacklightState.High,
            0x3 => WhiteKeyboardBacklightState.Auto,
            _ => throw new InvalidOperationException("Invalid state"),
        };
        return Task.FromResult(result);
    }
}