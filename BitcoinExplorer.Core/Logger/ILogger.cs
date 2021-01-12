using System;
using System.Collections.Generic;
using System.Text;

namespace BitcoinExplorer.Core.Logger
{
    interface ILogger
    {
        void LogHeader(string message);
        void Log(string message);
        void LogError(string message);
        void Log(IEnumerable<string> messages);
    }
}
