using System;
using System.Collections.Generic;
using SDEMViewModels.Models;

namespace SDEMViewModels.TCPClient
{
    public class TCPClientsManager
    {
        private static volatile TCPClientsManager instance;
        private static object syncRoot = new object();
        private static bool IsInitialized = false;

        private static Guid TEST_GUID = Guid.Parse("7306acb4-1fe9-44ef-aac1-523549484546");

        private static string TEST_USERNAME = "Second Chat User";

        private TCPClientsManager()
        {
            TCPClients = new Dictionary<ChatUser, TCPClientListener>();
        }

        public static TCPClientsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new TCPClientsManager();
                    }
                }

                return instance;
            }
        }

        private Dictionary<ChatUser, TCPClientListener> TCPClients { get; set; }

        public TCPClientListener AddTCPClient(ChatUser user, TCPClient tcpCLient)
        {
            var listener = new TCPClientListener(tcpCLient.RemoteIPAddress, tcpCLient.RemoteIPPort);
            TCPClients.Add(user, listener);
            return listener;
        }

        public TCPClientListener GetListener(ChatUser user, TCPClient client)
        {
            if (!TCPClients.ContainsKey(user))
            {
                AddTCPClient(user, client);
            }
            return TCPClients[user];
        }

        public void RemoteTCPClient(ChatUser user)
        {
            if (TCPClients.ContainsKey(user))
            {
                TCPClients.Remove(user);
            }
        }
    }
}