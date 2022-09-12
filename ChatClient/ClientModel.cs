using ChatExtensions;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ChatClient
{
    public class ClientModel : INotifyPropertyChanged
    {
        private TcpClient _socket;
        private string _messageBoard;
        public string MessageBoard
        {
            get { return _messageBoard; }
            set { 
                _messageBoard = value;
                NotifyPropertyChanged();
            }
        }

        private string _currentMessage = "";
        public string CurrentMessage
        {
            get { return _currentMessage; }
            set { 
                _currentMessage = value;
                NotifyPropertyChanged();
            }
        }

        public bool Connected
        {
            get { return _socket != null && _socket.Connected; }
        }

        public void Connect()
        {
            if (string.IsNullOrEmpty(_currentMessage))
            {
                MessageBoard = "You must enter text identfying yourself " +
                    "in the Message before you click connect.";
                return;
            }
            _socket = new TcpClient("127.0.0.1", 8888);
            NotifyPropertyChanged("Connected");
            Send();
            _messageBoard = "Welcome: " + _currentMessage;
            var thread = new Thread(GetMessage);
            thread.Start();
        }

        public void GetMessage()
        {
            while (true)
            {
                string msg = _socket.ReadString();
                MessageBoard += "\r\n" + msg;
            }
        }

        public void Send()
        {
            _socket.WriteString(_currentMessage + "\0");
        }



        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string prop = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        #endregion
    }
}
