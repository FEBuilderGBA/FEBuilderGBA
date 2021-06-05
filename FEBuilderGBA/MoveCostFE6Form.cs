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
            LoadNames();
            this.AddressList.OwnerDraw(ListBoxEx.DrawClassAndText, DrawMode.OwnerDrawFixed);
            this.CLASS_LISTBOX.OwnerDraw(ListBoxEx.DrawClassAndText, DrawMode.OwnerDrawFixed);
            this.CLASS_LISTBOX.ItemListToJumpForm("CLASS");

            this.FilterComboBox.SelectedIndex = 0;
            this.InputFormRef = Init(this);
            this.InputFormRef.IsMemoryNotContinuous = true; //メモリは連続していないので、警告不能.
            this.InputFormRef.IsSurrogateStructure = true;  //代理構造体で表示されているので警告不能
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.InputFormRef.CheckProtectionPaddingALIGN4 = false;

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

            MoveCostForm.SetupIconOnFilterCombo(this.FilterComboBox);
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

                    int filter = FilerFE6ToFE8(self.FilterComboBox.SelectedIndex);
                    uint p = ClassForm.GetMoveCostAddrLow(addr, (uint)filter);
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

            ShowExplain();
        }
        void ShowExplain()
        {
            this.EXPLAIN.Text = MoveCostForm.GetExplain(FilerFE6ToFE8(this.FilterComboBox.SelectedIndex));
        }

        static int FilerFE6ToFE8(int fe6Filter)
        {
            if (fe6Filter == 0)
            {
                return 0;
            }
            //天気 雨と雪は FE6にはない.
            return fe6Filter + 2;
        }
        static int FilerFE8ToFE6(int fe8Filter)
        {
            if (fe8Filter <= 2)
            {
                return 0;
            }
            //天気 雨と雪は FE6にはない.
            return fe8Filter - 2;
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int filter = FilerFE6ToFE8(this.FilterComboBox.SelectedIndex);

            if (filter == 6 || filter == 7 || filter == 8)
            {//地形回復/地形ステータス異常回復/地形情報 全クラス共通
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
            FilterComboBox.SelectedIndex = FilerFE8ToFE6(filter - 1);
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
            int filter = FilerFE6ToFE8(this.FilterComboBox.SelectedIndex);
            uint setting = ClassForm.GetMoveCostPointerAddrLow(classaddr, (uint)filter);
            if (setting == U.NOT_FOUND)
            {
                return;
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"MoveCost Independence");
            PatchUtil.WriteIndependence(p, 54, setting, name, undodata);
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

            int filter = FilerFE6ToFE8(this.FilterComboBox.SelectedIndex);
            if (filter <= 2)
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
        void LoadNames()
        {
            J_0.Text = MapTerrainNameForm.GetName(0, true);
            J_1.Text = MapTerrainNameForm.GetName(1, true);
            J_2.Text = MapTerrainNameForm.GetName(2, true);
            J_3.Text = MapTerrainNameForm.GetName(3, true);
            J_4.Text = MapTerrainNameForm.GetName(4, true);
            J_5.Text = MapTerrainNameForm.GetName(5, true);
            J_6.Text = MapTerrainNameForm.GetName(6, true);
            J_7.Text = MapTerrainNameForm.GetName(7, true);
            J_8.Text = MapTerrainNameForm.GetName(8, true);
            J_9.Text = MapTerrainNameForm.GetName(9, true);
            J_10.Text = MapTerrainNameForm.GetName(10, true);
            J_11.Text = MapTerrainNameForm.GetName(11, true);
            J_12.Text = MapTerrainNameForm.GetName(12, true);
            J_13.Text = MapTerrainNameForm.GetName(13, true);
            J_14.Text = MapTerrainNameForm.GetName(14, true);
            J_15.Text = MapTerrainNameForm.GetName(15, true);
            J_16.Text = MapTerrainNameForm.GetName(16, true);
            J_17.Text = MapTerrainNameForm.GetName(17, true);
            J_18.Text = MapTerrainNameForm.GetName(18, true);
            J_19.Text = MapTerrainNameForm.GetName(19, true);
            J_20.Text = MapTerrainNameForm.GetName(20, true);
            J_21.Text = MapTerrainNameForm.GetName(21, true);
            J_22.Text = MapTerrainNameForm.GetName(22, true);
            J_23.Text = MapTerrainNameForm.GetName(23, true);
            J_24.Text = MapTerrainNameForm.GetName(24, true);
            J_25.Text = MapTerrainNameForm.GetName(25, true);
            J_26.Text = MapTerrainNameForm.GetName(26, true);
            J_27.Text = MapTerrainNameForm.GetName(27, true);
            J_28.Text = MapTerrainNameForm.GetName(28, true);
            J_29.Text = MapTerrainNameForm.GetName(29, true);
            J_30.Text = MapTerrainNameForm.GetName(30, true);
            J_31.Text = MapTerrainNameForm.GetName(31, true);
            J_32.Text = MapTerrainNameForm.GetName(32, true);
            J_33.Text = MapTerrainNameForm.GetName(33, true);
            J_34.Text = MapTerrainNameForm.GetName(34, true);
            J_35.Text = MapTerrainNameForm.GetName(35, true);
            J_36.Text = MapTerrainNameForm.GetName(36, true);
            J_37.Text = MapTerrainNameForm.GetName(37, true);
            J_38.Text = MapTerrainNameForm.GetName(38, true);
            J_39.Text = MapTerrainNameForm.GetName(39, true);
            J_40.Text = MapTerrainNameForm.GetName(40, true);
            J_41.Text = MapTerrainNameForm.GetName(41, true);
            J_42.Text = MapTerrainNameForm.GetName(42, true);
            J_43.Text = MapTerrainNameForm.GetName(43, true);
            J_44.Text = MapTerrainNameForm.GetName(44, true);
            J_45.Text = MapTerrainNameForm.GetName(45, true);
            J_46.Text = MapTerrainNameForm.GetName(46, true);
            J_47.Text = MapTerrainNameForm.GetName(47, true);
            J_48.Text = MapTerrainNameForm.GetName(48, true);
            J_49.Text = MapTerrainNameForm.GetName(49, true);
            J_50.Text = MapTerrainNameForm.GetName(50, true);
        }
    }
}
