namespace AirTicketSearcher.Configuration
{
    using System;
    using System.IO;
    using NLog;
    using NLog.Common;
    using NLog.Config;
    using NLog.Targets;
    //using Nlog.MailTarget;

    public static class NLogConfigManager
    {
        public static Logger GetLogger()
        {
            Logger logger;
            

            LoggingConfiguration loggingConfig = new LoggingConfiguration();
            // File Target
            var fileTarget = new FileTarget("fileTarget")
            {
                Layout = "${longdate} ${uppercase:${level}} ${message}",
                FileName = "${basedir}/logs/${shortdate}.log"
            };

            // File Rule
            loggingConfig.AddTarget(fileTarget);
            loggingConfig.AddRule(LogLevel.Debug,LogLevel.Info, fileTarget, "*");
            
            // Console Target
            var consoleTarget = new ColoredConsoleTarget("consoleTarget")
            {
                Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception}"
            };
            
            // Console Rule
            loggingConfig.AddTarget(consoleTarget);
            loggingConfig.AddRuleForAllLevels(consoleTarget);

            NLog.LogManager.Configuration = loggingConfig;
            NLog.LogManager.ThrowConfigExceptions = true;
            NLog.LogManager.ThrowExceptions = true;
            
            InternalLogger.LogToConsole = true;
            InternalLogger.LogFile = "logs/internal.log";
            InternalLogger.LogLevel = LogLevel.Trace;

            logger = NLog.LogManager.GetLogger("myLogger");

            return logger;

        }
    }
}