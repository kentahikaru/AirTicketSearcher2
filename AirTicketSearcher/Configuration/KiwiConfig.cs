namespace AirTicketSearcher.Configuration
{
    using System.Collections;
    using System.Collections.Generic;

    public class KiwiConfig
    {
        public string baseUrl = "";
        public string emailSubject;
        // Parameters from  https://docs.kiwi.com/#flights-flights-get
        public Dictionary<string,string> kiwiUrlParameters = new Dictionary<string, string>();
    }
}