using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SDEMViews
{
    /// <summary>
    /// Interaction logic for NewServerConnectionDialog.xaml
    /// </summary>
    public partial class NewServerConnectionDialog : Window
    {
        public string IPAddress { get; private set; }
        public int Port { get; private set; }

        private string MyIP;
        private int MyPort;

        public NewServerConnectionDialog(string myIPAddress, int myPort)
        {
            InitializeComponent();

            this.Loaded += NewServerConnectionDialog_Loaded;

            MyIP = myIPAddress;
            MyPort = myPort;
        }

        private void NewServerConnectionDialog_Loaded(object sender, RoutedEventArgs e)
        {
            this.MyIPLabel.Content = MyIP;
            this.MyPortLabel.Content = MyPort;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void ConnectClick(object sender, RoutedEventArgs e)
        {
            this.IPAddress = this.IPAddressTextBox.Text;
            this.Port = int.Parse(this.PortTextBox.Text);
            this.DialogResult = true;
            this.Close();
        }
    }
}
