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
    public partial class ArenaEnemyWeaponForm : Form
    {
        public ArenaEnemyWeaponForm()
        {
            InitializeComponent();
            this.InputFormRef = Init(this);
            this.N_InputFormRef = N_Init(this);

            this.AddressList.OwnerDraw(ListBoxEx.DrawItemAndText, DrawMode.OwnerDrawFixed);
            this.N_AddressList.OwnerDraw(ListBoxEx.DrawItemAndText, DrawMode.OwnerDrawFixed);

            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
            this.N_InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , Program.ROM.RomInfo.arena_enemy_weapon_basic_pointer()
                , 1
                , (int i, uint addr) =>
                {
                    return i < 8;
                }
                , (int i, uint addr) =>
                {
                    string disp;
                    uint icon;
                    uint itemid = Program.ROM.u8(addr);
                    return U.ToHexString(itemid) + " " + ItemForm.GetItemName(itemid) + " ("+ GetBasicTypeName(i, out disp, out icon) + ")";
                }
                );
        }
        InputFormRef N_InputFormRef;
        static InputFormRef N_Init(Form self)
        {
            return new InputFormRef(self
                , "N_"
                , Program.ROM.RomInfo.arena_enemy_weapon_rankup_pointer()
                , 1
                , (int i, uint addr) =>
                {
                    return i < 0x1A;
                }
                , (int i, uint addr) =>
                {
                    string disp;
                    uint icon;
                    uint itemid = Program.ROM.u8(addr);
                    return U.ToHexString(itemid) + " " + ItemForm.GetItemName(itemid) + " (" + GetRankupTypeName(i, out disp, out icon) + ")";
                }
                );
        }

        public static List<U.AddrResult> MakeList()
        {
            InputFormRef InputFormRef = Init(null);
            return InputFormRef.MakeList();
        }

        private void ArenaEnemyWeaponForm_Load(object sender, EventArgs e)
        {
        }

        public static string GetBasicTypeName(int number,out string out_disp,out uint icontype)
        {
            if (number == 0x00)
            {
                icontype = 0;
                out_disp = R._("剣の基本となる武器です。誰でも使えるEランク武器を設定してください。");
                return R._("基本武器:剣");
            }
            if (number == 0x01)
            {
                icontype = 1;
                out_disp = R._("槍の基本となる武器です。誰でも使えるEランク武器を設定してください。");
                return R._("基本武器:槍");
            }
            if (number == 0x02)
            {
                icontype = 2;
                out_disp = R._("斧の基本となる武器です。誰でも使えるEランク武器を設定してください。");
                return R._("基本武器:斧");
            }
            if (number == 0x03)
            {
                icontype = 3;
                out_disp = R._("弓の基本となる武器です。誰でも使えるEランク武器を設定してください。");
                return R._("基本武器:弓");
            }
            if (number == 0x04)
            {
                icontype = 4;
                out_disp = R._("杖の基本となる武器です。(闘技場では利用できないので、0x00である必要があります)");
                return R._("基本武器:杖");
            }
            if (number == 0x05)
            {
                icontype = 5;
                out_disp = R._("理魔法の基本となる武器です。誰でも使えるEランク武器を設定してください。");
                return R._("基本武器:理魔法");
            }
            if (number == 0x06)
            {
                icontype = 6;
                out_disp = R._("光魔法の基本となる武器です。誰でも使えるEランク武器を設定してください。");
                return R._("基本武器:光魔法");
            }
            //if (number == 0x07)
            {
                icontype = 7;
                out_disp = R._("闇魔法の基本となる武器です。誰でも使えるEランク武器を設定してください。");
                return R._("基本武器:闇魔法");
            }
        }

        public static string GetRankupTypeName(int number, out string out_disp, out uint icontype)
        {
            if (number == 0x00)
            {
                icontype = 0;
                out_disp = R._("基本武器:剣と同じ武器にしてください。そうしないと、探索に失敗します。");
                return R._("ランクアップ:剣0");
            }
            if (number == 0x01)
            {
                icontype = 0;
                out_disp = R._("中ランクの剣アイテムを設定してください。");
                return R._("ランクアップ:剣1");
            }
            if (number == 0x02)
            {
                icontype = 0;
                out_disp = R._("最大ランクの剣アイテムを設定してください。");
                return R._("ランクアップ:剣2");
            }
            if (number == 0x03)
            {
                icontype = 0xFF;
                out_disp = R._("区切りです。0x00にしてください");
                return R._("区切り 0x00");
            }
            if (number == 0x04)
            {
                icontype = 1;
                out_disp = R._("基本武器:槍と同じ武器にしてください。そうしないと、探索に失敗します。");
                return R._("ランクアップ:槍0");
            }
            if (number == 0x05)
            {
                icontype = 1;
                out_disp = R._("中ランクの槍アイテムを設定してください。");
                return R._("ランクアップ:槍1");
            }
            if (number == 0x06)
            {
                icontype = 1;
                out_disp = R._("最大ランクの槍アイテムを設定してください。");
                return R._("ランクアップ:槍2");
            }
            if (number == 0x07)
            {
                icontype = 0xFF;
                out_disp = R._("区切りです。0x00にしてください");
                return R._("区切り 0x00");
            }
            if (number == 0x08)
            {
                icontype = 2;
                out_disp = R._("基本武器:斧と同じ武器にしてください。そうしないと、探索に失敗します。");
                return R._("ランクアップ:斧0");
            }
            if (number == 0x09)
            {
                icontype = 2;
                out_disp = R._("中ランクの斧アイテムを設定してください。");
                return R._("ランクアップ:斧1");
            }
            if (number == 0x0A)
            {
                icontype = 2;
                out_disp = R._("最大ランクの斧アイテムを設定してください。");
                return R._("ランクアップ:斧2");
            }
            if (number == 0x0B)
            {
                icontype = 0xFF;
                out_disp = R._("区切りです。0x00にしてください");
                return R._("区切り 0x00");
            }
            if (number == 0x0C)
            {
                icontype = 3;
                out_disp = R._("基本武器:弓と同じ武器にしてください。そうしないと、探索に失敗します。");
                return R._("ランクアップ:弓0");
            }
            if (number == 0x0D)
            {
                icontype = 3;
                out_disp = R._("中ランクの弓アイテムを設定してください。");
                return R._("ランクアップ:弓1");
            }
            if (number == 0x0E)
            {
                icontype = 3;
                out_disp = R._("最大ランクの弓アイテムを設定してください。");
                return R._("ランクアップ:弓2");
            }
            if (number == 0x0F)
            {
                icontype = 0xFF;
                out_disp = R._("区切りです。0x00にしてください");
                return R._("区切り 0x00");
            }
            if (number == 0x10)
            {
                icontype = 5;
                out_disp = R._("基本武器:理魔法と同じ武器にしてください。そうしないと、探索に失敗します。");
                return R._("ランクアップ:理魔法0");
            }
            if (number == 0x11)
            {
                icontype = 5;
                out_disp = R._("中ランクの理アイテムを設定してください。");
                return R._("ランクアップ:理魔法1");
            }
            if (number == 0x12)
            {
                icontype = 5;
                out_disp = R._("最大ランクの理魔法アイテムを設定してください。");
                return R._("ランクアップ:理魔法2");
            }
            if (number == 0x13)
            {
                icontype = 0xFF;
                out_disp = R._("区切りです。0x00にしてください");
                return R._("区切り 0x00");
            }
            if (number == 0x14)
            {
                icontype = 6;
                out_disp = R._("基本武器:光魔法と同じ武器にしてください。そうしないと、探索に失敗します。");
                return R._("ランクアップ:光魔法0");
            }
            if (number == 0x15)
            {
                icontype = 6;
                out_disp = R._("最大ランクの光魔法アイテムを設定してください。");
                return R._("ランクアップ:光魔法1");
            }
            if (number == 0x16)
            {
                icontype = 0xFF;
                out_disp = R._("区切りです。0x00にしてください");
                return R._("区切り 0x00");
            }
            if (number == 0x17)
            {
                icontype = 7;
                out_disp = R._("基本武器:闇魔法と同じ武器にしてください。そうしないと、探索に失敗します。");
                return R._("ランクアップ:闇魔法0");
            }
            if (number == 0x18)
            {
                icontype = 0xFF;
                out_disp = R._("区切りです。0x00にしてください");
                return R._("区切り 0x00");
            }
            //number == 0x19
            {
                icontype = 0xFF;
                out_disp = R._("終端です。必ず0xFFにしてください。");
                return R._("終端 0xFF");
            }
        }

        //全データの取得
        public static void MakeAllDataLength(List<Address> list)
        {
            {
                InputFormRef InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, InputFormRef, "ArenaEnemyWeapon", new uint[] { });
            }
            {
                InputFormRef N_InputFormRef = Init(null);
                FEBuilderGBA.Address.AddAddress(list, N_InputFormRef, "ArenaEnemyWeapon", new uint[] { });
            }
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string disp;
            uint icon;
            string label = GetBasicTypeName( this.AddressList.SelectedIndex, out disp, out icon);

            J_0_ITEM.Text = label;
            X_INFO.Text = disp;
            X_INFO_ICON.IconNumber = icon;
        }

        private void N_AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string disp;
            uint icon;
            string label = GetRankupTypeName(this.N_AddressList.SelectedIndex, out disp, out icon);

            N_J_0_ITEM.Text = label;
            N_X_INFO.Text = disp;
            N_X_INFO_ICON.IconNumber = icon;
        }
    }
}
