using Microsoft.Win32;

namespace DotNetVersions.Models
{
    public class DotnetRegistry
    {
        public string Name { get; }
        public string ServicePack { get; }
        public string InstallFlag { get; }
        public bool IsChild { get; private set; }

        public DotnetRegistry(string name, string servicePack, string installFlag)
        {
            Name = name;
            ServicePack = servicePack;
            InstallFlag = installFlag;
        }

        public static DotnetRegistry FromRegistryKey(RegistryKey registryKey)
        {
            var name = GetDotNetFrameworkVersion(registryKey);
            var servicePack = GetDotNetFrameworkServicePackNumber(registryKey);
            var installFlag = GetDotNetFrameworkInstallationFlag(registryKey);

            return new DotnetRegistry(name, servicePack, installFlag);

            string GetRegistyKeyValueOrEmpty(RegistryKey registryKey, string name) => registryKey.GetValue(name, "").ToString();
            string GetDotNetFrameworkVersion(RegistryKey registryKey) => GetRegistyKeyValueOrEmpty(registryKey, "Version");
            string GetDotNetFrameworkServicePackNumber(RegistryKey registryKey) => GetRegistyKeyValueOrEmpty(registryKey, "SP");
            string GetDotNetFrameworkInstallationFlag(RegistryKey registryKey) => GetRegistyKeyValueOrEmpty(registryKey, "Install");
        }

        public static DotnetRegistry FromRegistryKeyAsChild(RegistryKey registryKey)
        {
            var dotnetRegistry = FromRegistryKey(registryKey);
            dotnetRegistry.IsChild = true;

            return dotnetRegistry;
        }
    }
}
