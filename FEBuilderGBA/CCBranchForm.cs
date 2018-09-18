using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class CCBranchForm : Form
    {
        public CCBranchForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(ListBoxEx.DrawClassAndText, DrawMode.OwnerDrawFixed);
            this.CLASS_LISTBOX.OwnerDraw(ListBoxEx.DrawClassAndText, DrawMode.OwnerDrawFixed);
            this.CLASS_LISTBOX.ItemListToJumpForm("CLASS");

            int datcount = (int)ClassForm.DataCount();
            this.InputFormRef = Init(this, datcount);

            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self, int datcount)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.ccbranch_pointer()
                , 2
                , (int i, uint addr) =>
                {//読込最大値検索
                    return (i < datcount);
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + " " + ClassForm.GetClassName((uint)i);
                }
                );
        }
        private void CCBranchForm_Load(object sender, EventArgs e)
        {
            List<Control> controls = InputFormRef.GetAllControls(this);
            string[] args = new string[0];
            InputFormRef.makeLinkEventHandler("", controls, X_SELECT_UPCC1_CLASSID, X_SELECT_UPCC1_CLASS, 0, "CLASS", args);
            InputFormRef.makeLinkEventHandler("", controls, X_SELECT_UPCC2_CLASSID, X_SELECT_UPCC2_CLASS, 0, "CLASS", args);
            InputFormRef.makeLinkEventHandler("", controls, X_SELECT_UPCC3_CLASSID, X_SELECT_UPCC3_CLASS, 0, "CLASS", args);
            InputFormRef.makeLinkEventHandler("", controls, X_SELECT_UPCC4_CLASSID, X_SELECT_UPCC4_CLASS, 0, "CLASS", args);

            InputFormRef.makeLinkEventHandler("", controls, X_SELECT_UPCC1_CLASSID, X_SELECT_UPCC1_CLASSNAME, 0, "CLASSICON", args);
            InputFormRef.makeLinkEventHandler("", controls, X_SELECT_UPCC2_CLASSID, X_SELECT_UPCC2_CLASSNAME, 0, "CLASSICON", args);
            InputFormRef.makeLinkEventHandler("", controls, X_SELECT_UPCC3_CLASSID, X_SELECT_UPCC3_CLASSNAME, 0, "CLASSICON", args);
            InputFormRef.makeLinkEventHandler("", controls, X_SELECT_UPCC4_CLASSID, X_SELECT_UPCC4_CLASSNAME, 0, "CLASSICON", args);

            if (X_CC3Patch.Visible)
            {//CC3分岐パッチが導入されている場合
                InputFormRef.makeLinkEventHandler("", controls, X_CC3, X_L_CC3_CLASS, 0, "CLASS", args);
                InputFormRef.makeLinkEventHandler("", controls, X_CC3, X_L_CC3_CLASSICON, 0, "CLASSICON", args);

                InputFormRef.makeLinkEventHandler("", controls, X_SELECT_UPCC5_CLASSID, X_SELECT_UPCC5_CLASS, 0, "CLASS", args);
                InputFormRef.makeLinkEventHandler("", controls, X_SELECT_UPCC6_CLASSID, X_SELECT_UPCC6_CLASS, 0, "CLASS", args);
                InputFormRef.makeLinkEventHandler("", controls, X_SELECT_UPCC5_CLASSID, X_SELECT_UPCC5_CLASSNAME, 0, "CLASSICON", args);
                InputFormRef.makeLinkEventHandler("", controls, X_SELECT_UPCC6_CLASSID, X_SELECT_UPCC6_CLASSNAME, 0, "CLASSICON", args);
            }
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<U.AddrResult> list = this.InputFormRef.MakeList();
            uint class_id = (uint)this.AddressList.SelectedIndex;
            if (class_id >= this.InputFormRef.DataCount)
            {
                return;
            }

            //選択しているクラス名.
            X_SELECT_CLASS.Text = ClassForm.GetClassName(class_id);

            //このクラスにCCできるクラスを探す.
            CLASS_LISTBOX.BeginUpdate();
            CLASS_LISTBOX.Items.Clear();
            if (class_id >= 1)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    uint cc_class1 = Program.ROM.u8(list[i].addr + 0);
                    uint cc_class2 = Program.ROM.u8(list[i].addr + 1);

                    if (cc_class1 == class_id || cc_class2 == class_id)
                    {
                        string name = U.ToHexString(i) + " " + ClassForm.GetClassName((uint)i);
                        CLASS_LISTBOX.Items.Add(name);
                    }
                }
            }
            CLASS_LISTBOX.EndUpdate();

            if (X_CC3Patch.Visible)
            {//CC3分岐パッチが導入されている場合
                uint classAddr = ClassForm.GetClassAddr(class_id);
                if (classAddr == U.NOT_FOUND)
                {
                    U.ForceUpdate(X_CC3, 0);
                }
                else
                {
                    uint cc_class3 = Program.ROM.u8(classAddr + 5);
                    U.ForceUpdate(X_CC3, (int)cc_class3);
                }
            }
        }

        private void UpdateClass(object sender, EventArgs e)
        {
            List<U.AddrResult> list = this.InputFormRef.MakeList();
            uint class_id = (uint)this.AddressList.SelectedIndex;

            //現在選択しているクラスCCから派生するCCを取得
            {
                uint cc_class1 = (uint)B0.Value;
                uint cc_class2 = (uint)B1.Value;
                if (cc_class1 <= 0 || cc_class1 >= this.InputFormRef.DataCount)
                {//変なクラス
                    U.ForceUpdate(X_SELECT_UPCC1_CLASSID, (int)0);
                    U.ForceUpdate(X_SELECT_UPCC2_CLASSID, (int)0);
                }
                else
                {
                    uint up_cc_class1 = Program.ROM.u8(list[(int)cc_class1].addr + 0);
                    uint up_cc_class2 = Program.ROM.u8(list[(int)cc_class1].addr + 1);
                    U.ForceUpdate(X_SELECT_UPCC1_CLASSID, (int)up_cc_class1);
                    U.ForceUpdate(X_SELECT_UPCC2_CLASSID, (int)up_cc_class2);
                }

                if (cc_class2 <= 0 || cc_class2 >= this.InputFormRef.DataCount)
                {//変なクラス
                    U.ForceUpdate(X_SELECT_UPCC3_CLASSID, (int)0);
                    U.ForceUpdate(X_SELECT_UPCC4_CLASSID, (int)0);
                }
                else
                {
                    uint up_cc_class3 = Program.ROM.u8(list[(int)cc_class2].addr + 0);
                    uint up_cc_class4 = Program.ROM.u8(list[(int)cc_class2].addr + 1);
                    U.ForceUpdate(X_SELECT_UPCC3_CLASSID, (int)up_cc_class3);
                    U.ForceUpdate(X_SELECT_UPCC4_CLASSID, (int)up_cc_class4);
                }
            }

            if (X_CC3Patch.Visible)
            {//CC3分岐パッチが導入されている場合
                uint cc_class3 = (uint)X_CC3.Value;

                if (cc_class3 <= 0 || cc_class3 >= this.InputFormRef.DataCount)
                {//変なクラス
                    U.ForceUpdate(X_SELECT_UPCC5_CLASSID, (int)0);
                    U.ForceUpdate(X_SELECT_UPCC6_CLASSID, (int)0);
                }
                else
                {
                    uint up_cc_class5 = Program.ROM.u8(list[(int)cc_class3].addr + 0);
                    uint up_cc_class6 = Program.ROM.u8(list[(int)cc_class3].addr + 1);
                    U.ForceUpdate(X_SELECT_UPCC5_CLASSID, (int)up_cc_class5);
                    U.ForceUpdate(X_SELECT_UPCC6_CLASSID, (int)up_cc_class6);
                }

            }
        }

        public static int GetCCCount(uint shien_classs_id)
        {
            if (Program.ROM.RomInfo.version() <= 7)
            {//FE7までは分岐がないので、CCクラスを参照する.
                if (ClassForm.isHighClass(shien_classs_id))
                {//上位クラスなので、もう CCではない
                    return 0;
                }
                //下位クラスなのでCCできる.
                return 1;
            }


            int ccount1 = 0;
            int ccount2 = 0;

            int datcount = (int)ClassForm.DataCount();
            InputFormRef  InputFormRef = Init(null, datcount);
            List<U.AddrResult> list = InputFormRef.MakeList();
            if (shien_classs_id > 0 && shien_classs_id < list.Count)
            {
                uint up_cc_class1 = Program.ROM.u8(list[(int)shien_classs_id].addr + 0);
                uint up_cc_class2 = Program.ROM.u8(list[(int)shien_classs_id].addr + 1);
                if (up_cc_class1 > 0 && up_cc_class1 < list.Count)
                {
                    ccount1++;
                    uint up2_cc_class1 = Program.ROM.u8(list[(int)up_cc_class1].addr + 0);
                    uint up2_cc_class2 = Program.ROM.u8(list[(int)up_cc_class1].addr + 1);
                    if (
                          (up2_cc_class1 > 0 && up2_cc_class1 < list.Count)
                       || (up2_cc_class1 > 0 && up2_cc_class1 < list.Count)
                    )
                    {
                        ccount1++;
                    }
                }
                if (up_cc_class2 > 0 && up_cc_class2 < list.Count)
                {
                    ccount2++;
                    uint up2_cc_class1 = Program.ROM.u8(list[(int)up_cc_class2].addr + 0);
                    uint up2_cc_class2 = Program.ROM.u8(list[(int)up_cc_class2].addr + 1);
                    if (
                          (up2_cc_class1 > 0 && up2_cc_class1 < list.Count)
                       || (up2_cc_class1 > 0 && up2_cc_class1 < list.Count)
                    )
                    {
                        ccount2++;
                    }
                }

            }
            return Math.Max(ccount1,ccount2);
        }

        public static uint ExpandsArea(Form form, uint current_count,uint newdatacount, Undo.UndoData undodata)
        {
            InputFormRef InputFormRef = Init(null, (int)current_count);
            return InputFormRef.ExpandsArea(form, newdatacount, undodata, Program.ROM.RomInfo.ccbranch_pointer());
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            int datcount = (int)ClassForm.DataCount();
            InputFormRef InputFormRef = Init(null, datcount);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "CCBranch", new uint[] { });
        }

        private void WriteButton_Click(object sender, EventArgs e)
        {
            if (! X_CC3Patch.Visible)
            {
                return ;
            }
            //CC3分岐パッチが導入されている場合

            uint class_id = (uint)this.AddressList.SelectedIndex;
            if (class_id >= this.InputFormRef.DataCount)
            {
                return;
            }
            uint classAddr = ClassForm.GetClassAddr(class_id);
            if (classAddr == U.NOT_FOUND)
            {
                return;
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            Program.ROM.write_u8(classAddr + 5, (uint)X_CC3.Value, undodata);

            Program.Undo.Push(undodata);
        }

    }
}
