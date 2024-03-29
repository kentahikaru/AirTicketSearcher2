﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using PuppeteerSharp;


namespace AirTicketSearcher.Kiwi
{
    class KiwiWebReceiver
    {
        Configuration.Config config = null;

        public KiwiWebReceiver(Configuration.Config config)
        {
            this.config = config;
        }

        public List<string> GetWebResults(int futureMonths)
        {

            List<string> listUrls = MakeUrlList(futureMonths);
            List<Task<string>> taskList = new List<Task<string>>();
            int i = 0;
           
            int step = 3;
            for( i = 0; i < listUrls.Count; i += step)
            {
                for (int j = 0; j < step; j++)
                {
                    //string kiwiHtmlTask =  GetWebHtmlAsync(url)
                    string url = listUrls[i + j];
                    taskList.Add(GetWebHtmlAsync(url));

                    //string kiwiHtmlTask = await GetWebHtmlAsync(url);
                    Console.WriteLine(url);

                }

                Task.WaitAll(taskList.ToArray());
                Console.WriteLine("Tasks waited");
            }
                

            List<string> htmlList = new List<string>();
             i = 0;
            foreach (Task<string> task in taskList)
            {
                if(task.Result != "")
                {
                    htmlList.Add(task.Result);
                    //File.WriteAllText("KiwiWeb" + i++.ToString() + ".html", task.Result);
                }
                
            }

            return htmlList;
        }

        private List<string> MakeUrlList(int futureMonths)
        {
            //DateTime maxDate = new DateTime(DateTime.Now.AddYears(1).Year, DateTime.Now.AddMonths(3).Month, 1);
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, 1);
            DateTime maxDate = startDate.AddMonths(futureMonths);
            DateTime currentDate = startDate;
            string[] destinations = this.config.kiwiWebConfig.destinations.Split(',');
            List<string> listUrls = new List<string>();

            do
            {
                foreach (string destination in destinations)
                {
                    string url = CreateUrl(this.config.kiwiWebConfig.origin ,destination, GetStartEndMonth(currentDate), this.config.kiwiWebConfig.numberOfNights);

                    listUrls.Add(url);
                }

                currentDate = currentDate.AddMonths(1);
            } while (currentDate < maxDate);

            return listUrls;
        }

        public string GetStartEndMonth(DateTime currentDate)
        {
            DateTime endMonth = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));

            return currentDate.ToString("yyyy-MM-dd") + "_" + endMonth.ToString("yyyy-MM-dd");
        }

        public string CreateUrl(string origin, string destination, string departureDate, string numberOfNights)
        {

            string url = "https://www.kiwi.com/en/search/results/" + origin + "/" + destination + "/" + departureDate + "/" + numberOfNights;
            return url;
        }

        public async Task<string> GetWebHtmlAsync(string url)
        {
            string result = "";

            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = this.config.headless,
                ExecutablePath = this.config.chromePath,
                //Args = new string[] { "--incognito" },
                //Args = new string[] { "--disable-infobars", "--user-data-dir=/home/pi/.config/chromium" },
                Args = new string[] { "--ignore-certificate-errors" },
            }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    try
                    {
                        //page.DefaultTimeout = 60000;
                        await page.GoToAsync(url, WaitUntilNavigation.DOMContentLoaded);
                        //await page.GoToAsync(url, NavigationOptions.)
                        ElementHandle element;
                        //do
                        //{
                        //    element = await page.WaitForXPathAsync("div.LoadingProviders", options: new WaitForSelectorOptions() {Timeout = 1});

                        //await page.WaitForTimeoutAsync(15000);

                        //} while (element != null);
                        element = await page.WaitForXPathAsync("//Button[contains(div,'Load More')]", null);
                        //Task t = page.WaitForXPathAsync("Button.Button__StyledButton-sc-1brqp3f-1 kePvjv", null);
                        //t.ContinueWith(async neco => {
                        //     await page.ClickAsync("svg.JourneyArrow Icon__StyledIcon-sc-1pnzn3g-0 cuOaff", null);
                        //})
                        //await page.ClickAsync("svg.JourneyArrow Icon__StyledIcon-sc-1pnzn3g-0 cuOaff", null);
                        await page.EvaluateFunctionAsync(@"() => { 
                            var elements = document.getElementsByClassName('Journey-overview Journey-return'); 
                            for (i = 0; i < elements.length; i++) { 
                                elements[i].click(); 
                            }
                        }", "");


                        //await page.WaitForTimeoutAsync(10000);
                        result = await page.GetContentAsync();
                    }
                    catch (Exception ex)
                    {
                        await page.ScreenshotAsync("KiwiWebScreenshot.jpg",new ScreenshotOptions() { FullPage = true});
                        Console.WriteLine(ex.GetError());
                        // because i don't know how else to do it
                        result = "";
                    }
                    finally
                    {
                        //var elements = document.getElementsByClassName("Journey-overview Journey-return")
                        //for(i = 0; i < elements.length; i++) {elements[i].click();}
                    }

                    

                }
            }

            return result;
        }
    }
}
