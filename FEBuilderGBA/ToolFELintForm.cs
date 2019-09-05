using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ToolFELintForm : Form
    {
        public ToolFELintForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(DrawErrorList, DrawMode.OwnerDrawFixed, false);
            InputFormRef.TabControlHideTabOption(this.MainTab);
            U.AddCancelButton(this);
        }

        private void ToolFELintForm_Load(object sender, EventArgs e)
        {
            this.X_SUCCESSMESSAGE.BackColor = OptionForm.Color_Keyword_BackColor();
            this.X_SUCCESSMESSAGE.ForeColor = OptionForm.Color_Keyword_ForeColor();
            this.X_ERRORMESSAGE.BackColor = OptionForm.Color_Error_BackColor();
            this.X_ERRORMESSAGE.ForeColor = OptionForm.Color_Error_ForeColor();
        }

        private Size DrawErrorList(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            U.AddrResult ar = InputFormRef.SelectToAddrResult(lb, index);
            uint mapid = ar.addr;

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            SolidBrush errorBrush = new SolidBrush(OptionForm.Color_Error_ForeColor());

            Font normalFont = lb.Font;
            Font boldFont = new Font(lb.Font,FontStyle.Bold);

            string text;
            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;
            int maxHeight = (int)lb.Font.Height;

            
            bounds.X += U.DrawText(ar.name, g, boldFont, brush, isWithDraw, bounds);
            bounds.X += 20;

            text = R._("//エラー:{0}個のエラーがあります", ar.tag );
            bounds.X += U.DrawText(text, g, boldFont, errorBrush, isWithDraw, bounds);


            bounds.Y += maxHeight;
            return new Size(bounds.X, bounds.Y);
        }

        public static int CommandLineLint()
        {
            if (Program.ROM == null)
            {
                U.echo(R._("ROMが読みこまれていないので実行できません"));
                return -2;
            }

            U.echo("Check...");

            List<U.AddrResult> errorMapUI = ScanWithoutUI(null,false);
            for (int i = 0; i < errorMapUI.Count; i++)
            {
                U.AddrResult ar = errorMapUI[i];
                string text = R._("//エラー:{0}個のエラーがあります", ar.tag );
                U.echo(ar.name + text);
            }

            if (errorMapUI.Count > 0)
            {
                return -1;
            }
            U.echo("System ALL GREEN!");
            return 0;
        }


        static List<U.AddrResult> ScanWithoutUI(InputFormRef.AutoPleaseWait pleaseWait,bool useIgnoreData)
        {
            List<U.AddrResult> errorMapUI = new List<U.AddrResult>();
            List<U.AddrResult> maps = MapSettingForm.MakeMapIDList();

            List<DisassemblerTrumb.LDRPointer> ldrmap;
            ldrmap = DisassemblerTrumb.MakeLDRMap(Program.ROM.Data, 0x100);

            //システム全体の問題
            {
                if (pleaseWait != null)
                {
                    pleaseWait.DoEvents(R._("システムチェック中"));
                }

                List<FELint.ErrorSt> errorList = FELint.ScanMAP(FELint.SYSTEM_MAP_ID, ldrmap);
                if (! useIgnoreData)
                {
                    errorList = FELint.HiddenErrorFilter(errorList);
                }

                if (errorList.Count > 0)
                {//エラーがある
                    U.AddrResult ar = new U.AddrResult();
                    ar.addr = FELint.SYSTEM_MAP_ID;
                    ar.name = R._("システム");
                    ar.tag = (uint)errorList.Count;

                    errorMapUI.Add(ar);
                }
            }

            for (int i = 0; i < maps.Count; i++)
            {
                if (pleaseWait != null)
                {
                    pleaseWait.DoEvents(R._("調査中 {0}/{1}", i, maps.Count));
                }

                uint mapid = (uint)i;
                U.AddrResult ar = new U.AddrResult();

                //このマップのエラースキャン
                List<FELint.ErrorSt> errorList = FELint.ScanMAP(mapid, ldrmap);
                if (! useIgnoreData)
                {
                    errorList = FELint.HiddenErrorFilter(errorList);
                }
                if (errorList.Count <= 0)
                {//エラーがない
                    continue;
                }

                ar.addr = mapid;
                ar.name = maps[i].name;
                ar.tag = (uint)errorList.Count;

                errorMapUI.Add(ar);
            }


            return errorMapUI;
        }

        //エラーがない場合のフック
        public EventHandler OnNoErrorEventHandler;
        //エラーがある場合のフック
        public EventHandler OnErrorEventHandler;

        bool ShowAllError()
        {
            if (MainTab.SelectedTab == tabPageError)
            {
                return ShowAllError1.Checked;
            }
            if (MainTab.SelectedTab == tabPageNoError)
            {
                return ShowAllError2.Checked;
            }
            return false;
        }

        void Scan()
        {
            bool showAllError = ShowAllError();

            //スキャン中
            MainTab.SelectedTab = tabPageScan;

            List<U.AddrResult> errorMapUI ;
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                errorMapUI = ScanWithoutUI(pleaseWait, showAllError);
            }

            if (errorMapUI.Count <= 0)
            {//エラーがないよ
                MainTab.SelectedTab = tabPageNoError;

                if (OnNoErrorEventHandler != null)
                {
                    OnNoErrorEventHandler(this, new EventArgs());
                }
                return;
            }

            //エラーがあるよ.
            U.ConvertListBox(errorMapUI, ref AddressList);
            MainTab.SelectedTab = tabPageError;
            U.SelectedIndexSafety(AddressList, 0, true);

            if (OnErrorEventHandler != null)
            {
                OnErrorEventHandler(this, new EventArgs());
            }
        }

        private void NoError_ReloadButton_Click(object sender, EventArgs e)
        {
            Scan();
        }

        private void Error_ReloadButton_Click(object sender, EventArgs e)
        {
            Scan();
        }

        void EnterEvent()
        {
            U.AddrResult ar = InputFormRef.SelectToAddrResult(this.AddressList);
            if (ar.name == null)
            {
                return;
            }
            uint mapid = ar.addr; //マップIDが入っています.

            MainSimpleMenuEventErrorForm f = (MainSimpleMenuEventErrorForm)InputFormRef.JumpForm<MainSimpleMenuEventErrorForm>(U.NOT_FOUND);
            f.Init(mapid, ShowAllError1.Checked);
        }


        private void AddressList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                EnterEvent();
            }
        }

        private void AddressList_DoubleClick(object sender, EventArgs e)
        {
            EnterEvent();
        }



        private void ToolFELintForm_Shown(object sender, EventArgs e)
        {
            Scan();
        }

        private void ToolFELintForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.R)
            {
                Scan();
            }
        }

        private void DiffDebugToolButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<ToolDiffDebugSelectForm>();
        }
    }
}
