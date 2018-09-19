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
    public partial class MoveCostForm : Form
    {
        public MoveCostForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawClassAndText, DrawMode.OwnerDrawFixed);
            this.CLASS_LISTBOX.OwnerDraw(ListBoxEx.DrawClassAndText, DrawMode.OwnerDrawFixed);
            this.CLASS_LISTBOX.ItemListToJumpForm("CLASS");

            this.FilterComboBox.SelectedIndex = 0;
            this.InputFormRef = Init(this);
            this.InputFormRef.IsMemoryNotContinuous = true; //メモリは連続していないので、警告不能.
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            List<Control> controls = InputFormRef.GetAllControls(this);
            for (int i = 0; i < 64 + 1; i++)
            {
                Control c = InputFormRef.FindObjectByFormFirstMatch<NumericUpDown>(controls, "B" + i);
                if (c is NumericUpDown)
                {
                    NumericUpDown nud = (NumericUpDown)c;
                    nud.ValueChanged += NUD_ValueChanged;
                }
            }
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(MoveCostForm self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.class_pointer()
                , Program.ROM.RomInfo.class_datasize()
                , (int i, uint addr) =>
                {//読込最大値検索
                    if (i == 0)
                    {
                        return true;
                    }
                    uint no = Program.ROM.u8(addr + 4);
                    return (no != 0);
                }
                , (int i, uint addr) =>
                {
                    if (i == 0)
                    {
                        return new U.AddrResult(0, "");
                    }
                    uint p = ClassForm.GetMoveCostAddrLow(addr,(uint)self.FilterComboBox.SelectedIndex);
                    
                    string name = U.ToHexString(i) + " " + ClassForm.GetClassNameLow(addr);
                    return new U.AddrResult(p, name);
                }
                );
        }

        private void MoveCostForm_Load(object sender, EventArgs e)
        {
        }

        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ReloadListButton.PerformClick();
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int filter = this.FilterComboBox.SelectedIndex;

            if (filter == 6)
            {//地形回復 全クラス共通
                IndependenceButton.Enabled = false;
                CLASS_LISTBOX.Items.Clear();
                CLASS_LISTBOX.Items.Add(R._("全クラス共通"));
                return;
            }

            uint selectAddr = (uint)this.Address.Value;

            List<U.AddrResult> list = 
                ClassForm.MakeClassList((uint addr) =>
                {
                    uint p = ClassForm.GetMoveCostAddrLow(addr, (uint)this.FilterComboBox.SelectedIndex);

                    return (p == selectAddr);
                }
            );
            U.ConvertListBox(list, ref this.CLASS_LISTBOX);

            //クラスが2つ以上あるなら分離ボタンを有効かする
            IndependenceButton.Enabled = (list.Count >= 2);
        }

        public void JumpToClassID(uint classid,int filter)
        {
            U.ForceUpdate(FilterComboBox, filter - 1);
            for (int i = 0; i < AddressList.Items.Count; i++)
            {
                if (U.atoh(AddressList.Items[i].ToString()) == classid)
                {
                    U.ForceUpdate(AddressList, i);
                    return;
                }
            }
        }

        //移動コスト等の取得.
        public static uint GetMoveCost(uint cid, uint terrain_data, uint costtype)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {//FE6の場合
                return MoveCostFE6Form.GetMoveCost(cid, terrain_data, costtype);
            }

            uint addr = ClassForm.GetMoveCostAddr(cid, costtype);
            if (!U.isSafetyOffset(addr))
            {
                return U.NOT_FOUND;
            }
            return Program.ROM.u8(addr + terrain_data);
        }

        private void IndependenceButton_Click(object sender, EventArgs e)
        {
            if (this.AddressList.SelectedIndex < 0)
            {
                return;
            }
            uint classid = (uint)U.atoh(this.AddressList.Text);
            uint classaddr = ClassForm.GetClassAddr(classid);
            string name = U.ToHexString(classid) + " " + ClassForm.GetClassNameLow(classaddr);

            uint p = InputFormRef.SelectToAddr(this.AddressList);
            uint setting = ClassForm.GetMoveCostPointerAddrLow(classaddr, (uint)this.FilterComboBox.SelectedIndex);
            if (setting == U.NOT_FOUND)
            {
                return;
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"MoveCost Independence");
            InputFormRef.WriteIndependence(p, 68, setting, name, undodata);
            Program.Undo.Push(undodata);

            InputFormRef.ShowWriteNotifyAnimation(this, p);

            this.ReloadListButton.PerformClick();
            this.InputFormRef.JumpTo(classid);
        }
        static void MakeCheckErrorOneAvoid(FELint.Type type
            , uint addr
            , uint cid
            , List<U.AddrResult> classList
            , List<FELint.ErrorSt> errors)
        {
            if (addr == 0)
            {
                return;
            }

            string seetname;
            if (type == FELint.Type.MOVECOST_AVOID)
            {
                seetname = R._("地形回避");
            }
            else if (type == FELint.Type.MOVECOST_DEF)
            {
                seetname = R._("地形防御");
            }
            else if (type == FELint.Type.MOVECOST_RES)
            {
                seetname = R._("地形魔防");
            }
            else
            {
                Debug.Assert(false);
                return;
            }

            if (!U.isSafetyOffset(addr))
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.CLASS, classList[(int)cid].addr
                    , R._("クラス({0})の{1}のポインタ({2})が危険です。"
                    , classList[(int)cid].name, seetname, U.To0xHexString(addr)), cid));
                return;
            }
        }

        static void MakeCheckErrorOneMove(FELint.Type type
            ,uint addr
            ,uint cid
            ,List<U.AddrResult> classList
            ,List<FELint.ErrorSt> errors)
        {
            if (addr == 0)
            {
                return ;
            }

            string seetname;
            if (type == FELint.Type.MOVECOST_NORMAL)
            {
                seetname = R._("移動コスト");
            }
            else if (type == FELint.Type.MOVECOST_RAIN)
            {
                seetname = R._("移動コスト(雨)");
            }
            else if (type == FELint.Type.MOVECOST_SHOW)
            {
                seetname = R._("移動コスト(雪)");
            }
            else
            {
                Debug.Assert(false);
                return ;
            }
            
            if (!U.isSafetyOffset(addr))
            {
                errors.Add(new FELint.ErrorSt(FELint.Type.CLASS, classList[(int)cid].addr
                    , R._("クラス({0})の{1}のポインタ({2})が危険です。"
                    , classList[(int)cid].name, seetname, U.To0xHexString(addr)), cid));
                return;
            }
            uint max = 64 + 1;
            if (Program.ROM.RomInfo.version() == 6)
            {
                max = 50 + 1;
            }
            for (uint i = 0; i < max; i++)
            {
                uint v = Program.ROM.u8(addr + i);
                if (v == 255 || v <= 15)
                {
                    continue;
                }

                string tilename = MapTerrainNameForm.GetName(i);
                errors.Add(new FELint.ErrorSt(type, addr
                    , R._("クラス({0})の({1})の({2})のタイルの移動コスト({3})が正しくありません。\r\n移動できないタイルは255に、移動できるタイルは0-15の範囲に設定しないといけません。"
                    , classList[(int)cid].name, seetname, tilename, v ) , cid));
            }
        }

        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            bool isFE6 = (Program.ROM.RomInfo.version() == 6);
            InputFormRef InputFormRef = Init(null);

            List<U.AddrResult> classList = ClassForm.MakeClassList();
            for (uint cid = 1; cid < classList.Count; cid++)
            {
                uint class_addr = classList[(int)cid].addr;
                if (isFE6)
                {
                    uint addr = Program.ROM.p32(class_addr + 52);
                    MakeCheckErrorOneMove(FELint.Type.MOVECOST_NORMAL, addr, cid, classList, errors);
                    addr = Program.ROM.p32(class_addr + 56);
                    MakeCheckErrorOneAvoid(FELint.Type.MOVECOST_AVOID, addr, cid, classList, errors);
                    addr = Program.ROM.p32(class_addr + 60);
                    MakeCheckErrorOneAvoid(FELint.Type.MOVECOST_DEF, addr, cid, classList, errors);
                    addr = Program.ROM.p32(class_addr + 64);
                    MakeCheckErrorOneAvoid(FELint.Type.MOVECOST_RES, addr, cid, classList, errors);
                }
                else
                {
                    uint addr = Program.ROM.p32(class_addr + 56);
                    MakeCheckErrorOneMove(FELint.Type.MOVECOST_NORMAL, addr, cid, classList, errors);
                    addr = Program.ROM.p32(class_addr + 60);
                    MakeCheckErrorOneMove(FELint.Type.MOVECOST_RAIN, addr, cid, classList, errors);
                    addr = Program.ROM.p32(class_addr + 64);
                    MakeCheckErrorOneMove(FELint.Type.MOVECOST_SHOW, addr, cid, classList, errors);
                    addr = Program.ROM.p32(class_addr + 68);
                    MakeCheckErrorOneAvoid(FELint.Type.MOVECOST_AVOID, addr, cid, classList, errors);
                    addr = Program.ROM.p32(class_addr + 72);
                    MakeCheckErrorOneAvoid(FELint.Type.MOVECOST_DEF, addr, cid, classList, errors);
                    addr = Program.ROM.p32(class_addr + 76);
                    MakeCheckErrorOneAvoid(FELint.Type.MOVECOST_RES, addr, cid, classList, errors);
                }
            }
        }

        private void NUD_ValueChanged(object sender, EventArgs e)
        {
            if (!(sender is NumericUpDown))
            {
                return;
            }
            NumericUpDown nud = (NumericUpDown)(sender);
            uint id = InputFormRef.GetStructID("", nud.Name);


            Control c = InputFormRef.FindObjectByForm<LabelEx>(InputFormRef.Controls, "J_" + id);
            if (!(c is LabelEx))
            {
                return;
            }
            LabelEx label = (LabelEx)c;

            if (FilterComboBox.SelectedIndex <= 2)
            {
                uint value = (uint)nud.Value;
                if (value == 255 || value <= 15)
                {
                    label.ErrorMessage = "";
                }
                else
                {
                    label.ErrorMessage = R._("タイルの移動コストが正しくありません。\r\n移動できないタイルは255に、移動できるタイルは0-15の範囲に設定しないといけません。");
                }
            }
            else
            {
                label.ErrorMessage = "";
            }
        }

    }
}
