using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    class Log
    {
        public static event EventHandler UpdateEvent;
        public static StringBuilder NonWriteStringB = new StringBuilder();
        [Conditional("DEBUG")] 
        public static void Debug(
              params string[] args)
        {
            writeString("D:", args);
        }

        public static void Notify(
            params string[] args)
        {
            writeString("N:",  args);
        }
        public static void Error(
              params string[] args)
        {
            writeString("E:", args);
        }
        private static void writeString(string type, params string[] args)
        {
            string str = string.Join(" ", args);
            lock (thisLock)
            {
                NonWriteStringB.AppendLine(str);
            }
            System.Diagnostics.Debug.WriteLine(str);

            if (NonWriteStringB.Length >= 2048)
            {
                SyncLog();
            }

            if (UpdateEvent != null)
            {
                UpdateEvent(null, EventArgs.Empty);
            }
        }
        static string GetLogFilename()
        {
            return System.IO.Path.Combine(Program.BaseDirectory, "config","log", "log.txt");
        }
        static string GeOldLogFilename(int num)
        {
            return System.IO.Path.Combine(Program.BaseDirectory, "config","log","log."+num+".txt.7z");
        }
        private static Object thisLock = new Object();  
        public static void SyncLog()
        {
            lock (thisLock)
            {
                string fullfilename = GetLogFilename();
                try
                {
                    File.AppendAllText(fullfilename, NonWriteStringB.ToString());
                    NonWriteStringB.Clear();

                    if (U.GetFileSize(fullfilename) >= 1024 * 256)
                    {
                        Rotate(fullfilename);
                    }
                }
                catch (Exception)
                {
                    //どうすることもできない
                }
            }
        }

        const int GEN_COUNT = 30;
        static void Rotate(string current)
        {
            {
                string oldlog2 = GeOldLogFilename(GEN_COUNT);
                if (File.Exists(oldlog2))
                {
                    File.Delete(oldlog2);
                }
            }
            for (int i = GEN_COUNT; i > 0; i--)
            {
                string oldlog1 = GeOldLogFilename(i - 1);
                string oldlog2 = GeOldLogFilename(i);

                if (File.Exists(oldlog1))
                {
                    File.Move(oldlog1, oldlog2);
                }
            }
            {
                string oldlog2 = GeOldLogFilename(0);
                ArchSevenZip.Compress(oldlog2, current);
                File.Delete(current);
            }
        }
        static string[] LogToLines()
        {
            SyncLog();
            string[] lines;
            lock (thisLock)
            {
                string fullfilename = GetLogFilename();
                lines = File.ReadAllLines(fullfilename);
            }
            return lines;
        }

        public static String LogToString(int showLines = 100)
        {
            string[] lines = LogToLines();
            if (lines.Length < showLines)
            {
                return string.Join("\r\n", lines);
            }
            else
            {
                return string.Join("\r\n", lines, lines.Length - showLines, showLines);
            }
        }
        public static void LogToListBox(ListBox LogList)
        {
            string[] lines = LogToLines();
            int length = lines.Length;
            int limit = Math.Min(100, length);

            LogList.BeginUpdate();
            LogList.Items.Clear();

            for (int i = 0; i < limit; i++)
            {
                LogList.Items.Add(lines[length - (limit  - i)]);
            }
            LogList.EndUpdate();
        }

        public static void ToFile(string filename)
        {
            U.WriteAllText(filename, LogToString());
            U.SelectFileByExplorer(filename);
        }
        public static void OpenLogDir()
        {
            U.SelectFileByExplorer(GetLogFilename());
        }

        public static void TouchLogDirectory()
        {
            string dir = Path.Combine(Program.BaseDirectory, "config", "log");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    };
}
