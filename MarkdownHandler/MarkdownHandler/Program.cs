using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace MarkdownHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    if (o.Help)
                    {
                        Console.WriteLine($"Verbose output enabled. Current Arguments: -v {o.Help}");
                        Console.WriteLine("Quick Start Example! App is in Verbose mode!");
                    }
                    else
                    {
                        Console.WriteLine($"Current Arguments: -v {o.Help}");
                        Console.WriteLine("Quick Start Example!");
                    }
                });
        }

        static void RunOptions(Options opts)
        {
            if (opts.Help)
                PrintHelp();
            //handle options
        }
        static void HandleParseError(IEnumerable<Error> errs)
        {
            //handle errors
        }

        private static void PrintHelp()
        {
            var result = new StringBuilder();

            result.AppendLine("Hello, and welcome to the  console application.");
            result.AppendLine("This application takes in a data file and attempts to import that data into our systems.");
            result.AppendLine("Valid options are:");
            //result.AppendLine(HelpText.AutoBuild(args));
            result.AppendLine("Press any key to exit");

            Console.Write(result);
        }
    }
}