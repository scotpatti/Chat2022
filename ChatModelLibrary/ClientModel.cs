using ChatExtensions;
using ChatModelLibrary;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace ChatClient
{
    public class ClientModel : INotifyPropertyChanged
    {
        #region Properties and Variables
        private TcpClient? _socket;

        private string? _messageBoard;
        public string? MessageBoard
        {
            get { return _messageBoard; }
            set { 
                _messageBoard = value;
                NotifyPropertyChanged();
            }
        }

        private string? _currentMessage = "";
        public string? CurrentMessage
        {
            get { return _currentMessage; }
            set { 
                _currentMessage = value;
                NotifyPropertyChanged();
            }
        }

        private string? _user = "";
        public string? User
        {
            get { return _user; }
            set { 
                _user = value; 
                NotifyPropertyChanged();
                NotifyPropertyChanged("UserSet");
            }
        }

        public bool Connected
        {
            get { return _socket != null && _socket.Connected; }
        }

        public bool UserSet
        {
            get => string.IsNullOrWhiteSpace(_user); 
        }
        #endregion

        public ClientModel()
        {
            User = null;
            _socket = null;
            _currentMessage = null;
            _messageBoard = null;
        }

        public void Connect()
        {
            if (string.IsNullOrEmpty(_user))
            {
                MessageBoard = "You must enter text identfying yourself " +
                    "in the user box before you click connect.";
                return;
            }
            _socket = new TcpClient("127.0.0.1", 8888);
            NotifyPropertyChanged("Connected");
            SendHello();
            _messageBoard = _currentMessage;
            var thread = new Thread(GetMessage);
            thread.Start();
        }

        public void GetMessage()
        {
            if (_socket == null) return;
            while (true)
            {
                ChatMessage msg = _socket.ReadMessage<ChatMessage>();
                MessageBoard += msg.ToString() + "\r\n";
            }
        }

        public void SendHello()
        {
            if (_socket == null || _user == null) return;
            _socket.WriteMessage(new ChatMessage(_user, $"{_user} has joined the chat."));
        }

        public void Send()
        {
            if (_socket == null || _user == null || _currentMessage == null) return;
            _socket.WriteMessage(new ChatMessage(_user,_currentMessage));
            CurrentMessage = "";
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string? prop = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        #endregion
    }
}
