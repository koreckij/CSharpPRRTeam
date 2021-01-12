using BitcoinExplorer.Core;
using BitcoinExplorer.Core.Structs.Net;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using Version = BitcoinExplorer.Core.Structs.Net.Version;

namespace BitcoinExplorer.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Explorer.Explore();
            var explorer = new Explorer(Explorer.Explore().First(),8333);
            explorer.Handshake();
        }

    }
}
