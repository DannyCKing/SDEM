using System.Windows;
using MahApps.Metro.Controls;
using SDEMViewModels;

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
    }
}
