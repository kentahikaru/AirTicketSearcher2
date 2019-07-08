//-----------------------------------------------------------------------
// <copyright file="KiwiWeb.cs" company="IXTENT s.r.o.">
//     Copyright 2019 IXTENT s.r.o.
// </copyright>
//-----------------------------------------------------------------------

namespace AirTicketSearcher.Kiwi
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using PuppeteerSharp;
    using System.IO;

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
            KiwiWebReceiver kiwiWebReceiver = new KiwiWebReceiver(this.config);
            KiwiWebAnalyzer kiwiWebAnalyzer = new KiwiWebAnalyzer(this.config);

            List<string> resultList = kiwiWebReceiver.GetWebResults(this.config.kiwiWebConfig.monthsToLookFor);
            kiwiWebAnalyzer.AnalyzeWebResults(resultList);

        }

       
    }
}
