using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace FEBuilderGBA
{
    public class EAUtilLynDumpMode
    {
        public struct Data
        {
            public string Name;
            public uint StartLow;
            public uint Start;
        }
        List<Data> List;
        List<byte> Bin;
        uint LastORG;

        public EAUtilLynDumpMode()
        {
            this.List = new List<Data>();
            this.Bin = new List<byte>();
            this.LastORG = 0;
        }
        public bool ParseLine(string line)
        {
            if (CheckEnd(line))
            {//Lynモードの終了
                return false;
            }

            if (line.IndexOf("ORG CURRENTOFFSET+") == 0)
            {
                ParseORG(line);
            }
            else if (line.IndexOf("SHORT ") == 0)
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

        void ParseORG(string line)
        {
            string startString = U.cut(line, "+", ";");
            if (startString == "")
            {
                return;
            }
            uint start = U.atoi0x(startString);
            start += this.LastORG;

            if (this.LastORG == 0 && start > 1)
            {
                Data d = new Data();
                d.Name = "NONAME";
                d.StartLow = 0;
                d.Start = 0;
                this.List.Add(d);
            }

            {
                string labelString = U.cut(line, ";", ":");

                Data d = new Data();
                d.Name = labelString;
                d.StartLow = start;
                if (U.IsValueOdd(start))
                {
                    d.Start = start - 1;
                }
                else
                {
                    d.Start = start;
                }
                this.List.Add(d);
            }
            this.LastORG = start;
        }
        public byte[] GetDataAll()
        {
            return this.Bin.ToArray();
        }

        public int GetCount()
        {
            return this.List.Count;
        }
        public byte[] GetData(int index)
        {
            int next = index + 1;
            uint end;
            if (this.List.Count <= next)
            {
                end = (uint)this.Bin.Count;
            }
            else
            {
                end = this.List[next].Start;
            }
            uint start = this.List[index].Start;
            return this.Bin.GetRange((int)start, (int)(end - start)).ToArray();
        }
        public string GetName(int index)
        {
            return this.List[index].Name;
        }
#if DEBUG
        public static void TEST_LYNDUMP_TEST1()
        {
            string[] lines = File.ReadAllLines(Program.GetTestData("DanceAi.lyn.event"));
            EAUtilLynDumpMode lyn = new EAUtilLynDumpMode();
            foreach(string l in lines)
            {
                bool r = lyn.ParseLine(l);
                if (!r)
                {
                    break;
                }
            }

            Debug.Assert(lyn.List.Count == 2);
            Debug.Assert(lyn.List[0].Name == "DanceAiTryDecide");
            Debug.Assert(lyn.List[0].Start == 0);
            Debug.Assert(lyn.List[0].StartLow == 1);
            Debug.Assert(lyn.List[1].Name == "DanceAiDoAction");
            Debug.Assert(lyn.List[1].Start == 0x138);
            Debug.Assert(lyn.List[1].StartLow == 0x139);
        }
        public static void TEST_LYNDUMP_TEST2()
        {
            string[] lines = File.ReadAllLines(Program.GetTestData("SaveUnitExpanded.lyn.event"));
            EAUtilLynDumpMode lyn = new EAUtilLynDumpMode();
            foreach(string l in lines)
            {
                bool r = lyn.ParseLine(l);
                if (!r)
                {
                    break;
                }
            }

            Debug.Assert(lyn.List.Count == 9);
            Debug.Assert(lyn.List[0].Name == "NONAME");
            Debug.Assert(lyn.List[0].Start == 0);
            Debug.Assert(lyn.List[0].StartLow == 0);
            Debug.Assert(lyn.List[1].Name == "ESU_SaveGameUnits");
            Debug.Assert(lyn.List[1].Start == 0x274);
            Debug.Assert(lyn.List[1].StartLow == 0x275);
            Debug.Assert(lyn.List[2].Name == "ESU_LoadGameUnits");
            Debug.Assert(lyn.List[2].Start == 0x2BC);
            Debug.Assert(lyn.List[2].StartLow == 0x2BD);
            Debug.Assert(lyn.List[3].Name == "ESU_SavePlayerSuspendUnits");
            Debug.Assert(lyn.List[3].Start == 0x304);
            Debug.Assert(lyn.List[3].StartLow == 0x305);
            Debug.Assert(lyn.List[4].Name == "ESU_LoadPlayerSuspendUnits");
            Debug.Assert(lyn.List[4].Start == 0x34C);
            Debug.Assert(lyn.List[4].StartLow == 0x34D);
            Debug.Assert(lyn.List[5].Name == "ESU_SaveGreenSuspendUnits");
            Debug.Assert(lyn.List[5].Start == 0x394);
            Debug.Assert(lyn.List[5].StartLow == 0x395);
            Debug.Assert(lyn.List[6].Name == "ESU_LoadGreenSuspendUnits");
            Debug.Assert(lyn.List[6].Start == 0x3DC);
            Debug.Assert(lyn.List[6].StartLow == 0x3DD);
            Debug.Assert(lyn.List[7].Name == "ESU_SaveRedSuspendUnits");
            Debug.Assert(lyn.List[7].Start == 0x424);
            Debug.Assert(lyn.List[7].StartLow == 0x425);
            Debug.Assert(lyn.List[8].Name == "ESU_LoadRedSuspendUnits");
            Debug.Assert(lyn.List[8].Start == 0x46C);
            Debug.Assert(lyn.List[8].StartLow == 0x46D);

            Debug.Assert(lyn.Bin.Count == 0x4B4);
        }
#endif
    }
}
