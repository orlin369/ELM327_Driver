using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Loging
{
    public class Log
    {

        #region Variables
        
        /// <summary>
        /// Specify a name for your applicatin folder.
        /// </summary>
        private string logFolderPath = @"C:\";

        /// <summary>
        /// List of messages that must be write to a file.
        /// </summary>
        private List<String> logMessages = new List<String>();
        
        /// <summary>
        /// Maximum messages count in the message list.
        /// </summary>
        private int colectionSize = 1;
        
        /// <summary>
        /// Enable loging.
        /// </summary>
        public bool Enable = true;
        
        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="logFolderPath">Path for the application logs.</param>
        public Log(string logFolderPath)
        {
            // The folder for the roaming current user. 
            this.logFolderPath = logFolderPath;// Path.Combine(folder, applicationName);
        }

        #endregion

        #region Public

        /// <summary>
        /// Set colection message size.
        /// </summary>
        /// <param name="colectionSize">Size of messages to be writen after this count.</param>
        public void SetColectionSize(int colectionSize)
        {
            this.colectionSize = colectionSize;
        }

        /// <summary>
        /// This method will create automaticly.
        /// Log file in folder with staic path.
        /// Every new day will be create a one new file.
        /// </summary>
        /// <param name="LogSource">Who send this message log.</param>
        /// <param name="MessageText">Concreet message.</param>
        public void CreateRecord(string LogSource, string MessageText, LogMessageTypes MessageType, bool EndOfLogs = false)
        {
            // Write LOG record to the message buffer if is enabled.
            if (Enable)
            {
                // Structre of the message.
                // LogSource\tYear.Month.Day/Hour:Minute:Seconds.Miliseconds\tType\tMessageText
                string dateAndTime = DateTime.Now.ToString("yyyy.MM.dd/HH:mm:ss.fff", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                string message = LogSource + "\t" + dateAndTime + "\t" + MessageType.ToString() + "\t" + MessageText;
                
                // Add message to the message buffer.
                this.logMessages.Add(message);
                
                // Write end of log line
                if(EndOfLogs)
                {
                    this.logMessages.Add("\r\n===================================================================================\r\n");
                }

                // If filr are critical count, just write it to a file.
                if ((this.logMessages.Count > this.colectionSize) || EndOfLogs)
                {
                    this.WtriteToLogFile();
                }
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// Write messages to LOG file.
        /// </summary>
        private void WtriteToLogFile()
        {
            // Write buffer to the file if is enabled.
            if (Enable)
            {
                // Create Log file name.
                // Structure of file name.
                // Log_DateAndTime.txt
                string dateAndTime = DateTime.Now.ToString("yyyy.MM.dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                string fileName = "Log_" + dateAndTime + ".txt";
                
                // Combine the base AppData folder with your specific folder (AppFolderName).

                // Check if folder exists and if not, create it.
                if (!Directory.Exists(this.logFolderPath))
                {
                    Directory.CreateDirectory(this.logFolderPath);
                }

                // Generate full path log file folder.
                string fullPath = Path.Combine(this.logFolderPath, fileName);

                // Check if file exists and if not, create it.
                if (!System.IO.File.Exists(fullPath))
                {
                    try
                    {
                        // File writer it use for writing a LOG file.
                        // Create the file.
                        System.IO.StreamWriter theFile = new System.IO.StreamWriter(fullPath);

                        // Write header.
                        string header = "This file is automatic generated.\r\n";
                        header += String.Format("This file belongs to: \"{0}\"\r\n\r\n", this.logFolderPath);
                        header += "LOG SOURCE\tDATE & TIME        \tTYPE\tMESSAGE\r\n";
                        theFile.WriteLine(header);

                        for (int messageCount = 0; messageCount < this.logMessages.Count; messageCount++)
                        {
                            // Write the string to a file.
                            theFile.WriteLine(this.logMessages[messageCount]);
                        }
                        // Close the log file.
                        theFile.Close();
                    }
                    catch (Exception exception)
                    {
                        throw new Exception("Internal exception.", exception);
                    }
                }
                else
                {
                    // Append data to file.
                    try
                    {
                        // File writer it use for writing a LOG file.
                        // Create the file.
                        System.IO.StreamWriter theFile = new System.IO.StreamWriter(fullPath, true);

                        for (int messageCount = 0; messageCount < this.logMessages.Count; messageCount++)
                        {
                            // Write the string to a file.
                            theFile.WriteLine(this.logMessages[messageCount]);
                        }

                        // Close the log file.
                        theFile.Close();

                    }
                    catch (Exception exception)
                    {
                        throw new Exception("Internal exception.", exception);
                    }
                }

                // Clear the message tail.
                this.logMessages.Clear();
            }
        }

        #endregion

    }
}
