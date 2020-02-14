using System.Collections.Generic;
using CommandLine;

namespace MarkdownHandler
{
    public class Options
    {
        /// <summary>
        /// Property for input files collection.
        /// </summary>
        [Option('f', "files", Required = true, HelpText = "Collection .md and .markdown files for processing.")]
        public IEnumerable<string> Files { get; set; }

        //TODO:
        /// <summary>
        /// Bool property for generate report about input files.
        /// </summary>
        //[Option('r', "report", Required = false, HelpText = "Get report about count tables and figures.")]
        //public bool Report { get; set; }

        //TODO:
        /// <summary>
        /// Bool property for check correct captions for figure and tables.
        /// </summary>
        //[Option('c', "check-captions", Required = false, HelpText = "Check correct captions for figure and tables.")]
        //public bool Check { get; set; }
    }
}
