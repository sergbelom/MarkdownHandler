using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using CommandLine;

namespace MarkdownHandler
{
    class Program
    {
        public static readonly Logger logger = new Logger();

        public static readonly Report report = new Report();

        private static int _countFilesRead;

        private static void Main(String[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
            Console.ReadKey();
        }

        private static void RunOptions(Options opts)
        {
            if (opts.Files.Count() != 0)
            {
                var mdFiles = opts.Files.ToList();
                logger.Log.Info("Input Files: {0}", String.Join(", ", mdFiles));
                logger.Log.Info("Starting to read files! Count: {0}", mdFiles.Count());
                Parallel.ForEach(mdFiles, StartProcess);
                logger.Log.Info("Finish! Read {0} file(s).", _countFilesRead);
                report.Summary["count of files"] = _countFilesRead;
                report.PrintReport();
            }
        }

        private static void HandleParseError(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
                logger.Log.Error(error.Tag.ToString());
        }
        
        private static void StartProcess(String filePath)
        {
            logger.Log.Info("[{0}]: {1} ", Thread.CurrentThread.ManagedThreadId, filePath);
            MarkdownParser markdownParser = new MarkdownParser(filePath);
            markdownParser.CheckFile();
            _countFilesRead++;
        }
    }
}
