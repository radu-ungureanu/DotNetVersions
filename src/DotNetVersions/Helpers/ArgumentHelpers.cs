using System.Linq;

namespace DotNetVersions.Helpers
{
    public static class ArgumentHelpers
    {
        public static bool IsHelpArgument(string[] args) => MatchArgument(args, new[] { "?", "help" });

        public static bool IsBatchArgument(string[] args) => MatchArgument(args, new[] { "b" });

        private static bool MatchArgument(string[] args, string[] keywords)
        {
            return new[] { "/", "-", "--" }
                .SelectMany(_ => keywords, (first, second) => $"{first}{second}")
                .Contains(args.FirstOrDefault());
        }
    }
}
