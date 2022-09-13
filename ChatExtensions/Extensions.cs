using ChatContracts;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ChatExtensions
{
    public static class Extensions
    {
        public static T ReadMessage<T>(this TcpClient client) where T : class, IToJson
        {
            try
            {
                var stream = client.GetStream();
                var bytes = new byte[client.ReceiveBufferSize];
                stream.Read(bytes, 0, bytes.Length);
                var msg = Encoding.ASCII.GetString(bytes);
                var stmsg = JsonSerializer.Deserialize<T>(msg.Substring(0, msg.IndexOf('\0')));
                if (stmsg != null) return stmsg;
                throw new Exception($"Could not parse {msg}");
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static void WriteMessage<T>(this TcpClient client, T msg) where T : class, IToJson
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