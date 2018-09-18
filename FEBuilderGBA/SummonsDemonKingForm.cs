using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class SummonsDemonKingForm : Form
    {
        public SummonsDemonKingForm()
        {
            InitializeComponent();
            this.AddressList.OwnerDraw(EventUnitForm.AddressList_Draw, DrawMode.OwnerDrawVariable);

            EventUnitForm.AI1ToCombo(L_16_COMBO);
            EventUnitForm.AI2ToCombo(L_17_COMBO);

            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.InputFormRef.AddressListExpandsEvent += AddressListExpandsEvent;
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.summons_demon_king_pointer()
                , 20
                , (int i, uint addr) =>
                {
                    uint max = Program.ROM.u8(Program.ROM.RomInfo.summons_demon_king_count_address());
                    return i <= max;
                }
                , (int i, uint addr) =>
                {
                    uint unit_id = Program.ROM.u8(addr);
                    if (unit_id == 0)
                    {
                        return "-EMPTY-";
                    }
                    uint class_id = Program.ROM.u8(addr + 1);
                    uint unitgrow = Program.ROM.u16(addr + 3);
                    if (class_id == 0)
                    {//クラスIDが0だったらユーザIDで補完する
                        class_id = UnitForm.GetClassID(unit_id);
                    }

                    String unit_name = UnitForm.GetUnitName(unit_id);
                    String class_name = ClassForm.GetClassName(class_id);
                    uint level = U.ParseUnitGrowLV(unitgrow);

                    return unit_name + "(" + class_name + ")" + "  " + "Lv:" + level.ToString();
                }
                );
        }


        //リストが拡張されたとき
        void AddressListExpandsEvent(object sender, EventArgs arg)
        {
            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            Program.ROM.write_u8(Program.ROM.RomInfo.summons_demon_king_count_address(), eearg.NewDataCount);
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list,bool isPointerOnly)
        {
            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "Summons", new uint[] { });
        }
    }
}
