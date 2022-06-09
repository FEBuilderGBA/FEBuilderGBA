using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections.Concurrent;


namespace FEBuilderGBA
{
    public partial class MapPointerForm : Form
    {
        public MapPointerForm()
        {
            InitializeComponent();

            PListSplitsExpandsExplainLabel.Text = ExplainPlistSplitExpands();

            FilterComboBox.BeginUpdate();
            FilterComboBox.Items.Add(TypeToName(PLIST_TYPE.CONFIG));
            FilterComboBox.Items.Add(TypeToName(PLIST_TYPE.OBJECT) + "," + TypeToName(PLIST_TYPE.PALETTE));
            FilterComboBox.Items.Add(TypeToName(PLIST_TYPE.ANIMATION) + "," + TypeToName(PLIST_TYPE.ANIMATION2));
            FilterComboBox.Items.Add(TypeToName(PLIST_TYPE.MAP));
            FilterComboBox.Items.Add(TypeToName(PLIST_TYPE.CHANGE));
            FilterComboBox.Items.Add(TypeToName(PLIST_TYPE.EVENT));
            if (Program.ROM.RomInfo.version == 6)
            {
                FilterComboBox.Items.Add(TypeToName(PLIST_TYPE.WORLDMAP_FE6ONLY));
            }
            FilterComboBox.EndUpdate();

            this.InputFormRef = Init(this , IsPlistSplits());
            this.InputFormRef.UseWriteProtectionID00 = true; //ID:0x00を書き込み禁止
            if (IsPlistSplits())
            {
                PListSplitsExpandsPanel.Hide();
                FilterComboBox.Show();
                FilterComboBox.SelectedIndex = 0;
            }
            else
            {
                PListSplitsExpandsPanel.Show();
                FilterComboBox.Hide();
                this.InputFormRef.ReInitPointer(GetBasePointer(PLIST_TYPE.UNKNOWN));
            }
        }



        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self, bool isPLISTSplit)
        {
            List<U.AddrResult> mapSetting = MapSettingForm.MakeMapIDList();
            uint limit;
            if (isPLISTSplit)
            {//plistはbyteで参照するため、255までしかありえない
                limit = 256;
            }
            else
            {
                limit = Program.ROM.RomInfo.map_map_pointer_list_default_size;
            }

            InputFormRef ifr = null;
            ifr = new InputFormRef(self
                , ""
                , 0
                , 4
                , (int i, uint addr) =>
                {
                    if (i == 0)
                    {
                        return true;
                    }
                    if (i >= limit)
                    {
                        return false;
                    }
                    return true;
                }
                , (int i, uint addr) =>
                {
                    if (self == null)
                    {
                        return "";
                    }

                    uint plist = (uint)i ;
                    string usermapname;
                    if (isPLISTSplit)
                    {
                        usermapname = GetPListNameSplited(plist, ifr.BaseAddress, mapSetting);
                    }
                    else
                    {
                        usermapname = GetPListNameNotSplite(plist, mapSetting);
                    }

                    return U.ToHexString(i) + " " + usermapname;
                }
                );
            return ifr;
        }
        static string GetPListNameSplited(uint plist, uint baseaddr, List<U.AddrResult> mapSetting)
        {
            //分割している場合
            if (plist == 0)
            {//0番はnull
                return "NULL";
            }
            
            MapPointerForm.PLIST_TYPE type = ConvertBaseAddrToType(baseaddr);
            for (int mapid = 0; mapid < mapSetting.Count; mapid++)
            {
                uint addr = mapSetting[mapid].addr;
                MapSettingForm.PLists plists = MapSettingForm.GetMapPListsWhereAddr(addr);
                if (plists.anime1_plist == plist && type == PLIST_TYPE.ANIMATION)
                {
                    return "ANIME1 " + MapSettingForm.GetMapNameWhereAddr(addr);
                }
                if (plists.anime2_plist == plist && type == PLIST_TYPE.ANIMATION)
                {//ANIME2はANIMEと共有
                    return "ANIME2 " + MapSettingForm.GetMapNameWhereAddr(addr);
                }
                if (plists.config_plist == plist && type == PLIST_TYPE.CONFIG)
                {
                    return "CONFIG " + MapSettingForm.GetMapNameWhereAddr(addr);
                }
                if (plists.event_plist == plist && type == PLIST_TYPE.EVENT)
                {
                    return "EVENT " + MapSettingForm.GetMapNameWhereAddr(addr);
                }
                if (plists.mapchange_plist == plist && type == PLIST_TYPE.CHANGE)
                {
                    return "MAPCHANGE " + MapSettingForm.GetMapNameWhereAddr(addr);
                }
                if (plists.mappointer_plist == plist && type == PLIST_TYPE.MAP)
                {
                    return "MAP " + MapSettingForm.GetMapNameWhereAddr(addr);
                }
                if (plists.palette_plist == plist && type == PLIST_TYPE.OBJECT)
                {//OBJECTとPALは共有
                    return "PAL " + MapSettingForm.GetMapNameWhereAddr(addr);
                }
                if (plists.palette2_plist == plist && type == PLIST_TYPE.OBJECT)
                {//OBJECTとPALは共有
                    return "PAL2 " + MapSettingForm.GetMapNameWhereAddr(addr);
                }

                uint obj_plist_low = (plists.obj_plist & 0xFF);
                uint obj_plist_high = ((plists.obj_plist >> 8) & 0xFF);
                if (obj_plist_low == plist && type == PLIST_TYPE.OBJECT)
                {
                    return "OBJ " + MapSettingForm.GetMapNameWhereAddr(addr);
                }
                if (obj_plist_high == plist && type == PLIST_TYPE.OBJECT)
                {
                    return "OBJ " + MapSettingForm.GetMapNameWhereAddr(addr);
                }
                if (Program.ROM.RomInfo.version == 6)
                {
                    uint wmapevent_plist = MapSettingForm.GetWorldMapEventIDWhereAddr(addr);
                    if (wmapevent_plist == 0 && type == PLIST_TYPE.WORLDMAP_FE6ONLY)
                    {
                        return "WMEVENT " + MapSettingForm.GetMapNameWhereAddr(addr);
                    }
                }
            }
            return "-EMPTY-";
        }

        //分割していない場合
        static string GetPListNameNotSplite(uint plist, List<U.AddrResult> mapSetting)
        {
            if (plist == 0)
            {//0番はnull
                return "NULL";
            }

            for (int mapid = 0; mapid < mapSetting.Count; mapid++)
            {
                uint addr = mapSetting[mapid].addr;
                MapSettingForm.PLists plists = MapSettingForm.GetMapPListsWhereAddr(addr);
                if (plists.anime1_plist == plist)
                {
                    return "ANIME1 " + MapSettingForm.GetMapNameWhereAddr(addr);
                }
                if (plists.anime2_plist == plist)
                {
                    return "ANIME2 " + MapSettingForm.GetMapNameWhereAddr(addr);
                }
                if (plists.config_plist == plist)
                {
                    return "CONFIG " + MapSettingForm.GetMapNameWhereAddr(addr);
                }
                if (plists.event_plist == plist)
                {
                    return "EVENT " + MapSettingForm.GetMapNameWhereAddr(addr);
                }
                if (plists.mapchange_plist == plist)
                {
                    return "MAPCHANGE " + MapSettingForm.GetMapNameWhereAddr(addr);
                }
                if (plists.mappointer_plist == plist)
                {
                    return "MAP " + MapSettingForm.GetMapNameWhereAddr(addr);
                }
                if (plists.palette_plist == plist)
                {
                    return "PAL " + MapSettingForm.GetMapNameWhereAddr(addr);
                }
                if (plists.palette2_plist == plist)
                {
                    return "PAL2 " + MapSettingForm.GetMapNameWhereAddr(addr);
                }
                uint obj_plist_low = (plists.obj_plist & 0xFF);
                uint obj_plist_high = ((plists.obj_plist >> 8) & 0xFF);
                if (obj_plist_low == plist)
                {
                    return "OBJ " + MapSettingForm.GetMapNameWhereAddr(addr);
                }
                if (obj_plist_high == plist)
                {
                    return "OBJ " + MapSettingForm.GetMapNameWhereAddr(addr);
                }
                if (Program.ROM.RomInfo.version == 6)
                {
                    uint wmapevent_plist = MapSettingForm.GetWorldMapEventIDWhereAddr(addr);
                    if (wmapevent_plist == 0)
                    {
                        return "WMEVENT " + MapSettingForm.GetMapNameWhereAddr(addr);
                    }
                }
            }
            return "UNK";
        }

        private void MapPointerForm_Load(object sender, EventArgs e)
        {
        }
        public static uint PlistToOffsetAddr(MapPointerForm.PLIST_TYPE type, uint plist)
        {
            uint pointer;
            return PlistToOffsetAddrFast(type, plist, out pointer);
        }
        public static MapPointerForm.PLIST_TYPE ConvertStringToType(string type)
        {
            if ("CONFIG" == type)
            {
                return PLIST_TYPE.CONFIG;
            }
            if ("ANIME1" == type)
            {
                return PLIST_TYPE.ANIMATION;
            }
            if ("ANIME2" == type)
            {
                return PLIST_TYPE.ANIMATION2;
            }
            if ("OBJ" == type)
            {
                return PLIST_TYPE.OBJECT;
            }
            if ("PAL" == type)
            {
                return PLIST_TYPE.PALETTE;
            }
            if ("MAP" == type)
            {
                return PLIST_TYPE.MAP;
            }
            if ("MAPCHANGE" == type)
            {
                return PLIST_TYPE.CHANGE;
            }
            if ("EVENT" == type)
            {
                return PLIST_TYPE.EVENT;
            }
            if (Program.ROM.RomInfo.version == 6)
            {
                if ("WMAPEVENT" == type)
                {
                    return PLIST_TYPE.WORLDMAP_FE6ONLY;
                }
            }
            return PLIST_TYPE.UNKNOWN;
        }

        static MapPointerForm.PLIST_TYPE ConvertBaseAddrToType(uint baseaddr)
        {
            if (Program.ROM.p32(GetBasePointer(PLIST_TYPE.CONFIG)) == baseaddr)
            {
                return PLIST_TYPE.CONFIG;
            }
            if (Program.ROM.p32(GetBasePointer(PLIST_TYPE.ANIMATION)) == baseaddr)
            {
                return PLIST_TYPE.ANIMATION;
            }
            if (Program.ROM.p32(GetBasePointer(PLIST_TYPE.ANIMATION2)) == baseaddr)
            {
                return PLIST_TYPE.ANIMATION2;
            }
            if (Program.ROM.p32(GetBasePointer(PLIST_TYPE.OBJECT)) == baseaddr)
            {
                return PLIST_TYPE.OBJECT;
            }
            if (Program.ROM.p32(GetBasePointer(PLIST_TYPE.PALETTE)) == baseaddr)
            {
                return PLIST_TYPE.PALETTE;
            }
            if (Program.ROM.p32(GetBasePointer(PLIST_TYPE.MAP)) == baseaddr)
            {
                return PLIST_TYPE.MAP;
            }
            if (Program.ROM.p32(GetBasePointer(PLIST_TYPE.CHANGE)) == baseaddr)
            {
                return PLIST_TYPE.CHANGE;
            }
            if (Program.ROM.p32(GetBasePointer(PLIST_TYPE.EVENT)) == baseaddr)
            {
                return PLIST_TYPE.EVENT;
            }
            if (Program.ROM.RomInfo.version == 6)
            {
                if (Program.ROM.p32(GetBasePointer(PLIST_TYPE.WORLDMAP_FE6ONLY)) == baseaddr)
                {
                    return PLIST_TYPE.WORLDMAP_FE6ONLY;
                }
            }
            return PLIST_TYPE.UNKNOWN;
        }

        static uint GetBasePointer(MapPointerForm.PLIST_TYPE type)
        {
            if (type == PLIST_TYPE.CONFIG)
            {
                return (Program.ROM.RomInfo.map_config_pointer);
            }
            else if (type == PLIST_TYPE.ANIMATION)
            {
                return (Program.ROM.RomInfo.map_tileanime1_pointer);
            }
            else if (type == PLIST_TYPE.ANIMATION2)
            {
                return (Program.ROM.RomInfo.map_tileanime2_pointer);
            }
            else if (type == PLIST_TYPE.OBJECT)
            {
                return (Program.ROM.RomInfo.map_obj_pointer);
            }
            else if (type == PLIST_TYPE.PALETTE)
            {
                return (Program.ROM.RomInfo.map_pal_pointer);
            }
            else if (type == PLIST_TYPE.MAP)
            {
                return (Program.ROM.RomInfo.map_map_pointer_pointer);
            }
            else if (type == PLIST_TYPE.CHANGE)
            {
                return (Program.ROM.RomInfo.map_mapchange_pointer);
            }
            else if (type == PLIST_TYPE.EVENT)
            {
                return (Program.ROM.RomInfo.map_event_pointer);
            }
            else if (type == PLIST_TYPE.WORLDMAP_FE6ONLY)
            {
                Debug.Assert(Program.ROM.RomInfo.version == 6);
                return (Program.ROM.RomInfo.map_worldmapevent_pointer);
            }

            return (Program.ROM.RomInfo.map_config_pointer);
        }

        static ConcurrentDictionary<uint, uint> PlistCache = new ConcurrentDictionary<uint, uint>();
        public static void ClearPlistCache()
        {
            PlistCache.Clear();
        }
        public static uint PlistToOffsetAddrFast(MapPointerForm.PLIST_TYPE type, uint plist, out uint out_pointer)
        {
            //PLISTは何度もスキャンするので、キャッシュする.
            uint key = ((uint)type << 24) + plist;
            uint pointer;
            uint addr;
            if (PlistCache.TryGetValue(key, out pointer))
            {
                uint p = Program.ROM.p32(pointer);
                if (p != 0)
                {
                    out_pointer = pointer;
                    return p;
                }
            }

            addr = PlistToOffsetAddr(type, plist, out pointer);
            if (addr == U.NOT_FOUND || pointer == U.NOT_FOUND)
            {
                out_pointer = pointer;
                return addr;
            }
            PlistCache[key] = pointer;

            out_pointer = pointer;
            return addr;
        }
        static uint PlistToOffsetAddr(MapPointerForm.PLIST_TYPE type, uint plist,out uint out_pointer)
        {
            InputFormRef InputFormRef = Init(null, IsPlistSplits());
            InputFormRef.ReInitPointer(GetBasePointer(type));

            uint addr = InputFormRef.IDToAddr(plist );
            if (!U.isSafetyOffset(addr))
            {
                out_pointer = U.NOT_FOUND;
                return U.NOT_FOUND;
            }

            uint p= Program.ROM.p32(addr);
            if (p == 0)
            {
                out_pointer = addr;
                return p;
            }

            if (!U.isSafetyOffset(p))
            {
                out_pointer = U.NOT_FOUND;
                return U.NOT_FOUND;
            }
            out_pointer = addr;
            return p;
        }

        //既に拡張しているか?
        public static bool IsExtendsPlist()
        {
            bool isSplited = IsPlistSplits();
            if (isSplited)
            {
                return true;
            }
            InputFormRef InputFormRef = Init(null, isSplited);

            if (InputFormRef.DataCount >= 255)
            {
                return true;
            }
            return false;
        }

        public void PushSplitButton(EventHandler callback)
        {
            PListSplitsExpands();
            callback(null, null);
        }
        public static uint GetDataCount(MapPointerForm.PLIST_TYPE type)
        {
            InputFormRef InputFormRef = Init(null, IsPlistSplits());
            InputFormRef.ReInitPointer(GetBasePointer(type));

            return InputFormRef.DataCount;
        }



        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FilterComboBox.Text == TypeToName(PLIST_TYPE.CONFIG))
            {
                this.InputFormRef.ReInitPointer(GetBasePointer(PLIST_TYPE.CONFIG));
                return;
            }

            if (FilterComboBox.Text == TypeToName(PLIST_TYPE.MAP))
            {
                this.InputFormRef.ReInitPointer(GetBasePointer(PLIST_TYPE.MAP));
                return ;
            }

            if (FilterComboBox.Text == TypeToName(PLIST_TYPE.ANIMATION) + "," + TypeToName(PLIST_TYPE.ANIMATION2))
            {
                this.InputFormRef.ReInitPointer(GetBasePointer(PLIST_TYPE.ANIMATION));
                return ;
            }
            if (FilterComboBox.Text == TypeToName(PLIST_TYPE.OBJECT) + "," + TypeToName(PLIST_TYPE.PALETTE))
            {
                this.InputFormRef.ReInitPointer(GetBasePointer(PLIST_TYPE.OBJECT));
                return;
            }

            if(FilterComboBox.Text == TypeToName(PLIST_TYPE.CHANGE))
            {
                this.InputFormRef.ReInitPointer(GetBasePointer(PLIST_TYPE.CHANGE));
                return ;
            }

            if(FilterComboBox.Text == TypeToName(PLIST_TYPE.EVENT))
            {
                this.InputFormRef.ReInitPointer(GetBasePointer(PLIST_TYPE.EVENT));
                return ;
            }
            if(FilterComboBox.Text == TypeToName(PLIST_TYPE.EVENT))
            {
                this.InputFormRef.ReInitPointer(GetBasePointer(PLIST_TYPE.EVENT));
                return ;
            }
            if(FilterComboBox.Text == TypeToName(PLIST_TYPE.WORLDMAP_FE6ONLY))
            {
                this.InputFormRef.ReInitPointer(GetBasePointer(PLIST_TYPE.WORLDMAP_FE6ONLY));
                return ;
            }
        }



        string TypeToName(PLIST_TYPE type)
        {
            if (type == PLIST_TYPE.MAP)
            {
                return R._("マップポインタ");
            }
            else if(type == PLIST_TYPE.EVENT)
            {
                return R._("イベントポインタ");
            }
            else if(type == PLIST_TYPE.CHANGE)
            {
                return R._("マップ部分変化");
            }
            else if(type == PLIST_TYPE.PALETTE)
            {
                return R._("パレット");
            }
            else if(type == PLIST_TYPE.OBJECT)
            {
                return R._("オブジェクトタイプ");
            }
            else if(type == PLIST_TYPE.ANIMATION)
            {
                return R._("タイルアニメーション1");
            }
            else if(type == PLIST_TYPE.ANIMATION2)
            {
                return R._("タイルアニメーション2");
            }
            else if(type == PLIST_TYPE.CONFIG)
            {
                return R._("チップセットタイプ");
            }
            else if(type == PLIST_TYPE.WORLDMAP_FE6ONLY)
            {
                return R._("ワールドマップイベント");
            }
            return "";
        }

        public enum PLIST_TYPE
        {
            MAP
           ,EVENT
           ,CHANGE
           ,PALETTE
           ,OBJECT
           ,ANIMATION
           ,ANIMATION2
           ,CONFIG
           ,WORLDMAP_FE6ONLY
           ,UNKNOWN
        };

        //plist先へ書き込み.
        public static bool Write_Plsit(MapPointerForm.PLIST_TYPE type,uint mappointer_plist, uint newaddr, Undo.UndoData undodata)
        {
            if (mappointer_plist == 0)
            {//PLIST==0 への書き込みはできません.
                R.ShowStopError("PLIST=0 には、書き込むことができません。\r\n常に0にする必要があります。\r\n");
                return false;
            }

            InputFormRef InputFormRef = Init(null, IsPlistSplits());
            InputFormRef.ReInitPointer(GetBasePointer(type));

            uint addr = InputFormRef.IDToAddr(mappointer_plist);
            if (!U.isSafetyOffset(addr))
            {
                return false;
            }

            Program.ROM.write_p32(addr, newaddr, undodata);
            return true;
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list,bool isPointerOnly)
        {
            InputFormRef InputFormRef = Init(null, IsPlistSplits());
            InputFormRef.ReInitPointer(GetBasePointer(PLIST_TYPE.CONFIG));
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "MAPPOINTERS", new uint[] { 0 });
            List<U.AddrResult> configList = InputFormRef.MakeList();

            InputFormRef.ReInitPointer(GetBasePointer(PLIST_TYPE.ANIMATION)); //ANIMATION2と共有
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "MAPPOINTERS_ANIMATION", new uint[] { 0 });

            InputFormRef.ReInitPointer(GetBasePointer(PLIST_TYPE.OBJECT)); //PALETTEと共有
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "MAPPOINTERS_OBJECT", new uint[] { 0 });
            List<U.AddrResult> objList = InputFormRef.MakeList();

            InputFormRef.ReInitPointer(GetBasePointer(PLIST_TYPE.MAP));
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "MAPPOINTERS_MAP", new uint[] { 0 });
            List<U.AddrResult> mapList = InputFormRef.MakeList();

            InputFormRef.ReInitPointer(GetBasePointer(PLIST_TYPE.EVENT));
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "MAPPOINTERS_EVENT", new uint[] { 0 });

            InputFormRef.ReInitPointer(GetBasePointer(PLIST_TYPE.CHANGE));
            FEBuilderGBA.Address.AddAddress(list, InputFormRef, "MAPPOINTERS_CHANGE", new uint[] { 0 });

            if (Program.ROM.RomInfo.version == 6)
            {
                InputFormRef.ReInitPointer(GetBasePointer(PLIST_TYPE.CHANGE));
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "MAPPOINTERS_WMAP_EVENT", new uint[] { 0 });
            }

            List<U.AddrResult> mapSetting = MapSettingForm.MakeMapIDList();
            for (int mapid = 0; mapid < mapSetting.Count; mapid++)
            {
                MapSettingForm.PLists plists = MapSettingForm.GetMapPListsWhereAddr(mapSetting[mapid].addr);
                //plists.mapchange_plist //別途処理
                //plists.event_plist     //別途処理
                //plists.anime1_plist;   //別途処理
                //plists.anime2_plist;   //別途処理

                if (plists.config_plist > 0 && plists.config_plist < configList.Count)
                {
                    uint pointer = configList[(int)plists.config_plist].addr;
                    string name = "MAP:" + U.To0xHexString(mapid) + " MAP_CHIPSET" + U.ToHexString(plists.config_plist);
                    FEBuilderGBA.Address.AddLZ77Pointer(list
                        , pointer + 0
                        , name
                        , isPointerOnly
                        , FEBuilderGBA.Address.DataTypeEnum.LZ77MAPCONFIG);
                }

                if (plists.mappointer_plist > 0 && plists.mappointer_plist < mapList.Count)
                {
                    uint pointer = mapList[(int)plists.mappointer_plist].addr;
                    string name = "MAP:" + U.To0xHexString(mapid) + " MAP MAR:" + U.ToHexString(plists.mappointer_plist);
                    FEBuilderGBA.Address.AddLZ77Pointer(list
                        , pointer + 0
                        , name
                        , isPointerOnly
                        , FEBuilderGBA.Address.DataTypeEnum.LZ77MAPMAR);
                }

                if (plists.palette_plist > 0 && plists.palette_plist < objList.Count)
                {
                    uint pointer = objList[(int)plists.palette_plist].addr;
                    string name = "MAP:" + U.ToHexString(mapid) + " PALETTE:" + U.ToHexString(plists.palette_plist);
                    uint size = 0x20 * MapStyleEditorForm.MAX_MAP_PALETTE_COUNT;
                    FEBuilderGBA.Address.AddPointer(list
                        , pointer + 0
                        , size
                        , name
                        , FEBuilderGBA.Address.DataTypeEnum.PAL);
                }
                if (plists.palette2_plist > 0 && plists.palette2_plist < objList.Count)
                {
                    uint pointer = objList[(int)plists.palette2_plist].addr;
                    string name = "MAP:" + U.ToHexString(mapid) + " SECOND PALETTE:" + U.ToHexString(plists.palette2_plist);
                    uint size = 0x20 * MapStyleEditorForm.MAX_MAP_PALETTE_COUNT;
                    FEBuilderGBA.Address.AddPointer(list
                        , pointer + 0
                        , size
                        , name
                        , FEBuilderGBA.Address.DataTypeEnum.PAL);
                }

                uint obj_plist_low = (plists.obj_plist & 0xFF);
                uint obj_plist_high = ((plists.obj_plist >> 8) & 0xFF);

                if (obj_plist_low > 0 && obj_plist_low < objList.Count)
                {
                    uint pointer = objList[(int)obj_plist_low].addr;
                    string name = "MAP:" + U.ToHexString(mapid) + " OBJ:" + U.ToHexString(obj_plist_low);
                    FEBuilderGBA.Address.AddLZ77Pointer(list
                        , pointer + 0
                        , name
                        , isPointerOnly
                        , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                }
                if (obj_plist_high > 0 && obj_plist_high < objList.Count)
                {
                    uint pointer = objList[(int)obj_plist_high].addr;
                    string name = "MAP:" + U.ToHexString(mapid) + " OBJ:" + U.ToHexString(obj_plist_high);
                    FEBuilderGBA.Address.AddLZ77Pointer(list
                        , pointer + 0
                        , name
                        , isPointerOnly
                        , FEBuilderGBA.Address.DataTypeEnum.LZ77IMG);
                }
            }
        }

        string ExplainPlistSplitExpands()
        {
            string text = R._("PLIST分割\r\nPLISTにはいろいろな要素が詰め込まれているので、これを要素別に分割することで、利用できるPLSIT数を増やすことができます。\r\nPLIST分割をすると自動的にサイズを0xFFまで拡張します。\r\n(この処理は危険な処理です。必ずバックアップを取ってください。)\r\n");
            if (Program.ROM.RomInfo.version == 6)
            {
                text += R._("FE6では、7倍に増やせます。\r\n1.(オブジェクト), 2.(マップチップ画像、パレット), 3.(アニメーション), 4.(マップの並び順) , 5.(マップ変更) ,6.(イベント) ,7.(ワールドマップ)\r\n");
            }
            else if (Program.ROM.RomInfo.version == 7)
            {
                text += R._("FE7では、6倍に増やせます。\r\n1.(オブジェクト), 2.(マップチップ画像、パレット), 3.(アニメーション), 4.(マップの並び順) , 5.(マップ変更) ,6.(イベント) \r\n");
            }
            else
            {
                text += R._("FE8では、6倍に増やせます。\r\n1.(オブジェクト), 2.(マップチップ画像、パレット), 3.(アニメーション), 4.(マップの並び順) , 5.(マップ変更) ,6.(イベント) \r\n");
            }
            return text;
        }

        private void PListSplitsExpandsButton_Click(object sender, EventArgs e)
        {
            bool r = PListSplitsExpands();
            if (!r)
            {
                return;
            }

            Debug.Assert(IsPlistSplits() == true);

            this.InputFormRef = Init(this,true);
            PListSplitsExpandsPanel.Hide();
            FilterComboBox.Show();
            FilterComboBox.SelectedIndex = 0;
        }

        bool PListSplitsExpands()
        {
            if (IsPlistSplits())
            {//拡張済み  普通は表示しないのでありえない.
                Debug.Assert(false);
                return false;
            }

            DialogResult dr = R.ShowNoYes("PLISTを分割してもよろしいですか？\r\n");
            if (dr != DialogResult.Yes)
            {//キャンセル.
                return false;
            }

            ClearPlistCache(); //キャッシュをクリア やらなくてもいいんだろうけど...
            Undo.UndoData undodata = Program.Undo.NewUndoData("PListSplitsExpands");

            try
            {
                uint old_object_basePointer = GetBasePointer(PLIST_TYPE.OBJECT);
                uint old_config_basePointer = GetBasePointer(PLIST_TYPE.CONFIG);
                uint old_map_basePointer = GetBasePointer(PLIST_TYPE.MAP);
                uint old_change_basePointer = GetBasePointer(PLIST_TYPE.CHANGE);
                uint old_animation_basePointer = GetBasePointer(PLIST_TYPE.ANIMATION);
                uint old_event_basePointer = GetBasePointer(PLIST_TYPE.EVENT);
                uint old_wmap_event_basePointer = 0;
                if (Program.ROM.RomInfo.version == 6)
                {
                    old_wmap_event_basePointer = GetBasePointer(PLIST_TYPE.WORLDMAP_FE6ONLY);
                }
                uint old_object_baseaddr = Program.ROM.p32(old_object_basePointer);
                uint old_config_baseaddr = Program.ROM.p32(old_config_basePointer);
                uint old_map_baseaddr = Program.ROM.p32(old_map_basePointer);
                uint old_change_baseaddr = Program.ROM.p32(old_change_basePointer);
                uint old_animation_baseaddr = Program.ROM.p32(old_animation_basePointer);
                uint old_event_baseaddr = Program.ROM.p32(old_event_basePointer);
                uint old_wmap_event_baseaddr = 0;
                if (Program.ROM.RomInfo.version == 6)
                {
                    old_wmap_event_baseaddr = Program.ROM.p32(old_wmap_event_basePointer);
                }


                PListSplitsExpandsOne(PLIST_TYPE.OBJECT, undodata); //PALと共通
                PListSplitsExpandsOne(PLIST_TYPE.CONFIG, undodata);
                PListSplitsExpandsOne(PLIST_TYPE.MAP, undodata);
                PListSplitsExpandsOne(PLIST_TYPE.CHANGE, undodata);
                PListSplitsExpandsOne(PLIST_TYPE.ANIMATION, undodata); //ANIME2と共通
                PListSplitsExpandsOne(PLIST_TYPE.EVENT, undodata);
                if (Program.ROM.RomInfo.version == 6)
                {
                    PListSplitsExpandsOne(PLIST_TYPE.WORLDMAP_FE6ONLY, undodata);
                }

                ClearOrignalData(old_object_baseaddr, undodata);
                ClearOrignalData(old_config_baseaddr, undodata);
                ClearOrignalData(old_map_baseaddr, undodata);
                ClearOrignalData(old_change_baseaddr, undodata);
                ClearOrignalData(old_animation_baseaddr, undodata);
                ClearOrignalData(old_event_baseaddr, undodata);
                ClearOrignalData(old_wmap_event_baseaddr, undodata);
                Program.Undo.Push(undodata);
            }
            catch (PLISTExpandsException ee)
            {
                Program.Undo.Rollback(undodata);
                R.ShowStopError(ee.ToString());
                return false;
            }

            return true;
        }

        class PLISTExpandsException : Exception
        {
            public PLISTExpandsException(string message)
                : base(message)
            {
            }
        };

        //元のデータをクリアします
        void ClearOrignalData(uint baseaddress, Undo.UndoData undodata)
        {
            if (!U.isSafetyOffset(baseaddress))
            {
                return;
            }

            InputFormRef InputFormRef = Init(null,false);
            InputFormRef.ReInit(baseaddress);

            if (InputFormRef.DataCount <= 1)
            {//メモリを共有しているはずなので、ひとつクリアすれば、他はないはず...
                return;
            }

            Program.ROM.write_fill(baseaddress, InputFormRef.DataCount * InputFormRef.BlockSize, 0x00, undodata);
        }

        static void PListSplitExpandsOneConvertPointer(uint plist,InputFormRef ifr,byte[] newArray)
        {
            if (plist <= 0)
            {
                return;
            }
            uint pointer = ifr.IDToAddr(plist);
            if (!U.isSafetyOffset(pointer))
            {
                return ;
            }
            uint addr = Program.ROM.p32(pointer);
            if (!U.isSafetyOffset(addr))
            {
                return;
            }
            U.write_p32(newArray, plist * 4, addr);
        }

        uint PListSplitsExpandsOne(MapPointerForm.PLIST_TYPE type,Undo.UndoData undodata)
        {
            Debug.Assert(type != PLIST_TYPE.ANIMATION2);//Animation1と一緒に処理しないといけない
            Debug.Assert(type != PLIST_TYPE.PALETTE);//OBJECTと一緒に処理しないといけない

            InputFormRef InputFormRef = Init(null,false);
            InputFormRef.ReInitPointer(GetBasePointer(type));

            byte[] newArray = new byte[4 * (256)];
            List<U.AddrResult> mapSetting = MapSettingForm.MakeMapIDList();
            int mapmax = mapSetting.Count;
            for (int mapid = 0; mapid < mapmax; mapid++)
            {
                if (type == PLIST_TYPE.WORLDMAP_FE6ONLY)
                {
                    Debug.Assert(Program.ROM.RomInfo.version == 6);
                    uint wmapevent_plist = MapSettingForm.GetWorldMapEventIDWhereAddr(mapSetting[mapid].addr);

                    PListSplitExpandsOneConvertPointer(wmapevent_plist, InputFormRef, newArray);
                    continue;
                }

                MapSettingForm.PLists plists = MapSettingForm.GetMapPListsWhereAddr(mapSetting[mapid].addr);
                if (type == PLIST_TYPE.CHANGE)
                {
                    PListSplitExpandsOneConvertPointer(plists.mapchange_plist, InputFormRef, newArray);
                }
                else if (type == PLIST_TYPE.EVENT)
                {
                    PListSplitExpandsOneConvertPointer(plists.event_plist, InputFormRef, newArray);
                }
                else if (type == PLIST_TYPE.MAP)
                {
                    PListSplitExpandsOneConvertPointer(plists.mappointer_plist, InputFormRef, newArray);
                }
                else if (type == PLIST_TYPE.CONFIG)
                {
                    PListSplitExpandsOneConvertPointer(plists.config_plist, InputFormRef, newArray);
                }
                else if (type == PLIST_TYPE.ANIMATION)
                {
                    PListSplitExpandsOneConvertPointer(plists.anime1_plist, InputFormRef, newArray);
                    PListSplitExpandsOneConvertPointer(plists.anime2_plist, InputFormRef, newArray);
                }
                else
                {
                    uint obj1_plist = (plists.obj_plist & 0xFF);
                    uint obj2_plist = (plists.obj_plist >> 8) & 0xFF; //FE8にはないが FE7は、 plistを2つ設定できることがある.

                    PListSplitExpandsOneConvertPointer(obj1_plist, InputFormRef, newArray);
                    PListSplitExpandsOneConvertPointer(obj2_plist, InputFormRef, newArray);

                    PListSplitExpandsOneConvertPointer(plists.palette_plist, InputFormRef, newArray);
                }
            }
            //終端の0xFFFFFFFFを念のため入れておきます.
//            U.write_u32(newArray, 256 * 4, U.NOT_FOUND);

            uint newpos = InputFormRef.AppendBinaryData(newArray, undodata);
            if (newpos == U.NOT_FOUND)
            {
                throw new PLISTExpandsException(R._("PLIST拡張に失敗しました。\r\ntype={0}\r\nnewaddr=NOT_FOUND",type.ToString())); 
            }

            if (type == PLIST_TYPE.CONFIG)
            {
                Program.ROM.write_p32(Program.ROM.RomInfo.map_config_pointer, newpos, undodata);
            }
            else if (type == PLIST_TYPE.ANIMATION)
            {
                Program.ROM.write_p32(Program.ROM.RomInfo.map_tileanime1_pointer, newpos, undodata);
                Program.ROM.write_p32(Program.ROM.RomInfo.map_tileanime2_pointer, newpos, undodata);
            }
            else if (type == PLIST_TYPE.OBJECT)
            {
                Program.ROM.write_p32(Program.ROM.RomInfo.map_obj_pointer, newpos, undodata);
                Program.ROM.write_p32(Program.ROM.RomInfo.map_pal_pointer, newpos, undodata);
            }
            else if (type == PLIST_TYPE.MAP)
            {
                Program.ROM.write_p32(Program.ROM.RomInfo.map_map_pointer_pointer, newpos, undodata);
            }
            else if (type == PLIST_TYPE.CHANGE)
            {
                Program.ROM.write_p32(Program.ROM.RomInfo.map_mapchange_pointer, newpos, undodata);
            }
            else if (type == PLIST_TYPE.EVENT)
            {
                Program.ROM.write_p32(Program.ROM.RomInfo.map_event_pointer, newpos, undodata);
            }
            else if (type == PLIST_TYPE.WORLDMAP_FE6ONLY)
            {
                Program.ROM.write_p32(Program.ROM.RomInfo.map_worldmapevent_pointer, newpos, undodata);
            }
            else
            {
                Debug.Assert(false);
            }
            return newpos;
        }
        public static bool IsPlistSplits()
        {
            uint a,b;
            a = Program.ROM.p32(GetBasePointer(PLIST_TYPE.CONFIG));
            b = Program.ROM.p32(GetBasePointer(PLIST_TYPE.ANIMATION));
            if(a == b)
            {
                return false;
            }
            b = Program.ROM.p32(GetBasePointer(PLIST_TYPE.OBJECT));
            if(a == b)
            {
                return false;
            }
            b = Program.ROM.p32(GetBasePointer(PLIST_TYPE.MAP));
            if(a == b)
            {
                return false;
            }
            b = Program.ROM.p32(GetBasePointer(PLIST_TYPE.CHANGE));
            if(a == b)
            {
                return false;
            }
            b = Program.ROM.p32(GetBasePointer(PLIST_TYPE.EVENT));
            if(a == b)
            {
                return false;
            }

            if (Program.ROM.RomInfo.version == 6)
            {
                b = Program.ROM.p32(GetBasePointer(PLIST_TYPE.WORLDMAP_FE6ONLY));
                if(a == b)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
