namespace AirTicketSearcher.Configuration
{
    using System;
    using System.Configuration;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using System.IO;
    public class ConfigManager
    {
        public ConfigManager() 
        {
        }

        public Config LoadConfiguration()
        {

            // var builder = new ConfigurationBuilder()
            // .SetBasePath(System.AppContext.BaseDirectory)
            // .AddJsonFile("appsettings.json", 
            // optional: true, 
            // reloadOnChange: true).Build();

            // var value = builder["KiwiConfigParameters:base_url"];
            // var value1 = builder["KiwiUrlParameters:urlParameters:fly_from"];

            // var appSettings = builder.Get<AppSettings>();

            Config config = new Config();
            
            var kiwiSection = ConfigurationManager.GetSection("kiwiConfigParameters") as NameValueCollection;
            config.kiwiConfig.baseUrl = kiwiSection["base_url"];

            kiwiSection = ConfigurationManager.GetSection("kiwiUrlParameters") as NameValueCollection;
            foreach(string key in kiwiSection.AllKeys)
            {
                config.kiwiConfig.kiwiUrlParameters[key] = kiwiSection[key];
            }

            int port;
            var emailSection = ConfigurationManager.GetSection("emailParameters") as NameValueCollection;
            config.emailConfig.smtpServer = emailSection["smtpServer"];
            int.TryParse(emailSection["port"], out port);
            config.emailConfig.port = port;
            config.emailConfig.userName = emailSection["username"];
            config.emailConfig.Password = emailSection["password"];
            config.emailConfig.fromName = emailSection["from_name"];
            config.emailConfig.fromAddress = emailSection["from_address"];
            config.emailConfig.toName = emailSection["to_name"];
            config.emailConfig.toAddress = emailSection["to_address"];

            return config;
        }
    }
}