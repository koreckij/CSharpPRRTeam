using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPConnector
{
    public class TCPServer
    {
        private const int connectionsLimit = 10;

        private const string defaultHost = "localhost";
        private readonly IPEndPoint localEndPoint;
        private readonly Socket listener;

        private Socket handler;


        public TCPServer()
        {
            // Get Host IP Address that is used to establish a connection  
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
            // If a host has multiple addresses, you will get a list of addresses  
            var host = Dns.GetHostEntry(defaultHost);
            var ipAddress = host.AddressList[0];
            localEndPoint = new IPEndPoint(ipAddress, 11000);
            listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
            {
                Blocking = false
            };
        }

        public void StartListening()
        {
            // A Socket must be associated with an endpoint using the Bind method  
            listener.Bind(localEndPoint);
            listener.Listen(connectionsLimit);
            Console.WriteLine("Waiting for a connection...");
        }

        public void AcceptConnection()
        {
            try
            {
                handler = listener.Accept();
                Console.WriteLine("Socket connected.");
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.WouldBlock)
                {
                    Console.WriteLine($"Waiting for clients to connect.");
                }
                else
                    Console.WriteLine(ex.Message);
            }
        }

        public void SendMessage(string message)
        {
            // Encode the data string into a byte array. 
            byte[] msg = Encoding.ASCII.GetBytes(message);
            var msgLenght = msg.Length;
            int bytesTransfered = 0;
            while (bytesTransfered < msgLenght)
            {
                try
                {
                    // Send the data through the socket.    
                    bytesTransfered = handler.Send(msg);
                    msg = msg.Skip(bytesTransfered).ToArray();
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.WouldBlock)
                        continue;
                    Console.WriteLine(ex.Message);
                    break;
                }
            }
        }

        public void ReceiveMessage()
        {
            byte[] serverReply = new byte[1024];
            int currentDataindex = 0;
            try
            {
                while (handler.Available > 0)
                {
                    // Receive the response from the remote device.    
                    int bytesRec = handler.Receive(serverReply, currentDataindex, handler.Available, SocketFlags.None);
                    currentDataindex += bytesRec;
                }
                var receivedMessage = Encoding.ASCII.GetString(serverReply, 0, currentDataindex);
                Console.WriteLine(receivedMessage);
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.WouldBlock)
                    Console.WriteLine("No message to read");
                else
                    Console.WriteLine(ex.Message);
            }
        }

        public void CloseConnection()
        {
            try
            {
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                Console.WriteLine($"Connection closed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}