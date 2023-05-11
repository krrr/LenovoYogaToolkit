using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LenovoYogaToolkit.Lib.Extensions;
using LenovoYogaToolkit.Lib.Listeners;
using LenovoYogaToolkit.Lib.Utils;

namespace LenovoYogaToolkit.Lib.Automation.Listeners;

public class ProcessAutomationListener : IListener<ProcessEventInfo>
{
    private static readonly object _lock = new();

    private static readonly string[] _ignoredNames =
    {
        "backgroundTaskHost",
        "CompPkgSrv",
        "conhost",
        "dllhost",
        "Lenovo Legion Toolkit",
        "msedge",
        "msedgewebview2",
        "NvOAWrapperCache",
        "SearchProtocolHost",
        "svchost",
        "WmiPrvSE"
    };

    private static readonly string[] _ignoredPaths =
    {
        Environment.GetFolderPath(Environment.SpecialFolder.Windows),
    };

    public event EventHandler<ProcessEventInfo>? Changed;

    private readonly InstanceEventListener _instanceCreationListener;
    private readonly InstanceEventListener _instanceDeletionListener;

    private readonly Dictionary<int, ProcessInfo> _processCache = new();

    public ProcessAutomationListener()
    {
        _instanceCreationListener = new InstanceEventListener(ProcessEventInfoType.Started, "Win32_ProcessStartTrace");
        _instanceCreationListener.Changed += InstanceCreationListener_Changed;

        _instanceDeletionListener = new InstanceEventListener(ProcessEventInfoType.Stopped, "Win32_ProcessStopTrace");
        _instanceDeletionListener.Changed += InstanceDeletionListener_Changed;
    }

    public async Task Start()
    {
        await _instanceCreationListener.Start().ConfigureAwait(false);
        await _instanceDeletionListener.Start().ConfigureAwait(false);
    }

    public async Task Stop()
    {
        await _instanceCreationListener.Stop().ConfigureAwait(false);
        await _instanceDeletionListener.Stop().ConfigureAwait(false);

        lock (_lock)
        {
            _processCache.Clear();
        }
    }

    private void InstanceCreationListener_Changed(object? sender, (ProcessEventInfoType type, int processId, string processName) e)
    {
        lock (_lock)
        {
            if (e.processId < 0)
                return;

            if (string.IsNullOrWhiteSpace(e.processName))
                return;

            if (_ignoredNames.Contains(e.processName, StringComparer.InvariantCultureIgnoreCase))
                return;

            string? processPath = null;
            try
            {
                processPath = Process.GetProcessById(e.processId).GetFileName();
            }
            catch (Exception ex)
            {
                if (Log.Instance.IsTraceEnabled)
                    Log.Instance.Trace($"Can't get process {e.processName} details.", ex);
            }

            if (!string.IsNullOrEmpty(processPath) && _ignoredPaths.Any(p => processPath.StartsWith(p, StringComparison.InvariantCultureIgnoreCase)))
                return;

            var processInfo = new ProcessInfo(e.processName, processPath);
            _processCache[e.processId] = processInfo;

            Changed?.Invoke(this, new(e.type, processInfo));
        }
    }

    private void InstanceDeletionListener_Changed(object? sender, (ProcessEventInfoType type, int processId, string processName) e)
    {
        lock (_lock)
        {
            CleanUpCacheIfNecessary();

            if (e.processId < 0)
                return;

            if (string.IsNullOrWhiteSpace(e.processName))
                return;

            if (_ignoredNames.Contains(e.processName, StringComparer.InvariantCultureIgnoreCase))
                return;

            if (!_processCache.TryGetValue(e.processId, out var processInfo))
                return;

            _processCache.Remove(e.processId);

            Changed?.Invoke(this, new(e.type, processInfo));
        }
    }

    private void CleanUpCacheIfNecessary()
    {
        if (_processCache.Count < 250)
            return;

        if (Log.Instance.IsTraceEnabled)
            Log.Instance.Trace($"Cleaning up process cache. Current size: {_processCache.Count}.");

        foreach (var (processId, _) in _processCache)
        {
            try
            {
                _ = Process.GetProcessById(processId);
            }
            catch (ArgumentException)
            {
                _processCache.Remove(processId);
            }
        }

        if (Log.Instance.IsTraceEnabled)
            Log.Instance.Trace($"Cleaned up process cache. Current size: {_processCache.Count}.");
    }
}