﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AirTicketSearcher.Configuration;
using AirTicketSearcher.Kiwi;
using AirTicketSearcher.Mail;
using NLog;
//using NLog.Common;
using NLog.Config;
using PuppeteerSharp;

namespace AirTicketSearcher
{
    class Program
    {
        public static NLog.Logger logger;

        public static void  Main(string[] args)
        {
            //Test test = new Test();
            //test.start();
            ////test.neco();
            //Console.ReadLine();

            Configuration.Config config = LoadConfig();

            try
            {
                Console.WriteLine("Starting AirTicketSearcher program");

                logger = NLogConfigManager.GetLogger();

                List<ISearch> searchersList = InitializeSearchers(config);
                Search(searchersList);

                Console.WriteLine("Stopping AirTicketSearcher program");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error happened");
                Console.WriteLine(ex.Message);
                logger.Error(ex.Message);

                Exception innerException = ex.InnerException;

                while (innerException != null)
                {
                    logger.Error("Inner Exception: " + Environment.NewLine + innerException.Message);
                    logger.Error(innerException.StackTrace);
                    innerException = innerException.InnerException;
                }

                Mail.Mail mail = new Mail.Mail(config.emailConfig);
                mail.SendEmail("AirTicketSearcher - Japan - Error", ex.Message);
            }
            Console.ReadLine();
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

            //searcherList.Add(new Kiwi.Kiwi(config));
            searcherList.Add(new Kiwi.KiwiWeb(config));

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
