using System;
using System.Collections.Generic;
using System.Text;

namespace AirTicketSearcher
{
    public static class Utility
    {
        public static string GetError(this Exception ex)
        {
            //return ex.Message + Environment.NewLine + ex.StackTrace;

            string errorMsg = "";
            errorMsg = ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine;

            Exception innerException = ex.InnerException;

            while (innerException != null)
            {
                errorMsg += "Inner Exception ---> " + Environment.NewLine;
                errorMsg += innerException.Message + Environment.NewLine + innerException.StackTrace;
                innerException = innerException.InnerException;
            }

            return errorMsg;
        }
    }
}
