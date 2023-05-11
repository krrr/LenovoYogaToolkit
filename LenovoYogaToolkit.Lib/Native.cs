using System.Runtime.InteropServices;

namespace LenovoYogaToolkit.Lib;

#region Battery

[StructLayout(LayoutKind.Sequential)]
internal struct LENOVO_BATTERY_INFORMATION
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
    public byte[] Bytes1;
    public ushort Temperature;
    public ushort ManufactureDate;
    public ushort FirstUseDate;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public byte[] Bytes2;
}

#endregion

