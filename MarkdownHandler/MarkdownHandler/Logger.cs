using System;
using System.IO;
using NLog;

namespace MarkdownHandler
{
    class Logger
    {
        public NLog.Logger Log;

        /// <summary>
        /// Create log instance.
        /// </summary>
        public Logger()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logfile = new NLog.Targets.FileTarget("logfile")
            {
                FileName = Path.Combine(Directory.GetCurrentDirectory(),
                    String.Concat(nameof(MarkdownHandler), DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss"), ".log"))
            };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
            LogManager.Configuration = config;
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("Log created!");
            Log = logger;
        }
    }
}
