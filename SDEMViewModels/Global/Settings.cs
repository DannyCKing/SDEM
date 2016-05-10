using System;
using System.Xml.Linq;

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
                SaveSettings(this);
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
                SaveSettings(this);
            }
        }

        public void SaveSettings(Settings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Username))
                settings.Username = "No name";

            if (settings.UserId == Guid.Empty)
                settings.UserId = Guid.NewGuid();

            XDocument document = new XDocument(
                                    new XElement("Settings",
                                        new XElement("Username", settings.Username.ToString()),
                                        new XElement("UserId", settings.UserId.ToString())));

            document.Save(FilePathAndFileName);
        }


        public void ParseSavedSettings()
        {
            if (!System.IO.File.Exists(FilePathAndFileName))
            {
                SaveSettings(this);
            }
            var document = XDocument.Load(FilePathAndFileName);
            var settingsElement = document.Element("Settings");
            Username = settingsElement.Element("Username").Value;
            UserId = Guid.Parse(settingsElement.Element("UserId").Value);
        }

        private string FilePathAndFileName
        {
            get
            {
                string fileNameAndExtension = Constants.SETTINGS_FILE_NAME + ".set";

                var pathAndFile = Constants.APP_DATA_LOCATION + System.IO.Path.DirectorySeparatorChar + fileNameAndExtension;
                return pathAndFile;
            }
        }

    }

}
