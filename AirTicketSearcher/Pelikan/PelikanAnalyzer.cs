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
                pelikanbData.price = node.SelectSingleNode(".//div[@class='fly-search-price-info-wrapp']").InnerText;//.GetAttributeValue;


                HtmlNodeCollection subnodesCollection = node.SelectNodes(".//div[@class='row fly-row no-mrg']");

                foreach(HtmlNode subNode in subnodesCollection)
                {
                    PelikanSubData pelikanSubData = new PelikanSubData();

                    pelikanSubData.Tolerance = subNode.SelectSingleNode("//div[@class='fly-item-tolerance-new-reservation']")?.InnerHtml;
                    pelikanSubData.departureDay = subNode.SelectSingleNode("//span[@class='fly-item-day-new-reservation']").InnerHtml;
                    pelikanSubData.departureCity = subNode.SelectSingleNode("//div[@class='first-dest-item']").SelectSingleNode(".//h1[@class='airport']").InnerHtml;
                    pelikanSubData.departureTime = subNode.SelectSingleNode("//div[@class='first-dest-item']").SelectSingleNode(".//span[@class='fly-item-time-new-reservation active']").InnerHtml;
                    pelikanSubData.departureAirport = subNode.SelectSingleNode("//div[@class='first-dest-item']").SelectSingleNode(".//div[@class='place-define']").InnerHtml;

                    pelikanSubData.durationToDestination = subNode.SelectSingleNode("//div[@class='fly-item-arrow-new-reservation']/div").InnerText;

                    pelikanSubData.destinationCity = subNode.SelectSingleNode("//div[@class='second-dest-item']").SelectSingleNode(".//h1[@class='airport']").InnerHtml;
                    pelikanSubData.destinationTime = subNode.SelectSingleNode("//div[@class='second-dest-item']").SelectSingleNode(".//span[@class='fly-item-time-new-reservation active']").InnerHtml;
                    pelikanSubData.destinationAirport = subNode.SelectSingleNode("//div[@class='second-dest-item']").SelectSingleNode(".//div[@class='place-define']").InnerHtml;

                    pelikanbData.listPelikanSubData.Add(pelikanSubData);
                }

                listHtmlData.Add(pelikanbData);

            }

            return listHtmlData;
        }

    }
}
