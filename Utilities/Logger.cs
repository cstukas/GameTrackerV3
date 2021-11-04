using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class Logger
    {
        public static void Log(string message, bool writeToConsole = false)
        {
            return;

            DateTime now = DateTime.Now;
            string date = now.ToString("yyyyMMdd");

            string folderName = "Logs";
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            string logFileName = $@"{folderName}\Log_" + date + ".log";
            using (StreamWriter streamWriter = new StreamWriter(logFileName, true))
            {
                string time = now.ToString("HH:mm:ss");
                string line = string.Format("{0}\t{1}", time, message);
                streamWriter.WriteLine(line);
                if (writeToConsole)
                {
                    Console.WriteLine(line);
                }
            }
        }

    }
}
