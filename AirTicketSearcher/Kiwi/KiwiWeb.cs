//-----------------------------------------------------------------------
// <copyright file="KiwiWeb.cs" company="IXTENT s.r.o.">
//     Copyright 2019 IXTENT s.r.o.
// </copyright>
//-----------------------------------------------------------------------

namespace AirTicketSearcher.Kiwi
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using PuppeteerSharp;
    using AirTicketSearcher.Mail;

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

        public void Run()
        {
            string htmlMessage = "";
            try
            {
                KiwiWebReceiver kiwiWebReceiver = new KiwiWebReceiver(this.config);
                KiwiWebAnalyzer kiwiWebAnalyzer = new KiwiWebAnalyzer(this.config);

                List<string> resultList = kiwiWebReceiver.GetWebResults(this.config.monthsToLookFor);
                List<List<KiwiWebData>> listOfListOfResults = kiwiWebAnalyzer.AnalyzeWebResults(resultList);
                htmlMessage = MakeMailMessage(listOfListOfResults);
            }
            catch(Exception ex)
            {
                htmlMessage = ex.GetError();
            }

            Mail mail = new Mail(this.config.emailConfig);
            mail.SendEmail(this.config.kiwiWebConfig.emailSubject, htmlMessage);

        }

        private string MakeMailMessage(List<List<KiwiWebData>> listOfListOfResults)
        {
            if (listOfListOfResults.Count == 0)
                return "";

            StringBuilder sb = new StringBuilder();
            foreach(List<KiwiWebData> listOfResults in listOfListOfResults)
            {
                using (Html.Table table = new Html.Table(sb, id: "kiwiWebTable"))
                {
                    table.StartBody();

                    using (var tr = table.AddRow())
                    {
                        tr.AddCell("Price");
                        tr.AddCell("lengthOfStay");
                        tr.AddCell("Airlines to destination");
                        tr.AddCell("Airlines from destination");
                        tr.AddCell("Duration to destination");
                        tr.AddCell("Duration from destination");
                        tr.AddCell("Departure date");
                        tr.AddCell("Return date");
                        tr.AddCell("Departure time");
                        tr.AddCell("Return time");
                        tr.AddCell("Booking link");
                    }

                    foreach(KiwiWebData data in listOfResults)
                    {
                        using (var tr = table.AddRow())
                        {
                            tr.AddCell(data.price.ToString());
                            tr.AddCell(data.lengthOfStay);
                            tr.AddCell(data.airlinesToDestination);
                            tr.AddCell(data.airlinesFromDestination);
                            tr.AddCell(data.durationToDestination);
                            tr.AddCell(data.durationFromDestination);
                            tr.AddCell(data.departureDate);
                            tr.AddCell(data.returnDate);
                            tr.AddCell(data.departureTime);
                            tr.AddCell(data.returnTime);
                            tr.AddCell(data.bookingLink);
                        }
                    }

                    table.EndBody();
                }
                sb.Append("</br>");
                
            }

            return sb.ToString();
        }

    }
}
