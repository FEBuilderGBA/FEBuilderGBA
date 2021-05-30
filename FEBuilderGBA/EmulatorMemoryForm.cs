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
    public partial class EmulatorMemoryForm : Form
    {
        public EmulatorMemoryForm()
        {
            InitializeComponent();
        }
        private void EmulatorMemoryForm_Load(object sender, EventArgs e)
        {
            //初期化に時間がかかるので、 pleaseWait を出したいので、 Shownへ移動する.
        }
        //表示時にフォーカスを奪わない
        protected override bool ShowWithoutActivation
        {
            get
            {
                return true;
            }
        }
        private void EmulatorMemoryForm_Shown(object sender, EventArgs e)
        {
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                pleaseWait.DoEvents(R._("準備しています"));
                Init();
            }
        }
        void Init()
        {
            List<Control> controls = InputFormRef.GetAllControls(this);
            string[] args = new string[0];

            SetExplain();
            FE6ize();
            SetSpeechIcon();
            SetSubtileIcon();

            InitIgnoreEvent();

            InputFormRef.makeLinkEventHandler("", controls, this.BGM, this.BGMName, 0, "SONG", args);
            InputFormRef.makeJumpEventHandler(this.BGM, this.J_BGM, "SONG", args);

            InputFormRef.markupJumpLabel(RunningEventListBoxLabel);

            this.N_InputFormRef = new InputFormRef(this, "N_", 0, 0);
            this.PROCS_InputFormRef = new InputFormRef(this, "PROCS_", 0, 0);
            this.PARTY_InputFormRef = new InputFormRef(this, "PARTY_", 0, 0);

            //LocalizeRAMUnit();

            //頻繁にリストを更新するのでBrushをキャッシュする.
            this.ListBoxForeBrush = new SolidBrush(OptionForm.Color_Control_ForeColor());
            this.ListBoxForeKeywordBrush = new SolidBrush(OptionForm.Color_Keyword_ForeColor());
            this.ListBoxForeDecBrush = new SolidBrush(OptionForm.Color_InputDecimal_ForeColor());
            this.BoldFont = new Font(this.FlagListBox.Font, FontStyle.Bold);
            this.YubiYokoCursor = ImageSystemIconForm.YubiYoko();
            U.MakeTransparent(this.YubiYokoCursor);

            //リスト項目の初期化.
            InitUserStack();
            InitFlag();
            InitProcs();
            InitMemroySlot();
            InitEventHistoryList();
            InitRunningEventList();
            InitCheat(controls);
            InitParty();
            InitPaletteEtc();
            
            //メモリの内容を取得.
            UpdateALL();
            //updateタイマーのセット
            timer1.Start();
        }
        void LocalizeRAMUnit()
        {
            MagicSplitUtil.magic_split_enum magic_split = MagicSplitUtil.SearchMagicSplit();
            if (magic_split == MagicSplitUtil.magic_split_enum.FE8NMAGIC)
            {
                PARTY_J_26.Text = R._("魔力");
            }
            else if (magic_split == MagicSplitUtil.magic_split_enum.FE7UMAGIC
                || magic_split == MagicSplitUtil.magic_split_enum.FE8UMAGIC)
            {
                PARTY_J_58.Text = R._("魔力");
            }

            PatchUtil.skill_system_enum skill = PatchUtil.SearchSkillSystem();
            if (skill == PatchUtil.skill_system_enum.FE8N_ver2)
            {//FE8J FE8NSkillは、0x3Aに追加スキルを記録してる
                PARTY_J_58.Text = R._("追加スキル");
            }
        }

        //頻繁に更新するのでキャッシュしておこう.
        SolidBrush ListBoxForeBrush;
        SolidBrush ListBoxForeKeywordBrush;
        SolidBrush ListBoxForeDecBrush;
        Font BoldFont;
        Bitmap YubiYokoCursor;

        InputFormRef N_InputFormRef;
        InputFormRef PROCS_InputFormRef;
        InputFormRef PARTY_InputFormRef;

        const uint RAMUnitSizeOf = 72; //構造体のサイズ

        void InitParty()
        {
            U.SelectedIndexSafety(this.PartyCombo, 0);
            PartyCombo.OwnerDraw(ComboBoxEx.DrawIconAndText, DrawMode.OwnerDrawFixed);
            PartyCombo.AddIcon(0x0, ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(1, 0, true)); //00=プレイヤ
            PartyCombo.AddIcon(0x40, ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(16, 1, true)); //40=友軍
            PartyCombo.AddIcon(0x80, ImageUnitWaitIconFrom.DrawWaitUnitIconBitmap(7, 2, true)); //80=敵軍

            this.PartyListBox.OwnerDraw(DrawParty, DrawMode.OwnerDrawFixed, false);
        }
        void InitFlag()
        {
            this.UpdateCheckDataFlag = new byte[0x24];
            List<U.AddrResult> list = Program.FlagCache.MakeList();
            int limit = list.Count;
            if (limit > 0 && Program.ROM.RomInfo.version() == 8)
            {//0xFFFFを飛ばす
                limit--;
            }

            this.FlagListBox.BeginUpdate();
            this.FlagListBox.Items.Clear();
            for (int i = 1; i < limit; i++)
            {
                this.FlagListBox.Items.Add(U.ToHexString(list[i].addr) + " " + list[i].name);
            }
            this.FlagListBox.EndUpdate();
            this.FlagListBox.Tag = list;
            this.FlagListBox.OwnerDraw(DrawFlag, DrawMode.OwnerDrawFixed, false);
        }
        void InitProcs()
        {
            this.ProcsTree = new List<ProcsData>();
            this.ProcsListBox.OwnerDraw(DrawProcs, DrawMode.OwnerDrawFixed, false);
            this.ProcsListBox.ItemHeight = 22;
            InputFormRef.markupJumpLabel(this.PROCS_JUMP_CURSOL_CODE);
        }
        byte[] UserStack;
        void InitUserStack()
        {
            this.UserStack = new byte[0x03007F00 - 0x03007000];
        }
        void InitMemroySlot()
        {
            if (Program.ROM.RomInfo.version() != 8)
            {
                this.MemorySlotLabel.Hide();
                this.MemorySlotListBox.Hide();
                return;
            }
            this.UpdateCheckDataMemorySlot = new byte[(0xD+1) * 4];
            this.MemorySlotListBox.OwnerDraw(DrawMemorySlot, DrawMode.OwnerDrawFixed, false);
                
            //とりあえず0xD個確保
            this.MemorySlotListBox.DummyAlloc(0xD + 1, this.MemorySlotListBox.SelectedIndex);

            this.MemorySlotLabel.Show();
            this.MemorySlotListBox.Show();
        }

        byte[] UpdateCheckDataFlag;
        byte[] UpdateCheckDataMemorySlot;

        void UpdateALL()
        {
            if (!Program.RAM.UpdateMemory() )
            {
                ERROR_EMU_CONNECT.Text = Program.RAM.GetErrorMessage();
                ERROR_EMU_CONNECT.Show();

                //エミュレータは終了しているのでは?
                Program.UpdateWatcher.CheckALL();
                return;
            }
            else
            {
                ERROR_EMU_CONNECT.Hide();
            }

            UpdateUserStack();
            UpdateProcs();
            UpdateFlag();
            UpdateMemorySlot();
            UpdateEvent();
            UpdateString();
            UpdateRunningEventList();
            UpdateControlUnit();
            UpdateParty();
            UpdateEditon();
            UpdateSong();
            UpdateTrap();
            UpdateBWL();
            UpdateChapterData();
            UpdateBattleActor();
            UpdateBattleTarget();
            UpdateWorldmap();
            UpdateArenaData();
            UpdateAIData();
            UpdateSupplyData();
            UpdateActionData();
            UpdateDungeonData();
            UpdateBattleSomeData();
            UpdatePalette();
        }
        void UpdateUserStack()
        {
            this.UserStack = Program.RAM.getBinaryData(0x03007000, 0x03007F00 - 0x03007000);
        }

        bool FindCurrentEvent(ref uint out_event, ref uint out_running_line)
        {
            //イベントを実行しているProcを特定する.
            uint EventEngineLoopFunction = Program.ROM.RomInfo.function_event_engine_loop_address();
            uint ref_offset = Program.ROM.RomInfo.workmemory_reference_procs_event_address_offset();
            for (int i = 0; i < ProcsTree.Count; i++)
            {
                ProcsData pd = ProcsTree[i];
                if (EventEngineLoopFunction != pd.LoopRoutine)
                {
                    continue;
                }
                uint current_event = Program.RAM.u32(pd.RAMAddr + ref_offset);
                if (!U.isSafetyPointer(current_event))
                {
                    continue;
                }
                uint current_running_line = Program.RAM.u32(pd.RAMAddr + ref_offset + 4);
                if (!U.isSafetyPointer(current_running_line))
                {
                    continue;
                }

                if (this.IgnoreEvent.ContainsKey(current_event))
                {
                    return true;
                }
                out_event = current_event;
                out_running_line = current_running_line;
                return true;
            }
            out_event = 0;
            out_running_line = 0;
            return false;
        }

        //関数として利用されているイベントを表示しないようにします.
        Dictionary<uint, bool> IgnoreEvent = new Dictionary<uint,bool>();
        void InitIgnoreEvent()
        {
            List<Address> list = new List<Address>();
            EventScript.MakeEventASMMAPList(list, true, "", true);

            foreach (Address a in list)
            {
                this.IgnoreEvent[U.toPointer(a.Addr)] = true;
            }
        }


        //実行しているイベントの開始アドレス
        uint CurrentEventBegineAddr;
        //実行しているイベントの行のアドレス
        uint CurrentEventRunningLineAddr;
        void UpdateEvent()
        {
            FindCurrentEvent(ref CurrentEventBegineAddr, ref CurrentEventRunningLineAddr);
            uint lastStringAddr = Program.ROM.RomInfo.workmemory_last_string_address();

            uint currntBGMStructAddr = Program.ROM.RomInfo.workmemory_bgm_address();
            BGM.Value = Program.RAM.u16(currntBGMStructAddr + 4);

            uint stageStructAddr = Program.ROM.RomInfo.workmemory_chapterdata_address();
            this.N_InputFormRef.ReInit(stageStructAddr, 1);
        }

        string LastStringText; //最後に表示しているテキスト
        byte[] LastStringByte; //最後に表示しているテキストのバイト列表現
        uint NextStringOffset; //次のテキストの位置

        void UpdateString()
        {
            uint length = Program.RAM.strlen(Program.ROM.RomInfo.workmemory_text_buffer_address());
            if (length == U.NOT_FOUND)
            {
                return;
            }
            byte[] strbin = Program.RAM.getBinaryData(Program.ROM.RomInfo.workmemory_text_buffer_address(), length);
            uint nextTextBuffer = Program.RAM.u32(Program.ROM.RomInfo.workmemory_next_text_buffer_address());
            if (nextTextBuffer == U.NOT_FOUND)
            {
                return ;
            }
            //変な値の場合、バッファの先頭に移動.
            if (nextTextBuffer < Program.ROM.RomInfo.workmemory_text_buffer_address())
            {
                nextTextBuffer = Program.ROM.RomInfo.workmemory_text_buffer_address();
            }
            uint offset = nextTextBuffer - Program.ROM.RomInfo.workmemory_text_buffer_address();
            bool isStrChange = U.memcmp(strbin, this.LastStringByte) != 0;
            if (offset == this.NextStringOffset && isStrChange == false)
            {//変更なし
                return;
            }

            if (isStrChange)
            {
                this.LastStringByte = strbin;

                FETextDecode decoder = new FETextDecode();
                this.LastStringText = decoder.UnHffmanPatchDecodeLow(strbin);

                //自動読み上げ
                Speech(this.LastStringText);
                CurrentTextBox.Text = TextForm.ConvertEscapeText(this.LastStringText);
            }
            this.NextStringOffset = offset;

            //字幕表示
            Subtile();

            try
            {
                HightlightDisplayString();
            }
            catch (ObjectDisposedException e)
            {
                Log.Error(e.ToString());
                return;
            }
        }

        void HightlightDisplayString()
        {
            if (this.CurrentTextBox.IsDisposed)
            {//まれに破棄したオブジェクトでアクセスがある。なぜだ?
                return;
            }

            //変更点をマークしたいので、開始点と終了点を求めないといけない.
            //完ぺきではないがそこそこ早い方法を取る.
            FETextDecode decoder = new FETextDecode();
            string nextString = decoder.UnHffmanPatchDecodeLow(U.subrange(this.LastStringByte  , this.NextStringOffset,(uint)this.LastStringByte.Length));
            nextString = TextForm.ConvertEscapeText(nextString);
            //リッチテキストボックスは、改行コードが違うため、ずれる.
            //そのため、リッチボックス内のデータから検索することで微調整します.
            string text = TextForm.ConvertEscapeText(this.LastStringText);

            //次の文字列のバッファが取れるので、そのバッファの位置を検索
            int endPoint = text.IndexOf(nextString);
            if (endPoint <= 0)
            {
                endPoint = text.Length;
            }

            string keyword = TextForm.GetLineBreak();
            int startPoint;
            //バッファの位置は少しずれるみたいなので、@0003 / [A]の位置に戻す.
            int newEndPoint = text.LastIndexOf(keyword, endPoint);
            if (newEndPoint <= 0)
            {
                startPoint = text.LastIndexOf(keyword, endPoint);
            }
            else
            {
                startPoint = text.LastIndexOf(keyword, newEndPoint);
                endPoint = newEndPoint + keyword.Length;
            }

            if (startPoint < 0)
            {
                startPoint = 0;
            }
            else
            {
                startPoint += keyword.Length;
            }

            Color displayBackColor = OptionForm.Color_NotifyWrite_BackColor();
            Color displayForeColor = OptionForm.Color_NotifyWrite_ForeColor();

            //キーワードハイライトト
            TextForm.KeywordHighLight(this.CurrentTextBox);
            if (startPoint == 0 && endPoint == text.Length)
            {//全体を選択することになるので、やらなくてもいいと思う.
            }
            else
            {
                //表示部分の選択
                this.CurrentTextBox.SelectionStart = startPoint;
                this.CurrentTextBox.SelectionLength = endPoint - startPoint;
                this.CurrentTextBox.SelectionColor = displayForeColor;
                this.CurrentTextBox.SelectionBackColor = displayBackColor;

                //選択位置の調整
                this.CurrentTextBox.SelectionStart = startPoint;
                this.CurrentTextBox.SelectionLength = 0;
            }
        }

        private void BGM_Enter(object sender, EventArgs e)
        {
            uint currntBGMStructAddr = Program.ROM.RomInfo.workmemory_bgm_address();
            N_SelectAddress.Text = U.ToHexString8(currntBGMStructAddr + 4);
        }

        private void LastText1_Enter(object sender, EventArgs e)
        {
            uint currntBGMStructAddr = Program.ROM.RomInfo.workmemory_last_string_address();
            N_SelectAddress.Text = U.ToHexString8(currntBGMStructAddr);
        }

        void UpdateFlag()
        {
            byte[] bin = Program.RAM.getBinaryData(
                Program.ROM.RomInfo.workmemory_global_flag_address()
                , this.UpdateCheckDataFlag.Length);
            if (U.memcmp(this.UpdateCheckDataFlag, bin) == 0)
            {//変更なし
                return;
            }
            //変更有
            this.UpdateCheckDataFlag = bin;
            FlagListBox.Invalidate();
        }

        class ProcsData
        {
            public uint RAMAddr;
            public uint ROMAddr;
            public uint ROMCodeCursor;
            public uint TopTreeIndex;
            public uint LoopRoutine;
            public uint BlockCounter;
            public uint Jisage;
            public string CacheName;

            public bool Compare(ProcsData p)
            {
                return
                   p.ROMCodeCursor == this.ROMCodeCursor
                && p.ROMAddr == this.ROMAddr
                && p.RAMAddr == this.RAMAddr
                && p.BlockCounter == this.BlockCounter
                ;
            }
            public bool Compare(uint romaddr, uint ramaddr)
            {
                return romaddr == this.ROMAddr
                    && ramaddr == this.RAMAddr;
            }
        }
        byte[] ProcsDataArray = new byte[0x6c * 0x40];
        List<ProcsData> ProcsTree;
        void UpdateProcs()
        {
            uint addr = Program.ROM.RomInfo.workmemory_procs_pool_address();
            byte[] bin = Program.RAM.getBinaryData(addr, this.ProcsDataArray.Length);
            if (U.memcmp(this.ProcsDataArray, bin) == 0)
            {//変更なし
                return;
            }
            this.ProcsDataArray = bin;

            //木の生成.
            List<ProcsData> tree = new List<ProcsData>();
            uint ramaddr = Program.ROM.RomInfo.workmemory_procs_forest_address();
            for (uint i = 0; i < 8; i++, ramaddr += 4)
            {
                MakeProcNode(tree, ramaddr, i , 0);
            }
            //木の更新.
            this.ProcsTree = tree;

            //個数が違うので変更があった
            this.ProcsListBox.DummyAlloc(tree.Count, this.ProcsListBox.SelectedIndex);
        }
        bool MakeProcNode(List<ProcsData> tree, uint ramaddr,uint topTreeIndex, uint jisage)
        {
            if (jisage >= 20)
            {//スタックオーバーフローの可能性
                R.Error("MakeProcNodeで無限再帰 ramaddr:{0} topTreeIndex:{1}", U.To0xHexString(ramaddr), U.To0xHexString(topTreeIndex));
                return false;
            }

            uint procHeaderP = Program.RAM.u32(ramaddr);

            ProcsData pd = new ProcsData();
            if (!U.is_02RAMPointer(procHeaderP))
            {
                return false;
            }
            uint startCode = Program.RAM.u32(procHeaderP);

            {//循環参照で無限ループに入らないように、既に知っているかどうかチェックする.
                for (int i = 0; i < tree.Count; i++)
                {
                    if (tree[i].Compare(startCode, ramaddr))
                    {//既に知っている
                        return false;
                    }
                }
            }

            pd.ROMAddr = startCode;
            pd.ROMCodeCursor = Program.RAM.u32(procHeaderP+4);
            pd.RAMAddr = procHeaderP;
            pd.TopTreeIndex = topTreeIndex;
            pd.LoopRoutine = Program.RAM.u32(procHeaderP + 0xC);
            pd.BlockCounter = Program.RAM.u8(procHeaderP + 0x28);
            pd.Jisage = jisage;

            tree.Add(pd);

            //子ノードの追跡
            MakeProcNode(tree, procHeaderP + 0x18, topTreeIndex, jisage + 1);

            //同僚のノードを探索
            MakeProcNode(tree, procHeaderP + 0x20, topTreeIndex, jisage); //next
            return true;
        }
        string GetProcName(uint procHeaderP,uint startCode)
        {
            string name = Program.AsmMapFileAsmCache.GetProcsName(startCode);
            if (name != "")
            {
                return name;
            }
            uint nameP = Program.RAM.u32(procHeaderP + 0x10);
            if (U.isSafetyPointer(nameP))
            {
                name = Program.ROM.getString(U.toOffset(nameP)).Trim();
                if (U.isAsciiString(name))
                {
                    return name;
                }
            }
            return U.To0xHexString(startCode);
        }
        string GetCursolProcCode(uint procHeaderP, uint startCode)
        {
            uint cursolP = Program.RAM.u32(procHeaderP + 0x4);
            if (! U.isSafetyPointer(cursolP))
            {
                return "";
            }
            EventScript.OneCode code = Program.ProcsScript.DisAseemble(Program.ROM.Data, cursolP);
            return EventScript.makeCommandComboText(code.Script, false);
        }

        void UpdateMemorySlot()
        {
            if (Program.ROM.RomInfo.version() != 8)
            {
                return;
            }
            if (this.UpdateCheckDataMemorySlot == null)  //まれにnullになるらしい(原因不明)
            {
                this.UpdateCheckDataMemorySlot = new byte[(0xD + 1) * 4];
            }

            byte[] bin = Program.RAM.getBinaryData(
                Program.ROM.RomInfo.workmemory_memoryslot_address()
                , this.UpdateCheckDataMemorySlot.Length);
            if (U.memcmp(this.UpdateCheckDataMemorySlot, bin) == 0)
            {//変更なし
                return;
            }
            //変更有

            //個数が変わるのか？
            uint newCount = U.u32(bin, 0xD * 4);
            uint oldCount = U.u32(this.UpdateCheckDataMemorySlot, 0xD * 4);

            this.UpdateCheckDataMemorySlot = bin;
            if (newCount == oldCount)
            {//個数は変更されない 再描画だけでOK
                MemorySlotListBox.Invalidate();
                return;
            }
            if (newCount > 0x100)
            {//サイズがめちゃくちゃな値
                return;
            }


            //個数が変更されるため再描画が必要.
            this.MemorySlotListBox.DummyAlloc((int)(0xD + 1 + 1 + newCount), this.MemorySlotListBox.SelectedIndex);
        }

        private Size DrawFlag(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            string text = lb.Items[index].ToString();
            uint flag = U.atoh(text);

            bool flagStatus = false;
            if (flag == 0)
            {
                flagStatus = false;
            }
            else if (flag >= 0x65)
            {//クローバル
                uint a = (flag - 0x65);
                uint flagMemory = Program.ROM.RomInfo.workmemory_global_flag_address() + (a / 8);
                uint flagMask = (uint)(1 << (int)(a % 8));

                uint data = Program.RAM.u8(flagMemory);
                if ((data & flagMask) == flagMask)
                {
                    flagStatus = true;
                }
            }
            else
            {//ローカル
                uint a = (flag - 0x1);
                uint flagMemory = Program.ROM.RomInfo.workmemory_local_flag_address() + (a / 8);
                uint flagMask = (uint)(1 << (int)(a % 8));

                uint data = Program.RAM.u8(flagMemory);
                if ((data & flagMask) == flagMask)
                {
                    flagStatus = true;
                }
            }

            SolidBrush brush ;

            if (flagStatus)
            {//ON
                brush = this.ListBoxForeKeywordBrush;
                text += " = TRUE";
            }
            else
            {//OFF
                brush = this.ListBoxForeBrush;
                text += " = false";
            }

            Rectangle bounds = listbounds;

            int lineHeight = ListBoxEx.OWNER_DRAW_ICON_SIZE;

            bounds.X += U.DrawText(text, g, lb.Font, brush, isWithDraw, bounds);
            bounds.Y += lineHeight;
            // //ブラシをキャッシュするからやってはいけない.
            return new Size(bounds.X, bounds.Y);
        }
        private void FlagListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.FlagListBox.SelectedIndex;
            if (index < 0 || index >= this.FlagListBox.Items.Count)
            {
                return;
            }
            string text = this.FlagListBox.Items[index].ToString();
            uint flag = U.atoh(text);

            if (flag == 0)
            {
                N_SelectAddress.Text = "";
                return;
            }
            else if (flag >= 0x65)
            {//クローバル
                uint a = (flag - 0x65);
                uint flagMemory = Program.ROM.RomInfo.workmemory_global_flag_address() + (a / 8);

                N_SelectAddress.Text = U.ToHexString8(flagMemory) + String.Format(" (bit:{0})", a % 8);
            }
            else
            {//ローカル
                uint a = (flag - 0x1);
                uint flagMemory = Program.ROM.RomInfo.workmemory_local_flag_address() + (a / 8);

                N_SelectAddress.Text = U.ToHexString8(flagMemory) + String.Format(" (bit:{0})", a % 8);
            }
        }

        private void MemorySlotListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.MemorySlotListBox.SelectedIndex;
            if (index < 0 || index >= this.MemorySlotListBox.Items.Count)
            {
                return;
            }

            uint addr;
            if (IsEventCounter_Index(index, this.MemorySlotListBox))
            {
                addr = Program.ROM.RomInfo.workmemory_eventcounter_address();
            }
            else
            {
                addr = Program.ROM.RomInfo.workmemory_memoryslot_address() + ((uint)index * 4);
            }
            N_SelectAddress.Text = U.ToHexString8(addr);
        }

        //スタック領域にこのProcsのデータがあるかどうか
        //スタックポインタの位置が特定できないので、スタックに残っている全データを見るしかない
        //だから目安にして利用できない.
        bool IsRunningProcByStack(uint procsAddr)
        {
            for (uint i = 0; i < this.UserStack.Length; i += 4)
            {
                if (this.UserStack[i+3] != 0x02 || this.UserStack[i + 2] != 0x02)
                {//探索を早くするために 0x0202以外は見ない
                    continue;
                }
                uint p = U.u32(this.UserStack, i);
                if (p == procsAddr)
                {
                    return true;
                }
            }
            return false;
        }

        private Size DrawProcs(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            //実行している行を強調表示するための余白
            const int RunningBoxWidth = 12;
            if (index < 0 || index >= this.ProcsTree.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }

            ProcsData pd = this.ProcsTree[index];
            if (pd.CacheName == null)
            {
                pd.CacheName = GetProcName(pd.RAMAddr, pd.ROMAddr);
            }

            Rectangle bounds = listbounds;
            {
                Rectangle b = listbounds;
                b.Width = RunningBoxWidth;

                if (IsRunningProcByStack(pd.RAMAddr))
                {//実行位置の強調
                    U.DrawPicture(this.YubiYokoCursor, g, isWithDraw, b);
                }
                //縦線
                U.DrawLineByBrush(g, this.ListBoxForeBrush
                    , b.X + RunningBoxWidth - 1
                    , b.Y
                    , b.X + RunningBoxWidth
                    , b.Y + bounds.Height
                    , isWithDraw);
            }
            bounds.X += RunningBoxWidth;

            int lineHeight = ListBoxEx.OWNER_DRAW_ICON_SIZE;

            string text;
            if (pd.Jisage == 0)
            {
                text = "#" + pd.TopTreeIndex.ToString() + " ";
            }
            else
            {
                bounds.X += (int)(lb.Font.Height * 2 * pd.Jisage);
                text = "+ ";
            }
            bounds.X += U.DrawText(text, g, lb.Font, this.ListBoxForeKeywordBrush, isWithDraw, bounds);

            bounds.X += U.DrawText(pd.CacheName, g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);

            text = "+" + U.To0xHexString(pd.ROMCodeCursor - pd.ROMAddr);
            bounds.X += U.DrawText(text, g, this.BoldFont, this.ListBoxForeBrush, isWithDraw, bounds);

            if (pd.LoopRoutine != 0)
            {
                bounds.X += U.DrawText(" (", g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);

                if (pd.LoopRoutine == Program.ROM.RomInfo.function_sleep_handle_address())
                {
                    bounds.X += U.DrawText("Sleep", g, this.Font, this.ListBoxForeKeywordBrush, isWithDraw, bounds);
                }
                else
                {
                    string funcname = Program.AsmMapFileAsmCache.GetName(pd.LoopRoutine);
                    if (funcname == "")
                    {
                        bounds.X += U.DrawText(U.To0xHexString(pd.LoopRoutine), g, this.Font, this.ListBoxForeKeywordBrush, isWithDraw, bounds);
                    }
                    else
                    {
                        bounds.X += U.DrawText(funcname, g, this.Font, this.ListBoxForeKeywordBrush, isWithDraw, bounds);
                    }
                }

                if (pd.BlockCounter > 0)
                {
                    text = ":" + pd.BlockCounter;
                    bounds.X += U.DrawText(text, g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
                }

                text = ")";
                bounds.X += U.DrawText(text, g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            }

            bounds.X += 5;

            //現在実行しているProcsの行にあるイベントを表示
            Size size = DrawProcCursolCode(lb, pd, g, bounds, isWithDraw);
            bounds.X += size.Width;
            bounds.Y += Math.Max(lineHeight, size.Height);
            return new Size(bounds.X, bounds.Y);
        }
        private Size DrawProcCursolCode(ListBox lb, ProcsData pd, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            uint addr = pd.ROMCodeCursor ;
            if (U.isSafetyPointer(pd.ROMCodeCursor) == false || pd.ROMAddr >= addr )
            {//カーソルが不明な位置を指している
                addr = pd.ROMAddr;
            }

            EventScript.OneCode code = Program.ProcsScript.DisAseemble(Program.ROM.Data, addr);
            return EventScriptForm.DrawCode(lb, g, listbounds, isWithDraw, code);
        }
        bool IsEventCounter_Index(int index,ListBox lb)
        {
            return index == lb.Items.Count - 1;
        }

        private Size DrawMemorySlot(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            Rectangle bounds = listbounds;

            int lineHeight = ListBoxEx.OWNER_DRAW_ICON_SIZE;

            if (IsEventCounter_Index(index,lb))
            {//最後の項目はカウンター
                string text = "Counter: ";

                bounds.X += U.DrawText(text, g, this.Font, this.ListBoxForeKeywordBrush, isWithDraw, bounds);

                uint data = Program.RAM.u32(Program.ROM.RomInfo.workmemory_eventcounter_address());
                text = U.To0xHexString(data);

                bounds.X += U.DrawText(text, g, this.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            }
            else
            {//それ以外はメモリスロット
                string text;
                if (index <= 0xD)
                {
                    text = U.ToHexString(index) + " ";
                }
                else
                {
                    text = "Q:" + U.ToHexString(index - 0xD) + " ";
                }
                bounds.X += U.DrawText(text, g, this.Font, this.ListBoxForeKeywordBrush, isWithDraw, bounds);

                uint data = Program.RAM.u32((uint)(index * 4) + Program.ROM.RomInfo.workmemory_memoryslot_address());
                text = U.To0xHexString(data);

                bounds.X += U.DrawText(text, g, this.Font, this.ListBoxForeBrush, isWithDraw, bounds);

                text = Program.AsmMapFileAsmCache.GetName(data);
                if (text != "")
                {
                    text = " " + text;
                    bounds.X += U.DrawText(text, g, this.Font, this.ListBoxForeKeywordBrush, isWithDraw, bounds);
                }
            }

            bounds.Y += lineHeight;
            return new Size(bounds.X, bounds.Y);
        }


        private void AutoUpdateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.AutoUpdateCheckBox.Checked)
            {
                timer1.Start();
            }
            else
            {
                timer1.Stop();
            }
        }


        private void EmulatorMemoryForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Stop();
            if (this.ListBoxForeBrush != null)
            {
                this.ListBoxForeBrush.Dispose();
                this.ListBoxForeBrush = null;
            }
            if (this.ListBoxForeBrush != null)
            {
                this.ListBoxForeKeywordBrush.Dispose();
                this.ListBoxForeKeywordBrush = null;
            }
            if (this.ListBoxForeDecBrush != null)
            {
                this.ListBoxForeDecBrush.Dispose();
                this.ListBoxForeDecBrush = null;
            }
            if (this.BoldFont != null)
            {
                this.BoldFont.Dispose();
                this.BoldFont = null;
            }
            if (this.YubiYokoCursor != null)
            {
                this.YubiYokoCursor.Dispose();
                this.YubiYokoCursor = null;
            }
            if (IsAutoSpeech)
            {
                TextToSpeechForm.Stop();
                IsAutoSpeech = false;
            }
            if (IsSubtileSpeech)
            {
                ToolSubtitleSetingDialogForm.CloseSubTile();
                IsSubtileSpeech = false;
            }
        }

        private void ProcsListBox_DoubleClick(object sender, EventArgs e)
        {
            ShowFloatingControlpanel();
        }

        private void ProcsListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ShowFloatingControlpanel();
                return;
            }
        }

        void ShowFloatingControlpanel()
        {
            int index = this.ProcsListBox.SelectedIndex;
            if (index < 0 || index >= this.ProcsTree.Count)
            {//一件もない
                return;
            }

            ProcsData pd = this.ProcsTree[index];
            if (pd.CacheName == null)
            {
                pd.CacheName = GetProcName(pd.RAMAddr, pd.ROMAddr);
            }

            PROCS_Address.Value = pd.RAMAddr;
            PROCS_NAME.Text = pd.CacheName;
            PROCS_CURSOL_CODE.Text = GetCursolProcCode(pd.RAMAddr, pd.ROMAddr); ;
            this.PROCS_InputFormRef.ReInit(pd.RAMAddr, 1);
            Proc_ControlPanel.Show();
        }
        public void HideFloatingControlpanel()
        {
            if (MainTabControl.SelectedTab == EtcPage)
            {
                this.Party_ControlPanel.Hide();
                this.PartyListBox.Focus();
            }
            else
            {
                this.Proc_ControlPanel.Hide();
                this.ProcsListBox.Focus();
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            HideFloatingControlpanel();
        }

        private void EmulatorMemoryForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                HideFloatingControlpanel();
            }
            else if (e.Control && e.KeyCode == Keys.U)
            {
                MainTabControl.SelectedTab = CheatPage;
                CHEAT_SET_FLAG03.PerformClick();
            }
            else if (e.Control && e.KeyCode == Keys.G)
            {
                MainTabControl.SelectedTab = CheatPage;
                CHEAT_ALL_PLAYER_UNIT_GROW.PerformClick();
            }
        }

        private void ProcsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideFloatingControlpanel();
        }



        private void J_CurrentEventAddress_Click(object sender, EventArgs e)
        {
            if (!U.isSafetyPointer(CurrentEventBegineAddr))
            {
                return;
            }

            EventScriptForm f = (EventScriptForm)InputFormRef.JumpForm<EventScriptForm>(U.NOT_FOUND);
            if (!U.isSafetyPointer(CurrentEventRunningLineAddr))
            {
                f.JumpTo(U.toOffset(CurrentEventBegineAddr), U.NOT_FOUND);
            }
            else
            {
                f.JumpTo(U.toOffset(CurrentEventBegineAddr), U.toOffset(CurrentEventRunningLineAddr));
            }
        }

        private void PROCS_JUMP_CURSOL_CODE_Click(object sender, EventArgs e)
        {
            uint addr = U.toOffset((uint)PROCS_P0.Value);
            uint currnt_procs = U.toOffset((uint)PROCS_P4.Value);

            ProcsScriptForm f = (ProcsScriptForm)InputFormRef.JumpForm<ProcsScriptForm>(U.NOT_FOUND);
            f.JumpTo(addr, currnt_procs);
        }

        public static void CloseIfAutoClose()
        {
            if (OptionForm.auto_connect_emulator_enum.AutoConnectAndAutoClose != OptionForm.auto_connect_emulator())
            {
                //自動的に終了しない
                return;
            }
            //自動終了する.
            InputFormRef.CloseForm<EmulatorMemoryForm>();
        }

        private void RunningEventListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                JumpSelectedEventAddress();
            }
        }

        private void RunningEventListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            JumpSelectedEventAddress();
        }
        private void JumpSelectedEventAddress()
        {
            Debug.Assert(CurrentEventBegineAddr == DisplayEventBegineAddr);
            if (!U.isSafetyPointer(CurrentEventBegineAddr))
            {
                return;
            }

            EventScriptForm f = (EventScriptForm)InputFormRef.JumpForm<EventScriptForm>(U.NOT_FOUND);

            int selected = this.RunningEventListBox.SelectedIndex;
            if ( selected < 0
                || selected >= this.DisplayEventAsm.Count)
            {
                f.JumpTo(U.toOffset(CurrentEventBegineAddr), U.NOT_FOUND);
                return;
            }

            uint addr = EventScript.ConvertSelectedToAddr(CurrentEventBegineAddr, selected , this.DisplayEventAsm);
            f.JumpTo(CurrentEventBegineAddr, addr);
        }

        List<EventScript.OneCode> DisplayEventAsm;
        void InitRunningEventList()
        {
            this.LastStringText = "";
            this.DisplayEventAsm = new List<EventScript.OneCode>();
            this.RunningEventListBox.OwnerDraw(DrawRunningEventList, DrawMode.OwnerDrawVariable, false);
        }
        void InitCheat(List<Control> controls)
        {
            string[] args = new string[0];
            InputFormRef.makeLinkEventHandler("", controls, this.CHEAT_ITEM_ID, this.CHEAT_ITEM_NAME, 0, "ITEM", args);
            InputFormRef.makeLinkEventHandler("", controls, this.CHEAT_ITEM_ID, this.CHEAT_ITEM_ICON, 0, "ITEMICON", args);
            InputFormRef.makeJumpEventHandler(this.CHEAT_ITEM_ID, this.CHEAT_ITEM_JUMP, "ITEM", args);

            InputFormRef.makeLinkEventHandler("", controls, this.CHEAT_WEATHER_VALUE, this.CHEAT_WEATHER_COMBO, 0, "COMBO", args);

            if (Program.ROM.RomInfo.version() == 8)
            {
                this.CHEAT_ITEM_ID.Value = 0x88; //マスタープルフ
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                this.CHEAT_ITEM_ID.Value = 0x87; //地の刻印
            }
            else 
            {//6
                this.CHEAT_ITEM_ID.Value = 0x5F; //勇者の証
            }
            CanNotUpdateControlUnit();

            InputFormRef.makeLinkEventHandler("", controls, this.CHEAT_WARP_CHPATER_VALUE, this.CHEAT_WARP_CHPATER_NAME, 0, "MAP", args);
            InputFormRef.makeJumpEventHandler(this.CHEAT_WARP_CHPATER_VALUE, this.CHEAT_WARP_CHPATER_JUMP, "MAP", args);
            if (Program.ROM.RomInfo.version() >= 7)
            {
                InputFormRef.makeLinkEventHandler("", controls, this.CHEAT_WARP_EDTION_VALUE, this.CHEAT_WARP_EDTION_NAME, 0, "CHAPTORMODE", args);
            }
            else
            {
                CHEAT_WARP_EDTION_JUMP.Hide();
                CHEAT_WARP_EDTION_VALUE.Hide();
                CHEAT_WARP_EDTION_NAME.Hide();
            }

            if (Program.ROM.RomInfo.version() >= 8)
            {
                InputFormRef.makeLinkEventHandler("", controls, this.CHEAT_WARP_NODE_VALUE, this.CHEAT_WARP_NODE_NAME, 0, "BASEPOINT", args);
                InputFormRef.makeJumpEventHandler(this.CHEAT_WARP_NODE_VALUE, this.CHEAT_WARP_NODE_JUMP, "BASEPOINT", args);
            }
            else
            {
                CHEAT_WARP_NODE_JUMP.Hide();
                CHEAT_WARP_NODE_VALUE.Hide();
                CHEAT_WARP_NODE_NAME.Hide();
            }
        }

        //表示しているイベントの開始アドレス
        uint DisplayEventBegineAddr;
        //表示しているイベントの実行している行のアドレス
        uint DisplayEventRunningLineAddr;

        void UpdateRunningEventList()
        {
            int selected;
            if (this.DisplayEventBegineAddr == this.CurrentEventBegineAddr)
            {//表示しているイベントと実行中のイベントが同一
                if (this.DisplayEventRunningLineAddr == this.CurrentEventRunningLineAddr)
                {//実行している行の位置も同一なので何もしなくてよい.
                    return;
                }
                //実行している位置が変わったので描画しないといけない.
                this.DisplayEventRunningLineAddr = this.CurrentEventRunningLineAddr;

                //実行しているところを選択するのが一番簡単な方法だと思う.
                selected = EventScript.ConvertAddrToSelected(this.DisplayEventAsm, this.DisplayEventBegineAddr, this.DisplayEventRunningLineAddr);
                AddEventHistoryList(selected);

                this.RunningEventListBox.SelectedIndex = selected;
//                if (this.RunningEventListBox.SelectedIndex - this.RunningEventListBox.TopIndex > 15)
//                {
//                    this.RunningEventListBox.TopIndex = selected;
//                }
                return;
            }
            //表示しているアドレスと違う場合、イベントの取り直しと描画をし直さないといけない
            this.DisplayEventBegineAddr = this.CurrentEventBegineAddr;
            this.DisplayEventRunningLineAddr = this.CurrentEventRunningLineAddr; 
            
            this.DisplayEventAsm = new List<EventScript.OneCode>();

            uint addr = (uint)this.DisplayEventBegineAddr;
            addr = U.toOffset(addr);
            if (!U.isSafetyOffset(addr))
            {
                this.RunningEventListBox.Items.Clear();
                return;
            }

            bool isWorldMapEvent = WorldMapEventPointerForm.isWorldMapEvent(addr);
            uint bytecount = EventScript.SearchEveneLength(Program.ROM.Data, addr, isWorldMapEvent);

            uint limit = Math.Min(addr + bytecount, (uint)Program.ROM.Data.Length);
            while (addr < limit)
            {
                EventScript.OneCode code = Program.EventScript.DisAseemble(Program.ROM.Data, addr);
                this.DisplayEventAsm.Add(code);

                addr += (uint)code.Script.Size;
            }
            //最後に字下げ処理実行.
            EventScriptInnerControl.JisageReorder(this.DisplayEventAsm);

            selected = EventScript.ConvertAddrToSelected(this.DisplayEventAsm, this.DisplayEventBegineAddr, this.DisplayEventRunningLineAddr);
            AddEventHistoryList(selected);

            //リストの更新.
            this.RunningEventListBox.DummyAlloc(this.DisplayEventAsm.Count, selected);
        }
        private Size DrawRunningEventList(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            //実行している行を強調表示するための余白
            const int RunningBoxWidth = 12;
            if (index < 0 || index >= this.DisplayEventAsm.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            if (isWithDraw)
            {
                Rectangle bounds = listbounds;
                bounds.Width = RunningBoxWidth;
                
                int displaEventRunningLineNumber = EventScript.ConvertAddrToSelected(this.DisplayEventAsm, this.DisplayEventBegineAddr, this.DisplayEventRunningLineAddr);
                if (index == displaEventRunningLineNumber)
                {
                    U.DrawPicture(this.YubiYokoCursor, g, isWithDraw, bounds);
                }
                //縦線
                U.DrawLineByBrush(g, this.ListBoxForeBrush
                    ,bounds.X + RunningBoxWidth - 1
                    ,bounds.Y
                    ,bounds.X + RunningBoxWidth
                    ,bounds.Y + bounds.Height
                    ,isWithDraw);
            }

            EventScript.OneCode code = this.DisplayEventAsm[index];
            {
                //実行している行を強調表示するために余白を作る.
                Rectangle bounds = listbounds;
                bounds.X += RunningBoxWidth;
                return EventScriptForm.DrawCode(lb, g, bounds, isWithDraw, code);
            }
        }

        class EventHistoryStruct
        {
            public DateTime Time;
            public uint EventAddr;
            //public uint EventRunningAddr;
            public uint EventPackedAddr;
        }
        List<EventHistoryStruct> EventHistoryList;
        
        void InitEventHistoryList()
        {
            this.EventHistoryList = new List<EventHistoryStruct>();
            this.EventHistoryListBox.OwnerDraw(DrawEventHistoryList, DrawMode.OwnerDrawVariable, false);
        }
        void AddEventHistoryList(int selected)
        {
            if (!U.isSafetyPointer(this.DisplayEventBegineAddr))
            {
                return ;
            }

            EventHistoryStruct p = new EventHistoryStruct();
            p.Time = DateTime.Now;
            p.EventAddr = this.DisplayEventBegineAddr;
            //p.EventRunningAddr = this.DisplayEventRunningLineAddr;
            p.EventPackedAddr = EventScript.ConvertSelectedToAddr(this.DisplayEventBegineAddr, selected, this.DisplayEventAsm);
            if (this.EventHistoryList.Count >= 100)
            {//100件を超えるなら先頭を消そう.
                this.EventHistoryList.RemoveAt(0);
            }
            this.EventHistoryList.Add(p);

            //リストの更新.
            this.EventHistoryListBox.DummyAlloc(this.EventHistoryList.Count, this.EventHistoryListBox.SelectedIndex);
        }

        private Size DrawEventHistoryList(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= this.EventHistoryList.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            //逆順に描画する.
            EventHistoryStruct history = this.EventHistoryList[this.EventHistoryList.Count - 1 - index];

            Rectangle bounds = listbounds;
            string text = history.Time.ToLocalTime().ToString();
            U.DrawText(text, g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 140;

            text = U.ToHexString8(history.EventAddr);
            U.DrawText(text, g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 80;

            text = U.ToHexString8(history.EventPackedAddr);
            U.DrawText(text, g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 80;

            //アドレスにあるイベントを表示.
            EventScript.OneCode code = Program.EventScript.DisAseemble(Program.ROM.Data, U.toOffset(history.EventPackedAddr) );
            return EventScriptForm.DrawCode(lb, g, bounds, isWithDraw, code);
        }

        private void EventHistoryListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                JumpEventHistoryList();
            }
        }

        private void EventHistoryListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            JumpEventHistoryList();
        }
        void JumpEventHistoryList()
        {
            int index = this.EventHistoryListBox.SelectedIndex;
            if (index < 0 || index >= this.EventHistoryList.Count)
            {
                return;
            }
            //逆順に描画しているため.
            EventHistoryStruct history = this.EventHistoryList[this.EventHistoryList.Count - 1 - index];

            EventScriptForm f = (EventScriptForm)InputFormRef.JumpForm<EventScriptForm>(U.NOT_FOUND);
            if (!U.isSafetyPointer(history.EventPackedAddr))
            {
                f.JumpTo(U.toOffset(history.EventAddr), U.NOT_FOUND);
            }
            else
            {
                f.JumpTo(U.toOffset(history.EventAddr), U.toOffset(history.EventPackedAddr));
            }
        }

        private void Dump0x02000000Button_Click(object sender, EventArgs e)
        {
            if (!CheckConnectShowError())
            {
                return;
            }

            string title = R._("保存するファイル名を選択してください");
            string filter = R._("BIN|*.bin|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            Program.LastSelectedFilename.Load(this, "", save, "Page02");

            DialogResult dr = save.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            string filename = save.FileNames[0];
            Program.LastSelectedFilename.Save(this, "", save);

            if (!CheckConnectShowError())
            {
                return;
            }
            Program.RAM.DumpMemory0x02000000(filename);

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(filename);
        }

        private void Dump0x03000000Button_Click(object sender, EventArgs e)
        {
            if (!CheckConnectShowError())
            {
                return;
            }

            string title = R._("保存するファイル名を選択してください");
            string filter = R._("BIN|*.bin|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            Program.LastSelectedFilename.Load(this, "", save, "Page03");

            DialogResult dr = save.ShowDialog();
            if (dr != DialogResult.OK)
            {
                return;
            }
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            string filename = save.FileNames[0];
            Program.LastSelectedFilename.Save(this, "", save);

            if (!CheckConnectShowError())
            {
                return;
            }
            Program.RAM.DumpMemory0x03000000(filename);

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(filename);
        }

        bool CheckConnectShowError()
        {
            if (! Program.RAM.IsConnect())
            {
                R.ShowStopError("エミュレータと接続できませんでした。");
                return false;
            }
            return true;
        }

        private void CHEAT_MONEY_Click(object sender, EventArgs e)
        {
            if (!CheckConnectShowError())
            {
                return;
            }
            uint stageStructAddr = Program.ROM.RomInfo.workmemory_chapterdata_address();
            uint writeRAMPointer = stageStructAddr + 0x8;
            Program.RAM.write_u32(writeRAMPointer, (uint)CHEAT_MONEY_VALUE.Value);

            InputFormRef.ShowWriteNotifyAnimation(this, writeRAMPointer);
        }

        private void CHEAT_FOG_Click(object sender, EventArgs e)
        {
            if (!CheckConnectShowError())
            {
                return;
            }
            uint stageStructAddr = Program.ROM.RomInfo.workmemory_chapterdata_address();
            uint writeRAMPointer = stageStructAddr + 0xD;
            Program.RAM.write_u8(writeRAMPointer, (uint)CHEAT_FOG_VALUE.Value);

            InputFormRef.ShowWriteNotifyAnimation(this, writeRAMPointer);
        }
        void WriteFlag(uint flag,bool setValue)
        {
            if (flag == 0 || flag > 0x12C)
            {
                return;
            }
            if (!CheckConnectShowError())
            {
                return;
            }

            uint flagMemory;
            uint flagMask;
            uint data;
            if (flag >= 0x65)
            {//クローバル
                uint a = (flag - 0x65);
                flagMemory = Program.ROM.RomInfo.workmemory_global_flag_address() + (a / 8);
                flagMask = (uint)(1 << (int)(a % 8));
            }
            else
            {//ローカル
                uint a = (flag - 0x1);
                flagMemory = Program.ROM.RomInfo.workmemory_local_flag_address() + (a / 8);
                flagMask = (uint)(1 << (int)(a % 8));
            }

            data = Program.RAM.u8(flagMemory);
            if (setValue)
            {
                data |= flagMask;
            }
            else
            {
                data &= ~flagMask;
            }
            Program.RAM.write_u8(flagMemory, data);
            InputFormRef.ShowWriteNotifyAnimation(this, flagMemory);
        }
        private void FlagListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            UpdateFlagUI();
        }
        private void FlagListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UpdateFlagUI();
            }

        }

        void UpdateFlagUI()
        {
            if (!CheckConnectShowError())
            {
                return;
            }

            int index = FlagListBox.SelectedIndex;
            if (index < 0 || index >= FlagListBox.Items.Count)
            {
                return;
            }
            string text = FlagListBox.Items[index].ToString();
            uint flag = U.atoh(text);

            bool flagStatus = false;
            if (flag == 0)
            {
                return;
            }
            else if (flag >= 0x65)
            {//クローバル
                uint a = (flag - 0x65);
                uint flagMemory = Program.ROM.RomInfo.workmemory_global_flag_address() + (a / 8);
                uint flagMask = (uint)(1 << (int)(a % 8));

                uint data = Program.RAM.u8(flagMemory);
                if ((data & flagMask) == flagMask)
                {
                    flagStatus = true;
                }
            }
            else
            {//ローカル
                uint a = (flag - 0x1);
                uint flagMemory = Program.ROM.RomInfo.workmemory_local_flag_address() + (a / 8);
                uint flagMask = (uint)(1 << (int)(a % 8));

                uint data = Program.RAM.u8(flagMemory);
                if ((data & flagMask) == flagMask)
                {
                    flagStatus = true;
                }
            }

            if (flagStatus)
            {
                DialogResult dr = R.ShowYesNo("フラグ「{0}」を下しますか？", text);
                if (dr != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
                WriteFlag(flag, false);
            }
            else
            {
                DialogResult dr = R.ShowYesNo("フラグ「{0}」を立てますか", text);
                if (dr != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
                WriteFlag(flag, true);
            }
        }

        private void CHEAT_SET_FLAG03_Click(object sender, EventArgs e)
        {
//            WriteFlag(0x03, true);
            EmulatorMemoryUtil.CHEAT_CALLENDEvent(this);
        }

        void CanNotUpdateControlUnit()
        {
            CurrentControlUnitRAMAddress = 0;
            CHEAT_UNIT_MEMORY_AND_NAME.Text = R._("ユニットが選択されていません");
            CHEAT_UNIT_MEMORY_AND_ICON.Image = null;
        }

        uint CurrentControlUnitRAMAddress;
        void UpdateControlUnit()
        {
            uint control_unit_address = Program.ROM.RomInfo.workmemory_control_unit_address();
            uint unit_ram_address = Program.RAM.u32(control_unit_address);
            if (CurrentControlUnitRAMAddress == unit_ram_address)
            {//変更なし.
                return;
            }
            
            if (!U.is_02RAMPointer(unit_ram_address))
            {//選択しているキャラクターはいない.
                CanNotUpdateControlUnit();
                return;
            }

            uint romUnitPointer = Program.RAM.u32(unit_ram_address + 0);
            if (!U.isSafetyPointer(romUnitPointer) )
            {//不正な値が入っています.
                CanNotUpdateControlUnit();
                return;
            }

            uint romUnitAddr = U.toOffset(romUnitPointer);
            string name = UnitForm.GetUnitNameByAddr(romUnitAddr);
            uint uid = UnitForm.GetUnitIDByAddr(romUnitAddr);
            Bitmap iconBitmap = UnitForm.DrawUnitFacePictureByAddr(romUnitAddr, true);
            U.MakeTransparent(iconBitmap);
            CHEAT_UNIT_MEMORY_AND_ICON.Image = iconBitmap;
            CHEAT_UNIT_MEMORY_AND_NAME.Text = string.Format("{0} {1} //{2}->{3}->{4}"
                ,U.ToHexString(uid)
                ,name
                , U.ToHexString8(control_unit_address)
                , U.ToHexString8(unit_ram_address)
                , U.ToHexString8(romUnitPointer)
                );

            CurrentControlUnitRAMAddress = unit_ram_address;

            //CHEAT_UNIT_MEMORY_AND_NAME
        }
        void KillUnit(uint unitRAMAddress)
        {
            Program.RAM.write_u32(unitRAMAddress, 0);
        }
        void UpdateUnitAIToDoNotMOVE(uint unitRAMAddress, bool showNotify)
        {
            if (Program.ROM.RomInfo.version() == 6)
            {
                Program.RAM.write_u8(unitRAMAddress + 0x42, 0x3);
                Program.RAM.write_u8(unitRAMAddress + 0x44, 0x3);
            }
            else
            {
                Program.RAM.write_u8(unitRAMAddress + 0x42, 0x6);
                Program.RAM.write_u8(unitRAMAddress + 0x44, 0x3);
            }


            if (showNotify)
            {
                InputFormRef.ShowWriteNotifyAnimation(this, unitRAMAddress);
            }
        }
        void UpdateUnitHP1(uint unitRAMAddress, bool showNotify)
        {
            uint writeRAMPointer;
            if (Program.ROM.RomInfo.version() == 6)
            {
                writeRAMPointer = unitRAMAddress + 0x11;
            }
            else
            {
                writeRAMPointer = unitRAMAddress + 0x13;
            }
            Program.RAM.write_u8(writeRAMPointer, 1);

            if (showNotify)
            {
                InputFormRef.ShowWriteNotifyAnimation(this, writeRAMPointer);
            }
        }

        private void CHEAT_UNIT_HP_1_Click(object sender, EventArgs e)
        {
            if (!CheckConnectShowError())
            {
                return;
            }
            if (!CheckUnitSelectAndError())
            {
                return;
            }
            Debug.Assert(U.is_02RAMPointer(CurrentControlUnitRAMAddress));
            UpdateUnitHP1(CurrentControlUnitRAMAddress , showNotify: true);
        }

        private void CHEAT_UNIT_HAVE_ITEM_Click(object sender, EventArgs e)
        {
            if (!CheckConnectShowError())
            {
                return;
            }
            if (!CheckUnitSelectAndError())
            {
                return;
            }
            Debug.Assert(U.is_02RAMPointer(CurrentControlUnitRAMAddress));


            uint writeRAMPointer;
            if (Program.ROM.RomInfo.version() == 6)
            {
                writeRAMPointer = CurrentControlUnitRAMAddress + 0x1C;
            }
            else
            {
                writeRAMPointer = CurrentControlUnitRAMAddress + 0x1E;
            }
            bool itemFull = true;
            for (int i = 0; i < 5; i++, writeRAMPointer += 2)
            {
                uint havingItem = Program.RAM.u8(writeRAMPointer);
                if (havingItem == 0)
                {
                    itemFull = false;
                    break;
                }
            }

            if (itemFull)
            {
                R.ShowStopError("このユニットは、すでにアイテムを5つ持っているので、これ以上持たせられません。");
                return;
            }
            Program.RAM.write_u8(writeRAMPointer + 0, (uint)CHEAT_ITEM_ID.Value);
            Program.RAM.write_u8(writeRAMPointer + 1, (uint)CHEAT_ITEM_COUNT.Value);

            InputFormRef.ShowWriteNotifyAnimation(this, writeRAMPointer);
        }

        private void CHEAT_UNIT_GROW_Click(object sender, EventArgs e)
        {
            if (!CheckConnectShowError())
            {
                return;
            }
            if (!CheckUnitSelectAndError())
            {
                return;
            }
            Debug.Assert(U.is_02RAMPointer(CurrentControlUnitRAMAddress));

            CheatUnitGrow(CurrentControlUnitRAMAddress,growMovePower: true, showNotify: true );
        }
        void CheatUnitGrow(uint ramUnitAddress,bool growMovePower, bool showNotify)
        {
            uint romUnitPointer = Program.RAM.u32(ramUnitAddress + 0);
            if (!U.isSafetyPointer(romUnitPointer))
            {//不正な値が入っています.
                return;
            }
            uint romUnitAddr = U.toOffset(romUnitPointer);

            uint romClassPointer = Program.RAM.u32(ramUnitAddress + 4);
            if (!U.isSafetyPointer(romClassPointer))
            {//不正な値が入っています.
                return;
            }
            uint romClassAddr = U.toOffset(romClassPointer);

            //上級職かどうか
            bool promoted = UnitForm.IsUnitPromotedByAddr(romUnitAddr)
                || ClassForm.IsClassPromotedByAddr(romClassAddr);

            uint lv = Program.RAM.u8(ramUnitAddress + 0x8);
            if (promoted == false && lv < 10)
            {//LV10以下の場合 CCできるLV10まで引き上げる
                Program.RAM.write_u8(ramUnitAddress + 0x8, 10);
            }

            //保持している武器レベルを最大値まで上げる
            uint weaponLvAddr;
            if (Program.ROM.RomInfo.version() == 6)
            {
                weaponLvAddr = ramUnitAddress + 0x26;
            }
            else
            {
                weaponLvAddr = ramUnitAddress + 0x28;
            }
            for (int i = 0; i < 8; i++, weaponLvAddr++)
            {
                uint weaponLv = Program.RAM.u8(weaponLvAddr);
                if (weaponLv <= 0)
                {//この武器は利用できない
                    continue;
                }
                if (weaponLv >= 250)
                {//すでに武器レベルA以上なので何もしない
                    continue;
                }
                //武器Aまで引き上げる Sについては、どれを上げるかわからないので、できない.
                //とりあえず、一回殴ればSになるところまで引き上げる
                weaponLv = 250;
                Program.RAM.write_u8(weaponLvAddr, weaponLv);
            }

            uint maxHP;
            uint maxPower;
            uint maxSkill;
            uint maxSpeed;
            uint maxDef;
            uint maxRes;
            uint maxLuck;
            uint addMove;

            maxHP = Program.ROM.u8(romClassAddr + 19);
            uint move = Program.ROM.u8(romClassAddr + 18);
            if (move > 15)
            {
                addMove = 0;
            }
            else
            {
                addMove = 15 - move;
            }

            maxLuck = Program.ROM.u8(Program.ROM.RomInfo.max_luck_address());
            if (maxLuck < 10 || maxLuck >= 100)
            {//不明な値、とりあえず運の最大値は30にする.
                maxLuck = 30;
            }

            if (promoted)
            {//上級職なのでクラスの設定上限まで引き上げる
                maxPower = Program.ROM.u8(romClassAddr + 20);
                maxSkill = Program.ROM.u8(romClassAddr + 21);
                maxSpeed = Program.ROM.u8(romClassAddr + 22);
                maxDef = Program.ROM.u8(romClassAddr + 23);
                maxRes = Program.ROM.u8(romClassAddr + 24);
            }
            else
            {//下級職なので20までかな
                maxPower = 20;
                maxSkill = 20;
                maxSpeed = 20;
                maxDef = 20;
                maxRes = 20;
            }
            if (Program.ROM.RomInfo.version() == 6)
            {
                Program.RAM.write_u8(ramUnitAddress + 0x10, maxHP);
                Program.RAM.write_u8(ramUnitAddress + 0x11, maxHP);
                Program.RAM.write_u8(ramUnitAddress + 0x12, maxPower);
                Program.RAM.write_u8(ramUnitAddress + 0x13, maxSkill);
                Program.RAM.write_u8(ramUnitAddress + 0x14, maxSpeed);
                Program.RAM.write_u8(ramUnitAddress + 0x15, maxDef);
                Program.RAM.write_u8(ramUnitAddress + 0x16, maxRes);
                Program.RAM.write_u8(ramUnitAddress + 0x17, maxLuck);
                if (growMovePower)
                {
                    Program.RAM.write_u8(ramUnitAddress + 0x1A, addMove);
                }
            }
            else
            {
                Program.RAM.write_u8(ramUnitAddress + 0x12, maxHP);
                Program.RAM.write_u8(ramUnitAddress + 0x13, maxHP);
                Program.RAM.write_u8(ramUnitAddress + 0x14, maxPower);
                Program.RAM.write_u8(ramUnitAddress + 0x15, maxSkill);
                Program.RAM.write_u8(ramUnitAddress + 0x16, maxSpeed);
                Program.RAM.write_u8(ramUnitAddress + 0x17, maxDef);
                Program.RAM.write_u8(ramUnitAddress + 0x18, maxRes);
                Program.RAM.write_u8(ramUnitAddress + 0x19, maxLuck);
                if (growMovePower)
                {
                    Program.RAM.write_u8(ramUnitAddress + 0x1D, addMove);
                }

                //魔法分離パッチ
                MagicSplitUtil.magic_split_enum magic_split = MagicSplitUtil.SearchMagicSplit();
                if (magic_split != MagicSplitUtil.magic_split_enum.NO)
                {
                    uint cid = Program.ROM.u8(romClassAddr + 4);
                    uint maxMagic = 20;
                    if (promoted)
                    {
                        maxMagic = MagicSplitUtil.GetClassLimitMagicExtends(cid, romClassAddr);
                    }

                    if (magic_split == MagicSplitUtil.magic_split_enum.FE8NMAGIC)
                    {
                        Program.RAM.write_u8(ramUnitAddress + 0x1A, maxMagic);
                    }
                    else if (magic_split == MagicSplitUtil.magic_split_enum.FE7UMAGIC
                        || magic_split == MagicSplitUtil.magic_split_enum.FE8UMAGIC)
                    {
                        Program.RAM.write_u8(ramUnitAddress + 0x3A, maxMagic);
                    }
                }

            }

            if (showNotify)
            {
                InputFormRef.ShowWriteNotifyAnimation(this, ramUnitAddress);
            }
        }

        bool CheckUnitSelectAndError()
        {
            if (!U.is_02RAMPointer(CurrentControlUnitRAMAddress))
            {
                R.ShowStopError("エミュレータでユニットを選択していません。Aボタンを押してユニットの移動範囲が表示されている状態にしてください。");
                return false;
            }
            return true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer1.Enabled == false)
            {
                return;
            }
            UpdateALL();
        }

        private void CurrentTextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string text = this.CurrentTextBox.Text2;
            if (text == "")
            {
                return;
            }
            TextForm f = (TextForm)InputFormRef.JumpForm<TextForm>();
            f.JumpToSearch(text);
        }



        void UpdateParty()
        {
            byte[] bin = Program.RAM.getBinaryData(
                  GetShowRAMPartyUnitsAddr()
                , GetLimitRAMPartyUnits() * RAMUnitSizeOf);
            uint sum = U.CalcCheckSUM(bin);
            if (sum == this.UnitCheckSUM)
            {//変更なし
                return;
            }

            //変更有
            this.UnitCheckSUM = sum;
            bool isDetail = Party_ControlPanel.Visible;

            uint i = 0;
            uint limit = GetLimitRAMPartyUnits();
            uint lastZeroPoint = 0;
            uint addr = 0;
            for (; i < limit; i++, addr += RAMUnitSizeOf)
            {
                uint unitPointer = U.u32(bin, addr);
                if (unitPointer == 0)
                {
                    continue;
                }
                if (!U.isSafetyPointer(unitPointer))
                {
                    break;
                }
                lastZeroPoint = i + 1;
            }

            this.PartyListBox.DummyAlloc((int)lastZeroPoint, this.PartyListBox.SelectedIndex);
            this.PartyCount.Text = R._("合計: {0}人", lastZeroPoint);

            if (isDetail)
            {
                ShowPartyFloatingControlpanel();
            }
        }

        
        //Uint + テキスト (PARSER) + Class テキストを書くルーチン
        Size DrawParty(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            Rectangle bounds = listbounds;

            int lineHeight = ListBoxEx.OWNER_DRAW_ICON_SIZE;

            uint addr = GetShowRAMPartyUnitsAddr() + (uint)index * 72;
            uint unitPointer =  Program.RAM.u32(addr + 0);
            uint classPointer = Program.RAM.u32(addr + 4);

            if (unitPointer == 0)
            {
                bounds.X += ListBoxEx.OWNER_DRAW_ICON_SIZE;
                bounds.X += U.DrawText(R._("-Empty-"), g, this.BoldFont, this.ListBoxForeKeywordBrush, isWithDraw, bounds);
                bounds.Y += lineHeight;
                return new Size(bounds.X, bounds.Y);
            }

            if (!U.isSafetyPointer(unitPointer))
            {
                bounds.X += ListBoxEx.OWNER_DRAW_ICON_SIZE;
                bounds.X += U.DrawText(R._("破損:{0}", U.ToHexString8(unitPointer)), g, this.BoldFont, this.ListBoxForeKeywordBrush, isWithDraw, bounds);
            }
            else
            {
                uint unitid = Program.ROM.u8(U.toOffset(unitPointer) + 4);
                Bitmap bitmap = UnitForm.DrawUnitMapFacePicture(unitid);
                U.MakeTransparent(bitmap);

                //アイコンを描く. 処理速度を稼ぐためにマップアイコンの方を描画
                Rectangle b = bounds;
                b.Width = ListBoxEx.OWNER_DRAW_ICON_SIZE;
                b.Height = ListBoxEx.OWNER_DRAW_ICON_SIZE;
                bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
                bitmap.Dispose();

                bounds.X += U.DrawText(U.ToHexString(unitid), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
                bounds.X += 4;
                bounds.X += U.DrawText(UnitForm.GetUnitName(unitid), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            }
            bounds.X += U.DrawText(" : ", g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);

            if (!U.isSafetyPointer(classPointer))
            {
                bounds.X += ListBoxEx.OWNER_DRAW_ICON_SIZE;
                bounds.X += U.DrawText(R._("破損:{0}", U.ToHexString8(classPointer)), g, this.BoldFont, this.ListBoxForeKeywordBrush, isWithDraw, bounds);
            }
            else
            {
                uint classid = Program.ROM.u8(U.toOffset(classPointer) + 4);
                int palette_type = GetShowPartyClassPaletteType();
                Bitmap bitmap = ClassForm.DrawWaitIcon(classid, palette_type);
                U.MakeTransparent(bitmap);

                //アイコンを描く.
                Rectangle b = bounds;
                b.Width = ListBoxEx.OWNER_DRAW_ICON_SIZE;
                b.Height = ListBoxEx.OWNER_DRAW_ICON_SIZE;
                bounds.X += U.DrawPicture(bitmap, g, isWithDraw, b);
                bitmap.Dispose();

                bounds.X += U.DrawText(U.ToHexString(classid), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
                bounds.X += 4;
                bounds.X += U.DrawText(ClassForm.GetClassName(classid), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            }

            uint state;
            if (Program.ROM.RomInfo.version() == 6)
            {//状態1,2だけ
                state = Program.RAM.u32(addr + 0x0C) << 16;
            }
            else
            {//状態1,2,3,4
                state = Program.RAM.u32(addr + 0x0C);
            }
            string stateString = InputFormRef.GetRAM_UNIT_STATE(state);
            if (stateString != "")
            {
                bounds.X += U.DrawText("[", g, this.BoldFont, this.ListBoxForeBrush, isWithDraw, bounds);
                bounds.X += U.DrawText(stateString, g, lb.Font, this.ListBoxForeKeywordBrush, isWithDraw, bounds);
                bounds.X += U.DrawText("]", g, this.BoldFont, this.ListBoxForeBrush, isWithDraw, bounds);
            }

            bounds.Y += lineHeight;
            return new Size(bounds.X, bounds.Y);
        }

        private void PartyListBox_DoubleClick(object sender, EventArgs e)
        {
            int index = this.PartyListBox.SelectedIndex;
            if (index < 0)
            {
                return ;
            }

            this.CurrentControlUnitRAMAddress = Program.ROM.RomInfo.workmemory_player_units_address() + (uint)index * 72;
        }

        private void PartyListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Party_ControlPanel.Hide();
        }

        private void PartyListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowPartyFloatingControlpanel();
        }

        private void PartyListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ShowPartyFloatingControlpanel();
            }
        }
        void HidePartyFloatingControlpanel()
        {
            Party_ControlPanel.Hide();
        }
        void ShowPartyFloatingControlpanel()
        {
            int index = this.PartyListBox.SelectedIndex;
            if (index < 0)
            {
                return;
            }

            uint baseaddr = GetShowRAMPartyUnitsAddr();
            uint addr = baseaddr + (uint)index * 72;
            uint unitPointer = U.toOffset( Program.RAM.u32(addr + 0));
            uint classPointer = U.toOffset( Program.RAM.u32(addr + 4));

            string unitname = "";
            string classname = "";
            Bitmap faceImage = null;
            if (U.isSafetyOffset(unitPointer))
            {
                uint unitid = Program.ROM.u8(U.toOffset(unitPointer) + 4);
                unitname = U.ToHexString(unitid) + " " + UnitForm.GetUnitName(unitid);
                faceImage = UnitForm.DrawUnitFacePicture(unitid);
            }
            if (U.isSafetyOffset(classPointer))
            {
                uint classid = Program.ROM.u8(U.toOffset(classPointer) + 4);
                classname = U.ToHexString(classid) + " " + ClassForm.GetClassName(classid);
            }

            PARTY_Address.Value = addr;
            PARTY_SelectAddress.Text = "";
            PARTY_ROMUNITPOINTER.Text = unitname;
            PARTY_ROMCLASSPOINTER.Text = classname;
            PARTY_RAMUNITSTATE.Text = EmulatorMemoryUtil.GetRAMUnitStatus(addr);
            PARTY_RAMUNITAID.Text = EmulatorMemoryUtil.GetAIDUnitString(addr);
            PARTY_PORTRAIT.Image = faceImage;

            this.PARTY_InputFormRef.ReInit(addr, 1);
            PARTY_AI1_TEXT.Text = EventUnitForm.GetAIName1((uint)PARTY_B66.Value);
            PARTY_AI2_TEXT.Text = EventUnitForm.GetAIName2((uint)PARTY_B68.Value);

            Party_ControlPanel.Show();
        }


        uint GetShowRAMPartyUnitsAddr()
        {
            int partyComboSelectedIndex = this.PartyCombo.SelectedIndex;
            if (partyComboSelectedIndex == 1)
            {
                return Program.ROM.RomInfo.workmemory_npc_units_address();
            }
            if (partyComboSelectedIndex == 2)
            {
                return Program.ROM.RomInfo.workmemory_enemy_units_address();
            }

            return Program.ROM.RomInfo.workmemory_player_units_address();
        }
        int GetShowPartyClassPaletteType()
        {
            int partyComboSelectedIndex = this.PartyCombo.SelectedIndex;
            if (partyComboSelectedIndex == 1)
            {
                return 1;
            }
            if (partyComboSelectedIndex == 2)
            {
                return 2;
            }
            return 0;
        }
        uint GetLimitRAMPartyUnits()
        {
            int partyComboSelectedIndex = this.PartyCombo.SelectedIndex;
            if (partyComboSelectedIndex == 1)
            {
                return 20;
            }
            if (partyComboSelectedIndex == 2)
            {
                return 50;
            }
            return 62;
        }

        private void Party_CloseButton_Click(object sender, EventArgs e)
        {
            Party_ControlPanel.Hide();
        }

        bool IsAutoSpeech = false;
        private void SpeechButton_Click(object sender, EventArgs e)
        {
            IsAutoSpeech = TextToSpeechForm.OptionTextToSpeech(CurrentTextBox.Text2, true);
        }
        void SetSpeechIcon()
        {
            U.SetIcon(SpeechButton, Properties.Resources.icon_speaker);
        }
        void Speech(string text)
        {
            if (!IsAutoSpeech)
            {
                return;
            }
            text = TextForm.StripAllCode(text);
            TextToSpeechForm.Speak(text);
        }

        bool IsSubtileSpeech = false;
        private void SubtileButton_Click(object sender, EventArgs e)
        {
            IsSubtileSpeech = ToolSubtitleSetingDialogForm.OptionTextSubtile();
        }
        void SetSubtileIcon()
        {
            U.SetIcon(SubtileButton, Properties.Resources.icon_translate_subtile);
        }
        void Subtile()
        {
            if (!IsSubtileSpeech)
            {
                return;
            }
            FETextDecode decoder = new FETextDecode();
            string lowText = this.LastStringText;
            string nextString = decoder.UnHffmanPatchDecodeLow(U.subrange(this.LastStringByte, this.NextStringOffset, (uint)this.LastStringByte.Length));

            int endPoint = lowText.IndexOf(nextString);
            if (endPoint <= 0)
            {
                endPoint = lowText.Length;
            }

            string keyword = "@0003";
            int startPoint;
            //バッファの位置は少しずれるみたいなので、@0003 / [A]の位置に戻す.
            int newEndPoint = lowText.LastIndexOf(keyword, endPoint);
            if (newEndPoint <= 0)
            {
                startPoint = lowText.LastIndexOf(keyword, endPoint);
            }
            else
            {
                startPoint = lowText.LastIndexOf(keyword, newEndPoint);
                endPoint = newEndPoint + keyword.Length;
            }

            if (startPoint < 0)
            {
                startPoint = 0;
            }
            else
            {
                startPoint += keyword.Length;
            }

            ToolSubtitleSetingDialogForm.ShowSubtile(lowText,startPoint, endPoint);
        }
        void SetExplain()
        {
            this.SpeechButton.AccessibleDescription = R._("Windowsのテキスト読み上げ機能を利用して、文章を合成音声で読み上げます。\r\n利用するには、OSに合成音声ライブラリがインストールされている必要があります。");
            this.SubtileButton.AccessibleDescription = R._("翻訳リソースを利用して、翻訳した字幕を表示します。");
        }
        void FE6ize()
        {
            if (Program.ROM.RomInfo.version() != 6)
            {
                return;
            }
            this.PARTY_B58.Name = "PARTY_NONE58";
            this.PARTY_B58.Hide();
            this.PARTY_B57.Name = "PARTY_B58";
            this.PARTY_B56.Name = "PARTY_B54";
            this.PARTY_B55.Name = "PARTY_B53";
            this.PARTY_B54.Name = "PARTY_B52";
            this.PARTY_B53.Name = "PARTY_B51";
            this.PARTY_B52.Name = "PARTY_B50";
            this.PARTY_B51.Name = "PARTY_B49";
            this.PARTY_B50.Name = "PARTY_B48";
            this.PARTY_B49.Name = "PARTY_B47";
            this.PARTY_B48.Name = "PARTY_B46";
            this.PARTY_B47.Name = "PARTY_B45";
            this.PARTY_B46.Name = "PARTY_B44";
            this.PARTY_B45.Name = "PARTY_B43";
            this.PARTY_B44.Name = "PARTY_B42";
            this.PARTY_B43.Name = "PARTY_B41";
            this.PARTY_B42.Name = "PARTY_B40";
            this.PARTY_B41.Name = "PARTY_B39";
            this.PARTY_B40.Name = "PARTY_B38";
            this.PARTY_B39.Name = "PARTY_B37";
            this.PARTY_B38.Name = "PARTY_B36";
            this.PARTY_B37.Name = "PARTY_B35";
            this.PARTY_B36.Name = "PARTY_B34";
            this.PARTY_B35.Name = "PARTY_B33";
            this.PARTY_B34.Name = "PARTY_B32";
            this.PARTY_B33.Name = "PARTY_B31";
            this.PARTY_B32.Name = "PARTY_B30";
            this.PARTY_B31.Name = "PARTY_B29";
            this.PARTY_B30.Name = "PARTY_B28";
            this.PARTY_B29.Name = "PARTY_B27";
            this.PARTY_B28.Name = "PARTY_B26";
            this.PARTY_B27.Name = "PARTY_B25";
            this.PARTY_B26.Name = "PARTY_B24";
            this.PARTY_B25.Name = "PARTY_B23";
            this.PARTY_B24.Name = "PARTY_B22";
            this.PARTY_B23.Name = "PARTY_B21";
            this.PARTY_B22.Name = "PARTY_B20";
            this.PARTY_B21.Name = "PARTY_B19";
            this.PARTY_B20.Name = "PARTY_B18";
            this.PARTY_B19.Name = "PARTY_B17";
            this.PARTY_B18.Name = "PARTY_B16";
            this.PARTY_B17.Name = "PARTY_B15";
            this.PARTY_B16.Name = "PARTY_B14";
            this.PARTY_D12.Name = "PARTY_W12";
            this.PARTY_L_30_ITEM.Name = "PARTY_L_28_ITEM";
            this.PARTY_L_30_ITEMICON.Name = "PARTY_L_28_ITEMICON";
            this.PARTY_L_32_ITEM.Name = "PARTY_L_30_ITEM";
            this.PARTY_L_32_ITEMICON.Name = "PARTY_L_30_ITEMICON";
            this.PARTY_L_34_ITEM.Name = "PARTY_L_32_ITEM";
            this.PARTY_L_34_ITEMICON.Name = "PARTY_L_32_ITEMICON";
            this.PARTY_L_36_ITEM.Name = "PARTY_L_34_ITEM";
            this.PARTY_L_36_ITEMICON.Name = "PARTY_L_34_ITEMICON";
            this.PARTY_L_38_ITEM.Name = "PARTY_L_36_ITEM";
            this.PARTY_L_38_ITEMICON.Name = "PARTY_L_36_ITEMICON";
        }


        private void CHEAT_WEATHER_Click(object sender, EventArgs e)
        {
            if (!CheckConnectShowError())
            {
                return;
            }
            if (Program.ROM.RomInfo.version() <= 6)
            {
                return;
            }
            uint stageStructAddr = Program.ROM.RomInfo.workmemory_chapterdata_address();
            uint writeRAMPointer = stageStructAddr + 0x15;
            Program.RAM.write_u8(writeRAMPointer, (uint)CHEAT_WEATHER_VALUE.Value);

            InputFormRef.ShowWriteNotifyAnimation(this, writeRAMPointer);
        }


        void MultiUnitsGrow(uint addr, uint limit, bool growMovePower)
        {
            for (uint i = 0; i < limit; i++, addr += RAMUnitSizeOf)
            {
                uint unitPointer = Program.RAM.u32(addr);
                if (unitPointer == 0)
                {
                    continue;
                }
                if (!U.isSafetyPointer(unitPointer))
                {
                    break;
                }

                CheatUnitGrow(addr, growMovePower , showNotify: false);
            }
        }

        private void CHEAT_ALL_PLAYER_UNIT_GROW_Click(object sender, EventArgs e)
        {
            uint limit = 62;
            uint addr = Program.ROM.RomInfo.workmemory_player_units_address();
            MultiUnitsGrow(addr, limit, growMovePower: true);
            InputFormRef.ShowWriteNotifyAnimation(this , 0);
        }

        private void CHEAT_ALL_UNIT_GROW_Click(object sender, EventArgs e)
        {
            uint limit = 62;
            uint addr = Program.ROM.RomInfo.workmemory_player_units_address();
            MultiUnitsGrow(addr, limit, growMovePower: true);

            //敵軍  移動力は上げない
            addr = Program.ROM.RomInfo.workmemory_enemy_units_address();
            MultiUnitsGrow(addr, limit, growMovePower: false);

            //NPC  移動力は上げない
            limit = 0x20;
            addr = Program.ROM.RomInfo.workmemory_npc_units_address();
            MultiUnitsGrow(addr, limit, growMovePower: false);

            InputFormRef.ShowWriteNotifyAnimation(this, 0);
        }

        private void CHEAT_ALL_ENEMY_UNIT_HP_1_Click(object sender, EventArgs e)
        {
            uint addr = Program.ROM.RomInfo.workmemory_enemy_units_address();
            uint limit = 50;
            for (uint i = 0; i < limit; i++, addr += RAMUnitSizeOf)
            {
                uint unitPointer = Program.RAM.u32(addr);
                if (unitPointer == 0)
                {
                    continue;
                }
                if (!U.isSafetyPointer(unitPointer))
                {
                    break;
                }

                UpdateUnitHP1(addr, showNotify: true);
            }
            InputFormRef.ShowWriteNotifyAnimation(this, 0);
        }
        private void CHEAT_ALL_ENEMY_DO_NOT_MOVE_Click(object sender, EventArgs e)
        {
            uint addr = Program.ROM.RomInfo.workmemory_enemy_units_address();
            uint limit = 50;
            for (uint i = 0; i < limit; i++, addr += RAMUnitSizeOf)
            {
                uint unitPointer = Program.RAM.u32(addr);
                if (unitPointer == 0)
                {
                    continue;
                }
                if (!U.isSafetyPointer(unitPointer))
                {
                    break;
                }

                UpdateUnitAIToDoNotMOVE(addr, showNotify: true);
            }
            InputFormRef.ShowWriteNotifyAnimation(this, 0);
        }

        void InitPaletteEtc()
        {
            if (Program.ROM.RomInfo.version() < 8)
            {
                X_ETC_WorldmapNode_Text.Hide();
            }
            this.Edition = U.NOT_FOUND;
            this.Diffeclty = U.NOT_FOUND;
            this.WorldmapNode = U.NOT_FOUND;
            this.PaletteList.OwnerDraw(DrawPalette, DrawMode.OwnerDrawFixed, false);
            this.PaletteList.ItemHeight = 12;
            this.PaletteList.DummyAlloc(32, 0);
            InputFormRef.AppendEvent_CopyAddressToDoubleClick(this.PaletteAddress);

            this.SongWorkingRAMs = new uint[]{
                Program.ROM.RomInfo.workmemory_sound_player_00_address()
                ,Program.ROM.RomInfo.workmemory_sound_player_01_address()
                ,Program.ROM.RomInfo.workmemory_sound_player_02_address()
                ,Program.ROM.RomInfo.workmemory_sound_player_03_address()
                ,Program.ROM.RomInfo.workmemory_sound_player_04_address()
                ,Program.ROM.RomInfo.workmemory_sound_player_05_address()
                ,Program.ROM.RomInfo.workmemory_sound_player_06_address()
                ,Program.ROM.RomInfo.workmemory_sound_player_07_address()
                ,Program.ROM.RomInfo.workmemory_sound_player_08_address()
            };
            this.SongIDBuffer = new uint[this.SongWorkingRAMs.Length];
            this.SoundList.OwnerDraw(DrawSongList, DrawMode.OwnerDrawFixed, false);
            this.SoundList.ItemHeight = 12;
            this.SoundList.DummyAlloc(this.SongWorkingRAMs.Length, 0);
            InputFormRef.AppendEvent_CopyAddressToDoubleClick(this.SoundAddress);

            this.TrapList.OwnerDraw(DrawTrapList, DrawMode.OwnerDrawFixed, false);
            this.TrapList.ItemHeight = 12;
            this.TrapList.DummyAlloc(0x40, 0);
            InputFormRef.AppendEvent_CopyAddressToDoubleClick(this.TrapAddress);

            this.BWLList.OwnerDraw(DrawBWLList, DrawMode.OwnerDrawFixed, false);
            this.BWLList.ItemHeight = 16;
            this.BWLList.DummyAlloc(0x45, 0);
            InputFormRef.AppendEvent_CopyAddressToDoubleClick(this.BWLAddress);

            this.ClearTurnList.OwnerDraw(DrawClearTurnList, DrawMode.OwnerDrawFixed, false);
            this.ClearTurnList.ItemHeight = 12;
            if (PatchUtil.SearchCache_ClearTurn2x() == PatchUtil.ClearTurn2x_extends._2x)
            {
                this.ClearTurnList.DummyAlloc((int)Program.ROM.RomInfo.workmemory_clear_turn_count() * 2, 0);
            }
            else
            {
                this.ClearTurnList.DummyAlloc((int)Program.ROM.RomInfo.workmemory_clear_turn_count(), 0);
            }
            InputFormRef.AppendEvent_CopyAddressToDoubleClick(this.ClearTurnList);

            this.ChapterDataList.OwnerDraw(DrawChapterDataList, DrawMode.OwnerDrawFixed, false);
            this.ChapterDataList.ItemHeight = 12;
            this.ChapterDataList.DummyAlloc(ChapterDataStruct.Count, 0);
            InputFormRef.AppendEvent_CopyAddressToDoubleClick(this.ChapterDataAddress);

            this.BattleActorList.OwnerDraw(DrawBattleUnitBattleActorList, DrawMode.OwnerDrawFixed, false);
            this.BattleActorList.ItemHeight = 12;
            this.BattleActorList.DummyAlloc(BattleUnitStruct.Count, 0);
            InputFormRef.AppendEvent_CopyAddressToDoubleClick(this.BattleActorAddress);

            this.BattleTargetList.OwnerDraw(DrawBattleUnitBattleTargetList, DrawMode.OwnerDrawFixed, false);
            this.BattleTargetList.ItemHeight = 12;
            this.BattleTargetList.DummyAlloc(BattleUnitStruct.Count, 0);
            InputFormRef.AppendEvent_CopyAddressToDoubleClick(this.BattleTargetAddress);

            if (Program.ROM.RomInfo.version() == 8)
            {
                this.WorldmapList.OwnerDraw(DrawWorldmapList, DrawMode.OwnerDrawFixed, false);
                this.WorldmapList.ItemHeight = 12;
                this.WorldmapList.DummyAlloc(WorldmapStruct.Count, 0);
                InputFormRef.AppendEvent_CopyAddressToDoubleClick(this.WorldmapAddress);

                this.DungeonDataList.OwnerDraw(DrawDungeonDataList, DrawMode.OwnerDrawFixed, false);
                this.DungeonDataList.ItemHeight = 12;
                this.DungeonDataList.DummyAlloc(DungeonDataStruct.Count, 0);
                InputFormRef.AppendEvent_CopyAddressToDoubleClick(this.DungeonDataAddress);

                this.BattleSomeDataList.OwnerDraw(DrawBattleSomeDataList, DrawMode.OwnerDrawFixed, false);
                this.BattleSomeDataList.ItemHeight = 12;
                this.BattleSomeDataList.DummyAlloc(BattleSomeDataStruct.Count, 0);
                InputFormRef.AppendEvent_CopyAddressToDoubleClick(this.BattleSomeDataAddress);
            }
            else
            {
                tabControlEtc.TabPages.Remove(tabPageBattleSome);
                tabControlEtc.TabPages.Remove(tabPageWorldmap);
                tabControlEtc.TabPages.Remove(tabPageDungeon);
            }

            this.ArenaDataList.OwnerDraw(DrawArenaDataList, DrawMode.OwnerDrawFixed, false);
            this.ArenaDataList.ItemHeight = 12;
            this.ArenaDataList.DummyAlloc(ArenaDataStruct.Count, 0);
            InputFormRef.AppendEvent_CopyAddressToDoubleClick(this.ArenaDataAddress);

            this.AIDataList.OwnerDraw(DrawAIDataList, DrawMode.OwnerDrawFixed, false);
            this.AIDataList.ItemHeight = 12;
            this.AIDataList.DummyAlloc(AIDataStruct.Count, 0);
            InputFormRef.AppendEvent_CopyAddressToDoubleClick(this.AIDataAddress);

            this.ActionDataList.OwnerDraw(DrawActionDataList, DrawMode.OwnerDrawFixed, false);
            this.ActionDataList.ItemHeight = 12;
            this.ActionDataList.DummyAlloc(ActionDataStruct.Count, 0);
            InputFormRef.AppendEvent_CopyAddressToDoubleClick(this.ActionDataAddress);

            InitSupplyRAMAddress();
            this.SupplyDataList.OwnerDraw(DrawSupplyDataList, DrawMode.OwnerDrawFixed, false);
            this.SupplyDataList.ItemHeight = 16;
            this.SupplyDataList.DummyAlloc((int)this.SuppyMaxCount, 0);
            InputFormRef.AppendEvent_CopyAddressToDoubleClick(this.SupplyDataAddress);

        }

        void UpdateEditon()
        {
            {
                uint v = EmulatorMemoryUtil.GetEdition();
                if (v != this.Edition)
                {
                    this.Edition = v;
                    this.X_ETC_Edition_Text.Text = EmulatorMemoryUtil.ConvertEditionToString(v);
                    this.CHEAT_WARP_EDTION_VALUE.Value = EmulatorMemoryUtil.GetEdition();
                }
            }
            {
                uint v = EmulatorMemoryUtil.GetDiffecly();
                if (v != this.Diffeclty)
                {
                    this.Diffeclty = v;
                    this.X_ETC_Diffculty_Text.Text = EmulatorMemoryUtil.ConvertDiffeclyToString(v);
                }
            }
            if (Program.ROM.RomInfo.version() == 8)
            {
                uint v = EmulatorMemoryUtil.GetCurrentWorldmapNode();
                if (v != this.WorldmapNode)
                {
                    this.WorldmapNode = v;
                    this.X_ETC_WorldmapNode_Text.Text = EmulatorMemoryUtil.ConvertWorldmapNodeToString(v);
                }
            }
        }
        void UpdateSong()
        {
            bool isInvalidateSongList = false;
            for (int i = 0; i < this.SongWorkingRAMs.Length; i++)
            {
                uint p = this.SongWorkingRAMs[i];
                if (p == 0x0)
                {
                    continue;
                }
                uint v = Program.RAM.u32(p);
                if (v != this.SongIDBuffer[i])
                {
                    this.SongIDBuffer[i] = v;
                    isInvalidateSongList = true;
                }
            }
            if (isInvalidateSongList)
            {
                this.SoundList.Invalidate();
            }
        }
        void UpdateTrap()
        {
            byte[] bin = Program.RAM.getBinaryData(
                  Program.ROM.RomInfo.workmemory_trap_address()
                , 8 * 0x40);
            uint sum = U.CalcCheckSUM(bin);
            if (sum != this.TrapCheckSUM)
            {//変更有
                this.TrapCheckSUM = sum;
                this.TrapList.Invalidate();
            }
        }
        void UpdateBWL()
        {
            byte[] bin = Program.RAM.getBinaryData(
                  Program.ROM.RomInfo.workmemory_bwl_address() + 16
                , 16 * 0x45);
            uint sum = U.CalcCheckSUM(bin);
            if (sum != this.BWLCheckSUM)
            {//変更有
                this.BWLCheckSUM = sum;
                this.BWLList.Invalidate();
            }
        }
        void UpdateChapterData()
        {
            byte[] bin = Program.RAM.getBinaryData(
                  Program.ROM.RomInfo.workmemory_chapterdata_address()
                , Program.ROM.RomInfo.workmemory_chapterdata_size() );
            uint sum = U.CalcCheckSUM(bin);
            if (sum != this.ChapterDataCheckSUM)
            {//変更有
                this.ChapterDataCheckSUM = sum;
                this.ChapterDataList.Invalidate();
            }
        }
        void UpdateBattleActor()
        {
            byte[] bin = Program.RAM.getBinaryData(
                  Program.ROM.RomInfo.workmemory_battle_actor_address()
                , 0x80);
            uint sum = U.CalcCheckSUM(bin);
            if (sum != this.BattleActorSUM)
            {//変更有
                this.BattleActorSUM = sum;
                this.BattleActorList.Invalidate();
            }
        }
        void UpdateBattleTarget()
        {
            byte[] bin = Program.RAM.getBinaryData(
                  Program.ROM.RomInfo.workmemory_battle_target_address()
                , 0x80);
            uint sum = U.CalcCheckSUM(bin);
            if (sum != this.BattleTargetSUM)
            {//変更有
                this.BattleTargetSUM = sum;
                this.BattleTargetList.Invalidate();
            }
        }

        void UpdateClearTurn()
        {
            byte[] bin = Program.RAM.getBinaryData(
                  Program.ROM.RomInfo.workmemory_clear_turn_address()
                , 4 * Program.ROM.RomInfo.workmemory_clear_turn_count());
            uint sum = U.CalcCheckSUM(bin);
            if (sum != this.ClearTurnSUM)
            {//変更有
                this.ClearTurnSUM = sum;
                this.ClearTurnList.Invalidate();
            }
        }
        void UpdatePalette()
        {
            byte[] bin = Program.RAM.getBinaryData(
                  Program.ROM.RomInfo.workmemory_palette_address()
                , 2 * 16 * 16 * 2);
            uint sum = U.CalcCheckSUM(bin);
            if (sum != this.PaletteCheckSUM)
            {//変更有
                this.PaletteCheckSUM = sum;
                this.PaletteList.Invalidate();
            }
        }
        void UpdateWorldmap()
        {
            byte[] bin = Program.RAM.getBinaryData(
                  Program.ROM.RomInfo.workmemory_worldmap_data_address()
                , Program.ROM.RomInfo.workmemory_worldmap_data_size());
            uint sum = U.CalcCheckSUM(bin);
            if (sum != this.WorldmapSUM)
            {//変更有
                this.WorldmapSUM = sum;
                this.WorldmapList.Invalidate();
            }
        }
        void UpdateArenaData()
        {
            byte[] bin = Program.RAM.getBinaryData(
                  Program.ROM.RomInfo.workmemory_arena_data_address()
                , 0x68);
            uint sum = U.CalcCheckSUM(bin);
            if (sum != this.ArenaDataSUM)
            {//変更有
                this.ArenaDataSUM = sum;
                this.ArenaDataList.Invalidate();
            }
        }
        void UpdateAIData()
        {
            byte[] bin = Program.RAM.getBinaryData(
                  Program.ROM.RomInfo.workmemory_ai_data_address()
                , 0x100);
            uint sum = U.CalcCheckSUM(bin);
            if (sum != this.AIDataSUM)
            {//変更有
                this.AIDataSUM = sum;
                this.AIDataList.Invalidate();
            }
        }

        uint SuppyRAMAddress;
        uint SuppyMaxCount;
        void InitSupplyRAMAddress()
        {
            uint addr = Program.ROM.RomInfo.supply_pointer_address();
            SuppyRAMAddress = Program.ROM.u32(addr);
            SuppyMaxCount = Program.ROM.u16(addr + 4);
        }

        void UpdateSupplyData()
        {
            byte[] bin = Program.RAM.getBinaryData(
                  SuppyRAMAddress
                , SuppyMaxCount);
            uint sum = U.CalcCheckSUM(bin);
            if (sum != this.SupplyDataSUM)
            {//変更有
                this.SupplyDataSUM = sum;
                this.SupplyDataList.Invalidate();
            }
        }
        void UpdateActionData()
        {
            byte[] bin = Program.RAM.getBinaryData(
                  Program.ROM.RomInfo.workmemory_action_data_address()
                , 0x20);
            uint sum = U.CalcCheckSUM(bin);
            if (sum != this.ActionDataSUM)
            {//変更有
                this.ActionDataSUM = sum;
                this.ActionDataList.Invalidate();
            }
        }
        void UpdateDungeonData()
        {
            byte[] bin = Program.RAM.getBinaryData(
                  Program.ROM.RomInfo.workmemory_dungeon_data_address()
                , 0x20);
            uint sum = U.CalcCheckSUM(bin);
            if (sum != this.DungeonDataSUM)
            {//変更有
                this.DungeonDataSUM = sum;
                this.DungeonDataList.Invalidate();
            }
        }
        void UpdateBattleSomeData()
        {
            byte[] bin = Program.RAM.getBinaryData(
                  Program.ROM.RomInfo.workmemory_battlesome_data_address()
                , 0x100);
            uint sum = U.CalcCheckSUM(bin);
            if (sum != this.BattleSomeDataSUM)
            {//変更有
                this.BattleSomeDataSUM = sum;
                this.BattleSomeDataList.Invalidate();
            }
        }
        
        
        Size DrawPalette(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            Rectangle bounds = listbounds;

            string name;
            if (index >= 16)
            {
                name = "OGP"+(index-16).ToString("X");
            }
            else
            {
                name = "BGP"+index.ToString("X");
            }
            U.DrawText(name, g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            int namelength = 40;

            uint oneWidth = (uint)(listbounds.Width - namelength) / 16;
            uint lineHeight = 12;
            for (uint i = 0; i < 16; i++)
            {
                uint a = (uint)((i * 2) + (index * 16 * 2));
                uint addr = Program.ROM.RomInfo.workmemory_palette_address() + a;
                uint rgb = Program.RAM.u16(addr);
                Color c = ImageUtil.GBARGBToColor(rgb);
                Brush b = new SolidBrush(c);
                Rectangle rc = new Rectangle(
                        (int)(i * oneWidth + namelength)
                    , listbounds.Y
                    , (int)oneWidth
                    , (int)lineHeight);
                if (isWithDraw)
                {
                    g.FillRectangle(b, rc);
                }
            }

            bounds.X += listbounds.Width;
            bounds.Y += (int)lineHeight;
            return new Size(bounds.X, bounds.Y);
        }
        Size DrawSongList(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            Rectangle bounds = listbounds;
            uint lineHeight = 12;

            uint addr = SongWorkingRAMs[index];
            U.DrawText(U.ToHexString8(addr), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 60;

            U.DrawText("->", g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 20;

            addr = SongIDBuffer[index];
            U.DrawText(U.ToHexString8(addr), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 60;

            U.DrawText("->", g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 20;

            string name = SongTableForm.GetSongNameWhereSongHeader(addr);
            bounds.X += U.DrawText(name, g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);

            bounds.Y += (int)lineHeight;
            return new Size(bounds.X, bounds.Y);
        }
        Size DrawTrapList(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            Rectangle bounds = listbounds;
            uint lineHeight = 12;

            uint addr = Program.ROM.RomInfo.workmemory_trap_address() + ((uint)index * 0x8);
            uint x = Program.RAM.u8(addr + 0);
            uint y = Program.RAM.u8(addr + 1);
            uint trapID = Program.RAM.u8(addr + 2);

            U.DrawText(U.ToHexString8(addr), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 60;

            U.DrawText(InputFormRef.GetTrapName(trapID), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 90;

            if (trapID == 0)
            {
                bounds.Y += (int)lineHeight;
                return new Size(bounds.X, bounds.Y);
            }

            U.DrawText("X:" + x, g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 40;

            U.DrawText("Y:" + y, g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 40;

            uint ext1 = Program.RAM.u8(addr + 3);
            uint ext2 = Program.RAM.u8(addr + 4);
            uint ext3 = Program.RAM.u8(addr + 5);
            uint ext4 = Program.RAM.u8(addr + 6);
            uint ext5 = Program.RAM.u8(addr + 7);
            U.DrawText(U.ToHexString2(ext1), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 20;
            U.DrawText(U.ToHexString2(ext2), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 30;
            U.DrawText(U.ToHexString2(ext3), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 30;
            U.DrawText(U.ToHexString2(ext4), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 30;
            U.DrawText(U.ToHexString2(ext5), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 30;

            bounds.Y += (int)lineHeight;
            return new Size(bounds.X, bounds.Y);
        }

        Size DrawBWLList(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            index = index + 1;
            Rectangle bounds = listbounds;
            uint lineHeight = 16;

            uint addr = Program.ROM.RomInfo.workmemory_bwl_address() + ((uint)index * 16);
            U.DrawText(U.ToHexString8(addr), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 60;

            uint b = Program.RAM.u16(addr + 0xc);
            b = ((b << 0x12) >> 0x14);

            uint w = Program.RAM.u8(addr + 0xb);
            uint w_2 = Program.RAM.u8(addr + 0xc);
            w_2 = (w_2 & 0x3) << 0x8;
            w = w | w_2;

            uint l = Program.RAM.u8(addr + 0x0);

            U.DrawText("B:" + b.ToString(), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 35;

            U.DrawText("W:" + w.ToString(), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 35;

            U.DrawText("L:" + l.ToString(), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 25;

            
            uint uid = (uint)index;
            Bitmap icon = UnitForm.DrawUnitMapFacePicture(uid);
            U.MakeTransparent(icon);
            Rectangle bb = bounds;
            bb.Width = (int)lineHeight;
            bb.Height = (int)lineHeight;
            bounds.X += U.DrawPicture(icon, g, isWithDraw, bb);
            bounds.X += 2;
            icon.Dispose();

            U.DrawText(UnitForm.GetUnitNameWithID(uid), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 75;

            for (uint i = 0; i < 16; i++)
            {
                uint v;
                v = Program.RAM.u8(addr + i);
                U.DrawText(U.ToHexString2(v), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
                bounds.X += 16;

                i++;
                v = Program.RAM.u8(addr + i);
                U.DrawText(U.ToHexString2(v), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
                bounds.X += 20;
            }

            bounds.Y += (int)lineHeight;
            return new Size(bounds.X, bounds.Y);
        }
        Size DrawSupplyDataList(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            Rectangle bounds = listbounds;
            uint lineHeight = 16;

            uint addr = this.SuppyRAMAddress + ((uint)index * 2);
            U.DrawText(U.ToHexString8(addr), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 60;

            uint item = Program.RAM.u8(addr + 0x0);
            Bitmap icon = ItemForm.DrawIcon(item);
            U.MakeTransparent(icon);
            Rectangle bb = bounds;
            bb.Width = (int)lineHeight;
            bb.Height = (int)lineHeight;
            bounds.X += U.DrawPicture(icon, g, isWithDraw, bb);
            bounds.X += 2;
            icon.Dispose();

            string text = U.ToHexString2(item) + " " + ItemForm.GetItemName(item);
            U.DrawText(text, g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 90;

            uint count = Program.RAM.u8(addr + 0x1);
            U.DrawText(count.ToString() , g, lb.Font, this.ListBoxForeDecBrush, isWithDraw, bounds);
            bounds.X += 20;

            bounds.Y += (int)lineHeight;
            return new Size(bounds.X, bounds.Y);
        }

        Size DrawChapterDataList(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            return DrawAddressList(ChapterDataStruct, Program.ROM.RomInfo.workmemory_chapterdata_address(), lb, index, g, listbounds, isWithDraw);
        }
        Size DrawBattleUnitBattleActorList(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            return DrawAddressList(BattleUnitStruct, Program.ROM.RomInfo.workmemory_battle_actor_address(), lb, index, g, listbounds, isWithDraw);
        }
        Size DrawBattleUnitBattleTargetList(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            return DrawAddressList(BattleUnitStruct, Program.ROM.RomInfo.workmemory_battle_target_address(), lb, index, g, listbounds, isWithDraw);
        }
        Size DrawWorldmapList(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            return DrawAddressList(WorldmapStruct, Program.ROM.RomInfo.workmemory_worldmap_data_address(), lb, index, g, listbounds, isWithDraw , 5);
        }
        Size DrawArenaDataList(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            return DrawAddressList(ArenaDataStruct, Program.ROM.RomInfo.workmemory_arena_data_address(), lb, index, g, listbounds, isWithDraw);
        }
        Size DrawAIDataList(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            return DrawAddressList(AIDataStruct, Program.ROM.RomInfo.workmemory_ai_data_address(), lb, index, g, listbounds, isWithDraw);
        }
        Size DrawActionDataList(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            return DrawAddressList(ActionDataStruct, Program.ROM.RomInfo.workmemory_action_data_address(), lb, index, g, listbounds, isWithDraw);
        }
        Size DrawDungeonDataList(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            return DrawAddressList(DungeonDataStruct, Program.ROM.RomInfo.workmemory_dungeon_data_address(), lb, index, g, listbounds, isWithDraw);
        }
        Size DrawBattleSomeDataList(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            return DrawAddressList(BattleSomeDataStruct, Program.ROM.RomInfo.workmemory_battlesome_data_address(), lb, index, g, listbounds, isWithDraw , 20);
        }
       

        Size DrawAddressList(List<EmulatorMemoryUtil.AddressList> list, uint baseaddr, ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw, int shiftDrawX = 0)
        {
            if (index < 0 || index >= lb.Items.Count || index >= list.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            Rectangle bounds = listbounds;
            uint lineHeight = 12;

            EmulatorMemoryUtil.AddressList a = list[index];

            uint addr = baseaddr + a.Plus;

            if (a.Size == 0)
            {//Label
                U.DrawText(U.ToHexString8(addr), g, lb.Font, this.ListBoxForeKeywordBrush, isWithDraw, bounds);
                bounds.X += 60;
                U.DrawText(a.Name, g, lb.Font, this.ListBoxForeKeywordBrush, isWithDraw, bounds);
                bounds.X += 40;
                bounds.X += 120 + shiftDrawX;
            }
            else
            {//Value
                U.DrawText(U.ToHexString8(addr), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
                bounds.X += 60;

                U.DrawText("(+" + U.ToHexString2(a.Plus) + ")", g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
                bounds.X += 40;

                U.DrawText(a.Name, g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
                bounds.X += 120 + shiftDrawX;
            }


            string r = EmulatorMemoryUtil.GetAddressList(a, addr);
            if (a.Type == "DEC")
            {
                U.DrawText(r, g, lb.Font, this.ListBoxForeDecBrush, isWithDraw, bounds);
            }
            else
            {
                U.DrawText(r, g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            }

            bounds.Y += (int)lineHeight;
            return new Size(bounds.X, bounds.Y);
        }

        Size DrawClearTurnList(ListBox lb, int index, Graphics g, Rectangle listbounds, bool isWithDraw)
        {
            if (index < 0 || index >= lb.Items.Count)
            {
                return new Size(listbounds.X, listbounds.Y);
            }
            Rectangle bounds = listbounds;
            uint lineHeight = 12;

            uint addr;
            if (PatchUtil.SearchCache_ClearTurn2x() == PatchUtil.ClearTurn2x_extends._2x)
            {
                addr = Program.ROM.RomInfo.workmemory_clear_turn_address() + ((uint)index * 0x2);
            }
            else
            {
                addr = Program.ROM.RomInfo.workmemory_clear_turn_address() + ((uint)index * 0x4);
            }
            uint mix = Program.RAM.u16(addr + 0);

            uint turns;
            uint mapid;
            if (Program.ROM.RomInfo.version() == 6)
            {
                turns = mix >> 6;
                mapid = mix & 0x2f;
            }
            else
            {
                turns = mix >> 7;
                mapid = mix & 0x7f;
            }

            uint seconds = 0;
            if (PatchUtil.SearchCache_ClearTurn2x() == PatchUtil.ClearTurn2x_extends.NO)
            {
                seconds = Program.RAM.u16(addr + 2); 
            }

            U.DrawText(U.ToHexString8(addr), g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 60;

            if (mix == 0)
            {
                bounds.Y += (int)lineHeight;
                return new Size(bounds.X, bounds.Y);
            }
            string name = U.ToHexString(mapid) +" "+ MapSettingForm.GetMapName(mapid);
            U.DrawText(name, g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 200;

            U.DrawText(turns + " Turn", g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
            bounds.X += 80;

            if (PatchUtil.SearchCache_ClearTurn2x() == PatchUtil.ClearTurn2x_extends.NO)
            {
                U.DrawText(seconds + " Seconds", g, lb.Font, this.ListBoxForeBrush, isWithDraw, bounds);
                bounds.X += 80;
            }

            bounds.Y += (int)lineHeight;
            return new Size(bounds.X, bounds.Y);
        }


        uint Edition;
        uint Diffeclty;
        uint WorldmapNode;
        uint PaletteCheckSUM;
        uint[] SongIDBuffer;
        uint[] SongWorkingRAMs;
        uint UnitCheckSUM;
        uint TrapCheckSUM;
        uint BWLCheckSUM;
        uint ClearTurnSUM;
        uint ChapterDataCheckSUM;
        uint BattleActorSUM;
        uint BattleTargetSUM;
        uint WorldmapSUM;
        uint ArenaDataSUM;
        uint AIDataSUM;
        uint SupplyDataSUM;
        uint ActionDataSUM;
        uint DungeonDataSUM;
        uint BattleSomeDataSUM;

        private void PaletteSearchButton_Click(object sender, EventArgs e)
        {
            HexEditorForm hex = (HexEditorForm)InputFormRef.JumpForm<HexEditorForm>();
            hex.Search(SelectPalette.Text,isAlign4:true );
        }

        private void PaletteList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = PaletteList.SelectedIndex;
            if (index < 0 || index >= PaletteList.Items.Count)
            {
                SelectPalette.Text = "";
                return ;
            }
            StringBuilder sb = new StringBuilder();
            for (uint i = 0; i < 16; i++)
            {
                uint a = (uint)((i * 2) + (index * 16 * 2));
                uint addr_ = Program.ROM.RomInfo.workmemory_palette_address() + a;
                uint rgb = Program.RAM.u8(addr_);

                sb.Append(U.ToHexString(rgb));
                sb.Append(' ');

                addr_ = Program.ROM.RomInfo.workmemory_palette_address() + a + 1;
                rgb = Program.RAM.u8(addr_);
                sb.Append(U.ToHexString(rgb));
                sb.Append(' ');
            }
            SelectPalette.Text = sb.ToString();

            uint addr = Program.ROM.RomInfo.workmemory_palette_address() + (0x20 * (uint)index);
            PaletteAddress.Text = U.ToHexString(addr);
        }

        private void PaletteList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PaletteList_SelectedIndexChanged(null, null);
            PaletteSearchButton.PerformClick();
        }

        private void SoundList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = SoundList.SelectedIndex;
            if (index < 0 || index >= SoundList.Items.Count)
            {
                return;
            }
            uint addr = this.SongIDBuffer[index];
            string name = SongTableForm.GetSongNameWhereSongHeader(addr);
            uint song_id = U.atoh(name);
            if (song_id <= 0)
            {
                return;
            }
            InputFormRef.JumpForm<SongTableForm>(song_id);
        }

        private void TrapList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = TrapList.SelectedIndex;
            if (index < 0 || index >= TrapList.Items.Count)
            {
                return;
            }
            uint addr = Program.ROM.RomInfo.workmemory_trap_address() + ((uint)index * 0x8);
            uint x = Program.RAM.u8(addr + 0);
            uint y = Program.RAM.u8(addr + 1);
            uint trapID = Program.RAM.u8(addr + 2);

            uint ext1 = Program.RAM.u8(addr + 3);
            uint ext2 = Program.RAM.u8(addr + 4);
            uint ext3 = Program.RAM.u8(addr + 5);
            uint ext4 = Program.RAM.u8(addr + 6);
            uint ext5 = Program.RAM.u8(addr + 7);

            uint mapid = (uint)N_B14.Value;
            if (trapID == 0x03 || trapID == 0x06)
            {//ext1がマップ変化ID
                MapChangeForm f = (MapChangeForm)InputFormRef.JumpForm<MapChangeForm>();
                f.JumpToMAPIDAndID( mapid, ext1);
            }
        }
        private void ClearTurnList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = ClearTurnList.SelectedIndex;
            if (index < 0 || index >= ClearTurnList.Items.Count)
            {
                return;
            }
            uint addr;
            if (PatchUtil.SearchCache_ClearTurn2x() == PatchUtil.ClearTurn2x_extends._2x)
            {
                addr = Program.ROM.RomInfo.workmemory_clear_turn_address() + ((uint)index * 0x2);
            }
            else
            {
                addr = Program.ROM.RomInfo.workmemory_clear_turn_address() + ((uint)index * 0x4);
            }
            ClearTurnAddress.Text = U.ToHexString(addr);
        }



        private void PARTY_ROMUNITPOINTER_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            uint unit_id = U.atoh(PARTY_ROMUNITPOINTER.Text);
            InputFormRef.JumpTo(null, unit_id, "UNIT", new string[] { });
        }

        private void PARTY_ROMCLASSPOINTER_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            uint class_id = U.atoh(PARTY_ROMCLASSPOINTER.Text);
            InputFormRef.JumpTo(null, class_id, "CLASS", new string[] { });
        }

        private void PARTY_RAMUNITAID_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            uint unit_id = U.atoh(PARTY_RAMUNITAID.Text);
            InputFormRef.JumpTo(null, unit_id, "UNIT", new string[] { });
        }

        private void CHEAT_WARP_Click(object sender, EventArgs e)
        {
            if (Program.ROM.RomInfo.version() == 8)
            {
                EmulatorMemoryUtil.CHEAT_WARP_FE8(this, (uint)this.CHEAT_WARP_CHPATER_VALUE.Value, (uint)this.CHEAT_WARP_EDTION_VALUE.Value, (uint)this.CHEAT_WARP_NODE_VALUE.Value);
            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                EmulatorMemoryUtil.CHEAT_WARP_FE7(this, (uint)this.CHEAT_WARP_CHPATER_VALUE.Value, (uint)this.CHEAT_WARP_EDTION_VALUE.Value);
            }
            else if (Program.ROM.RomInfo.version() == 6)
            {
                EmulatorMemoryUtil.CHEAT_WARP_FE6(this, (uint)this.CHEAT_WARP_CHPATER_VALUE.Value);
            }
            else
            {
                R.ShowStopError("このバージョンでは、章ワープを利用できません。");
                return;
            }
        }

        private void N_B14_ValueChanged(object sender, EventArgs e)
        {
            uint next_chapter = (uint)N_B14.Value + 1;
            if (next_chapter <= 0xff)
            {
                CHEAT_WARP_CHPATER_VALUE.Value = next_chapter;
            }
        }

        private void CHEAT_WARP_CHPATER_VALUE_ValueChanged(object sender, EventArgs e)
        {
            if (Program.ROM.RomInfo.version() == 8)
            {
                CHEAT_WARP_NODE_VALUE.Value = EmulatorMemoryUtil.GetWorldmapNode((uint)CHEAT_WARP_CHPATER_VALUE.Value);
            }

        }

        private void TrapList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = TrapList.SelectedIndex;
            if (index < 0 || index >= TrapList.Items.Count)
            {
                TrapAddress.Text = "";
                return ;
            }
            uint addr = Program.ROM.RomInfo.workmemory_trap_address() + ((uint)index * 0x8);
            TrapAddress.Text = U.ToHexString(addr);
        }
        private void SoundList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = SoundList.SelectedIndex;
            if (index < 0 || index >= SongWorkingRAMs.Length)
            {
                SoundAddress.Text = "";
                return;
            }
            uint addr = this.SongWorkingRAMs[index];
            SoundAddress.Text = U.ToHexString(addr);
        }

        private void TrapList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TrapList_MouseDoubleClick(sender, null);
            }
        }

        private void SoundList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SoundList_MouseDoubleClick(sender, null);
            }
        }

        private void PaletteList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PaletteList_MouseDoubleClick(sender, null);
            }
        }

        private void CHEAT_TURN_Click(object sender, EventArgs e)
        {
            if (!CheckConnectShowError())
            {
                return;
            }
            uint stageStructAddr = Program.ROM.RomInfo.workmemory_chapterdata_address();
            uint writeRAMPointer = stageStructAddr + 0x10;
            Program.RAM.write_u8(writeRAMPointer, (uint)CHEAT_TURN_VALUE.Value);

            InputFormRef.ShowWriteNotifyAnimation(this, writeRAMPointer);
        }

        private void BWLList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = BWLList.SelectedIndex;
            if (index < 0 || index >= BWLList.Items.Count)
            {
                BWLAddress.Text = "";
                return;
            }
            uint addr = Program.ROM.RomInfo.workmemory_bwl_address() + 16 + ((uint)index * 16);
            BWLAddress.Text = U.ToHexString(addr);
        }

        private void BWLList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BWLList_SelectedIndexChanged(null, null);
            U.FireOnMouseDoubleClick(ClearTurnAddress);
        }

        static private void AddressList_SelectedIndexChanged(List<EmulatorMemoryUtil.AddressList> list,uint baseaddress, ListBox listbox,TextBox textbox)
        {
            int index = listbox.SelectedIndex;
            if (index < 0 || index >= listbox.Items.Count || index >= list.Count)
            {
                textbox.Text = "";
                return;
            }
            EmulatorMemoryUtil.AddressList a = list[index];
            uint addr = baseaddress + a.Plus;
            textbox.Text = U.ToHexString(addr);
        }


        List<EmulatorMemoryUtil.AddressList> ChapterDataStruct = EmulatorMemoryUtil.GetChapterDataStruct();
        private void ChapterDataList_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddressList_SelectedIndexChanged(ChapterDataStruct, Program.ROM.RomInfo.workmemory_chapterdata_address(), ChapterDataList, ChapterDataAddress);
        }

        List<EmulatorMemoryUtil.AddressList> BattleUnitStruct = EmulatorMemoryUtil.GetBattleUnitStruct();

        private void BattleActorList_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddressList_SelectedIndexChanged(BattleUnitStruct, Program.ROM.RomInfo.workmemory_battle_actor_address(), BattleActorList, BattleActorAddress);
        }

        private void BattleTargetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddressList_SelectedIndexChanged(BattleUnitStruct, Program.ROM.RomInfo.workmemory_battle_target_address(), BattleTargetList, BattleTargetAddress);
        }
        List<EmulatorMemoryUtil.AddressList> WorldmapStruct = EmulatorMemoryUtil.GetWorldmapStruct();
        private void WorldmapList_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddressList_SelectedIndexChanged(WorldmapStruct, Program.ROM.RomInfo.workmemory_worldmap_data_address(), WorldmapList, WorldmapAddress);
        }

        private void ClearTurnList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ClearTurnList_SelectedIndexChanged(null, null);
            U.FireOnMouseDoubleClick(ClearTurnAddress);
        }
        private void ChapterDataList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ChapterDataList_SelectedIndexChanged(null, null);
            U.FireOnMouseDoubleClick(ChapterDataAddress);
        }

        private void BattleActorList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BattleActorList_SelectedIndexChanged(null, null);
            U.FireOnMouseDoubleClick(BattleActorAddress);
        }

        private void BattleTargetList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BattleTargetList_SelectedIndexChanged(null, null);
            U.FireOnMouseDoubleClick(BattleTargetAddress);
        }

        private void WorldmapList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WorldmapList_SelectedIndexChanged(null, null);
            U.FireOnMouseDoubleClick(WorldmapAddress);
        }

        List<EmulatorMemoryUtil.AddressList> ArenaDataStruct = EmulatorMemoryUtil.GetArenaDataStruct();
        private void ArenaDataList_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddressList_SelectedIndexChanged(ArenaDataStruct, Program.ROM.RomInfo.workmemory_arena_data_address(), ArenaDataList, ArenaDataAddress);
        }

        private void ArenaDataList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ArenaDataList_SelectedIndexChanged(null, null);
            U.FireOnMouseDoubleClick(ArenaDataAddress);
        }

        List<EmulatorMemoryUtil.AddressList> AIDataStruct = EmulatorMemoryUtil.GetAIDataStruct();
        private void AIDataList_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddressList_SelectedIndexChanged(AIDataStruct, Program.ROM.RomInfo.workmemory_ai_data_address(), AIDataList, AIDataAddress);
        }

        private void AIDataList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            AIDataList_SelectedIndexChanged(null, null);
            U.FireOnMouseDoubleClick(AIDataAddress);
        }

        List<EmulatorMemoryUtil.AddressList> ActionDataStruct = EmulatorMemoryUtil.GetActionDataStruct();
        private void ActionDataList_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddressList_SelectedIndexChanged(ActionDataStruct, Program.ROM.RomInfo.workmemory_action_data_address(), ActionDataList, ActionDataAddress);
        }

        private void ActionDataList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ActionDataList_SelectedIndexChanged(null, null);
            U.FireOnMouseDoubleClick(ActionDataAddress);
        }

        private void SupplyDataList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = SupplyDataList.SelectedIndex;
            if (index < 0 || index >= SupplyDataList.Items.Count)
            {
                BWLAddress.Text = "";
                return;
            }
            uint addr = this.SuppyRAMAddress + ((uint)index * 2);
            SupplyDataAddress.Text = U.ToHexString(addr);
        }

        private void SupplyDataList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SupplyDataList_SelectedIndexChanged(null, null);
            U.FireOnMouseDoubleClick(SupplyDataAddress);
        }

        List<EmulatorMemoryUtil.AddressList> DungeonDataStruct = EmulatorMemoryUtil.GetDungeonDataStruct();
        private void DungeonDataList_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddressList_SelectedIndexChanged(DungeonDataStruct, Program.ROM.RomInfo.workmemory_dungeon_data_address(), DungeonDataList, DungeonDataAddress);
        }

        private void DungeonDataList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DungeonDataList_SelectedIndexChanged(null, null);
            U.FireOnMouseDoubleClick(DungeonDataAddress);
        }

        List<EmulatorMemoryUtil.AddressList> BattleSomeDataStruct = EmulatorMemoryUtil.GetBattleSomeDataStruct();
        private void BattleSomeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddressList_SelectedIndexChanged(BattleSomeDataStruct, Program.ROM.RomInfo.workmemory_battlesome_data_address(), BattleSomeDataList, BattleSomeDataAddress);
        }

        private void BattleSomeList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BattleSomeList_SelectedIndexChanged(null, null);
            U.FireOnMouseDoubleClick(BattleSomeDataAddress);
        }
    }
}
