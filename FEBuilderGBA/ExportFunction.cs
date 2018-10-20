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

                if (!U.isSafetyOffset(addr))
                {
                    continue;
                }
                sb.AppendLine("PUSH");
                sb.Append("ORG ");
                sb.AppendLine(U.To0xHexString(addr));
                sb.Append(name);
                sb.AppendLine(":");
                sb.AppendLine("POP");
            }
        }

    }
}
