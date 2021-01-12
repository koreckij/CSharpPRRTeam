using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPConnector
{
    public class TCPClient
    {
        private const string defaultHost = "localhost";
        private readonly IPEndPoint remoteEP;
        private Socket sender;

        public TCPClient()
        {
            // Connect to a Remote server  
            // Get Host IP Address that is used to establish a connection  
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
            // If a host has multiple addresses, you will get a list of addresses  
            var host = Dns.GetHostEntry(defaultHost);
            var ipAddress = host.AddressList[0];
            remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 18444);
        }

        /// <summary>
        /// Perform DNS lookup for given dns name or ip address
        /// </summary>
        /// <param name="dns">DNS name or ip address</param>
        /// <returns></returns>
        public IEnumerable<IPAddress> DnsLookup(string dns= "seed.bitcoinstats.com")
        {
            return Dns.GetHostEntry(dns).AddressList.Where(adr => adr.AddressFamily == AddressFamily.InterNetwork);
        }

        private void CreateSocket()
        {
            sender = new Socket(remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
            {
                Blocking = false
            };
        }

        public void Connect()
        {
            try
            {
                // Connect to Remote EndPoint
                if (sender == null) CreateSocket();
                sender.Connect(remoteEP);
                Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
            }
            catch (SocketException ex)
            {
                switch (ex.SocketErrorCode)
                {
                    case SocketError.WouldBlock:
                        Console.WriteLine("Connecting to server...");
                        return;
                    case SocketError.InProgress:
                        Console.WriteLine("Server do not response immediately, trying to connect...");
                        return;
                    case SocketError.AlreadyInProgress:
                        Console.WriteLine("Still trying to connect...");
                        return;
                    case SocketError.IsConnected:
                        Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
                        return;
                    default:
                        Console.WriteLine(ex.Message);
                        return;
                }
            }
            catch (ObjectDisposedException ex)
            {
                CreateSocket();
                Connect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        public void SendMessage(string message)
        {
            try
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
                        bytesTransfered = sender.Send(msg);
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
            catch (ObjectDisposedException ex)
            {
                CreateSocket();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ReceiveMessage()
        {
            try
            {
                byte[] serverReply = new byte[1024];
                int currentDataindex = 0;
                while (sender.Available > 0)
                {
                    // Receive the response from the remote device.    
                    int bytesRec = sender.Receive(serverReply, currentDataindex, sender.Available, SocketFlags.None);
                    currentDataindex += bytesRec;
                }
                var receivedMessage = Encoding.ASCII.GetString(serverReply, 0, currentDataindex);
                Console.WriteLine(receivedMessage);
            }
            catch (ObjectDisposedException ex)
            {
                CreateSocket();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void CloseConnection()
        {
            try
            {
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
                Console.WriteLine($"Connection to {remoteEP} closed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}