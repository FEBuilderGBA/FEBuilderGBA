using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEBuilderGBA
{
    public class ExportFunction
    {
        Dictionary<string, uint> Dic = new Dictionary<string, uint>();

        public void Clear()
        {
            Dic.Clear();
        }
        public void Add(string name,uint addr)
        {
            this.Dic[name] = addr;
        }
        public Dictionary<string, uint> GetDic() // const
        {
            return this.Dic;
        }
        public void ExportEA(StringBuilder sb)
        {
            foreach (var pair in this.Dic)
            {
                string name = pair.Key;
                uint addr = U.toOffset(pair.Value);
                One(sb, name, addr);
            }
        }
        public static void One(StringBuilder sb, string name, uint addr)
        {
            if (!U.isSafetyOffset(addr))
            {
                return;
            }
            EAUtil.AddORG(sb, addr, name);
        }

    }
}
