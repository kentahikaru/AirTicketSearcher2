//-----------------------------------------------------------------------
// <copyright file="KiwiWeb.cs" company="IXTENT s.r.o.">
//     Copyright 2019 IXTENT s.r.o.
// </copyright>
//-----------------------------------------------------------------------

namespace AirTicketSearcher.Kiwi
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using PuppeteerSharp;

    /// <summary>
    /// 
    /// </summary>
    public class KiwiWeb : ISearch
    {
        Configuration.Config config;

        public KiwiWeb(Configuration.Config config)
        {
            this.config = config;
        }

        public async void Run()
        {
            DateTime maxDate = new DateTime(DateTime.Now.AddYears(1).Year, DateTime.Now.AddMonths(3).Month, 1);
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, 1);
            DateTime currentDate = startDate;
            string[] destinations = this.config.kiwiWebConfig.destinations.Split(',');

            do
            {
                foreach(string destination in destinations)
                {
                    string url = CreateUrl(destination, GetStartEndMonth(currentDate), this.config.kiwiWebConfig.numberOfNights);
                    string kiwiHtml = await GetWebHtml(url);
                    Console.WriteLine(GetStartEndMonth(currentDate));
                    Console.ReadLine();
                }

                currentDate = currentDate.AddMonths(1);
            } while (currentDate < maxDate);
        }

        public string GetStartEndMonth(DateTime currentDate)
        {
            DateTime endMonth = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));

            return currentDate.ToString("yyyy-MM-dd") + "_" + endMonth.ToString("yyyy-MM-dd");
        }

        public string CreateUrl(string destination, string departureDate, string numberOfNights)
        {
            
            string url = "https://www.kiwi.com/en/search/results/prague-czechia/" + destination + "/" + departureDate + "/" + numberOfNights;
            return url;
        }

        public async Task<string> GetWebHtml(string url)
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
            });
            using (var page = await browser.NewPageAsync())
            {
                await page.GoToAsync("https://www.kiwi.com/en/search/results/prague-czechia/tokyo-japan/2019-10-02_2019-10-31/12-16", WaitUntilNavigation.Networkidle0);
                try
                {
                    ElementHandle element;
                    do
                    {
                        element = await page.WaitForXPathAsync("div.LoadingProviders", options: new WaitForSelectorOptions() { Timeout = 1 });
                    } while (element != null);
                }
                catch (Exception ex)
                {
                    // because i don't know how else to do it
                }

                string result = await page.GetContentAsync();
                return result;
            }
        }
    }
}
