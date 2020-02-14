using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using CommandLine;
using System.Collections.Specialized;

namespace MarkdownHandler
{
    class Program
    {
        private static readonly Logger _logger = new Logger();

        private static Report _report = new Report();

        private static int _countFilesRead;

        //todo: make better
        private static ICollection<String> _imageTags = ReadAppSettings(nameof(_imageTags));

        private static ICollection<String> _tableTags = ReadAppSettings(nameof(_tableTags));

        private static ICollection<String> _captionTags = ReadAppSettings(nameof(_captionTags));

        private static ICollection<String> _captionContent = ReadAppSettings(nameof(_captionContent));

        private static void Main(String[] args)
        {
            _logger.CreateLog();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
            Console.ReadKey();
        }

        private static List<String> ReadAppSettings(String tagName)
        {
            tagName = tagName.Replace("_", "");
            List<String> result = SettingsReader.ReadAppSettings(tagName).ToList();
            return result;
        }

        private static void RunOptions(Options opts)
        {
            if (opts.Files.Count() != 0)
            {
                var mdFiles = opts.Files.ToList(); //todo: make delete dublicate
                _logger.Log.Info("Input Files: {0}", String.Join(", ", mdFiles));
                _logger.Log.Info("Starting to read files! Count: {0}", mdFiles.Count());

                _report.Summary.Add("count of files", mdFiles.Count());
                _report.Summary.Add("count of images", 0);
                _report.Summary.Add("count of tables", 0);

                Parallel.ForEach(mdFiles, StartProcess);
                _logger.Log.Info("Finish! Read {0} file(s).", _countFilesRead);
                _report.IsFinished = true;
                _report.PrintReport();
            }
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
            int countCaption = 0;
            //todo: make better, may be to AppSettings
            char tagPrefix = '<';
            char tagPostfix = '>';

            bool captionCheck = false;

            foreach (var line in lines)
            {
                if (captionCheck && _captionTags.Any(s => line.Trim().StartsWith(String.Concat(tagPrefix, s, tagPostfix))))
                {
                    if (IsCorrectCaption(line))
                    {
                        countCaption++;
                    }
                    continue;
                }
                if (_imageTags.Any(s => line.Trim().StartsWith(String.Concat(tagPrefix, s))))
                {
                    countImage++;
                    captionCheck = true;
                }
                else if (_tableTags.Any(s => line.Trim().StartsWith(String.Concat(tagPrefix, s))))
                {
                    countTable++;
                    captionCheck = true;
                }
            }
            _logger.Log.Info("File: {0} contains {1} lines, {2} images, {3} tables and {4} caption",
                filePath, lines.Length, countImage, countTable, countCaption);
            _report.Data.Add(filePath, new int[] {lines.Length, countImage, countTable, countCaption});
            _report.Summary["count of images"] += countImage;
            _report.Summary["count of tables"] += countTable;
            _report.Summary["count of caption"] += countCaption;
        }

        private static bool IsCorrectCaption(String line)
        {
            return _captionContent.Any(s => line.Contains(s));
        }
    }
}