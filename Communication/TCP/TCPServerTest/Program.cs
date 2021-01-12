using System;
using TCPConnector;

namespace TCPServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new TCPServer();
            server.StartListening();
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                switch (keyInfo.Key)
                {
                    case ConsoleKey.A: server.AcceptConnection(); break;
                    case ConsoleKey.S: server.SendMessage("This is test message<EOF>"); break;
                    case ConsoleKey.R: server.ReceiveMessage(); break;
                    case ConsoleKey.E: server.CloseConnection(); break;
                    case ConsoleKey.Q: return;
                }
            }
        }
    }
}
