using System;
using SDEMViewModels.TCPClient;

namespace SDEMViewModels.Models
{
    public class ChatUser
    {
        public Status UserStatus { get; set; }

        public Guid UserId { get; private set; }

        public string Username { get; private set; }

        #region TCPClient

        public TCPClientListener TCPClient { get; private set; }

        #endregion

        public ChatUser(Guid userId, string username, TCPClientListener clientListener)
        {
            UserId = userId;
            Username = username;
            TCPClient = clientListener;
        }
    }
}
