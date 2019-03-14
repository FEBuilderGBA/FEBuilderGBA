using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class EventScriptTemplateForm : Form
    {
        public EventScriptTemplateForm()
        {
            InitializeComponent();
            LoadTemplate();
            this.SampleEventListbox.OwnerDraw(DrawEvent, DrawMode.OwnerDrawVariable, false);

            U.AddCancelButton(this);
        }

        private void EventScriptTemplateForm_Load(object sender, EventArgs e)
        {
            U.SelectedIndexSafety(this.TemplateListbox, 0);
        }

        uint MapID;
        EventScriptInnerControl CurrentControl;
        public void Init(uint mapid, EventScriptInnerControl currentControl)
        {
            this.MapID = mapid;
            this.CurrentControl = currentControl;
        }


        private Size DrawEvent(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= this.Codes.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }

            //アドレスにあるイベントを表示.
            //EventScript.OneCode code = Program.EventScript.DisAseemble(Program.ROM.Data, U.toOffset(history.EventPackedAddr));
            return EventScriptForm.DrawCode(lb, g, listbounds, isWithDraw, this.Codes[index]);
        }

        void LoadTemplate()
        {
            this.EventTemplateList = new List<EventTemplate>();

            string configFilename = U.ConfigDataFilename("template_list_event_");
            if (!File.Exists(configFilename))
            {
                return;
            }
            string[] lines = File.ReadAllLines(configFilename);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (U.OtherLangLine(line))
                {
                    continue;
                }
                string[] sp = line.Split('\t');
                if (sp.Length < 2)
                {
                    continue;
                }
                EventTemplate et = new EventTemplate();
                et.Filename = sp[0];
                et.Info = sp[1];

                this.EventTemplateList.Add(et);
                this.TemplateListbox.Items.Add(et.Info);
            }
        }

        class EventTemplate
        {
            public string Filename;
            public string Info;
        }

        List<EventTemplate> EventTemplateList;
        public List<EventScript.OneCode> Codes { get; private set; }


        private void TemplateList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.TemplateListbox.SelectedIndex;
            if (index < 0 || index >= this.EventTemplateList.Count)
            {
                return;
            }
            EventTemplate et = this.EventTemplateList[index];
            SelectFilename.Text = et.Filename;
            LoadCodes(et);
        }

        static string ToPointerToString(uint addr)
        {
            if (addr == U.NOT_FOUND)
            {
                addr = EventUnitForm.INVALIDATE_UNIT_POINTER;
            }
            return U.ToHexString8(U.ChangeEndian32(U.toPointer(addr)));
        }
        static string ToUShortToString(uint addr)
        {
            return U.ToHexString8(U.ChangeEndian32(addr)).Substring(0,4);
        }

        void LoadCodes(EventTemplate et)
        {
            this.Codes = new List<EventScript.OneCode>();

            string fullfilename = Path.Combine(Program.BaseDirectory, "config", "data", et.Filename);
            if (!File.Exists(fullfilename))
            {
                return ;
            }

            string XXXXXXXX = null;
            string YYYYYYYY = null;
            if (et.Filename.IndexOf("template_event_CALL_END_EVENT") >= 0)
            {
                XXXXXXXX = ToPointerToString(EventCondForm.GetEndEvent(this.MapID));
            }
            else if (et.Filename.IndexOf("template_event_PREPARATION") >= 0)
            {
                XXXXXXXX = ToPointerToString(EventCondForm.GetPlayerUnits(this.MapID));
                YYYYYYYY = ToPointerToString(EventCondForm.GetEnemyUnits(this.MapID));
            }
            else if (et.Filename.IndexOf("_COND_") >= 0)
            {
                uint labelX = GetUnuseLabelID(0x9000);
                XXXXXXXX = ToUShortToString(labelX);

                if (et.Filename.IndexOf("_ELSE_") >= 0)
                {
                    uint labelY = GetUnuseLabelID(labelX + 1);
                    YYYYYYYY = ToUShortToString(labelY);
                }
            }


            byte[] bin = EventScriptInnerControl.ConverteventTextToBin(fullfilename,false, XXXXXXXX, YYYYYYYY);
            uint addr = 0;
            uint limit = (uint)bin.Length;
            while (addr < limit)
            {
                EventScript.OneCode code = Program.EventScript.DisAseemble(bin, addr);
                this.Codes.Add(code);
                addr += (uint)code.Script.Size;
            }
            this.SampleEventListbox.DummyAlloc(this.Codes.Count, 0);
        }

        uint GetUnuseLabelID(uint startID)
        {
            for (uint id = startID; id < 0xffff; id++)
            {
                if (! this.CurrentControl.IsUseLabelID(id))
                {
                    return id;
                }
            }
            return 0xffff;
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            int selected = this.TemplateListbox.SelectedIndex;
            if (selected < 0)
            {
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void TemplateListbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectButton_Click(sender, e);
            }
        }

        private void SelectFilename_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string fullfilename = Path.Combine(Program.BaseDirectory, "config", "data", SelectFilename.Text);
            if (!File.Exists(fullfilename))
            {
                return;
            }
            System.Diagnostics.Process.Start(fullfilename);
        }
    }
}
