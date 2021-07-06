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
        public void AppendList(List<UseValsID> list)
        {
            foreach (var pair in this.EtcTextID)
            {
                UseValsID.AppendTextID(list, FELint.Type.TEXTID_FOR_USER, U.NOT_FOUND, pair.Value, pair.Key);
            }
            foreach (var pair in this.TextID)
            {
                UseValsID.AppendTextID(list, FELint.Type.TEXTID_FOR_SYSTEM, U.NOT_FOUND, pair.Value, pair.Key);
            }

            if (Program.ROM.RomInfo.version() == 8)
            {
                if (Program.ROM.RomInfo.is_multibyte())
                {
                    for (uint textid = 0xE00; textid <= 0xEFF; textid++)
                    {
                        UseValsID.AppendTextID(list, FELint.Type.TEXTID_FOR_SYSTEM, U.NOT_FOUND, "", textid);
                    }
                }
                else
                {
                    for (uint textid = 0xE00; textid <= 0xFFF; textid++)
                    {
                        UseValsID.AppendTextID(list, FELint.Type.TEXTID_FOR_SYSTEM, U.NOT_FOUND, "", textid);
                    }
                }
            }
        }
        public UseValsID MakeUseTextID(uint textid)
        {
            string name;
            if (this.EtcTextID.TryGetValue(textid , out name))
            {
                return new UseValsID(FELint.Type.TEXTID_FOR_USER, U.NOT_FOUND, name, textid, UseValsID.TargetTypeEnum.TEXTID);
            }
            if (this.TextID.TryGetValue(textid, out name))
            {
                return new UseValsID(FELint.Type.TEXTID_FOR_SYSTEM, U.NOT_FOUND, name, textid, UseValsID.TargetTypeEnum.TEXTID);
            }
            if (Program.ROM.RomInfo.version() == 8)
            {
                if (Program.ROM.RomInfo.is_multibyte())
                {
                    if (textid >= 0xE00 && textid <= 0xEFF)
                    {
                        return new UseValsID(FELint.Type.TEXTID_FOR_SYSTEM, U.NOT_FOUND, name, textid, UseValsID.TargetTypeEnum.TEXTID);
                    }
                }
                else
                {
                    if (textid >= 0xE00 && textid <= 0xFFF)
                    {
                        return new UseValsID(FELint.Type.TEXTID_FOR_SYSTEM, U.NOT_FOUND, name, textid, UseValsID.TargetTypeEnum.TEXTID);
                    }
                }
            }

            return null;
        }

        public string GetName(uint textid)
        {
            UseValsID p = MakeUseTextID(textid);
            if (p == null)
            {
                return "";
            }
            return p.Info;
        }
    }
}
