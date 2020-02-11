using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace MarkdownHandler
{
    public class Options
    {
        [Option('h', "help", Default = false, Required = false, HelpText = "Help about console app.")]
        public bool Help { get; set; }

        [Option('r', "read", Required = false, HelpText = "Input files to be processed.")]
        public IEnumerable<string> InputFiles { get; set; }

        [Option('e', "count-elements", Required = false, HelpText = "Get report about count tables and figures.")]
        public bool Count { get; set; }

        [Option('c', "check-captions", Required = false, HelpText = "Check correct captions for figure and tables.")]
        public bool Check { get; set; }
    }

    //class CommandLineHandler
    //{
    //    //cmds:
    //    // --help - help 
    //    // --input "*.md" - start read .md and .markdown file from current dir
    //    // --countelem - подсчет суммарного количества изображений и таблиц в файлах;
    //    // --checkcaps - проверяет для каждого файла, есть ли у каждого изображения или таблицы подпись вида "Рисунок 1. ....", "Таблица 2. ....", выводит список неподписанных;
    //    // --quit - quit from console app
    //}
}
