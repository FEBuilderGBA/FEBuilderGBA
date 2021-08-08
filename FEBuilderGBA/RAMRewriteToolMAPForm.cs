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
    public partial class RAMRewriteToolMAPForm : Form
    {
        public RAMRewriteToolMAPForm()
        {
            InitializeComponent();
            U.AddCancelButton(this);

            this.MapPictureBox.MapMouseDownEvent += MapPictureBox_MouseClick;
            this.MapPictureBox.MapDoubleClickEvent += MapPictureBox_MouseDoubleClick;
        }

        uint Address;
        public void Init(uint addr)
        {
            this.Address = addr;
            this.ValueTextBox.Text = U.ToHexString(addr);

            uint x = Program.RAM.u8(addr);
            this.ReWriteValueX.Value = x;
            this.ReWriteValueX.Maximum = 255;
            this.ReWriteValueX.Hexadecimal = false;

            uint y = Program.RAM.u8(addr + 1);
            this.ReWriteValueY.Value = y;
            this.ReWriteValueY.Maximum = 255;
            this.ReWriteValueY.Hexadecimal = false;

            this.MapPictureBox.SetPoint("", (int)x, (int)y);
            this.MapPictureBox.SetDefualtIcon(ImageSystemIconForm.Blank16());

            this.ReWriteValueX.BackColor = OptionForm.Color_InputDecimal_BackColor();
            this.ReWriteValueX.ForeColor = OptionForm.Color_InputDecimal_ForeColor();
            this.ReWriteValueY.BackColor = OptionForm.Color_InputDecimal_BackColor();
            this.ReWriteValueY.ForeColor = OptionForm.Color_InputDecimal_ForeColor();
            this.ActiveControl = ReWriteValueX;
        }

        private void CopyPointer_Click(object sender, EventArgs e)
        {
            U.SetClipboardText(U.ToHexString(U.toPointer(this.Address)));
            this.Close();
        }

        private void CopyClipboard_Click(object sender, EventArgs e)
        {
            U.SetClipboardText(this.ValueTextBox.Text);
            this.Close();
        }

        private void CopyLittleEndian_Click(object sender, EventArgs e)
        {
            uint a = U.toPointer(this.Address);
            uint r = (((a & 0xFF) << 24)
                + ((a & 0xFF00) << 8)
                + ((a & 0xFF0000) >> 8)
                + ((a & 0xFF000000) >> 24));

            U.SetClipboardText(U.ToHexString(r));
            this.Close();
        }

        private void CopyNoDollGBARadBreakPoint_Click(object sender, EventArgs e)
        {
            U.SetClipboardText("[" + U.ToHexString(U.toPointer(this.Address)) + "]?");
            this.Close();
        }

        private void ReWriteButton_Click(object sender, EventArgs e)
        {
            uint x = (uint)this.ReWriteValueX.Value;
            Program.RAM.write_u8(this.Address, x);

            uint y = (uint)this.ReWriteValueY.Value;
            Program.RAM.write_u8(this.Address + 1, y);

            //非表示フラグが設定されていれば折る


            this.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.Close();
        }

        private void RAMRewriteToolForm_Load(object sender, EventArgs e)
        {
            LoadMapInfo();
        }
        void LoadMapInfo()
        {
            uint mapid = EmulatorMemoryUtil.GetMapID();
            this.MapPictureBox.LoadMap(mapid);
            DrawAllUnits();
        }
        public void DrawAllUnits()
        {
            MapPictureBox.ClearStaticItem();

            DrawUnits(Program.ROM.RomInfo.workmemory_player_units_address(), 62);
            DrawUnits(Program.ROM.RomInfo.workmemory_enemy_units_address(), 50);
            DrawUnits(Program.ROM.RomInfo.workmemory_npc_units_address(), 20);

            MapPictureBox.Invalidate();
        }
        public void DrawUnits(uint topaddr , int max)
        {
            const uint RAMUnitSizeOf = 72; //構造体のサイズ
            bool isFE6 = (Program.ROM.RomInfo.version() == 6);

            uint addr = topaddr;
            for (int i = 0; i < max; i++, addr += RAMUnitSizeOf)
            {
                uint unitPointer = Program.RAM.u32(addr + 0);
                uint classPointer = Program.RAM.u32(addr + 4);

                if (unitPointer == 0)
                {
                    continue;
                }

                if (!U.isSafetyPointer(unitPointer))
                {
                    continue;
                }
                if (!U.isSafetyPointer(classPointer))
                {
                    continue;
                }

                uint classid = Program.ROM.u8(U.toOffset(classPointer) + 4);

                uint unit_number = Program.RAM.u8(addr + 11);
                int palette_type = GetShowPartyClassPaletteType(unit_number);
                Bitmap bitmap = ClassForm.DrawWaitIcon(classid, palette_type);
                U.MakeTransparent(bitmap);

                uint x, y;
                if (isFE6)
                {
                    x = Program.RAM.u8(addr + 14);
                    y = Program.RAM.u8(addr + 15);
                }
                else
                {
                    x = Program.RAM.u8(addr + 16);
                    y = Program.RAM.u8(addr + 17);
                }

                MapPictureBox.StaticItem st = new MapPictureBox.StaticItem();
                st.bitmap = bitmap;
                st.x = (int)x;
                st.y = (int)y;
                st.draw_x_add = 0;
                st.draw_y_add = 0;

                MapPictureBox.SetStaticItem(U.To0xHexString(addr), st.x, st.y, st.bitmap, st.draw_x_add, st.draw_y_add);
            }
        }
        int GetShowPartyClassPaletteType(uint unit_number)
        {
            if (unit_number < 0x40)
            {//Player
                return 0;
            }
            if (unit_number < 0x80)
            {//NPC
                return 1;
            }
            if (unit_number < 0xC0)
            {//ENEMY
                return 2;
            }
            if (PatchUtil.SearchCache_FourthAllegiance() == PatchUtil.FourthAllegiance_extends.FourthAllegiance)
            {
                //第4の忠誠
                return 3;
            }
            //ENEMY
            return 2;
        }

        private void MapPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            int clickx = MapPictureBox.CursolToTile(e.X);
            int clicky = MapPictureBox.CursolToTile(e.Y);

            this.ReWriteValueX.Value = clickx;
            this.ReWriteValueY.Value = clicky;
        }

        private void MapPictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MapPictureBox_MouseClick(sender, e);
            this.ReWriteButton.PerformClick();
        }

        private void ReWriteValueX_ValueChanged(object sender, EventArgs e)
        {
            uint x = (uint)this.ReWriteValueX.Value;
            uint y = (uint)this.ReWriteValueY.Value;
            this.MapPictureBox.SetPoint("", (int)x, (int)y);
        }

    }
}
