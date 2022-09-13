using ChatExtensions;
using ChatModelLibrary;
using System.Net;
using System.Net.Sockets;

namespace Chat2022;

public static class Program 
{ 
    public static Dictionary<string, TcpClient> ClientList =
        new Dictionary<string, TcpClient>();
    static void Main()
    {
        var serverSocket = new TcpListener(IPAddress.Any, 8888);
        serverSocket.Start();
        Console.WriteLine("Chat server has started...");
        while (true)
        {
            try
            {
                var clientSocket = serverSocket.AcceptTcpClient();
                ChatMessage data = clientSocket.ReadMessage<ChatMessage>();
                ClientList.Add(data.User, clientSocket);
                Broadcast(data);
                Console.WriteLine(data.User + " joined the chatroom.");
                var client = new HandleClient(clientSocket, data.User);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client aborted Connection: {ex.Message}");
            }
        }
    }

    public static void Broadcast(ChatMessage? msg)
    {
        if (msg == null) return;
        foreach (var item in ClientList)
        {
            item.Value.WriteMessage(msg);
        }
    }
}