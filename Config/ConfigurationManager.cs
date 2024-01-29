using Microsoft.Extensions.Configuration;

namespace BookApi {
    /// <summary>
    /// Provides access to the application configuration settings.
    /// </summary>
    static class ConfigurationManager
    {
        /// <summary>
        /// Gets the application settings configuration.
        /// </summary>
        public static IConfiguration AppSetting { get; }

        /// <summary>
        /// Initializes the ConfigurationManager class.
        /// </summary>
        static ConfigurationManager () {
            AppSetting = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        }
    }
}