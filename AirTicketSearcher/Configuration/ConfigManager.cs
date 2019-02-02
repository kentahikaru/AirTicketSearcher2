namespace AirTicketSearcher.Configuration
{
    using System.Configuration;
    public class ConfigManager
    {
        public ConfigManager() 
        {
        }

        public Config LoadConfiguration()
        {
            Config config = new Config();

            string test = ConfigurationManager.AppSettings["test1"];
            
            return config;
        }
    }
}