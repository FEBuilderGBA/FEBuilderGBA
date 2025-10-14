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
    public partial class ToolLZ77Form : Form
    {
        const string THIS_ROM = "<<THIS ROM>>";

        public ToolLZ77Form()
        {
            InitializeComponent();

            DeCompressSRCFilename.Text = THIS_ROM;
            DeCompressSRCFilename.AllowDropFilename();
            DeCompressDESTFilename.AllowDropFilename();
            CompressSRCFilename.AllowDropFilename();
            CompressDESTFilename.AllowDropFilename();
        }

        private void DeCompressSRCSelectButton_Click(object sender, EventArgs e)
        {
            string title = R._("開くファイル名を選択してください");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            Program.LastSelectedFilename.Load(this, "", open);
            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                DeCompressSRCFilename.Text = THIS_ROM;
                return;
            }
            if (!U.CanReadFileRetry(open))
            {
                DeCompressSRCFilename.Text =THIS_ROM;
                return ;
            }

            Program.LastSelectedFilename.Save(this, "", open);
            DeCompressSRCFilename.Text = open.FileNames[0];
        }

        private void DeCompressDESTSelectButton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            DialogResult dr = save.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return ;
            }
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return ;
            }
            Program.LastSelectedFilename.Save(this, "", save);
            DeCompressDESTFilename.Text = save.FileNames[0];
        }

        private void CompressSRCSelectButton_Click(object sender, EventArgs e)
        {
            string title = R._("開くファイル名を選択してください");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (!U.CanReadFileRetry(open))
            {
                return;
            }

            CompressSRCFilename.Text = open.FileNames[0];
        }

        private void CompressDESTSelectButton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            DialogResult dr = save.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            Program.LastSelectedFilename.Save(this, "", save);
            CompressDESTFilename.Text = save.FileNames[0];
        }

        private void LZ77ToolForm_Load(object sender, EventArgs e)
        {

        }

        private void DeCompressSRCFilename_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DeCompressSRCSelectButton.PerformClick();
        }

        private void DeCompressDESTFilename_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DeCompressDESTSelectButton.PerformClick();
        }

        private void CompressSRCFilename_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            CompressSRCSelectButton.PerformClick();
        }

        private void CompressDESTFilename_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            CompressDESTSelectButton.PerformClick();
        }

        private void DeCompressFireButton_Click(object sender, EventArgs e)
        {
            if (DeCompressSRCFilename.Text == "" || DeCompressDESTFilename.Text == "")
            {
                return;
            }

            uint address = U.toOffset(DeCompressAddress.Value);

            byte[] data;
            if (DeCompressSRCFilename.Text == THIS_ROM)
            {
                if (Program.ROM.Data.Length < (uint)address)
                {
                    return;
                }
                data = LZ77.decompress(Program.ROM.Data, address);
            }
            else
            {
                byte[] src = File.ReadAllBytes(DeCompressSRCFilename.Text);
                if (src.Length < (uint)address)
                {
                    return;
                }
                data = LZ77.decompress(src, address);
            }

            U.WriteAllBytes(DeCompressDESTFilename.Text,data);

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(DeCompressDESTFilename.Text);
        }

        private void CompressFireButton_Click(object sender, EventArgs e)
        {
            if (CompressSRCFilename.Text == "" || CompressDESTFilename.Text == "")
            {
                return;
            }

            byte[] src = File.ReadAllBytes(CompressSRCFilename.Text);
            byte[] data = LZ77.compress(src);

            U.WriteAllBytes(CompressDESTFilename.Text, data);
        
            //エクスプローラで選択しよう
            U.SelectFileByExplorer(CompressDESTFilename.Text);
        }

        private void ZeroClearButton_Click(object sender, EventArgs e)
        {
            uint from = U.toOffset( (uint)ZeroClearFrom.Value);
            uint to = U.toOffset( (uint)ZeroClearTo.Value);

            if (to < from)
            {
                U.Swap<uint>(ref from, ref to);
            }
            uint size = to - from;

            if (!U.CheckZeroAddressWriteHigh(from))
            {
                return;
            }
            if (!U.CheckZeroAddressWriteHigh(to))
            {
                return;
            }
            DialogResult dr = R.ShowNoYes("{0}から{1}までの領域({2} bytes)をゼロクリアしてもよろしいですか？", U.To0xHexString(from) , U.To0xHexString(to) , size);
            if (dr != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }
            Undo.UndoData undodata = Program.Undo.NewUndoData(R._("ZeroClear {0}-{1} ({2}size)", U.To0xHexString(from), U.To0xHexString(to), size));
            Program.ROM.write_fill(from, size, 0 , undodata);
            Program.Undo.Push(undodata);
            InputFormRef.ShowWriteNotifyAnimation(this, 0);
        }

        private void MoveButton_Click(object sender, EventArgs e)
        {
            uint moveAddr = U.toOffset((uint)MoveFromAddress.Value);
            uint toAddr = U.toOffset((uint)MoveToAddress.Value);
            uint length = (uint)MoveLength.Value;
            if (length == 0)
            {//自動推測できたらいいなあ
                return;
            }

            if (!U.isSafetyOffset(moveAddr) || !U.isSafetyOffset(moveAddr + length))
            {
                R.ShowStopError("FROMに指定されたアドレスがROMの範囲外です");
                return ;
            }
            if (toAddr != 0)
            {
                if (!U.isSafetyOffset(toAddr) || !U.isSafetyOffset(toAddr + length))
                {
                    R.ShowStopError("TOに指定されたアドレスがROMの範囲外です");
                    return;
                }
            }
            DialogResult dr;

            {
                byte[] moveBytes = Program.ROM.getBinaryData(moveAddr, length);
                if (U.IsEmptyRange(moveBytes))
                {
                    R.ShowStopError("FROMに指定された{0}のコンテンツが全部nullです。\r\n既に移動済みの利用域をしていると思いますので、処理を停止します。", U.To0xHexString(moveAddr));
                    return;
                }
                if (toAddr != 0)
                {
                    byte[] toBytes = Program.ROM.getBinaryData(toAddr, length);
                    if (!U.IsEmptyRange(toBytes))
                    {
                        dr = R.ShowNoYes("TOに指定された{0}のコンテンツが全部nullではありません。\r\n既に別のデータで利用しているかもしれません。続行してもよろしいですか？", U.To0xHexString(toAddr));
                        if (dr != DialogResult.Yes)
                        {
                            return;
                        }
                    }
                }
            }

            dr = R.ShowNoYes("{0}から{1}までの領域({2} bytes)を{3}に移動してもよろしいですか？", U.To0xHexString(moveAddr), U.To0xHexString(moveAddr + length), length, U.To0xHexString(toAddr));
            if (dr != DialogResult.Yes)
            {
                return;
            }


            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                List<uint> movepointerlist = MoveToFreeSapceForm.SearchPointer(moveAddr);
                if (movepointerlist.Count != 1)
                {
                    dr = R.ShowNoYes("この移動で{0}か所のポインタの置換が行われます。\r\n本当に継続してもよろしいですか？", movepointerlist.Count);
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                }
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData(R._("MoveAddress {0}-{1} => {2} ({3}size)", U.To0xHexString(moveAddr),U.To0xHexString(moveAddr + length), U.To0xHexString(toAddr), length));
            uint r = InputFormRef.MoveBinaryData(this, moveAddr, toAddr, length, undodata);
            if (r == U.NOT_FOUND)
            {
                return;
            }
            Program.Undo.Push(undodata);
        }

        private void ReCompressButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }
            if (Program.ROM.Modified)
            {
                R.ShowYesNo("編集中のデータがあります。\r\n危険なので、一度ROMを保存してから再度試してください。\r\n");
                return;
            }

            DialogResult dr = R.ShowNoYes("lz77再圧縮を実行してもよろしいですか?");
            if (dr != DialogResult.Yes)
            {
                return;
            }

            uint totalSize = 0;
            uint totalCount = 0;
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                List<Address> list = SearchAllLZ77Data(pleaseWait);
                pleaseWait.DoEvents("...");

                Undo.UndoData undodata = Program.Undo.NewUndoData("ReCompressLZ77");
                foreach (Address a in list)
                {
                    pleaseWait.DoEvents(R._("{0} 削減サイズ:{1}", U.To0xHexString(a.Addr), totalSize));
                    uint diff = RecompressOne(a, undodata);
                    if (diff == 0)
                    {
                        pleaseWait.DoEvents();
                        continue;
                    }
                    totalSize += diff;
                    totalCount++;
                }
                Program.Undo.Push(undodata);
            }

            if (totalSize == 0)
            {
                R.ShowOK("再圧縮しても容量を増やすことができませんでした。\r\nすべてのデータは新しいルーチンで圧縮されているものだと思われます。");
            }
            else
            {
                R.ShowOK("{0}個のデータを再圧縮し、{1}バイトの追加の余白を得ました。\r\nこの余白を最大限利用するには、動作テストの後にrebuildを実行してください。", totalCount, totalSize);
            }
        }

        List<Address> SearchAllLZ77Data(InputFormRef.AutoPleaseWait wait)
        {
            //念のためパッチのCheckIFをスキャンをやり直す.
            PatchForm.ClearCheckIF();

            wait.DoEvents("GrepAllStructPointers");
            List<DisassemblerTrumb.LDRPointer> ldrmap = DisassemblerTrumb.MakeLDRMap(Program.ROM.Data, 0x100, Program.ROM.RomInfo.compress_image_borderline_address, true);
            List<Address> structList = U.MakeAllStructPointersList(false);
            U.AppendAllASMStructPointersList(structList
                , ldrmap
                , isPatchInstallOnly: true
                , isPatchPointerOnly: false
                , isPatchStructOnly: false
                , isUseOtherGraphics: false
                , isUseOAMSP: false
                );
            AsmMapFile.InvalidateUNUNSED(structList);

            Dictionary<uint, bool> dupCheck = new Dictionary<uint, bool>();

            List<Address> ret = new List<Address>();
            foreach (Address a in structList)
            {
                if (! Address.IsLZ77(a.DataType))
                {
                    continue;
                }
                if (dupCheck.ContainsKey(a.Addr))
                {
                    continue;
                }
                ret.Add(a);
                dupCheck[a.Addr] = true;
            }
            ret.Sort((aa, bb) => { return (int)(aa.Addr - bb.Addr); });

            return ret;
        }

        uint RecompressOne(Address a, Undo.UndoData undodata)
        {
            if (!Address.IsLZ77(a.DataType))
            {//念のためlz77圧縮の確認
                return 0;
            }
            if (!U.isPadding4(a.Addr))
            {//4バイトパディングされていないデータはありえない
                return 0;
            }

            byte[] unzipData = LZ77.decompress(Program.ROM.Data, a.Addr);
            if (unzipData.Length <= 0)
            {
                return 0;
            }
            uint uncompressSize = LZ77.getUncompressSize(Program.ROM.Data, a.Addr);
            if (unzipData.Length != uncompressSize)
            {
                return 0;
            }

            byte[] lz77Data = LZ77.compress(unzipData);
            if (lz77Data.Length >= a.Length)
            {//無意味なのでやらない
                return 0;
            }
            uint diff = (uint)(a.Length - lz77Data.Length);
            if (diff <= 3)
            {//無意味なのでやらない
                return 0;
            }
            //書き込み
            Program.ROM.write_fill(a.Addr, a.Length, 0, undodata);
            Program.ROM.write_range(a.Addr, lz77Data, undodata);

            return diff;
        }

        private void CompressPortraitButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {
                return;
            }
            ImagePortraitForm.Execute_ToolCompressAllPortrait();
        }
        
        private void OptimizationBattleAnimationOAMButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {
                return;
            }
            ImageBattleAnimeForm.Execute_ToolAutoGenLeftToRightAllAnimation();
        }

        private void Base64TextToFileButton_Click(object sender, EventArgs e)
        {
            string text = Base64RichTextEdit.Text;
            if (text == "")
            {
                R.ShowStopError("Textにbase64のデータを入力してください");
                return;
            }
            byte[] bin;
            text = text.Trim();
            text = text.Replace(' ', '+');
            if (!U.Base64Encode(text, out bin))
            {
                R.ShowStopError("Base64を復号できませんでした");
                return;
            }

            string title = R._("保存するファイル名を選択してください。");
            string filter = R._("All files|*");
            string ext = U.GuessExtension(bin);

            SaveFileDialog save = new SaveFileDialog();
            if (ext != "")
            {
                save.FileName = "foo" + ext;
            }
            save.Title = title;
            save.Filter = filter;
            save.ShowDialog();
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            File.WriteAllBytes(save.FileName, bin);
            U.SelectFileByExplorer(save.FileName);
        }

        private void FileToBase64TextButton_Click(object sender, EventArgs e)
        {
            string title = R._("開くファイル名を選択してください");
            string filter = R._("All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            open.ShowDialog();
            if (open.FileNames.Length <= 0 || !U.CanReadFileRetry(open.FileNames[0]))
            {
                return;
            }
            byte[] bin = File.ReadAllBytes(open.FileName);
            string text = System.Convert.ToBase64String(bin);
            if (text == "")
            {
                R.ShowStopError("base64に変換できませんでした。");
                return;
            }
            Base64RichTextEdit.Text = text;
        }

        private void OptimizationSongGotoFineButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {
                return;
            }
            SongTableForm.Execute_ToolOptimizationSongGotoFine();
        }

        private void Base64TextToEmulatorButton_Click(object sender, EventArgs e)
        {
            string text = Base64DirectEmulatorRichTextEdit.Text;
            if (text == "")
            {
                R.ShowStopError("Textにbase64のデータを入力してください");
                return;
            }
            byte[] bin;
            text = text.Trim();
            text = text.Replace(' ', '+');
            if (!U.Base64Encode(text, out bin))
            {
                R.ShowStopError("Base64を復号できませんでした");
                return;
            }

            using (U.MakeTempDirectory tempdir = new U.MakeTempDirectory())
            {
                string save7zfile = Path.Combine(tempdir.Dir, "foo.7z");
                File.WriteAllBytes(save7zfile, bin);

                ArchSevenZip.Extract(save7zfile, tempdir.Dir, isHide: true);
                string[] files = U.Directory_GetFiles_Safe(tempdir.Dir, "*.sav", SearchOption.AllDirectories);
                if (files.Length < 1)
                {
                    files = U.Directory_GetFiles_Safe(tempdir.Dir, "*.sa1", SearchOption.AllDirectories);
                    if (files.Length < 1)
                    {
                        R.ShowStopError("Base64を復号しましたが、7zファイルからsavを回収できませんでした。");
                        return;
                    }
                }
                
                string targetsavFile = U.GetEmulatorSavFile(".emulator.sav");
                File.Copy(files[0], targetsavFile , true);
            }

            MainFormUtil.RunAs("emulator");
        }
    }
}
