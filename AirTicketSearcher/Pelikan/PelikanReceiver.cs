namespace AirTicketSearcher.Pelikan
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using PuppeteerSharp;

    /// <summary>
    /// 
    /// </summary>
    public class PelikanReceiver
    {
        Configuration.Config config = null;

        public PelikanReceiver(Configuration.Config config)
        {
            this.config = config;
        }

        public List<string> GetWebResults(int futureMonths)
        {
            List<string> listUrls = MakeUrlList(futureMonths);

            List<Task<string>> taskList = new List<Task<string>>();
            int i = 0;

            int step = 3;
            for (i = 0; i < listUrls.Count; i += step)
            {
                for (int j = 0; j < step; j++)
                {
                    //string kiwiHtmlTask =  GetWebHtmlAsync(url)
                    string url = listUrls[i+j];
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
                if (task.Result != "")
                {
                    htmlList.Add(task.Result);
                    File.WriteAllText("Pelikan" + i++.ToString() + ".html", task.Result);
                }
            }

            return htmlList;

        }

        private List<string> MakeUrlList(int futureMonths)
        {
            //DateTime maxDate = new DateTime(DateTime.Now.AddYears(1).Year, DateTime.Now.AddMonths(3).Month, 1);
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, 1);
            DateTime maxDate = startDate.AddMonths(futureMonths); // TODO: Prehodit future months do configu
            DateTime currentDate = startDate;
            string[] destinations = this.config.pelikanConfig.destinations.Split(',');
            List<string> listUrls = new List<string>();

            do
            {
                foreach (string destination in destinations)
                {
                    string url = CreateUrl(this.config.pelikanConfig.origin, destination, currentDate.ToString("yyyy_MM_dd"), currentDate.AddDays(14).ToString("yyyy_MM_dd"));

                    listUrls.Add(url);
                }

                currentDate = currentDate.AddDays(6);
            } while (currentDate < maxDate);

            return listUrls;
        }

        public string CreateUrl(string origin, string destination, string departureDate, string returnDate)
        {
            string url = "https://www.pelikan.cz/cs/letenky/T:1,P:1000E_0_0,CDF:C" + origin + ",CDT:C" + destination + ",R:1,DD:" + departureDate + ",DR:" + returnDate + "/";
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
                //Args = new string[] { "--disable-infobars", "--user-data-dir=/home/pi/.config/chromium", "--version" },
                Args = new string[] { "--ignore-certificate-errors" },
            }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    page.DefaultTimeout = 30000;
                    await page.GoToAsync(url, WaitUntilNavigation.Load);
                    try
                    {
                        //ElementHandle element;
                        //do
                        //{
                        //    element = await page.WaitForXPathAsync("div.LoadingProviders", options: new WaitForSelectorOptions() {Timeout = 1});

                        // await page.WaitForNavigationAsync(new NavigationOptions() { WaitUntil = new WaitUntilNavigation[] { WaitUntilNavigation.Networkidle0 }  });

                        //} while (element != null);
                        //element = await page.WaitForXPathAsync("//button[@class='btn']", null);
                        //await page.WaitForXPathAsync("//button[@class='btn']", null);
                        //element = await page.WaitForXPathAsync("//flights-fake-flight", null);

                        //Task t = page.WaitForXPathAsync("Button.Button__StyledButton-sc-1brqp3f-1 kePvjv", null);
                        //t.ContinueWith(async neco => {
                        //     await page.ClickAsync("svg.JourneyArrow Icon__StyledIcon-sc-1pnzn3g-0 cuOaff", null);
                        //})
                        //await page.ClickAsync("svg.JourneyArrow Icon__StyledIcon-sc-1pnzn3g-0 cuOaff", null);

                        //await page.EvaluateFunctionAsync(@"() => { 
                        //    var elements = document.getElementsByClassName('Journey-overview Journey-return'); 
                        //    for (i = 0; i < elements.length; i++) { 
                        //        elements[i].click(); 
                        //    }
                        //}", "");

                        await page.WaitForXPathAsync("//div[contains(@id,'flight-1000')]",null);


                        //await page.WaitForTimeoutAsync(10000);
                        result = await page.GetContentAsync();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.GetError());
                        await page.ScreenshotAsync("PelikanScreenshot.jpg");
                        result = "";
                        // because i don't know how else to do it
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
