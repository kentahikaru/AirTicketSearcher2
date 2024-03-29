﻿namespace AirTicketSearcher.Pelikan
{
    using System;
    using System.Collections.Generic;
    using System.IO;
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
                    Console.WriteLine(ex.GetError());
                    File.WriteAllText("PelikanPage.html", html);
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

            if (nodes == null || nodes.Count == 0)
            {
                throw new Exception("No Nodes to analyze");
            }

            foreach (HtmlNode node in nodes)
            {
                try
                {
                    PelikanData pelikanbData = new PelikanData();

                    string price = node.SelectSingleNode(".//div[@class='fly-search-price-info-wrapp']").InnerText;//.GetAttributeValue;
                    price = price.Replace(" ", "");
                    int intPrice = int.Parse(price.Substring(0, price.Length - 2));
                    if (intPrice > this.config.maxPrice)
                        continue;

                    pelikanbData.price = price;

                    HtmlNodeCollection subnodesCollection = node.SelectNodes(".//div[@class='row fly-row no-mrg']");

                    foreach (HtmlNode subNode in subnodesCollection)
                    {
                        PelikanSubData pelikanSubData = new PelikanSubData();

                        pelikanSubData.Tolerance = subNode.SelectSingleNode(".//div[@class='fly-item-tolerance-new-reservation']")?.InnerHtml;
                        pelikanSubData.departureDay = subNode.SelectSingleNode(".//span[@class='fly-item-day-new-reservation']")?.InnerHtml;
                        pelikanSubData.departureDate = subNode.SelectSingleNode(".//div[@class='fly-item-one-trip-no-radio-new-reservation']")?.InnerText;
                        pelikanSubData.departureCity = subNode.SelectSingleNode(".//div[@class='first-dest-item']")?.SelectSingleNode(".//h1[@class='airport']")?.InnerHtml;
                        pelikanSubData.departureTime = subNode.SelectSingleNode(".//div[@class='first-dest-item']")?.SelectSingleNode(".//span[@class='fly-item-time-new-reservation active']")?.InnerHtml;
                        pelikanSubData.departureAirport = subNode.SelectSingleNode(".//div[@class='first-dest-item']")?.SelectSingleNode(".//div[@class='place-define']")?.InnerHtml;

                        pelikanSubData.durationToDestination = subNode.SelectSingleNode(".//div[@class='fly-item-arrow-new-reservation']/div").InnerText;

                        pelikanSubData.destinationCity = subNode.SelectSingleNode(".//div[@class='second-dest-item']")?.SelectSingleNode(".//h1[@class='airport']")?.InnerHtml;
                        pelikanSubData.destinationTime = subNode.SelectSingleNode(".//div[@class='second-dest-item']")?.SelectSingleNode(".//span[@class='fly-item-time-new-reservation active']")?.InnerHtml;
                        pelikanSubData.destinationAirport = subNode.SelectSingleNode(".//div[@class='second-dest-item']")?.SelectSingleNode(".//div[@class='place-define']")?.InnerHtml;

                        pelikanbData.listPelikanSubData.Add(pelikanSubData);
                    }

                    listHtmlData.Add(pelikanbData);
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
