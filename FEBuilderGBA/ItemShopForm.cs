using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ItemShopForm : Form
    {
        public ItemShopForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawItemAndText, DrawMode.OwnerDrawFixed);

            this.InputFormRef = Init(this);
            this.InputFormRef.PreWriteHandler += OnPreWrite;
            U.ConvertListBox(MakeShopList(), ref SHOP_LIST);

            this.InputFormRef.AddressListExpandsEvent += AddressListExpandsEvent;
            this.InputFormRef.MakeGeneralAddressListContextMenu(true);
        }

        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , 0
                , 2
                , (int i, uint addr) =>
                {
                    return Program.ROM.u8(addr)!=0x00;
                }
                , (int i, uint addr) =>
                {
                    uint item_id = Program.ROM.u8(addr);
                    return U.ToHexString(item_id) + " " + ItemForm.GetItemName(item_id);
                }
                );
        }
        private void ItemShopForm_Load(object sender, EventArgs e)
        {
        }

        public static void ClearCache()
        {
            g_ShopListCache = null;
        }

        static List<U.AddrResult>  g_ShopListCache = null;
        static List<U.AddrResult> MakeShopList()
        {
            List<U.AddrResult> ret = g_ShopListCache;
            if (ret == null)
            {
                ret = MakeShopListLow();
                g_ShopListCache = ret;
            }
            if (g_ShopListCache == null)
            {//スレッド競合の可能性?
                if (ret != null)
                {//値が取れているならその値を返す
                    return ret;
                }
                //取れていないならば、仕方ないので、再取得.
                return MakeShopListLow();
            }
            return ret;
        }
        static List<U.AddrResult> MakeShopListLow()
        {
            List<U.AddrResult> ret = new List<U.AddrResult>();
            uint p;

            //編成準備店.
            p = Program.ROM.RomInfo.item_shop_hensei_pointer();
            if (p != 0)
            {
                ret.Add(new U.AddrResult(
                      Program.ROM.p32(p)
                    , R._("編成準備店")
                    , p));
            }
            if (Program.ROM.RomInfo.version() >= 8)
            {
                //まずワールドマップイベントをスキャン.
                List<U.AddrResult> worldmaplist = WorldMapPointForm.MakeWorldMapPointList();
                for(uint i = 0 ; i < worldmaplist.Count ; i ++)
                {
                    List<U.AddrResult> shops =
                        WorldMapPointForm.GetShopAddr( U.toOffset(worldmaplist[(int)i].addr) );
                    foreach (U.AddrResult shop in shops)
                    {
                        if ( Program.ROM.u8(shop.addr) == 0 )
                        {//店に品物がない.
                            continue;
                        }

                        ret.Add(shop);
                    }
                }
            }
            List<U.AddrResult> maplist = MapSettingForm.MakeMapIDList();
            for (int n = 0; n < maplist.Count; n++)
            {
                uint addr = MapSettingForm.GetEventAddrWhereMapID((uint)n);
                if (!U.isSafetyOffset(addr))
                {
                    continue;
                }
                string mapname = MapSettingForm.GetMapName((uint)n);

                List<U.AddrResult> shops = EventCondForm.MakeShopPointerListBox(addr);
                foreach (U.AddrResult shop in shops)
                {
                    if (Program.ROM.u8(shop.addr) == 0)
                    {//店に品物がない.
                        continue;
                    }
                    shop.name = mapname + " " + shop.name;
                    ret.Add(shop);
                }
            }

            return ret;
        }

        private void SHOP_LIST_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint addr = InputFormRef.SelectToAddr(this.SHOP_LIST, this.SHOP_LIST.SelectedIndex);

            this.InputFormRef.ReInit(addr);
        }

        public void JumpTo(uint addr)
        {
            uint select = InputFormRef.AddrToSelect(this.SHOP_LIST,addr);
            if (select != U.NOT_FOUND)
            {
                this.SHOP_LIST.SelectedIndex = (int)select;
                return;
            }
            //店リストになければアドレス直地.
            InputFormRef.ReInit(addr);
        }
        //リストが拡張されたとき
        void AddressListExpandsEvent(object sender, EventArgs arg)
        {
            int index = this.SHOP_LIST.SelectedIndex;
            U.ConvertListBox(MakeShopList(), ref SHOP_LIST);
            U.ForceUpdate(this.SHOP_LIST, index);
        }

        public static void MakeAllDataLength(List<Address> list)
        {
            string name = "Shop";
            List<U.AddrResult> shop_arlist = MakeShopList();
            for (int i = 0; i < shop_arlist.Count; i++)
            {
                uint shop_addr = shop_arlist[i].addr;
                InputFormRef InputFormRef = Init(null);
                InputFormRef.ReInit(shop_addr);

                FEBuilderGBA.Address.AddAddress(list, shop_addr
                    , (uint)(InputFormRef.DataCount+1) * InputFormRef.BlockSize
                    , shop_arlist[i].tag
                    , name
                    , FEBuilderGBA.Address.DataTypeEnum.BIN);
            }
        }
        public ListBox GetShopListBox()
        {
            return this.SHOP_LIST;
        }
        private void OnPreWrite(object sender, EventArgs e)
        {
            if (this.B0.Value == 0)
            {
                this.B1.Value = 0;
            }
        }
    }
}
