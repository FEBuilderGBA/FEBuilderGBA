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
            this.EventTemplate = new EventTemplateImpl();
            this.EventTemplate.LoadTemplate();

            this.SampleEventListbox.OwnerDraw(DrawEvent, DrawMode.OwnerDrawVariable, false);

            TemplateListbox.BeginUpdate();
            foreach(EventTemplateImpl.EventTemplate et in this.EventTemplate.GetTemplateAll())
            {
                TemplateListbox.Items.Add(et.Info);
            }
            TemplateListbox.EndUpdate();

            U.AddCancelButton(this);
        }
        EventTemplateImpl EventTemplate;

        private void EventScriptTemplateForm_Load(object sender, EventArgs e)
        {
            U.SelectedIndexSafety(this.TemplateListbox, 0);
        }

        public void Init(uint mapid, EventScriptInnerControl currentControl)
        {
            EventTemplate.Init(mapid, currentControl);
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

        public List<EventScript.OneCode> Codes { get; private set; }


        private void TemplateList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.TemplateListbox.SelectedIndex;
            EventTemplateImpl.EventTemplate et = this.EventTemplate.GetTemplate(index);
            if (et == null)
            {
                return ;
            }
            SelectFilename.Text = et.Filename;
            this.Codes = this.EventTemplate.GetCodes(et);
            this.SampleEventListbox.DummyAlloc(this.Codes.Count, 0);
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
            U.OpenURLOrFile(fullfilename);
        }
    }
}
