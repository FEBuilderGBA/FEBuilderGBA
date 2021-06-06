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
    public partial class MapSettingForm : Form
    {
        public MapSettingForm()
        {
            InitializeComponent();

            InputFormRef.markupJumpLabel(X_MAPSTYLE_CHANGE);
            MapPictureBox.HideCommandBar();
            MapPictureBox.SetPointIcon("L_143_MAPXY_144", ImageSystemIconForm.ExitPoint());
            U.ConvertComboBox(InputFormRef.MakeTerrainSet(), ref L_19_COMBO , true);
            this.InputFormRef = Init(this);
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);

            uint shinan = PatchUtil.SearchShinanTablePatch();
            if (shinan != U.NOT_FOUND)
            {
                InputFormRef.markupJumpLabel(X_JUMP_SHINAN);
                X_JUMP_SHINAN.Show();
            }
        }
        private void MapSettingForm_Load(object sender, EventArgs e)
        {
            //章拡張を表示するかどうか
            if (MapSettingForm.IsShowChapterExetdns(this.AddressList.Items.Count))
            {
                AddressListExpandsButton_255.Show();
            }
            else
            {
                this.AddressList.Height += AddressListExpandsButton_255.Height;
                AddressListExpandsButton_255.Hide();
            }
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(MapSettingForm self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.map_setting_pointer()
                , Program.ROM.RomInfo.map_setting_datasize()
                , (int i, uint addr) =>
                {
                    return IsMapSettingEnd(addr);
                }
                , (int i, uint addr) =>
                {
                    return U.ToHexString(i) + " " + GetMapNameWhereAddr(addr);
                }
                );
        }
        static bool IsMapSettingEnd(uint addr)
        {
            uint a = Program.ROM.u32(addr + 0);
            if (U.isPointer(a))
            {
                return true;
            }

            if (a == 0)
            {//CPを0にしている人対策
                uint weather = Program.ROM.u8(addr + 12);
                if (weather >= 0xE)
                {//天気で未知のデータがある
                    return false;
                }
                uint plist_direct = Program.ROM.u32(addr + 4);
                if (plist_direct == 0 || plist_direct == 0xFFFFFFFF)
                {//PLSITとして読めない
                    //念のためもう一つチェック
                    plist_direct = Program.ROM.u32(addr + 8);
                    if (plist_direct == 0 || plist_direct == 0xFFFFFFFF)
                    {//PLSITとして読めない
                        return false;
                    }
                }
                uint textmax = TextForm.GetDataCount();
                uint map1 = Program.ROM.u16(addr + 0x70);
                if (map1 >= textmax)
                {
                    return false;
                }
                uint map2 = Program.ROM.u16(addr + 0x72);
                if (map2 >= textmax)
                {
                    return false;
                }
                uint clearcond1 = Program.ROM.u16(addr + 0x88);
                if (clearcond1 >= textmax)
                {
                    return false;
                }
                uint clearcond2 = Program.ROM.u16(addr + 0x8A);
                if (clearcond2 >= textmax)
                {
                    return false;
                }
                return true;
            }
            return false;
        }


        static String ChaptereToString(uint chaptere)
        {
            if (U.isEven(chaptere))
            {
                return "Ch" + (chaptere / 2).ToString();
            }
            else
            {
                return "Ch" + (chaptere / 2).ToString() + "x";
            }
        }

        public static String GetMapNameWhereAddr(uint addr)
        {
            uint id;
            if (Program.ROM.RomInfo.version() == 6)
            {//FE6
                id = Program.ROM.u16(addr + 56);
                return FETextDecode.Direct(id);
            }

            String map_cp = "";
            uint chaptere = Program.ROM.u8(addr + 128);
            if (chaptere > 0)
            {
                map_cp = ChaptereToString(chaptere);
            }

            id = Program.ROM.u16(addr + 112);
            return map_cp + " " + TextForm.Direct(id);
        }
        public static String GetMapName(uint id)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return "";
            }
            return GetMapNameWhereAddr(addr);
        }
        public static uint GetMapAddr(uint id)
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.IDToAddr(id);
        }


        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint obj_plist = (uint)W4.Value;
            uint palette_plist = (uint)B6.Value;
            uint config_plist = (uint)B7.Value;
            uint mappointer_plist = (uint)B8.Value;
            MapPictureBox.LoadMap(ImageUtilMap.DrawMap(obj_plist, palette_plist, config_plist, mappointer_plist));
        }

        public class MapAnimations
        {
            public byte[] change_bitmap_bytes;
            public uint change_palette_start_index;
            public byte[] change_palette_bytes; 
        };

        public static Bitmap DrawMap(uint id, MapAnimations anime = null)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(id);
            if ( !U.isSafetyOffset(addr) )
            {
                return ImageUtil.BlankDummy(16);
            }

            uint obj_plist = (uint)Program.ROM.u16(addr+4);
            uint palette_plist = (uint)Program.ROM.u8(addr+6);
            uint config_plist = (uint)Program.ROM.u8(addr+7);
            uint mappointer_plist = (uint)Program.ROM.u8(addr + 8);
            return ImageUtilMap.DrawMap(obj_plist, palette_plist, config_plist, mappointer_plist, anime);
        }
        public static Bitmap DrawMap(uint id, uint obj_plist, uint palette_plist, uint config_plist, MapAnimations anime = null)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return ImageUtil.BlankDummy(16);
            }

            uint mappointer_plist = (uint)Program.ROM.u8(addr + 8);
            return ImageUtilMap.DrawMap(obj_plist, palette_plist, config_plist, mappointer_plist, anime);
        }
        public static Bitmap DrawMapStyle(uint id, MapAnimations anime = null)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return ImageUtil.BlankDummy(16);
            }

            uint obj_plist = (uint)Program.ROM.u16(addr + 4);
            uint palette_plist = (uint)Program.ROM.u8(addr + 6);
            uint config_plist = (uint)Program.ROM.u8(addr + 7);
            uint mappointer_plist = (uint)Program.ROM.u8(addr + 8);
            return ImageUtilMap.DrawMap(obj_plist, palette_plist, config_plist, mappointer_plist, anime);
        }

        public static Bitmap DrawMapChange(uint id, int width, int height, uint change_address, MapAnimations anime = null)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(id);
            if (!U.isSafetyOffset(addr))
            {
                return ImageUtil.BlankDummy();
            }
            if (width <= 0 || height <= 0)
            {
                return ImageUtil.BlankDummy();
            }

            uint obj_plist = (uint)Program.ROM.u16(addr + 4);
            uint palette_plist = (uint)Program.ROM.u8(addr + 6);
            uint config_plist = (uint)Program.ROM.u8(addr + 7);
            return ImageUtilMap.DrawChangeMap(obj_plist, palette_plist, config_plist, width, height, change_address, anime);
        }

        //PLSITからマップIDを検索します.
        public static List<uint> GetMapIDsWherePlist(MapPointerForm.PLIST_TYPE type, uint plist)
        {
            Debug.Assert(plist >= 1);

            List<uint> useMapID = new List<uint>();
            InputFormRef InputFormRef = Init(null);

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++)
            {
                uint obj_plist = (uint)Program.ROM.u32(addr + 4);
                uint palette_plist = (uint)Program.ROM.u8(addr + 6);
                uint config_plist = (uint)Program.ROM.u8(addr + 7);
                uint mappointer_plist = (uint)Program.ROM.u8(addr + 8);
                uint anime1_plist = (uint)Program.ROM.u8(addr + 9);
                uint anime2_plist = (uint)Program.ROM.u8(addr + 10);
                uint mapchange_plist = (uint)Program.ROM.u8(addr + 11);

                uint event_plist = Program.ROM.u8(addr + Program.ROM.RomInfo.map_setting_event_plist_pos());
                uint worldmapevent_plist = Program.ROM.u8(addr + Program.ROM.RomInfo.map_setting_worldmap_plist_pos());

                if (type == MapPointerForm.PLIST_TYPE.UNKNOWN)
                {//UNKNOWNだと全部知らべます
                    if ((obj_plist & 0xff) == plist
                        || ((obj_plist >> 8)&0xff) == plist
                        || palette_plist == plist
                        || config_plist == plist
                        || mappointer_plist == plist
                        || anime1_plist == plist
                        || anime2_plist == plist
                        || mapchange_plist == plist
                        || event_plist == plist
                        || worldmapevent_plist == plist
                        )
                    {
                        useMapID.Add((uint)i);
                    }
                }
                else if (type == MapPointerForm.PLIST_TYPE.ANIMATION)
                {
                    if ( anime1_plist == plist)
                    {
                        useMapID.Add((uint)i);
                    }
                }
                else if (type == MapPointerForm.PLIST_TYPE.ANIMATION2)
                {
                    if (anime2_plist == plist)
                    {
                        useMapID.Add((uint)i);
                    }
                }
                else if (type == MapPointerForm.PLIST_TYPE.CHANGE)
                {
                    if (mapchange_plist == plist)
                    {
                        useMapID.Add((uint)i);
                    }
                }
                else if (type == MapPointerForm.PLIST_TYPE.CONFIG)
                {
                    if (config_plist == plist)
                    {
                        useMapID.Add((uint)i);
                    }
                }
                else if (type == MapPointerForm.PLIST_TYPE.EVENT)
                {
                    if (event_plist == plist)
                    {
                        useMapID.Add((uint)i);
                    }
                }
                else if (type == MapPointerForm.PLIST_TYPE.WORLDMAP_FE6ONLY)
                {
                    if (worldmapevent_plist == plist)
                    {
                        useMapID.Add((uint)i);
                    }
                }
                else if (type == MapPointerForm.PLIST_TYPE.PALETTE)
                {
                    if (palette_plist == plist)
                    {
                        useMapID.Add((uint)i);
                    }
                }
                else if (type == MapPointerForm.PLIST_TYPE.MAP)
                {
                    if (mappointer_plist == plist)
                    {
                        useMapID.Add((uint)i);
                    }
                }
                else if (type == MapPointerForm.PLIST_TYPE.OBJECT)
                {
                    if ((obj_plist & 0xff) == plist
                        || ((obj_plist >> 8) & 0xff) == plist
                        )
                    {
                        useMapID.Add((uint)i);
                    }
                }
                
                addr += InputFormRef.BlockSize;
            }
            return useMapID;
        }
        //MAPCHANGEのPLISTの書き込み
        public static bool WriteMapChangePlist(uint mapid, uint plist, Undo.UndoData undodata)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(mapid);
            if (!U.isSafetyOffset(addr))
            {
                return false;
            }
            undodata.list.Add(new Undo.UndoPostion(addr + 11,1));
            Program.ROM.write_u8(addr + 11,plist);

            return true;
        }

        public class PLists
        {
            public uint obj_plist;
            public uint palette_plist;
            public uint config_plist;
            public uint mappointer_plist;
            public uint mapchange_plist;
            public uint anime1_plist;
            public uint anime2_plist;

            public uint event_plist;

            public uint palette2_plist; //パッチで拡張されたplist
        };

        //各種Plist一括取得.
        public static PLists GetMapPListsWhereMapID(uint mapid )
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(mapid);
            return GetMapPListsWhereAddr(addr);
        }

        //各種Plist一括取得.
        public static PLists GetMapPListsWhereAddr(uint addr)
        {
            PLists plists = new PLists();

            if (!U.isSafetyOffset(addr))
            {
                return plists;
            }
            plists.obj_plist = (uint)Program.ROM.u16(addr + 4);
            plists.palette_plist = (uint)Program.ROM.u8(addr + 6);
            plists.config_plist = (uint)Program.ROM.u8(addr + 7);
            plists.mappointer_plist = (uint)Program.ROM.u8(addr + 8);
            plists.anime1_plist = (uint)Program.ROM.u8(addr + 9);
            plists.anime2_plist = (uint)Program.ROM.u8(addr + 10);
            plists.mapchange_plist = (uint)Program.ROM.u8(addr + 11);

            plists.event_plist = Program.ROM.u8(addr + Program.ROM.RomInfo.map_setting_event_plist_pos());
            //            //FE6だけ worldmapは plistなんだけど。。。 別ルーチンでやっているからいれないことにする.
            //            plists.worldmapevent_plist = Program.ROM.u8(addr + Program.ROM.RomInfo.map_setting_worldmap_plist_pos());

            //マップ第2パレット
            PatchUtil.MapSecondPalette_extends secondPalette = PatchUtil.SearchFlag0x28ToMapSecondPalettePatch();
            if (secondPalette == PatchUtil.MapSecondPalette_extends.Flag0x28_146)
            {
                plists.palette2_plist = (uint)Program.ROM.u8(addr + 146);
            }
            else if (secondPalette == PatchUtil.MapSecondPalette_extends.Flag0x28_45)
            {
                plists.palette2_plist = (uint)Program.ROM.u8(addr + 45);
            }

            return plists;
        }


        //マップIDからイベントaddrを取得する.
        public static uint GetEventAddrWhereMapID(uint mapid,out uint pointer)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(mapid);
            if (!U.isSafetyOffset(addr))
            {
                pointer = U.NOT_FOUND;
                return U.NOT_FOUND;
            }

            uint event_plist = Program.ROM.u8(addr + Program.ROM.RomInfo.map_setting_event_plist_pos());
            uint eventaddr = MapPointerForm.PlistToOffsetAddrFast(MapPointerForm.PLIST_TYPE.EVENT, event_plist, out pointer);
            return eventaddr;
        }
        public static uint GetEventAddrWhereMapID(uint mapid)
        {
            uint pointer;
            return GetEventAddrWhereMapID(mapid,out pointer);
        }
        //マップIDからマップ変化addrを取得する.
        public static uint GetMapChangeAddrWhereMapID(uint mapid)
        {
            uint out_pointer;
            return GetMapChangeAddrWhereMapID( mapid , out out_pointer);
        }

        //マップIDからマップ変化addrを取得する.
        public static uint GetMapChangeAddrWhereMapID(uint mapid,out uint out_pointer)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(mapid);
            if (!U.isSafetyOffset(addr))
            {
                out_pointer = U.NOT_FOUND;
                return U.NOT_FOUND;
            }
            uint mapchange_plist = (uint)Program.ROM.u8(addr + 11);
            uint mapchangeaddr = MapPointerForm.PlistToOffsetAddrFast(MapPointerForm.PLIST_TYPE.CHANGE, mapchange_plist,out out_pointer);
            if (!U.isSafetyOffset(mapchangeaddr))
            {
                out_pointer = U.NOT_FOUND;
                return U.NOT_FOUND;
            }

            return mapchangeaddr;
        }


        public static string GetMapNameFromMapNo(uint map_no)
        {
            InputFormRef InputFormRef = Init(null);
            List<uint> selectAddress = new List<uint>();
            uint addr = (uint)InputFormRef.BaseAddress;
            int limitsize = (int)InputFormRef.DataCount;

            for (int i = 0; i < limitsize; i++)
            {
                uint no = (uint)Program.ROM.u8(addr + 14);
                if (map_no == no)
                {
                    return GetMapNameWhereAddr(addr);
                }
                addr += InputFormRef.BlockSize;
            }
            return "";
        }
        public static uint GetWorldMapEventIDWhereMapID(uint mapid)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(mapid);
            if (!U.isSafetyOffset(addr))
            {
                return U.NOT_FOUND;
            }
            return GetWorldMapEventIDWhereAddr(addr);
        }

        //ワールドマップイベントIDを取得する.
        public static uint GetWorldMapEventIDWhereAddr(uint addr)
        {//FE6だけPLIST
            uint worldmapevent_plist = Program.ROM.u8(addr + Program.ROM.RomInfo.map_setting_worldmap_plist_pos());
            return worldmapevent_plist;
        }

        public static string GetMapNameFromWorldMapEventID(uint worldmapEventID)
        {
            InputFormRef InputFormRef = Init(null);
            List<uint> selectAddress = new List<uint>();
            uint addr = (uint)InputFormRef.BaseAddress;
            int limitsize = (int)InputFormRef.DataCount;

            for (int i = 0; i < limitsize; i++)
            {
                uint no = GetWorldMapEventIDWhereAddr(addr);
                if (worldmapEventID == no)
                {
                    return GetMapNameWhereAddr(addr);
                }
                addr += InputFormRef.BlockSize;
            }
            return "";
        }


        //マップ名List.
        public static List<U.AddrResult> MakeMapIDList()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList();
        }
        //マップ名List.
        public static List<U.AddrResult> MakeMapIDList(Func<uint, bool> condCallback)
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList(condCallback);
        }

        public static uint GetDataCounte()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.DataCount;
        }


        public static string GetMapNameAndANYIFOVER(uint mapid)
        {
            if (mapid >= GetDataCounte())
            {
                return R._("ANY");
            }
            return GetMapName(mapid);
        }
        public static string GetMapNameAndANYFE(uint mapid)
        {
            if (mapid == 0xFE)
            {
                return R._("ANY");
            }
            return GetMapName(mapid);
        }
        public static string GetMapNameAndANYFF(uint mapid)
        {
            if (Program.ROM.RomInfo.version() >= 8)
            {
                if (mapid == 0xFF)
                {
                    return R._("ANY");
                }
            }
            else
            {
                if (mapid == 0x45)
                {
                    return R._("ANY");
                }
            }
            return GetMapName(mapid);
        }

        public static string GetMapNameAndANY45_FE7(uint mapid)
        {
            if (mapid == 0x45)
            {
                return R._("ANY");
            }
            return GetMapName(mapid);
        }

        public static uint GetDataCount()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.DataCount;
        }

        public static bool Write_MapStyle(uint mapid
                    , uint obj_plist
                    , uint palette_plist
                    , uint config_plist
                    , uint anime1_plist
                    , uint anime2_plist
                    , Undo.UndoData undodata)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(mapid);
            if (!U.isSafetyOffset(addr))
            {
                return false;
            }
            undodata.list.Add(new Undo.UndoPostion(addr, InputFormRef.BlockSize));
            Program.ROM.write_u16(addr + 4, obj_plist);
            Program.ROM.write_u8(addr + 6, palette_plist);
            Program.ROM.write_u8(addr + 7, config_plist);
            Program.ROM.write_u8(addr + 9, anime1_plist);
            Program.ROM.write_u8(addr + 10, anime2_plist);
            return true;
        }


        //輸送体の位置(FE7のみ)
        public static Point GetTransporter(uint mapid, bool isElwood)
        {
            if (Program.ROM.RomInfo.version() != 7)
            {
                return new Point(65535, 65535);
            }
            if (Program.ROM.RomInfo.is_multibyte())
            {
                return MapSettingFE7Form.GetTransporter(mapid, isElwood);
            }
            else
            {
                return MapSettingFE7UForm.GetTransporter(mapid, isElwood);
            }
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "MapSetting";

            InputFormRef InputFormRef = Init(null);
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, name, new uint[] { 0 });

            uint addr = InputFormRef.BaseAddress;
            for (int i = 0; i < InputFormRef.DataCount; i++ , addr += InputFormRef.BlockSize)
            {
                FEBuilderGBA.Address.AddCString(list
                    , addr + 0);
            }
        }

        public static string GetExplainClearButShowOnly()
        {
            return R._("ただし、表示するだけです。この値で何を指定しても、ゲームには影響を与えません。\r\n例えば、この値で、制圧を指定したとしても、\r\n実際のイベントが、ボス撃破でクリアならば、ボス撃破でクリアになります。\r\n\r\n実際の判定には、イベントが利用されます。\r\n") + GetExplainClearFlag03();
        }
        public static string GetExplainClearFlag03()
        {
            return R._("FEでは、フラグ0x03の制圧フラグを有効にして、終了イベントを呼ぶと、クリアになります。\r\n");
        }
        public static string GetExplainPLIST()
        {
            return R._("この値は、MAP POINTERテーブルを参照するIDです。(PLIST)\r\n");
        }
        public static string GetExplainMapStyle()
        {
            return R._("obj,pal,configの3つがマップチップの主要な構成要素です。\r\nここに、タイルアニメーションの2つの指定が入り、マップチップセットが完成します。\r\nマップチップセットは、マップスタイルエディタから、変更できます。\r\n");
        }

        public static string GetExplainMapChipset3()
        {
            return R._("obj,pal,configの3つがマップチップセットの主要な構成要素です。\r\nマップチップセットは、マップスタイルエディタから、変更できます。\r\n");
        }
        //エラー検出
        public static void MakeCheckErrors(uint mapid, List<FELint.ErrorSt> errors)
        {
            uint mapaddr = GetMapAddr(mapid);
            if (mapaddr == U.NOT_FOUND)
            {
                return;
            }

            PLists plists = GetMapPListsWhereAddr(mapaddr);
            {
                uint obj2_plist = (plists.obj_plist >> 8) & 0xFF; //FE8にはないが FE7は、 plistを2つ設定できることがある.
                uint obj2_offset = 0;

                uint obj_offset = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.OBJECT, plists.obj_plist & 0xFF);
                if (!U.isSafetyOffset(obj_offset))
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.MAPSETTING_PLIST_OBJECT, mapaddr
                        , R._("PLIST({0})が無効です", plists.obj_plist)));
                }
                else
                {
                    FELint.CheckLZ77ImageErrors(obj_offset, errors, FELint.Type.MAPSETTING_PLIST_OBJECT, mapaddr, 256, 16);
                }

                if (obj2_plist > 0)
                {//plist2があれば
                    obj2_offset = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.OBJECT, obj2_plist);
                    if (!U.isSafetyOffset(obj2_offset))
                    {
                        errors.Add(new FELint.ErrorSt(FELint.Type.MAPSETTING_PLIST_OBJECT, mapaddr
                            , R._("PLIST({0})が無効です", obj2_plist)));
                    }
                    else
                    {
                        FELint.CheckLZ77ImageErrors(obj2_offset, errors, FELint.Type.MAPSETTING_PLIST_OBJECT, mapaddr, 256, 16);
                    }
                }
            }
            {
                uint config_offset = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.CONFIG, plists.config_plist);
                if (!U.isSafetyOffset(config_offset))
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.MAPSETTING_PLIST_CONFIG, mapaddr
                        , R._("PLIST({0})が無効です", plists.config_plist)));
                }
                else
                {
                    FELint.CheckLZ77(config_offset, errors, FELint.Type.MAPSETTING_PLIST_CONFIG, mapaddr);
                }
            }
            {
                uint map_offset = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.MAP, plists.mappointer_plist);
                if (!U.isSafetyOffset(map_offset))
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.MAPSETTING_PLIST_MAP, mapaddr
                        , R._("PLIST({0})が無効です", plists.mappointer_plist)));
                }
                else
                {
                    string error;
                    Size mapsize = ImageUtilMap.GetMapSize(plists.obj_plist, plists.palette_plist, plists.config_plist, plists.mappointer_plist, out error);
                    if (mapsize.Width <= 0 || mapsize.Height <= 0)
                    {
                        //LZ77に失敗してそうだからチェックする
                        FELint.CheckLZ77(map_offset, errors, FELint.Type.MAPSETTING_PLIST_MAP, mapaddr);
                        errors.Add(new FELint.ErrorSt(FELint.Type.MAPSETTING_PLIST_MAP, mapaddr
                            , R._("マップが破損しています。{0}", error)
                            , mapid));
                    }

                    uint limitWidth = ImageUtilMap.GetLimitMapWidth(mapsize.Height);
                    if (mapsize.Width < ImageUtilMap.MAP_MIN_WIDTH || mapsize.Height < ImageUtilMap.MAP_MIN_HEIGHT)
                    {
                        if (IsFreliaCasle(mapid))
                        {
                            //nop
                            //フレリア城
                        }
                        else
                        {
                            errors.Add(new FELint.ErrorSt(FELint.Type.MAPSETTING_PLIST_MAP, mapaddr
                                , R._("マップが狭すぎます。現在のサイズ:({0},{1}) 利用できる最少サイズ:({2},{3})"
                                , mapsize.Width, mapsize.Height, ImageUtilMap.MAP_MIN_WIDTH, ImageUtilMap.MAP_MIN_HEIGHT)
                                , mapid));
                        }
                    }
                    else if (limitWidth == 0)
                    {
                        errors.Add(new FELint.ErrorSt(FELint.Type.MAPSETTING_PLIST_MAP, mapaddr
                                , R._("マップが広すぎます。\r\n現在のサイズ({0},{1})", mapsize.Width, mapsize.Height, limitWidth)
                                , mapid));
                    }
                    else if (mapsize.Width > limitWidth)
                    {
                        errors.Add(new FELint.ErrorSt(FELint.Type.MAPSETTING_PLIST_MAP, mapaddr
                                , R._("マップが広すぎます。\r\n現在のサイズ({0},{1})\r\nこの幅だと、利用可能な高さは、幅は{2}までです。", mapsize.Width, mapsize.Height, limitWidth)
                                ,mapid));
                    }
                }
            }
            {
                uint pal_offset = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.PALETTE, plists.palette_plist);
                if (!U.isSafetyOffset(pal_offset))
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.MAPSETTING_PLIST_PALETTE, mapaddr
                        , R._("PLIST({0})が無効です", plists.palette_plist), mapid));
                }
            }

            if (plists.mapchange_plist != 0 )
            {
                uint mapchange_offset = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.CHANGE, plists.mapchange_plist);
                if (!U.isSafetyOffset(mapchange_offset))
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.MAPSETTING_PLIST_MAPCHANGE, mapaddr
                        , R._("PLIST({0})が無効です", plists.mapchange_plist), mapid));
                }
            }
            if (plists.anime1_plist != 0)
            {
                uint anime1_offset = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.ANIMATION, plists.anime1_plist);
                if (!U.isSafetyOffset(anime1_offset))
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.MAPSETTING_PLIST_ANIMETION1, mapaddr
                        , R._("PLIST({0})が無効です", plists.anime1_plist), mapid));
                }
            }
            if (plists.anime2_plist != 0)
            {
                uint anime2_offset = MapPointerForm.PlistToOffsetAddr(MapPointerForm.PLIST_TYPE.ANIMATION2, plists.anime2_plist);
                if (!U.isSafetyOffset(anime2_offset))
                {
                    errors.Add(new FELint.ErrorSt(FELint.Type.MAPSETTING_PLIST_ANIMETION2, mapaddr
                        , R._("PLIST({0})が無効です", plists.anime2_plist), mapid));
                }
            }
        }

        public static bool IsFreliaCasle(uint mapid)
        {
            return Program.ROM.RomInfo.version() == 8 && mapid == 0x38;
        }

        private void W4_ValueChanged(object sender, EventArgs e)
        {
            if (InputFormRef.IsUpdateLock)
            {
                return;
            }
            AddressList_SelectedIndexChanged(sender, e);
        }

        private void X_MAPSTYLE_CHANGE_Click(object sender, EventArgs e)
        {
            MapStyleEditorAppendPopupForm f = (MapStyleEditorAppendPopupForm)InputFormRef.JumpFormLow<MapStyleEditorAppendPopupForm>();
            f.Init((uint)W4.Value, (uint)B6.Value, (uint)B7.Value, (uint)B8.Value, (uint)B9.Value, (uint)B10.Value);
            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            W4.Value = f.GetOBJPLIST();
            B6.Value = f.GetPALPLIST();
            B7.Value = f.GetCONFIGPLIST();
            B9.Value = f.GetANIME1PLIST();
            B10.Value = f.GetANIME2PLIST();
        }
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            InputFormRef InputFormRef = Init(null);
            UseValsID.AppendTextID(list, FELint.Type.MAPSETTING, InputFormRef, new uint[] { 112,114,136,138 });
            if (PatchUtil.IsPreparationBGMByChapter())
            {
                UseValsID.AppendSongID(list, FELint.Type.MAPSETTING, InputFormRef, new uint[] { 22, 24, 26, 28, 30, 32, 34, 36, 38, 40, 42 });
            }
            else
            {
                UseValsID.AppendSongID(list, FELint.Type.MAPSETTING, InputFormRef, new uint[] { 22, 24, 26, 28, 30, 32, 34, 36, 40, 42 });
            }
        }

        private void X_JUMP_SHINAN_Click(object sender, EventArgs e)
        {
            uint shinan = PatchUtil.SearchShinanTablePatch();
            if (shinan == U.NOT_FOUND)
            {
                return;
            }
            if (this.AddressList.SelectedIndex < 0)
            {
                return;
            }
            uint selected = (uint)this.AddressList.SelectedIndex;
            uint addr = shinan + (selected * 4);
            if ( !U.isSafetyOffset(addr) )
            {
                return;
            }
            uint eventAddr = Program.ROM.p32(addr);
            if ( !U.isSafetyOffset(eventAddr))
            {
                return;
            }

            EventScriptForm f = (EventScriptForm)InputFormRef.JumpForm<EventScriptForm>(U.NOT_FOUND);
            f.JumpTo(eventAddr);
        }

        public static bool IsShowChapterExetdns(int count)
        {
            if (count > Program.ROM.RomInfo.map_default_count())
            {//拡張している場合、表示する
                return true;
            }
            return (OptionForm.show_chapter_extends() == OptionForm.show_extends_enum.Show);
        }

        //カットシーン用のマップかどうか判定します.
        //カットシーンとは、DetailClearConditonが0に設定されているマップとします
        public static bool IsCutCutsceneMapID(uint mapid)
        {
            InputFormRef InputFormRef = Init(null);
            uint addr = InputFormRef.IDToAddr(mapid);
            if (!U.isSafetyOffset(addr))
            {//不明
                return false;
            }

            uint clear_cond_text_id = Program.ROM.u16(addr + Program.ROM.RomInfo.map_setting_clear_conditon_text_pos());
            if (clear_cond_text_id == 0)
            {
                return true;
            }
            uint nametext_id = Program.ROM.u16(addr + Program.ROM.RomInfo.map_setting_name_text_pos());
            if (nametext_id == 0)
            {
                return true;
            }
            return false;
        }

        private void B18_ValueChanged(object sender, EventArgs e)
        {
            if (B18.Value != 0x7)
            {
                J_18.ErrorMessage = "";
                return;
            }

            PatchUtil.HPBar_enum hpbar = PatchUtil.SearchHPBarPatch();
            if (hpbar != PatchUtil.HPBar_enum.HPBar)
            {
                J_18.ErrorMessage = "";
                return;
            }

            J_18.ErrorMessage = R._("HPBarを表示するパッチをインストールしている時には、表示が乱れるので天気を曇りには設定しないでください。");
        }
    }
}
