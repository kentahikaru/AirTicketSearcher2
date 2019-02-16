namespace AirTicketSearcher.Kiwi
{
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using System.Text;
    using AirTicketSearcher.TransportLayer;
    using Newtonsoft.Json;
    using AirTicketSearcher.Mail;

    public class Kiwi : ISearch
    {
        Configuration.Config config;
        public Kiwi(Configuration.Config config)
        {
            this.config = config;
        }

        public void Run()
        {
            Transport tr = new Transport();
            string url = ComposeUrl();
            Program.logger.Info(url);
            string result = tr.GetDataFromWeb(url);
            Program.logger.Info(result);
            KiwiRespond respond = JsonConvert.DeserializeObject<KiwiRespond>(result);
            string htmlMessage = MakeMailMessage(respond);
            Mail mail = new Mail(this.config.emailConfig);
            mail.SendEmail("AirTicketSearcher - Japan",htmlMessage);
        }

        private string ComposeUrl()
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
                  
                url += kvp.Key.ToString() + "=" + kvp.Value.ToString();
            }

            return url;
        }

        private string MakeMailMessage(KiwiRespond respond)
        {
            if(respond == null)
                return "";

        StringBuilder sb = new StringBuilder();
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

                    tr.AddCell("Personal weight: " + data.baggage.personal_item.weight.ToString());
                    tr.AddCell("Personal price: " + data.baggage.personal_item.price.ToString());
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


        return sb.ToString();
        }
    }
}