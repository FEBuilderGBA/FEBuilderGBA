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
    public partial class EventTemplate2Form : Form
    {
        public EventTemplate2Form()
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
            this.GenCode = Program.ROM.RomInfo.Default_event_script_toplevel_code();
            this.Close();
        }

        private void EnterByPlayer_button_Click(object sender, EventArgs e)
        {
            this.GenCode = EventScriptInnerControl.ConverteventTextToBin(U.ConfigDataFilename("template_event_ENTER_BY_PLAYER_")
                , EventScriptInnerControl.TermCode.SimpleTermCode
                );
            this.Close();
        }

        private void EnterByUnit_button_Click(object sender, EventArgs e)
        {
            this.GenCode = EventScriptInnerControl.ConverteventTextToBin(U.ConfigDataFilename("template_event_ENTER_BY_UNIT_")
                , EventScriptInnerControl.TermCode.SimpleTermCode
                );
            this.Close();
        }

        private void GameOver_Button_Click(object sender, EventArgs e)
        {
            this.GenCode = EventScriptInnerControl.ConverteventTextToBin(U.ConfigDataFilename("template_event_EnterByUnitToGameOver_")
                , EventScriptInnerControl.TermCode.SimpleTermCode
                );
            this.Close();
        }

        private void DesertTreasure_Button_Click(object sender, EventArgs e)
        {
            this.GenCode = EventScriptInnerControl.ConverteventTextToBin(U.ConfigDataFilename("template_event_DESERTT_REASURE_")
                , EventScriptInnerControl.TermCode.SimpleTermCode
                );
            this.Close();
        }

        private void CALL_EndEvent_button_Click(object sender, EventArgs e)
        {
            uint mapid = EventCondForm.GetMapID(this.ParentControls);
            this.CallEventAddr = EventCondForm.GetEndEvent(mapid);
            this.NeedFlag03 = true;
            this.Close();
        }

        private void EnterByEnemy_button_Click(object sender, EventArgs e)
        {
            this.GenCode = EventScriptInnerControl.ConverteventTextToBin(U.ConfigDataFilename("template_event_ENTER_BY_ENEMY_")
                , EventScriptInnerControl.TermCode.SimpleTermCode
                );
            this.Close();
        }

        private void EnterByNPC_button_Click(object sender, EventArgs e)
        {
            this.GenCode = EventScriptInnerControl.ConverteventTextToBin(U.ConfigDataFilename("template_event_ENTER_BY_NPC_")
                , EventScriptInnerControl.TermCode.SimpleTermCode
                );
            this.Close();
        }

    }
}
