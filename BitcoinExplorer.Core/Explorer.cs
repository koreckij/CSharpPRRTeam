using BitcoinExplorer.Core.Logger;
using BitcoinExplorer.Core.Structs.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Version = BitcoinExplorer.Core.Structs.Net.Version;

namespace BitcoinExplorer.Core
{
    public class Explorer
    {
        //only for test
        private const string LocalTestnetPeer = "localhost";
        private const int LocalTestnetPort = 18444;

        private readonly TcpClient client;
        private bool useTestnet;
        private static readonly ILogger logger = new SimpleConsoleLogger();

        public Explorer(string ipv4 = LocalTestnetPeer, int port = LocalTestnetPort)
        {
            client = new TcpClient(ipv4, port);
            if (port != 8333)
                useTestnet = true;
        }

        /// <summary>
        /// Perform DNS lookup for given dns name or ip address
        /// </summary>
        /// <param name="dns">Ipv4 address collection</param>
        /// <returns></returns>
        public static IEnumerable<string> Explore(string dns = "seed.bitcoinstats.com")
        {
            logger.LogHeader($"Dns lookup for {dns}");
            var addrs = Dns.GetHostEntry(dns).AddressList.Where(adr => adr.AddressFamily == AddressFamily.InterNetwork);
            var strAddrs = addrs.Select(ip => ip.ToString().Split(':').Last());
            logger.Log(strAddrs);
            return strAddrs;
        }

        /// <summary>
        /// Perform peers handshake
        /// </summary>
        public void Handshake()
        {
            try
            {
                NetworkStream ns = client.GetStream();
                var localAddr = new NetAddr(Services.NODE_NETWORK, ((IPEndPoint)client.Client.LocalEndPoint).Address, (UInt16)((IPEndPoint)client.Client.LocalEndPoint).Port);
                var remoteAddr = new NetAddr(Services.NODE_NETWORK, ((IPEndPoint)client.Client.RemoteEndPoint).Address, (UInt16)((IPEndPoint)client.Client.RemoteEndPoint).Port);
                new Message("version", Version.Default(remoteAddr, localAddr, 0), useTestnet).Write(ns);
                Console.WriteLine($"\nSuccessfully connected to node {remoteAddr.address.ToString().Split(':').Last()}:{remoteAddr.port}");            
                logger.LogHeader("Version message send");
                while (true)
                {
                    Message msg = Message.FromStream(ns);
                    switch (msg.strcmd)
                    {
                        case "version":
                            {
                                logger.LogHeader("Version message received");
                                // TODO: Decode and print version 
                                new Message("verack", new VerAck(), useTestnet);
                                logger.LogHeader("Verack message send");
                            }
                            break;
                        case "verack":
                            {
                                logger.LogHeader("Verack message received");
                                // TODO: Decode and print verack 
                                logger.LogHeader("Handshake ended successfully");
                            }
                            return;
                        case "alert":
                            {
                                logger.LogHeader("Alert message received");
                                // TODO: Decode and print alert 
                            }
                            break;
                        default:
                            throw new Exception("Received not excepted message during handshake");
                    }
                }
            }
            catch (EndOfStreamException ex)
            {
                logger.LogHeader("End message received");
                return;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error during handshake: {ex.Message}");
            }
        }
    }
}
