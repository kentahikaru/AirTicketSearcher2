using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HtmlAgilityPack;
using NLog;

namespace AirTicketSearcher.Kiwi
{
    class KiwiWebAnalyzer
    {
        Configuration.Config config = null;

        public KiwiWebAnalyzer(Configuration.Config config)
        {
            this.config = config;
        }

        public List<List<KiwiWebData>> AnalyzeWebResults(List<string> htmlList)
        {
            List<List<KiwiWebData>> listOfListsKiwiWebData = new List<List<KiwiWebData>>();

            foreach(string html in htmlList)
            {
                try
                {
                    List<KiwiWebData> listKiwiWebData = AnalyzeHtml(html);
                    listOfListsKiwiWebData.Add(listKiwiWebData);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetError());
                    File.WriteAllText("KiwiPage.html", html);

                }


            }

            return listOfListsKiwiWebData;
        }

        private List<KiwiWebData> AnalyzeHtml(string html)
        {

            List<KiwiWebData> listHtmlData = new List<KiwiWebData>();

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            //HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class='Journey-overview Journey-return']");
            //HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class='Journey clear spCard open _unseen']");
            //HtmlNodeCollection nodes2 = doc.DocumentNode.SelectNodes("//div[@class='Journey clear spCard open']");
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(".//div[contains(@class,'Journey clear spCard open')]");
            
            if (nodes == null || nodes.Count == 0)
            {
                throw new Exception("No Nodes to analyze");
            }
                

            foreach (HtmlNode node in nodes)
            {
                try
                {
                    KiwiWebData kiwiWebData = new KiwiWebData();
                    HtmlNodeCollection subnodesCollection = node.SelectNodes(".//div[@class='TripInfo _results']");
                    HtmlNode toDestination = subnodesCollection[0];
                    HtmlNode fromDestination = subnodesCollection[1];

                    string price = node.SelectSingleNode(".//div[@class='JourneyInfoStyles__JourneyInfoPrice-vpsxn5-2 gnDWaH']").InnerText;//.GetAttributeValue;
                    price = price.Replace(" ", "").Replace(",", "");
                    int intPrice = int.Parse(price.Substring(0, price.Length - 2));
                    if (intPrice > this.config.maxPrice)
                        continue;

                    kiwiWebData.price = price;
                    kiwiWebData.lengthOfStay = node.SelectSingleNode(".//div[@class='Journey-nights-place']").InnerText;

                    kiwiWebData.airlinesToDestination = toDestination.SelectSingleNode(".//div[@class='AirlineNames']").InnerText;
                    kiwiWebData.airlinesFromDestination = fromDestination.SelectSingleNode(".//div[@class='AirlineNames']").InnerText;

                    kiwiWebData.durationToDestination = toDestination.SelectSingleNode(".//div[@class='TripInfoField-flight-duration']").InnerText;
                    kiwiWebData.durationFromDestination = fromDestination.SelectSingleNode(".//div[@class='TripInfoField-flight-duration']").InnerText;

                    kiwiWebData.departureDate = toDestination.SelectSingleNode(".//div[@class='TripInfoField-date']").InnerText;
                    kiwiWebData.returnDate = fromDestination.SelectSingleNode(".//div[@class='TripInfoField-date']").InnerText;

                    kiwiWebData.departureTime = toDestination.SelectSingleNode(".//div[@class='TripInfoField-time']").InnerText;
                    kiwiWebData.returnTime = fromDestination.SelectSingleNode(".//div[@class='TripInfoField-time']").InnerText;

                    kiwiWebData.bookingLink = "https://www.kiwi.com" + node.SelectSingleNode(".//div[@class='JourneyBookingButtonLink']/a").GetAttributeValue("href", "notfound"); //Attributes["href"].Value;
                                                                                                                                                          //HtmlNode nonode = node.SelectSingleNode(".//div[@class='JourneyBookingButtonLink']/a");
                                                                                                                                                          //JourneyButtons

                    listHtmlData.Add(kiwiWebData);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.GetError());
                }
                

            }

            return listHtmlData;
        }
    }
}
