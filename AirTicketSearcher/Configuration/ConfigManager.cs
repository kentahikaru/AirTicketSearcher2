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
            // Config config = new Config();

            // string test = ConfigurationManager.AppSettings["test1"];
            
            




var section = ConfigurationManager.GetSection("kiwi") as NameValueCollection;
var value = section["key1"];


            // System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // ConfigurationSectionGroupCollection sectionGroups = config.SectionGroups;

          
            // if (sectionGroups["searchers"] != null)
            // {
            //     foreach (ConfigurationSection section in sectionGroups["searchers"].Sections)
            //     {
            //         NameValueCollection db = (NameValueCollection)ConfigurationSettings.GetConfig("searchers\\" + section.SectionInformation.Name);
            //         ConfigurationSection secction = section;
            //         foreach (var item1 in db.AllKeys)
            //         {
            //             Console.WriteLine("{0}----{1}",item1,db[item1]);
            //         }
            //     }
            // }




            
            return new Config(); //config;
        }
    }
}