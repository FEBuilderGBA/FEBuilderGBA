using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class FE8SpellMenuExtendsForm : Form
    {
        public FE8SpellMenuExtendsForm()
        {
            InitializeComponent();

            uint assignLevelUpP = FindFE8SpellPatchPointer();
            if (assignLevelUpP == U.NOT_FOUND)
            {
                return;
            }

            this.AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);
            InputFormRef = Init(this, assignLevelUpP);
            InputFormRef.MakeGeneralAddressListContextMenu(true);

            this.N1_AddressList.OwnerDraw(ListBoxEx.DrawItemAndText, DrawMode.OwnerDrawFixed);
            N1_InputFormRef = N1_Init(this);
            N1_InputFormRef.PostAddressListExpandsEvent += N1_InputFormRef_AddressListExpandsEvent;
            N1_InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self, uint assignLevelUpP)
        {
            uint unitDataCount = UnitForm.DataCount();

            InputFormRef ifr = new InputFormRef(self
                , ""
                , assignLevelUpP
                , 1
                , (int i, uint addr) =>
                {//読込最大値検索
                    return i < unitDataCount;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + " " + UnitForm.GetUnitName((uint)i);
                }
            );
            return ifr;
        }

        private void FE8SpellMenuForm_Load(object sender, EventArgs e)
        {
            uint assignLevelUpP = FindFE8SpellPatchPointer();
            if (assignLevelUpP == U.NOT_FOUND)
            {
                R.ShowStopError("FE8SpellMagic: GaidenStyleパッチがインストールされていません。");
                this.Close();
                return;
            }
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            uint assignLevelUpP = FindFE8SpellPatchPointer();
            if (assignLevelUpP == U.NOT_FOUND)
            {
                return;
            }
            uint assignLevelUpAddr = Program.ROM.p32(assignLevelUpP);
            if (assignLevelUpAddr == U.NOT_FOUND)
            {
                return;
            }

            InputFormRef InputFormRef;
            {
                InputFormRef = Init(null, assignLevelUpP);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "SkillAssignmentUnitSkillSystem", new uint[] { });

                Dictionary<uint, string> skillNames = new Dictionary<uint, string>();
                InputFormRef N1_InputFormRef = N1_Init(null);

                for (uint i = 0; i < InputFormRef.DataCount; i++, assignLevelUpAddr += 4)
                {
                    if (!U.isSafetyOffset(assignLevelUpAddr))
                    {
                        break;
                    }

                    uint levelupList = Program.ROM.p32(assignLevelUpAddr);
                    if (!U.isSafetyOffset(levelupList))
                    {
                        continue;
                    }

                    N1_InputFormRef.ReInitPointer(assignLevelUpAddr);
                    FEBuilderGBA.Address.AddAddress(list, N1_InputFormRef, "SkillAssignmentUnitSkillSystem.Levelup" + i, new uint[] { });
                }
            }
        }


        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint assignLevelUpP = FindFE8SpellPatchPointer();
            if (assignLevelUpP == U.NOT_FOUND)
            {
                return;
            }
            uint assignLevelUpAddr = Program.ROM.p32(assignLevelUpP);
            if (!U.isSafetyOffset(assignLevelUpAddr))
            {
                return;
            }

            uint addr = assignLevelUpAddr + (((uint)AddressList.SelectedIndex) * 4);
            uint levelupList = Program.ROM.p32(addr);
            if (!U.isSafetyOffset(levelupList))
            {
                N1_InputFormRef.ClearSelect(true);
                IndependencePanel.Visible = false;
                return;
            }

            N1_InputFormRef.ReInit(levelupList);

            //他のクラスでこのデータを参照しているならば、独立ボタンを出す.
            IndependencePanel.Visible = UpdateIndependencePanel();
            //N1の書き込みボタンが反応してしまうときがあるのでやめさせる.
            InputFormRef.WriteButtonToYellow(this.N1_WriteButton, false);

        }
        //他のクラスでこのデータを参照しているか?
        bool UpdateIndependencePanel()
        {
            uint assignLevelUpP = FindFE8SpellPatchPointer();
            if (assignLevelUpP == U.NOT_FOUND)
            {
                return false;
            }
            uint assignLevelUpAddr = Program.ROM.p32(assignLevelUpP);
            if (!U.isSafetyOffset(assignLevelUpAddr))
            {
                return false;
            }

            return SkillAssignmentClassSkillSystemForm.IsShowIndependencePanel(this.AddressList, assignLevelUpAddr);
        }

        public InputFormRef N1_InputFormRef;
        static InputFormRef N1_Init(Form self)
        {
            return new InputFormRef(self
                , "N1_"
                , 0
                , 2
                , (int i, uint addr) =>
                {//読込最大値検索
                    uint a = Program.ROM.u16(addr);
                    if (a == 0xFFFF || a == 0)
                    {
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    uint item_id = Program.ROM.u8(addr + 1);
                    return U.ToHexString(item_id) + " " + ItemForm.GetItemName(item_id);
                }
            );
        }

        private void N1_B1_ValueChanged(object sender, EventArgs e)
        {
        }

        void N1_InputFormRef_AddressListExpandsEvent(object sender, EventArgs e)
        {
            uint assignLevelUpP = FindFE8SpellPatchPointer();
            if (assignLevelUpP == U.NOT_FOUND)
            {
                return;
            }
            uint assignLevelUpAddr = Program.ROM.p32(assignLevelUpP);
            if (!U.isSafetyOffset(assignLevelUpAddr))
            {
                return;
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData(this, "AssignLevelUpBase");

            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)e;
            uint addr = eearg.NewBaseAddress;
            int count = (int)eearg.NewDataCount;

            uint termAddr = (uint)(addr + eearg.BlockSize * (count)); //余分に確保した終端データ
            uint termData = Program.ROM.u16(termAddr);
            if ((termData != 0 && count > 1))
            {//スキルリストは特殊で終端データは、0x00 0x00 でないといけない
                //終端コードを 0x00 0x00 にする.
                Program.ROM.write_u16(termAddr, 0x0000, undodata);
            }

            //拡張したアドレスを書き込む.
            uint write_addr = assignLevelUpAddr + (((uint)AddressList.SelectedIndex) * 4);
            Program.ROM.write_p32(write_addr, addr, undodata);

            Program.Undo.Push(undodata);
        }

        private void WriteButton_Click(object sender, EventArgs e)
        {
            uint assignLevelUpP = FindFE8SpellPatchPointer();
            if (assignLevelUpP == U.NOT_FOUND)
            {
                return;
            }
            uint assignLevelUpAddr = Program.ROM.p32(assignLevelUpP);
            if (!U.isSafetyOffset(assignLevelUpAddr))
            {
                return;
            }

            uint addr = (uint)N1_ReadStartAddress.Value;
            uint write_addr = assignLevelUpAddr + (((uint)AddressList.SelectedIndex) * 4);
            Program.Undo.Push("AssignLevelUpBase", write_addr, 4);

            Program.ROM.write_p32(write_addr, addr);
        }

        static uint g_Cache_FE8SpellPatchPointer = PatchUtil.NO_CACHE;
        public static void ClearCache()
        {
            g_Cache_FE8SpellPatchPointer = PatchUtil.NO_CACHE;
        }

        public static uint FindFE8SpellPatchPointer()
        {
            if (g_Cache_FE8SpellPatchPointer == PatchUtil.NO_CACHE)
            {
                g_Cache_FE8SpellPatchPointer = FindFE8SpellPatchPointerLow();
            }
            return g_Cache_FE8SpellPatchPointer;
        }

        static uint FindFE8SpellPatchPointerLow()
        {
            string SpellsGetter_dmp = Path.Combine(Program.BaseDirectory, "config", "patch2", "FE8U", "FE8SpellMenu", "GaidenMagic", "SpellsGetter.dmp");
            if (!File.Exists(SpellsGetter_dmp))
            {
                return U.NOT_FOUND;
            }
            byte[] SpellsGetter_dmp_bin = File.ReadAllBytes(SpellsGetter_dmp);
            uint pointer = U.GrepEnd(Program.ROM.Data, SpellsGetter_dmp_bin, Program.ROM.RomInfo.compress_image_borderline_address(), 0, 4);
            if (pointer == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }
            return pointer;
        }

        private void N1_B0_ValueChanged(object sender, EventArgs e)
        {
            if (sender != N1_B0)
            {
                return;
            }
            uint b0 = (uint)N1_B0.Value;
            N1_X_Promoted.Checked = ((b0 & 0x80) == 0x80);
            N1_X_Level.Value = (b0 & 0x7f);
        }

        private void X_Level_ValueChanged(object sender, EventArgs e)
        {
            if (sender != N1_X_Level)
            {
                return;
            }
            N1_B0.Value = MakeB0();
        }

        private void X_Promoted_CheckedChanged(object sender, EventArgs e)
        {
            if (sender != N1_X_Promoted)
            {
                return;
            }
            N1_B0.Value = MakeB0();
        }
        uint MakeB0()
        {
            uint b0 = (uint)N1_X_Level.Value;
            if (N1_X_Promoted.Checked)
            {
                b0 += 0x80;
            }
            return b0;
        }
    }
}
