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
    public partial class ItemEtcForm : Form
    {
        public ItemEtcForm()
        {
            InitializeComponent();
        }

        private void ItemEtcForm_Load(object sender, EventArgs e)
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
            InputFormRef.makeLinkEventHandler("", controls, IF5, IF5_Item, 0, "ITEM", args);
            InputFormRef.makeLinkEventHandler("", controls, IF6, IF6_Item, 0, "ITEM", args);
            InputFormRef.makeLinkEventHandler("", controls, IF7, IF7_Item, 0, "ITEM", args);
            InputFormRef.makeLinkEventHandler("", controls, IF8, IF8_Item, 0, "ITEM", args);

            InputFormRef.makeLinkEventHandler("", controls, IF1, IF1_ItemIcon, 0, "ITEMICON", args);
            InputFormRef.makeLinkEventHandler("", controls, IF2, IF2_ItemIcon, 0, "ITEMICON", args);
            InputFormRef.makeLinkEventHandler("", controls, IF3, IF3_ItemIcon, 0, "ITEMICON", args);
            InputFormRef.makeLinkEventHandler("", controls, IF4, IF4_ItemIcon, 0, "ITEMICON", args);
            InputFormRef.makeLinkEventHandler("", controls, IF5, IF5_ItemIcon, 0, "ITEMICON", args);
            InputFormRef.makeLinkEventHandler("", controls, IF6, IF6_ItemIcon, 0, "ITEMICON", args);
            InputFormRef.makeLinkEventHandler("", controls, IF7, IF7_ItemIcon, 0, "ITEMICON", args);
            InputFormRef.makeLinkEventHandler("", controls, IF8, IF8_ItemIcon, 0, "ITEMICON", args);
            

            U.CopyItem(CMP1, CMP2);
            U.CopyItem(CMP1, CMP3);
            U.CopyItem(CMP1, CMP4);
            U.CopyItem(CMP1, CMP5);
            U.CopyItem(CMP1, CMP6);
            U.CopyItem(CMP1, CMP7);
            U.CopyItem(CMP1, CMP8);

            uint shinki = Program.ROM.RomInfo.weapon_effectiveness_2x3x_address();
            IF1.Value = Program.ROM.u8(shinki + 0);
            U.SelectCombo(CMP1, Program.ROM.u8(shinki + 3));
            IF2.Value = Program.ROM.u8(shinki + 4);
            U.SelectCombo(CMP2, Program.ROM.u8(shinki + 7));
            IF3.Value = Program.ROM.u8(shinki + 8);
            U.SelectCombo(CMP3, Program.ROM.u8(shinki + 11));
            IF4.Value = Program.ROM.u8(shinki + 12);
            U.SelectCombo(CMP4, Program.ROM.u8(shinki + 15));
            IF5.Value = Program.ROM.u8(shinki + 18);
            U.SelectCombo(CMP5, Program.ROM.u8(shinki + 21));
            IF6.Value = Program.ROM.u8(shinki + 22);
            U.SelectCombo(CMP6, Program.ROM.u8(shinki + 25));
            IF7.Value = Program.ROM.u8(shinki + 26);
            U.SelectCombo(CMP7, Program.ROM.u8(shinki + 29));
            IF8.Value = Program.ROM.u8(shinki + 30);
            U.SelectCombo(CMP8, Program.ROM.u8(shinki + 33));


            InputFormRef.makeLinkEventHandler("", controls, IF2_1, IF2_1_Item, 0, "ITEM", args);
            InputFormRef.makeLinkEventHandler("", controls, IF2_2, IF2_2_Item, 0, "ITEM", args);
            InputFormRef.makeLinkEventHandler("", controls, IF2_3, IF2_3_Item, 0, "ITEM", args);
            InputFormRef.makeLinkEventHandler("", controls, IF2_4, IF2_4_Item, 0, "ITEM", args);
            InputFormRef.makeLinkEventHandler("", controls, IF2_5, IF2_5_Item, 0, "ITEM", args);
            InputFormRef.makeLinkEventHandler("", controls, IF2_6, IF2_6_Item, 0, "ITEM", args);
            InputFormRef.makeLinkEventHandler("", controls, IF2_7, IF2_7_Item, 0, "ITEM", args);
            InputFormRef.makeLinkEventHandler("", controls, IF2_8, IF2_8_Item, 0, "ITEM", args);
            InputFormRef.makeLinkEventHandler("", controls, IF2_9, IF2_9_Item, 0, "ITEM", args);
            InputFormRef.makeLinkEventHandler("", controls, IF2_10, IF2_10_Item, 0, "ITEM", args);

            InputFormRef.makeLinkEventHandler("", controls, IF2_1, IF2_1_ItemIcon, 0, "ITEMICON", args);
            InputFormRef.makeLinkEventHandler("", controls, IF2_2, IF2_2_ItemIcon, 0, "ITEMICON", args);
            InputFormRef.makeLinkEventHandler("", controls, IF2_3, IF2_3_ItemIcon, 0, "ITEMICON", args);
            InputFormRef.makeLinkEventHandler("", controls, IF2_4, IF2_4_ItemIcon, 0, "ITEMICON", args);
            InputFormRef.makeLinkEventHandler("", controls, IF2_5, IF2_5_ItemIcon, 0, "ITEMICON", args);
            InputFormRef.makeLinkEventHandler("", controls, IF2_6, IF2_6_ItemIcon, 0, "ITEMICON", args);
            InputFormRef.makeLinkEventHandler("", controls, IF2_7, IF2_7_ItemIcon, 0, "ITEMICON", args);
            InputFormRef.makeLinkEventHandler("", controls, IF2_8, IF2_8_ItemIcon, 0, "ITEMICON", args);
            InputFormRef.makeLinkEventHandler("", controls, IF2_9, IF2_9_ItemIcon, 0, "ITEMICON", args);
            InputFormRef.makeLinkEventHandler("", controls, IF2_10, IF2_10_ItemIcon, 0, "ITEMICON", args);


            U.CopyItem(CMP1, CMP2_1);
            U.CopyItem(CMP1, CMP2_2);
            U.CopyItem(CMP1, CMP2_3);
            U.CopyItem(CMP1, CMP2_4);
            U.CopyItem(CMP1, CMP2_5);
            U.CopyItem(CMP1, CMP2_6);
            U.CopyItem(CMP1, CMP2_7);
            U.CopyItem(CMP1, CMP2_8);
            U.CopyItem(CMP1, CMP2_9);
            U.CopyItem(CMP1, CMP2_10);

            shinki = Program.ROM.RomInfo.weapon_battle_flash_address();
            IF2_1.Value = Program.ROM.u8(shinki + 0);
            U.SelectCombo(CMP2_1, Program.ROM.u8(shinki + 3));
            IF2_2.Value = Program.ROM.u8(shinki + 4);
            U.SelectCombo(CMP2_2, Program.ROM.u8(shinki + 7));
            IF2_3.Value = Program.ROM.u8(shinki + 8);
            U.SelectCombo(CMP2_3, Program.ROM.u8(shinki + 11));
            IF2_4.Value = Program.ROM.u8(shinki + 12);
            U.SelectCombo(CMP2_4, Program.ROM.u8(shinki + 15));
            IF2_5.Value = Program.ROM.u8(shinki + 16);
            U.SelectCombo(CMP2_5, Program.ROM.u8(shinki + 19));
            IF2_6.Value = Program.ROM.u8(shinki + 22);
            U.SelectCombo(CMP2_6, Program.ROM.u8(shinki + 25));
            IF2_7.Value = Program.ROM.u8(shinki + 28);
            U.SelectCombo(CMP2_7, Program.ROM.u8(shinki + 31));
            IF2_8.Value = Program.ROM.u8(shinki + 32);
            U.SelectCombo(CMP2_8, Program.ROM.u8(shinki + 35));
            IF2_9.Value = Program.ROM.u8(shinki + 36);
            U.SelectCombo(CMP2_9, Program.ROM.u8(shinki + 39));
            IF2_10.Value = Program.ROM.u8(shinki + 40);
            U.SelectCombo(CMP2_10, Program.ROM.u8(shinki + 43));

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
                U.Swap(ref stat1, ref stat2);
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
            uint cmp5 = U.atoh(CMP5.Text);
            uint cmp6 = U.atoh(CMP6.Text);
            uint cmp7 = U.atoh(CMP7.Text);
            uint cmp8 = U.atoh(CMP8.Text);

            Program.Undo.Push("shinki", shinki, 34);

            Program.ROM.write_u8(shinki + 0, (uint)IF1.Value);
            if (cmp1 > 0) Program.ROM.write_u8(shinki + 3, cmp1);
            Program.ROM.write_u8(shinki + 4, (uint)IF2.Value);
            if (cmp2 > 0) Program.ROM.write_u8(shinki + 7, cmp2);
            Program.ROM.write_u8(shinki + 8, (uint)IF3.Value);
            if (cmp3 > 0) Program.ROM.write_u8(shinki + 11, cmp3);
            Program.ROM.write_u8(shinki + 12, (uint)IF4.Value);
            if (cmp4 > 0) Program.ROM.write_u8(shinki + 15, cmp4);
            Program.ROM.write_u8(shinki + 18, (uint)IF5.Value);
            if (cmp5 > 0) Program.ROM.write_u8(shinki + 21, cmp5);
            Program.ROM.write_u8(shinki + 22, (uint)IF6.Value);
            if (cmp6 > 0) Program.ROM.write_u8(shinki + 25, cmp6);
            Program.ROM.write_u8(shinki + 26, (uint)IF7.Value);
            if (cmp7 > 0) Program.ROM.write_u8(shinki + 29, cmp7);
            Program.ROM.write_u8(shinki + 30, (uint)IF8.Value);
            if (cmp8 > 0) Program.ROM.write_u8(shinki + 33, cmp8);
        }

        void AllWriteButton_Shinki2(uint shinki)
        {
            uint cmp1 = U.atoh(CMP2_1.Text);
            uint cmp2 = U.atoh(CMP2_2.Text);
            uint cmp3 = U.atoh(CMP2_3.Text);
            uint cmp4 = U.atoh(CMP2_4.Text);
            uint cmp5 = U.atoh(CMP2_5.Text);
            uint cmp6 = U.atoh(CMP2_6.Text);
            uint cmp7 = U.atoh(CMP2_7.Text);
            uint cmp8 = U.atoh(CMP2_8.Text);
            uint cmp9 = U.atoh(CMP2_9.Text);
            uint cmp10 = U.atoh(CMP2_10.Text);

            Program.Undo.Push("shinki", shinki, 44);

            Program.ROM.write_u8(shinki + 0, (uint)IF2_1.Value);
            if (cmp1 > 0) Program.ROM.write_u8(shinki + 3, cmp1);
            Program.ROM.write_u8(shinki + 4, (uint)IF2_2.Value);
            if (cmp2 > 0) Program.ROM.write_u8(shinki + 7, cmp2);
            Program.ROM.write_u8(shinki + 8, (uint)IF2_3.Value);
            if (cmp3 > 0) Program.ROM.write_u8(shinki + 11, cmp3);
            Program.ROM.write_u8(shinki + 12, (uint)IF2_4.Value);
            if (cmp4 > 0) Program.ROM.write_u8(shinki + 15, cmp4);
            Program.ROM.write_u8(shinki + 16, (uint)IF2_5.Value);
            if (cmp5 > 0) Program.ROM.write_u8(shinki + 19, cmp5);
            Program.ROM.write_u8(shinki + 22, (uint)IF2_6.Value);
            if (cmp6 > 0) Program.ROM.write_u8(shinki + 25, cmp6);
            Program.ROM.write_u8(shinki + 28, (uint)IF2_7.Value);
            if (cmp7 > 0) Program.ROM.write_u8(shinki + 31, cmp7);
            Program.ROM.write_u8(shinki + 32, (uint)IF2_8.Value);
            if (cmp8 > 0) Program.ROM.write_u8(shinki + 35, cmp8);
            Program.ROM.write_u8(shinki + 36, (uint)IF2_9.Value);
            if (cmp9 > 0) Program.ROM.write_u8(shinki + 39, cmp9);
            Program.ROM.write_u8(shinki + 40, (uint)IF2_10.Value);
            if (cmp10 > 0) Program.ROM.write_u8(shinki + 43, cmp10);
        }

        private void AllWriteButton_Click(object sender, EventArgs e)
        {
            AllWriteButton_RankS();

            uint shinki = Program.ROM.RomInfo.weapon_effectiveness_2x3x_address();
            AllWriteButton_Shinki(shinki);
            uint shinki2 = Program.ROM.RomInfo.weapon_battle_flash_address();
            AllWriteButton_Shinki2(shinki2);

            InputFormRef.WriteButtonToYellow(this.AllWriteButton, false);
            InputFormRef.ShowWriteNotifyAnimation(this, 0);
        }


    }
}
