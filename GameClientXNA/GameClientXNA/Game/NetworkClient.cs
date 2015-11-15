using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


namespace GameClientXNA
{

    class NetworkClient
    {
        IPAddress ipAddress;
        int sendPort;
        int listenPort;

        //For sending data
        TcpClient tcpClient;


        //For listening 
        TcpListener tcpListener;
        Thread thread;
        bool recieving = false;

        //Read Message and check whether a new message
        private String recievedData;
        public bool isNewRecievedData;
        public String RecievedData{ get{ isNewRecievedData = false; return recievedData; } }
        public bool IsNewRecievedData{ get { return isNewRecievedData; } }

        //Instance
        private static NetworkClient instance;
        

        public static NetworkClient getInstance(string ipAddress, int sendPort, int listenPort)
        {
            if (instance == null)
            {
                instance = new NetworkClient(ipAddress, sendPort, listenPort);
                return instance;
            }
            instance.StopListening();
            instance.Set(ipAddress, sendPort, listenPort);
            return instance;
        }
        private NetworkClient(string ipAddress, int sendPort, int listenPort)
        {
            this.ipAddress = IPAddress.Parse(ipAddress);
            this.sendPort = sendPort;
            this.listenPort = listenPort;
        }

        public void Set(string ipAddress, int sendPort, int listenPort)
        {
            this.ipAddress = IPAddress.Parse(ipAddress);
            this.sendPort = sendPort;
            this.listenPort = listenPort;
        }


        // Wrap event invocations inside a protected virtual method


        public bool StartListening()
        {

            //start listening to server's broadcast port
            try
            {
                // Set the listener on the local IP address 
                // and specify the port.
                tcpListener = new TcpListener(ipAddress, listenPort);
                tcpListener.Start();
                thread = new Thread(new ThreadStart(Recieve));
                recieving = true;
                thread.Start();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to start listener {0}", e.ToString());
                return false;
            }
        }

        public void Recieve()
        {
            while (recieving)
            {
                try
                {
                    // Always use a Sleep call in a while(true) loop 
                    // to avoid locking up your CPU.
                    Thread.Sleep(10);
                    // Create a TCP socket. 
                    // If you ran this server on the desktop, you could use 
                    // Socket socket = tcpListener.AcceptSocket() 
                    // for greater flexibility.
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    // Read the data stream from the client. 
                    byte[] bytes = new byte[256];
                    NetworkStream stream = tcpClient.GetStream();
                    stream.Read(bytes, 0, bytes.Length);
                    string msg = Encoding.ASCII.GetString(bytes);

                    //Do what should be done (UPDATE recieved data)
                    recievedData = msg.Substring(0, msg.IndexOf('\0'));
                    isNewRecievedData = true;

                }
                catch (Exception e)
                {
                    Console.WriteLine("Recive Error: {0}", e.ToString());
                }
            }
        }

        public bool Send(string data)
        {
            try
            {

                tcpClient = new TcpClient();
                tcpClient.Connect(ipAddress, sendPort);

                if (tcpClient.Connected)
                {
                    NetworkStream stream = tcpClient.GetStream();
                    BinaryWriter writer = new BinaryWriter(stream);
                    Byte[] bytes = Encoding.ASCII.GetBytes(data);
                    writer.Write(bytes);
                    writer.Close();
                    stream.Close();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Sending failed : {0}", e.ToString());
                return false;
            }
        }

        public bool StopListening()
        {
            try
            {
                recieving = false;
                tcpListener.Stop();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
                return false;
            }
        }
    }
}
