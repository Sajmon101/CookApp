using Microsoft.Extensions.Configuration;
using System;

namespace CookApp
{
    public static class AppSettings
    {
        private static IConfigurationRoot configuration;

        static AppSettings()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public static string BaseUrl => configuration["ApiSettings:BaseUrl"];
    }
}
