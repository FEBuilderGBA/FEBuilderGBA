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
    public partial class EventTemplate4Form : Form
    {
        public EventTemplate4Form()
        {
            InitializeComponent();
            FormIcon.Image = SystemIcons.Question.ToBitmap();
            U.AddCancelButton(this);
        }
        public void Init(List<Control> controls)
        {
            this.ParentControls = controls;
        }
        List<Control> ParentControls;
        public byte[] GenCode = null;
        public uint CallEventAddr = U.NOT_FOUND;
        public bool NeedFlag03 = false;

        private void BLANK_Button_Click(object sender, EventArgs e)
        {
            this.GenCode = Program.ROM.RomInfo.Default_event_script_toplevel_code;
            this.Close();
        }

        private void VILLAGE_TALK_Button_Click(object sender, EventArgs e)
        {
            this.GenCode = EventScriptInnerControl.ConverteventTextToBin(U.ConfigDataFilename("template_event_TalkEvent_"));
            this.Close();
        }

        private void VILLAGE_ITEM_Button_Click(object sender, EventArgs e)
        {
            this.GenCode = EventScriptInnerControl.ConverteventTextToBin(U.ConfigDataFilename("template_event_TalkEventItem_"));
            this.Close();
        }

        private void VILLAGE_UNIT_button_Click(object sender, EventArgs e)
        {
            this.GenCode = EventScriptInnerControl.ConverteventTextToBin(U.ConfigDataFilename("template_event_TalkEventJoin_"));
            this.Close();
        }

        private void CALL_EndEvent_button_Click(object sender, EventArgs e)
        {
            uint mapid = EventCondForm.GetMapID(this.ParentControls);
            this.CallEventAddr = EventCondForm.GetEndEvent(mapid);
            this.NeedFlag03 = true;
            this.Close();
        }
        private void CALL_1_button_Click(object sender, EventArgs e)
        {
            this.CallEventAddr = 1;
            this.Close();
        }

    }
}
