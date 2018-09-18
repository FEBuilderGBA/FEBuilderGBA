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
    public partial class ItemEtcFE7Form : Form
    {
        public ItemEtcFE7Form()
        {
            InitializeComponent();
        }

        private void ItemEtcFE7Form_Load(object sender, EventArgs e)
        {
            List<Control> controls = InputFormRef.GetAllControls(this);
            InputFormRef.RegistNotifyNumlicUpdate(this.AllWriteButton, controls);

            //武器レベルS
            U.CopyItem(RankS_Bonus_Stat1, RankS_Bonus_Stat2);
            uint rankS = Program.ROM.RomInfo.weapon_rank_s_bonus_address();

            RankS_Exp.Value = Program.ROM.u8(rankS + 0);
            uint stat1 = Program.ROM.u8(rankS + 6);
            U.SelectCombo(RankS_Bonus_Stat1, stat1);
            RankS_Bonus_Stat1_Value.Value = Program.ROM.u8(rankS + 10);

            uint stat1add = Program.ROM.u8(rankS + 14);
            U.SelectCombo(RankS_Bonus_Stat2, stat1 + stat1add);
            RankS_Bonus_Stat2_Value.Value = Program.ROM.u8(rankS + 18);


            //神器フラッシュ(ハードコーディングされているので・・・)
            string[] args = new string[]{"ITEM"};
            InputFormRef.makeLinkEventHandler("", controls, IF1, IF1_Item, 0, "ITEM", args);
            InputFormRef.makeLinkEventHandler("", controls, IF2, IF2_Item, 0, "ITEM", args);
            InputFormRef.makeLinkEventHandler("", controls, IF3, IF3_Item, 0, "ITEM", args);
            InputFormRef.makeLinkEventHandler("", controls, IF4, IF4_Item, 0, "ITEM", args);

            InputFormRef.makeLinkEventHandler("", controls, IF1, IF1_ItemIcon, 0, "ITEMICON", args);
            InputFormRef.makeLinkEventHandler("", controls, IF2, IF2_ItemIcon, 0, "ITEMICON", args);
            InputFormRef.makeLinkEventHandler("", controls, IF3, IF3_ItemIcon, 0, "ITEMICON", args);
            InputFormRef.makeLinkEventHandler("", controls, IF4, IF4_ItemIcon, 0, "ITEMICON", args);


            U.CopyItem(CMP1, CMP2);
            U.CopyItem(CMP1, CMP3);
            U.CopyItem(CMP1, CMP4);

            uint shinki = Program.ROM.RomInfo.weapon_battle_flash_address();
            IF1.Value = Program.ROM.u8(shinki + 0);
            U.SelectCombo(CMP1, Program.ROM.u8(shinki + 3));
            IF2.Value = Program.ROM.u8(shinki + 10);
            U.SelectCombo(CMP2, Program.ROM.u8(shinki + 13));
            IF3.Value = Program.ROM.u8(shinki + 20);
            U.SelectCombo(CMP3, Program.ROM.u8(shinki + 23));
            IF4.Value = Program.ROM.u8(shinki + 30);
            U.SelectCombo(CMP4, Program.ROM.u8(shinki + 33));

            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
        }

        private void AllWriteButton_RankS()
        {
            uint rankS = Program.ROM.RomInfo.weapon_rank_s_bonus_address();
            Program.Undo.Push("rankS", rankS, 20);

            Program.ROM.write_u8(rankS + 0, (uint)RankS_Exp.Value);

            uint stat1 = U.atoh(RankS_Bonus_Stat1.Text);
            uint stat2 = U.atoh(RankS_Bonus_Stat2.Text);

            if (stat1 == 0)
            {
                //無選択なので 何もしない
                return;
            }

            if (stat1 > stat2)
            {//stat2 は、増値を書くしかないので、マイナスにはできない.
                U.Swap(ref stat1,ref stat2);
            }
            Program.ROM.write_u8(rankS + 6, stat1);
            Program.ROM.write_u8(rankS + 10, (uint)RankS_Bonus_Stat1_Value.Value);

            Program.ROM.write_u8(rankS + 14, stat2 - stat1);
            Program.ROM.write_u8(rankS + 18, (uint)RankS_Bonus_Stat2_Value.Value);
        }

        void AllWriteButton_Shinki(uint shinki)
        {
            uint cmp1 = U.atoh(CMP1.Text);
            uint cmp2 = U.atoh(CMP2.Text);
            uint cmp3 = U.atoh(CMP3.Text);
            uint cmp4 = U.atoh(CMP4.Text);

            Program.Undo.Push("shinki", shinki, 34);

            Program.ROM.write_u8(shinki + 0, (uint)IF1.Value);
            if (cmp1 > 0) Program.ROM.write_u8(shinki + 3, cmp1);
            Program.ROM.write_u8(shinki + 10, (uint)IF2.Value);
            if (cmp2 > 0) Program.ROM.write_u8(shinki + 13, cmp2);
            Program.ROM.write_u8(shinki + 20, (uint)IF3.Value);
            if (cmp3 > 0) Program.ROM.write_u8(shinki + 23, cmp3);
            Program.ROM.write_u8(shinki + 30, (uint)IF4.Value);
            if (cmp4 > 0) Program.ROM.write_u8(shinki + 33, cmp4);
        }

        private void AllWriteButton_Click(object sender, EventArgs e)
        {
            AllWriteButton_RankS();

            uint shinki = Program.ROM.RomInfo.weapon_battle_flash_address();
            AllWriteButton_Shinki(shinki);

            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
            InputFormRef.ShowWriteNotifyAnimation(this, 0);
        }

    }
}
