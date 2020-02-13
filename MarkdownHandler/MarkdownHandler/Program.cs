using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CommandLine;

namespace MarkdownHandler
{
    class Program
    {
        private static readonly Logger _logger = new Logger();

        private static int _countFilesRead;

        //todo: make better
        private static ICollection<String> _imageTags = new List<String>();

        private static ICollection<String> _tableTags = new List<String>();

        private static void Main(String[] args)
        {
            _logger.CreateLog();
            ReadAppSettings();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
            Console.ReadKey();
        }

        private static void ReadAppSettings()
        {
            _imageTags = SettingsReader.ReadAppSettings(nameof(_imageTags).Replace("_",""));
            _logger.Log.Info("Image Tags: {0}", String.Join(' ', _imageTags));
            _tableTags = SettingsReader.ReadAppSettings(nameof(_tableTags).Replace("_", ""));
            _logger.Log.Info("Table Tags: {0}", String.Join(' ', _tableTags));
        }

        private static void PrintReport(Dictionary<String, String> result)
        {
            Console.WriteLine("\n File name: {0} \n\t count of lines: {1} \n\t count of images: {2} \n\t count of tables: {3} \n",
                    result["fileName"], result["countLines"], result["countImages"], result["countTables"]);
        }

        private static void RunOptions(Options opts)
        {
            var mdFiles = opts.Files;
            _logger.Log.Info("Input Files= {0}", String.Join(", ", mdFiles));
            _logger.Log.Info("Starting to read files! Count: {0}", mdFiles.Count());
            Parallel.ForEach(mdFiles, StartProcess);
            _logger.Log.Info("Finish! Read {0} file(s).", _countFilesRead);

        }

        private static void HandleParseError(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
                _logger.Log.Error(error.Tag.ToString());
        }

        private static void StartProcess(String filePath)
        {
            //_logger.Log.Info("[{0}]: {1} ", Thread.CurrentThread.ManagedThreadId, filePath);
            CalcImageAndTable(filePath);
            _countFilesRead++;
        }

        private static void CalcImageAndTable(String filePath)
        {
            String[] lines = File.ReadAllLines(filePath);
            Dictionary<String, String> result = new Dictionary<string, string>();
            //_logger.Log.Info("[{0}] line count: {1} ", filePath, lines.Length);
            int countImage = 0;
            int countTable = 0;
            //todo: make better, may be to AppSettings
            char tagPrefix = '<';
            char tagPostfix = '>';

            foreach (var line in lines)
            {
                if (_imageTags.Any(s => line.Trim().StartsWith(String.Concat(tagPrefix, s))))
                {
                    countImage++;
                    //_logger.Log.Info(line);
                }
                if (_tableTags.Any(s => line.Trim().StartsWith(String.Concat(tagPrefix, s))))
                {
                    countTable++;
                    //_logger.Log.Info(line);
                }
            }
            _logger.Log.Info("File: {0} contains {1} lines, {2} images and {3} tables.", filePath, lines.Length, countImage, countTable);

            result.Add("fileName", filePath);
            result.Add("countLines", lines.Length.ToString());
            result.Add("countImages", countImage.ToString());
            result.Add("countTables", countTable.ToString());
            PrintReport(result);
        }
    }
}