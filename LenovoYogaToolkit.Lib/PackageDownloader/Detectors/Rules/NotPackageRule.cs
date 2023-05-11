﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LenovoYogaToolkit.Lib.PackageDownloader.Detectors.Rules;

internal readonly struct NotPackageRule : IPackageRule
{
    private IPackageRule Rule { get; init; }

    public static bool TryCreate(IEnumerable<IPackageRule> rules, out NotPackageRule value)
    {
        var rule = rules.FirstOrDefault();
        if (rule is null)
        {
            value = default;
            return false;
        }

        value = new NotPackageRule { Rule = rule };
        return true;
    }

    public async Task<bool> CheckDependenciesSatisfiedAsync(List<DriverInfo> driverInfoCache, HttpClient httpClient, CancellationToken token)
    {
        var result = !await Rule.CheckDependenciesSatisfiedAsync(driverInfoCache, httpClient, token).ConfigureAwait(false);
        return result;
    }

    public async Task<bool> DetectInstallNeededAsync(List<DriverInfo> driverInfoCache, HttpClient httpClient, CancellationToken token)
    {
        var result = !await Rule.DetectInstallNeededAsync(driverInfoCache, httpClient, token).ConfigureAwait(false);
        return result;
    }
}
