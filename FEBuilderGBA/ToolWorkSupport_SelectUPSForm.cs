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
    public partial class ToolWorkSupport_SelectUPSForm : Form
    {
        public ToolWorkSupport_SelectUPSForm()
        {
            InitializeComponent();
        }

        private void ApplyUPSPatchButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        string UPSFilename;
        public void OpenUPS(string upsFilename)
        {
            this.UPSFilename = upsFilename;
        }
        public string GetOrignalFilename()
        {
            return this.OrignalFilename.Text;
        }

        private void WorkSupport_SelectUPSForm_Shown(object sender, EventArgs e)
        {
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                String dir = Path.GetDirectoryName(this.UPSFilename);
                String filename = Path.GetFileName(this.UPSFilename);
                pleaseWait.DoEvents(R._("{0}に適合する元ファイルを自動検索中・・・", filename));

                uint srcCRC32 = UPSUtil.GetUPSSrcCRC32(this.UPSFilename);

                string orignal_romfile = MainFormUtil.FindOrignalROMByCRC32(dir, srcCRC32);
                this.OrignalFilename.Text = orignal_romfile;
            }
        }

        private void WorkSupport_SelectUPSForm_Load(object sender, EventArgs e)
        {

        }
    }
}
