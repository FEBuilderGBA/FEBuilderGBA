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
            if (id == 0)
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
                    if (id == 0)
                    {
                        continue;
                    }
                    list.Add(new UseTextID(dataType, ar.addr, ar.name, id, (uint)i));
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
                    if (id == 0)
                    {
                        continue;
                    }
                    list.Add(new UseTextID(dataType, ar.addr, ar.name, id, (uint)i));
                }
            }
        }
        public static void AppendByTestARList(List<UseTextID> list, FELint.Type dataType, List<U.AddrResult> text_arlist,uint event_addr, string basename)
        {
            int length = text_arlist.Count;
            for (int i = 0; i < length; i++)
            {
                U.AddrResult ar = text_arlist[i];
                uint id = ar.addr; //text_arlistの場合、addrにテキストIDが入っている.
                if (id == 0)
                {
                    continue;
                }
                string name = basename + " " + ar.name;

                list.Add(new UseTextID(dataType, event_addr, name, id, ar.tag ));
            }
        }
    }
}
