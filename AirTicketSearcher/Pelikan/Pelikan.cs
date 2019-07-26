namespace AirTicketSearcher.Pelikan
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using AirTicketSearcher.Mail;

    /// <summary>
    /// 
    /// </summary>
    public class Pelikan : ISearch
    {
        Configuration.Config config;
        public Pelikan(Configuration.Config config)
        {
            this.config = config;
        }

        public void Run()
        {
            string htmlMessage = "";
            try
            {
                PelikanReceiver pelikanReceiver = new PelikanReceiver(this.config);
                PelikanAnalyzer pelikanAnalyzer = new PelikanAnalyzer(this.config);

                List<string> resultList = pelikanReceiver.GetWebResults(this.config.monthsToLookFor);
                List<List<PelikanData>> listOfListOfResults = pelikanAnalyzer.AnalyzeWebResults(resultList);

                htmlMessage = MakeMailMessage(listOfListOfResults);
            }
            catch(Exception ex)
            {
                htmlMessage = ex.GetError();
            }
            
            Mail mail = new Mail(this.config.emailConfig);
            mail.SendEmail(this.config.pelikanConfig.emailSubject, htmlMessage);

        }

        private string MakeMailMessage(List<List<PelikanData>> listOfListOfResults)
        {
            if (listOfListOfResults.Count == 0)
                return "";

            StringBuilder sb = new StringBuilder();
            foreach (List<PelikanData> listOfResults in listOfListOfResults)
            {
                using (Html.Table table = new Html.Table(sb, id: "kiwiWebTable"))
                {
                    table.StartBody();

                    using (var tr = table.AddRow())
                    {
                        tr.AddCell("Price");
                        tr.AddCell("Tolerance");
                        tr.AddCell("departureDay");

                        tr.AddCell("Departure City");
                        tr.AddCell("Departure airport");
                        tr.AddCell("Departure date");
                        tr.AddCell("Departure time");
                        
                        tr.AddCell("Duration to destination");

                        tr.AddCell("Destination City");
                        tr.AddCell("Destination airport");
                        tr.AddCell("Destination time");
                        
                    }

                    foreach (PelikanData data in listOfResults)
                    {
                        using (var tr = table.AddRow())
                        {
                            tr.AddCell(data.price.ToString(),"","","4");

                            foreach(PelikanSubData subData in data.listPelikanSubData)
                            {
                                using (var tr2 = table.AddRow())
                                {
                                    tr2.AddCell(data.price.ToString(),"","","");
                                    tr2.AddCell(subData.Tolerance);
                                    tr2.AddCell(subData.departureDay);
                                    tr2.AddCell(subData.departureCity);
                                    tr2.AddCell(subData.departureAirport);
                                    tr2.AddCell(subData.departureDate);
                                    tr2.AddCell(subData.departureTime);
                                    tr2.AddCell(subData.durationToDestination);
                                    tr2.AddCell(subData.destinationCity);
                                    tr2.AddCell(subData.destinationAirport);
                                    tr2.AddCell(subData.destinationTime);
                                    
                                }
                            }
                            
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
