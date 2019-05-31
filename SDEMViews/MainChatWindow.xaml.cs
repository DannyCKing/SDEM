using System.Windows;
using MahApps.Metro.Controls;
using SDEMViewModels;
using SDEMViewModels.Global;

namespace SDEMViews
{
    /// <summary>
    /// Interaction logic for MainChatWindow.xaml
    /// </summary>
    public partial class MainChatWindow : MetroWindow
    {
        public MainChatWindow()
        {
            InitializeComponent();

            this.Loaded += MainChatWindow_Loaded;

            this.Closed += MainChatWindow_Closed;
        }

        private void MainChatWindow_Closed(object sender, System.EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        void MainChatWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MessageText.KeyUp += MessageText_KeyUp;
            //this
            //var x = this.MessageTe;
        }

        void MessageText_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ConversationViewModel currentConversation = this.DataContext as ConversationViewModel;
            if (currentConversation != null)
            {
                currentConversation.CurrentMessage = this.MessageText.Text;
                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    currentConversation.SendMessageCommand.Execute(null);
                }
            }
        }

        public MainChatWindow(MainChatViewModel viewModel)
            : this()
        {
            this.DataContext = viewModel;
        }

        private void ScrollViewer_ScrollChanged_1(object sender, System.Windows.Controls.ScrollChangedEventArgs e)
        {
            MessagesScrollViewer.ScrollToEnd();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // connect to server
            var viewModel = this.DataContext as MainChatViewModel;
            if (viewModel != null)
            {
                var ip = viewModel.ConnectionServer.IPAddress;
                var port = Settings.Instance.FinderTCPServerPort;
                var newConnectionDialog = new NewServerConnectionDialog(ip, port);
                newConnectionDialog.ShowDialog();

                if (newConnectionDialog.DialogResult.HasValue && newConnectionDialog.DialogResult.Value == true)
                {
                    if (viewModel != null)
                    {
                        viewModel.ConnectToServer(newConnectionDialog.IPAddress, newConnectionDialog.Port);
                    }
                }
            }
        }
    }
}
