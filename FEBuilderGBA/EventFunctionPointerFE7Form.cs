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
    public partial class EventFunctionPointerFE7Form : Form
    {
        public EventFunctionPointerFE7Form()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.event_function_pointer_table_pointer
                , 8
                , (int i, uint addr) =>
                {
                    uint a = Program.ROM.u32(addr);
                    if (!U.isPointer(a))
                    {
                        return false;
                    }
                    if (! U.IsValueOdd(a))
                    {
                        return false;
                    }
                    uint b = Program.ROM.u32(addr+4);
                    if (b >= 0x10)
                    {//サイズ?カテゴリ? 
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + " " + "Event" + U.ToHexString(i);
                }
                );
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            X_NAME.Text = FindEventInfo(AddressList.SelectedIndex);
        }

        string FindEventInfo(int search_eventid)
        {
            if (search_eventid <= 0)
            {
                return "NULL";
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Program.EventScript.Scripts.Length; i++)
            {
                EventScript.Script sc = Program.EventScript.Scripts[i];
                if (sc.Data.Length <= 1)
                {
                    continue;
                }
                uint eventid = U.u8(sc.Data, 0);
                if (eventid == 0)
                {
                    continue;
                }
                if (eventid == search_eventid)
                {
                    string comment = "";
                    if(sc.Category != "")
                    {
                        comment += " "+sc.Category;
                    }
                    if(sc.PopupHint != "")
                    {
                        comment += " "+sc.PopupHint;
                    }
                    sb.AppendLine(EventScript.makeCommandComboText(sc,false) + comment);
                }
            }
            return sb.ToString();
        }

        private void EventFunctionPointerFE7Form_Load(object sender, EventArgs e)
        {

        }
        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "EventFunctionPointer";

            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list
                , InputFormRef
                , name
                , new uint[] { 0 }
                , FEBuilderGBA.Address.DataTypeEnum.InputFormRef_ASM);

            List<U.AddrResult> arlist = InputFormRef.MakeList();
            FEBuilderGBA.Address.AddFunctions(list, arlist, 0, name);
        }

    }
}
