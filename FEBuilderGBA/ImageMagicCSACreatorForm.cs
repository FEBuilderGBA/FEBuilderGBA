﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FEBuilderGBA
{
    public partial class ImageMagicCSACreatorForm : Form
    {
        public ImageMagicCSACreatorForm()
        {
            InitializeComponent();
            U.SetIcon(MagicAnimeExportButton, Properties.Resources.icon_arrow);
            U.SetIcon(MagicAnimeImportButton, Properties.Resources.icon_upload);

            if (ImageUtilMagic.SearchMagicSystem(out MagicEngineBaseAddr, out DimAddr, out NoDimAddr) != ImageUtilMagic.magic_system_enum.CSA_CREATOR)
            {
                Debug.Assert(false);
                return;
            }

            InputFormRef.markupJumpLabel(LinkInternt);

            U.SelectedIndexSafety(ShowZoomComboBox, 0);
            Dictionary<uint, string> effectDic = U.LoadDicResource(U.ConfigDataFilename("item_anime_effect_"));
            uint spellDataCount = ImageUtilMagicFEditor.SpellDataCount();
            uint csaSpellTablePointer;
            uint csaSpellTable = ImageUtilMagic.FindCSASpellTable("SCA_Creator", out csaSpellTablePointer);
            if (csaSpellTable == U.NOT_FOUND)
            {
//                uint csaSpellTablePointer = ImageUtilMagic.FindCSASpellTable("SCA_Creator",true);
                R.ShowWarning("魔法リストの拡張がされていません。\r\nリストの拡張を選択して、魔法リストを増やしてください。");
            }

            this.InputFormRef = Init(this, DimAddr, NoDimAddr, spellDataCount, csaSpellTable, effectDic);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.InputFormRef.IsMemoryNotContinuous = true; //メモリは連続していないので、警告不能.
            if (this.InputFormRef.DataCount > Program.ROM.RomInfo.magic_effect_original_data_count() + 2)
            {//拡張済みなので、拡張ボタンを無効にする
                MagicListExpandsButton.Enabled = false;
            }
        }
        uint MagicEngineBaseAddr, DimAddr, NoDimAddr;
        
        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self,uint dimAddr,uint no_dimAddr, uint spellDataCount, uint csaSpellTable, Dictionary<uint, string> effectDic)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.magic_effect_pointer()
                , 4
                , (int i, uint addr) =>
                {//読込最大値検索
                    if (csaSpellTable == U.NOT_FOUND)
                    {
                        return false;
                    }
                    if (i >= spellDataCount )
                    {
                        return false;
                    }
                    if (i >= 0xFE )
                    {
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    uint baseaddress = Program.ROM.p32(Program.ROM.RomInfo.magic_effect_pointer());
                    uint csaaddress = (uint)(csaSpellTable + (20 * i));

                    string effectname = U.at(effectDic, i);
                    if (effectname == "")
                    {
                        effectname = Program.CommentCache.At(csaaddress);
                    }
                    string name = U.ToHexString(i) + " " + effectname;

                    uint dataaddr = Program.ROM.p32(addr);
                    if (dataaddr == 0)
                    {
                        if (i < Program.ROM.RomInfo.magic_effect_original_data_count())
                        {//もともとのデータ
                            return new U.AddrResult();
                        }
                        else
                        {
                            return new U.AddrResult(csaaddress, name + " EMPTY", addr);
                        }
                    }
                    
                    if (
                        dataaddr == dimAddr
                     || dataaddr == no_dimAddr)
                    {
                        return new U.AddrResult(csaaddress, name, addr);
                    }
                    //もともとのデータ
                    return new U.AddrResult();
                }
                );
        }

        private void ImageMagicForm_Load(object sender, EventArgs e)
        {
            if (ImageUtilMagic.SearchMagicSystem() != ImageUtilMagic.magic_system_enum.CSA_CREATOR)
            {
                this.Close();
            }

            uint csaSpellTablePointer;
            uint csaSpellTable = ImageUtilMagic.FindCSASpellTable("SCA_Creator", out csaSpellTablePointer);
            if (csaSpellTable == U.NOT_FOUND)
            {
                this.MagicListExpandsButton.PerformClick();
            }
            U.AllowDropFilename(this, new string[] { ".TXT" }, (string filename) =>
            {
                using (ImageFormRef.AutoDrag ad = new ImageFormRef.AutoDrag(filename))
                {
                    MagicAnimeImportButton_Click(null, null);
                }
            });
        }

        void DrawSelectedAnime()
        {
            X_B_ANIME_PIC2.Image = ImageUtilMagicCSACreator.Draw((uint)ShowFrameUpDown.Value, (uint)P0.Value, (uint)P4.Value, (uint)P12.Value);
        }
        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowFrameUpDown.Value = 0;

            U.AddrResult ar = InputFormRef.SelectToAddrResult(this.AddressList);
            uint dim = Program.ROM.p32(ar.tag);
            if ( dim == this.DimAddr )
            {
                DimComboBox.SelectedIndex = 0;
            }
            else if (dim == this.NoDimAddr)
            {
                DimComboBox.SelectedIndex = 1;
            }
            else
            {
                DimComboBox.SelectedIndex = 2;
            }

            this.MagicComment.Text = Program.CommentCache.At(ar.addr);
            DrawSelectedAnime();
        }
        private void ShowFrameUpDown_ValueChanged(object sender, EventArgs e)
        {
            DrawSelectedAnime();
        }

        private void MagicAnimeExportButton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("魔法アニメ コメントあり|*.txt|魔法アニメ コメントなし|*.txt|アニメGIF|*.gif|Dump All|*.txt|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.AddExtension = true;
            Program.LastSelectedFilename.Load(this, "", save, "magic_" + this.AddressList.Text.Trim());

            DialogResult dr = save.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            string filename = save.FileNames[0];
            Program.LastSelectedFilename.Save(this, "", save);

            if (save.FilterIndex == 2)
            {//コメントなし
                bool enableComment = false;
                ImageUtilMagicCSACreator.Export(enableComment, filename, (uint)P0.Value, (uint)P4.Value, (uint)P12.Value);
            }
            else if (save.FilterIndex == 3)
            {//GIF
                ImageUtilMagicCSACreator.ExportGif(filename, (uint)P0.Value, (uint)P4.Value, (uint)P12.Value);
            }
            else if (save.FilterIndex == 4)
            {//All
                bool enableComment = false;
                ImageUtilMagicCSACreator.Export(enableComment, filename, (uint)P0.Value, (uint)P4.Value, (uint)P12.Value);
                filename = U.ChangeExtFilename(filename, ".gif");
                ImageUtilMagicCSACreator.ExportGif(filename, (uint)P0.Value, (uint)P4.Value, (uint)P12.Value);
            }
            else
            {//コメントあり
                bool enableComment = true;
                ImageUtilMagicCSACreator.Export(enableComment, filename, (uint)P0.Value, (uint)P4.Value, (uint)P12.Value);
            }

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(filename);
        }

        private void MagicAnimeImportButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }

            string filename;
            if (ImageFormRef.GetDragFilePath(out filename))
            {
            }
            else
            {
                string title = R._("開くファイル名を選択してください");
                string filter = R._("TEXT|*.txt|All files|*");

                OpenFileDialog open = new OpenFileDialog();
                open.Title = title;
                open.Filter = filter;
                Program.LastSelectedFilename.Load(this, "", open);
                DialogResult dr = open.ShowDialog();
                if (dr != DialogResult.OK)
                {
                    return;
                }
                if (!U.CanReadFileRetry(open))
                {
                    return;
                }
                Program.LastSelectedFilename.Save(this, "", open);
                filename = open.FileName;
            }

            //インポート実行
            uint id = (uint)AddressList.SelectedIndex + 1;
            string error = MagicAnimeImportDirect(id, filename);
            if (error != "")
            {
                R.ShowStopError(error);
                return;
            }
        }

        public string MagicAnimeImportDirect(uint id, string filename)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return R._("現在他の処理中です");
            }

            if (id <= 0)
            {
                return R._("指定されたID({0})は存在しません。", U.To0xHexString(id));
            }

            uint magic_baseaddress = InputFormRef.SelectToAddr(AddressList, (int)id - 1);
            if (magic_baseaddress == U.NOT_FOUND)
            {
                return R._("指定されたID({0})は存在しません。", U.To0xHexString(id));
            }

            string error = "";

            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                string ext = U.GetFilenameExt(filename);
                error = ImageUtilMagicCSACreator.Import(filename, magic_baseaddress);
            }

            if (error != "")
            {
                return error;
            }

            if (DimComboBox.SelectedIndex >= 2)
            {//EMPTYになっている場合は、dim_pcを選択.
                DimComboBox.SelectedIndex = 0;
                WriteDim();
            }

            U.ReSelectList(AddressList);
            InputFormRef.ShowWriteNotifyAnimation(this, magic_baseaddress);

            return "";
        }

        private void ShowZoomComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ShowZoomComboBox.SelectedIndex == 0)
            {
                X_B_ANIME_PIC2.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                X_B_ANIME_PIC2.SizeMode = PictureBoxSizeMode.Normal;
            }
        }

        private void N_WriteButton_Click(object sender, EventArgs e)
        {
            WriteDim();
            WriteMagicName();
        }

        void WriteDim()
        {
            if (this.AddressList.SelectedIndex < 0)
            {
                return;
            }

            U.AddrResult ar = InputFormRef.SelectToAddrResult(this.AddressList);
            Debug.Assert(ar.tag != 0);
            if (!U.CheckZeroAddressWriteHigh(ar.tag))
            {
                return;
            }
            if (!U.CheckPaddingALIGN4(ar.tag))
            {
                return;
            }

            if (DimComboBox.SelectedIndex == 0)
            {//dim
                Program.ROM.write_p32(ar.tag, this.DimAddr);
            }
            else if (DimComboBox.SelectedIndex == 1)
            {//no dim
                Program.ROM.write_p32(ar.tag, this.NoDimAddr);
            }
            else if (DimComboBox.SelectedIndex == 2)
            {//empty
                Program.ROM.write_u32(ar.tag, 0);
            }
        }
        void WriteMagicName()
        {
            uint newaddr = (uint)this.Address.Value;
            Program.CommentCache.Update(newaddr, this.MagicComment.Text);
        }

        public static Dictionary<uint, string> MakeItemEffectAndAppendMagic(Form from)
        {
            InputFormRef InputFormRef;
            Dictionary<uint, string> effectDic = U.LoadDicResource(U.ConfigDataFilename("item_anime_effect_"));

            uint baseaddr, dimaddr, no_dimaddr;
            if (ImageUtilMagic.SearchMagicSystem(out baseaddr, out dimaddr, out no_dimaddr) != ImageUtilMagic.magic_system_enum.CSA_CREATOR)
            {
                return effectDic;
            }

            uint spellDataCount = ImageUtilMagicFEditor.SpellDataCount();
            uint csaSpellTablePointer;
            uint csaSpellTable = ImageUtilMagic.FindCSASpellTable("SCA_Creator", out csaSpellTablePointer);

            InputFormRef = Init(from, dimaddr, no_dimaddr, spellDataCount, csaSpellTable, effectDic);
            List<U.AddrResult> ret = InputFormRef.MakeList();
            for (int i = 0; i < ret.Count; i++)
            {
                U.AddrResult ar = ret[i];
                if (ar.name.IndexOf(" EMPTY") >= 0)
                {
                    continue;
                }

                //処理効率最悪なのだが、 テキストの先頭に16進数でIDが入っている
                uint no = U.atoh(ar.name);
                string effectname = R._("追加魔法");
                if (Program.CommentCache.CheckFast(ar.addr))
                {
                    effectname = " " + Program.CommentCache.At(ar.addr);
                }

                effectDic[no] = effectname;
            }
            U.OrderBy(effectDic, (x) => { return (int)x.Key; });
            return effectDic;
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list,bool isPointerOnly)
        {
            string name;
            InputFormRef InputFormRef;
            uint baseaddr, dimaddr, no_dimaddr;
            if (ImageUtilMagic.SearchMagicSystem(out baseaddr, out dimaddr, out no_dimaddr) != ImageUtilMagic.magic_system_enum.CSA_CREATOR)
            {
                return ;
            }

            {
                uint spellDataCount = ImageUtilMagicFEditor.SpellDataCount();
                uint csaSpellTablePointer;
                uint csaSpellTable = ImageUtilMagic.FindCSASpellTable("SCA_Creator", out csaSpellTablePointer);
                Dictionary<uint, string> effectDic = new Dictionary<uint, string>();
                InputFormRef = Init(null,dimaddr,no_dimaddr, spellDataCount, csaSpellTable, effectDic);

                name = "Magic";
                FEBuilderGBA.Address.AddAddress(list
                    , InputFormRef
                    , name,new uint[]{0}
                    );

                //追加魔法テーブル(結構無駄な構造ですが、仕方ない)
                FEBuilderGBA.Address.AddAddress(list
                    , csaSpellTable
                    , InputFormRef.DataCount * 4 * 5
                    , csaSpellTablePointer
                    , "Magic_Append_SpellTable"
                    , FEBuilderGBA.Address.DataTypeEnum.MAGIC_APPEND_SPELLTABLE
                    );

                uint addr = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++ , addr += InputFormRef.BlockSize)
                {
                    uint baseaddress = Program.ROM.p32(Program.ROM.RomInfo.magic_effect_pointer());
                    uint csaaddress = (uint)(csaSpellTable + (20 * i));

                    uint dataaddr = Program.ROM.p32(addr);
                    if (dataaddr == 0)
                    {
                        continue;
                    }
                    if (
                        dataaddr == dimaddr
                     || dataaddr == no_dimaddr)
                    {
                        name = "Magic:" + U.To0xHexString(i);
                        ImageUtilMagicCSACreator.RecycleOldAnime(ref list, name, isPointerOnly, csaaddress);
                    }

                }
            }
        }
        //エラー検出
        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            string name;
            InputFormRef InputFormRef;
            uint baseaddr, dimaddr, no_dimaddr;
            if (ImageUtilMagic.SearchMagicSystem(out baseaddr, out dimaddr, out no_dimaddr) != ImageUtilMagic.magic_system_enum.CSA_CREATOR)
            {
                return;
            }

            {
                uint spellDataCount = ImageUtilMagicFEditor.SpellDataCount();
                uint csaSpellTablePointer;
                uint csaSpellTable = ImageUtilMagic.FindCSASpellTable("SCA_Creator", out csaSpellTablePointer);
                Dictionary<uint, string> effectDic = new Dictionary<uint, string>();
                InputFormRef = Init(null, dimaddr, no_dimaddr, spellDataCount, csaSpellTable, effectDic);

                uint addr = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, addr += InputFormRef.BlockSize)
                {
                    uint baseaddress = Program.ROM.p32(Program.ROM.RomInfo.magic_effect_pointer());
                    uint csaaddress = (uint)(csaSpellTable + (20 * i));

                    uint dataaddr = Program.ROM.p32(addr);
                    if (dataaddr == 0)
                    {
                        continue;
                    }
                    if (
                        dataaddr == dimaddr
                     || dataaddr == no_dimaddr)
                    {
                        name = "Magic:" + U.To0xHexString(i);
                        ImageUtilMagicCSACreator.MakeCheckError(ref errors, name, csaaddress, (uint)i);
                    }

                }
            }
        }

        private void MagicListExpandsButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = R.ShowYesNo("魔法テーブルを拡張してもよろしいですか？");
            if (dr != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"expands");

            uint csaSpellTablePointer;
            uint csaSpellTable = ImageUtilMagic.FindCSASpellTable("SCA_Creator", out csaSpellTablePointer);
            if (csaSpellTablePointer == U.NOT_FOUND)
            {
                R.ShowStopError("CSASpellTable Not Found.");
                return;
            }

            //魔法エフェクトテーブルの拡張.
            uint spellDataCount = ImageUtilMagicFEditor.SpellDataCount();
            uint datasize = spellDataCount;
            InputFormRef.ExpandsArea(this, 254, Program.ROM.RomInfo.magic_effect_pointer(), datasize,FEBuilderGBA.InputFormRef.ExpandsFillOption.NO,4, undodata);

            //CSA追加魔法テーブルの拡張
            if (U.isSafetyOffset(csaSpellTablePointer))
            {//CSA追加魔法テーブルが正しくセットされている場合
                datasize = InputFormRef.DataCount;
            }
            else
            {//セットされていなければ初期値は0
                datasize = 0;
            }

            csaSpellTable = InputFormRef.ExpandsArea(this, 254, csaSpellTablePointer, datasize,FEBuilderGBA.InputFormRef.ExpandsFillOption.NO,5 * 4, undodata);
            Program.Undo.Push(undodata);

            //開きなおす.
            InputFormRef.ReOpenForm<ImageMagicCSACreatorForm>();
        }

        private void X_N_JumpEditor_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }

            uint ID = (uint)AddressList.SelectedIndex + 1;

            string filehint = this.AddressList.Text;

            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            //テンポラリディレクトリを利用する
            using (U.MakeTempDirectory tempdir = new U.MakeTempDirectory())
            {
                string filename = Path.Combine(tempdir.Dir, "anime.txt");
                ImageUtilMagicCSACreator.Export(false, filename, (uint)P0.Value, (uint)P4.Value, (uint)P12.Value);
                if (!File.Exists(filename))
                {
                    R.ShowStopError("アニメーションエディタを表示するために、アニメーションをエクスポートしようとしましたが、アニメをファイルにエクスポートできませんでした。\r\n\r\nファイル:{0}", filename);
                    return;
                }

                ToolAnimationCreatorForm f = (ToolAnimationCreatorForm)InputFormRef.JumpFormLow<ToolAnimationCreatorForm>();
                f.Init(ToolAnimationCreatorUserControl.AnimationTypeEnum.MagicAnime_CSACreator
                    ,ID, filehint, filename);
                f.Show();
            }
        }

        private void LinkInternt_Click(object sender, EventArgs e)
        {
            MainFormUtil.GotoMoreData();
        }

    
    }
}
