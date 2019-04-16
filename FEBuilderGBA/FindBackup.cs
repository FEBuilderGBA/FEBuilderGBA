using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;

namespace FEBuilderGBA
{
    class FindBackup
    {
        public class FileInfo
        {
            public string FilePath;
            public DateTime Date;
            public Undo Undo;
        };
        public List<FileInfo> Files{ get; private set; }
        public string Prefix{ get; private set; }
        public string OrignalFilename{ get; private set; }

        public FindBackup()
        {
            this.Files = new List<FileInfo>();
            String dir = Path.GetDirectoryName(Program.ROM.Filename);

            this.OrignalFilename = MainFormUtil.FindOrignalROM(dir);

            if (Program.ROM.IsVirtualROM)
            {
                this.Prefix = "";
                return;
            }

            string prefix = Path.GetFileNameWithoutExtension(Program.ROM.Filename);
            this.Prefix = prefix + ".backup.";
            Scan(dir, this.Prefix);
        }

        bool IsTempFile(string filename, string prefix)
        {
            int prefixLength = prefix.Length;

            string[] prefixArray = new string[] { ".emulator"
                , ".emulator1"
                , ".emulator2"
                , ".sapply"
                , ".program1"
                , ".program2"
                , ".program3"
            };
            for (int i = 0; i < prefixArray.Length; i++)
            {
                int hit = filename.LastIndexOf(prefixArray[i]);
                if (hit >= prefixLength)
                {
                    return true;
                }
            }
            return false;
        }

        //末尾に無改造ROMを追加する.
        public void AppendOrignalROMToBackupList()
        {
            if (this.OrignalFilename == "")
            {
                return;
            }
            FileInfo fi = MakeOrignalFileInfo(this.OrignalFilename);
            this.Files.Add(fi);
            //日付順にソートする.
            this.Files.Sort((a, b) => { return b.Date.CompareTo(a.Date); });
        }

        FileInfo MakeOrignalFileInfo(string filepath)
        {
            FileInfo fi = new FileInfo();
            fi.FilePath = filepath;
            fi.Date = GetFileDate(filepath);
            fi.Undo = new Undo();

            return fi;
        }

        public bool Scan(string dir,string prefix)
        {
            string[] files = Directory.GetFiles(dir, "*", SearchOption.TopDirectoryOnly);
            foreach (string filepath in files)
            {
                string ext = U.GetFilenameExt(filepath);
                if (!(ext == ".GBA" || ext == ".7Z" || ext == ".BIN" || ext == ".UPS"))
                {
                    continue;
                }

                string filename = Path.GetFileNameWithoutExtension(filepath);
                if (filename.IndexOf(prefix) != 0)
                {//prefixを保持していない.
                    continue;
                }

                if (IsAlreadyRegist(filename))
                {
                    continue;
                }

                if (IsTempFile(filename, prefix))
                {
                    continue;
                }

                FileInfo fi = new FileInfo();
                fi.FilePath = filepath;
                fi.Date = GetFileDate(filepath);
                fi.Undo = new Undo();
                Files.Add(fi);
            }

            if (Program.ROM.Modified == true)
            {//現在のROMを変更している場合、未保存の現在のROMが比較対象に設定できますね
                FileInfo fi = new FileInfo();
                fi.FilePath = Program.ROM.Filename;
                fi.Date = File.GetLastWriteTime(Program.ROM.Filename);
                fi.Undo =  Program.Undo;
                Files.Add(fi);
            }

            //日付順にソートする.
            this.Files.Sort((a, b) => { return b.Date.CompareTo(a.Date); });
            return true;
        }
        DateTime GetFileDate(string filepath)
        {
            filepath = Path.GetFileNameWithoutExtension(filepath);

            int backupIndex = filepath.IndexOf(".backup.");
            if (backupIndex < 0)
            {
                return File.GetLastWriteTime(filepath);
            }
            backupIndex += ".backup.".Length;
            string s = U.substr(filepath, backupIndex,4+2+2+2+2+2);

            DateTime rdate;
            bool r = DateTime.TryParseExact(s, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out rdate);
            if (!r)
            {
                return File.GetLastWriteTime(filepath);
            }
            return rdate;
        }
        bool IsAlreadyRegist(string filename)
        {
            for (int i = 0; i < Files.Count; i++)
            {
                if (Files[i].FilePath == filename)
                {
                    return true;
                }
            }
            return false;
        }

        public byte[] OpenToByte(FileInfo fi)
        {
            return MainFormUtil.OpenROMToByte(fi.FilePath);
        }

    }
}
