using ChatExtensions;
using ChatModelLibrary;
using System.Net.Sockets;


namespace Chat2022
{
    internal class HandleClient
    {
        private TcpClient clientSocket;
        private string clientName;

        public HandleClient(TcpClient client, string name)
        {
            clientSocket = client;
            clientName = name;
            var thread = new Thread(DoChat);
            thread.Start();
        }

        private void DoChat()
        {
            while (true)
            {
                try
                {
                    ChatMessage? dataFromClient = clientSocket.ReadMessage();
                    Program.Broadcast(dataFromClient);
                    Console.WriteLine(dataFromClient);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
            }
        }
    }
}
