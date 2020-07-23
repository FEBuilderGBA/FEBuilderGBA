using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEBuilderGBA
{
    public class UseValsID
    {
        public FELint.Type DataType { get; private set; }
        public uint ID { get; private set; }
        public string Info { get; private set; }
        public uint Addr { get; private set; }
        public uint Tag { get; private set; }
        public TargetTypeEnum TargetType { get; private set; }

        public enum TargetTypeEnum
        {
             TEXTID
            ,BG
            ,SONG
        };

        public UseValsID(FELint.Type dataType, uint addr, string info, uint id, TargetTypeEnum targetType, uint tag = U.NOT_FOUND)
        {
            this.DataType = dataType;
            this.Addr = addr;
            this.Info = info;
            this.ID = id;
            this.TargetType = targetType;
            this.Tag = tag;
        }
        public static void AppendTextID(List<UseValsID> list, FELint.Type dataType, uint addr, string info, uint id, uint tag = U.NOT_FOUND)
        {
            if (id == 0 || id >= 0x7FFF)
            {
                return;
            }
            list.Add(new UseValsID(dataType, addr, info, id, TargetTypeEnum.TEXTID, tag));
        }
        public static void AppendSongID(List<UseValsID> list, FELint.Type dataType, uint addr, string info, uint id, uint tag = U.NOT_FOUND)
        {
            if (id == 0 || id >= 0x7FFF)
            {
                return;
            }
            list.Add(new UseValsID(dataType, addr, info, id, TargetTypeEnum.SONG, tag));
        }
        public static void AppendBGID(List<UseValsID> list, FELint.Type dataType, uint addr, string info, uint id, uint tag = U.NOT_FOUND)
        {
            if (id >= 0x7FFF)
            {
                return;
            }
            list.Add(new UseValsID(dataType, addr, info, id, TargetTypeEnum.BG, tag));
        }

        static void AppendVarsID_Low(List<UseValsID> list, FELint.Type dataType, InputFormRef ifr, uint[] textIDIndexes,TargetTypeEnum targetType)
        {
            List<U.AddrResult> arlist = ifr.MakeList();
            for (int i = 0; i < ifr.DataCount; i++)
            {
                U.AddrResult ar = arlist[i];
                for (int n = 0; n < textIDIndexes.Length; n++)
                {
                    uint id = Program.ROM.u16(ar.addr + textIDIndexes[n]);
                    if (id == 0 || id >= 0x7FFF)
                    {
                        continue;
                    }
                    list.Add(new UseValsID(dataType, ar.addr, ar.name, id, targetType, (uint)i));
                }
            }
        }
        static void AppendVarsID_Byte_Low(List<UseValsID> list, FELint.Type dataType, InputFormRef ifr, uint[] textIDIndexes, TargetTypeEnum targetType)
        {
            List<U.AddrResult> arlist = ifr.MakeList();
            for (int i = 0; i < ifr.DataCount; i++)
            {
                U.AddrResult ar = arlist[i];
                for (int n = 0; n < textIDIndexes.Length; n++)
                {
                    uint id = Program.ROM.u8(ar.addr + textIDIndexes[n]);
                    if (id == 0 || id >= 0x7FFF)
                    {
                        continue;
                    }
                    list.Add(new UseValsID(dataType, ar.addr, ar.name, id, targetType, (uint)i));
                }
            }
        }
        public static void AppendTextID(List<UseValsID> list, FELint.Type dataType, InputFormRef ifr, uint[] textIDIndexes)
        {
            AppendVarsID_Low(list, dataType, ifr, textIDIndexes, TargetTypeEnum.TEXTID);
        }
        public static void AppendSongID(List<UseValsID> list, FELint.Type dataType, InputFormRef ifr, uint[] songIDIndexes , bool isByte = false)
        {
            if (isByte)
            {
                AppendVarsID_Byte_Low(list, dataType, ifr, songIDIndexes, TargetTypeEnum.SONG);
            }
            else
            {
                AppendVarsID_Low(list, dataType, ifr, songIDIndexes, TargetTypeEnum.SONG);
            }
        }
        public static void AppendTextID(List<UseValsID> list, FELint.Type dataType, InputFormRef ifr, uint[] textIDIndexes, uint eventIndex, List<uint> tracelist)
        {
            List<U.AddrResult> arlist = ifr.MakeList();
            for (int i = 0; i < ifr.DataCount; i++)
            {
                U.AddrResult ar = arlist[i];
                for (int n = 0; n < textIDIndexes.Length; n++)
                {
                    uint id = Program.ROM.u16(ar.addr + textIDIndexes[n]);
                    if (id == 0 || id >= 0x7FFF)
                    {
                        continue;
                    }
                    list.Add(new UseValsID(dataType, ar.addr, ar.name, id, TargetTypeEnum.TEXTID, (uint)i));
                }
                uint eventAddr = Program.ROM.p32(ar.addr + eventIndex);
                if (U.isSafetyOffset(eventAddr))
                {
                    EventCondForm.MakeVarsIDArrayByEventAddress(list, eventAddr, ar.name, tracelist);
                }
            }
        }

        public static void AppendTextIDPP(List<UseValsID> list, FELint.Type dataType, InputFormRef ifr, uint[] textIDIndexes)
        {
            List<U.AddrResult> arlist = ifr.MakeList();
            for (int i = 0; i < ifr.DataCount; i++)
            {
                U.AddrResult ar = arlist[i];
                for (int n = 0; n < textIDIndexes.Length; n++)
                {
                    uint pointer = Program.ROM.p32(ar.addr + textIDIndexes[n]);
                    if (!U.isSafetyOffset(pointer))
                    {
                        continue;
                    }
                    uint id = Program.ROM.u32(pointer);
                    if (id == 0 || id >= 0x7FFF)
                    {
                        continue;
                    }
                    list.Add(new UseValsID(dataType, ar.addr, ar.name, id, TargetTypeEnum.TEXTID, (uint)i));
                }
            }
        }
        public static void AppendASMDATATextID(List<UseValsID> list, AsmMapFile.AsmMapSt p,uint startaddr, uint sizeof_)
        {
            uint end = startaddr + p.Length;
            for (uint addr = startaddr; addr < end; addr += sizeof_ )
            {
                uint id = Program.ROM.u16(addr);
                list.Add(new UseValsID(FELint.Type.ASMDATA, addr, p.Name, id, TargetTypeEnum.TEXTID, addr));
            }
        }

        public static Dictionary<uint, bool> ConvertMaps(List<UseValsID> textIDList)
        {
            Dictionary<uint, bool> textmap = new Dictionary<uint, bool>();
            textmap[0] = true;
            foreach(var p in textIDList)
            {
                textmap[p.ID] = true;
            }
            return textmap;
        }

        public static UseValsID GetNowMeasuring()
        {
            return new UseValsID(FELint.Type.FELINTBUZY_MESSAGE, U.NOT_FOUND, R._("計測中..."), U.NOT_FOUND, TargetTypeEnum.TEXTID);
        }
        public static void RemoveDuplicates(List<UseValsID> list)
        {
            if (list.Count <= 1)
            {
                return;
            }

            for (int n = 0; n < list.Count; n++ )
            {
                UseValsID now = list[n];
                for (int i = n + 1; i < list.Count; )
                {
                    UseValsID c = list[i];
                    if (now.ID == c.ID && now.Addr == c.Addr && now.TargetType == c.TargetType)
                    {
                        list.RemoveAt(i);
                    }
                    else
                    {
                        i ++;
                    }
                }
            }
        }

    }
}
