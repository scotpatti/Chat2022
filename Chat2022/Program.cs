using ChatExtensions;
using System.Net;
using System.Net.Sockets;

namespace Chat2022;

public static class Program 
{ 
    //List of Chat Clients <name, socket>
    public static Dictionary<string, TcpClient> ClientList =
        new Dictionary<string, TcpClient>();
    static void Main()
    {
        //Creating a Server Socket or a Listening Socket
        var serverSocket = new TcpListener(IPAddress.Any, 8888);
        serverSocket.Start();
        Console.WriteLine("Chat server has started...");
        while (true)
        {
            try
            {
                //This next line of code _BLOCKS_ or stops and waits
                var clientSocket = serverSocket.AcceptTcpClient();
                //Someone has connected to our socket, and we'll need to 
                //deal with that new connection
                string data = clientSocket.ReadString();
                //Add it to my list of chat clients
                ClientList.Add(data, clientSocket);
                //Tell all the chat clients that someone has joined
                Broadcast(data + " joined.", data, false);
                Console.WriteLine(data + " joined the chatroom.");
                //Setup a new thread to handle future communication from 
                //this chat client.
                var client = new HandleClient();
                client.StartClient(clientSocket, data);
            }
            catch (Exception)
            {
                Console.WriteLine("Client aborted Connection.");
            }
        }
    }

    public static void Broadcast(string msg, string uname, bool flag)
    {
        foreach (var item in ClientList)
        {
            var m = flag ? uname + " says: " + msg : msg;
            item.Value.WriteString(m);
        }
    }
}