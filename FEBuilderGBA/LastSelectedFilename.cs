using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FEBuilderGBA
{
    //編集しやすいように、最後に選択したファイル名を保存する.
    class LastSelectedFilename
    {
        class LastSelected
        {
            public string FileName;
            public int FilterIndex;
        }

        Dictionary<string, LastSelected> Dic;
        public LastSelectedFilename(string openrom)
        {
            this.Dic = new Dictionary<string, LastSelected>();

            //最初の一発目は、開いたROM自身を入れる.
            {
                LastSelected ls = new LastSelected();
                ls.FileName = openrom;
                string key = MakeKey(null, "save", "rom");
                this.Dic[key] = ls;
            }

            {
                LastSelected ls = new LastSelected();
                ls.FileName = openrom;
                string key = MakeKey(null, "open", "rom");
                this.Dic[key] = ls;
            }
        }
        void UpdateOther(string key, string fileName)
        {
            if (this.Dic.ContainsKey(key) )
            {
                this.Dic[key].FileName = fileName;
            }
            else
            {
                LastSelected ls = new LastSelected();
                ls.FileName = fileName;
                ls.FilterIndex = 0;
                this.Dic[key] = ls;
            }
        }

        public void Save(Control self,string addName,OpenFileDialog d)
        {
            LastSelected ls = new LastSelected();
            ls.FileName = d.FileName;
            ls.FilterIndex = d.FilterIndex;

            string key;
            key = MakeKey(self, "open", addName);
            this.Dic[key] = ls;

            key = MakeKey(self, "save", addName);
            UpdateOther(key, d.FileName);

            Log.Notify("Open:", d.FileName);
        }
        public void Save(Control self, string addName, SaveFileDialog d)
        {
            LastSelected ls = new LastSelected();
            ls.FileName = d.FileName;
            ls.FilterIndex = d.FilterIndex;

            string key;
            key = MakeKey(self, "save", addName);
            this.Dic[key] = ls;

            key = MakeKey(self, "open", addName);
            UpdateOther(key, d.FileName);
        }

        string MakeKey(Control self, string type, string addName)
        {
            string key ;
            if (self == null)
            {
                key = "null";
            }
            else
            {
                key = self.Name;
            }
            key += " " + type + " " + addName;
            return key;
        }

        public void Load(Control self, string addName, OpenFileDialog d)
        {
            string key = MakeKey(self,"open", addName);

            if (!this.Dic.ContainsKey(key))
            {
                return ;
            }
            LastSelected ls = this.Dic[key];

            d.FileName = ls.FileName;
            int index = 0;
            if (d.Filter != null)
            {
                int n = ls.FilterIndex * 2;
                string[] sp = d.Filter.Split('|');
                if (n + 1 < sp.Length)
                {
                    index = ls.FilterIndex;
                }
            }

            d.FilterIndex = index;
            d.InitialDirectory = Path.GetDirectoryName(ls.FileName);
            d.RestoreDirectory = true;
//            d.CheckFileExists = true;
        }
        public void Load(Control self, string addName, SaveFileDialog d, string filename)
        {
            string key = MakeKey(self,"save", addName);

            if (filename != "")
            {
                d.FileName = filename;
            }

            if (!this.Dic.ContainsKey(key))
            {
                return;
            }
            LastSelected ls = this.Dic[key];

            //d.FileName = ls.FileName;
            int index = 0;
            if (d.Filter != null)
            {
                int n = ls.FilterIndex * 2;
                string[] sp = d.Filter.Split('|');
                if (n + 1 < sp.Length)
                {
                    index = ls.FilterIndex;
                }
            }
            d.FilterIndex = index;
        }
        public string GetLastFilename()
        {
            LastSelected ls = null;
            foreach(var pair in Dic)
            {
                ls = pair.Value;
            }
            if (ls == null)
            {
                return "";
            }
            return ls.FileName;
        }
        public string GetLastFilename(Control self, string addName)
        {
            string key = MakeKey(self, "save", addName);

            if (!this.Dic.ContainsKey(key))
            {
                return "";
            }
            LastSelected ls = this.Dic[key];
            return ls.FileName;
        }
    }
}
