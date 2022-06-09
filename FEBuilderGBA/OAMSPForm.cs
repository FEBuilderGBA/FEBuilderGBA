using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class OAMSPForm : Form
    {
        public OAMSPForm()
        {
            InitializeComponent();

        }
        List<Address> ListOAM;
        private void OAMSPForm_Shown(object sender, EventArgs e)
        {
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                pleaseWait.DoEvents(R._("準備しています"));

                this.AddressList.BeginUpdate();
                this.AddressList.Items.Clear();


                List<DisassemblerTrumb.LDRPointer> ldrmap = Program.AsmMapFileAsmCache.GetLDRMapCache();

                this.ListOAM = new List<Address>();
                List<Address> listOAM12 = new List<Address>();
                MakeAllDataLength(this.ListOAM, listOAM12, ldrmap);

                List<U.AddrResult> list = new List<U.AddrResult>();
                for (int i = 0; i < this.ListOAM.Count; i++)
                {
                    list.Add(new U.AddrResult((uint)i, this.ListOAM[i].Info));
                    this.AddressList.Items.Add(this.ListOAM[i].Info);
                }
                this.AddressList.Tag = list;
                this.AddressList.EndUpdate();
            }
        }
        

        private void OAMSPForm_Load(object sender, EventArgs e)
        {

        }

        public static void MakeAllDataLength(List<Address> list, List<Address> listoam12, List<DisassemblerTrumb.LDRPointer> ldrmap)
        {
            Dictionary<uint, string> oamName = U.LoadDicResource(U.ConfigDataFilename("oam_name_"));

            Dictionary<uint, bool> alreadyMatch = new Dictionary<uint, bool>();
            Dictionary<uint, bool> alreadyMatch12 = new Dictionary<uint, bool>();
            for (int i = 0; i < ldrmap.Count; i++)
            {
                uint addr = ldrmap[i].ldr_data;
                if (!U.isSafetyPointer(addr))
                {
                    continue;
                }
                addr = U.toOffset(addr);
                if (addr < Program.ROM.RomInfo.compress_image_borderline_address)
                {
                    continue;
                }

                if (alreadyMatch.ContainsKey(addr))
                {//既に知っている.
                    continue;
                }

                string name = U.at(oamName, ldrmap[i].ldr_data_address);
                if (name == "")
                {
                    name = U.at(oamName, ldrmap[i].ldr_address);
                }
                if (name == "")
                {
                    name = U.ToHexString8(ldrmap[i].ldr_data);
                }
                name = "OAMSP " + name;
                List<Address> listoam12_local = new List<Address>();
                uint length = CalcLengthAndCheck(addr, name, ref listoam12_local, ref alreadyMatch12);
                if (U.NOT_FOUND == length)
                {
                    alreadyMatch.Add(addr, false); //ダメだったということを記録しておこう
                    continue;
                }
                if (length < 4 * 3)
                {
                    alreadyMatch.Add(addr, false); //ダメだったということを記録しておこう
                    continue;
                }

                FEBuilderGBA.Address.AddAddress(list, addr, length, ldrmap[i].ldr_data_address, name, FEBuilderGBA.Address.DataTypeEnum.OAMSP);
                listoam12.AddRange(listoam12_local);
                alreadyMatch.Add(addr, true);

                if (InputFormRef.DoEvents(null, "OAMSP " + U.ToHexString(addr))) return;
            }
            foreach (var pair in oamName)
            {
                uint addr = U.toOffset(pair.Key);
                if (alreadyMatch.ContainsKey(addr))
                {//既に知っている.
                    continue;
                }

                string name = "OAMSP_ " + pair.Value;
                List<Address> listoam12_local = new List<Address>();
                uint length = CalcLengthAndCheck(addr, name, ref listoam12_local, ref alreadyMatch12);
                if (U.NOT_FOUND == length)
                {
                    alreadyMatch.Add(addr, false); //ダメだったということを記録しておこう
                    continue;
                }
                if (length < 4)
                {
                    alreadyMatch.Add(addr, false); //ダメだったということを記録しておこう
                    continue;
                }

                FEBuilderGBA.Address.AddAddress(list, addr, length, U.NOT_FOUND, name , FEBuilderGBA.Address.DataTypeEnum.OAMSP);
                listoam12.AddRange(listoam12_local);
                alreadyMatch.Add(addr, true);

                if (InputFormRef.DoEvents(null, "OAMSP " + U.ToHexString(addr))) return;
            }
        }
        static uint CalcLengthAndCheckOAM12(uint addr)
        {
            uint start = addr;
            uint length = (uint)(Program.ROM.Data.Length - 12);
            while (addr < length)
            {
                byte[] oam = Program.ROM.getBinaryData(addr, 12);

                addr += 12;
                if (oam[0] == 0 && oam[0 + 1] == 0xFF && oam[0 + 2] == 0xFF && oam[0 + 3] == 0xFF)
                {//FEditorシリアライズを読み込んだときは別終端がある?
                    break;
                }
                if (oam[0] == 0 && oam[1] == 0 && oam[2] == 0 && oam[3] == 0 
                    && oam[4] == 0 && oam[5] == 0 && oam[6] == 0 && oam[7] == 0)
                {
                    break;
                }

//                if (oam[0] == 0 && oam[1] == 0 && oam[2] == 0 && oam[3] == 0)
//                {//全部nullだともしかして終わり?
//                    Log.Debug(U.ToHexString8(addr) + " " + U.DumpByte(oam));
//                    break;
//                }

                if (oam[0 + 2] == 0xFF && oam[0 + 3] == 0xFF)
                {//2バイト目と3バイト目が FFだったら 別ルーチン
                    continue;
                }
                if (oam[0] == 0xFF && oam[1] == 0xFF )
                {//FF FF xx xx なんか特殊命令?
                    continue;
                }

                if (oam[0] == 1)
                {//終端
                    break;
                }
                if (oam[0] == 0)
                {//データ
                    continue;
                }

                //OAM規約違反
                return U.NOT_FOUND;
            }

            uint ret_length = addr - start;
            return ret_length;
        }

        static uint CalcLengthAndCheck(uint addr, string name, ref List<Address> list, ref Dictionary<uint, bool> alreadyMatch)
        {
            uint start = addr;
            uint length = (uint)(Program.ROM.Data.Length - 4);
            while (addr < length)
            {
                uint p = Program.ROM.u32(addr);
                if ((p & (uint)0x88FFFF00) == 0x80000000)
                {//OAM term 0x8X0000XX
                    //         0x800000XX or 0x810000XX or 0x820000XX or 0x840000XX
                    addr += 4;
                    break;
                }
                if ((p & (uint)0x70000000) > 0)
                {//OAM term
                    //以下のように、先頭7が入っているものがある.
                    p = (p & 0x0FFFFFFF); //78 67 EA 3B
                }
                if (! U.isSafetyPointer(p))
                {
                    //理解不能の命令があったのでOAMではない
                    return U.NOT_FOUND;
                }
                
                uint oam12Addr = U.toOffset(p);
                //多分奇数の場合 整数に直す??? まったくわからないがそう考えると都合がいい.
                oam12Addr = DisassemblerTrumb.ProgramAddrToPlain(oam12Addr);
                if (!alreadyMatch.ContainsKey(oam12Addr))
                {//まだ知らない
                    uint oam12length = CalcLengthAndCheckOAM12(oam12Addr);
                    if (oam12length == U.NOT_FOUND)
                    {
                        return U.NOT_FOUND;
                    }
                    FEBuilderGBA.Address.AddAddress(list,oam12Addr, oam12length, addr, name + "_OAM",FEBuilderGBA.Address.DataTypeEnum.OAMSP12);
                    alreadyMatch.Add(oam12Addr, true);
                }

                addr += 4;
            }
            uint ret_length = addr - start;
            return ret_length;
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint index = InputFormRef.SelectToAddr(this.AddressList);
            if (index == U.NOT_FOUND || index >= this.ListOAM.Count)
            {
                return ;
            }

            uint addr = this.ListOAM[(int)index].Addr;
            if (!U.isSafetyOffset(addr))
            {
                return;
            }
            this.Address.Value = addr;

            List<Address> listoam12 = new List<Address>();
            Dictionary<uint, bool> alreadyMatch = new Dictionary<uint,bool>();
            uint length = CalcLengthAndCheck(addr, "", ref listoam12, ref alreadyMatch);

            StringBuilder sb = new StringBuilder();
            for (uint i = 0; i < length; i += 4)
            {
                uint p = Program.ROM.u32(addr + i);
                sb.Append(U.ToHexString8(p));
                sb.Append(" ");
            }
            sb.AppendLine("");
            sb.AppendLine("");
            for (int n = 0; n < listoam12.Count; n++)
            {
                addr = listoam12[n].Addr;
                length = listoam12[n].Length;
                sb.AppendLine(U.ToHexString8(addr) + ":");
                if (length == U.NOT_FOUND)
                {
                    sb.AppendLine("-unknown-");
                    for (uint i = 0; i < 12*100; i += 12 , addr += 12)
                    {
                        sb.Append(U.HexDump(Program.ROM.getBinaryData(addr, 12)));
                    }
                }
                else
                {
                    for (uint i = 0; i < length; i += 12, addr += 12)
                    {
                        sb.Append(U.HexDump(Program.ROM.getBinaryData(addr, 12)));
                    }
                }
                sb.AppendLine("");
                sb.AppendLine("");
            }

            X_DATA.Text = sb.ToString();
        }

    }
}
