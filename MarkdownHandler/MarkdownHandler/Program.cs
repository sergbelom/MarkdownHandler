using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using CommandLine;

namespace MarkdownHandler
{
    static class Logger
    {
        public static NLog.Logger Log;

        /// <summary>
        /// Create log instance 
        /// </summary>
        public static void CreateLog()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = Path.Combine(Directory.GetCurrentDirectory(),
                String.Concat(nameof(MarkdownHandler), DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss"), ".log"))};
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
            Logger.CreateLog();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
            Console.ReadKey();
        }

        private static int _filesRead;

        static void RunOptions(Options opts)
        {
            var mdFiles = opts.Files;
            Logger.Log.Info("Input Files= {0}", String.Join(",", mdFiles));
            Logger.Log.Info("Starting to read files! Count: {0}", mdFiles.Count());
            Parallel.ForEach(mdFiles, DoStuff);
            Logger.Log.Info("Finish! Read {0} file(s).", _filesRead);

        }

        static void HandleParseError(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
                Logger.Log.Error(error.Tag.ToString());
        }

        private static void DoStuff(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            Logger.Log.Info("[{0}] {1}: ", Thread.CurrentThread.ManagedThreadId, fileName);
            _filesRead++;
        }
    }
}