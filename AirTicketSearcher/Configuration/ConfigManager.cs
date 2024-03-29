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

            //----------------------- Common section -----------------------//
            config.chromePath = ConfigurationManager.AppSettings["chrome_path"];
            config.monthsToLookFor = int.Parse(ConfigurationManager.AppSettings["months_to_look_for"]);
            config.headless = bool.Parse(ConfigurationManager.AppSettings["headless"]);
            config.maxPrice = int.Parse(ConfigurationManager.AppSettings["max_price"]);

            //----------------------- Kiwi section -----------------------//
            var kiwiSection = ConfigurationManager.GetSection("kiwiConfigParameters") as NameValueCollection;
            config.kiwiConfig.baseUrl = kiwiSection["base_url"];
            config.kiwiConfig.emailSubject = kiwiSection["email_subject"];

            kiwiSection = ConfigurationManager.GetSection("kiwiUrlParameters") as NameValueCollection;
            foreach(string key in kiwiSection.AllKeys)
            {
                config.kiwiConfig.kiwiUrlParameters[key] = kiwiSection[key];
            }

            //----------------------- KiwiWeb section -----------------------//
            kiwiSection = ConfigurationManager.GetSection("kiwiWebUrlParameters") as NameValueCollection;
            config.kiwiWebConfig.origin = kiwiSection["origin"];
            config.kiwiWebConfig.destinations = kiwiSection["destinations"];
            config.kiwiWebConfig.numberOfNights = kiwiSection["number_of_nights"];
            config.kiwiWebConfig.emailSubject = kiwiSection["email_subject"];

            //----------------------- Pelikan section -----------------------//
            var pelikanSection = ConfigurationManager.GetSection("pelikanUrlParameters") as NameValueCollection;
            config.pelikanConfig.origin = pelikanSection["origin"];
            config.pelikanConfig.destinations = pelikanSection["destinations"];
            config.pelikanConfig.emailSubject = pelikanSection["email_subject"];

            //----------------------- Skyscanner section -----------------------//
            var skyscannerSection = ConfigurationManager.GetSection("skyscannerParameters") as NameValueCollection;
            config.skyscannerConfig.origin = skyscannerSection["origin"];
            config.skyscannerConfig.destinations = skyscannerSection["destinations"];
            config.skyscannerConfig.emailSubject = skyscannerSection["email_subject"];


            //----------------------- Email section -----------------------//
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