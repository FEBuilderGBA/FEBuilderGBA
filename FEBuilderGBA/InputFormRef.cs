using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Reflection;
using System.IO;
using System.Collections.Concurrent;

namespace FEBuilderGBA
{
    //入力フォーム関係の雑多な用を全部引き受ける.
    //このクラスは邪道です.
    public class InputFormRef
    {
        public static List<Control> GetAllControls(Control self)
        {
            List<Control> list = new List<Control>();
            foreach (Control c in self.Controls)
            {
                GetAllControls(c, ref list);
            }
            return list;
        }
        static void GetAllControls(Control control, ref List<Control> list)
        {
            list.Add(control);
            foreach (Control c in control.Controls)
            {
                GetAllControls(c, ref list);
            }
        }

        public static void MakeLinkEvent(string prefix, List<Control> controls)
        {
            foreach (Control link_info in controls)
            {
                MakeLinkEventOne(prefix,controls,link_info);
            }
        }
        public static void MakeLinkEventOne(string prefix, List<Control> controls, Control link_info)
        {
            String link_name = SkipPrefixName(link_info.Name, prefix);
            if (link_name.Length <= 0)
            {
                return;
            }

            //以下のような表記になる.
            //L_数字ID部_属性部
            //L_数字ID部_属性部_引数部
            if (link_name[0] != 'L' || link_name[1] != '_')
            {
                return;
            }

            string[] sp  = link_name.Split('_');
            if (sp.Length <= 2)
            {
                return;
            }

            uint id = U.atoi(sp[1]);
            string linktype = sp[2];
            string[] args = U.subrange(sp,3,(uint)sp.Length);

            NumericUpDown src_object = FindSRCObject(prefix, controls, id, linktype);
            if (src_object == null)
            {
                throw new Exception("broken link:" + link_info.Name);
            }
            makeLinkEventHandler(prefix, controls,src_object, link_info, id, linktype, args);
        }

        public static NumericUpDown FindSRCObject(string prefix, List<Control> controls, uint searchlink, string linktype = "")
        {
            foreach (Control search_info in controls)
            {
                if (!(search_info is NumericUpDown))
                {
                    continue;
                }
                String search_name = SkipPrefixName(search_info.Name, prefix);
                if (search_name.Length < 2)
                {
                    continue;
                }

                char typeword = search_name[0];
                if (!IsTypeWord(typeword))
                {
                    continue;
                }
                if (!U.isnum(search_name[1]))
                {
                    continue;
                }
                if (IsHalfTypeWord(typeword))
                {
                    if (!CheckHalfByteLHSupport(typeword, linktype))
                    {
                        continue;
                    }
                }

                uint search_id = U.atoi(search_name.Substring(1));
                if (searchlink == search_id)
                {
                    return (NumericUpDown)search_info;
                }
            }
            return null;
        }
        public static bool IsHalfTypeWord(char c)
        {
            return c == 'l' || c == 'h';
        }
        static bool CheckHalfByteLHSupport(char c , string linktype)
        {
            if (c == 'l')
            {
                if (linktype == "COMBOl")
                {
                    return true;
                }
            }
            if (c == 'h')
            {
                if (linktype == "COMBOh")
                {
                    return true;
                }
            }
            return false;
        }

        static MapPictureBox FindMapObject(string prefix, List<Control> controls)
        {
            MapPictureBox map = (MapPictureBox)FindObject<MapPictureBox>(prefix, controls);
            if (map == null)
            {
                map = (MapPictureBox)FindObject<MapPictureBox>("", controls);
                if (map == null)
                {
                    return null;
                }
            }
            return map;
        }

        public static Control FindObject<_CLASS>(string prefix, List<Control> controls)
        {
            foreach (Control search_info in controls)
            {
                if (! (search_info is _CLASS))
                {
                    continue;
                }
                String search_name = SkipPrefixName(search_info.Name, prefix);
                if (search_name.Length <= 0)
                {
                    continue;
                }
                return search_info;
            }
            return null;
        }

        public static Control FindObject(string prefix, List<Control> controls,string name)
        {
            foreach (Control search_info in controls)
            {
                String search_name = SkipPrefixName(search_info.Name, prefix);
                if (search_name.Length <= 0)
                {
                    continue;
                }
                if (search_name != name)
                {
                    continue;
                }
                return search_info;
            }
            return null;
        }

        public static Control FindObjectByForm<_CLASS>(List<Control> controls,string name)
        {
            foreach (Control search_info in controls)
            {
                if (!(search_info is _CLASS))
                {
                    continue;
                }
                if (search_info.Name != name)
                {
                    continue;
                }
                return search_info;
            }
            return null;
        }
        public static Control FindObjectByFormFirstMatch<_CLASS>(List<Control> controls,string name)
        {
            foreach (Control search_info in controls)
            {
                if (!(search_info is _CLASS))
                {
                    continue;
                }
                if (search_info.Name.IndexOf(name) != 0)
                {
                    continue;
                }
                return search_info;
            }
            return null;
        }

        public static void makeLinkEventHandler(string prefix, List<Control> controls,NumericUpDown src_object, Control link_info, uint link_id, String linktype, String[] args)
        {
            //よく参照するarg1を変数化.
            string arg1 = U.at(args, 0);

            if (linktype == "TEXT" || linktype == "CSTRING")
            {//テキストデータとリンク.
                //元データが変更されたら、リンクデータも変更する.
                if (link_info is TextBoxEx)
                {
                    TextBoxEx link_object = ((TextBoxEx)link_info);
                    src_object.ValueChanged += (sender, e) =>
                    {
                        uint id = (uint)src_object.Value;
                        string str = TextForm.Direct(id);
                        link_object.ErrorMessage = TextForm.GetErrorMessage(str, id, arg1);
                        link_object.Text = str;
                    };
                    link_object.DoubleClick += (sender, e) =>
                    {
                        JumpTo(src_object, link_info, "TEXT", new string[] { });
                    };
                    return;
                }
            }
            if (linktype == "MAGICEX")
            {//FE8N magic patch.
                if (link_info is NumericUpDown)
                {
                    bool updateLock = false;

                    NumericUpDown link_object = ((NumericUpDown)link_info);
                    src_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        uint v = (uint)src_object.Value;
                        if (arg1 == "B0")
                        {
                            v = v & 0xFF;
                        }
                        else if (arg1 == "B1")
                        {
                            v = (v >> 8) & 0xFF;
                        }
                        else if (arg1 == "B2")
                        {
                            v = (v >> 16) & 0xFF;
                        }
                        else if (arg1 == "B3")
                        {
                            v = (v >> 24) & 0xFF;
                        }

                        updateLock = true;
                        link_object.Value = (byte)v;
                        updateLock = false;
                    };
                    link_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        uint v = (uint)src_object.Value;
                        uint linkvalue = (uint)link_object.Value;
                        if (arg1 == "B0")
                        {
                            v = (v & 0xFFFFFF00) | (linkvalue);
                        }
                        else if (arg1 == "B1")
                        {
                            v = (v & 0xFFFF00FF) | (linkvalue<<8);
                        }
                        else if (arg1 == "B2")
                        {
                            v = (v & 0xFF00FFFF) | (linkvalue<<16);
                        }
                        else if (arg1 == "B3")
                        {
                            v = (v & 0x00FFFFFF) | (linkvalue<<24);
                        }
                        else
                        {
                            v = linkvalue;
                        }

                        updateLock = true;
                        src_object.Value = v;
                        updateLock = false;
                    };
                    return;
                }
            }
            if (linktype == "PORTRAIT")
            {//顔画像とリンク.
                PictureBox link_object = ((PictureBox)link_info);

                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value;
                    Bitmap bitmap;
                    if (arg1 == "CLASS")
                    {
                        bitmap = ImagePortraitForm.DrawPortraitClass(id);
                    }
                    else if (arg1 == "MAP")
                    {
                        bitmap = ImagePortraitForm.DrawPortraitMap(id);
                    }
                    else if (arg1 == "UNIT")
                    {
                        bitmap = ImagePortraitForm.DrawPortraitUnit(id);
                    }
                    else
                    {
                        bitmap = ImagePortraitForm.DrawPortraitAuto(id);
                    }
                    U.MakeTransparent(bitmap);
                    link_object.Image = bitmap;
                };

                link_object.Cursor = Cursors.Hand;
                link_object.Click += (sender, e) =>
                {
                    JumpTo(src_object, link_object, "PORTRAIT", new string[] { });
                };

                return;
            }
            if (linktype == "GENERICENEMYPORTRAIT")
            {//一般兵の顔像とリンク.
                PictureBox link_object = ((PictureBox)link_info);

                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value;
                    Bitmap bitmap;
                    if (id > 0)
                    {
                        bitmap = ImageGenericEnemyPortraitForm.Draw(id);
                    }
                    else
                    {
                        bitmap = ImageUtil.Blank(32,32);
                    }

                    U.MakeTransparent(bitmap);
                    link_object.Image = bitmap;
                };

                link_object.Cursor = Cursors.Hand;
                link_object.Click += (sender, e) =>
                {
                    JumpTo(src_object, link_object, "GENERICENEMYPORTRAIT", new string[] { });
                };

                return;
            }
            if (linktype == "BIT")
            {//チェックボックス(ビット表現)とリンク
                CheckBox link_object = ((CheckBox)link_info);

                uint bit = U.atoh(arg1);
                bool updateLock = false;
                src_object.ValueChanged += (sender, e) =>
                {
                    if (updateLock)
                    {
                        return;
                    }
                    updateLock = true;
                    uint id = (uint)src_object.Value;
                    link_object.Checked = ((id & bit) > 0);
                    updateLock = false;
                };
                link_object.CheckedChanged += (sender, e) =>
                {
                    if (updateLock)
                    {
                        return;
                    }
                    updateLock = true;
                    uint id = (uint)src_object.Value;
                    if (link_object.Checked)
                    {
                        U.ForceUpdate(src_object, id | bit);
                    }
                    else
                    {
                        U.ForceUpdate(src_object, id & (~bit));
                    }
                    updateLock = false;
                };
                return;
            }
            if (linktype == "COMBO" || linktype == "COMBOl" || linktype == "COMBOh")
            {//コンボボックスで状態表記
                ComboBox link_object = ((ComboBox)link_info);

                bool updateLock = false;
                src_object.ValueChanged += (sender, e) =>
                {
                    if (updateLock)
                    {
                        return;
                    }
                    updateLock = true;
                    uint id = (uint)src_object.Value;
                    string name = FindNameFromValueComboBoxWhereID(link_object, id);
                    if (name == "")
                    {
                        link_object.SelectedIndex = -1;
                    }
                    else
                    {
                        link_object.Text = name;
                    }
                    updateLock = false;
                };
                link_object.SelectedIndexChanged += (sender, e) =>
                {
                    if (updateLock)
                    {
                        return;
                    }
                    updateLock = true;
                    uint id = ParseIDFromValueComboBox(link_object.Text);
                    if (id != 0xFFFFFFFF)
                    {
                        U.ForceUpdate(src_object, id);
                    }
                    updateLock = false;
                };

                U.SelectedIndexSafety(link_object, 0);
                return;
            }
            if (linktype == "CHECKADDRESS")
            {//アドレスチェック
                //元データが変更されたら、リンクデータも変更する.
                if (link_info is TextBoxEx)
                {
                    TextBoxEx link_object = ((TextBoxEx)link_info);
                    src_object.ValueChanged += (sender, e) =>
                    {
                        uint id = (uint)src_object.Value;
                        if (U.isOffset(id) && U.IsOrderOfHuman(sender))
                        {//人間が、オフセット値を書いた場合、自動補正する
                            id = U.toPointer(id);
                        }

                        string text = "";
                        string errormessage = "";
                        if (!U.isPadding4(id))
                        {
                            text = R._("アドレスが4の倍数ではありません。");
                            errormessage = R._("アドレスは4で割り切れる数字である必要があります。\r\nアドレスが4の倍数でないとゲーム中で再生できないことがあります。\r\nもしsappyでは再生できる場合は、再インポートすることをお勧めします。\r\n");
                        }
                        else if (!U.isSafetyPointer(id))
                        {
                            text = R._("このアドレスは危険です。");
                            errormessage = R._("指定したアドレスは範囲外です。");
                        }
                        if (text == "")
                        {
                            link_object.Hide();
                            return;
                        }
                        link_object.Show();
                        link_object.Text = text;
                        link_object.ErrorMessage = errormessage;
                    };
                    return;
                }
            }
            if (linktype == "ATTRIBUTE")
            {//属性とリンク
                Label link_object = ((Label)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value;
                    link_object.Text = UpdateCoron(link_object.Text, GetAttributeName(id));
                };

                link_info.Cursor = Cursors.Hand;
                link_info.Click += (sender, e) =>
                {
                    JumpTo(src_object, link_info, "ATTRIBUTE", new string[] { });
                };

                return;
            }
            if (linktype == "ATTRIBUTEICON")
            {//属性アイコンとリンク
                PictureBox link_object = ((PictureBox)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value;
                    Bitmap bitmap = ImageSystemIconForm.Attribute(id);
                    U.MakeTransparent(bitmap);
                    link_object.Image = bitmap;
                };

                link_info.Cursor = Cursors.Hand;
                link_info.Click += (sender, e) =>
                {
                    JumpTo(src_object, link_info, "ATTRIBUTE", new string[] { });
                };
                return;
            }
            if (linktype == "GAMEOPTIONICON")
            {//オプションのアイコンとリンク
                PictureBox link_object = ((PictureBox)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value / 2;
                    Bitmap bitmap = ImageSystemIconForm.MusicIcon(id);
                    U.MakeTransparent(bitmap);
                    link_object.Image = bitmap;
                };
                return;
            }
            if (linktype == "WEAPON")
            {//武器レベルとリンク
                if (link_info is Label)
                {
                    Label link_object = ((Label)link_info);
                    link_object.Text = GetWeaponClass(0) + " ";
                    src_object.ValueChanged += (sender, e) =>
                    {
                        uint id = (uint)src_object.Value;
//                        link_object.Text = UpdateCoron(link_object.Text, GetWeaponClass(id));
                        link_object.Text = GetWeaponClass(id) + " ";
                    };
                    return;
                }
                else if (link_info is ComboBox)
                {
                    ComboBox link_object = ((ComboBox)link_info);
                    U.SelectedIndexSafety(link_object, 0);

                    bool updateLock = false;
                    src_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint id = (uint)src_object.Value;
                        link_object.Text = GetWeaponClass(id);
                        updateLock = false;
                    };
                    link_object.SelectedIndexChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;

                        uint id = GetWeaponClassRev(link_object.Text);
                        U.ForceUpdate(src_object, id);
                        updateLock = false;
                    };
                    return;
                }
            }
            if (linktype == "WEAPONTYPEICON")
            {//武器分類のアイコンとリンク
                PictureBox link_object = ((PictureBox)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value;
                    Bitmap bitmap = ImageSystemIconForm.WeaponIcon(id);
                    U.MakeTransparent(bitmap);
                    link_object.Image = bitmap;
                };
                return;
            }
            if (linktype == "CLASS")
            {//クラスとリンク
                TextBoxEx link_object = ((TextBoxEx)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    //クラス名の取得
                    link_object.Text = ClassForm.GetClassName((uint)src_object.Value);
                };
                link_object.DoubleClick += (sender, e) =>
                {//ダブルクリックでアイテムへ
                    JumpTo(src_object, link_info, "CLASS", new string[] { });
                };

                return;
            }
            if (linktype == "SONG")
            {//曲名とリンク.
                TextBoxEx link_object = ((TextBoxEx)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    link_object.ErrorMessage = SongTableForm.GetErrorMessage((uint)src_object.Value,arg1);

                    link_object.Text = SongTableForm.GetSongName((uint)src_object.Value);
                };
                link_object.DoubleClick += (sender, e) =>
                {//曲名ダブルクリックで再生
                    uint song_id = (uint)src_object.Value;
                    MainFormUtil.RunAsSappy(song_id);
                };

                return;
            }
            if (linktype == "ITEM")
            {//アイテム名とリンク.
                TextBoxEx link_object = ((TextBoxEx)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    link_object.Text = ItemForm.GetItemName((uint)src_object.Value);
                };
                link_object.DoubleClick += (sender, e) =>
                {//ダブルクリックでアイテムへ
                    JumpTo(src_object, link_info, "ITEM", new string[] { });
                };

                return;
            }
            if (linktype == "SONGPLAY")
            {//曲を再生する
                Button link_object = ((Button)link_info);
                link_object.Click += (sender, e) =>
                {
                    uint song_id = (uint)src_object.Value;
                    MainFormUtil.RunAsSappy(song_id);
                };
                return;
            }
            if (linktype == "UNIT")
            {//ユニット名とリンク.
                TextBoxEx link_object = ((TextBoxEx)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    if (arg1 == "ANY")
                    {
                        //ユニット名の取得
                        link_object.Text = UnitForm.GetUnitNameAndANY((uint)src_object.Value);
                    }
                    else
                    {
                        //ユニット名の取得
                        link_object.Text = UnitForm.GetUnitName((uint)src_object.Value);
                    }
                };
                link_object.DoubleClick += (sender, e) =>
                {//ダブルクリックでユニットへ
                    JumpTo(src_object, link_info, "UNIT", new string[] { });
                };

                return;
            }
            if (linktype == "MAP")
            {//マップ名リンク.
                TextBoxEx link_object = ((TextBoxEx)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    //マップ名の取得 
                    if (arg1 == "ANYFF")
                    {
                        link_object.Text = MapSettingForm.GetMapNameAndANYFF((uint)src_object.Value);
                    }
                    else if (arg1 == "ANYFE")
                    {
                        link_object.Text = MapSettingForm.GetMapNameAndANYFE((uint)src_object.Value);
                    }
                    else if (arg1 == "ANYIFOVER")
                    {
                        link_object.Text = MapSettingForm.GetMapNameAndANYIFOVER((uint)src_object.Value);
                    }
                    else
                    {
                        link_object.Text = MapSettingForm.GetMapName((uint)src_object.Value);
                    }
                };
                link_object.DoubleClick += (sender, e) =>
                {//ダブルクリックでマップへ
                    JumpTo(src_object, link_info, "MAP", new string[] { });
                };
                return;
            }
            if (linktype == "UNITPALETTE")
            {//ユニットパレット.
                TextBoxEx link_object = ((TextBoxEx)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value;
                    if (arg1 == "PLUS1")
                    {
                        if (id == 0xFF || id < 0x80)
                        {
                            link_object.ErrorMessage = "";
                        }
                        else
                        {
                            link_object.ErrorMessage = R._("0x80以上のパレットを指定しても無視されます。");
                        }

                        id = id + 1;
                    }
                    link_object.Text = ImageUnitPaletteForm.GetPaletteName(id);
                };

                return;
            }
            if (linktype == "TERRAINBATTLE")
            {//バトル地形
                TextBoxEx link_object = ((TextBoxEx)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value;
                    if (arg1 == "PLUS1")
                    {
                        id = id + 1;
                    }
                    link_object.Text = ImageBattleTerrainForm.GetBattleTerrainName(id);
                };

                return;
            }
            if (linktype == "TERRAINBATTLEICON")
            {//バトル地形の画像
                PictureBox link_object = ((PictureBox)link_info);

                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value;
                    if (arg1 == "PLUS1")
                    {
                        id = id + 1;
                    }
                    Bitmap bitmap = ImageBattleTerrainForm.DrawSquare(id, ImageBattleTerrainForm.RangeType.Melee);
//                    U.MakeTransparent(bitmap);
                    link_object.Image = bitmap;
                };
                return;
            }
            if (linktype == "BASEPOINT")
            {//ワールドマップの拠点とリンク.
                TextBoxEx link_object = ((TextBoxEx)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    link_object.Text = WorldMapPointForm.GetWorldMapPointName((uint)src_object.Value);
                };

                return;
            }
            if (linktype == "ITEMEFFECT")
            {//アイテムエフェクト.
                TextBoxEx link_object = ((TextBoxEx)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value;
                    if (arg1 == "00ANY" && id == 0x0)
                    {
                        link_object.Text = "";
                        return;
                    }
                    link_object.Text = ItemWeaponEffectForm.GetName(id);
                };

                return;
            }
            if (linktype == "MAPXY")
            {//地図座標
                MapPictureBox map = FindMapObject(prefix, controls);
                NumericUpDown src_objectY = FindSRCObject(prefix, controls, U.atoi(arg1));

                bool updateLock = false;

                Func<int,int,int> movecallback = (int x,int y)=>{
                    if (updateLock)
                    {
                        return 0;
                    }
                    updateLock = true;

                    U.ForceUpdate(src_object, x);
                    U.ForceUpdate(src_objectY, y);

                    updateLock = false;
                    return 0;
                };
                EventHandler link_map_update_function = (sender, e) =>
                {
                    map.SetPoint(link_info.Name
                        ,(int)src_object.Value,(int)src_objectY.Value);
                };
                EventHandler link_map_focus_function = (sender, e) =>
                {
                    map.setNotifyMode(src_object.Name,movecallback);
                };

                src_object.ValueChanged += link_map_update_function;
                src_objectY.ValueChanged += link_map_update_function;
                src_object.GotFocus += link_map_focus_function;
                src_objectY.GotFocus += link_map_focus_function;

                return;
            }
            if (linktype == "UNITPOS")
            {//ユニット配置での座標指定
                bool updateLock = false;

                if (arg1 == "X")
                {
                    MapPictureBox map = FindMapObject(prefix, controls);
                    NumericUpDown link_object = ((NumericUpDown)link_info);
                    link_object.Maximum = 63;

                    link_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint x = (uint)link_object.Value;
                        uint id = (uint)src_object.Value;
                        U.ForceUpdate(src_object, (x & 0x3F) | (id & 0xFFC0)) ;
                        updateLock = false;
                    };


                    Func<int, int, int> movecallback = (int x, int y) =>
                    {
                        if (updateLock)
                        {
                            return 0;
                        }

                        uint id = (uint)src_object.Value;
                        uint xy = ((id & 0xF000) | ((((uint)y) & 0x3F) << 6) | ( ((uint)x) & 0x3F));
                        U.ForceUpdate(src_object, xy);

                        return 0;
                    };
                    EventHandler link_map_focus_function = (sender, e) =>
                    {
                        if (map != null)
                        {
                            map.setNotifyMode(src_object.Name, movecallback);
                        }
                    };

                    src_object.GotFocus += link_map_focus_function;
                    link_object.GotFocus += link_map_focus_function;
 
                    src_object.ValueChanged += (sender, e) =>
                    {
                        uint id = (uint)src_object.Value;
                        int x = (int)((id) & 0x3F);

                        int y = (int)((id >> 6) & 0x3F);

                        if (map != null)
                        {
                            map.SetPoint(src_object.Name , x, y);
                        }
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        U.ForceUpdate(link_object, x);
                        updateLock = false;
                    };
                }
                else if (arg1 == "Y")
                {
                    NumericUpDown link_object = ((NumericUpDown)link_info);
                    link_object.Maximum = 63;

                    link_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint y = (uint)link_object.Value;
                        uint id = (uint)src_object.Value;
                        U.ForceUpdate(src_object, ((y & 0x3F)<<6) | ((id & 0xF03F)));
                        updateLock = false;
                    };

                    src_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint id = (uint)src_object.Value;
                        uint y = (id>>6) & 0x3F;
                        U.ForceUpdate(link_object, y);
                        updateLock = false;
                    };
                }
                else if (arg1 == "SP")
                {
                    ComboBox link_object = ((ComboBox)link_info);

                    link_object.SelectedIndexChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint sp = (uint)link_object.SelectedIndex;
                        uint id = (uint)src_object.Value;
                        U.ForceUpdate(src_object, ((sp & 0xF) << 12) | ((id & 0xFFF)));
                        updateLock = false;
                    };

                    src_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint id = (uint)src_object.Value;
                        uint sp = (id >> 12) & 0xF;
                        if (sp > link_object.Items.Count)
                        {
                            link_object.SelectedIndex = -1;
                        }
                        else
                        {
                            U.SelectedIndexSafety(link_object, sp, false);
                        }
                        updateLock = false;
                    };
                }

                return;
            }
            if (linktype == "UNITGROW")
            {//ユニット配置でのユニット依存
                bool updateLock = false;

                if (arg1 == "LV")
                {
                    NumericUpDown link_object = ((NumericUpDown)link_info);
                    link_object.Maximum = 31;

                    link_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint lv = (uint)link_object.Value;
                        uint id = (uint)src_object.Value;
                        U.ForceUpdate(src_object, ((id & (0x7)) | ((lv&0x1F) << 3)));
                        updateLock = false;
                    };

                    src_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint id = (uint)src_object.Value;
                        uint lv = (id >> 3) & 0x1F;
                        U.ForceUpdate(link_object, lv);
                        updateLock = false;
                    };
                }
                else if(arg1 == "ASSIGN")
                {
                    ComboBox link_object = ((ComboBox)link_info);

                    link_object.SelectedIndexChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint assign = (uint)link_object.SelectedIndex;
                        if (assign >= 0)
                        {
                            uint id = (uint)src_object.Value;
                            U.ForceUpdate(src_object, ((id & (0xF9)) | ((assign&0x3) << 1)));
                        }
                        updateLock = false;
                    };
                    src_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint id = (uint)src_object.Value;
                        uint assign = (id >> 1) & 0x3;
                        U.SelectedIndexSafety(link_object, assign, false);
                        updateLock = false;
                    };
                }
                else if (arg1 == "GROW")
                {
                    ComboBox link_object = ((ComboBox)link_info);

                    link_object.SelectedIndexChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint grow = (uint)link_object.SelectedIndex;
                        if (grow >= 0)
                        {
                            uint id = (uint)src_object.Value;
                            U.ForceUpdate(src_object, (id & (0xFE) | (grow&0x01)));
                        }
                        updateLock = false;
                    };
                    src_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint id = (uint)src_object.Value;
                        uint grow = (id) & 0x1;
                        U.SelectedIndexSafety(link_object, grow, false);
                        updateLock = false;
                    };
                }

                return;
            }
            if (linktype == "TSA")
            {//TSA指定
                bool updateLock = false;

                MapPictureBox map = FindMapObject(prefix, controls);
                if (arg1 == "X")
                {
                    NumericUpDown link_object = ((NumericUpDown)link_info);
                    link_object.Maximum = 32;

                    link_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint x = (uint)link_object.Value;
                        uint src = (uint)src_object.Value;

                        uint mapwidth8 = (uint)(map.GetMapBitmapWidth() / 8);
                        uint tileNumber = (src & 0x03FF);
                        uint y = tileNumber / mapwidth8;
                        tileNumber = x + y * mapwidth8;

                        U.ForceUpdate(src_object, tileNumber | (src & 0xFC00));
                        updateLock = false;
                    };


                    Func<int, int, int> movecallback = (int x, int y) =>
                    {
                        if (updateLock)
                        {
                            return 0;
                        }

                        uint mapwidth8 = (uint)(map.GetMapBitmapWidth() / 8);
                        uint src = (uint)src_object.Value;
                        uint tileNumber = (uint)(x + (y * mapwidth8)) &0x3FF;

                        U.ForceUpdate(src_object, (src & 0xFC00) | tileNumber);

                        return 0;
                    };
                    EventHandler link_map_focus_function = (sender, e) =>
                    {
                        map.setNotifyMode(src_object.Name, movecallback);
                    };

                    src_object.GotFocus += link_map_focus_function;
                    link_object.GotFocus += link_map_focus_function;

                    src_object.ValueChanged += (sender, e) =>
                    {
                        uint src = (uint)src_object.Value;

                        uint mapwidth8 = (uint)(map.GetMapBitmapWidth() / 8);
                        uint tileNumber = (src & 0x03FF);
                        uint x = tileNumber % mapwidth8;
                        uint y = tileNumber / mapwidth8;

                        map.SetPoint(src_object.Name
                            , (int)x, (int)y);
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        U.ForceUpdate(link_object, x);
                        updateLock = false;
                    };
                }
                else if (arg1 == "Y")
                {
                    NumericUpDown link_object = ((NumericUpDown)link_info);
                    link_object.Maximum = 63;

                    link_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;

                        uint mapwidth8 = (uint)(map.GetMapBitmapWidth() / 8);
                        uint src = (uint)src_object.Value;
                        uint tileNumber = (src & 0x03FF);
                        uint x = tileNumber % mapwidth8;
                        tileNumber = x + (uint)(link_object.Value * mapwidth8);

                        U.ForceUpdate(src_object, (src & 0xFC00) | tileNumber);
                        
                        updateLock = false;
                    };

                    src_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;

                        uint mapwidth8 = (uint)(map.GetMapBitmapWidth() / 8);
                        uint src = (uint)src_object.Value;
                        uint tileNumber = (src & 0x03FF);
                        uint y = tileNumber / mapwidth8;
                        U.ForceUpdate(link_object, y);

                        updateLock = false;
                    };
                }
                else if (arg1 == "FLIP")
                {
                    ComboBox link_object = ((ComboBox)link_info);

                    link_object.SelectedIndexChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;

                        uint src = (uint)src_object.Value;
                        src = src & 0xF3FF;
                        uint flip = ParseIDFromValueComboBox(link_object.Text);
                        if (flip == 0x0000 || flip == 0x0400 || flip == 0x0800 || flip == 0x0C00)
                        {
                            U.ForceUpdate(src_object, src | flip);
                        }

                        updateLock = false;
                    };

                    src_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint src = (uint)src_object.Value;
                        src = src & 0x0C00;
                        link_object.Text = FindNameFromValueComboBoxWhereID(link_object, src);
                        updateLock = false;
                    };
                }
                else if (arg1 == "PALETTE")
                {
                    ComboBox link_object = ((ComboBox)link_info);

                    link_object.SelectedIndexChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;

                        uint src = (uint)src_object.Value;
                        src = src & 0x0FFF;
                        uint palette = ParseIDFromValueComboBox(link_object.Text);
                        if (palette <= 0xF000)
                        {
                            U.ForceUpdate(src_object, src | palette);
                        }

                        updateLock = false;
                    };

                    src_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint src = (uint)src_object.Value;
                        src = src & 0xF000;
                        link_object.Text = FindNameFromValueComboBoxWhereID(link_object, src);
                        updateLock = false;
                    };
                }
                return;
            }
            if (linktype == "AI3")
            {//AI3 標的と回復
                bool updateLock = false;

                if (arg1 == "HYOUTEKI")
                {
                    ComboBox link_object = ((ComboBox)link_info);
                    U.SelectedIndexSafety(link_object,0);

                    link_object.SelectedIndexChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint v = ParseIDFromValueComboBox(link_object.Text);
                        if (v == 0xFFFFFFFF)
                        {
                            v = 0;
                        }

                        uint id = (uint)src_object.Value;
                        U.ForceUpdate(src_object, ((id & (0x7)) | (v)));
                        updateLock = false;
                    };

                    src_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint id = (uint)src_object.Value;
                        uint v = (id & 0xF8);
                        link_object.Text = FindNameFromValueComboBoxWhereID(link_object, v);
                        updateLock = false;
                    };
                }
                else if (arg1 == "KAIFUKU")
                {
                    ComboBox link_object = ((ComboBox)link_info);
                    U.SelectedIndexSafety(link_object,0);

                    link_object.SelectedIndexChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint v = ParseIDFromValueComboBox(link_object.Text);
                        if (v == 0xFFFFFFFF)
                        {
                            v = 0;
                        }

                        uint id = (uint)src_object.Value;
                        U.ForceUpdate(src_object, ((id & (0xF8)) | (v&0x7)));
                        updateLock = false;
                    };

                    src_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint id = (uint)src_object.Value;
                        uint v = (id&0x7);
                        link_object.Text = FindNameFromValueComboBoxWhereID(link_object,v);
                        updateLock = false;
                    };
                }

                return;
            }
            if (linktype == "BATTLEANIMEITEMICON")
            {//戦闘アニメアイテム指定へのアイコン
                PictureBox link_object = ((PictureBox)link_info);
                NumericUpDown src_objectSP = FindSRCObject(prefix, controls, U.atoi(arg1));

                src_object.ValueChanged += (sender, e) =>
                {
                    uint b0 = (uint)src_object.Value;
                    uint b1 = (uint)src_objectSP.Value;
                    Bitmap bitmap = ImageBattleAnimeForm.getSPTypeIcon(b0, b1);
                    U.MakeTransparent(bitmap);
                    link_object.Image = bitmap;
                };
                return;
            }
            if (linktype == "BATTLEANIMEITEM")
            {//戦闘アニメアイテム指定 アイテム名(または特殊ID)
                bool updateLock = false;

                TextBoxEx link_object = ((TextBoxEx)link_info);
                NumericUpDown src_objectSP = FindSRCObject(prefix, controls, U.atoi(arg1));

                src_object.ValueChanged += (sender, e) =>
                {
                    if (updateLock)
                    {
                        return;
                    }

                    updateLock = true;
                    uint b0 = (uint)src_object.Value;
                    uint b1 = (uint)src_objectSP.Value;
                    link_object.Text = ImageBattleAnimeForm.getSPTypeName(b0, b1);
                    updateLock = false;
                };
                return;
            }
            if (linktype == "BATTLEANIMESP")
            {//戦闘アニメアイテム指定　特殊
                bool updateLock = false;

                NumericUpDown src_objectItem = FindSRCObject(prefix, controls, U.atoi(arg1));
                ComboBox link_object = ((ComboBox)link_info);
                U.SelectedIndexSafety(link_object,0);

                link_object.SelectedIndexChanged += (sender, e) =>
                {
                    if (updateLock)
                    {
                        return;
                    }

                    updateLock = true;
                    src_object.Value = (uint)link_object.SelectedIndex;
                    updateLock = false;

                    U.ForceUpdate(src_objectItem, src_objectItem.Value);
                };

                src_object.ValueChanged += (sender, e) =>
                {
                    if (updateLock)
                    {
                        return;
                    }

                    updateLock = true;
                    uint b1 = (uint)src_object.Value;

                    string name = FindNameFromValueComboBoxWhereID(link_object, b1);
                    if (name == "")
                    {
                        link_object.SelectedIndex = -1;
                    }
                    else
                    {
                        link_object.Text = name;
                    }
                    updateLock = false;

                };
                return;
            }
            if (linktype == "SPLITSTRING")
            {//文字列を1バイトずつ表示
                TextBoxEx link_object = ((TextBoxEx)link_info);
                uint maxid = U.atoi(arg1);
                if (maxid < link_id)
                {
                    throw new Exception("linkerror maxid:"+maxid+" link_id:"+link_id+" link_info:" + link_info.Name);
                }

                NumericUpDown[] src_objests = new NumericUpDown[maxid-link_id + 1];
                src_objests[0] = src_object;
                for (uint nn = 1; nn < src_objests.Length ; nn++)
                {
                    src_objests[nn] = FindSRCObject( prefix, controls, nn + link_id);
                    if (src_objests[nn]  == null)
                    {
                        throw new Exception("broken link:" + link_info.Name + " nn:" + nn.ToString());
                    }
                }

                bool updateLock = false;
                EventHandler link_text_update_function = (sender, e) =>
                {
                    if (updateLock)
                    {
                        return;
                    }
                    updateLock = true;

                    byte[] str = new byte[maxid - link_id + 1];
                    for (uint nn = 0; nn < src_objests.Length; nn++)
                    {
                        str[nn] = (byte)src_objests[nn].Value;
                    }

                    FETextDecode decoder = new FETextDecode();
                    link_object.Text = decoder.listbyte_to_string(str, src_objests.Length);
                    updateLock = false;

                };
                link_object.KeyUp += (sender, e) =>
                {
                    if (updateLock)
                    {
                        return;
                    }
                    updateLock = true;

                    byte[] sjisstr = Program.SystemTextEncoder.Encode(link_object.Text);
                    for (uint nn = 0; nn < src_objests.Length; nn++)
                    {
                        if (nn >= sjisstr.Length)
                        {
                            U.ForceUpdate(src_objests[nn], 0);
                        }
                        else
                        {
                            U.ForceUpdate(src_objests[nn], sjisstr[nn]);
                        }
                    }
                    updateLock = false;
                };

                for (uint nn = 0; nn < src_objests.Length; nn++)
                {
                    src_objests[nn].ValueChanged += link_text_update_function;
                }
                link_text_update_function(null, null);
                return;
            }
            if (linktype == "WSPLITSTRING")
            {//文字列を2バイトずつ表示
                TextBoxEx link_object = ((TextBoxEx)link_info);
                uint maxid = U.atoi(arg1);
                if (maxid < link_id)
                {
                    throw new Exception("linkerror maxid:" + maxid + " link_id:" + link_id + " link_info:" + link_info.Name);
                }

                NumericUpDown[] src_objests = new NumericUpDown[maxid - link_id + 1];
                src_objests[0] = src_object;
                for (uint nn = 1; nn < src_objests.Length; nn++)
                {
                    src_objests[nn] = FindSRCObject( prefix, controls, nn + link_id);
                    if (src_objests[nn] == null)
                    {
                        throw new Exception("broken link:" + link_info.Name + " nn:" + nn.ToString());
                    }
                }

                bool updateLock = false;
                EventHandler link_text_update_function = (sender, e) =>
                {
                    if (updateLock)
                    {
                        return;
                    }
                    updateLock = true;

                    byte[] str = new byte[(maxid - link_id + 1) * 2];
                    for (uint nn = 0; nn < src_objests.Length; nn++)
                    {
                        str[nn * 2 + 0] = (byte)( ((uint)src_objests[nn].Value) & 0xFF);
                        str[nn * 2 + 1] = (byte)((((uint)src_objests[nn].Value) >> 8) & 0xFF);
                    }
                    FETextDecode decoder = new FETextDecode();
                    link_object.Text = decoder.listbyte_to_string(str, str.Length);
                    updateLock = false;

                };
                link_object.KeyUp += (sender, e) =>
                {
                    if (updateLock)
                    {
                        return;
                    }
                    updateLock = true;

                    string str = link_object.Text;
                    str = FETextEncode.ConvertSPMoji(str);
                    byte[] sjisstr = Program.SystemTextEncoder.Encode(str);
                    for (uint nn = 0; nn < src_objests.Length; nn++)
                    {
                        if (nn+1 >= sjisstr.Length)
                        {
                            U.ForceUpdate(src_objests[nn], 0);
                        }
                        else
                        {
                            U.ForceUpdate(src_objests[nn], 
                                (((uint)sjisstr[nn * 2 + 0])) +
                                (((uint)sjisstr[nn * 2 + 1]) << 8)
                                );
                        }
                    }
                    updateLock = false;
                };

                for (uint nn = 0; nn < src_objests.Length; nn++)
                {
                    src_objests[nn].ValueChanged += link_text_update_function;
                }
                link_text_update_function(null, null);
                return;
            }
            if (linktype == "UNITICON")
            {//ユニット名からアイコン表示
                src_object.ValueChanged += (sender, e) =>
                {
                    PictureBox link_object = ((PictureBox)link_info);

                    Bitmap bitmap;
                    if (arg1 == "BIG")
                    {
                        bitmap = UnitForm.DrawUnitFacePicture((uint)src_object.Value);
                    }
                    else
                    {
                        bitmap = UnitForm.DrawUnitMapFacePicture((uint)src_object.Value);
                    }
                    U.MakeTransparent(bitmap);
                    link_object.Image = bitmap;
                };

                link_info.Cursor = Cursors.Hand;
                link_info.Click += (sender, e) =>
                {
                    JumpTo(src_object, link_info, "UNIT", new string[] { });
                };

                return;
            }
            if (linktype == "CLASSICON")
            {//クラスIDからアイコン表示

                EventHandler callback = (sender, e) =>
                {
                    PictureBox link_object = ((PictureBox)link_info);
                    int color = 0;
                    bool height16_limit = false;
                    if (arg1 == "UNITGROW")
                    {
                        Control unitgrowAssign = (Control)FindObject(prefix, controls, "L_" + U.at(args, 1) + "_UNITGROW_ASSIGN");
                        if (unitgrowAssign is ComboBox)
                        {
                            color = ((ComboBox)unitgrowAssign).SelectedIndex;
                            height16_limit = true;
                        }
                    }

                    Bitmap bitmap = ClassForm.DrawWaitIcon((uint)src_object.Value, color, height16_limit);
                    U.MakeTransparent(bitmap);
                    link_object.Image = bitmap;

                    if (arg1 == "MAPDEF")
                    {
                        MapPictureBox map = FindMapObject(prefix, controls);
                        if (map == null)
                        {
                            throw new Exception("linkerror can not found MapPictureBox  link_info:" + link_info.Name);
                        }
                        map.SetDefualtIcon(bitmap,0, 16 - bitmap.Height);
                    }
                };
                src_object.ValueChanged += callback;

                if (arg1 == "UNITGROW")
                {
                    Control unitgrowAssign = (Control)FindObject(prefix, controls, "L_" + U.at(args, 1) + "_UNITGROW_ASSIGN");
                    if (unitgrowAssign is ComboBox)
                    {
                        ((ComboBox)unitgrowAssign).SelectedIndexChanged += callback;
                    }
                }

                link_info.Cursor = Cursors.Hand;
                link_info.Click += (sender, e) =>
                {
                    JumpTo(src_object, link_info, "CLASS", new string[] { });
                };
                return;
            }
            if (linktype == "ITEMICON")
            {//アイテムIDからアイコン表
                src_object.ValueChanged += (sender, e) =>
                {
                    PictureBox link_object = ((PictureBox)link_info);
                    Bitmap bitmap = ItemForm.DrawIcon((uint)src_object.Value);
                    U.MakeTransparent(bitmap);
                    link_object.Image = bitmap;
                };

                link_info.Cursor = Cursors.Hand;
                link_info.Click += (sender, e) =>
                {
                    JumpTo(src_object, link_info, "ITEM", new string[] { });
                };
                return;
            }
            if (linktype == "CLASSICONSRC")
            {//クラスアイコン表示
                src_object.ValueChanged += (sender, e) =>
                {
                    PictureBox link_object = ((PictureBox)link_info);

                    Bitmap bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap((uint)src_object.Value, 0, false);
                    U.MakeTransparent(bitmap);
                    link_object.Image = bitmap;

                    if (arg1 == "MAPDEF")
                    {
                        MapPictureBox map = FindMapObject(prefix, controls);
                        if (map == null)
                        {
                            throw new Exception("linkerror can not found MapPictureBox  link_info:" + link_info.Name);
                        }
                        map.SetDefualtIcon(bitmap,0, 16 - bitmap.Height);
                    }
                };

                link_info.Cursor = Cursors.Hand;
                link_info.Click += (sender, e) =>
                {
                    JumpTo(src_object, link_info, "WAITICON", new string[] { });
                };
                return;
            }
            if (linktype == "CLASSMOVEICONSRC")
            {//クラス移動アイコン
                src_object.ValueChanged += (sender, e) =>
                {
                    PictureBox link_object = ((PictureBox)link_info);

                    Bitmap bitmap = ImageUnitMoveIconFrom.DrawMoveUnitIconBitmap((uint)src_object.Value, 0, 0 );
                    U.MakeTransparent(bitmap);
                    link_object.Image = bitmap;
                };

//                link_info.Cursor = Cursors.Hand;
//                link_info.Click += (sender, e) =>
//                {
//                    JumpTo(src_object, link_info, "MOVEICON", new string[] { });
//                };
                return;
            }
            if (linktype == "ITEMICONSRC")
            {//アイテムアイコン表示
                src_object.ValueChanged += (sender, e) =>
                {
                    PictureBox link_object = ((PictureBox)link_info);
                    Bitmap bitmap;
                    if (arg1 == "WEAPON")
                    {
                        bitmap = ImageItemIconForm.DrawIconWhereID_UsingWeaponPalette((uint)src_object.Value);
                    }
                    else if (arg1 == "SKILLFE8NVer2")
                    {
                        bitmap = ImageItemIconForm.DrawIconWhereID_UsingWeaponPalette_SKILLFE8NVer2((uint)src_object.Value);
                    }
                    else
                    {
                        bitmap = ImageItemIconForm.DrawIconWhereID((uint)src_object.Value);
                    }
                    U.MakeTransparent(bitmap);
                    link_object.Image = bitmap;
                };
                if (arg1 == "WEAPON" || arg1 == "SKILLFE8NVer2")
                {
                }
                else
                {
                    link_info.Cursor = Cursors.Hand;
                    link_info.Click += (sender, e) =>
                    {
                        JumpTo(src_object, link_info, "ICON", new string[] { });
                    };
                }

                return;
            }
            if (linktype == "OPCLASSDEMOFONT")
            {//OPクラス紹介の日本語フォント
                src_object.ValueChanged += (sender, e) =>
                {
                    PictureBox link_object = ((PictureBox)link_info);
                    link_object.Image = OPClassFontForm.DrawFontByID((uint)src_object.Value);
                };               
                return;
            }
            if (linktype == "BATTLEANIME")
            {//バトルアニメ名.
                TextBoxEx link_object = ((TextBoxEx)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value;
                    if (arg1 == "PLUS1")
                    {
                        id = id + 1;
                    }
                    link_object.Text = ImageBattleAnimeForm.GetBattleAnimeName(id);
                };

                return;
            }
            if (linktype == "BATTLEANIMEICON")
            {//バトルアニメ.
                PictureBox link_object = ((PictureBox)link_info);

                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value;
                    if (arg1 == "PLUS1")
                    {
                        id = id + 1;
                    }
                    Bitmap image = ImageBattleAnimeForm.DrawBattleAnime(id);
                    link_object.Image = image;
                };

                link_info.Cursor = Cursors.Hand;
                link_info.Click += (sender, e) =>
                {
                    JumpTo(src_object, link_info, "BATTLEANIMEICON", new string[] { });
                };
                return;
            }
            if (linktype == "BATTLEANIMEPOINTER")
            {//バトルアニメ設定のポインタ指定
                TextBoxEx link_object = ((TextBoxEx)link_info);

                src_object.ValueChanged += (sender, e) =>
                {
                    uint addr = (uint)src_object.Value;
                    uint cid = ClassForm.GetIDWhereBattleAnimeAddr(addr);

                    link_object.Text = ClassForm.GetClassName(cid);
                };

                link_info.Cursor = Cursors.Hand;
                link_info.Click += (sender, e) =>
                {
                    JumpTo(src_object, link_info, "BATTLEANIMEICON", new string[] { });
                };
                return;
            }
            if (linktype == "BATTLEANIMEPOINTERICON")
            {//バトルアニメ設定のポインタ指定のアイコン
                PictureBox link_object = ((PictureBox)link_info);

                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = ImageBattleAnimeForm.GetAnimeIDByAnimeSettingPointer((uint)src_object.Value);
                    if (arg1 == "PLUS1")
                    {
                        id = id + 1;
                    }
                    Bitmap image = ImageBattleAnimeForm.DrawBattleAnime(id);
                    link_object.Image = image;
                };

                link_info.Cursor = Cursors.Hand;
                link_info.Click += (sender, e) =>
                {
                    JumpTo(src_object, link_info, "BATTLEANIMEICON", new string[] { });
                };
                return;
            }
            if (linktype == "WMICON")
            {//ワールドマップアイコン
                PictureBox link_object = ((PictureBox)link_info);

                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value;

                    Bitmap bitmap = WorldMapImageForm.DrawWorldMapIcon(id);
                    U.MakeTransparent(bitmap);
                    link_object.Image = bitmap;

                    if (arg1 == "MAPDEF")
                    {
                        MapPictureBox map = FindMapObject(prefix, controls);
                        if (map == null)
                        {
                            throw new Exception("linkerror can not found MapPictureBox  link_info:" + link_info.Name);
                        }
                        map.SetDefualtIcon(bitmap,- (bitmap.Width / 2) , -(bitmap.Height / 2) );
                    }
                };

                link_info.Cursor = Cursors.Hand;
                link_info.Click += (sender, e) =>
                {
                    JumpTo(src_object, link_info, "WMICON", new string[] { });
                };
                return;
            }
            if (linktype == "BGICON")
            {//BG
                PictureBox link_object = ((PictureBox)link_info);

                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value;
                    Bitmap bitmap = ImageBGForm.DrawBG(id);
                    link_object.Image = bitmap;
                };


                link_info.Cursor = Cursors.Hand;
                link_info.Click += (sender, e) =>
                {
                    JumpTo(src_object, link_info, "BG", new string[] { });
                };
                return;
            }
            if (linktype == "CGICON")
            {//CG
                PictureBox link_object = ((PictureBox)link_info);

                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value;
                    Bitmap bitmap = ImageCGForm.DrawImageByID(id);
                    link_object.Image = bitmap;
                };

                link_info.Cursor = Cursors.Hand;
                link_info.Click += (sender, e) =>
                {
                    JumpTo(src_object, link_info, "CG", new string[] { });
                };
                return;
            }
            if (linktype == "BATTLEBG")
            {//戦闘背景
                TextBoxEx link_object = ((TextBoxEx)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    link_object.Text = ImageBattleBGForm.GetName((uint)src_object.Value);
                };

                return;
            }
            if (linktype == "BATTLEBGICON")
            {//戦闘背景
                PictureBox link_object = ((PictureBox)link_info);

                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value;
                    if (id <= 0)
                    {
                        link_object.Image = null;
                        return;
                    }
                    Bitmap bitmap = ImageBattleBGForm.DrawBG(id);
                    link_object.Image = bitmap;
                };


                link_info.Cursor = Cursors.Hand;
                link_info.Click += (sender, e) =>
                {
                    JumpTo(src_object, link_info, "BATTLEBG", new string[] { });
                };
                return;
            }
            if (linktype == "SWEEP")
            {//楽器 squarewave
                bool updateLock = false;

                if (arg1 == "SHIFT")
                {
                    ComboBox link_object = ((ComboBox)link_info);
                    U.SelectedIndexSafety(link_object,0);

                    link_object.SelectedIndexChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint v = ParseIDFromValueComboBox(link_object.Text);
                        if (v == 0xFFFFFFFF)
                        {
                            v = 0;
                        }

                        uint id = (uint)src_object.Value;
                        U.ForceUpdate(src_object, ((id & (0xF)) | (v<<4)));
                        updateLock = false;
                    };

                    src_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint id = (uint)src_object.Value;
                        uint v = (id & 0xF0) >> 4;
                        link_object.Text = FindNameFromValueComboBoxWhereID(link_object, v);
                        updateLock = false;
                    };
                }
                else if (arg1 == "TIME")
                {
                    ComboBox link_object = ((ComboBox)link_info);
                    U.SelectedIndexSafety(link_object,0);

                    link_object.SelectedIndexChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint v = ParseIDFromValueComboBox(link_object.Text);
                        if (v == 0xFFFFFFFF)
                        {
                            v = 0;
                        }

                        uint id = (uint)src_object.Value;
                        U.ForceUpdate(src_object, ((id & (0xF0)) | (v)));
                        updateLock = false;
                    };

                    src_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint id = (uint)src_object.Value;
                        uint v = (id & 0x0F);
                        link_object.Text = FindNameFromValueComboBoxWhereID(link_object, v);
                        updateLock = false;
                    };
                }
                return;
            }
            if (linktype == "PAN")
            {//楽器 directsound
                bool updateLock = false;

                if (arg1 == "PAN")
                {
                    ComboBox link_object = ((ComboBox)link_info);
                    U.SelectedIndexSafety(link_object, 0);

                    link_object.SelectedIndexChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint v = ParseIDFromValueComboBox(link_object.Text);
                        if (v == 0xFFFFFFFF)
                        {
                            v = 0;
                        }
                        v = v & 0x01;

                        uint id = (uint)src_object.Value;
                        U.ForceUpdate(src_object, ((id & (0x7F)) | (v<<7)));
                        updateLock = false;
                    };

                    src_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint id = (uint)src_object.Value;
                        uint v = (id & 0x80) >> 7;
                        link_object.Text = FindNameFromValueComboBoxWhereID(link_object, v);
                        updateLock = false;
                    };
                }
                else if (arg1 == "PANPOT")
                {
                    NumericUpDown link_object = ((NumericUpDown)link_info);

                    link_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint v = (uint)link_object.Value;

                        uint id = (uint)src_object.Value;
                        U.ForceUpdate(src_object, ((id & (0x80)) | (v & 0x7F)));
                        updateLock = false;
                    };

                    src_object.ValueChanged += (sender, e) =>
                    {
                        if (updateLock)
                        {
                            return;
                        }
                        updateLock = true;
                        uint id = (uint)src_object.Value;
                        uint v = (id & 0x7F);
                        U.ForceUpdate(link_object, v);
                        updateLock = false;
                    };
                }

                return;
            }
            if (linktype == "NEWALLOC")
            {//新規に割り当てを行うボタン.
                Button link_object = ((Button)link_info);
                ListBox addressListBox = FindObjectAddressList(prefix, controls);
                link_object.Visible = UpdateStateByAllocEvent(src_object, addressListBox, prefix, controls, args);

                addressListBox.SelectedIndexChanged += (sender, e) =>
                {//選択位置が変わったときは表示するかどうかを再評価する.
                    link_object.Visible = UpdateStateByAllocEvent(src_object, addressListBox, prefix, controls, args);
                };

                src_object.ValueChanged += (sender, e) =>
                {
                    link_object.Visible = UpdateStateByAllocEvent(src_object, addressListBox, prefix, controls, args);
                };
                link_object.Click += (sender, e) =>
                {
                    AllocEvent(arg1,link_object, src_object, prefix, controls);
                };

                NumericUpDown altEventWithTextNud = GetNUDAltEventByAllocEvent(prefix, controls, args);
                if (altEventWithTextNud != null)
                {//死亡セリフや交戦セリフにある代替えテキスト
                    altEventWithTextNud.ValueChanged += (sender, e) =>
                    {
                        link_object.Visible = UpdateStateByAllocEvent(src_object, addressListBox, prefix, controls, args);
                    };
                }

                return;
            }
            if (linktype == "RADIO")
            {//ラジオボタン.
                RadioButton link_object = ((RadioButton)link_info);

                bool updateLock = false;
                link_object.CheckedChanged += (sender, e) =>
                {
                    if (updateLock)
                    {
                        return;
                    }
                    updateLock = true;
                    U.ForceUpdate(src_object, U.atoi(arg1));
                    updateLock = false;
                };
                src_object.ValueChanged += (sender, e) =>
                {
                    if (updateLock)
                    {
                        return;
                    }
                    updateLock = true;
                    if (U.atoi(arg1) == (int)src_object.Value)
                    {
                        link_object.Checked = true;
                    }
                    updateLock = false;
                };
                return;
            }
            if (linktype == "FLAG")
            {//フラグとリンク
                TextBoxEx link_object = ((TextBoxEx)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value;

                    string text;
                    string errorMessage;
                    if (arg1 == "SKIPFFFF" && id == 0xFFFF)
                    {//ワールドマップでフラグを使わない場合は0xFFFFらしい. 0にしろよ!
                        text = "";
                        errorMessage = "";
                    }
                    else
                    {
                        text = GetFlagName(id, out errorMessage);
                    }

                    link_object.ErrorMessage = errorMessage;
                    link_object.Text = text;
                };
                link_object.DoubleClick += (sender, e) =>
                {
                    JumpTo(src_object, link_info, "FLAG", new string[] { });
                };

                return;
            }
            if (linktype == "ID")
            {//IDとリンク
                PanelEx link_object = ((PanelEx)link_info);
                ListBox addressListBox = FindObjectAddressList(prefix, controls);

                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value;

                    string errorMessage;

                    int addressListID = addressListBox.SelectedIndex;
                    if (addressListID < 0)
                    {//未選択のため判別不能
                        return ;
                    }
                    if (arg1 == "PLUS1")
                    {//1からスタートするものたち
                        addressListID ++;
                    }

                    if (src_object.Value == addressListID)
                    {
                        errorMessage = "";
                    }
                    else
                    {
                        errorMessage = R._("リストの並び順とIDが一致していません。\r\nこのデータのIDは、{0}になるべきです。\r\n\r\nIDはデータの逆引きに利用されます。\r\nリストの並び順と一致していないと、バグの原因になります。", U.ToHexString(addressListID));
                    }

                    link_object.ErrorMessage = errorMessage;
                };

                return;
            }
            if (linktype == "STATUSPARAM")
            {//ステータスパラメータ.
                if (link_info is TextBoxEx)
                {
                    TextBoxEx link_object = ((TextBoxEx)link_info);
                    src_object.ValueChanged += (sender, e) =>
                    {
                        uint id = (uint)src_object.Value;
                        link_object.Text = StatusParamForm.GetParamName(id);
                    };
                    link_object.DoubleClick += (sender, e) =>
                    {
                        JumpTo(src_object, link_info, "HEX", new string[] { });
                    };
                    return;
                }
            }
            if (linktype == "MSEC")
            {//ミリ秒.
                if (link_info is TextBoxEx)
                {
                    TextBoxEx link_object = ((TextBoxEx)link_info);
                    src_object.ValueChanged += (sender, e) =>
                    {
                        link_object.Text = InputFormRef.GetFSEC((uint)src_object.Value);
                    };
                    return;
                }
            }
            if (linktype == "STATUSRMENU")
            {//ステータスパラメータ.
                if (link_info is TextBoxEx)
                {
                    TextBoxEx link_object = ((TextBoxEx)link_info);
                    src_object.ValueChanged += (sender, e) =>
                    {
                        uint id = (uint)src_object.Value;
                        if (arg1 == "TID")
                        {
                            link_object.Text = StatusRMenuForm.GetTIDName(id);
                        }
                        else
                        {
                            link_object.Text = StatusRMenuForm.GetMenuName(id);
                        }
                    };
                    return;
                }
            }
            if (linktype == "RAMPROCS")
            {
                //元データが変更されたら、リンクデータも変更する.
                if (link_info is TextBoxEx)
                {
                    TextBoxEx link_object = ((TextBoxEx)link_info);
                    src_object.ValueChanged += (sender, e) =>
                    {
                        uint id = (uint)src_object.Value;
                        if (id <= 8)
                        {
                            link_object.Hide();
                            return;
                        }
                        if (! U.is_02RAMPointer(id))
                        {
                            link_object.Show();
                            link_object.Text = R._("ERROR:RAM領域ではありません");
                            link_object.ErrorMessage = R._("ERROR:RAM領域ではありません");
                            return;
                        }

                        uint romProcs = Program.RAM.u32(id);

                        string text = Program.AsmMapFileAsmCache.GetProcsName(romProcs);
                        if (text == "")
                        {
                            link_object.Hide();
                            return;
                        }
                        link_object.Show();
                        link_object.Text = text;
                        link_object.ErrorMessage = "";
                    };
                    link_object.DoubleClick += (sender, e) =>
                    {
                        JumpTo(src_object, link_info, "RAMPROCS", new string[] { });
                    };
                    return;
                }
            }
            if (linktype == "PROCS")
            {//PROCS関数とリンク.
                //元データが変更されたら、リンクデータも変更する.
                if (link_info is TextBoxEx)
                {
                    TextBoxEx link_object = ((TextBoxEx)link_info);
                    src_object.ValueChanged += (sender, e) =>
                    {
                        uint id = (uint)src_object.Value;
                        if (U.isOffset(id) && U.IsOrderOfHuman(sender))
                        {//人間が、オフセット値を書いた場合、自動補正する
                            id = U.toPointer(id);
                        }

                        if (U.IsValueOdd(id))
                        {
                            link_object.Show();
                            link_object.Text = R._("危険なポインタ");
                            link_object.ErrorMessage = R._("危険なポインタです。\r\nここはPROCSへのネストのため、+1してはいけません。");
                            return;
                        }

                        string text = Program.AsmMapFileAsmCache.GetProcsName(id);
                        if (text == "")
                        {
                            link_object.Hide();
                            return;
                        }
                        link_object.Show();
                        link_object.Text = text;
                        link_object.ErrorMessage = "";
                    };
                    link_object.DoubleClick += (sender, e) =>
                    {
                        JumpTo(src_object, link_info, "PROCS", new string[] { });
                    };
                    return;
                }
            }
            if (linktype == "ASM")
            {//ASM関数とリンク.
                bool isSWITCH = (arg1 == "SWITCH");

                //元データが変更されたら、リンクデータも変更する.
                if (link_info is TextBoxEx)
                {
                    TextBoxEx link_object = ((TextBoxEx)link_info);
                    src_object.ValueChanged += (sender, e) =>
                    {
                        uint id = (uint)src_object.Value;
                        if (U.isOffset(id) && U.IsOrderOfHuman(sender))
                        {//人間が、オフセット値を書いた場合、自動補正する
                            id = U.toPointer(id);
                        }

                        string errormessage;
                        string text = Program.AsmMapFileAsmCache.GetASMName(id, isSWITCH, out errormessage);
                        if (text == "" && errormessage == "")
                        {
                            link_object.Hide();
                            return;
                        }
                        link_object.Show();
                        link_object.Text = text;
                        link_object.ErrorMessage = errormessage;
                    };
                    link_object.DoubleClick += (sender, e) =>
                    {
                        JumpTo(src_object, link_info, "ASM", new string[] { });
                    };
                    return;
                }
            }
            if (linktype == "EVENT" || linktype == "ITEMSHOP")
            {//EVENTとリンク.
                //元データが変更されたら、リンクデータも変更する.
                if (link_info is TextBoxEx)
                {
                    TextBoxEx link_object = ((TextBoxEx)link_info);
                    src_object.ValueChanged += (sender, e) =>
                    {
                        uint id = (uint)src_object.Value;
                        if (U.isOffset(id) && U.IsOrderOfHuman(sender))
                        {//人間が、オフセット値を書いた場合、自動補正する
                            id = U.toPointer(id);
                        }

                        string errorMessage;
                        string text = Program.AsmMapFileAsmCache.GetEventName(id, out errorMessage);
                        if (text == "" && errorMessage == "")
                        {
                            link_object.Hide();
                            return;
                        }
                        link_object.Show();
                        link_object.Text = text;
                        link_object.ErrorMessage = errorMessage;
                    };
                    link_object.DoubleClick += (sender, e) =>
                    {
                        JumpTo(src_object, link_info, "EVENT", new string[] { });
                    };
                    return;
                }
            }
            if (linktype == "EVENTORCHEST")
            {//EVENTまたはランダム宝箱
                //元データが変更されたら、リンクデータも変更する.
                if (link_info is TextBoxEx)
                {
                    TextBoxEx link_object = ((TextBoxEx)link_info);
                    src_object.ValueChanged += (sender, e) =>
                    {
                        uint id = (uint)src_object.Value;
                        if (U.isOffset(id) && U.IsOrderOfHuman(sender))
                        {//人間が、オフセット値を書いた場合、自動補正する
                            id = U.toPointer(id);
                        }

                        string errorMessage;
                        string text = Program.AsmMapFileAsmCache.GetEventName(id, out errorMessage);
                        if (text == "" && errorMessage == "")
                        {
                            link_object.Hide();
                            return;
                        }
                        link_object.Show();
                        link_object.Text = text;
                        link_object.ErrorMessage = errorMessage;
                    };
                    link_object.DoubleClick += (sender, e) =>
                    {
                        JumpTo(src_object, link_info, "EVENTORCHEST", new string[] { });
                    };
                    return;
                }
            }
            if (linktype == "PALETTE")
            {//ステータスパラメータ.
                bool updateLock = false;
                if (arg1 == "COLOR" && link_info is Label)
                {
                    Label obj = (Label)link_info;
                    obj.Cursor = Cursors.Hand;
                    src_object.ValueChanged += (sender, e) =>
                    {
                        int dp = (int)src_object.Value;
                        byte dr = (byte)((dp & 0x1F));
                        byte dg = (byte)(((dp >> 5) & 0x1F));
                        byte db = (byte)(((dp >> 10) & 0x1F));
                        obj.BackColor = Color.FromArgb(dr << 3, dg << 3, db << 3);
                    };
                    obj.Click += (sender, e) =>
                    {
                        ColorDialog cd = new ColorDialog();
                        cd.Color = obj.BackColor;
                        if (cd.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }
                        byte r = (byte)(((cd.Color.R>>3) & 0x1F));
                        byte g = (byte)(((cd.Color.G>>3) & 0x1F));
                        byte b = (byte)(((cd.Color.B>>3) & 0x1F));

                        U.ForceUpdate(src_object, (r) | (g << 5) | (b << 5));
                    };
                }
                else if (arg1 == "R" && link_info is NumericUpDown)
                {
                    NumericUpDown obj = (NumericUpDown)link_info;

                    src_object.ValueChanged += (sender, e) =>
                    {
                        if(updateLock)
                        {
                            return ;
                        }
                        int dp = (int)src_object.Value;
                        byte r = (byte)((dp & 0x1F));
                        updateLock = true;
                        U.ForceUpdate(obj , r << 3);
                        updateLock = false;
                    };
                    obj.ValueChanged += (sender, e) =>
                    {
                        int v = (int)obj.Value;
                        obj.BackColor = Color.FromArgb(v, 0, 0);
                        obj.ForeColor = Color.FromArgb(0, 255 - v, 255 - v);

                        if(updateLock)
                        {
                            return ;
                        }
                        updateLock = true;
                        int dp = (int)src_object.Value;
                        dp = (int)((dp & ~0x1F) | (v >> 3) );
                        U.ForceUpdate(src_object, dp);
                        updateLock = false;
                    };
                }
                else if (arg1 == "G" && link_info is NumericUpDown)
                {
                    NumericUpDown obj = (NumericUpDown)link_info;

                    src_object.ValueChanged += (sender, e) =>
                    {
                        if(updateLock)
                        {
                            return ;
                        }
                        int dp = (int)src_object.Value;
                        byte g = (byte)(((dp >> 5) & 0x1F));
                        updateLock = true;
                        U.ForceUpdate(obj, g << 3);
                        updateLock = false;
                    };
                    obj.ValueChanged += (sender, e) =>
                    {
                        int v = (int)obj.Value;
                        obj.BackColor = Color.FromArgb(0, v, 0);
                        obj.ForeColor = Color.FromArgb(255 - v, 0, 255 - v);

                        if(updateLock)
                        {
                            return ;
                        }
                        updateLock = true;
                        int dp = (int)src_object.Value;
                        dp = (int)(((dp>>5) & ~0x1F) | (v >> 3) );
                        U.ForceUpdate(src_object, dp);
                        updateLock = false;
                    };
                }
                else if (arg1 == "B" && link_info is NumericUpDown)
                {
                    NumericUpDown obj = (NumericUpDown)link_info;

                    src_object.ValueChanged += (sender, e) =>
                    {
                        if(updateLock)
                        {
                            return ;
                        }
                        int dp = (int)src_object.Value;
                        byte b = (byte)(((dp >> 10) & 0x1F));
                        updateLock = true;
                        U.ForceUpdate(obj, b << 3);
                        updateLock = false;
                    };
                    obj.ValueChanged += (sender, e) =>
                    {
                        int v = (int)obj.Value;
                        obj.BackColor = Color.FromArgb(0, 0, v);
                        obj.ForeColor = Color.FromArgb(255 - v, 255 - v, 0);

                        if(updateLock)
                        {
                            return ;
                        }
                        updateLock = true;
                        int dp = (int)src_object.Value;
                        dp = (int)(((dp>>10) & ~0x1F) | (v >> 3) );
                        U.ForceUpdate(src_object, dp);
                        updateLock = false;
                    };
                }
                return;
            }
            if (linktype == "CHAPTORMODE")
            {//編
                TextBoxEx link_object = ((TextBoxEx)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value;

                    string text;
                    if (arg1 == "ANYFF" && id == 0xFF)
                    {//ワールドマップでフラグを使わない場合は0xFFらしい. 0にしろよ!
                        text = "ANY";
                    }
                    else
                    {
                        text = GetEditon(id);
                    }

                    link_object.Text = text;
                };

                return;
            }
            if (linktype == "DIFFCULTY")
            {
                TextBoxEx link_object = ((TextBoxEx)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value;

                    string text = MapSettingDifficultyForm.GetDiffcultyText(id);

                    link_object.Text = text;
                };
                link_object.DoubleClick += (sender, e) =>
                {
                    JumpTo(src_object, link_info, "DIFFCULTY", new string[] { });
                };
                return;
            }
            if (linktype == "CLASSTYPE")
            {
                TextBoxEx link_object = ((TextBoxEx)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value;

                    string text = ClassForm.GetClassType(id);
                    link_object.Text = text;
                };
                link_object.DoubleClick += (sender, e) =>
                {
                    JumpTo(src_object, link_info, "CLASSTYPE", new string[] { });
                };
                return;
            }
            if (linktype == "GAMEOPTION")
            {
                TextBoxEx link_object = ((TextBoxEx)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    uint id = (uint)src_object.Value;

                    string text = StatusOptionForm.GetNameIndex(id);
                    link_object.Text = text;
                };
                link_object.DoubleClick += (sender, e) =>
                {
                    JumpTo(src_object, link_info, "GAMEOPTION", new string[] { });
                };
                return;
            }
            if (linktype == "CLASSTYPEICON")
            {//クラスタイプ名からアイコン表示
                src_object.ValueChanged += (sender, e) =>
                {
                    PictureBox link_object = ((PictureBox)link_info);

                    Bitmap bitmap;
                    bitmap = ClassForm.DrawClassTypeIcon((uint)src_object.Value);
                    U.MakeTransparent(bitmap);
                    link_object.Image = bitmap;
                };

                link_info.Cursor = Cursors.Hand;
                link_info.Click += (sender, e) =>
                {
                    JumpTo(src_object, link_info, "CLASSTYPE", new string[] { });
                };

                return;
            }
            if (linktype == "DWORD")
            {//HEXDUMP
                TextBoxEx link_object = ((TextBoxEx)link_info);
                uint maxid = link_id + 3;

                NumericUpDown[] src_objests = new NumericUpDown[maxid - link_id + 1];
                src_objests[0] = src_object;
                for (uint nn = 1; nn < src_objests.Length; nn++)
                {
                    src_objests[nn] = FindSRCObject(prefix, controls, nn + link_id);
                    if (src_objests[nn] == null)
                    {
                        throw new Exception("broken link:" + link_info.Name + " nn:" + nn.ToString());
                    }
                }

                bool updateLock = false;
                EventHandler link_text_update_function = (sender, e) =>
                {
                    if (updateLock)
                    {
                        return;
                    }
                    updateLock = true;

                    string str = "";
                    for (uint nn = 0; nn < src_objests.Length; nn++)
                    {
                        str = ((byte)src_objests[nn].Value).ToString("X02") + str;
                    }

                    link_object.Text = str;
                    updateLock = false;

                };

                for (uint nn = 0; nn < src_objests.Length; nn++)
                {
                    src_objests[nn].ValueChanged += link_text_update_function;
                }
                link_text_update_function(null, null);
                return;
            }
            if (linktype == "RAMUNITSTATE" 
                || linktype == "ROMUNITPOINTER" 
                || linktype == "ROMCLASSPOINTER"
                || linktype == "RAMUNITAID"
                || linktype == "PatchImage"
                || linktype == "AP"
                )
            {
                return;
            }
            if (linktype == "TEXTOREVENT")
            {//敵捕獲パッチが解放時専用セリフに、イベントポインタ領域を割り当てているので、両立を目指す.
                if (link_info is TextBoxEx)
                {
                    TextBoxEx link_object = ((TextBoxEx)link_info);
                    src_object.ValueChanged += (sender, e) =>
                    {
                        uint id = (uint)src_object.Value;
                        if (U.isSafetyOffset(id))
                        {
                            string errorMessage;
                            string text = Program.AsmMapFileAsmCache.GetEventName(id, out errorMessage);
                            if (text == "" && errorMessage == "")
                            {
                                link_object.Hide();
                                return;
                            }
                            link_object.Show();
                            link_object.Text = text;
                            link_object.ErrorMessage = errorMessage;
                        }
                        else
                        {
                            string str = TextForm.Direct(id);
                            link_object.ErrorMessage = TextForm.GetErrorMessage(str, id, arg1);
                            link_object.Text = str;
                        }
                    };
                    link_object.DoubleClick += (sender, e) =>
                    {
                        JumpTo(src_object, link_info, "TEXTOREVENT", new string[] { });
                    };
                    return;
                }
            }
            if (linktype == "TILE")
            {//タイル名
                TextBoxEx link_object = ((TextBoxEx)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    //タイル名の取得
                    link_object.Text = MapTerrainNameForm.GetName((uint)src_object.Value);
                };
                return;
            }
            if (linktype == "HZ1024")
            {//HZ1024.
                TextBoxEx link_object = ((TextBoxEx)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    double hz1024 = (double)src_object.Value;
                    link_object.Text = R._("{0}hz",hz1024 / 1024);
                };
                return;
            }
            if (linktype == "VENNOUWEAPONLOCK")
            {//専用武器 vennou
                TextBoxEx link_object = ((TextBoxEx)link_info);

                src_object.ValueChanged += (sender, e) =>
                {
                    uint addr = (uint)src_object.Value;
                    link_object.Text = VennouWeaponLockForm.GetNames(addr);
                };

                link_info.Cursor = Cursors.Hand;
                link_info.Click += (sender, e) =>
                {
                    JumpTo(src_object, link_info, "VENNOUWEAPONLOCK", new string[] { });
                };
                return;
            }
            if (linktype == "VENNOUWEAPONLOCK_INDEX")
            {
                TextBoxEx link_object = ((TextBoxEx)link_info);

                src_object.ValueChanged += (sender, e) =>
                {
                    uint i = (uint)src_object.Value;
                    link_object.Text = VennouWeaponLockForm.GetNamesByIndex(i);
                };

                link_info.Cursor = Cursors.Hand;
                link_info.Click += (sender, e) =>
                {
                    PatchForm f = (PatchForm)InputFormRef.JumpForm<PatchForm>();
                    f.JumpTo("WeaponLockArray SkillSystems", 1);
                };
                return;
            }
            if (linktype == "BADSTATUS")
            {//バッドステータス
                TextBoxEx link_object = ((TextBoxEx)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    link_object.Text = GetBADSTATUS((uint)src_object.Value);
                };
                return;
            }
            if (linktype == "SMEPROMOLIST")
            {//smeのCC分岐
                TextBoxEx link_object = ((TextBoxEx)link_info);

                src_object.ValueChanged += (sender, e) =>
                {
                    uint addr = (uint)src_object.Value;
                    link_object.Text = SMEPromoListForm.GetNames(addr);
                };

                link_info.Cursor = Cursors.Hand;
                link_info.Click += (sender, e) =>
                {
                    JumpTo(src_object, link_info, "SMEPROMOLIST", new string[] { });
                };
                return;
            }
            if (linktype == "SKILL")
            {//SKILL名を表示
                TextBoxEx link_object = ((TextBoxEx)link_info);
                src_object.ValueChanged += (sender, e) =>
                {
                    link_object.Text = SkillUtil.GetSkillName((uint)src_object.Value);
                };

                return;
            }
            if (linktype == "SKILLICON")
            {//SKILL名からアイコン表示
                src_object.ValueChanged += (sender, e) =>
                {
                    PictureBox link_object = ((PictureBox)link_info);

                    Bitmap bitmap = SkillUtil.DrawIcon((uint)src_object.Value);
                    U.MakeTransparent(bitmap);
                    link_object.Image = bitmap;
                };

                return;
            }
            
            
#if DEBUG            
            throw new Exception("linkerror linktype(" + linktype + ") error  link_info:" + link_info.Name);
#endif
        }

        static ListBox FindObjectAddressList(string prefix, List<Control> controls)
        {
            ListBox addresslist = (ListBox)FindObject(prefix, controls, "AddressList");
            if (addresslist == null)
            {//アドレスリストがない。
                //UNIONの可能性を検討
                string unionprefix = GetUnionPrefix(prefix);
                if (unionprefix.Length > 0)
                {
                    addresslist = (ListBox)FindObject(unionprefix, controls, "AddressList");
                }
            }
            return addresslist;
        }

        //新規割り当てボタンを表示するかどうか
        static bool UpdateStateByAllocEvent(NumericUpDown src_object
            ,ListBox addressListBox
            , string prefix ,List<Control> controls, string[] args)
        {
            if (addressListBox.SelectedIndex < 0)
            {//アドレスリストに項目がない場合、 0初期化されているが、そこにボタンを出すのは見栄えが悪いのでダメ.
                return false;
            }

            string arg1 = U.at(args, 0);
            if (addressListBox.SelectedIndex == 0)
            {
                if (arg1 == "ITEMSTATBOOSTER"
                    || arg1 == "EFFECTIVENESS"
                    )
                {//アイテム:0 null には表示させたくない.
                    return false;
                }
            }
            if (arg1 == "ALTEVENT")
            {//死亡セリフや交戦セリフにある、テキストの代わりのイベントの場合
                NumericUpDown c = GetNUDAltEventByAllocEvent(prefix, controls, args);
                if (c != null)
                {
                    if (((NumericUpDown)c).Value != 0)
                    {//テキストIDが入力されているので、確保ボタンを消す.
                        return false;
                    }
                }
            }

            if (src_object.Value <= 0)
            {
                return true;
            }
            return false;
        }

        //死亡セリフや交戦セリフにある、テキストの代わりのイベントを表示する場合の、テキストを入力するNUDオブジェクトの取得
        //失敗したらnull
        static NumericUpDown GetNUDAltEventByAllocEvent(string prefix
            ,List<Control> controls
            , string[] args)
        {
            string arg1 = U.at(args, 0);
            if (arg1 == "ALTEVENT")
            {//死亡セリフや交戦セリフにある、テキストの代わりのイベントの場合
                string arg2 = U.at(args, 1);
                if (arg2 != "")
                {
                    Control c = FindObjectByFormFirstMatch<NumericUpDown>(controls,prefix + arg2);
                    if (c is NumericUpDown && c != null)
                    {
                        return (NumericUpDown)c;
                    }
                }
            }
            return null;
        }


        static string GetUnionPrefix(string prefix)
        {
            string[] sp = prefix.Split('_');

            if (sp.Length <= 2)
            {//UNIONではない L_0_ 
                return "";
            }
            //UNIONの可能性あり  N01_UNION_
            string ret = "";
            for (int n = 0 ; n < sp.Length - 2; n++)
            {
                ret += sp[n] + "_";
            }
            return ret;
        }

        static void AllocEvent(string arg1
            , Button link_object
            , NumericUpDown src_object
            , string prefix, List<Control> controls)
        {

            if (link_object.Visible == false)
            {//非表示なのにできわけないだろう.
                return ;
            }

            //なぜかイベントが2回呼ばれることがある???
            //仕方ないので、モンキーパッチではあるが、時刻を記録して逃げる.
            long lastUpdate = link_object.Tag == null ? 0 : (long)link_object.Tag;
            if (lastUpdate >= DateTime.Now.Ticks - 5)
            {
                return;
            }
            link_object.Tag = DateTime.Now.Ticks;

            //書き込みボタンの検出を行います.
            Button writeButton = (Button)FindObject(prefix, controls, "WriteButton");
            if (writeButton == null)
            {//書き込みボタンがない。
                //UNIONの可能性を検討
                string unionprefix = GetUnionPrefix(prefix);
                if (unionprefix.Length > 0)
                {
                    writeButton = (Button)FindObject(unionprefix, controls, "WriteButton");
                }
                if (writeButton == null)
                {//書き込みボタンの取得ができなかった...
                    Debug.Assert(false);
                    return;
                }
            }

            if (Program.ROM.RomInfo.version() == 8)
            {
                if (arg1 == "EVENT1"
                    || arg1 == "EVENT2"
                    || arg1 == "EVENT3"
                    || arg1 == "EVENT4"
                    || arg1 == "EVENT5"
                    || arg1 == "EVENT6"
                    )
                {//イベントテンプレートは複雑なので別処理
                    AllocEvent_EventTemplate(arg1, link_object, src_object, prefix, controls, writeButton);
                    return;
                }
                else if (arg1 == "EVENTORCHEST")
                {
                    bool isRandomChest = CheckRandomChest(src_object);
                    if (isRandomChest == false)
                    {
                        AllocEvent_EventTemplate("EVENT1", link_object, src_object, prefix, controls, writeButton);
                        return;
                    }
                }
            }
            
            string alllocQMessage;
            string alllocedMessage;
            byte[] alloc;
            if (arg1 == "EVENT"
                || arg1 == "EVENT1"
                || arg1 == "EVENT2"
                || arg1 == "EVENT3"
                || arg1 == "EVENT4"
                || arg1 == "EVENT5"
                || arg1 == "EVENT6"
                || arg1 == "ALTEVENT"
                )
            {
                alllocQMessage = R._("新規にイベント命令の領域を割り当てますか？");
                alllocedMessage = R._("領域を割り振りました。イベント命令画面からイベントを作ってください。");
                //とりあえず終端命令だけのイベントを作る.
                alloc = Program.ROM.RomInfo.defualt_event_script_toplevel_code();
            }
            else if (arg1 == "EVENTORCHEST")
            {
                bool isRandomChest = CheckRandomChest(src_object);
                if (isRandomChest)
                {
                    alllocQMessage = R._("新規にランダム宝箱の領域を割り当てますか？");
                    alllocedMessage = R._("領域を割り振りました。ランダム宝箱の画面からイベントを作ってください。");
                    alloc = new byte[] { 0x74, 0x14, 0x75, 0x32, 0x76, 0x1E, 0x00, 0x00 };
                }
                else
                {
                    alllocQMessage = R._("新規にイベント命令の領域を割り当てますか？");
                    alllocedMessage = R._("領域を割り振りました。イベント命令画面からイベントを作ってください。");
                    //とりあえず終端命令だけのイベントを作る.
                    alloc = Program.ROM.RomInfo.defualt_event_script_toplevel_code();
                }
            }
            else if (arg1 == "ITEMSHOP")
            {
                alllocQMessage = R._("新規に店データを作りますか？");
                alllocedMessage = R._("領域を割り振りました。店画面より値を割り振ってください。");
                alloc = new byte[12];
                alloc[0] = 1;
                alloc[2] = 2;
                alloc[4] = 3;
                alloc[6] = 4;
                alloc[8] = 5;
                alloc[10] = 0;
            }
            else if (arg1 == "ITEMSTATBOOSTER")
            {
                alllocQMessage = R._("このアイテムに、能力補正を新規に割り振りますか？");
                alllocedMessage = R._("領域を割り振りました。能力補正画面より値を割り振ってください。");
                if (CheckVennoExtendsStatBooster(src_object))
                {
                    alloc = new byte[16];
                    alloc[1] = 5;
                }
                else
                {
                    alloc = new byte[20];
                    alloc[1] = 5;
                }
            }
            else if (arg1 == "EFFECTIVENESS")
            {
                alllocQMessage = R._("このアイテムに、特効効果を新規に割り振りますか？");
                alllocedMessage = R._("領域を割り振りました。特効効果画面より値を割り振ってください。");
                if (PatchUtil.SearchClassType() == PatchUtil.class_type_enum.SkillSystems_Rework)
                {//SkillSystemsによる 特効リワーク
                    alloc = U.FillArray(12, 0);
                    alloc[1] = 6;
                    alloc[2] = 1; //アーマー
                    alloc[5] = 6;
                    alloc[6] = 2; //騎兵
                }
                else
                {
                    alloc = U.FillArray(12, 1);
                    alloc[11] = 0;
                }
            }
            else if (arg1 == "IMAGEUNITPALETTE")
            {
                alllocQMessage = R._("パレットを新規に割り振りますか？");
                alllocedMessage = R._("領域を割り振りました。");

                byte[] basepalette = LZ77.decompress(Program.ROM.Data,ImageUnitPaletteForm.GetPaletteAddr(1));
                alloc = LZ77.compress(basepalette);
            }
            else if (arg1 == "BATTLEANIMEPOINTER")
            {
                alllocQMessage = R._("新規に、武器単位の戦闘アニメーション指定データを割り振りますか？");
                alllocedMessage = R._("領域を割り振りました。武器単位の戦闘アニメーションを割り振ってください。");
                alloc = new byte[] { 0x00, 0x01, 0x03, 0x00, 0x09, 0x01, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00 };
            }
            else if (arg1 == "VENNOUWEAPONLOCK")
            {
                alllocQMessage = R._("新規に、専用武器のデータを割り振りますか？");
                alllocedMessage = R._("領域を割り振りました。専用武器を割り振ってください。");
                alloc = new byte[] { 0x01, 0x01, 0x02, 0x03, 0x04, 0x05, 0x00 };
            }
            else if (arg1 == "SMEPROMOLIST")
            {
                alllocQMessage = R._("新規に、Promotionのデータを割り振りますか？");
                alllocedMessage = R._("領域を割り振りました。データを割り振ってください。");
                alloc = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x00 };
            }
            else
            {//リンクミス.
                Debug.Assert(false);
                return;
            }

            DialogResult dr = R.ShowYesNo(alllocQMessage);

            link_object.Tag = DateTime.Now.Ticks;
            if (dr != System.Windows.Forms.DialogResult.Yes)
            {//ユーザが割り当てを拒否した.
                return ;
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData(src_object,"NewAlloc",arg1);
            uint addr = InputFormRef.AppendBinaryData(alloc, undodata);
            if (addr == U.NOT_FOUND)
            {//割り当て失敗
                return ;
            }

            U.ForceUpdate(src_object, addr);
            //新規に追加した分のデータを書き込み.
            Program.Undo.Push(undodata);

            //書き込みボタンを押して、本体に新規に確保したアドレスを確定させます.
            writeButton.PerformClick();


            R.ShowWarning(alllocedMessage);
            return ;
        }
        static void AllocEvent_EventTemplate(string arg1
            , Button link_object
            , NumericUpDown src_object
            , string prefix, List<Control> controls, Button writeButton)
        {

            byte[] alloc;
            uint callEventAddr;
            bool needFlag03;
            bool counterReinforcementEvent = false;
            if (arg1 == "EVENT1")
            {
                EventTemplate1Form f = (EventTemplate1Form)InputFormRef.JumpFormLow<EventTemplate1Form>();
                f.Init(controls);
                f.ShowDialog();
                alloc = f.GenCode;
                callEventAddr = f.CallEventAddr;
                needFlag03 = f.NeedFlag03;
            }
            else if (arg1 == "EVENT2")
            {
                EventTemplate2Form f = (EventTemplate2Form)InputFormRef.JumpFormLow<EventTemplate2Form>();
                f.Init(controls);
                f.ShowDialog();
                alloc = f.GenCode;
                callEventAddr = f.CallEventAddr;
                needFlag03 = f.NeedFlag03;
            }
            else if (arg1 == "EVENT3")
            {
                EventTemplate3Form f = (EventTemplate3Form)InputFormRef.JumpFormLow<EventTemplate3Form>();
                f.Init(controls);
                f.ShowDialog();
                alloc = f.GenCode;
                callEventAddr = f.CallEventAddr;
                needFlag03 = f.NeedFlag03;
                counterReinforcementEvent = f.CounterReinforcementEvent;
            }
            else if (arg1 == "EVENT4")
            {
                EventTemplate4Form f = (EventTemplate4Form)InputFormRef.JumpFormLow<EventTemplate4Form>();
                f.Init(controls);
                f.ShowDialog();
                alloc = f.GenCode;
                callEventAddr = f.CallEventAddr;
                needFlag03 = f.NeedFlag03;
            }
            else if (arg1 == "EVENT5")
            {
                EventTemplate5Form f = (EventTemplate5Form)InputFormRef.JumpFormLow<EventTemplate5Form>();
                f.Init(controls);
                f.ShowDialog();
                alloc = f.GenCode;
                callEventAddr = f.CallEventAddr;
                needFlag03 = f.NeedFlag03;
            }
            else if (arg1 == "EVENT6")
            {
                EventTemplate6Form f = (EventTemplate6Form)InputFormRef.JumpFormLow<EventTemplate6Form>();
                f.Init(controls);
                f.ShowDialog();
                alloc = f.GenCode;
                callEventAddr = f.CallEventAddr;
                needFlag03 = f.NeedFlag03;
            }
            else
            {
                Debug.Assert(false);
                return;
            }

            if (alloc == null && callEventAddr == U.NOT_FOUND)
            {//キャンセルされた
                return;
            }

            //必要であれば、勝利フラグを立てる. または不要ならば下す.
            {
                string flag_name = prefix + "W2";
                Control c = FindObjectByForm<NumericUpDown>(controls, flag_name);
                if ((c is NumericUpDown))
                {
                    NumericUpDown nup = ((NumericUpDown)c);
                    if (needFlag03)
                    {
                        nup.Value = 0x03;
                    }
                    else
                    {
                        if (nup.Value == 0x03)
                        {//不要なのに、0x03が設定されていれば下す.
                            nup.Value = 0;
                        }
                    }
                }
            }
            //カウンターを利用した増援
            if (counterReinforcementEvent)
            {
                string flag_name = prefix + "B8";
                Control c = FindObjectByForm<NumericUpDown>(controls, flag_name);
                if ((c is NumericUpDown))
                {
                    NumericUpDown nup = ((NumericUpDown)c);
                    nup.Value = 1;
                }
                flag_name = prefix + "B9";
                c = FindObjectByForm<NumericUpDown>(controls, flag_name);
                if ((c is NumericUpDown))
                {
                    NumericUpDown nup = ((NumericUpDown)c);
                    nup.Value = 255;
                }
            }

            if (callEventAddr != U.NOT_FOUND)
            {//既存のイベント呼び出し
                if (callEventAddr == 1)
                {
                    src_object.Value = callEventAddr;
                }
                else
                {
                    src_object.Value = U.toPointer(callEventAddr);
                }

                //書き込みボタンを押して、本体に新規に確保したアドレスを確定させます.
                writeButton.PerformClick();
                return;
            }
            //新規イベント割当
            Undo.UndoData undodata = Program.Undo.NewUndoData(src_object, "NewAlloc", arg1);
            uint addr = InputFormRef.AppendBinaryData(alloc, undodata);
            if (addr == U.NOT_FOUND)
            {//割り当て失敗
                return;
            }

            U.ForceUpdate(src_object, addr);
            //新規に追加した分のデータを書き込み.
            Program.Undo.Push(undodata);


            //書き込みボタンを押して、本体に新規に確保したアドレスを確定させます.
            writeButton.PerformClick();

            string alllocedMessage = R._("領域を割り振りました。イベント命令画面からイベントを作ってください。");
            R.ShowWarning(alllocedMessage);
        }

        static void MakeJumpEventOne(string prefix, List<Control> controls, Label jump_object, NumericUpDown Address, ListBox AddressList)
        {
            String link_name = SkipPrefixName(jump_object.Name, prefix);
            if (link_name.Length <= 0)
            {
                return;
            }

            //以下のような表記になる.
            //J_数字ID部_属性部
            //J_数字ID部_属性部_引数部
            if (link_name[0] != 'J' || link_name[1] != '_')
            {
                return;
            }

            string[] sp = link_name.Split('_');
            if (sp.Length <= 2)
            {
                return;
            }

            uint id = U.atoi(sp[1]);
            string linktype = sp[2];
            string[] args = U.subrange(sp, 3, (uint)sp.Length);

            if (sp[1] == "ID")
            {
                args = ReplaceJumpArgsMacro(prefix, controls, args);
                makeJumpEventHandler(AddressList, jump_object, linktype, args);
            }
            else if (sp[1] == "ADDR")
            {
                args = ReplaceJumpArgsMacro(prefix, controls, args);
                makeJumpEventHandler(Address, jump_object, linktype, args);
            }
            else
            {
                NumericUpDown src_object = null;
                foreach (Control search_info in controls)
                {
                    if (!(search_info is NumericUpDown))
                    {
                        continue;
                    }
                    String search_name = SkipPrefixName(search_info.Name, prefix);
                    if (search_name.Length <= 0)
                    {
                        continue;
                    }

                    if (!IsTypeWord(search_name[0])) continue;
                    uint search_id = U.atoi(search_name.Substring(1));

                    if (id == search_id)
                    {
                        src_object = ((NumericUpDown)search_info);
                        break;
                    }
                }
                if (src_object == null)
                {
                    throw new Exception("broken link:" + link_name);
                }
                if (sp[1] == "NONE")
                {
                    return;
                }
                if (linktype.IndexOf("$COMBO") >= 0)
                {
                    return;
                }

                args = ReplaceJumpArgsMacro(prefix, controls, args);
                makeJumpEventHandler(src_object, jump_object, linktype, args);
            }
        }


        static void MakeJumpEvent(string prefix, List<Control> controls , NumericUpDown Address,ListBox AddressList)
        {
            foreach (Control link_info in controls)
            {
                if (!(link_info is Label))
                {
                    continue;
                }
                Label jump_object = ((Label)link_info);
                MakeJumpEventOne(prefix, controls, jump_object, Address, AddressList);
            }
        }

        static string[] ReplaceJumpArgsMacro(string prefix, List<Control> controls,string[] sp)
        {
            for (int i = 0; i < sp.Length; i++ )
            {
                foreach (Control search_info in controls)
                {
                    if (!(search_info is NumericUpDown))
                    {
                        continue;
                    }
                    String search_name = SkipPrefixName(search_info.Name, prefix);
                    if (search_name.Length <= 0)
                    {
                        continue;
                    }

                    if (sp[i] == search_name)
                    {
                        sp[i] = ((NumericUpDown)search_info).Value.ToString();
                    }
                }
            }
            return sp;
        }

        static void JumpHoverColorEvent(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            if (c.Cursor != Cursors.Hand)
            {
                return;
            }
            c.BackColor = OptionForm.Color_List_SelectedColor();
        }
        static void JumpUnHoverColorEvent(object sender, EventArgs e)
        {
            if (!(sender is Control))
            {
                return;
            }
            Control c = (Control)sender;
            c.BackColor = OptionForm.Color_Control_BackColor();
        }

        //Labelがクリックできることをユーザに伝える.
        public static void markupJumpLabel(Label jump_object)
        {
            jump_object.Cursor = Cursors.Hand;
            jump_object.Font=new System.Drawing.Font(jump_object.Font.Name,jump_object.Font.Size,System.Drawing.FontStyle.Underline);
            jump_object.MouseEnter -= JumpHoverColorEvent;
            jump_object.MouseLeave -= JumpUnHoverColorEvent;
            jump_object.MouseEnter += JumpHoverColorEvent;
            jump_object.MouseLeave += JumpUnHoverColorEvent;
        }
        //上で作った変化のキャンセル.
        public static void unmarkupJumpLabel(Label jump_object)
        {
            jump_object.Cursor = Cursors.Arrow;
            jump_object.Font=new System.Drawing.Font(jump_object.Font.Name,jump_object.Font.Size,System.Drawing.FontStyle.Regular);
            jump_object.MouseEnter -= JumpHoverColorEvent;
            jump_object.MouseLeave -= JumpUnHoverColorEvent;
        }
        public static void JumpTo(Control src_object, Control jump_object, String linktype, string[] args)
        {
            if (src_object is NumericUpDown)
            {
                NumericUpDown obj = (NumericUpDown)src_object;
                uint value = (uint)(obj.Value);
                JumpTo(obj, value, linktype, args);
            }
            else if (src_object is ListBoxEx)
            {
                uint value = (uint)((ListBox)src_object).SelectedIndex;
                JumpTo(null, value, linktype, args);
            }
            else
            {
                throw new Exception("linkerror src_object!=typeof(NumericUpDown) or (ListBox)  src_object:" + src_object.Name);
            }
        }

        public static void JumpTo(NumericUpDown src_object, uint value, String linktype, string[] args)
        {
            //よく参照するarg1を変数化.
            string arg1 = U.at(args, 0);
                
            if (linktype == "UNIT")
            {
                if (Program.ROM.RomInfo.version() >= 8)
                {
                    InputFormRef.JumpForm<UnitForm>(value - 1, "AddressList", src_object);
                }
                else if (Program.ROM.RomInfo.version() >= 7)
                {//FE7
                    InputFormRef.JumpForm<UnitFE7Form>(value - 1, "AddressList", src_object);
                }
                else
                {//FE6
                    InputFormRef.JumpForm<UnitFE6Form>(value - 1, "AddressList", src_object);
                }
            }
            else if (linktype == "CLASS")
            {
                if (Program.ROM.RomInfo.version() >= 7)
                {//FE7 FE8
                    InputFormRef.JumpForm<ClassForm>(value, "AddressList", src_object);
                }
                else
                {//FE6
                    InputFormRef.JumpForm<ClassFE6Form>(value, "AddressList", src_object);
                }
            }
            else if (linktype == "ICON")
            {
                InputFormRef.JumpForm<ImageItemIconForm>(value, "AddressList", src_object);
            }
            else if (linktype == "MAP")
            {
                if (Program.ROM.RomInfo.version() >= 8)
                {
                    InputFormRef.JumpForm<MapSettingForm>(value, "AddressList", src_object);
                }
                else if (Program.ROM.RomInfo.version() >= 7)
                {//FE7
                    InputFormRef.JumpForm<MapSettingFE7Form>(value, "AddressList", src_object);
                }
                else
                {//FE6
                    InputFormRef.JumpForm<MapSettingFE6Form>(value, "AddressList", src_object);
                }
            }
            else if (linktype == "WAITICON")
            {
                InputFormRef.JumpForm<ImageUnitWaitIconFrom>(value, "AddressList", src_object);
            }
            else if (linktype == "MOVEICON")
            {
                InputFormRef.JumpForm<ImageUnitMoveIconFrom>(value - 1, "AddressList", src_object);
            }
            else if (linktype == "SONG")
            {
                InputFormRef.JumpForm<SongTableForm>(value, "AddressList", src_object);
            }
            else if (linktype == "SOUNDROOM")
            {
                if (Program.ROM.RomInfo.version() == 6)
                {
                    SoundRoomFE6Form f = (SoundRoomFE6Form)InputFormRef.JumpForm<SoundRoomFE6Form>(U.NOT_FOUND, "AddressList", src_object);
                    f.JumpToSongID(value);
                }
                else
                {
                    SoundRoomForm f = (SoundRoomForm)InputFormRef.JumpForm<SoundRoomForm>(U.NOT_FOUND, "AddressList", src_object);
                    f.JumpToSongID(value);
                }
            }
            else if (linktype == "PORTRAIT")
            {
                if (Program.ROM.RomInfo.version() == 6)
                {
                    InputFormRef.JumpForm<ImagePortraitFE6Form>(value, "AddressList", src_object);
                }
                else
                {
                    InputFormRef.JumpForm<ImagePortraitForm>(value, "AddressList", src_object);
                }
            }
            else if (linktype == "PALETTE")
            {
                InputFormRef.JumpForm<ImageUnitPaletteForm>(value, "AddressList", src_object);
            }
            else if (linktype == "GENERICENEMYPORTRAIT")
            {
                InputFormRef.JumpForm<ImageGenericEnemyPortraitForm>(value, "AddressList", src_object);
            }
            else if (linktype == "TEXT")
            {
                if (U.isSafetyPointer((uint)src_object.Value))
                {
                    CStringForm f = (CStringForm)InputFormRef.JumpForm<CStringForm>();
                    f.Init(src_object);
                }
                else
                {
                    TextForm f = (TextForm)InputFormRef.JumpForm<TextForm>(value, "AddressList", src_object);
                    f.JumpTo(value);
                }
            }
            else if (linktype == "ITEM")
            {
                InputFormRef.JumpForm<ItemForm>(value, "AddressList", src_object);
            }
            else if (linktype == "ITEMUSAGEPOINTER")
            {
                ItemUsagePointerForm f = (ItemUsagePointerForm)InputFormRef.JumpForm<ItemUsagePointerForm>(U.NOT_FOUND);
                f.JumpTo(value);
            }
            else if (linktype == "EVENT")
            {
                value = CheckAndAlloc(src_object, value, linktype);
                if (value == U.NOT_FOUND)
                {
                    return;
                }
                EventScriptForm f = (EventScriptForm)InputFormRef.JumpForm<EventScriptForm>(U.NOT_FOUND);
                f.JumpTo(value, U.NOT_FOUND, MakeAddressListExpandsCallback_Handler(src_object) );
            }
            else if (linktype == "EVENTORCHEST")
            {
                bool isRandomChest = CheckRandomChest(src_object);
                value = CheckAndAlloc(src_object, value, linktype);
                if (value == U.NOT_FOUND)
                {
                    return;
                }

                if (isRandomChest)
                {
                    ItemRandomChestForm f = (ItemRandomChestForm)InputFormRef.JumpForm<ItemRandomChestForm>(U.NOT_FOUND);
                    f.JumpTo(value, MakeAddressListExpandsCallback_Handler(src_object));
                }
                else
                {
                    EventScriptForm f = (EventScriptForm)InputFormRef.JumpForm<EventScriptForm>(U.NOT_FOUND);
                    f.JumpTo(value, U.NOT_FOUND, MakeAddressListExpandsCallback_Handler(src_object));
                }
            }
            else if (linktype == "RAMPROCS")
            {
                if (value <= 8)
                {
                    return;
                }
                if (!U.is_02RAMPointer(value))
                {
                    return;
                }

                uint romProcs = Program.RAM.u32(value);
                ProcsScriptForm f = (ProcsScriptForm)InputFormRef.JumpForm<ProcsScriptForm>(U.NOT_FOUND);
                f.JumpTo(romProcs);
            }
            else if (linktype == "PROCS")
            {
                ProcsScriptForm f = (ProcsScriptForm)InputFormRef.JumpForm<ProcsScriptForm>(U.NOT_FOUND);
                f.JumpTo(value);
            }
            else if (linktype == "ASM")
            {
                DisASMForm f = (DisASMForm)InputFormRef.JumpForm<DisASMForm>(U.NOT_FOUND);
                if (arg1 == "THUMB")
                {
                    f.JumpTo(value);
                }
                else
                {
                    f.JumpTo(DisassemblerTrumb.ProgramAddrToPlain(value));

                }
            }
            else if (linktype == "ITEMSHOP")
            {
                value = CheckAndAlloc(src_object, value, linktype);
                if (value == U.NOT_FOUND)
                {
                    return;
                }

                ItemShopForm f = (ItemShopForm)InputFormRef.JumpForm<ItemShopForm>(U.NOT_FOUND, "SHOP_LIST", src_object);
                f.JumpTo(value);
            }
            else if (linktype == "SUPPORTUNIT")
            {
                if (Program.ROM.RomInfo.version() == 6)
                {
                    SupportUnitFE6Form f = (SupportUnitFE6Form)InputFormRef.JumpForm<SupportUnitFE6Form>();
                    f.JumpToAddr(value);
                }
                else
                {
                    SupportUnitForm f = (SupportUnitForm)InputFormRef.JumpForm<SupportUnitForm>();
                    f.JumpToAddr(value);
                }
            }
            else if (linktype == "ITEMSTATBOOSTER")
            {//能力補正
                value = CheckAndAlloc(src_object, value, linktype);
                if (value == U.NOT_FOUND)
                {
                    return;
                }
                if (CheckVennoExtendsStatBooster(src_object))
                {
                    InputFormRef.JumpForm<ItemStatBonusesVennoForm>(value, "AddressList", src_object);
                }
                else
                {
                    InputFormRef.JumpForm<ItemStatBonusesForm>(value, "AddressList", src_object);
                }
            }
            else if (linktype == "EFFECTIVENESS")
            {//特効
                value = CheckAndAlloc(src_object, value, linktype);
                if (value == U.NOT_FOUND)
                {
                    return;
                }

                if (PatchUtil.SearchClassType() == PatchUtil.class_type_enum.SkillSystems_Rework)
                {//SkillSystemsによる 特効リワーク
                    InputFormRef.JumpForm<ItemEffectivenessSkillSystemsReworkForm>(value, "AddressList", src_object);
                }
                else
                {
                    InputFormRef.JumpForm<ItemEffectivenessForm>(value, "AddressList", src_object);
                }
            }
            else if (linktype == "ITEMEFFECT")
            {//間接エフェクト
                ItemWeaponEffectForm f = (ItemWeaponEffectForm)InputFormRef.JumpForm<ItemWeaponEffectForm>(U.NOT_FOUND, "AddressList", src_object);
                f.JumpTo(value);
            }
            else if (linktype == "EVENTUNIT")
            {//ユニット配置
                if (Program.ROM.RomInfo.version() >= 8)
                {
                    EventUnitForm f = (EventUnitForm)InputFormRef.JumpForm<EventUnitForm>(U.NOT_FOUND);
                    f.JumpTo(value);
                }
                else if (Program.ROM.RomInfo.version() >= 7)
                {//FE7
                    EventUnitFE7Form f = (EventUnitFE7Form)InputFormRef.JumpForm<EventUnitFE7Form>(U.NOT_FOUND);
                    f.JumpTo(value);
                }
                else
                {//FE6
                    EventUnitFE6Form f = (EventUnitFE6Form)InputFormRef.JumpForm<EventUnitFE6Form>(U.NOT_FOUND);
                    f.JumpTo(value);
                }
            }
            else if (linktype == "MAPEXITPOINT")
            {//離脱ポイント
                InputFormRef.JumpForm<MapExitPointForm>(value);
            }
            else if (linktype == "IMAGECHAPTER")
            {//章タイトル
                if (Program.ROM.RomInfo.version() == 7 && Program.ROM.RomInfo.is_multibyte())
                {
                    InputFormRef.JumpForm<ImageChapterTitleFE7Form>(value, "AddressList", src_object);
                }
                else
                {
                    InputFormRef.JumpForm<ImageChapterTitleForm>(value);
                }
            }
            else if (linktype == "UNITPALETTE")
            {//ユニットパレット
                if (arg1 == "PLUS1")
                {
                    value = value + 1;
                }
                InputFormRef.JumpForm<ImageUnitPaletteForm>(value - 1, "AddressList", src_object);
            }
            else if (linktype == "UNITCUSTOMBATTLEANIME")
            {//ユニット専用アニメ
                InputFormRef.JumpForm<UnitCustomBattleAnimeForm>(value, "N2_AddressList", src_object);
            }
            else if (linktype == "WORLDMAPEVENT")
            {//ワールドマップイベント
                if (Program.ROM.RomInfo.version() >= 8)
                {
                    WorldMapEventPointerForm f = (WorldMapEventPointerForm)InputFormRef.JumpForm<WorldMapEventPointerForm>(U.NOT_FOUND);
                    f.JumpTo(value);
                }
                else if (Program.ROM.RomInfo.version() >= 7)
                {//FE7
                    WorldMapEventPointerFE7Form f = (WorldMapEventPointerFE7Form)InputFormRef.JumpForm<WorldMapEventPointerFE7Form>(U.NOT_FOUND);
                    f.JumpTo(value);
                }
                else
                {//FE6
                    WorldMapEventPointerFE6Form f = (WorldMapEventPointerFE6Form)InputFormRef.JumpForm<WorldMapEventPointerFE6Form>(U.NOT_FOUND);
                    f.JumpTo(value);
                }
            }
            else if (linktype == "EVENTCOND")
            {//イベント条件
                EventCondForm f = (EventCondForm)InputFormRef.JumpForm<EventCondForm>(U.NOT_FOUND);
                if (arg1 == "MAPID")
                {
                    f.JumpToMAPID(value);
                }
            }
            else if (linktype == "MAPCHANGE")
            {//マップ変化
                MapChangeForm f = (MapChangeForm)InputFormRef.JumpForm<MapChangeForm>(U.NOT_FOUND);
                if (arg1 == "MAPID")
                {
                    f.JumpToMAPID(value);
                }
            }
            else if (linktype == "BATTLEANIME")
            {//バトルアニメ
                ImageBattleAnimeForm f = (ImageBattleAnimeForm)InputFormRef.JumpForm<ImageBattleAnimeForm>(U.NOT_FOUND);
                if (arg1 == "CLASSID")
                {
                    f.JumpToClassID(value);
                }
                else if (arg1 == "ANIMEID")
                {
                    f.JumpToAnimeID(value);
                }
                else if (U.isPointer(value))
                {
                    f.JumpToAnimeSettingPointer(value);
                }
                else if (arg1 == "PLUS1")
                {
                    f.JumpToAnimeID(value + 1);
                }
                else if (arg1 == "MINUS1")
                {
                    f.JumpToAnimeID(value - 1);
                }
            }
            else if (linktype == "BATTLEANIMEPOINTER")
            {//バトルアニメ
                value = CheckAndAlloc(src_object, value, linktype);
                if (value == U.NOT_FOUND)
                {
                    return;
                }

                ImageBattleAnimeForm f = (ImageBattleAnimeForm)InputFormRef.JumpForm<ImageBattleAnimeForm>(U.NOT_FOUND);
                f.JumpToAnimeSettingPointer(value);
            }
            else if (linktype == "VENNOUWEAPONLOCK")
            {//専用武器vennou
                value = CheckAndAlloc(src_object, value, linktype);
                if (value == U.NOT_FOUND)
                {
                    return;
                }

                VennouWeaponLockForm f = (VennouWeaponLockForm)InputFormRef.JumpForm<VennouWeaponLockForm>(U.NOT_FOUND);
                f.JumpTo(value);
            }
            else if (linktype == "SMEPROMOLIST")
            {//smeのCC分岐
                value = CheckAndAlloc(src_object, value, linktype);
                if (value == U.NOT_FOUND)
                {
                    return;
                }

                SMEPromoListForm f = (SMEPromoListForm)InputFormRef.JumpForm<SMEPromoListForm>(U.NOT_FOUND);
                f.JumpTo(value);
            }
            else if (linktype == "MOVECOST1")
            {//移動コスト
                if (Program.ROM.RomInfo.version() >= 7)
                {//FE7 FE8
                    MoveCostForm f = (MoveCostForm)InputFormRef.JumpForm<MoveCostForm>(U.NOT_FOUND);
                    if (arg1 == "CLASSID")
                    {
                        f.JumpToClassID(value, 1);
                    }
                }
                else
                {//FE6
                    MoveCostFE6Form f = (MoveCostFE6Form)InputFormRef.JumpForm<MoveCostFE6Form>(U.NOT_FOUND);
                    if (arg1 == "CLASSID")
                    {
                        f.JumpToClassID(value, 1);
                    }
                }
            }
            else if (linktype == "MOVECOST2")
            {//移動コスト　雨
                if (Program.ROM.RomInfo.version() >= 7)
                {//FE7 FE8
                    MoveCostForm f = (MoveCostForm)InputFormRef.JumpForm<MoveCostForm>(U.NOT_FOUND);
                    if (arg1 == "CLASSID")
                    {
                        f.JumpToClassID(value, 2);
                    }
                }
                else
                {//FE6にはないです.
                    return;
                }
            }
            else if (linktype == "MOVECOST3")
            {//移動コスト 雪
                if (Program.ROM.RomInfo.version() >= 7)
                {//FE7 FE8
                    MoveCostForm f = (MoveCostForm)InputFormRef.JumpForm<MoveCostForm>(U.NOT_FOUND);
                    if (arg1 == "CLASSID")
                    {
                        f.JumpToClassID(value, 3);
                    }
                }
                else
                {//FE6にはないです
                }
            }
            else if (linktype == "MOVECOST4")
            {//地形回避
                if (Program.ROM.RomInfo.version() >= 7)
                {//FE7 FE8
                    MoveCostForm f = (MoveCostForm)InputFormRef.JumpForm<MoveCostForm>(U.NOT_FOUND);
                    if (arg1 == "CLASSID")
                    {
                        f.JumpToClassID(value, 4);
                    }
                }
                else
                {//FE6
                    MoveCostFE6Form f = (MoveCostFE6Form)InputFormRef.JumpForm<MoveCostFE6Form>(U.NOT_FOUND);
                    if (arg1 == "CLASSID")
                    {
                        f.JumpToClassID(value, 4);
                    }
                }
            }
            else if (linktype == "MOVECOST5")
            {//地形防御
                if (Program.ROM.RomInfo.version() >= 7)
                {//FE7 FE8
                    MoveCostForm f = (MoveCostForm)InputFormRef.JumpForm<MoveCostForm>(U.NOT_FOUND);
                    if (arg1 == "CLASSID")
                    {
                        f.JumpToClassID(value, 5);
                    }
                }
                else
                {//FE6
                    MoveCostFE6Form f = (MoveCostFE6Form)InputFormRef.JumpForm<MoveCostFE6Form>(U.NOT_FOUND);
                    if (arg1 == "CLASSID")
                    {
                        f.JumpToClassID(value, 5);
                    }
                }
            }
            else if (linktype == "MOVECOST6")
            {//地形魔防
                if (Program.ROM.RomInfo.version() >= 7)
                {//FE7 FE8
                    MoveCostForm f = (MoveCostForm)InputFormRef.JumpForm<MoveCostForm>(U.NOT_FOUND);
                    if (arg1 == "CLASSID")
                    {
                        f.JumpToClassID(value, 6);
                    }
                }
                else
                {//FE6
                    MoveCostFE6Form f = (MoveCostFE6Form)InputFormRef.JumpForm<MoveCostFE6Form>(U.NOT_FOUND);
                    if (arg1 == "CLASSID")
                    {
                        f.JumpToClassID(value, 6);
                    }
                }
            }
            else if (linktype == "OPCLASSALPHANAME")
            {//OPクラス英語表記
                if (Program.ROM.RomInfo.is_multibyte())
                {//まちるばいと
                    OPClassAlphaNameForm f = (OPClassAlphaNameForm)InputFormRef.JumpForm<OPClassAlphaNameForm>(U.NOT_FOUND);
                    if (arg1 == "ADDR")
                    {
                        f.JumpToADDR(value);
                    }
                }
                else
                {//英語版の場合 文字ID
                    if (arg1 == "CLASSID")
                    {
                        value = ClassForm.GetClassNameID(value);
                    }
                    InputFormRef.JumpForm<TextForm>(value);
                }
            }
            else if (linktype == "TERRAINBATTLE")
            {//バトル地形
                if (arg1 == "PLUS1")
                {
                    value = value + 1;
                }
                InputFormRef.JumpForm<ImageBattleTerrainForm>(value);
            }
            else if (linktype == "BATTLEBG")
            {//戦闘背景
                InputFormRef.JumpForm<ImageBattleBGForm>(value - 1);
            }
            else if (linktype == "MAPEDITOR")
            {//マップエディタ
                MapEditorForm f = (MapEditorForm)InputFormRef.JumpForm<MapEditorForm>(U.NOT_FOUND);
                f.JumpTo(value);
            }
            else if (linktype == "PATHEDITOR")
            {//道エディタ
                WorldMapPathEditorForm f = (WorldMapPathEditorForm)InputFormRef.JumpForm<WorldMapPathEditorForm>(U.NOT_FOUND);
                f.JumpTo(value);
            }
            else if (linktype == "PATHMOVEEDITOR")
            {//道移動エディタ
                WorldMapPathMoveEditorForm f = (WorldMapPathMoveEditorForm)InputFormRef.JumpForm<WorldMapPathMoveEditorForm>(U.NOT_FOUND);
                f.JumpTo(value);
            }
            else if (linktype == "FONT")
            {//フォント
                FontForm f = (FontForm)InputFormRef.JumpForm<FontForm>(U.NOT_FOUND);
                if (arg1 == "ITEM")
                {
                    f.JumpToItem(value);
                }
                else if (arg1 == "SERIF")
                {
                    f.JumpToSerif(value);
                }
            }
            else if (linktype == "WMICON")
            {//ワールドマップ画像
                WorldMapImageForm f = (WorldMapImageForm)InputFormRef.JumpForm<WorldMapImageForm>(U.NOT_FOUND);
                f.JumpToWMIcon();
            }
            else if (linktype == "UNITPALETTEFE8")
            {
                UnitPaletteForm f = (UnitPaletteForm)InputFormRef.JumpForm<UnitPaletteForm>(U.NOT_FOUND);
                if (arg1 == "UID")
                {
                    f.JumpTo(Math.Max(value - 1, 0));
                }
                else
                {
                    f.JumpTo(value);
                }
            }
            else if (linktype == "ATTRIBUTE")
            {
                InputFormRef.JumpForm<SupportAttributeForm>(value, "AddressList", src_object);
            }
            else if (linktype == "SONGTRACK")
            {
                InputFormRef.JumpForm<SongTrackForm>(value);
            }
            else if (linktype == "INSTRUMENT")
            {
                SongInstrumentForm f = (SongInstrumentForm)InputFormRef.JumpForm<SongInstrumentForm>(U.NOT_FOUND);
                if (arg1 == "ADDR")
                {
                    f.JumpToAddr(value);
                }
                else if (arg1 == "SONGID")
                {
                    f.JumpToSongID(value);
                }
            }
            else if (linktype == "SKILLASSIGNMENT")
            {
                if (Program.ROM.RomInfo.version() == 8 && Program.ROM.RomInfo.is_multibyte())
                {
                    PatchUtil.skill_system_enum skill = PatchUtil.SearchSkillSystem();
                    if (skill == PatchUtil.skill_system_enum.FE8N_ver2)
                    {
                        SkillConfigFE8NVer2SkillForm f = (SkillConfigFE8NVer2SkillForm)InputFormRef.JumpForm<SkillConfigFE8NVer2SkillForm>(value, "AddressList", src_object);
                    }
                    else
                    {
                        SkillAssignmentUnitFE8NForm f = (SkillAssignmentUnitFE8NForm)InputFormRef.JumpForm<SkillAssignmentUnitFE8NForm>(U.NOT_FOUND);
                        if (arg1 == "SCROLL1")
                        {
                            InputFormRef.MakeInjectionApplyButtonCallback(f, f.GetApplyButton(), src_object);
                            f.JumpToSCROLL1(value);
                        }
                        else if (arg1 == "SCROLL2")
                        {
                            InputFormRef.MakeInjectionApplyButtonCallback(f, f.GetApplyButton(), src_object);
                            f.JumpToSCROLL2(value);
                        }
                        else if (arg1 == "MASTERY")
                        {
                            InputFormRef.MakeInjectionApplyButtonCallback(f, f.GetApplyButton(), src_object);
                            f.JumpToMASTERY(value);
                        }
                    }
                }
                else if (Program.ROM.RomInfo.version() == 8)
                {
                    PatchUtil.skill_system_enum skill = PatchUtil.SearchSkillSystem();
                    if (skill == PatchUtil.skill_system_enum.SkillSystem)
                    {
                        InputFormRef.JumpForm<SkillConfigSkillSystemForm>(value, "AddressList", src_object);
                    }
                }
            }
            else if (linktype == "CLASSTYPE")
            {//SkillSystemsの ClassType Rework
                if (Program.ROM.RomInfo.version() == 8 && Program.ROM.RomInfo.is_multibyte() == false)
                {
                    PatchUtil.skill_system_enum skill = PatchUtil.SearchSkillSystem();
                    if (skill == PatchUtil.skill_system_enum.SkillSystem)
                    {
                        SkillSystemsEffectivenessReworkClassTypeForm f = (SkillSystemsEffectivenessReworkClassTypeForm)InputFormRef.JumpForm<SkillSystemsEffectivenessReworkClassTypeForm>(U.NOT_FOUND);
                        InputFormRef.MakeInjectionApplyButtonCallback(f, f.GetApplyButton(), src_object);
                        f.JumpToClassType(value);
                    }
                }
            }
            else if (linktype == "MENU")
            {
                MenuCommandForm f = (MenuCommandForm)InputFormRef.JumpForm<MenuCommandForm>(U.NOT_FOUND);
                f.JumpToAddr(value);
            }
            else if (linktype == "MENUDEFINITION")
            {
                MenuDefinitionForm f = (MenuDefinitionForm)InputFormRef.JumpForm<MenuDefinitionForm>(U.NOT_FOUND);
                f.JumpToAddr(value);
            }
            else if (linktype == "HEX")
            {
                HexEditorForm hexeditor = (HexEditorForm)InputFormRef.JumpForm<HexEditorForm>(U.NOT_FOUND);
                hexeditor.JumpTo((uint)src_object.Value);
            }
            else if (linktype == "EVENTBATTLEDATA")
            {
                EventBattleDataFE7Form f = (EventBattleDataFE7Form)InputFormRef.JumpForm<EventBattleDataFE7Form>(U.NOT_FOUND);
                f.JumpToAddr((uint)src_object.Value);
            }
            else if (linktype == "PLIST")
            {
                MapPointerForm.PLIST_TYPE type = MapPointerForm.ConvertStringToType(arg1);
                if (type == MapPointerForm.PLIST_TYPE.UNKNOWN)
                {
                    Debug.Assert(false);
                    return;
                }
                value = PListCheckAndAlloc(src_object, value, type);
                if (value == U.NOT_FOUND)
                {
                    return;
                }
                PListJumptTo(src_object, type);
            }
            else if (linktype == "FLAG")
            {
                ToolFlagNameForm f = (ToolFlagNameForm)InputFormRef.JumpForm<ToolFlagNameForm>(U.NOT_FOUND);
                f.JumpTo(value, MakeAddressListFlagExpandsCallback_Handler(src_object) , U.NOT_FOUND);
            }
            else if (linktype == "TALKGROUP")
            {
                InputFormRef.JumpForm<ToolUnitTalkGroupForm>(value, "AddressList", src_object);
            }
            else if (linktype == "MAPTERRAINBGLOOKUPTABLE")
            {
                MapTerrainBGLookupTableForm f = (MapTerrainBGLookupTableForm)InputFormRef.JumpForm<MapTerrainBGLookupTableForm>();
                f.JumpTo(value,0);
            }
            else if (linktype == "RAMUNIT_STATE")
            {
                UwordBitFlagForm f = (UwordBitFlagForm)InputFormRef.JumpForm<UwordBitFlagForm>(U.NOT_FOUND);
                f.JumpTo(EventScript.ArgType.RAM_UNIT_STATE, value);
            }
            else if (linktype == "TEXTOREVENT")
            {
                if (U.isSafetyOffset(value))
                {
                    EventScriptForm f = (EventScriptForm)InputFormRef.JumpForm<EventScriptForm>(U.NOT_FOUND);
                    f.JumpTo(value, U.NOT_FOUND, MakeAddressListExpandsCallback_Handler(src_object));
                }
                else
                {
                    TextForm f = (TextForm)InputFormRef.JumpForm<TextForm>(U.NOT_FOUND, "AddressList", src_object);
                    f.JumpTo(value);
                }
            }
            else if (linktype == "DIFFCULTY")
            {//ダイアログを表示する.
                MapSettingDifficultyForm f = (MapSettingDifficultyForm)InputFormRef.JumpFormLow<MapSettingDifficultyForm>();
                f.SetDifficultyValue((uint)src_object.Value);
                f.ShowDialog();

                src_object.Value = f.GetDifficultyValue();
            }
            else if (linktype == "GAMEOPTION")
            {
                InputFormRef.JumpForm<StatusOptionForm>(value, "AddressList", src_object);
            }
            else if (linktype == "DIRECTSOUND")
            {
                SongInstrumentDirectSoundForm f = (SongInstrumentDirectSoundForm)InputFormRef.JumpForm<SongInstrumentDirectSoundForm>(U.NOT_FOUND);
                f.JumpToAddr(value);
            }
            else if (linktype == "BG" || linktype == "BGICON")
            {
                ImageBGForm f = (ImageBGForm)InputFormRef.JumpForm<ImageBGForm>(value, "AddressList", src_object);
            }
            else if (linktype == "CG" || linktype == "CGICON")
            {
                if (Program.ROM.RomInfo.version() == 7 && Program.ROM.RomInfo.is_multibyte() == false)
                {//FE7U
                    ImageCGFE7UForm f = (ImageCGFE7UForm)InputFormRef.JumpForm<ImageCGFE7UForm>(value, "AddressList", src_object);
                }
                else
                {
                    ImageCGForm f = (ImageCGForm)InputFormRef.JumpForm<ImageCGForm>(value, "AddressList", src_object);
                }
            }
            else if (linktype == "BASEPOINT")
            {
                InputFormRef.JumpForm<WorldMapPointForm>(value, "AddressList", src_object);
            }
        }
        static void PListJumptTo(NumericUpDown value, MapPointerForm.PLIST_TYPE type)
        {
            //PLISTが0なので新規に割り当てないといけない
            string prefix = GetPrefixName(value.Name);
            uint id = GetStructID(prefix, value.Name);
            if (id == U.NOT_FOUND)
            {//IDが取れないので無理.
                return ;
            }
            Form f = U.ControlToParentForm(value);
            if (f == null)
            {//フォームが取れないので無理.
                return ;
            }
            List<Control> controls = InputFormRef.GetAllControls(f);

            //残念ながらPLISTは、NEWALLOCを出せるほどの余白がない
            string addresslist_name = prefix + "AddressList";
            Control c = FindObjectByForm<ListBox>(controls, addresslist_name);
            if (!(c is ListBoxEx))
            {
                return ;
            }
            ListBox addressList = ((ListBox)c);
            if (addressList.SelectedIndex < 0)
            {
                return ;
            }

            uint mapid = (uint)addressList.SelectedIndex;

            if (type == MapPointerForm.PLIST_TYPE.MAP)
            {
                MapEditorForm ff = (MapEditorForm)InputFormRef.JumpForm<MapEditorForm>(U.NOT_FOUND);
                ff.JumpTo(mapid);
            }
            else if (type == MapPointerForm.PLIST_TYPE.CHANGE)
            {
                MapChangeForm ff = (MapChangeForm)InputFormRef.JumpForm<MapChangeForm>(U.NOT_FOUND);
                ff.JumpToMAPID(mapid);
            }
            else if (type == MapPointerForm.PLIST_TYPE.EVENT)
            {
                EventCondForm ff = (EventCondForm)InputFormRef.JumpForm<EventCondForm>(U.NOT_FOUND);
                ff.JumpToMAPID(mapid);
            }
            else if (type == MapPointerForm.PLIST_TYPE.PALETTE)
            {
                MapStyleEditorForm ff = (MapStyleEditorForm)InputFormRef.JumpForm<MapStyleEditorForm>(U.NOT_FOUND);
                ff.JumpToMAPID(mapid);
            }
            else if (type == MapPointerForm.PLIST_TYPE.CONFIG)
            {
                MapStyleEditorForm ff = (MapStyleEditorForm)InputFormRef.JumpForm<MapStyleEditorForm>(U.NOT_FOUND);
                ff.JumpToMAPID(mapid);
            }
            else if (type == MapPointerForm.PLIST_TYPE.OBJECT)
            {
                MapStyleEditorForm ff = (MapStyleEditorForm)InputFormRef.JumpForm<MapStyleEditorForm>(U.NOT_FOUND);
                ff.JumpToMAPID(mapid);
            }
            else if (type == MapPointerForm.PLIST_TYPE.ANIMATION)
            {
                MapTileAnimation1Form ff = (MapTileAnimation1Form)InputFormRef.JumpForm<MapTileAnimation1Form>(U.NOT_FOUND);
                ff.JumpToPlist((uint)value.Value);
            }
            else if (type == MapPointerForm.PLIST_TYPE.ANIMATION2)
            {
                MapTileAnimation2Form ff = (MapTileAnimation2Form)InputFormRef.JumpForm<MapTileAnimation2Form>(U.NOT_FOUND);
                ff.JumpToPlist((uint)value.Value);
            }
        }
        static uint PListCheckAndAlloc(NumericUpDown value, uint plist, MapPointerForm.PLIST_TYPE type)
        {
            if (plist != 0)
            {
                uint plist_addr = MapPointerForm.PlistToOffsetAddr(type, plist);
                if (plist_addr != 0)
                {
                    return plist;
                }
            }

            DialogResult dr = R.ShowNoYes("PLISTが0です。\r\n新規に割り当てますか？\r\n");
            if (dr != DialogResult.Yes)
            {
                return U.NOT_FOUND;
            }


            //PLISTが0なので新規に割り当てないといけない
            string prefix = GetPrefixName(value.Name);
            uint id = GetStructID(prefix, value.Name);
            if (id == U.NOT_FOUND)
            {//IDが取れないので無理.
                return U.NOT_FOUND;
            }
            Form f = U.ControlToParentForm(value);
            if (f == null)
            {//フォームが取れないので無理.
                return U.NOT_FOUND;
            }
            List<Control> controls = InputFormRef.GetAllControls(f);

            //残念ながらPLISTは、NEWALLOCを出せるほどの余白がない
            string addresslist_name = prefix + "AddressList";
            Control c = FindObjectByForm<ListBox>(controls, addresslist_name);
            if (!(c is ListBoxEx))
            {
                return U.NOT_FOUND;
            }
            ListBox addressList = ((ListBox)c);
            if (addressList.SelectedIndex < 0)
            {
                return U.NOT_FOUND;
            }

            string writebutton_name = prefix + "WriteButton";
            c = FindObjectByForm<Button>(controls, writebutton_name);
            if (c == null)
            {//書き込みボタンの取得ができなかった...
                Debug.Assert(false);
                return U.NOT_FOUND;
            }
            Button writeButton = ((Button)c);

            uint mapid = (uint)addressList.SelectedIndex;

            if (type == MapPointerForm.PLIST_TYPE.MAP)
            {
                plist = ImageUtilMap.PreciseMapDataArea(mapid);
            }
            else if (type == MapPointerForm.PLIST_TYPE.CHANGE)
            {
                plist = MapChangeForm.PreciseChangeListForce(mapid);
            }
            else if (type == MapPointerForm.PLIST_TYPE.EVENT)
            {
                plist = EventCondForm.PreciseEevntCondArea(mapid);
            }
            else if (type == MapPointerForm.PLIST_TYPE.PALETTE)
            {
                plist = ImageUtilMap.PrecisePaletteArea(mapid);
            }
            else if (type == MapPointerForm.PLIST_TYPE.CONFIG)
            {
                plist = ImageUtilMap.PreciseConfigArea(mapid);
            }
            else if (type == MapPointerForm.PLIST_TYPE.OBJECT)
            {
                plist = ImageUtilMap.PreciseObjectArea(mapid);
            }
            else if (type == MapPointerForm.PLIST_TYPE.ANIMATION)
            {
                plist = MapTileAnimation1Form.PreciseMapTileAnimation1Area(mapid);
            }
            else if (type == MapPointerForm.PLIST_TYPE.ANIMATION2)
            {
                plist = MapTileAnimation2Form.PreciseMapTileAnimation2Area(mapid);
            }
            
            if (plist == 0)
            {
                return U.NOT_FOUND;
            }

            //PLISTの値を変える.
            value.Value = plist;
            //書きボタンを押す.
            writeButton.PerformClick();

            //念のため確認する.
            if (value.Value != plist)
            {//書き込みできてない.
                return U.NOT_FOUND;
            }

            return plist;
        }

        static bool CheckRandomChest(NumericUpDown value)
        {
            if (Program.ROM.RomInfo.version() != 8)
            {
                return false;
            }
            //FE8にはランダム宝箱があるので、どちらか見分ける必要がある

            //確保されていないので自動確保を試みる.
            string prefix = GetPrefixName(value.Name);
            uint id = GetStructID(prefix, value.Name);
            if (id == U.NOT_FOUND)
            {//IDが取れないので無理.
                return false;
            }

            Form f = U.ControlToParentForm(value);
            if (f == null)
            {//フォームが取れないので無理.
                return false;
            }
            List<Control> controls = InputFormRef.GetAllControls(f);

            string object_type_name = prefix + "W10";
            Control c = FindObjectByForm<NumericUpDown>(controls, object_type_name);
            if (!(c is NumericUpDown))
            {//判別不能
                return false;
            }
            if (((NumericUpDown)c).Value == 0x14)
            {//ランダム宝箱
                return true;
            }
            //違う
            return false;
        }
        static bool CheckVennoExtendsStatBooster(NumericUpDown value)
        {
            if (PatchUtil.SearchGrowsMod() != PatchUtil.growth_mod_enum.Vennou)
            {
                return false;
            }
            //Vennoが独自に拡張したstatbooster?

            //確保されていないので自動確保を試みる.
            string prefix = GetPrefixName(value.Name);
            uint id = GetStructID(prefix, value.Name);
            if (id == U.NOT_FOUND)
            {//IDが取れないので無理.
                return false;
            }

            Form f = U.ControlToParentForm(value);
            if (f == null)
            {//フォームが取れないので無理.
                return false;
            }
            List<Control> controls = InputFormRef.GetAllControls(f);

            string object_type_name = prefix + "B34";
            Control c = FindObjectByForm<NumericUpDown>(controls, object_type_name);
            if (!(c is NumericUpDown))
            {//判別不能
                return false;
            }
            if (((NumericUpDown)c).Value == 0x1)
            {//vennoの独自拡張
                return true;
            }
            //違う
            return false;
        }

        static uint CheckAndAlloc(NumericUpDown value,uint p , string linktype)
        {
            uint addr = U.toOffset(p);
            if (U.isSafetyOffset(addr))
            {//正しく確保されてそう.
                return addr;
            }
            //確保されていないので自動確保を試みる.
            string prefix = GetPrefixName(value.Name);
            uint id = GetStructID(prefix, value.Name);
            if (id == U.NOT_FOUND)
            {//IDが取れないので無理.
                return U.NOT_FOUND;
            }
            Form f = U.ControlToParentForm(value);
            if (f == null)
            {//フォームが取れないので無理.
                return U.NOT_FOUND;
            }
            List<Control> controls = InputFormRef.GetAllControls(f);
            string newalloc_button_name = prefix + "L_" + id.ToString() + "_NEWALLOC_" + linktype;
            Control c = FindObjectByForm<Button>(controls, newalloc_button_name);
            if (!(c is Button))
            {//確保ボタンがないので無理
                return U.NOT_FOUND;
            }
            if (c.Visible == false)
            {//表示されていないボタンは押せない
                return U.NOT_FOUND;
            }
            Button button = (Button)c;
            button.PerformClick();

            //汚い方法ではあるが、ここで値が修正されたかを確認する.
            addr = U.toOffset((uint)value.Value);
            if (U.isSafetyOffset(addr))
            {//正しく確保されてそう.
                return addr;
            }

            //ユーザが拒否した.
            return U.NOT_FOUND;
        }
        public static string GetPrefixName(string name)
        {
            string[] op = name.Split('_');
            for(int i = 0; i < op.Length ; i++)
            {
                if (op[i].Length <= 0)
                {
                    continue;
                }
                if (op[i] == "L" || op[i] == "J")
                {
                    if (i+1 >= op.Length)
                    {
                        continue;
                    }
                    if (! U.isNumString(op[i+1]))
                    {
                        continue;
                    }

                    string ret = string.Join("_", U.subrange(op, 0, (uint)i));
                    if (ret.Length > 0)
                    {
                        return ret + "_";
                    }
                    return "";
                }

                if (IsTypeWord(op[i][0]))
                {
                    string idString = op[i].Substring(1);
                    if (! U.isNumString(idString))
                    {
                        continue;
                    }
                    string ret = string.Join("_", U.subrange(op, 0, (uint)i));
                    if (ret.Length > 0)
                    {
                        return ret + "_";
                    }
                    return "";
                }
            }
            return "";
        }
        public static bool IsTypeWord(char c)
        {
            return c == '_'
                || c == 'B'
                || c == 'b'
                || c == 'W'
                || c == 'D'
                || c == 'P'
                || c == 'l'
                || c == 'h'
                ;
        }
        public static uint GetTypeLength(char c)
        {
            switch(c)
            {
                case 'B': return 1;
                case 'b': return 1;
                case 'W': return 2;
                case 'D': return 4;
                case 'P': return 4;
                case 'l': return 1;
                case 'h': return 1;
            }
            return 0;
        }
        public static uint GetStructID(string prefix, string name)
        {
            string nname = SkipPrefixName(name,prefix);
            if (nname.Length <= 0)
            {//ない.
                return U.NOT_FOUND;
            }
            if (nname[0] == '_'|| IsTypeWord(nname[0]) )
            {
                nname = nname.Substring(1);
                if (nname.Length <= 0)
                {//ない.
                    return U.NOT_FOUND;
                }
            }

            if (U.isnum(nname[0]))
            {//IDを発見
                return U.atoi(nname);
            }

            //ない
            return U.NOT_FOUND;
        }

        public static void makeJumpEventHandler(Control src_object, Label jump_object, String linktype, string[] args)
        {
            markupJumpLabel(jump_object);

            EventHandler jumpHandler = (sender, e) =>
            {
                JumpTo(src_object, jump_object, linktype, args);
            };
            jump_object.Click += jumpHandler;
        }

        public static String SkipPrefixName(String name, String prefix)
        {
            if (prefix.Length <= 0)
            {
                return name;
            }
            if (name.IndexOf(prefix) == 0)
            {
                return name.Substring(prefix.Length);
            }
            return "";
        }

        void RomToUI( uint addr, String prefix, List<Control> controls)
        {
            foreach (Control info in controls)
            {
                if (!(info is NumericUpDown))
                {
                    continue;
                }
                NumericUpDown info_object = ((NumericUpDown)info);

                String name = SkipPrefixName(info.Name, prefix);
                if (name.Length <= 0)
                {
                    continue;
                }
                if (name[0] == 'B' && name[1] >= '0' && name[1] <= '9')
                {
                    uint id = U.atoi(name.Substring(1));

                    if (addr + id >= Program.ROM.Data.Length)
                    {
                        if (U.is_RAMPointer(addr + id))
                        {
                            info_object.Value = Program.RAM.u8(addr + id);
                        }
                        else
                        {
                            R.Error("警告:範囲外へのアクセスです name:{0} id:{1} addr{2} form:{3}", name, id, U.ToHexString(addr + id), this.SelfForm.Text);
                            Debug.Assert(false);
                            U.ForceUpdate(info_object, 0);
                        }
                    }
                    else
                    {
                        U.ForceUpdate(info_object, Program.ROM.u8(addr + id));
                    }
                }
                else if (name[0] == 'b' && name[1] >= '0' && name[1] <= '9')
                {
                    uint id = U.atoi(name.Substring(1));
                    if (addr + id >= Program.ROM.Data.Length)
                    {
                        if (U.is_RAMPointer(addr + id))
                        {
                            U.ForceUpdate(info_object, (sbyte)Program.RAM.u8(addr + id));
                        }
                        else
                        {
                            R.Error("警告:範囲外へのアクセスです name:{0} id:{1} addr{2} form:{3}", name, id, U.ToHexString(addr + id), this.SelfForm.Text);
                            Debug.Assert(false);
                            U.ForceUpdate(info_object, 0);
                        }
                    }
                    else
                    {
                        U.ForceUpdate(info_object, (sbyte)Program.ROM.u8(addr + id));
                    }
                }
                else if (name[0] == 'W' && name[1] >= '0' && name[1] <= '9')
                {
                    uint id = U.atoi(name.Substring(1));
                    if (addr + id + 2 >= Program.ROM.Data.Length)
                    {
                        if (U.is_RAMPointer(addr + id))
                        {
                            U.ForceUpdate(info_object, Program.RAM.u16(addr + id));
                        }
                        else
                        {
                            R.Error("警告:範囲外へのアクセスです name:{0} id:{1} addr{2} form:{3}", name, id, U.ToHexString(addr + id), this.SelfForm.Text);
                            Debug.Assert(false);
                            U.ForceUpdate(info_object, 0);
                        }
                    }
                    else
                    {
                        U.ForceUpdate(info_object, Program.ROM.u16(addr + id));
                    }
                }
                else if (name[0] == 'D' && name[1] >= '0' && name[1] <= '9')
                {
                    uint id = U.atoi(name.Substring(1));
                    if (addr + id + 4 >= Program.ROM.Data.Length)
                    {
                        if (U.is_RAMPointer(addr + id))
                        {
                            U.ForceUpdate(info_object, Program.RAM.u32(addr + id));
                        }
                        else
                        {
                            R.Error("警告:範囲外へのアクセスです name:{0} id:{1} addr{2} form:{3}", name, id, U.ToHexString(addr + id), this.SelfForm.Text);
                            Debug.Assert(false);
                            U.ForceUpdate(info_object, 0);
                        }
                    }
                    else
                    {
                        U.ForceUpdate(info_object, Program.ROM.u32(addr + id));
                    }
                }
                else if (name[0] == 'P' && name[1] >= '0' && name[1] <= '9')
                {
                    uint id = U.atoi(name.Substring(1));
                    if (addr + id + 4 >= Program.ROM.Data.Length)
                    {
                        if (U.is_RAMPointer(addr + id))
                        {
                            U.ForceUpdate(info_object, Program.RAM.u32(addr + id));
                        }
                        else
                        {
                            R.Error("警告:範囲外へのアクセスです name:{0} id:{1} addr{2} form:{3}", name, id, U.ToHexString(addr + id), this.SelfForm.Text);
                            Debug.Assert(false);
                            U.ForceUpdate(info_object, 0);
                        }
                    }
                    else
                    {
                        U.ForceUpdate(info_object, Program.ROM.u32(addr + id));
                    }
                }
                else if (name[0] == 'l' && name[1] >= '0' && name[1] <= '9')
                {
                    uint id = U.atoi(name.Substring(1));
                    if (addr + id >= Program.ROM.Data.Length)
                    {
                        if (U.is_RAMPointer(addr + id))
                        {
                            U.ForceUpdate(info_object, (sbyte)Program.RAM.u4(addr + id , false));
                        }
                        else
                        {
                            R.Error("警告:範囲外へのアクセスです name:{0} id:{1} addr{2} form:{3}", name, id, U.ToHexString(addr + id), this.SelfForm.Text);
                            Debug.Assert(false);
                            U.ForceUpdate(info_object, 0);
                        }
                    }
                    else
                    {
                        U.ForceUpdate(info_object, Program.ROM.u4(addr + id, false));
                    }
                }
                else if (name[0] == 'h' && name[1] >= '0' && name[1] <= '9')
                {
                    uint id = U.atoi(name.Substring(1));
                    if (addr + id >= Program.ROM.Data.Length)
                    {
                        if (U.is_RAMPointer(addr + id))
                        {
                            U.ForceUpdate(info_object, (sbyte)Program.RAM.u4(addr + id, true));
                        }
                        else
                        {
                            R.Error("警告:範囲外へのアクセスです name:{0} id:{1} addr{2} form:{3}", name, id, U.ToHexString(addr + id), this.SelfForm.Text);
                            Debug.Assert(false);
                            U.ForceUpdate(info_object, 0);
                        }
                    }
                    else
                    {
                        U.ForceUpdate(info_object, Program.ROM.u4(addr + id, true));
                    }
                }
            }

            //コメントがあれば設定する
            UI_ReadUIToComment(addr, prefix, controls);
        }

        public bool IsSurrogateStructure;

        public void UIToRom(uint addr, String prefix,List<Control> controls)
        {
            if (IsSurrogateStructure)
            {//代理構造体で表現されているのでサイズ不明
                if (addr + 1 > Program.ROM.Data.Length)
                {
                    R.ShowStopError("警告:範囲外への書き込みです prefix:{0} addr:{1}+{2} length:{3} form:{4}", prefix, U.ToHexString(addr), U.ToHexString(this.BlockSize), U.ToHexString(Program.ROM.Data.Length), this.SelfForm.Text);
                    Debug.Assert(false);
                    return;
                }
            }
            else
            {
                if (addr + this.BlockSize > Program.ROM.Data.Length)
                {
                    R.ShowStopError("警告:範囲外への書き込みです prefix:{0} addr:{1}+{2} length:{3} form:{4}", prefix, U.ToHexString(addr), U.ToHexString(this.BlockSize), U.ToHexString(Program.ROM.Data.Length), this.SelfForm.Text);
                    Debug.Assert(false);
                    return;
                }
            }

            foreach (Control info in controls)
            {
                if (!(info is NumericUpDown))
                {
                    continue;
                }
                NumericUpDown info_object = ((NumericUpDown)info);

                String name = SkipPrefixName(info.Name, prefix);
                if (name.Length <= 0)
                {
                    continue;
                }
                if (name[0] == 'B' && name[1] >= '0' && name[1] <= '9')
                {
                    uint id = U.atoi(name.Substring(1));
                    uint value = (uint)info_object.Value;
                    Program.ROM.write_u8(addr + id, value);
                }
                else if (name[0] == 'b' && name[1] >= '0' && name[1] <= '9')
                {
                    uint id = U.atoi(name.Substring(1));
                    uint value = (uint)((int)info_object.Value);
                    Program.ROM.write_u8(addr + id, value);
                }
                else if (name[0] == 'W' && name[1] >= '0' && name[1] <= '9')
                {
                    uint id = U.atoi(name.Substring(1));
                    uint value = (uint)info_object.Value;
                    Program.ROM.write_u16(addr + id, value);
                }
                else if (name[0] == 'D' && name[1] >= '0' && name[1] <= '9')
                {
                    uint id = U.atoi(name.Substring(1));
                    uint value = (uint)info_object.Value;
                    Program.ROM.write_u32(addr + id, value);
                }
                else if (name[0] == 'P' && name[1] >= '0' && name[1] <= '9')
                {
                    uint id = U.atoi(name.Substring(1));
                    uint value = U.toPointer((uint)info_object.Value); //ポインタへ変換
                    Program.ROM.write_u32(addr + id, value);
                }
                else if (name[0] == 'l' && name[1] >= '0' && name[1] <= '9')
                {
                    uint id = U.atoi(name.Substring(1));
                    uint value = (uint)((int)info_object.Value);
                    Program.ROM.write_u4(addr + id, value , false);
                }
                else if (name[0] == 'h' && name[1] >= '0' && name[1] <= '9')
                {
                    uint id = U.atoi(name.Substring(1));
                    uint value = (uint)((int)info_object.Value);
                    Program.ROM.write_u4(addr + id, value, true);
                }
            }
            UI_WriteCommentToUI(addr, prefix, controls);
        }
        void UI_WriteCommentToUI(uint addr, String prefix, List<Control> controls)
        {
            foreach (Control info in controls)
            {
                if (!(info is TextBoxEx))
                {
                    continue;
                }
                TextBoxEx info_object = ((TextBoxEx)info);

                String name = SkipPrefixName(info.Name, prefix);
                if (name.Length <= 0)
                {
                    continue;
                }
                if (name == "Comment")
                {
                    Program.CommentCache.Update(addr, info_object.Text);
                    return;
                }
            }
        }
        void UI_ReadUIToComment(uint addr, String prefix, List<Control> controls)
        {
            foreach (Control info in controls)
            {
                if (!(info is TextBoxEx))
                {
                    continue;
                }
                TextBoxEx info_object = ((TextBoxEx)info);

                String name = SkipPrefixName(info.Name, prefix);
                if (name.Length <= 0)
                {
                    continue;
                }
                if (name == "Comment")
                {
                    info_object.Text = Program.CommentCache.At(addr);
                    return;
                }
            }
        }

        public uint BasePointer { get; private set; }
        public uint BaseAddress { get; private set; }
        public uint BlockSize { get; private set; }
        public uint DataCount { get; private set; }

        //更新中のフラグ
        public bool IsUpdateLock { get; private set; }

        //プレフィックス
        public string Prefix { get; private set; }

        //データが存在するか?
        public Func<int ,uint, bool> IsDataExistsCallback { get; private set; }
        //一覧リストの項目を作る.
        public Func<int, uint, U.AddrResult> LoopCallback { get; private set; }
        //次のデータアドレスへ
        public Func<uint, uint> NextAddrCallback { get; private set; }


        public NumericUpDown ReadStartAddress  { get; private set; }
        public NumericUpDown ReadCount  { get; private set; }
        public NumericUpDown Address { get; private set; }
        public Button ReloadListButton  { get; private set; }
        public ListBoxEx AddressList  { get; private set; }
        public Form SelfForm { get; private set; }
        Button WriteButton;
        Button AddressListExpandsButton;
        public TabControl UnionTab { get; private set; }
        TextBoxEx BlockSizeTextBox;
        TextBoxEx SelectAddressTextBox;
        public List<String> UnionPrefixList { get; private set; }
        public List<Control> Controls { get; private set; }

        public EventHandler PreAddressListExpandsEvent; //リストを拡張しようとした時のイベント
        public EventHandler AddressListExpandsEvent; //リストを拡張した時のイベント

        //拡張されたときのイベント引数
        public class ExpandsEventArgs : EventArgs
        {
            public uint BlockSize;

            public uint OldBaseAddress;
            public uint OldDataCount;

            public uint NewBaseAddress;
            public uint NewDataCount;

            public bool IsCancel;
            public bool IsReload;

        };
        
        public uint GetUIAddress()
        {
            if (Address == null)
            {
                return U.NOT_FOUND;
            }
            return (uint)Address.Value;
        }

        public static uint SelectToAddr(ListBox list)
        {
            return SelectToAddr(list, list.SelectedIndex);
        }
        public static uint SelectToAddr(ComboBox list)
        {
            return SelectToAddr(list, list.SelectedIndex);
        }
        public static uint SelectToAddr(ListBox list, int SelectedIndex)
        {
            if (!(list.Tag is List<U.AddrResult>))
            {
                return U.NOT_FOUND;
            }
            List<U.AddrResult> addresslist = (List<U.AddrResult>)list.Tag;
            if (SelectedIndex < 0)
            {
                return U.NOT_FOUND;
            }
            if (SelectedIndex >= addresslist.Count)
            {
                return U.NOT_FOUND;
            }
            return addresslist[SelectedIndex].addr;
        }
        public static U.AddrResult SelectToAddrResult(ListBox list)
        {
            return SelectToAddrResult(list, list.SelectedIndex);
        }
        public static U.AddrResult SelectToAddrResult(ListBox list, int SelectedIndex)
        {
            if (!(list.Tag is List<U.AddrResult>))
            {
                return new U.AddrResult();
            }
            List<U.AddrResult> addresslist = (List<U.AddrResult>)list.Tag;
            if (SelectedIndex < 0)
            {
                return new U.AddrResult();
            }
            if (SelectedIndex >= addresslist.Count)
            {
                return new U.AddrResult();
            }
            return addresslist[SelectedIndex];
        }
        public static uint SelectToAddr(ComboBox list, int SelectedIndex)
        {
            if (!(list.Tag is List<U.AddrResult>))
            {
                return U.NOT_FOUND;
            }
            List<U.AddrResult> addresslist = (List<U.AddrResult>)list.Tag;
            if (SelectedIndex < 0)
            {
                return U.NOT_FOUND;
            }
            if (SelectedIndex >= addresslist.Count)
            {
                return U.NOT_FOUND;
            }
            return addresslist[SelectedIndex].addr;
        }
        public static U.AddrResult SelectToAddrResult(ComboBox list, int SelectedIndex)
        {
            if (!(list.Tag is List<U.AddrResult>))
            {
                return new U.AddrResult();
            }
            List<U.AddrResult> addresslist = (List<U.AddrResult>)list.Tag;
            if (SelectedIndex < 0)
            {
                return new U.AddrResult();
            }
            if (SelectedIndex >= addresslist.Count)
            {
                return new U.AddrResult();
            }
            return addresslist[SelectedIndex];
        }
        public static uint AddrToSelect(ListBox list, uint addr)
        {
            if (!(list.Tag is List<U.AddrResult>))
            {
                return U.NOT_FOUND;
            }

            addr = U.toOffset(addr);
            List<U.AddrResult> addresslist = (List<U.AddrResult>)list.Tag;
            for (int i = 0; i < addresslist.Count; i++)
            {
                if (addresslist[i].addr == addr)
                {
                    return (uint)i;
                }
            }
            return U.NOT_FOUND;
        }
        public uint IDToAddr(uint id)
        {
            if (id >= this.DataCount)
            {
                return U.NOT_FOUND;
            }
            return (this.BaseAddress) + (this.BlockSize * id);
        }
        public uint AddrToID(uint addr)
        {
            addr = U.toOffset(addr);
            if (addr < this.BaseAddress)
            {
                return 0;
            }
            uint id = (addr - this.BaseAddress) / this.BlockSize;
            if (id > this.DataCount)
            {
                return 0;
            }
            return id;
        }
        //Union切替TABで現在選択されいるページNameより、 利用するべき prefixを求める.
        public static string ConvertUniTabPageToSelectedUnionPrefixName(TabControl unionTab)
        {
            string name = unionTab.SelectedTab.Name;
            string prefix = name.Replace("UNIONTAB_","") + "_";
            if (name == prefix)
            {
                throw new Exception("broken union prefix! name" + name + " == prefix" + prefix);
            }
            return prefix;
        }
        List<string> MakeSearchPrefixNames(string prefix,List<string> unionPrefixList)
        {
            List<string> r = new List<string>();
            r.Add(prefix);
            foreach(string p in unionPrefixList)
            {
                r.Add(prefix + p);
            }
            return r;
        }

        //サイズnullで、データ長を取る処理は、あまりに多用されるのでキャッシュする.
        //このキャッシュはあればうれしい程度なので、
        //速度よりも安全第一を取る.
        //そのため、ファイルに書き込んだりしたりしたらすべて破棄する.
        static ConcurrentDictionary<ulong, uint> CacheDataCount = new ConcurrentDictionary<ulong, uint>();

        ulong MakeHashForCacheDataCount()
        {
            Debug.Assert(this.BaseAddress != 0);
            return (this.BaseAddress << 8) + this.BlockSize;
        }
        public static void ClearCacheDataCount()
        {
            CacheDataCount.Clear();
            TextForm.UpdateDataCountCache();
            OptionForm.ClearCache();
            ImageUtilMagic.ClearCache();
            ItemShopForm.ClearCache();
            MapPointerForm.ClearPlistCache();
            MagicSplitUtil.ClearCache();
            SkillConfigFE8NSkillForm.ClearCache();
            SkillConfigFE8NVer2SkillForm.ClearCache();
            SkillConfigSkillSystemForm.ClearCache();
            FE8SpellMenuExtendsForm.ClearCache();
            MapChangeForm.ClearCache();
            ItemWeaponEffectForm.ClearCache();

            Cache_Setting_checkbox = new ConcurrentDictionary<string, Dictionary<uint, string>>();
            PatchUtil.ClearCache();
        }


        public InputFormRef(Form self, String prefix, uint basepointer, uint blocksize)
        {
            Init(self
                , prefix
                , new List<String>()
                , basepointer
                , blocksize
                , (uint addr) => { return addr + blocksize; }
                , (int i, uint addr) => { return i < 1; }
                , (int i, uint addr) => { return new U.AddrResult(addr, U.ToHexString(i) ); }
                );
        }
        public InputFormRef(Form self, String prefix, uint basepointer, uint blocksize
            , Func<int, uint, string> loopcallback)
        {
            Init(self
                , prefix
                , new List<String>()
                , basepointer
                , blocksize
                , (uint addr) => { return addr + blocksize; }
                , (int i, uint addr) => { return i < 1; }
                , (int i, uint addr) => { return new U.AddrResult(addr, loopcallback(i, addr)); }
                );
        }
    
        public InputFormRef(Form self, String prefix, uint basepointer, uint blocksize
            , Func<int, uint, bool> isDataExistsCallback
            , Func<int, uint,string> loopcallback
            )
        {
            Init(self
                , prefix
                , new List<String>()
                , basepointer
                , blocksize
                , (uint addr) => { return addr + blocksize; }
                , isDataExistsCallback
                , (int i, uint addr) => { return new U.AddrResult(addr, loopcallback(i, addr)); }
                );
        }


        public InputFormRef(Form self
            ,string prefix
            ,List<String> unionPrefixList
            ,uint basepointer, uint blocksize
            ,Func<int, uint, bool> isDataExistsCallback
            ,Func<int, uint,String> loopcallback
            )
        {
            Init(self
                , prefix
                , unionPrefixList
                , basepointer
                , blocksize
                , (uint addr) => { return addr + blocksize; }
                , isDataExistsCallback
                , (int i, uint addr) => { return new U.AddrResult(addr, loopcallback(i, addr)); }
                );
        }
        public InputFormRef(Form self, String prefix, uint basepointer, uint blocksize
            , Func<int, uint, bool> isDataExistsCallback
            , Func<int, uint, U.AddrResult> loopcallback
            )
        {
            Init(self
                , prefix
                , new List<String>()
                , basepointer
                , blocksize
                , (uint addr) => { return addr + blocksize; }
                , isDataExistsCallback
                , loopcallback
                );
        }

        public InputFormRef(Form self
            ,string prefix
            ,List<String> unionPrefixList
            ,uint basepointer, uint blocksize
            ,Func<int, uint, bool> isDataExistsCallback
            ,Func<int, uint, U.AddrResult> loopcallback
            )
        {
            Init(self
                , prefix
                , unionPrefixList
                , basepointer
                , blocksize
                , (uint addr) => { return addr + blocksize; }
                , isDataExistsCallback
                , loopcallback
                );
        }
        public InputFormRef(Form self
            , string prefix
            , List<String> unionPrefixList
            , uint basepointer, uint blocksize
            , Func<uint, uint> nextAddrCallback
            , Func<int, uint, bool> isDataExistsCallback
            , Func<int, uint, U.AddrResult> loopcallback
            )
        {
            Init(self
                , prefix
                , unionPrefixList
                , basepointer
                , blocksize
                , nextAddrCallback
                , isDataExistsCallback
                , loopcallback
                );
        }

        void CalcDataCountWhenSelfNULL()
        {
            Debug.Assert(this.SelfForm == null);
            if (this.BaseAddress != 0)
            {//サイズを求められるならば...
                //あまりに多用されるのでキャッシュします.
                uint dataCount;
                if (! CacheDataCount.TryGetValue(MakeHashForCacheDataCount(), out dataCount))
                {//キャッシュにないのでサイズを求め計算しなおす.
                    dataCount = Program.ROM.getBlockDataCount(this.BaseAddress, this.BlockSize, this.IsDataExistsCallback);
                    Debug.Assert(dataCount <= 0x10000);
                    CacheDataCount[MakeHashForCacheDataCount()] = dataCount;
                }
                Debug.Assert(dataCount <= 0x10000);
                this.DataCount = dataCount;
            }
            else
            {
                this.DataCount = 0;
            }
        }
        void CalcDataCount()
        {
            Debug.Assert(this.SelfForm != null);
            if (this.BaseAddress != 0)
            {
                this.DataCount = Program.ROM.getBlockDataCount(this.BaseAddress, this.IsDataExistsCallback, this.NextAddrCallback, this.BlockSize);
                CacheDataCount[MakeHashForCacheDataCount()] = this.DataCount;
            }
            else
            {
                this.DataCount = 0;
            }
        }

        public void Init(Form self
            , string prefix
            , List<String> unionPrefixList
            , uint basepointer, uint blocksize
            , Func<uint ,uint > nextAddrCallback
            , Func<int, uint, bool> isDataExistsCallback
            , Func<int, uint, U.AddrResult> loopcallback
            )
        {
            this.BasePointer = U.toOffset(basepointer);
            if (U.isSafetyOffset(this.BasePointer))
            {
                this.BaseAddress = Program.ROM.p32(this.BasePointer);
                if (! U.isSafetyOffset(this.BaseAddress))
                {
                    //Debug.Assert(false);
                    //ポインタ先に有効なデータがない
                    this.BaseAddress = 0;
                    this.BasePointer = 0;
                }
            }
            else
            {
                Debug.Assert(this.BasePointer == 0 || this.BasePointer == U.NOT_FOUND);
                this.BaseAddress = 0;
            }

            this.BlockSize = blocksize;
            this.IsDataExistsCallback = isDataExistsCallback;
            this.LoopCallback = loopcallback;
            this.NextAddrCallback = nextAddrCallback;
            this.Prefix = prefix;
            this.SelfForm = self;
            this.UnionPrefixList = unionPrefixList;

            if (self == null)
            {//静的データ取得.
                CalcDataCountWhenSelfNULL();
                return;
            }

            if (!U.isPadding4(blocksize))
            {//4の倍数ではないので、チェックは無理
                this.CheckProtectionPaddingALIGN4 = false;
            }

            this.Controls = GetAllControls(self);

            //まず主要ボタンの検出を行います.
            foreach (Control info in this.Controls)
            {
                String name = SkipPrefixName(info.Name, prefix);
                if (name.Length <= 0)
                {
                    continue;
                }

                if (name == "ReadStartAddress" && info is NumericUpDown)
                {
                    this.ReadStartAddress = (NumericUpDown)info;
                    this.ReadStartAddress.Increment = blocksize;
                    continue;
                }
                else if (name == "ReadCount" && info is NumericUpDown)
                {
                    this.ReadCount = (NumericUpDown)info;
                    continue;
                }
                else if (name == "Address" && info is NumericUpDown)
                {
                    this.Address = (NumericUpDown)info;
                    this.Address.Increment = blocksize;
                    continue;
                }
                else if (name == "BlockSize" && info is TextBoxEx)
                {
                    this.BlockSizeTextBox = (TextBoxEx)info;
                    this.BlockSizeTextBox.ForeColor = OptionForm.Color_InputDecimal_ForeColor();
                    this.BlockSizeTextBox.BackColor = OptionForm.Color_InputDecimal_BackColor();
                    continue;
                }
                else if (name == "SelectAddress" && info is TextBoxEx)
                {
                    this.SelectAddressTextBox = (TextBoxEx)info;
                    continue;
                }
                else if (name == "WriteButton" && info is Button)
                {
                    this.WriteButton = (Button)info;
                    continue;
                }
                else if (name == "ReloadListButton" && info is Button)
                {
                    this.ReloadListButton = (Button)info;
                    continue;
                }
                else if (name == "AddressList" && info is ListBoxEx)
                {
                    this.AddressList = (ListBoxEx)info;
                    continue;
                }
                else if (name.IndexOf("AddressListExpandsButton") == 0 && info is Button)
                {
                    this.AddressListExpandsButton = (Button)info;
                    continue;
                }
                else if (name == "UNIONTAB" && info is TabControl)
                {
                    this.UnionTab = (TabControl)info;
                    continue;
                }
            }

            List<string> searchPrefixNames = MakeSearchPrefixNames(prefix, unionPrefixList);
            foreach(string pprefix in searchPrefixNames)
            {
                //次に、 D0 みたいなアドレスものを検出します.
                foreach (Control info in this.Controls)
                {
                    if (!(info is NumericUpDown))
                    {
                        continue;
                    }

                    String name = SkipPrefixName(info.Name, pprefix);
                    if (name.Length <= 0)
                    {
                        continue;
                    }

                    NumericUpDown found_object = (NumericUpDown)info;
                    uint id = U.NOT_FOUND;

                    if (name[0] == 'B' && name[1] >= '0' && name[1] <= '9')
                    {
                        id = U.atoi(name.Substring(1));
                        found_object.Minimum = 0;
                        found_object.Maximum = 255;
                    }
                    else if (name[0] == 'b' && name[1] >= '0' && name[1] <= '9')
                    {
                        id = U.atoi(name.Substring(1));
                        found_object.Minimum = -127;
                        found_object.Maximum = 127;
                    }
                    else if (name[0] == 'W' && name[1] >= '0' && name[1] <= '9')
                    {
                        id = U.atoi(name.Substring(1));
                        found_object.Minimum = 0;
                        found_object.Maximum = 65535;
                    }
                    else if (name[0] == 'D' && name[1] >= '0' && name[1] <= '9')
                    {
                        id = U.atoi(name.Substring(1));
                        found_object.Minimum = 0;
                        found_object.Maximum = 0xFFFFFFFF;
                    }
                    else if (name[0] == 'P' && name[1] >= '0' && name[1] <= '9')
                    {
                        id = U.atoi(name.Substring(1));
                        found_object.Minimum = 0;
                        found_object.Maximum = 0xFFFFFFFF;
                    }
                    else if (name[0] == 'l' && name[1] >= '0' && name[1] <= '9')
                    {
                        id = U.atoi(name.Substring(1));
                        found_object.Minimum = 0;
                        found_object.Maximum = 16;
                    }
                    else if (name[0] == 'h' && name[1] >= '0' && name[1] <= '9')
                    {
                        id = U.atoi(name.Substring(1));
                        found_object.Minimum = 0;
                        found_object.Maximum = 16;
                    }

                    if (id != U.NOT_FOUND)
                    {
                        if (this.SelectAddressTextBox != null)
                        {
                            found_object.Enter += DisplaySelectBoxToAddressHandler;
                        }
                        found_object.ValueChanged += LightupWriteButtonHandler;
                    }
                    if (UnionTab != null && name.IndexOf("UNIONKEY") >= 0)
                    {//union切替を担当する
                        found_object.ValueChanged += OnChangeUnionKeyHandler;
                    }
                }

                //リンクの設定.
                MakeLinkEvent(pprefix, this.Controls);
                //ジャンプの設定
                MakeJumpEvent(pprefix, this.Controls, Address, AddressList);
            }

            //データ長があれば長さを求める
            CalcDataCount();

            if (this.ReadStartAddress != null)
            {
                this.ReadStartAddress.Increment = this.BlockSize;
                this.ReadStartAddress.Maximum = 0xFFFFFFFF;
                U.ForceUpdate(this.ReadStartAddress, U.toOffset(this.BaseAddress));
            }
            if (this.ReadCount != null)
            {
                this.ReadCount.Increment = 1;
                this.ReadCount.Maximum = 0xFFFFFFFF;
                U.ForceUpdate(this.ReadCount, this.DataCount);
            }
            if (this.Address != null)
            {
                this.Address.Increment = this.BlockSize;
                this.Address.Maximum = 0xFFFFFFFF;
                U.ForceUpdate(this.Address, U.toOffset(this.BaseAddress));
            }
            if (this.BlockSizeTextBox != null)
            {
                this.BlockSizeTextBox.Text = this.BlockSize.ToString();
            }

            if (this.AddressList != null)
            {
                if (this.AddressList.DrawMode == DrawMode.Normal)
                {//ディフォルトならば、テキストを描くだけのオーナードローを割り当てる Hover狙い.
                    this.AddressList.OwnerDraw(ListBoxEx.DrawTextOnly, DrawMode.OwnerDrawFixed);
                }

                //リストボックスの項目を選択した時.
                //IDEからは SelectedIndexChangedになるので、あえて SelectedValueChangedの方にフックをかける.
                //SelectedValueChangedの方が先に呼ばれるので、とても都合がよい.
                this.AddressList.SelectedValueChanged += AddressListSelectedHandler;
            }
            if (this.SelectAddressTextBox != null)
            {//選択アドレス項目をダブルクリックしたらバイナリエディタをだそうか.
                this.SelectAddressTextBox.MouseDoubleClick += ShowDumpSelectDialogHandler;
            }

            if (this.WriteButton != null)
            {
                //書き込みボタンを押したとき
                this.WriteButton.Click += WriteHandler;
            }

            if (this.ReloadListButton != null)
            {
                //再取得を押したとき
                this.ReloadListButton.Click += this.OnReloadButtonClickHandler;
            }
            //ベースアドレスがちゃんと入っていれば、ディフォルトで再取得する.
            if (this.BaseAddress != 0)
            {
                AddressList_ReloadHandler(null, null);
            }
            if (this.AddressListExpandsButton != null)
            {//リストの拡張
                //リストの拡張ボタンを押したとき
                this.AddressListExpandsButton.Click += this.OnAddressListExpandsEventHandler;

                //拡張上限が設定されていれば設定する.
                this.AddressListExpandsMax = GetAddressListExpandsMax();
            }
            if (this.ReadStartAddress != null)
            {
                this.ReadStartAddress.KeyDown += Address_ReturnKeyToReloadList;
            }
            if (this.ReadCount != null)
            {
                this.ReadCount.KeyDown += Address_ReturnKeyToReloadList;
            }
            if (this.Address != null)
            {
                this.Address.KeyDown += Address_ReturnKeyToReloadList;
            }
            if (UnionTab != null)
            {
                TabControlHideTabOption(UnionTab);
            }

            //最大化禁止 C#のanchorの仕様は変だと思う。 C++Builderはあんなによかったのに.
            self.MaximizeBox = false;
        }
        void AddressListSelectedHandler(object sender, EventArgs e)
        {
            //選択された場所のアドレスを取得する.
            //非表示の項目もあるので、リストに記憶しておいて問い合わせる.
            uint addr = SelectToAddr(this.AddressList);
            if (addr == U.NOT_FOUND)
            {
                return;
            }
            if (this.Address != null)
            {
                U.ForceUpdate(this.Address, U.toOffset(addr));
            }
            if (this.SelectAddressTextBox != null)
            {
                this.SelectAddressTextBox.Text = addr.ToString("X8");
            }
            RomToUI(addr);
        }
        void RomToUI(uint addr)
        {
            IsUpdateLock = true;

            List<string> searchPrefixNames = MakeSearchPrefixNames(this.Prefix, this.UnionPrefixList);
            foreach (string pprefix in searchPrefixNames)
            {
                RomToUI(addr, pprefix, this.Controls);
            }

            IsUpdateLock = false;
            if (this.WriteButton != null)
            {
                WriteButtonToYellow(this.WriteButton, false);
            }
        }

        void DisplaySelectBoxToAddressHandler(object sender, EventArgs e)
        {
            //選択されたコントロールの、 ベースコードから +NN という位置を表示する
            if (this.Address == null)
            {
                this.SelectAddressTextBox.Text = "";
                return;
            }
            uint addr = U.toOffset((uint)(this.Address.Value));
            if (addr == 0)
            {
                this.SelectAddressTextBox.Text = "";
                return;
            }
            if (!(sender is Control))
            {
                this.SelectAddressTextBox.Text = "";
                return;
            }

            Control c = (Control)sender;
            string prefix = GetPrefixName(c.Name);
            uint id = GetStructID(prefix, c.Name);
            if (id == U.NOT_FOUND)
            {//IDが取れないので無理.
                this.SelectAddressTextBox.Text = "";
                return;
            }

            string r = (addr + id).ToString("X");
            if (id != U.NOT_FOUND)
            {
                this.SelectAddressTextBox.Text = r + " (+0x" + id.ToString("X") + ")";
            }
            else
            {
                this.SelectAddressTextBox.Text = r;
            }
        }
        void LightupWriteButtonHandler(object sender, EventArgs e)
        {//変更されたときに、書き込むボタンを強調する.
            if (this.IsUpdateLock)
            {
                return;
            }
            WriteButtonToYellow(this.WriteButton, true);
        }
        void OnChangeUnionKeyHandler(object sender, EventArgs e)
        {
            if (!(sender is NumericUpDown))
            {
                return;
            }
            NumericUpDown found_object = (NumericUpDown)sender;

            int v = (int)found_object.Value;
            string tabName = this.Prefix + "UNIONTAB_N" + v.ToString("X02");
            TabPage page = UnionTab.TabPages[tabName];
            if (page == null)
            {
                return;
            }

            MapPictureBox map = FindMapObject(this.Prefix, this.Controls);
            if (map != null)
            {
                map.SetStopDrawMarkupMarkNotUnionTab(this.Prefix + "N" + v.ToString("X02"));
            }

            Control prevFocusedControl = this.SelfForm.ActiveControl;
            UnionTab.SelectedTab = page;
            if (prevFocusedControl != null)
            {
                prevFocusedControl.Focus();
            }
        }

        public EventHandler PreWriteHandler;
        public EventHandler PostWriteHandler;

        void WriteHandler(object sender, EventArgs e)
        {
            //書き込むアドレス
            uint addr;
            if (this.Address == null)
            {//もし、Address(現在選択しているデータ)がわからななければ先頭に.
                if (this.ReadStartAddress == null)
                {
                    throw new Exception("Address And ReadStartAddress is null");
                }
                addr = (uint)this.ReadStartAddress.Value;
            }
            else
            {
                addr = (uint)this.Address.Value;
            }

            addr = U.toOffset(addr);
            if (!CheckZeroAddressWrite(addr))
            {//先頭には書き込めません!
                return;
            }
            if (!CheckSaftyRangeWrite(addr))
            {//範囲外書き込みで拒否された.
                return;
            }
            if (!CheckWriteProtectionID00())
            { //先頭のID:0x00は書き込み禁止になっていた
                return;
            }
            if (!CheckPaddingALIGN4(addr))
            {//ALIGN4ではないアドレスに書き込みをした
                return;
            }

            if (PreWriteHandler != null)
            {
                PreWriteHandler(sender, e);
            }

            string undo_name = "";
            if (this.SelfForm != null)
            {
                undo_name += U.StripROMFilename(this.SelfForm.Text) + " (" + this.SelfForm.Name + ")";
            }
            if (this.AddressList != null)
            {
                undo_name += " @" + this.AddressList.Text;
            }
            Program.Undo.Push(undo_name, addr, this.BlockSize);


            UIToRom(addr, this.Prefix, this.Controls);
            if (UnionTab != null)
            {
                UIToRom(addr, ConvertUniTabPageToSelectedUnionPrefixName(UnionTab), this.Controls);
            }

            ShowWriteNotifyAnimation(this.SelfForm, addr);
            ReloadAddressList();

            WriteButtonToYellow(this.WriteButton, false);
            if (PostWriteHandler != null)
            {
                PostWriteHandler(sender, e);
            }
        }
        public bool CheckProtectionAddrHigh = true; //高い範囲のアドレスを自動保護する
        public bool CheckZeroAddressWrite(uint addr)
        {
            if (this.CheckProtectionAddrHigh)
            {
                return U.CheckZeroAddressWriteHigh(addr);
            }
            else
            {
                return U.CheckZeroAddressWrite(addr);
            }
        }

        public bool CheckProtectionPaddingALIGN4 = true; //常にALIGN4である必要があるデータ
        bool CheckPaddingALIGN4(uint addr)
        {
            if (CheckProtectionPaddingALIGN4)
            {
                return U.CheckPaddingALIGN4(addr);
            }
            //確認不能.
            return true;
        }

        //ID:0x00を書き込み禁止にするかどうか. ディフォルトはしない.
        public bool UseWriteProtectionID00;
        public bool CheckWriteProtectionID00()
        {
            if (UseWriteProtectionID00 == false)
            {//ID:0x00に対して書き込み禁止を利用しない
                return true;
            }
            if (this.AddressList == null)
            {//アドレスリストがないので不明
                return true;
            }
            if (this.AddressList.SelectedIndex != 0)
            {//リストの先頭ではないなら無視
                return true;
            }
            uint id = U.atoh(this.AddressList.Text);
            if (id != 0)
            {//リストの中身がゼロではない.
                return true;
            }
            DialogResult dr = R.ShowNoYes("{0}を変更しようとしています。\r\nID:0x00は、未設定のデータを表現するために予約されているデータです。\r\nこの変更するのは危険です。\r\nそれでも、書き込んでもよろしいですか？", this.AddressList.Text);
            return (dr == DialogResult.Yes);
        }

        //メモリは連続していないので単純な警告は出せない.
        public bool IsMemoryNotContinuous;

        //範囲外書き込みの警告
        bool CheckSaftyRangeWrite(uint addr)
        {
            if (this.IsMemoryNotContinuous)
            {//確認不能.
                return true;
            }

            bool r = IsSaftyRangeWrite(addr);
            if (r)
            {//安全 範囲外ではない.
                return true;
            }
            OptionForm.write_out_of_range_enum option = OptionForm.write_out_of_range();
            if (option == OptionForm.write_out_of_range_enum.NoWarning)
            {
                R.Notify("アドレス{0}は範囲外ですが、設定により、警告はしません",U.ToHexString8(addr) );
                return true;
            }
            if (option == OptionForm.write_out_of_range_enum.Warning)
            {
                DialogResult dr = R.ShowNoYes("アドレス{0}は範囲外ですが、書き込んでもよろしいですか？\r\n範囲外へ書き込むとROMを破壊する危険性があります。\r\n\r\nHINT:データ数を安全に増やすには、リストの下部にある「リストの拡張」ボタンを利用してください。", U.ToHexString8(addr));
                if (dr == DialogResult.Yes)
                {//ユーザが書き込むらしいという選択をした.
                    return true;
                }
                //書き込みしない.
                return false;
            }
            R.ShowStopError("アドレス{0}は範囲外なので、書き込むことができません。\r\n範囲外へ書き込むとROMを破壊する危険性があるため、設定オプションで拒否する設定になっています。\r\n\r\nHINT:データ数を安全に増やすには、リストの下部にある「リストの拡張」ボタンを利用してください。", U.ToHexString8(addr));
            return false;
        }

        //安全な領域か？
        bool IsSaftyRangeWrite(uint addr)
        {
            Debug.Assert(U.isOffset(addr));

            if (OldBaseAddress == 0 || OldDataCount == 0 || this.BlockSize == 0)
            {//判別不明
                Debug.Assert(false);
                return true;
            }
            if (addr < OldBaseAddress)
            {//開始アドレスより低いアドレスに書こうとしている.
                return false;
            }
            uint limitAddr = OldBaseAddress + ( (OldDataCount) * this.BlockSize);
            if (addr >= limitAddr)
            {//終端を超えようとしている
                return false;
            }
            return true;
        }

        //リロードボタンを押したとき
        uint OldBaseAddress;    //リロードする前のアドレスを記録しておく.
        uint OldDataCount;      //リロードする前の長さを記録しておく.
        void OnReloadButtonClickHandler(object sender, EventArgs e)
        {
            //更新前の状態を記録
            uint oldBaseAddress = this.BaseAddress;
            uint oldDataCount = this.OldDataCount;

            //選択位置があれば保存する.
            int selected = this.AddressList.SelectedIndex;

            //更新処理
            this.AddressList_ReloadHandler(sender, e);
            
            if (sender != null)
            {//ユーザーがリロードボタンを手で押したらしい
                this.OldBaseAddress = oldBaseAddress;
                this.OldDataCount = oldDataCount;
            }

            //再選択
            U.SelectedIndexSafety(this.AddressList, selected);
        }

        void OnAddressListExpandsEventHandler(object sender,EventArgs e)
        {
            if (OldDataCount != 0 && (OldBaseAddress != BaseAddress || OldDataCount != DataCount))
            {
                Debug.Assert(false);

                //ユーザが勝手に拡張している危険だから元に戻すか.
                this.BaseAddress = this.OldBaseAddress;
                this.DataCount = this.OldDataCount;
            }


            if (this.PreAddressListExpandsEvent != null)
            {
                ExpandsEventArgs eventarg = new ExpandsEventArgs();
                eventarg.OldBaseAddress = BaseAddress;
                eventarg.BlockSize = BlockSize;
                eventarg.OldDataCount = DataCount;

                this.PreAddressListExpandsEvent(sender, (EventArgs)eventarg);
                Debug.Assert(eventarg.NewBaseAddress != U.NOT_FOUND);

                if (eventarg.IsCancel)
                {//キャンセル.
                    return;
                }
            }

            MoveToFreeSapceForm f = (MoveToFreeSapceForm)InputFormRef.JumpForm<MoveToFreeSapceForm>();
            f.FromInputFormRef(this, (uint newDataAddr, uint newDataCount) =>
            {
                ExpandsEventArgs eventarg = new ExpandsEventArgs();
                eventarg.OldBaseAddress = BaseAddress;
                eventarg.BlockSize = BlockSize;
                eventarg.OldDataCount = DataCount;
                eventarg.NewBaseAddress = newDataAddr;
                eventarg.NewDataCount = newDataCount;
                        
                //拡張した領域を検索してサイズ等を再設定
                ReInit(newDataAddr, newDataCount);

                if (this.AddressListExpandsEvent != null)
                {
                    this.AddressListExpandsEvent(this.AddressList, eventarg);
                    if (eventarg.IsReload)
                    {
                        ReInit(eventarg.NewBaseAddress, eventarg.NewDataCount);
                    }
                }

                NotifyChangePointer(eventarg.OldBaseAddress, eventarg.NewBaseAddress);
                return 0;
            }
            );
        }

        public uint AddressListExpandsMax = U.NOT_FOUND;

        //拡張できる上限
        uint GetAddressListExpandsMax()
        {
            string expands = U.substr(this.AddressListExpandsButton.Name, this.Prefix.Length);
            string[] sp = expands.Split('_');
            if (sp.Length < 2)
            {//未定義
                return U.NOT_FOUND;
            }
            //定義がある場合.
            return U.atoi(sp[1]);
        }

        public static void WriteButtonToYellow(Button writeButton, bool isChange)
        {
            if (writeButton == null)
            {
                return;
            }

            if (isChange)
            {
                writeButton.ForeColor = OptionForm.Color_NotifyWrite_ForeColor();
                writeButton.BackColor = OptionForm.Color_NotifyWrite_BackColor();
            }
            else
            {
                writeButton.ForeColor = OptionForm.Color_Control_ForeColor();
                writeButton.BackColor = OptionForm.Color_Control_BackColor();
            }
        }
        //多少非効率だが、Numlicがちょっとしかないホーム用で InputFormRefを適応していない場合において、
        //変更があったら、書き込みボタンを光らせたい場合に。
        public static void RegistNotifyNumlicUpdate(Button writeButton,List<Control> controls)
        {
            for (int i = 0; i < controls.Count; i++)
            {
                if (controls[i] is NumericUpDown)
                {
                    NumericUpDown numObj = (NumericUpDown)controls[i];
                    numObj.ValueChanged += (sender, e) =>
                    {
                        InputFormRef.WriteButtonToYellow(writeButton, true);
                    };
                }
            }
        }



        //タブコントロールからタブを消す.
        public static void TabControlHideTabOption(TabControl tab)
        {
            tab.Location = new System.Drawing.Point(tab.Location.X, tab.Location.Y - 30);
            tab.Size = new Size(tab.Size.Width, tab.Size.Height + 30);
        }
        public static void TabControlHideTabOption(MapPictureBox tab)
        {
            tab.Location = new System.Drawing.Point(tab.Location.X, tab.Location.Y - 30);
            tab.Size = new Size(tab.Size.Width, tab.Size.Height + 30);
        }

        //再取得する命令.
        public void Address_ReturnKeyToReloadList(object sender,System.Windows.Forms.KeyEventArgs  e)
        {
            if (e.KeyData == Keys.Return)
            {
                OnReloadButtonClickHandler(sender, e);
            }
        }

        public void GeneralAddressList_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.AddressList == null)
            {
                return;
            }

            if (e.Control && e.KeyCode == Keys.C)
            {
                CopyToClipbord();
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                ClipbordToPaste();
            }
//            else if (e.Control && e.Alt && e.Shift && e.KeyCode == Keys.Up)
//            {
//                ShiftData(false);
//            }
//            else if (e.Control && e.Alt && e.Shift && e.KeyCode == Keys.Down)
//            {
//                ShiftData(true);
//            }
            else if (e.Control && e.KeyCode == Keys.Up)
            {
                SwapData(false);
            }
            else if (e.Control && e.KeyCode == Keys.Down)
            {
                SwapData(true);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                ClearData();
            }
        }

        void ClearData()
        {
            if (this.UseMenuDeleteAction == false)
            {//削除は利用できない
                return;
            }

            uint destAddr = InputFormRef.SelectToAddr(this.AddressList);
            if (destAddr == U.NOT_FOUND)
            {
                return;
            }

            DialogResult dr = R.ShowYesNo("このデータを消去してもよろしいですか？\r\nこのデータが終端になり、このデータまでが有効なデータとなります。");
            if (dr != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            byte[] data = new byte[this.BlockSize];

            Undo.UndoData undodata = Program.Undo.NewUndoData(this.SelfForm);
            Program.ROM.write_range(destAddr, data, undodata);
            Program.Undo.Push(undodata);

            ReloadAddressList();

            InputFormRef.ShowWriteNotifyAnimation(this.SelfForm, destAddr);
            this.AddressList.Refresh();
        }
        public void CopyToClipbord()
        {
            uint srcAddr = InputFormRef.SelectToAddr(this.AddressList);
            if (srcAddr == U.NOT_FOUND)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(this.AddressList.Name);
            sb.Append('@');
            sb.Append(this.SelfForm.Name);

            byte[] data = Program.ROM.getBinaryData(srcAddr, this.BlockSize);
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(' ');
                sb.Append(data[i].ToString("X"));
            }

            string text = sb.ToString();
            U.Clipboard_SetText(text);
        }
        public void ClipbordToPaste()
        {
            uint destAddr = InputFormRef.SelectToAddr(this.AddressList);
            if (destAddr == U.NOT_FOUND)
            {
                return;
            }

            string text = Clipboard.GetText();
            string[] sp = text.Split(' ');
            if (sp.Length != this.BlockSize + 1)
            {
                return;
            }

            if (text == ""
                || U.isSafetyOffset(destAddr) == false
                || sp[0] != this.AddressList.Name + "@" + this.SelfForm.Name
                )
            {
                return;
            }

            DialogResult dr = R.ShowYesNo("クリップボードのデータで上書きしてもよろしいですか？");
            if (dr != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }
            List<byte> data = new List<byte>();
            for (int i = 1; i < sp.Length; i++)
            {
                data.Add((byte)U.atoh(sp[i]));
            }

            Undo.UndoData undodata = Program.Undo.NewUndoData(this.SelfForm);
            Program.ROM.write_range(destAddr, data.ToArray(), undodata);
            Program.Undo.Push(undodata);

            ReloadAddressList();

            InputFormRef.ShowWriteNotifyAnimation(this.SelfForm, destAddr);
            this.AddressList.Refresh();
        }

        public void SwapData(bool isDown)
        {
            if (this.AddressList == null)
            {
                return;
            }
            int selected = this.AddressList.SelectedIndex;
            if (selected < 0)
            {
                return;
            }

            U.AddrResult a = SelectToAddrResult(this.AddressList, selected);
            U.AddrResult b;
            if (isDown)
            {
                if (selected + 1 >= this.AddressList.Items.Count)
                {
                    return;
                }
                b = SelectToAddrResult(this.AddressList, selected + 1);
            }
            else
            {
                if (selected <= 0)
                {
                    return;
                }
                b = SelectToAddrResult(this.AddressList, selected - 1);
            }
            DialogResult dr = R.ShowYesNo("{0}と{1}を入れ替えもよろしいですか？", a.name, b.name);
            if (dr != DialogResult.Yes)
            {
                return;
            }
            Undo.UndoData undodata = Program.Undo.NewUndoData(this.SelfForm, "Swap");
            byte[] abin = Program.ROM.getBinaryData(a.addr, this.BlockSize);
            byte[] bbin = Program.ROM.getBinaryData(b.addr, this.BlockSize);

            Program.ROM.write_range(a.addr, bbin, undodata);
            Program.ROM.write_range(b.addr, abin, undodata);

            Program.Undo.Push(undodata);
            ShowWriteNotifyAnimation(this.SelfForm, a.addr);
            ReloadAddressList();
            this.AddressList.Refresh();

            if (isDown)
            {
                U.SelectedIndexSafety(this.AddressList, selected );
            }
            else
            {
                U.SelectedIndexSafety(this.AddressList, selected );
            }
        }
        void DragAndDropData(int startIndex, int targetIndex)
        {
            if (this.AddressList == null)
            {
                return;
            }
            int totalCount = this.AddressList.Items.Count;
            if (targetIndex < 0 || targetIndex > totalCount)
            {
                return;
            }
            if (startIndex < 0 || startIndex > totalCount)
            {
                return;
            }
            if (targetIndex == startIndex)
            {
                return;
            }
            U.AddrResult targetAR = SelectToAddrResult(this.AddressList, targetIndex);
            U.AddrResult startAR = SelectToAddrResult(this.AddressList, startIndex);

            Undo.UndoData undodata = Program.Undo.NewUndoData(this.SelfForm, "D&D");

            DialogResult dr = R.ShowYesNo(R._("{0}を{1}の位置に移動してもよろしいですか？", startAR.name, targetAR.name));
            if (dr != DialogResult.Yes)
            {
                return;
            }

            byte[] currentData = Program.ROM.getBinaryData(startAR.addr, this.BlockSize);
            if (targetIndex > startIndex)
            {//下にドラッグした
                byte[] shiftData = Program.ROM.getBinaryData(startAR.addr + this.BlockSize, (uint)(targetIndex - startIndex) * this.BlockSize);

                Program.ROM.write_range(startAR.addr, shiftData, undodata);
                Program.ROM.write_range(targetAR.addr, currentData, undodata);
            }
            else
            {//上にドラッグした
                byte[] shiftData = Program.ROM.getBinaryData(targetAR.addr, (uint)(startIndex - targetIndex) * this.BlockSize);

                Program.ROM.write_range(targetAR.addr + this.BlockSize, shiftData, undodata);
                Program.ROM.write_range(targetAR.addr, currentData, undodata);
            }

            Program.Undo.Push(undodata);
            ShowWriteNotifyAnimation(this.SelfForm, startAR.addr);
            ReloadAddressList();
            this.AddressList.Refresh();
        }

        //リストビューを更新して、現在選択しているところを再選択.
        public void ReloadAddressList()
        {
            if (this.AddressList == null)
            {
                if (this.SelfForm == null)
                {
                    return;
                }
                AddressList_ReloadHandler(this.BaseAddress, this.DataCount);
                return;
            }
            int selectedIndex = this.AddressList.SelectedIndex;

            AddressList_ReloadHandler(this.BaseAddress, this.DataCount );


            if (selectedIndex >= 0 && selectedIndex < this.AddressList.Items.Count)
            {
                U.ForceUpdate(this.AddressList, selectedIndex);
            }
            else
            {
                U.SelectedIndexSafety(this.AddressList, 0);//とりあえず一番上を選択.
            }
        }

        //再初期化 ポインタ更新版
        public void ReInitPointer(uint newPointer, uint newDataCount = U.NOT_FOUND, bool IsManualForcedChange = false)
        {
            newPointer = U.toOffset(newPointer);
            this.BasePointer = newPointer;

            uint newAddr = Program.ROM.p32(newPointer);
            ReInit(newAddr, newDataCount, IsManualForcedChange);
        }

        //再初期化.
        public void ReInit(uint newAddr, uint newDataCount = U.NOT_FOUND, bool IsManualForcedChange = false)
        {
            newAddr = U.toOffset(newAddr);

            if (newDataCount == U.NOT_FOUND)
            {//長さが不明な場合、検索する.
                if (U.isSafetyOffset(newAddr))
                {
                    newDataCount = Program.ROM.getBlockDataCount(newAddr, this.IsDataExistsCallback, this.NextAddrCallback, this.BlockSize);
                }
                else
                {
                    newDataCount = 0;
                }
            }

            Debug.Assert(newDataCount <= 0x10000);
            this.BaseAddress = newAddr;
            this.DataCount = newDataCount;
            if (IsManualForcedChange == false)
            {
                this.OldBaseAddress = this.BaseAddress;
                this.OldDataCount = this.DataCount;
            }

            if (this.ReadStartAddress != null)
            {
                U.ForceUpdate(this.ReadStartAddress, newAddr);
            }
            if (this.ReadCount != null)
            {
                U.ForceUpdate(this.ReadCount, newDataCount);
            }
            if (this.Address != null)
            {
                U.ForceUpdate(this.Address, U.toOffset((uint)newAddr));
            };
            ReloadAddressList();
        }

        //再取得する命令.
        void AddressList_ReloadHandler(object sender, EventArgs e)
        {
            uint addr;
            if (this.ReadStartAddress == null)
            {
                addr = this.BaseAddress ;
            }
            else
            {
                addr = U.toOffset((uint)this.ReadStartAddress.Value);
            }

            uint datacount ;
            if (this.ReadCount == null)
            {
                datacount = this.DataCount;
            }
            else
            {
                datacount = (uint)this.ReadCount.Value;
            }

            this.BaseAddress = addr;
            this.DataCount = datacount;
            this.OldBaseAddress = addr;
            this.OldDataCount = datacount;

            AddressList_ReloadHandler(addr, datacount);
        }

        //再取得する命令.
        void AddressList_ReloadHandler(uint addr,uint limitsize)
        {
            if (this.AddressList == null)
            {
                if (this.SelfForm == null)
                {
                    return;
                }
                RomToUI(addr);
                return;
            }
            //リストボックスで選択した項目からアドレスに変換する表
            //(フィルタがあるので単純計算でも止まらないことがあるので、表を作る.)
            List<U.AddrResult> selectAddress = new List<U.AddrResult>();

            //終端
            uint limitter = (uint)Program.ROM.Data.Length;

            this.AddressList.BeginUpdate();
            this.AddressList.Items.Clear();
            if (addr + this.BlockSize > limitter)
            {//終端を超えるので探索不能
                R.Error("アドレス({0}+{1})は、終端(2)を超えるので表示できません。", U.To0xHexString(addr), U.To0xHexString(this.BlockSize), U.To0xHexString(limitter));
            }
            else
            {//探索する
                for (int i = 0; i < limitsize; i++)
                {
                    U.AddrResult ar = this.LoopCallback(i, addr);
                    if (!ar.isNULL())
                    {
                        this.AddressList.Items.Add(ar.name);
                        selectAddress.Add(ar);
                    }

                    addr = this.NextAddrCallback(addr);

                    if (addr >= limitter)
                    {//終端を超えるので探索強制打ち切り.
                        Log.Error("アドレス({0}+{1})は、終端(2)を超えるので表示できません。", U.To0xHexString(addr), U.To0xHexString(this.BlockSize), U.To0xHexString(limitter));
                        break;
                    }
                    if (i >= 0xffff)
                    {//65535個を超えるので探索強制打ち切り.
                        Log.Error("アドレス({0}+{1})は、65535個のアイテムがあります。壊れています。", U.To0xHexString(addr), U.To0xHexString(this.BlockSize));
                        break;
                    }

                }
            }
            this.AddressList.EndUpdate();
            this.AddressList.Tag = selectAddress;


            if (this.AddressList.Items.Count <= 0)
            {//アイテムが何もない場合、設定項目をすべてゼロクリアする
                ClearSelect(false);
            }

            //再取得したので、書き込むボタンをもとに戻す.
            WriteButtonToYellow(WriteButton, false);
        }




        //武器レベル
        public static String GetWeaponClass(uint num)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                if (num <= 0) return "-";
                if (num <= 50) return "E";
                if (num <= 100) return "D";
                if (num <= 150) return "C";
                if (num <= 200) return "B";
                if (num <= 250) return "A";
                return "S";
            }
            else
            {
                if (num <= 0) return "-";
                if (num <= 30) return "E";
                if (num <= 70) return "D";
                if (num <= 120) return "C";
                if (num <= 180) return "B";
                if (num <= 250) return "A";
                return "S";
            }
        }
        public static uint GetWeaponClassRev(string lv)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                if (lv == "E") return 1;
                else if (lv == "D") return 51;
                else if (lv == "C") return 101;
                else if (lv == "B") return 151;
                else if (lv == "A") return 201;
                else if (lv == "S") return 251;
                return 0;
            }
            else
            {
                if (lv == "E") return 1;
                else if (lv == "D") return 31;
                else if (lv == "C") return 71;
                else if (lv == "B") return 121;
                else if (lv == "A") return 181;
                else if (lv == "S") return 251;
                return 0;
            }
        }
        //属性
        public static String GetAttributeName(uint num)
        {
            switch (num)
            {
                case 1:
                    return R._("炎");
                case 2:
                    return R._("雷");
                case 3:
                    return R._("風");
                case 4:
                    return R._("氷");
                case 5:
                    return R._("闇");
                case 6:
                    return R._("光");
                case 7:
                    return R._("理");
            }
            return "-";
        }
        //武器
        public static String GetWeaponTypeName(uint num)
        {
            switch (num)
            {
                case 0x0://剣
                    return R._("剣");
                case 0x1://槍
                    return R._("槍");
                case 0x2://斧
                    return R._("斧");
                case 0x3://弓
                    return R._("弓");
                case 0x4://杖
                    return R._("杖");
                case 0x5://理
                    return R._("理");
                case 0x6://光
                    return R._("光");
                case 0x7://闇
                    return R._("闇");
                case 0x9://道具
                    return R._("道具");
                case 0xB://竜石・魔物専用
                    return R._("竜石・魔物専用");
                case 0xC://指輪
                    return R._("指輪");
                case 0x11://火竜石
                    return R._("火竜石");
                case 0x12://踊る用指輪
                    return R._("踊る用指輪");
            }
            return "-";
        }

        //トラップデータ
        public static string GetTrapName(uint num)
        {
            if (num == 0)
            {
                return R._("0=None");
            }
            else if (num == 1)
            {
                return R._("1=Ballista");
            }
            else if (num == 2)
            {
                return R._("2=Wall/Snag");
            }
            else if (num == 3)
            {
                return R._("3=MapChange");
            }
            else if (num == 4)
            {
                return R._("4=Fire Trap");
            }
            else if (num == 5)
            {
                return R._("5=Gas Trap");
            }
            else if (num == 6)
            {
                return R._("6=MapChange2");
            }
            else if (num == 7)
            {
                return R._("7=Arrow Trap");
            }
            else if (num == 0xA)
            {
                return R._("A=TorchStaff");
            }
            else if (num == 0xB)
            {
                return R._("B=Mine");
            }
            else if (num == 0xC)
            {
                return R._("C=Gorgon Egg");
            }
            else if (num == 0xD)
            {
                return R._("D=Light Rune");
            }
            return U.ToHexString(num) + "=??";
        }

        //フラグ
        public static String GetFlagName(uint num, out string errorMessage)
        {
            string ret;
            if (Program.FlagCache.TryGetValue(num, out ret))
            {
                errorMessage = "";
                return ret;
            }

            errorMessage = R._("この領域は利用できません");
            return R._("利用不可");
        }
        //FE8のメモリスロット
        public static string GetMEMORYSLOT(uint num, out string errorMessae)
        {
            errorMessae = "";
            if (num == 0)
            {
                return R._("常に0(書き込み禁止)");
            }
            if (num == 0x1)
            {
                return R._("Slot1");
            }
            if (num == 0x2)
            {
                return R._("Slot2");
            }
            if (num == 0x3)
            {
                return R._("Slot3");
            }
            if (num == 0x4)
            {
                return R._("Slot4");
            }
            if (num == 0x5)
            {
                return R._("Slot5");
            }
            if (num == 0x6)
            {
                return R._("Slot6");
            }
            if (num == 0x7)
            {
                return R._("Slot7");
            }
            if (num == 0x8)
            {
                return R._("Slot8");
            }
            if (num == 0x9)
            {
                return R._("Slot9");
            }
            if (num == 0xA)
            {
                return R._("SlotA");
            }
            if (num == 0xB)
            {
                return R._("座標結果");
            }
            if (num == 0xC)
            {
                return R._("処理結果");
            }
            if (num == 0xD)
            {
                return R._("キューサイズ");
            }
            errorMessae = R._("メモリスロットは、0x00～0x0D までです。");
            return R._("警告: 下限を超えています");
        }
        //FE8のカウンター
        public static string GetCOUNTER(uint num, out string errorMessae)
        {
            if (num <= 0x7)
            {
                errorMessae = "";
                return "Counter" + num;
            }
            errorMessae = R._("カウンターは、0x00～0x07 までです。");
            return R._("警告: 下限を超えています");
        }

        public static string GetPACKED_MEMORYSLOT(uint num, string op, out string errorMessae)
        {
            uint x = (num >> 4) & 0xF;
            uint y = (num >> 8) & 0xF;
            uint z = (num) & 0xF;

            string o = "";
            if (op.IndexOf("ADD") >= 0)
            {
                o = "+";
            }
            else if (op.IndexOf("SUB") >= 0)
            {
                o = "-";
            }
            else if (op.IndexOf("MUL") >= 0)
            {
                o = "*";
            }
            else if (op.IndexOf("DIV") >= 0)
            {
                o = "/";
            }
            else if (op.IndexOf("MOD") >= 0)
            {
                o = "%";
            }
            else if (op.IndexOf("AND") >= 0)
            {
                o = "&";
            }
            else if (op.IndexOf("ORR") >= 0)
            {
                o = "|";
            }
            else if (op.IndexOf("XOR") >= 0)
            {
                o = "^";
            }
            else if (op.IndexOf("LSL") >= 0)
            {
                o = "<<";
            }
            else if (op.IndexOf("LSR") >= 0)
            {
                o = ">>";
            }

            string ret = "Slot" + z.ToString("X") + "=" + "Slot" + x.ToString("X") + o + "Slot" + y.ToString("X");
            if (z == 0)
            {
                errorMessae = R._("警告: MemorySlot0に書き込もうとしています。") + ret;
                return R._("警告: MemorySlot0に書き込もうとしています。") + ret;
            }
            if (z > 0xD)
            {
                errorMessae = R._("警告: MemorySlotD以降にアクセスしています。") + ret;
                return R._("警告: MemorySlotD以降にアクセスしています。") + ret;
            }
            if (x > 0xD)
            {
                errorMessae = R._("警告: MemorySlotD以降にアクセスしています。") + ret;
                return R._("警告: MemorySlotD以降にアクセスしています。") + ret;
            }
            if (y > 0xD)
            {
                errorMessae = R._("警告: MemorySlotD以降にアクセスしています。") + ret;
                return R._("警告: MemorySlotD以降にアクセスしています。") + ret;
            }
            errorMessae = "";
            return ret;
        }

        //マップでの漫符
        public static string GetMAPEMOTION(uint num)
        {
            Dictionary<uint, string> dic = ConfigDataDatanameCache("map_emotion_exetnds_well_known_list_");
            return U.at(dic, num);
        }

        public static Dictionary<uint,string> MakeTerrainSet()
        {
            return ConfigDataDatanameCache("battleterrain_set_");
        }


        //ユニットの特殊状態
        public static string GetRAM_UNIT_STATE(uint num)
        {
            Dictionary<uint, string> dic = ConfigDataDatanameCache("ramunit_state_checkbox_");
            return GetInfoByBitFlag(num, dic);
        }
        public static string GetDISABLEOPTIONS(uint num)
        {
            if (num == 0)
            {
                return R._("すべてのメニューを許可");
            }
            Dictionary<uint, string> dic = ConfigDataDatanameCache("DISABLEOPTIONS_checkbox_");
            return GetInfoByBitFlag(num, dic);
        }
        public static string GetDISABLEWEAPONS(uint num)
        {
            if (num == 0)
            {
                return R._("すべてのメニューを許可");
            }
            Dictionary<uint, string> dic = ConfigDataDatanameCache("DISABLEWEAPONS_checkbox_");
            return GetInfoByBitFlag(num, dic);
        }
        public static string GetIGNORE_KEYS(uint num)
        {
            if (num == 0)
            {
                return R._("すべてのキーを許可");
            }
            Dictionary<uint, string> dic = ConfigDataDatanameCache("IGNORE_KEYS_checkbox_");
            return GetInfoByBitFlag(num, dic);
        }
        public static string GetUNITCLASSABILITY(uint num)
        {
            if (num == 0)
            {
                return R._("");
            }
            Dictionary<uint, string> dic = ConfigDataDatanameCache("unitclass_checkbox_");
            return GetInfoByBitFlag(num, dic);
        }
        public static string GetPressKEYS(uint num)
        {
            Dictionary<uint, string> dic = ConfigDataDatanameCache("IGNORE_KEYS_checkbox_");
            return GetInfoByBitFlag(num, dic);
        }

        static ConcurrentDictionary<string, Dictionary<uint, string>> Cache_Setting_checkbox = new ConcurrentDictionary<string, Dictionary<uint, string>>();
        public static Dictionary<uint, string> ConfigDataDatanameCache(string type)
        {
            if (Cache_Setting_checkbox.ContainsKey(type))
            {
                return Cache_Setting_checkbox[type];
            }

            string filename = U.ConfigDataFilename(type);
            Cache_Setting_checkbox[type] = U.LoadDicResource(filename);
            return Cache_Setting_checkbox[type];
        }

        static string GetInfoByBitFlag(uint num, Dictionary<uint, string> dic)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var pair in dic)
            {
                uint key = pair.Key;
                int shift = (int)(key & 0xF);
                if (shift <= 0)
                {
                    continue;
                }
                shift = shift - 1;
                shift += (int)((key & 0xf0) >> 4) * 8;

                int bit = 1 << shift;
                if ((num & bit) == bit)
                {
                    string[] sp = pair.Value.Split('\t');
                    sb.Append(',');
                    sb.Append(sp[0]);
                }
            }

            return U.substr(sb.ToString(), 1);
        }
        public static string GetFSEC(uint num)
        {
            if (num == 0xffff || num == 0xffffffff)
            {
                return R._("瞬間移動");
            }
            double sec = Math.Round( ((double)num / 60.0f) , 4);
            return R._("フレーム秒({0}秒)", sec );
        }
        public static string GetMAPXY(uint num)
        {
            uint x = ((num & 0xffff0000) >> 16);
            uint y = ((num & 0x0000ffff) );
            return String.Format("({0},{1})", x, y);
        }

        public static string GetMapChangeName(uint mapid, uint changeid)
        {
            if (changeid == 0xFFFE)
            {
                return R._("アクティブユニットの座標");
            }
            return MapChangeForm.GetName(mapid, changeid);
        }

        //RAM UNIT PARAM
        public static string GetRAM_UNIT_PARAM(uint num, out string errorMessae)
        {
            errorMessae = "";
            StringBuilder sb = new StringBuilder();

            Dictionary<uint, string> ramunit_param_dic;
            if (Cache_Setting_checkbox.ContainsKey("ramunit_param_"))
            {
                ramunit_param_dic = Cache_Setting_checkbox["ramunit_param_"];
            }
            else
            {
                ramunit_param_dic = GetRAM_UNIT_PARAM_Low();
            }
            return U.at(ramunit_param_dic, num);
        }
        static Dictionary<uint, string> GetRAM_UNIT_PARAM_Low()
        {
            Dictionary<uint, string>  ramunit_param_dic = InputFormRef.ConfigDataDatanameCache("ramunit_param_");

            MagicSplitUtil.magic_split_enum magic_split = MagicSplitUtil.SearchMagicSplit();
            if (magic_split == MagicSplitUtil.magic_split_enum.FE8NMAGIC)
            {
                ramunit_param_dic[0x1A] = R._("魔力");
            }
            else if (magic_split == MagicSplitUtil.magic_split_enum.FE7UMAGIC
                || magic_split == MagicSplitUtil.magic_split_enum.FE8UMAGIC)
            {
                ramunit_param_dic[0x3A] = R._("魔力");
            }

            PatchUtil.skill_system_enum skill = PatchUtil.SearchSkillSystem();
            if (skill == PatchUtil.skill_system_enum.FE8N_ver2)
            {//FE8J FE8NSkillは、0x3Aに追加スキルを記録してる
                ramunit_param_dic[0x3A] = R._("追加スキル");
            }
            return ramunit_param_dic;
        }
        public static string GetRAM_UNIT_VALUE(uint prevValue, uint num, out string errorMessae)
        {
            errorMessae = "";

            if (prevValue == 0xC)
            {//状態
                return GetRAM_UNIT_STATE(num);
            }
            else if (prevValue == 0x1E || prevValue == 0x20 || prevValue == 0x22 || prevValue == 0x24 || prevValue == 0x26)
            {//アイテム
                return ItemForm.GetItemName(num);
            }
            else if (prevValue == 0x30)
            {//状態とターン
                return GetBADSTATUS(num);
            }
//            else if (prevValue == 0x40)
//            {//AI3
//            }
//            else if (prevValue == 0x41)
//            {//AI4
//            }
            else if (prevValue == 0x42)
            {//AI1
                return GetAI1(num);
            }
            else if (prevValue == 0x44)
            {//AI2
                return GetAI2(num);
            }
            else if (prevValue == 0x3A)
            {
                PatchUtil.skill_system_enum skill = PatchUtil.SearchSkillSystem();
                if (skill == PatchUtil.skill_system_enum.FE8N_ver2)
                {//FE8J FE8NSkillは、0x3Aに追加スキルを記録してる
                    return SkillConfigFE8NVer2SkillForm.GetSkillText(num);
                }
            }
            return InputFormRef.GetDigitHint(num);
        }
        public static string GetDigitHint(uint v)
        {
            if (v >= 10)
            {
                return v.ToString() + "<=(" + R._("10進数") + ")";
            }
            return "";
        }
        public static string GetBOOL(uint num)
        {
            if (num == 0)
            {
                return "FALSE";
            }
            else if (num == 1)
            {
                return "TRUE";
            }
            return "";
        }
        public static string GetTRAP(uint num)
        {
            if (num == 1)
            {
                return R._("アーチ");
            }
            else if (num == 2)
            {
                return R._("壊れる壁/古木");
            }
            else if (num == 3)
            {
                return R._("マップ変化1");
            }
            else if (num == 4)
            {
                return R._("炎");
            }
            else if (num == 5)
            {
                return R._("毒ガス");
            }
            else if (num == 6)
            {
                return R._("マップ変化2");
            }
            else if (num == 7)
            {
                return R._("神の矢");
            }
            else if (num == 8)
            {
                return R._("不明1");
            }
            else if (num == 9)
            {
                return R._("不明2");
            }
            else if (num == 0xA)
            {
                return R._("トーチ");
            }
            else if (num == 0xB)
            {
                return R._("フレイボム");
            }
            else if (num == 0xC)
            {
                return R._("卵");
            }
            else if (num == 0xD)
            {
                return R._("光の結界");
            }
            return "";
        }

        public static string GetEditon(uint v)
        {
            if (Program.ROM.RomInfo.version() == 8)
            {
                if (v == 1)
                {
                    return R._("序盤");
                }
                if (v == 2)
                {
                    return R._("エイリーク編");
                }
                if (v == 3)
                {
                    return R._("エフラム編");
                }
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                if (v == 1)
                {
                    return R._("リン編");
                }
                if (v == 2)
                {
                    return R._("エリウッド編");
                }
                if (v == 3)
                {
                    return R._("ヘクトル編");
                }
            }
            if (v == 0xFF)
            {
                return R._("FF=無条件");
            }
            return "";
        }
        public static string GetDifficulty(uint v)
        {
            if (v == 0)
            {
                return R._("Easy");
            }
            if (v == 1)
            {
                return R._("Normal");
            }
            if (v == 2)
            {
                return R._("Hard");
            }
            return "";
        }

        public static string GetSuportLevel(uint num)
        {
            if (num == 0)
            {
                return R._("支援なし");
            }
            else if (num == 1)
            {
                return R._("支援C");
            }
            else if (num == 2)
            {
                return R._("支援B");
            }
            else if (num == 3)
            {
                return R._("支援A");
            }
            return "";
        }

        //ワールドマップに表示するユニット
        public static string GetWMAP_SPRITE_ID(uint num, out string errorMessae)
        {
            errorMessae = "";
            if (Program.ROM.RomInfo.version() == 8)
            {
                if (num <= 0x6)
                {
                    return "ID:" + num.ToString();
                }
            }
            else
            {
                if (num <= 0x4)
                {
                    return "ID:"+num.ToString();
                }
            }
            errorMessae = R._("警告: 上限を超えています");
            return R._("警告: 上限を超えています");
        }


        //FE8の世界地図の移動 -8 ～ 52の範囲
        public static string GetMagvelYName(short num, out string errorMessae)
        {
            errorMessae = "";
            if (num < -8)
            {
                errorMessae = R._("警告: 上限を超えています");
                return R._("警告: 上限を超えています");
            }
            if (num == -8)
            {
                return R._("フレリア、ロストン");
            }
            if (num < 0)
            {
                return R._("フレリア、ロストン ～ カルチノ付近");
            }
            if (num == 0)
            {
                return R._("カルチノ");
            }
            if (num < 24)
            {
                return R._("カルチノ ～ ルネス付近");
            }
            if (num == 24)
            {
                return R._("ルネス");
            }
            if (num < 30)
            {
                return R._("ルネス ～ ジャハナ付近");
            }
            if (num == 30)
            {
                return R._("ジャハナ");
            }
            if (num < 48)
            {
                return R._("ジャハナ ～ グラド付近");
            }
            if (num == 48)
            {
                return R._("グラド");
            }
            if (num < 52)
            {
                return R._("グラド　～ 南の果て");
            }
            if (num == 52)
            {
                return R._("一番下");
            }

            errorMessae = R._("警告: 下限を超えています");
            return R._("警告: 下限を超えています");
        }

        public static string GetPORTRAIT_DIRECTION(uint num)
        {
            switch(num)
            {
                case 0x00:
                    return "None";///No Translate
                case 0x01:
                    return "←";///No Translate
                case 0x02:
                    return "→";///No Translate
            }
            return "";
        }

        public static string GetEVBIT(uint num,out string out_errorMessage)
        {
            out_errorMessage = "";
            switch (num)
            {
                case 0x00:
                    return R._("呼び出された親イベントに戻らないで終了する");
                case 0x01:
                    return R._("キューに入っているイベント呼び出しを待機させる");
                case 0x02:
                    return R._("イベントスキップ中");
                case 0x03:
                    return R._("テキストスキップ中");
                case 0x04:
                    return R._("スタートボタンでのイベントスキップ防止");
                case 0x05:
                    return R._("Bボタンでのテキストスキップ防止");
                case 0x06:
                    return R._("Aボタン等でのテキスト早送りを防止");
                case 0x07:
                    return R._("フェードインを解除");
                case 0x08:
                    return R._("フェードイン中");
                case 0x09:
                    return R._("カメラを主人公に固定");
                case 0x0A:
                    return R._("別の章に移動中(?)");
                case 0x0B:
                    return R._("ゲームモードを変更中(GAMECTRLフィールドの0x29を変化?)");
                case 0x0C:
                    return R._("グラフィックがイベントによってロック中(codes 0x23/0x24)");
                case 0x0D:
                    return "?";///No Translate
                case 0x0E:
                    return "?";///No Translate
            }

            return "";
        }

        public static string GetEVBIT_MODIFY(uint num, out string out_errorMessage)
        {
            out_errorMessage = "";
            switch (num)
            {
                case 0x00:
                    return R._("シーンスキップ、ダイアログスキップ、ダイアログ早送りが可能です。");
                case 0x01:
                    return R._("シーンスキップ、ダイアログスキップ、ダイアログ早送りを禁止します。");
                case 0x02:
                    return R._("シーンスキップとダイアログスキップを許可するが、ダイアログ早送りは許可しない。");
                case 0x03:
                    return R._("シーンスキップを許可しないが、ダイアログスキップとダイアログ早送りを許可する。");
                case 0x04:
                    return R._("シーンスキップとダイアログスキップを許可しないが、ダイアログ早送りは許可する。");
                case 0x05:
                    return "?";///No Translate
            }

            return "";
        }


        public static string GetDIRECTION(uint num)
        {
            switch(num)
            {
                case 0x00:
                    return "←";///No Translate
                case 0x01:
                    return "→";///No Translate
                case 0x02:
                    return "↓";///No Translate
                case 0x03:
                    return "↑";///No Translate
            }
            return "";
        }
        public static string GetFOG(uint num)
        {
            switch (num)
            {
                case 0x00:
                    return R._("霧無し");
                case 0xFF:
                    return R._("マップ初期値");
            }
            return "";
        }
        public static string GetPHASE(uint num)
        {
            switch (num)
            {
                case 0x0:
                    return R._("自軍(青)");
                case 0x40:
                    return R._("友軍(緑)");
                case 0x80:
                    return R._("敵軍(赤)");
            }
            return "";
        }
        public static string GetWEATHER(uint num)
        {
            switch (num)
            {
                case 0x00:
                    return R._("晴れ");
                case 0x01:
                    return R._("雪");
                case 0x02:
                    return R._("吹雪");
                case 0x03:
                    return R._("??");
                case 0x04:
                    return R._("雨");
                case 0x05:
                    return R._("マグマ");
                case 0x06:
                    return R._("砂嵐");
                case 0x07:
                    return R._("曇り");
            }
            return "";
        }
        public static string GetDungeonType(uint num)
        {
            switch (num)
            {
                case 0x00:
                    return R._("塔");
                case 0x01:
                    return R._("遺跡");
            }
            return "";
        }

        public static string GetChatperStaff(uint num)
        {
            string r = "";
            if ( (num & 0x1) == 0x1)
            {
                r += " ,0x01 Stat screen page 1 (Inventory)";
            }
            else if ((num & 0x2) == 0x2)
            {
                r += " ,0x02 Stat screen page 2 (Difficulty)";
            }
            else if ((num & 0x10) == 0x10)
            {
                r += " ,0x10 Set when on prep screen";
            }
            else if ((num & 0x40) == 0x40)
            {
                r += " ,0x40 Set for hard mode";
            }
            else if ((num & 0x80) == 0x80)
            {
                r += " ,0x80 Don't gain weapon exp";
            }
            return U.substr(r, 2);
        }
        public static string GetChapterDataConfig(uint num)
        {
            Dictionary<uint, string> dic = ConfigDataDatanameCache("chapterdata_config_");
            return GetInfoByBitFlag(num, dic);
        }
        public static string GetWorldmapStructWMFLAG1(uint num)
        {
            Dictionary<uint, string> dic = ConfigDataDatanameCache("worldmap_struct_wmflag1_");
            return GetInfoByBitFlag(num, dic);
        }
        public static string GetWorldmapNodeFLAG(uint num)
        {
            Dictionary<uint, string> dic = ConfigDataDatanameCache("worldmap_node_flag_");
            return GetInfoByBitFlag(num, dic);
        }


        public static string GetBADSTATUS(uint num)
        {
            uint status = (num & 0x0f);
            uint turn = ((num & 0xf) >> 8);

            string turnString;
            if (turn >= 1)
            {
                turnString = " " + R._("{0}ターン", turn);
            }
            else
            {
                turnString = "";
            }

            switch (status)
            {
                case 0x00:
                    return R._("なし");
                case 0x01:
                    return R._("ポイズン") + turnString;
                case 0x02:
                    return R._("スリープ") + turnString;
                case 0x03:
                    return R._("サイレス") + turnString;
                case 0x04:
                    return R._("バーサク") + turnString;
                case 0x05:
                    return R._("攻撃") + turnString;
                case 0x06:
                    return R._("守備") + turnString;
                case 0x07:
                    return R._("必殺") + turnString;
                case 0x08:
                    return R._("回避") + turnString;
                case 0x0A:
                    return R._("卵") + turnString;
                case 0x0B:
                    return R._("ストーン") + turnString;
            }
            return "";
        }
        public static string GetSkillName(uint num)
        {
            PatchUtil.skill_system_enum skill = PatchUtil.SearchSkillSystem();
            if (skill == PatchUtil.skill_system_enum.SkillSystem)
            {
                return SkillConfigSkillSystemForm.GetSkillText(num);
            }
            else if (skill == PatchUtil.skill_system_enum.FE8N_ver2)
            {
                return SkillConfigFE8NVer2SkillForm.GetSkillText(num);
            }
            else
            {
                return R.Error("スキル拡張がありません");
            }
        }
        public static Bitmap DrawSkillIcon(uint num)
        {
            PatchUtil.skill_system_enum skill = PatchUtil.SearchSkillSystem();
            if (skill == PatchUtil.skill_system_enum.SkillSystem)
            {
                return SkillConfigSkillSystemForm.DrawSkillIcon(num);
            }
            else if (skill == PatchUtil.skill_system_enum.FE8N_ver2)
            {
                return SkillConfigFE8NVer2SkillForm.DrawSkillIcon(num);
            }
            else
            {
                return ImageUtil.Blank(16, 16);
            }
        }

        public static string GetUNIT_COLOR(uint num)
        {
            switch (num)
            {
                case 0x00:
                    return R._("元");
                case 0x01:
                    return R._("青");
                case 0x02:
                    return R._("赤");
                case 0x03:
                    return R._("緑");
                case 0x04:
                    return R._("セピア");
            }
            return "";
        }
        public static string GetEARTHQUAKE(uint num)
        {
            switch (num)
            {
                case 0x00:
                    return R._("横揺");
                case 0x01:
                    return R._("縦揺");
            }
            return "";
        }
        public static string GetATTACK_TYPE(uint num)
        {
            if (num == 0)
            {
                return R._("当てる");
            }
            Dictionary<uint, string> dic = ConfigDataDatanameCache("EVENTBATTLE_checkbox_");
            return GetInfoByBitFlag(num, dic);
        }

        public static string GetMENUCOMMAND(uint num)
        {
            return MenuDefinitionForm.GetMenuNameWhereMenuCommandID(num);
        }
        public static string GetAI1(uint num)
        {
            if (num < EventUnitForm.AI1.Count)
            {
                return EventUnitForm.AI1[(int)num].Name;
            }
            return "";
        }
        public static string GetAI2(uint num)
        {
            if (num < EventUnitForm.AI2.Count)
            {
                return EventUnitForm.AI2[(int)num].Name;
            }
            return "";
        }
        public static string GetCGComment(uint num)
        {
            if (Program.ROM.RomInfo.version() == 7
                && Program.ROM.RomInfo.is_multibyte())
            {
                return ImageCGFE7UForm.GetComment(num);
            }
            return ImageCGForm.GetComment(num);
        }


        public static string GetWMSTYLE1(uint num)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                switch (num)
                {
                    case 0x00:
                        return R._("左上");
                    case 0x01:
                        return R._("右上");
                    case 0x02:
                        return R._("左下");
                    case 0x03:
                        return R._("右下");
                }
                return "";
            }
            if (Program.ROM.RomInfo.version() == 7)
            {
                switch (num)
                {
                    case 0x00:
                        return R._("座標通り");
                    case 0x01:
                        return R._("左下");
                    case 0x02:
                        return R._("右下");
                    case 0x03:
                        return R._("左上");
                    case 0x04:
                        return R._("右上");
                    case 0x05:
                        return R._("上");
                    case 0x06:
                        return R._("下");
                    case 0x07:
                        return R._("左");
                    case 0x08:
                        return R._("右");
                }
                return "";
            }
            return "";
        }
        public static string GetWMSTYLE2(uint num)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                switch (num)
                {
                    case 0x00:
                        return R._("通常");
                    case 0x01:
                        return R._("丸に突き刺す見出し");
                }
                return "";
            }
            if (Program.ROM.RomInfo.version() == 7)
            {
                switch (num)
                {
                    case 0x00:
                        return R._("世界全体");
                    case 0x01:
                        return R._("拡大");
                    case 0x02:
                        return R._("少し拡大");
                    case 0x03:
                        return R._("CG表示");
                }
                return "";
            }
            return "";
        }
        public static string GetWMSTYLE3(uint num)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                switch (num)
                {
                    case 0x00:
                        return R._("赤");
                    case 0x01:
                        return R._("青");
                    case 0x02:
                        return R._("緑");
                }
                return "";
            }
            if (Program.ROM.RomInfo.version() == 7)
            {
                switch (num)
                {
                    case 0x00:
                        return R._("赤");
                    case 0x01:
                        return R._("青");
                }
                return "";
            }
            return "";
        }

        public static string GetWMENREGION(uint num)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                switch (num)
                {
                    case 0x00:
                        return R._("The Kingdom of Etruria");
                    case 0x01:
                        return R._("The Kingdom of Bern");
                    case 0x02:
                        return R._("The League of Lycia");
                    case 0x03:
                        return R._("Ilia");
                    case 0x05:
                        return R._("Fibernia");
                    case 0x08:
                        return R._("Caledonia");
                    case 0x09:
                        return R._("Dia");
                    case 0x0A:
                        return R._("Sacae");
                    case 0x10:
                        return R._("Eburacum");
                    case 0x0D:
                        return R._("Pherae");
                    case 0x18:
                        return R._("Ostia");
                    case 0x1B:
                        return R._("Thria");
                    case 0x1C:
                        return R._("Araphen");
                }
                return "";
            }
            return "";
        }

        public static string GetWMREGION(uint num)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                switch (num)
                {
                    case 0x01:
                        return R._("サカ");
                    case 0x03:
                        return R._("リキア");
                    case 0x04:
                        return R._("ベルン");
                    case 0x05:
                        return R._("イリア");
                    case 0x06:
                        return R._("エトルリア");
                }
                return "";
            }
            if (Program.ROM.RomInfo.version() == 7)
            {
                switch (num)
                {
                    case 0x00:
                        return R._("リキア");
                    case 0x01:
                        return R._("イリア");
                    case 0x02:
                        return R._("エトルリア");
                    case 0x03:
                        return R._("西方三島");
                    case 0x04:
                        return R._("ベルン");
                    case 0x05:
                        return R._("サカ");
                    case 0x06:
                        return R._("ミスル半島");
                    case 0x07:
                        return R._("魔の島");
                    case 0x08:
                        return R._("フェレ");
                }
                return "";
            }

            switch (num)
            {
                case 0x00:
                    return R._("フレリア");
                case 0x01:
                    return R._("グラド");
                case 0x02:
                    return R._("ジャハナ");
                case 0x03:
                    return R._("カルチノ");
                case 0x04:
                    return R._("闇の樹海");
                case 0x05:
                    return R._("ロストン");
                case 0x06:
                    return R._("ポカラ");
                case 0x07:
                    return R._("ルネス");
            }
            return "";
        }
        public static string GetAFFILIATION(uint num)
        {
            switch (num)
            {
                case 0x00:
                    return R._("自軍(青)");
                case 0x40:
                    return R._("友軍(緑)");
                case 0x80:
                    return R._("敵軍(赤)");
            }
            return "";
        }
        public static string GetWMAPAFFILIATION(uint num)
        {
            if (Program.ROM.RomInfo.version() == 7)
            {
                switch (num)
                {
                    case 0x00:
                        return R._("自軍通常(青)");
                    case 0x01:
                        return R._("自軍移動中(青)");
                    case 0x02:
                        return R._("自軍選択時(青)");
                    case 0x20:
                        return R._("敵軍通常(赤)");
                    case 0x21:
                        return R._("敵軍移動中(赤)");
                    case 0x22:
                        return R._("敵軍選択時(赤)");
                    case 0x40:
                        return R._("友軍通常(緑)");
                    case 0x41:
                        return R._("友軍移動中(緑)");
                    case 0x42:
                        return R._("友軍選択時(緑)");
                    case 0x80:
                        return R._("自軍通常(青)(Class==0のみ)");
                    case 0x81:
                        return R._("自軍移動中(青)(Class==0のみ)");
                }
                return "";
            }

            switch (num)
            {
                case 0x00:
                    return R._("自軍(青)");
                case 0x01:
                    return R._("敵軍(赤)");
                case 0x02:
                    return R._("友軍(緑)");
            }
            return "";
        }
        public static string GetWMAP2AFFILIATION(uint num)
        {
            if (Program.ROM.RomInfo.version() == 7)
            {
                switch (num)
                {
                    case 0x1C:
                        return R._("自軍(青)");
                    case 0x1D:
                        return R._("敵軍(赤)");
                    case 0x1E:
                        return R._("友軍(緑)");
                }
                return "";
            }

            return "";
        }

        public static string GetMonthName(uint num)
        {
            switch (num)
            {
                case 0x00:
                    return R._("1月");
                case 0x01:
                    return R._("2月");
                case 0x02:
                    return R._("3月");
                case 0x03:
                    return R._("4月");
                case 0x04:
                    return R._("5月");
                case 0x05:
                    return R._("6月");
                case 0x06:
                    return R._("7月");
                case 0x07:
                    return R._("8月");
                case 0x08:
                    return R._("9月");
                case 0x09:
                    return R._("10月");
                case 0x0A:
                    return R._("11月");
                case 0x0B:
                    return R._("12月");
            }
            return "";
        }
        public static string GetBloodType(uint num)
        {
            switch (num)
            {
                case 0x00:
                    return R._("A");
                case 0x01:
                    return R._("B");
                case 0x02:
                    return R._("O");
                case 0x03:
                    return R._("AB");
            }
            return "";
        }
        
        public static string GetEVENTUNITPOS(uint num)
        {
            uint x = U.ParseFE8UnitPosX(num);
            uint y = U.ParseFE8UnitPosY(num);
//            uint ext = U.ParseFE8UnitPosExtraBit(num);

            return R._("X:{0},Y:{1}", x, y);
        }

        public static String UpdateCoron(String str, String newValue)
        {
            int pos = str.IndexOf(':');
            if (pos < 0)
            {//分離するコロンがない
                return str;
            }
            return str.Substring(0,pos+1) + newValue;
        }

        //combobox値からID値を検索して見つかったら、その文字列を返す
        public static String FindNameFromValueComboBoxWhereID(ComboBox comb,uint id)
        {
            for (int i = 0; i < comb.Items.Count; i++)
            {
                String name = comb.Items[i].ToString();
                uint a = ParseIDFromValueComboBox(name);
                if (a == id)
                {
                    return name;
                }
            }
            return "";
        }
        //名前をパースして IDを返す.
        public static uint ParseIDFromValueComboBox(String name)
        {
            int sep=name.IndexOf('=');
            if (sep < 0)
            {
                return 0xFFFFFFFF;
            }
            return U.atoh(name.Substring(0, sep));
        }

        //検索
        public bool OnSearch(uint key)
        {
            return false;
        }

        public static void InitControl(Control f,ToolTipEx tooltip)
        {
            //全てのコントロールの列挙
            List<Control> controls = GetAllControls(f);
            //MODの読込
            InputFormRef.ApplyMod(f, controls, tooltip);
            //翻訳
            InputFormRef.ControlTranslate(f);
            //配色変更
            InputFormRef.ReColor(f);
        }

        static bool IsWriteButton(Control c)
        {
            if (c is Button)
            {
                if (c.Name.IndexOf("WriteButton") >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static void UpdateAllTextID(uint textid)
        {
            foreach (var pair in Forms)
            {
                Form f = pair.Value.Form;
                if (f.IsDisposed)
                {
                    continue;
                }

                //全てのコントロールの列挙
                List<Control> controls = GetAllControls(f);

                //既に黄色いボタンを消すわけにはいかないので、黄色がついていないボタンだけ得る.
                List<Button> buttonList = new List<Button>();
                foreach (Control c in controls)
                {
                    if (IsWriteButton(c))
                    {
                        Button b = (Button)c;
                        if (IsWriteButtonToYellow(b) == false)
                        {//黄色くない書き込みボタンなので記録
                            buttonList.Add(b);
                        }
                    }

                    if (!(c is NumericUpDown))
                    {
                        continue;
                    }
                    if (!RegexCache.IsMatch(c.Name, "[W|D][0-9]+"))
                    {
                        continue;
                    }

                    NumericUpDown nup = (NumericUpDown)c;
                    if (!nup.Hexadecimal)
                    {
                        continue;
                    }

                    uint value = (uint)nup.Value;
                    if (value != textid)
                    {
                        continue;
                    }
                    //TextIDの中身が変わったので再設定することで、関連付けられたイベントを発動させる
                    U.ForceUpdate(nup, nup.Value);
                }

                foreach (Button writeButton in buttonList)
                {
                    WriteButtonToYellow(writeButton, false);
                }
            }

        }

        static void NotifyChangePointer(uint oldaddr, uint newaddr)
        {
            oldaddr = U.toPointer(oldaddr);
            newaddr = U.toPointer(newaddr);

            foreach (var pair in Forms)
            {
                Form f = pair.Value.Form;
                if (f.IsDisposed)
                {
                    continue;
                }

                if (f is EventScriptForm)
                {
                    ((EventScriptForm)f).NotifyChangePointer(oldaddr, newaddr);
                    continue;
                }
                if (f is ProcsScriptForm)
                {
                    ((ProcsScriptForm)f).NotifyChangePointer(oldaddr, newaddr);
                    continue;
                }
                if (f is AIScriptForm)
                {
                    ((AIScriptForm)f).NotifyChangePointer(oldaddr, newaddr);
                    continue;
                }

                //全てのコントロールの列挙
                List<Control> controls = GetAllControls(f);

                //既に黄色いボタンを消すわけにはいかないので、黄色がついていないボタンだけ得る.
                List<Button> buttonList = new List<Button>();
                foreach (Control c in controls)
                {
                    if (IsWriteButton(c))
                    {
                        Button b = (Button)c;
                        if (IsWriteButtonToYellow(b) == false)
                        {//黄色くない書き込みボタンなので記録
                            buttonList.Add(b);
                        }
                    }

                    if (! (c is NumericUpDown))
                    {
                        continue;
                    }
                    if (!RegexCache.IsMatch(c.Name, "[D|P][0-9]+"))
                    {
                        continue;
                    }

                    NumericUpDown nup = (NumericUpDown)c;
                    if (! nup.Hexadecimal)
                    {
                        continue;
                    }
                    if (nup.Maximum <= 0xFFFF)
                    {
                        continue;
                    }

                    uint value = U.toPointer((uint)nup.Value);
                    if (value != oldaddr)
                    {
                        continue;
                    }

                    //古いポインタが見つかったので、新しいのに書き換える.
                    nup.Value = newaddr;
                }

                foreach (Button writeButton in buttonList)
                {
                    WriteButtonToYellow(writeButton, false);
                }
            }
        }

        //フォームを表示する.
        class FormSt
        {
            public Form Form; //フォーム 
            public ToolTipEx ToolTip; //ツールチップ なぜかC#ではフォームに関連付けされない.
        };
        static Dictionary<int, FormSt> Forms = new Dictionary<int, FormSt>();
        public static Form JumpFormLow<Type>()
        {
            //すでに作っているなら新規では作らない
            FormSt formst;
            int hashCode = typeof(Type).GetHashCode();
            if (!Forms.TryGetValue(hashCode, out formst) || formst.Form.IsDisposed)
            {
                formst = new FormSt();
                //new でフォームを作る.
                formst.Form = (Form)Activator.CreateInstance(typeof(Type));
                Forms[hashCode] = formst;

                //全てのコントロールの列挙
                List<Control> controls = GetAllControls(formst.Form);
                //ツールチップを作る
                formst.ToolTip = InputFormRef.NewTooltip(formst.Form,controls);
                //MODの読込
                InputFormRef.ApplyMod(formst.Form, controls, formst.ToolTip);
                //翻訳
                InputFormRef.ControlTranslate(formst.Form);
                //ショートカットキー
                InputFormRef.ReShortCutKey(formst.Form);
                //配色変更
                InputFormRef.ReColor(formst.Form);
                //フォームを作ったことを通知
                InputFormRef.ActiveFormEvent(formst.Form);
                //ROMファイル名をタイトルに表示
                InputFormRef.ShowROMFilename(formst.Form);
                //最大化禁止(C#のresizeはなんかおかしい)
                formst.Form.MaximizeBox = false;

                formst.Form.FormClosing += OnCloseingCheckWriteConfirmation;

                //フォームが閉じられたらリストから消す.
                formst.Form.FormClosed += (sender, e) =>
                {
                    formst.ToolTip.Dispose();
                    Forms.Remove(hashCode);
                };
            }
            return formst.Form;
        }
        public static void CloseForm<Type>()
        {
            FormSt formst;
            int hashCode = typeof(Type).GetHashCode();
            if (!Forms.TryGetValue(hashCode, out formst) 
                || formst.Form.IsDisposed
                )
            {//存在しない
                return;
            }
            formst.Form.Close();
        }
        public static Form GetForm<Type>()
        {
            FormSt formst;
            int hashCode = typeof(Type).GetHashCode();
            if (!Forms.TryGetValue(hashCode, out formst)
                || formst.Form.IsDisposed
                )
            {//存在しない
                return null;
            }
            return formst.Form;
        }

        public static void ReOpenForm<Type>()
        {
            FormSt formst;
            int hashCode = typeof(Type).GetHashCode();
            if (!Forms.TryGetValue(hashCode, out formst)
                || formst.Form.IsDisposed
                )
            {//存在しない
            }
            else
            {
                formst.Form.Close();
            }

            JumpForm<Type>();
        }

        static void OnCloseingCheckWriteConfirmation(object sender, FormClosingEventArgs e)
        {
            if (!(sender is Control))
            {
                return;
            }

            //全てのコントロールの列挙
            List<Control> controls = GetAllControls((Control)sender);
            if (IsChangeContents(controls))
            {
                DialogResult dr = R.ShowNoYes("書き込んでいないデータがありますが、フォームを閉じてもよろしいですか？");
                if (dr != System.Windows.Forms.DialogResult.Yes)
                {//ユーザキャンセル.
                    e.Cancel = true;
                }
            }
            return;
        }

        //変更ボタンは黄色になっているか?
        public static bool IsWriteButtonToYellow(Button button)
        {
            if (OptionForm.Color_Control_ForeColor() == OptionForm.Color_NotifyWrite_ForeColor() 
                && OptionForm.Color_Control_BackColor() == OptionForm.Color_NotifyWrite_BackColor()
            )
            {
                //設定で、変更カラーが同一色になっているので判別不能.
                return false;
            }

            if (button.ForeColor == OptionForm.Color_NotifyWrite_ForeColor()
                && button.BackColor == OptionForm.Color_NotifyWrite_BackColor())
            {
                //変更カラーが付いているボタンがあるぞ!
                return true;
            }

            return false;
        }

        //変更されているコンテンツがあるかどうか.
        public static bool IsChangeContents(List<Control> controls)
        {
            if (OptionForm.Color_Control_ForeColor() == OptionForm.Color_NotifyWrite_ForeColor() 
                && OptionForm.Color_Control_BackColor() == OptionForm.Color_NotifyWrite_BackColor()
            )
            {
                return false;
            }

            foreach (Control search_info in controls)
            {
                if (!(search_info is Button))
                {
                    continue;
                }

                if (search_info.Visible == false)
                {//非表示
                    continue;
                }

                if (search_info.ForeColor == OptionForm.Color_NotifyWrite_ForeColor()
                    && search_info.BackColor == OptionForm.Color_NotifyWrite_BackColor())
                {
                    //変更カラーが付いているボタンがあるぞ!
                    return true;
                }
            }
            return false;
        }

        public static Form JumpForm<Type>(uint selectedID = 0, string AddressListName = "AddressList",NumericUpDown injectionCallback = null)
        {
            //すでに作っているなら新規では作らない
            Form f = JumpFormLow<Type>();
            f.Show();

            ListBox addressList = null;
            if (selectedID != U.NOT_FOUND)
            {
                addressList = JumpFormInner(f, selectedID, AddressListName, injectionCallback);
            }
            if (f.WindowState == FormWindowState.Minimized)
            {//フォームが最小化されていたら元に戻す.
                f.WindowState = FormWindowState.Normal;
            }
            //アクティブにする.
            f.Activate();
            f.TopMost = true;
            f.TopMost = false;

            if (addressList != null)
            {
                if (injectionCallback != null)
                {
                    ShowUpperNotifyAnimation(f , addressList);
                }
                addressList.Focus();
            }
            return f;
        }

        static ListBox JumpFormInner(Form f, uint selectedID, string AddressListName, NumericUpDown injectionCallback)
        {
            List<Control> controls = GetAllControls(f);

            ListBox addressList = null;
            foreach (Control info in controls)
            {
                if (info.Name == AddressListName && (info is ListBoxEx))
                {
                    addressList = (ListBox)info;
                    break;
                }
            }

            if (addressList == null)
            {
                return null;
            }

            if (selectedID < addressList.Items.Count)
            {
                U.SelectedIndexSafety(addressList, selectedID);
            }
            else if (selectedID != U.NOT_FOUND)
            {
                uint pid = InputFormRef.AddrToSelect(addressList, selectedID);
                if (pid < addressList.Items.Count)
                {
                    U.SelectedIndexSafety(addressList, pid);
                }
            }

            if (injectionCallback != null)
            {
                if (IsPointerNumupDownControl(injectionCallback))
                {
                    MakeInjectionAddrCallback(f, addressList, injectionCallback);
                }
                else
                {
                    MakeInjectionCallback(f, addressList, injectionCallback);
                }
            }
            return addressList;
        }

        //ポインタを扱っているコントロールかどうか.
        static bool IsPointerNumupDownControl(NumericUpDown nmd)
        {
            string[] sp = nmd.Name.Split('_');
            for (int i = 0; i < sp.Length; i++)
            {
                string name = sp[i];
                if (name.Length <= 0)
                {
                    return false;
                }
                if (name[0] == 'P' && name[1] >= '0' && name[1] <= '9')
                {
                    return true;
                }
            }
            return false;
        }

        //ToolTipの取得.
        public static ToolTipEx GetToolTip<Type>()
        {
            FormSt formst;
            int hashCode = typeof(Type).GetHashCode();
            if (!Forms.TryGetValue(hashCode, out formst) || formst.Form.IsDisposed)
            {//ありえないが、ないらしい.
                Debug.Assert(false);
                return null;
            }
            return formst.ToolTip;
        }

        public void MakeAddressListExpandsCallback(EventHandler eventHandler)
        {
            this.AddressListExpandsEvent += eventHandler;
        }

        public static int GetCorrectionStartID(Form f)
        {
            if (f.Name == "UnitForm" || f.Name == "UnitFE7Form" || f.Name == "UnitFE6Form")
            {
                return 1;
            }
            return 0;
        }

        //adresslist doubleclickで、呼び出し元の injecttionCallbackへ値を代入. フォームを閉じる
        public static void MakeInjectionCallback(Form f,ListBox addressList,NumericUpDown injectionCallback)
        {
            if(injectionCallback == null)
            {
                return ;
            }

            Form srcForm = U.ControlToParentForm(injectionCallback);
            if(srcForm == null)
            {
                return ;
            }

            addressList.MouseDoubleClick += (sender,e) =>
            {
                int correctValue = GetCorrectionStartID(f);
                U.ForceUpdate(injectionCallback, (uint)addressList.SelectedIndex + correctValue);
                f.Close();
            };
        }
        //adresslist doubleclickで、呼び出し元の injecttionCallbackへ値を代入. フォームを閉じる
        public static void MakeInjectionAddrCallback(Form f, ListBox addressList, NumericUpDown injectionCallback)
        {
            if (injectionCallback == null)
            {
                return;
            }

            Form srcForm = U.ControlToParentForm(injectionCallback);
            if (srcForm == null)
            {
                return;
            }

            addressList.MouseDoubleClick += (sender, e) =>
            {
                uint addr = InputFormRef.SelectToAddr(addressList);
                addr = U.toPointer(addr);
                U.ForceUpdate(injectionCallback, addr);
                f.Close();
            };
        }

        //ボタンclickで、呼び出し元の injecttionCallbackへ値を代入. フォームを閉じる
        public static void MakeInjectionApplyButtonCallback(Form f, Button applyButton, NumericUpDown injectionCallback)
        {
            if (injectionCallback == null)
            {
                return;
            }

            Form srcForm = U.ControlToParentForm(injectionCallback);
            if (srcForm == null)
            {
                return;
            }

            applyButton.MouseClick += (sender, e) =>
            {
                uint value = (uint)applyButton.Tag;
                U.ForceUpdate(injectionCallback, value);
                f.Close();
            };
        }

        public static void MakeInjectionAddressListExpandsCallback(Form f, ListBox addressList, Button applyButton, NumericUpDown injectionCallback)
        {
            if (injectionCallback == null)
            {
                return;
            }

            Form srcForm = U.ControlToParentForm(injectionCallback);
            if (srcForm == null)
            {
                return;
            }

            applyButton.MouseClick += (sender, e) =>
            {
                uint addr = InputFormRef.SelectToAddr(addressList);
                U.ForceUpdate(injectionCallback, addr);
            };
        }


        //すべてのフォームを再描画する.
        public static void InvalidateALLForms()
        {
            foreach(var pair in Forms)
            {
                Form f = pair.Value.Form;
                ReColor(f);
                f.Refresh();
            }
        }



        //タイトルバーにROM名を出す.
        public static void ShowROMFilename(Form self)
        {
            if (Program.ROM == null)
            {
                return ;
            }
            self.Text += " " + Path.GetFileName(Program.ROM.Filename);
        }

        public static Type GetControll<Type>(Form self,Type not_found_def)
        {
            foreach (object o in self.Controls)
            {
                if (o.GetType() == typeof(Type))
                {//既に所持.
                    return (Type)o;
                }
            }
            return not_found_def;
        }

        //応答なしにならないようにメッセージループを回す
        public static bool DoEvents(Form self = null,string message = null)
        {
            if ( System.Threading.Thread.CurrentThread.IsBackground )
            {//スレッド処理 停止の確認
                return (Program.AsmMapFileAsmCache.IsStopFlagOn());
            }

            if (message == null)
            {
                //nop
            }
            else if (LastPleaseWaitStaticCache != null)
            {
                LastPleaseWaitStaticCache.Message(message);
            }
            else if (self != null)
            {
                NotifyPleaseWaitUserControl c = GetControll<NotifyPleaseWaitUserControl>(self, null);

                if (c != null)
                {//表示している.
                    c.Message(message);
                    LastPleaseWaitStaticCache = c;
                }
            }
            else
            {//ウィンドウがない状態
                return false;
            }
            //ループを回す
            Application.DoEvents();
            //停止は受け付けない!
            return false;
        }

        //しばらくお待ちください.
        static NotifyPleaseWaitUserControl LastPleaseWaitStaticCache = null;
        public static void ShowPleaseWait(Form self)
        {
            if (GetControll<NotifyPleaseWaitUserControl>(self, null) != null)
            {//既に表示中.
                return;
            }   

            NotifyPleaseWaitUserControl notifyControl = new NotifyPleaseWaitUserControl();
            InputFormRef.ControlTranslate(notifyControl);
            notifyControl.Location = new System.Drawing.Point
            (
              (self.Width - notifyControl.Width) / 2
            , (self.Height - notifyControl.Height) / 2
            );

            notifyControl.Hide();
            self.Controls.Add(notifyControl);
            notifyControl.BringToFront();
            notifyControl.Show();
            notifyControl.Update();

            LastPleaseWaitStaticCache = notifyControl;
        }
        public static void HidePleaseWait(Form self)
        {
            NotifyPleaseWaitUserControl c = GetControll<NotifyPleaseWaitUserControl>(self, null);

            if (c != null)
            {//表示しているなら消す
                self.Controls.Remove(c);
                LastPleaseWaitStaticCache = null;
            }
        }
        public static bool IsPleaseWaitDialog(Form self)
        {
            if (self == null)
            {
                self = LastActiveForm;
                if (self == null)
                {
                    return false;
                }
            }

            NotifyPleaseWaitUserControl c = GetControll<NotifyPleaseWaitUserControl>(self, null);
            return (c != null);
        }

        public class AutoPleaseWait : IDisposable
        {
            Form SelfForm;
            public AutoPleaseWait()
            {
                Init(null);
            }
            public AutoPleaseWait(Form self)
            {
                Init(self);
            }
            void Init(Form self)
            {
                if (self == null)
                {
                    self = LastActiveForm;
                }

                this.SelfForm = self;
                if (this.SelfForm == null)
                {
                    return;
                }
                InputFormRef.ShowPleaseWait(this.SelfForm);
            }
            public void Dispose()
            {
                if (this.SelfForm == null)
                {
                    return;
                }
                InputFormRef.HidePleaseWait(this.SelfForm);
            }
            public void DoEvents(string message = null)
            {
                if (this.SelfForm == null)
                {
                    U.echo(message);
                    return;
                }
                InputFormRef.DoEvents(this.SelfForm, message);
            }
        };

        public static void ShowWriteNotifyAnimation(Form self, uint addr)
        {
            int write_notify_time = OptionForm.write_notify_time();
            if (write_notify_time == 0)
            {//表示しない.
                return;
            }

            NotifyWriteUserControl c = GetControll<NotifyWriteUserControl>(self, null);

            if (c != null)
            {//既に表示中.
                return;
            }

            //二郎カラーの通知コントロール.
            NotifyWriteUserControl notifyControl = new NotifyWriteUserControl();
            InputFormRef.ControlTranslate(notifyControl);
            notifyControl.UpdateText(addr);
            notifyControl.ShowAnimation(self, write_notify_time);

            //デバッグメッセージ
            //Log.Notify(self.Name , self.Text , notifyControl.GetBigText());
        }

        //上に表示する通知メッセージアニメーション
        public static void ShowUpperNotifyAnimation(Form self,ListBox listbox)
        {
            int write_notify_time = OptionForm.notify_upper_time();
            if (write_notify_time == 0)
            {//表示しない.
                return;
            }

            NotifyDirectInjectionNotifyUserControl c = GetControll<NotifyDirectInjectionNotifyUserControl>(self, null);

            if (c != null)
            {//既に表示中.
                return;
            }

            //上に出す通知コントロール.
            NotifyDirectInjectionNotifyUserControl notifyControl = new NotifyDirectInjectionNotifyUserControl();
            InputFormRef.ControlTranslate(notifyControl);
            notifyControl.ShowAnimation(self, listbox , write_notify_time);
        }


        
        //ショートカットキーを設定します.
        public static void ReShortCutKey(Form self)
        {
            if (self.KeyPreview)
            {
                return ;
            }

            //コントロール上にあるキー操作もフォームが受け取られるようにする.
            self.KeyPreview = true;

            self.KeyDown += (sender, e) =>
            {
                if (Program.ROM == null)
                {
                    return;
                }

                for (int i = 0; i < Program.Config.ShortCutKeys.Length; i++)
                {
                    if ( e.KeyData != Program.Config.ShortCutKeys[i] )
                    {
                        continue;
                    }
                    //ショートカットマッチ.
                    switch (U.atoi(Program.Config.at("ShortCutValue" + (i+1).ToString(),"255")))
                    {
                        case 0://何もしない
                            return;
                        case 1: //1=エミュレーターで実行
                            MainFormUtil.RunAs("emulator");
                            break;
                        case 2: //2=エミュレーター2で実行
                            MainFormUtil.RunAs("emulator2");
                            break;
                        case 3: //3=バイナリエディタ
                            MainFormUtil.RunAs("binary_editor");
                            break;
                        case 4: //4=最後に利用したファイル
                            InputFormRef.JumpForm<OpenLastSelectedFileForm>();
                            break;
                        case 5: //5=Sappy
                            MainFormUtil.RunAs("sappy");
                            break;
                        case 6: //6=Program1
                            MainFormUtil.RunAs("program1");
                            break;
                        case 7: //7=Program2
                            MainFormUtil.RunAs("program2");
                            break;
                        case 8: //8=Program3
                            MainFormUtil.RunAs("program3");
                            break;
                        case 9: //9=UNDO表示
                            InputFormRef.JumpForm<ToolUndoForm>();
                            break;
                        case 10: //10=ログ
                            InputFormRef.JumpForm<LogForm>();
                            break;
                        case 11: //11=メインホームに移動
                            Form f = Program.MainForm();
                            f.Show();
                            f.Activate();
                            break;
                        case 12: //書き込みボタンを押す.
                            InputFormRef.ClickWriteButton(self);
                            break;
                        case 13: //閉じる.
                            CloseForm(self);
                            break;
                        case 14: //14=Lint
                            InputFormRef.JumpForm<ToolFELintForm>();
                            break;
                        case 15: //15=ROMを開く
                            MainFormUtil.Open(self);
                            break;
                        case 16: //16=ROMを上書き保存する
                            MainFormUtil.SaveOverraide(self);
                            break;
                        case 17: //17=ROMを別名で保存する
                            MainFormUtil.SaveAs(self);
                            break;
                        case 18: //18=Lintして問題なければ上書き保存する(この機能はSaveOverraideに統合されました)
                            MainFormUtil.SaveOverraide(self , forceCheck: true );
                            break;
                        case 19: //19=リストから次を検索
                            InputFormRef.SearchNextByListBox(self);
                            break;
                        case 20: //20=ユニット画面を開く
                            if (Program.ROM.RomInfo.version() == 6)
                            {
                                InputFormRef.JumpForm<UnitFE6Form>();
                            }
                            else if (Program.ROM.RomInfo.version() == 7)
                            {
                                InputFormRef.JumpForm<UnitFE7Form>();
                            }
                            else
                            {
                                InputFormRef.JumpForm<UnitForm>();
                            }
                            break;
                        case 21: //21=クラス画面を開く
                            if (Program.ROM.RomInfo.version() == 6)
                            {
                                InputFormRef.JumpForm<ClassFE6Form>();
                            }
                            else
                            {
                                InputFormRef.JumpForm<ClassForm>();
                            }
                            break;
                        case 22: //22=アイテム画面を開く
                            if (Program.ROM.RomInfo.version() == 6)
                            {
                                InputFormRef.JumpForm<ItemFE6Form>();
                            }
                            else
                            {
                                InputFormRef.JumpForm<ItemForm>();
                            }
                            break;
                        case 0xFF: //未定義
                            R.ShowStopError("ショートカットキーの設定がされていません。\r\nメニューの、設定→オプションから、ショートカットキーを定義してください。");
                            break;
                    }
                    e.Handled = true;
                    return;
                }
            };
        }

        static void SearchNextByListBox(Form self)
        {
            Control c = self.ActiveControl;
            if (c == null)
            {
                return;
            }

            TabPage tab = TabControlEx.ControlToParentTabExPage(c);
            if (tab == null)
            {//タブはないので、普通にフォーム全域を探索する.
                SearchNextByListBoxBase(self, self);
            }
            else
            {//タブに取り込まれている中を探索する必要がある
                SearchNextByListBoxBase(self, tab);
            }

            //見つからない.
            return;
        }

        static bool SearchNextByListBoxBase(Form self, Control baseControl)
        {
            Control c = self.ActiveControl;
            if (c == null)
            {
                return false;
            }
            if (c is ListBoxEx)
            {
                ((ListBoxEx)c).SearchNext();
                return true;
            }
            if (c is SearchHelperControl)
            {
                ((SearchHelperControl)c).SearchNext();
                return true;
            }

            string name = c.Name;
            string prefix = "";

            string[] sp = name.Split('_');
            if (sp.Length >= 2)
            {//複数のものがある場合、先頭のものをprefixとして扱う.
                prefix = sp[0] + "_";
            }

            //リストボックスを検索.
            List<Control> controls = GetAllControls(baseControl);

//            //フロートコントロールパネルがあったらそちらを優先して探索する.
//            if (prefix == "")
//            {
//                Control controlPanelF = FindObject(prefix, controls, "ControlPanel");
//                if (controlPanelF != null)
//                {
//                    if (ClickWriteButtonF(self, controlPanelF))
//                    {
//                        return true;
//                    }
//                }
//            }
            bool r = SearchNextByListBoxBaseListBox(prefix , controls,self, baseControl);
            if (!r)
            {
                return true;
            }
            r = SearchNextByListBoxBaseNextSearchButton(prefix, controls, self, baseControl);
            if (!r)
            {
                return true;
            }
            return false;
        }
        static bool SearchNextByListBoxBaseListBox(string prefix 
            , List<Control> controls 
            , Form self
            , Control baseControl
            )
        {
            //リストボックスを探索
            Control addressList = FindObject(prefix, controls, "AddressList");
            if (addressList != null && addressList is ListBoxEx)
            {
                ((ListBoxEx)addressList).SearchNext();
                return true;
            }

            if (prefix == "")
            {
                return true;
            }

            //prefixなしで検索してみる.
            prefix = "";
            addressList = FindObject(prefix, controls, "AddressList");
            if (addressList != null && addressList is ListBoxEx)
            {
                ((ListBoxEx)addressList).SearchNext();
                return true;
            }

            //見つからない.
            return false;
        }
        static bool SearchNextByListBoxBaseNextSearchButton(string prefix
            , List<Control> controls
            , Form self
            , Control baseControl
            )
        {
            //検索ボタンを探索
            Control button = FindObject(prefix, controls, "SearchButton");
            if (button != null && button is Button)
            {
                ((Button)button).PerformClick();
                return true;
            }

            if (prefix == "")
            {
                return true;
            }

            //prefixなしで検索してみる.
            prefix = "";
            button = FindObject(prefix, controls, "SearchButton");
            if (button != null && button is Button)
            {
                ((Button)button).PerformClick();
                return true;
            }

            //見つからない.
            return false;
        }

        static void CloseForm(Form self)
        {
            List<Control> controls = GetAllControls(self);
            foreach (Control search_info in controls)
            {
                if (search_info is TabControlEx)
                {//タブコントロールを発見。奴に閉じる命令を送って終わり.
                    ((TabControlEx)search_info).CloseTab(-1);
                    return;
                }
            }

            //タブがないので、フォームに閉じる命令を送って終了
            self.Close();
        }
        
        static void ClickWriteButton(Form self)
        {
            Control c = self.ActiveControl;
            if (c == null)
            {
                return;
            }
            TabPage tab = TabControlEx.ControlToParentTabExPage(c);
            if (tab == null)
            {//タブはないので、普通にフォーム全域を探索する.
                ClickWriteButtonBase(self, self);
            }
            else
            {//タブに取り込まれている中を探索する必要がある
                ClickWriteButtonBase(self, tab);
            }

            //見つからない.
            return;
        }

        static bool ClickWriteButtonBase(Form self,Control baseControl)
        {
            Control c = self.ActiveControl;
            if (c == null)
            {
                return false;
            }

            string name = c.Name;
            string prefix = "";

            string[] sp = name.Split('_');
            if (sp.Length >= 2)
            {//複数のものがある場合、先頭のものをprefixとして扱う.
                prefix = sp[0] + "_";
            }

            //書き込みボタンを検索.
            List<Control> controls = GetAllControls(baseControl);

            //フロートコントロールパネルがあったらそちらを優先して探索する.
            if (prefix == "")
            {
                Control controlPanelF = FindObject(prefix, controls, "ControlPanel");
                if (controlPanelF != null)
                {
                    if (ClickWriteButtonF(self, controlPanelF))
                    {
                        return true;
                    }
                }
            }

            //書き込みボタン探索
            Control writeButton = FindObject(prefix, controls, "WriteButton");
            if (writeButton != null && writeButton is Button)
            {
                ((Button)writeButton).PerformClick();
                return true;
            }
            writeButton = FindObject(prefix, controls, "AllWriteButton");
            if (writeButton != null && writeButton is Button)
            {
                ((Button)writeButton).PerformClick();
                return true;
            }
            writeButton = FindObject(prefix, controls, "PaletteWriteButton");
            if (writeButton != null && writeButton is Button)
            {
                ((Button)writeButton).PerformClick();
                return true;
            }
            writeButton = FindObject(prefix, controls, "WriteTextButton");
            if (writeButton != null && writeButton is Button)
            {
                ((Button)writeButton).PerformClick();
                return true;
            }

            if (prefix == "")
            {
                return true;
            }

            //prefixなしで検索してみる.
            prefix = "";
            writeButton = FindObject(prefix, controls, "WriteButton");
            if (writeButton != null && writeButton is Button)
            {
                ((Button)writeButton).PerformClick();
                return true;
            }
            writeButton = FindObject(prefix, controls, "AllWriteButton");
            if (writeButton != null && writeButton is Button)
            {
                ((Button)writeButton).PerformClick();
                return true;
            }
            writeButton = FindObject(prefix, controls, "PaletteWriteButton");
            if (writeButton != null && writeButton is Button)
            {
                ((Button)writeButton).PerformClick();
                return true;
            }
            writeButton = FindObject(prefix, controls, "WriteTextButton");
            if (writeButton != null && writeButton is Button)
            {
                ((Button)writeButton).PerformClick();
                return true;
            }

            //見つからない.
            return false;
        }

        //フロートコントロールパネルで更新と新規追加のボタンが点灯していたら、そちらを押す.
        static bool ClickWriteButtonF(Form self,Control controlPanelF)
        {
            if (controlPanelF.Visible == false)
            {//非表示
                return false;
            }

            List<Control> controlPanelsControlList = GetAllControls(controlPanelF);
            Control updateButton = FindObject("", controlPanelsControlList, "UpdateButton");
            Control newButton = FindObject("", controlPanelsControlList, "NewButton");
            if (!(updateButton != null && updateButton is Button))
            {
                return false;
            }
            if (!(newButton != null && newButton is Button))
            {
                return false;
            }
            Button updateButtonObj = (Button)updateButton;
            if (updateButtonObj.Visible == false)
            {
                return false;
            }
            Button newButtonObj = (Button)newButton;
            if (newButtonObj.Visible == false)
            {
                return false;
            }

            //どっちのボタンを押すの?
            ToolClickWriteFloatControlPanelButtonForm f = (ToolClickWriteFloatControlPanelButtonForm)InputFormRef.JumpFormLow<ToolClickWriteFloatControlPanelButtonForm>();
            DialogResult dr = f.ShowDialog();

            if (dr == DialogResult.OK)
            {//Update
                updateButtonObj.PerformClick();
                return true;
            }
            if (dr == DialogResult.Yes)
            {//Insert
                newButtonObj.PerformClick();
                return true;
            }
            //拒否された
            return true;
        }

        static bool isErrorColorData(Control self)
        {
            if (self.Name.IndexOf("ERROR_") == 0)
            {
                return true;
            }
            return false;
        }

        //コントロールの色を設定どおりにカスタムします.
        public static void ReColor(Control self)
        {
            for (int i = 0; i < self.Controls.Count; i++)
            {
                Control c = self.Controls[i];
                ReColor(c);
            }
            if (self is NumericUpDown)
            {
                NumericUpDown info_object = ((NumericUpDown)self);
                bool isReadOnly = info_object.ReadOnly;
                if (info_object.Hexadecimal)
                {//16進数
                    info_object.BackColor = OptionForm.Color_Input_BackColor();
                    info_object.ForeColor = OptionForm.Color_Input_ForeColor();
                }
                else
                {//10進数
                    info_object.BackColor = OptionForm.Color_InputDecimal_BackColor();
                    info_object.ForeColor = OptionForm.Color_InputDecimal_ForeColor();
                }
                if (isReadOnly)
                {
                    info_object.Increment = 0;
                    info_object.TabStop = false;
                    info_object.BackColor = OptionForm.Color_Control_BackColor();
                }
                info_object.ImeMode = ImeMode.Disable;

                //フォーカスを持った時に全文選択状態にする.
                info_object.Enter += (sender, e) =>
                {
                    info_object.Select(0, info_object.Text.Length);
                };
            }
            else if (isErrorColorData(self))
            {
                self.BackColor = OptionForm.Color_Error_BackColor();
                self.ForeColor = OptionForm.Color_Error_ForeColor();
                self.Visible = false; //ディフォルト非表示
            }
            else if (self is ListBox
                || self is TextBox
                || self is RichTextBox
                || self is TextBoxEx
                || self is RichTextBoxEx
                || self is ComboBox)
            {
                if (self.BackColor == OptionForm.Color_InputDecimal_BackColor()
                 && self.ForeColor == OptionForm.Color_InputDecimal_ForeColor())
                {

                }
                else
                {
                    self.BackColor = OptionForm.Color_Input_BackColor();
                    self.ForeColor = OptionForm.Color_Input_ForeColor();
                }
            }
            else
            {
                self.BackColor = OptionForm.Color_Control_BackColor();
                self.ForeColor = OptionForm.Color_Control_ForeColor();
            }

        }


        public static void RecolorMenuStrip(MenuStrip menu)
        {
            if (menu == null)
            {
                return;
            }
            Color Color_Control_BackColor = OptionForm.Color_Control_BackColor();
            Color Color_Control_ForeColor = OptionForm.Color_Control_ForeColor();
            for (int i = 0; i < menu.Items.Count; i++)
            {
                RecolorMenuStrip((ToolStripMenuItem)menu.Items[i], Color_Control_BackColor, Color_Control_ForeColor);

            }
        }

        //メニュー内カラー
        static void RecolorMenuStrip(ToolStripMenuItem menu
            ,Color Color_Control_BackColor
            ,Color Color_Control_ForeColor)
        {
            menu.BackColor = Color_Control_BackColor;
            menu.ForeColor = Color_Control_ForeColor;

            for (int i = 0; i < menu.DropDownItems.Count; i++)
            {
                ToolStripItem item = menu.DropDownItems[i];
                if ((item is ToolStripSeparatorEx))
                {
                    ToolStripSeparatorEx sep = (ToolStripSeparatorEx)item;
                    sep.BackColor = Color_Control_BackColor;
                    sep.ForeColor = Color_Control_ForeColor;
                }
                else if ((item is ToolStripMenuItem))
                {
                    ToolStripMenuItem strip = (ToolStripMenuItem)item;
                    RecolorMenuStrip(strip, Color_Control_BackColor, Color_Control_ForeColor);
                }
            }
        }

        //不明Exceptionが発生した時に、forms.ActiveForm は nullになるらしいので独自に記録する.
        static Form LastActiveForm = null;
        public static string GetDebugInfo()
        {
            if (LastActiveForm == null)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("LastForm_Name:" + LastActiveForm.Name + " @Text:" + LastActiveForm.Text );

            Control c = LastActiveForm.ActiveControl;
            if (c != null)
            {
                sb.AppendLine("LastControl_Name:" + c.Name + " @Text:" + c.Text);
            }
            List<Control> controls = GetAllControls(LastActiveForm);
            for (int i = 0; i < controls.Count; i++)
            {
                c = controls[i];
                if (c.Visible == false)
                {
                    continue;
                }
                if (c is ListBoxEx)
                {
                    sb.AppendLine(c.Name + ":" + c.Text + " :(" + ((ListBox)c).SelectedIndex +  ")");
                }
                else if (c.Name.IndexOf("BlockSize") >= 0
                    || c.Name.IndexOf("SelectAddress") >= 0
                    || c.Name.IndexOf("Address") >= 0
                    || c.Name.IndexOf("FilterComboBox") >= 0
                    || c.Name.IndexOf("ReadCount") >= 0
                    || c.Name.IndexOf("ReadStartAddress") >= 0
                    || c.Name.IndexOf("UNIONTAB") >= 0
                    || c.Name.IndexOf("UNIONKEY") >= 0
                    || c.Name.IndexOf("Pointer") >= 0
                    || c.Name.IndexOf("CLASS_LISTBOX") >= 0
                    || c.Name.IndexOf("ParamSrc") >= 0
                    || c.Name.IndexOf("ScriptCodeName") >= 0
                    || c.Name.IndexOf("DataCount") >= 0
                    )
                {
                    sb.AppendLine(c.Name + ":" + c.Text);
                }
                else if (c.Name == "TextArea")
                {//Text編集
                    sb.AppendLine(c.Name + ":" + c.Text);
                }
                else if (c is NotifyPleaseWaitUserControl)
                {
                    sb.AppendLine(c.Name + ":" + ((NotifyPleaseWaitUserControl)c).GetMessage() );
                }
            }

            return sb.ToString();
        }
        public static Form GetLastActiveForm()
        {
            return LastActiveForm;
        }


        public static void ActiveFormEvent(Form self)
        {
            self.Activated += (sender, e) =>
            {
                if (Program.UpdateWatcher != null)
                {
                    Program.UpdateWatcher.CheckALL();
                }
                LastActiveForm = self;
            };
        }

        //テキストデータ 連続するデータの書き込み
        public static uint WriteTextData(
              uint baseaddress
            , uint datacount
            , uint writeindex
            , uint data_start, uint data_end
            , byte[] dataByte
            , bool raiseUnHuffman
            , Func<uint, bool,MoveToUnuseSpace.ADDR_AND_LENGTH> get_data_pos_callback //データサイズを求める.
            , Undo.UndoData undodata
        )
        {
            uint write_pointer = baseaddress + (4 * writeindex);
            if (U.isSafetyOffset(write_pointer) == false)
            {
                return U.NOT_FOUND;
            }
            uint write_addr = Program.ROM.u32(write_pointer);

            bool useUnHuffmanPatch = false;
            if (FETextEncode.IsUnHuffmanPatchPointer(write_addr))
            {//文字列なので、海外改造によくある unHuffman patchの可能性を見る.
                useUnHuffmanPatch = true;
                write_addr = FETextEncode.ConvertUnHuffmanPatchToPointer(write_addr);
            }
            write_addr = U.toOffset(write_addr);

            //オリジナルのサイズを求めます.
            uint original_size = MoveToUnuseSpace.OriginalDataSize(
                 baseaddress
                , baseaddress + (datacount * 4)
                , data_start
                , data_end
                , writeindex
                , get_data_pos_callback
                );
            if (original_size >= 20000)//20000 byte over
            {//長すぎる.
                R.Error("この領域({0})は、余りに長すぎます({1} bytes)。おそらくデータが壊れている。よって、再利用しません", U.To0xHexString(write_addr), original_size);
                original_size = 0;
            }
            else if (write_addr + original_size > Program.ROM.Data.Length)
            {//実データより長すぎる
                R.Error("この領域({0})は、実データより長い({1} bytes)と判定された。実データのサイズに合わせる", U.To0xHexString(write_addr), original_size);
                original_size = (uint)Program.ROM.Data.Length - write_addr;
            }


            Debug.Assert(original_size < 0x100000);
            if (dataByte.Length <= original_size)
            {//オリジナルより小さいなら書き換え可能.
                undodata.list.Add(new Undo.UndoPostion(write_addr, original_size));

                //文字列データの書き込み.
                Program.ROM.write_range(write_addr, dataByte);
                //余剰領域は0x00クリア
                byte[] zerofill = U.FillArray(original_size - (uint)dataByte.Length, 0x0);
                Program.ROM.write_range(write_addr + (uint)dataByte.Length, zerofill);


                if (raiseUnHuffman)
                {//un-Huffman化する.
                    Program.ROM.write_u32(write_pointer, FETextEncode.ConvertPointerToUnHuffmanPatchPointer(U.toPointer(write_addr)));
                }

                return write_addr;
            }
/*
#if DEBUG
            //空き領域を詰めた場合どれだけ入るのか求める.
            uint max_size = MoveToUnuseSpace.CheckMoveToUnuseSpace(
                    baseaddress
                , baseaddress + (datacount * this.BlockSize)
                , data_start
                , data_end
                , writeindex
                , get_data_pos_callback
                );
            if (dataByte.Length < max_size + original_size)
            {//空き領域を詰めたら入るらしいので、空き領域を詰める
                write_pointer = MoveToUnuseSpace.RunMoveToUnuseSpace(
                      baseaddress
                    , baseaddress + (datacount * this.BlockSize)
                    , data_start
                    , data_end
                    , writeindex
                    , get_data_pos_callback
                    , undodata.list
                    );
                if (write_pointer == U.NOT_FOUND)
                {
                    return U.NOT_FOUND;
                }

                //文字列の書き込み
                write_addr = Program.ROM.p32(write_pointer);
                Program.ROM.write_range(write_addr, dataByte,undodata);

                ReloadScreen();
                ShowWriteNotifyAnimation(this.SelfForm, write_addr);
                return write_addr;
            }
#endif
*/
            //空き領域もないので、0x09000000 以降の拡張領域を提案

            //拡張領域から探す気はアライメントを 4 にして探します.
            //さらに、4バイトの余白がないと、ダメなケースがあるらしい???
            uint searchFreespaceSize = U.Padding4((uint)dataByte.Length) + 4;

            //拡張領域に移動.
            uint freespace = MoveToFreeSapceForm.SearchFreeSpaceOne(searchFreespaceSize, isProgramArea: false);
            if (freespace == U.NOT_FOUND)
            {
                //空き領域が本当何もない!
                R.ShowStopError("空き領域がもうありません。");
                return U.NOT_FOUND;
            }
            if (OptionForm.rom_extends() == OptionForm.rom_extends_enum.NO && freespace >= 0x01000000)
            {//0x09000000以降のROM拡張を設定で拒否
                R.ShowStopError("空き領域がもうありません。");
                return U.NOT_FOUND;
            }

            //元データの保存
            undodata.list.Add(new Undo.UndoPostion(write_pointer, 4));
            undodata.list.Add(new Undo.UndoPostion(write_addr, original_size));
            if (original_size > 0)
            {
                //元データをクリアします.
                byte[] fill = U.FillArray(original_size, 0x00);
                Program.ROM.write_range(write_addr, fill);
            }

            //新規サイズの書き込み.
            uint newFreeSapceAddr = freespace;
            if (useUnHuffmanPatch || raiseUnHuffman)
            {
                Program.ROM.write_u32(write_pointer, FETextEncode.ConvertPointerToUnHuffmanPatchPointer(U.toPointer(newFreeSapceAddr)));
            }
            else
            {
                Program.ROM.write_p32(write_pointer, newFreeSapceAddr);
            }

            if (newFreeSapceAddr + searchFreespaceSize > Program.ROM.Data.Length)
            {//必要サイズがROMサイズを超えていたら増設する.
                bool isResizeSuccess = Program.ROM.write_resize_data((uint)(newFreeSapceAddr + searchFreespaceSize));
                if (isResizeSuccess == false)
                {
                    return U.NOT_FOUND;
                }
            }

            //文字列の書き込み
            Program.ROM.write_range(newFreeSapceAddr, dataByte);

            return newFreeSapceAddr;
        }

        //ポインタの先を書き換える.
        public static uint WriteBinaryDataPointer(Form self
            , uint pointer
            , byte[] dataByte
            , Func<uint, MoveToUnuseSpace.ADDR_AND_LENGTH> get_data_pos_callback //データサイズを求める.
            , Undo.UndoData undodata
            )
        {
            uint addr = U.toOffset(pointer);
            if (U.isSafetyOffset(addr) == false)
            {
                return U.NOT_FOUND;
            }

            uint dataaddr = Program.ROM.p32(addr);

            //書き込み
            uint newaddr = WriteBinaryData(self, dataaddr, dataByte, get_data_pos_callback, undodata);
            if (newaddr == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }

            //ポインタ先も書き込み
            Program.ROM.write_p32(addr, newaddr, undodata);

            return newaddr;
        }

        //連続するデータの書き込み
        public static uint WriteBinaryData(Form self
            , uint addr
            , byte[] dataByte
            , Func<uint,MoveToUnuseSpace.ADDR_AND_LENGTH> get_data_pos_callback //データサイズを求める.
            , Undo.UndoData undodata
        )
        {
            uint write_addr = U.toOffset(addr);

            uint original_size = 0;
            if ( U.isSafetyOffset(write_addr) == false)
            {//無効なアドレスが指定された
                if (write_addr == 0)
                {//0ならば新設するか.
                    return AppendBinaryData(dataByte, undodata);
                }
                Debug.Assert(false);
                R.ShowStopError("危険なアドレス({0})が指定されました。\r\n",U.To0xHexString(write_addr));
                return U.NOT_FOUND;
            }

            //オリジナルのサイズを求めます.
            MoveToUnuseSpace.ADDR_AND_LENGTH aal = get_data_pos_callback(write_addr);
            original_size = aal.length;
            if (aal.length >= 0x00200000)
            {//長すぎる.
                R.Error("この領域({0})は、余りに長すぎます({1} bytes)。おそらくデータが壊れている。よって、再利用しません",U.To0xHexString(aal.addr),aal.length);
                original_size = 0;
            }
            else if (aal.addr >= Program.ROM.Data.Length)
            {//アドレスが危険
                R.ShowStopError("危険なアドレス({0})が指定されました。\r\n", U.To0xHexString(aal.addr));
                return U.NOT_FOUND;
            }
            else if (aal.addr + aal.length > Program.ROM.Data.Length)
            {//実データより長すぎる
                R.Error("この領域({0})は、実データより長い({1} bytes)と判定された。実データのサイズに合わせる", U.To0xHexString(aal.addr), aal.length);
                original_size = (uint)Program.ROM.Data.Length - aal.addr;
            }

            if (dataByte.Length <= original_size)
            {//オリジナルより小さいなら書き換え可能.
                undodata.list.Add(new Undo.UndoPostion(write_addr, original_size));

                Program.ROM.write_range(write_addr, dataByte);
                //余剰領域は0x00クリア
                byte[] zerofill = U.FillArray(original_size - (uint)dataByte.Length, 0x0);
                Program.ROM.write_range(write_addr + (uint)dataByte.Length, zerofill);

                return addr;
            }
            //自分がROM末尾であった場合
            if ( U.Padding4(addr + original_size)  == Program.ROM.Data.Length)
            {
                //ROMサイズを増設.
                bool isResizeSuccess = Program.ROM.write_resize_data(U.Padding4(addr + (uint)dataByte.Length));
                if (isResizeSuccess == false)
                {
                    return U.NOT_FOUND;
                }

                Program.ROM.write_range(addr, dataByte, undodata);
                return addr;
            }

            //空き領域もないので、0x09000000 以降の拡張領域を提案

            //拡張領域から探す気はアライメントを 4 にして探します.
            uint searchFreespaceSize = U.Padding4((uint)dataByte.Length) ;

            //拡張領域に移動.
            uint freespace = MoveToFreeSapceForm.SearchFreeSpaceOne(searchFreespaceSize, isProgramArea: false);
            if (freespace == U.NOT_FOUND)
            {
                //空き領域が本当何もない!
                R.ShowStopError("空き領域がもうありません。");
                return U.NOT_FOUND;
            }
            if (OptionForm.rom_extends() == OptionForm.rom_extends_enum.NO && freespace >= 0x01000000)
            {//0x09000000以降のROM拡張を設定で拒否
                R.ShowStopError("空き領域がもうありません。");
                return U.NOT_FOUND;
            }

            //元データの保存
            undodata.list.Add(new Undo.UndoPostion(write_addr, original_size));

            //新規サイズ
            uint newFreeSapceAddr = freespace;
            if (newFreeSapceAddr + searchFreespaceSize > Program.ROM.Data.Length)
            {//必要サイズがROMサイズを超えていたら増設する.
                bool isResizeSuccess = Program.ROM.write_resize_data((uint)(newFreeSapceAddr + searchFreespaceSize));
                if (isResizeSuccess == false)
                {
                    return U.NOT_FOUND;
                }
            }

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(self))
            {
                //LDRとEVEVNT
                //影響を受けるポインタサーチ
                List<uint> movepointerlist = MoveToFreeSapceForm.SearchPointer(write_addr);

                //影響を受けるポインタの書き換え.
                for (int i = 0; i < movepointerlist.Count; i++)
                {
                    InputFormRef.DoEvents(null, "PointerCount:" + i);

                    Program.ROM.write_u32(movepointerlist[i], U.toPointer(newFreeSapceAddr), undodata);
                }
            }

            //コメントやLintなどの付属データの書き換え
            MoveToFreeSapceForm.RepointEtcData(write_addr, original_size, newFreeSapceAddr);

            //元データをクリアします.
            byte[] fill = U.FillArray(original_size, 0x00);
            Program.ROM.write_range(write_addr, fill);

            //データの書き込み
            Program.ROM.write_range(newFreeSapceAddr, dataByte);
            //拡張されたことを通知する
            InputFormRef.NotifyChangePointer(addr, newFreeSapceAddr);

            return newFreeSapceAddr;
        }

        //拡張する必要があるか.
        public bool IsNeedExpandsArea(uint newdatacount)
        {
            uint newdatasize = (newdatacount + 1);
            uint olddatasize = this.DataCount;

            if (newdatasize <= olddatasize)
            {//拡張する必要がない.
                return false;
            }
            return true;
        }

        //だまって拡張
        public uint ExpandsArea(Form form, uint newdatacount, Undo.UndoData undodata, uint basepointer)
        {
            if (basepointer == 0 ||  basepointer == U.NOT_FOUND)
            {//拡張できません.
                return U.NOT_FOUND;
            }

            uint newdatasize = (newdatacount + 1);
            uint olddatasize = this.DataCount;

            if (newdatasize <= olddatasize + 2)
            {//拡張する必要がない.(現在のベースアドレスを返す.)
                return this.BaseAddress;
            }

            uint newaddr = InputFormRef.ExpandsArea(form
                , newdatasize
                , basepointer
                , olddatasize
                , FEBuilderGBA.InputFormRef.ExpandsFillOption.FIRST
                , this.BlockSize
                , undodata);
            return newaddr;
        }

        public enum ExpandsFillOption
        {
             NO
            ,FIRST
        };

        public static uint ExpandsArea(Form self
            ,uint new_count
            ,uint original_pointer
            ,uint original_count
            ,ExpandsFillOption fillOption
            ,uint block_size
            ,Undo.UndoData undodata
            )
        {
            Debug.Assert(block_size >= 1);
            if (new_count <= original_count)
            {//拡張する必要がない.
                Debug.Assert(false);
                return U.NOT_FOUND;
            }

            uint new_size = new_count * block_size;
            uint original_size = original_count * block_size;

            //拡張領域から探す時はアライメントを 4 にして探します.
            uint searchFreespaceSize = U.Padding4(new_size);
            bool use_ffffffff_term_data = false;
            if (original_count <= 0 && new_size >= 1024)
            {//オリジナルサイズが0で、ある程度の大きさのデータを確保するときは、
                //広大な空きデータと間違われないために、明かな終端フラグ 0xFFFFFFFFを入れよう.
                use_ffffffff_term_data = true;
                searchFreespaceSize += 4;
            }

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(self))
            {
                //拡張領域に移動.
                uint freespace = MoveToFreeSapceForm.SearchFreeSpaceOne(searchFreespaceSize, isProgramArea: false);
                if (freespace == U.NOT_FOUND)
                {
                    //空き領域が本当何もない!
                    R.ShowStopError("空き領域がもうありません。");
                    return U.NOT_FOUND;
                }
                if (OptionForm.rom_extends() == OptionForm.rom_extends_enum.NO && freespace >= 0x01000000)
                {//0x09000000以降のROM拡張を設定で拒否
                    R.ShowStopError("空き領域がもうありません。");
                    return U.NOT_FOUND;
                }

                uint orignal_addr = Program.ROM.p32(original_pointer);

                //元データの保存
                byte[] orignal_data = Program.ROM.getBinaryData(orignal_addr, original_size);
                undodata.list.Add(new Undo.UndoPostion(orignal_addr, original_size));

                //新規サイズ
                uint newFreeSapceAddr = freespace;
                if (newFreeSapceAddr + searchFreespaceSize > Program.ROM.Data.Length)
                {//必要サイズがROMサイズを超えていたら増設する.
                    bool isResizeSuccess = Program.ROM.write_resize_data((uint)(newFreeSapceAddr + searchFreespaceSize));
                    if (isResizeSuccess == false)
                    {
                        return U.NOT_FOUND;
                    }
                }
                if (newFreeSapceAddr + original_size > Program.ROM.Data.Length)
                {//必要サイズがROMサイズを超えていたら増設する.
                    bool isResizeSuccess = Program.ROM.write_resize_data((uint)(newFreeSapceAddr + original_size));
                    if (isResizeSuccess == false)
                    {
                        return U.NOT_FOUND;
                    }
                }

                //LDRとEVEVNT
                //影響を受けるポインタサーチ
                List<uint> movepointerlist = MoveToFreeSapceForm.SearchPointer(orignal_addr);

                if (movepointerlist.IndexOf(original_pointer) < 0)
                {//元ポインターの書き換えが抜けているので追加する.
                    movepointerlist.Add(original_pointer);
                }

                //影響を受けるポインタの書き換え.
                for (int i = 0; i < movepointerlist.Count; i++)
                {
                    Program.ROM.write_u32(movepointerlist[i], U.toPointer(newFreeSapceAddr), undodata);
                }

                //データを新規領域にコピー.
                Program.ROM.write_range(newFreeSapceAddr, orignal_data, undodata);

                if (fillOption == ExpandsFillOption.FIRST && new_size >= block_size)
                {
                    //増やした部分は最初のデータで埋める.
                    byte[] first_data = Program.ROM.getBinaryData(orignal_addr, block_size);
                    new_size -= block_size; //末尾は埋めてはいけない.
                    for (uint start = original_size; start < new_size; start += block_size)
                    {
                        Program.ROM.write_range(newFreeSapceAddr + start, first_data, undodata);
                    }
                }

                //コメントやLintなどの付属データの書き換え
                MoveToFreeSapceForm.RepointEtcData(orignal_addr, original_size, newFreeSapceAddr);

                //元データをクリアします.
                byte[] fill = U.FillArray(original_size, 0x00);
                Program.ROM.write_range(orignal_addr, fill, undodata);

                //オリジナルサイズが0で、ある程度の大きさのデータを確保するときは、
                //広大な空きデータと間違われないために、明かな終端フラグ 0xFFFFFFFFを入れよう.
                if (use_ffffffff_term_data)
                {
                    Program.ROM.write_u32(newFreeSapceAddr + new_size, 0xFFFFFFFF , undodata);
                }

                return newFreeSapceAddr;
            }

        }

        //末尾に新規データを作成する.
        public static uint AppendBinaryData(
              byte[] dataByte
            , Undo.UndoData undodata
        )
        {
            if (OptionForm.rom_extends() == OptionForm.rom_extends_enum.NO)
            {//ROM拡張を拒否
                R.ShowStopError("空き領域がもうありません。");
                return U.NOT_FOUND;
            }

            //拡張領域から探すときは、ファイル終端に備えて、アライメントを考えて、ちょい大目に探さないといけない.
            uint searchFreespaceSize = U.Padding4((uint)dataByte.Length) ;

            bool use_null_term_data = false;
            if (dataByte.Length >= 4 && U.u32(dataByte, (uint)dataByte.Length - 4) == 0)
            {//終端がNULLなデータ
                use_null_term_data = true;
            }
            
            //拡張領域に移動.
            uint freespace = MoveToFreeSapceForm.SearchFreeSpaceOne(searchFreespaceSize, isProgramArea: false);
            if (freespace == U.NOT_FOUND)
            {
                //空き領域が本当何もない!
                R.ShowStopError("空き領域がもうありません。");
                return U.NOT_FOUND;
            }
            //新規サイズ
            uint newFreeSapceAddr = freespace;
            if (newFreeSapceAddr + searchFreespaceSize > Program.ROM.Data.Length)
            {//必要サイズがROMサイズを超えていたら増設する.
                bool isResizeSuccess = Program.ROM.write_resize_data((uint)(newFreeSapceAddr + searchFreespaceSize));
                if (isResizeSuccess == false)
                {
                    return U.NOT_FOUND;
                }

                //終端だったら気にしない.
                use_null_term_data = false;
            }

            //文字列の書き込み
            Program.ROM.write_range(newFreeSapceAddr, dataByte, undodata);

            //データの終端がnullの場合、誤読を避けるために、termデータ0xFFFFFFを入れるべきかどうか.
            if (use_null_term_data)
            {
                if (IsAppendTermData(newFreeSapceAddr, searchFreespaceSize))
                {//TERMデータを入れるべき
                    Program.ROM.write_u32(newFreeSapceAddr + searchFreespaceSize + 4, 0xFFFFFFFF, undodata);
                }
            }

            return newFreeSapceAddr;
        }

        static bool IsAppendTermData(uint newFreeSapceAddr, uint searchFreespaceSize)
        {
            uint termDataAddr = newFreeSapceAddr + searchFreespaceSize;
            if (termDataAddr + 4 > Program.ROM.Data.Length)
            {//範囲外だから、入れられるわけがない.
                return false;
            }
            uint freespace = MoveToFreeSapceForm.SearchFreeSpaceOne(0, isProgramArea: false);
            if (freespace == U.NOT_FOUND)
            {//連続しない
                return false;
            }
            //連続する領域を発見.
            if (freespace < newFreeSapceAddr)
            {//しかし発見したのははるか前方
                return false;
            }
            if (freespace > termDataAddr)
            {//しかし発見したのははるか後方
                return false;
            }
            //ダブり TERMが必要
            return true;
        }

        //データを書き込む
        public static uint WriteBinaryDataDirect(
              uint addr
            , byte[] dataByte
            , Undo.UndoData undodata
        )
        {
            //アライメントを考える
            uint size = U.Padding4((uint)dataByte.Length);

            //新規サイズ
            if (addr + size > Program.ROM.Data.Length)
            {//必要サイズがROMサイズを超えていたら増設する.
                bool isResizeSuccess  = Program.ROM.write_resize_data((uint)(addr + size));
                if (isResizeSuccess == false)
                {
                    return U.NOT_FOUND;
                }
            }

            //文字列の書き込み
            Program.ROM.write_range(addr, dataByte, undodata);

            return addr;
        }
        public static uint AllocBinaryData(
              uint size
            , bool isProgramArea = false
        )
        {
            if (OptionForm.rom_extends() == OptionForm.rom_extends_enum.NO)
            {//ROM拡張を拒否
                R.ShowStopError("空き領域がもうありません。");
                return U.NOT_FOUND;
            }

            //拡張領域から探すときは、ファイル終端に備えて、アライメントを考えて、ちょい大目に探さないといけない.
            uint searchFreespaceSize = U.Padding4(size) ;

            //拡張領域に移動.
            uint freespace = MoveToFreeSapceForm.SearchFreeSpaceOne(searchFreespaceSize, isProgramArea);
            if (freespace == U.NOT_FOUND)
            {
                //空き領域が本当何もない!
                R.ShowStopError("空き領域がもうありません。");
                return U.NOT_FOUND;
            }

            return freespace;
        }
        //LZ77で圧縮されている場合の長さを求める汎用ルーチン.
        public static MoveToUnuseSpace.ADDR_AND_LENGTH get_data_pos_callback_lz77(uint addr)
        {
            addr = U.toOffset(addr);

            MoveToUnuseSpace.ADDR_AND_LENGTH aal = new MoveToUnuseSpace.ADDR_AND_LENGTH();
            aal.addr = addr;
            aal.length = LZ77.getCompressedSize(Program.ROM.Data, addr);
            UpdateLZ77Padding(ref aal);

            return aal;
        }
        //APデータの長さを求める汎用ルーチン.
        public static MoveToUnuseSpace.ADDR_AND_LENGTH get_data_pos_callback_ap(uint addr)
        {
            addr = U.toOffset(addr);

            MoveToUnuseSpace.ADDR_AND_LENGTH aal = new MoveToUnuseSpace.ADDR_AND_LENGTH();
            aal.addr = addr;
            aal.length = ImageUtilAP.CalcAPLength(addr);

            return aal;
        }
        public static void UpdateLZ77Padding(ref MoveToUnuseSpace.ADDR_AND_LENGTH aal)
        {
            if (U.isPadding4(aal.length))
            {
                return;
            }
            uint mod = aal.length % 4;

            for (uint i = 0; i < mod; i++)
            {
                uint addr = aal.addr + aal.length + i;
                if (!U.isSafetyOffset(addr))
                {//ROM末尾を超えている
                    return;
                }

                uint a = Program.ROM.u8(addr);
                if (a != 0)
                {//0ではないのでpaddingではない.
                    return;
                }
            }
            //最大値の算出しなおし.
            aal.length = U.Padding4(aal.length);
        }

        //画像みたいなデータの書き込み.
        public uint WriteImageData(NumericUpDown numObj, byte[] image, bool useLZ77, Undo.UndoData undodata, uint[] forceSeparationAddress = null)
        {
            uint addr = (uint)numObj.Value;
            uint newAddr = WriteImageData(addr, image, useLZ77, undodata,forceSeparationAddress);
            if (newAddr == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }
            U.ForceUpdate(numObj, U.toPointer(newAddr));
            return newAddr;
        }

        //未知の衝突
        static bool IsUnknownCollision(uint addr, uint newSize)
        {
            uint a = U.toPointer(addr);
            uint add_addr = a + newSize;

            Dictionary<uint, AsmMapFile.AsmMapSt> map = Program.AsmMapFileAsmCache.GetAsmMapFile().GetAsmMap();
            foreach (var pair in map)
            {
                if (pair.Value.IsFreeArea)
                {
                    continue;
                }

                uint start = pair.Key;
                if (a == start)
                {//自分自身
                    continue;
                }

                uint end = pair.Key + pair.Value.Length;
                if ((a >= start && a < end)
                    || (add_addr >= start && add_addr < end))
                {
                    Log.Notify("InputFormRef IsUnknownCollision", "Addr:", U.To0xHexString(addr), "Length:", U.To0xHexString(newSize), "Collision:", pair.Value.Name, U.To0xHexString(start), U.To0xHexString(end));
                    return true;
                }
            }
            //衝突していない
            return false;
        }

        //画像みたいなデータの書き込み.
        public uint WriteImageData(uint addr, byte[] image, bool useLZ77, Undo.UndoData undodata, uint[] forceSeparationAddress = null)
        {
            if (useLZ77)
            {
                image = LZ77.compress(image);
            }

            uint newAddr;
            if (!U.isSafetyOffset(U.toOffset(addr)))
            {
                //ここには書き込めないため、新規領域を確保する.
                newAddr = AppendBinaryData(image, undodata);
            }
            else if (!U.isSafetyOffset(U.toOffset(addr) + (uint)image.Length - 1))
            {
                //十分な余白がないので、新規拡張する必要がある.
                newAddr = AppendBinaryData(image, undodata);
            }
            else if (ImageFormRef.checkForceSeparationAddress(addr, forceSeparationAddress))
            {//汎用TSAアドレスなので絶対に上書きしてはいけない
                newAddr = AppendBinaryData(image, undodata);
            }
            else
            {
                List<uint> pointers = U.GrepPointerAll(Program.ROM.Data, U.toPointer(addr), this.BaseAddress,this.BaseAddress + this.BlockSize * this.DataCount);
                if (pointers.Count <= 1)
                {//単体から参照されていないので上書きできるなら上書きします.
                    if (useLZ77)
                    {
                        newAddr = WriteBinaryData(this.SelfForm, U.toOffset(addr), image, get_data_pos_callback_lz77, undodata);
                    }
                    else
                    {//固定長
                        if (IsUnknownCollision(addr, (uint)image.Length))
                        {//未知の衝突の検出されたので追記します.
                            newAddr = AppendBinaryData(image, undodata);
                        }
                        else
                        {
                            Program.ROM.write_range(U.toOffset(addr), image, undodata);
                            newAddr = U.toOffset(addr);
                        }
                    }
                }
                else
                {//複数個所から参照されているので、新規に領域を確保します.
                    newAddr = AppendBinaryData(image, undodata);
                }
            }
            if (newAddr == U.NOT_FOUND)
            {//書き込みキャンセル.
                return U.NOT_FOUND;
            }

            return U.toOffset(newAddr);
        }

        //ポインタバージョン
        public uint WriteImageDataPointer(uint pointer, byte[] image, bool useLZ77, Undo.UndoData undodata, uint[] forceSeparationAddress = null)
        {
            uint addr = Program.ROM.p32(U.toOffset(pointer));
            uint a = WriteImageData(addr, image, useLZ77, undodata, forceSeparationAddress);
            if (a == U.NOT_FOUND)
            {
                return a;
            }
            Program.ROM.write_p32(addr, a, undodata);
            return a;
        }

        //画像 10分割みたいなデータの書き込み.
        public uint WriteImageData10(NumericUpDown numObj, byte[] image, Undo.UndoData undodata, uint[] forceSeparationAddress = null)
        {
            uint addr = U.toOffset((uint)numObj.Value);
            if (!U.isSafetyOffset(addr))
            {
                addr = 0;
            }

            //データサイズは必ず 0x800ブロックずつにしないといけない.
            const uint splitByte = 0x800;

            byte[] addrMap = new byte[10*4];
            for(int i = 0 ; i < 10 ; i++)
            {
                uint paddr;
                if (addr == 0)
                {
                    paddr = 0;
                }
                else
                {
                    paddr = Program.ROM.p32((uint)(addr + (i * 4)));
                }
                byte[] pimage = U.subrange(image
                    , (uint)(splitByte * i)
                    , (uint)(splitByte * (i + 1)));

                uint pnewaddr = WriteImageData(paddr, pimage, true, undodata, forceSeparationAddress);
                if (pnewaddr == U.NOT_FOUND)
                {
                    return U.NOT_FOUND;
                }
                U.write_p32(addrMap,(uint)(i*4) , pnewaddr);
            }

            uint newAddr = WriteImageData(addr, addrMap, false, undodata, forceSeparationAddress);
            if (newAddr == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }
            U.ForceUpdate(numObj, U.toPointer(newAddr));
            return newAddr;
        }
        //画像みたいなデータの書き込み. 2分割
        public bool WriteImageData2(NumericUpDown numObj, NumericUpDown numObj2, byte[] image, bool useLZ77, Undo.UndoData undodata, uint[] forceSeparationAddress = null)
        {
            //データサイズは必ず 0x3000ブロックずつにしないといけない.
            const uint splitByte = 0x3000;
            {
                byte[] pimage = U.subrange(image
                    , 0
                    , (uint)(splitByte)
                );

                uint addr = (uint)numObj.Value;
                uint newAddr = WriteImageData(addr, pimage, useLZ77, undodata, forceSeparationAddress);
                if (newAddr == U.NOT_FOUND)
                {
                    return false;
                }
                U.ForceUpdate(numObj, U.toPointer(newAddr));
            }
            {
                byte[] pimage = U.subrange(image
                    , (uint)(splitByte)
                    , (uint)image.Length
                );

                uint addr = (uint)numObj2.Value;
                uint newAddr = WriteImageData(addr, pimage, useLZ77, undodata, forceSeparationAddress);
                if (newAddr == U.NOT_FOUND)
                {
                    return false;
                }
                U.ForceUpdate(numObj, U.toPointer(newAddr));
            }
            return true;
        }

        public static void WriteOnePointer(uint pointer, NumericUpDown numObj, Undo.UndoData undodata)
        {
            uint addr = U.toOffset(pointer);
            if (!U.CheckZeroAddressWrite(addr))
            {//ポインタなので、 CheckZeroAddressWriteHighは無理.
                return;
            }

            Program.ROM.write_p32(addr, (uint)numObj.Value, undodata);
        }

        //アイテムリストを得る
        public List<U.AddrResult> MakeList()
        {
            List<U.AddrResult> ret = new List<U.AddrResult>();

            uint addr = (uint)this.BaseAddress;
            int limitsize = (int)this.DataCount;


            //終端
            uint limitter = (uint)Program.ROM.Data.Length;
            if (addr + this.BlockSize >= limitter)
            {//終端を超えるので探索強制打ち切り.
                return ret;
            }
            if (addr == EventUnitForm.INVALIDATE_UNIT_POINTER)
            {//無効値
                return ret;
            }

            for (int i = 0; i < limitsize; i++)
            {
                U.AddrResult ar = this.LoopCallback(i, addr);
                if (!ar.isNULL())
                {
                    ret.Add(ar);
                }

                addr = this.NextAddrCallback(addr);

                if (addr + this.BlockSize > limitter)
                {//終端を超えるので探索強制打ち切り.
//                    Debug.Assert(false);
                    break;
                }
            }
            return ret;
        }
        //アイテムリストを得る
        public List<U.AddrResult> MakeList(Func<uint, bool> condCallback)
        {
            List<U.AddrResult> ret = new List<U.AddrResult>();

            uint addr = (uint)this.BaseAddress;
            int limitsize = (int)this.DataCount;

            //終端
            uint limitter = (uint)Program.ROM.Data.Length;
            if (addr + this.BlockSize >= limitter)
            {//終端を超えるので探索強制打ち切り.
                return ret;
            }

            for (int i = 0; i < limitsize; i++)
            {
                bool cr = condCallback(addr);
                if (cr)
                {
                    U.AddrResult ar = this.LoopCallback(i, addr);
                    if (!ar.isNULL())
                    {
                        ret.Add(ar);
                    }
                }
                addr = this.NextAddrCallback(addr);

                if (addr + this.BlockSize > limitter)
                {//終端を超えるので探索強制打ち切り.
                    Debug.Assert(false);
                    break;
                }
            }
            return ret;
        }
        public List<U.AddrResult> MakeList(Func<U.AddrResult,U.AddrResult> condCallback)
        {
            List<U.AddrResult> ret = new List<U.AddrResult>();

            uint addr = (uint)this.BaseAddress;
            int limitsize = (int)this.DataCount;

            //終端
            uint limitter = (uint)Program.ROM.Data.Length;
            if (addr + this.BlockSize >= limitter)
            {//終端を超えるので探索強制打ち切り.
                return ret;
            }

            for (int i = 0; i < limitsize; i++)
            {
                U.AddrResult ar = this.LoopCallback(i, addr);
                if (!ar.isNULL())
                {
                    U.AddrResult cr = condCallback(ar);
                    if (!cr.isNULL())
                    {
                        ret.Add(cr);
                    }
                }
                addr = this.NextAddrCallback(addr);

                if (addr + this.BlockSize > limitter)
                {//終端を超えるので探索強制打ち切り.
                    Debug.Assert(false);
                    break;
                }
            }
            return ret;
        }

        public static void AppendNameString(List<U.AddrResult> list
            , string pre_name,string before_name = "")
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (pre_name != "")
                {
                    list[i].name = pre_name + " " + list[i].name;
                }
                if (before_name != "")
                {
                    list[i].name = list[i].name + " " + before_name;
                }
            }
        }

        public void ClearSelect(bool clearReadStartAddress)
        {
            if (this.AddressList != null)
            {
                this.AddressList.Items.Clear();
                this.AddressList.Tag = null;
            }
            if (this.Address != null)
            {
                this.Address.Value = 0;
            }
            if (clearReadStartAddress && this.ReadStartAddress != null)
            {
                this.ReadStartAddress.Value = 0;
            }
            if (this.ReadCount != null)
            {
                ReadCount.Value = 0;
            }
            this.OldBaseAddress = 0;
            this.OldDataCount = 0;

            List<Control> controls = GetAllControls(this.SelfForm);
            for (int i = 0; i < controls.Count; i++)
            {
                string name = SkipPrefixName(controls[i].Name, this.Prefix);
                if (name == "")
                {
                    continue;
                }

                if (controls[i] is NumericUpDown && IsTypeWord(name[0]))
                {
                    ((NumericUpDown)controls[i]).Value = 0;
                }
                else if (name.IndexOf("_NEWALLOC") >= 0)
                {//新規割り当てボタンがあったら、消します.
                    controls[i].Visible = false;
                }
            }
        }

        public static void WritePointerButton(Form self,uint write_addr,uint write_pointer_value,string undo_name)
        {
            write_addr = U.toOffset(write_addr);
            write_pointer_value = U.toPointer(write_pointer_value);

            if (!U.CheckZeroAddressWrite(write_addr))
            {//ポインタなので CheckZeroAddressWriteHighは過剰
                return;
            }

            Program.Undo.Push(undo_name, write_addr, 4);

            Program.ROM.write_u32(write_addr,write_pointer_value );
            InputFormRef.ShowWriteNotifyAnimation(self, write_addr);
        }

        public static string ConvertAddToValue(string str)
        {
            int pos = str.IndexOf("[0x");
            if (pos < 0)
            {
                return str;
            }
            pos += 3; //skip [0x
            int end_pos = str.IndexOf("]", pos);

            string target = str.Substring(pos, end_pos - pos);
            if (target.IndexOf(",") > 0)
            {//x,y座標
                string[] sp = target.Split(',');
                if (sp.Length < 2)
                {
                    return str;
                }

                uint x,y;
                x = U.atoh(sp[0]);
                if (sp[1].IndexOf("0x") >= 0)
                {
                    y = U.atoi0x(sp[1]);
                }
                else
                {
                    y = U.atoh(sp[1]);
                }
                if (!U.isSafetyOffset(x) || !U.isSafetyOffset(y))
                {
                    return str;
                }
                x = Program.ROM.u8(x);
                y = Program.ROM.u8(y);
                string names = string.Format("=> {0},{1}", x, y);
                return str.Insert(end_pos, names);
            }
            else
            {//ユニット
                uint addr = U.atoh(target);
                if (!U.isSafetyOffset(addr))
                {
                    return str;
                }

                string names = "";
                for (uint i = 0; true; i += 2)
                {
                    uint uid = Program.ROM.u8(addr + i);
                    if (uid == 0 || uid >= 0xFE)
                    {
                        break;
                    }
                    string name = UnitForm.GetUnitName(uid);
                    names += " " + U.ToHexString(uid) + ":" + name;
                    if (i > 1)
                    {//以下省略
                        names += "...";
                        break;
                    }
                }
                return str.Insert(end_pos, names);
            }
        }

        //保存するファイルに名前を作る.
        public string MakeSaveImageFilename(string ext = ".png")
        {
            string name = this.SelfForm.Text;
            if (this.AddressList != null && this.AddressList.SelectedIndex >= 0)
            {
                name = name + "_" + this.AddressList.SelectedIndex + "@" + this.AddressList.Text;
            }
            if (this.Address != null)
            {
                name = name + "_" + U.ToHexString(this.Address.Value);
            }
            return U.escape_filename(name) + ext;
        }
        public static string MakeSaveImageFilename(Form self, NumericUpDown addr, string ext = ".png")
        {
            string name = self.Text;
            name = name + "_" + U.ToHexString(addr.Value);
            return U.escape_filename(name) + ext;
        }
        public static string MakeSaveImageFilename(Form self, ComboBox combo, string ext = ".png")
        {
            string name = self.Text;
            name = name + "_" + combo.Text;
            return U.escape_filename(name) + ext;
        }
        public static string MakeSaveImageFilename(Form self, string append_name, string ext = ".png")
        {
            string name = self.Text;
            name = name + "_" + append_name;
            return U.escape_filename(name) + ext;
        }

        public static void LoadComboResource(ComboBox combo, string fullfilename)
        {
            combo.Items.Clear();
            if (!U.IsRequiredFileExist(fullfilename))
            {
                return;
            }

            combo.BeginUpdate();
            using (StreamReader reader = File.OpenText(fullfilename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (U.IsComment(line) || U.OtherLangLine(line))
                    {
                        continue;
                    }
                    line = U.ClipComment(line);
                    combo.Items.Add(line);
                }
            }
            combo.EndUpdate();
        }

        public static void LoadComboResource(ComboBox combo, Dictionary<uint, string> resource)
        {
            combo.Items.Clear();
            combo.BeginUpdate();

            var sortedlist = U.OrderBy(resource, (x) => { return (int)x.Key; });
            foreach (var pair in sortedlist)
            {
                combo.Items.Add( U.ToHexString(pair.Key)+"="+pair.Value );
            }
            combo.EndUpdate();
        }



        //色変更COMBO
        public static void OwnerDrawColorCombo(ComboBox lb)
        {
            List<Color> colorList = new List<Color>();
            for (int i = 0; i < lb.Items.Count; i++)
            {
                string name = lb.Items[i].ToString();
                if(name.IndexOf("=普通")>=0 || name.IndexOf("=白")>=0) ///No Translate
                {
                    colorList.Add(Color.White);
                }
                else if(name.IndexOf("=灰色")>=0) ///No Translate
                {
                    colorList.Add(Color.Gray);
                }
                else if(name.IndexOf("=青")>=0) ///No Translate
                {
                    colorList.Add(Color.Blue);
                }
                else if(name.IndexOf("=黄")>=0) ///No Translate
                {
                    colorList.Add(Color.Yellow);
                }
                else if(name.IndexOf("=緑")>=0) ///No Translate
                {
                    colorList.Add(Color.Green);
                }
                else if(name.IndexOf("=黒")>=0) ///No Translate
                {
                    colorList.Add(Color.Black);
                }
                else if(name.IndexOf("=赤")>=0) ///No Translate
                {
                    colorList.Add(Color.Red);
                }
            }


            lb.DrawMode = DrawMode.OwnerDrawFixed;

            lb.DrawItem += (sender, e) =>
            {
                if (!(sender is ComboBox))
                {
                    return;
                }
                ComboBox combo = (ComboBox)sender;
                if (e.Index < 0 || e.Index >= combo.Items.Count)
                {
                    return;
                }

                // Draw the background 
                e.DrawBackground();


                Color c;
                Color b = lb.ForeColor;
                if (colorList.Count < e.Index)
                {
                    c = lb.ForeColor;
                }
                else
                {
                    c = colorList[e.Index];
                    if (c == lb.BackColor)
                    {
                        c = lb.ForeColor;
                    }
                }

                // Get the item text    
                string text = combo.Items[e.Index].ToString();

                // Determine the forecolor based on whether or not the item is selected    
                Brush brush = new SolidBrush(c);

                // Draw the text    
                e.Graphics.DrawString(text, combo.Font, brush, e.Bounds.X, e.Bounds.Y);
            };
        }

        static void MenuStripTranslate(ToolStripMenuItem menu)
        {
            string s = menu.Text;
            string t = MyTranslateResource.str(s);
            if (s != t)
            {
                menu.Text = t;
            }

            for (int i = 0; i < menu.DropDownItems.Count; i++)
            {
                ToolStripItem item = menu.DropDownItems[i];
                if (! (item is ToolStripMenuItem))
                {
                    continue;
                }

                ToolStripMenuItem strip = (ToolStripMenuItem)item;
                MenuStripTranslate(strip);
            }
        }

        //コントロールの翻訳
        public static void ControlTranslate(Control self)
        {
            string s = self.Text;
            string t = MyTranslateResource.str(s);
            if (s != t)
            {
                self.Text = t;
            }
            if (self is ComboBox)
            {
                ComboBox c = (ComboBox)self;
                for (int i = 0; i < c.Items.Count; i++)
                {
                    s = (string)c.Items[i];
                    t = MyTranslateResource.str(s);
                    if (s != t)
                    {
                        c.Items[i] = t;
                    }
                }
            }
            else if (self is ListBoxEx)
            {
                ListBox c = (ListBox)self;
                for (int i = 0; i < c.Items.Count; i++)
                {
                    s = (string)c.Items[i];
                    t = MyTranslateResource.str(s);
                    if (s != t)
                    {
                        c.Items[i] = t;
                    }
                }
            }
            else if (self is Form)
            {
                Form c = (Form)self;
                if (c.MainMenuStrip != null)
                {
                    for (int i = 0; i < c.MainMenuStrip.Items.Count; i++)
                    {
                        MenuStripTranslate((ToolStripMenuItem)c.MainMenuStrip.Items[i]);
                    }
                }
            }
            else if (self is Label)
            {
                Label c = (Label)self;
                c.AutoEllipsis = true;
            }
            else if (self is Button)
            {
                Button c = (Button)self;
                if (c.Name.IndexOf("SONGPLAY") >= 0)
                {
                }
                else
                {
                    c.AutoEllipsis = true;
                }
            }

            foreach (Control c in self.Controls)
            {
                ControlTranslate(c);
            }
        }

        //Modの適応.
        static void ApplyMod(Control f, List<Control> controls,ToolTipEx tooltip)
        {
            if (Program.Mod == null)
            {
                return;
            }

            string forname = f.Name;
            foreach( Mod.ModSt m in Program.Mod.Mods)
            {
                if (forname != m.Form)
                {
                    continue;
                }

                //名前の変更
                foreach (Mod.ModTypeSt mtype in m.Param)
                {
                    string search = mtype.key;
                    foreach (Control c in controls)
                    {
                        if (c.Name.IndexOf(search) == 0
                         && (c.Name.Length <= search.Length || !U.isnum(c.Name[search.Length]))
                            )
                        {
                            if (mtype.type == "NAME")
                            {
                                c.Name = mtype.value;
                                if (c is Label)
                                {
                                    InputFormRef.MakeJumpEventOne("", controls, (Label)c, null, null);
                                }
                                else
                                {
                                    InputFormRef.MakeLinkEventOne("", controls, c);
                                }
                            }
                            else if (mtype.type == "HINT")
                            {
                                tooltip.SetToolTip(c, U.nl2br(mtype.value));
                            }
                            else if (mtype.type == "COMBOAPPEND")
                            {
                                if (c is ComboBox)
                                {
                                    ((ComboBox)c).Items.Add(mtype.value);
                                }
                                else if (c is ListBoxEx)
                                {
                                    ((ListBox)c).Items.Add(mtype.value);
                                }
                            }
                            else if (mtype.type == "VISIBLE")
                            {
                                c.Visible = U.stringbool(mtype.value);
                            }
                            else
                            {
                                c.Text = mtype.value;
                            }
                        }
                    }
                }
            }
        }

        //FE Editor が書き込んでいるデータ長があれば参照する
        static public uint GetFEditorLengthHint(uint data_offset)
        {
            if (OptionForm.lookup_feditor() != OptionForm.lookup_feditor_enum.Lookup)
            {//FEditorのヒント区を利用しない.
                return U.NOT_FOUND;
            }

            if (! U.isSafetyOffset(data_offset - 4) )
            {
                return U.NOT_FOUND;
            }

            uint value = Program.ROM.u32(data_offset - 4);
            if (value >= 1024)
            {//大きすぎる
                return U.NOT_FOUND;
            }
            if (value < 100)
            {//小さすぎる、別の値では？
                return U.NOT_FOUND;
            }
            return value;
        }
        //リストが拡張されたとき、FEditorのリスト長さのヒントを書き込む
        static public void WriteFEditorLengthHint(object sender, EventArgs arg)
        {
            ListBox lb = (ListBox)sender;
            int count = lb.Items.Count;
            if (count <= 0)
            {
                return;
            }
            if (count < 100 || count >= 0xffff)
            {//誤解させる値は書かない.
                return;
            }

            uint data_offset = InputFormRef.SelectToAddr(lb, 0);
            if (!U.isSafetyOffset(data_offset - 4))
            {
                return;
            }
            uint value = Program.ROM.u32(data_offset - 4);
            if (value > 0xffff)
            {
                return;
            }
            if (value < 100)
            {//小さすぎる、別の値では？
                return;
            }

            Program.ROM.write_u32(data_offset - 4, (uint)count);
        }

        public void JumpTo(uint search_id)
        {
            if (this.AddressList == null)
            {
                return;
            }

            int limit = this.AddressList.Items.Count;
            for (int i = 0; i < limit; i++)
            {
                string name = this.AddressList.Items[i].ToString();
                uint id = U.atoh(name);
                if (id == search_id)
                {
                    U.SelectedIndexSafety(this.AddressList, i);
                    return;
                }
            }
        }

        public static string GetBusyErrorExplain()
        {
            return R._("現在、処理中のため実行できません。\r\n処理が終わるまでお待ちください。");
        }
        static string GetExplainPointer()
        {
            return R._("この値は、設定値へのポインタで指定されます。\r\n");
        }
        public static string GetExplain(string str)
        {
            if (str == "@AchievementFlag")
            {
                str = EventCondForm.GetExplainOfAchievementFlag();
            }
            else if (str == "@ITEM_USES")
            {
                str = R._("アイテムの耐久度です。\r\n0になると壊れます。\r\n無限に使わせたい場合は、特性で「耐久無限」を指定してください。\r\n");
            }
            else if (str == "@ITEM_STATBOOSTER")
            {
                str = R._("アイテムでのドーピング効果を指定します。\r\n") + GetExplainPointer();
            }
            else if (str == "@ITEM_EFFECTIVENESS")
            {
                str = R._("アイテムでの特効効果を指定します。\r\n") + GetExplainPointer();
            }
            else if (str == "@ITEM_USE_EFFECT")
            {
                str = R._("アイテムを利用したときの効果を指定します。\r\n実際に何が起きるのかは、詳細メニューの「アイテム利用効果」から定義します。\r\n");
            }
            else if (str == "@ITEM_WEXP")
            {
                str = R._("アイテムを利用した時に得られる武器経験値を指定します。\r\n");
            }
            else if (str == "@ITEM_PRICE")
            {
                str = R._("アイテムの単価を指定します\r\n単価*残り=価格です。\r\nその半額が店で売るときの値段になります。\r\n");
            }
            else if (str == "@ITEM_ID")
            {
                str = R._("アイテムIDです。\r\n通常は、混乱を避けるため、リストにある順番通りの順番を指定します。");
            }
            else if (str == "@NAME")
            {
                str = R._("名前を指定します。\r\n") + TextForm.GetExplainOneLine();
            }
            else if (str == "@DETAIL")
            {
                str = R._("詳細を指定します。\r\n") + TextForm.GetExplainThreeLine();
            }
            else if (str == "@CLASS_BASE")
            {
                str = R._("クラスの基礎値となる値です。\r\nこのクラス能力値とユニット能力値を合算したものがユニットの初期値になります。\r\nそこから成長率でLv成長させます。");
            }
            else if (str == "@CLASS_LIMIT")
            {
                str = R._("クラスの最大ステータスです。\r\nこのステータス以上には成長させられません。\r\nまた、FEでは31以上のパラメータは保存されません。\r\n31以上にするには、パッチを利用して、セーブデータを改造する必要があります。");
            }
            else if (str == "@CLASS_GROW")
            {
                str = R._("ユニット配置で、成長率を「クラス依存」にした場合の成長率です。\r\n主に敵軍で利用されます。");
            }
            else if (str == "@CLASS_MOVE_SPEED")
            {
                str = R._("ユニットの移動速度を指定します。\r\n通常は、0を指定してください。\r\nアーマーナイトのような重いクラスは、 1を指定してください。\r\n");
            }
            else if (str == "@CLASS_CC")
            {
                if (Program.ROM.RomInfo.version() == 8)
                {
                    str = R._("FE8では、分岐クラスチェンジが実装されたので、\r\nクラスチェンジの値としての意味をなさなくなりましたが、\r\n敵の自動成長に利用されています。\r\n敵の上級職を自動成長に成長させる時に、ここに設定されている下級職の成長率で成長させます。\r\nまた、クラスチェンジ3分岐パッチなどで、第3の選択子として利用されることもあります。");
                }
                else
                {
                    str = R._("{0}する時のクラスとして利用されます。", R._("クラスチェンジ"));
                }
            }
            else if (str == "@UNIT_SORT_ORDER")
            {
                str = R._("部隊メニューでソートした時の順番です。");
            }
            else if (str == "@TURNCOND_TURN")
            {
                str = R._("「開始ターン」から「終了ターン」までの間、イベントを動作させます。");
            }
            else if (str == "@TURNCOND_WHOSE_TURN")
            {
                str = R._("誰のターンにイベントを動作させるかを選択します。\r\n例えば、敵ターンに増援イベントを作ると、敵が即動くので難易度があがってしまいます。");
            }
            else if (str == "@MAPSETTING_ID")
            {
                if (Program.ROM.RomInfo.version() == 7 && Program.ROM.RomInfo.is_multibyte() == false)
                {
                    str = R._("章タイトル画像を表示するためのIDですが、\r\nFE7Uではテキスト形式のデータを利用するため、利用されません。");
                }
                else
                {
                    str = R._("章タイトル画像を表示するためのIDです。");
                }
            }
            else if (str == "@MAPSETTING_ID2")
            {
                if (Program.ROM.RomInfo.version() >= 8)
                {
                    str = R._("前作の残骸です。利用されません。");
                    str = R._("ヘクトル編の章タイトル画像を表示するためのIDです。");
                }
                else if (Program.ROM.RomInfo.version() == 7 && Program.ROM.RomInfo.is_multibyte() == false)
                {
                    str = R._("ヘクトル編の章タイトル画像を表示するためのIDですが、\r\nFE7Uではテキスト形式のデータを利用するため、利用されません。");
                }
                else
                {
                    str = R._("ヘクトル編の章タイトル画像を表示するためのIDです。");
                }
            }
            else if (str == "@MAPSETTING_PREP_SCREEN_BOOL")
            {
                if (Program.ROM.RomInfo.version() >= 8)
                {
                    str = R._("前作の残骸です。利用されません。");
                    str = str + "\r\n" + R._("以前は、進撃準備画面を利用するかどうかの判定に利用されていました。");
                }
                else
                {
                    str = R._("進撃準備画面を利用するかどうかのbool値です。\r\n0の場合、進撃準備画面は利用されません。\r\n1の場合、進撃準備画面が利用されます。");
                }
            }
            else if (str == "@UNIT_ID")
            {
                str = R._("ユニットIDです。\r\n通常は、混乱を避けるため、リストにある順番通りの順番を指定します。\r\n");
                if (Program.ROM.RomInfo.version() >= 7)
                {
                    str += R._("仲間にできるユニットは 0x45までのユニットです。\r\nそれ以降は戦績データが記録されません。\r\n");
                    uint enemy_id = 0;
                    if (Program.ROM.RomInfo.version() == 7)
                    {
                        enemy_id = 0xFB;
                    }
                    else
                    {
                        enemy_id = 0xFD;
                    }
                    str += R._("また、闘技場に出てくるユニットは、 {0}のユニットです。\r\nそれ以外は利用されません。\r\n", enemy_id.ToString("X02"));
                }
            }
            else if (str == "@CLASS_ID")
            {
                str = R._("クラスを識別するIDです。\r\nFEでは、クラスIDが移動アイコンのIDとなります。\r\nそのため、この値を変更するときは、移動アイコンも変更する必要があります。\r\nたいていは、混乱を避けるため、リストにある並び順通りに設定します。");
            }
            else if (str == "@TRAP_INITTIMER")
            {
                str = EventCondForm.GetExplainOfTrapTimer();
            }
            else if (str == "@TRAP_REPEATTIMER")
            {
                str = EventCondForm.GetExplainOfTrapTimer();
            }
            else if (str == "@EVENTTYPE")
            {
                str = R._("イベントの種類を定義します。\r\n必ず所定の値を設定してください。\r\n終端にする場合は、0x00を設定し、フラグにも、0x00を設定してください。");
            }
            else if (str == "@MAPOBJECTTYPE")
            {
                str = R._("マップオブジェクトの種類を指定します。\r\n必ず所定の値を指定してください。0x00のままだと動作しません。\r\n");
            }
            else if (str == "@IFASM")
            {
                str = R._("このアセンブラ関数を実行し、戻り値がr0==1だった場合にのみ有効になります。");
            }
            else if (str == "@SIM")
            {
                str = R._("成長率で、成長させた場合の期待値のシミュレーションです。\r\n設定の目安として利用してください。");
            }
            else if (str == "@SIM_CLASS")
            {
                str = R._("敵兵に利用されるクラスの成長率で、成長させた場合の期待値のシミュレーションです。\r\n設定の目安として利用してください。\r\nFEでは敵兵の上級職は、レベル+10として計算されます。\r\n敵上級職は、Lv10でクラスチェンジしたとして計算されます。");
            }
            else if (str == "@UNITGROW")
            {
                str = R._("ユニットがレベルアップした時の成長率を定義します。\r\n");
            }
            else if (str == "@UNITBASE")
            {
                str = R._("ユニットの基礎値です。\r\nこの値に、クラスの能力値を加えたものがユニットの初期値になります。\r\nマイナス値にすることもできますが、合算した時に-1以下になると、2の補数により、ロード時に最大数になります。\r\n");
            }
            else if (str == "@WEAPONLV")
            {
                str = R._("武器レベルの基礎値です。\r\n敵軍の武器レベル成長についてはまだよくわかっていません。\r\n武器レベルが足りないとせっかく持たせた武器が使えません。\r\n逆に、Sになるとボーナスが付与されるので、より強くなります。\r\n魔法と武器を同時に利用するには、パッチが必要です。\r\n(パッチを利用しないと、魔法使いとして認識され、攻撃モーションは、必ず間接攻撃となります。)\r\n");
            }
            else if (str == "@MAPSETTING_OBJ")
            {
                str = R._("マップチップを構成する基本となる画像を選択します。(obj)\r\n") + MapSettingForm.GetExplainPLIST() + MapSettingForm.GetExplainMapChipset3();
            }
            else if (str == "@MAPSETTING_PAL")
            {
                str = R._("マップチップの配色を決めるパレットを指定します。(pal)\r\nobjとconfigは、使いたいタイルセットの種類である程度決まります。\r\nフィールドなのか、城なのか、などで、定番の値が決まります。\r\nただ、パレットは、朝と夕暮れのように、場合により、切り替えられることができます。\r\n") + "\r\n" + MapSettingForm.GetExplainPLIST() + MapSettingForm.GetExplainMapChipset3();
            }
            else if (str == "@MAPSETTING_CONFIG")
            {
                str = R._("マップチップを組み立てと役割を定義します。(config)\r\nobjから画像を切り出し、何番目のpalを利用するかを定義します。\r\nまた、タイルチップが、草原なのか、川なのか、村なのか、といった指定も行います。\r\n") + "\r\n" + MapSettingForm.GetExplainPLIST() + MapSettingForm.GetExplainMapChipset3();
            }
            else if (str == "@MAPSETTING_MAP")
            {
                str = R._("マップチップをどう並べるかを定義します。\r\nマップエディターで編集できるマップチップの並び順データを格納します。\r\n") + MapSettingForm.GetExplainPLIST();
            }
            else if (str == "@MAPSETTING_ANIMATION1")
            {
                str = R._("川のせせらぎや、海の波などのタイルアニメーションを指定します。\r\nobjの画像を強引にタイマーで書き換えることで実装されています。\r\n") + "\r\n" + MapSettingForm.GetExplainPLIST();
            }
            else if (str == "@MAPSETTING_ANIMATION2")
            {
                str = R._("パレットを変化させる、パレットアニメーションを指定します。\r\nFEでは、火山などの効果で利用されます。\r\nタイルアニメーション2は、詳細メニューから、変更できます。\r\n") + "\r\n" + MapSettingForm.GetExplainPLIST();
            }
            else if (str == "@MAPSETTING_MAPCHANGE")
            {
                str = R._("マップ変更を管理するポインタを指定します。\r\n扉や宝箱、村訪問、壊れる壁などが関係します。\r\n") + MapSettingForm.GetExplainPLIST();
            }
            else if (str == "@MAPSETTING_EVENT")
            {
                str = R._("イベントを管理する基準となるポインタIDを指定します。\r\n") + MapSettingForm.GetExplainPLIST();
            }
            else if (str == "@MAPSETTING_WMAP_EVENT")
            {
                str = R._("ワールドマップイベントを管理する基準となるポインタIDを指定します。\r\n");
                if (Program.ROM.RomInfo.version() == 6)
                {
                    str += MapSettingForm.GetExplainPLIST();
                }
                else
                {
                    str += R._("この値は、ワールドマップイベントテーブルへのIDです。");
                }
            }
            else if (str == "@MAPSETTING_CLEAR_COND1_DISPLAY_ONLY")
            {
                str = R._("画面右上に表示される、クリア条件を定義します。\r\n") + "\r\n" + MapSettingForm.GetExplainClearButShowOnly();
            }
            else if (str == "@MAPSETTING_CLEAR_COND2_DISPLAY_ONLY")
            {
                str = R._("状況メニューを開いたときに表示される、勝利条件を定義します。\r\n") + "\r\n" + MapSettingForm.GetExplainClearButShowOnly();
            }
            else if (str == "@MAPSETTING_CLEAR_COND3_DISPLAY_ONLY")
            {
                str = R._("右上のメニューを変化させます。\r\n「敵全滅」　残りの敵数が表示されます。\r\n「ターン防衛」　現在のターン数が表示されます。\r\n") + "\r\n" + MapSettingForm.GetExplainClearButShowOnly();
            }
            else if (str == "@MAPSETTING_CLEAR_COND3_DISPLAY_ONLY_FE8")
            {
                str = R._("右上のメニューを変化させます。\r\n「敵全滅」　残りの敵数が表示されます。\r\n「ターン防衛」　現在のターン数が表示されます。\r\n") + "\r\n" + MapSettingForm.GetExplainClearButShowOnly() 
                    + "\r\n" + R._("ただし、FE8では、敵全滅とボス撃破を選択した場合、敵ユニットがいなくなると自動的に終了イベントが呼ばれることがあります。\r\nこれはフリーマップのルーチンが関係しています。\r\nこれらの条件のマップでは、敵がいなくなる状態を作らないでください。");
            }
            else if (str == "@MAPSETTING_CLEAR_COND4_DISPLAY_ONLY")
            {
                str = R._("「特殊表示」で「ターン防衛」を選択した時に利用されます。\r\n指定したターン数 - 1 == 「最終のターン」\r\nに、なります。\r\nもちろん、表示だけで、実際にそれが最終ターンなのかは、イベントで定義されます。\r\n") + "\r\n" + MapSettingForm.GetExplainClearButShowOnly();
            }
            else if (str == "@MAPSETTING_CHAPTOR_ID")
            {
                str = R._("章の番号です。\r\n{0}の右上に表示されます。\r\nまた、エンディングに表示されるクリアターン数表示にも利用されます。", R._("進撃準備画面"));
            }
            else if (str == "@MAPSETTING_DEFENSE_UNIT")
            {
                str = R._("防衛ユニットにマークを表示します。\r\nもちろん、マークを表示するだけです。\r\n実際に防衛対象かどうかはイベントで定義されます。\r\n");
            }
            else if (str == "@MAPSETTING_IS_FADE_BLACK")
            {
                str = R._("開始イベントが呼び出される時に、画面を暗転したままにするかどうかを指定します。\r\n開始イベントの前に、演出を行うため、マップを見せたくない場合に設定します。\r\n");
            }
            else if (str == "@MAPSETTING_LEFT_ENEMY")
            {
                str = R._("勝利BGMに切り替わる敵の残数を指定します。\r\n通常は、1が指定されます。\r\nまた、値は1以上である必要があります。\r\n\r\n汎用の勝利BGMを、この章だけ変更したい場合は、\r\nフラグ4を有効にして、第2BGMを利用するか、\r\nまたは、FE8の場合は、勝利BGMを章ごとに変更するパッチを利用してください。\r\n");
            }
            else if (str == "@MAPSETTING_SECOND_BGM")
            {
                str = R._("フラグ4を有効にした場合に、切り替える第2BGMを指定します。");
            }
            else if (str == "@MAPSETTING_NOT_USE")
            {
                str = R._("前作の残骸です。利用されません。");
            }
            else if (str == "@MAPSETTING_ESCAPE_POINT")
            {
                str = R._("{0}の▲マークを表示します。\r\nただし、表示するだけです。\r\n\r\n実際の{0}は、詳細メニューの「{0}」から設定されます。\r\nマークを消す場合は、255 , 0 を設定してください。\r\n", R._("離脱ポイント"));
            }
            else if (str == "@MAPSETTING_INIT_COORDINATE")
            {
                str = R._("マップ開始時の初期座標です。\r\n開始時に暗転しない場合は、この座標で、ユーザにマップを見せることになります。\r\n");
            }
            else if (str == "@MAPSETTING_HARD_BOOST")
            {
                str = R._("難易度でハードを選んだ場合に、敵に与える強化ボーナス値を指定します。\r\n");
            }
            else if (str == "@MAPSETTING_NORMAL_MODE_LEVEL_PENALTY")
            {
                str = R._("難易度で普通を選んだ場合に、敵を弱くするペナルティ値を指定します。\r\n");
            }
            else if (str == "@MAPSETTING_EAZY_MODE_LEVEL_PENALTY")
            {
                str = R._("難易度で簡単を選んだ場合に、敵をとても弱くするペナルティ値を指定します。\r\n");
            }
            else if (str == "@EVENTUNIT_NEW")
            {
                str = R._("新規にユニット群を追加します。\r\n追加したユニット群は、イベントに関連付ける必要があります。\r\n");
            }
            else if (str == "@EVENTUNIT_LISTEXPANDS")
            {
                str = R._("ユニット数を増やします。");
            }
            else if (str == "@EVENTUNIT_UNIT_ID")
            {
                str = R._("登場させるユニットを選択します。\r\nこの値を0にすると、終端データとなり、それ以降のユニットはロードされなくなるので注意してください。\r\n");
            }
            else if (str == "@EVENTUNIT_CLASS_ID")
            {
                str = R._("指定したユニットを登場させるクラスを選択します。\r\n");
            }
            else if (str == "@EVENTUNIT_COMMANDER")
            {
                str = R._("指揮官を定義します。\r\n特定のAIや、指揮パッチなどを使った場合に効果があります。\r\n通常はなしでも構いません。\r\n");
            }
            else if (str == "@EVENTUNIT_UNIT")
            {
                str = R._("登場させるユニットの状態を定義します。\r\nこの値は、レベル、所属、成長率を組み合わせた値です。\r\n");
            }
            else if (str == "@EVENTUNIT_UNIT_LV")
            {
                str = R._("ユニットを登場させるレベルを指定します。\r\nユニットは、ユニット設定で定義された初期レベルから、このレベルまで成長して登場します。\r\n");
            }
            else if (str == "@EVENTUNIT_UNIT_ASSIGN")
            {
                str = R._("ユニットの所属を定義します。\r\nこのユニットデータをロードする方法にもよりますが、\r\n自軍ユニットにすると、簡単に仲間に加えることができます。\r\n");
            }
            else if (str == "@EVENTUNIT_UNIT_GROW")
            {
                str = R._("ユニットを指定したレベルまで成長させる方法を定義します。\r\n「成長させない」は、ユニットを自動的に成長させません。値を固定できるため、主に、味方ユニットや、ボスキャラで利用します。\r\n「クラス依存」は、クラス設定で定義した成長率で成長させます。主に、敵軍の一般兵士に利用されます。\r\n");
            }
            else if (str == "@EVENTUNIT_AI1")
            {
                str = R._("ユニットを制御するAIを指定します。\r\nAI1は主に行動を定義します。\r\n");
            }
            else if (str == "@EVENTUNIT_AI2")
            {
                str = R._("ユニットを制御するAIを指定します。\r\nAI2は主に移動方法を定義します。\r\n");
            }
            else if (str == "@EVENTUNIT_AI3")
            {
                str = R._("ユニットを制御するAIを指定します。\r\nAI3は主に性格を定義します。\r\n残りHPでの回復アイテムを使うように選択したり、\r\n攻撃するユニットを選ぶ標的AIを設定します。\r\n標的AIはさまざななパラメータからスコアを計算し、狙うユニットを決定します。\r\n");
            }
            else if (str == "@EVENTUNIT_AI4")
            {
                str = R._("回復するときに、退避行動をするかどうかを指定します。\r\n城門から絶対に動かしたくないボスには、 0x20 に指定します。\r\n逆に、移動する一般兵に、0x20を指定してはいけません。\r\n一般兵に指定すると、移動しないで攻撃できる場合にしか攻撃しないモードになってしまいます。\r\n");
            }
            else if (str == "@EVENTUNIT_AI3_FE6")
            {
                str = R._("ユニットを制御するAIを指定します。\r\nAI3は主に性格を定義します。\r\n残りHPでの回復アイテムを使うようになります。\r\n");
            }
            else if (str == "@EVENTUNIT_OPTION_FE8")
            {
                str = R._("「なし」: 通常はこれを指定してください。\r\n「アイテムドロップ」:敵がアイテムを落とすようになります。落とすアイテムは、一番下のアイテム欄にあるアイテムです。\r\n「魔物」:確率テーブルに従って魔物を設置します。クラスIDは確率テーブルへのIDとして動作します。\r\n「特殊」:イベント命令_0x2B21を使って、敵ユニットの位置をランダム化します。\r\n");
            }
            else if (str == "@EVENTUNIT_AFTER_FE7")
            {
                str = R._("ユニットを最終的に登場させる座標を指定します。\r\nユニットは、配置前座標から登場し、\r\nイベントで移動開始命令が指定されると、配置後座標まで移動します。\r\n");
            }
            else if (str == "@EVENTUNIT_BEFORE")
            {
                str = R._("ユニットを一番最初に登場させる座標を指定します。\r\nユニットは、配置前座標から登場し、\r\nイベントで移動開始命令が指定されると、配置後座標まで移動します。\r\n");
            }
            else if (str == "@EVENTUNIT_ITEM")
            {
                str = R._("ユニットの所持アイテムを設定します。\r\nユニットの一番上の武器がディフォルトの装備武器になります。\r\n\r\nユニットがアイテムを落とす設定にしている場合、\r\nユニットの一番下のアイテムがドロップアイテムになります。\r\n");
                if (Program.ROM.RomInfo.version() == 6)
                {
                    str += R._("FE6でアイテムを落とすようにするには、FE6-Droppable Itemsパッチを適応したあとで、AI4(退避AI)に0x40のビットフラグを立ててください。\r\n");
                }
                else if (Program.ROM.RomInfo.version() == 7)
                {
                    str += R._("FE7でアイテムを落とすようにするには、ユニットまたはクラスの特性4のドロップアイテムのビットを有効にしてください。\r\n");
                }
                else if (Program.ROM.RomInfo.version() == 8)
                {
                    str += R._("FE8でアイテムを落とすようにするには、ユニットの一番最初の座標データにある特殊設定をアイテムドロップに変更します。\r\n");
                }
            }
            else if (str == "@PALETTE_COLOR")
            {
                str = ImagePalletForm.GetExplainColor();
            }
            else if (str == "@PALETTE_COLOR0")
            {
                str = ImagePalletForm.GetExplainColor0();
            }
            else if (str == "@UNIT_SUPPORT")
            {
                str = R._("ユニットの支援データへのポインタを指定します。\r\n支援が不要な場合0になります。\r\n支援が必要な場合は、該当する支援データへのポインタを設定します。\r\n");
            }
            else if (str == "@UNIT_CONVERSATION_GROUP")
            {
                str = R._("ユニットが所属する会話グループへの設定を行います。\r\n村訪問などで、訪問したキャラクターがセリフをしゃべるときに、\r\n会話グループによって、セリフの口調を切り替えたい場合に利用します。\r\n");
            }
            else if (str == "@IMAGE_BATTLE_ANIME_CLASS_ANIMESET")
            {
                str = R._("武器ごとに利用するアニメーションを設定します。\r\n投げ槍は、槍の汎用アニメーションを利用できますが、\r\n投げ斧は、個別に指定する必要があります。\r\n詳しくは、斧を使うクラスの設定を参考にしてください。\r\n斧使いは、投げ斧を個別に設定することを、絶対に忘れないでください。\r\n");
            }
            else if (str == "@INNERNAME")
            {
                str = R._("名前を定義します。\r\nこの値はゲーム中からは参照されません。\r\n製作者が理解しやすいように設定する名前です。\r\n");
            }
            else if (str == "@BATTLEANIME_PALETTE")
            {
                str = R._("戦闘アニメーションの汎用色のパレットを定義します。\r\n戦闘アニメーションの汎用色パレットは、所属ごとに、自軍、敵、友軍、グレーの4種類があります。\r\n") + ImageUnitPaletteForm.GetExplainPaletteRule();
            }
            else if (str == "@UNIT_LOW_CLASS_PALETTE")
            {
                str = R._("{0}を定義します。\r\n0の場合は、戦闘アニメーションの汎用色のパレットが利用されます。", R._("下位クラス戦闘アニメ色")) + "\r\n" + ImageUnitPaletteForm.GetExplainPaletteRule();
            }
            else if (str == "@UNIT_HIGH_CLASS_PALETTE")
            {
                str = R._("{0}を定義します。\r\n0の場合は、戦闘アニメーションの汎用色のパレットが利用されます。", R._("下位クラス戦闘アニメ色")) + "\r\n" + ImageUnitPaletteForm.GetExplainPaletteRule();
            }
            else if (str == "@UNIT_FE8_CLASS_PALETTE")
            {
                str = R._("{0}を定義します。\r\nFE8では分岐{1}が導入されたため、別テーブルでクラスごとに設定することになっています。\r\n", R._("ユニット別パレット"), R._("クラスチェンジ")) + "\r\n" + ImageUnitPaletteForm.GetExplainPaletteRule();
            }
            else if (str == "@FOG")
            {
                str = R._("霧レべルを指定します。\r\n0: 霧を設定しません\r\n1: 視界1マス (最も強い霧)\r\n2: 視界2マス\r\n3: 視界3マス\r\n4: 視界4マス\r\n...\r\n\r\nレベルを上げるごとに視界が広がっていき、弱い霧になります。\r\n");
            }
            else if (str == "@MAPSETTING_BATTLE_BACKGROUND")
            {
                str = R._("戦闘背景を指定します。\r\nゲーム内の設定で、背景を有効にした場合に、利用されます。\r\n");
            }
            else if (str == "@WEATHER")
            {
                str = R._("天気を設定します。\r\n天気が変わると移動に必要な値が変化します。\r\n");
            }
            else if (str == "@UNIT_PORTRAIT")
            {
                str = R._("ユニットの顔画像を指定します。\r\nこの値を0にすると、一般兵扱いとなり、\r\nクラスの一般兵顔画像(クラスカード)が利用されます。\r\n");
            }
            else if (str == "@UNIT_MINI_PORTRAIT")
            {
                str = R._("マップ左上に表示する、小さいマップ顔を指定します。\r\nこの値を0にすると、ユニットの顔画像にあるマップ顔が利用されます。\r\n1以上にすると、一般兵となり、値に対応するシンボルマークが表示されます。\r\n");
            }
            else if (str == "@CLASS_PORTRAIT")
            {
                str = R._("一般兵の顔画像(クラスカード)を指定します。\r\nユニット設定で、顔画像を0にした場合、\r\nそのユニットは一般兵となり、このクラスカードの値が利用されます。\r\n");
            }
            else if (str == "@ITEM_DESCRIPTION")
            {
                str = R._("アイテムの詳細を書きます。\r\n武器の場合、1行の説明を書けます。\r\n杖とアイテムの場合、最大で3行までの説明を書くことができます。\r\n");
            }
            else if (str == "@ITEM_USES_DESCRIPTION")
            {
                str = R._("利用するときに表示される説明を書きます。\r\n杖とアイテムの場合にのみ利用されます。\r\n最大で3行までの説明を書くことができます。\r\n");
            }
            else if (str == "@X_UNITSKILL")
            {
                str = R._("ユニットごとに設定できるスキルを設定します。");
            }
            else if (str == "@X_CLASSSKILL")
            {
                str = R._("クラスごとに設定できるスキルを設定します。");
            }
            else if (str == "@PORTRAIT_MOUTH_COORD")
            {
                str = R._("キャラクターの口の座標を指定します。");
            }
            else if (str == "@PORTRAIT_EYE_COORD")
            {
                str = R._("キャラクターの目の座標を指定します。");
            }
            else if (str == "@EXTENDSMAGIC_LISTBOX")
            {
                str = R._("ユーザ定義の魔法エフェクトを追加します。\r\nリストが途中から始まっているように見えるのは、リストの先頭には通常の魔法や、弓、投げ槍などの間接攻撃のエフェクトで予約されているためです。\r\nここで定義した魔法を、間接エフェクトで設定すると、魔法を発動できます。\r\n");
            }
            else if (str == "@UNITMOVE_ANIME_AP")
            {
                str = R._("アニメ処理のために利用するROMTCSデータです。\r\n踊り子などの特殊なクラスを変更するときに、変更する必要があります。\r\nそれ以外は、変更しなくても問題はありません。\r\n");
            }
            else if (str == "@MAGICDIM")
            {
                str = R._("魔法発動時に、画面暗くするかどうか選択します。\r\ndim_pc=発動時に、画面を暗くします。\r\ndim=暗くしません。\r\nNULL=この魔法スロットを利用しないというフラグです。\r\n");
            }
            else if (str == "@ALTEVENT")
            {
                str = R._("テキストIDに0を指定した時だけ、ここで指定したイベントを実行できます。\r\nイベント命令のため、分岐をさせることで特殊な会話を実装できるようです。\r\nしかし、非常に制限が多いので、特別な理由がない限り利用しない方がいいでしょう。\r\nイベントを設定する場合は、イベント領域を確保して指定してください。\r\n");
            }
            else if (str == "@COMMENT")
            {
                str = R._("自由に設定できるコメントです。\r\n開発時のメモに利用してください。\r\nこの内容はROMには保存されません。以下のファイルに記録されます。") + "\r\nconfig/etc/ROMNAME/comment_.txt";
            }
            else if (str == "@DEBUG_SYMBOL")
            {
                str = R._("デバッグ用のシンボルをコメントとして格納します。\r\nシンボルを埋め込むと、逆アセンブルやデバッグのヒントになります。\r\nこの内容はROMには保存されません。以下のファイルに記録されます。") + "\r\nconfig/etc/ROMNAME/comment_.txt";
            }
            else if (str == "@ITEM_WLEVEL")
            {
                str = R._("武器を使用できる武器レベルを指定します。\r\nこのレベル以上であれば、武器を利用できます。") ;
            }
            else if (str == "@ITEM_RANGE")
            {
                str = R._("武器の射程を指定します。\r\n右側のリストから、射程を選択することができます。\r\n\r\nリストにない射程の武器を作りたい場合は、数字を直接入力してください。\r\n最大射程と最少射程を16進数で書き込みます。\r\n例えば、最小射程が「5」で、最大射程「10」、だったとしたら、「5A」になります。");
            }
            else if (str == "@SHOW_LOW_COMMAND")
            {
                str = R._("チェックをつけると、低レイヤーの命令を表示します。\r\n低レイヤー命令は、他の命令と組み合わせて利用する命令なので、ディフォルトでは非表示になっています。");
            }
            else if (str == "@MAPSETTING_DEBRIS_FE7")
            {
                str = R._("FE7の残骸データですが、スコアの計算は実行されています。\r\nただし、スコアを表示する画面がないため表示できませんでした。\r\nFE8Jで画面の表示方法が確立できたので、画面を復活させました。\r\nそれぞれのパラメータの内容は現在調査中です。");
            }
            else if (str == "@TRANSLATE_FROM_ROM")
            {
                str = R._("翻訳のヒントに使うROMを指定します。\r\n無変更のテキストを取り出して比較するのに利用します。");
            }
            else if (str == "@TRANSLATE_TO_ROM")
            {
                str = R._("翻訳のヒントに使うROMを指定します。\r\n無変更のテキストの翻訳文を取り出すために利用します。");
            }
            else if (str == "@TRANSLATE_HINT_FILE")
            {
                str = R._("そのゲーム専用の翻訳データがあれば指定してください。\r\n");
            }
            else if (str == "@SOUNDROOM_SOUNGTIME")
            {
                str = R._("曲の長さは、ランダム再生時に利用されます。\r\n時間はミリ秒で指定します。\r\nただし内部ではフレーム秒で処理され端数は切り捨てられます。\r\n指定した時間に達すると曲がフェードアウトし、次の曲に切り替わります。");
            }
            else if (str == "@EXPLAIN_UNITICON_SYSTEM_PALETTE")
            {
                str = R._("ユニットのパレットを変更はシステムアイコンから行えます。\r\nただし、パレットは全ユニット共通になるので注意してください。");
            }
            else if (str == "@EXPLAIN_ITEMICON_SYSTEM_PALETTE")
            {
                str = R._("アイテムのパレットを変更はシステムアイコンから行えます。\r\nただし、パレットは全アイテム共通になるので注意してください。");
            }
            else if (str == "@OP_CLASSDEMO_BATTLEANIME_PALETTE")
            {
                str = R._("指定したID+1のカスタムパレットで描画されます。\r\nカスタムパレットを利用しない場合は、0xFFを指定してください。\r\n注意:0x80以上のパレットを指定すると無視されます。");
            }
            else if (str == "@OP_CLASSDEMO_BATTLEANIME")
            {
                str = R._("指定したID+1の戦闘アニメで描画されます。");
            }
            else if (str == "@CLASS_POWER")
            {
                str = R._("敵との差が大きければ、経験値をより獲得できます。");
                str += "\r\n";
                if (Program.ROM.RomInfo.version() == 6)
                {
                    str += R._("この値が関係するのは、{}で囲まれた部分です。\r\n未撃破経験値　　＝（31＋相手のLV＋相手の上級職補正－自分のLV－自分の上級職補正）÷{{自分の経験値補正値}}\r\n基礎撃破経験値　＝（相手のLV×{{相手の経験値補正値}}＋{{相手の階級修正B}}）－（自分のLV×{{自分の経験値補正値}}＋{{自分の階級修正B}}）÷プレイモード係数\r\n撃破経験値　　　＝未撃破経験値＋max（0，基礎撃破経験値＋20＋ボス修正＋シーフ修正）");
                }
                else if (Program.ROM.RomInfo.version() == 7)
                {
                    str += R._("この値が関係するのは、{}で囲まれた部分です。\r\n未撃破経験値　　＝（31＋相手のLV＋相手の上級職補正－自分のLV－自分の上級職補正）÷{{自分の経験値補正値}}\r\n基礎撃破経験値　＝（相手のLV×{{相手の経験値補正値}}＋{{相手の階級修正B}}）－（自分のLV×{{自分の経験値補正値}}＋{{自分の階級修正B}}）÷プレイモード係数\r\n撃破経験値　　　＝未撃破経験値＋max（0，基礎撃破経験値＋20＋ボス修正＋シーフ修正）×瞬殺係数");
                }
                else if (Program.ROM.RomInfo.version() == 8)
                {
                    str += R._("この値が関係するのは、{}で囲まれた部分です。\r\n未撃破経験値　　＝（31＋相手のLV＋相手の上級職補正－自分のLV－自分の上級職補正）÷{{自分の経験値補正値}}\r\n基礎撃破経験値　＝（相手のLV×{{相手の経験値補正値}}＋{{相手の階級修正B}}）－（自分のLV×{{自分の経験値補正値}}＋{{自分の階級修正B}}）÷プレイモード係数\r\n撃破経験値　　　＝未撃破経験値＋max（0，基礎撃破経験値＋20＋ボス修正＋シーフ修正＋マミー修正）×瞬殺係数");
                }
            }
            else if (str == "@MAPSETTING_RANK_TACT")
            {
                str = R._("指定したターン以内にクリアすれば評価を得られます。");
            }
            else if (str == "@MAPSETTING_RANK_EXP")
            {
                str = R._("指定した経験値を得ていれば評価を得られます。");
            }
            else if (str == "@MAPSETTING_RANK_ASSET")
            {
                str = R._("指定したゴールド相当の資産を得られていればA評価を貰えます。\r\n達成していなければ、達成できた割合で評価が決まります。");
            }
            else if (str == "@MAPCHANGE_SIZE")
            {
                str = R._("タイルデータのサイズを指定します。\r\n\r\n注意:\r\n手動で、より大きい値に変更すると危険です。\r\nデータサイズを増やしたいときは、マップエディタから変更してください。");
            }
            else if (str == "@ADSR")
            {
                str = R._("シンセサイザーで利用される4つのパラメータを設定します。\r\nAttack(音の立ち上がり時間)Decay(持続音への減衰時間)\r\nSustain(減衰後の保持,持続音量)\r\nRelease(余韻)\r\n詳しく知りたい場合は、「ADSR dtm」で検索してください。");
            }
            else if (str == "@EXPLAIN_ANIMATION_POINTER")
            {
                str = R._("ユニットの戦闘アニメで利用するデータへのポインタを指定します。\r\n武器や魔道書ごとにどんな戦闘アニメを再生するかを決定します。\r\n");
            }
            else if (str == "@EXPLAIN_MOVEMENTCOST_POINTER")
            {
                str = R._("ユニットが移動できるタイルへのポインタを指定します。\r\n")
                    + R._("晴れた日のマップでの移動コストを定義します。\r\n各タイルごとの移動コストがどれだけかかるかを定義します。\r\n山を飛び越える飛行ユニットや、高い山に登れる山賊などの特性を設定します。\r\n");
            }
            else if (str == "@EXPLAIN_MOVEMENTCOST_RAIN_POINTER")
            {
                str = R._("雨が降っているマップでの移動コストを指定します。\r\n一般的に、雨が降ると移動力コストが上昇し、ユニットは移動しにくくなります。\r\n");
            }
            else if (str == "@EXPLAIN_MOVEMENTCOST_SNOW_POINTER")
            {
                str = R._("雪が降っているマップでの移動コストを指定します。\r\n");
            }
            else if (str == "@EXPLAIN_MOVEMENTCOST")
            {
                str = R._("晴れた日のマップでの移動コストを定義します。\r\n各タイルごとに移動コストがどれだけかかるかを定義します。\r\n山を飛び越える飛行ユニットや、高い山に登れる山賊などの特性を設定します。\r\n")
                    + "\r\n"
                    + R._("地形タイルごとに移動コストを設定します。\r\n平地は1、森は2などと指定します。\r\n255は移動できないタイルです。\r\n左上の--のタイルは必ず255である必要があります。\r\n");
            }
            else if (str == "@EXPLAIN_MOVEMENTCOST_RAIN")
            {
                str = R._("雨が降っているマップでの移動コストを指定します。\r\n一般的に、雨が降ると移動力コストが上昇し、ユニットは移動しにくくなります。\r\n")
                    +"\r\n"
                    + R._("地形タイルごとに移動コストを設定します。\r\n平地は1、森は2などと指定します。\r\n255は移動できないタイルです。\r\n左上の--のタイルは必ず255である必要があります。\r\n");
            }
            else if (str == "@EXPLAIN_MOVEMENTCOST_SNOW")
            {
                str = R._("雪が降っているマップでの移動コストを指定します。\r\n")
                    + "\r\n"
                    + R._("地形タイルごとに移動コストを設定します。\r\n平地は1、森は2などと指定します。\r\n255は移動できないタイルです。\r\n左上の--のタイルは必ず255である必要があります。\r\n");
            }
            else if (str == "@EXPLAIN_MOVEMENTCOST_AVO")
            {
                str = R._("地形ごとの回避ボーナスを指定します。\r\n森や砦などに設定されます。\r\n飛行ユニットは地形ボーナスを受けられない等を表現します。\r\n");
            }
            else if (str == "@EXPLAIN_MOVEMENTCOST_DEF")
            {
                str = R._("地形ごとの防御ボーナスを指定します。\r\n砦や城門などに設定されます。\r\n");
            }
            else if (str == "@EXPLAIN_MOVEMENTCOST_RES")
            {
                str = R._("地形ごとの魔防ボーナスを指定します。\r\n城門や魔法陣などに設定されます。\r\n");
            }
            else if (str == "@EXPLAIN_MOVEMENTCOST_HEALING")
            {
                str = R._("地形ごとの回復ボーナスを指定します。\r\n砦や城門にいると回復するという設定に利用されます。\r\nこの設定はすべてのクラスで共通しています。\r\n個別に指定することはできません。\r\n回復するHPの値を指定します。\r\n");
            }
            else if (str == "@EXPLAIN_MOVEMENTCOST_RECOVERY")
            {
                str = R._("地形ごとの毒や睡眠などのバットステータスを自動回復するボーナスを指定します。\r\n城門にいると回復するという設定に利用されます。\r\nこの設定はすべてのクラスで共通しています。\r\n個別に指定することはできません。\r\n1を設定すると有効になります。\r\nその地形にいるユニットのバッドステータスが自動的に回復します。\r\n");
            }
            else if (str == "@HARDCODING_WARNING")
            {
                str = R._("このデータに対するハードコーディングがあります。\r\n詳しくは、ラベルをクリックして、関連するパッチを見てください。\r\n");
            }
            else if (str == "@WALL_OR_SNAG_HP")
            {
                str = R._("壊れる壁と古木のHPを設定します。\r\n最大で120まで設定できます。\r\n");
            }
            else if (str == "@UNINSTALL_EA")
            {
                str = R._("直近にインストールしたスクリプトを消す場合は、Undoを選択してください。\r\n過去にインストールしたスクリプトを消す場合は、アンインストールを選択してください。\r\n");
            }
            else if (str == "@SOUNDROOM_SONGID")
            {
                str = R._("サウンドルームに登録できるのは、SongID 0x7fまでの曲です。\r\nそれ以上のSongIDを使う場合は、「サウンドルーム全開」パッチを利用してください。\r\nそうしないと、それらの曲はUnLockされない可能性があります。\r\n");
            }
            else if (str == "@CHANGEVELOCITY")
            {
                str = R._("通常は利用しないでください。\r\nボリュームを変更しても、音量がまったく反映されない場合にのみ利用してください。\r\n");
            }
            else if (str == "@MAPEDITOR_ID")
            {
                str = R._("タイル変化のIDです。\r\n無駄な混乱を避けるためリストの並び順と同じIDを割り振ることを推奨します。\r\nこの値を0xFFにするとリスト終端として扱われます。");
            }
            else if (str == "@SONGDATAFINGERPRINT")
            {
                str = R._("楽器データを識別するための値です。\r\nもし、まだ名前が設定されていない楽器の名前を知っている場合は、\r\nこのテキストの内容と、楽器名をコミニティに報告してください。\r\n");
            }
            else
            {
                //未定義のヒント
                Debug.Assert(false);
            }
            return str;
        }

        static ToolTipEx NewTooltip(Form self, List<Control> controls)
        {
            ToolTipEx tooltip = new ToolTipEx();

            for (int i = 0; i < controls.Count; i++)
            {
                Control c = controls[i];
                if (c is TextBoxEx)
                {
                    ((TextBoxEx)c).SetToolTipEx(tooltip);
                }
                else if (c is PanelEx)
                {
                    ((PanelEx)c).SetToolTipEx(tooltip);
                }
                else if (c is LabelEx)
                {
                    ((LabelEx)c).SetToolTipEx(tooltip);
                }

                string str = c.AccessibleDescription;
                if (str == null || str == "")
                {
                    continue;
                }
                if (str[0] == '@')
                {//何度も出てくる定型文

                    //定型文の場合、翻訳されているものがもらえるのでそのまま追加.
                    str = GetExplain(str);
                    tooltip.SetToolTip(c, str);
                }
                else
                {//翻訳リソースを利用して追加.
                    str = str.Replace("\\r\\n", "\r\n");
                    str = R._(str);
                    tooltip.SetToolTip(c, str);
                }
                c.AccessibleDescription = null;
            }


            return tooltip;
        }

        public static void LoadCheckboxesResource(string datename
            , List<Control> controls
            , ToolTipEx tooltip
            , string prefix
            , string target0, string target1, string target2, string target3)
        {
            Dictionary<uint, string> dic = InputFormRef.ConfigDataDatanameCache(datename);
            foreach (var pair in dic)
            {
                uint key = pair.Key;
                string bit = "";
                switch (key & 0xf)
                {
                    case 1:
                        bit = "01";
                        break;
                    case 2:
                        bit = "02";
                        break;
                    case 3:
                        bit = "04";
                        break;
                    case 4:
                        bit = "08";
                        break;
                    case 5:
                        bit = "10";
                        break;
                    case 6:
                        bit = "20";
                        break;
                    case 7:
                        bit = "40";
                        break;
                    case 8:
                        bit = "80";
                        break;
                }
                string name = "";
                if (key < 0x10)
                {//L_8_BIT_
                    name = target0 + bit;
                }
                else if (key < 0x20)
                {
                    name = target1 + bit;
                }
                else if (key < 0x30)
                {
                    name = target2 + bit;
                }
                else if (key < 0x40)
                {
                    name = target3 + bit;
                }
                else
                {
                    continue;
                }
                Control c = FindObject(prefix, controls, name);
                if ( !(c is CheckBox) )
                {
                    continue;
                }

                string[] sp = pair.Value.Split('\t');
                if (c.Text.Length <= sp[0].Length)
                {
                    c.Text = sp[0];
                }

                if (sp.Length > 1)
                {
                    string hint = sp[1].Replace("\\r\\n", "\r\n");
                    tooltip.SetToolTipIfNew(c, hint);
                }
            }
        }
        public static EventHandler MakeAddressListExpandsCallback_Handler(NumericUpDown injectionCallback)
        {
            EventHandler eventHandler =
            (object sender, EventArgs e) =>
            {
                InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)e;
                uint addr = eearg.NewBaseAddress;

                U.ForceUpdate(injectionCallback, U.toPointer(addr));
            };
            return eventHandler;
        }

        public static EventHandler MakeAddressListFlagExpandsCallback_Handler(NumericUpDown injectionCallback)
        {
            EventHandler eventHandler =
            (object sender, EventArgs e) =>
            {
                InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)e;
                uint addr = eearg.NewBaseAddress;

                U.ForceUpdate(injectionCallback, addr);
            };
            return eventHandler;
        }

        public static void MakeEditListboxContextMenu(ListBox listbox, KeyEventHandler keydownfunc)
        {
            ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
            MenuItem menuItem;
            menuItem = new MenuItem(R._("元に戻す(&Z)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,keydownfunc, Keys.Control | Keys.Z));
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("削除(DEL)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,keydownfunc, Keys.Delete));
            contextMenu.MenuItems.Add(menuItem);
            menuItem = new MenuItem(R._("コピー(&C)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,keydownfunc, Keys.Control | Keys.C));
            contextMenu.MenuItems.Add(menuItem);
            menuItem = new MenuItem(R._("貼り付け(&V)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,keydownfunc, Keys.Control | Keys.V));
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("編集画面を出す(ENTER)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,keydownfunc, Keys.Control | Keys.Enter));
            contextMenu.MenuItems.Add(menuItem);

            listbox.ContextMenu = contextMenu;
        }

        public static void MakeEditListboxContextMenuN(ListBox listbox, KeyEventHandler keydownfunc, KeyEventHandler parentFormKeydownfunc , bool useTemplate)
        {
            ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
            MenuItem menuItem;
            menuItem = new MenuItem(R._("元に戻す(&Z)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,keydownfunc, Keys.Control | Keys.Z));
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("削除(DEL)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,keydownfunc, Keys.Delete));
            contextMenu.MenuItems.Add(menuItem);
            menuItem = new MenuItem(R._("コピー(&C)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,keydownfunc, Keys.Control | Keys.C));
            contextMenu.MenuItems.Add(menuItem);
            menuItem = new MenuItem(R._("貼り付け(&V)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,keydownfunc, Keys.Control | Keys.V));
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("ジャンプ(&J)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,keydownfunc, Keys.Control | Keys.J));
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("ジャンプ2"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,keydownfunc, Keys.Alt | Keys.J));
            contextMenu.MenuItems.Add(menuItem);


            if (useTemplate)
            {
                menuItem = new MenuItem("-");
                contextMenu.MenuItems.Add(menuItem);

                menuItem = new MenuItem(R._("テンプレート(&T)"));
                menuItem.Click += new EventHandler(U.FireKeyDown(listbox, keydownfunc, Keys.Control | Keys.Alt  | Keys.T));
                contextMenu.MenuItems.Add(menuItem);
            }

            menuItem = new MenuItem("-");
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("コメント編集(&/)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,parentFormKeydownfunc, Keys.Control | Keys.Divide));
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("直接編集(&N)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,parentFormKeydownfunc, Keys.Control | Keys.N));
            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem(R._("編集画面を出す(ENTER)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(listbox,keydownfunc, Keys.Control | Keys.Enter));
            contextMenu.MenuItems.Add(menuItem);

            listbox.ContextMenu = contextMenu;
        }

        //削除を使える
        bool UseMenuDeleteAction = false;

        public void MakeGeneralAddressListContextMenu(bool useUpDown = true, bool useClear = false, KeyEventHandler keyDown = null)
        {
            ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
            MenuItem menuItem;
            if (keyDown == null)
            {
                keyDown = this.GeneralAddressList_KeyDown;
            }
            this.AddressList.KeyDown += keyDown;
            
            menuItem = new MenuItem(R._("コピー(&C)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(this.AddressList, keyDown, Keys.Control | Keys.C));
            contextMenu.MenuItems.Add(menuItem);
            menuItem = new MenuItem(R._("貼り付け(&V)"));
            menuItem.Click += new EventHandler(U.FireKeyDown(this.AddressList, keyDown, Keys.Control | Keys.V));
            contextMenu.MenuItems.Add(menuItem);

            if (useUpDown)
            {
                menuItem = new MenuItem("-");
                contextMenu.MenuItems.Add(menuItem);

                menuItem = new MenuItem(R._("↑データ入れ替え(Ctrl + Up)"));
                menuItem.Click += new EventHandler(U.FireKeyDown(this.AddressList, keyDown, Keys.Control | Keys.Up));
                contextMenu.MenuItems.Add(menuItem);
                menuItem = new MenuItem(R._("↓データ入れ替え(Ctrl + Down)"));
                menuItem.Click += new EventHandler(U.FireKeyDown(this.AddressList, keyDown, Keys.Control | Keys.Down));
                contextMenu.MenuItems.Add(menuItem);

                this.AddressList.AllowDrop = true;
                this.AddressList.MouseDown += new MouseEventHandler(ListBoxDD_MouseDown);
                this.AddressList.MouseMove += new MouseEventHandler(ListBoxDD_MouseMove);
                this.AddressList.DragEnter += new DragEventHandler(ListBoxDD_DragEnter);
                this.AddressList.DragDrop += new DragEventHandler(ListBoxDD_DragDrop);
            }
            if (useClear)
            {
                menuItem = new MenuItem("-");
                contextMenu.MenuItems.Add(menuItem);

                menuItem = new MenuItem(R._("無効化する(DEL)"));
                menuItem.Click += new EventHandler(U.FireKeyDown(this.AddressList, keyDown, Keys.Delete));
                contextMenu.MenuItems.Add(menuItem);

                this.UseMenuDeleteAction = true;
            }

            this.AddressList.ContextMenu = contextMenu;
        }

        Rectangle DragBoxFromMouseDown = Rectangle.Empty;
        private void ListBoxDD_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.AddressList.SelectedItem == null)
            {
                this.DragBoxFromMouseDown = Rectangle.Empty;
                return;
            }
            if (e.Button != MouseButtons.Left)
            {
                this.DragBoxFromMouseDown = Rectangle.Empty;
                return;
            }
            // Remember the point where the mouse down occurred. The DragSize indicates
            // the size that the mouse can move before a drag event should be started.                
            Size dragSize = SystemInformation.DragSize;

            // Create a rectangle using the DragSize, with the mouse position being
            // at the center of the rectangle.
            this.DragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                           e.Y - (dragSize.Height / 2)), dragSize);
        }

        void ListBoxDD_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
            {
                this.DragBoxFromMouseDown = Rectangle.Empty;
                return;
            }
            // If the mouse moves outside the rectangle, start the drag.
            if (this.DragBoxFromMouseDown == Rectangle.Empty ||
                this.DragBoxFromMouseDown.Contains(e.X, e.Y))
            {
                return ;
            }

            //ドラッグを許可するので、マウスの位置は初期化する
            this.DragBoxFromMouseDown = Rectangle.Empty;

            int startIndex = this.AddressList.SelectedIndex;
            if (startIndex < 0)
            {
                return;
            }
            if (this.UseWriteProtectionID00)
            {
                if (startIndex == 0)
                {
                    return;
                }
            }

            this.AddressList.DoDragDrop(this.GetHashCode(), DragDropEffects.Move);
        }

        private void ListBoxDD_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(int)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            int hash = (int)e.Data.GetData(typeof(int));
            if (hash != this.GetHashCode())
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Move;
        }

        private void ListBoxDD_DragDrop(object sender, DragEventArgs e)
        {
            this.DragBoxFromMouseDown = Rectangle.Empty;

            Point point = this.AddressList.PointToClient(new Point(e.X, e.Y));
            int targetIndex = this.AddressList.IndexFromPoint(point);
            if (targetIndex < 0)
            {
                targetIndex = this.AddressList.Items.Count - 1;
            }

            int startIndex = this.AddressList.SelectedIndex;
            if (startIndex == targetIndex)
            {
                return;
            }

            if (this.UseWriteProtectionID00)
            {
                if (targetIndex == 0 || startIndex == 0)
                {
                    R.ShowStopError(R._("00のデータの変更は許可されていません。"));
                    return;
                }
            }

            DragAndDropData(startIndex, targetIndex);
        }

        void ShowDumpSelectDialogHandler(Object sender,EventArgs e)
        {
            DumpStructSelectDialogForm.ShowDumpSelectDialog(this,this.SelectAddressTextBox.Text);
        }

        public static bool ShowZeroPointerPanel(ListBox addressList, NumericUpDown nud)
        {
            if (addressList != null)
            {
                if (addressList.SelectedIndex <= 0)
                {
                    return false;
                }
            }
            if (nud.Value == 0)
            {
                return true;
            }
            return false;
        }

        //リフレクションを利用して、Form型から、ポインタ情報を高速に取得するルーチン
        //ポインタであるカラムだけを取れます。
        //性質上 4バイトアライメントがあるため、データはその制約を受けるものだと思われる.
        public static uint[] MakePointerByteIndexArray(Type q,string prefix)
        {
            List<uint> indexes = new List<uint>();
            Debug.Assert(q.IsClass);
            Debug.Assert(q.Name.LastIndexOf("Form") == q.Name.Length - 4);

            const BindingFlags FINDS_FLAG = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo[] fields = q.GetFields(FINDS_FLAG);
            foreach(FieldInfo f in fields)
            {
                string name = f.Name;
                if (name.IndexOf("L_") >= 0 || name.IndexOf("J_") >= 0)
                {
                    continue;
                }

                //string prefix = InputFormRef.GetPrefixName(name);
                uint id = InputFormRef.GetStructID(prefix, name);
                if (id == U.NOT_FOUND)
                {
                    continue;
                }

                string a = InputFormRef.SkipPrefixName(name,prefix);
                if (a.Length <= 0)
                {
                    continue;
                }
                else if (a[0] == 'D')
                {
                    indexes.Add(id);
                }
                else if (a[0] == 'P')
                {
                    indexes.Add(id);
                }
                else
                {//不明
                    continue;
                }
            }
            return indexes.ToArray();
        }

        public void SaveDumpAutomatic(
              StringBuilder sb
            , string saveDir)
        {
            string fullpath;
            fullpath = DumpStructSelectDialogForm.SaveDumpAutomatic(this, DumpStructSelectDialogForm.Func.Func_NMM, saveDir); //NMM
            sb.AppendLine("//" + fullpath);///No Translate
            fullpath = DumpStructSelectDialogForm.SaveDumpAutomatic(this, DumpStructSelectDialogForm.Func.Func_CSV, saveDir); //CSV
            sb.AppendLine("//" + fullpath);///No Translate
            sb.AppendLine();
        }

        public string GetComment(uint id)
        {
            uint addr = this.IDToAddr(id);
            if (addr == U.NOT_FOUND)
            {
                return "";
            }
            return Program.CommentCache.At(addr);
        }
        public static string GetCommentSA(uint addr)
        {
            return Program.CommentCache.S_At(addr);
        }
        public static void AppendEvent_CopyAddressToDoubleClick(Control v)
        {
            v.MouseDoubleClick += (self, ee ) => {
                PointerToolCopyToForm f = (PointerToolCopyToForm)InputFormRef.JumpFormLow<PointerToolCopyToForm>();
                f.Init((uint)U.atoh(v.Text));
                f.ShowDialog();
            };
        }
        public static Size DrawRefTextList(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            UseValsID t = (UseValsID)lb.Items[index];

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            SolidBrush keywordBrush = new SolidBrush(OptionForm.Color_Keyword_ForeColor());

            Font normalFont = lb.Font;
            Font boldFont = new Font(lb.Font, FontStyle.Bold);

            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;
            int maxWidth = 0;

            string text = MainSimpleMenuEventErrorForm.TypeToString(t.DataType, t.Addr, t.Tag);

            U.DrawText(text, g, boldFont, keywordBrush, isWithDraw, bounds);
            bounds.Y += lineHeight;

            text = t.Info;
            bounds.X = listbounds.X;
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

            brush.Dispose();
            boldFont.Dispose();

            bounds.X = maxWidth;
            bounds.Y += lineHeight + lineHeight;
            return new Size(bounds.X, bounds.Y);
        }
        public static void GotoRef(ListBox refListBox)
        {
            int index = refListBox.SelectedIndex;
            if (index < 0 || index >= refListBox.Items.Count)
            {
                return;
            }
            UseValsID t = (UseValsID)refListBox.Items[index];
            MainSimpleMenuEventErrorForm.GotoEvent(t.DataType, t.Addr, t.Tag, U.NOT_FOUND);
        }
        public static bool UpdateRef(ListBox refListBox, uint id,UseValsID.TargetTypeEnum targetType)
        {
            AsmMapFile map = Program.AsmMapFileAsmCache.GetAsmMapFile();
            List<UseValsID> textIDList = map.GetVarsIDArray();
            if (textIDList == null)
            {
                refListBox.BeginUpdate();
                refListBox.Items.Clear();
                refListBox.Items.Add(UseValsID.GetNowMeasuring());
                refListBox.EndUpdate();
                return false;
            }

            refListBox.BeginUpdate();
            refListBox.Items.Clear();

            int count = textIDList.Count;
            for (int i = 0; i < count; i++)
            {
                UseValsID t = textIDList[i];
                if (t.TargetType != targetType
                    || t.ID != id)
                {
                    continue;
                }
                refListBox.Items.Add(t);
            }

            refListBox.EndUpdate();
            return true;
        }
    }
}
