using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ToolASMEditForm : Form
    {
        public ToolASMEditForm()
        {
            InitializeComponent();
        }

        private void ToolASMEditForm_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = true;
        }

        uint EditAddr;
        DisASMInnerControl CallbackASMForm;

        string ParseCodeOnly(string code)
        {
            string programCode;
            if (code.IndexOf(" BL 0x") >= 0)
            {
                programCode = code.Substring(8 + 1 + 4 + 1 + 4 + 1).Trim();
                programCode = ReplaceOffsetAddress(programCode);
            }
            else if (code.IndexOf(" //LDRDATA") >= 0)
            {
                programCode = ".word 0x";
                programCode += code.Substring(8 + 1 + 4 + 1, 4);
                programCode += code.Substring(8 + 1, 4);
            }
            else
            {
                programCode = code.Substring(8 + 1 + 4 + 1).Trim();
                programCode = ReplaceOffsetAddress(programCode);
            }
            return programCode;
        }
        string  ReplaceOffsetAddress(string code)
        {
            return RegexCache.Replace(code, "#?(0x0?[8-9][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f])", "$1 + . - origin");
        }

        public void Init(DisASMInnerControl callbackWindow, string code)
        {
            code = code.Trim();

            this.EditAddr = U.atoh(code);
            this.CallbackASMForm = callbackWindow;

            string currentCode = ParseCodeOnly(code);
            currentCode = currentCode.Replace("//", "@").Replace("# ", "@ ").Replace(";", "@");

            string asm = ".thumb          @Do not delete this two line.\r\n";
            asm += ".equ origin, " + U.To0xHexString(this.EditAddr) + "\r\n";
            asm += "\r\n";
            asm += currentCode + "\r\n";

            this.Code.Text = asm;
            this.Code.Select(asm.Length,0);
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
        }

        private void AllWriteButton_Click(object sender, EventArgs e)
        {
            if (this.EditAddr < U.toPointer(0x100) )
            {
                R.ShowStopError("0x100以下への書き込みは、危険なのでできません。");
                this.Focus();
                return;
            }

            string temp_asm = Path.GetTempFileName();
            U.WriteAllText(temp_asm ,this.Code.Text2);

            string output;
            string symbol;
            bool r = MainFormUtil.CompilerDevkitPro(temp_asm, out output, out symbol, MainFormUtil.CompileType.NONE, false);

            File.Delete(temp_asm);

            if (!r || File.Exists(output) == false )
            {
                NotifyArea.Text = output;
                R.ShowStopError(output);
                this.Focus();
                return;
            }

            //変更点の抽出
            byte[] bin = File.ReadAllBytes(output);
            File.Delete(output);

            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            Program.ROM.write_range(U.toOffset(this.EditAddr), bin, undodata);
            Program.Undo.Push(undodata);

            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
            this.Close();

            if (this.CallbackASMForm == null || this.CallbackASMForm.IsDisposed)
            {
                return;
            }
            this.CallbackASMForm.ReloadASM();
            InputFormRef.ShowWriteNotifyAnimation(this.CallbackASMForm.ParentForm, U.toOffset(this.EditAddr));
        }

        private void Code_TextChanged(object sender, EventArgs e)
        {
            InputFormRef.WriteButtonToYellow(this.AllWriteButton, true);
            SimpleKeywordHighLight(this.Code);
        }

        public void SimpleKeywordHighLight(RichTextBoxEx target)
        {
            target.LockWindowUpdate();
            Color keywordBackColor = OptionForm.Color_Keyword_BackColor();
            Color keywordForeColor = OptionForm.Color_Keyword_ForeColor();

            Color commentBackColor = OptionForm.Color_Input_BackColor();
            Color commentForeColor = OptionForm.Color_Comment_ForeColor();

            string[] keywords = new string[] { "LSL", "LSR", "ASR", "ADD", "SUB", "MOV", "CMP", "AND", "EOR", "ADC", "SBC", "ROR", "TST", "NEG", "CMP", "CMN", "ORR", "BIC", "MVN", "BX", "LDR", "LDRH", "LDRB", "STR", "STRH", "STRB", "LDSB", "LDSH", "PUSH", "POP", "STMIA", "LDMIA", "BEQ", "BNE", "BCS", "BCC", "BMI", "BPL", "BVS", "BVC", "BHI", "BLS", "BGE", "BLT", "BGT", "BLE", "SWI", "B", "BL", ".THUMB", ".ORG",".EQU",".LTORG" };

            //ハイライトを解除.
            target.SelectionStart = 0;
            target.SelectionLength = target.TextLength;
            target.SelectionColor = OptionForm.Color_Input_ForeColor();
            target.SelectionBackColor = OptionForm.Color_Input_BackColor();
            //改行コードが違うので、必ず取得しなおす
            string text = target.Text;
            int length = text.Length;
            for (int i = 0; i < length; )
            {
                if (U.Wordrap(text[i]))
                {
                    i++;
                    continue;
                }

                int start = i;
                for ( i = i + 1 ; i < length; i++)
                {
                    if (U.Wordrap(text[i]))
                    {
                        break;
                    }
                }

                string token = text.Substring(start, i - start);
                if (token.IndexOf('@') == 0)
                {//コメント
                    int term = text.IndexOf("\n", i);
                    if (term > 0)
                    {
                        i = term;
                    }

                    target.SelectionStart = start;
                    target.SelectionLength = i - start;
                    target.SelectionColor = commentForeColor;
                    target.SelectionBackColor = commentBackColor;
                    continue;
                }

                token = token.ToUpper();
                foreach(string k in keywords)
                {
                    if (token == k)
                    {//キーワード
                        target.SelectionStart = start;
                        target.SelectionLength = i - start;
                        target.SelectionColor = keywordForeColor;
                        target.SelectionBackColor = keywordBackColor;
                        break;
                    }
                }
            }

            target.UnLockWindowUpdate();
        }

    
    }
}
