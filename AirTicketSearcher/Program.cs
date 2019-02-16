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

                // Mail.Mail email = new Mail.Mail();

                // StringBuilder sb = new StringBuilder();
                // sb.Append("<div><table>");
                
                // for(int i = 0; i < 6; i++)
                // {
                //     sb.Append("<tr>");
                //     for(int j = 0 ; j < 5; j++)
                //         sb.Append("<td>" + j.ToString() + "</td>");
                //     sb.Append("</tr>");
                // }
                // sb.Append("</table></div>");

                // email.SendEmail(sb.ToString());
              

                //Console.ReadLine();
                Console.WriteLine("Stopping AirTicketSearcher program");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error happened");
                Console.WriteLine(ex.Message);
                logger.Error(ex.Message);
                Mail.Mail mail = new Mail.Mail();
                mail.SendEmail("AirTicketSearcher - Japan - Error", ex.Message);
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
