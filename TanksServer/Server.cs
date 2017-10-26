using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using TanksServer.API;

namespace TanksServer
{
    public class Server
    {
        private readonly Socket thisSocket;
        
        public bool Stop { get; set; }
        
        public Server()
        {
            thisSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start(IPAddress address, int port)
        {
            try
            {
                thisSocket.Bind(new IPEndPoint(address, port));
                thisSocket.Listen(10);

                while (!Stop)
                {
                    Console.WriteLine("Waiting for connection...");
                    Socket client = thisSocket.Accept();
                    
                    Thread clientThread = new Thread(() =>
                    {
                        while (true)
                        {
                            byte[] bytes = new byte[1024];

                            try
                            {
                                int bytesRec = client.Receive(bytes);
                                string data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                                if (data == "")
                                    throw new NullReferenceException();
                                Console.WriteLine("Client " + (client.RemoteEndPoint as IPEndPoint)?.Address +
                                                  " send " +
                                                  data);
                                Message message = JsonConvert.DeserializeObject<Message>(data);
                                //Console.WriteLine("Type: " + message.Type);
                            }
                            catch (NullReferenceException)
                            {
                                Console.WriteLine("Client disonnected.");
                                break;
                            }
                        }
                    });
                    clientThread.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}