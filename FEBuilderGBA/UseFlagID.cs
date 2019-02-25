using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEBuilderGBA
{
    public class UseFlagID
    {
        public FELint.Type DataType { get; private set; }
        public uint ID { get; private set; }
        public string Info { get; private set; }
        public uint Addr { get; private set; }
        public uint Tag { get; private set; }
        public uint MapID { get; private set; }

        public UseFlagID(FELint.Type dataType, uint addr, string info, uint id, uint mapid, uint tag = U.NOT_FOUND)
        {
            this.DataType = dataType;
            this.Addr = addr;
            this.Info = info;
            this.ID = id;
            this.MapID = mapid;
            this.Tag = tag;
        }
        public static void AppendUseFlagID(List<UseFlagID> list, FELint.Type dataType, uint addr, string info, uint id, uint mapid, uint tag = U.NOT_FOUND)
        {
            if (id == 0)
            {
                return;
            }
            list.Add(new UseFlagID(dataType, addr, info, id,mapid, tag));
        }

        public static void AppendFlagID(List<UseFlagID> list, FELint.Type dataType, InputFormRef ifr, uint flagIDPlus, uint chapterIDPlus)
        {
            List<U.AddrResult> arlist = ifr.MakeList();
            for (int i = 0; i < ifr.DataCount; i++)
            {
                U.AddrResult ar = arlist[i];
                uint id = Program.ROM.u16(ar.addr + flagIDPlus);
                if (id == 0)
                {
                    continue;
                }
                uint mapid = Program.ROM.u8(ar.addr + chapterIDPlus);
                if (mapid >= 0xF0)
                {
                    mapid = U.NOT_FOUND;
                }

                list.Add(new UseFlagID(dataType, ar.addr, ar.name, id,mapid, (uint)i));
            }
        }

        public static void AppendFlagIDFixedMapID(List<UseFlagID> list, FELint.Type dataType, InputFormRef ifr, uint flagIDPlus, uint chapterIDPlus)
        {
            List<U.AddrResult> arlist = ifr.MakeList();
            for (int i = 0; i < ifr.DataCount; i++)
            {
                U.AddrResult ar = arlist[i];
                uint id = Program.ROM.u16(ar.addr + flagIDPlus);
                if (id == 0)
                {
                    continue;
                }
                uint mapid = Program.ROM.u8(ar.addr + chapterIDPlus);
                if (mapid >= 0xF0)
                {
                    mapid = U.NOT_FOUND;
                }

                list.Add(new UseFlagID(dataType, ar.addr, ar.name, id, mapid, (uint)i));
            }
        }

    }
}
