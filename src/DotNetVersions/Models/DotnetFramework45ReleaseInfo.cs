using System.Collections.Generic;

namespace DotNetVersions.Models
{
    public class DotnetFramework45ReleaseInfo
    {
        /// <summary>
        /// Checking the version using >= enables forward compatibility
        /// </summary>
        public int MinimumReleaseValue { get; }
        public string DotnetFrameworkVersion { get; }

        public DotnetFramework45ReleaseInfo(int minimumReleaseValue, string dotnetFrameworkVersion)
        {
            MinimumReleaseValue = minimumReleaseValue;
            DotnetFrameworkVersion = dotnetFrameworkVersion;
        }

        public static IEnumerable<DotnetFramework45ReleaseInfo> GetKnownVersions()
        {
            yield return new DotnetFramework45ReleaseInfo(528040, "4.8");
            yield return new DotnetFramework45ReleaseInfo(461808, "4.7.2");
            yield return new DotnetFramework45ReleaseInfo(461308, "4.7.1");
            yield return new DotnetFramework45ReleaseInfo(460798, "4.7");
            yield return new DotnetFramework45ReleaseInfo(394802, "4.6.2");
            yield return new DotnetFramework45ReleaseInfo(394254, "4.6.1");
            yield return new DotnetFramework45ReleaseInfo(393295, "4.6");
            yield return new DotnetFramework45ReleaseInfo(379893, "4.5.2");
            yield return new DotnetFramework45ReleaseInfo(378675, "4.5.1");
            yield return new DotnetFramework45ReleaseInfo(378389, "4.5");
        }
    }
}
