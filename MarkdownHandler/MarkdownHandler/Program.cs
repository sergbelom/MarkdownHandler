using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using CommandLine;

namespace MarkdownHandler
{
    class Program
    {
        private static readonly Logger _logger = new Logger();

        private static Report _report = new Report();

        private static int _countFilesRead;

        //todo: make better
        private static ICollection<String> _imageTags = new List<String>();

        private static ICollection<String> _tableTags = new List<String>();

        private static void Main(String[] args)
        {
            _logger.CreateLog();
            ReadAppSettings(); //todo: add appsetting for ext file
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
            //if (_report.IsFinished)
            //{
            //    Console.WriteLine("Input -r for print report");
            //    var cmd = Console.Read().ToString().Split();
            //    Parser.Default.ParseArguments<Options>(cmd)
            //        .WithParsed(RunOptions);
            //}
            Console.ReadKey();
        }

        private static void ReadAppSettings()
        {
            _imageTags = SettingsReader.ReadAppSettings(nameof(_imageTags).Replace("_",""));
            _logger.Log.Info("Image Tags: {0}", String.Join(' ', _imageTags));
            _tableTags = SettingsReader.ReadAppSettings(nameof(_tableTags).Replace("_", ""));
            _logger.Log.Info("Table Tags: {0}", String.Join(' ', _tableTags));
        }

        private static void RunOptions(Options opts)
        {
            if (opts.Files.Count() != 0)
            {
                var mdFiles = opts.Files.ToList(); //todo: make delete dublicate
                _logger.Log.Info("Input Files= {0}", String.Join(", ", mdFiles));
                _logger.Log.Info("Starting to read files! Count: {0}", mdFiles.Count());

                _report.Summary.Add("count of files", mdFiles.Count());
                _report.Summary.Add("count of images", 0);
                _report.Summary.Add("count of tables", 0);

                Parallel.ForEach(mdFiles, StartProcess);
                _logger.Log.Info("Finish! Read {0} file(s).", _countFilesRead);
                _report.IsFinished = true;
                if (opts.Report && _report.IsFinished)
                    _report.PrintReport();
            }
            //if (opts.Report && _report.IsFinished)
            //    _report.PrintReport();
        }

        private static void HandleParseError(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
                _logger.Log.Error(error.Tag.ToString());
        }

        private static void StartProcess(String filePath)
        {
            _logger.Log.Info("[{0}]: {1} ", Thread.CurrentThread.ManagedThreadId, filePath);
            CalcImageAndTable(filePath);
            _countFilesRead++;
        }

        //todo: make class
        private static void CalcImageAndTable(String filePath)
        {
            if (!File.Exists(filePath))
            {
                _logger.Log.Warn("File: {0} not exist!", filePath);
                return;
            }

            String[] lines = File.ReadAllLines(filePath);
            int countImage = 0;
            int countTable = 0;
            //todo: make better, may be to AppSettings
            char tagPrefix = '<';
            char tagPostfix = '>';

            foreach (var line in lines)
            {
                if (_imageTags.Any(s => line.Trim().StartsWith(String.Concat(tagPrefix, s))))
                    countImage++;
                if (_tableTags.Any(s => line.Trim().StartsWith(String.Concat(tagPrefix, s))))
                    countTable++;
            }
            _logger.Log.Info("File: {0} contains {1} lines, {2} images and {3} tables.", filePath, lines.Length, countImage, countTable);
            _report.Data.Add(filePath, new int[] {lines.Length,countImage,countTable});
            _report.Summary["count of images"] += countImage;
            _report.Summary["count of tables"] += countTable;
        }
    }
}