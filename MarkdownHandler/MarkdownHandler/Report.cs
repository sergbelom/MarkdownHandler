using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace MarkdownHandler
{
    class Report
    {
        private IDictionary<String, StringCollection> _data;

        public IDictionary<String, StringCollection> Data
        {
            get => _data;
            set { _data = value; }
        }

        private bool _isFinished;

        public bool IsFinished
        {
            get => _isFinished;
            set { _isFinished = value; }
        }

        public Report()
        {
            _data = new Dictionary<string, StringCollection>();
            _isFinished = false;
        }

        public void PrintReport()
        {
            if (_data.Count == 0)
            {
                Console.WriteLine("Data for report is empty!");
                return;
            }

            //todo: make soooooooooooooooooooooooort data before print
            foreach (var file in _data)
            {
                Console.WriteLine(
                    "\n File name: {0} \n\t count of lines: {1} \n\t count of images: {2} \n\t count of tables: {3} \n",
                    file.Key, file.Value[0], file.Value[1], file.Value[2]);
            }
        }
    }
}