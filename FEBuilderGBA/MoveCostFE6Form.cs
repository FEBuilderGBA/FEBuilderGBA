using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class MoveCostFE6Form : Form
    {
        public MoveCostFE6Form()
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
            for (int i = 0; i < 50 + 1; i++)
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
        static InputFormRef Init(MoveCostFE6Form self)
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
                    if (p == 0)
                    {
                        return new U.AddrResult();
                    }
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
            if (filter == 1 || filter == 2)
            {//天気 雨と雪は FE6にはない.
                filter = 0;
            }

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
                    uint p = ClassForm.GetMoveCostAddrLow(addr, (uint)filter);

                    return (p == selectAddr);
                }
            );
            U.ConvertListBox(list, ref this.CLASS_LISTBOX);

            //クラスが2つ以上あるなら分離ボタンを有効かする
            IndependenceButton.Enabled = (list.Count >= 2);
        }


        public void JumpToClassID(uint classid,int filter)
        {
            FilterComboBox.SelectedIndex = filter - 1;
            for (int i = 0; i < AddressList.Items.Count; i++)
            {
                if (U.atoh(AddressList.Items[i].ToString()) == classid)
                {
                    AddressList.SelectedIndex = i;
                    return;
                }
            }
        }

        //移動コスト等の取得.
        public static uint GetMoveCost(uint cid, uint terrain_data, uint costtype)
        {
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
            InputFormRef.WriteIndependence(p, 54, setting, name, undodata);
            Program.Undo.Push(undodata);

            InputFormRef.ShowWriteNotifyAnimation(this, p);

            this.ReloadListButton.PerformClick();
            this.InputFormRef.JumpTo(classid);
        }

        private void NUD_ValueChanged(object sender, EventArgs e)
        {
            if (!(sender is NumericUpDown))
            {
                return;
            }
            NumericUpDown nud = (NumericUpDown)(sender);
            uint id = InputFormRef.GetStructID("", nud.Name);

            Control c = InputFormRef.FindObjectByForm<LabelEx>(this.InputFormRef.Controls, "J_" + id);
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
