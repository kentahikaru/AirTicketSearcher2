using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

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
                List<KiwiWebData> listKiwiWebData = AnalyzeHtml(html);
                listOfListsKiwiWebData.Add(listKiwiWebData);

            }

            return listOfListsKiwiWebData;
        }

        private List<KiwiWebData> AnalyzeHtml(string html)
        {

            List<KiwiWebData> listHtmlData = new List<KiwiWebData>();

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class='Journey-overview Journey-return']");
            string resulttext = "";
            foreach (HtmlNode node in nodes)
            {
                KiwiWebData kiwiWebData = new KiwiWebData();
                kiwiWebData.price = node.SelectSingleNode(".//div[@class='JourneyInfoStyles__JourneyInfoPrice-vpsxn5-2 gnDWaH']").InnerText;//.GetAttributeValue;
                kiwiWebData.lengthOfStay = node.SelectSingleNode(".//div[@class='Journey-nights-place']").InnerText;
                
                kiwiWebData.airlinesToDestination = node.SelectSingleNode(".//div[@class='AirlineNames']").InnerText;
                kiwiWebData.airlinesFromDestination = node.SelectSingleNode(".//div[@class='AirlineNames']").InnerText;

                kiwiWebData.durationToDestination = node.SelectSingleNode(".//div[@class='TripInfoField-flight-duration']").InnerText;
                kiwiWebData.durationFromDestination = node.SelectSingleNode(".//div[@class='TripInfoField-flight-duration']").InnerText;
                
                listHtmlData.Add(kiwiWebData);

            }

            return listHtmlData;
        }
    }
}
