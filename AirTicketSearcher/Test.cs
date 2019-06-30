namespace AirTicketSearcher
{
    using System;
    using System.IO;
    using PuppeteerSharp;
    using HtmlAgilityPack;

    public class Test
    {
         public async void start()
         {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
            });
            using (var page = await browser.NewPageAsync())
            {
                await page.GoToAsync("https://www.kiwi.com/en/search/results/prague-czechia/tokyo-japan/2019-10-02_2019-10-31/12-16", WaitUntilNavigation.Networkidle0);
                //await page.WaitForSelectorAsync("div.Journey-overview.Journey-return");
                //await page.ClickAsync("#nmpiPixel");
                //await page.WaitForXPathAsync("//div[@class='Journey-overview.Journey-return']", null);
                //await page.WaitForSelectorAsync(".LoadingProviders", null);
                //var timeout = TimeSpan.FromSeconds(1).Milliseconds;
                try
                {
                    ElementHandle element;
                    do
                    {
                        //element = await page.WaitForXPathAsync("//div[@class='LoadingProviders']", options: new WaitForSelectorOptions() { Timeout = 1 });
                        element = await page.WaitForXPathAsync("div.LoadingProviders", options: new WaitForSelectorOptions() { Timeout = 1 });
                    } while (element != null);
                }
                catch (Exception ex)
                {
                    // because i don't know how else to do it
                }
                //await page.WaitForXPathAsync("//div[@class='SearchResultsView-padding _noPadding']", null);
                //Console.WriteLine(page);

                //ElementHandle[] elements = await page.QuerySelectorAllAsync(".Journey-overview.Journey-return");

                ElementHandle body = await page.QuerySelectorAsync("body");
                Console.WriteLine(body.ToString());
                string result = await page.GetContentAsync();
                

                //Console.WriteLine(result);
                File.WriteAllText("Test.html",result);

                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(result);

                HtmlNodeCollection tags = doc.DocumentNode.SelectNodes("//div[@class='Journey-overview Journey-return']");
                string resulttext = "";
                foreach (HtmlNode tag in tags)
                {
                    string priceNode = tag.SelectSingleNode(".//div[@class='JourneyInfoStyles__JourneyInfoPrice-vpsxn5-2 gnDWaH']").InnerText;//.GetAttributeValue;
                    
                    File.AppendAllText("prices.txt", priceNode + Environment.NewLine);
                }

                browser.CloseAsync();
            }
        }

        public async void neco()
        {
            try
            {
                await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

                var url = "https://twitter.com/search?f=tweets&vertical=default&q=netconfar&src=typd";
                using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = false,
                    UserDataDir = "/Users/neo/Library/Application Support/Google/Chrome/Default"
                }))
                using (var page = await browser.NewPageAsync())
                {
                    await page.GoToAsync(url, WaitUntilNavigation.Networkidle0);
                    await page.WaitForSelectorAsync(".js-stream-tweet");

                    var linkButtons = await page.QuerySelectorAllAsync(".js-stream-tweet:not(.favorited) .js-actionFavorite");
                    await linkButtons[0].ClickAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }



        //public async void start()
        //{
        //    await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
        //    var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        //    {
        //        Headless = false
        //    });
        //    using (var page = await browser.NewPageAsync())
        //    {
        //        await page.GoToAsync("https://www.kiwi.com/en/search/results/prague-czechia/tokyo-japan/2019-10-01_2019-10-31/12-16", WaitUntilNavigation.Networkidle2);
        //        //await page.WaitForSelectorAsync("div.Journey-overview.Journey-return");
        //        //await page.ClickAsync("#nmpiPixel");
        //        //await page.WaitForXPathAsync("//div[@class='Journey-overview.Journey-return']", null);
        //        //await page.WaitForSelectorAsync(".LoadingProviders", null);
        //        //var timeout = TimeSpan.FromSeconds(1).Milliseconds;
        //        //try
        //        //{
        //        //    ElementHandle element;
        //        //    do
        //        //    {
        //        //        element = await page.WaitForXPathAsync("//div[@class='LoadingProviders']", options: new WaitForSelectorOptions() { Timeout = 1 });
        //        //    } while (element != null);
        //        //}
        //        //catch(Exception ex)
        //        //{
        //        //    // because i don't know how else to do it
        //        //}
        //        //await page.WaitForXPathAsync("//div[@class='SearchResultsView-padding _noPadding']", null);
        //        //Console.WriteLine(page);

        //        //ElementHandle[] elements = await page.QuerySelectorAllAsync(".Journey-overview.Journey-return");


        //        string result = await page.GetContentAsync();

        //        Console.WriteLine(result);
        //        File.WriteAllText("Test.html", result);
        //    }
        //}



    }
}