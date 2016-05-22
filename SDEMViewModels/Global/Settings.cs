using System;

namespace SDEMViewModels.Global
{
    public class Settings
    {
        private static volatile Settings instance;
        private static object syncRoot = new object();
        private static bool IsInitialized = false;

        public static bool IsTestAccount = false;

        private static Guid TEST_GUID = Guid.Parse("7306acb4-1fe9-44ef-aac1-523549484546");

        private static string TEST_USERNAME = "Second Chat User";

        private const int DEFAULT_TCP_SERVER_PORT = 43431;

        private Settings()
        {
            if (!IsInitialized)
            {
                ParseSavedSettings();
                IsInitialized = true;
            }
        }

        public static Settings Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Settings();
                    }
                }

                return instance;
            }
        }

        public bool DesktopNotifications { get; set; }

        public int LengthOfHistory { get; set; }

        public bool RequireHover { get; set; }

        public bool Sounds { get; set; }

        public int TCPServerPort
        {
            get
            {
                if (IsTestAccount)
                    return DEFAULT_TCP_SERVER_PORT + 100;
                else
                    return DEFAULT_TCP_SERVER_PORT;
            }
        }

        private string _Username;

        public string Username
        {
            get
            {
                if (IsTestAccount)
                    return TEST_USERNAME;
                else
                    return _Username;
            }
            set
            {
                if (_Username == value)
                    return;

                _Username = value;
                SaveSettings();
            }
        }

        private Guid _UserId;

        public Guid UserId
        {
            get
            {
                if (IsTestAccount)
                    return TEST_GUID;
                else
                    return _UserId;
            }
            set
            {
                if (_UserId == value)
                    return;

                _UserId = value;
                SaveSettings();
            }
        }

        private void InitializeSettings()
        {
            Username = "No name";
            UserId = Guid.NewGuid();
            this.DesktopNotifications = true;
            this.Sounds = true;
            this.RequireHover = false;
            this.LengthOfHistory = 100;
        }

        public void SaveSettings()
        {
            SettingSaver.SaveSettingsToFile(this);
        }


        public void ParseSavedSettings()
        {
            if (!SettingSaver.DoesSettingsFileExist())
            {
                InitializeSettings();
                SaveSettings();
            }

            if (!SettingSaver.GetSettingsFromFile(this))
            {
                SaveSettings();
            }
        }



    }

}
