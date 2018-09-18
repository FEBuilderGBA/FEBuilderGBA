using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class WorldMapRoadForm : Form
    {
        public WorldMapRoadForm()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
        }

        InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.p32(Program.ROM.RomInfo.worldmap_road_pointer())
                , 12
                , (int i, uint addr) =>
                {
                    return ROM.isPointer(Program.ROM.u32(addr))
                        ;
                }
                , (int i, uint addr) =>
                {
                    uint basepoint1 = Program.ROM.u8(addr+4);
                    uint basepoint2 = Program.ROM.u8(addr+4+1);
                    string basepointstring1 = WorldMapPointForm.GetWorldMapPointName(basepoint1);
                    string basepointstring2 = WorldMapPointForm.GetWorldMapPointName(basepoint2);
                    return U.ToHexString(i) + " " + basepointstring1 + " -> " + basepointstring2;
                }
                );
        }

        private void WorldMapRoadForm_Load(object sender, EventArgs e)
        {

        }

        public static Bitmap GetRoadImage()
        {
            uint palette = Program.ROM.p32(Program.ROM.RomInfo.worldmap_icon_palette_pointer());

            uint image = Program.ROM.p32(Program.ROM.RomInfo.worldmap_road_tile_pointer());
            byte[] imageUZ = LZ77.decompress(Program.ROM.Data, image);
            int width = 8;
            int height = ImageUtil.CalcHeight(width, imageUZ.Length);
            Bitmap bitmap = ImageUtil.ByteToImage16Tile(width, height, imageUZ, 0, Program.ROM.Data, (int)palette,1);
            return bitmap;
        }


        public static List<MapPictureBox.StaticItem> DrawRoad(uint id)
        {
            List<MapPictureBox.StaticItem> list = new List<MapPictureBox.StaticItem>();

            Bitmap roadbase = GetRoadImage();

            InputFormRef InputFormRef = Init(null);
            //現在のIDに対応するデータ
            uint addr = InputFormRef.IDToAddr(id);
            if (!Program.ROM.isSafetyOffset(addr))
            {
                return list;
            }
            uint road = Program.ROM.p32(addr+0);

            while(true)
            {
                uint x8 = Program.ROM.u8(road + 0);
                uint y8 = Program.ROM.u8(road + 1);
                uint count = Program.ROM.u8(road + 2);

                if (x8 == 0xFF)
                {
                    return list;
                }

                road += 4;
                for (uint ix = 0; ix < count; ix++)
                {
                    uint tile = Program.ROM.u8(road+0 );
                    uint flag = Program.ROM.u8(road+1 );
                    road += 2;
                    
                    MapPictureBox.StaticItem si = new MapPictureBox.StaticItem();
                    si.x = (int)(x8 * 8 + (ix*8));
                    si.y = (int)y8 * 8;
                    if (flag == 4)
                    {
                        si.bitmap = ImageUtil.Copy(roadbase, 0, (int)tile * 8, 8, 8, false, true);
                    }
                    else if (flag == 8)
                    {
                        si.bitmap = ImageUtil.Copy(roadbase, 0, (int)tile * 8, 8, 8, true, false);
                    }
                    else if (flag == 0xC)
                    {
                        si.bitmap = ImageUtil.Copy(roadbase, 0, (int)tile * 8, 8, 8,true,true);
                    }
                    else
                    {
                        si.bitmap = ImageUtil.Copy(roadbase, 0, (int)tile * 8, 8, 8);
                    }


                    si.bitmap.MakeTransparent(si.bitmap.Palette.Entries[0]);
                    list.Add(si);
                }
            }
        }
        public static uint DataCount()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.DataCount;
        }
    }
}
