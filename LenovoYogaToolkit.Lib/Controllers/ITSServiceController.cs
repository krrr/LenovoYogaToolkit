using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace LenovoYogaToolkit.Lib.Controllers;

/* https://github.com/dantmnf/OpenLenovoSettings/blob/master/OpenLenovoSettings.FeatureLib/Backend/ITSService.cs */
public class ITSService {
    public const uint ITS_VERSION = 0x4000;
    public const uint ITS_VERSION_4 = 0x5000;
    public const uint ITS_VERSION_5 = 0x6000;
    internal static uint GetItsServiceCapability() {
        var value = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LITSSVC\LNBITS\IC", "Version", 0);
        return value switch {
            int valuei => (uint)valuei,
            _ => 0,
        };
    }
    public static ServiceController? OpenService() {
        try {
            return new ServiceController("LITSSVC");
        } catch {
            return null;
        }
    }

    public static bool IsSupported() {
        using var ctlr = OpenService();
        return ctlr?.Status == ServiceControllerStatus.Running;
    }
}
