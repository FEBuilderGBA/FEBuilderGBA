using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class StatusRMenuForm : Form
    {
        public StatusRMenuForm()
        {
            InitializeComponent();

            this.InputFormRef = Init(this);
            this.InputFormRef.IsMemoryNotContinuous = true; //メモリは連続していないので、警告不能.
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            this.FilterComboBox.BeginUpdate();
            this.FilterComboBox.Items.Add(R._("0=ステータスパラメータ"));
            this.FilterComboBox.Items.Add(R._("1=所持アイテム"));
            this.FilterComboBox.Items.Add(R._("2=武器レベル"));
            this.FilterComboBox.Items.Add(R._("3=戦闘予測1"));
            this.FilterComboBox.Items.Add(R._("4=戦闘予測2"));
            if (Program.ROM.RomInfo.version == 8)
            {
                this.FilterComboBox.Items.Add(R._("5=状況画面"));
            }
            this.FilterComboBox.EndUpdate();
            FilterComboBox.SelectedIndex = 0;
        }

        static void ListFounder(uint addr, ref List<uint> rmenulist, ref List<uint> already)
        {
            if (!U.isSafetyOffset(addr) || !U.isSafetyOffset(addr + 28))
            {
                return;
            }

            uint u = Program.ROM.p32(addr + 0);
            uint d = Program.ROM.p32(addr + 4);
            uint l = Program.ROM.p32(addr + 8);
            uint r = Program.ROM.p32(addr + 12);

            if (U.isSafetyOffset(u) && already.IndexOf(u) < 0)
            {
                rmenulist.Add(u);
                already.Add(u);
            }

            if (U.isSafetyOffset(d) && already.IndexOf(d) < 0)
            {
                rmenulist.Add(d);
                already.Add(d);
            }

            if (U.isSafetyOffset(l) && already.IndexOf(l) < 0)
            {
                rmenulist.Add(l);
                already.Add(l);
            }

            if (U.isSafetyOffset(r) && already.IndexOf(r) < 0)
            {
                rmenulist.Add(r);
                already.Add(r);
            }
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            List<uint> rmenulist = new List<uint>() {};
            List<uint> already = new List<uint>() {};

            return new InputFormRef(self
                , ""
                , new List<String>()
                , 0
                , 28
                , (uint addr) =>
                {
                    if (rmenulist.Count > 0)
                    {
                        uint nextaddr = rmenulist[0];
                        rmenulist.RemoveAt(0);
                        return nextaddr;
                    }

                    already.Clear();
                    return U.NOT_FOUND;
                }
                , (int i, uint addr) =>
                {
                    if (i == 0)
                    {
                        already.Clear();
                    }
                    if (!U.isSafetyOffset(addr) || !U.isSafetyOffset(addr + 28))
                    {
                        return false;
                    }

                    ListFounder(addr, ref rmenulist, ref already);

                    return true;
                }
                , (int i, uint addr) =>
                {
                    if (i == 0)
                    {
                        already.Clear();
                    }
                    ListFounder(addr, ref rmenulist, ref already);

                    string str = U.ToHexString(i) + " " + GetMenuName(addr);
                    return new U.AddrResult(addr, str);
                }
                );
        }

        public static string GetMenuName(uint addr)
        {
            addr = U.toOffset(addr);
            if (!U.isSafetyOffset(addr) || !U.isSafetyOffset(addr + 28))
            {
                return "";
            }

            uint tid = Program.ROM.u16(addr + 18);
            if (tid <= 0x10)
            {
                return "";
            }

            string name = TextForm.Direct(tid);

            //最初の方だけ
            name = U.cut(name, "\r\n");
            return name;
        }
        public static string GetTIDName(uint tid)
        {
            if (tid <= 0x10)
            {
                return "";
            }
            string name = TextForm.Direct(tid);
            return name;
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {

        }

        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.InputFormRef == null)
            {
                return;
            }

            int selected = this.FilterComboBox.SelectedIndex;
            if (selected == 0)
            {
                this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.status_rmenu_unit_pointer) );
            }
            else if (selected == 1)
            {
                this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.status_rmenu_game_pointer));
            }
            else if (selected == 2)
            {
                this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.status_rmenu3_pointer));
            }
            else if (selected == 3)
            {
                this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.status_rmenu4_pointer));
            }
            else if (selected == 4)
            {
                this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.status_rmenu5_pointer));
            }
            else if (selected == 5)
            {
                this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.status_rmenu6_pointer));
            }
        }

        static void MakeAllDataLengthSub(List<Address> list, uint p,uint pointer,Dictionary<uint, bool> foundDic, uint[] pointerIndexes)
        {
            if (!U.isSafetyOffset(p + 18))
            {
                return;
            }

            string name = "RMENU " + U.To0xHexString( Program.ROM.u16(p + 18) );
            if (!foundDic.ContainsKey(p))
            {
                list.Add(new Address(p, 28, U.NOT_FOUND, name,FEBuilderGBA.Address.DataTypeEnum.MIX ,28 , pointerIndexes));
            }
            foundDic[p] = true;

            uint pp;
            pp = Program.ROM.p32(p + 0);
            if (U.isSafetyOffset(pp) && !foundDic.ContainsKey(pp))
            {
                MakeAllDataLengthSub(list, pp, p + 0, foundDic , pointerIndexes);
            }
            pp = Program.ROM.p32(p + 4);
            if (U.isSafetyOffset(pp) && !foundDic.ContainsKey(pp))
            {
                MakeAllDataLengthSub(list, pp, p + 4, foundDic, pointerIndexes);
            }
            pp = Program.ROM.p32(p + 8);
            if (U.isSafetyOffset(pp) && !foundDic.ContainsKey(pp))
            {
                MakeAllDataLengthSub(list, pp, p + 8, foundDic, pointerIndexes);
            }
            pp = Program.ROM.p32(p + 12);
            if (U.isSafetyOffset(pp) && !foundDic.ContainsKey(pp))
            {
                MakeAllDataLengthSub(list, pp, p + 12, foundDic, pointerIndexes);
            }

            FEBuilderGBA.Address.AddFunction(list
                , p + 20
                , name + "+P20"
                );
            FEBuilderGBA.Address.AddFunction(list
                , p + 24
                , name + "+P24"
                );
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            Dictionary<uint, bool> foundDic = new Dictionary<uint, bool>();
            uint[] pointerIndexes = new uint[] { 0, 4, 8, 12, 20, 24 };
            uint[] addlist = new uint[] { Program.ROM.RomInfo.status_rmenu_unit_pointer
                , Program.ROM.RomInfo.status_rmenu_game_pointer
                , Program.ROM.RomInfo.status_rmenu3_pointer
                , Program.ROM.RomInfo.status_rmenu4_pointer
                , Program.ROM.RomInfo.status_rmenu5_pointer
                , Program.ROM.RomInfo.status_rmenu6_pointer
            };

            for (int n = 0; n < addlist.Length; n++)
            {
                uint pointer = addlist[n];
                if (pointer == 0)
                {
                    continue;
                }
                uint p = Program.ROM.p32(pointer + 0);
                MakeAllDataLengthSub(list, p, pointer, foundDic, pointerIndexes);
            }
        }
        static void MakeVarsIDArraySub(List<UseValsID> list, uint p,uint pointer,Dictionary<uint, bool> foundDic)
        {
            if (! U.isSafetyOffset(p + 12))
            {
                return;
            }

            string name = "RMENU " + U.To0xHexString(Program.ROM.u16(p + 18));
            if (!foundDic.ContainsKey(p))
            {
                uint id = Program.ROM.u16(p + 18);
                UseValsID.AppendTextID(list, FELint.Type.RMENU, p + 18, "RMENU", id, p);
            }
            foundDic[p] = true;

            uint pp;
            pp = Program.ROM.p32(p + 0);
            if (U.isSafetyOffset(pp) && !foundDic.ContainsKey(pp))
            {
                MakeVarsIDArraySub(list, pp, p + 0, foundDic);
            }
            pp = Program.ROM.p32(p + 4);
            if (U.isSafetyOffset(pp) && !foundDic.ContainsKey(pp))
            {
                MakeVarsIDArraySub(list, pp, p + 4, foundDic);
            }
            pp = Program.ROM.p32(p + 8);
            if (U.isSafetyOffset(pp) && !foundDic.ContainsKey(pp))
            {
                MakeVarsIDArraySub(list, pp, p + 8, foundDic);
            }
            pp = Program.ROM.p32(p + 12);
            if (U.isSafetyOffset(pp) && !foundDic.ContainsKey(pp))
            {
                MakeVarsIDArraySub(list, pp, p + 12, foundDic);
            }
        }

        //全データの取得
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            Dictionary<uint, bool> foundDic = new Dictionary<uint, bool>();
            uint[] addlist = new uint[] { Program.ROM.RomInfo.status_rmenu_unit_pointer
                , Program.ROM.RomInfo.status_rmenu_game_pointer
                , Program.ROM.RomInfo.status_rmenu3_pointer
                , Program.ROM.RomInfo.status_rmenu4_pointer
                , Program.ROM.RomInfo.status_rmenu5_pointer
                , Program.ROM.RomInfo.status_rmenu6_pointer
            };

            for (int n = 0; n < addlist.Length; n++)
            {
                uint pointer = addlist[n];
                if (pointer == 0)
                {
                    continue;
                }
                uint p = Program.ROM.p32(pointer + 0);
                MakeVarsIDArraySub(list, p, pointer, foundDic);
            }
        }
    }
}
