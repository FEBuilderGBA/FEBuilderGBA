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
    public partial class WorldMapPathMoveEditorForm : Form
    {
        Pen MouseCursorPen = new Pen(Color.Red, 1);
        Pen SelectMarkupPen = new Pen(Color.Blue, 3);
       
        public WorldMapPathMoveEditorForm()
        {
            InitializeComponent();
            InputFormRef = Init(this);
            InputFormRef.PostAddressListExpandsEvent += AddressListExpandsEvent;
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , 0
                , 8
                , (int i, uint addr) =>
                {
                    return Program.ROM.u32(addr) != 0xFFFFFFFF;
                }
                , (int i, uint addr) =>
                {
                  
                    return U.ToHexString(i) ;
                }
                );
        }




        private void WorldMapPathEditorForm_Load(object sender, EventArgs e)
        {
            MakePathCombo();
            MapPictureBox.SetChipSize(1);
            Bitmap icon = ImageSystemIconForm.YubiTate();
            U.MakeTransparent(icon);
            MapPictureBox.SetDefualtIcon(icon, -8, -14);
            U.SelectedIndexSafety(PathType, 0);
        }
        void MakePathCombo()
        {
            List<U.AddrResult> list = WorldMapPathForm.MakeList();
            U.ConvertComboBox(list, ref this.PathType);
        }

        void MakeWorldMap()
        {
            MapPictureBox.LoadWorldMap();
            MapPictureBox.ClearStaticItem();

            int pathid = this.PathType.SelectedIndex;
            if (pathid >= 0)
            {
                //道の描画
                List<MapPictureBox.StaticItem> list = WorldMapPathForm.DrawPath((uint)pathid);
                for (int n = 0; n < list.Count; n++)
                {
                    MapPictureBox.SetStaticItem("road" + pathid.ToString() + "_" + n.ToString(), list[n].x, list[n].y, list[n].bitmap);
                }
            }

            //拠点を追加
            List<U.AddrResult> arlist = WorldMapPointForm.MakeWorldMapPointList();
            for (int i = 0; i < arlist.Count; i++)
            {
                MapPictureBox.StaticItem item = WorldMapPointForm.DrawBasePointAddr(arlist[i].addr);
                MapPictureBox.SetStaticItem("base" + i.ToString(), item.x, item.y, item.bitmap, item.draw_x_add, item.draw_y_add);
            }

            MapPictureBox.InvalidateMap();
        }

        private void PathType_SelectedIndexChanged(object sender, EventArgs e)
        {
            MakeWorldMap();

            uint addr = InputFormRef.SelectToAddr(this.PathType);
            if (addr == U.NOT_FOUND)
            {
                return;
            }
            uint pathmove = Program.ROM.p32(addr + 8);
            if (pathmove == 0x0)
            {
                InputFormRef.ReInit(0);
                return;
            }

            if (!U.isSafetyOffset(pathmove))
            {
                return;
            }
            InputFormRef.ReInit(pathmove);
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        //リストが拡張されたとき
        void AddressListExpandsEvent(object sender, EventArgs e)
        {
            uint pathaddr = InputFormRef.SelectToAddr(PathType);

            //拡張したアドレスの先頭アドレスを知りたい
            ListBox lb = (ListBox)sender;

            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)e;
            uint addr = eearg.NewBaseAddress;
            int count = (int)eearg.NewDataCount;

            uint old_addr = Program.ROM.u32(pathaddr + 8);
            if (old_addr != 0x0)
            {//NULLでなければ、自動更新で更新されるだろう
                return;
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData(this.Text + " " + PathType.Text + " Expapnds");
            
            //null の状態から拡張したデータには、 0xFF 0xFF 0xFF 0xFF の終端データがないので追加します.
            uint term_addr = (uint)(addr + ((count) * InputFormRef.BlockSize));
            Program.ROM.write_u32(term_addr, 0xFFFFFFFF, undodata);

            //拡張したアドレスを道情報に書き込みます.
            //なぜなら、道情報は null がありうるので、関連したポインタの自動更新から外れる時があるためです.
            Program.ROM.write_p32(pathaddr + 8, addr, undodata);

            Program.Undo.Push(undodata);
        }

        public void JumpTo(uint pathid)
        {
            U.SelectedIndexSafety(PathType, pathid, false);
            U.SelectedIndexSafety(AddressList, 0, true);
        }
    }
}
