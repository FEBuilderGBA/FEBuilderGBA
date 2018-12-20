using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace FEBuilderGBA
{
    public partial class DecreaseColorTSAToolForm : Form
    {
        public DecreaseColorTSAToolForm()
        {
            InitializeComponent();

            //背景を選択.
            this.Method.SelectedIndex = 1;
            this.ConvertSizeMethod.SelectedIndex = 1;

            AFilename.AllowDropFilename();
            BFilename.AllowDropFilename();

            U.AllowDropFilename(this, ImageFormRef.IMAGE_FILE_FILTER, (string filename) =>
            {
                AFilename.Text = filename;
            });
        }

        public void InitMethod(int i)
        {
            U.SelectedIndexSafety(this.Method, i);
        }

        private void DecreaseColorTSAToolForm_Load(object sender, EventArgs e)
        {

        }

        private void Method_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Method.SelectedIndex == 1)
            {//背景とCG
                U.ForceUpdate(this.ConvertWidth,30 * 8);
                U.ForceUpdate(this.ConvertHeight, 20 * 8);
                U.ForceUpdate(this.ConvertYohaku,  2 * 8);
                U.ForceUpdate(this.ConvertPaletteNo, 8);
                U.ForceUpdate(this.ConvertReserveColor, 1);
                IgnoreTSA.Checked = false;
            }
            else if (Method.SelectedIndex == 2)
            {//戦闘背景
                U.ForceUpdate(this.ConvertWidth, 30 * 8);
                U.ForceUpdate(this.ConvertHeight, 20 * 8);
                U.ForceUpdate(this.ConvertYohaku, 0);
                U.ForceUpdate(this.ConvertPaletteNo, 8);
                U.ForceUpdate(this.ConvertReserveColor, 1);
                IgnoreTSA.Checked = false;
            }
            else if (Method.SelectedIndex == 3)
            {//ワールドマップ(でかい)
                if (Program.ROM.RomInfo.version() == 8)
                {
                    U.ForceUpdate(this.ConvertWidth, 480);
                    U.ForceUpdate(this.ConvertHeight, 320);
                    U.ForceUpdate(this.ConvertYohaku, 0);
                    U.ForceUpdate(this.ConvertPaletteNo, 4);
                    U.ForceUpdate(this.ConvertReserveColor, 1);
                }
                else if (Program.ROM.RomInfo.version() == 7)
                {
                    U.ForceUpdate(this.ConvertWidth, 1024);
                    U.ForceUpdate(this.ConvertHeight, 688);
                    U.ForceUpdate(this.ConvertYohaku, 0);
                    U.ForceUpdate(this.ConvertPaletteNo, 4);
                    U.ForceUpdate(this.ConvertReserveColor, 1);
                }
                else if (Program.ROM.RomInfo.version() == 6)
                {//256色なので不要
                    U.ForceUpdate(this.ConvertWidth, 240);
                    U.ForceUpdate(this.ConvertHeight, 160);
                    U.ForceUpdate(this.ConvertYohaku, 0);
                    U.ForceUpdate(this.ConvertPaletteNo, 16);
                    U.ForceUpdate(this.ConvertReserveColor, 0);
                }
                IgnoreTSA.Checked = false;
            }
            else if (Method.SelectedIndex == 4)
            {// 04=ワールドマップ(イベント用)
                if (Program.ROM.RomInfo.version() == 8)
                {
                    U.ForceUpdate(this.ConvertWidth, 30 * 8);
                    U.ForceUpdate(this.ConvertHeight, 20 * 8);
                    U.ForceUpdate(this.ConvertYohaku, 2 * 8);
                    U.ForceUpdate(this.ConvertPaletteNo, 4);
                    U.ForceUpdate(this.ConvertReserveColor, 1);
                    IgnoreTSA.Checked = false;
                }
                else if (Program.ROM.RomInfo.version() == 7)
                {
                    U.ForceUpdate(this.ConvertWidth, 30 * 8);
                    U.ForceUpdate(this.ConvertHeight, 20 * 8);
                    U.ForceUpdate(this.ConvertYohaku, 2 * 8);
                    U.ForceUpdate(this.ConvertPaletteNo, 4);
                    U.ForceUpdate(this.ConvertReserveColor, 1);
                    IgnoreTSA.Checked = false;
                }
                else if (Program.ROM.RomInfo.version() == 6)
                {//256色なので不要
                    U.ForceUpdate(this.ConvertWidth, 240);
                    U.ForceUpdate(this.ConvertHeight, 160);
                    U.ForceUpdate(this.ConvertYohaku, 0);
                    U.ForceUpdate(this.ConvertPaletteNo, 16);
                    U.ForceUpdate(this.ConvertReserveColor, 0);
                    IgnoreTSA.Checked = false;
                }
            }
            else if (Method.SelectedIndex == 5)
            {//TSAを利用しない256色
                U.ForceUpdate(this.ConvertWidth, 30 * 8);
                U.ForceUpdate(this.ConvertHeight, 20 * 8);
                U.ForceUpdate(this.ConvertYohaku, 0);
                U.ForceUpdate(this.ConvertPaletteNo, 16);
                U.ForceUpdate(this.ConvertReserveColor, 1);
                IgnoreTSA.Checked = true;
            }
            else if (Method.SelectedIndex == 6)
            {//ステータス画面背景(FE8)
                U.ForceUpdate(this.ConvertWidth, 30 * 8);
                U.ForceUpdate(this.ConvertHeight, 20 * 8);
                U.ForceUpdate(this.ConvertYohaku, 0);
                U.ForceUpdate(this.ConvertPaletteNo, 4);
                U.ForceUpdate(this.ConvertReserveColor, 1);
                IgnoreTSA.Checked = false;
            }
        }

        private void AFileSelectButton_Click(object sender, EventArgs e)
        {
            string title = R._("開くファイル名を選択してください");
            string filter = R._("IMAGES|*.png;*.bmp;*.jpg|PNG|*.png|BMP|*.bmp|JPG|*.jpg|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            DialogResult dr = open.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (!U.CanReadFileRetry(open))
            {
                return;
            }
            this.AFilename.Text = open.FileName;
        }

        private void BFileSelectButton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("PNG|*.png|BMP|*.bmp|All files|*");
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
            Program.LastSelectedFilename.Save(this, "", save);
            this.BFilename.Text = save.FileName;
        }

        private void MakeButton_Click(object sender, EventArgs e)
        {
            if (!File.Exists(AFilename.Text))
            {
                return;
            }
            if (BFilename.Text == "")
            {
                return;
            }
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }

            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                DecreaseColor dc = new DecreaseColor();

                Bitmap src = new Bitmap(AFilename.Text);

                int width = (int)this.ConvertWidth.Value;
                int height = (int)this.ConvertHeight.Value;
                int yohaku = (int)this.ConvertYohaku.Value;
                int paletteno = (int)this.ConvertPaletteNo.Value;
                bool isReserve1stColor = (this.ConvertReserveColor.SelectedIndex == 1);

                Bitmap src2;
                if(this.ConvertSizeMethod.SelectedIndex == 0)
                {
                    src2 = ImageUtil.BitmapSizeChange(src,0,0,width,height);
                }
                else
                {
                    src2 = ImageUtil.BitmapScale(src,width,height);
                }
                bool ignoreTSA = IgnoreTSA.Checked;
                Bitmap dest = dc.Convert(src2, paletteno, yohaku, isReserve1stColor, ignoreTSA);

                dest.Save(BFilename.Text);

                src.Dispose();
                src2.Dispose();
                dest.Dispose();
            }
            //エクスプローラで選択しよう
            U.SelectFileByExplorer(BFilename.Text);
        }

        private void AFilename_DoubleClick(object sender, EventArgs e)
        {
            this.AFileSelectButton.PerformClick();
        }

        private void BFilename_DoubleClick(object sender, EventArgs e)
        {
            this.BFileSelectButton.PerformClick();
        }

        public static string GetExplainDecreaseColor()
        {
            return R._("16色を超える画像を利用したい場合は、減色ツールを利用してください。\r\nTSAを考慮して減色することで、16色*8パレット利用できます。\r\n");
        }

        private void ConvertPaletteNo_ValueChanged(object sender, EventArgs e)
        {
            IgnoreTSA.Visible = (ConvertPaletteNo.Value == 16);
        }

    }
}
