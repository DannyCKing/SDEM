﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Crypt;

namespace SDEMViewModels.TCPClient
{
    public class TCPClientListener
    {
        private byte[] byteData = new byte[2048];

        private Socket _ClientSocket { get; set; }

        // host for the remote device
        private readonly string _ServerAddress;

        // The port number for the remote device.
        private readonly int _ServerPort;

        // ManualResetEvent instances signal completion.
        private ManualResetEvent connectDone =
            new ManualResetEvent(false);

        // The response from the remote device.
        private String response = String.Empty;

        public TCPClientListener(string address, int port)
        {
            _ServerAddress = address;
            _ServerPort = port;
        }

        public void StartClient()
        {
            // Connect to a remote device.
            try
            {
                // Establish the remote endpoint for the socket.
                IPHostEntry ipHostInfo = Dns.Resolve(this._ServerAddress);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, _ServerPort);

                // Create a TCP/IP socket.
                _ClientSocket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.
                _ClientSocket.BeginConnect(remoteEP,
                    new AsyncCallback(OnConnect), _ClientSocket);
                connectDone.WaitOne();

                //_ClientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);

                byteData = new byte[2048];
                //Start listening to the data asynchronously
                _ClientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void OnConnect(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        //private void Receive(Socket client)
        //{
        //    try
        //    {
        //        // Create the state object.
        //        ClientStateObject state = new ClientStateObject();
        //        state.workSocket = client;

        //        // Begin receiving the data from the remote device.
        //        client.BeginReceive(state.buffer, 0, ClientStateObject.BufferSize, 0,
        //            new AsyncCallback(ReceiveCallback), state);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //}

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket 
                // from the asynchronous state object.
                ClientStateObject state = (ClientStateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    // Get the rest of the data.
                    client.BeginReceive(state.buffer, 0, ClientStateObject.BufferSize, 0,
                        new AsyncCallback(OnReceive), state);
                }
                else
                {
                    // All the data has arrived; put it in response.
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Send(String data)
        {
            string encryptedText = new PasswordConverter().Encrypt(data);

            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(encryptedText);

            // Begin sending the data to the remote device.
            _ClientSocket.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), _ClientSocket);
        }


        public void Send(string[] data)
        {
            int count = 0;
            foreach (var message in data)
            {
                string encryptedText = new PasswordConverter().Encrypt(message);

                Console.WriteLine(string.Format("Sending Message {0} of {1}", count, data.Length));

                Console.WriteLine("Message length: " + encryptedText.Length);

                Console.WriteLine("Message: ");
                Console.WriteLine(encryptedText);
                Console.WriteLine();

                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes(encryptedText);

                // Begin sending the data to the remote device.
                _ClientSocket.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), _ClientSocket);
                count++;
            }
        }


        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}
