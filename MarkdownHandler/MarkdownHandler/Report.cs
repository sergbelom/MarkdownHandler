using System;
using System.Collections.Generic;

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
        
        public Report()
        {
            _data = new SortedDictionary<String, int[]>();
            _summary = new Dictionary<String, int>();
            //TODO:
            _summary.Add("count of files", 0);
            _summary.Add("count of images", 0);
            _summary.Add("count of tables", 0);
            _summary.Add("count of caption", 0);
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
                    "\nFile name: {0} \n\t count of lines: {1} \n\t count of images: {2} \n\t count of tables: {3} count of captions: {4}",
                    file.Key, file.Value[0], file.Value[1], file.Value[2], file.Value[3]);
            Console.WriteLine("\nSummary:");
            foreach (var line in _summary)
                Console.WriteLine("\t{0}: {1}", line.Key, line.Value);
        }
    }
}
