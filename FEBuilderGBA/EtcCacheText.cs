using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEBuilderGBA
{
    class EtcCacheTextID
    {
        Dictionary<uint, string> EtcTextID;
        Dictionary<uint, string> TextID;

        public EtcCacheTextID()
        {
            this.EtcTextID = U.LoadTSVResource1(U.ConfigEtcFilename("textid_"), false);
            this.TextID = U.LoadDicResource(U.ConfigDataFilename("textid_"));
//            U.OrderBy(this.TextID, (x) => { return (int)x.Key; });
        }

        public void Update(uint textid, string comment)
        {
            if (comment == "")
            {
                if (this.EtcTextID.ContainsKey(textid))
                {
                    this.EtcTextID.Remove(textid);
                }
            }
            else
            {
                this.EtcTextID[textid] = comment;
            }
        }
        public void Save(string romBaseFilename)
        {
            U.SaveConfigEtcTSV1("textid_", this.EtcTextID, romBaseFilename);
        }

        //マージ専用
        public void AppendList(List<UseTextID> list)
        {
            foreach (var pair in this.EtcTextID)
            {
                UseTextID.AppendTextID(list, FELint.Type.TEXTID_FOR_USER, U.NOT_FOUND, pair.Value, pair.Key);
            }
            foreach (var pair in this.TextID)
            {
                UseTextID.AppendTextID(list, FELint.Type.TEXTID_FOR_SYSTEM, U.NOT_FOUND, pair.Value, pair.Key);
            }

            if (Program.ROM.RomInfo.version() == 8)
            {
                if (Program.ROM.RomInfo.is_multibyte())
                {
                    for (uint textid = 0xE00; textid <= 0xEFF; textid++)
                    {
                        UseTextID.AppendTextID(list, FELint.Type.TEXTID_FOR_SYSTEM, U.NOT_FOUND, "", textid);
                    }
                }
                else
                {
                    for (uint textid = 0xE00; textid <= 0xFFF; textid++)
                    {
                        UseTextID.AppendTextID(list, FELint.Type.TEXTID_FOR_SYSTEM, U.NOT_FOUND, "", textid);
                    }
                }
            }
        }
        public UseTextID MakeUseTextID(uint textid)
        {
            string name;
            if (this.EtcTextID.TryGetValue(textid , out name))
            {
                return new UseTextID(FELint.Type.TEXTID_FOR_USER, U.NOT_FOUND, name, textid);
            }
            if (this.TextID.TryGetValue(textid, out name))
            {
                return new UseTextID(FELint.Type.TEXTID_FOR_SYSTEM, U.NOT_FOUND, name, textid);
            }
            if (Program.ROM.RomInfo.version() == 8)
            {
                if (Program.ROM.RomInfo.is_multibyte())
                {
                    if (textid >= 0xE00 && textid <= 0xEFF)
                    {
                        return new UseTextID(FELint.Type.TEXTID_FOR_SYSTEM, U.NOT_FOUND, name, textid);
                    }
                }
                else
                {
                    if (textid >= 0xE00 && textid <= 0xFFF)
                    {
                        return new UseTextID(FELint.Type.TEXTID_FOR_SYSTEM, U.NOT_FOUND, name, textid);
                    }
                }
            }

            return null;
        }

        public string GetName(uint textid)
        {
            UseTextID p = MakeUseTextID(textid);
            if (p == null)
            {
                return "";
            }
            return p.Info;
        }
    }
}
