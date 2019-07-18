namespace AirTicketSearcher.Pelikan
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using HtmlAgilityPack;

    /// <summary>
    /// 
    /// </summary>
    public class PelikanAnalyzer
    {
        Configuration.Config config = null;

        public PelikanAnalyzer(Configuration.Config config)
        {
            this.config = config;
        }

        public List<List<PelikanData>> AnalyzeWebResults(List<string> htmlList)
        {
            List<List<PelikanData>> listOfListsPelikanData = new List<List<PelikanData>>();

            foreach (string html in htmlList)
            {
                try
                {
                    List<PelikanData> listPelikanData = AnalyzeHtml(html);
                    listOfListsPelikanData.Add(listPelikanData);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                }


            }

            return listOfListsPelikanData;
        }

        private List<PelikanData> AnalyzeHtml(string html)
        {

            List<PelikanData> listHtmlData = new List<PelikanData>();

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            //HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@class='Journey-overview Journey-return']");
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[contains(@id,'flight-1000')]");
            //HtmlNodeCollection nodes2 = doc.DocumentNode.SelectNodes("//div[@class='Journey clear spCard open']");

            string resulttext = "";
            foreach (HtmlNode node in nodes)
            {
                PelikanData pelikanbData = new PelikanData();
                HtmlNodeCollection subnodesCollection = node.SelectNodes(".//div[@class='row fly-row no-mrg']");
                HtmlNode toDestination = subnodesCollection[0];
                HtmlNode fromDestination = subnodesCollection[1];

                pelikanbData.price = node.SelectSingleNode(".//div[@class='fly-search-price-info-icon-wrapp']").InnerText;//.GetAttributeValue;
                //pelikanbData.lengthOfStay = node.SelectSingleNode(".//div[@class='Journey-nights-place']").InnerText;

                //pelikanbData.airlinesToDestination = toDestination.SelectSingleNode(".//div[@class='AirlineNames']").InnerText;
                //pelikanbData.airlinesFromDestination = fromDestination.SelectSingleNode(".//div[@class='AirlineNames']").InnerText;

                //pelikanbData.durationToDestination = toDestination.SelectSingleNode(".//div[@class='TripInfoField-flight-duration']").InnerText;
                //pelikanbData.durationFromDestination = fromDestination.SelectSingleNode(".//div[@class='TripInfoField-flight-duration']").InnerText;

                //pelikanbData.departureDate = toDestination.SelectSingleNode(".//div[@class='TripInfoField-date']").InnerText;
                //pelikanbData.returnDate = fromDestination.SelectSingleNode(".//div[@class='TripInfoField-date']").InnerText;

                //pelikanbData.departureTime = toDestination.SelectSingleNode(".//div[@class='TripInfoField-time']").InnerText;
                //pelikanbData.returnTime = fromDestination.SelectSingleNode(".//div[@class='TripInfoField-time']").InnerText;

                //pelikanbData.bookingLink = node.SelectSingleNode(".//div[@class='JourneyBookingButtonLink']/a").GetAttributeValue("href", "notfound"); //Attributes["href"].Value;
                //HtmlNode nonode = node.SelectSingleNode(".//div[@class='JourneyBookingButtonLink']/a");

                listHtmlData.Add(pelikanbData);

            }

            return listHtmlData;
        }

    }
}
