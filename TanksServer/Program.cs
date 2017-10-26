using System.Net;

namespace TanksServer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Server server = new Server();
            server.Start(IPAddress.Parse("192.168.100.105"), 6000);
        }
    }
}