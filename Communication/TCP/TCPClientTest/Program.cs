using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TCPConnector;

namespace TCPClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new TCPClient();
            Console.WriteLine();
            var l = client.DnsLookup();
            return;
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                switch (keyInfo.Key)
                {
                    case ConsoleKey.C: client.Connect(); break;
                    case ConsoleKey.S: client.SendMessage("This is test message<EOF>"); break;
                    case ConsoleKey.R: client.ReceiveMessage(); break;
                    case ConsoleKey.E: client.CloseConnection(); break;
                    case ConsoleKey.Q: return;
                }
            }
        }
    }
}
