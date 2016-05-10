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
            }
        }

        private void Button_PreviewMouseDown_1(object sender, MouseButtonEventArgs e)
        {
            var startup = this.DataContext as StartUpViewModel;

            string username = this.UsernameTextBox.Text;
            startup.Username = username;
            Settings.Instance.Username = startup.Username;


            string ipAddress = "";
            int multicastPort = 0;
            if (startup.UseDefaults)
            {
                ipAddress = Constants.DEFAULT_MULTICAST_IP_ADDRESS;
                multicastPort = Constants.DEFAULT_MULTICAST_PORT;
            }
            else
            {
                ipAddress = startup.MulticastIPAddress;
                multicastPort = startup.MulticastPort;
            }

            var chatVM = new MainChatViewModel(ipAddress, multicastPort, startup.TCPServerPort);

            MainChatWindow newWindow = new MainChatWindow(chatVM);
            newWindow.Show();
            this.Close();
        }
    }
}
