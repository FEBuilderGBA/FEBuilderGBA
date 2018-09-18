using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class OPClassAlphaNameForm : Form
    {
        public OPClassAlphaNameForm()
        {
            InitializeComponent();
            this.N_AddressList.OwnerDraw(ListBoxEx.DrawClassAndText, DrawMode.OwnerDrawFixed);
            int class_data_count = (int)ClassForm.DataCount();
            this.N_InputFormRef = N_Init(this, class_data_count);
            this.N_InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        InputFormRef N_InputFormRef;
        static InputFormRef N_Init(Form self, int class_data_count)
        {
            return new InputFormRef(self
                , "N_"
                , Program.ROM.RomInfo.class_alphaname_pointer()
                , 20
                , (int i, uint addr) =>
                {//読込最大値検索
                    return i < class_data_count;
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + " " + ClassForm.GetClassName((uint)i) + ":" + Program.ROM.getString(addr);
                }
                );
        }

        private void OPClassAlphaNameForm_Load(object sender, EventArgs e)
        {
        }
        private void N_L_0_SPLITSTRING_19_TextChanged(object sender, EventArgs e)
        {
            if ( U.isAlphaString( N_L_0_SPLITSTRING_19.Text) )
            {
                ERROR_ALPHA.Visible = false;
            }
            else
            {
                ERROR_ALPHA.Visible = true;
            }
        }
        public void JumpToADDR(uint addr)
        {
            uint id = N_InputFormRef.AddrToID(addr);
            if (id == U.NOT_FOUND)
            {
                return;
            }
            N_AddressList.SelectedIndex = (int)id;
        }
        public static uint ExpandsArea(Form form,uint current_count, uint newdatacount, Undo.UndoData undodata)
        {
            InputFormRef InputFormRef = N_Init(null, (int)current_count);
            return InputFormRef.ExpandsArea(form, newdatacount, undodata, Program.ROM.RomInfo.class_alphaname_pointer());
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            int datcount = (int)ClassForm.DataCount();
            InputFormRef InputFormRef = N_Init(null, datcount);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "CCClassAlphaName", new uint[] { });
        }

    }
}
