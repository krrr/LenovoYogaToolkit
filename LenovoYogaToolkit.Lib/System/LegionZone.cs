using System;

namespace LenovoYogaToolkit.Lib.System;

public class LegionZone : SoftwareDisabler
{
    protected override string[] ScheduledTasksPaths => Array.Empty<string>();
    protected override string[] ServiceNames => new[] { "LZService" };
    protected override string[] ProcessNames => new[] { "LegionZone", "LZTray" };
}