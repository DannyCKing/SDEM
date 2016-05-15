using System.Net.Sockets;

namespace SDEMViewModels.TCPServer
{
    public class ClientInfo
    {
        public Socket ClientSocket { get; set; }

        public string ClientName { get; set; }
    }
}
