using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class UnitCustomBattleAnimeForm : Form
    {
        public UnitCustomBattleAnimeForm()
        {
            InitializeComponent();

            ImageBattleAnimeForm.MakeComboBattleAnimeSP(L_1_BATTLEANIMESP_0);

            U.ConvertListBox(ClassForm.MakeClassList(), ref  this.N2_AddressList);
            this.InputFormRef = Init(this);
            this.InputFormRef.PostAddressListExpandsEvent += AddressListExpandsEvent;

            this.N2_InputFormRef = N2_Init(this);
            this.N2_InputFormRef.PostAddressListExpandsEvent += N2_AddressListExpandsEvent;
        }
        public InputFormRef InputFormRef;
        static InputFormRef Init(Form self)
        {
            return new InputFormRef(self
                , ""
                , 0
                , 4
                , (int i, uint addr) =>
                {//読込最大値検索
                    //00まで検索
                    return Program.ROM.u32(addr + 0) != 0;
                }
                , (int i, uint addr) =>
                {
                    uint b0 = Program.ROM.u8(addr + 0);
                    uint b1 = Program.ROM.u8(addr + 1);
                    uint w2 = Program.ROM.u16(addr + 2);
                    return U.ToHexString(w2) + " " + ImageBattleAnimeForm.getSPTypeName(b0, b1);
                }
                );
        }
        InputFormRef N2_InputFormRef;
        static InputFormRef N2_Init(Form self)
        {
            return new InputFormRef(self
                , "N2_"
                , Program.ROM.RomInfo.unit_custom_battle_anime_pointer()
                , 4
                , (int i, uint addr) =>
                {//読込最大値検索
                    if (i == 0) return true;
                    return U.isPointer(Program.ROM.u32(addr + 0));
                }
                , (int i, uint addr) =>
                {
                    string name = UnitForm.GetNameWhereCustomBattleAnime((uint)i);
                    return U.ToHexString(i) + " " + name;
                }
                );
        }
        private void UnitCustomBattleAnimeForm_Load(object sender, EventArgs e)
        {
        }


       private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
       {

       }

       public static string GetBattleAnimeHint(uint search_animeindex)
       {
           InputFormRef InputFormRef = Init(null);
           for (int id = 0; id < InputFormRef.DataCount; id++)
           {
               uint addr = InputFormRef.IDToAddr((uint)id);
               if (!U.isSafetyOffset(addr))
               {
                   continue;
               }
               for (int i = 0; true; i += 4)
               {
                   uint item = Program.ROM.u8((uint)(addr + i + 0));
                   uint sp = Program.ROM.u8((uint)(addr + i + 1));
                   uint anime = Program.ROM.u16((uint)(addr + i + 2));
                   if (item == 0 && sp == 0 && anime == 0)
                   {
                       break;
                   }
                   if (anime != search_animeindex)
                   {
                       continue;
                   }

                   //発見!
                   string name = UnitForm.GetNameWhereCustomBattleAnime((uint)id);
                   return name + " " + ImageBattleAnimeForm.getSPTypeName(item, sp);
               }
           }

           //ない
           return "";
       }

       //リストが拡張されたとき
       void AddressListExpandsEvent(object sender, EventArgs arg)
       {
           U.ReSelectList(this.N2_AddressList);
       }

       private void N2_AddressList_SelectedIndexChanged(object sender, EventArgs e)
       {
           uint addr = InputFormRef.SelectToAddr(N2_AddressList);
           if (addr == U.NOT_FOUND)
           {
               return ;
           }
           if (addr == 0)
           {
               return;
           }
           addr = Program.ROM.p32(U.toOffset(addr));
           this.InputFormRef.ReInit(addr);

           IndependencePanel.Visible = 
               UpdateIndependencePanel(addr,N2_AddressList.SelectedIndex);
       }

        //リストが拡張されたとき
       void N2_AddressListExpandsEvent(object sender, EventArgs arg)
       {
           U.ReSelectList(this.N2_AddressList);
       }

       //全データの取得
       public static void MakeAllDataLength(List<Address> list)
       {
           InputFormRef InputFormRef = Init(null);
           InputFormRef N2_InputFormRef = N2_Init(null);
           FEBuilderGBA.Address.AddAddress(list, N2_InputFormRef, "UnitCustomBattle", new uint[] { 0 });

           uint p = N2_InputFormRef.BaseAddress;
           for (int i = 0; i < N2_InputFormRef.DataCount; i++, p += N2_InputFormRef.BlockSize)
           {
               InputFormRef.ReInitPointer(p);
               FEBuilderGBA.Address.AddAddress(list, InputFormRef, "UnitCustomBattle" + U.To0xHexString(i), new uint[] { });
           }
       }

       //他のクラスでこのデータを参照しているか?
       bool UpdateIndependencePanel(uint current_addr,int current_index)
       {
           List<U.AddrResult> list = this.N2_InputFormRef.MakeList();
           for (int i = 0; i < list.Count; i++)
           {
               if (i == current_index)
               {//自分自身
                   continue;
               }
               uint p = Program.ROM.p32(list[i].addr);
               if (p == current_addr)
               {
                   return true;
               }
           }

           return false;
       }
       private void IndependenceButton_Click(object sender, EventArgs e)
       {
           if (this.N2_AddressList.SelectedIndex < 0)
           {
               return;
           }
           string name = this.N2_AddressList.Text;

           uint pointer = InputFormRef.SelectToAddr(N2_AddressList);
           if (pointer == U.NOT_FOUND)
           {
               return;
           }
           if (pointer == 0)
           {
               return;
           }
           uint currentP = Program.ROM.p32(U.toOffset(pointer));

           if (!U.isSafetyOffset(currentP))
           {
               return;
           }

           if (InputFormRef.BaseAddress != currentP)
           {
               return;
           }

           Undo.UndoData undodata = Program.Undo.NewUndoData(this, this.Name + " Independence");

           uint dataSize = (InputFormRef.DataCount + 1) * InputFormRef.BlockSize;
           PatchUtil.WriteIndependence(currentP, dataSize, pointer, name, undodata);
           Program.Undo.Push(undodata);

           InputFormRef.ShowWriteNotifyAnimation(this, currentP);

           this.ReloadListButton.PerformClick();
           U.ReSelectList(N2_AddressList);
       }
    }
}
