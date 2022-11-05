﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class SkillAssignmentClassSkillSystemForm : Form
    {
        public SkillAssignmentClassSkillSystemForm()
        {
            InitializeComponent();

            uint iconP = SkillConfigSkillSystemForm.FindIconPointer();
            uint textP = SkillConfigSkillSystemForm.FindTextPointer();
            uint assignClassP = SkillConfigSkillSystemForm.FindAssignClassSkillPointer();
            uint assignLevelUpP = SkillConfigSkillSystemForm.FindAssignClassLevelUpSkillPointer();

            if (iconP == U.NOT_FOUND)
            {
                R.ShowStopError("スキル拡張 SkillSystem の、アイコンを取得できません。");
                return;
            }
            if (textP == U.NOT_FOUND)
            {
                R.ShowStopError("スキル拡張 SkillSystem の、テキストを取得できません。");
                return;
            }
            if (assignClassP == U.NOT_FOUND)
            {
                R.ShowStopError("スキル拡張 SkillSystem の、クラススキルを取得できません。");
                return;
            }
            if (assignLevelUpP == U.NOT_FOUND)
            {
                R.ShowStopError("スキル拡張 SkillSystem の、レベルアップスキルを取得できません。");
                return;
            }
            this.SkillNames = SkillConfigSkillSystemForm.LoadSkillNames();

            this.TextBaseAddress = Program.ROM.p32(textP);
            this.IconBaseAddress = Program.ROM.p32(iconP);
            this.AssignClassBaseAddress = Program.ROM.p32(assignClassP);
            this.AssignLevelUpBaseAddress = Program.ROM.p32(assignLevelUpP);

            this.N1_AddressList.OwnerDraw(DrawSkillAndText, DrawMode.OwnerDrawFixed);
            InputFormRef.markupJumpLabel(this.N1_J_1_SKILLASSIGNMENT);
            N1_InputFormRef = N1_Init(this, this.SkillNames);
            N1_InputFormRef.PostAddressListExpandsEvent += N1_InputFormRef_AddressListExpandsEvent;
            N1_InputFormRef.MakeGeneralAddressListContextMenu(true);

            this.AddressList.OwnerDraw(ListBoxEx.DrawClassAndText, DrawMode.OwnerDrawFixed);
            InputFormRef.markupJumpLabel(this.J_0_SKILLASSIGNMENT);
            InputFormRef = Init(this, assignClassP);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.InputFormRef.CheckProtectionPaddingALIGN4 = false;

            U.SetIcon(ExportAllButton, Properties.Resources.icon_arrow);
            U.SetIcon(ImportAllButton, Properties.Resources.icon_upload);
            InputFormRef.markupJumpLabel(X_LEARNINFO);

            if (SkillConfigSkillSystemForm.IsClassSkillExtends())
            {
                UseXLevelAddPanel = true;
            }
        }

        bool UseXLevelAddPanel = false;

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self, uint assignClass)
        {
            uint classDataCount = ClassForm.DataCount();

            InputFormRef ifr = new InputFormRef(self
                , ""
                , assignClass
                , 1
                , (int i, uint addr) =>
                {//読込最大値検索
                    if (i >= classDataCount)
                    {//既存のクラスの最大値を超えたらダメ
                        return false;
                    }
                    if (i >= 0xFE)
                    {
                        if (Program.ROM.u8(addr) == 0xFF)
                        {//終端コードが出てきたらそこで強制終了
                            return false;
                        }
                    }

                    return true;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + " " + ClassForm.GetClassName((uint)i);
                }
            );

            return ifr;
        }

        public InputFormRef N1_InputFormRef;
        static InputFormRef N1_Init(Form self, Dictionary<uint, string> skillNames)
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
                    uint skillid = Program.ROM.u8(addr+1);
                    return U.ToHexString(skillid) + " " + U.at(skillNames, skillid);
                }
            );
        }

        private void SkillAssignmentClassSkillSystemForm_Load(object sender, EventArgs e)
        {

        }

        uint TextBaseAddress;
        uint IconBaseAddress;
        uint AssignClassBaseAddress;
        uint AssignLevelUpBaseAddress;
        Dictionary<uint, string> SkillNames;

        //Skill + テキストを書くルーチン
        Size DrawSkillAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            return DrawSkillAndText(lb, index, g, listbounds, isWithDraw, this.IconBaseAddress);
        }

        //Skill + テキストを書くルーチン
        public static Size DrawSkillAndText(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw, uint iconBaseAddress)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            string text = lb.Items[index].ToString();

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;
            Rectangle bounds = listbounds;

            int textmargineY = (ListBoxEx.OWNER_DRAW_ICON_SIZE - (int)lb.Font.Height) / 2;

            uint icon = U.atoh(text);
            Bitmap bitmap = SkillConfigSkillSystemForm.DrawIcon(icon, iconBaseAddress);
            U.MakeTransparent(bitmap);

            //アイコンを描く.
            Rectangle b = bounds;
            b.Width = ListBoxEx.OWNER_DRAW_ICON_SIZE;
            b.Height = ListBoxEx.OWNER_DRAW_ICON_SIZE;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
            bitmap.Dispose();

            //テキストを描く.
            b = bounds;
            b.Y += textmargineY;
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, b);
            bounds.Y += ListBoxEx.OWNER_DRAW_ICON_SIZE;

            brush.Dispose();
            return new Size(bounds.X, bounds.Y);
        }

        private void B0_ValueChanged(object sender, EventArgs e)
        {
            SKILLICON.Image = SkillConfigSkillSystemForm.DrawIcon((uint)this.B0.Value, this.IconBaseAddress);
            SKILLTEXT.Text = SkillConfigSkillSystemForm.GetSkillText((uint)this.B0.Value, this.TextBaseAddress);
            SKILLNAME.Text = U.at(this.SkillNames, (uint)this.B0.Value);
        }


        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint addr = AssignLevelUpBaseAddress + (((uint)AddressList.SelectedIndex)*4);
            uint levelupList = Program.ROM.u32(addr);
            X_LevelUpAddr.Value = levelupList;

            ShowExtraControlPanel(levelupList);
        }

        void ShowExtraControlPanel(uint levelupList)
        {
            if (AddressList.SelectedIndex == 0)
            {
                ZeroPointerPanel.Visible = false;
                IndependencePanel.Visible = false;
            }
            else if (levelupList == 0)
            {
                ZeroPointerPanel.Visible = true;
                IndependencePanel.Visible = false;
            }
            else
            {
                ZeroPointerPanel.Visible = false;
                IndependencePanel.Visible = IsShowIndependencePanel();
            }
        }

        private void N1_B1_ValueChanged(object sender, EventArgs e)
        {
            N1_SKILLICON.Image = SkillConfigSkillSystemForm.DrawIcon((uint)this.N1_B1.Value, this.IconBaseAddress);
            N1_SKILLTEXT.Text = SkillConfigSkillSystemForm.GetSkillText((uint)this.N1_B1.Value, this.TextBaseAddress);
            N1_SKILLNAME.Text = U.at(this.SkillNames, (uint)this.N1_B1.Value);
        }


        void N1_InputFormRef_AddressListExpandsEvent(object sender, EventArgs e)
        {
            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"AssignLevelUpBase");
            
            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)e;
            uint addr = eearg.NewBaseAddress;
            int count = (int)eearg.NewDataCount;

            uint termAddr = (uint)(addr + eearg.BlockSize * (count)); //余分に確保した終端データ
            uint termData = Program.ROM.u16(termAddr);
            if ( (termData != 0 && count > 1) )
            {//スキルリストは特殊で終端データは、0x00 0x00 でないといけない
                //終端コードを 0x00 0x00 にする.
                Program.ROM.write_u16(termAddr, 0x0000, undodata);
            }

            //スキルが0だと終端がわからなくなるので、適当なものを入れる.
            uint a = addr + (eearg.OldDataCount * eearg.BlockSize);
            const uint default_skill_lv = 0x0101;
            for (int i = (int)eearg.OldDataCount; i < count; i++)
            {
                uint skill_lv = Program.ROM.u16(a);
                if (skill_lv == 0)
                {
                    Program.ROM.write_u16(a, default_skill_lv, undodata);
                }

                a += eearg.BlockSize;
            }

            //拡張したアドレスを書き込む.
            uint write_addr = AssignLevelUpBaseAddress + (((uint)AddressList.SelectedIndex) * 4);
            Program.ROM.write_p32(write_addr, addr, undodata);
            this.X_LevelUpAddr.Value = U.toPointer(addr);

            Program.Undo.Push(undodata);

//            N1_ReadCount.Value = eearg.NewDataCount;
//            N1_InputFormRef.ReInit(addr, eearg.NewDataCount);
        }

        private void WriteButton_Click(object sender, EventArgs e)
        {
            uint addr = (uint)X_LevelUpAddr.Value;
            uint write_addr = AssignLevelUpBaseAddress + (((uint)AddressList.SelectedIndex) * 4);
            Program.Undo.Push("AssignLevelUpBase", write_addr, 4);

            Program.ROM.write_p32(write_addr , addr );
        }
        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            InputFormRef InputFormRef;
            if (PatchUtil.SearchSkillSystem() != PatchUtil.skill_system_enum.SkillSystem)
            {
                return;
            }

            {
                uint iconP = SkillConfigSkillSystemForm.FindIconPointer();
                uint textP = SkillConfigSkillSystemForm.FindTextPointer();
                uint assignClassP = SkillConfigSkillSystemForm.FindAssignClassSkillPointer();
                uint assignLevelUpP = SkillConfigSkillSystemForm.FindAssignClassLevelUpSkillPointer();

                if (iconP == U.NOT_FOUND)
                {
                    return;
                }
                if (textP == U.NOT_FOUND)
                {
                    return;
                }
                if (assignClassP == U.NOT_FOUND)
                {
                    return;
                }
                if (assignLevelUpP == U.NOT_FOUND)
                {
                    return;
                }

                InputFormRef = Init(null, assignClassP);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "SkillAssignmentClassSkillSystem", new uint[] { });

                Dictionary<uint, string> skillNames = new Dictionary<uint,string>();
                InputFormRef N1_InputFormRef = N1_Init(null, skillNames);

                uint assignLevelUpAddr = Program.ROM.p32(assignLevelUpP);
                FEBuilderGBA.Address.AddAddressInstantIFR(list, assignLevelUpP, 4, InputFormRef.DataCount, "SkillAssignmentClassLeveList", new uint[] { 0 });
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
                    string name = "SkillAssignmentClassSkillSystem.Levelup" + i;
                    FEBuilderGBA.Address.AddAddress(list, N1_InputFormRef, name, new uint[] { });
                }
            }
        }

        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            InputFormRef InputFormRef;
            if (PatchUtil.SearchSkillSystem() != PatchUtil.skill_system_enum.SkillSystem)
            {
                return;
            }

            {
                uint iconP = SkillConfigSkillSystemForm.FindIconPointer();
                uint textP = SkillConfigSkillSystemForm.FindTextPointer();
                uint assignClassP = SkillConfigSkillSystemForm.FindAssignClassSkillPointer();
                uint assignLevelUpP = SkillConfigSkillSystemForm.FindAssignClassLevelUpSkillPointer();

                if (iconP == U.NOT_FOUND)
                {
                    return;
                }
                if (textP == U.NOT_FOUND)
                {
                    return;
                }
                if (assignClassP == U.NOT_FOUND)
                {
                    return;
                }
                if (assignLevelUpP == U.NOT_FOUND)
                {
                    return;
                }

                InputFormRef = Init(null, assignClassP);

                Dictionary<uint, string> skillNames = new Dictionary<uint, string>();
                InputFormRef N1_InputFormRef = N1_Init(null, skillNames);

                uint assignLevelUpAddr = Program.ROM.p32(assignLevelUpP);
                for (uint i = 0; i < InputFormRef.DataCount; i++, assignLevelUpAddr += 4)
                {
                    if (!U.isSafetyOffset(assignLevelUpAddr))
                    {
                        errors.Add(new FELint.ErrorSt(FELint.Type.SKILL_CLASS,assignLevelUpAddr,R._("Skillのクラス割り当てが(ClassID: {0})までで、途中で終わってしまいました。",U.To0xHexString(i)),i));
                        break;
                    }

                    uint levelupList = Program.ROM.u32(assignLevelUpAddr);
                    if (levelupList == 0)
                    {//empty
                        continue;
                    }
                    else if (!U.isSafetyPointer(levelupList))
                    {
                        errors.Add(new FELint.ErrorSt(FELint.Type.SKILL_CLASS, assignLevelUpAddr, R._("Skillのクラス割り当て、(ClassID: {0})のデータポインタ({1})が壊れています。\r\nこのクラスをLOADする時に無限ループが発生する可能性があります。\r\n正しいアドレスを入力するか、0に設定してください。", U.To0xHexString(i), U.To0xHexString(levelupList)), i));
                        continue;
                    }

                    N1_InputFormRef.ReInitPointer(assignLevelUpAddr);
                    if (N1_InputFormRef.DataCount >= 20)
                    {
                        errors.Add(new FELint.ErrorSt(FELint.Type.SKILL_CLASS, assignLevelUpAddr, R._("Skillのクラス割り当て、(ClassID: {0})のデータポインタ({1})には、大量の({2})個のデータが登録されています。\r\nアドレスが間違っていませんか？\r\n正しいアドレスを入力するか、0に設定してください。", U.To0xHexString(i), U.To0xHexString(levelupList), N1_InputFormRef.DataCount), i));
                        continue;
                    }
                }
            }
        }

        public static void ExportAllData(string filename)
        {
            InputFormRef InputFormRef;
            if (PatchUtil.SearchSkillSystem() != PatchUtil.skill_system_enum.SkillSystem)
            {
                return;
            }

            List<string> lines = new List<string>();
            {
                uint iconP = SkillConfigSkillSystemForm.FindIconPointer();
                uint textP = SkillConfigSkillSystemForm.FindTextPointer();
                uint assignClassP = SkillConfigSkillSystemForm.FindAssignClassSkillPointer();
                uint assignLevelUpP = SkillConfigSkillSystemForm.FindAssignClassLevelUpSkillPointer();

                if (iconP == U.NOT_FOUND)
                {
                    return;
                }
                if (textP == U.NOT_FOUND)
                {
                    return;
                }
                if (assignClassP == U.NOT_FOUND)
                {
                    return;
                }
                if (assignLevelUpP == U.NOT_FOUND)
                {
                    return;
                }

                InputFormRef = Init(null, assignClassP);

                Dictionary<uint, string> skillNames = new Dictionary<uint, string>();
                InputFormRef N1_InputFormRef = N1_Init(null, skillNames);

                uint classBaseSkillAddr = InputFormRef.BaseAddress;
                uint assignLevelUpAddr = Program.ROM.p32(assignLevelUpP);
                for (uint i = 0; i < InputFormRef.DataCount; 
                    i++, assignLevelUpAddr += 4 , classBaseSkillAddr += 1)
                {
                    if (!U.isSafetyOffset(assignLevelUpAddr))
                    {
                        break;
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.Append(U.ToHexString(Program.ROM.u8(classBaseSkillAddr + 0)));

                    uint levelupList = Program.ROM.p32(assignLevelUpAddr);
                    sb.Append("\t");
                    sb.Append(U.ToHexString(levelupList));
                    if (!U.isSafetyOffset(levelupList))
                    {
                        lines.Add(sb.ToString());
                        continue;
                    }

                    N1_InputFormRef.ReInitPointer(assignLevelUpAddr);
                    uint levelupAddr = N1_InputFormRef.BaseAddress;
                    for (uint n = 0; n < N1_InputFormRef.DataCount; n++, levelupAddr += 2)
                    {
                        sb.Append("\t");
                        sb.Append(U.ToHexString(Program.ROM.u8(levelupAddr + 0)));
                        sb.Append("\t");
                        sb.Append(U.ToHexString(Program.ROM.u8(levelupAddr + 1)));
                    }
                    lines.Add(sb.ToString());
                }
            }
            File.WriteAllLines(filename, lines);
        }
        public static bool ImportAllData(string filename)
        {
            InputFormRef InputFormRef;
            if (PatchUtil.SearchSkillSystem() != PatchUtil.skill_system_enum.SkillSystem)
            {
                return false;
            }

            string[] lines = File.ReadAllLines(filename);
            {
                uint iconP = SkillConfigSkillSystemForm.FindIconPointer();
                uint textP = SkillConfigSkillSystemForm.FindTextPointer();
                uint assignClassP = SkillConfigSkillSystemForm.FindAssignClassSkillPointer();
                uint assignLevelUpP = SkillConfigSkillSystemForm.FindAssignClassLevelUpSkillPointer();

                if (iconP == U.NOT_FOUND)
                {
                    return false;
                }
                if (textP == U.NOT_FOUND)
                {
                    return false;
                }
                if (assignClassP == U.NOT_FOUND)
                {
                    return false;
                }
                if (assignLevelUpP == U.NOT_FOUND)
                {
                    return false;
                }

                InputFormRef = Init(null, assignClassP);

                Dictionary<uint, string> skillNames = new Dictionary<uint, string>();
                InputFormRef N1_InputFormRef = N1_Init(null, skillNames);

                uint classBaseSkillAddr = InputFormRef.BaseAddress;
                uint assignLevelUpAddr = Program.ROM.p32(assignLevelUpP);
                for (uint i = 0; i < InputFormRef.DataCount;
                    i++, assignLevelUpAddr += 4, classBaseSkillAddr += 1)
                {
                    if (!U.isSafetyOffset(assignLevelUpAddr))
                    {
                        break;
                    }
                    if (i >= lines.Length)
                    {
                        break;
                    }
                    string[] sp = lines[i].Split('\t');
                    if (sp.Length < 2)
                    {
                        continue;
                    }
                    {
                        uint skill = U.atoh(sp[0]);
                        Program.ROM.write_u8(classBaseSkillAddr + 0, skill);
                    }

                    uint levelupSkillAddr = U.atoh(sp[1]);
                    if (U.isExtrendsROMArea(levelupSkillAddr) || levelupSkillAddr == 0)
                    {//拡張領域、または0の値が設定されている場合は書き戻す
                        Program.ROM.write_p32(assignLevelUpAddr, levelupSkillAddr);
                        continue;
                    }

                    uint levelupList = Program.ROM.p32(assignLevelUpAddr);
                    if (!U.isSafetyOffset(levelupList))
                    {
                        continue;
                    }

                    N1_InputFormRef.ReInitPointer(assignLevelUpAddr);
                    uint levelupAddr = N1_InputFormRef.BaseAddress;
                    for (uint n = 0; n < N1_InputFormRef.DataCount; n++, levelupAddr += 2)
                    {
                        uint level = U.atoh(U.at(sp, 2 + (n * 2) + 0));
                        uint skill = U.atoh(U.at(sp, 2 + (n * 2) + 1));
                        Program.ROM.write_u8(levelupAddr + 0 , level);
                        Program.ROM.write_u8(levelupAddr + 1 , skill);
                    }
                }
            }
            return true;
        }

        public static int MakeClassSkillButtons(uint cid, Button[] buttons, ToolTipEx tooltip)
        {
            uint iconP = SkillConfigSkillSystemForm.FindIconPointer();
            uint textP = SkillConfigSkillSystemForm.FindTextPointer();
            uint assignClassP = SkillConfigSkillSystemForm.FindAssignClassSkillPointer();
            uint assignLevelUpP = SkillConfigSkillSystemForm.FindAssignClassLevelUpSkillPointer();

            if (iconP == U.NOT_FOUND)
            {
                return 0;
            }
            if (textP == U.NOT_FOUND)
            {
                return 0;
            }
            if (assignClassP == U.NOT_FOUND)
            {
                return 0;
            }
            if (assignLevelUpP == U.NOT_FOUND)
            {
                return 0;
            }

            InputFormRef InputFormRef = Init(null, assignClassP);
            List<U.AddrResult> list = InputFormRef.MakeList();
            if (cid >= list.Count)
            {
                return 0;
            }

            uint classaddr = list[(int)cid].addr;
            if (!U.isSafetyOffset(classaddr))
            {
                return 0;
            }

            uint icon = Program.ROM.p32(iconP);
            uint text = Program.ROM.p32(textP);
            uint assignLevelUp = Program.ROM.p32(assignLevelUpP);

            int skillCount = 0;
            uint b0 = Program.ROM.u8(classaddr);
            if (b0 > 0)
            {//クラスの基本スキル.
                Bitmap bitmap = SkillConfigSkillSystemForm.DrawIcon((uint)b0, icon);
                U.MakeTransparent(bitmap);
                buttons[0].BackgroundImage = bitmap;
                buttons[0].Tag = b0;

                string skillCaption = SkillConfigSkillSystemForm.GetSkillText((uint)b0, text);
                tooltip.SetToolTip(buttons[skillCount], skillCaption);
                skillCount++;
            }

            MakeUnitSkillButtonsList(cid, buttons, tooltip, assignLevelUpP, icon, text, skillCount);
            return skillCount;
        }

        //他のクラスでこのデータを参照しているか?
        bool IsShowIndependencePanel()
        {
            return IsShowIndependencePanel(this.AddressList, this.AssignLevelUpBaseAddress);
        }

        public static bool IsShowIndependencePanel(ListBox addressList,uint assignLevelUpBaseAddress)
        {
            if (addressList.SelectedIndex < 0)
            {
                return false;
            }
            uint classid = (uint)U.atoh(addressList.Text);
            uint setting = assignLevelUpBaseAddress + (classid * 4);
            if (!U.isSafetyOffset(setting))
            {
                return false;
            }
            uint currentP = Program.ROM.p32(setting);
            if (!U.isSafetyOffset(currentP))
            {
                return false;
            }

            uint class_count = (uint)addressList.Items.Count;
            for (uint i = 0; i < class_count; i++)
            {
                if (i == classid)
                {//自分自身
                    continue;
                }
                uint c = assignLevelUpBaseAddress + (uint)(i * 4);
                uint p = Program.ROM.p32(c);

                if (p == currentP)
                {
                    return true;
                }
            }

            return false;
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

            uint setting = this.AssignLevelUpBaseAddress + (classid * 4);
            if (!U.isSafetyOffset(setting))
            {
                return;
            }

            uint p = Program.ROM.p32(setting);
            if (! U.isSafetyOffset(p))
            {
                return;
            }
            if (N1_InputFormRef.BaseAddress != p)
            {
                return;
            }
            if (N1_InputFormRef.DataCount == 0)
            {
                DialogResult dr = R.ShowNoYes("リストが0件です。\r\n空のリストを分離させても意味がないのですが、それでも分離独立させますか？");
                if (dr != DialogResult.Yes)
                {
                    return;
                }
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData(this, this.Name + " Independence");

            uint dataSize = (N1_InputFormRef.DataCount+1) * N1_InputFormRef.BlockSize;
            PatchUtil.WriteIndependence(p, dataSize, setting, name, undodata);
            Program.Undo.Push(undodata);

            InputFormRef.ShowWriteNotifyAnimation(this, p);

            this.ReloadListButton.PerformClick();
            this.InputFormRef.JumpTo(classid);
        }

        private void X_LevelUpAddr_ValueChanged(object sender, EventArgs e)
        {
            uint addr = (uint)X_LevelUpAddr.Value;
            N1_InputFormRef.ReInit(addr);
            ZeroPointerPanel.Visible = InputFormRef.ShowZeroPointerPanel(this.AddressList, this.X_LevelUpAddr);
            if (addr == 0 || U.isSafetyPointer(addr))
            {
                this.X_LevelUPSkillLabel.ErrorMessage = "";
            }
            else
            {
                this.X_LevelUPSkillLabel.ErrorMessage = R._("アドレス「{0}」は無効なアドレスです。", U.To0xHexString(addr));
            }

            //N1の書き込みボタンが反応してしまうときがあるのでやめさせる.
            InputFormRef.WriteButtonToYellow(this.N1_WriteButton, false);
        }

        public static int MakeUnitSkillButtonsList(uint id, Button[] buttons, ToolTipEx tooltip, uint assignLevelUpP, uint icon, uint text, int skillCount)
        {
            uint assignLevelUp = Program.ROM.p32(assignLevelUpP);
            if (!U.isSafetyOffset(assignLevelUp))
            {
                return skillCount;
            }

            uint assignLevelUpAddr = assignLevelUp + (id * 4);
            if (!U.isSafetyOffset(assignLevelUpAddr))
            {
                return skillCount;
            }

            uint levelupList = Program.ROM.p32(assignLevelUpAddr);
            if (!U.isSafetyOffset(levelupList))
            {
                return skillCount;
            }

            //レベルアップで覚えるスキル.
            Dictionary<uint, string> skillNames = new Dictionary<uint, string>();
            InputFormRef N1_InputFormRef = N1_Init(null, skillNames);
            N1_InputFormRef.ReInit(levelupList);

            List<U.AddrResult> levelupSkillList = N1_InputFormRef.MakeList();
            for (int i = 0; i < levelupSkillList.Count; i++)
            {
                uint levelUpSkillAddr = levelupSkillList[i].addr;
                uint level = Program.ROM.u8(levelUpSkillAddr + 0);
                uint skill = Program.ROM.u8(levelUpSkillAddr + 1);

                if (skill <= 0)
                {
                    continue;
                }

                Bitmap bitmap = SkillConfigSkillSystemForm.DrawIcon(skill, icon);
                U.MakeTransparent(bitmap);
                buttons[skillCount].BackgroundImage = bitmap;
                buttons[skillCount].Tag = skill;

                string skillCaption = SkillConfigSkillSystemForm.GetSkillText(skill, text);
                if (level > 0 && level < 0xFF)
                {
                    skillCaption = skillCaption + "\r\n" + R._("(Lv{0}で習得)", level);
                }
                tooltip.SetToolTip(buttons[skillCount], skillCaption);
                skillCount++;
                if (skillCount >= buttons.Length)
                {
                    break;
                }
            }
            return skillCount;
        }

        private void ExportAllButton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("SkillAssignmentClass|*.SkillAssignmentClass.tsv|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            Program.LastSelectedFilename.Load(this, "", save, "");

            DialogResult dr = save.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }

            using (InputFormRef.AutoPleaseWait wait = new InputFormRef.AutoPleaseWait(this))
            {
                ExportAllData(save.FileName);
            }
        }

        private void ImportAllButton_Click(object sender, EventArgs e)
        {
            string title = R._("読込むファイル名を選択してください");
            string filter = R._("SkillAssignmentClass|*.SkillAssignmentClass.tsv|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;

            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (open.FileNames.Length <= 0 || !U.CanReadFileRetry(open.FileNames[0]))
            {
                return;
            }
            string filename = open.FileNames[0];
            Program.LastSelectedFilename.Save(this, "", open);

            using (InputFormRef.AutoPleaseWait wait = new InputFormRef.AutoPleaseWait(this))
            {
                bool r = ImportAllData(filename);
                if (!r)
                {
                    R.ShowStopError("インポートに失敗しました。");
                    return;
                }
            }
            U.ReSelectList(this.AddressList);
            R.ShowOK("データのインポートが完了しました。");
        }

        private void X_LEARNINFO_Click(object sender, EventArgs e)
        {
            string url = "https://dw.ngmansion.xyz/doku.php?id=en:guide_febuildergba_learnskillinfo";
            U.OpenURLOrFile(url);
        }

        bool X_LV_UpdateLock = false;
        private void N1_B0_ValueChanged(object sender, EventArgs e)
        {
            if (X_LV_UpdateLock)
            {
                return;
            }

            X_LV_UpdateLock = true;
            uint lv = (uint)N1_B0.Value;
            if (!UseXLevelAddPanel)
            {
                if (lv == 0xFF)
                {
                    X_LV_255.Show();
                }
                else
                {
                    X_LV_255.Hide();
                }
            }
            else
            {
                if (lv == 0xFF)
                {
                    X_LevelAddPanel.Hide();
                    X_LV_255.Show();
                }
                else
                {
                    X_LV_255.Hide();
                    X_LevelAddPanel.Show();
                }

                uint trueLevel = lv & 0x1f;

                X_LV_Value.Value = trueLevel;
                X_LV_PlayerOnlyCheckBox.Checked = ((lv & 32) == 32) ? true : false;
                X_LV_EnemyOnlyCheckBox.Checked = ((lv & 64) == 64) ? true : false;
                X_LV_NormalHardCheckBox.Checked = ((lv & 96) == 96) ? true : false;
                X_LV_HardOnlyCheckBox.Checked = ((lv & 128) == 128) ? true : false;
            }


            X_LV_UpdateLock = false;
        }

        private void X_LV_PlayerOnlyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (X_LV_UpdateLock)
            {
                return;
            }
            X_LV_UpdateLock = true;
            N1_B0.Value = U.UpdateCheckBitBox(X_LV_PlayerOnlyCheckBox, (uint)N1_B0.Value, 32);
            X_LV_UpdateLock = false;
        }

        private void X_LV_EnemyOnlyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (X_LV_UpdateLock)
            {
                return;
            }
            X_LV_UpdateLock = true;
            N1_B0.Value = U.UpdateCheckBitBox(X_LV_EnemyOnlyCheckBox, (uint)N1_B0.Value, 64);
            X_LV_UpdateLock = false;
        }

        private void X_LV_NormalHardCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (X_LV_UpdateLock)
            {
                return;
            }
            X_LV_UpdateLock = true;
            N1_B0.Value = U.UpdateCheckBitBox(X_LV_NormalHardCheckBox, (uint)N1_B0.Value, 96);
            X_LV_UpdateLock = false;
        }

        private void X_LV_HardOnlyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (X_LV_UpdateLock)
            {
                return;
            }
            X_LV_UpdateLock = true;
            N1_B0.Value = U.UpdateCheckBitBox(X_LV_HardOnlyCheckBox, (uint)N1_B0.Value, 128);
            X_LV_UpdateLock = false;
        }

        private void X_LV_Value_ValueChanged(object sender, EventArgs e)
        {
            if (X_LV_UpdateLock)
            {
                return;
            }
            X_LV_UpdateLock = true;
            uint lv = (uint)N1_B0.Value;
            uint trueLevel = (uint)X_LV_Value.Value;
            lv = lv - (lv & 0x1f);
            lv += trueLevel;

            N1_B0.Value = lv;
            X_LV_UpdateLock = false;
        }

    }
}
