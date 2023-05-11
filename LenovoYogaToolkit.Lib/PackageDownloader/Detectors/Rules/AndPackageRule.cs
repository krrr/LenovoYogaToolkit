﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LenovoYogaToolkit.Lib.PackageDownloader.Detectors.Rules;

internal readonly struct AndPackageRule : IPackageRule
{
    private IEnumerable<IPackageRule> Rules { get; init; }

    public static bool TryCreate(IEnumerable<IPackageRule> rules, out AndPackageRule value)
    {
        value = new AndPackageRule { Rules = rules };
        return true;
    }

    public async Task<bool> CheckDependenciesSatisfiedAsync(List<DriverInfo> driverInfoCache, HttpClient httpClient, CancellationToken token)
    {
        foreach (var rule in Rules)
        {
            if (!await rule.CheckDependenciesSatisfiedAsync(driverInfoCache, httpClient, token).ConfigureAwait(false))
                return false;
        }

        return true;
    }

    public async Task<bool> DetectInstallNeededAsync(List<DriverInfo> driverInfoCache, HttpClient httpClient, CancellationToken token)
    {
        foreach (var rule in Rules)
        {
            if (!await rule.DetectInstallNeededAsync(driverInfoCache, httpClient, token).ConfigureAwait(false))
                return false;
        }

        return true;
    }
}
