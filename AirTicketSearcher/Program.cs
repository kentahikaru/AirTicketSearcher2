using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AirTicketSearcher.Configuration;
using AirTicketSearcher.Kiwi;
using NLog;
//using NLog.Common;
using NLog.Config;

namespace AirTicketSearcher
{
    class Program
    {
        public static NLog.Logger logger;
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting AirTicketSearcher program");

                logger = NLogConfigManager.GetLogger();
                Configuration.Config config = LoadConfig();
                List<ISearch> searchersList = InitializeSearchers(config);
                Search(searchersList);


              

                Console.ReadLine();
                Console.WriteLine("Stopping AirTicketSearcher program");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error happened");
                Console.WriteLine(ex.Message);
            }
            //Console.ReadLine();
        }

        private static Configuration.Config LoadConfig()
        {
            Configuration.ConfigManager configManager = new Configuration.ConfigManager();
            Configuration.Config config = configManager.LoadConfiguration();

            return config;
        }

        private static List<ISearch> InitializeSearchers(Config config)
        {
            List<ISearch> searcherList = new List<ISearch>();

            searcherList.Add(new Kiwi.Kiwi(config));

            return searcherList;
        }

        private static void Search(List<ISearch> searchersList)
        {
            //List<Task> tasks = new List<Task>();
            foreach (ISearch item in searchersList)
            {
                //Task task = item.Run();
                //task.Wait();
                //tasks.Add(task);
                item.Run();
            }
        }
    }
}
