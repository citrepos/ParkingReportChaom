using System;
using System.IO;
using ParkingManagementReport.Common;

namespace ParkingManagementReport.Utilities
{
    internal class Logger
    {
        public static void Error(string message, string functionName = "")
        {
            WriteLogFile(message, functionName, LogTypes.Error);
        }

        public static void Warn(string message, string functionName = "")
        {
            WriteLogFile(message, functionName, LogTypes.Warning);
        }

        public static void Info(string message, string functionName = "")
        {
            WriteLogFile(message, functionName, LogTypes.Inform);
        }

        public static void Success(string message, string functionName = "")
        {
            WriteLogFile(message, functionName, LogTypes.Success);
        }

        private static void WriteLogFile(string message, string functionName, string logType)
        {
            try
            {
                if (Configs.UseLogReport)
                {
                    DateTime dateTimeNow = DateTime.Now;
                    string folder = dateTimeNow.Month.ToString();

                    string strFile = $"{Configs.Paths.BackupDirectory}\\LOGCarpark\\{folder}\\LOGReport_{dateTimeNow:ddMMyyyy}.txt";
                    if (!Directory.Exists(strFile))
                        Directory.CreateDirectory(strFile);

                    functionName = String.IsNullOrEmpty(functionName) ? "" : $"[{functionName}]";

                    string strLog = $"[{dateTimeNow.ToString("yyyy-MM-dd HH:mm:ss")}] [{logType}] => {functionName} {message} | User >> {AppGlobalVariables.OperatingUser.UserName}";

                    if (!File.Exists(strFile))
                    {
                        StreamWriter sw = File.CreateText(strFile);
                        sw.WriteLine(strLog);
                        sw.Flush();
                        sw.Close();
                    }
                    else
                    {
                        FileStream MyFileStream = new FileStream(strFile, FileMode.Append, FileAccess.Write, FileShare.Read);
                        StreamWriter sw = new StreamWriter(MyFileStream);
                        sw.WriteLine(strLog);
                        sw.Close();
                        MyFileStream.Close();
                    }
                }
            }
            catch { }
        }

        static class LogTypes
        {
            public static readonly string Error = "ERROR";
            public static readonly string Warning = "WARNING";
            public static readonly string Inform = "INFO";
            public static readonly string Success = "SUCCESS";
        }
    }
}