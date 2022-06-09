using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class EventFunctionPointerForm : Form
    {
        public EventFunctionPointerForm()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            this.FilterComboBox.SelectedIndex = 0;
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            uint wmap_event_base = Program.ROM.p32(Program.ROM.RomInfo.event_function_pointer_table2_pointer);

            InputFormRef ifr = null;
            ifr = new InputFormRef(self
                , ""
                , 0
                , 4
                , (int i, uint addr) =>
                {
                    uint a = Program.ROM.u32(addr);
                    if (!U.isPointer(a))
                    {
                        return false;
                    }
                    if (!U.IsValueOdd(a))
                    {
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    int id = i;
                    if(ifr.BaseAddress == wmap_event_base)
                    {
                        id += 0x80;
                    }
                    return U.ToHexString(id) + " " + "Event"+U.ToHexString(id);
                }
                );
            return ifr;
        }

        private void EventFunctionPointerForm_Load(object sender, EventArgs e)
        {
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int append = 0;
            if (this.FilterComboBox.SelectedIndex == 1)
            {
                append = 0x80;
            }

            X_NAME.Text = FindEventInfo(AddressList.SelectedIndex + append);

        }
        string FindEventInfo(int search_eventid)
        {
            if (search_eventid <= 0)
            {
                return "NULL";
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Program.EventScript.Scripts.Length; i++ )
            {
                EventScript.Script sc = Program.EventScript.Scripts[i];
                if (sc.Data.Length <= 1+1)
                {
                    continue;
                }
                uint eventid = U.u8(sc.Data, 1);
                if (eventid == 0)
                {
                    continue;
                }
                if (eventid == search_eventid)
                {
                    string comment = "";
                    if (sc.Category != "")
                    {
                        comment += " " + sc.Category;
                    }
                    if (sc.PopupHint != "")
                    {
                        comment += " " + sc.PopupHint;
                    }
                    sb.AppendLine(EventScript.makeCommandComboText(sc, false) + comment);
                }
            }
            return sb.ToString();
        }

        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.FilterComboBox.SelectedIndex == 0)
            {
                this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.event_function_pointer_table_pointer));
            }
            else
            {
                this.InputFormRef.ReInitPointer((Program.ROM.RomInfo.event_function_pointer_table2_pointer));
            }
            U.SelectedIndexSafety(this.AddressList, 0, false);
        }
        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            if (Program.ROM.RomInfo.version <= 7)
            {
                EventFunctionPointerFE7Form.MakeAllDataLength(list);
                return;
            }
            string name = "EventFunctionPointer";
            InputFormRef InputFormRef = Init(null);
            InputFormRef.ReInitPointer((Program.ROM.RomInfo.event_function_pointer_table_pointer));
            FEBuilderGBA.Address.AddAddress(list
                , InputFormRef
                , name
                , new uint[] { 0 }
                , FEBuilderGBA.Address.DataTypeEnum.InputFormRef_ASM);

            List<U.AddrResult> arlist = InputFormRef.MakeList();
            FEBuilderGBA.Address.AddFunctions(list, arlist, 0, name);

            name = "EventFunctionPointer Worldmap";
            InputFormRef.ReInitPointer((Program.ROM.RomInfo.event_function_pointer_table2_pointer));
            FEBuilderGBA.Address.AddAddress(list
                , InputFormRef
                , name
                , new uint[] { 0 }
                , FEBuilderGBA.Address.DataTypeEnum.InputFormRef_ASM);

            arlist = InputFormRef.MakeList();
            FEBuilderGBA.Address.AddFunctions(list, arlist, 0, name);
        }
    
    }
}
