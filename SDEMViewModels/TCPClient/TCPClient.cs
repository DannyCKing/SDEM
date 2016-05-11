
namespace SDEMViewModels.TCPClient
{
    public class TCPClient
    {
        public string RemoteIPAddress { get; private set; }

        public int RemoteIPPort { get; private set; }

        public TCPClient(string remoteAddress, int remotePort)
        {
            RemoteIPAddress = remoteAddress;
            RemoteIPPort = remotePort;
        }
    }
}
