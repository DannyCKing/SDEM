using System.Windows;
using SDEMViewModels;

namespace SDEMViews
{
    /// <summary>
    /// Interaction logic for MainChatWindow.xaml
    /// </summary>
    public partial class MainChatWindow : Window
    {
        public MainChatWindow()
        {
            InitializeComponent();

            this.Loaded += MainChatWindow_Loaded;
        }

        void MainChatWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public MainChatWindow(MainChatViewModel viewModel)
            : this()
        {
            this.DataContext = viewModel;
        }
    }
}
