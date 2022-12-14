using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChatClient
{
    public class ClientViewModel : INotifyPropertyChanged
    {
        private readonly ClientModel _clientModel;
        public string Message
        {
            get { return _clientModel.CurrentMessage; }
            set { _clientModel.CurrentMessage = value; NotifyPropertyChanged(); }
        }
        public string MessageBoard
        {
            get { return _clientModel.MessageBoard; }
            set { _clientModel.CurrentMessage = value; NotifyPropertyChanged(); }
        }
        public DelegateCommand<object> ConnectCommand { get; set; }
        public DelegateCommand<object> SendCommand { get; set; }

        public ClientViewModel()
        {
            _clientModel = new ClientModel();
            _clientModel.PropertyChanged += ClientModelChanged;
            ConnectCommand = new DelegateCommand<object>(
                a => _clientModel.Connect(),
                b => !_clientModel.Connected);
            SendCommand = new DelegateCommand<object>(
                a => _clientModel.Send(),
                b => _clientModel.Connected);
        }

        private void ClientModelChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Connected"))
            {
                NotifyPropertyChanged("Connected");
                ConnectCommand.RaiseCanExecuteChanged();
                SendCommand.RaiseCanExecuteChanged();
            }
            if (e.PropertyName.Equals("MessageBoard"))
            {
                NotifyPropertyChanged("MessageBoard");
            }
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
