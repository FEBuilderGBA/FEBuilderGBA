using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FEBuilderGBA
{
    public partial class EventCondForm : Form
    {
        public EventCondForm()
        {
            InitializeComponent();
            EventRelationIconsCache = new Dictionary<uint,EventRelationIcons>();

            for (int i = 0; i < EventCondForm.MapCond.Count; i++)
            {
                FilterComboBox.Items.Add(EventCondForm.MapCond[i].Name);
            }
            SupportOtherSeries();

            U.SelectedIndexSafety(this.FilterComboBox,0);

            this.InputFormRefN02 = InitN02(this);
            this.InputFormRefN02.PreWriteHandler += PreWriteHandler_N02_W0;
            this.InputFormRefN02.PostWriteHandler += OnPostWriteHandler_ClearCache;
            this.InputFormRefN02.AddressListExpandsEvent += AddressListExpandsEventNoCopyP4;
            this.InputFormRefN02.AddressList.OwnerDraw(DrawEventListTurn, DrawMode.OwnerDrawFixed);
            this.InputFormRefN02.MakeGeneralAddressListContextMenu(true, true, (sender, e) => {
                this.CustomKeydownHandler(sender, e, this.InputFormRefN02);
            });


            this.InputFormRefTalk = InitTalk(this);
            this.InputFormRefTalk.PreWriteHandler += PreWriteHandler_TALK_W0_UNIONKEY;
            this.InputFormRefTalk.PostWriteHandler += OnPostWriteHandler_ClearCache;
            this.InputFormRefTalk.AddressListExpandsEvent += AddressListExpandsEventNoCopyP4;
            this.InputFormRefTalk.AddressList.OwnerDraw(DrawEventListTalk, DrawMode.OwnerDrawFixed);
            this.InputFormRefTalk.MakeGeneralAddressListContextMenu(true, true, (sender, e) =>{
                this.CustomKeydownHandler(sender, e, this.InputFormRefTalk);
            });

            this.InputFormRefObject = InitObject(this);
            this.InputFormRefObject.PreWriteHandler += PreWriteHandler_OBJECT_W0_UNIONKEY;
            this.InputFormRefObject.PostWriteHandler += OnPostWriteHandler_ClearCache;
            this.InputFormRefObject.AddressListExpandsEvent += AddressListExpandsEventNoCopyP4;
            this.InputFormRefObject.AddressList.OwnerDraw(DrawEventListObject, DrawMode.OwnerDrawFixed);
            this.InputFormRefObject.MakeGeneralAddressListContextMenu(true, true, (sender, e) => {
                this.CustomKeydownHandler(sender, e, this.InputFormRefObject);
            });

            this.InputFormRefAlways = InitAlways(this);
            this.InputFormRefAlways.PreWriteHandler += PreWriteHandler_ALWAYS_W0_UNIONKEY;
            this.InputFormRefAlways.PostWriteHandler += OnPostWriteHandler_ClearCache;
            this.InputFormRefAlways.AddressListExpandsEvent += AddressListExpandsEventNoCopyP4;
            this.InputFormRefAlways.AddressList.OwnerDraw(DrawEventListAlways, DrawMode.OwnerDrawFixed);
            this.InputFormRefAlways.MakeGeneralAddressListContextMenu(true, true, (sender, e) =>{
                this.CustomKeydownHandler(sender, e, this.InputFormRefAlways);
            });

            this.InputFormRefTrap = InitTrap(this);
            this.InputFormRefTrap.PostWriteHandler += OnPostWriteHandler_ClearCache;
            this.InputFormRefTrap.AddressListExpandsEvent += AddressListExpandsEventTrap;
            this.InputFormRefTrap.AddressList.OwnerDraw(DrawEventListTrap, DrawMode.OwnerDrawFixed);
            this.InputFormRefTrap.MakeGeneralAddressListContextMenu(true, true, (sender, e) => {
                this.CustomKeydownHandler(sender, e, this.InputFormRefTrap);
            });

            //For FE8  他シリーズでもとりあえず初期化だけはやる.
            this.InputFormRefTutorial = InitTutorial(this);
            this.InputFormRefTutorial.PostWriteHandler += OnPostWriteHandler_ClearCache;
            this.InputFormRefTutorial.AddressListExpandsEvent += AddressListExpandsEventNoCopyP0;
            this.InputFormRefTutorial.MakeGeneralAddressListContextMenu(true, true, (sender, e) =>{
                this.CustomKeydownHandler(sender, e, this.InputFormRefTutorial);
            });

            //イベント条件の指定を忘れる人が多いので、アイコンをつけて目立たせる.
            this.FilterComboBox.OwnerDraw(DrawFilterCombo, DrawMode.OwnerDrawFixed);

            this.MAP_LISTBOX.OwnerDraw(ListBoxEx.DrawTextOnly, DrawMode.OwnerDrawFixed);
            InputFormRef.TabControlHideTabOption(CondTabControl);

            //マップIDリストを作る.
            U.ConvertListBox(MapSettingForm.MakeMapIDList(), ref  this.MAP_LISTBOX);
            //マップを最前面に移動する.
            MapPictureBox.BringToFront();
        }

        void SupportOtherSeries()
        {
            OBJECT_L_0_COMBO.OwnerDraw(ComboBoxEx.DrawIconAndText, DrawMode.OwnerDrawFixed);
            OBJECT_L_0_COMBO.AddIcon(0x5, ImageSystemIconForm.Throne()); //05=制圧ポイントと民家
            OBJECT_L_0_COMBO.AddIcon(0x6,ImageSystemIconForm.Village()); //06=訪問村
            OBJECT_L_0_COMBO.AddIcon(0x7,ImageSystemIconForm.Chest()); //07=宝箱
            OBJECT_L_0_COMBO.AddIcon(0x8,ImageSystemIconForm.Door()); //08=扉
            OBJECT_L_0_COMBO.AddIcon(0xA,ImageSystemIconForm.Vendor()); //0A=店

            TRAP_L_0_COMBO.OwnerDraw(ComboBoxEx.DrawIconAndText, DrawMode.OwnerDrawFixed);
            TRAP_L_0_COMBO.AddIcon(0x1, ImageSystemIconForm.BaristaIcon()); //01=アーチ配置
            TRAP_L_0_COMBO.AddIcon(0x4, ImageSystemIconForm.Cursol()); //04=ダメージ床
            TRAP_L_0_COMBO.AddIcon(0x7, ImageSystemIconForm.Cursol()); //07=神の矢
            TRAP_L_0_COMBO.AddIcon(0x8, ImageSystemIconForm.Cursol()); //08=炎
            TRAP_L_0_COMBO.AddIcon(0x8, ImageItemIconForm.DrawIconWhereID(Program.ROM.RomInfo.itemicon_mine_id())); //0B=地雷
            TRAP_L_0_COMBO.AddIcon(0x8, ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x53, 2, true)); //0C=ゴーゴンの卵

            OBJECT_N05_L_10_COMBO.OwnerDraw(ComboBoxEx.DrawIconAndText, DrawMode.OwnerDrawFixed);
            OBJECT_N06_L_10_COMBO.OwnerDraw(ComboBoxEx.DrawIconAndText, DrawMode.OwnerDrawFixed);
            OBJECT_N07_L_10_COMBO.OwnerDraw(ComboBoxEx.DrawIconAndText, DrawMode.OwnerDrawFixed);
            OBJECT_N08_L_10_COMBO.OwnerDraw(ComboBoxEx.DrawIconAndText, DrawMode.OwnerDrawFixed);
            OBJECT_N0A_L_10_COMBO.OwnerDraw(ComboBoxEx.DrawIconAndText, DrawMode.OwnerDrawFixed);

            TALK_L_0_COMBO.OwnerDraw(ComboBoxEx.DrawIconAndText, DrawMode.OwnerDrawFixed);
            N02_L_0_COMBO.OwnerDraw(ComboBoxEx.DrawIconAndText, DrawMode.OwnerDrawFixed);
            ALWAYS_L_0_COMBO.OwnerDraw(ComboBoxEx.DrawIconAndText, DrawMode.OwnerDrawFixed);
            N02_L_10_COMBO.OwnerDraw(ComboBoxEx.DrawIconAndText, DrawMode.OwnerDrawFixed);

            TALK_L_0_COMBO.AddIcon(0x3, ImageSystemIconForm.TalkIcon()); //03=話す条件
            TALK_L_0_COMBO.AddIcon(0x4, ImageSystemIconForm.MusicIcon(3)); //04=話す条件ASM

            ALWAYS_L_0_COMBO.AddIcon(0x1, ImageSystemIconForm.MusicIcon(13)); //01=常時条件
            ALWAYS_L_0_COMBO.AddIcon(0xB, ImageSystemIconForm.MusicIcon(11)); //0B=範囲条件
            ALWAYS_L_0_COMBO.AddIcon(0xE, ImageSystemIconForm.MusicIcon(3)); //0E=ASM条件

            N02_L_0_COMBO.AddIcon(0x2, ImageSystemIconForm.MusicIcon(12)); //02=ターン条件
            N02_L_10_COMBO.AddIcon(0x0, ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(1, 0, true)); //00=プレイヤ
            N02_L_10_COMBO.AddIcon(0x40, ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(16, 1, true)); //40=友軍
            N02_L_10_COMBO.AddIcon(0x80, ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(7, 2, true)); //80=敵軍
            if (Program.ROM.RomInfo.version() == 8)
            {//FE8.
                OBJECT_N05_L_10_COMBO.Items.Add(R._("11=制圧"));
                OBJECT_N05_L_10_COMBO.AddIcon(0x11, ImageSystemIconForm.Throne()); //11=制圧
                OBJECT_N05_L_10_COMBO.Items.Add(R._("20=村の中心(盗賊のターゲット)"));
                OBJECT_N05_L_10_COMBO.AddIcon(0x20, ImageSystemIconForm.VillageCenter()); //20=村の中心(盗賊のターゲット)
                OBJECT_N05_L_10_COMBO.Items.Add(R._("10=民家"));
                OBJECT_N05_L_10_COMBO.AddIcon(0x10, ImageSystemIconForm.House()); //10=民家
                OBJECT_N05_L_10_COMBO.Items.Add(R._("14=ランダム宝箱"));
                OBJECT_N05_L_10_COMBO.AddIcon(0x14, ImageSystemIconForm.Chest()); //14=ランダム宝箱
                if (PatchUtil.SearchEscapePatch() != PatchUtil.Escape_enum.NO)
                {
                    OBJECT_N05_L_10_COMBO.Items.Add(R._("13=離脱"));
                    OBJECT_N05_L_10_COMBO.AddIcon(0x13, ImageSystemIconForm.ExitPoint()); //13=離脱
                    OBJECT_N05_L_10_COMBO.Items.Add(R._("19=到着"));
                    OBJECT_N05_L_10_COMBO.AddIcon(0x19, ImageSystemIconForm.Castle()); //19=到着
                }
                if (PatchUtil.SearchRaidPatch() != PatchUtil.Raid_enum.NO)
                {
                    OBJECT_N05_L_10_COMBO.Items.Add(R._("21=Raid"));
                    OBJECT_N05_L_10_COMBO.AddIcon(0x21, ClassForm.DrawWaitIcon(0x41)); //21=Raid
                }
                if (PatchUtil.SearchStairsHackPatch())
                {
                    OBJECT_N05_L_10_COMBO.Items.Add(R._("22=階段拡張"));
                    OBJECT_N05_L_10_COMBO.AddIcon(0x22, ImageSystemIconForm.Stairs()); //22=階段
                }
                OBJECT_N05_L_10_COMBO.Items.Add(R._("0=--"));


                OBJECT_N06_L_10_COMBO.AddIcon(0x20, ImageSystemIconForm.VillageCenter()); //20=村の中心(盗賊のターゲット)
                OBJECT_N06_L_10_COMBO.AddIcon(0x10, ImageSystemIconForm.Village()); //10=民家

                OBJECT_N07_L_10_COMBO.AddIcon(0x14, ImageSystemIconForm.Chest()); //14=宝箱

                OBJECT_N08_L_10_COMBO.AddIcon(0x12, ImageSystemIconForm.Door()); //12=扉

                OBJECT_N0A_L_10_COMBO.AddIcon(0x16, ImageSystemIconForm.Armory()); //16=武器屋
                OBJECT_N0A_L_10_COMBO.AddIcon(0x17, ImageSystemIconForm.Vendor()); //17=道具屋
                OBJECT_N0A_L_10_COMBO.AddIcon(0x18, ImageSystemIconForm.SecretShop()); //18=秘密の店

                if (PatchUtil.SearchSkillSystem() == PatchUtil.skill_system_enum.SkillSystem)
                {
                    TRAP_L_0_COMBO.Items.Insert(4, R._("06=DragonVein"));
                }
                if (PatchUtil.SearchCache_FourthAllegiance() == PatchUtil.FourthAllegiance_extends.FourthAllegiance)
                {
                    N02_L_10_COMBO.Items.Add("C0=第4軍ターンに実行");
                    N02_L_10_COMBO.AddIcon(0xC0, ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(10, 4, true)); //C0=第4軍
                }

                return;
            }

            //FE7とFE6はイベントオブジェクトの種類のIDが違う.
            OBJECT_N05_L_10_COMBO.BeginUpdate();
            OBJECT_N05_L_10_COMBO.Items.Add(R._("0F=制圧"));
            OBJECT_N05_L_10_COMBO.Items.Add(R._("1D=村の中心(盗賊のターゲット)"));
            OBJECT_N05_L_10_COMBO.Items.Add(R._("0E=民家"));
            OBJECT_N05_L_10_COMBO.AddIcon(0x0F, ImageSystemIconForm.Throne()); //F=制圧
            OBJECT_N05_L_10_COMBO.AddIcon(0x1D, ImageSystemIconForm.VillageCenter()); //1D=村の中心(盗賊のターゲット)
            OBJECT_N05_L_10_COMBO.AddIcon(0x0E, ImageSystemIconForm.House()); //0E=民家


            if (Program.ROM.RomInfo.version() == 6)
            {//FE6には、イベント付き宝箱がある
                OBJECT_N05_L_10_COMBO.Items.Add(R._("12=イベント付き宝箱"));
                OBJECT_N05_L_10_COMBO.AddIcon(0x12, ImageSystemIconForm.Chest());
            }
            if (PatchUtil.SearchStairsHackPatch())
            {
                OBJECT_N05_L_10_COMBO.Items.Add(R._("22=階段拡張"));
                OBJECT_N05_L_10_COMBO.AddIcon(0x22, ImageSystemIconForm.Stairs()); //22=階段
            }
            OBJECT_N05_L_10_COMBO.Items.Add(R._("0=不明"));
            OBJECT_N05_L_10_COMBO.EndUpdate();

            OBJECT_N06_L_10_COMBO.BeginUpdate();
            OBJECT_N06_L_10_COMBO.Items.Clear();
            OBJECT_N06_L_10_COMBO.Items.Add(R._("1D=村の中心(盗賊のターゲット)"));
            OBJECT_N06_L_10_COMBO.Items.Add(R._("0E=民家"));
            OBJECT_N06_L_10_COMBO.Items.Add(R._("0=--"));
            OBJECT_N06_L_10_COMBO.EndUpdate();
            OBJECT_N06_L_10_COMBO.AddIcon(0x1D, ImageSystemIconForm.VillageCenter()); //20=村の中心(盗賊のターゲット)
            OBJECT_N06_L_10_COMBO.AddIcon(0x0E, ImageSystemIconForm.Village()); //0E=民家

            OBJECT_N07_L_10_COMBO.BeginUpdate();
            OBJECT_N07_L_10_COMBO.Items.Clear();
            OBJECT_N07_L_10_COMBO.Items.Add(R._("12=宝箱"));
            OBJECT_N07_L_10_COMBO.Items.Add(R._("0=--"));
            OBJECT_N07_L_10_COMBO.EndUpdate();
            OBJECT_N07_L_10_COMBO.AddIcon(0x12, ImageSystemIconForm.Chest()); //12=宝箱

            OBJECT_N08_L_10_COMBO.BeginUpdate();
            OBJECT_N08_L_10_COMBO.Items.Clear();
            OBJECT_N08_L_10_COMBO.Items.Add(R._("10=扉"));
            OBJECT_N08_L_10_COMBO.Items.Add(R._("0=--"));
            OBJECT_N08_L_10_COMBO.EndUpdate();
            OBJECT_N08_L_10_COMBO.AddIcon(0x10, ImageSystemIconForm.Door()); //10=扉

            OBJECT_N0A_L_10_COMBO.BeginUpdate();
            OBJECT_N0A_L_10_COMBO.Items.Clear();
            OBJECT_N0A_L_10_COMBO.Items.Add(R._("13=武器屋"));
            OBJECT_N0A_L_10_COMBO.Items.Add(R._("14=道具屋"));
            OBJECT_N0A_L_10_COMBO.Items.Add(R._("15=秘密の店"));
            OBJECT_N0A_L_10_COMBO.Items.Add(R._("0=--"));
            OBJECT_N0A_L_10_COMBO.EndUpdate();
            OBJECT_N0A_L_10_COMBO.AddIcon(0x13, ImageSystemIconForm.Armory()); //13=武器屋
            OBJECT_N0A_L_10_COMBO.AddIcon(0x14, ImageSystemIconForm.Vendor()); //14=道具屋
            OBJECT_N0A_L_10_COMBO.AddIcon(0x15, ImageSystemIconForm.SecretShop()); //15=秘密の店

            TRAP_L_0_COMBO.BeginUpdate();
            TRAP_L_0_COMBO.Items.RemoveAt(TRAP_L_0_COMBO.Items.Count - 1); //0xCゴーゴンの卵を消す
            TRAP_L_0_COMBO.EndUpdate();

            if (Program.ROM.RomInfo.version() == 6)
            {//FE6の常時条件のasm条件の発生条件は、 0Eではなくて0D
                TALKFE6_L_0_COMBO.OwnerDraw(ComboBoxEx.DrawIconAndText, DrawMode.OwnerDrawFixed);
                TALKFE6_L_0_COMBO.AddIcon(0x3, ImageSystemIconForm.TalkIcon()); //03=話す条件
                TALKFE6_L_0_COMBO.AddIcon(0x4, ImageSystemIconForm.MusicIcon(3)); //04=話す条件ASM

                ALWAYS_L_0_COMBO.AddIcon(0xD, ImageSystemIconForm.MusicIcon(3)); //0D=ASM条件

                ALWAYS_L_0_COMBO.BeginUpdate();
                ALWAYS_L_0_COMBO.Items.RemoveAt(ALWAYS_L_0_COMBO.Items.Count - 1); //0xCゴーゴンの卵を消す
                ALWAYS_L_0_COMBO.Items.Add(R._("0D=ASM条件"));
                ALWAYS_L_0_COMBO.EndUpdate();
            }
            else
            {
                NFE702_L_0_COMBO.OwnerDraw(ComboBoxEx.DrawIconAndText, DrawMode.OwnerDrawFixed);
                NFE702_L_0_COMBO.AddIcon(0x2, ImageSystemIconForm.MusicIcon(12)); //02=ターン条件
                NFE702_L_10_COMBO.AddIcon(0x0, ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(1, 0, true)); //00=プレイヤ
                NFE702_L_10_COMBO.AddIcon(0x40, ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(16, 1, true)); //40=友軍
                NFE702_L_10_COMBO.AddIcon(0x80, ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(7, 2, true)); //80=敵軍
            }
        }


        //リストが拡張されたとき
        void AddressListExpandsEvent(object sender, EventArgs arg)
        {
            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            U.ForceUpdate(this.EventPointer, eearg.NewBaseAddress);
            InputFormRef.WriteButtonToYellow(WriteButton, false);
        }

        //リストが拡張されたとき P4イベントポインタをNULLにする.
        void AddressListExpandsEventNoCopyP4(object sender, EventArgs arg)
        {
            if (!(sender is Control))
            {
                return;
            }
            uint default_type = GetDefaultEventType();

            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            uint addr = eearg.NewBaseAddress;
            int count = (int)eearg.NewDataCount;
            U.ForceUpdate(this.EventPointer, addr);
            InputFormRef.WriteButtonToYellow(WriteButton, false);

            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"ClearP4Pointer");
            addr = addr + (eearg.OldDataCount * eearg.BlockSize);

            for (int i = (int)eearg.OldDataCount; i < count; i++)
            {
                //種類が0だと終端がわからなくなるので、適当なものを入れる.
                uint type = Program.ROM.u8(addr + 0);
                if (type == 0)
                {
                    Program.ROM.write_u8(addr + 0, default_type, undodata);
                }

                //増えた分のP4をゼロにする.
                //Program.ROM.write_u32(addr + 4, 0 , undodata);
                Program.ROM.write_fill(addr + 1, eearg.BlockSize - 1, 0, undodata);//クリア

                addr += eearg.BlockSize;
            }
            Program.Undo.Push(undodata);

            //イベントの関連アイコンを取得しなおしたい
            EventRelationIconsCache.Clear();
            WriteButton.PerformClick();
        }


        uint GetDefaultEventType()
        {
            if (FilterComboBox.SelectedIndex < 0)
            {
                return 0;
            }
            CONDTYPE condtype = EventCondForm.MapCond[FilterComboBox.SelectedIndex].Type;
            if (condtype == CONDTYPE.TURN)
            {
                return 2; //ターン条件
            }
            if (condtype == CONDTYPE.TALK)
            {
                if (Program.ROM.RomInfo.version() == 6)
                {
                    return 4; //会話条件
                }
                return 3; //会話条件
            }
            if (condtype == CONDTYPE.OBJECT)
            {
                return 5; //制圧と民家
            }
            if (condtype == CONDTYPE.ALWAYS)
            {
                return 1; //常時
            }
            if (condtype == CONDTYPE.TRAP)
            {
                return 1; //バリスタ
            }
            return 0;
        }

        void AddressListExpandsEventTrap(object sender, EventArgs arg)
        {
            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            uint addr = eearg.NewBaseAddress;
            int count = (int)eearg.NewDataCount;
            U.ForceUpdate(this.EventPointer, addr);
            InputFormRef.WriteButtonToYellow(WriteButton, false);

            Undo.UndoData undodata = Program.Undo.NewUndoData(this, "ClearP4Pointer");
            addr = addr + (eearg.OldDataCount * eearg.BlockSize);

            for (int i = (int)eearg.OldDataCount; i < count; i++)
            {
                //種類が0だと終端がわからなくなるので、適当なものを入れる.
                uint type = Program.ROM.u8(addr + 0);
                if (type == 0)
                {
                    Program.ROM.write_u8(addr + 0, 0x1,undodata);//アーチ
                }
                Program.ROM.write_fill(addr + 1, eearg.BlockSize - 1, 0, undodata);//クリア

                addr += eearg.BlockSize;
            }
            Program.Undo.Push(undodata);
            WriteButton.PerformClick();
        }

        //リストが拡張されたとき P0イベントポインタをNULLにする.
        void AddressListExpandsEventNoCopyP0(object sender, EventArgs arg)
        {
            InputFormRef.ExpandsEventArgs eearg = (InputFormRef.ExpandsEventArgs)arg;
            uint addr = eearg.NewBaseAddress;
            int count = (int)eearg.NewDataCount;
            U.ForceUpdate(this.EventPointer, addr);
            InputFormRef.WriteButtonToYellow(WriteButton, false);

            //増えた分のP4をゼロにする.
            Undo.UndoData undodata = Program.Undo.NewUndoData(this,"ClearP0Pointer");
            addr = addr + (eearg.OldDataCount * eearg.BlockSize);
            for (int i = (int)eearg.OldDataCount; i < count; i++)
            {
                Program.ROM.write_u32(addr + 0, 0, undodata);

                addr += eearg.BlockSize;
            }
            Program.Undo.Push(undodata);
            WriteButton.PerformClick();
        }

        //ターン条件
        InputFormRef InputFormRefN02;
        public static InputFormRef InitN02(Form self)
        {
            if (Program.ROM.RomInfo.version() == 7)
            {//FE7だけサイズが違う.
                return new InputFormRef(self
                    , "NFE702_"
                    , new List<String>()
                    , 0, Program.ROM.RomInfo.eventcond_tern_size()
                    , (uint addr) =>
                    {
                        uint type = Program.ROM.u8(addr);
                        if (type == 1)
                        {//信じられないことにFE7ではサイズは可変長である.
                            return addr + 12;
                        }
                        return addr + Program.ROM.RomInfo.eventcond_tern_size();
                    }
                    , (int i, uint addr) =>
                    {//00まで検索
                        return Program.ROM.u32(addr + 0) != 0;
                    }
                    , (int i, uint addr) =>
                    {
                        return new U.AddrResult(addr, (addr).ToString("X08"));
                    }
                    );
            }
            else
            {
                return new InputFormRef(self
                    , "N02_"
                    , 0
                    , Program.ROM.RomInfo.eventcond_tern_size()
                    , (int i, uint addr) =>
                    {//00まで検索
                        return Program.ROM.u32(addr + 0) != 0;
                    }
                    , (int i, uint addr) =>
                    {
                        return (addr).ToString("X08");
                    }
                    );
            }

        }

        //話す条件
        InputFormRef InputFormRefTalk;
        static InputFormRef InitTalk(Form self)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {//FE6だけ意味が違うしサイズも違う
                return new InputFormRef(self
                    , "TALKFE6_"
                    , new List<string>() { "N04_", "N0D_" }
                    , 0
                    , Program.ROM.RomInfo.eventcond_talk_size()
                    , (int i, uint addr) =>
                    {//00まで検索
                        return Program.ROM.u32(addr + 0) != 0;
                    }
                    , (int i, uint addr) =>
                    {
                        return (addr).ToString("X08");
                    }
                    );
            }
            else
            {
                return new InputFormRef(self
                    , "TALK_"
                    , new List<string>() { "N03_", "N04_" }
                    , 0
                    , Program.ROM.RomInfo.eventcond_talk_size()
                    , (int i, uint addr) =>
                    {//00まで検索
                        return Program.ROM.u32(addr + 0) != 0;
                    }
                    , (int i, uint addr) =>
                    {
                        return (addr).ToString("X08");
                    }
                    );
            }
        }
        //オブジェクト
        InputFormRef InputFormRefObject;
        static InputFormRef InitObject(Form self)
        {
            return new InputFormRef(self
                , "OBJECT_"
                , new List<string>() { "N05_", "N06_", "N07_", "N08_", "N0A_" }
                , 0
                , 12
                , (int i, uint addr) =>
                {//00まで検索
                    return Program.ROM.u32(addr + 0) != 0;
                }
                , (int i, uint addr) =>
                {
                    return (addr).ToString("X08");
                }
                );
        }
        //常時条件と範囲条件
        InputFormRef InputFormRefAlways;
        static InputFormRef InitAlways(Form self)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {//FE6の常時条件のasm条件の発生条件は、 0Eではなくて0D
                return new InputFormRef(self
                    , "ALWAYS_"
                    , new List<string>() { "N01_", "N0B_", "N0D_" }
                    , 0
                    , 12
                    , (int i, uint addr) =>
                    {//00まで検索
                        return Program.ROM.u32(addr + 0) != 0;
                    }
                    , (int i, uint addr) =>
                    {
                        return (addr).ToString("X08");
                    }
                    );
            }
            else
            {
                return new InputFormRef(self
                    , "ALWAYS_"
                    , new List<string>() { "N01_", "N0B_", "N0E_" }
                    , 0
                    , 12
                    , (int i, uint addr) =>
                    {//00まで検索
                        return Program.ROM.u32(addr + 0) != 0;
                    }
                    , (int i, uint addr) =>
                    {
                        return (addr).ToString("X08");
                    }
                    );
            }
        }
        //チュートリアル
        InputFormRef InputFormRefTutorial;
        public static InputFormRef InitTutorial(Form self)
        {
            return new InputFormRef(self
                , "TUTORIAL_"
                , 0
                , 4
                , (int i, uint addr) =>
                {//有効なポインタまで 基本的に0で停止
                    return U.isPointer(Program.ROM.u32(addr + 0));
                }
                , (int i, uint addr) =>
                {
                    return (addr).ToString("X08");
                }
                );
        }
        InputFormRef InputFormRefTrap;
        static InputFormRef InitTrap(Form self)
        {
            return new InputFormRef(self
                , "TRAP_"
                , new List<string>() { "N01_", "N04_", "N05_", "N06_", "N07_", "N08_", "N0A_", "N0B_", "N0C_" }
                , 0
                , 6
                , (int i, uint addr) =>
                {//00まで検索
                    return Program.ROM.u8(addr + 0) != 0;
                }
                , (int i, uint addr) =>
                {
                    return (addr).ToString("X08");
                }
                );
        }

        private void EventCondForm_Load(object sender, EventArgs e)
        {
            U.SelectedIndexSafety(this.MAP_LISTBOX , 0);

            N02_11_EXPLAIN.Text += "\r\n\r\n" + ExplainSampleCondTurn();
            NFE702_12_EXPLAIN.Text += "\r\n\r\n" + ExplainSampleCondTurnFE7();

            ALWAYS_N01_EXPLAIN.Text += "\r\n\r\n" + ExplainSampleAchievementFlag();
            ALWAYS_N0B_EXPLAIN.Text += "\r\n\r\n" + ExplainSampleAchievementFlag();
            if (Program.ROM.RomInfo.version() == 6)
            {//FE6の常時条件は、0xD
                ALWAYS_N0D_EXPLAIN.Text += "\r\n\r\n" + ExplainSampleAchievementFlag();
            }
            else
            {//FE7,8の常時条件は、0xE
                ALWAYS_N0E_EXPLAIN.Text += "\r\n\r\n" + ExplainSampleAchievementFlag();
            }

            OBJECT_N05_EXPLAIN.Text += "\r\n\r\n" + ExplainSampleAchievementFlag(); //制圧
            OBJECT_N06_EXPLAIN.Text += "\r\n\r\n" + ExplainSampleAchievementFlag(); //街
            OBJECT_N07_EXPLAIN.Text += "\r\n\r\n" + ExplainSampleAchievementFlag();//宝箱
            OBJECT_N08_EXPLAIN.Text += "\r\n\r\n" + ExplainSampleAchievementFlag();//扉
            OBJECT_N0A_EXPLAIN.Text += "\r\n\r\n" + ExplainSampleAchievementFlag();//店

            if (Program.ROM.RomInfo.version() == 6)
            {//FE6の常時条件は、0xD
                TALKFE6_N04_EXPLAIN.Text += "\r\n\r\n" + ExplainSampleCondTalk();
                TALKFE6_N0D_EXPLAIN.Text += "\r\n\r\n" + ExplainSampleCondTalkASM();
            }
            else
            {
                TALK_N03_EXPLAIN.Text += "\r\n\r\n" + ExplainSampleCondTalk();
                TALK_N04_EXPLAIN.Text += "\r\n\r\n" + ExplainSampleCondTalkASM();
            }
        }
        private void MAP_LISTBOX_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint mapid = (uint)MAP_LISTBOX.SelectedIndex;
            if (mapid == U.NOT_FOUND)
            {
                return;
            }
            uint addr = MapSettingForm.GetEventAddrWhereMapID(mapid);
            if (!U.isSafetyOffset(addr))
            {
                return;
            }

            U.ForceUpdate(this.ReadStartAddress,addr);

            MapPictureBox.LoadMap(mapid);
            ReloadEventList(U.NOT_FOUND, false);
        }


        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadEventList(U.NOT_FOUND , false);
        }

        private void ReloadListButton_Click(object sender, EventArgs e)
        {
            ReloadEventList((uint)ReadCount.Value, true);
        }

        void ReloadEventList(uint readCount, bool IsManualForcedChange)
        {
            MapPictureBox.ClearStaticItem();

            if (FilterComboBox.SelectedIndex < 0)
            {
                return;
            }

            CONDTYPE condtype = EventCondForm.MapCond[FilterComboBox.SelectedIndex].Type;
            uint selected = (uint)FilterComboBox.SelectedIndex;
            if (this.ReadStartAddress.Value <= 0)
            {
                return;
            }
            uint addr = (uint)(this.ReadStartAddress.Value + (4 * selected));
            addr = Program.ROM.u32(addr);
            this.EventPointer.Value = addr;
            InputFormRef.WriteButtonToYellow(WriteButton, false);

            addr = U.toOffset(addr);
            Control prevFocusedControl = this.ActiveControl;
            if (condtype == CONDTYPE.TURN)
            {//0=ターン条件(02)
                this.InputFormRefN02.ClearSelect(false);
                this.InputFormRefN02.ReInit(addr, readCount, IsManualForcedChange);
                ReadCount.Value = this.InputFormRefN02.DataCount;
                if (Program.ROM.RomInfo.version() == 7)
                {//FE7だけターン条件のサイズが違う
                    this.CondTabControl.SelectedTab = tabPage02FE7;
                }
                else
                {
                    this.CondTabControl.SelectedTab = tabPage02;
                }
                //イベントの関連アイコンを取得しなおしたい
                EventRelationIconsCache.Clear();
            }
            else if (condtype == CONDTYPE.TALK)
            {//1=話すコマンドの会話条件(03)
                this.InputFormRefTalk.ClearSelect(false);
                this.InputFormRefTalk.ReInit(addr, readCount, IsManualForcedChange);
                ReadCount.Value = this.InputFormRefTalk.DataCount;
                if (Program.ROM.RomInfo.version() == 6)
                {//FE6だけいろいろ違う
                    this.CondTabControl.SelectedTab = tabPage03FE6;
                }
                else
                {
                    this.CondTabControl.SelectedTab = tabPage03;
                }
            }
            else if (condtype == CONDTYPE.OBJECT)
            {//2=制圧ポイント、宝箱、扉等のオブジェクト(05,06,07,08,0A)
                this.InputFormRefObject.ClearSelect(false);
                this.InputFormRefObject.ReInit(addr, readCount, IsManualForcedChange);
                ReadCount.Value = this.InputFormRefObject.DataCount;
                this.CondTabControl.SelectedTab = tabPage0506070A0C;
                //イベントの関連アイコンを取得しなおしたい
                EventRelationIconsCache.Clear();
            }
            else if (condtype == CONDTYPE.ALWAYS)
            {//3=範囲条件及び、勝利条件などの常時条件(0B,01)
                this.InputFormRefAlways.ClearSelect(false);
                this.InputFormRefAlways.ReInit(addr, readCount, IsManualForcedChange);
                ReadCount.Value = this.InputFormRefAlways.DataCount;
                this.CondTabControl.SelectedTab = tabPage010B;
            }
            else if (condtype == CONDTYPE.TUTORIAL)
            {//7=チュートリアル
                this.InputFormRefTutorial.ClearSelect(false);
                this.InputFormRefTutorial.ReInit(addr, readCount, IsManualForcedChange);
                ReadCount.Value = this.InputFormRefTutorial.DataCount;
                this.CondTabControl.SelectedTab = tabPageTutorial;
            }
            else if (condtype == CONDTYPE.TRAP)
            {//8=アーチ、ゴーゴンの卵、ダメージ床(01,0C,04)
                this.InputFormRefTrap.ClearSelect(false);
                this.InputFormRefTrap.ReInit(addr, readCount, IsManualForcedChange);
                ReadCount.Value = this.InputFormRefTrap.DataCount;
                this.CondTabControl.SelectedTab = tabPage01040C;
            }
            else if (condtype == CONDTYPE.PLAYER_UNIT
                || condtype == CONDTYPE.ENEMY_UNIT
                || condtype == CONDTYPE.FREEMAP_PLAYER_UNIT
                || condtype == CONDTYPE.FREEMAP_ENEMY_UNIT
            )
            {//1011121314151617 配置関係
                ReadCount.Value = 0;
                DrawUnits(addr,true , true);
                this.CondTabControl.SelectedTab = tabPage1011121314151617;
                UpdatePalcerExplain(condtype);
            }
            else if (condtype == CONDTYPE.START_EVENT
                  || condtype == CONDTYPE.END_EVENT)
            {//18,19 開始イベント 終了イベント
                ReadCount.Value = 0;
                DrawEventUnits(addr, false);
                this.CondTabControl.SelectedTab = tabPage1819;
                UpdateEventExplain(condtype);
            }
            else
            {
                ReadCount.Value = 0;
                this.CondTabControl.SelectedTab = tabPageNOP;
            }

            if (prevFocusedControl != null)
            {
                prevFocusedControl.Focus();
            }
        }
        void UpdatePalcerExplain(CONDTYPE condtype)
        {
            string str = R._("{0}を定義します。\r\n", this.FilterComboBox.Text);
            if (condtype == CONDTYPE.PLAYER_UNIT)
            {
                str += R._("仲間にしたことがないユニットがいれば、基本的に自動的に加入します。\r\n");
            }
            else if (condtype == CONDTYPE.ENEMY_UNIT)
            {
            }
            else if (condtype == CONDTYPE.FREEMAP_PLAYER_UNIT)
            {
            }
            else if (condtype == CONDTYPE.FREEMAP_ENEMY_UNIT)
            {
            }

            this.EXPLAIN_PLACER.Text = str;
        }
        void UpdateEventExplain(CONDTYPE condtype)
        {
            string str = "";
            if (condtype == CONDTYPE.START_EVENT)
            {
                str = R._("開始イベントは、章開始時に、ゲームシステムから自動的に呼び出されるイベントです。\r\n章開始時の演出を行い、自軍配置のユニットをロードします。\r\n必要であれば、{0}を表示します。\r\n", R._("進撃準備画面"));
            }
            else if (condtype == CONDTYPE.END_EVENT)
            {
                str = R._("終了イベントは、章終了時に呼び出す必要があるイベントです。\r\nフラグ03 「{0}」を有効にして、終了イベントを呼び出すと章をクリアになります。\r\n終了イベントには、次の章へ進む処理を記述する必要があります。\r\n\r\nフラグ03「{0}」を有効にして、ユニットを待機させるとシステムから自動的に呼ばれます。\r\nターン防衛などで、待機させられない場合は、フラグ03「{0}」を有効にした後で、直接呼ぶこともできます。\r\n(フラグ03「{0}」を有効にしてから呼ばないとクリアターン数が保存されないなどの問題があることがあります。)\r\n", R._("制圧フラグ"));
            }

            this.EXPLAIN_EVENT.Text = str;
        }

        private void WriteButton_Click(object sender, EventArgs e)
        {//イベントポインタの変更.
            int selected = FilterComboBox.SelectedIndex;
            if (selected < 0)
            {
                return;
            }
            uint addr = (uint)(this.ReadStartAddress.Value + (4 * selected));
            uint event_pointer = (uint)this.EventPointer.Value;
            string undo_name = this.MAP_LISTBOX.Text + " " + FilterComboBox.Text;

            InputFormRef.WritePointerButton(this, addr, event_pointer, undo_name);
            InputFormRef.WriteButtonToYellow(WriteButton, false);
            //イベントの関連アイコンを取得しなおしたい
            EventRelationIconsCache.Clear();
        }

        private void EventPointer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ReloadListButton_Click(sender, null);
            }
        }

        //店 スキャン.
        public static List<U.AddrResult> MakeShopPointerListBox(uint mapcond_addr)
        {
            List<U.AddrResult> list = new List<U.AddrResult>();

            for (int i = 0; i < MapCond.Count; i++)
            {
                CONDTYPE condtype = MapCond[i].Type;
                uint addr = Program.ROM.p32((uint)(mapcond_addr + (4 * i)));

                if (condtype != CONDTYPE.OBJECT)
                {
                    continue;
                }
                //2=制圧ポイント、宝箱、扉等のオブジェクト(05,06,07,08,0A)
                if (!U.isSafetyOffset(addr))
                {
                    continue;
                }
                for (; Program.ROM.u32(addr) != 0; addr += 12)
                {
                    if (!U.isSafetyOffset(addr + 12))
                    {
                        break;
                    }
                    uint shop_addr = Program.ROM.p32(addr + 4);
                    if (!U.isSafetyOffset(shop_addr))
                    {
                        continue;
                    }

                    uint shop_object = Program.ROM.u8(addr + 10);
                    string name = Program.ROM.RomInfo.get_shop_name(shop_object);
                    if (name != "")
                    {
                        list.Add(new U.AddrResult(shop_addr, name , addr + 4));
                    }
                }
            }
            return list;
        }

        public static List<U.AddrResult> MakePointerListBox(uint mapid,CONDTYPE filter_condtype)
        {
            List<U.AddrResult> list = new List<U.AddrResult>();
            uint mapcond_addr = MapSettingForm.GetEventAddrWhereMapID(mapid);
            if (!U.isSafetyOffset(mapcond_addr))
            {
                return list;
            }

            uint length = (uint)Program.ROM.Data.Length;

            for (int i = 0; i < MapCond.Count; i++)
            {
                CONDTYPE condtype = MapCond[i].Type;
                uint addr = Program.ROM.p32((uint)(mapcond_addr + (4 * i)));

                if (condtype != filter_condtype)
                {
                    continue;
                }
                if (!U.isSafetyOffset(addr))
                {
                    continue;
                }
                if (addr + 8 >= length)
                {
                    break;
                }

                if (filter_condtype == CONDTYPE.START_EVENT
                    || filter_condtype == CONDTYPE.END_EVENT
                    || filter_condtype == CONDTYPE.FREEMAP_ENEMY_UNIT
                    || filter_condtype == CONDTYPE.FREEMAP_PLAYER_UNIT
                    || filter_condtype == CONDTYPE.PLAYER_UNIT
                    || filter_condtype == CONDTYPE.ENEMY_UNIT
                    || filter_condtype == CONDTYPE.UNKNOWN
                    )
                {
                    list.Add(new U.AddrResult(addr, MapCond[i].Name, (uint)filter_condtype));
                }
                else if (filter_condtype == CONDTYPE.TURN)
                {
                    for (int n = 0; addr < length  ; n++)
                    {
                        if (Program.ROM.u32(addr) == 0)
                        {
                            break;
                        }

                        uint type = Program.ROM.u8(addr);

                        list.Add(new U.AddrResult(addr, MapCond[i].Name, (uint)((n << 8) + (uint)filter_condtype)));
                        if (Program.ROM.RomInfo.version() == 7
                            && type == 1)
                        {//信じられないがFE7は12バイトの短いターンイベントが存在する
                            addr += 12;
                        }
                        else
                        {
                            addr += Program.ROM.RomInfo.eventcond_tern_size();
                        }
                    }
                }
                else if (filter_condtype == CONDTYPE.TALK)
                {
                    for (int n = 0; addr < length; addr += Program.ROM.RomInfo.eventcond_talk_size(), n++)
                    {
                        if (Program.ROM.u32(addr) == 0)
                        {
                            break;
                        }

                        list.Add(new U.AddrResult(addr, MapCond[i].Name, (uint)((n << 8) + (uint)filter_condtype)));
                    }
                }
                else if (filter_condtype == CONDTYPE.TRAP)
                {
                    for (int n = 0; addr < length; addr += 6, n++)
                    {
                        if (Program.ROM.u8(addr) == 0)
                        {
                            break;
                        }

                        list.Add(new U.AddrResult(addr, MapCond[i].Name, (uint)((n << 8) + (uint)filter_condtype)));
                    }
                }
                else if (filter_condtype == CONDTYPE.OBJECT)
                {
                    for (int n = 0; addr < length; addr += 12, n++)
                    {
                        if (Program.ROM.u32(addr) == 0)
                        {
                            break;
                        }

                        list.Add(new U.AddrResult(addr, MapCond[i].Name, (uint)((n << 8) + (uint)filter_condtype)));
                    }
                }
                else
                {
                    for (int n = 0; addr < length; addr += 12, n++)
                    {
                        if (Program.ROM.u32(addr) == 0)
                        {
                            break;
                        }

                        list.Add(new U.AddrResult(addr, MapCond[i].Name, (uint)((n << 8) + (uint)filter_condtype)));
                    }
                }
            }
            return list;
        }
        
        //エラー検出
        public static void MakeCheckErrors(uint mapid,List<FELint.ErrorSt> errors)
        {
            uint mapcond_addr = MapSettingForm.GetEventAddrWhereMapID(mapid);
            if (!U.isSafetyOffset(mapcond_addr))
            {
                return;
            }

            List<uint> tracelist = new List<uint>();

//            uint objectTypeOfSeize = 0x11;
            uint objectTypeOfDoor = 0x12;
            uint objectTypeOfTownCenter = 0x20;
            uint objectTypeOfHouse = 0x10;
            uint objectTypeOfChest = 0x14;
            uint objectTypeOfArmory = 0x16;
            uint objectTypeOfVendor = 0x17;
            uint objectTypeOfSecret = 0x18;
            if (Program.ROM.RomInfo.version() <= 7)
            {
//                objectTypeOfSeize = 0xF;
                objectTypeOfDoor = 0x10;
                objectTypeOfTownCenter = 0x1D;
                objectTypeOfHouse = 0xE;
                objectTypeOfChest = 0x12;
                objectTypeOfArmory = 0x13;
                objectTypeOfVendor = 0x14;
                objectTypeOfSecret = 0x15;
            }

            List<U.AddrResult> list;
            list = MakePointerListBox(mapid, CONDTYPE.OBJECT);
            for (int i = 0; i < list.Count; i++)
            {
                uint addr = list[i].addr;
                if (!U.isSafetyOffset(addr + 12))
                {
                    break;
                }

                uint type = Program.ROM.u16(addr + 0);
                uint flag = Program.ROM.u16(addr + 2);
                uint event_addr = Program.ROM.u32(addr + 4);
                uint x = Program.ROM.u8(addr + 8);
                uint y = Program.ROM.u8(addr + 9);
                uint object_type = Program.ROM.u8(addr + 10);

                FELint.CheckFlag(flag, errors, CONDTYPE.OBJECT, addr);

                if (event_addr == 0)
                {//イベントが0
                    continue;
                }

                if (type == 0x5)
                {//05=制圧ポイントと民家
                }
                else if (type == 0x6)
                {//06=訪問村
                    FELint.CheckEventPointer(event_addr, errors, CONDTYPE.OBJECT, addr, false, tracelist);
                    if (!(object_type == objectTypeOfHouse || object_type == objectTypeOfTownCenter))
                    {
                        errors.Add(new FELint.ErrorSt(CONDTYPE.OBJECT, addr
                            , R._("「制圧ポイントと民家」なのに、種類で「{0}」が設定されています。", U.To0xHexString(object_type))));
                    }
                }
                else if (type == 0x7)
                {//07=宝箱
                    if (object_type != objectTypeOfChest)
                    {
                        errors.Add(new FELint.ErrorSt(CONDTYPE.OBJECT, addr
                            , R._("宝箱なのに、種類で「{0}」が設定されています。", U.To0xHexString(object_type))));
                    }
                    uint item_id = Program.ROM.u8(addr + 4);
                    uint gold = Program.ROM.u16(addr + 6);
                    if (item_id != Program.ROM.RomInfo.item_gold_id()
                        && gold > 0)
                    {//ゴールドではないのに、ゴールドの設定がある.
                        errors.Add(new FELint.ErrorSt(CONDTYPE.OBJECT, addr
                            , R._("ゴールドとアイテムの設定が不適切です。\r\n\r\nゴールドが入っている宝箱を作るには、アイテムを「{0} ゴールド」にしないといけません。\r\n逆に、ゴールドではなくアイテムが入っている宝箱の場合は、ゴールドの欄は0にしてください。\r\n"
                            , U.ToHexString(Program.ROM.RomInfo.item_gold_id()))
                        ));
                    }
                }
                else if (type == 0x8)
                {//08=扉
                    FELint.CheckEventPointer(event_addr, errors, CONDTYPE.OBJECT, addr, false, tracelist);
                    if (object_type != objectTypeOfDoor)
                    {
                        errors.Add(new FELint.ErrorSt(CONDTYPE.OBJECT, addr
                            , R._("ドアなのに、種類で「{0}」が設定されています。", U.To0xHexString(object_type))));
                    }
                }
                else if (type == 0xA)
                {//0A=店
                    if (!(object_type == objectTypeOfArmory || object_type == objectTypeOfVendor || object_type == objectTypeOfSecret))
                    {
                        errors.Add(new FELint.ErrorSt(CONDTYPE.OBJECT, addr
                            , R._("店なのに、種類で「{0}」が設定されています。", U.To0xHexString(object_type))));
                    }

                    if (flag > 0)
                    {//お店なのに達成フラグが設定されている
                        errors.Add(new FELint.ErrorSt(CONDTYPE.OBJECT, addr
                            , R._("お店に「{0}」が設定されています。\r\n意図的にやっている場合を除き、ここに「{0}」を設定するべきではありません。", GetNameOfAchievementFlag())));
                    }
                }
                else if (type == 0x00 && flag > 0)
                {
                    errors.Add(new FELint.ErrorSt(CONDTYPE.OBJECT, addr
                        , R._("マップオブジェクトに、間違った終端データがあります。\r\n発生タイプが0なのに、フラグ「{0}」が設定されています。\r\n終端データの場合は、フラグも0にしないと危険です。", U.To0xHexString(flag))));
                }
                else
                {
                    errors.Add(new FELint.ErrorSt(CONDTYPE.OBJECT, addr
                        , R._("マップオブジェクトに不明なタイプ「{0}」が利用されました。", U.To0xHexString(type))));
                }
            }

            list = MakePointerListBox(mapid, CONDTYPE.TALK);
            CheckAlien4(list, errors, CONDTYPE.TALK);
            for (int i = 0; i < list.Count; i++)
            {
                uint addr = list[i].addr;
                if (!U.isSafetyOffset(addr + Program.ROM.RomInfo.eventcond_talk_size()))
                {
                    break;
                }

                uint type = Program.ROM.u16(addr + 0);
                uint flag = Program.ROM.u16(addr + 2);
                uint event_addr = Program.ROM.u32(addr + 4);

                FELint.CheckFlag(flag, errors, CONDTYPE.TALK, addr);
                FELint.CheckEventPointer(event_addr, errors, CONDTYPE.TALK, addr, false, tracelist);

                if (type == 0x00 && flag > 0)
                {
                    errors.Add(new FELint.ErrorSt(CONDTYPE.TALK, addr
                        , R._("会話イベントに、間違った終端データがあります。\r\n発生タイプが0なのに、フラグ「{0}」が設定されています。\r\n終端データの場合は、フラグも0にしないと危険です。", U.To0xHexString(flag))));
                    continue;
                }

                if (Program.ROM.RomInfo.version() == 6)
                {
                    if (type == 0x4)
                    {//04=会話
                    }
                    else if (type == 0xD)
                    {//0D=ASM会話
                        uint asm = Program.ROM.u32(addr + 8);
                        FELint.CheckASMPointer(asm, errors, CONDTYPE.TALK, addr);
                    }
                    else
                    {
                        errors.Add(new FELint.ErrorSt(CONDTYPE.TALK, addr
                            , R._("会話イベントに不明なタイプ「{0}」が利用されました。", U.To0xHexString(type))));
                    }
                }
                else
                {
                    if (type == 0x3)
                    {//03=会話
                        uint jflag = Program.ROM.u16(addr + 14);
                        FELint.CheckFlag(jflag, errors, CONDTYPE.TALK, addr);
                    }
                    else if (type == 0x4)
                    {//04=ASM会話
                        uint asm = Program.ROM.u32(addr + 12);
                        FELint.CheckASMPointer(asm, errors, CONDTYPE.TALK, addr);
                    }
                    else
                    {
                        errors.Add(new FELint.ErrorSt(CONDTYPE.TALK, addr
                            , R._("会話イベントに不明なタイプ「{0}」が利用されました。", U.To0xHexString(type))));
                    }
                }

            }


            list = MakePointerListBox(mapid, CONDTYPE.TURN);
            CheckAlien4(list, errors, CONDTYPE.TURN);
            for (int i = 0; i < list.Count; i++)
            {
                uint addr = list[i].addr;
                if (!U.isSafetyOffset(addr + 12))
                {
                    break;
                }
                uint type = Program.ROM.u16(addr + 0);
                uint flag = Program.ROM.u16(addr + 2);
                uint event_addr = Program.ROM.u32(addr + 4);

                FELint.CheckFlag(flag, errors, CONDTYPE.TURN, addr);
                FELint.CheckEventPointer(event_addr, errors, CONDTYPE.TURN, addr, false, tracelist);

                if (Program.ROM.RomInfo.version() == 6 && (type == 0x1 || type == 0x2 || type == 0x3 || type == 0xD))
                {//FE6 には、ターン1-3まである
                    if (type == 0xD)
                    {//FE6にはasmによるターンイベントがあるらしい(恐ろしい)
                        uint asm = Program.ROM.u32(addr + 8);
                        FELint.CheckASMPointer(asm, errors, CONDTYPE.TURN, addr);
                    }
                }
                else if (Program.ROM.RomInfo.version() == 7 && (type == 0x1 || type == 0x2))
                {//FE7 には、ターン1-2まである
                    //ターン1はサイズが小さい
                    //ターン2はサイズが大きい
                }
                else if (Program.ROM.RomInfo.version() == 8 && (type == 0x2))
                {//FE8 には、ターン2がある
                }
                else if (type == 0x00 && flag > 0)
                {
                    errors.Add(new FELint.ErrorSt(CONDTYPE.TURN, addr
                        , R._("ターンイベントに、間違った終端データがあります。\r\n発生タイプが0なのに、フラグ「{0}」が設定されています。\r\n終端データの場合は、フラグも0にしないと危険です。", U.To0xHexString(flag))));
                }
                else
                {
                    errors.Add(new FELint.ErrorSt(CONDTYPE.TURN, addr
                        , R._("ターンイベントに不明なタイプ「{0}」が利用されました。", U.To0xHexString(type))));
                }
            }

            list = MakePointerListBox(mapid, CONDTYPE.ALWAYS);
            CheckAlien4(list, errors, CONDTYPE.ALWAYS);
            for (int i = 0; i < list.Count; i++)
            {
                uint addr = list[i].addr;
                if (!U.isSafetyOffset(addr + 12))
                {
                    break;
                }
                uint type = Program.ROM.u16(addr + 0);
                uint flag = Program.ROM.u16(addr + 2);
                uint event_addr = Program.ROM.u32(addr + 4);

                FELint.CheckFlag(flag, errors, CONDTYPE.ALWAYS, addr);
                FELint.CheckEventPointer(event_addr, errors, CONDTYPE.ALWAYS, addr, false, tracelist);

                if (type == 0xB)
                {//0B=範囲条件
                }
                else if (type == 0x1)
                {//01=常時条件
                    uint jflag = Program.ROM.u32(addr + 8);
                    FELint.CheckFlag(jflag, errors, CONDTYPE.ALWAYS, addr);
                }
                else if (type == 0xE && Program.ROM.RomInfo.version() >= 7)
                {//0E=ASM条件
                    uint asm = Program.ROM.u32(addr + 8);
                    FELint.CheckASMPointer(asm, errors, CONDTYPE.ALWAYS, addr);
                }
                else if (type == 0xD && Program.ROM.RomInfo.version() <= 6)
                {//0D=ASM条件 FE6のみ
                    uint asm = Program.ROM.u32(addr + 8);
                    FELint.CheckASMPointer(asm, errors, CONDTYPE.ALWAYS, addr);
                }
                else if (type == 0x00 && flag > 0)
                {
                    errors.Add(new FELint.ErrorSt(CONDTYPE.ALWAYS, addr
                        , R._("常時条件イベントに、間違った終端データがあります。\r\n発生タイプが0なのに、フラグ「{0}」が設定されています。\r\n終端データの場合は、フラグも0にしないと危険です。", U.To0xHexString(flag))));
                }
                else
                {
                    errors.Add(new FELint.ErrorSt(CONDTYPE.ALWAYS, addr
                        , R._("常時条件イベントに不明なタイプ「{0}」が利用されました。", U.To0xHexString(type))));
                }
            }

            if (Program.ROM.RomInfo.version() == 8)
            {
                uint player_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 10)));
                uint player_hard_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 11)));
                uint start_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 18)));
                uint end_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 19)));

                FELint.CheckPointerAlien4(player_addr, errors, CONDTYPE.PLAYER_UNIT, (uint)(mapcond_addr + (4 * 10)));
                FELint.CheckPointerAlien4(player_hard_addr, errors, CONDTYPE.PLAYER_UNIT, (uint)(mapcond_addr + (4 * 11)));
                FELint.CheckEventPointer(start_addr, errors, CONDTYPE.START_EVENT, (uint)(mapcond_addr + (4 * 18)), true, tracelist);
                FELint.CheckEventPointer(end_addr, errors, CONDTYPE.END_EVENT, (uint)(mapcond_addr + (4 * 19)), true, tracelist);
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                uint eliwood_enemy_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 6)));
                uint eliwood_enemy_hard_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 7)));
                uint hextor_enemy_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 8)));
                uint hextor_enemy_hard_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 9)));
                uint eliwood_player_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 10)));
                uint eliwood_player_hard_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 11)));
                uint hextor_player_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 12)));
                uint hextor_player_hard_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 13)));
                uint start_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 14)));
                uint end_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 15)));

                FELint.CheckPointerAlien4(eliwood_enemy_addr, errors, CONDTYPE.ENEMY_UNIT, (uint)(mapcond_addr + (4 * 6)));
                FELint.CheckPointerAlien4(eliwood_enemy_hard_addr, errors, CONDTYPE.ENEMY_UNIT, (uint)(mapcond_addr + (4 * 7)));
                FELint.CheckPointerAlien4(hextor_enemy_addr, errors, CONDTYPE.ENEMY_UNIT, (uint)(mapcond_addr + (4 * 8)));
                FELint.CheckPointerAlien4(hextor_enemy_hard_addr, errors, CONDTYPE.ENEMY_UNIT, (uint)(mapcond_addr + (4 * 9)));
                FELint.CheckPointerAlien4(eliwood_player_addr, errors, CONDTYPE.PLAYER_UNIT, (uint)(mapcond_addr + (4 * 10)));
                FELint.CheckPointerAlien4(eliwood_player_hard_addr, errors, CONDTYPE.PLAYER_UNIT, (uint)(mapcond_addr + (4 * 11)));
                FELint.CheckPointerAlien4(hextor_player_addr, errors, CONDTYPE.PLAYER_UNIT, (uint)(mapcond_addr + (4 * 12)));
                FELint.CheckPointerAlien4(hextor_player_hard_addr, errors, CONDTYPE.PLAYER_UNIT, (uint)(mapcond_addr + (4 * 13)));
                FELint.CheckEventPointer(start_addr, errors, CONDTYPE.START_EVENT, (uint)(mapcond_addr + (4 * 14)), true, tracelist);
                FELint.CheckEventPointer(end_addr, errors, CONDTYPE.END_EVENT, (uint)(mapcond_addr + (4 * 15)), true, tracelist);
            }
            else if (Program.ROM.RomInfo.version() == 6)
            {
                uint player_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 4)));
                uint enemy_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 5)));
                uint end_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 6)));

                FELint.CheckPointerAlien4(player_addr, errors, CONDTYPE.PLAYER_UNIT, (uint)(mapcond_addr + (4 * 4)));
                FELint.CheckPointerAlien4(enemy_addr, errors, CONDTYPE.ENEMY_UNIT, (uint)(mapcond_addr + (4 * 5)));
                FELint.CheckEventPointer(end_addr, errors, CONDTYPE.END_EVENT, (uint)(mapcond_addr + (4 * 6)), true, tracelist);
            }

            {
                uint turn_cond_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 0)));
                uint talk_cond_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 1)));
                uint object_cond_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 2)));
                uint always_cond_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 3)));
                FELint.CheckPointerAlien4(turn_cond_addr, errors, CONDTYPE.TURN, (uint)(mapcond_addr + (4 * 0)));
                FELint.CheckPointerAlien4(talk_cond_addr, errors, CONDTYPE.TALK, (uint)(mapcond_addr + (4 * 1)));
                FELint.CheckPointerAlien4(object_cond_addr, errors, CONDTYPE.OBJECT, (uint)(mapcond_addr + (4 * 2)));
                FELint.CheckPointerAlien4(always_cond_addr, errors, CONDTYPE.ALWAYS, (uint)(mapcond_addr + (4 * 3)));
            }

            if (Program.ROM.RomInfo.version() == 8)
            {
                uint always1_cond_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 4)));
                uint always2_cond_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 5)));
                uint always3_cond_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 6)));
                uint tutorial_cond_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 7)));
                uint trap1_cond_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 8)));
                uint trap2_cond_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 9)));
                FELint.CheckPointerAlien4(always1_cond_addr, errors, CONDTYPE.ALWAYS, (uint)(mapcond_addr + (4 * 4)));
                FELint.CheckPointerAlien4(always2_cond_addr, errors, CONDTYPE.ALWAYS, (uint)(mapcond_addr + (4 * 5)));
                FELint.CheckPointerAlien4(always3_cond_addr, errors, CONDTYPE.ALWAYS, (uint)(mapcond_addr + (4 * 6)));
                FELint.CheckPointerAlien4(tutorial_cond_addr, errors, CONDTYPE.TUTORIAL, (uint)(mapcond_addr + (4 * 7)));
                FELint.CheckPointer(trap1_cond_addr, errors, CONDTYPE.TRAP, (uint)(mapcond_addr + (4 * 8)));
                FELint.CheckPointer(trap2_cond_addr, errors, CONDTYPE.TRAP, (uint)(mapcond_addr + (4 * 9)));
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                uint trap1_cond_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 4)));
                uint trap2_cond_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 5)));
                FELint.CheckPointer(trap1_cond_addr, errors, CONDTYPE.TRAP, (uint)(mapcond_addr + (4 * 4)));
                FELint.CheckPointer(trap2_cond_addr, errors, CONDTYPE.TRAP, (uint)(mapcond_addr + (4 * 5)));
            }
        }

        static void CheckAlien4(List<U.AddrResult> list, List<FELint.ErrorSt> errors, EventCondForm.CONDTYPE cond)
        {
            if (list.Count <= 0)
            {
                return;
            }
            uint addr = list[0].addr;
            FELint.CheckAlien4(addr, errors, cond, addr);
        }

        public static bool IsChestObjectType(uint object_type)
        {
            if (Program.ROM.RomInfo.version() == 8)
            {
                return object_type == 0x14;
            }
            else
            {
                return object_type == 0x12;
            }
        }
        public static bool IsShopObjectType(uint object_type)
        {
            if (Program.ROM.RomInfo.version() == 8)
            {
                return object_type == 0x16 || object_type == 0x17 || object_type == 0x18;
            }
            else
            {
                return object_type == 0x13 || object_type == 0x14 || object_type == 0x15 ;
            }
        }

        //自軍のユニット配置アドレスかどうか判定する.
        public static bool IsPlayerUnit(uint unitPlacerAddr, uint mapid)
        {
            uint rom_length = (uint)Program.ROM.Data.Length;

            uint mapcond_addr = MapSettingForm.GetEventAddrWhereMapID(mapid);
            if (!U.isSafetyOffset(mapcond_addr))
            {
                return false;
            }

            for (int i = 0; i < MapCond.Count; i++)
            {
                CONDTYPE condtype = MapCond[i].Type;
                uint eventRootAddr = (uint)(mapcond_addr + (4 * i));
                if (eventRootAddr + 4 > rom_length)
                {
                    break;
                }
                uint addr = Program.ROM.p32(eventRootAddr);
                if (!U.isSafetyOffset(addr))
                {
                    continue;
                }

                if (condtype == CONDTYPE.PLAYER_UNIT
                    || condtype == CONDTYPE.FREEMAP_PLAYER_UNIT)
                {//自軍配置のみ
                    if (addr == unitPlacerAddr)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        //配置イベントだけのリストを作る.
        public static List<U.AddrResult> MakeUnitPointer(uint mapid)
        {
            List<uint> tracelist = new List<uint>();
            List<U.AddrResult> list = new List<U.AddrResult>();
            bool isFE7 = Program.ROM.RomInfo.version() == 7;
            uint rom_length = (uint)Program.ROM.Data.Length;

            uint mapcond_addr = MapSettingForm.GetEventAddrWhereMapID(mapid);
            if (!U.isSafetyOffset(mapcond_addr))
            {
                return list;
            }

            for (int i = 0; i < MapCond.Count; i++)
            {
                CONDTYPE condtype = MapCond[i].Type;
                uint eventRootAddr = (uint)(mapcond_addr + (4 * i));
                if (eventRootAddr + 4 > rom_length)
                {
                    break;
                }
                uint addr = Program.ROM.p32(eventRootAddr);
                if (!U.isSafetyOffset(addr))
                {
                    continue;
                }

                if (condtype == CONDTYPE.TURN)
                {//0=ターン条件(02)
                    while(true)
                    {
                        if (addr + Program.ROM.RomInfo.eventcond_tern_size() > rom_length)
                        {
                            break;
                        }

                        uint type = Program.ROM.u8(addr);
                        if (type == 0)
                        {
                            break;
                        }

                        uint event_addr = Program.ROM.p32(addr + 4);
                        if (U.isSafetyOffset(event_addr))
                        {
                            MakeUnitPointerEventScan(ref list, MapCond[i].Name + " " + U.To0xHexString(addr), event_addr, mapid, tracelist);
                        }

                        if (isFE7 && type == 1)
                        {//信じられないがFE7は12バイトの短いターンイベントが存在する
                            addr += 12;
                        }
                        else
                        {
                            addr += Program.ROM.RomInfo.eventcond_tern_size();
                        }
                    }
                }
                else if (condtype == CONDTYPE.TALK)
                {//1=話すコマンドの会話条件(03)
                    for (; true; addr += Program.ROM.RomInfo.eventcond_talk_size())
                    {
                        if (addr + Program.ROM.RomInfo.eventcond_talk_size() > rom_length)
                        {
                            break;
                        }
                        if (Program.ROM.u8(addr) == 0)
                        {
                            break;
                        }

                        uint event_addr = Program.ROM.p32(addr + 4);
                        if (!U.isSafetyOffset(event_addr))
                        {
                            continue;
                        }
                        MakeUnitPointerEventScan(ref list, MapCond[i].Name + " " + U.To0xHexString(addr), event_addr, mapid, tracelist);
                    }
                }
                else if (condtype == CONDTYPE.OBJECT)
                {//2=制圧ポイント、宝箱、扉等のオブジェクト(05,06,07,08,0A)
                    for (; true; addr += 12)
                    {
                        if (addr + 12 > rom_length)
                        {
                            break;
                        }
                        if (Program.ROM.u8(addr) == 0)
                        {
                            break;
                        }

                        uint event_addr = Program.ROM.p32(addr + 4);
                        if (!U.isSafetyOffset(event_addr))
                        {
                            continue;
                        }

                        uint object_type = Program.ROM.u8(addr + 10);
                        if (IsShopObjectType(object_type) || IsChestObjectType(object_type))
                        {//店か宝箱
                        }
                        else
                        {//それ以外
                            MakeUnitPointerEventScan(ref list, MapCond[i].Name + " " + U.To0xHexString(addr), event_addr, mapid, tracelist);
                        }
                    }
                }
                else if (condtype == CONDTYPE.ALWAYS)
                {//3=範囲条件及び、勝利条件などの常時条件(0B,01)
                    for (; true ; addr += 12)
                    {
                        if (addr + 12 > rom_length)
                        {
                            break;
                        }
                        if (Program.ROM.u32(addr) == 0)
                        {//FE8では32ビットでの評価です。
                            break;
                        }

                        uint event_addr = Program.ROM.p32(addr + 4);
                        if (!U.isSafetyOffset(event_addr))
                        {
                            continue;
                        }

                        MakeUnitPointerEventScan(ref list, MapCond[i].Name + " " + U.To0xHexString(addr), event_addr, mapid, tracelist);
                    }
                }
                else if (condtype == CONDTYPE.TUTORIAL)
                {//チュートリアル
                    for (; true ; addr += 4)
                    {
                        if (addr + 4 > rom_length)
                        {
                            break;
                        }
                        if ( ! U.isPointer(Program.ROM.u32(addr)))
                        {
                            break;
                        }

                        uint event_addr = Program.ROM.p32(addr + 0);
                        if (!U.isSafetyOffset(event_addr))
                        {
                            continue;
                        }
                        MakeUnitPointerEventScan(ref list, MapCond[i].Name + " " + U.To0xHexString(addr), event_addr, mapid, tracelist);
                    }
                }
                else if (
                    (
                       condtype == CONDTYPE.PLAYER_UNIT
                    || condtype == CONDTYPE.ENEMY_UNIT
                    || condtype == CONDTYPE.FREEMAP_PLAYER_UNIT
                    || condtype == CONDTYPE.FREEMAP_ENEMY_UNIT
                    ) 
                    && U.FindList(list, addr) == U.NOT_FOUND)
                {//1011121314151617 配置関係
                    list.Add(new U.AddrResult(
                        addr
                        , MapCond[i].Name
                        , mapid
                    ));
                }
                else if (condtype == CONDTYPE.START_EVENT
                     ||  condtype == CONDTYPE.END_EVENT
                    )
                {//18,19 開始イベント 終了イベント
                    //イベントのスキャン.
                    MakeUnitPointerEventScan(ref list, MapCond[i].Name, addr, mapid, tracelist);
                }
            }

            return list;
        }


        public static uint MakeUnitPointerEventScan(ref List<U.AddrResult> list,  string name, uint event_addr, uint start_mapid,  List<uint> tracelist )
        {
            uint mapid = start_mapid;
            int unknown_count = 0;
            uint addr = event_addr;
            uint lastBranchAddr = 0;

            while ( true )
            {
                //バイト列をイベント命令としてDisassembler.
                EventScript.OneCode code = Program.EventScript.DisAseemble(Program.ROM.Data, addr);
                if (EventScript.IsExitCode(code, addr, lastBranchAddr))
                {//終端命令
                    break;
                }
                else if (code.Script.Has == EventScript.ScriptHas.UNKNOWN)
                {
                    unknown_count++;
                    if (unknown_count > 10)
                    {//不明命令が10個連続して続いたら打ち切る
                        break;
                    }
                }
                else
                {
                    //少なくとも不明ではない.
                    unknown_count = 0;

                    if (code.Script.Has == EventScript.ScriptHas.IF_CONDITIONAL)
                    {//IF文で分岐の前に、現在のマップを記録します.
                     //終端のラベルで書き戻します.
                        start_mapid = mapid;
                        lastBranchAddr = addr;
                    }
                    else if (code.Script.Has == EventScript.ScriptHas.LABEL_CONDITIONAL)
                    {//ラベル条件で戻ってきた時、MAPIDを復元します.
                     //完ぺきではないが、そこそこ正しいを目指す.
                        mapid = start_mapid;
                        lastBranchAddr = 0;
                    }
                    else if (code.Script.Has == EventScript.ScriptHas.POINTER_UNIT_OR_EVENT)
                    {//イベント命令または増援へジャンプするものをもっているらしい.
                        for (int i = 0; i < code.Script.Args.Length; i++)
                        {
                            EventScript.Arg arg = code.Script.Args[i];
                            if (arg.Type == EventScript.ArgType.POINTER_EVENT)
                            {
                                uint v = EventScript.GetArgValue(code, arg);

                                v = U.toOffset(v);
                                if (U.isSafetyOffset(v)             //安全であり
                                    && tracelist.IndexOf(v) < 0     //まだ読んだことがなければ
                                    )
                                {
                                    tracelist.Add(v);
                                    mapid = MakeUnitPointerEventScan(ref list, name, v, mapid,tracelist);
                                }
                            }
                            else if (arg.Type == EventScript.ArgType.POINTER_UNIT)
                            {
                                uint v = EventScript.GetArgValue(code, arg);
                                if (!U.isPointer(v))
                                {
                                    continue;
                                }
                                v = U.toOffset(v);
                                if (v == EventUnitForm.INVALIDATE_UNIT_POINTER)
                                {//ユニット指定がない状態
                                    continue;
                                }
                                if (U.isSafetyOffset(v) && U.FindList(list, v) == U.NOT_FOUND)
                                {
                                    list.Add(new U.AddrResult(
                                            v
                                        , name
                                        , mapid
                                    ));
                                }
                            }
                        }
                    }
                    else if (code.Script.Has == EventScript.ScriptHas.MAP)
                    {//マップ切り替え命令をもっているらしい.
                        for (int i = 0; i < code.Script.Args.Length; i++)
                        {
                            EventScript.Arg arg = code.Script.Args[i];
                            if (arg.Type == EventScript.ArgType.MAPCHAPTER)
                            {
                                mapid = EventScript.GetArgValue(code, arg);
                            }
                        }
                    }
                }
                addr += (uint)code.Script.Size;
            }
            return mapid;
        }

        //アイテムを取得できるイベントのスキャン
        public static void MakeGiveItemEventScan(ref List<U.AddrResult> list, uint event_addr, List<uint> tracelist)
        {
            int unknown_count = 0;
            uint lastBranchAddr = 0;
            uint addr = event_addr;
            while (true)
            {
                //バイト列をイベント命令としてDisassembler.
                EventScript.OneCode code = Program.EventScript.DisAseemble(Program.ROM.Data, addr);
                if (EventScript.IsExitCode(code, addr, lastBranchAddr))
                {//終端命令
                    break;
                }
                else if (code.Script.Has == EventScript.ScriptHas.UNKNOWN)
                {
                    unknown_count++;
                    if (unknown_count > 10)
                    {//不明命令が10個連続して続いたら打ち切る
                        break;
                    }
                }
                else
                {
                    //少なくとも不明ではない.
                    unknown_count = 0;

                    if (code.Script.Has == EventScript.ScriptHas.POINTER_UNIT_OR_EVENT)
                    {//イベント命令または増援へジャンプするものをもっているらしい.
                        for (int i = 0; i < code.Script.Args.Length; i++)
                        {
                            EventScript.Arg arg = code.Script.Args[i];
                            if (arg.Type == EventScript.ArgType.POINTER_EVENT)
                            {
                                uint v = EventScript.GetArgValue(code, arg);

                                v = U.toOffset(v);
                                if (U.isSafetyOffset(v)             //安全であり
                                    && tracelist.IndexOf(v) < 0     //まだ読んだことがなければ
                                    )
                                {
                                    tracelist.Add(v);
                                    MakeGiveItemEventScan(ref list, v, tracelist);
                                }
                            }
                        }
                    }
                    else if (code.Script.Has == EventScript.ScriptHas.GIVE_ITEM)
                    {//アイテムをくれるイベント
                        for (int i = 0; i < code.Script.Args.Length; i++)
                        {
                            EventScript.Arg arg = code.Script.Args[i];
                            if (arg.Type == EventScript.ArgType.ITEM)
                            {
                                uint itemid = EventScript.GetArgValue(code, arg);
                                list.Add(new U.AddrResult(itemid, "GIVE_ITEM"));
                            }
                        }
                    }
                    else if (code.Script.Has == EventScript.ScriptHas.LABEL_CONDITIONAL)
                    {//LABEL
                        lastBranchAddr = 0;
                    }
                    else if (code.Script.Has == EventScript.ScriptHas.IF_CONDITIONAL)
                    {//IF
                        lastBranchAddr = addr;
                    }
                }
                addr += (uint)code.Script.Size;
            }
        }

        //FE7のチュートリアルイベントの一覧の取得
        public static List<U.AddrResult> MakeEventScriptForFE7Tutorial(uint mapid)
        {
            List<U.AddrResult> list = new List<U.AddrResult>();
            Debug.Assert(Program.ROM.RomInfo.version() == 7);

            if (mapid > 0x30)
            {
                return list;
            }

            uint tutorial_pointer = Program.ROM.RomInfo.event_tutorial_pointer();
            uint tutorial_addr = Program.ROM.p32(tutorial_pointer);

            tutorial_addr = tutorial_addr + (mapid * 4);
            if (!U.isSafetyOffset(tutorial_addr))
            {
                return list;
            }
            uint addr = Program.ROM.p32(tutorial_addr);
            if (!U.isSafetyOffset(addr))
            {
                return list;
            }

            {//3=範囲条件及び、勝利条件などの常時条件(0B,01)
                for (; true; addr += 12)
                {
                    if (!U.isSafetyOffset(addr + 12))
                    {
                        break;
                    }
                    if (Program.ROM.u32(addr) == 0)
                    {
                        break;
                    }

                    uint event_addr = Program.ROM.p32(addr + 4);
                    if (!U.isSafetyOffset(event_addr))
                    {
                        continue;
                    }

                    list.Add(new U.AddrResult(
                            event_addr
                        , "Tutorial FE7"
                        , mapid
                    ));
                }
            }
            return list;
        }


        //イベント命令　一覧の取得
        public static List<U.AddrResult> MakeEventScriptPointer(uint mapid)
        {
            List<U.AddrResult> list = new List<U.AddrResult>();
            bool isFE7 = Program.ROM.RomInfo.version() == 7;

            uint mapcond_addr = MapSettingForm.GetEventAddrWhereMapID(mapid);
            if (!U.isSafetyOffset(mapcond_addr))
            {
                return list;
            }

            for (int i = 0; i < MapCond.Count; i++)
            {
                CONDTYPE condtype = MapCond[i].Type;
                uint addr = Program.ROM.p32((uint)(mapcond_addr + (4 * i)));
                if (!U.isSafetyOffset(addr))
                {
                    continue;
                }

                if (condtype == CONDTYPE.TURN)
                {//0=ターン条件(02)
                    while (true)
                    {
                        if (Program.ROM.u32(addr) == 0)
                        {
                            break;
                        }

                        uint type = Program.ROM.u8(addr);
                        if (type == 0)
                        {
                            break;
                        }
                        if (!U.isSafetyOffset(addr + Program.ROM.RomInfo.eventcond_tern_size()))
                        {
                            break;
                        }

                        uint event_addr = Program.ROM.p32(addr + 4);
                        if (U.isSafetyOffset(event_addr))
                        {
                            list.Add(new U.AddrResult(
                                  event_addr
                                , MapCond[i].Name
                                , mapid
                            ));
                        }


                        if (isFE7 && type == 1)
                        {//信じられないがFE7は12バイトの短いターンイベントが存在する
                            addr += 12;
                        }
                        else
                        {
                            addr += Program.ROM.RomInfo.eventcond_tern_size();
                        }
                    }
                }
                else if (condtype == CONDTYPE.TALK)
                {//1=話すコマンドの会話条件(03)
                    for (; true ; addr += Program.ROM.RomInfo.eventcond_talk_size())
                    {
                        if (!U.isSafetyOffset(addr + Program.ROM.RomInfo.eventcond_talk_size()))
                        {
                            break;
                        }
                        if (Program.ROM.u32(addr) == 0)
                        {
                            break;
                        }

                        uint event_addr = Program.ROM.p32(addr + 4);
                        if (!U.isSafetyOffset(event_addr))
                        {
                            continue;
                        }
                        list.Add(new U.AddrResult(
                              event_addr
                            , MapCond[i].Name
                            , mapid
                        ));
                    }
                }
                else if (condtype == CONDTYPE.OBJECT)
                {//2=制圧ポイント、宝箱、扉等のオブジェクト(05,06,07,08,0A)
                    for (; true; addr += 12)
                    {
                        if (!U.isSafetyOffset(addr + 12))
                        {
                            break;
                        }
                        if (Program.ROM.u32(addr) == 0)
                        {
                            break;
                        }

                        uint event_addr = Program.ROM.p32(addr + 4);
                        if (!U.isSafetyOffset(event_addr))
                        {
                            continue;
                        }

                        uint object_type = Program.ROM.u8(addr + 10);
                        if (IsShopObjectType(object_type) || IsChestObjectType(object_type))
                        {//店か宝箱
                        }
                        else
                        {//店以外
                            list.Add(new U.AddrResult(
                                  event_addr
                                , MapCond[i].Name
                                , mapid
                            ));
                        }
                    }
                }
                else if (condtype == CONDTYPE.ALWAYS)
                {//3=範囲条件及び、勝利条件などの常時条件(0B,01)
                    for (; true; addr += 12)
                    {
                        if (!U.isSafetyOffset(addr + 12))
                        {
                            break;
                        }
                        if (Program.ROM.u32(addr) == 0)
                        {
                            break;
                        }

                        uint event_addr = Program.ROM.p32(addr + 4);
                        if (!U.isSafetyOffset(event_addr))
                        {
                            continue;
                        }

                        list.Add(new U.AddrResult(
                             event_addr
                            , MapCond[i].Name
                            , mapid
                        ));
                    }
                }
                else if (condtype == CONDTYPE.TUTORIAL)
                {//チュートリアル
                    for (; true; addr += 4)
                    {
                        if (!U.isSafetyOffset(addr + 4))
                        {
                            break;
                        }
                        if (!U.isPointer(Program.ROM.u32(addr)))
                        {
                            break;
                        }

                        uint event_addr = Program.ROM.p32(addr + 0);
                        if (!U.isSafetyOffset(event_addr))
                        {
                            continue;
                        }
                        list.Add(new U.AddrResult(
                             event_addr
                            , MapCond[i].Name
                            , mapid
                        ));
                    }
                }
                else if (condtype == CONDTYPE.START_EVENT
                    ||   condtype == CONDTYPE.END_EVENT)
                {//18,19 開始イベント 終了イベント
                    list.Add(new U.AddrResult(
                        addr
                        , MapCond[i].Name
                        , mapid
                    ));
                }
            }

            return list;
        }

        //イベント命令　一覧の取得(開始イベントと終了イベントのみ)
        public static List<U.AddrResult> MakeEventScriptPointerStartAndEndEventOnly(uint mapid)
        {
            List<U.AddrResult> list = new List<U.AddrResult>();

            uint mapcond_addr = MapSettingForm.GetEventAddrWhereMapID(mapid);
            if (!U.isSafetyOffset(mapcond_addr))
            {
                return list;
            }

            for (int i = 0; i < MapCond.Count; i++)
            {
                CONDTYPE condtype = MapCond[i].Type;

                if (condtype == CONDTYPE.START_EVENT
                    || condtype == CONDTYPE.END_EVENT)
                {//18,19 開始イベント 終了イベント

                    uint addr = Program.ROM.p32((uint)(mapcond_addr + (4 * i)));
                    if (!U.isSafetyOffset(addr))
                    {
                        continue;
                    }

                    list.Add(new U.AddrResult(
                        addr
                        , MapCond[i].Name
                        , mapid
                    ));
                }
            }

            return list;
        }
    
        private void Jump_TO_EventScript_Click(object sender, EventArgs e)
        {
            uint addr = (uint)this.EventPointer.Value;
            if (addr == 0)
            {//アドレスが0だったら割り当てを行う.
                DialogResult dr = R.ShowNoYes("新規にイベント命令の領域を割り当てますか？");
                if (dr != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
                //とりあえず終端命令だけのイベントを作る.
                byte[] alloc = Program.ROM.RomInfo.defualt_event_script_toplevel_code();
                Undo.UndoData undodata = Program.Undo.NewUndoData("NewAlloc Event");
                addr = InputFormRef.AppendBinaryData(alloc, undodata);
                if (addr == U.NOT_FOUND)
                {//割り当て失敗
                    return;
                }
                Program.Undo.Push(undodata);

                addr = U.toPointer(addr);
                this.EventPointer.Value = addr;
                //イベントアドレスの書き込み
                this.WriteButton.PerformClick();

                R.ShowOK("領域を割り振りました。イベント命令画面からイベントを作ってください。");
            }

            EventScriptForm f = (EventScriptForm)InputFormRef.JumpForm<EventScriptForm>(U.NOT_FOUND);
            f.JumpTo(addr);
        }

        private void Jump_TO_EventUnit_Click(object sender, EventArgs e)
        {
            uint value = (uint)this.EventPointer.Value;
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

        public void JumpToMAPID(uint mapid)
        {
            U.SelectedIndexSafety(MAP_LISTBOX,mapid);
        }
        public void JumpToMAPIDAndAddr(uint mapid, EventCondForm.CONDTYPE condtype, uint addr)
        {
            U.SelectedIndexSafety(MAP_LISTBOX, mapid);
            uint mapcond_addr = MapSettingForm.GetEventAddrWhereMapID(mapid);
            if (!U.isSafetyOffset(mapcond_addr))
            {
                return;
            }

            for (int i = 0; i < EventCondForm.MapCond.Count; i++)
            {
                if (EventCondForm.MapCond[i].Type != condtype)
                {
                    continue;
                }
                FilterComboBox.SelectedIndex = i;

                if (condtype == EventCondForm.CONDTYPE.OBJECT)
                {
                    uint select = InputFormRef.AddrToSelect(OBJECT_AddressList, addr);
                    if (select != U.NOT_FOUND)
                    {
                        U.SelectedIndexSafety(OBJECT_AddressList, select);
                        return;
                    }
                }
                else if (condtype == EventCondForm.CONDTYPE.TALK)
                {
                    if (Program.ROM.RomInfo.version() == 6)
                    {
                        uint select = InputFormRef.AddrToSelect(TALKFE6_AddressList, addr);
                        if (select != U.NOT_FOUND)
                        {
                            U.SelectedIndexSafety(TALKFE6_AddressList, select);
                            return;
                        }
                    }
                    else
                    {
                        uint select = InputFormRef.AddrToSelect(TALK_AddressList, addr);
                        if (select != U.NOT_FOUND)
                        {
                            U.SelectedIndexSafety(TALK_AddressList, select);
                            return;
                        }
                    }
                }
                else if (condtype == EventCondForm.CONDTYPE.TRAP)
                {
                    uint select = InputFormRef.AddrToSelect(TRAP_AddressList, addr);
                    if (select != U.NOT_FOUND)
                    {
                        U.SelectedIndexSafety(TRAP_AddressList, select);
                        return;
                    }
                }
                else if (condtype == EventCondForm.CONDTYPE.ALWAYS)
                {
                    uint select = InputFormRef.AddrToSelect(ALWAYS_AddressList, addr);
                    if (select != U.NOT_FOUND)
                    {
                        U.SelectedIndexSafety(ALWAYS_AddressList, select);
                        return;
                    }
                }
                else if (condtype == EventCondForm.CONDTYPE.TURN)
                {
                    if (Program.ROM.RomInfo.version() == 7)
                    {
                        uint select = InputFormRef.AddrToSelect(NFE702_AddressList, addr);
                        if (select != U.NOT_FOUND)
                        {
                            U.SelectedIndexSafety(NFE702_AddressList, select);
                            return;
                        }
                    }
                    else
                    {
                        uint select = InputFormRef.AddrToSelect(N02_AddressList, addr);
                        if (select != U.NOT_FOUND)
                        {
                            U.SelectedIndexSafety(N02_AddressList, select);
                            return;
                        }
                    }
                }
                else
                {
                    uint event_addr = (uint)(mapcond_addr + (4 * i));
                    if (event_addr == addr)
                    {
                        return;
                    }
                }
            }
        }

        public enum CONDTYPE
        {
             TURN
            ,TALK
            ,OBJECT
            ,ALWAYS
            ,TUTORIAL
            ,TRAP
            ,PLAYER_UNIT
            ,ENEMY_UNIT
            ,FREEMAP_PLAYER_UNIT
            ,FREEMAP_ENEMY_UNIT
            ,START_EVENT
            ,END_EVENT
            ,UNKNOWN
        };
        public class MapCondTitleSt
        {
            public CONDTYPE Type;
            public string Name;
        };
        public static List<MapCondTitleSt> MapCond = new List<MapCondTitleSt>();

        public static void PreLoadResource(string fullfilename)
        {
            MapCond.Clear();

            if (!U.IsRequiredFileExist(fullfilename))
            {
                return;
            }
            
            using (StreamReader reader = File.OpenText(fullfilename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (U.IsComment(line) || U.OtherLangLine(line))
                    {
                        continue;
                    }
                    string[] sp = line.Split('\t');
                    if (sp.Length <= 1)
                    {
                        continue;
                    }

                    string flag = sp[0];
                    string name = sp[1];

                    MapCondTitleSt p = new MapCondTitleSt();
                    if (flag == "TURN")
                    {
                        p.Type = CONDTYPE.TURN;
                    }
                    else if (flag == "TALK")
                    {
                        p.Type = CONDTYPE.TALK;
                    }
                    else if (flag == "OBJECT")
                    {
                        p.Type = CONDTYPE.OBJECT;
                    }
                    else if (flag == "ALWAYS")
                    {
                        p.Type = CONDTYPE.ALWAYS;
                    }
                    else if (flag == "TRAP")
                    {
                        p.Type = CONDTYPE.TRAP;
                    }
                    else if (flag == "PLAYER_UNIT")
                    {
                        p.Type = CONDTYPE.PLAYER_UNIT;
                    }
                    else if (flag == "ENEMY_UNIT")
                    {
                        p.Type = CONDTYPE.ENEMY_UNIT;
                    }
                    else if (flag == "FREEMAP_PLAYER_UNIT")
                    {
                        p.Type = CONDTYPE.FREEMAP_PLAYER_UNIT;
                    }
                    else if (flag == "FREEMAP_ENEMY_UNIT")
                    {
                        p.Type = CONDTYPE.FREEMAP_ENEMY_UNIT;
                    }
                    else if (flag == "START_EVENT")
                    {
                        p.Type = CONDTYPE.START_EVENT;
                    }
                    else if (flag == "END_EVENT")
                    {
                        p.Type = CONDTYPE.END_EVENT;
                    }
                    else if (flag == "TUTORIAL")
                    {
                        p.Type = CONDTYPE.TUTORIAL;
                    }
                    else
                    {
                        p.Type = CONDTYPE.UNKNOWN;
                    }
                    p.Name = name;
                    MapCond.Add(p);

                }
            }
        }

        public static void MakeAllDataLength(List<Address> list)
        {
            List<uint> tracelist = new List<uint>();
            uint mapmax = MapSettingForm.GetDataCount();
            for (uint mapid = 0; mapid < mapmax; mapid++)
            {
                if (InputFormRef.DoEvents(null, "EventCond " + U.ToHexString(mapid))) return ;
                
                uint mapcond_pointer;
                uint mapcond_addr = MapSettingForm.GetEventAddrWhereMapID(mapid, out mapcond_pointer);
                if (!U.isSafetyOffset(mapcond_addr))
                {
                    continue;
                }
                string mapidString = U.To0xHexString(mapid);

                for (int i = 0; i < MapCond.Count; i++)
                {
                    CONDTYPE condtype = MapCond[i].Type;
                    uint base_pointer = (uint) (mapcond_addr + (4 * i));
                    uint base_addr = Program.ROM.p32(base_pointer);
                    if (!U.isSafetyOffset(base_addr))
                    {
                        continue;
                    }

                    uint startaddr = base_addr;
                    if (condtype == CONDTYPE.TURN)
                    {//0=ターン条件(02)
                        while(true)
                        {
                            if (!U.isSafetyOffset(base_addr + Program.ROM.RomInfo.eventcond_tern_size()))
                            {
                                break;
                            }
                            uint type = Program.ROM.u8(base_addr);
                            if (type == 0)
                            {
                                break;
                            }

                            if (Program.ROM.RomInfo.version() == 6)
                            {//FE6だけ特殊で ASMをかけるらしいよ.
                                if (type == 0x0D)
                                {
                                    FEBuilderGBA.Address.AddFunction(list
                                        , base_addr + 8
                                        , "EventCond map:" + mapidString + " talk: ASM");
                                }
                            }

                            uint event_addr = Program.ROM.p32(base_addr + 4);
                            if (U.isSafetyOffset(event_addr))
                            {
                                EventScriptForm.ScanScript( list
                                    , base_addr + 4
                                    , true, false
                                    , "EventScript map:" + mapidString + " turn:" + U.To0xHexString(event_addr)
                                    , tracelist
                                    );
                            }


                            if (Program.ROM.RomInfo.version() == 7 
                                && type == 1)
                            {//信じられないがFE7は12バイトの短いターンイベントが存在する
                                base_addr += 12;
                            }
                            else
                            {
                                base_addr += Program.ROM.RomInfo.eventcond_tern_size();
                            }
                        }

                        FEBuilderGBA.Address.AddAddress(list
                            , startaddr
                            , base_addr - startaddr + Program.ROM.RomInfo.eventcond_tern_size()
                            , base_pointer
                            , "EventCond map:" + mapidString + " turn:" + U.ToHexString(startaddr)
                            , Address.DataTypeEnum.EVENTCOND_TURN);
                    }
                    else if (condtype == CONDTYPE.TALK)
                    {//1=話すコマンドの会話条件(03)
                        string basename = "EventScript map:" + mapidString + " talk:";
                        for (; true; base_addr += Program.ROM.RomInfo.eventcond_talk_size())
                        {
                            if (!U.isSafetyOffset(base_addr + Program.ROM.RomInfo.eventcond_talk_size()))
                            {
                                break;
                            }
                            uint type = Program.ROM.u8(base_addr + 0);
                            if (type == 0)
                            {
                                break;
                            }
                            if (Program.ROM.RomInfo.version() == 6)
                            {
                                if (type == 0x0D)
                                {
                                    FEBuilderGBA.Address.AddFunction(list
                                        , base_addr + 8
                                        , "EventCond map:" + mapidString + " talk: ASM");
                                }
                            }
                            else
                            {//FE7,8
                                if (type == 0x04)
                                {
                                    FEBuilderGBA.Address.AddFunction(list
                                        , base_addr + 12
                                        , "EventCond map:" + mapidString + " talk: ASM");
                                }
                            }

                            uint event_addr = Program.ROM.p32(base_addr + 4);
                            if (!U.isSafetyOffset(event_addr))
                            {
                                continue;
                            }

                            EventScriptForm.ScanScript( list
                                , base_addr + 4
                                , true, false
                                , "EventScript map:" + mapidString + " talk:" + U.To0xHexString(event_addr)
                                , tracelist
                                );
                        }

                        FEBuilderGBA.Address.AddAddress(list
                            , startaddr
                            , base_addr - startaddr + Program.ROM.RomInfo.eventcond_talk_size(), base_pointer
                            , "EventCond map:" + mapidString + " talk:" + U.ToHexString(startaddr)
                            , Address.DataTypeEnum.EVENTCOND_TALK);
                    }
                    else if (condtype == CONDTYPE.OBJECT)
                    {//2=制圧ポイント、宝箱、扉等のオブジェクト(05,06,07,08,0A)
                        for (; true ; base_addr += 12)
                        {
                            if (!U.isSafetyOffset(base_addr + 12))
                            {
                                break;
                            }
                            uint type = Program.ROM.u8(base_addr);
                            if (type == 0)
                            {
                                break;
                            }

                            uint event_addr = Program.ROM.p32(base_addr + 4);
                            if (!U.isSafetyOffset(event_addr))
                            {
                                continue;
                            }

                            uint object_type = Program.ROM.u8(base_addr + 10);
                            if (IsShopObjectType(object_type))
                            {//店は別データで回収するので不要
                            }
                            else if(IsChestObjectType(object_type))
                            {//宝箱
                                if (Program.ROM.RomInfo.version() == 8
                                    && type == 0x5)
                                {
                                    ItemRandomChestForm.MakeAllDataLength(list, base_addr + 4, mapidString);
                                }
                            }
                            else
                            {//店以外
                                EventScriptForm.ScanScript( list
                                    , base_addr + 4
                                    , true, false
                                    , "EventScript map:" + mapidString + " obj:" + U.To0xHexString(event_addr)
                                    , tracelist
                                    );
                            }
                        }
                        FEBuilderGBA.Address.AddAddress(list
                            , startaddr, base_addr - startaddr + 12
                            , base_pointer
                            , "EventCond Object map:" + mapidString + " obj:" + U.To0xHexString(startaddr)
                            , Address.DataTypeEnum.EVENTCOND_OBJECT);
                    }
                    else if (condtype == CONDTYPE.ALWAYS)
                    {//3=範囲条件及び、勝利条件などの常時条件(0B,01)
                        for (; true; base_addr += 12)
                        {
                            if (!U.isSafetyOffset(base_addr + 12))
                            {
                                break;
                            }
                            uint type = Program.ROM.u32(base_addr);
                            if (type == 0)
                            {
                                break;
                            }

                            if (Program.ROM.RomInfo.version() == 6)
                            {
                                if (type == 0x0D)
                                {
                                    FEBuilderGBA.Address.AddFunction(list
                                        , base_addr + 8
                                        , "EventCond map:" + mapidString + " always: ASM");
                                }
                            }
                            else
                            {//FE7,8
                                if (type == 0x0E)
                                {
                                    FEBuilderGBA.Address.AddFunction(list
                                        , base_addr + 12
                                        , "EventCond map:" + mapidString + " always: ASM");
                                }
                            }

                            uint event_addr = Program.ROM.p32(base_addr + 4);
                            if (!U.isSafetyOffset(event_addr))
                            {
                                continue;
                            }

                            EventScriptForm.ScanScript( list
                                , base_addr + 4
                                , true, false
                                , "EventScript map:" + mapidString + " always:" + U.To0xHexString(event_addr)
                                , tracelist
                                );
                        }

                        FEBuilderGBA.Address.AddAddress(list
                            , startaddr
                            , base_addr - startaddr + 12
                            , base_pointer
                            , "EventCond map:" + mapidString + " always:" + U.To0xHexString(startaddr)
                            , Address.DataTypeEnum.EVENTCOND_ALWAYS);
                    }
                    else if (condtype == CONDTYPE.TRAP)
                    {
                        for (; true; base_addr += 6)
                        {
                            if (!U.isSafetyOffset(base_addr + 6))
                            {
                                break;
                            }
                            if (Program.ROM.u8(base_addr) == 0)
                            {
                                break;
                            }

                            //nop
                        }
                        FEBuilderGBA.Address.AddAddress(list
                            , startaddr
                            , base_addr - startaddr + 6
                            , base_pointer
                            , "EventScript map:" + mapidString + " trap:" + U.To0xHexString(startaddr)
                            , Address.DataTypeEnum.EVENTTRAP);
                    }
                    else if (condtype == CONDTYPE.TUTORIAL)
                    {
                        for (; true; base_addr += 4)
                        {
                            if (!U.isSafetyOffset(base_addr + 4))
                            {
                                break;
                            }
                            if (Program.ROM.u8(base_addr) == 0)
                            {
                                break;
                            }

                            uint event_addr = Program.ROM.p32(base_addr + 0);
                            if (!U.isSafetyOffset(event_addr))
                            {
                                continue;
                            }

                            EventScriptForm.ScanScript( list
                                , base_addr + 0
                                , true, false
                                , "EventScript map:" + mapidString + " tutorial:" + U.To0xHexString(event_addr)
                                , tracelist
                                );
                        }

                        FEBuilderGBA.Address.AddAddress(list
                            , startaddr
                            , base_addr - startaddr + 4
                            , base_pointer
                            , "EventScript map:" + mapidString + " tutorial:" + U.To0xHexString(startaddr)
                            , Address.DataTypeEnum.POINTER);
                    }
                    else if (condtype == CONDTYPE.PLAYER_UNIT
                        || condtype == CONDTYPE.ENEMY_UNIT
                        || condtype == CONDTYPE.FREEMAP_PLAYER_UNIT
                        || condtype == CONDTYPE.FREEMAP_ENEMY_UNIT
                        )
                    {//ユニット配置
                        EventUnitForm.RecycleOldUnits(ref list
                            , "EventCond map:" + mapidString + " units:" + U.To0xHexString(startaddr)
                            ,base_pointer); 
                    }
                    else if (condtype == CONDTYPE.START_EVENT
                        || condtype == CONDTYPE.END_EVENT)
                    {//18,19 開始イベント 終了イベント
                        //イベントのスキャン.
                        EventScriptForm.ScanScript( list
                            , base_pointer
                            , true, false
                            , "EventScript map:" + mapidString + " chaptor:" + U.To0xHexString(startaddr)
                            , tracelist
                            );
                    }
                }
                FEBuilderGBA.Address.AddAddress(list
                    ,mapcond_addr
                    , (uint)MapCond.Count * 4
                    , mapcond_pointer 
                    , "EventCond map:" + mapidString
                    , Address.DataTypeEnum.POINTER);
            }
        }

        static void MakeVarsIDEventScan(List<UseValsID> list, uint event_addr,string info, List<uint> tracelist)
        {
            uint lastBranchAddr = 0;
            int unknown_count = 0;
            uint addr = event_addr;
            while (true)
            {
                //バイト列をイベント命令としてDisassembler.
                EventScript.OneCode code = Program.EventScript.DisAseemble(Program.ROM.Data, addr);
                if (EventScript.IsExitCode(code, addr, lastBranchAddr))
                {//終端命令
                    break;
                }
                else if (code.Script.Has == EventScript.ScriptHas.UNKNOWN)
                {
                    unknown_count++;
                    if (unknown_count > 10)
                    {//不明命令が10個連続して続いたら打ち切る
                        break;
                    }
                }
                else
                {
                    //少なくとも不明ではない.
                    unknown_count = 0;
                    if (code.Script.Has == EventScript.ScriptHas.LABEL_CONDITIONAL)
                    {//LABEL
                        lastBranchAddr = 0;
                    }
                    else if (code.Script.Has == EventScript.ScriptHas.IF_CONDITIONAL)
                    {//IF
                        lastBranchAddr = addr;
                    }

                    for (int i = 0; i < code.Script.Args.Length; i++)
                    {
                        EventScript.Arg arg = code.Script.Args[i];
                        if (arg.Type == EventScript.ArgType.TEXT
                            || arg.Type == EventScript.ArgType.CONVERSATION_TEXT
                            || arg.Type == EventScript.ArgType.SYSTEM_TEXT
                            || arg.Type == EventScript.ArgType.ONELINE_TEXT
                            )
                        {//テキスト関係の命令.
                            uint v = EventScript.GetArgValue(code, arg);
                            UseValsID.AppendTextID(list, FELint.Type.EVENTSCRIPT, event_addr, info, v, addr);
                        }
                        else if (arg.Type == EventScript.ArgType.POINTER_MENUEXTENDS)
                        {//分岐メニュー拡張
                            uint v = EventScript.GetArgValue(code, arg);
                            MenuExtendSplitMenuForm.MakeVarsIDArray(list, v);
                        }
                        else if (arg.Type == EventScript.ArgType.POINTER_UNITSSHORTTEXT)
                        {//unitに関連付けられたshort型データ
                            uint v = EventScript.GetArgValue(code, arg);
                            UnitsShortTextForm.MakeVarsIDArray(list, v);
                        }
                        else if (arg.Type == EventScript.ArgType.POINTER_TALKGROUP)
                        {//会話グループ拡張
                            uint v = EventScript.GetArgValue(code, arg);
                            EventTalkGroupFE7Form.MakeVarsIDArray(list, v);
                        }
                        else if (arg.Type == EventScript.ArgType.POINTER_EVENT)
                        {//イベント命令へジャンプするものをもっているらしい.
                            uint v = EventScript.GetArgValue(code, arg);

                            v = U.toOffset(v);
                            if (U.isSafetyOffset(v)         //安全で
                                && tracelist.IndexOf(v) < 0 //まだ読んだことがなければ
                                )
                            {
                                tracelist.Add(v);
                                MakeVarsIDEventScan(list, v, info,tracelist);
                            }
                        }
                        else if (arg.Type == EventScript.ArgType.SOUND || arg.Type == EventScript.ArgType.MUSIC)
                        {//音楽
                            uint v = EventScript.GetArgValue(code, arg);
                            UseValsID.AppendSongID(list, FELint.Type.EVENTSCRIPT, event_addr, info, v, addr);
                        }
                        else if (arg.Type == EventScript.ArgType.BG)
                        {//BG
                            uint v = EventScript.GetArgValue(code, arg);
                            UseValsID.AppendBGID(list, FELint.Type.EVENTSCRIPT, event_addr, info, v, addr);
                        }
                    }
                }
                addr += (uint)code.Script.Size;
            }

            if (Program.ROM.RomInfo.version() == 8)
            {
                MakeTextIDEventScanFE8SPEvent(list, event_addr, addr);
            }
        }
        static void MakeTextIDEventScanFE8SPEvent(List<UseValsID> list, uint event_addr, uint end_addr)
        {
            byte[][] table = new byte[][] { 
                 //ヴィガルドさんたちの悪巧み
                 new byte[]{ 0x20, 0x12, 0x2E, 0x00, 0x40, 0x05, 0x02, 0x00, 0xFF, 0xFF, 0x00, 0x00, 0x40, 0x05, 0x03, 0x00, 0xFF, 0xFF, 0x00, 0x00, 0x40, 0x05, 0x04, 0x00, 0xFF, 0xFF, 0x00, 0x00, 0x40, 0x0A, 0x00, 0x00 ,0xEE,0xEE,0xEE,0xEE },
                 //ノベラさん
                 new byte[]{ 0x40,0x05,0x07,0x00,0x01,0x00,0x00,0x00,0x40,0x0C,0x01,0x00,0x0C,0x00,0x07,0x00,0x40,0x05,0x07,0x00,0x02,0x00,0x00,0x00,0x40,0x0C,0x02,0x00,0x0C,0x00,0x07,0x00,0x20,0x08,0x00,0x00,0x40,0x05,0x02,0x00,0xFF,0xFF,0x00,0x00,0x20,0x09,0x03,0x00,0x20,0x08,0x01,0x00,0x40,0x05,0x02,0x00,0xFF,0xFF,0x00,0x00,0x20,0x09,0x03,0x00,0x20,0x08,0x02,0x00,0x40,0x05,0x02,0x00,0xFF,0xFF,0x00,0x00,0x20,0x08,0x03,0x00,0x20,0x1A,0x00,0x00,0x20,0x1B,0xEE,0xEE},
                 //レナックイベント
                 new byte[]{ 0x40,0x05,0x04,0x00,0xEE,0xEE,0x00,0x00,0x40,0x05,0x0D,0x00,0x00,0x00,0x00,0x00,0x40,0x05,0x01,0x00,0xFF,0xFF,0x00,0x00,0x21,0x07,0x00,0x00,0x40,0x05,0x01,0x00,0xFF,0xFF,0x00,0x00,0x21,0x07,0x00,0x00,0x40,0x05,0x01,0x00,0xFF,0xFF,0x00,0x00,0x21,0x07,0x00,0x00,0x40,0x05,0x01,0x00,0xFF,0xFF,0x00,0x00,0x21,0x07,0x00,0x00,0x40,0x05,0x01,0x00,0xFF,0xFF,0x00,0x00,0x21,0x07,0x00,0x00,0x40,0x05,0x01,0x00,0xFF,0xFF,0x00,0x00,0x21,0x07,0x00,0x00,0x40,0x0A,0x00,0x00,0xEE,0xEE,0xEE,0xEE },
                 //ルーテさんイベント ユニットが生きていたら分岐
                 new byte[]{ 0x22,0x33,0xEE,0x00,0x40,0x0C,0xEE,0xEE,0x0C,0x00,0x00,0x00,0x40,0x05,0x02,0x00,0xFF,0xFF,0x00,0x00,0x20,0x09,0xEE,0xEE,0x20,0x08,0xEE,0xEE,0x40,0x05,0x02,0x00,0xFF,0xFF,0x00,0x00,0x20,0x08,0xEE,0xEE,0x20,0x1B,0xEE,0xEE,0x20,0x1D,0x00,0x00,0x22,0x1B,0x00,0x00},
                 //編分岐
                 new byte[]{ 0x20,0x19,0x00,0x00,0x40,0x05,0x01,0x00,0x02,0x00,0x00,0x00,0x41,0x0C,0xEE,0xEE,0x0C,0x00,0x01,0x00,0x40,0x05,0x02,0x00,0xFF,0xFF,0x00,0x00,0x20,0x09,0xEE,0xEE,0x20,0x08,0xEE,0xEE,0x40,0x05,0x02,0x00,0xFF,0xFF,0x00,0x00,0x20,0x08,0xEE,0xEE,0x20,0x1A,0x00,0x00,0x20,0x1B,0xEE,0xEE},
            };


            foreach(byte[] bin in table)
            {
                uint addr = event_addr;
                bool[] mask = U.MakeMask2(bin, 0xFF, 0xEE);
                while(true)
                {
                    addr = U.GrepPatternMatch(Program.ROM.Data, bin, mask, addr, end_addr, 4);
                    if (addr == U.NOT_FOUND)
                    {
                        break;
                    }

                    string name = event_addr.ToString();
                    List<UseValsID> tempList = new List<UseValsID>();
                    for (uint i = 0; i < bin.Length; i += 2)
                    {
                        if (bin[i] != 0xFF)
                        {
                            continue;
                        }

                        uint textid = Program.ROM.u16(addr + i);
                        if (textid <= 0 || textid >= 0x7FFF)
                        {//規約違反があったので、これは探していたデータではない
                            tempList.Clear();
                            break;
                        }
                        UseValsID.AppendTextID(tempList, FELint.Type.EVENTSCRIPT, addr + i, "", textid, event_addr);
                    }

                    if (tempList.Count <= 0)
                    {
                        continue;
                    }

                    list.AddRange(tempList);
                    addr += (uint)bin.Length;
                }
            }
        }

        private void EventPointer_ValueChanged(object sender, EventArgs e)
        {
            if (sender == EventPointer)
            {
                InputFormRef.WriteButtonToYellow(WriteButton, true);
            }
        }

        void DrawEventUnits(uint event_addr, bool isPlayerUnitJoinOK)
        {
            uint start_mapid = (uint)this.MAP_LISTBOX.SelectedIndex;
            List<uint> tracelist = new List<uint>();

            List<U.AddrResult> list = new List<U.AddrResult>();
            uint mapid = MakeUnitPointerEventScan(ref list, "", event_addr, start_mapid, tracelist);
            int len = list.Count;
            for (int i = 0; i < len; i++)
            {
                if (list[i].tag != start_mapid)
                {
                    continue;
                }
                DrawUnits(list[i].addr, isPlayerUnitJoinOK,false);
            }
            MapPictureBox.InvalidateMap();
        }

        void DrawUnits(uint addr, bool isPlayerUnitJoinOK, bool isReDraw)
        {
            if (!U.isSafetyOffset(addr) || addr == EventUnitForm.INVALIDATE_UNIT_POINTER)
            {
                if (isReDraw)
                {
                    MapPictureBox.InvalidateMap();
                }
                return;
            }

            List<U.AddrResult> arlist = EventUnitForm.MakeList(addr);
            for (int k = 0; k < arlist.Count; k++)
            {
                uint assign = U.ParseUnitGrowAssign(Program.ROM.u8(arlist[k].addr + 3));
                if (assign == 0 && !isPlayerUnitJoinOK)
                {//自軍だが、現在追加は許可されていない。たぶん、進撃準備前の茶番だろうから無視する.
                    continue;
                }

                MapPictureBox.StaticItem item = EventUnitForm.DrawAfterPosUnit(arlist[k].addr);
                MapPictureBox.SetStaticItem("unit_" + k
                        , item.x
                        , item.y
                        , item.bitmap
                        , item.draw_x_add
                        , item.draw_y_add
                        );
            }

            if (isReDraw)
            {
                MapPictureBox.InvalidateMap();
            }
        }

        
        public static string GetNameOfAchievementFlag()
        {
            return R._("達成フラグ");
        }
        public static string GetExplainOfAchievementFlag()
        {
            return R._("イベントが発動すると有効になります。\r\n一度、「{0}」で指定されたフラグが有効になると、再度イベント動作条件を満たしても、もうイベントは動作しなくなります。\r\nもし、何度でもイベントを発動させたい場合は、「{0}」に 0 を指定してください。\r\n", GetNameOfAchievementFlag());
        }
        
        public static string GetExplainOfTrapTimer()
        {
            return R._("トラップの発動条件は、タイマーでターン単位で判断されます。\r\nタイマーは、初期タイマーからターンが経過する度に値が減り、0 になるとトラップが発動します。\r\nトラップが発動すると、タイマーはリピートタイマに設定されます。\r\n");
        }

        private void OBJECT_AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(sender is ListBoxEx))
            {
                return;
            }

            MapPictureBox.ClearStaticItem();
            if (CondTabControl.SelectedTab == tabPage0506070A0C)
            {
                if (OBJECT_UNIONTAB.SelectedTab == OBJECT_UNIONTAB_N0A)
                {//店
                    MapPictureBox.InvalidateMap();
                    return;
                }
                if (OBJECT_UNIONTAB.SelectedTab == OBJECT_UNIONTAB_N07)
                {//宝箱
                    MapPictureBox.InvalidateMap();
                    return;
                }
            }

            uint addr = InputFormRef.SelectToAddr( (ListBox)sender );
            if (! U.isSafetyOffset(addr+8))
            {
                MapPictureBox.InvalidateMap();
                return;
            }
            uint eventAddr = Program.ROM.p32(addr + 4);
            DrawEventUnits(eventAddr, true);
        }

        private void TUTORIAL_AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(sender is ListBoxEx))
            {
                return;
            }

            MapPictureBox.ClearStaticItem();

            uint addr = InputFormRef.SelectToAddr((ListBox)sender);
            if (!U.isSafetyOffset(addr + 4))
            {
                MapPictureBox.InvalidateMap();
                return;
            }
            uint eventAddr = Program.ROM.p32(addr + 0);
            DrawEventUnits(eventAddr, true);
        }
        static string GetExplainOfAchievementFlagCond1()
        {
            return R._("ただし、以下の条件時は実行しません。\r\nもし、{0}が0ではなく、且つ、{0}が既に有効だった場合は、イベントを実行しません。\r\n", GetNameOfAchievementFlag());
        }
        static string GetExplainOfAchievementFlagCond2()
        {
            return R._("\r\nまた、{0}が0でなければ、\r\nイベントを実行する前に、条件を達成したとして、{0}を有効にしてから、イベントを実行します。\r\nこの場合、{0}が有効になるので、次は実行されなくなります。\r\n", GetNameOfAchievementFlag());
        }
        string ExplainSampleCondTurn()
        {
            return 
                GetExplainOfAchievementFlagCond1() 
                + GetExplainOfAchievementFlagCond2() 
                ;
        }
        string ExplainSampleCondTurnFE7()
        {
            return
                GetExplainOfAchievementFlagCond1()
                + R._("もし、編が指定されたものでない場合は、イベントを実行しません。\r\n")
                + GetExplainOfAchievementFlagCond2()
                ;
        }
        string ExplainSampleCondTalk()
        {
            return
                GetExplainOfAchievementFlagCond1()
                + R._("もし、追加判定で「判定フラグ」が指定されていた場合、且つ、「判定フラグ」が無効であれば、実行されません。\r\n")
                + GetExplainOfAchievementFlagCond2()
                ;
        }
        string ExplainSampleCondTalkASM()
        {
            return
                GetExplainOfAchievementFlagCond1()
                + R._("もし、判定ASM関数の戻り値r0が1でなければ、実行されません。\r\n")
                + GetExplainOfAchievementFlagCond2()
                ;
        }
        string ExplainSampleAchievementFlag()
        {
            return
                GetExplainOfAchievementFlagCond1()
                + GetExplainOfAchievementFlagCond2()
                ;
        }

        private void TALK_N03_W12_ValueChanged(object sender, EventArgs e)
        {
            if (TALK_N03_W12.Value == 0)
            {//追加判定なし
                TALK_N03_W14.Value = 0;
                TALK_N03_J_14_FLAG.Hide();
                TALK_N03_W14.Hide();
                TALK_N03_L_14_FLAG.Hide();
            }
            else
            {//追加判定あり
                TALK_N03_J_14_FLAG.Show();
                TALK_N03_W14.Show();
                TALK_N03_L_14_FLAG.Show();
            }
        }

        //イベント領域のベース領域の強制割り当て
        public static uint PreciseEevntCondArea(uint mapid)
        {
            MapPointerNewPLISTPopupForm f = (MapPointerNewPLISTPopupForm)InputFormRef.JumpFormLow<MapPointerNewPLISTPopupForm>();
            f.Init(MapPointerForm.PLIST_TYPE.EVENT);
            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return 0;
            }

            uint plist = f.GetSelectPLIST();

            Undo.UndoData undodata = Program.Undo.NewUndoData("Precise EevntCondArea", mapid.ToString("X"));


            //イベント領域を新規に割り当てる
            byte[] data = new byte[EventCondForm.MapCond.Count * 4];

            uint write_addr = InputFormRef.AppendBinaryData(data, undodata);
            if (write_addr == U.NOT_FOUND)
            {
                Program.Undo.Rollback(undodata);
                return 0;
            }

            bool r = MapPointerForm.Write_Plsit(MapPointerForm.PLIST_TYPE.EVENT, plist, write_addr, undodata);
            if (!r)
            {
                Program.Undo.Rollback(undodata);
                return 0;
            }

            Program.Undo.Push(undodata);

            return plist;
        }

        private Size DrawEventListTurn(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            U.AddrResult ar = InputFormRef.SelectToAddrResult(lb, index);
            if (ar.isNULL())
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            if (!U.isSafetyOffset(ar.addr + 12))
            {
                return new Size(listbounds.X, listbounds.Y);
            }

            SolidBrush brush = new SolidBrush(lb.ForeColor);

            Font normalFont = lb.Font;
            Font boldFont = new Font(lb.Font, FontStyle.Bold);

            string text;
            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;
            int maxHeight = lineHeight;

            uint start_turn = Program.ROM.u8(ar.addr + 8);
            uint end_turn = Program.ROM.u8(ar.addr + 9);
            if (start_turn < end_turn)
            {
                text = R._("ターン:") + start_turn + "～" + end_turn;
            }
            else
            {
                text = R._("ターン:") + start_turn;
            }
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

            uint turntype = Program.ROM.u8(ar.addr + 10);
            if (turntype == 0)
            {
                text = R._("(自ターン)");
            }
            else if (turntype == 0x40)
            {
                text = R._("(友軍ターン)");
            }
            else if (turntype == 0x80)
            {
                text = R._("(敵軍ターン)");
            }
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

            int maxWidth = bounds.X;

            //次の行
            bounds = listbounds;
            bounds.Y += lineHeight;

            //このイベントに対応する増援があれば、描画する.
            bounds.X = DrawEventRelationIcons(ar, true, g, normalFont, brush, bounds, isWithDraw);

            bounds.X = DrawAchievementFlag(ar, g, boldFont, isWithDraw, bounds);
            //コメントを書けるなら書く.
            bounds.X = DrawComment(ar, g, normalFont, isWithDraw, bounds);
            //大きい行数の方を採用.
            bounds.X = Math.Max(maxWidth, bounds.X);

            bounds.Y += maxHeight;
            brush.Dispose();
            boldFont.Dispose();
            return new Size(bounds.X, bounds.Y);
        }

        public static Size DrawEventListTurnOneLiner(uint addr, ListBox lb, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (!U.isSafetyOffset(addr + 12))
            {
                return new Size(listbounds.X, listbounds.Y);
            }

            SolidBrush brush = new SolidBrush(lb.ForeColor);

            Font normalFont = lb.Font;
            Font boldFont = new Font(lb.Font, FontStyle.Bold);

            string text;
            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;
            int maxHeight = lineHeight;

            uint start_turn = Program.ROM.u8(addr + 8);
            uint end_turn = Program.ROM.u8(addr + 9);
            if (start_turn < end_turn)
            {
                text = R._("ターン:") + start_turn + "～" + end_turn;
            }
            else
            {
                text = R._("ターン:") + start_turn;
            }
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

            uint turntype = Program.ROM.u8(addr + 10);
            if (turntype == 0)
            {
                text = R._("(自ターン)");
            }
            else if (turntype == 0x40)
            {
                text = R._("(友軍ターン)");
            }
            else if (turntype == 0x80)
            {
                text = R._("(敵軍ターン)");
            }
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
            bounds.Y += lineHeight;

            brush.Dispose();
            boldFont.Dispose();
            return new Size(bounds.X, bounds.Y);
        }

        //ターン条件とマップオブジェクトのイベントに関連するアイコンの描画
        class EventRelationIcons
        {
            public enum TypeEnum
            {
                 Units //増援
                ,Shop  //店
                ,Items //アイテム
            };
            public TypeEnum Type;
            public List<U.AddrResult> List;
        }
        Dictionary<uint,EventRelationIcons> EventRelationIconsCache;

        int DrawEventRelationIcons(U.AddrResult ar, bool isTurnEvent, Graphics g, Font font, SolidBrush brush, Rectangle bounds, bool isWithDraw)
        {
            uint start_mapid = (uint)this.MAP_LISTBOX.SelectedIndex;

            EventRelationIcons icons;
            if (!EventRelationIconsCache.TryGetValue(ar.addr, out icons))
            {//キャッシュにないので作る必要あり.
                List<uint> tracelist = new List<uint>();

                icons = new EventRelationIcons();

                uint event_addr = Program.ROM.p32(ar.addr + 4);
                if (isTurnEvent)
                {
                    icons.Type = EventRelationIcons.TypeEnum.Units;
                    icons.List = new List<U.AddrResult>();
                    MakeUnitPointerEventScan(ref icons.List, "units", event_addr, start_mapid, tracelist);

                    if (icons.List.Count <= 0)
                    {//ユニットが見つからない場合、アイテムを探索
                        icons.Type = EventRelationIcons.TypeEnum.Items;
                        tracelist = new List<uint>();
                        MakeGiveItemEventScan(ref icons.List, event_addr, tracelist);
                    }
                }
                else
                {//Object
                    uint objtype = Program.ROM.u8(ar.addr + 10);
                    if (IsShopObjectType(objtype) && U.isSafetyOffset(event_addr))
                    {//店
                        icons.Type = EventRelationIcons.TypeEnum.Shop;
                        icons.List = new List<U.AddrResult>();
                        icons.List.Add(new U.AddrResult(event_addr, "Shop"));
                    }
                    else if (IsChestObjectType(objtype))
                    {//宝箱
                        uint type = Program.ROM.u8(ar.addr + 0);
                        if (type == 0x05 && U.isSafetyOffset(event_addr))
                        {//ランダム箱
                            icons.Type = EventRelationIcons.TypeEnum.Shop; //便宜上店扱い
                            icons.List = new List<U.AddrResult>();
                            icons.List.Add(new U.AddrResult(event_addr, "RandomChest"));
                        }
                        else
                        {//宝箱

                            icons.Type = EventRelationIcons.TypeEnum.Items;
                            icons.List = new List<U.AddrResult>();

                            uint contents = Program.ROM.u32(ar.addr + 4);
                            uint item_id = contents & 0xff;
                            if (item_id > 0)
                            {
                                uint gold = 0;
                                if (contents > 0x10000)
                                {//ゴールド取得?
                                    gold = (contents >> 16) & 0xffff;
                                }

                                icons.List.Add(new U.AddrResult(item_id, "Chest" , gold));
                            }
                        }
                    }
                    else
                    {//それ以外
                        icons.Type = EventRelationIcons.TypeEnum.Units;
                        icons.List = new List<U.AddrResult>();
                        MakeUnitPointerEventScan(ref icons.List, "units", event_addr, start_mapid, tracelist);

                        if (icons.List.Count <= 0)
                        {//ユニットが見つからない場合、アイテムを探索
                            icons.Type = EventRelationIcons.TypeEnum.Items;
                            tracelist = new List<uint>();
                            MakeGiveItemEventScan(ref icons.List, event_addr, tracelist);
                        }
                    }
                }

                //探索したのでキャッシュに追加.
                EventRelationIconsCache[ar.addr] = icons;
            }


            //変なアドレスを指定されたときに、問題にならないように描画数でリミッターをつける.
            int MAX_DRAW_COUNT = bounds.X + (14 * 20);

            //描画する
            if (icons.Type == EventRelationIcons.TypeEnum.Units)
            {//ユニット
                int len = icons.List.Count;
                for (int i = 0; i < len; i++)
                {
                    if (icons.List[i].tag != start_mapid)
                    {//現在のマップではない.
                        continue;
                    }
                    uint addr = icons.List[i].addr;
                    while (Program.ROM.u8(addr) != 0x0)
                    {
                        if (bounds.X >= MAX_DRAW_COUNT)
                        {
                            break;
                        }

                        Bitmap icon = EventUnitForm.DrawUnitIconOnly(addr);
                        U.MakeTransparent(icon);

                        Rectangle b = bounds;
                        b.Width = 14;
                        b.Height = 14;
                        bounds.X += U.DrawPicture(icon, g, isWithDraw, b);
                        icon.Dispose();

                        addr += Program.ROM.RomInfo.eventunit_data_size();
                        if (!U.isSafetyOffset(addr))
                        {
                            break;
                        }
                    }
                }
            }
            else if (icons.Type == EventRelationIcons.TypeEnum.Shop)
            {
                int len = icons.List.Count;
                for (int i = 0; i < len; i++)
                {
                    uint addr = icons.List[i].addr;
                    while (Program.ROM.u8(addr) != 0x0)
                    {
                        if (bounds.X >= MAX_DRAW_COUNT)
                        {
                            break;
                        }

                        uint itemid = Program.ROM.u8(addr);
                        Bitmap icon = ItemForm.DrawIcon(itemid); 
                        U.MakeTransparent(icon);

                        Rectangle b = bounds;
                        b.Width = 14;
                        b.Height = 14;
                        bounds.X += U.DrawPicture(icon, g, isWithDraw, b);
                        icon.Dispose();

                        addr += 2;
                        if (!U.isSafetyOffset(addr))
                        {
                            Debug.Assert(false);
                            break;
                        }
                    }
                }
            }
            else
            {//アイテム
                int len = icons.List.Count;
                for (int i = 0; i < len; i++)
                {
                    if (bounds.X >= MAX_DRAW_COUNT)
                    {
                        break;
                    }

                    U.AddrResult data = icons.List[i];

                    Bitmap icon = ItemForm.DrawIcon(data.addr); //便宜上 addr を利用する.
                    U.MakeTransparent(icon);

                    Rectangle b = bounds;
                    b.Width = 14;
                    b.Height = 14;
                    bounds.X += U.DrawPicture(icon, g, isWithDraw, b);
                    icon.Dispose();

                    if (data.tag > 0)
                    {//ゴールド取得イベント?
                        string text = R._("{0}ゴールド" , data.tag);
                        bounds.X += U.DrawText(text, g, font, brush, isWithDraw, bounds);
                    }
                }
            }
            return bounds.X;
        }

        private Size DrawEventListTalk(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            U.AddrResult ar = InputFormRef.SelectToAddrResult(lb, index);
            if (ar.isNULL())
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            if (!U.isSafetyOffset(ar.addr + 12))
            {
                return new Size(listbounds.X, listbounds.Y);
            }

            SolidBrush brush = new SolidBrush(lb.ForeColor);

            Font normalFont = lb.Font;
            Font boldFont = new Font(lb.Font, FontStyle.Bold);

            string text;
            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;
            int maxHeight = lineHeight;

            //話す
            uint from_unit = Program.ROM.u8(ar.addr + 8);
            uint to_unit = Program.ROM.u8(ar.addr + 9);

            Bitmap from_unit_image = UnitForm.DrawUnitMapFacePicture(from_unit);
            Bitmap to_unit_image = UnitForm.DrawUnitMapFacePicture(to_unit);

            U.MakeTransparent(from_unit_image);
            U.MakeTransparent(to_unit_image);

            Rectangle b = bounds;
            b.Width = lineHeight * 2;
            b.Height = lineHeight * 2;
            bounds.X += U.DrawPicture(from_unit_image, g, isWithDraw, b);
            maxHeight = Math.Max(maxHeight, b.Height);

            text = " -> ";
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

            b = bounds;
            b.Width = lineHeight * 2;
            b.Height = lineHeight * 2;
            bounds.X += U.DrawPicture(to_unit_image, g, isWithDraw, b);
            maxHeight = Math.Max(maxHeight, b.Height);

            from_unit_image.Dispose();
            to_unit_image.Dispose();
            int maxWidth = bounds.X;

            //コメントを書けるなら書く.
            bounds.X += DrawComment(ar, g, normalFont, isWithDraw, bounds);

            //下にフラグを書こう.
            b = bounds;
            b.X = maxWidth + 4;
            b.Y += lineHeight;
            maxWidth += DrawAchievementFlag(ar, g, boldFont, isWithDraw, b);

            //大きい行数の方を採用.
            bounds.X = Math.Max(maxWidth, bounds.X);

            bounds.Y += maxHeight;
            brush.Dispose();
            boldFont.Dispose();
            return new Size(bounds.X, bounds.Y);
        }
        public static Size DrawEventListTalkOneLiner(uint addr, ListBox lb, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (!U.isSafetyOffset(addr + 12))
            {
                return new Size(listbounds.X, listbounds.Y);
            }

            SolidBrush brush = new SolidBrush(lb.ForeColor);

            Font normalFont = lb.Font;
            Font boldFont = new Font(lb.Font, FontStyle.Bold);

            string text;
            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;

            //話す
            uint from_unit = Program.ROM.u8(addr + 8);
            uint to_unit = Program.ROM.u8(addr + 9);

            Bitmap from_unit_image = UnitForm.DrawUnitMapFacePicture(from_unit);
            Bitmap to_unit_image = UnitForm.DrawUnitMapFacePicture(to_unit);

            U.MakeTransparent(from_unit_image);
            U.MakeTransparent(to_unit_image);

            Rectangle b = bounds;
            b.Width = lineHeight;
            b.Height = lineHeight;
            bounds.X += U.DrawPicture(from_unit_image, g, isWithDraw, b);

            text = " -> ";
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

            b = bounds;
            b.Width = lineHeight;
            b.Height = lineHeight;
            bounds.X += U.DrawPicture(to_unit_image, g, isWithDraw, b);

            from_unit_image.Dispose();
            to_unit_image.Dispose();
            int maxWidth = bounds.X;

            bounds.Y += lineHeight;
            brush.Dispose();
            boldFont.Dispose();
            return new Size(bounds.X, bounds.Y);
        }
        private Size DrawEventListAlways(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            U.AddrResult ar = InputFormRef.SelectToAddrResult(lb, index);
            if (ar.isNULL())
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            if (!U.isSafetyOffset(ar.addr + 12))
            {
                return new Size(listbounds.X, listbounds.Y);
            }

            SolidBrush brush = new SolidBrush(lb.ForeColor);

            Font normalFont = lb.Font;
            Font boldFont = new Font(lb.Font, FontStyle.Bold);

            string text;
            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;
            int maxHeight = (int)lb.Font.Height;

            uint type = Program.ROM.u8(ar.addr + 0);
            if (type == 0x1)
            {//常時条件 判定フラグ
                uint flag = Program.ROM.u16(ar.addr + 8);

                Bitmap bitmap = ImageSystemIconForm.MusicIcon(13);
                Rectangle b = bounds;
                b.Width = lineHeight * 2;
                b.Height = lineHeight * 2;
                bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
                maxHeight = Math.Max(maxHeight, b.Height);

                text = R._("常時条件:");
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

                if (flag == 0)
                {
                    text = U.To0xHexString(flag) + "(" + R._("常に実行") + ")";
                }
                else if (Program.FlagCache.TryGetValue(flag, out text))
                {
                    text = U.To0xHexString(flag) + "(" + text + ")";
                }
                else
                {
                    text = U.To0xHexString(flag);
                }
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
            }
            else if (type == 0xB)
            {//範囲条件
                Bitmap bitmap = ImageSystemIconForm.MusicIcon(11);
                Rectangle b = bounds;
                b.Width = lineHeight * 2;
                b.Height = lineHeight * 2;
                bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
                maxHeight = Math.Max(maxHeight, b.Height);

                text = R._("範囲条件:");
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

                uint x1 = Program.ROM.u8(ar.addr + 8);
                uint y1 = Program.ROM.u8(ar.addr + 9);
                uint x2 = Program.ROM.u8(ar.addr + 10);
                uint y2 = Program.ROM.u8(ar.addr + 11);

                text = "(" + x1 + "," + y1 + ")～(" + x2 + "," + y2 + ")";
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
            }
            else
            {//常時条件asm
                Bitmap bitmap = ImageSystemIconForm.MusicIcon(3);
                Rectangle b = bounds;
                b.Width = lineHeight * 2;
                b.Height = lineHeight * 2;
                bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
                maxHeight = Math.Max(maxHeight, b.Height);

                text = R._("常時条件:");
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

                uint asmPointer = Program.ROM.u32(ar.addr + 8);
                string dummy;
                string asmName = Program.AsmMapFileAsmCache.GetASMName(asmPointer, false, out dummy);
                if (asmName == "")
                {
                    text = "(ASM:" + U.To0xHexString(U.toOffset(asmPointer)) + ")";
                }
                else
                {
                    text = "(ASM:" + asmName + ")";
                }
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
            }

            int maxWidth = bounds.X;

            //次の行にコメントを書けるなら書く.
            bounds = listbounds;
            bounds.X += lineHeight * 2; //アイコン部分をずらす.

            bounds.Y += lineHeight;
            bounds.X += DrawAchievementFlag(ar, g, boldFont, isWithDraw, bounds);
            bounds.X += DrawComment(ar, g, normalFont, isWithDraw, bounds);
            //大きい行数の方を採用.
            bounds.X = Math.Max(maxWidth, bounds.X);

            bounds.Y += maxHeight;
            brush.Dispose();
            boldFont.Dispose();
            return new Size(bounds.X, bounds.Y);
        }
        public static Size DrawEventListAlwaysOneLiner(uint addr, ListBox lb, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (!U.isSafetyOffset(addr + 12))
            {
                return new Size(listbounds.X, listbounds.Y);
            }

            SolidBrush brush = new SolidBrush(lb.ForeColor);

            Font normalFont = lb.Font;
            Font boldFont = new Font(lb.Font, FontStyle.Bold);

            string text;
            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;

            uint type = Program.ROM.u8(addr + 0);
            if (type == 0x1)
            {//常時条件 判定フラグ
                uint flag = Program.ROM.u16(addr + 8);

                Bitmap bitmap = ImageSystemIconForm.MusicIcon(13);
                Rectangle b = bounds;
                b.Width = lineHeight;
                b.Height = lineHeight;
                bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);

                text = R._("常時条件:");
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

                if (Program.FlagCache.TryGetValue(flag, out text))
                {
                    text = U.To0xHexString(flag) + "(" + text + ")";
                }
                else
                {
                    text = U.To0xHexString(flag);
                }
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
            }
            else if (type == 0xB)
            {//範囲条件
                Bitmap bitmap = ImageSystemIconForm.MusicIcon(11);
                Rectangle b = bounds;
                b.Width = lineHeight;
                b.Height = lineHeight;
                bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);

                text = R._("範囲条件:");
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

                uint x1 = Program.ROM.u8(addr + 8);
                uint y1 = Program.ROM.u8(addr + 9);
                uint x2 = Program.ROM.u8(addr + 10);
                uint y2 = Program.ROM.u8(addr + 11);

                text = "(" + x1 + "," + y1 + ")～(" + x2 + "," + y2 + ")";
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
            }
            else
            {//常時条件asm
                Bitmap bitmap = ImageSystemIconForm.MusicIcon(3);
                Rectangle b = bounds;
                b.Width = lineHeight;
                b.Height = lineHeight;
                bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);

                text = R._("常時条件:");
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

                uint asmPointer = Program.ROM.u32(addr + 8);
                string dummy;
                string asmName = Program.AsmMapFileAsmCache.GetASMName(asmPointer, false, out dummy);
                if (asmName == "")
                {
                    text = "(ASM:" + U.To0xHexString(U.toOffset(asmPointer)) + ")";
                }
                else
                {
                    text = "(ASM:" + asmName + ")";
                }
                bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
            }

            bounds.Y += lineHeight;
            brush.Dispose();
            boldFont.Dispose();
            return new Size(bounds.X, bounds.Y);
        }

        public static string GetObjectName(uint addr, out Bitmap out_bitmap)
        {
            uint type = Program.ROM.u8(addr + 0);
            uint objecttype = Program.ROM.u8(addr + 10);
            if (Program.ROM.RomInfo.version() == 8)
            {
                if (objecttype == 0x11)
                {
                    out_bitmap = ImageSystemIconForm.Throne();
                    return U.ToHexString(type) + ":" + R._("11=制圧");
                }
                else if (objecttype == 0x20)
                {
                    out_bitmap = ImageSystemIconForm.VillageCenter();
                    return U.ToHexString(type) + ":" + R._("20=村の中心(盗賊のターゲット)");
                }
                else if (objecttype == 0x10 && type == 0x5)
                {
                    out_bitmap = ImageSystemIconForm.House();
                    return U.ToHexString(type) + ":" + R._("10=民家");
                }
                else if (objecttype == 0x10)
                {
                    out_bitmap = ImageSystemIconForm.Village();
                    return U.ToHexString(type) + ":" + R._("10=民家");
                }
                else if (objecttype == 0x14 && type == 0x5)
                {
                    out_bitmap = ImageSystemIconForm.Chest();
                    return U.ToHexString(type) + ":" + R._("14=ランダム宝箱");
                }
                else if (objecttype == 0x14 && type == 0x7 )
                {
                    out_bitmap = ImageSystemIconForm.Chest();
                    return U.ToHexString(type) + ":" + R._("14=宝箱");
                }
                else if (objecttype == 0x12 && type == 0x8 )
                {
                    out_bitmap = ImageSystemIconForm.Door();
                    return U.ToHexString(type) + ":" + R._("12=扉");
                }
                else if (objecttype == 0x16 )
                {
                    out_bitmap = ImageSystemIconForm.Armory();
                    return U.ToHexString(type) + ":" + R._("16=武器屋");
                }
                else if (objecttype == 0x17 )
                {
                    out_bitmap = ImageSystemIconForm.Vendor();
                    return U.ToHexString(type) + ":" + R._("17=道具屋");
                }
                else if (objecttype == 0x18 )
                {
                    out_bitmap = ImageSystemIconForm.SecretShop();
                    return U.ToHexString(type) + ":" + R._("18=秘密の店");
                }
            }
            else
            {
                if (objecttype == 0xF)
                {
                    out_bitmap = ImageSystemIconForm.Throne();
                    return U.ToHexString(type) + ":" + R._("0F=制圧");
                }
                else if (objecttype == 0x1D)
                {
                    out_bitmap = ImageSystemIconForm.VillageCenter();
                    return U.ToHexString(type) + ":" + R._("1D=村の中心(盗賊のターゲット)");
                }
                else if (objecttype == 0xE && type == 0x5)
                {
                    out_bitmap = ImageSystemIconForm.House();
                    return U.ToHexString(type) + ":" + R._("0E=民家");
                }
                else if (objecttype == 0xE)
                {
                    out_bitmap = ImageSystemIconForm.Village();
                    return U.ToHexString(type) + ":" + R._("0E=民家");
                }
                else if (objecttype == 0x12 && type == 0x5 && Program.ROM.RomInfo.version() == 6)
                {//FE6のみ
                    out_bitmap = ImageSystemIconForm.Chest();
                    return U.ToHexString(type) + ":" + R._("12=イベント付き宝箱");
                }
                else if (objecttype == 0x12 && type == 0x7)
                {
                    out_bitmap = ImageSystemIconForm.Chest();
                    return U.ToHexString(type) + ":" + R._("12=宝箱");
                }
                else if (objecttype == 0x10 && type == 0x8)
                {
                    out_bitmap = ImageSystemIconForm.Door();
                    return U.ToHexString(type) + ":" + R._("10=扉");
                }
                else if (objecttype == 0x13)
                {
                    out_bitmap = ImageSystemIconForm.Armory();
                    return U.ToHexString(type) + ":" + R._("13=武器屋");
                }
                else if (objecttype == 0x14)
                {
                    out_bitmap = ImageSystemIconForm.Vendor();
                    return U.ToHexString(type) + ":" + R._("14=道具屋");
                }
                else if (objecttype == 0x15)
                {
                    out_bitmap = ImageSystemIconForm.SecretShop();
                    return U.ToHexString(type) + ":" + R._("15=秘密の店");
                }
            }

            if (type == 0x5)
            {
                if (objecttype == 0x22)
                {
                    out_bitmap = ImageSystemIconForm.Stairs();
                    return U.ToHexString(type) + ":" + R._("22=階段拡張");
                }
                if (objecttype == 0x21)
                {
                    out_bitmap = ClassForm.DrawWaitIcon(0x41);
                    return U.ToHexString(type) + ":" + R._("22=Raid");
                }
                if (objecttype == 0x13)
                {
                    out_bitmap = ImageSystemIconForm.ExitPoint();
                    return U.ToHexString(type) + ":" + R._("13=離脱");
                }
                if (objecttype == 0x19)
                {
                    out_bitmap = ImageSystemIconForm.Castle();
                    return U.ToHexString(type) + ":" + R._("19=到着");
                }
            }

            out_bitmap = ImageSystemIconForm.Cursol();
            return U.ToHexString(type) + ":" + U.ToHexString(objecttype) + "=" + "???";
        }

        public static string GetObjectName(uint addr)
        {
            uint type = Program.ROM.u8(addr + 0);
            uint objecttype = Program.ROM.u8(addr + 10);
            if (Program.ROM.RomInfo.version() == 8)
            {
                if (objecttype == 0x11)
                {
                    return U.ToHexString(type) + ":" + R._("11=制圧");
                }
                else if (objecttype == 0x20)
                {
                    return U.ToHexString(type) + ":" + R._("20=村の中心(盗賊のターゲット)");
                }
                else if (objecttype == 0x10 && type == 0x5)
                {
                    return U.ToHexString(type) + ":" + R._("10=民家");
                }
                else if (objecttype == 0x10)
                {
                    return U.ToHexString(type) + ":" + R._("10=民家");
                }
                else if (objecttype == 0x14 && type == 0x5)
                {
                    return U.ToHexString(type) + ":" + R._("14=ランダム宝箱");
                }
                else if (objecttype == 0x14 && type == 0x7)
                {
                    return U.ToHexString(type) + ":" + R._("14=宝箱");
                }
                else if (objecttype == 0x12 && type == 0x8)
                {
                    return U.ToHexString(type) + ":" + R._("12=扉");
                }
                else if (objecttype == 0x16)
                {
                    return U.ToHexString(type) + ":" + R._("16=武器屋");
                }
                else if (objecttype == 0x17)
                {
                    return U.ToHexString(type) + ":" + R._("17=道具屋");
                }
                else if (objecttype == 0x18)
                {
                    return U.ToHexString(type) + ":" + R._("18=秘密の店");
                }
            }
            else
            {
                if (objecttype == 0xF)
                {
                    return U.ToHexString(type) + ":" + R._("0F=制圧");
                }
                else if (objecttype == 0x1D)
                {
                    return U.ToHexString(type) + ":" + R._("1D=村の中心(盗賊のターゲット)");
                }
                else if (objecttype == 0xE && type == 0x5)
                {
                    return U.ToHexString(type) + ":" + R._("0E=民家");
                }
                else if (objecttype == 0xE)
                {
                    return U.ToHexString(type) + ":" + R._("0E=民家");
                }
                else if (objecttype == 0x12 && type == 0x5 && Program.ROM.RomInfo.version() == 6)
                {//FE6のみ
                    return U.ToHexString(type) + ":" + R._("12=イベント付き宝箱");
                }
                else if (objecttype == 0x12 && type == 0x7)
                {
                    return U.ToHexString(type) + ":" + R._("12=宝箱");
                }
                else if (objecttype == 0x10 && type == 0x8)
                {
                    return U.ToHexString(type) + ":" + R._("10=扉");
                }
                else if (objecttype == 0x13)
                {
                    return U.ToHexString(type) + ":" + R._("13=武器屋");
                }
                else if (objecttype == 0x14)
                {
                    return U.ToHexString(type) + ":" + R._("14=道具屋");
                }
                else if (objecttype == 0x15)
                {
                    return U.ToHexString(type) + ":" + R._("15=秘密の店");
                }
            }

            if (objecttype == 0x22 && type == 0x5)
            {
                return U.ToHexString(type) + ":" + R._("22=階段拡張");
            }

            return U.ToHexString(type) + ":" + U.ToHexString(objecttype) + "=" + "???";
        }

        private Size DrawEventListObject(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            U.AddrResult ar = InputFormRef.SelectToAddrResult(lb, index);
            if (ar.isNULL())
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            if (! U.isSafetyOffset(ar.addr + 12))
            {
                return new Size(listbounds.X, listbounds.Y);
            }

            SolidBrush brush = new SolidBrush(lb.ForeColor);

            Font normalFont = lb.Font;
            Font boldFont = new Font(lb.Font, FontStyle.Bold);

            string text;
            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;
            int maxHeight = (int)lb.Font.Height;

            //オブジェクト
            int x = (int)Program.ROM.u8(ar.addr + 8);
            int y = (int)Program.ROM.u8(ar.addr + 9);

            Bitmap bitmap;
            text = GetObjectName(ar.addr, out bitmap);

            Rectangle b = bounds;
            b.Width = lineHeight * 2;
            b.Height = lineHeight * 2;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
            maxHeight = Math.Max(maxHeight, b.Height);

            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

            b = bounds;
            b.Width = lineHeight * 2;
            b.Height = lineHeight * 2;
            maxHeight = Math.Max(maxHeight, b.Height);

            bitmap.Dispose();
            int maxWidth = bounds.X;

            //次の行にコメントを書けるなら書く.
            bounds = listbounds;
            bounds.Y += lineHeight;
            bounds.X += lineHeight * 2; //アイコンの部分無視する.

            //このイベントに対応する増援があれば、描画する.
            bounds.X = DrawEventRelationIcons(ar, false, g, normalFont, brush, bounds, isWithDraw);

            bounds.X = DrawAchievementFlag(ar, g, boldFont, isWithDraw, bounds);


            uint event_addr = Program.ROM.p32(ar.addr + 4);
            uint object_type = Program.ROM.u8(ar.addr + 10);
            if (event_addr  <= 0x01
                || IsShopObjectType(object_type) 
                || IsChestObjectType(object_type))
            {
                //制圧や扉用のダミーイベント 0x01
                //店と宝箱はコメントを出せない
                //NOP
            }
            else
            {
                bounds.X += DrawComment(ar, g, normalFont, isWithDraw, bounds);
            }
            //大きい行数の方を採用.
            bounds.X = Math.Max(maxWidth, bounds.X);

            bounds.Y += maxHeight;
            brush.Dispose();
            boldFont.Dispose();
            return new Size(bounds.X, bounds.Y);
        }

        public static Size DrawEventListObjectOneLiner(uint addr ,ListBox lb, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (!U.isSafetyOffset(addr + 12))
            {
                return new Size(listbounds.X, listbounds.Y);
            }

            SolidBrush brush = new SolidBrush(lb.ForeColor);

            Font normalFont = lb.Font;
            Font boldFont = new Font(lb.Font, FontStyle.Bold);

            string text;
            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;

            //オブジェクト
            int x = (int)Program.ROM.u8(addr + 8);
            int y = (int)Program.ROM.u8(addr + 9);

            Bitmap bitmap;
            text = GetObjectName(addr, out bitmap);

            Rectangle b = bounds;
            b.Width = lineHeight;
            b.Height = lineHeight;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);

            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);
            bitmap.Dispose();

            brush.Dispose();
            boldFont.Dispose();

            bounds.Y += lineHeight;
            return new Size(bounds.X, bounds.Y);
        }


        private Size DrawEventListTrap(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            U.AddrResult ar = InputFormRef.SelectToAddrResult(lb, index);
            if (ar.isNULL())
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            if (!U.isSafetyOffset(ar.addr + 8))
            {
                return new Size(listbounds.X, listbounds.Y);
            }

            SolidBrush brush = new SolidBrush(lb.ForeColor);

            Font normalFont = lb.Font;
            Font boldFont = new Font(lb.Font, FontStyle.Bold);

            string text;
            Rectangle bounds = listbounds;

            int lineHeight = (int)lb.Font.Height;
            int maxHeight = (int)lb.Font.Height;

            //トラップ
            uint type = Program.ROM.u8(ar.addr + 0);

            Bitmap bitmap;
            if (type == 0x01)
            {//バリスタ
                bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0, 1, true);
                uint archtype = Program.ROM.u8(ar.addr + 3);
                if (archtype == 0x35)
                {
                    text = U.ToHexString(type) + ":" + R._("35=ロングアーチ");
                    bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(Program.ROM.RomInfo.unit_wait_barista_id(), 0, true);
                }
                else if (archtype == 0x36)
                {
                    text = U.ToHexString(type) + ":" + R._("36=アイアンアーチ");
                    bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(Program.ROM.RomInfo.unit_wait_barista_id() + 1, 0, true);
                }
                else if (archtype == 0x37)
                {
                    text = U.ToHexString(type) + ":" + R._("37=キラーアーチ");
                    bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(Program.ROM.RomInfo.unit_wait_barista_id() + 2, 0, true);
                }
                else
                {
                    text = U.ToHexString(type) + ":" + U.ToHexString(archtype) + "=" + "???";
                    bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(Program.ROM.RomInfo.unit_wait_barista_id(), 0, true);
                }
                U.MakeTransparent(bitmap);
            }
            else if (type == 0x04 || type == 0x08)
            {//ダメージ床 or 炎
                if (type == 0x04)
                {
                    text = U.ToHexString(type) + ":" + R._("04=ダメージ床");
                }
                else
                {
                    text = U.ToHexString(type) + ":" + R._("08=炎");
                }
                int x = (int)Program.ROM.u8(ar.addr + 1);
                int y = (int)Program.ROM.u8(ar.addr + 2);
                bitmap = this.MapPictureBox.GetMapDotBitmap(x * 16 - 4, y * 16 - 8, 24, 24);
            }
            else if (type == 0x0B)
            {//地雷
                text = U.ToHexString(type) + ":" + R._("0B=地雷");
                bitmap = ImageItemIconForm.DrawIconWhereID(Program.ROM.RomInfo.itemicon_mine_id());
                U.MakeTransparent(bitmap);
            }
            else if (type == 0x0C && Program.ROM.RomInfo.version() == 8)
            {//ゴーゴンの卵
                text = U.ToHexString(type) + ":" + R._("0C=ゴーゴンの卵");
                bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x53, 2, true);
                U.MakeTransparent(bitmap);
            }
            else if (type == 0x05)
            {
                text = U.ToHexString(type) + ":" + R._("05=毒ガス");
                bitmap = ImageSystemIconForm.Cursol();
                U.MakeTransparent(bitmap);
            }
            else if (type == 0x07)
            {
                text = U.ToHexString(type) + ":" + R._("07=神の矢");
                bitmap = ImageSystemIconForm.Cursol();
                U.MakeTransparent(bitmap);
            }
            else
            {
                text = U.ToHexString(type) + ":" + "???";
                bitmap = ImageSystemIconForm.Cursol();
                U.MakeTransparent(bitmap);
            }

            Rectangle b = bounds;
            b.Width = lineHeight * 2;
            b.Height = lineHeight * 2;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
            maxHeight = Math.Max(maxHeight, b.Height);

            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);

            b = bounds;
            b.Width = lineHeight * 2;
            b.Height = lineHeight * 2;
            maxHeight = Math.Max(maxHeight, b.Height);

            bitmap.Dispose();
            int maxWidth = bounds.X;

            //次の行にコメントを書けるなら書く.
            bounds = listbounds;
            bounds.Y += lineHeight;
            bounds.X += lineHeight * 2; //アイコンの部分無視する.
            //大きい行数の方を採用.
            bounds.X = Math.Max(maxWidth, bounds.X);

            bounds.Y += maxHeight;
            brush.Dispose();
            boldFont.Dispose();
            return new Size(bounds.X, bounds.Y);
        }

        public static int DrawComment(U.AddrResult ar, Graphics g, Font font, bool isWithDraw, Rectangle bounds)
        {
            uint eventPointer = Program.ROM.u32(ar.addr + 4);
            return DrawComment(eventPointer, g, font, isWithDraw, bounds);
        }
        public static int DrawComment(uint eventPointer, Graphics g, Font font, bool isWithDraw, Rectangle bounds)
        {
            string text;
            string dummy;
            string eventName = Program.AsmMapFileAsmCache.GetEventName(eventPointer, out dummy);
            if (eventName == "")
            {
                return 0;
            }
            else
            {
                text = "//" + eventName;
            }

            SolidBrush commentBrush = new SolidBrush(OptionForm.Color_Comment_ForeColor());
            bounds.X += U.DrawText(text, g, font, commentBrush, isWithDraw, bounds);
            commentBrush.Dispose();

            return bounds.X;
        }

        //達成フラグの描画
        public static int DrawAchievementFlag(U.AddrResult ar, Graphics g, Font font, bool isWithDraw, Rectangle bounds)
        {
            return DrawAchievementFlag(ar.addr, g, font, isWithDraw, bounds);
        }
        public static int DrawAchievementFlag(uint addr, Graphics g, Font font, bool isWithDraw, Rectangle bounds)
        {
            uint flag = Program.ROM.u16(addr+2);
            if (flag <= 0)
            {//フラグが0なので描画しない
                return bounds.X;
            }

            Bitmap bitmap = ImageSystemIconForm.FlagIcon();
            U.MakeTransparent(bitmap);

            int lineHeight = font.Height;

            Rectangle b = bounds;
            b.Width = lineHeight;
            b.Height = lineHeight;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
            bitmap.Dispose();


            string text = ":" + U.ToHexString(flag);
            SolidBrush brush = new SolidBrush(OptionForm.Color_Control_ForeColor());
            bounds.X += U.DrawText(text, g, font, brush, isWithDraw, bounds);
            brush.Dispose();

            return bounds.X;
        }


        //ターンイベントで書き込みボタンを押したときに、リストを再度探索する.
        private void N02_WriteButton_Click(object sender, EventArgs e)
        {
            EventRelationIconsCache.Clear();
        }


        //イベント条件の指定を忘れる人が多いので、アイコンをつけて目立たせる.
        public static Size DrawFilterCombo(ComboBoxEx lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            string text = lb.Items[index].ToString();

            SolidBrush brush = new SolidBrush(lb.ForeColor);
            Font normalFont = lb.Font;
            Rectangle bounds = listbounds;

            int textmargineY = (ComboBoxEx.OWNER_DRAW_ICON_SIZE - (int)lb.Font.Height) / 2;

            Bitmap bitmap;
            if (Program.ROM.RomInfo.version() == 8)
            {
                switch (index + 1)
                {
                    case 1:   //TURN	ターン条件
                        //ぐるぐる
                        bitmap = ImageSystemIconForm.MusicIcon(12);
                        break;
                    case 2:   //TALK	会話条件(話すコマンド)
                        //顔アイコン
                        bitmap = ImageSystemIconForm.TalkIcon();
                        break;
                    case 3:   //OBJECT	マップオブジェクト(制圧ポイント、宝箱、扉、民家、村)
                        //宝箱だしたいなあ
                        bitmap = ImageSystemIconForm.Cursol();
                        break;
                    case 4:   //ALWAYS	常時条件(範囲条件、勝利条件等)
                        //目立つ緑のアイコン
                        bitmap = ImageSystemIconForm.MusicIcon(3);
                        break;
                    case 8:   //TUTORIAL	チュートリアル
                        //星マーク
                        bitmap = ImageSystemIconForm.MusicIcon(16);
                        break;
                    case 9:   //TRAP	トラップ(アーチ、ゴーゴンの卵、ダメージ床)
                        //バリスタ
                        bitmap = ImageSystemIconForm.BaristaIcon();
                        break;
                    case 10:   //TRAP	トラップ2
                        //バリスタ
                        bitmap = ImageSystemIconForm.BaristaIcon();
                        break;
                    case 11:   //PLAYER_UNIT	初めて普通の時の自軍配置
                        bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x1, 0, true);
                        break;
                    case 12:   //PLAYER_UNIT	難しい時の自軍配置
                        bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x2, 0, true);
                        break;
                    case 13:   //FREEMAP_PLAYER_UNIT	魔物出現マップ時 自軍配置1
                        bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x5, 0, true);
                        break;
                    case 14:   //FREEMAP_PLAYER_UNIT	魔物出現マップ時 自軍配置2
                        bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0xC, 0, true);
                        break;
                    case 15:   //FREEMAP_PLAYER_UNIT	魔物出現マップ時 自軍配置3
                        bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x16, 0, true);
                        break;
                    case 16:   //FREEMAP_ENEMY_UNIT	魔物出現マップ時 魔物配置1
                        bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x43, 2, true);
                        break;
                    case 17:   //FREEMAP_ENEMY_UNIT	魔物出現マップ時 魔物配置2
                        bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x45, 2, true);
                        break;
                    case 18:   //FREEMAP_ENEMY_UNIT	魔物出現マップ時 魔物配置3
                        bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x54, 2, true);
                        break;
                    case 19:    //START_EVENT	章開始イベント
                        //剣のアイコン
                        bitmap = ImageSystemIconForm.MusicIcon(10);
                        break;
                    case 20:    //END_EVENT	章終了イベント
                        //色補正アイコン
                        bitmap = ImageSystemIconForm.MusicIcon(8);
                        break;
                    default:
                        bitmap = ImageUtil.Blank(16, 16);
                        break;
                }
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                switch (index + 1)
                {
                    case 1:   //TURN	ターン条件
                        //ぐるぐる
                        bitmap = ImageSystemIconForm.MusicIcon(12);
                        break;
                    case 2:   //TALK	会話条件(話すコマンド)
                        //顔アイコン
                        bitmap = ImageSystemIconForm.TalkIcon();
                        break;
                    case 3:   //OBJECT	マップオブジェクト(制圧ポイント、宝箱、扉、民家、村)
                        //宝箱だしたいなあ
                        bitmap = ImageSystemIconForm.Cursol();
                        break;
                    case 4:   //ALWAYS	常時条件(範囲条件、勝利条件等)
                        //目立つ緑のアイコン
                        bitmap = ImageSystemIconForm.MusicIcon(3);
                        break;
                    case 5:   //TRAP	トラップ(アーチ、ダメージ床)（エリウッド）
                        //バリスタ
                        bitmap = ImageSystemIconForm.BaristaIcon();
                        break;
                    case 6:   //TRAP	トラップ(アーチ、ダメージ床)（ヘクトル）
                        //バリスタ
                        bitmap = ImageSystemIconForm.BaristaIcon();
                        break;
                    case 7:   //ENEMY_UNIT	敵配置（エリウッドノーマル）
                        bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x10, 2, true);
                        break;
                    case 8:   //ENEMY_UNIT	敵配置（エリウッドハード）
                        bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x11, 2, true);
                        break;
                    case 9:   //ENEMY_UNIT	敵配置（ヘクトルノーマル）
                        bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x1E, 2, true);
                        break;
                    case 10:   //ENEMY_UNIT	敵配置（ヘクトルハード）
                        bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x20, 2, true);
                        break;
                    case 11:   //PLAYER_UNIT	自軍配置（エリウッドノーマル）
                        bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x0, 0, true);
                        break;
                    case 12:   //PLAYER_UNIT	自軍配置（エリウッドハード）
                        bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x1, 0, true);
                        break;
                    case 13:   //PLAYER_UNIT	自軍配置（ヘクトルノーマル）
                        bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x2, 0, true);
                        break;
                    case 14:   //PLAYER_UNIT	自軍配置（ヘクトルハード）
                        bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x3, 0, true);
                        break;
                    case 15:    //START_EVENT	章開始イベント
                        //剣のアイコン
                        bitmap = ImageSystemIconForm.MusicIcon(10);
                        break;
                    case 16:    //END_EVENT	章終了イベント
                        //色補正アイコン
                        bitmap = ImageSystemIconForm.MusicIcon(8);
                        break;
                    default:
                        bitmap = ImageUtil.Blank(16, 16);
                        break;
                }
            }
            else
            {//6
                switch (index + 1)
                {
                    case 1:   //TURN	ターン条件
                        //ぐるぐる
                        bitmap = ImageSystemIconForm.MusicIcon(12);
                        break;
                    case 2:   //TALK	会話条件(話すコマンド)
                        //顔アイコン
                        bitmap = ImageSystemIconForm.TalkIcon();
                        break;
                    case 3:   //OBJECT	マップオブジェクト(制圧ポイント、宝箱、扉、民家、村)
                        //宝箱だしたいなあ
                        bitmap = ImageSystemIconForm.Cursol();
                        break;
                    case 4:   //ALWAYS	常時条件(範囲条件、勝利条件等)
                        //目立つ緑のアイコン
                        bitmap = ImageSystemIconForm.MusicIcon(3);
                        break;
                    case 5:   //PLAYER_UNIT	自軍配置
                        bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x0, 0, true);
                        break;
                    case 6:   //ENEMY_UNIT	敵配置
                        bitmap = ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(0x38, 2, true);
                        break;
                    case 7:    //END_EVENT	章終了イベント
                        //色補正アイコン
                        bitmap = ImageSystemIconForm.MusicIcon(8);
                        break;
                    default:
                        bitmap = ImageUtil.Blank(16, 16);
                        break;
                }
            }

            U.MakeTransparent(bitmap);

            //アイコンを描く
            Rectangle b = bounds;
            b.Width = ComboBoxEx.OWNER_DRAW_ICON_SIZE;
            b.Height = ComboBoxEx.OWNER_DRAW_ICON_SIZE;
            bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
            bitmap.Dispose();

            //見つからなかったので、普通にテキストを描く.
            bounds.X += U.DrawText(text, g, normalFont, brush, isWithDraw, bounds);


            brush.Dispose();

            bounds.Y += ComboBoxEx.OWNER_DRAW_ICON_SIZE;
            return new Size(bounds.X, bounds.Y);
        }

        private void OBJECT_N07_B4_ValueChanged(object sender, EventArgs e)
        {
            uint item_id = (uint)OBJECT_N07_W4.Value;
            uint gold = (uint)OBJECT_N07_W6.Value;
            if (item_id != Program.ROM.RomInfo.item_gold_id()
                && gold <= 0)
            {//アイテムなのでゴールドの欄を消します.
                OBJECT_N07_J_6.Visible = false;
                OBJECT_N07_W6.Visible = false;
                OBJECT_N07_X_4_GOLD_LABEL.Visible = false;
                OBJECT_N07_J_5.Visible = false;
                OBJECT_N07_B5.Visible = false;
                OBJECT_N07_L_4_ITEM.ErrorMessage = "";
                OBJECT_N07_W6.Value = 0; //おそらくゴールドの欄をゼロにしないと誤判定があるかもしれない.
                OBJECT_N07_B5.Value = 0;
            }
            else
            {//ゴールドまたは不明 ゴールドを表示する.
                OBJECT_N07_J_6.Visible = true;
                OBJECT_N07_W6.Visible = true;
                OBJECT_N07_X_4_GOLD_LABEL.Visible = true;
                OBJECT_N07_J_5.Visible = true;
                OBJECT_N07_B5.Visible = true;

                if (item_id == Program.ROM.RomInfo.item_gold_id())
                {
                    OBJECT_N07_L_4_ITEM.ErrorMessage = "";
                }
                else
                {
                    OBJECT_N07_L_4_ITEM.ErrorMessage = R._("ゴールドとアイテムの設定が不適切です。\r\n\r\nゴールドが入っている宝箱を作るには、アイテムを「{0} ゴールド」にしないといけません。\r\n逆に、ゴールドではなくアイテムが入っている宝箱の場合は、ゴールドの欄は0にしてください。\r\n"
                        , U.ToHexString(Program.ROM.RomInfo.item_gold_id()));
                }
            }

        }

        private void OBJECT_N0A_W2_ValueChanged(object sender, EventArgs e)
        {
            if (OBJECT_N0A_W2.Value > 0)
            {
                OBJECT_N0A_J_2_FLAG.ErrorMessage = R._("お店に「{0}」が設定されています。\r\nこの設定では、一度入店すると、再度入店できません。\r\n意図的にやっている場合を除き、ここに「{0}」を設定するべきではありません。", GetNameOfAchievementFlag());
            }
            else
            {
                OBJECT_N0A_J_2_FLAG.ErrorMessage = "";
            }
        }

        //テキストIDの取得.
        public static void MakeVarsIDArray(List<UseValsID> list)
        {
            List<uint> tracelist = new List<uint>();
            uint mapmax = MapSettingForm.GetDataCount();
            for (uint mapid = 0; mapid < mapmax; mapid++)
            {
                if (InputFormRef.DoEvents(null, "MakeVarsIDArray "+ U.ToHexString(mapid))) return;

                List<U.AddrResult> eventCondList = MakeEventScriptPointer(mapid);

                if (Program.ROM.RomInfo.version() == 7)
                {
                    List<U.AddrResult> tutorialCondList = MakeEventScriptForFE7Tutorial(mapid);
                    eventCondList.AddRange(tutorialCondList);
                }

                int count = eventCondList.Count;
                for (int i = 0; i < count; i++)
                {
                    U.AddrResult ar = eventCondList[i];
                    string info = "MAP " + U.ToHexString(mapid) + " " + ar.name;
                    EventCondForm.MakeVarsIDEventScan(list, ar.addr, info , tracelist);
                }

            }
        }
        public static void MakeVarsIDArrayByEventPointer(List<UseValsID> list, uint event_pointer, string name, List<uint> tracelist)
        {
            event_pointer = U.toOffset(event_pointer);
            if (!U.isSafetyOffset(event_pointer))
            {
                return;
            }
            uint event_addr = Program.ROM.p32(event_pointer);
            MakeVarsIDArrayByEventAddress(list,event_addr , name , tracelist);
        }
        public static void MakeVarsIDArrayByEventAddress(List<UseValsID> list, uint event_addr, string name, List<uint> tracelist)
        {
            if (!U.isSafetyOffset(event_addr))
            {
                return;
            }

            EventCondForm.MakeVarsIDEventScan(list, event_addr, name, tracelist);
        }

        public static uint GetMapID(List<Control> parentControls)
        {
            ListBox list = (ListBox)InputFormRef.FindObjectByForm<ListBox>(parentControls, "MAP_LISTBOX");
            if (list == null)
            {
                return U.NOT_FOUND;
            }
            uint mapid = (uint)list.SelectedIndex;
            return mapid;
        }
        public static uint GetEndEvent(uint mapid)
        {
            List<U.AddrResult> arlist = EventCondForm.MakePointerListBox(mapid, EventCondForm.CONDTYPE.END_EVENT);
            if (arlist.Count <= 0)
            {
                return U.NOT_FOUND;
            }
            return arlist[0].addr;
        }
        public static uint GetPlayerUnits(uint mapid)
        {
            List<U.AddrResult> arlist = EventCondForm.MakePointerListBox(mapid, EventCondForm.CONDTYPE.PLAYER_UNIT);
            if (arlist.Count <= 0)
            {
                return U.NOT_FOUND;
            }
            return arlist[0].addr;
        }
        public static uint GetEnemyUnits(uint mapid)
        {
            List<U.AddrResult> arlist = EventCondForm.MakePointerListBox(mapid, EventCondForm.CONDTYPE.ENEMY_UNIT);
            if (arlist.Count <= 0)
            {
                return U.NOT_FOUND;
            }
            return arlist[0].addr;
        }
        public static bool isTopLevelEvent(uint event_addr)
        {
            List<uint> tracelist = new List<uint>();
            uint mapmax = MapSettingForm.GetDataCount();
            for (uint mapid = 0; mapid < mapmax; mapid++)
            {
                List<U.AddrResult> eventCondList = MakeEventScriptPointer(mapid);
                int count = eventCondList.Count;
                for (int i = 0; i < count; i++)
                {
                    U.AddrResult ar = eventCondList[i];
                    if (ar.addr == event_addr)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void EventCondForm_Activated(object sender, EventArgs e)
        {
            //イベントの関連アイコンを取得しなおしたい
            EventRelationIconsCache.Clear();
        }

        uint ReConvertListBoxToObjectSize(ListBoxEx listbox)
        {
            string[]sp =listbox.Name.Split('_');
            string prefix = sp[0] + "_";            
            List<Control> controls = InputFormRef.GetAllControls(this);
            Control block = InputFormRef.FindObject(prefix, controls, "BlockSize");
            if (!(block is TextBoxEx))
            {
                return U.NOT_FOUND;
            }
            return U.atoi(block.Text);
        }

        void ClearData(ListBoxEx listbox)
        {
            uint destAddr = InputFormRef.SelectToAddr(listbox);
            if (destAddr == U.NOT_FOUND)
            {
                return;
            }

            DialogResult dr = R.ShowYesNo("このイベントを無効化してもよろしいですか？");
            if (dr != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            uint blockSize = ReConvertListBoxToObjectSize(listbox);
            if (blockSize == U.NOT_FOUND)
            {
                R.ShowStopError("オブジェクトのサイズを特定できませんでした");
                return;
            }


            Undo.UndoData undodata = Program.Undo.NewUndoData(this);
            if (blockSize >= 4 + 4)
            {
                uint type = Program.ROM.u16(destAddr);
                Program.ROM.write_fill(destAddr, blockSize, 0, undodata);
                Program.ROM.write_u16(destAddr, type, undodata);  //種類
                Program.ROM.write_u32(destAddr + 4, 1, undodata); //イベントを1にする
            }
            else
            {
                Program.ROM.write_fill(destAddr, blockSize, 0, undodata);
            }

            Program.Undo.Push(undodata);
            EventRelationIconsCache.Clear();

            //再描画と再選択.
            //listbox.Invalidate();
            U.ReSelectList(listbox);

            InputFormRef.ShowWriteNotifyAnimation(this, destAddr);
        }


        void CustomKeydownHandler(object sender, KeyEventArgs e , InputFormRef ifr)
        {
            if (e.KeyCode == Keys.Delete)
            {
                ClearData((ListBoxEx)sender);
                return;
            }
            if (e.Control)
            {
                EventRelationIconsCache.Clear();
            }
            ifr.GeneralAddressList_KeyDown(sender, e);
            if (e.Control)
            {
                EventRelationIconsCache.Clear();
            }
        }

        //Event Type00 にしたとき、フラグも00にする
        private void PreWriteHandler_OBJECT_W0_UNIONKEY(object sender, EventArgs e)
        {
            if (this.OBJECT_W0_UNIONKEY.Value == 0)
            {
                OBJECT_N05_W2.Value = 0;
                OBJECT_N06_W2.Value = 0;
                OBJECT_N07_W2.Value = 0;
                OBJECT_N08_W2.Value = 0;
                OBJECT_N0A_W2.Value = 0;
            }
        }

        private void PreWriteHandler_TALK_W0_UNIONKEY(object sender, EventArgs e)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                if (this.TALKFE6_W0_UNIONKEY.Value == 0)
                {
                    TALKFE6_N04_W2.Value = 0;
                    TALKFE6_N0D_W2.Value = 0;
                }
            }
            else
            {
                if (this.TALK_W0_UNIONKEY.Value == 0)
                {
                    TALK_N03_W2.Value = 0;
                    TALK_N04_W2.Value = 0;
                }
            }
        }

        private void PreWriteHandler_N02_W0(object sender, EventArgs e)
        {
            if (Program.ROM.RomInfo.version() == 7)
            {
                if (this.NFE702_W0.Value == 0)
                {
                    NFE702_W2.Value = 0;
                }
            }
            else
            {
                if (this.N02_W0.Value == 0)
                {
                    N02_W2.Value = 0;
                }
            }
        }

        private void PreWriteHandler_ALWAYS_W0_UNIONKEY(object sender, EventArgs e)
        {
            if (this.ALWAYS_W0_UNIONKEY.Value == 0)
            {
                ALWAYS_N0B_B2.Value = 0;
                ALWAYS_N01_W2.Value = 0;
                ALWAYS_N0E_W2.Value = 0;
                ALWAYS_N0D_W2.Value = 0;
            }

        }
        private void OnPostWriteHandler_ClearCache(object sender, EventArgs e)
        {
            //イベントの関連アイコンを取得しなおしたい
            EventRelationIconsCache.Clear();
        }

        private void OBJECT_N05_B10_ValueChanged(object sender, EventArgs e)
        {
            if (OBJECT_N05_W10.Value == 0x22)
            {
                OBJECT_N05_J_2_FLAG.Text = R._("階段ID");
                OBJECT_N05_L_2_FLAG.Hide();
                OBJECT_N05_J_4_EVENTORCHEST.Text = R._("1を設定");
                return;
            }

            OBJECT_N05_J_2_FLAG.Text = GetNameOfAchievementFlag();
            OBJECT_N05_L_2_FLAG.Show();

            if (Program.ROM.RomInfo.version() == 8 
                && OBJECT_N05_W10.Value == 0x14)
            {
                OBJECT_N05_J_4_EVENTORCHEST.Text = R._("宝箱の中身");
                return;
            }
            OBJECT_N05_J_4_EVENTORCHEST.Text = R._("イベント");
        }


        static void MakeFlagIDArrayOne(uint mapid, uint event_addr, uint index, List<UseFlagID> flaglist)
        {
            List<U.AddrResult> list = new List<U.AddrResult>();
            List<uint> tracelist = new List<uint>();
            MakeFlagIDEventScan(ref list, event_addr, tracelist);
            foreach (U.AddrResult ar in list)
            {
                UseFlagID.AppendUseFlagID(flaglist, FELint.Type.EVENTSCRIPT, U.atoi(ar.name),"", ar.addr, mapid, ar.tag);
            }
        }

        static void MakeFlagIDEventScan(ref List<U.AddrResult> list, uint event_addr, List<uint> tracelist)
        {
            uint lastBranchAddr = 0;
            int unknown_count = 0;
            uint addr = event_addr;
            while (true)
            {
                //バイト列をイベント命令としてDisassembler.
                EventScript.OneCode code = Program.EventScript.DisAseemble(Program.ROM.Data, addr);
                if (EventScript.IsExitCode(code, addr, lastBranchAddr))
                {//終端命令
                    break;
                }
                else if (code.Script.Has == EventScript.ScriptHas.UNKNOWN)
                {
                    unknown_count++;
                    if (unknown_count > 10)
                    {//不明命令が10個連続して続いたら打ち切る
                        break;
                    }
                }
                else
                {
                    //少なくとも不明ではない.
                    unknown_count = 0;

                    if (code.Script.Has == EventScript.ScriptHas.POINTER_UNIT_OR_EVENT)
                    {//イベント命令へジャンプするものをもっているらしい.
                        for (int i = 0; i < code.Script.Args.Length; i++)
                        {
                            EventScript.Arg arg = code.Script.Args[i];
                            if (arg.Type == EventScript.ArgType.POINTER_EVENT)
                            {
                                uint v = EventScript.GetArgValue(code, arg);

                                v = U.toOffset(v);
                                if (U.isSafetyOffset(v)         //安全で
                                    && tracelist.IndexOf(v) < 0 //まだ読んだことがなければ
                                    )
                                {
                                    tracelist.Add(v);
                                    MakeFlagIDEventScan(ref list, v, tracelist);
                                }
                            }
                        }
                    }
                    else if (code.Script.Has == EventScript.ScriptHas.LABEL_CONDITIONAL)
                    {//LABEL
                        lastBranchAddr = 0;
                    }
                    else 
                    {//テキスト関係の命令.
                        if (code.Script.Has == EventScript.ScriptHas.IF_CONDITIONAL)
                        {//IF
                            lastBranchAddr = addr;
                        }

                        for (int i = 0; i < code.Script.Args.Length; i++)
                        {
                            EventScript.Arg arg = code.Script.Args[i];
                            if (arg.Type == EventScript.ArgType.FLAG
                                )
                            {
                                uint v = EventScript.GetArgValue(code, arg);

                                if (U.FindList(list, v) == U.NOT_FOUND)
                                {
                                    list.Add(new U.AddrResult(
                                          v
                                        , event_addr.ToString()
                                        , addr
                                    ));
                                }
                            }
                        }
                    }
                }
                addr += (uint)code.Script.Size;
            }
        }

        public static void MakeFlagIDArray(uint mapid, List<UseFlagID> flaglist)
        {
            uint mapcond_addr = MapSettingForm.GetEventAddrWhereMapID(mapid);
            if (!U.isSafetyOffset(mapcond_addr))
            {
                return;
            }

            List<uint> tracelist = new List<uint>();

            List<U.AddrResult> list;
            list = MakePointerListBox(mapid, CONDTYPE.OBJECT);
            for (int i = 0; i < list.Count; i++)
            {
                uint addr = list[i].addr;
                if (!U.isSafetyOffset(addr + 12))
                {
                    break;
                }
                uint flag = Program.ROM.u16(addr + 2);
                uint event_addr = Program.ROM.u32(addr + 4);

                UseFlagID.AppendUseFlagID(flaglist, FELint.Type.EVENT_COND_OBJECT, addr, list[i].name, flag, mapid, (uint)i);
                MakeFlagIDArrayOne(mapid, event_addr, (uint)i, flaglist);
            }

            list = MakePointerListBox(mapid, CONDTYPE.TALK);
            for (int i = 0; i < list.Count; i++)
            {
                uint addr = list[i].addr;
                if (!U.isSafetyOffset(addr + Program.ROM.RomInfo.eventcond_talk_size()))
                {
                    break;
                }
                uint flag = Program.ROM.u16(addr + 2);
                uint event_addr = Program.ROM.u32(addr + 4);

                UseFlagID.AppendUseFlagID(flaglist, FELint.Type.EVENT_COND_TALK, addr, list[i].name, flag, mapid, (uint)i);
                MakeFlagIDArrayOne(mapid, event_addr, (uint)i, flaglist);
            }


            list = MakePointerListBox(mapid, CONDTYPE.TURN);
            for (int i = 0; i < list.Count; i++)
            {
                uint addr = list[i].addr;
                if (!U.isSafetyOffset(addr + 12))
                {
                    break;
                }
                uint flag = Program.ROM.u16(addr + 2);
                uint event_addr = Program.ROM.u32(addr + 4);

                UseFlagID.AppendUseFlagID(flaglist, FELint.Type.EVENT_COND_TURN, addr, list[i].name, flag, mapid, (uint)i);
                MakeFlagIDArrayOne(mapid, event_addr, (uint)i, flaglist);
            }

            list = MakePointerListBox(mapid, CONDTYPE.ALWAYS);
            for (int i = 0; i < list.Count; i++)
            {
                uint addr = list[i].addr;
                if (!U.isSafetyOffset(addr + 12))
                {
                    break;
                }
                uint type = Program.ROM.u16(addr + 0);
                uint flag = Program.ROM.u16(addr + 2);
                uint event_addr = Program.ROM.u32(addr + 4);

                UseFlagID.AppendUseFlagID(flaglist, FELint.Type.EVENT_COND_ALWAYS, addr, list[i].name, flag, mapid, (uint)i);
                if (type == 1)
                {
                    uint use_flag = Program.ROM.u16(addr + 8);
                    UseFlagID.AppendUseFlagID(flaglist, FELint.Type.EVENT_COND_ALWAYS, addr, list[i].name, use_flag, mapid, (uint)i);
                }
                MakeFlagIDArrayOne(mapid, event_addr, (uint)i, flaglist);

            }

            if (Program.ROM.RomInfo.version() == 8)
            {
                uint start_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 18)));
                uint end_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 19)));

                MakeFlagIDArrayOne(mapid, start_addr, 0, flaglist);
                MakeFlagIDArrayOne(mapid, end_addr, 0, flaglist);
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                uint start_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 14)));
                uint end_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 15)));

                MakeFlagIDArrayOne(mapid, start_addr, 0, flaglist);
                MakeFlagIDArrayOne(mapid, end_addr, 0, flaglist);
            }
            else if (Program.ROM.RomInfo.version() == 6)
            {
                uint end_addr = Program.ROM.u32((uint)(mapcond_addr + (4 * 6)));

                MakeFlagIDArrayOne(mapid, end_addr, 0, flaglist);
            }
        }

        public static Size DrawEventByAddr(string eventtext, ListBox lb, Graphics g, Rectangle listbounds, bool isWithDraw)
        {//設計失敗した。イベントの種類とアドレスがわからん
         //こういうことはやりたくないが、名前に入れるしか方法がない・・・

            int pos = eventtext.IndexOf(" 0x");
            if (pos >= 0)
            {
                uint eventAddr = U.atoh(eventtext.Substring(pos + 3));
                eventtext = eventtext.Substring(0, pos);

                for (int n = 0; n < MapCond.Count; n++)
                {
                    if (MapCond[n].Name == eventtext)
                    {
                        CONDTYPE type = MapCond[n].Type;
                        switch (type)
                        {
                            case CONDTYPE.ALWAYS:
                                return DrawEventListAlwaysOneLiner(eventAddr, lb, g, listbounds, isWithDraw);
                            case CONDTYPE.OBJECT:
                                return DrawEventListObjectOneLiner(eventAddr, lb, g, listbounds, isWithDraw);
                            case CONDTYPE.TALK:
                                return DrawEventListTalkOneLiner(eventAddr, lb, g, listbounds, isWithDraw);
                            case CONDTYPE.TURN:
                                return DrawEventListTurnOneLiner(eventAddr, lb, g, listbounds, isWithDraw);
                        }
                    }
                }
            }


            //不明なイベントなのでそのまま描画する.
            {
                SolidBrush brush = new SolidBrush(lb.ForeColor);
                Font normalFont = lb.Font;
                Rectangle b = listbounds;
                b.Width = 1024;
                b.Height = normalFont.Height;
                listbounds.X += U.DrawText(eventtext, g, normalFont, brush, isWithDraw, b);
                brush.Dispose();

                return new Size(listbounds.X, listbounds.Y);
            }
        }

    }
}
