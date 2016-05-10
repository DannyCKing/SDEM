
using System;
namespace SDEMViewModels.Global
{
    public class Constants
    {
        public const string MESSAGE_HEADER = "Message";

        public const string MESSAGE_TYPE_HEADER = "MessageType";

        public const string MESSAGE_DETAILS_HEADER = "MessageDetails";

        public const string ALIVE_MESSAGE_HEADER = "AliveMessage";

        public const string DIRECT_MESSAGE_HEADER = "DirectMessage";

        public const string READ_RECIEPT_MESSAGE_HEADER = "ReadMessage";

        public const string DEFAULT_MULTICAST_IP_ADDRESS = "232.95.38.32";

        public const int DEFAULT_MULTICAST_PORT = 42431;

        public const int DEFAULT_TCP_SERVER_PORT = 43431;

        public const string APP_NAME = "SDEM";

        public const string SETTINGS_FILE_NAME = "SDEM_Settings";

        public static string APP_DATA_LOCATION
        {
            get
            {
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                string fullPath = appDataPath + System.IO.Path.DirectorySeparatorChar + APP_NAME;

                if (Settings.IsTestAccount)
                    fullPath = fullPath + "Testing";

                bool exists = System.IO.Directory.Exists(fullPath);

                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(fullPath);
                }

                return fullPath;
            }
        }
    }
}
