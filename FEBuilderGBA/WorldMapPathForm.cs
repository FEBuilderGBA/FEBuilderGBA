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
    public partial class WorldMapPathForm : Form
    {
        public WorldMapPathForm()
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
                , Program.ROM.RomInfo.worldmap_road_pointer
                , 12
                , (int i, uint addr) =>
                {
                    return U.isPointer(Program.ROM.u32(addr))
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

        private void WorldMapPathForm_Load(object sender, EventArgs e)
        {
            MapPictureBox.HideCommandBar();
            MapPictureBox.SetChipSize(1);
            Bitmap icon = ImageSystemIconForm.YubiTate();
            U.MakeTransparent(icon);
            MapPictureBox.SetDefaultIcon(icon, -8, -14);

            //拡張ボタンを表示するかどうか
            if (WorldMapPathForm.IsShowWorldmapPathExetdns(this.AddressList))
            {
                AddressListExpandsButton_255.Show();
            }
            else
            {
                this.AddressList.Height += AddressListExpandsButton_255.Height;
                AddressListExpandsButton_255.Hide();
            }
        }

        public static string GetPathName(uint id)
        {
            InputFormRef InputFormRef = Init(null);
            //現在のIDに対応するデータ
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return "";
            }
            uint basepoint1 = Program.ROM.u8(addr + 4);
            uint basepoint2 = Program.ROM.u8(addr + 4 + 1);
            string basepointstring1 = WorldMapPointForm.GetWorldMapPointName(basepoint1);
            string basepointstring2 = WorldMapPointForm.GetWorldMapPointName(basepoint2);
            return basepointstring1 + " -> " + basepointstring2;
        }

        public static Bitmap GetPathImage()
        {
            uint palette = Program.ROM.p32(Program.ROM.RomInfo.worldmap_icon_palette_pointer);

            uint image = Program.ROM.p32(Program.ROM.RomInfo.worldmap_road_tile_pointer);
            byte[] imageUZ = LZ77.decompress(Program.ROM.Data, image);
            int width = 8;
            int height = ImageUtil.CalcHeight(width, imageUZ.Length);
            Bitmap bitmap = ImageUtil.ByteToImage16Tile(width, height, imageUZ, 0, Program.ROM.Data, (int)palette,1);
            return bitmap;
        }


        public static List<MapPictureBox.StaticItem> DrawPath(uint id)
        {
            List<MapPictureBox.StaticItem> list = new List<MapPictureBox.StaticItem>();

            Bitmap roadbase = GetPathImage();

            InputFormRef InputFormRef = Init(null);
            //現在のIDに対応するデータ
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return list;
            }
            uint p = Program.ROM.p32(addr+0);
            if (!U.isSafetyOffset(p))
            {
                return list;
            }

            while(true)
            {
                if (!U.isSafetyOffset(p + 4))
                {
                    Log.Error("Worldmap Path Broken : " + id);
                    return list;
                }

                uint x8 = Program.ROM.u8(p + 0);
                uint y8 = Program.ROM.u8(p + 1);
                uint count = Program.ROM.u8(p + 2);

                if (x8 == 0xFF)
                {
                    return list;
                }
                if (count >= 200)
                {//ありえないデータなので打ち切る
                    Log.Error("Worldmap Path Broken : " + id);
                    return list;
                }

                p += 4;
                if (!U.isSafetyOffset(p + (count * 2)))
                {
                    Log.Error("Worldmap Path Broken : " + id);
                    return list;
                }
                for (uint ix = 0; ix < count; ix++)
                {
                    uint tile = Program.ROM.u8(p+0 );
                    uint flag = Program.ROM.u8(p+1 );
                    p += 2;
                    
                    MapPictureBox.StaticItem si = new MapPictureBox.StaticItem();
                    si.x = (int)(x8 * 8 + (ix*8));
                    si.y = (int)y8 * 8;
                    if (flag == 4)
                    {
                        si.bitmap = ImageUtil.Copy(roadbase, 0, (int)tile * 8, 8, 8, true, false);
                    }
                    else if (flag == 8)
                    {
                        si.bitmap = ImageUtil.Copy(roadbase, 0, (int)tile * 8, 8, 8, false, true);
                    }
                    else if (flag == 0xC)
                    {
                        si.bitmap = ImageUtil.Copy(roadbase, 0, (int)tile * 8, 8, 8,true,true);
                    }
                    else
                    {
                        si.bitmap = ImageUtil.Copy(roadbase, 0, (int)tile * 8, 8, 8,false,false);
                    }

                    list.Add(si);
                }
            }
        }
        public static uint DataCount()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.DataCount;
        }
        public static List<U.AddrResult> MakeList()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList();
        }

        void MakeWorldMap()
        {
            MapPictureBox.LoadWorldMap();
            MapPictureBox.ClearStaticItem();

            int pathid = this.AddressList.SelectedIndex;
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

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            MakeWorldMap();
        }
        
        public static uint CalcPathDataLength(uint addr)
        {
            if (!U.isSafetyOffset(addr))
            {
                return 0;
            }
            uint p = addr;
            while (true)
            {
                if (!U.isSafetyOffset(p + 4))
                {
                    return p - addr;
                }

                uint x8 = Program.ROM.u8(p + 0);
                uint y8 = Program.ROM.u8(p + 1);
                uint count = Program.ROM.u8(p + 2);

                p += 4;
                if (x8 == 0xFF)
                {
                    return p - addr;
                }
                p += count * 2;
            }
        }
        public static uint CalcPathMoveDataLength(uint addr)
        {
            if (!U.isSafetyOffset(addr))
            {
                return 0;
            }

            uint p = addr;
            while (true)
            {
                if (!U.isSafetyOffset(p))
                {
                    Debug.Assert(false);
                    return p - addr;
                }
                uint a = Program.ROM.u32(p);

                p += 4;
                if (a == 0xFFFFFFFF)
                {
                    return p - addr;
                }
            }
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "WorldMapPath";
            {
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 0 , 8 });

                uint p = InputFormRef.BaseAddress;
                for (int i = 0; i < InputFormRef.DataCount; i++, p += InputFormRef.BlockSize)
                {
                    uint a0 = Program.ROM.p32(p + 0);
                    uint a8 = Program.ROM.p32(p + 8);

                    if (a0 > 0)
                    {
                        name = "WorldMapPath:" + U.To0xHexString(i);
                        FEBuilderGBA.Address.AddAddress(list,a0
                            , CalcPathDataLength(a0)
                            , p + 0
                            , name
                            , FEBuilderGBA.Address.DataTypeEnum.BIN);
                    }

                    if (a8 > 0)
                    {
                        name = "WorldMapPathMove:" + U.To0xHexString(i);
                        FEBuilderGBA.Address.AddAddress(list,a8
                            , CalcPathMoveDataLength(a8)
                            , p + 8
                            , name
                            , FEBuilderGBA.Address.DataTypeEnum.POINTER);
                    }
                }
            }
        }

        public static bool IsShowWorldmapPathExetdns(ListBox list)
        {
            if (list.Items.Count > 0x20)
            {//拡張している場合、表示する
                return true;
            }
            return (OptionForm.show_worldmap_path_extends() == OptionForm.show_extends_enum.Show);
        }
    }
}
