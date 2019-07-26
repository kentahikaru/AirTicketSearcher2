using System;
using System.Collections.Generic;
using System.Text;

namespace AirTicketSearcher.Common
{
    public static class Utility
    {
        public static string GetError(this Exception ex)
        {
            return ex.Message + Environment.NewLine + ex.StackTrace;
        }
    }
}
