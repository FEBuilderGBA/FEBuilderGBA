using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEBuilderGBA
{
    public class UseTextID
    {
        public FELint.Type DataType { get; private set; }
        public uint ID { get; private set; }
        public string Info { get; private set; }
        public uint Addr { get; private set; }
        public uint Tag { get; private set; }

        public UseTextID(FELint.Type dataType,uint addr,string info,uint id,uint tag = U.NOT_FOUND)
        {
            this.DataType = dataType;
            this.Addr = addr;
            this.Info = info;
            this.ID = id;
            this.Tag = tag;
        }
        public static void AppendTextID(List<UseTextID> list,FELint.Type dataType, uint addr, string info, uint id, uint tag = U.NOT_FOUND)
        {
            if (id == 0 || id >= 0x7FFF)
            {
                return;
            }
            list.Add(new UseTextID(dataType, addr, info, id, tag));
        }
        public static void AppendTextID(List<UseTextID> list, FELint.Type dataType, InputFormRef ifr,uint[] textIDIndexes)
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
                    list.Add(new UseTextID(dataType, ar.addr, ar.name, id, (uint)i));
                }
            }
        }
        public static void AppendTextID(List<UseTextID> list, FELint.Type dataType, InputFormRef ifr, uint[] textIDIndexes, uint eventIndex)
        {
            List<uint> tracelist = new List<uint>();
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
                    list.Add(new UseTextID(dataType, ar.addr, ar.name, id, (uint)i));
                }
                uint eventAddr = Program.ROM.p32(ar.addr + eventIndex);
                if (U.isSafetyOffset(eventAddr))
                {
                    EventCondForm.MakeTextIDArrayByEventAddress(list, eventAddr, ar.name, tracelist);
                }
            }
        }

        public static void AppendTextIDPP(List<UseTextID> list, FELint.Type dataType, InputFormRef ifr, uint[] textIDIndexes)
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
                    list.Add(new UseTextID(dataType, ar.addr, ar.name, id, (uint)i));
                }
            }
        }
        public static void AppendASMDATATextID(List<UseTextID> list, AsmMapFile.AsmMapSt p,uint startaddr, uint sizeof_)
        {
            uint end = startaddr + p.Length;
            for (uint addr = startaddr; addr < end; addr += sizeof_ )
            {
                uint id = Program.ROM.u16(addr);
                list.Add(new UseTextID(FELint.Type.ASMDATA, addr, p.Name, id , addr ));
            }
        }

        public static Dictionary<uint, bool> ConvertMaps(List<UseTextID> textIDList)
        {
            Dictionary<uint, bool> textmap = new Dictionary<uint, bool>();
            textmap[0] = true;
            foreach(var p in textIDList)
            {
                textmap[p.ID] = true;
            }
            return textmap;
        }
    }
}
