namespace AirTicketSearcher.Configuration
{
    using System;
    using System.Configuration;
    using System.Collections.Specialized;
    public class ConfigManager
    {
        public ConfigManager() 
        {
        }

        public Config LoadConfiguration()
        {
            Config config = new Config();
            
            var kiwiSection = ConfigurationManager.GetSection("kiwiConfigParameters") as NameValueCollection;
            config.kiwiConfig.baseUrl = kiwiSection["base_url"];

            kiwiSection = ConfigurationManager.GetSection("kiwiUrlParameters") as NameValueCollection;
            foreach(string key in kiwiSection.AllKeys)
            {
                config.kiwiConfig.kiwiUrlParameters[key] = kiwiSection[key];
            }


            
            return config;
        }
    }
}