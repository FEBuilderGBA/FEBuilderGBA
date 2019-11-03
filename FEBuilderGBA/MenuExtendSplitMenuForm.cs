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
    public partial class MenuExtendSplitMenuForm : Form
    {
        public MenuExtendSplitMenuForm()
        {
            InitializeComponent();
        }

        public uint AllocIfNeed(NumericUpDown src)
        {
            if (src.Value == 0)
            {
                string alllocQMessage = R._("新規にメニューデータを作成しますか？");
                DialogResult dr = R.ShowYesNo(alllocQMessage);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    src.Value = NewAlloc();
                }
            }
            return (uint)src.Value;
        }
        public void JumpToAddr(uint addr)
        {
            addr = U.toOffset(addr);
            JumpToAddrLow(addr);
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
        }
        void JumpToAddrLow(uint addr)
        {
            this.ReadStartAddress.Value = addr;
            this.B0.Value = Program.ROM.u8(addr + 0);
            this.B1.Value = Program.ROM.u8(addr + 1);
            this.B2.Value = Program.ROM.u8(addr + 2);
            this.D4.Value = Program.ROM.u32(addr + 4);

            uint menuaddr = Program.ROM.p32(addr + 8);

            uint a = menuaddr + (36 * 0) + 4;
            if (! U.isSafetyOffset(a + 2))
            {
                return;
            }
            this.STR1.Value = Program.ROM.u16(a);

            a = menuaddr + (36 * 1) + 4;
            if (!U.isSafetyOffset(a + 2))
            {
                return;
            }
            this.STR2.Value = Program.ROM.u16(a);

            a = menuaddr + (36 * 2) + 4;
            if (!U.isSafetyOffset(a + 2))
            {
                return;
            }
            this.STR3.Value = Program.ROM.u16(a);

            a = menuaddr + (36 * 3) + 4;
            if (!U.isSafetyOffset(a + 2))
            {
                return;
            }
            this.STR4.Value = Program.ROM.u16(a);

            a = menuaddr + (36 * 4) + 4;
            if (!U.isSafetyOffset(a + 2))
            {
                return;
            }
            this.STR5.Value = Program.ROM.u16(a);

            if (GetDataLength(menuaddr) == 5)
            {
                LSTR6.Hide();
                STR6.Hide();
                STR6_TEXT.Hide();

                LSTR7.Hide();
                STR7.Hide();
                STR7_TEXT.Hide();

                LSTR8.Hide();
                STR8.Hide();
                STR8_TEXT.Hide();
            }
            else
            {
                a = menuaddr + (36 * 5) + 4;
                if (!U.isSafetyOffset(a + 2))
                {
                    return;
                }
                this.STR6.Value = Program.ROM.u16(a);

                a = menuaddr + (36 * 6) + 4;
                if (!U.isSafetyOffset(a + 2))
                {
                    return;
                }
                this.STR7.Value = Program.ROM.u16(a);

                a = menuaddr + (36 * 7) + 4;
                if (!U.isSafetyOffset(a + 2))
                {
                    return;
                }
                this.STR8.Value = Program.ROM.u16(a);
            }
        }

        private void MenuExtendSplitMenuForm_Load(object sender, EventArgs e)
        {
            List<Control> controls = InputFormRef.GetAllControls(this);
            InputFormRef.makeLinkEventHandler("", controls, this.STR1, this.STR1_TEXT, 0, "TEXT", new string[] { });
            InputFormRef.makeLinkEventHandler("", controls, this.STR2, this.STR2_TEXT, 0, "TEXT", new string[] { });
            InputFormRef.makeLinkEventHandler("", controls, this.STR3, this.STR3_TEXT, 0, "TEXT", new string[] { });
            InputFormRef.makeLinkEventHandler("", controls, this.STR4, this.STR4_TEXT, 0, "TEXT", new string[] { });
            InputFormRef.makeLinkEventHandler("", controls, this.STR5, this.STR5_TEXT, 0, "TEXT", new string[] { });
            InputFormRef.makeLinkEventHandler("", controls, this.STR6, this.STR6_TEXT, 0, "TEXT", new string[] { });
            InputFormRef.makeLinkEventHandler("", controls, this.STR7, this.STR7_TEXT, 0, "TEXT", new string[] { });
            InputFormRef.makeLinkEventHandler("", controls, this.STR8, this.STR8_TEXT, 0, "TEXT", new string[] { });

            InputFormRef.RegistNotifyNumlicUpdate(this.AllWriteButton, controls);
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
        }

        uint NewAlloc()
        {
            byte[] alloc = new byte[36 + (36 * 9) + 4];

            Undo.UndoData undodata = Program.Undo.NewUndoData("NewAlloc");
            uint addr = InputFormRef.AppendBinaryData(alloc, undodata);
            if (addr == U.NOT_FOUND)
            {//割り当て失敗
                return U.NOT_FOUND;
            }
            uint prEventMenuCommandEffect = FindEventMenuCommandEffect();
            if (prEventMenuCommandEffect == U.NOT_FOUND)
            {//パッチがインストールされていない.
                return U.NOT_FOUND;
            }

            uint prEventMenuDisplayCommand = FindEventMenuDisplayCommand();
            uint prEventMenuDrawFunction = FindEventMenuDrawCommand();

            Program.ROM.write_u8(addr + 0, 6, undodata); //x
            Program.ROM.write_u8(addr + 1, 8, undodata); //y
            Program.ROM.write_u8(addr + 2, 18, undodata); //width
            Program.ROM.write_u8(addr + 3, 0, undodata); //height
            Program.ROM.write_u32(addr + 4, 1, undodata); //style
            Program.ROM.write_p32(addr + 8, addr + 36, undodata); //Command Definitions


            uint[] texts;
            if (Program.ROM.RomInfo.is_multibyte())
            {
                texts = new uint[] { 0xBD5, 0xBD6, 0, 0, 0, 0, 0, 0 };
            }
            else
            {
                texts = new uint[] { 0xC15, 0xC16, 0, 0, 0, 0, 0, 0 };
            }

            uint a = addr + 36;
            for (uint i = 0; i < 9; i++, a += 36)
            {
                Program.ROM.write_u16(a + 4, texts[i], undodata); //text
                if (texts[i] == 0)
                {
                    break;
                }
                if (Program.ROM.RomInfo.is_multibyte())
                {
                    Program.ROM.write_p32(a + 0, 0x1f5310, undodata); //null文字列
                }

                Program.ROM.write_u8(a + 9, i, undodata); //MenuID
                Program.ROM.write_p32(a + 12, prEventMenuDisplayCommand, undodata); //表示時
                Program.ROM.write_p32(a + 16, prEventMenuDrawFunction, undodata); //描画時
                Program.ROM.write_p32(a + 20, prEventMenuCommandEffect, undodata); //選択時
            }

            {//nullが大量にできるので、終端マークを入れます.
                a = addr + 36 + (36 * 9);
                Program.ROM.write_u32(a , 0xffffffff, undodata);
            }

            //新規に追加した分のデータを書き込み.
            Program.Undo.Push(undodata);

            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
            InputFormRef.ShowWriteNotifyAnimation(this, 0);
            return addr;
        }
        uint FindEventMenuDisplayCommand()
        {
            Debug.Assert(Program.ROM.RomInfo.version() == 8);

            if (Program.ROM.RomInfo.is_multibyte())
            {
                return 0x0501BC + 1;
            }
            else
            {
                return 0x04F448 + 1;
            }
        }
        uint FindEventMenuDrawCommand()
        {
            Debug.Assert(Program.ROM.RomInfo.version() == 8);

            if (Program.ROM.RomInfo.is_multibyte())
            {
                return 0x0887e0 + 1;
            }
            else
            {
                return 0;
            }
        }

        uint FindEventMenuCommandEffect()
        {
            Debug.Assert(Program.ROM.RomInfo.version() == 8);

            byte[] need;
            if (Program.ROM.RomInfo.is_multibyte())
            {
                need = new byte[] { 0x00, 0xB5, 0x3C, 0x20, 0x08, 0x5C, 0x03, 0x4B, 0x9E, 0x46, 0x00, 0xF8, 0x17, 0x20, 0x02, 0xBC, 0x08, 0x47, 0x00, 0x00, 0xBC, 0xD4, 0x00, 0x08 };
            }
            else
            {
                need = new byte[] { 0x00, 0xB5, 0x3C, 0x20, 0x08, 0x5C, 0x03, 0x4B, 0x9E, 0x46, 0x00, 0xF8, 0x17, 0x20, 0x02, 0xBC, 0x08, 0x47, 0x00, 0x00, 0xF8, 0xD1, 0x00, 0x08 };
            }
            uint p = U.Grep(Program.ROM.Data, need, 0xE00000, 0, 4);
            if (p == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }
            return p + 1;
        }

        private void AllWriteButton_Click(object sender, EventArgs e)
        {
            uint addr = (uint)this.ReadStartAddress.Value;
            if (!U.isSafetyOffset(addr + 8 + 4))
            {
                R.ShowStopError("メニューの領域が確保されていません。");
                return;
            }

            uint menuaddr = Program.ROM.p32(addr + 8);
            if (!U.isSafetyOffset(menuaddr))
            {
                R.ShowStopError("メニューの個々の選択子の領域が確保されていません。");
                return;
            }
            uint prEventMenuCommandEffect = FindEventMenuCommandEffect();
            uint prEventMenuDisplayCommand = FindEventMenuDisplayCommand();
            uint prFindEventMenuDrawCommand = FindEventMenuDrawCommand();

            Undo.UndoData undodata = Program.Undo.NewUndoData(this);

            Program.ROM.write_u8 (addr + 0 , (uint)this.B0.Value , undodata); //x
            Program.ROM.write_u8 (addr + 1 , (uint)this.B1.Value , undodata); //y
            Program.ROM.write_u8 (addr + 2 , (uint)this.B2.Value , undodata); //width
            Program.ROM.write_u32(addr + 4 , (uint)this.D4.Value , undodata); //style

            uint[] texts;
            if (GetDataLength(menuaddr) == 8)
            {
                texts = new uint[]{(uint)this.STR1.Value ,(uint)this.STR2.Value
                ,(uint)this.STR3.Value ,(uint)this.STR4.Value   ,(uint)this.STR5.Value
                ,(uint)this.STR6.Value,(uint)this.STR7.Value,(uint)this.STR8.Value};
            }
            else
            {
                texts = new uint[]{(uint)this.STR1.Value ,(uint)this.STR2.Value
                ,(uint)this.STR3.Value ,(uint)this.STR4.Value   ,(uint)this.STR5.Value};
            }

            uint i = 0;
            for (i = 0; i < texts.Length; i++)
            {
                uint a = menuaddr + (36 * i);
                if (Program.ROM.RomInfo.is_multibyte())
                {
                    Program.ROM.write_p32(a + 0, 0x1f5310, undodata); //null文字列
                }

                Program.ROM.write_u16(a + 4, texts[i], undodata); //textid
                Program.ROM.write_u8(a + 9, i, undodata); //MenuID
                if (texts[i] == 0)
                {
                    Program.ROM.write_u32(a + 12, 0, undodata); //表示時
                    Program.ROM.write_u32(a + 16, 0, undodata); //描画時
                    Program.ROM.write_u32(a + 20, 0, undodata); //選択時
                    break;
                }
                else
                {
                    
                    Program.ROM.write_p32(a + 12, prEventMenuDisplayCommand , undodata); //表示時
                    Program.ROM.write_p32(a + 16, prFindEventMenuDrawCommand, undodata); //描画時
                    Program.ROM.write_p32(a + 20, prEventMenuCommandEffect , undodata); //選択時
                }
            }

            Program.Undo.Push(undodata);
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
            InputFormRef.ShowWriteNotifyAnimation(this, 0);
        }

        uint GetDataLength(uint menuaddr)
        {
            uint termData = menuaddr + (36 * 9);
            if (!U.isSafetyOffset(termData - 1))
            {
                return 5;
            }

            for (uint i = 6; i < 8; i++)
            {
                uint addr = menuaddr + (36 * i);
                if (!U.isSafetyOffset(addr))
                {
                    return 5;
                }
                if (!U.isSafetyOffset(addr + 36 - 1))
                {
                    return 5;
                }

                uint no = Program.ROM.u8(addr + 9);
                if (no == 0)
                {
                    uint textid = Program.ROM.u16(addr + 4);
                    if (textid != 0)
                    {
                        return 5;
                    }

                    uint a = Program.ROM.u32(addr + 12);
                    if (a != 0)
                    {
                        return 5;
                    }
                    a = Program.ROM.u32(addr + 16);
                    if (a != 0)
                    {
                        return 5;
                    }
                    a = Program.ROM.u32(addr + 20);
                    if (a != 0)
                    {
                        return 5;
                    }
                }
                else
                {
                    if (no != i)
                    {//不明なデータなので5ということにしておく.
                        return 5;
                    }

                    uint a = Program.ROM.u32(addr + 12);
                    if (!U.isPointerASMOrNull(a))
                    {
                        return 5;
                    }
                    a = Program.ROM.u32(addr + 16);
                    if (!U.isPointerASMOrNull(a))
                    {
                        return 5;
                    }
                    a = Program.ROM.u32(addr + 20);
                    if (!U.isPointerASMOrNull(a))
                    {
                        return 5;
                    }
                }
            }
            return 8;
        }

        //全データの取得
        public static void RecycleOldData(ref List<Address> recycle, uint script_pointer)
        {
            MenuDefinitionForm.MakeAllDataLength(recycle, script_pointer , isDirectAddress: true);
        }

        public static void MakeTextIDArray(List<UseTextID> list, uint script_pointer)
        {
            MenuDefinitionForm.MakeTextIDArray(list, script_pointer, isDirectAddress: true);
        }

    }
}
