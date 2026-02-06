using System.Collections.Generic;

namespace PowerPages.SolutionComponents.Console.Constants
{
    public static class ExcludedSettings
    {
        /// <summary>
        /// Prefixes of site setting names that should be excluded when the exclusion option is enabled.
        /// </summary>
        public static readonly List<string> ExcludedSiteSettingPrefixes = new ()
        {
            "Authentication/",
        };
    }
}
