using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public static class Logger
    {
        public static void Log(string message, bool writeToConsole = false)
        {
            string t = DateTime.Now.ToString("HH:mm:ss");
            string mess = string.Format("{0}\t{1}", t, message);
            Console.WriteLine(mess);
            return;

            DateTime now = DateTime.Now;
            string date = now.ToString("yyyyMMdd");

            string folderName = "Logs";
            if(!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);
            
            string logFileName = $@"{folderName}\DataAccessLog_" + date + ".log";

            bool writeSuccess = false;
            int attempts = 0;
            int maxAttempts = 200;
            
            // Want to make sure that the message will be written even if
            // the file is currently being used by another process
            // (Have run into exceptions for this happening due to connection timeouts)
            while(!writeSuccess)
            {
                // Dont want to get stuck in an infinite loop so we will only try a certain amount of times
                attempts++;
                if (attempts > maxAttempts) return;

                try
                {
                    using (StreamWriter streamWriter = new StreamWriter(logFileName, true))
                    {
                        string time = now.ToString("HH:mm:ss");
                        string line = string.Format("{0}\t{1}", time, message);
                        streamWriter.WriteLine(line);
                        if (writeToConsole) Console.WriteLine(line);
                        writeSuccess = true;
                    }
                }
                catch
                {
                    Console.WriteLine("RETRYING LOGGING");
                }

            }

          



        }

    }
}
