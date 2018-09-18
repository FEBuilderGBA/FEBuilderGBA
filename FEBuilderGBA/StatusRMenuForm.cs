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
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            this.FilterComboBox.BeginUpdate();
            this.FilterComboBox.Items.Add(R._("0=ステータスパラメータ"));
            this.FilterComboBox.Items.Add(R._("1=所持アイテム"));
            this.FilterComboBox.Items.Add(R._("2=武器レベル"));
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
                        already.Clear();
                        return false;
                    }

                    ListFounder(addr, ref rmenulist, ref already);
                    if (rmenulist.Count <= 0)
                    {
                        already.Clear();
                        return false;
                    }

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
                this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.status_rmenu1_pointer()) );
            }
            else if (selected == 1)
            {
                this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.status_rmenu2_pointer()));
            }
            else if (selected == 2)
            {
                this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.status_rmenu3_pointer()));
            }
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            uint[] addlist = new uint[]{ Program.ROM.RomInfo.status_rmenu1_pointer(),Program.ROM.RomInfo.status_rmenu2_pointer(),Program.ROM.RomInfo.status_rmenu3_pointer()};

            for (int n = 0; n < addlist.Length; n++)
            {
                uint addr = addlist[n];

                InputFormRef InputFormRef = Init(null);
                List<U.AddrResult> makelist = InputFormRef.MakeList(addr);
                string name = "RMENU" + n;
                FEBuilderGBA.Address.AddAddress(list
                    , InputFormRef
                    , name
                    , new uint[] { 0,4,8,12,20,24}
                    , FEBuilderGBA.Address.DataTypeEnum.InputFormRef_MIX
                    );

                for (int i = 0; i < makelist.Count ; i++)
                {
                    uint p = makelist[i].addr;

                    uint paddr = Program.ROM.p32(20 + p);
                    FEBuilderGBA.Address.AddAddress(list,
                            DisassemblerTrumb.ProgramAddrToPlain(paddr)
                        , 0 //プログラムなので長さ不明
                        , p + 20
                        , name + "+P20"
                        , FEBuilderGBA.Address.DataTypeEnum.ASM);

                    paddr = Program.ROM.p32(24 + p);
                    FEBuilderGBA.Address.AddAddress(list,
                            DisassemblerTrumb.ProgramAddrToPlain(paddr)
                        , 0 //プログラムなので長さ不明
                        , p + 24
                        , name + "+P24"
                        , FEBuilderGBA.Address.DataTypeEnum.ASM);
                }
            }
        }
        public static void MakeTextIDArray(List<TextID> list)
        {
            InputFormRef InputFormRef = Init(null);
            TextID.AppendTextID(list, FELint.Type.RMENU, InputFormRef, new uint[] { 18 });
        }
    }
}
