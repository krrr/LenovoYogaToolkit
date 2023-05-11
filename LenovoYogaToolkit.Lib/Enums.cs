using System;
using System.ComponentModel.DataAnnotations;
using LenovoYogaToolkit.Lib.Resources;

namespace LenovoYogaToolkit.Lib;

public enum AlwaysOnUSBState
{
    [Display(ResourceType = typeof(Resource), Name = "AlwaysOnUSBState_Off")]
    Off,
    [Display(ResourceType = typeof(Resource), Name = "AlwaysOnUSBState_OnWhenSleeping")]
    OnWhenSleeping,
    [Display(ResourceType = typeof(Resource), Name = "AlwaysOnUSBState_OnAlways")]
    OnAlways
}

public enum AutorunState
{
    [Display(ResourceType = typeof(Resource), Name = "AutorunState_Enabled")]
    Enabled,
    [Display(ResourceType = typeof(Resource), Name = "AutorunState_EnabledDelayed")]
    EnabledDelayed,
    [Display(ResourceType = typeof(Resource), Name = "AutorunState_Disabled")]
    Disabled
}

public enum BatteryState
{
    [Display(ResourceType = typeof(Resource), Name = "BatteryState_Conservation")]
    Conservation,
    [Display(ResourceType = typeof(Resource), Name = "BatteryState_Normal")]
    Normal,
    [Display(ResourceType = typeof(Resource), Name = "BatteryState_RapidCharge")]
    RapidCharge
}

[Flags]
public enum DriverKey
{
    Fn_F10 = 32,
    Fn_F4 = 256,
    Fn_F8 = 8192,
    Fn_Space = 4096,
}

public enum FanTableType
{
    Unknown,
    CPU,
    GPU,
    CPUSensor
}

public enum FlipToStartState
{
    Off,
    On
}

public enum FnLockState
{
    Off,
    On
}

public enum GSyncState
{
    On,
    Off
}

public enum HDRState
{
    Off,
    On
}

public enum HybridModeState
{
    [Display(ResourceType = typeof(Resource), Name = "HybridModeState_On")]
    On,
    [Display(ResourceType = typeof(Resource), Name = "HybridModeState_OnIGPUOnly")]
    OnIGPUOnly,
    [Display(ResourceType = typeof(Resource), Name = "HybridModeState_OnAuto")]
    OnAuto,
    [Display(ResourceType = typeof(Resource), Name = "HybridModeState_Off")]
    Off
}

public enum IGPUModeState
{
    Default,
    IGPUOnly,
    Auto
}

public enum KeyboardLayout
{
    Ansi,
    Iso
}

public enum KnownFolder
{
    Contacts,
    Downloads,
    Favorites,
    Links,
    SavedGames,
    SavedSearches
}

public enum LightingChangeState
{
    Panel = 0,
    Ports = 1,
}

public enum MicrophoneState
{
    Off,
    On
}

public enum NativeWindowsMessage
{
    LidOpened,
    LidClosed,
    MonitorOn,
    MonitorOff,
    MonitorConnected,
    MonitorDisconnected,
    OnDisplayDeviceArrival
}

public enum NotificationDuration
{
    Short,
    Long
}

public enum NotificationType
{
    ACAdapterConnected,
    ACAdapterConnectedLowWattage,
    ACAdapterDisconnected,
    CameraOn,
    CameraOff,
    CapsLockOn,
    CapsLockOff,
    FnLockOn,
    FnLockOff,
    MicrophoneOff,
    MicrophoneOn,
    NumLockOn,
    NumLockOff,
    PanelLogoLightingOn,
    PanelLogoLightingOff,
    PortLightingOn,
    PortLightingOff,
    PowerModeQuiet,
    PowerModeBalance,
    PowerModePerformance,
    RefreshRate,
    SmartKeyDoublePress,
    SmartKeySinglePress,
    TouchpadOn,
    TouchpadOff,
    UpdateAvailable,
    WhiteKeyboardBacklightChanged,
    WhiteKeyboardBacklightOff
}

public enum NotificationPosition
{
    [Display(ResourceType = typeof(Resource), Name = "NotificationPosition_BottomRight")]
    BottomRight,
    [Display(ResourceType = typeof(Resource), Name = "NotificationPosition_BottomCenter")]
    BottomCenter,
    [Display(ResourceType = typeof(Resource), Name = "NotificationPosition_BottomLeft")]
    BottomLeft,
    [Display(ResourceType = typeof(Resource), Name = "NotificationPosition_CenterLeft")]
    CenterLeft,
    [Display(ResourceType = typeof(Resource), Name = "NotificationPosition_TopLeft")]
    TopLeft,
    [Display(ResourceType = typeof(Resource), Name = "NotificationPosition_TopCenter")]
    TopCenter,
    [Display(ResourceType = typeof(Resource), Name = "NotificationPosition_TopRight")]
    TopRight,
    [Display(ResourceType = typeof(Resource), Name = "NotificationPosition_CenterRight")]
    CenterRight,
    [Display(ResourceType = typeof(Resource), Name = "NotificationPosition_Center")]
    Center
}

public enum OneLevelWhiteKeyboardBacklightState
{
    Off,
    On
}

public enum OS
{
    [Display(Name = "Windows 11")]
    Windows11,
    [Display(Name = "Windows 10")]
    Windows10,
    [Display(Name = "Windows 8")]
    Windows8,
    [Display(Name = "Windows 7")]
    Windows7
}

public enum OverDriveState
{
    Off,
    On
}

public enum PowerAdapterStatus
{
    Connected,
    ConnectedLowWattage,
    Disconnected
}

public enum PowerModeState
{
    [Display(ResourceType = typeof(Resource), Name = "PowerModeState_Quiet")]
    Quiet,
    [Display(ResourceType = typeof(Resource), Name = "PowerModeState_Balance")]
    Balance,
    [Display(ResourceType = typeof(Resource), Name = "PowerModeState_Performance")]
    Performance,
}

public enum ProcessEventInfoType
{
    Started,
    Stopped
}

public enum RebootType
{
    NotRequired = 0,
    Forced = 1,
    Requested = 3,
    ForcedPowerOff = 4,
    Delayed = 5
}


public enum SoftwareStatus
{
    Enabled,
    Disabled,
    NotFound
}

public enum SpecialKey
{
    FnF9 = 1,
    FnLockOn = 2,
    FnLockOff = 3,
    FnPrtSc = 4,
    CameraOn = 12,
    CameraOff = 13,
    FnR = 16,
    FnR2 = 0x0041002A,
    PanelLogoLightingOn = 20,
    PanelLogoLightingOff = 21,
}


public enum Theme
{
    [Display(ResourceType = typeof(Resource), Name = "Theme_System")]
    System,
    [Display(ResourceType = typeof(Resource), Name = "Theme_Light")]
    Light,
    [Display(ResourceType = typeof(Resource), Name = "Theme_Dark")]
    Dark
}

public enum AccentColorSource
{
    [Display(ResourceType = typeof(Resource), Name = "AccentColorSource_System")]
    System,
    [Display(ResourceType = typeof(Resource), Name = "AccentColorSource_Custom")]
    Custom
}

public enum AppBackground {
    [Display(ResourceType = typeof(Resource), Name = "AppBackground_Acrylic")]
    Acrylic,
    [Display(ResourceType = typeof(Resource), Name = "AppBackground_Mica")]
    Mica
}

public enum TemperatureUnit
{
    C,
    F
}

public enum ThermalModeState
{
    Unknown,
    Quiet,
    Balance,
    Performance,
}

public enum TouchpadLockState
{
    Off,
    On
}

public enum WhiteKeyboardBacklightState {
    [Display(ResourceType = typeof(Resource), Name = "WhiteKeyboardBacklightState_Off")]
    Off,
    [Display(ResourceType = typeof(Resource), Name = "WhiteKeyboardBacklightState_Low")]
    Low,
    [Display(ResourceType = typeof(Resource), Name = "WhiteKeyboardBacklightState_High")]
    High,
    [Display(ResourceType = typeof(Resource), Name = "WhiteKeyboardBacklightState_Auto")]
    Auto
}

public enum WinKeyState
{
    Off,
    On
}

public enum WinKeyChanged { }