namespace AirTicketSearcher.Kiwi
{
    using System.Collections;
    using System.Collections.Generic;
    using AirTicketSearcher.TransportLayer;
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
    }
}