using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace MarkdownHandler
{
    class Report
    {
        private SortedDictionary<String, int[]> _data;

        public SortedDictionary<String, int[]> Data
        {
            get => _data;
            set { _data = value; }
        }

        private IDictionary<String, int> _summary;

        public IDictionary<String, int> Summary
        {
            get => _summary;
            set { _summary = value; }
        }

        private bool _isFinished;

        public bool IsFinished
        {
            get => _isFinished;
            set { _isFinished = value; }
        }

        public Report()
        {
            _data = new SortedDictionary<String, int[]>();
            _summary = new Dictionary<String, int>();
            _isFinished = false;
        }

        public void PrintReport()
        {
            if (_data.Count == 0)
            {
                Console.WriteLine("Data for report is empty!");
                return;
            }
            Console.WriteLine("\nReport:");
            foreach (var file in _data)
                Console.WriteLine(
                    "\nFile name: {0} \n\t count of lines: {1} \n\t count of images: {2} \n\t count of tables: {3}",
                    file.Key, file.Value[0], file.Value[1], file.Value[2]);
            Console.WriteLine("\nSummary:");
            foreach (var line in _summary)
                Console.WriteLine("\t{0}: {1}", line.Key, line.Value);
        }
    }
}