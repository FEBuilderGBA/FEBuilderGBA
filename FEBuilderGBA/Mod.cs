using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace FEBuilderGBA
{
    //PATCHと似ているが、
    //PATCHは機能を追加するのに対して、
    //MODはツールの画面に変更を加える
    //ただ、MODは最悪値として画面を開くたびに評価しないといけないので速度に注意しないといけない。
    public class Mod
    {
        public void Load()
        {
            //MOD
            ScanMods(System.IO.Path.Combine(Program.BaseDirectory, "config", "patch2", Program.ROM.RomInfo.VersionToFilename()));
        }

        public class ModTypeSt
        {
            public string key;
            public string type;
            public string value;
        };

        public class ModSt
        {
            public string Form;
            public List<ModTypeSt> Param;
        };
        public List<ModSt> Mods { get; private set; }

        void ScanMods(string path)
        {
            Mods = new List<ModSt>();
            if (Program.ROM.RomInfo.version() == 0)
            {
                return;
            }

            string lang = OptionForm.lang();

            string[] files;
            try
            {
                files = Directory.GetFiles(path, "MOD_*.txt", SearchOption.AllDirectories);
            }
            catch (System.IO.IOException e)
            {
                R.ShowStopError("パッチ探索中にエラーが発生しました。\r\n{0}", e.ToString());
                return ;
            }

            foreach (string fullfilename in files)
            {
                ModSt m = LoadMod(fullfilename, lang);
                if (m == null)
                {
                    continue;
                }
                Mods.Add(m);
            }
        }
        ModSt LoadMod(string fullfilename, string lang)
        {
            ModSt p = new ModSt();
            p.Param = new List<ModTypeSt>();
            Dictionary<string, ModTypeSt> keymap = new Dictionary<string, ModTypeSt>();
            if (!U.IsRequiredFileExist(fullfilename))
            {
                return p;
            }

            using (StreamReader reader = File.OpenText(fullfilename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (U.IsComment(line))
                    {
                        continue;
                    }
                    line = U.ClipComment(line);

                    int sep = line.IndexOf('=');
                    if (sep < 0)
                    {
                        continue;
                    }
                    string key = line.Substring(0, sep);
                    string value = line.Substring(sep + 1);

                    //言語名があるか？
                    int langpos = key.IndexOf('.');
                    if (langpos > 0)
                    {
                        if (key.Substring(langpos + 1) != lang)
                        {//言語指定があるが現在の言語ではない.
                            continue;
                        }
                        //言語指定があった!
                        key = key.Substring(0, langpos);
                    }
                    
                    if (key.IndexOf("IF:") == 0)
                    {//MODでは $GREP4マクロとかは無効. IF_NOTもない.
                     //よって、事前定数になるので、条件にマッチしないMODは無視できる.

                        string[] cp = key.Split(':');
                        if (cp.Length <= 1)
                        {
                            continue;
                        }
                        uint address = U.atoi0x(cp[1]);
                        if (!U.isSafetyOffset(address))
                        {
                            R.Error("IFパースエラー、このアドレス({1})は危険です。\r\n{0}\r\n{2}", line, U.To0xHexString(address), fullfilename);
                            continue;
                        }

                        string[] args = value.Split(' ');
                        if (args.Length <= 1)
                        {
                            R.Error("IFパースエラー、この値にはスペースがありません \r\n{0}\r\n{1}", line, fullfilename);
                            continue;
                        }

                        uint[] data = new uint[args.Length];
                        for (int i = 0; i < args.Length; i++)
                        {
                            data[i] = Program.ROM.u8(address + (uint)i);
                        }

                        uint[] need = new uint[args.Length];
                        for (int i = 0; i < args.Length; i++)
                        {
                            need[i] = U.atoi0x(args[i]);
                        }

                        bool notFound = false;
                        for (int i = 0; i < args.Length; i++)
                        {
                            if (data[i] != need[i])
                            {
                                notFound = true;
                                break;
                            }
                        }

                        if (notFound)
                        {//不成立
//                            R.Notify("IF不成立!" 
//                                    + line + "\r\n"
//                                    + "Address:" + U.To0xHexString(address) + "\r\n"
//                                    + "Need   :" + U.DumpByte(need) + "\r\n"
//                                    + "ROM Data:" + U.DumpByte(data) + "\r\n"
//                                    + fullfilename + "\r\n"
                            //                                    );
                            return null;
                        }
                    }
                    else if (key == "FORM")
                    {
                        p.Form = value;
                    }
                    else
                    {
                        ModTypeSt mtype;
                        bool first;
                        if (keymap.ContainsKey(key))
                        {
                            mtype = keymap[key];
                            first = false;
                        }
                        else
                        {
                            mtype = new ModTypeSt();
                            first = true;
                        }

                        string[] cp = key.Split(':');
                        mtype.key = U.at(cp,0);
                        mtype.type = U.at(cp, 1, "VALUE");
                        mtype.value = value;

                        if ( first )
                        {
                            p.Param.Add(mtype);
                            //英語などの多言語があるので、マップに記録していきます.
                            keymap[key] = mtype;
                        }
                    }
                }
            }


            if (U.IsEmpty(p.Form))
            {
                R.Error("FORMがありません\r\n{0}", fullfilename);
                return null;
            }

            return p;
        }
    }
}
