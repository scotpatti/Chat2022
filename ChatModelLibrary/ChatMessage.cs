using System.Text.Json;

namespace ChatModelLibrary
{
    public class ChatMessage : IToJson
    {
        public string Message { get; set; }    
        public string User { get; set; }
        public ChatMessage(string user, string message)
        {
            User = user;
            Message = message;
        }

        public string ToJson()
        {
            var val = JsonSerializer.Serialize<ChatMessage>(this);
            return val + "\0";
        }

        public override string ToString()
        {
            return $"{User} says: {Message}";
        }
    }
}