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
    public partial class ToolAllWorkSupportForm : Form
    {
        public ToolAllWorkSupportForm()
        {
            InitializeComponent();
            ReloadWorks();
        }

        private void ToolAllWorkSupportForm_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = true;
            ReDraw();
        }

        void ReloadWorks()
        {
            WorkList = new List<Work>();
            string etcDir = Path.Combine(Program.BaseDirectory, "config", "etc");
            string[] files = U.Directory_GetFiles_Safe(etcDir, "worksupport_.txt", SearchOption.AllDirectories);
            foreach(string filename in files)
            {
                Dictionary<uint, string> etc = U.LoadTSVResource1(filename, false);
                string romfilename = U.at(etc, 0);
                if (!File.Exists(romfilename))
                {
                    continue;
                }

                string updateinfo_filename = ToolWorkSupportForm.GetUpdateInfo(romfilename);
                if (!File.Exists(updateinfo_filename))
                {
                    continue;
                }

                Dictionary<string, string> updateinfo = ToolWorkSupportForm.LoadUpdateInfo(updateinfo_filename);

                Work w = new Work();
                w.ROMFilenme = romfilename;
                w.UpdateinfoLines = updateinfo;
                if (updateinfo.ContainsKey("NAME"))
                {
                    w.Name = U.at(updateinfo, "NAME");
                }
                else
                {
                    w.Name = Path.GetFileNameWithoutExtension(romfilename);
                }
                if (updateinfo.ContainsKey("LOGO_FILENAME"))
                {
                    w.LogoFilename = Path.Combine(Path.GetDirectoryName(romfilename), U.at(updateinfo, "LOGO_FILENAME"));
                }
                else
                {
                    w.LogoFilename = "";
                }
                w.IsUpdateMark = false;
                WorkList.Add(w);
            }
        }

        const int PADDING = 5;
        const int BUTTON_WIDTH  = 270;
        const int BUTTON_HEIGHT = 180;
        const int FONT_HIGHT = 20;
        const int CELL_WIDTH = PADDING + BUTTON_WIDTH + PADDING;
        const int CELL_HEIGHT = PADDING + BUTTON_HEIGHT + PADDING + FONT_HIGHT + PADDING;
        
        private void ToolAllWorkSupportForm_Resize(object sender, EventArgs e)
        {
            ReDraw();
        }

        public class Work
        {
            public string ROMFilenme;
            public string LogoFilename;
            public string Name;
            public Dictionary<string, string> UpdateinfoLines;
            public bool IsUpdateMark;
        };

        List<Work> WorkList = new List<Work>();
        void ReDraw()
        {
            Ctrl.Controls.Clear();
            Bitmap icon = ImageSystemIconForm.MusicIcon(12);
            U.MakeTransparent(icon);

            int y = PADDING;
            int x = PADDING;
            foreach (Work work in WorkList)
            {
                if (x + (BUTTON_WIDTH) >= this.Width)
                {
                    x = PADDING;
                    y += CELL_HEIGHT;
                }

                if (work.IsUpdateMark)
                {
                    PictureBox pic = new PictureBox();
                    pic.Image = icon;
                    pic.SizeMode = PictureBoxSizeMode.StretchImage;
                    pic.Size = new System.Drawing.Size(32, 32);
                    pic.Location = new Point(x + BUTTON_WIDTH - 24, y + BUTTON_HEIGHT - 24);
                    pic.Tag = work;
                    pic.Click += ToolAllWorkSupportForm_ClickTitle;
                    Ctrl.Controls.Add(pic);
                }

                Button b = new Button();
                b.BackgroundImageLayout = ImageLayout.Stretch;
                b.BackgroundImage = ToolWorkSupportForm.MakeLogoLow(work.LogoFilename, work.ROMFilenme);
                b.Size = new System.Drawing.Size(BUTTON_WIDTH, BUTTON_HEIGHT);
                b.Location = new Point(x, y);
                b.Tag = work;
                b.Click += ToolAllWorkSupportForm_ClickTitle;
                Ctrl.Controls.Add(b);

                Label l = new Label();
                l.Text = work.Name;
                l.Size = new System.Drawing.Size(BUTTON_WIDTH, FONT_HIGHT);
                l.Location = new Point(x, y + BUTTON_HEIGHT + PADDING);
                l.TextAlign = ContentAlignment.MiddleCenter;
                l.Tag = work;
                l.DoubleClick += ToolAllWorkSupportForm_ClickTitle;
                Ctrl.Controls.Add(l);

                x += CELL_WIDTH;
            }
        }
        private void ToolAllWorkSupportForm_ClickTitle(object sender, EventArgs e)
        {
            Work work;
            if (sender is Control)
            {
                work = (Work)((Control)sender).Tag;
            }
            else
            {
                return;
            }

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                MainFormUtil.Open(this, work.ROMFilenme, true, "");
            }
        }

        private void UpdateCheckButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                UpdateCheck();
            }
            ReDraw();
        }

        void UpdateCheck()
        {
            foreach (Work work in WorkList)
            {
                ToolWorkSupportForm.UPDATE_RESULT ur =ToolWorkSupportForm.CheckUpdateLow(work.UpdateinfoLines,work.ROMFilenme, isSlientMode: true);
                if (ur == ToolWorkSupportForm.UPDATE_RESULT.UPDATEABLE)
                {
                    work.IsUpdateMark = true;
                }
                else
                {
                    work.IsUpdateMark = false;
                }
            }
        }
    }
}
