using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Runtime.CompilerServices;

namespace FEBuilderGBA
{
    class EtcCache
    {
        public EtcCache(string type)
        {
            this.Type = type;
            this.Cache = U.LoadTSVResource1(U.ConfigEtcFilename(this.Type), false);
        }
        protected string Type;
        protected Dictionary<uint, string> Cache;

        [MethodImpl(256)]
        public bool CheckFast(uint num)
        {
            return Cache.ContainsKey(num);
        }
        [MethodImpl(256)]
        public string At(uint num, string def = "")
        {
            string str;
            if (Cache.TryGetValue(num, out str))
            {
                return str;
            }
            return def;
        }
        [MethodImpl(256)]
        public string S_At(uint num)
        {
            string str;
            if (Cache.TryGetValue(num, out str))
            {
                return " " + str;
            }
            return "";
        }
        [MethodImpl(256)]
        public bool TryGetValue(uint num, out string out_data)
        {
            return Cache.TryGetValue(num,out out_data);
        }

        public void Update(uint addr,string comment)
        {
            if (comment == "")
            {
                if (this.Cache.ContainsKey(addr))
                {
                    this.Cache.Remove(addr);
                }
            }
            else
            {
                this.Cache[addr] = comment;
            }
        }
        public void Remove(uint addr)
        {
            if (this.Cache.ContainsKey(addr))
            {
                this.Cache.Remove(addr);
            }
        }
        public void RemoveOverRange(uint addr)
        {
            uint[] keys = U.DicKeys(this.Cache);
            for (int i = 0; i < keys.Length; i++)
            {
                uint key = keys[i];
                if (key >= addr)
                {
                    this.Cache.Remove(key);
                }
            }
        }
        public void RemoveRange(uint addr, uint limit)
        {
            uint[] keys = U.DicKeys(this.Cache);
            for (int i = 0; i < keys.Length; i++)
            {
                uint key = keys[i];
                if (key >= addr && key < limit)
                {
                    this.Cache.Remove(key);
                }
            }
        }

        public void UpdateCode(uint startAddr,uint oldSize, List<EventScript.OneCode> codes)
        {
            uint addr = startAddr;
            for (int i = 0; i < codes.Count; i++)
            {
                EventScript.OneCode code = codes[i];
                string old;
                if (Cache.TryGetValue(addr, out old))
                {
                    if (old != code.Comment)
                    {
                        Cache[addr] = code.Comment;
                    }
                }
                else
                {
                    if (code.Comment != "")
                    {
                        Cache[addr] = code.Comment;
                    }
                }

                uint newAddr = addr + (uint)code.ByteData.Length;
                if (addr < newAddr)
                {
                    addr++;
                    for (; addr < newAddr; addr++)
                    {
                        if (Cache.TryGetValue(addr, out old))
                        {
                            if (old == "")
                            {
                                continue;
                            }
                            Cache.Remove(addr);
                        }
                    }
                }
            }

            uint endAddr = startAddr + oldSize;
            for( ; addr < endAddr ; addr ++)
            {
                if (Cache.ContainsKey(addr))
                {
                    Cache.Remove(addr);
                }
            }
        }

        public void RepointEtcData(uint oldAddr, uint oldSize, uint newAddr)
        {
            string fullfilename = U.ConfigEtcFilename(this.Type);
            Dictionary<uint, string> newDic = new Dictionary<uint, string>();
            foreach (var pair in this.Cache)
            {
                if (pair.Key >= oldAddr && pair.Key < oldAddr + oldSize)
                {
                    uint addr = (pair.Key - oldAddr) + newAddr;
                    newDic[(pair.Key - oldAddr) + newAddr] = pair.Value;
                }
                else
                {
                    newDic[pair.Key] = pair.Value;
                }
            }

            this.Cache = newDic;
        }
        public void ShrinkEtcData(uint blank_start_addr, uint blank_size)
        {
            string fullfilename = U.ConfigEtcFilename(this.Type);
            Dictionary<uint, string> newDic = new Dictionary<uint, string>();
            foreach (var pair in this.Cache)
            {
                if (pair.Key >= blank_start_addr && pair.Key < blank_start_addr + blank_size)
                {//erase data
                }
                else
                {
                    newDic[pair.Key] = pair.Value;
                }
            }

            this.Cache = newDic;
        }
        public void SaveCurrent()
        {
            Save(Path.GetFileNameWithoutExtension(Program.ROM.Filename));
        }

        public void Save(string romBaseFilename)
        {
            U.SaveConfigEtcTSV1(this.Type , this.Cache , romBaseFilename);
        }

        public void MakeAddressList(List<Address> list)
        {
            foreach (var pair in this.Cache)
            {
                Address.AddCommentData(list, pair.Key , pair.Value);
            }
        }

    }
}
