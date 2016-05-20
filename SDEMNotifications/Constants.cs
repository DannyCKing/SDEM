using System;
using System.IO;
using System.Linq;

namespace SDEMNotifications
{
    class Constants
    {
        public static string NOTIFICATION_IMAGE_LOCATION = GetSDEMLogo();

        private static string GetSDEMLogo()
        {
            string fileName = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames().FirstOrDefault(x => x.Contains("letter_s"));
            //var logofile = SDEMNotifications.Properties.Resources.letter_s.G
            String strAppPath = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            String strFilePath = Path.Combine(strAppPath, "//..//..//Resources");
            String strFullFilename = Path.Combine(strFilePath, "letter_s.jpg");

            return @"C:\Users\dking\Documents\Visual Studio 2012\Projects\SDEM\SDEMNotifications\Resources\letter_s.jpg";
            //return ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar + "letter_s.jpg";
        }
    }
}
