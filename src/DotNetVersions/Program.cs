using DotNetVersions.Helpers;
using DotNetVersions.Models;
using Microsoft.Win32;
using System;
using System.Linq;

namespace DotNetVersions
{
    class Program
    {
        const string DotnetFrameworkRegistryKey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\";
        const string DotnetFramework45Key = "v4";
        readonly static string DotnetFramework45RegistryKey = @$"{DotnetFrameworkRegistryKey}{DotnetFramework45Key}\Full\";

        static void Main(string[] args)
        {
            if (ArgumentHelpers.IsHelpArgument(args))
            {
                Console.WriteLine("Writes all the currently installed versions of \"classic\" .NET platform in the system.");
                Console.WriteLine("Use --b, -b or /b to use in a batch, showing only the installed versions, without any extra informational lines.");
                return;
            }

            if (!ArgumentHelpers.IsBatchArgument(args))
            {
                Console.WriteLine("Currently installed \"classic\" .NET Versions in the system:");
            }

            Get1To45VersionFromRegistry();
            Get45PlusFromRegistry();
        }

        private static void Get1To45VersionFromRegistry()
        {
            using var ndpKey = Registry.LocalMachine.OpenSubKey(DotnetFrameworkRegistryKey);
            var versionKeyNames = ndpKey
                .GetSubKeyNames()
                .Where(versionKeyName => versionKeyName.StartsWith("v"))
                .Where(versionKeyName => versionKeyName != DotnetFramework45Key)
                .ToList();

            foreach (var versionKeyName in versionKeyNames)
            {
                using var versionKey = ndpKey.OpenSubKey(versionKeyName);
                var registryInfo = DotnetRegistry.FromRegistryKey(versionKey);

                Write(registryInfo);

                if (!string.IsNullOrEmpty(registryInfo.Name))
                {
                    continue;
                }

                foreach (var subVersionKeyName in versionKey.GetSubKeyNames())
                {
                    using var subVersionKey = versionKey.OpenSubKey(subVersionKeyName);
                    var subRegistryInfo = DotnetRegistry.FromRegistryKeyAsChild(subVersionKey);
                    Write(subRegistryInfo);
                }
            }
        }

        private static void Get45PlusFromRegistry()
        {
            using var ndpKey = Registry.LocalMachine.OpenSubKey(DotnetFramework45RegistryKey);
            if (ndpKey == null)
                return;

            var version = ndpKey.GetValue("Version", "").ToString();
            var releaseValue = (int?)ndpKey.GetValue("Release");

            if (!string.IsNullOrEmpty(version))
            {
                Write(version);
                return;
            }

            if (!releaseValue.HasValue)
                return;

            var dotnetFrameworkReleaseInfo = GetDotnetFrameworkReleaseInfo(releaseValue.Value);
            if (dotnetFrameworkReleaseInfo == null)
                return;

            Write(dotnetFrameworkReleaseInfo.DotnetFrameworkVersion);
        }

        private static DotnetFramework45ReleaseInfo GetDotnetFrameworkReleaseInfo(int releaseValue)
        {
            return DotnetFramework45ReleaseInfo
                .GetKnownVersions()
                .OrderByDescending(release => release.MinimumReleaseValue)
                .FirstOrDefault(release => releaseValue >= release.MinimumReleaseValue);
        }

        private static void Write(DotnetRegistry registryInfo)
        {
            if (string.IsNullOrEmpty(registryInfo.InstallFlag))
            {
                Write(registryInfo.Name);
                return;
            }

            if (!string.IsNullOrEmpty(registryInfo.ServicePack) && registryInfo.InstallFlag == "1")
            {
                Write(registryInfo.Name, registryInfo.ServicePack);
                return;
            }

            if (registryInfo.IsChild && registryInfo.InstallFlag == "1")
            {
                Write(registryInfo.Name);
            }
        }

        private static void Write(string version, string spLevel = "")
        {
            version = version.Trim();

            if (string.IsNullOrEmpty(version))
                return;

            if (!string.IsNullOrEmpty(spLevel))
                spLevel = $" Service Pack {spLevel}";

            Console.WriteLine($"{version}{spLevel}");
        }
    }
}
