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
            for (int i = 0; i < 64 + 1; i++)
            {
                Control c = InputFormRef.FindObjectByFormFirstMatch<NumericUpDown>(controls, "B" + i);
                if (c is NumericUpDown)
                {
                    NumericUpDown nud = (NumericUpDown)c;
                    nud.ValueChanged += NUD_ValueChanged;
                }
            }
            SetupIconOnFilterCombo(this.FilterComboBox);
        }

        public static void SetupIconOnFilterCombo(ComboBoxEx combo)
        {
            combo.OwnerDraw(ComboBoxEx.DrawIconAndText, DrawMode.OwnerDrawFixed);

            combo.AddIcon(R._("移動コスト: 天気普通"), Properties.Resources.icon_sunny);
            combo.AddIcon(R._("移動コスト: 天気雨"), Properties.Resources.icon_rain);
            combo.AddIcon(R._("移動コスト: 天気雪"), Properties.Resources.icon_snow);
            combo.AddIcon(R._("地形回避"), ImageSystemIconForm.Forest());
            combo.AddIcon(R._("地形防御"), ImageSystemIconForm.Castle());
            //combo.AddIcon(R._("地形魔防"), );
            //combo.AddIcon(R._("地形回復"), ImageSystemIconForm.Throne());
            //combo.AddIcon(R._("地形ステータス異常回復"), ImageSystemIconForm.Throne());
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
            ShowExplain();
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int filter = this.FilterComboBox.SelectedIndex;

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
            PatchUtil.WriteIndependence(p, 68, setting, name, undodata);
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

            for (uint id = 0; id < max; id++)
            {
                uint v = Program.ROM.u8(addr + id);
                if (id == 0)
                {
                    if (v != 255)
                    {
                        string tilename = MapTerrainNameForm.GetName(id);
                        errors.Add(new FELint.ErrorSt(type, addr
                            , R._("クラス({0})の({1})の({2})のタイルの移動コスト({3})が正しくありません。\r\nこのタイルは必ず255に設定する必要があります。"
                            , classList[(int)cid].name, seetname, tilename, v), cid));
                    }
                }
                else
                {
                    if (v >= 31 || v <= 15)
                    {
                    }
                    else
                    {
                        string tilename = MapTerrainNameForm.GetName(id);
                        errors.Add(new FELint.ErrorSt(type, addr
                            , R._("クラス({0})の({1})の({2})のタイルの移動コスト({3})が正しくありません。\r\n移動できないタイルは255に、移動できるタイルは0-15の範囲に設定しないといけません。"
                            , classList[(int)cid].name, seetname, tilename, v), cid));
                    }
                }
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

            string error = "";
            if (FilterComboBox.SelectedIndex <= 2)
            {
                uint value = (uint)nud.Value;
                if (id == 0)
                {//最初のタイルは255である必要がある。
                    if (value != 255)
                    {
                        error = R._("タイルの移動コストが正しくありません。\r\nこのタイルは必ず255に設定する必要があります。");
                    }
                }
                else
                {
                    if (value >= 128 || value <= 15)
                    {//nop
                    }
                    else
                    {
                        error = R._("タイルの移動コストが正しくありません。\r\n移動できないタイルは255に、移動できるタイルは0-15の範囲に設定しないといけません。");
                    }
                }
            }
            label.ErrorMessage = error;
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
            J_51.Text = MapTerrainNameForm.GetName(51, true);
            J_52.Text = MapTerrainNameForm.GetName(52, true);
            J_53.Text = MapTerrainNameForm.GetName(53, true);
            J_54.Text = MapTerrainNameForm.GetName(54, true);
            J_55.Text = MapTerrainNameForm.GetName(55, true);
            J_56.Text = MapTerrainNameForm.GetName(56, true);
            J_57.Text = MapTerrainNameForm.GetName(57, true);
            J_58.Text = MapTerrainNameForm.GetName(58, true);
            J_59.Text = MapTerrainNameForm.GetName(59, true);
            J_60.Text = MapTerrainNameForm.GetName(60, true);
            J_61.Text = MapTerrainNameForm.GetName(61, true);
            J_62.Text = MapTerrainNameForm.GetName(62, true);
            J_63.Text = MapTerrainNameForm.GetName(63, true);
            J_64.Text = MapTerrainNameForm.GetName(64, true);
        }
        void ShowExplain()
        {
            this.EXPLAIN.Text = MoveCostForm.GetExplain(this.FilterComboBox.SelectedIndex);
            this.ExplainIcon.Image = this.FilterComboBox.GetCurrentIcon();
        }

        public static string GetExplain(int index)
        {
            if (index == 0)
            {
                return InputFormRef.GetExplain("@EXPLAIN_MOVEMENTCOST");
            }
            else if (index == 1)
            {
                return InputFormRef.GetExplain("@EXPLAIN_MOVEMENTCOST_RAIN");
            }
            else if (index == 2)
            {
                return InputFormRef.GetExplain("@EXPLAIN_MOVEMENTCOST_SNOW");
            }
            else if (index == 3)
            {
                return InputFormRef.GetExplain("@EXPLAIN_MOVEMENTCOST_AVO");
            }
            else if (index == 4)
            {
                return InputFormRef.GetExplain("@EXPLAIN_MOVEMENTCOST_DEF");
            }
            else if (index == 5)
            {
                return InputFormRef.GetExplain("@EXPLAIN_MOVEMENTCOST_RES");
            }
            else if (index == 6)
            {
                return InputFormRef.GetExplain("@EXPLAIN_MOVEMENTCOST_HEALING");
            }
            else if (index == 7)
            {
                return InputFormRef.GetExplain("@EXPLAIN_MOVEMENTCOST_RECOVERY");
            }
            else if (index == 8)
            {
                return InputFormRef.GetExplain("@EXPLAIN_MOVEMENTCOST_SHOW_INFOMATION");
            }
            return "";
        }

    }
}
