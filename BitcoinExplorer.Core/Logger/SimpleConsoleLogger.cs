using System;
using System.Collections.Generic;
using System.Text;

namespace BitcoinExplorer.Core.Logger
{
    public class SimpleConsoleLogger : ILogger
    {
        private const string HeaderBorder = "---------";
        private const string ErrorBorder = "!!Something went wrong!!";

        public void LogError(string message)
        {
            Console.WriteLine($"{ErrorBorder}");
            Console.WriteLine($"{message}");
            Console.WriteLine();
        }

        public void LogHeader(string message)
        {
            Console.WriteLine($"{HeaderBorder} {message} {HeaderBorder}");
        }

        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void Log(IEnumerable<string> messages)
        {
            foreach (var message in messages)
            {
                Log(message);
            }
        }
    }
}
