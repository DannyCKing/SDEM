using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEMViewModels.Global
{
    class Logger
    {
        private static readonly string FOLDER_NAME = "SDEM";

        private static readonly string LOG_FOLDER_NAME = "Log";

        private static string ProgramInstance = "";

        public static string ApplicationDataFolder
        {
            get
            {
                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var appFolder = Path.Combine(appDataFolder, FOLDER_NAME);

                // check if folder exists
                if (!Directory.Exists(appFolder))
                {
                    Directory.CreateDirectory(appFolder);
                }

                var logFolder = Path.Combine(appDataFolder, FOLDER_NAME, LOG_FOLDER_NAME);

                // check if folder exists
                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }

                return appFolder;
            }
        }

        public static string LogFolder
        {
            get
            {
                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var appFolder = Path.Combine(appDataFolder, FOLDER_NAME);

                // check if folder exists
                if (!Directory.Exists(appFolder))
                {
                    Directory.CreateDirectory(appFolder);
                }

                var logFolder = Path.Combine(appDataFolder, FOLDER_NAME, LOG_FOLDER_NAME);

                // check if folder exists
                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }

                return logFolder;
            }
        }



        private const string BASE_LOG_FILE_NAME = "log.txt";

        public static void Log(string message, Exception e = null)
        {
            if(ProgramInstance == "")
            {
                ProgramInstance = DateTime.Now.ToString("MMss");
            }
            try
            {
                string folderPath = LogFolder;

                var fileName = Path.Combine(folderPath, GetDateString() + BASE_LOG_FILE_NAME);

                File.AppendAllText(fileName, GetLogMessage(message, e));
            }
            catch (Exception logException)
            {
                // Unable to log to file
                Console.WriteLine("Unable to log to file");
                Console.WriteLine(GetLogMessage(message, e));
            }
        }

        private static string GetDateString()
        {
            var result = DateTime.Now.ToString("yyyy_MM_dd_");
            return result;
        }

        private static string GetLogMessage(string message, Exception e)
        {
            string exceptionMessage = GetExceptionString(e);
            string logMessage = GetDateTimeString();
            logMessage += " " + ProgramInstance + " " + message + exceptionMessage + Environment.NewLine;

            return logMessage;
        }

        /// <summary>
        /// delete log files older the X days
        /// </summary>
        /// <param name="days">log files older than this will be deleted</param>
        internal static void DeleteOldLogsFiles(int days)
        {
            try
            {
                string folderPath = LogFolder;

                var files = Directory.GetFiles(folderPath);
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo fi = new FileInfo(files[i]);
                    int daysToDelete = 0 - days;
                    if (fi.LastAccessTime < DateTime.Now.AddDays(daysToDelete))
                    {
                        fi.Delete();
                    }
                }
                Log("Deleting files older than " + days + " days.");
            }
            catch (Exception logException)
            {
                // Unable to log to file
                Log("Unable to delete files older than " + days + " days.");
            }
        }

        private static string GetDateTimeString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
        }

        private static string GetExceptionString(Exception error)
        {
            string errorString = "";
            Exception currentError = error;
            while (currentError != null)
            {
                errorString += currentError.Message + " ";

                // Get stack trace for the exception with source file information
                var st = new StackTrace(currentError, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();

                errorString += st;
                errorString += frame;
                errorString += line;

                currentError = currentError.InnerException;
            }

            return errorString;
        }
    }
}
