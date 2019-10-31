using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEBuilderGBA
{
    class EAUtilLynDumpMode
    {
        List<byte> Bin;

        public EAUtilLynDumpMode()
        {
            this.Bin = new List<byte>();
        }
        public bool ParseLine(string line)
        {
            if (CheckEnd(line))
            {//Lynモードの終了
                return false;
            }

            if (line.IndexOf("SHORT ") == 0)
            {
                ParseSHORT(line);
            }
            else if (line.IndexOf("BYTE ") == 0)
            {
                ParseBYTE(line);
            }
            else if (line.IndexOf("POIN ") == 0)
            {
                this.Bin.Add(0);
                this.Bin.Add(0);
                this.Bin.Add(0);
                this.Bin.Add(0);
            }
            else if (line.IndexOf("WORD ") == 0)
            {
                ParseWORD(line);
            }

            return true;
        }
        void ParseWORD(string line)
        {
            string[] sp = line.Split(' ');
            for (int i = 1; i < sp.Length; i++)
            {
                U.append_u32(this.Bin, U.atoi0x(sp[i]));
            }
        }
        void ParseSHORT(string line)
        {
            string[] sp = line.Split(' ');
            for (int i = 1; i < sp.Length; i++)
            {
                U.append_u16(this.Bin,U.atoi0x(sp[i]));
            }
        }
        void ParseBYTE(string line)
        {
            string[] sp = line.Split(' ');
            for (int i = 1; i < sp.Length; i++)
            {
                this.Bin.Add((byte)U.atoi0x(sp[i]));
            }
        }

        bool CheckEnd(string line)
        {
            return (line == "");
        }

        public byte[] GetData()
        {
            return this.Bin.ToArray();
        }
    }
}
