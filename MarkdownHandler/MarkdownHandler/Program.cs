using NLog;
using System;
using System.IO;
using System.Collections.Generic;
using CommandLine;

namespace MarkdownHandler
{
    static class Logger
    {
        public static NLog.Logger Log;

        /// <summary>
        /// Create text log file.
        /// </summary>
        public static void CreateLogFile()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = Path.Combine(Directory.GetCurrentDirectory(),
                String.Concat( nameof(MarkdownHandler), DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss"), ".log")) };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
            LogManager.Configuration = config;
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("Log created!");
            Log = logger;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Logger.CreateLogFile();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
            Console.ReadKey();
        }

        static void RunOptions(Options opts)
        {
            var files = opts.Files;
            Logger.Log.Info("Input Files= {0}", String.Join(",", files));
        }

        static void HandleParseError(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
                Logger.Log.Error(error.Tag.ToString());
        }
    }
}