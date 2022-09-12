using ChatModelLibrary;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ChatExtensions
{
    public static class Extensions
    {
        public static ChatMessage ReadMessage(this TcpClient client)
        {
            try
            {
                var stream = client.GetStream();
                var bytes = new byte[client.ReceiveBufferSize];
                stream.Read(bytes, 0, bytes.Length);
                var msg = Encoding.ASCII.GetString(bytes);
                var stmsg = JsonSerializer.Deserialize<ChatMessage>(msg.Substring(0, msg.IndexOf('\0')));
                if (stmsg != null) return stmsg;
                throw new Exception($"Could not parse {msg}");
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ChatMessage("", "");
            }
        }

        public static void WriteMessage(this TcpClient client, ChatMessage msg)
        {
            try
            {
                var bytes = Encoding.ASCII.GetBytes(msg.ToJson());
                var stream = client.GetStream();
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}