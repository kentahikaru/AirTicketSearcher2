using System;
using System.Collections.Generic;
using System.Text;

namespace AirTicketSearcher.Pelikan
{
    public class PelikanData
    {
        public string price;
        public string bookingLink;

        public List<PelikanSubData> listPelikanSubData = new List<PelikanSubData>();
    }
}
