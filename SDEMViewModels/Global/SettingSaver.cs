using System;
using System.IO;
using System.Xml.Linq;
using Crypt;

namespace SDEMViewModels.Global
{
    public static class SettingSaver
    {
        //            new XElement("Username", settings.Username.ToString()),
        //new XElement("UserId", settings.UserId.ToString()),
        //new XElement("LengthOfHistory", settings.LengthOfHistory.ToString()),
        //new XElement("DesktopNotifications", settings.DesktopNotifications.ToString()),
        //new XElement("Sounds", settings.Sounds.ToString()),
        //new XElement("RquireHover", settings.RequireHover.ToString())));
        private const string USERNAME_TAG = "Username";

        private const string USER_ID_TAG = "UserId";

        private const string LENGTH_OF_HISTORY_TAG = "LengthOfHistory";

        private const string DESKTOP_NOTIFICATIONS_TAG = "DesktopNotifications";

        private const string SOUNDS_TAG = "Sounds";

        private const string REQUIRE_HOVER_RAG = "RequireHover";

        private static PasswordConverter Encryption = new PasswordConverter();

        private static string FilePathAndFileName
        {
            get
            {
                string fileNameAndExtension = Constants.SETTINGS_FILE_NAME + ".set";

                var pathAndFile = Constants.APP_DATA_LOCATION + System.IO.Path.DirectorySeparatorChar + fileNameAndExtension;
                return pathAndFile;
            }
        }

        public static bool DoesSettingsFileExist()
        {
            return System.IO.File.Exists(FilePathAndFileName);
        }

        public static void SaveSettingsToFile(Settings settings)
        {
            // Try 5 times to save file
            bool success = false;

            int count = 0;
            while (count < 5 && !success)
            {
                success = SaveSettingsWork(settings);
            }
        }

        private static bool SaveSettingsWork(Settings settings)
        {
            try
            {
                XDocument document = new XDocument(
                        new XElement("Settings",
                            new XElement(USERNAME_TAG, settings.Username.ToString()),
                            new XElement(USER_ID_TAG, settings.UserId.ToString()),
                            new XElement(LENGTH_OF_HISTORY_TAG, settings.LengthOfHistory.ToString()),
                            new XElement(DESKTOP_NOTIFICATIONS_TAG, settings.DesktopNotifications.ToString()),
                            new XElement(SOUNDS_TAG, settings.Sounds.ToString()),
                            new XElement(REQUIRE_HOVER_RAG, settings.RequireHover.ToString())));

                var xmlString = XMLUtils.XmlToString(document);

                var encryptedString = Encryption.Encrypt(xmlString);

                System.IO.File.WriteAllLines(FilePathAndFileName, new string[] { encryptedString });

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static void GetSettingsFromFile(Settings settings)
        {
            // Try 5 times to save file
            bool success = false;

            int count = 0;
            while (count < 5 && !success)
            {
                success = GetSettingsFromFileDoWork(settings);
            }
        }

        private static bool GetSettingsFromFileDoWork(Settings settings)
        {

            //            new XElement("Username", settings.Username.ToString()),
            //new XElement("UserId", settings.UserId.ToString()),
            //new XElement("LengthOfHistory", settings.LengthOfHistory.ToString()),
            //new XElement("DesktopNotifications", settings.DesktopNotifications.ToString()),
            //new XElement("Sounds", settings.Sounds.ToString()),
            //new XElement("RquireHover", settings.RequireHover.ToString())));
            string username = null;
            Guid userId = Guid.Empty;
            int? lengthOfHistory = null;
            bool? desktopNotifications = null;
            bool? sounds = null;
            bool? requireHover = null;
            try
            {
                // TO DO: catch exceptions for individual settings

                var lines = File.ReadAllLines(FilePathAndFileName);
                var encryptedString = lines[0];
                var xml = Encryption.Decrypt(encryptedString);
                var document = XDocument.Parse(xml);
                var settingsElement = document.Element("Settings");
                username = settingsElement.Element(USERNAME_TAG).Value;
                userId = Guid.Parse(settingsElement.Element(USER_ID_TAG).Value);
                lengthOfHistory = int.Parse(settingsElement.Element(LENGTH_OF_HISTORY_TAG).Value);
                desktopNotifications = bool.Parse(settingsElement.Element(DESKTOP_NOTIFICATIONS_TAG).Value);
                requireHover = bool.Parse(settingsElement.Element(REQUIRE_HOVER_RAG).Value);
                sounds = bool.Parse(settingsElement.Element(SOUNDS_TAG).Value);

                settings.Username = username;
                settings.UserId = userId;

                if (lengthOfHistory.HasValue)
                    settings.LengthOfHistory = lengthOfHistory.Value;
                else
                    settings.LengthOfHistory = 100;

                if (desktopNotifications.HasValue)
                    settings.DesktopNotifications = desktopNotifications.Value;
                else
                    settings.DesktopNotifications = true;

                if (requireHover.HasValue)
                    settings.RequireHover = requireHover.Value;
                else
                    settings.RequireHover = false;

                if (sounds.HasValue)
                    settings.Sounds = sounds.Value;
                else
                    settings.Sounds = true;

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
