namespace AirTicketSearcher.Pelikan
{
    using System;
    using System.Collections.Generic;
    using System.Text;

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
            PelikanReceiver pelikanReceiver = new PelikanReceiver(this.config);
            PelikanAnalyzer pelikanAnalyzer = new PelikanAnalyzer(this.config);

            List<string> resultList = pelikanReceiver.GetWebResults(this.config.monthsToLookFor);
            List<List<PelikanData>> listOfListOfResults = pelikanAnalyzer.AnalyzeWebResults(resultList);
        }


    }
}
