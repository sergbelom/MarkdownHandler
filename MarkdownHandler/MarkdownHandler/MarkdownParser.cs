using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MarkdownHandler
{
    class MarkdownParser
    {
        //TODO:
        private static readonly Logger _logger = Program.logger;

        private static readonly Report _report = Program.report;

        private static readonly ICollection<String> _imageTags = SettingsReader.ReadAppSettings(nameof(_imageTags));

        private static readonly ICollection<String> _tableTags = SettingsReader.ReadAppSettings(nameof(_tableTags));

        private static readonly ICollection<String> _captionTags = SettingsReader.ReadAppSettings(nameof(_captionTags));

        private static readonly ICollection<String> _captionContent = SettingsReader.ReadAppSettings(nameof(_captionContent));

        private string _pathToFile;
        
        //TODO:
        private char _tagPrefix = '<';

        private char _tagPostfix = '>';

        /// <summary>
        /// MarkdownParser instance.
        /// </summary>
        /// <param name="pathToFile">Path to checking file.</param>
        public MarkdownParser(string pathToFile)
        {
            this._pathToFile = pathToFile;
        }

        /// <summary>
        /// Calculate count of images, tables and check correct captions.
        /// </summary>
        public void CheckFile()
        {
            if(!CheckExistFile())
                return;
            var lines = ReadFile();

            int countImage = 0;
            int countTable = 0;
            int countCaption = 0;
            bool captionCheck = false;

            foreach (var line in lines)
            {
                if (captionCheck && _captionTags.Any(s => line.Trim().StartsWith(String.Concat(_tagPrefix, s, _tagPostfix))))
                {
                    if (_captionContent.Any(s => line.Contains(s)))
                        countCaption++;
                    captionCheck = false;
                    continue;
                }
                if (_imageTags.Any(s => line.Trim().StartsWith(String.Concat(_tagPrefix, s))))
                {
                    countImage++;
                    captionCheck = true;
                }
                else if (_tableTags.Any(s => line.Trim().StartsWith(String.Concat(_tagPrefix, s))))
                {
                    countTable++;
                    captionCheck = true;
                }
            }
            _logger.Log.Info("File: {0} contains {1} lines, {2} images, {3} tables and {4} caption",
                _pathToFile, lines.Length, countImage, countTable, countCaption);
            _report.Data.Add(_pathToFile, new int[] { lines.Length, countImage, countTable, countCaption });
            //TODO:
            _report.Summary["count of images"] += countImage;
            _report.Summary["count of tables"] += countTable;
            _report.Summary["count of caption"] += countCaption;
        }

        private bool CheckExistFile()
        {
            if (!File.Exists(_pathToFile))
            {
                _logger.Log.Warn("File: {0} not exist!", _pathToFile);
                return false;
            }
            return true;
        }

        private String[] ReadFile()
        {
            String[] lines = null;
            try
            {
                lines = File.ReadAllLines(_pathToFile);
            }
            catch (Exception)
            {
                _logger.Log.Warn("File: {0} not not readable!", _pathToFile);
                return null;
            }
            return lines;
        }
    }
}
