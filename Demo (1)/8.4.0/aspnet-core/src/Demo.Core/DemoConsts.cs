using Demo.Debugging;

namespace Demo
{
    public class DemoConsts
    {
        public const string LocalizationSourceName = "Demo";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "530df6ec31a04ec98770cc5d6595fdf4";
    }
}
