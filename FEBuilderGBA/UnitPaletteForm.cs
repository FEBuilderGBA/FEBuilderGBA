using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class UnitPaletteForm : Form
    {
        public UnitPaletteForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);
            this.N_AddressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);
            this.InputFormRef = Init(this);
            this.N_InputFormRef = N_Init(this);

            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N_InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.unit_palette_color_pointer()
                , 7
                , (int i, uint addr) =>
                {//個数が固定できまっている
                    return i < Program.ROM.RomInfo.unit_maxcount();
                }
                , (int i, uint addr) =>
                {//リストボックスに乗せる項目
                    return U.ToHexString(i + 1) + " " + UnitForm.GetUnitName((uint)i+1);
                }
                );
        }

        InputFormRef N_InputFormRef;
        static InputFormRef N_Init(Form self)
        {
            return new InputFormRef(self
                , "N_"
                , Program.ROM.RomInfo.unit_palette_class_pointer()
                , 7
                , (int i, uint addr) =>
                {//個数が固定できまっている
                    return i < Program.ROM.RomInfo.unit_maxcount();
                }
                , (int i, uint addr) =>
                {//リストボックスに乗せる項目
                    return U.ToHexString(i + 1) + " " + UnitForm.GetUnitName((uint)i+1);
                }
                );
        }

        
        
        private void UnitPaletteForm_Load(object sender, EventArgs e)
        {
        }

        private void B0_ValueChanged(object sender, EventArgs e)
        {
            X_PIC0.Image = ImageBattleAnimeForm.DrawBattleAnime(ImageBattleAnimeForm.GetAnimeIDByClassID((uint)N_B0.Value)
                , ImageBattleAnimeForm.ScaleTrim.SCALE_90, (uint)B0.Value);
        }

        private void B1_ValueChanged(object sender, EventArgs e)
        {
            X_PIC1.Image = ImageBattleAnimeForm.DrawBattleAnime(ImageBattleAnimeForm.GetAnimeIDByClassID((uint)N_B1.Value)
                , ImageBattleAnimeForm.ScaleTrim.SCALE_90, (uint)B1.Value);
        }

        private void B2_ValueChanged(object sender, EventArgs e)
        {
            X_PIC2.Image = ImageBattleAnimeForm.DrawBattleAnime(ImageBattleAnimeForm.GetAnimeIDByClassID((uint)N_B2.Value)
                , ImageBattleAnimeForm.ScaleTrim.SCALE_90, (uint)B2.Value);
        }

        private void B3_ValueChanged(object sender, EventArgs e)
        {
            X_PIC3.Image = ImageBattleAnimeForm.DrawBattleAnime(ImageBattleAnimeForm.GetAnimeIDByClassID((uint)N_B3.Value)
                , ImageBattleAnimeForm.ScaleTrim.SCALE_90, (uint)B3.Value);
        }

        private void B4_ValueChanged(object sender, EventArgs e)
        {
            X_PIC4.Image = ImageBattleAnimeForm.DrawBattleAnime(ImageBattleAnimeForm.GetAnimeIDByClassID((uint)N_B4.Value)
                , ImageBattleAnimeForm.ScaleTrim.SCALE_90, (uint)B4.Value);
        }

        private void B5_ValueChanged(object sender, EventArgs e)
        {
            X_PIC5.Image = ImageBattleAnimeForm.DrawBattleAnime(ImageBattleAnimeForm.GetAnimeIDByClassID((uint)N_B5.Value)
                , ImageBattleAnimeForm.ScaleTrim.SCALE_90, (uint)B5.Value);
        }

        private void B6_ValueChanged(object sender, EventArgs e)
        {
            X_PIC6.Image = ImageBattleAnimeForm.DrawBattleAnime(ImageBattleAnimeForm.GetAnimeIDByClassID((uint)N_B6.Value)
                , ImageBattleAnimeForm.ScaleTrim.SCALE_90, (uint)B6.Value);
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.AddressList.Focused)
            {
                U.SelectedIndexSafety(this.N_AddressList, this.AddressList.SelectedIndex);
            }
        }

        private void N_AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.N_AddressList.Focused)
            {
                U.SelectedIndexSafety(this.AddressList, this.N_AddressList.SelectedIndex);
            }
        }
        public void JumpTo(uint id)
        {
            this.AddressList.SelectedIndex = (int)id;
        }

        public static Bitmap DrawSample(uint unitid,uint type)
        {
            InputFormRef InputFormRef = Init(null);
            InputFormRef N_InputFormRef = N_Init(null);

            uint addr1 = InputFormRef.IDToAddr(unitid);
            uint addr2 = N_InputFormRef.IDToAddr(unitid);
            if (addr1 == U.NOT_FOUND || addr2 == U.NOT_FOUND)
            {
                return ImageUtil.BlankDummy();
            }
            uint b = Program.ROM.u8(addr1 + type);
            uint n_b = Program.ROM.u8(addr2 + type);
            return ImageBattleAnimeForm.DrawBattleAnime(ImageBattleAnimeForm.GetAnimeIDByClassID(n_b)
                , ImageBattleAnimeForm.ScaleTrim.SCALE_90, b);
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "UnitPalette";

            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { });

            InputFormRef N_InputFormRef = N_Init(null);
            FEBuilderGBA.Address.AddAddress(list, N_InputFormRef, name, new uint[] { });
        }

        private void UnitPaletteForm_Shown(object sender, EventArgs e)
        {
            this.N_AddressList.SelectedIndex = this.AddressList.SelectedIndex;
        }

    }
}
