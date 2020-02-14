using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MarkdownHandler
{
    class MarkdownParser
    {
        private string _pathToFile;

        public string PathToFile
        {
            get => _pathToFile;
            set { _pathToFile = value; }
        }

        public MarkdownParser(string pathToFile)
        {
            this._pathToFile = pathToFile;
        }
    }
}
