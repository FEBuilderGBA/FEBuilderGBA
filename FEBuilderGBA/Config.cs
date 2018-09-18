using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    class Config :  Dictionary<string,string>
    {
        public void Save()
        {
            Save(this.ConfigFilename);
        }
            
        public void Save(string fullfilename)
        {
            //XMLシリアライザが初期化できないので自前でやる.

            XmlDocument xml = new XmlDocument();
            XmlElement elem = xml.CreateElement("root");
            xml.AppendChild(elem);

            foreach (var pair in this)
            {
                XmlElement item_elem = xml.CreateElement("item");
                elem.AppendChild(item_elem);

                XmlElement key_elem = xml.CreateElement("key");
                item_elem.AppendChild(key_elem);

                XmlNode item_node = xml.CreateNode(XmlNodeType.Text, "", "");
                item_node.Value = pair.Key;
                key_elem.AppendChild(item_node);

                XmlElement value_elem = xml.CreateElement("value");
                item_elem.AppendChild(value_elem);

                item_node = xml.CreateNode(XmlNodeType.Text, "", "");
                item_node.Value = pair.Value;
                value_elem.AppendChild(item_node);
            }
            try
            {
                using (StreamWriter w = new StreamWriter(fullfilename))
                {
                    xml.Save(w);
                }
            }
            catch (Exception e)
            {
                R.ShowStopError("設定ファイルに書き込めません。\r\n{0}\r\n{1}",fullfilename,e.ToString());
            }
            return;
        }
        string ConfigFilename;

        public void Load(string fullfilename)
        {
            this.ConfigFilename = fullfilename;
            if (!System.IO.File.Exists(fullfilename))
            {
                return;
            }

            //XMLシリアライザが初期化できないので自前でやる.
            using (StreamReader r = new StreamReader(fullfilename))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(r);

                XmlElement elem = xml.DocumentElement;
                foreach (XmlNode node in elem.SelectNodes("item"))
                {
                    XmlNode key = node.SelectSingleNode("key");
                    XmlNode value = node.SelectSingleNode("value");
                    this[key.InnerText] = value.InnerText;
                }
            }
        }

        public string at(string key, string def = "")
        {
            if (!this.ContainsKey(key))
            {//設定されていないっぽい
                return def;
            }
            return this[key];
        }


        public Keys[] ShortCutKeys { get; private set; }
        public void UpdateShortcutKeys()
        {
            ShortCutKeys = new Keys[15];
            U.CheckKeys(Program.Config.at("ShortCutKey1", "F5"), out ShortCutKeys[0]);
            U.CheckKeys(Program.Config.at("ShortCutKey2", "Ctrl+F5"), out ShortCutKeys[1]);
            U.CheckKeys(Program.Config.at("ShortCutKey3", ""), out ShortCutKeys[2]);
            U.CheckKeys(Program.Config.at("ShortCutKey4", ""), out ShortCutKeys[3]);
            U.CheckKeys(Program.Config.at("ShortCutKey5", ""), out ShortCutKeys[4]);
            U.CheckKeys(Program.Config.at("ShortCutKey6", ""), out ShortCutKeys[5]);
            U.CheckKeys(Program.Config.at("ShortCutKey7", ""), out ShortCutKeys[6]);
            U.CheckKeys(Program.Config.at("ShortCutKey8", ""), out ShortCutKeys[7]);
            U.CheckKeys(Program.Config.at("ShortCutKey9", ""), out ShortCutKeys[8]);
            U.CheckKeys(Program.Config.at("ShortCutKey10", ""), out ShortCutKeys[9]);
            U.CheckKeys(Program.Config.at("ShortCutKey11", ""), out ShortCutKeys[10]);
            U.CheckKeys(Program.Config.at("ShortCutKey12", ""), out ShortCutKeys[11]);
            U.CheckKeys(Program.Config.at("ShortCutKey13", ""), out ShortCutKeys[12]);
            U.CheckKeys(Program.Config.at("ShortCutKey14", ""), out ShortCutKeys[13]);
            U.CheckKeys(Program.Config.at("ShortCutKey15", ""), out ShortCutKeys[14]);
        }
    }
}
