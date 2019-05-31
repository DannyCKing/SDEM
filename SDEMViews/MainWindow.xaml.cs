using System.Windows;
using System.Windows.Input;
using SDEMViewModels;
using SDEMViewModels.Global;

namespace SDEMViews
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                Settings.IsTestAccount = true;
            }

            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                var startup = this.DataContext as StartUpViewModel;
                startup.ShowDeveloperOptions = true;
                startup.UseDefaults = false;
            }

            this.UsernameTextBox.Focus();
            this.UsernameTextBox.SelectionStart = this.UsernameTextBox.Text.Length; // add some logic if length is 0
            this.UsernameTextBox.SelectionLength = 0;
        }

        private void Button_PreviewMouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Login();

        }

        private void Login()
        {
            var startup = this.DataContext as StartUpViewModel;

            string username = this.UsernameTextBox.Text;
            startup.Username = username;
            Settings.Instance.Username = startup.Username;

            var tcpPort = 0;
            string ipAddress = "";
            int multicastPort = 0;
            if (startup.UseDefaults)
            {
                ipAddress = Constants.DEFAULT_MULTICAST_IP_ADDRESS;
                multicastPort = Constants.DEFAULT_MULTICAST_PORT;
                tcpPort = Settings.Instance.TCPServerPort;
            }
            else
            {
                ipAddress = startup.MulticastIPAddress;
                multicastPort = startup.MulticastPort;
                tcpPort = startup.TCPServerPort;
                //Settings.IsTestAccount = true;
            }

            var chatVM = new MainChatViewModel(ipAddress, multicastPort, tcpPort);

            MainChatWindow newWindow = new MainChatWindow(chatVM);
            newWindow.Show();
            this.Close();
        }

        private void Window_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }

    }
}
