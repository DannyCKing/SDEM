using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SDEMViewModels.Global;
using SDEMViewModels.MessageHandlers;

namespace SDEMViewModels.TCPServer
{
    public class TCPServerListener
    {
        private string output;

        private readonly int _PortNumber;

        private TcpListener _Server = null;

        private Socket _ServerSocket;

        byte[] byteData = new byte[1024];

        List<ClientInfo> Clients = new List<ClientInfo>();

        MessageHandlerFactory MessageHandlerFactory = new MessageHandlerFactory();

        private MainChatViewModel MainChatVM;

        public TCPServerListener(MainChatViewModel chatViewModel, int portNumber)
        {
            MainChatVM = chatViewModel;
            _PortNumber = portNumber;
        }

        public void StartListener()
        {
            // Data buffer for incoming data.
            byteData = new Byte[1024];

            // Establish the local endpoint for the socket.
            // The DNS name of the computer
            // running the listener is "host.contoso.com".
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            //IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, _PortNumber);

            // Create a TCP/IP socket.
            _ServerSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                _ServerSocket.Bind(localEndPoint);
                _ServerSocket.Listen(100);

                while (true)
                {

                    // Start an asynchronous socket to listen for connections.
                    //Console.WriteLine("Waiting for a connection...");
                    _ServerSocket.BeginAccept(
                        new AsyncCallback(OnAccept),
                        _ServerSocket);

                    System.Threading.Thread.Sleep(100); // 5,000 ms
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        public void OnAccept(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = _ServerSocket.EndAccept(ar);

                //Start listening for more clients
                _ServerSocket.BeginAccept(new AsyncCallback(OnAccept), null);

                //Once the client connects then start receiving the commands from her
                clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None,
                    new AsyncCallback(OnReceive), clientSocket);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in On Accept" + ex.Message);
            }
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket. 
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.
                state.sb.Append(Encoding.UTF8.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read 
                // more data.
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the 
                    // client. Display it on the console.
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content);
                    // Echo the data back to the client.
                    Send(handler, content);
                }
                else
                {
                    // Not all data received. Get more.
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = (Socket)ar.AsyncState;
                clientSocket.EndReceive(ar);

                var message = XMLUtils.FormatXML(byteData);
                var handler = MessageHandlerFactory.GetMessageHandler(message);

                handler.HandleMessage(MainChatVM, message);

                Console.WriteLine(message);

                clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), clientSocket);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in OnRead in TCPServer");
                Console.WriteLine(ex.Message);
                //MessageBox.Show(ex.Message, "GSserverTCP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.UTF8.GetBytes(data);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}
