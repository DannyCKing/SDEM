
using SDEMViewModels.Global;
namespace SDEMViewModels
{
    public class StartUpViewModel : NotifyPropertyChanged
    {
        private bool _UseDefaults = true;
        private string _MulticastIpAddress;
        private int _MulticastPort;
        private bool _ShowDeveloperOptions;
        private bool _TestAccount;
        private int _TCPServerPort;
        private string _Username;

        public string MulticastIPAddress
        {
            get
            {
                return _MulticastIpAddress;
            }
            set
            {
                if (_MulticastIpAddress == value)
                    return;

                _MulticastIpAddress = value;
                RaisePropertyChanged("MulticastIPAddress");
            }
        }

        public int MulticastPort
        {
            get
            {
                return _MulticastPort;
            }
            set
            {
                if (_MulticastPort == value)
                    return;

                _MulticastPort = value;
                RaisePropertyChanged("MulticastPort");
            }
        }

        public bool ShowDeveloperOptions
        {
            get
            {
                return _ShowDeveloperOptions;
            }
            set
            {
                if (_ShowDeveloperOptions == value)
                    return;

                _ShowDeveloperOptions = value;
                RaisePropertyChanged("ShowDeveloperOptions");
            }
        }

        public int TCPServerPort
        {
            get
            {
                return _TCPServerPort;
            }
            set
            {
                if (_TCPServerPort == value)
                    return;

                _TCPServerPort = value;
                RaisePropertyChanged("TCPServerPort");
            }
        }

        public bool TestAccount
        {
            get
            {
                return _TestAccount;
            }
            set
            {
                if (value == _TestAccount)
                    return;

                _TestAccount = value;
                RaisePropertyChanged("TestAccount");
            }
        }

        public bool UseDefaults
        {
            get
            {
                return _UseDefaults;
            }
            set
            {
                if (value == _UseDefaults)
                    return;

                _UseDefaults = value;
                RaisePropertyChanged("UseDefaults");
            }
        }

        public string Username
        {
            get
            {
                return _Username;
            }
            set
            {
                if (value == _Username)
                    return;

                _Username = value;
                RaisePropertyChanged("Username");
            }
        }


        public StartUpViewModel()
        {
            Username = Settings.Instance.Username;

            MulticastIPAddress = Constants.DEFAULT_MULTICAST_IP_ADDRESS;
            MulticastPort = Constants.DEFAULT_MULTICAST_PORT;
            TCPServerPort = Constants.DEFAULT_TCP_SERVER_PORT;
        }
    }
}
