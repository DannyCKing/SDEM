
using System;
using SDEMViewModels.Global;
using SDEMViewModels.Models;
namespace SDEMViewModels.Messages
{
    public class AliveMessageContent : IMessageContent
    {
        public Status CurrentStatus { get; set; }

        public const string MessageType = Constants.ALIVE_MESSAGE_HEADER;

        public string TCPServerAddress { get; set; }

        public int TCPPort { get; set; }

        public Guid SenderId { get; set; }

        public string Username { get; set; }

        public AliveMessageContent(string tcpAddres, int tcpPort, Guid senderId, string username)
        {
            TCPServerAddress = tcpAddres;
            TCPPort = tcpPort;
            SenderId = senderId;
            Username = username;
        }
    }
}
