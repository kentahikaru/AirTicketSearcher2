namespace AirTicketSearcher.Kiwi
{
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using System.Text;
    using Newtonsoft.Json;
    using AirTicketSearcher.Mail;
    using AirTicketSearcher.TransportLayer;
    using AirTicketSearcher.Common;

    public class Kiwi : ISearch
    {
        Configuration.Config config;
        public Kiwi(Configuration.Config config)
        {
            this.config = config;
        }

        public void Run()
        {
            string htmlMessage = "";

            try
            {
                List<KiwiRespond> listRespond = GetResults();
                htmlMessage = MakeMailMessage(listRespond);
            }
            catch(Exception ex)
            {
                htmlMessage = ex.GetError();
            }


            Mail mail = new Mail(this.config.emailConfig);
            mail.SendEmail(this.config.kiwiConfig.emailSubject, htmlMessage);
        }

        private List<KiwiRespond> GetResults()
        {
            Transport tr = new Transport();
            string[] destinations = GetDestinations();
            List<KiwiRespond> listResponses = new List<KiwiRespond>();

            foreach(string destination in destinations)
            {
                try
                {
                string url = ComposeUrl(destination);
                Program.logger.Info(url);
                string result = tr.GetDataFromWeb(url);
                //Program.logger.Info(result);
                KiwiRespond respond = JsonConvert.DeserializeObject<KiwiRespond>(result);
                if(respond.data.Count > 0)
                    listResponses.Add(respond);
                }
                catch(Exception ex)
                {
                    NLog.Logger logger = NLog.LogManager.GetLogger("myLogger");

                    Console.WriteLine("Error happened");
                    Console.WriteLine(ex.Message);
                    logger.Error(ex.Message);
                    
                    Exception innerException = ex.InnerException;

                    while (innerException != null)
                    {
                        logger.Error("Inner Exception: " + Environment.NewLine + innerException.Message);
                        logger.Error(innerException.StackTrace);
                        innerException = innerException.InnerException;
                    }
                }
            }
            
            return listResponses;
        }

        private string[] GetDestinations()
        {
            return this.config.kiwiConfig.kiwiUrlParameters["fly_to"].Split(',');
        }

        private string ComposeUrl(string destination)
        {
            string url = config.kiwiConfig.baseUrl;

            bool first = true;
            foreach(KeyValuePair<string,string> kvp in config.kiwiConfig.kiwiUrlParameters)
            {
                if(first)
                {
                    url += "?"; 
                    first = false;
                }
                else
                {
                    url += "&";
                }
                  
                if(kvp.Key.ToString().Equals("fly_to"))
                {
                    url += kvp.Key.ToString() + "=" + destination;
                }
                else
                {
                    url += kvp.Key.ToString() + "=" + kvp.Value.ToString();
                }
            }

            return url;
        }

        private string MakeMailMessage(List<KiwiRespond> listResponds)
        {
            if(listResponds.Count == 0)
                return "";

            StringBuilder sb = new StringBuilder();
            foreach(KiwiRespond respond in listResponds)
            {
                
                using(Html.Table table = new Html.Table(sb, id: "kiwiTabule"))
                {
                    table.StartBody();

                 
                    using(var tr = table.AddRow())
                    {   
                        tr.AddCell("Price");
                        tr.AddCell("Currency");
                        tr.AddCell("From");
                        tr.AddCell("To");
                        tr.AddCell("Flight duration");
                        tr.AddCell("Return flight duration");
                        tr.AddCell("Bags price");
                        tr.AddCell("Bag hand limit");
                        tr.AddCell("Bag hold limit");
                        tr.AddCell("Deep_link");
                    }

                    foreach(Data data in respond.data)
                    {
                        using(var tr = table.AddRow())
                        {
                            tr.AddCell("Destination: " + data.countryTo.name + " : " + data.cityTo,"","","6");
                        }
                        using(var tr = table.AddRow())
                        {
                            tr.AddCell(data.price.ToString());
                            tr.AddCell(respond.currency);
                            tr.AddCell(data.flyFrom);
                            tr.AddCell(data.flyTo);
                            tr.AddCell(data.fly_duration);
                            tr.AddCell(data.return_duration);
                            tr.AddCell(data.bags_price.__invalid_name__1.ToString());
                            tr.AddCell("Hand weight: " + data.baglimit.hand_weight.ToString());
                            tr.AddCell("Hold weight: " + data.baglimit.hold_weight.ToString());
                            tr.AddCell(data.deep_link);
                        }

                    using(var tr = table.AddRow())
                        {
                            foreach(List<string> routes in data.routes)
                            {
                                string oneRoute = "";
                                foreach(string route in routes)
                                {
                                    oneRoute += route + "-";
                                }
                                tr.AddCell("Route: " + oneRoute);
                                
                            }

                            tr.AddCell("Personal weight: " + data.baggage?.personal_item?.weight.ToString());
                            tr.AddCell("Personal price: " + data.baggage?.personal_item?.price.ToString());
                            tr.AddCell("Hand weight: " + data.baggage.hand.weight.ToString());
                            tr.AddCell("Hand price: " + data.baggage.hand.price.ToString());
                            foreach(Hold hold in data.baggage.hold)
                            {
                                tr.AddCell("Hold weight: " + hold.weight);
                                tr.AddCell("Hold price: " + hold.price);
                            }
                        }

                        using(var tr = table.AddRow())
                        {
                            tr.AddCell("Routes","","","4");
                        }

                        foreach(Route route in data.route)
                        {
                            using(var tr = table.AddRow())
                            {

                                tr.AddCell("Route Price: " + route.price.ToString());
                                tr.AddCell(respond.currency);
                                tr.AddCell("Route from: " + route.cityFrom + " - " + route.flyFrom);
                                tr.AddCell("Route to: " + route.cityTo + " - " + route.flyTo);
                                DateTime time = DateTime.UnixEpoch.AddSeconds(route.dTime);
                                tr.AddCell("Departure time: " + time.ToString());
                                //time = DateTime.UnixEpoch.AddSeconds(route.dTimeUTC);
                                //tr.AddCell("Departure timeUtc: " + time.ToString());
                                time = DateTime.UnixEpoch.AddSeconds(route.aTime);
                                tr.AddCell("Arrival time: " + time.ToString());
                                //time = DateTime.UnixEpoch.AddSeconds(route.aTimeUTC);
                                //tr.AddCell("Arrival timeUtc: " + time.ToString());
                                
                            }
                        }

                    
                        
                        using(var tr = table.AddRow())
                        {
                            tr.AddCell("°°°°");
                        }
                    }
                    table.EndBody();


                }
            }


            return sb.ToString();
        }
    }
}