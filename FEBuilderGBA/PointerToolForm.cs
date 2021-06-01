using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace FEBuilderGBA
{
    public partial class PointerToolForm : Form
    {
        public PointerToolForm()
        {
            InitializeComponent();

            this.OtherLoadName.Text = "";
            WarningLevelComboBox.SelectedIndex = 1;
            TestMatchDataSizeComboBox.SelectedIndex = 2;
            DataType.SelectedIndex = 1;//ASM
            GrepType.SelectedIndex = 0;
            SlideComboBox.SelectedIndex = 0;
            AutomaticTrackingComboBox.SelectedIndex = 2;
        }

        private void PointerToolForm_Load(object sender, EventArgs e)
        {
        }

        public static bool[] makeSkipDataByPointer(byte[] need)
        {
            uint length = (uint)need.Length;

            bool[] isSkip = new bool[U.Padding4(length)];
            for (uint i = 0; i < length - 3; i += 4)
            {
                if (U.isPointer( U.u32(need, i) ))
                {
                    isSkip[i+0] = true;
                    isSkip[i+1] = true;
                    isSkip[i+2] = true;
                    isSkip[i+3] = true;
                }
            }
            return isSkip;
        }
        public static bool[] makeSkipDataByCode(byte[] need)
        {
            DisassemblerTrumb Disassembler = new DisassemblerTrumb();
            DisassemblerTrumb.VM vm = new DisassemblerTrumb.VM();
            uint length = (uint)need.Length;
            bool isFunctionEnd = false;

            uint ldrCount = 0;

            bool[] isSkip = new bool[U.Padding4(length)];
            for (uint i = 0; i < length - 3; )
            {
                if(isFunctionEnd)
                {//関数は抜けてしまったので、ldr用のポインタだけを見る.
                    if (ldrCount <= 0)
                    {//LDR値の地帯を抜けたので次はまた関数に復帰するはず.
                        isFunctionEnd = false;
                        continue;
                    }
                    ldrCount--;

                    if (U.isPointer(U.u32(need, i)))
                    {
                        isSkip[i + 0] = true;
                        isSkip[i + 1] = true;
                        isSkip[i + 2] = true;
                        isSkip[i + 3] = true;
                    }
                    i += 4;
                    continue;
                }

                DisassemblerTrumb.Code code =
                    Disassembler.Disassembler(need,i,length,vm);
                if (code.Type == DisassemblerTrumb.CodeType.CALL)
                {//関数呼び出しはアドレスの違いがかなり出てくるので.
                    isSkip[i+0] = true;
                    isSkip[i+1] = true;
                    isSkip[i+2] = true;
                    isSkip[i+3] = true;
                }
                if (code.Type == DisassemblerTrumb.CodeType.LDR)
                {
                    //LDRで超長距離飛んでしまう場合は例外.
                    uint a = U.u16(need, i);
                    uint imm = DisassemblerTrumb.LDR_IMM(a);
                    if (imm >= 0x64)
                    {//あまりに長い距離飛ぶ場合、シリーズ間でずれることがあるらしい 0x64は適当な値です.
                        isSkip[i + 0] = true;
                        isSkip[i + 1] = true;
                        isSkip[i + 2] = true; //長距離LDRをした次の命令も変わることがあるらしいので、ねんのため
                        isSkip[i + 3] = true;
                    }
                    else
                    {
                        isSkip[i + 0] = true;

                        //次の関数の開始位置を推測. BXJMPで抜けた後 LDRデータが終わったら、次の関数があるはず.
                        ldrCount++;
                    }
                }

                i += code.GetLength();

                if (code.Type == DisassemblerTrumb.CodeType.BXJMP)
                {//関数終端 以降あるのは ldr用のアドレス地帯だろう
                    isFunctionEnd = true;

                    //関数を抜けたときに 4バイトパディングを維持しないといけない.
                    i = U.Padding4(i);
                }


                
            }
            return isSkip;
        }

        private uint DGrep(byte[] data, byte[] need)
        {
            if (GrepType.SelectedIndex == 0)
            {//完全一致
                return U.Grep(data, need , 0 , 0 , 2);
            }
            else
            {//パターンマッチ.
                bool[] isSkip;
                if (DataType.SelectedIndex == 1)
                {
                    isSkip = makeSkipDataByCode(need);
                }
                else
                {
                    isSkip = makeSkipDataByPointer(need);
                }
                return U.GrepPatternMatch(data, need, isSkip, 0 , 0 , 2);
            }
        }
        private void ShortCut_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                this.Address.Focus();
                this.Address.SelectAll();
                return;
            }
            if (e.KeyCode == Keys.Enter)
            {
                OtherROMAddress_MouseDoubleClick(sender, null);
                return;
            }
        }


        void AutoSearch()
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }
            if (this.OtherROMData == null
                || this.OtherROMData.Length <= 0)
            {//別のROMを読込んでいないので探索不可能
                SearchCurrentROM();
                return;
            }
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                bool isNearMatch = false;
                int lastNearMatchSlideComboBox = -1;
                int lastNearMatchTestMatchDataSizeComboBox = -1;
                int lastNearMatchGrepType = -1;
                HideError();

                uint addr = U.atoh(Address.Text);
                if (addr == 0)
                {
                    return;
                }

                bool isCode = (addr % 4) == 1;
                if (isCode)
                {
                    addr = addr - 1;
                    DataType.SelectedIndex = 1;//ASM
                }

                addr = U.ChangeEndian32(addr);
                SearchCurrentROM();

                if (UseASMMAPCheckBox.Checked)
                {
                    if (SearchASMMap())
                    {
                        return;
                    }
                }

                uint autoMatictrackLevel = U.atoh(AutomaticTrackingComboBox.Text);
                if (autoMatictrackLevel == 0)
                {//自動追跡しない
                    Search();

                    if (IsDataFound(out isNearMatch))
                    {//見つかった
                        return;
                    }
                    return;
                }

                //自動追跡のため初期化
                SlideComboBox.SelectedIndex = 0;//スキップしない.
                TestMatchDataSizeComboBox.SelectedIndex = 0; //ディフォルトは32バイト
                Search();

                if (this.OtherROMData == null)
                {
                    return;
                }

                //自動追跡初期化
                pleaseWait.DoEvents(R._("MakeOtherROMLDRFuncList"));
                {
                    uint address = U.atoh(Address.Text);
                    uint pointer = U.toPointer(address);
                    this.OtherROMLDRFuncList = MakeOtherROMLDRFuncList(pointer);
                }

                int maxDeepSearch = (int)((autoMatictrackLevel >> 8) & 0xF) + 2;
                int maxSkipSearch = (int)(autoMatictrackLevel & 0xF) + 1;
                for (int deepSearch = 1; deepSearch < maxDeepSearch; deepSearch++)
                {
                    pleaseWait.DoEvents(R._("自動追跡 {0}/{1}", deepSearch * maxSkipSearch, maxDeepSearch * maxSkipSearch));

                    TestMatchDataSizeComboBox.SelectedIndex = deepSearch;//マッチサイズを少しだけ下げる.
                    GrepType.SelectedIndex = 0;//パターンマッチしない
                    if (IsDataFound(out isNearMatch))
                    {//見つかった
                        return;
                    }
                    if (isNearMatch && lastNearMatchSlideComboBox == -1)
                    {//惜しいマッチをした場合記録する.
                        lastNearMatchSlideComboBox = SlideComboBox.SelectedIndex;
                        lastNearMatchTestMatchDataSizeComboBox = TestMatchDataSizeComboBox.SelectedIndex;
                        lastNearMatchGrepType = GrepType.SelectedIndex;
                    }

                    GrepType.SelectedIndex = 1;//パターンマッチ
                    if (IsDataFound(out isNearMatch))
                    {//見つかった
                        return;
                    }
                    if (isNearMatch && lastNearMatchSlideComboBox == -1)
                    {//惜しいマッチをした場合記録する.
                        lastNearMatchSlideComboBox = SlideComboBox.SelectedIndex;
                        lastNearMatchTestMatchDataSizeComboBox = TestMatchDataSizeComboBox.SelectedIndex;
                        lastNearMatchGrepType = GrepType.SelectedIndex;
                    }

                    for (int skipSearch = 1; skipSearch < maxSkipSearch; skipSearch++)
                    {
                        pleaseWait.DoEvents(R._("自動追跡 {0}/{1}", deepSearch * maxSkipSearch + skipSearch, maxDeepSearch * maxSkipSearch));

                        //Nバイトスキップ
                        SlideComboBox.SelectedIndex = skipSearch;
                        if (IsDataFound(out isNearMatch))
                        {//見つかった
                            return;
                        }
                        if (isNearMatch && lastNearMatchSlideComboBox == -1)
                        {//惜しいマッチをした場合記録する.
                            lastNearMatchSlideComboBox = SlideComboBox.SelectedIndex;
                            lastNearMatchTestMatchDataSizeComboBox = TestMatchDataSizeComboBox.SelectedIndex;
                            lastNearMatchGrepType = GrepType.SelectedIndex;
                        }
                    }
                }

                //どこにもマッチしなかった場合、
                //惜しいマッチをしていた場合、それを復元する.
                if (lastNearMatchSlideComboBox != -1)
                {//惜しいマッチをした場合記録する.
                    SlideComboBox.SelectedIndex = lastNearMatchSlideComboBox;
                    TestMatchDataSizeComboBox.SelectedIndex = lastNearMatchTestMatchDataSizeComboBox;
                    GrepType.SelectedIndex = lastNearMatchGrepType;
                }

            }
        }
        //名前で探索する
        bool SearchASMMap()
        {
            if (this.OtherROMASMMap == null)
            {
                return false;
            }

            string name = Program.AsmMapFileAsmCache.GetName(this.CurrentPointer);
            uint foundAddr = this.OtherROMASMMap.SearchName(name);
            if (foundAddr == U.NOT_FOUND)
            {
                return false;
            }
            uint littleendian = ((foundAddr >> 24) & 0xFF) + (((foundAddr >> 16) & 0xFF) << 8) + (((foundAddr >> 8) & 0xFF) << 16) + (((foundAddr) & 0xFF) << 24);

            byte[] b = new byte[4];
            b[0] = (byte)((littleendian >> 24) & 0xFF);
            b[1] = (byte)((littleendian >> 16) & 0xFF);
            b[2] = (byte)((littleendian >> 8) & 0xFF);
            b[3] = (byte)((littleendian) & 0xFF);
            uint refaddress = U.Grep(this.OtherROMData, b);

            SetAddressText(this.OtherROMAddressWithLDR, U.NOT_FOUND);
            SetAddressText(this.OtherROMAddressWithLDRRef, U.NOT_FOUND);
            SetAddressText(this.OtherROMAddress2, foundAddr);
            SetAddressText(this.OtherROMRefPointer2, refaddress);

            return true;
        }
        
        private void Address_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter
                || e.KeyCode == Keys.Tab)
            {
                AutoSearch();
                return;
            }
        }

        bool IsFoundAddress(uint p)
        {
            if (p == U.NOT_FOUND || p == 0)
            {
                return false;
            }
            return true;
        }

        bool IsDataFound(out bool out_isNearMatch)
        {
            int warningLevel = WarningLevelComboBox.SelectedIndex;
            uint address = U.atoh(Address.Text);
            uint foundaddress;

            out_isNearMatch = false;

            foundaddress = GetAddressText(OtherROMAddress2);
            if (IsFoundAddress(foundaddress))
            {
                out_isNearMatch = true;
                this.ERROR_VERYFAR1.Visible = checkVeryFar(address, foundaddress);
                this.ERROR_ZERO1.Visible = checkZeroData(this.OtherROMData, foundaddress, foundaddress + 0x200);

                if (warningLevel == 0)
                {//警告をエラーにする
                    if (this.ERROR_VERYFAR1.Visible == false && this.ERROR_ZERO1.Visible == false)
                    {//警告が出ていなければ、マッチ
                        return true;
                    }
                }
                else if (warningLevel == 1)
                {//参照あれば警告を無視する
                    //参照があるか警告がでていないかだったら、受け入れよう.
                    if (IsFoundAddress(GetAddressText(OtherROMRefPointer2)))
                    {//参照がある
                        return true;
                    }
                    else if (this.ERROR_VERYFAR1.Visible == false && this.ERROR_ZERO1.Visible == false)
                    {//参照はないがエラーが出ていない.
                        return true;
                    }
                }
                else
                {//エラーを無視する.
                    return true;
                }
            }

            foundaddress = GetAddressText(OtherROMAddressWithLDR);
            if (IsFoundAddress(foundaddress))
            {
                out_isNearMatch = true;
                this.ERROR_VERYFAR3.Visible = checkVeryFar(address, foundaddress);
                this.ERROR_ZERO3.Visible = checkZeroData(this.OtherROMData, foundaddress, foundaddress + 0x200);

                if (warningLevel == 0)
                {//警告をエラーにする
                    if (this.ERROR_VERYFAR3.Visible == false && this.ERROR_ZERO3.Visible == false)
                    {//警告が出ていなければ、マッチ
                        return true;
                    }
                }
                else if (warningLevel == 1)
                {//参照あれば警告を無視する
                    //参照があるか警告がでていないかだったら、受け入れよう.
                    if (IsFoundAddress(GetAddressText(OtherROMAddressWithLDRRef)))
                    {//参照がある
                        return true;
                    }
                    else if (this.ERROR_VERYFAR3.Visible == false && this.ERROR_ZERO3.Visible == false)
                    {//参照はないがエラーが出ていない.
                        return true;
                    }
                }
                else
                {//エラーを無視する.
                    return true;
                }
            }
            return false;
        }


        void HideError()
        {
            ERROR_ZERO1.Visible = false;
            ERROR_VERYFAR1.Visible = false;
            ERROR_ZERO3.Visible = false;
            ERROR_VERYFAR3.Visible = false;
        }


        private void Address_ValueChanged(object sender, EventArgs e)
        {
            HideError();
            SearchCurrentROM();
            Search();

            if (this.OtherROMData == null)
            {
                return;
            }
            bool isNearMatch;
            IsDataFound(out isNearMatch);
        }

        void SetAddressText(TextBox text, uint value)
        {
            text.Text = value.ToString("X08");
            text.Tag = value;
        }
        uint GetAddressText(TextBox text)
        {
            if (text.Tag is uint)
            {
                return (uint)text.Tag;
            }
            return 0;
        }

        uint CurrentPointer;
        uint CurrentREFAddress;
        private void SearchCurrentROM()
        {
            //マッチテストサイズ
            int testMatchDataSize = minusAtoH(TestMatchDataSizeComboBox.Text);

            uint address = U.atoh(Address.Text);
            uint pointer = U.toPointer(address);
            uint littleendian = ((pointer >> 24) & 0xFF) + (((pointer >> 16) & 0xFF) << 8) + (((pointer >> 8) & 0xFF) << 16) + (((pointer) & 0xFF) << 24);

            byte[] b = new byte[4];
            b[0] = (byte)((littleendian >> 24) & 0xFF);
            b[1] = (byte)((littleendian >> 16) & 0xFF);
            b[2] = (byte)((littleendian >> 8) & 0xFF);
            b[3] = (byte)((littleendian) & 0xFF);
            uint refaddress = U.Grep(Program.ROM.Data, b);

            SetAddressText(this.Pointer, pointer);
            SetAddressText(this.LittleEndian, littleendian);
            SetAddressText(this.RefPointer, refaddress);
            SetAddressText(this.DataAddress, U.NOT_FOUND);

            if (U.isSafetyPointer(pointer))
            {
                uint addr = U.toOffset(address);
                uint thisPointer = Program.ROM.u32(addr);
                if (U.isPointer(thisPointer))
                {
                    SetAddressText(this.DataAddress, thisPointer);
                }
            }
            this.CurrentPointer = pointer;
            this.CurrentREFAddress = refaddress;
        }

        private void Search()
        {
            uint slide = U.atoh(this.SlideComboBox.Text);

            //LDR参照値を利用した比較
            {
                uint otherROMAddressWithLDR, otherROMAddressWithLDRRef;
                FindOtherROMDataWithLDR(this.CurrentPointer, this.CurrentREFAddress, (int)slide
                    , out otherROMAddressWithLDR, out otherROMAddressWithLDRRef);

                SetAddressText(this.OtherROMAddressWithLDR, otherROMAddressWithLDR);
                SetAddressText(this.OtherROMAddressWithLDRRef, otherROMAddressWithLDRRef);
            }

            //現在地のデータが別のROMにもあるか？
            {
                uint otherROMAddress2, otherROMRefPointer2;
                FindOtherROMData(this.CurrentPointer, (int)slide
                    , out otherROMAddress2, out otherROMRefPointer2);

                SetAddressText(this.OtherROMAddress2, otherROMAddress2);
                SetAddressText(this.OtherROMRefPointer2, otherROMRefPointer2);
            }

        }

        int minusAtoH(string str)
        {
            if (str.Length <= 0) return 0;
            if (str[0] == '-')
            {
                return -1 * (int)U.atoh(str.Substring(1));
            }
            return (int)U.atoh(str);
        }

        //データ数の半分を0が占めたら警告する
        bool checkZeroData(byte[] data,uint start,uint end)
        {
            if (data.Length < start)
            {
                return false;
            }
            if (data.Length < end)
            {
                end = (uint)data.Length;
            }

            int zeroCount = 0;
            for (uint i = start; i < end; i++)
            {
                if (data[i] == 0x0)
                {
                    zeroCount++;
                }
            }

            if (zeroCount > (end-start) / 2)
            {
                return true;
            }
            return false;
        }

        //元データとあまりに離れている場合警告する.
        bool checkVeryFar(uint search_addr ,uint addr)
        {
            int diff1 = Math.Abs(((int)U.toOffset(addr)) - ((int)U.toOffset(search_addr)));

            int yoningosa = GetYoninGosa(Math.Min(U.toPointer(addr), U.toPointer(search_addr)));

            if (diff1 >= yoningosa)
            {
                return true;
            }
            return false;
        }

        //許容ずれ 経験上 後半になるほどずれることを元にしています.
        int GetYoninGosa(uint p)
        {
            if (p < 0x02040000) return 0x100;

            if (p < 0x08001000) return 0x400;
            if (p < 0x08008000) return 0x800;
            if (p < 0x08010000) return 0x2000;
            if (p < 0x08040000) return 0x5000;
            if (p < 0x08080000) return 0x8000;
            if (p < 0x08100000) return 0x20000;
            if (p < 0x08200000) return 0x40000;
            return 0x100000;
        }

        class OtherROMLDRFuncSt
        {
            public uint FuncAddr;
            public uint BackSize;
        };

        const int SEARCH_PUSH_MAX = 0x10 * 2;
        List<OtherROMLDRFuncSt> OtherROMLDRFuncList;
        List<OtherROMLDRFuncSt> MakeOtherROMLDRFuncList(uint search_address)
        {
            List<OtherROMLDRFuncSt> list = new List<OtherROMLDRFuncSt>();
            for (int i = 0; i < this.LDRMAPs.Count; i++)
            {
                DisassemblerTrumb.LDRPointer ldr = this.LDRMAPs[i];
                if ( ldr.ldr_data != search_address  )
                {
                    continue;
                }
                if (ldr.ldr_address <= SEARCH_PUSH_MAX)
                {
                    continue;
                }

                DisassemblerTrumb Disassembler = new DisassemblerTrumb();
                DisassemblerTrumb.VM vm = new DisassemblerTrumb.VM();
                uint limit = ldr.ldr_address - SEARCH_PUSH_MAX;
                for(uint n = ldr.ldr_address - 2; n >= limit ; n -= 2)
                {
                    DisassemblerTrumb.Code code =
                        Disassembler.Disassembler(Program.ROM.Data, n , (uint)Program.ROM.Data.Length , vm);
                    if ( code.Type == DisassemblerTrumb.CodeType.PUSH )
                    {
                        OtherROMLDRFuncSt pp = new OtherROMLDRFuncSt();
                        pp.FuncAddr = U.toPointer(n);
                        pp.BackSize = ldr.ldr_address - n;
                        list.Add(pp);
                    }
                    else if (code.Type == DisassemblerTrumb.CodeType.BXJMP)
                    {
                        break;
                    }
                }
            }
            return list;
        }


        bool FindOtherROMDataWithLDR(uint search_address, uint refaddress, int slide
            , out uint out_addr, out uint out_ref)
        {
            out_addr = U.NOT_FOUND;
            out_ref = U.NOT_FOUND;
            if (OtherROMLDRFuncList == null)
            {
                return false;
            }

            for(int i = 0 ; i < OtherROMLDRFuncList.Count ; i++)
            {
                uint otherrom_addr;
                uint otherrom_ref;

                uint addr = OtherROMLDRFuncList[i].FuncAddr;
                bool r = FindOtherROMData(addr, slide, out otherrom_addr, out otherrom_ref);
                if (!r)
                {
                    continue;
                }

                otherrom_addr = U.toOffset(otherrom_addr + OtherROMLDRFuncList[i].BackSize);
                for (int n = 0; n < this.OtherLDRMAPs.Count; n++)
                {
                    DisassemblerTrumb.LDRPointer otherldr = this.OtherLDRMAPs[n];
                    if (otherldr.ldr_address != otherrom_addr)
                    {
                        continue;
                    }

                    out_ref = otherldr.ldr_data_address;
                    out_addr = otherldr.ldr_data;
                    return true;
                }
            }
            return false;
        }
        bool FindOtherROMData(uint thisPointer, int appendoffset
            , out uint out_addr, out uint out_ref)
        {
            out_addr = U.NOT_FOUND;
            out_ref = U.NOT_FOUND;

            //マッチテストサイズ
            int testMatchDataSize = minusAtoH(TestMatchDataSizeComboBox.Text);

            //現在地のポインタの中身の先にあるものが同様に別ROMにもあるか？
            if (!U.isPointer(thisPointer))
            {
                return false;
            }
            thisPointer = U.toOffset(thisPointer);

            //少し先を探す場合はここで加算.
            if (thisPointer + appendoffset < 0)
            {
                return false;
            }
            thisPointer = (uint)((int)thisPointer + appendoffset);

            byte[] data2 = Program.ROM.getBinaryData(thisPointer, testMatchDataSize);
            uint otherROMAddres2 = DGrep(this.OtherROMData, data2);
            if (otherROMAddres2 == U.NOT_FOUND)
            {
                return false;
            }

            uint otherROMRefPointer2 = U.NOT_FOUND;
            if (otherROMAddres2 != U.NOT_FOUND)
            {
                //加算していた場合は戻す.
                otherROMAddres2 = (uint)((int)otherROMAddres2 - appendoffset);

                otherROMAddres2 = U.toPointer(otherROMAddres2);

                uint otherROMlittleendian2 = ((otherROMAddres2 >> 24) & 0xFF) + (((otherROMAddres2 >> 16) & 0xFF) << 8) + (((otherROMAddres2 >> 8) & 0xFF) << 16) + (((otherROMAddres2) & 0xFF) << 24);
                byte[] otherROMB2 = new byte[4];
                otherROMB2[0] = (byte)((otherROMlittleendian2 >> 24) & 0xFF);
                otherROMB2[1] = (byte)((otherROMlittleendian2 >> 16) & 0xFF);
                otherROMB2[2] = (byte)((otherROMlittleendian2 >> 8) & 0xFF);
                otherROMB2[3] = (byte)((otherROMlittleendian2) & 0xFF);
                otherROMRefPointer2 = DGrep(this.OtherROMData, otherROMB2);
            }

            out_addr = otherROMAddres2;
            out_ref = otherROMRefPointer2;
            return true;
        }


        List<DisassemblerTrumb.LDRPointer> LDRMAPs;
        List<DisassemblerTrumb.LDRPointer> OtherLDRMAPs;
        
        string OtherROMFilename;
        byte[] OtherROMData;
        AsmMapFile OtherROMASMMap;
        
        private void LoadOtherROMButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }

            string title = R._("編集するROMを選択してください");
            string filter = R._("GBA ROMs|*.gba|Binary files|*.bin|All files|*");

            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            Program.LastSelectedFilename.Load(this, "", open);
            open.ShowDialog();
            if (!U.CanReadFileRetry(open))
            {
                return;
            }
            Program.LastSelectedFilename.Save(this, "", open);


            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                LoadTargetROM(open.FileNames[0]);
            }

            AutoSearch();
        }
        void LoadTargetROM(string filename)
        {
            ROM OtherROM = new ROM();
            string version;
            if (OtherROM.Load(filename, out version))
            {
                this.OtherROMASMMap = new AsmMapFile(OtherROM);
            }

            this.OtherROMFilename = filename;
            this.OtherROMData = File.ReadAllBytes(filename);

            this.OtherLoadName.Text = R._("別ゲームROM:{0}", Path.GetFileNameWithoutExtension(this.OtherROMFilename));
            //自分のLDRMAPをここで作る. 相手のROMに探索に利用する
            this.LDRMAPs = Program.AsmMapFileAsmCache.GetLDRMapCache();
            //相手のROMのLDRMAPを作る.
            this.OtherLDRMAPs = DisassemblerTrumb.MakeLDRMap(this.OtherROMData, 0x100, 0);
        }

        private void OtherROMAddress_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Control v = (Control)(sender);

            PointerToolCopyToForm f = (PointerToolCopyToForm)InputFormRef.JumpFormLow<PointerToolCopyToForm>();
            f.Init((uint) U.atoh(v.Text));
            f.ShowDialog();
        }

        private void WhatIsButton_Click(object sender, EventArgs e)
        {
            uint addr = U.atoh(Address.Text);
            uint pointer = U.toPointer(addr);
            addr = U.toOffset(addr);

            string hint = "";
            AsmMapFile asmMap = Program.AsmMapFileAsmCache.GetAsmMapFile();
            AsmMapFile.AsmMapSt p;
            if (asmMap.TryGetValue(pointer, out p))
            {
                hint = p.ToStringInfo();
            }
            else
            {
                uint near_pointer = asmMap.SearchNear(pointer);
                if (asmMap.TryGetValue(near_pointer, out p))
                {
                    if (pointer < near_pointer + p.Length)
                    {
                        uint offset = (pointer - near_pointer);
                        hint = U.To0xHexString(U.toOffset(near_pointer)) + "+" + offset + "(" + U.To0xHexString(offset) + ") " + p.ToStringInfo();
                    }
                }

            }
            if (hint.Length <= 0)
            {
                R.ShowOK("not found");
                return;
            }
            R.ShowOK("アドレス{0}は {1} 領域です。", U.To0xHexString(addr), hint);
        }
        public static int ComandLineSearch()
        {
            string target = U.at(Program.ArgsDic, "--target");
            if (!File.Exists(target))
            {
                U.echo(R.Error("--targetで、相手のROMを指定してください。"));
                return -2;
            }

            PointerToolForm f = (PointerToolForm)InputFormRef.JumpFormLow<PointerToolForm>();
            f.LoadTargetROM(target);

            string tracelevel = U.at(Program.ArgsDic,"--tracelevel");
            if (tracelevel != "")
            {
                U.SelectedIndexSafety(f.AutomaticTrackingComboBox, U.atoi0x(tracelevel));
            }

            string address = U.at(Program.ArgsDic, "--address");
            if (File.Exists(address))
            {
                address = File.ReadAllText(address);
            }
            string[] lines = address.Split(new string[] { "\r\n" , "," , "\t"}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string a in lines)
            {
                if (! U.isHexString(a))
                {
                    continue;
                }

                f.Address.Text = a;
                f.AutoSearch();

                bool isNearMatch;
                if (!f.IsDataFound(out isNearMatch))
                {//見つからなかった
                    U.echo(
                        f.Address.Text + "\t"
                        + U.ToHexString(U.NOT_FOUND) + "\t"
                        + U.ToHexString(U.NOT_FOUND) + "\t"
                        + U.ToHexString(U.NOT_FOUND) + "\t"
                        + U.ToHexString(U.NOT_FOUND));
                }
                else
                {
                    U.echo(
                        f.Address.Text + "\t"
                        + f.OtherROMAddressWithLDRRef.Text + "\t"
                        + f.OtherROMAddressWithLDR.Text + "\t"
                        + f.OtherROMRefPointer2.Text + "\t"
                        + f.OtherROMAddress2.Text);
                }
            }

            f.Close();
            return 0;
        }

        private void BatchButton_Click(object sender, EventArgs e)
        {
            if (this.OtherROMData == null
                || this.OtherROMData.Length <= 0)
            {
                R.ShowStopError("先に、ターゲットのROMを読込んでください。");
                return;
            }

            PointerToolBatchInputForm f = (PointerToolBatchInputForm)InputFormRef.JumpFormLow<PointerToolBatchInputForm>();
            f.SetTextData("");
            DialogResult dr =  f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            string text = f.GetTextData();
            string[] lines = text.Split(new string[] { "\r\n" },StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (U.IsComment(line))
                {
                    continue;
                }

                if (TryBatchImageHex(ref line))
                {
                    lines[i] = line;
                    continue;
                }

                if (TryBatchHex(ref line))
                {
                    lines[i] = line;
                    continue;
                }

                if (TryDefHex(ref line))
                {
                    lines[i] = line;
                    continue;
                }
            }
            f.SetTextData(string.Join("\r\n", lines));
            f.ShowDialog();
        }

        bool TryBatchImageHex(ref string line)
        {
            System.Text.RegularExpressions.Match match =
                RegexCache.Match(line, @"[A-Za-z0-9_]+=0x[A-Za-z0-9]+//0x([A-Za-z0-9]+)");
            if (match.Groups.Count < 1)
            {
                return false;
            }

            bool found = false;
            for (int n = 1; n < match.Groups.Count; n++)
            {
                string v = match.Groups[n].Value;
                uint addr = U.atoh(v);
                if (addr < 0x100)
                {
                    continue;
                }
                this.Address.Text = U.ToHexString(addr);
                AutoSearch();

                bool isNearMatch;
                if (!IsDataFound(out isNearMatch))
                {//見つからなかった
                    continue;
                }
                found = true;
                string newV = v + "(0x" + OtherROMAddressWithLDRRef.Text + "//0x" + OtherROMAddressWithLDR.Text + ",0x" + OtherROMRefPointer2.Text + "//0x" + OtherROMAddress2.Text + ")";
                line = line.Replace(v, newV);
            }
            return found;
        }
        bool TryBatchHex(ref string line)
        {
            System.Text.RegularExpressions.Match match =
                RegexCache.Match(line, @"[A-Za-z0-9_]+=0x([A-Za-z0-9]+)$");
            if (match.Groups.Count < 1)
            {
                return false;
            }

            bool found = false;
            for (int n = 1; n < match.Groups.Count; n++)
            {
                string v = match.Groups[n].Value;
                uint addr = U.atoh(v);
                if (addr < 0x100)
                {
                    continue;
                }
                this.Address.Text = U.ToHexString(addr);
                AutoSearch();
                bool isNearMatch;
                if (!IsDataFound(out isNearMatch))
                {//見つからなかった
                    continue;
                }
                found = true;
                string newV = v + "(" + OtherROMAddressWithLDRRef.Text + "->" + OtherROMAddressWithLDR.Text + "," + OtherROMRefPointer2.Text + "->" + OtherROMAddress2.Text + ")";
                line = line.Replace(v, newV);
            }
            return found;
        }

        bool TryDefHex(ref string line)
        {
            System.Text.RegularExpressions.Match match =
                RegexCache.Match(line, @"([0-9a-fA-Z][0-9a-fA-Z][0-9a-fA-Z]+)");
            if (match.Groups.Count < 1)
            {
                return false;
            }
            bool found = false;
            for (int n = 1; n < match.Groups.Count; n++)
            {
                string v = match.Groups[n].Value;
                uint addr = U.atoh(v);
                if (addr < 0x100)
                {
                    continue;
                }
                this.Address.Text = U.ToHexString(addr);
                AutoSearch();
                bool isNearMatch;
                if (!IsDataFound(out isNearMatch))
                {//見つからなかった
                    continue;
                }
                found = true;
                string newV = v + "(" + OtherROMAddressWithLDRRef.Text + "->" + OtherROMAddressWithLDR.Text + "," + OtherROMRefPointer2.Text + "->" + OtherROMAddress2.Text + ")";
                line = line.Replace(v, newV);
            }
            return found;
        }

        string ReplaceFunction(string prefix, uint addr, string orignalValue, Dictionary<uint, uint> addressCache)
        {
            if (prefix == "0x")
            {
                if (addr < 0x10000)
                {//小さすぎるアドレスには何もしない
                    return orignalValue;
                }
            }
            else
            {
                if (addr < 0xA00)
                {//小さすぎるアドレスには何もしない
                    return orignalValue;
                }
            }
            if (addr >= 0x0F000000)
            {//大きすぎる
                return orignalValue;
            }
            
            uint cacheHitAddr;
            if ( addressCache.TryGetValue(addr,out cacheHitAddr))
            {
                return prefix + U.ToHexString(cacheHitAddr);
            }

            this.Address.Text = U.ToHexString(addr);
            AutoSearch();
            bool isNearMatch;
            if (!IsDataFound(out isNearMatch))
            {//見つからなかった
                return "??=>" + orignalValue;
            }

            uint foundAddr = U.atoh(OtherROMAddress2.Text);
            if (foundAddr == U.NOT_FOUND)
            {
                foundAddr = U.atoh(OtherROMAddressWithLDR.Text);
                if (foundAddr == U.NOT_FOUND)
                {//ない
                    return "??=>" + orignalValue;
                }
            }

            //奇数データが必要ならば奇数化する.
            if (U.IsValueOdd(addr))
            {
                if (!U.IsValueOdd(foundAddr))
                {
                    foundAddr++;
                }
            }
            //ポインタ化が必要ならばポインタ化する
            if (U.isPointer(addr))
            {
                foundAddr = U.toPointer(foundAddr);
            }
            else if (U.isOffset(addr))
            {
                foundAddr = U.toOffset(foundAddr);
            }

            addressCache[addr] = foundAddr;
            return prefix + U.ToHexString(foundAddr);
        }

    }
}
