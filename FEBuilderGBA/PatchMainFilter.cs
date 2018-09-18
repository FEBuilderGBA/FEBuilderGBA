using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace FEBuilderGBA
{
    public class PatchMainFilter
    {
        Button PatchButton = null;
        Button[] Buttons = null;
        Label PatchLabelName = null;
        PatchForm.PatchSt[] Patchs = null;
        string LastSearchWord;

        public PatchMainFilter(Button patchButton
            , Label label
            , Button[] buttons)
        {
            this.PatchButton = patchButton;
            this.PatchLabelName = label;
            this.Buttons = buttons;

            InputFormRef.markupJumpLabel(this.PatchLabelName);

            for (int n = 0; n < buttons.Length; n++)
            {
                Buttons[n].Click += OnButtonClick;
            }
        }
        void OnButtonClick(Object Sender, EventArgs e)
        {
            if (!(Sender is Control))
            {
                return;
            }
            Control c = (Control)Sender;
            string numString = U.substr(c.Name, -1);
            int index = (int)U.atoi(numString);

            PatchForm f = (PatchForm)InputFormRef.JumpForm<PatchForm>();
            f.JumpTo(this.LastSearchWord, index);
        }
        public void CleanChache()
        {
            this.Patchs = null;
        }
        public void ApplyFilter(string search, bool isJP, ToolTipEx tooltip)
        {
            this.LastSearchWord = search;
            if (search == "" || this.PatchButton.Visible)
            {
                this.PatchLabelName.Hide();
                for (int i = 0; i < Buttons.Length; i++)
                {
                    this.Buttons[i].Hide();
                }
                return;
            }
            if (Patchs == null)
            {
                Patchs = PatchForm.ScanPatch();
            }

            int foundCount = 0;
            for (int i = 0; i < this.Patchs.Length; i++)
            {
                PatchForm.PatchSt patch = this.Patchs[i];
                if (!U.StrStrEx(patch.SearchData, search, isJP))
                {//フィルターで消す.
                    continue;
                }
                Button b = Buttons[foundCount];
                b.Text = patch.Name;
                string info = U.at(patch.Param, "INFO");
                tooltip.SetToolTipOverraide(b, U.nl2br(info));
                b.Show();

                foundCount++;
                if (foundCount == 1)
                {//ボタンがあるならラベルも出す.
                    this.PatchLabelName.Show();
                }

                if (foundCount >= Buttons.Length)
                {
                    return;
                }
            }

            for (int n = foundCount; n < Buttons.Length; n++)
            {
                Button b = Buttons[n];
                b.Hide();
            }
        }
    }
}
