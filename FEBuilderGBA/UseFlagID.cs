using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEBuilderGBA
{
    class UseFlagID
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

    }
}
