using System;
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
            //DateTime maxDate = new DateTime(DateTime.Now.AddYears(1).Year, DateTime.Now.AddMonths(3).Month, 1);
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, 1);
            DateTime maxDate = startDate.AddMonths(futureMonths);
            DateTime currentDate = startDate;
            string[] destinations = this.config.kiwiWebConfig.destinations.Split(',');

            List<Task<string>> taskList = new List<Task<string>>();
            do
            {
                foreach (string destination in destinations)
                {
                    Console.WriteLine("destination: " + destination);
                    string url = CreateUrl(destination, GetStartEndMonth(currentDate), this.config.kiwiWebConfig.numberOfNights);
                    //string kiwiHtmlTask =  GetWebHtmlAsync(url)
                    taskList.Add(GetWebHtmlAsync(url));

                    //string kiwiHtmlTask = await GetWebHtmlAsync(url);
                    Console.WriteLine(GetStartEndMonth(currentDate));
                }

                Task.WaitAll(taskList.ToArray());
                Console.WriteLine("Tasks waited");
                currentDate = currentDate.AddMonths(15);
            } while (currentDate < maxDate);

            List<string> htmlList = new List<string>();
            int i = 0;
            foreach (Task<string> task in taskList)
            {
                htmlList.Add(task.Result);
                File.WriteAllText("Results" + i++.ToString() + ".html", task.Result);
            }

            return htmlList;
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

        public async Task<string> GetWebHtmlAsync(string url)
        {
            string result = "";

            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                ExecutablePath = this.config.chromePath,
            }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    await page.GoToAsync("https://www.kiwi.com/en/search/results/prague-czechia/tokyo-japan/2019-10-02_2019-10-31/12-16", WaitUntilNavigation.Networkidle0);
                    try
                    {
                        ElementHandle element;
                        //do
                        //{
                        //    element = await page.WaitForXPathAsync("div.LoadingProviders", options: new WaitForSelectorOptions() {Timeout = 1});



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



                        await page.WaitForTimeoutAsync(10000);
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        // because i don't know how else to do it
                    }
                    finally
                    {
                        //var elements = document.getElementsByClassName("Journey-overview Journey-return")
                        //for(i = 0; i < elements.length; i++) {elements[i].click();}
                    }

                    result = await page.GetContentAsync();

                }
            }

            return result;
        }
    }
}
