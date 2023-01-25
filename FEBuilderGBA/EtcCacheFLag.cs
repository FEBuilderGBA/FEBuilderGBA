using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEBuilderGBA
{
    class EtcCacheFLag
    {
        Dictionary<uint, string> EtcFlag;
        Dictionary<uint, string> Flag;

        public EtcCacheFLag()
        {
            this.EtcFlag = U.LoadTSVResource1(U.ConfigEtcFilename("flag_"), false);
            this.Flag = U.LoadDicResource(U.ConfigDataFilename("flag_"));
            foreach (var pair in EtcFlag)
            {
                string name = pair.Value;
                if (name.Length > 0)
                {
                    this.Flag[pair.Key] = name;
                }
            }

            U.OrderBy(this.Flag, (x) => { return (int)x.Key; });
        }

        public bool CheckFast(uint num)
        {
            return Flag.ContainsKey(num);
        }
        public bool TryGetValue(uint num, out string out_data)
        {
            return Flag.TryGetValue(num, out out_data);
        }

        public void Update(uint addr, string comment,string baseName)
        {
            if (comment == "")
            {
                if (this.EtcFlag.ContainsKey(addr))
                {
                    this.EtcFlag.Remove(addr);
                }
                this.Flag[addr] = baseName;
            }
            else
            {
                this.EtcFlag[addr] = comment;
                this.Flag[addr] = comment;
            }
        }
        public void Save(string romBaseFilename)
        {
            if (this.EtcFlag.Count >= 1)
            {
                U.SaveConfigEtcTSV1("flag_", this.EtcFlag, romBaseFilename);
            }
        }

        public List<U.AddrResult> MakeList()
        {
            List<U.AddrResult> list = new List<U.AddrResult>();
            foreach (var pair in this.Flag)
            {
                U.AddrResult ar = new U.AddrResult(
                     pair.Key
                    , pair.Value
                    );
                list.Add(ar);
            }
            return list;
        }

        //マージ専用
        public void MargeFlags(Dictionary<uint, string> flags)
        {
            foreach (var pair in flags)
            {
                if (this.EtcFlag.ContainsKey(pair.Key))
                {
                    continue;
                }
                this.Flag[pair.Key] = pair.Value;
            }
            U.OrderBy(this.Flag, (x) => { return (int)x.Key; });
        }
    }
}
