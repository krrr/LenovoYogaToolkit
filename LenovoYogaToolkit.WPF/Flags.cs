using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LenovoYogaToolkit.Lib.Utils;

namespace LenovoYogaToolkit.WPF;

public class Flags
{
    public bool IsTraceEnabled { get; }
    public bool Minimized { get; }
    public bool SkipCompatibilityCheck { get; }
    public bool DisableTrayTooltip { get; }

    public Flags(IEnumerable<string> startupArgs)
    {
        var args = startupArgs.Concat(LoadExternalArgs()).ToArray();

        IsTraceEnabled = args.Contains("--trace");
        Minimized = args.Contains("--minimized");
        SkipCompatibilityCheck = args.Contains("--skip-compat-check");
        DisableTrayTooltip = args.Contains("--disable-tray-tooltip");
    }

    private static IEnumerable<string> LoadExternalArgs()
    {
        try
        {
            var argsFile = Path.Combine(Folders.AppData, "args.txt");
            return !File.Exists(argsFile) ? Array.Empty<string>() : File.ReadAllLines(argsFile);
        }
        catch
        {
            return Array.Empty<string>();
        }
    }
}
