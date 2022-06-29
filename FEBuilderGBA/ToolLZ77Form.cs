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
            Program.ROM.write_fill(from, size);
            Program.Undo.Push(undodata);
            InputFormRef.ShowWriteNotifyAnimation(this, 0);
        }

        private void MoveButton_Click(object sender, EventArgs e)
        {
            uint moveAddr = U.toOffset((uint)MoveFromAddress.Value);
            uint toAddr = U.toOffset((uint)MoveToAddress.Value);
            if (!U.isSafetyOffset(moveAddr))
            {
                R.ShowStopError("FROMに指定されたアドレスがROMの範囲外です");
                return ;
            }
            if (!U.isSafetyOffset(toAddr))
            {
                R.ShowStopError("TOに指定されたアドレスがROMの範囲外です");
                return ;
            }

            uint length = (uint)MoveLength.Value;
            if (length == 0)
            {//自動推測できたらいいなあ
                return;
            }
            DialogResult dr = R.ShowNoYes("{0}から{1}までの領域({2} bytes)を{3}に移動してもよろしいですか？", U.To0xHexString(moveAddr), U.To0xHexString(moveAddr + length), length, U.To0xHexString(toAddr));
            if (dr != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData(R._("MoveAddress {0}-{1} => {2} ({3}size)", U.To0xHexString(moveAddr),U.To0xHexString(moveAddr + length), U.To0xHexString(toAddr), length));
            uint r = InputFormRef.MoveBinaryData(this, moveAddr, toAddr, length, undodata);
            if (r == U.NOT_FOUND)
            {
                return;
            }
            Program.Undo.Push(undodata);
        }

    }
}
