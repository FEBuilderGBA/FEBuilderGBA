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
    public partial class ProcsScriptForm : Form
    {

        public ProcsScriptForm()
        {
            InitializeComponent();

            this.AddressList.OwnerDraw(ListBoxEx.DrawTextOnly, DrawMode.OwnerDrawFixed, true);

        }

        List<Address> ProcsList;
        ToolTipEx ToolTip;
        private void ProcsScriptForm_Shown(object sender, EventArgs e)
        {
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                pleaseWait.DoEvents(R._("準備しています"));

                ProcsList = new List<Address>();
                List<DisassemblerTrumb.LDRPointer> ldrmap = DisassemblerTrumb.MakeLDRMap(Program.ROM.Data, 0x100);
                FindProc find = new FindProc(ProcsList, null,ldrmap);

                this.AddressList.BeginUpdate();
                this.AddressList.Items.Clear();

                for (int i = 0; i < ProcsList.Count; i++)
                {
                    Address address = ProcsList[i];
                    string name = U.ToHexString(address.Addr) + " " + address.Info.Substring(6);
                    this.AddressList.Items.Add(name);
                }
                this.AddressList.EndUpdate();
            }
        }
        static string Get6CName(uint addr, uint length, string hint)
        {
            string name = U.ToHexString(addr);
            if (hint.Length > 0)
            {
                name += " " + hint;
            }
            else
            {
                name += Get6CNameAddr(addr, length);
            }

            return name;
        }
        //名前を見つけたら利用する.
        static string Get6CNameAddr(uint addr, uint length)
        {
            uint end = addr + length ;
            for (uint i = addr; i < end; i += 8)
            {
                if (Program.ROM.u16(i + 0) != 0x01)
                {
                    continue;
                }
                uint nameP = Program.ROM.u32(i + 4);
                if (! U.isSafetyPointer(nameP))
                {
                    continue;
                }
                string name = TextForm.Direct(nameP);
                if (name == "")
                {
                    continue;
                }
                return " " + name;
            }
            return "";
        }
        
        //全データの取得
        static string Get6CName2(uint addr, uint length, string hint, string hint2)
        {
            string name = "Procs ";
            if (hint.Length > 0)
            {
                name += hint;
            }
            else if (hint2.Length > 0)
            {
                name += hint2;
            }
            else
            {
                name += Get6CNameAddr(addr, length);
            }

            return name;
        }

        class FindProc
        {
            Dictionary<uint, string> ProcsName;
            Dictionary<uint, string> ProcsNameByAddr;
            Dictionary<uint, bool> AlreadyMatch = new Dictionary<uint, bool>();
            List<Address> List;
            List<Address> SubDataList;

            public FindProc(List<Address> list, List<Address> subDataList, List<DisassemblerTrumb.LDRPointer> ldrmap)
            {
                this.ProcsName = U.LoadDicResource(U.ConfigDataFilename("6c_name_"));
                this.ProcsNameByAddr = MakeProcNameByAddr(this.ProcsName);
                this.List = list;
                this.SubDataList = subDataList;

                for (int i = 0; i < ldrmap.Count; i++)
                {
                    if (Program.AsmMapFileAsmCache.IsStopFlagOn()) return;

                    uint addr = ldrmap[i].ldr_data;
                    if (!U.isSafetyPointer(addr))
                    {
                        continue;
                    }
                    addr = U.toOffset(addr);
                    if (this.AlreadyMatch.ContainsKey(addr))
                    {//既に知っている.
                        continue;
                    }
                    FindProcOne(ldrmap[i].ldr_data_address, addr , false);
                }
                foreach (var pair in this.ProcsName)
                {
                    if (Program.AsmMapFileAsmCache.IsStopFlagOn()) return;

                    uint pointer = pair.Key;
                    pointer = U.toOffset(pointer);
                    if (!U.isSafetyOffset(pointer))
                    {
                        continue;
                    }
                    uint addr = Program.ROM.p32(pointer);
                    if (this.AlreadyMatch.ContainsKey(addr))
                    {//既に知っている.
                        continue;
                    }
                    FindProcOne(pointer, addr, false);
                }

            }

            Dictionary<uint, string> MakeProcNameByAddr(Dictionary<uint, string> procsName)
            {
                Dictionary<uint, string> ret = new Dictionary<uint, string>();
                foreach (var pair in procsName)
                {
                    if (!U.isSafetyOffset(pair.Key))
                    {
                        continue;
                    }
                    uint p = Program.ROM.p32(pair.Key);
                    ret[p] = pair.Value;
                }
                return ret;
            }

            bool IsTooShort(uint length, uint addr)
            {
                if (length >= 8 * 2)
                {
                    return true;
                }
                //短すぎる
                return false;
            }

            void FindProcOne(uint pointer,uint addr, bool child6C)
            {
                uint length = CalcLengthAndCheck(addr);
                if (U.NOT_FOUND == length)
                {
                    this.AlreadyMatch[addr] = false; //ダメだったということを記録しておこう
                    return;
                }

                if (child6C == false)
                {
                    if (IsTooShort(length,addr) == false)
                    {
                        this.AlreadyMatch[addr] = false; //ダメだったということを記録しておこう
                        return;
                    }
                }

                string name = Get6CName2(addr, length, U.at(ProcsName, pointer), U.at(ProcsNameByAddr, addr));
                FEBuilderGBA.Address.AddAddress(this.List, addr, length, pointer, name , Address.DataTypeEnum.PROCS);
                this.AlreadyMatch[addr] = true;

                uint end = addr + length;
                for (; addr < end; addr += 8 )
                {
                    uint code = Program.ROM.u8(addr);
                    //uint code = Program.ROM.u16(addr + 0);
                    if (code >= 5 && code <= 0xA)
                    {//5 or 6 or 7 or 8 or 9 or Aは、内部にProcsをネストする
                        uint arg = Program.ROM.p32(addr + 4);

                        bool result;
                        if (this.AlreadyMatch.TryGetValue(arg, out result))
                        {//既に知っている.
                            if (result)
                            {
                                continue;
                            }
                        }
                        FindProcOne(addr + 4, arg, true);
                    }
                    else if (code == 0x01)
                    {//Set name
                        if (this.SubDataList != null)
                        {
                            FEBuilderGBA.Address.AddCString(this.SubDataList, addr + 4);
                        }
                    }
                }
            }
        }

        public static void MakeAllDataLength(List<Address> list, List<DisassemblerTrumb.LDRPointer> ldrmap)
        {
            FindProc find = new FindProc(list,list, ldrmap);
        }

        private void AddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.AddressList.SelectedIndex < 0 || this.AddressList.SelectedIndex >= ProcsList.Count)
            {
                return;
            }
            uint addr = ProcsList[this.AddressList.SelectedIndex].Addr;
            JumpToInnerCurrentTab(addr);
        }


        //長さを求める.(チェックも兼ねる.)
        public static uint CalcLengthAndCheck(uint addr)
        {
            if (U.IsValueOdd(addr))
            {//奇数から始まるのはどう考えてもおかしい.
                return U.NOT_FOUND;
            }

            uint start = addr;
            uint limit = (uint)Program.ROM.Data.Length;

            while (addr + 8 <= limit)
            {
                uint code = Program.ROM.u16(addr + 0);
                uint sarg = Program.ROM.u16(addr + 2);
                uint parg = Program.ROM.u32(addr + 4);

                addr += 8; //命令は8バイト固定.
                if (addr + 8 > limit)
                {
                    break;
                }
                if (code == 0x00)
                {//arg all null
                    if (sarg == 0)
                    {
                        if (parg != 0)
                        {//規約違反
                            return U.NOT_FOUND;
                        }
                    }
                    else if (sarg <= 10)
                    {//10だったときは、何か値が入ることがあるようだ
                     //例: 0000 1000 08001800
                        if (parg == 0)
                        {//規約違反
                            return U.NOT_FOUND;
                        }
                    }
                }
                else if (code == 0x10 || code == 0x11 || code == 0x12 || code == 0x13|| code == 0x15|| code == 0x17 || code == 0x19)
                {//arg all null
                    if (sarg != 0 || parg != 0)
                    {//規約違反
                        return U.NOT_FOUND;
                    }
                }
                else if (code == 0x01 || code == 0x02 || code == 0x03 || code == 0x04 || code == 0x05 ||  code == 0x0D || code == 0x14 || code == 0x16)
                {//sarg is null, parg is pointer
                    if (sarg != 0 || !U.isSafetyPointer(parg))
                    {//規約違反
                        return U.NOT_FOUND;
                    }
                }
                else if (code == 0x06)
                {//parg is pointer, sargは1になることがあるらしい
                    if (!U.isPointer(parg))
                    {//規約違反
                        return U.NOT_FOUND;
                    }
                    if (U.IsValueOdd(parg))
                    {//6C呼び出しなので絶対に偶数でなければならない.
                        return U.NOT_FOUND;
                    }
                    //Debug.Assert(sarg == 1);
                    if (sarg >= 2)
                    {
                        return U.NOT_FOUND;
                    }
                }
                else if (code == 0x07 || code == 0x08 || code == 0x09 || code == 0x0A)
                {
                    if (!U.isPointer(parg))
                    {//規約違反
                        return U.NOT_FOUND;
                    }
                    if (U.IsValueOdd(parg))
                    {//6C呼び出しなので絶対に偶数でなければならない.
                        return U.NOT_FOUND;
                    }
                    if (sarg != 0)
                    {//必ずsarg引数は0
                        return U.NOT_FOUND;
                    }
                }
                else if (code == 0x0B ||code == 0x0E || code == 0x0F)
                {//parg is null 
                    if (parg != 0)
                    {//規約違反
                        return U.NOT_FOUND;
                    }
                }
                else if (code == 0x0C)
                {//parg is null 
                    if (sarg == 0)
                    {//GOTO LABEL 0があった
                        //Goto 0の時だけは、pargにゴミが入るときがある.

                        //先読みをしてみる.
                        uint sakiyomi = CalcLengthAndCheck(addr + 8);
                        if (sakiyomi == U.NOT_FOUND)
                        {//この先が壊れているなら、自分が終端.
                            if (start == addr)
                            {//自分が終端なのに、それが最初に出てくるのはおかしいよね.
                                return U.NOT_FOUND;
                            }
                            break;
                        }
                    }
                    else if (parg != 0)
                    {//規約違反
                        return U.NOT_FOUND;
                    }
                }
                else if (code == 0x18)
                {// parg is pointer
                    if (!U.isSafetyPointer(parg))
                    {//規約違反
                        return U.NOT_FOUND;
                    }
                }
                else if (code == 0x800)
                {//EXIT その3
                    break;
                }
                else
                {
                    return U.NOT_FOUND;
                }

                if (code == 0x00)
                {//EXIT
                    break;
                }
            }
            return addr - start;
        }

        string ConvertName(uint addr, out int out_ExistingTabIndex)
        {
            addr = U.toOffset(addr);

            string name;
            if (U.isSafetyOffset(addr))
            {
                string dummy;
                string comment = Program.AsmMapFileAsmCache.GetEventName(U.toPointer(addr), out dummy);
                name = addr.ToString("X");
                if (comment.Length > 0)
                {
                    name = comment;
                }
            }
            else
            {
                name = "";
            }

            if (addr == 0)
            {//addr == 0 のタブだけは特別に何個も開ける.
                out_ExistingTabIndex = -1;
            }
            else
            {
                out_ExistingTabIndex = this.MainTab.FindTab(addr);
            }
            return name;
        }

        void OnNavigation(object sender, EventArgs e)
        {
            if (!(e is EventScriptInnerControl.NavigationEventArgs))
            {
                return;
            }
            EventScriptInnerControl.NavigationEventArgs arg = (EventScriptInnerControl.NavigationEventArgs)e;
            if (arg.IsNewTab)
            {
                JumpToInner(arg.Address);
            }
            else
            {
                int dummy;
                string name = ConvertName(arg.Address, out dummy);
                MainTab.UpdateTab(this.MainTab.SelectedIndex, name, arg.Address);
            }
        }

        private void ProcsScriptForm_Load(object sender, EventArgs e)
        {
            this.ToolTip = InputFormRef.GetToolTip<ProcsScriptForm>();
            this.MaximizeBox = true;

            this.MainTab.NewTabEvent += (s, ee) =>
            {
                JumpTo(0x0);
            };
        }

        public void JumpTo(uint addr, uint event_current_addr = U.NOT_FOUND, EventHandler addressListExpandsEvent = null)
        {
            Show();
            if (addr == 0)
            {
                JumpToInner(0);
                return;
            }
            addr = U.toOffset(addr);

            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                pleaseWait.DoEvents(R._("検索中"));
                addr = U.toOffset(addr);

                int selected = -1;
                for (int i = 0; i < ProcsList.Count; i++)
                {
                    Address address = ProcsList[i];
                    if (address.Addr == addr)
                    {
                        selected = i;
                        break;
                    }
                }
                if (selected == -1)
                {//既存のタブでは、見つからないので新規.
                    JumpToInner(addr, event_current_addr, addressListExpandsEvent);
                    return;
                }


                int existingTabIndex;
                string name = ConvertName(addr, out existingTabIndex);
                if (existingTabIndex >= 0)
                {//既存タブがある
                    //アドレスを選択するだけで自動的に、解決する.
                    U.SelectedIndexSafety(this.AddressList, selected);
                    return;
                }

                //アドレスリストを選択すると、既存のタブで開いてしまうので、まずは空のページを開く.
                JumpToInner(0);
                //アドレスリストから選択することでページを読みこませる.
                U.SelectedIndexSafety(this.AddressList, selected);
                if (event_current_addr != U.NOT_FOUND)
                {
                    //行を選択する場合
                    JumpToInner(addr, event_current_addr, addressListExpandsEvent);
                }
            }
        }

        void JumpToInnerCurrentTab(uint addr)
        {
            int existingTabIndex;
            string name = ConvertName(addr, out existingTabIndex);
            if (existingTabIndex >= 0)
            {//既存タブがある
                this.MainTab.SelectedIndex = existingTabIndex;
                return;
            }
            if (this.MainTab.SelectedIndex < 0)
            {//一つもタブがない場合、新規に開くしかない.
                JumpToInner(addr);
                return;
            }
            Control c = this.MainTab.GetTabControl(this.MainTab.SelectedIndex);
            if (!(c is ProcsScriptInnerControl))
            {
                JumpToInner(addr);
                return;
            }

            ProcsScriptInnerControl f = (ProcsScriptInnerControl)c;
            f.JumpTo(addr);

            //タブの名前を更新.
            MainTab.UpdateTab(this.MainTab.SelectedIndex, name, addr);
        }

        void JumpToInner(uint addr , uint event_current_addr = U.NOT_FOUND, EventHandler addressListExpandsEvent = null)
        {
            int existingTabIndex;
            string name = ConvertName(addr, out existingTabIndex);
            if (existingTabIndex >= 0)
            {//既存タブがある
                this.MainTab.SelectedIndex = existingTabIndex;
                return;
            }

            ProcsScriptInnerControl f = new ProcsScriptInnerControl();
            InputFormRef.InitControl(f, this.ToolTip);
            f.Init(this.ToolTip, this.ProcsScriptForm_KeyDown);
            f.JumpTo(addr, event_current_addr);
            if (addressListExpandsEvent != null)
            {
                f.AddressListExpandsEvent += addressListExpandsEvent;
            }

            this.MainTab.Add(name, f, addr);
            f.Navigation += OnNavigation;
            f.AddressListExpandsEvent += OnAddressListExpandsEvent;
            //ProcsScriptForm_Resize(null, null);

            f.SetFocus();
        }
        private void OnAddressListExpandsEvent(object sender, EventArgs e)
        {
            if (!(e is InputFormRef.ExpandsEventArgs))
            {
                return;
            }

            InputFormRef.ExpandsEventArgs eventarg = (InputFormRef.ExpandsEventArgs)e;
            //アドレスが書き換わったときのみ通知される.
            Debug.Assert(eventarg.OldBaseAddress != eventarg.NewBaseAddress);

            for (int i = 0; i < ProcsList.Count; i++)
            {
                Address address = ProcsList[i];
                if (address.Addr != eventarg.OldBaseAddress)
                {
                    continue;
                }

                //FEBuilderGBAの自動書き換えが働いて、参照を書き換えているはず.
                Debug.Assert(Program.ROM.p32(U.toOffset(address.Pointer)) == eventarg.NewBaseAddress);

                ProcsList[i] = new Address(eventarg.NewBaseAddress, eventarg.NewDataCount, address.Pointer, address.Info , Address.DataTypeEnum.PROCS);
            }
        }

        private void ProcsScriptForm_KeyDown(object sender, KeyEventArgs e)
        {

        }

    }
}
