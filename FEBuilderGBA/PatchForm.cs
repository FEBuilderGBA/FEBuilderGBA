using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Concurrent;


namespace FEBuilderGBA
{
    public partial class PatchForm : Form
    {
        public PatchForm()
        {
            InitializeComponent();

            InputFormRef.TabControlHideTabOption(this.TAB);
            this.MaximizeBox = false;
            ClearCheckIF();
            ReScan();
            this.PatchList.OwnerDraw(ListBoxEx.DrawTextOnly, DrawMode.Normal);
            InputFormRef.markupJumpLabel(this.FilterExLabel);
        }

        ToolTipEx ToolTip;
        private void PatchForm_Load(object sender, EventArgs e)
        {
            this.ToolTip = InputFormRef.GetToolTip<PatchForm>();
        }

        public static string GetPatchDirectory()
        {
            return
                Path.Combine(Program.BaseDirectory, "config", "patch2", Program.ROM.VersionToFilename());
        }

        //パッチをスキャンしなおす.
        void ReScan()
        {
            this.Patchs = ScanPatchs(GetPatchDirectory(), false);
            ReFilter();
        }
        //フィルターする.
        void ReFilter()
        {
            this.FiltedPatchs = MakeFiltedPatchs(Filter.Text);

            this.PatchList.Items.Clear();
            this.PatchList.BeginUpdate();

            for (int i = 0; i < this.FiltedPatchs.Count; i++)
            {
                this.PatchList.Items.Add(this.FiltedPatchs[i].Name);
            }

            this.PatchList.EndUpdate();
            U.SelectedIndexSafety(this.PatchList, 0, true);
        }


        public class PatchSt
        {
            public string PatchFileName;
            public string Name;
            public string SearchData; //検索用データ

            public Dictionary<string,string> Param;
        };
        //全データ
        List<PatchSt> Patchs;
        //表示しているデータ
        List<PatchSt> FiltedPatchs;

        public static PatchSt[] ScanPatch()
        {
            List<PatchSt> patchs = ScanPatchs(GetPatchDirectory(), false);
            return patchs.ToArray();
        }
        static List<PatchSt> ScanPatchs(string path, bool isScanOnly)
        {
            List<PatchSt> patchs = new List<PatchSt>();

            string[] files;
            try
            {
                files = Directory.GetFiles(path, "PATCH_*.txt", SearchOption.AllDirectories);
            }
            catch (System.IO.IOException e)
            {
                R.ShowStopError("パッチ探索中にエラーが発生しました。\r\n{0}" , e.ToString());
                return patchs;
            }

            foreach (string fullfilename in files)
            {
                PatchSt patch = LoadPatch(fullfilename, isScanOnly);
                if (patch == null)
                {
                    continue;
                }
                
                patchs.Add(patch);
            }
            return patchs;
        }

        List<PatchSt> MakeFiltedPatchs(string filter)
        {
            if (filter == "")
            {
                return this.Patchs;
            }

            List<PatchSt> patchs = new List<PatchSt>();
            string lang = OptionForm.lang();
            bool isJP = (lang == "ja");
            filter = U.CleanupFindString(filter , isJP);

            if (filter == "!")
            {//インストールしているパッチだけ
                for (int i = 0; i < this.Patchs.Count; i++)
                {
                    PatchSt patch = this.Patchs[i];
                    string iferror = CheckIF(patch);
                    if (iferror.IndexOf("PATCHED_IF") < 0)
                    {//インストールしていないので消す.
                        continue;
                    }
                    patchs.Add(patch);
                }
            }
            else
            {
                for (int i = 0; i < this.Patchs.Count; i++)
                {
                    PatchSt patch = this.Patchs[i];
                    if (!U.StrStrEx(patch.SearchData, filter, isJP))
                    {//フィルターで消す.
                        continue;
                    }
                    patchs.Add(patch);
                }
            }
            return patchs;
        }


        static string CleanupKey(string key, string lang, bool canSecondLanguageEnglish , PatchSt patch)
        {
            if (key.Length < 3)
            {
                return key;
            }
            if (key[key.Length - 3] != '.')
            {
                return key;
            }
            string k = key.Substring(key.Length - 2);
            if (k == lang)
            {
                return key.Substring(0, key.Length - 3);
            }

            if (canSecondLanguageEnglish)
            {//第2言語として英語がつかるか?
                if (k == "en")
                {
                    string ret_key = key.Substring(0, key.Length - 3);
                    if (!patch.Param.ContainsKey(ret_key))
                    {//第1言語で既にあったら上書きできない.
                        return ret_key;
                    }
                }
            }

            return "";
        }
        static PatchSt LoadPatch(string fullfilename, bool isScanOnly)
        {
            string[] lines = File.ReadAllLines(fullfilename);
            return LoadPatch(lines,fullfilename, isScanOnly);
        }

        static PatchSt LoadPatch(string[] lines,string fullfilename, bool isScanOnly)
        {
            string lang = OptionForm.lang();
            bool canSecondLanguageEnglish = U.CanSecondLanguageEnglish(lang);

            PatchSt p = new PatchSt();
            p.PatchFileName = fullfilename;
            p.Param = new Dictionary<string, string>();

            for (int i = 0; i < lines.Length; i++ )
            {
                string line = lines[i];
                if (U.IsComment(line) || U.OtherLangLine(line))
                {
                    continue;
                }
                line = U.ClipComment(line);
                line = line.Trim();

                int sep = line.IndexOf('=');
                if (sep < 0)
                {
                    continue;
                }
                string key = line.Substring(0, sep);
                string value = line.Substring(sep + 1);

                key = CleanupKey(key, lang, canSecondLanguageEnglish, p);
                if (key == "")
                {
                    continue;
                }

                p.Param[key] = value;
            }

            string type = U.at(p.Param, "TYPE");
            if (type == "")
            {
                return null;
            }
            if (isScanOnly == false)
            {
                //PATCH_を読み飛ばす
                string search_filename = Path.GetFileNameWithoutExtension(fullfilename).Substring(6);

                //名前
                string name = U.at(p.Param, "NAME");
                if (name == "")
                {
                    name = search_filename;
                }
                p.Name = name;

                //検索用データ
                p.SearchData = name + "\t" + search_filename + "\t" + U.at(p.Param, "INFO") + "\t" + U.at(p.Param, "AUTHOR") + "\t" + U.at(p.Param, "TAG");
            }
            return p;
        }
        void LoadPatch(PatchSt patch)
        {
            string type = U.at(patch.Param, "TYPE");
            if (type == "IMAGE")
            {
                LoadPatchImage(patch, PatchPage, this);
            }
            else if (type == "ADDR")
            {
                LoadPatchAddr(patch);
            }
            else if (type == "STRUCT")
            {
                LoadPatchStruct(patch);
            }
            else if (type == "BIN")
            {
                LoadPatchBin(patch);
            }
            else if (type == "EA")
            {
                LoadPatchEA(patch);
            }
            else if (type == "SWITCH")
            {
                LoadPatchSwitch(patch);
            }

        }
        const int CONTROL_HEIGHT = 26;

        static string MakeInfoAndAuthorString(PatchSt patch)
        {
            StringBuilder sb = new StringBuilder();
            string deprecated = U.at(patch.Param, "DEPRECATED","false");
            if (U.stringbool(deprecated))
            {
                sb.AppendLine("!!!! DEPRECATED !!!!!!!!!!!!!!!!!!!!!!!!!!!");
                sb.AppendLine(R._("このパッチは、古いパッチです。\r\nこのパッチではなく、新しいパッチの利用を推奨します。\r\n"));
            }

            string info = atMultiLine(patch, "INFO");
            if (info  != "")
            {
                sb.AppendLine(R._(info));
            }
            string tag = atMultiLine(patch, "TAG");
            if (tag != "")
            {
                sb.Append(R._("TAG:"));
                sb.AppendLine(tag);
            }
            if (tag.IndexOf("#HIDDEN")>=0)
            {
                sb.AppendLine(R._("このパッチは、内部処理用に作られたものです。通常は利用しないでください。"));
            }

            string name = patch.Name;
            if (patch.Name == "")
            {
                name = U.at(patch.Param, "NAME");
            }
            sb.Append(R._("パッチ名:"));
            sb.AppendLine(R._(name) + "  @" + Program.ROM.VersionToFilename());

            string author = atMultiLine(patch, "AUTHOR");
            if (author != "")
            {
                sb.Append(R._("著者/出典元:"));
                sb.AppendLine(R._(author));
            }

            

            sb.AppendLine("");

            if (File.Exists(patch.PatchFileName))
            {
                DateTime updateTime = File.GetLastWriteTime(patch.PatchFileName);
                if (updateTime.AddDays(14) > DateTime.Now)
                {
                    sb.AppendLine(R._("このパッチは、{0}に更新されました。まだ新しいパッチなので、注意してください。", updateTime.ToShortDateString()));
                }
                sb.AppendLine(R._("パッチを適応する時は、ひとつ適応する度に動作を確認してください。\r\n問題があるパッチがあったら、7743にバグを報告してください。\r\n"));
            }

            return sb.ToString();
        }
        //詳細と著者
        static int InfoAndAuthor(Control patchPage, PatchSt patch)
        {
            int y = LowestPositionY(patchPage);

            string msg = MakeInfoAndAuthorString(patch);
            if (msg.Length <= 0)
            {
                return y;
            }

            if (y < patchPage.Height / 2)
            {
                y += CONTROL_HEIGHT * 3;
            }
            else
            {
                y += CONTROL_HEIGHT;
            }

            TextBoxEx info = new TextBoxEx();
            info.Multiline = true;
            info.Location = new Point(30, y);
            info.Size = new Size(600, CONTROL_HEIGHT * 4);
            info.ReadOnly = true;
            info.ScrollBars = ScrollBars.Both; 
            info.Text = msg;
            patchPage.Controls.Add(info);

            return y + info.Size.Height;
        }
        static int LowestPositionY(Control patchPage)
        {
            int maxBottom = 0;
            int count = patchPage.Controls.Count;
            for (int i = 0; i < count; i++)
            {
                int a = patchPage.Controls[i].Location.Y + patchPage.Controls[i].Size.Height;
                if (a > maxBottom)
                {
                    maxBottom = a;
                }
            }
            return maxBottom;
        }

        class StructImage
        {
            public uint ImageIndex { get; private set; }
            public NumericUpDown ImageNup { get; private set; }
            public string ImageTypeName { get; private set; }
            public uint TSAIndex { get; private set; }
            public NumericUpDown TSANup { get; private set; }
            public string TSATypeName { get; private set; }
            public uint PaletteIndex { get; private set; }
            public NumericUpDown PaletteNup { get; private set; }
            public string PaletteTypeName { get; private set; }

            public void SetImage(uint index, NumericUpDown nup, string typename)
            {
                this.ImageIndex = index;
                this.ImageNup = nup;
                this.ImageTypeName = typename;
            }
            public void SetTSA(uint index, NumericUpDown nup, string typename)
            {
                this.TSAIndex = index;
                this.TSANup = nup;
                this.TSATypeName = typename;
            }
            public void SetPalette(uint index, NumericUpDown nup, string typename)
            {
                this.PaletteIndex = index;
                this.PaletteNup = nup;
                this.PaletteTypeName = typename;
            }
            public bool HasImage()
            {
                return this.ImageNup != null;
            }
            public bool HasPalette()
            {
                return this.PaletteNup != null;
            }
            public bool HasTSA()
            {
                return this.TSANup != null ;
            }
        }

        void LoadPatchStruct(PatchSt patch)
        {
            PatchPage.Controls.Clear();
            uint struct_pointer = U.NOT_FOUND;
            uint struct_address = 0;
            string basedir = Path.GetDirectoryName(patch.PatchFileName);

            string pointer_str = U.at(patch.Param, "POINTER");
            if (pointer_str != "")
            {
                struct_pointer = convertBinAddressString(pointer_str, 8, struct_address, basedir);
                if (!U.isSafetyOffset(struct_pointer))
                {
                    throw new PatchException(R.Error("このポインタ({0})は危険です.\r\n", U.ToHexString(struct_pointer)));
                }
                struct_address = Program.ROM.p32(struct_pointer);
                if (!U.isSafetyOffset(struct_address))
                {
                    throw new PatchException(R.Error("このポインタ({1})から指されるアドレス({0})は危険です.\r\n", U.ToHexString(struct_address), U.ToHexString(struct_pointer)));
                }
            }
            else
            {
                string address_str = U.at(patch.Param, "ADDRESS");
                if (address_str == "")
                {
                    throw new PatchException(R.Error("POINTERまたはADDRESSの指定がありません"));
                }
                struct_address = convertBinAddressString(address_str, 8, 0x100, basedir);
                if (!U.isSafetyOffset(struct_address))
                {
                    throw new PatchException(R.Error("このポインタ({0})は危険です.\r\n", U.ToHexString(struct_pointer)));
                }
                struct_pointer = U.NOT_FOUND;
            }

            uint datasize = U.atoi0x(U.at(patch.Param, "DATASIZE"));
            if (datasize <= 0)
            {
                throw new SyntaxException(R.Error("データサイズ定義がありません。\r\n\r\n例:\r\nDATASIZE=4"));
            }

            uint datacount;
            string datacount_str = U.at(patch.Param, "DATACOUNT");
            if (datacount_str.Length > 0 && datacount_str[0] == '$')
            {//grep等
                datacount = convertBinAddressString(datacount_str, 8, struct_address,basedir);
                if (datacount == U.NOT_FOUND)
                {
                    throw new SyntaxException(R.Error("データ件数を調べようとしましたが、終端データを取得できません。"));
                }
                if (datacount > struct_address)
                {
                    datacount = (uint)Math.Ceiling((datacount - struct_address) / (double)datasize);
                }
            }
            else
            {//直値
                datacount = U.atoi0x(datacount_str);
            }
            if (datacount <= 0)
            {
                if (datacount_str == "")
                {
                    throw new SyntaxException(R.Error("データ個数定義がありません。\r\n\r\n例:\r\nDATACOUNT=4\r\nor\r\nDATACOUNT=$GREP4 0xFF 0xFF 0xFF 0xFF"));
                }
            }

            int y = 10;

            Label label = new Label();
            label.Text = patch.Name;
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Location = new Point(0, y);
            label.Size = new Size(500, CONTROL_HEIGHT);
            label.TextAlign = ContentAlignment.MiddleCenter;
            PatchPage.Controls.Add(label);

            Button writebutton = new Button();
            writebutton.Text = R._("書き込む");
            writebutton.Location = new Point(550, y);
            writebutton.Size = new Size(100, CONTROL_HEIGHT);
            writebutton.Name = "WriteButton";
            PatchPage.Controls.Add(writebutton);

            y += CONTROL_HEIGHT;
            y += 10;

            label = new Label();
            label.Text = R._("先頭アドレス");
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Location = new Point(0, y);
            label.Size = new Size(100, CONTROL_HEIGHT);
            label.TextAlign = ContentAlignment.MiddleCenter;
            PatchPage.Controls.Add(label);

            NumericUpDown ReadStartAddress = new NumericUpDown();
            ReadStartAddress.Increment = datacount;
            ReadStartAddress.Maximum = 0xFFFFFFFF;
            ReadStartAddress.Hexadecimal = true;
            ReadStartAddress.Location = new Point(100, y);
            ReadStartAddress.Size = new Size(100, CONTROL_HEIGHT);
            U.ForceUpdate(ReadStartAddress, struct_address);
            ReadStartAddress.Name = "ReadStartAddress";
            PatchPage.Controls.Add(ReadStartAddress);


            label = new Label();
            label.Text = R._("読込数");
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Location = new Point(200, y);
            label.Size = new Size(100, CONTROL_HEIGHT);
            label.TextAlign = ContentAlignment.MiddleCenter;
            PatchPage.Controls.Add(label);

            NumericUpDown ReadCount = new NumericUpDown();
            ReadCount.Increment = 1;
            ReadCount.Maximum = 0xFFFFFFFF;
            ReadCount.Hexadecimal = false;
            ReadCount.Location = new Point(300, y);
            ReadCount.Size = new Size(100, CONTROL_HEIGHT);
            U.ForceUpdate(ReadCount, struct_address);
            ReadCount.Name = "ReadCount";
            PatchPage.Controls.Add(ReadCount);

            Button ReloadListButton = new Button();
            ReloadListButton.Location = new Point(400, y);
            ReloadListButton.Size = new Size(100, CONTROL_HEIGHT);
            ReloadListButton.Text = R._("再取得");
            ReloadListButton.Name = "ReloadListButton";
            PatchPage.Controls.Add(ReloadListButton);

            y += CONTROL_HEIGHT;

            ListBoxEx AddressList = new ListBoxEx();
            AddressList.Location = new Point(0, y);
            AddressList.Size = new Size(200, 235);
            AddressList.Name = "AddressList";
            PatchPage.Controls.Add(AddressList);

            //拡張上限縛り
            string max_explands_size = U.at(patch.Param, "MAX_EXPLANDS_SIZE");
            if (U.atoi0x(max_explands_size) > 0)
            {
                uint max_explands = U.atoi0x(max_explands_size);
                max_explands_size = "_" + max_explands;

                if (datacount > max_explands)
                {
                    datacount = max_explands;
                }
            }
            

            Button AddressListExpandsButton = new Button();
            AddressListExpandsButton.Location = new Point(0, y + 230);
            AddressListExpandsButton.Size = new Size(100, CONTROL_HEIGHT);
            AddressListExpandsButton.Text = R._("リストの拡張");
            AddressListExpandsButton.Name = "AddressListExpandsButton" + max_explands_size;
            PatchPage.Controls.Add(AddressListExpandsButton);

            if (struct_pointer == U.NOT_FOUND)
            {//アドレスで指定した場合、データは固定長になります。拡張できません.
                AddressListExpandsButton.Hide();
            }

            label = new Label();
            label.Text = R._("アドレス");
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Location = new Point(200, y);
            label.Size = new Size(100, CONTROL_HEIGHT);
            label.TextAlign = ContentAlignment.MiddleCenter;
            PatchPage.Controls.Add(label);

            NumericUpDown Address = new NumericUpDown();
            Address.Increment = 4;
            Address.Maximum = 0xFFFFFFFF;
            Address.Hexadecimal = true;
            Address.Location = new Point(300, y);
            Address.Size = new Size(100, CONTROL_HEIGHT);
            U.ForceUpdate(Address, struct_address);
            Address.Name = "Address";
            PatchPage.Controls.Add(Address);

            label = new Label();
            label.Text = R._("選択アドレス");
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Location = new Point(400, y);
            label.Size = new Size(120, CONTROL_HEIGHT);
            label.TextAlign = ContentAlignment.MiddleCenter;
            PatchPage.Controls.Add(label);

            TextBoxEx SelectAddress = new TextBoxEx();
            SelectAddress.Location = new Point(520, y);
            SelectAddress.Size = new Size(100, CONTROL_HEIGHT);
            SelectAddress.ReadOnly = true;
            SelectAddress.Name = "SelectAddress";
            PatchPage.Controls.Add(SelectAddress);

            y += 50;
            StructImage image = new StructImage();
            foreach (var pair in patch.Param)
            {
                string[] sp = pair.Key.Split(':');
                string key = sp[0];
                string type = U.at(sp, 1);
                string value = pair.Value;

                if ( ! U.isnum(key[1]) )
                {
                    continue;
                }
                int datanum = (int)U.atoi(key.Substring(1));

                label = new Label();
                label.Text = R._(value);
                label.BorderStyle = BorderStyle.FixedSingle;
                label.Location = new Point(200, y);
                label.Size = new Size(200, CONTROL_HEIGHT);
                label.TextAlign = ContentAlignment.MiddleCenter;
                if (type != "")
                {
                    if (type != "DECIMAL")
                    {
                        label.Name = "J_" + datanum + "_" + type;
                    }
                }
                PatchPage.Controls.Add(label);

                NumericUpDown data = new NumericUpDown();
                data.Increment = 1;
                data.Hexadecimal = (type != "DECIMAL");
                data.Location = new Point(405, y);
                data.Size = new Size(100 - 5, CONTROL_HEIGHT);
                data.Name = key;
                
                if (key[0] == 'P')
                {
                    data.Increment = 4;
                    data.Maximum = 0xFFFFFFFF;
                }
                else if (key[0] == 'D')
                {
                    data.Maximum = 0xFFFFFFFF;
                }
                else if (key[0] == 'W')
                {
                    data.Maximum = 0xFFFF;
                }
                else if (key[0] == 'B')
                {
                    data.Maximum = 0xFF;
                }
                else if (key[0] == 'b')
                {
                    data.Maximum = 127;
                    data.Minimum = -127;
                }
                PatchPage.Controls.Add(data);

                if (type.Length <= 0)
                {//NOP
                }
                else if (type == "DECIMAL")
                {//10進数
                }
                else if (U.substr(type, 0, 5 + 1 + 1) == "$COMBO ")
                {//リソース読込.
                    string filename = Path.Combine(basedir, U.substr(type, 5 + 1 + 1));

                    Dictionary<uint, string> dic = U.LoadDicResource(filename);

                    ComboBox link = new ComboBox();
                    link.Location = new Point(500, y);
                    link.Size = new Size(100, CONTROL_HEIGHT);
                    link.Name = "L_" + datanum + "_" + "COMBO";
                    link.DropDownStyle = ComboBoxStyle.DropDownList;
                    PatchPage.Controls.Add(link);
                }
                else if (type.IndexOf("BATTLEANIMEITEM") == 0)
                {//戦闘アニメの特別指定1 面倒だが仕方ない.
                    TextBoxEx link = new TextBoxEx();
                    link.SetToolTipEx(this.ToolTip);
                    link.Location = new Point(500, y);
                    link.Size = new Size(100, CONTROL_HEIGHT);
                    link.ReadOnly = true;
                    link.Name = "L_" + datanum + "_" + type;
                    PatchPage.Controls.Add(link);

                    PictureBox pic = new PictureBox();
                    pic.Location = new Point(600, y);
                    pic.Size = new Size(CONTROL_HEIGHT, CONTROL_HEIGHT);
                    pic.SizeMode = PictureBoxSizeMode.Zoom;

                    pic.Name = "L_" + datanum + "_" + "BATTLEANIMEITEMICON";
                    PatchPage.Controls.Add(pic);
                }
                else if (type.IndexOf("PORTRAIT") == 0)
                {//顔画像.
                    PictureBox pic = new PictureBox();
                    pic.Location = new Point(500, y);
                    pic.Size = new Size(CONTROL_HEIGHT, CONTROL_HEIGHT);
                    pic.SizeMode = PictureBoxSizeMode.Zoom;

                    pic.Name = "L_" + datanum + "_" + "PORTRAIT";
                    PatchPage.Controls.Add(pic);
                }
                else if (type.IndexOf("BATTLEANIMESP") == 0)
                {//戦闘アニメの特別指定2 面倒だが仕方ない.
                    ComboBox link = new ComboBox();
                    link.Location = new Point(500, y);
                    link.Size = new Size(100, CONTROL_HEIGHT);
                    link.Name = "L_" + datanum + "_" + type;
                    link.DropDownStyle = ComboBoxStyle.DropDownList;
                    ImageBattleAnimeForm.MakeComboBattleAnimeSP(link);
                    PatchPage.Controls.Add(link);
                }
                else if (type.IndexOf("PatchImage") == 0)
                {//画像指定
                    if (type.IndexOf("_IMAGE") >= 0 || type.IndexOf("_ZIMAGE") >= 0)
                    {
                        image.SetImage((uint)datanum, data, type);
                    }
                    else if (type.IndexOf("_TSA") >= 0 || type.IndexOf("_ZTSA") >= 0 || type.IndexOf("_ZHEADERTSA") >= 0 || type.IndexOf("_HEADERTSA") >= 0)
                    {
                        image.SetTSA((uint)datanum, data, type);
                    }
                    else if (type.IndexOf("_PALETTE") >= 0 || type.IndexOf("_ZPALETTE") >= 0 )
                    {
                        image.SetPalette((uint)datanum, data, type);
                    }
                    label.Name = "J_" + datanum;
                }
                else
                {
                    TextBoxEx link = new TextBoxEx();
                    link.SetToolTipEx(this.ToolTip);
                    link.Location = new Point(500, y);
                    link.Size = new Size(100, CONTROL_HEIGHT);
                    link.ReadOnly = true;
                    link.Name = "L_" + datanum + "_" + type;
                    PatchPage.Controls.Add(link);

                    if (type == "UNIT" || type == "ITEM" || type == "CLASS" || type == "BATTLEANIME" || type == "BATTLEANIMEPOINTER")
                    {
                        PictureBox pic = new PictureBox();
                        pic.Location = new Point(600, y);
                        pic.Size = new Size(CONTROL_HEIGHT, CONTROL_HEIGHT);
                        pic.SizeMode = PictureBoxSizeMode.Zoom;

                        pic.Name = "L_" + datanum + "_" + type + "ICON";
                        PatchPage.Controls.Add(pic);
                    }
                    else if (type == "SONG")
                    {
                        Button play = new Button();
                        play.Location = new Point(600, y);
                        play.Size = new Size(CONTROL_HEIGHT, CONTROL_HEIGHT);

                        play.Name = "L_" + datanum + "_SONGPLAY";
                        PatchPage.Controls.Add(play);
                    }
                }

                if (type == "BATTLEANIMEPOINTER" || type == "EVENT" || type == "ITEMSHOP")
                {//データの確保が必要になる場合
                    Button alloc = new Button();
                    alloc.Location = new Point(500, y);
                    alloc.Size = new Size(100, CONTROL_HEIGHT);
                    alloc.Name = "L_" + datanum + "_NEWALLOC_" + type;
                    alloc.Text = R._("新規確保");
                    alloc.Visible = false;
                    PatchPage.Controls.Add(alloc);
                    alloc.BringToFront();
                }
                y += CONTROL_HEIGHT;
            }

            //コメントフォーム
            {
                y += CONTROL_HEIGHT / 2;
                label = new Label();
                label.Text = R._("コメント");
                label.BorderStyle = BorderStyle.FixedSingle;
                label.Location = new Point(200, y);
                label.Size = new Size(200, CONTROL_HEIGHT);
                label.TextAlign = ContentAlignment.MiddleCenter;
                this.ToolTip.SetToolTipOverraide(label, InputFormRef.GetExplain("@COMMENT"));
                PatchPage.Controls.Add(label);

                TextBoxEx comment = new TextBoxEx();
                comment.SetToolTipEx(this.ToolTip);
                comment.Location = new Point(405, y);
                comment.Size = new Size(200-5, CONTROL_HEIGHT);
                comment.Name = "Comment";
                PatchPage.Controls.Add(comment);
                y += CONTROL_HEIGHT;
            }

            //画像を利用する場合
            if (image.HasImage() )
            {
                LoadPatchStructWithImage(patch, AddressList, writebutton
                    , image, struct_address, datasize, "PatchImage");
            }
            //詳細と著者  (画像の場合、画像表示のついでにやるので問題なし)
            InfoAndAuthor(PatchPage, patch);


            //リストの名前　未定義の場合、アドレスのみ
            string listname = U.at(patch.Param, "LISTNAME");

            Dictionary<uint, string> listname_combo_dic = new Dictionary<uint,string>();
            //リストの型がわかるなら、アイコンを描画できるも
            InitStructListName(patch, listname, AddressList, out listname_combo_dic);

            InputFormRef ifr = new InputFormRef(this, "", struct_pointer, datasize
                , (int i, uint address) =>
                {
                    return (i < datacount);
                }
                , (int i, uint address) =>
                {
                    return GetStructListName(i, address, listname, listname_combo_dic);
                });
            if (U.NOT_FOUND == struct_pointer)
            {
                ifr.ReInit(struct_address);
            }

            ReadStartAddress.Tag = ifr;
            ifr.MakeGeneralAddressListContextMenu(true);

            U.SelectedIndexSafety(AddressList, 0, false);
        }

        uint LoadFixedPalette(PatchSt patch
            ,StructImage image
            ,string prefix)
        {
            NumericUpDown palette = new NumericUpDown();
            palette.Increment = 4;
            palette.Maximum = 0xFFFFFFFF;
            palette.Hexadecimal = true;
            palette.Location = new Point(120, 0);
            palette.Size = new Size(100, CONTROL_HEIGHT);
            palette.Name = prefix + "_PALETTE";
            palette.Visible = false;

            uint palette_pointer = 0;
            uint p = atOffset(patch.Param, "PALETTE_POINTER");
            if (U.isSafetyOffset(p))
            {
                palette_pointer = p;
                palette.Name = prefix + "_PALETTE";
                U.ForceUpdate(palette, Program.ROM.p32(p));
            }
            else
            {
                p = atOffset(patch.Param, "PALETTE_ADDRESS");
                if (U.isSafetyOffset(p))
                {
                    palette_pointer = U.NOT_FOUND;

                    //無圧縮パレットはアドレス指定でもいいや
                    palette.Name = prefix + "_PALETTE";
                    U.ForceUpdate(palette, U.toPointer(p));
                }
                else
                {
                    throw new SyntaxException("can not found palette.need PALETTE_POINTER or PALETTE_ADDRESS");
                }
            }

            this.PatchPage.Controls.Add(palette);
            return p;
        }


        void LoadPatchStructWithImage(PatchSt patch
            , ListBox addressList, Button writeButton,StructImage image
            , uint struct_address, uint dataSize, string prefix)
        {
            Control parent = this.PatchPage;
            int y = LowestPositionY(parent);

            //メイン画像
            PictureBox pic = new PictureBox();
            pic.Location = new Point(300, y - (CONTROL_HEIGHT*2));
            pic.Size = new Size(CONTROL_HEIGHT * 6, CONTROL_HEIGHT * 4);
            pic.Name = prefix + "_Picture";
            pic.SizeMode = PictureBoxSizeMode.Zoom;
            PatchPage.Controls.Add(pic);


            uint width = U.atoi0x(U.at(patch.Param, "WIDTH"));
            uint height = U.atoi0x(U.at(patch.Param, "HEIGHT"));
            uint palette_index = U.atoi0x(U.at(patch.Param, "PALETTE"));

            bool keepimage = U.stringbool(U.at(patch.Param, "KEEPIMAGE", "false"));
            bool keeppalette = U.stringbool(U.at(patch.Param, "KEEPPALETTE", "false"));
            bool keeptsa = U.stringbool(U.at(patch.Param, "KEEPTSA", "false"));

            if (keeptsa)
            {
                Debug.Assert(keeppalette == false);

                Button export = new Button();
                export.Location = new Point(200, y);
                export.Size = new Size(90, CONTROL_HEIGHT);
                export.Text = "Export";
                export.Name = prefix + "_Export";
                parent.Controls.Add(export);

                Button import = new Button();
                import.Location = new Point(100, y);
                import.Size = new Size(90, CONTROL_HEIGHT);
                import.Text = "Import";
                import.Name = prefix + "_Import_KeepTSA";
                parent.Controls.Add(import);

                //TSAを維持する場合は、TSAEditorの選択肢を表示
                Button tsaEditor = new Button();
                tsaEditor.Location = new Point(5, y);
                tsaEditor.Size = new Size(90, CONTROL_HEIGHT);
                tsaEditor.Text = "TSAEditor";
                tsaEditor.Name = prefix + "_TSAEditor";
                parent.Controls.Add(tsaEditor);
            }
            else if (keeppalette)
            {
                Button export = new Button();
                export.Location = new Point(200, y);
                export.Size = new Size(90, CONTROL_HEIGHT);
                export.Text = "Export";
                export.Name = prefix + "_Export";
                parent.Controls.Add(export);

                Button import = new Button();
                import.Location = new Point(100, y);
                import.Size = new Size(90, CONTROL_HEIGHT);
                import.Text = "Import";
                import.Name = prefix + "_Import_KeepPalette";
                parent.Controls.Add(import);
            }
            else if (keepimage)
            {//パレットだけ変更する
                Button export = new Button();
                export.Location = new Point(200, y);
                export.Size = new Size(90, CONTROL_HEIGHT);
                export.Text = "Export";
                export.Name = prefix + "_Export";
                parent.Controls.Add(export);

                Button import = new Button();
                import.Location = new Point(100, y);
                import.Size = new Size(90, CONTROL_HEIGHT);
                import.Text = "Import Palette";
                import.Name = prefix + "_Import_KeepImage";
                parent.Controls.Add(import);

                Button paletteEditor = new Button();
                paletteEditor.Location = new Point(5, y);
                paletteEditor.Size = new Size(90, CONTROL_HEIGHT);
                paletteEditor.Text = "PaletteEditor";
                paletteEditor.Name = prefix + "_PaletteEditor";
                parent.Controls.Add(paletteEditor);
            }
            else
            {
                Button export = new Button();
                export.Location = new Point(200, y);
                export.Size = new Size(90, CONTROL_HEIGHT);
                export.Text = "Export";
                export.Name = prefix + "_Export";
                parent.Controls.Add(export);

                Button import = new Button();
                import.Location = new Point(100, y);
                import.Size = new Size(90, CONTROL_HEIGHT);
                import.Text = "Import";
                import.Name = prefix + "_Import";
                parent.Controls.Add(import);

                //TSAを変更しない場合Paletteを変更する選択肢を出す
                Button paletteEditor = new Button();
                paletteEditor.Location = new Point(5, y);
                paletteEditor.Size = new Size(90, CONTROL_HEIGHT);
                paletteEditor.Text = "PaletteEditor";
                paletteEditor.Name = prefix + "_PaletteEditor";
                parent.Controls.Add(paletteEditor);
            }

            //グラフィックツールへ
            {
                y += CONTROL_HEIGHT;

                Button graphicsEditor = new Button();
                graphicsEditor.Location = new Point(5, y);
                graphicsEditor.Size = new Size(160, CONTROL_HEIGHT);
                graphicsEditor.Text = R._("グラフィックツール");
                graphicsEditor.Name = prefix + "_JumpGraphicsTool";
                parent.Controls.Add(graphicsEditor);
            }
            //減色ツール
            {
                Button decreaseColorButton = new Button();
                decreaseColorButton.Location = new Point(170, y);
                decreaseColorButton.Size = new Size(120, CONTROL_HEIGHT);
                decreaseColorButton.Text = R._("減色ツール");
                decreaseColorButton.Name = prefix + "_JumpDecreaseColorTool";
                parent.Controls.Add(decreaseColorButton);
            }

            uint image_pointer_base = struct_address + image.ImageIndex;
            uint palette_pointer_base ;
            if (image.HasPalette())
            {
                palette_pointer_base = struct_address + image.PaletteIndex;
            }
            else
            {
                palette_pointer_base = U.NOT_FOUND;
                LoadFixedPalette(patch, image, prefix);
            }

            uint tsa_pointer_base = U.NOT_FOUND;
            if (image.HasTSA())
            {
                tsa_pointer_base = struct_address + image.TSAIndex;
            }

            ImageFormRef ifr = new ImageFormRef(this, "", (int)width, (int)height, (int)palette_index, image_pointer_base, tsa_pointer_base, palette_pointer_base);
            ifr.UpdateAllWriteButton(writeButton);
            ifr.RegistAllWriteEvent(patch.Name);

            ifr.UpdateNumericUpDown(image.ImageNup, image.ImageTypeName);
            if (image.HasPalette())
            {
                ifr.UpdateNumericUpDown(image.PaletteNup, image.PaletteTypeName);
            }
            if (image.HasTSA() )
            {
                ifr.UpdateNumericUpDown(image.TSANup, image.TSATypeName);
            }

            addressList.SelectedIndexChanged += (Object sender,EventArgs e) =>
            {
                int selected = addressList.SelectedIndex;
                if (selected < 0)
                {
                    return ;
                }
                uint addr = (uint)(struct_address + selected * dataSize);
                uint image_pointer = addr + image.ImageIndex;
                uint palette_pointer ;
                if (image.HasPalette())
                {
                    palette_pointer = addr + image.PaletteIndex;
                }
                else
                {
                    palette_pointer = palette_pointer_base;
                }

                uint tsa_pointer = U.NOT_FOUND;
                if (image.HasTSA() )
                {
                    tsa_pointer = addr + image.TSAIndex;
                }

                ifr.UpdatePointers(image_pointer, tsa_pointer , palette_pointer);
                ifr.DrawPictureBox();
            };
        }

        void InitStructListName(PatchSt patch, string listname,ListBoxEx addressList,out Dictionary<uint, string> out_listname_combo_dic)
        {
            out_listname_combo_dic = null;

            //リストの型がわかるなら、アイコンを描画できるも
            if (listname == "UNIT")
            {
                addressList.OwnerDraw(ListBoxEx.DrawUnitAndText, DrawMode.OwnerDrawFixed);
            }
            else if (listname == "CLASS")
            {
                addressList.OwnerDraw(ListBoxEx.DrawClassAndText, DrawMode.OwnerDrawFixed);
            }
            else if (listname == "ITEM")
            {
                addressList.OwnerDraw(ListBoxEx.DrawItemAndText, DrawMode.OwnerDrawFixed);
            }
            else if (listname == "PORTRAIT")
            {
                addressList.OwnerDraw(ListBoxEx.DrawImagePortraitAndText, DrawMode.OwnerDrawFixed);
            }
            else if (listname == "BATTLEANIME")
            {
                addressList.OwnerDraw(ListBoxEx.DrawImageBattleAndText, DrawMode.OwnerDrawFixed);
            }
            else if (listname == "CG")
            {
                addressList.OwnerDraw(ListBoxEx.DrawCGAndText, DrawMode.OwnerDrawFixed);
            }
            else if (listname == "BG")
            {
                addressList.OwnerDraw(ListBoxEx.DrawBGAndText, DrawMode.OwnerDrawFixed);
            }
            else if (U.substr(listname, 0, 5 + 1 + 1) == "$COMBO ")
            {
                string basedir = Path.GetDirectoryName(patch.PatchFileName);
                string filename = Path.Combine(basedir, U.substr(listname, 5 + 1 + 1));

                out_listname_combo_dic = U.LoadDicResource(filename);
            }
            else if (listname.Length > 2 && listname[0] == '$')
            {
                int classtype = listname.IndexOf(':');
                if (classtype > 0)
                {
                    string innerlistname = U.substr(listname, classtype + 1);
                    InitStructListName(patch,innerlistname ,addressList,out out_listname_combo_dic);
                }
            }
        }
        string GetStructListName(int i, uint address, string listname, Dictionary<uint, string> listname_combo_dic)
        {
            string appendname = "";
            if (listname == "MAP")
            {
                appendname = U.ToHexString(i) + " " + MapSettingForm.GetMapName((uint)i);
            }
            else if (listname == "UNIT")
            {
                appendname = U.ToHexString(i) + " " + UnitForm.GetUnitName((uint)i);
            }
            else if (listname == "CLASS")
            {
                appendname = U.ToHexString(i) + " " + ClassForm.GetClassName((uint)i);
            }
            else if (listname == "ITEM")
            {
                appendname = U.ToHexString(i) + " " + ItemForm.GetItemName((uint)i);
            }
            else if (listname == "PORTRAIT")
            {
                appendname = U.ToHexString(i) + " " + ImagePortraitForm.GetPortraitName((uint)i);
            }
            else if (listname == "BATTLEANIME")
            {
                appendname = U.ToHexString(i) + " " + ImageBattleAnimeForm.GetBattleAnimeName((uint)i);
            }
            else if (listname == "CG")
            {
                appendname = U.ToHexString(i);
            }
            else if (listname == "BG")
            {
                appendname = U.ToHexString(i);
            }
            else if (U.substr(listname, 0, 5 + 1 + 1) == "$COMBO ")
            {
                appendname = U.ToHexString(i) + " " + U.at(listname_combo_dic, (uint)i);
            }
            else if (listname.Length > 2 && listname[0] == '$')
            {
                uint offset = U.atoh(U.substr(listname, 2));
                if (listname[1] == 'P')
                {
                    i = (int)Program.ROM.p32(address + offset);
                }
                else if (listname[1] == 'D')
                {
                    i = (int)Program.ROM.u32(address + offset);
                }
                else if (listname[1] == 'W')
                {
                    i = (int)Program.ROM.u16(address + offset);
                }
                else if (listname[1] == 'B')
                {
                    i = (int)Program.ROM.u8(address + offset);
                }
                else if (listname[1] == 'b')
                {
                    i = (int)Program.ROM.u8(address + offset);
                }
                else
                {
                    i = 0;
                    Debug.Assert(false);
                }
                int classtype = listname.IndexOf(':');
                if (classtype > 0)
                {
                    string innerlistname = U.substr(listname, classtype + 1);
                    return GetStructListName((int)i, address, innerlistname, listname_combo_dic);
                }
            }
            else
            {
                string comment = Program.CommentCache.At(address);
                if (comment == "")
                {
                    appendname = U.ToHexString(i) + " " + appendname + " " + U.ToHexString(address);
                }
                else
                {
                    appendname = U.ToHexString(i) + " " + appendname + " " + comment;
                }
            }
            return appendname;
        }


        void LoadPatchSwitch(PatchSt patch)
        {
            PatchPage.Controls.Clear();

            int y = 10;

            Label label = new Label();
            label.Text = patch.Name;
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Location = new Point(0, y);
            label.Size = new Size(500, CONTROL_HEIGHT);
            label.TextAlign = ContentAlignment.MiddleCenter;
            PatchPage.Controls.Add(label);

            Button writebutton = new Button();
            writebutton.Text = R._("書き込む");
            writebutton.Location = new Point(550, y);
            writebutton.Size = new Size(100, CONTROL_HEIGHT);
            writebutton.Name = "WriteButton";
            PatchPage.Controls.Add(writebutton);

            y += CONTROL_HEIGHT;
            y += 10;

            int x = 10;

            //値を変更したとき、書き込むボタンを強調します.
            InputFormRef.WriteButtonToYellow(writebutton, true);
            ComboBox combo = new ComboBox();
            combo.Location = new Point(x, y);
            combo.Size = new Size(600, CONTROL_HEIGHT);
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.Name = "PatchMainCombo";
            PatchPage.Controls.Add(combo);
            x += 600;

            string comboData = U.at(patch.Param, "COMBO");
            string[] comboSP = comboData.Split('|');

            combo.Items.Clear();
            for (int i = 0; i < comboSP.Length; i += 2)
            {
                combo.Items.Add(comboSP[i]);
            }

            //ディフォルトの値を設定する.
            {
                for (int n = 0; n < comboSP.Length; n += 2)
                {
                    string keyword = U.at(comboSP,n + 1);
                    uint determinationAddress = atOffset(patch.Param, "DETERMINATION_ADDRESS");

                    foreach (var pair in patch.Param)
                    {
                        string[] sp = pair.Key.Split(':');
                        string key = sp[0];
                        string address = U.at(sp, 1);
                        string value = pair.Value;

                        if (key != keyword)
                        {
                            continue;
                        }

                        uint addr = U.toOffset(U.atoi0x(address));
                        if (!U.CheckZeroAddressWrite(addr))
                        {
                            throw new SyntaxException(R.Error("このアドレスは危険です。", U.To0xHexString(addr)));
                        }
                        if (determinationAddress != 0 && determinationAddress != addr)
                        {//判別アドレスが定義されている場合で、このアドレスが判別アドレスでなければ無視
                            continue;
                        }

                        bool match = false;
                        string[] changevalueSP = value.Split(' ');
                        for (int i = 0; i < changevalueSP.Length; i++)
                        {
                            uint p = (uint)(addr + i);
                            if (p >= Program.ROM.Data.Length)
                            {
                                match = false;
                                break;
                            }
                            uint v = U.atoi0x(changevalueSP[i]);
                            uint romdata = Program.ROM.u8(p);
                            if (v != romdata)
                            {
                                match = false;
                                break;
                            }
                            match = true;
                        }

                        if (match)
                        {
                            combo.SelectedIndex = n / 2;
                        }
                    }
                }
            }


            combo.SelectedIndexChanged += (sender, e) =>
            {
                InputFormRef.WriteButtonToYellow(writebutton, true);
            };

            //詳細と著者
            InfoAndAuthor(PatchPage, patch);

            writebutton.Click += (sender, e) =>
            {
                string keyword = U.at(comboSP, combo.SelectedIndex * 2 + 1);

                Undo.UndoData undodata = Program.Undo.NewUndoData(this, patch.Name);
                foreach (var pair in patch.Param)
                {
                    string[] sp = pair.Key.Split(':');
                    string key = sp[0];
                    string address = U.at(sp, 1);
                    string value = pair.Value;

                    if (key != keyword)
                    {
                        continue;
                    }

                    uint addr = U.toOffset(U.atoi0x(address));
                    if (!U.CheckZeroAddressWrite(addr))
                    {
                        throw new SyntaxException(R.Error("アドレス0番地-0x100番地には書き込むことができません。", U.To0xHexString(addr)));
                    }

                    string[] changevalueSP = value.Split(' ');
                    for (int i = 0; i < changevalueSP.Length; i++)
                    {
                        uint p = (uint)(addr + i);
                        if (p >= Program.ROM.Data.Length)
                        {
                            break;
                        }

                        uint v = U.atoi0x(changevalueSP[i]);
                        Program.ROM.write_u8(p, v, undodata);
                    }
                }

                if (undodata.list.Count <= 0)
                {
                    throw new SyntaxException(R.Error("更新するべきデータがありません keyword:{0}", keyword));
                }

                Program.Undo.Push(undodata);
                InputFormRef.ShowWriteNotifyAnimation(this, 0);
                U.ReSelectList(this.PatchList);

                Program.ReLoadSetting();
            };
        }


        void LoadPatchAddr(PatchSt patch)
        {
            PatchPage.Controls.Clear();
            uint addr_address = 0;

            string address_string = U.at(patch.Param, "ADDRESS");
            if (address_string.Length <= 0)
            {
                throw new PatchException("bad address : " + address_string);
            }

            string[] address_sp;
            uint addr;
            if (address_string[0] == '$')
            {//マクロ展開.
                string basedir = Path.GetDirectoryName(patch.PatchFileName);
                addr = convertBinAddressString(address_string, 0, 0x100,basedir); //check only.
                if (!U.isSafetyOffset(addr))
                {
                    throw new PatchException("bad address : " + address_string);
                }
                address_sp = new string[] { U.To0xHexString(addr) };
            }
            else
            {//直値か、直値が複数.
                address_sp = address_string.Split(' ');
            }

            addr = atOffset(address_sp,0);
            if (addr > 0)
            {
                if (addr >= Program.ROM.Data.Length)
                {
                    throw new PatchException("bad address : " + U.ToHexString(addr));
                }
                if (!U.isSafetyOffset(addr))
                {
                    throw new SyntaxException("bad address : " + U.ToHexString(addr));
                }
                addr_address = addr;
            }
            for (int i = 1; i < address_sp.Length; i++ )
            {
                addr = atOffset(address_sp, i);
                if (addr > 0)
                {
                    if (!U.isSafetyOffset(addr))
                    {
                        throw new SyntaxException("bad address : " + U.ToHexString(addr) + " ADDRESS [" + i + "]");
                    }
                }
            }

            if (addr_address <= 0)
            {
                throw new SyntaxException(R.Error("アドレス指定がありません.\r\n\r\n例:\r\nADDRESS=0x1234"));
            }

            int y = 10;

            Label label = new Label();
            label.Text = patch.Name;
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Location = new Point(0, y);
            label.Size = new Size(500, CONTROL_HEIGHT);
            label.TextAlign = ContentAlignment.MiddleCenter;
            PatchPage.Controls.Add(label);

            Button writebutton = new Button();
            writebutton.Text = R._("書き込む");
            writebutton.Location = new Point(550, y);
            writebutton.Size = new Size(100, CONTROL_HEIGHT);
            writebutton.Name = "WriteButton";
            PatchPage.Controls.Add(writebutton);

            y += CONTROL_HEIGHT;
            y += 10;

            string address_type = U.at(patch.Param, "ADDRESS_TYPE");
            label = new Label();
            label.Text = string.Join("\r\n",address_sp);
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Location = new Point(0, y);
            label.Size = new Size(100, CONTROL_HEIGHT - this.Font.Height  + this.Font.Height * address_sp.Length);
            label.TextAlign = ContentAlignment.MiddleCenter;
            PatchPage.Controls.Add(label);

            NumericUpDown AddrValue = new NumericUpDown();
            AddrValue.Increment = 1;


            if (U.stringbool(U.at(patch.Param, "HEX", "true")))
            {
                AddrValue.Hexadecimal = true;
            }
            else
            {
                AddrValue.Hexadecimal = false;
            }

            AddrValue.Location = new Point(100, y);
            AddrValue.Size = new Size(100, CONTROL_HEIGHT);
            AddrValue.Name = R._("値");
            PatchPage.Controls.Add(AddrValue);

            int databyte = (int)atOffset(patch.Param, "DATASIZE", "1");
            int x = 200;

            //タイプが指定されている場合、ヒントリンクと画像を追加する.
            if (address_type != "")
            {
                InputFormRef.markupJumpLabel(label);
                label.Click += (sender, e) =>
                {
                    InputFormRef.JumpTo(AddrValue, label, address_type, new string[0]);
                };

                TextBoxEx link = new TextBoxEx();
                link.Location = new Point(x, y);
                link.Size = new Size(200, CONTROL_HEIGHT);
                link.ReadOnly = true;
                if (address_type == "UNIT" || address_type == "ITEM" || address_type == "CLASS")
                {
                    link.Name = "L_" + 0 + "_" + address_type;
                    PatchPage.Controls.Add(link);
                    x += 200;

                    PictureBox pic = new PictureBox();
                    pic.Location = new Point(x, y);
                    pic.Size = new Size(CONTROL_HEIGHT, CONTROL_HEIGHT);
                    x += CONTROL_HEIGHT;

                    pic.Name = "L_" + 0 + "_" + address_type + "ICON";
                    PatchPage.Controls.Add(pic);

                    List<Control> controls = InputFormRef.GetAllControls(PatchPage);
                    InputFormRef.makeLinkEventHandler("", controls, AddrValue, link, 0, address_type, new string[0]);
                    InputFormRef.makeLinkEventHandler("", controls, AddrValue, pic, 0, address_type +"ICON", new string[0]);
                }
                else if (address_type == "TEXT" || address_type == "MAP")
                {
                    link.Name = "L_" + 0 + "_" + address_type;
                    PatchPage.Controls.Add(link);
                    x += 200;

                    List<Control> controls = InputFormRef.GetAllControls(PatchPage);
                    InputFormRef.makeLinkEventHandler("", controls, AddrValue, link, 0, address_type, new string[0]);
                }
                else if (address_type == "PORTRAIT")
                {
                    PictureBox pic = new PictureBox();
                    pic.Location = new Point(x, y);
                    pic.Size = new Size(CONTROL_HEIGHT * 3, CONTROL_HEIGHT * 3);
                    x += CONTROL_HEIGHT * 3;

                    pic.Name = "L_" + 0 + "_PORTRAIT";
                    PatchPage.Controls.Add(pic);

                    List<Control> controls = InputFormRef.GetAllControls(PatchPage);
                    InputFormRef.makeLinkEventHandler("", controls, AddrValue, pic, 0, "PORTRAIT", new string[0]);
                }
                else if (address_type == "BATTLEANIME")
                {
                    link.Name = "L_" + 0 + "_BATTLEANIME";
                    PatchPage.Controls.Add(link);
                    x += 200;

                    PictureBox pic = new PictureBox();
                    pic.Location = new Point(x, y);
                    pic.Size = new Size(CONTROL_HEIGHT*3, CONTROL_HEIGHT*3);
                    x += CONTROL_HEIGHT*3;

                    pic.Name = "L_" + 0 + "_BATTLEANIME";
                    PatchPage.Controls.Add(pic);

                    List<Control> controls = InputFormRef.GetAllControls(PatchPage);
                    InputFormRef.makeLinkEventHandler("", controls, AddrValue, link, 0, "BATTLEANIME", new string[0]);
                    InputFormRef.makeLinkEventHandler("", controls, AddrValue, pic, 0, "BATTLEANIME", new string[0]);
                }
                else if (address_type == "BATTLEANIME_PLUS1")
                {
                    link.Name = "L_" + 0 + "_BATTLEANIME_PLUS1";
                    PatchPage.Controls.Add(link);
                    x += 200;

                    PictureBox pic = new PictureBox();
                    pic.Location = new Point(x, y);
                    pic.Size = new Size(CONTROL_HEIGHT * 3, CONTROL_HEIGHT * 3);
                    x += CONTROL_HEIGHT * 3;

                    pic.Name = "L_" + 0 + "_BATTLEANIMEICON_PLUS1";
                    PatchPage.Controls.Add(pic);

                    List<Control> controls = InputFormRef.GetAllControls(PatchPage);
                    InputFormRef.makeLinkEventHandler("", controls, AddrValue, link, 0, "BATTLEANIME", new string[] { "PLUS1" });
                    InputFormRef.makeLinkEventHandler("", controls, AddrValue, pic, 0, "BATTLEANIMEICON", new string[] { "PLUS1" });
                }
                else if (address_type == "SONG")
                {
                    link.Name = "L_" + 0 + "_" + address_type;
                    PatchPage.Controls.Add(link);
                    x += 200;

                    Button play = new Button();
                    play.Location = new Point(x, y);
                    play.Size = new Size(CONTROL_HEIGHT, CONTROL_HEIGHT);
                    play.Text = "♪";
                    x += CONTROL_HEIGHT;

                    play.Name = "L_" + 0 + "_SONGPLAY";
                    PatchPage.Controls.Add(play);

                    List<Control> controls = InputFormRef.GetAllControls(PatchPage);
                    InputFormRef.makeLinkEventHandler("", controls, AddrValue, link, 0, address_type, new string[0]);
                    InputFormRef.makeLinkEventHandler("", controls, AddrValue, play, 0, "SONGPLAY", new string[0]);
                }
                else
                {
                    List<Control> controls = InputFormRef.GetAllControls(PatchPage);
                    InputFormRef.makeLinkEventHandler("", controls, AddrValue, link, 0, address_type, new string[0]);
                }

            }

            string comboData = U.at(patch.Param, "COMBO");
            string[] comboSP = comboData.Split('|');
            if (comboSP.Length < 2)
            {
                AddrValue.Maximum = 0xFF;
                U.ForceUpdate(AddrValue, Program.ROM.u8(addr_address));
            }
            else
            {
                ComboBox combo = new ComboBox();
                combo.Location = new Point(x, y);
                combo.Size = new Size(400, CONTROL_HEIGHT);
                combo.DropDownStyle = ComboBoxStyle.DropDownList;
                combo.Name = "PatchMainCombo";
                x += 600;

                bool updatelock = false;
                combo.SelectedIndexChanged += (sender, e) =>
                {
                    if (updatelock)
                    {
                        return;
                    }

                    string changevalue = U.at(comboSP, combo.SelectedIndex * 2 + 1);
                    string[] changevalueSP = changevalue.Split(' ');
                    uint value = 0;
                    for (int i = 0; i < changevalueSP.Length; i++)
                    {
                        value += U.atoi0x(changevalueSP[i]) << (i * 8);
                    }

                    updatelock = true;
                    U.ForceUpdate(AddrValue, value);
                    updatelock = false;
                };
                AddrValue.ValueChanged += (sender, e) =>
                {
                    if (updatelock)
                    {
                        return;
                    }
                    for (int i = 0; i < comboSP.Length; i += 2)
                    {
                        string[] changevalueSP = comboSP[i + 1].Split(' ');
                        uint value = 0;
                        for (int n = 0; n < changevalueSP.Length; n++)
                        {
                            value += U.atoi0x(changevalueSP[n]) << (n * 8);
                        }
                        if (AddrValue.Value == value)
                        {
                            updatelock = true;
                            combo.SelectedIndex = i / 2;
                            updatelock = false;
                            return;
                        }
                    }

                    updatelock = true;
                    combo.SelectedIndex = -1;
                    updatelock = false;
                    return;

                };

                for (int i = 0; i < comboSP.Length; i+= 2)
                {
                    databyte = Math.Max(comboSP[i+1].Split(' ').Length , databyte);
                    combo.Items.Add(R._(comboSP[i]) + " " + comboSP[i+1]);
                }
                PatchPage.Controls.Add(combo);
            }

            if (databyte == 1)
            {
                AddrValue.Maximum = 0xFF;
                U.ForceUpdate(AddrValue, Program.ROM.u8(addr_address));
            }
            else if (databyte == 2)
            {
                AddrValue.Maximum = 0xFFFF;
                U.ForceUpdate(AddrValue, Program.ROM.u16(addr_address));
            }
            else if (databyte <= 4)
            {
                AddrValue.Maximum = 0xFFFFFFFF;
                U.ForceUpdate(AddrValue, Program.ROM.u32(addr_address));
            }
            else
            {
                //サイズオーバー 
                //いまさらだが、4バイト以上を NumUpdownでは扱えないので、別ルーチンに丸ごと取り替えるしかない
                LoadPatchAddrLong(patch);
                return;
            }

            //値を変更したとき、書き込むボタンを強調します.
            AddrValue.ValueChanged += (sender, e) =>
            {
                InputFormRef.WriteButtonToYellow(writebutton, true);
            };

            //詳細と著者
            InfoAndAuthor(PatchPage, patch);

            writebutton.Click += (sender, e) =>
            {
                Undo.UndoData undodata = Program.Undo.NewUndoData(this, patch.Name);
                for (int i = 0; i < address_sp.Length; i++)
                {
                    addr = atOffset(address_sp, i);
                    if (databyte == 1)
                    {
                        Program.ROM.write_u8(addr, (uint)AddrValue.Value, undodata);
                    }
                    else if (databyte == 2)
                    {
                        Program.ROM.write_u16(addr, (uint)AddrValue.Value, undodata);
                    }
                    else
                    {
                        Program.ROM.write_u32(addr, (uint)AddrValue.Value, undodata);
                    }
                }
                Program.Undo.Push(undodata);
                InputFormRef.ShowWriteNotifyAnimation(this, addr_address);
                U.ReSelectList(this.PatchList);

                Program.ReLoadSetting();
            };

        }

        void LoadPatchAddrLong(PatchSt patch)
        {
            PatchPage.Controls.Clear();
            uint addr_address = 0;

            string address_string = U.at(patch.Param, "ADDRESS");
            if (address_string.Length <= 0)
            {
                throw new PatchException("bad address : " + address_string);
            }

            string[] address_sp;
            uint addr;
            if (address_string[0] == '$')
            {//マクロ展開.
                string basedir = Path.GetDirectoryName(patch.PatchFileName);
                addr = convertBinAddressString(address_string, 0, 0x100,basedir); //check only.
                if (!U.isSafetyOffset(addr))
                {
                    throw new PatchException("bad address : " + address_string);
                }
                address_sp = new string[] { U.To0xHexString(addr) };
            }
            else
            {//直値か、直値が複数.
                address_sp = address_string.Split(' ');
            }

            addr = atOffset(address_sp, 0);
            if (addr > 0)
            {
                if (addr >= Program.ROM.Data.Length)
                {
                    throw new PatchException("bad address : " + U.ToHexString(addr));
                }
                if (!U.isSafetyOffset(addr))
                {
                    throw new SyntaxException("bad address : " + U.ToHexString(addr));
                }
                addr_address = addr;
            }
            for (int i = 1; i < address_sp.Length; i++)
            {
                addr = atOffset(address_sp, i);
                if (addr > 0)
                {
                    if (!U.isSafetyOffset(addr))
                    {
                        throw new SyntaxException("bad address : " + U.ToHexString(addr) + " ADDRESS [" + i + "]");
                    }
                }
            }

            if (addr_address <= 0)
            {
                throw new SyntaxException(R.Error("アドレス指定がありません.\r\n\r\n例:\r\nADDRESS=0x1234"));
            }

            int y = 10;

            Label label = new Label();
            label.Text = patch.Name;
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Location = new Point(0, y);
            label.Size = new Size(500, CONTROL_HEIGHT);
            label.TextAlign = ContentAlignment.MiddleCenter;
            PatchPage.Controls.Add(label);

            Button writebutton = new Button();
            writebutton.Text = R._("書き込む");
            writebutton.Location = new Point(550, y);
            writebutton.Size = new Size(100, CONTROL_HEIGHT);
            writebutton.Name = "WriteButton";
            PatchPage.Controls.Add(writebutton);

            y += CONTROL_HEIGHT;
            y += 10;

            string address_type = U.at(patch.Param, "ADDRESS_TYPE");
            label = new Label();
            label.Text = string.Join("\r\n", address_sp);
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Location = new Point(0, y);
            label.Size = new Size(100, CONTROL_HEIGHT - this.Font.Height + this.Font.Height * address_sp.Length);
            label.TextAlign = ContentAlignment.MiddleCenter;
            PatchPage.Controls.Add(label);

            int x = 100;

            string comboData = U.at(patch.Param, "COMBO");
            string[] comboSP = comboData.Split('|');
            if (comboSP.Length < 2)
            {
                throw new SyntaxException(R.Error("COMBOの長さが不足しています。 4バイト以上ある必要があます."));
            }
            ComboBox combo = new ComboBox();
            combo.Location = new Point(x, y);
            combo.Size = new Size(550, CONTROL_HEIGHT);
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            x += 600;

            int databyte = (int)atOffset(patch.Param, "DATASIZE", "1");
            for (int i = 0; i < comboSP.Length; i += 2)
            {
                databyte = Math.Max(comboSP[i+1].Split(' ').Length , databyte);
                combo.Items.Add(comboSP[i] + " " + comboSP[i + 1]);
            }

            //現在のデータにマッチする行を選択.
            byte[] now_data = Program.ROM.getBinaryData(addr, databyte);
            for (int i = 0; i < comboSP.Length; i += 2)
            {
                string[] changevalueSP = comboSP[i + 1].Split(' ');
                int n = 0;
                for ( ; n < changevalueSP.Length; n++)
                {
                    if (U.atoi0x(changevalueSP[n]) != now_data[n])
                    {
                        break;
                    }
                }

                if (n == changevalueSP.Length)
                {
                    combo.SelectedIndex = i / 2;
                }
            }


            combo.SelectedIndexChanged += (sender, e) =>
            {
                InputFormRef.WriteButtonToYellow(writebutton, true);
            };
            PatchPage.Controls.Add(combo);

            //詳細と著者
            InfoAndAuthor(PatchPage, patch);

            writebutton.Click += (sender, e) =>
            {
                Undo.UndoData undodata = Program.Undo.NewUndoData(this, patch.Name);
                for (int i = 0; i < address_sp.Length; i++)
                {
                    string changevalue = U.at(comboSP, combo.SelectedIndex * 2 + 1);
                    string[] changevalueSP = changevalue.Split(' ');

                    byte[] value = new byte[changevalueSP.Length];
                    for (int n = 0; n < changevalueSP.Length; n++)
                    {
                        value[n] = (byte) U.atoi0x(changevalueSP[n]);
                    }

                    addr = U.toOffset(U.atoi0x(address_sp[i]));
                    Program.ROM.write_range(addr, value, undodata);
                }
                Program.Undo.Push(undodata);
                InputFormRef.ShowWriteNotifyAnimation(this, addr_address);
                U.ReSelectList(this.PatchList);

                Program.ReLoadSetting();
            };

        }

        static void LoadPatchImage(PatchSt patch, Control parent, Form self)
        {
            parent.Controls.Clear();
            string prefix = "PatchImage";
            int y = 10;

            uint width = U.atoi0x(U.at(patch.Param, "WIDTH"));
            uint height = U.atoi0x(U.at(patch.Param, "HEIGHT"));
            uint palette_index = U.atoi0x(U.at(patch.Param, "PALETTE"));

            //画像はタイルである必要があるので、8の倍数にする
            width = width / 8 * 8;
            height = height / 8 * 8;
            if (palette_index <= 0)
            {
                palette_index = 1;
            }

            if (width <= 0)
            {
                throw new SyntaxException(R._("幅設定がありません.\r\n\r\n例:\r\nWIDTH=16"));
            }

            Label label = new Label();
            label.Text = patch.Name;
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Location = new Point(0, y);
            label.Size = new Size(500, CONTROL_HEIGHT);
            label.TextAlign = ContentAlignment.MiddleCenter;
            parent.Controls.Add(label);

            Button writebutton = new Button();
            writebutton.Text = R._("書き込む");
            writebutton.Location = new Point(550, y);
            writebutton.Size = new Size(100, CONTROL_HEIGHT);
            writebutton.Name = prefix + "_AllWriteButton";
            parent.Controls.Add(writebutton);

            y += CONTROL_HEIGHT;
            y += CONTROL_HEIGHT;


            label = new Label();
            label.Text = R._("画像");
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Location = new Point(0, y);
            label.Size = new Size(100, CONTROL_HEIGHT);
            label.TextAlign = ContentAlignment.MiddleCenter;
            parent.Controls.Add(label);


            NumericUpDown image = new NumericUpDown();
            image.Increment = 4;
            image.Maximum = 0xFFFFFFFF;
            image.Hexadecimal = true;
            image.Location = new Point(120, y);
            image.Size = new Size(100, CONTROL_HEIGHT);

            uint image_pointer = 0;
            uint p;
            p = atOffset(patch.Param, "IMAGE_POINTER");
            if (U.isSafetyOffset(p))
            {
                image_pointer = p;
                image.Name = prefix + "_IMAGE";
                U.ForceUpdate(image, Program.ROM.p32(U.toOffset(p)));
            }
            p = atOffset(patch.Param, "ZIMAGE_POINTER");
            if (U.isSafetyOffset(p))
            {
                image_pointer = p;
                image.Name = prefix + "_ZIMAGE";
                U.ForceUpdate(image, Program.ROM.p32(p));
            }
            p = atOffset(patch.Param, "Z256IMAGE_POINTER");
            if (U.isSafetyOffset(p))
            {
                image_pointer = p;
                image.Name = prefix + "_Z256IMAGE";
                U.ForceUpdate(image, Program.ROM.p32(p));

                palette_index = 16;
            }


            if (image_pointer >= Program.ROM.Data.Length)
            {
                throw new PatchException(R.Error("画像として指定されたポインタ 0x{0} は、ROM終端を超えています。", U.ToHexString(image_pointer)));
            }
            if (image_pointer <= 0)
            {
                throw new PatchException(R.Error("画像指定がありません。\r\n\r\nFile:{0} Line:{1}\r\n", U.ToHexString(image_pointer), U.ToHexString(image.Value)));
            }

            if (height <= 0)
            {
                if (LZ77.iscompress(Program.ROM.Data, (uint)image.Value))
                {
                    height = (uint)ImageUtil.CalcHeight((int)width, (int)LZ77.getUncompressSize(Program.ROM.Data, (uint)image.Value));
                }
                if (height <= 0)
                {
                    throw new PatchException(R.Error("画像として指定されたポインタ 0x{0} アドレス0x{1}が指している画像の高さが0です。", U.ToHexString(image_pointer), U.ToHexString(image.Value)));
                }
            }


            parent.Controls.Add(image);
            y += CONTROL_HEIGHT;

            uint image_pointer2 = 0;
            p = atOffset(patch.Param, "Z2IMAGE_POINTER");
            if (U.isSafetyOffset(p))
            {
                label = new Label();
                label.Text = R._("第2画像");
                label.BorderStyle = BorderStyle.FixedSingle;
                label.Location = new Point(0, y);
                label.Size = new Size(100, CONTROL_HEIGHT);
                label.TextAlign = ContentAlignment.MiddleCenter;
                parent.Controls.Add(label);

                NumericUpDown image2 = new NumericUpDown();
                image2.Increment = 4;
                image2.Maximum = 0xFFFFFFFF;
                image2.Hexadecimal = true;
                image2.Location = new Point(120, y);
                image2.Size = new Size(100, CONTROL_HEIGHT);

                image2.Name = prefix + "_Z2IMAGE";
                U.ForceUpdate(image2, Program.ROM.p32(p));
                image_pointer2 = p;

                if (!U.isSafetyOffset((uint)image2.Value))
                {
                    throw new PatchException(R.Error("画像として指定されたポインタ 0x{0} が指しているアドレス0x{1}は危険です。", U.ToHexString(image_pointer), U.ToHexString(image.Value)));
                }
                parent.Controls.Add(image2);
                y += CONTROL_HEIGHT;
            }


            NumericUpDown tsa = new NumericUpDown();
            tsa.Increment = 4;
            tsa.Maximum = 0xFFFFFFFF;
            tsa.Hexadecimal = true;
            tsa.Location = new Point(120, y);
            tsa.Size = new Size(100, CONTROL_HEIGHT);

            uint tsa_pointer = 0;
            p = atOffset(patch.Param, "TSA_POINTER");
            if (U.isSafetyOffset(p))
            {
                tsa_pointer = p;
                tsa.Name = prefix + "_TSA";
                U.ForceUpdate(tsa, Program.ROM.p32(p));
            }
            p = atOffset(patch.Param, "ZTSA_POINTER");
            if (U.isSafetyOffset(p))
            {
                tsa_pointer = p;
                tsa.Name = prefix + "_ZTSA";
                U.ForceUpdate(tsa, Program.ROM.p32(p));
            }
            p = atOffset(patch.Param, "HEADERTSA_POINTER");
            if (U.isSafetyOffset(p))
            {
                tsa_pointer = p;
                tsa.Name = prefix + "_HEADERTSA";
                U.ForceUpdate(tsa, Program.ROM.p32(p));
            }
            p = atOffset(patch.Param, "ZHEADERTSA_POINTER");
            if (U.isSafetyOffset(p))
            {
                tsa_pointer = p;
                tsa.Name = prefix + "_ZHEADERTSA";
                U.ForceUpdate(tsa, Program.ROM.p32(p));
            }

            if (tsa_pointer > 0)
            {
                parent.Controls.Add(tsa);

                label = new Label();
                label.Text = R._("TSA");
                label.BorderStyle = BorderStyle.FixedSingle;
                label.Location = new Point(0, y);
                label.Size = new Size(100, CONTROL_HEIGHT);
                label.TextAlign = ContentAlignment.MiddleCenter;
                parent.Controls.Add(label);

                y += CONTROL_HEIGHT;
            }
            else
            {
                tsa.Dispose();
            }

            label = new Label();
            label.Text = R._("パレット");
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Location = new Point(0, y);
            label.Size = new Size(100, CONTROL_HEIGHT);
            label.TextAlign = ContentAlignment.MiddleCenter;
            parent.Controls.Add(label);


            NumericUpDown palette = new NumericUpDown();
            palette.Increment = 4;
            palette.Maximum = 0xFFFFFFFF;
            palette.Hexadecimal = true;
            palette.Location = new Point(120, y);
            palette.Size = new Size(100, CONTROL_HEIGHT);
            palette.Name = prefix + "_PALETTE";

            uint palette_pointer = 0;
            p = atOffset(patch.Param, "PALETTE_POINTER");
            if (U.isSafetyOffset(p))
            {
                palette_pointer = p;
                palette.Name = prefix + "_PALETTE";
                U.ForceUpdate(palette, Program.ROM.p32(p));
            }
            else
            {
                p = atOffset(patch.Param, "PALETTE_ADDRESS");
                if (U.isSafetyOffset(p))
                {
                    palette_pointer = U.NOT_FOUND;

                    //無圧縮パレットはアドレス指定でもいいや
                    palette.Name = prefix + "_PALETTE";
                    U.ForceUpdate(palette, U.toPointer(p));
                }
                else
                {
                    throw new SyntaxException("can not found palette.need PALETTE_POINTER or PALETTE_ADDRESS");
                }
            }


            y += CONTROL_HEIGHT;
            parent.Controls.Add(palette);

            PictureBox pic = new PictureBox();
            pic.Location = new Point(300, 50);
            pic.Size = new Size(300, 200);
            pic.Name = prefix + "_Picture";
            pic.SizeMode = PictureBoxSizeMode.AutoSize;
            parent.Controls.Add(pic);

            y += CONTROL_HEIGHT;

            bool keepimage = U.stringbool(U.at(patch.Param, "KEEPIMAGE", "false"));
            bool keeppalette = U.stringbool(U.at(patch.Param, "KEEPPALETTE", "false"));
            bool keeptsa = U.stringbool(U.at(patch.Param, "KEEPTSA", "false"));

            if (keeptsa)
            {
                Debug.Assert(keeppalette == false);

                Button export = new Button();
                export.Location = new Point(200, y);
                export.Size = new Size(90, CONTROL_HEIGHT);
                export.Text = "Export";
                export.Name = prefix + "_Export";
                parent.Controls.Add(export);

                Button import = new Button();
                import.Location = new Point(100, y);
                import.Size = new Size(90, CONTROL_HEIGHT);
                import.Text = "Import";
                import.Name = prefix + "_Import_KeepTSA";
                parent.Controls.Add(import);

                //TSAを維持する場合は、TSAEditorの選択肢を表示
                Button tsaEditor = new Button();
                tsaEditor.Location = new Point(5, y);
                tsaEditor.Size = new Size(90, CONTROL_HEIGHT);
                tsaEditor.Text = "TSAEditor";
                tsaEditor.Name = prefix + "_TSAEditor";
                parent.Controls.Add(tsaEditor);
            }
            else if (keeppalette)
            {
                Button export = new Button();
                export.Location = new Point(200, y);
                export.Size = new Size(90, CONTROL_HEIGHT);
                export.Text = "Export";
                export.Name = prefix + "_Export";
                parent.Controls.Add(export);

                Button import = new Button();
                import.Location = new Point(100, y);
                import.Size = new Size(90, CONTROL_HEIGHT);
                import.Text = "Import";
                import.Name = prefix + "_Import_KeepPalette";
                parent.Controls.Add(import);
            }
            else if (keepimage)
            {//パレットだけ変更する
                Button export = new Button();
                export.Location = new Point(200, y);
                export.Size = new Size(90, CONTROL_HEIGHT);
                export.Text = "Export";
                export.Name = prefix + "_Export";
                parent.Controls.Add(export);

                Button import = new Button();
                import.Location = new Point(100, y);
                import.Size = new Size(90, CONTROL_HEIGHT);
                import.Text = "Import Palette";
                import.Name = prefix + "_Import_KeepImage";
                parent.Controls.Add(import);

                Button paletteEditor = new Button();
                paletteEditor.Location = new Point(5, y);
                paletteEditor.Size = new Size(90, CONTROL_HEIGHT);
                paletteEditor.Text = "PaletteEditor";
                paletteEditor.Name = prefix + "_PaletteEditor";
                parent.Controls.Add(paletteEditor);
            }
            else
            {
                Button export = new Button();
                export.Location = new Point(200, y);
                export.Size = new Size(90, CONTROL_HEIGHT);
                export.Text = "Export";
                export.Name = prefix + "_Export";
                parent.Controls.Add(export);

                Button import = new Button();
                import.Location = new Point(100, y);
                import.Size = new Size(90, CONTROL_HEIGHT);
                import.Text = "Import";
                import.Name = prefix + "_Import";
                parent.Controls.Add(import);

                //TSAを変更しない場合Paletteを変更する選択肢を出す
                Button paletteEditor = new Button();
                paletteEditor.Location = new Point(5, y);
                paletteEditor.Size = new Size(90, CONTROL_HEIGHT);
                paletteEditor.Text = "PaletteEditor";
                paletteEditor.Name = prefix + "_PaletteEditor";
                parent.Controls.Add(paletteEditor);
            }

            //グラフィックツールへ
            {
                y += CONTROL_HEIGHT;

                Button graphicsEditor = new Button();
                graphicsEditor.Location = new Point(5, y);
                graphicsEditor.Size = new Size(160, CONTROL_HEIGHT);
                graphicsEditor.Text = R._("グラフィックツール");
                graphicsEditor.Name = prefix + "_JumpGraphicsTool";
                parent.Controls.Add(graphicsEditor);
            }
            //減色ツール
            {
                Button decreaseColorButton = new Button();
                decreaseColorButton.Location = new Point(170, y);
                decreaseColorButton.Size = new Size(120, CONTROL_HEIGHT);
                decreaseColorButton.Text = R._("減色ツール");
                decreaseColorButton.Name = prefix + "_JumpDecreaseColorTool";
                parent.Controls.Add(decreaseColorButton);
            }

            //上書き禁止アドレスを取得.
            List<uint> forceSeparationAddress = new List<uint>();
            {
                string foceSeparationAddressString = U.at(patch.Param, "FORCE_SEPARATION_ADDRESS");
                string[] foceSeparationAddressSP = foceSeparationAddressString.Split(' ');
                for (int i = 0; i < foceSeparationAddressSP.Length; i++)
                {
                    forceSeparationAddress.Add(U.atoi0x(foceSeparationAddressSP[i]));
                }
            }

            //詳細と著者
            InfoAndAuthor(parent, patch);

            ImageFormRef ifr = new ImageFormRef(self, "", (int)width, (int)height, (int)palette_index, image_pointer, tsa_pointer, palette_pointer, forceSeparationAddress.ToArray(), image_pointer2);
            ifr.RegistAllWriteEvent(patch.Name);

            image.Tag = ifr;
        }

        class BinBlock
        {
            public string filename;
            public uint addr;
            public BinBlock(uint addr,string filename)
            {
                this.addr = addr;
                this.filename = filename;
            }
        };
        static uint atOffset(string[] dic, int at, string def = "0")
        {
            string v = U.at(dic, at, def);
            return convertBinAddressString(v, 0, 0x100,"");
        }
        static uint atOffset(Dictionary<string, string> dic, string at, string def = "0")
        {
            string v = U.at(dic, at, def);
            return convertBinAddressString(v, 0, 0x100, "");
        }
        static uint atOffset(Dictionary<uint, string> dic, uint at, string def = "0")
        {
            string v = U.at(dic, at, def);
            return convertBinAddressString(v, 0, 0x100, "");
        }
        static uint convertBinAddressString(string addrstring, uint appnedSize, uint start_offset , string basedir)
        {
            if (addrstring == "")
            {
                return U.NOT_FOUND;
            }

            if (addrstring[0] != '$')
            {//マクロではなく アドレスの数字として解釈.
                uint addr = U.toOffset(U.atoi0x(addrstring));
                return addr;
            }

            //マクロ展開 $
            string value = addrstring.Substring(1);
            if (U.isnum(value[0]))
            {//$0x123 -> [123] ポインタ
                uint addr = U.toOffset(U.atoi0x(value));
                if (!U.isSafetyOffset(addr))
                {
                    throw new PatchException(R.Error(("ポインタ{0}が指す領域 0x{1}にはアクセスできません"), addrstring, U.ToHexString(addr)));
                }
                return Program.ROM.p32(addr);
            }

            if (value == "FREEAREA")
            {
                if (appnedSize == 0)
                {
                    return 0;
                }

                //拡張領域から探すときは、ファイル終端に備えて、アライメントを考えて、ちょい大目に探さないといけない.
                uint searchFreespaceSize = U.Padding4(appnedSize) + 4;

                //拡張領域に移動.
                uint freespace = MoveToFreeSapceForm.SearchFreeSpaceOne(searchFreespaceSize, OptionForm.rom_extends_option());
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

                return freespace;
            }

            //データの検索
            if (value.IndexOf("GREP16 ") == 0)
            {
                return U.Grep(Program.ROM.Data, MakeGrepData(value), start_offset, 0, 16);
            }
            if (value.IndexOf("GREP12 ") == 0)
            {
                return U.Grep(Program.ROM.Data, MakeGrepData(value), start_offset, 0, 12);
            }
            if (value.IndexOf("GREP8 ") == 0)
            {
                return U.Grep(Program.ROM.Data, MakeGrepData(value), start_offset, 0, 8);
            }
            if (value.IndexOf("GREP4 ") == 0)
            {
                return U.Grep(Program.ROM.Data, MakeGrepData(value), start_offset, 0, 4);
            }
            if (value.IndexOf("GREP2 ") == 0)
            {
                return U.Grep(Program.ROM.Data, MakeGrepData(value), start_offset, 0, 2);
            }
            if (value.IndexOf("GREP1 ") == 0)
            {
                return U.Grep(Program.ROM.Data, MakeGrepData(value), start_offset, 0, 1);
            }
            if (value.IndexOf("GREP4END ") == 0)
            {
                return U.GrepEnd(Program.ROM.Data, MakeGrepData(value), start_offset, 0, 4, 0, true);
            }
            if (value.IndexOf("GREP1END ") == 0)
            {
                return U.GrepEnd(Program.ROM.Data, MakeGrepData(value), start_offset, 0, 1, 0, true);
            }
            if (value.IndexOf("GREP4END+4 ") == 0)
            {
                return U.GrepEnd(Program.ROM.Data, MakeGrepData(value), start_offset, 0, 4 ,4, true);
            }
            if (value.IndexOf("GREP4END+8 ") == 0)
            {
                return U.GrepEnd(Program.ROM.Data, MakeGrepData(value), start_offset, 0, 4, 8, true);
            }
            if (value.IndexOf("GREP4END+12 ") == 0)
            {
                return U.GrepEnd(Program.ROM.Data, MakeGrepData(value), start_offset, 0, 4, 12, true);
            }
            if (value.IndexOf("GREP4END+16 ") == 0)
            {
                return U.GrepEnd(Program.ROM.Data, MakeGrepData(value), start_offset, 0, 4, 16, true);
            }
            if (value.IndexOf("GREP4END+20 ") == 0)
            {
                return U.GrepEnd(Program.ROM.Data, MakeGrepData(value), start_offset, 0, 4, 20, true);
            }

            if (value.IndexOf("FGREP16 ") == 0)
            {
                return U.Grep(Program.ROM.Data, MakeGrepData(value, basedir), start_offset, 0, 16);
            }
            if (value.IndexOf("FGREP12 ") == 0)
            {
                return U.Grep(Program.ROM.Data, MakeGrepData(value, basedir), start_offset, 0, 12);
            }
            if (value.IndexOf("FGREP8 ") == 0)
            {
                return U.Grep(Program.ROM.Data, MakeGrepData(value, basedir), start_offset, 0, 8);
            }
            if (value.IndexOf("FGREP4 ") == 0)
            {
                return U.Grep(Program.ROM.Data, MakeGrepData(value, basedir), start_offset, 0, 4);
            }
            if (value.IndexOf("FGREP1 ") == 0)
            {
                return U.Grep(Program.ROM.Data, MakeGrepData(value, basedir), start_offset, 0, 1);
            }
            if (value.IndexOf("FGREP4END ") == 0)
            {
                return U.GrepEnd(Program.ROM.Data, MakeGrepData(value, basedir), start_offset, 0, 4, 0, true);
            }
            if (value.IndexOf("FGREP1END ") == 0)
            {
                return U.GrepEnd(Program.ROM.Data, MakeGrepData(value, basedir), start_offset, 0, 1, 0, true);
            }
            if (value.IndexOf("FGREP4END+4 ") == 0)
            {
                return U.GrepEnd(Program.ROM.Data, MakeGrepData(value, basedir), start_offset, 0, 4, 4, true);
            }
            if (value.IndexOf("FGREP4END+8 ") == 0)
            {
                return U.GrepEnd(Program.ROM.Data, MakeGrepData(value, basedir), start_offset, 0, 4, 8, true);
            }
            if (value.IndexOf("FGREP4END+12 ") == 0)
            {
                return U.GrepEnd(Program.ROM.Data, MakeGrepData(value, basedir), start_offset, 0, 4, 12, true);
            }
            if (value.IndexOf("FGREP4END+16 ") == 0)
            {
                return U.GrepEnd(Program.ROM.Data, MakeGrepData(value, basedir), start_offset, 0, 4, 16, true);
            }
            if (value.IndexOf("FGREP4END+20 ") == 0)
            {
                return U.GrepEnd(Program.ROM.Data, MakeGrepData(value, basedir), start_offset, 0, 4, 20 , true);
            }
            if (value.IndexOf("P32 ") == 0)
            {
                return ReadPointer(value,0);
            }
            if (value.IndexOf("P32+4 ") == 0)
            {
                return ReadPointer(value, 4);
            }


            throw new PatchException(R.Error(("アドレス指定が正しくありません。 値:0x{0}"), addrstring));
        }

        static uint ReadPointer(string value, uint plus)
        {
            string[] sp = value.Split(' ');
            if (sp.Length < 1)
            {
                return U.NOT_FOUND;
            }
            uint p = U.atoi0x(sp[1]);
            if (! U.isSafetyOffset(p))
            {
                return U.NOT_FOUND;
            }

            uint pp = Program.ROM.u32(p + plus);
            if (! U.isSafetyPointer(pp))
            {
                return U.NOT_FOUND;
            }
            if (U.IsValueOdd(pp))
            {
                pp--;
            }
            pp = U.toOffset(pp);

            return pp;
        }

        static byte[] MakeGrepData(string value)
        {
            string[] sp = value.Split(' ');
            List<byte> grepdata = new List<byte>();
            for (int i = 1; i < sp.Length; i++)
            {
                grepdata.Add((byte)U.atoi0x(sp[i]));
            }
            return grepdata.ToArray();
        }
        static byte[] MakeGrepData(string value, string basedir)
        {
            int firstSp = value.IndexOf(' ');
            if (firstSp < 0)
            {
                return new byte[0];
            }
            string filename = value.Substring(firstSp + 1);
            string fullpath = Path.Combine(basedir, filename);
            if (!File.Exists(fullpath))
            {
                return new byte[0];
            }
            List<byte> grepdata = new List<byte>(File.ReadAllBytes(fullpath));
            return grepdata.ToArray();
        }

        void LoadPatchEA(PatchSt patch)
        {
            PatchPage.Controls.Clear();

            int y = 10;

            Label label = new Label();
            label.Text = patch.Name;
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Location = new Point(0, y);
            label.Size = new Size(500, CONTROL_HEIGHT);
            label.TextAlign = ContentAlignment.MiddleCenter;
            PatchPage.Controls.Add(label);

            Button writebutton = new Button();
            writebutton.Text = R._("書き込む");
            writebutton.Location = new Point(550, y);
            writebutton.Size = new Size(100, CONTROL_HEIGHT);
            writebutton.Name = "WriteButton";
            PatchPage.Controls.Add(writebutton);
            y += CONTROL_HEIGHT;
            y += 10;

            uint freearea = InputFormRef.AllocBinaryData(1024 * 1024); //とりあえず1MBの空きがあるところ.

            string EAFilename = "";
            foreach (var pair in patch.Param)
            {
                string[] sp = pair.Key.Split(':');
                string key = sp[0];
                string addrstring = U.at(sp, 1);
                string value = pair.Value;

                if (key == "EA")
                {
                    label = new Label();
                    label.Text = pair.Key;
                    label.BorderStyle = BorderStyle.FixedSingle;
                    label.Location = new Point(0, y);
                    label.Size = new Size(300, CONTROL_HEIGHT);
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    PatchPage.Controls.Add(label);

                    TextBoxEx eventfilename = new TextBoxEx();
                    eventfilename.Location = new Point(300, y);
                    eventfilename.Size = new Size(200, CONTROL_HEIGHT);
                    eventfilename.ReadOnly = true;
                    eventfilename.Text = value;
                    PatchPage.Controls.Add(eventfilename);

                    string basedir = Path.GetDirectoryName(patch.PatchFileName);
                    EAFilename = Path.Combine(basedir, value);

                    y += CONTROL_HEIGHT;
                }
                else if (key == "FREEAREA")
                {
                    if (U.stringbool(value) == false)
                    {//フリーエリアを利用しない
                        freearea = 0;
                    }
                }
            }

            //詳細と著者
            InfoAndAuthor(PatchPage, patch);

            if (EAFilename.Length <= 0)
            {
                writebutton.Enabled = false;
            }
            
            writebutton.Click += (sender, e) =>
            {
                Undo.UndoData undodata = Program.Undo.NewUndoData(this, patch.Name);

                try
                {
                    SymbolUtil.DebugSymbol storeSymbol = SymbolUtil.DebugSymbol.SaveComment;
                    EventAssemblerForm.WriteEA(EAFilename, freearea, undodata, storeSymbol);
                }
                catch (PatchException exception)
                {
                    Program.Undo.Rollback(undodata);  //操作の取り消し

                    R.ShowStopError(exception.Message);
                    return;
                }
                catch (Exception)
                {
                    Program.Undo.Rollback(undodata);  //操作の取り消し
                    throw; //再送
                }

                Program.Undo.Push(undodata);
                InputFormRef.ShowWriteNotifyAnimation(this, 0);
                U.ReSelectList(this.PatchList);

                Program.ReLoadSetting();
            };
        }

        void LoadPatchBin(PatchSt patch)
        {
            PatchPage.Controls.Clear();

            int y = 10;

            Label label = new Label();
            label.Text = patch.Name;
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Location = new Point(0, y);
            label.Size = new Size(500, CONTROL_HEIGHT);
            label.TextAlign = ContentAlignment.MiddleCenter;
            PatchPage.Controls.Add(label);

            Button writebutton = new Button();
            writebutton.Text = R._("書き込む");
            writebutton.Location = new Point(550, y);
            writebutton.Size = new Size(100, CONTROL_HEIGHT);
            writebutton.Name = "WriteButton";
            PatchPage.Controls.Add(writebutton);
            y += CONTROL_HEIGHT;
            y += 10;


            Panel panel = new Panel();
            panel.Location = new Point(0, y);
            panel.Size = new Size(600, 150);
            y += 100;

            string[] keywords = new string[]{ 
                "BIN"         //バイナリをそのまま埋め込む
               ,"BINP"        //バイナリをそのまま埋め込む ポインタ
               ,"BINAP"       //バイナリをそのまま埋め込む ASMポインタ
               ,"BINF"        //バイナリをそのまま埋め込む 画像など
               ,"CLEAR"       //00クリア
               ,"COPY"        //データコピー
               ,"SLIDE"       //データのスライド移動
               ,"TEXT"        //テキストデータ
               ,"TEXTADV"     //テキストデータ FEditorAdv形式
               ,"EXTENDS"     //拡張
               ,"JUMP"        //割り込みコード生成(必ず最後に書かないとダメ)
            };

            bool isTextCommand = false;
            int yy = 0;
            int bincount = 0;
            foreach (var pair in patch.Param)
            {
                string basedir = Path.GetDirectoryName(patch.PatchFileName);
                foreach (string keyword in keywords)
                {
                    string[] sp = pair.Key.Split(':');
                    string key = sp[0];
                    string addrstring = U.at(sp, 1);
                    string value = pair.Value;

                    if (key != keyword)
                    {
                        continue;
                    }

                    convertBinAddressString(addrstring, 0, 0x100, basedir); //check only.

                    if (keyword != "JUMP" 
                        && keyword != "COPY" 
                        && keyword != "SLIDE"
                        && keyword != "EXTENDS"
                        && keyword != "CLEAR")
                    {
                        string filename = Path.Combine(basedir, value);
                        if (!File.Exists(filename))
                        {
                            throw new PatchException(keyword + "(" + filename + ") file can not found.\r\n" + pair.Key + "=" + pair.Value);
                        }
                    }
                    if (keyword == "TEXT" || keyword == "TEXTADV")
                    {
                        isTextCommand = true;
                    }

                    label = new Label();
                    label.Text = pair.Key;
                    label.BorderStyle = BorderStyle.FixedSingle;
                    label.Location = new Point(0, yy);
                    label.Size = new Size(300, CONTROL_HEIGHT);
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    panel.Controls.Add(label);

                    TextBoxEx binfilename = new TextBoxEx();
                    binfilename.Location = new Point(300, yy);
                    binfilename.Size = new Size(200, CONTROL_HEIGHT);
                    binfilename.ReadOnly = true;
                    binfilename.Text = value;
                    panel.Controls.Add(binfilename);

                    yy += CONTROL_HEIGHT;
                    bincount++;
                }
            }
            panel.AutoScroll = true;
            PatchPage.Controls.Add(panel);

            //詳細と著者
            InfoAndAuthor(PatchPage, patch);

            if (bincount <= 0)
            {
                writebutton.Enabled = false;
            }
            if (isTextCommand)
            {
                if (!InputFormRef.SearchAntiHuffmanPatch())
                {
                    throw new PatchException(R.Error("このパッチは、テキストを変更するので、インストールする前に AntiHuffmanパッチが必要です。"));
                }
            }

            writebutton.Click += (sender, e) =>
            {
                if (InputFormRef.IsPleaseWaitDialog(this))
                {//2重割り込み禁止
                    return;
                }
                //少し時間がかかるので、しばらくお待ちください表示.
                using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
                {
                    Undo.UndoData undodata = Program.Undo.NewUndoData(this, patch.Name);

                    try
                    {
                        writeBIN(patch, keywords, undodata);
                    }
                    catch (PatchException exception)
                    {
                        Program.Undo.Rollback(undodata); //操作の取り消し

                        R.ShowStopError(exception.Message);
                        return;
                    }
                    catch (SyntaxErrorException exception)
                    {
                        Program.Undo.Rollback(undodata); //操作の取り消し

                        R.ShowStopError(exception.Message);
                        return;
                    }
                    catch (Exception)
                    {
                        Program.Undo.Rollback(undodata);  //操作の取り消し
                        throw; //再送
                    }

                    Program.Undo.Push(undodata);
                    InputFormRef.ShowWriteNotifyAnimation(this, 0);
                    U.ReSelectList(this.PatchList);

                    //設定の読み直し
                    Program.ReLoadSetting();
                }
            };
        }

        void writeBIN(PatchSt patch, string[] keywords, Undo.UndoData undodata)
        {
            List<BinBlock> binBlocks = new List<BinBlock>();
            foreach (var pair in patch.Param)
            {
                foreach (string keyword in keywords)
                {
                    string[] sp = pair.Key.Split(':');
                    string key = sp[0];
                    string value = pair.Value;

                    if (key != keyword)
                    {
                        continue;
                    }

                    string basedir = Path.GetDirectoryName(patch.PatchFileName);
                    string filename = Path.Combine(basedir, value);
                    makeBinMod(sp, filename, binBlocks, undodata);
                }
            }
        }


        byte[] makeBinModInnerReadFile(string[] sp, string filename, out uint out_addr)
        {
            //ファイルを読み込んで、
            byte[] b = File.ReadAllBytes(filename);

            string basedir = Path.GetDirectoryName(filename);

            //割り当てるアドレスを求めて
            uint addr = convertBinAddressString(sp[1], (uint)b.Length, 0x100 , basedir);

            //アドレスの移動がある場合は、移動して
            uint chaddr = U.atoi0x(U.at(sp, 2));
            if (chaddr > 0)
            {
                b = ChangeAddress(b, addr, chaddr);
            }

            //結果を返します.
            out_addr = addr;
            return b;
        }


        void makeBinMod(string[] sp, string filename, List<BinBlock> binBlocks, Undo.UndoData undodata)
        {
            string keyword = sp[0];

            if (keyword == "BIN" || keyword == "BINP" || keyword == "BINAP" || keyword == "BINF")
            {
                BinWriteFile(sp, filename, binBlocks, undodata);
            }
            else if (keyword == "COPY")
            {
                BinWriteCOPY(sp, filename, binBlocks, undodata);
            }
            else if (keyword == "SLIDE")
            {
                BinWriteSLIDE(sp, filename, binBlocks, undodata);
            }
            else if (keyword == "JUMP")
            {
                BinWriteJump(sp, filename, binBlocks, undodata);
            }
            else if (keyword == "TEXT" || keyword == "TEXTADV")
            {
                BinWriteTEXT(sp, filename, binBlocks, undodata);
            }
            else if (keyword == "EXTENDS")
            {
                BinWriteExtebds(sp, filename, binBlocks, undodata);
            }
            else if (keyword == "CLEAR")
            {
                BinClear(sp, filename, binBlocks, undodata);
            }
            else
            {
                throw new SyntaxException(R.Error("不明なキーワードです : {0}", keyword));
            }
        }
        void BinWriteFile(string[] sp, string filename, List<BinBlock> binBlocks, Undo.UndoData undodata)
        {
            string keyword = sp[0];
            byte[] b;
            uint addr;
            b = makeBinModInnerReadFile(sp, filename, out addr);
            WriteBB(addr, filename, b, binBlocks, undodata);
        }

        void WriteBB(uint addr, string filename, byte[] b, List<BinBlock> binBlocks, Undo.UndoData undodata)
        {
            if (!U.CheckZeroAddressWrite(addr))
            {
                throw new SyntaxException(R.Error("アドレス0番地-0x100番地には書き込むことができません。", U.To0xHexString(addr)));
            }

            if (addr + b.Length > Program.ROM.Data.Length)
            {//必要サイズがROMサイズを超えていたら増設する.
                Program.ROM.write_resize_data((uint)(addr + b.Length));
            }

            Program.ROM.write_range(addr, b, undodata);

            binBlocks.Add(new BinBlock(addr, filename));
        }

        void BinClear(string[] sp, string filename, List<BinBlock> binBlocks, Undo.UndoData undodata)
        {
            string basedir = Path.GetDirectoryName(filename);

            string addrstring = sp[1];
            uint dest_addr = convertBinAddressString(addrstring, 8, 0x100, basedir);
            if (!U.isSafetyOffset(dest_addr))
            {
                throw new PatchException(R.Error("コピー先アドレスの指定が正しくありません dest:{0}", addrstring));
            }

            uint src_datasize = U.atoi0x(U.at(sp, 2));
            if (src_datasize <= 0)
            {
                throw new PatchException(R.Error("コピー元のアドレスの指定が正しくありません"));
            }

            byte[] s = new byte[src_datasize];
            WriteBB(dest_addr, filename, s, binBlocks, undodata);
        }

        void BinWriteCOPY(string[] sp, string filename,List<BinBlock> binBlocks, Undo.UndoData undodata)
        {
            string basedir = Path.GetDirectoryName(filename);

            string addrstring = sp[1];
            uint dest_addr = convertBinAddressString(addrstring, 8, 0x100, basedir);
            if (!U.isSafetyOffset(dest_addr))
            {
                throw new PatchException(R.Error("コピー先アドレスの指定が正しくありません dest:{0}", addrstring));
            }

            uint src_datasize = U.atoi0x(U.at(sp, 2));
            if (src_datasize <= 0)
            {
                throw new PatchException(R.Error("コピー元のアドレスの指定が正しくありません"));
            }

            uint dest_datasize = U.atoi0x(U.at(sp, 3));
            if (dest_datasize <= 0)
            {
                throw new PatchException(R.Error("コピー先のアドレスの指定が正しくありません"));
            }

            string filenameOnlyName = Path.GetFileName(filename);
            uint src_addr = convertBinAddressString(filenameOnlyName, 8, 0, basedir);
            if (!U.isSafetyOffset(src_addr))
            {
                throw new PatchException(R.Error("コピー元のアドレスの指定が正しくありません"));
            }

            byte[] s = Program.ROM.getBinaryData(src_addr, src_datasize);
            s = U.ResizeArray(s, dest_datasize);

            WriteBB(dest_addr, filename, s, binBlocks, undodata);
        }
        void BinWriteSLIDE(string[] sp, string filename, List<BinBlock> binBlocks, Undo.UndoData undodata)
        {
            string basedir = Path.GetDirectoryName(filename);

            string addrstring = sp[1];
            uint dest_addr = convertBinAddressString(addrstring, 8, 0x100, basedir);
            if (!U.isSafetyOffset(dest_addr))
            {
                throw new PatchException(R.Error("SLIDE先アドレスの指定が正しくありません dest:{0}", addrstring));
            }

            uint src_datasize = U.atoi0x(U.at(sp, 2));
            if (src_datasize <= 0)
            {
                throw new PatchException(R.Error("SLIDE元のアドレスの指定が正しくありません"));
            }

            uint dest_datasize = U.atoi0x(U.at(sp, 3));
            if (dest_datasize <= 0)
            {
                throw new PatchException(R.Error("SLIDE先のアドレスの指定が正しくありません"));
            }

            string filenameOnlyName = Path.GetFileName(filename);
            uint src_addr = convertBinAddressString(filenameOnlyName, 8, 0, basedir);
            if (!U.isSafetyOffset(src_addr))
            {
                throw new PatchException(R.Error("SLIDE元のアドレスの指定が正しくありません"));
            }

            if (src_addr > dest_addr)
            {
                U.Swap(ref src_addr, ref dest_addr);
            }

            byte[] s = Program.ROM.getBinaryData(src_addr, src_datasize);
            s = U.ResizeArray(s, src_datasize);

            uint slideSize = dest_addr - src_addr;
            byte[] d = Program.ROM.getBinaryData(src_addr + src_datasize, slideSize);
            d = U.ResizeArray(d, slideSize);

            //sをdの末尾データと入れ替える
            Array.Copy(s, 0, d, slideSize - src_datasize, src_datasize);

            WriteBB(src_addr, filename, d, binBlocks, undodata);
        }
        void BinWriteJump(string[] sp, string filename, List<BinBlock> binBlocks, Undo.UndoData undodata)
        {
            string basedir = Path.GetDirectoryName(filename);
            string addrstring = sp[1];
            uint injection_addr = convertBinAddressString(addrstring, 8, 0x100, basedir);
            if (!U.isSafetyOffset(injection_addr))
            {
                throw new PatchException(R.Error("割り込み元のアドレス 0x{0} は2で割り切れる偶数である必要があります", addrstring));
            }
            if (injection_addr % 2 != 0)
            {
                throw new PatchException(R.Error("割り込み元のアドレス 0x{0} は2で割り切れる偶数である必要があります", addrstring));
            }

            uint usereg = 0;
            if (sp.Length > 2)
            {
                if (sp[2].Length >= 3 && (sp[2][2] >= '0' && sp[2][2] <= '7'))
                {
                    usereg = U.atoi(sp[2].Substring(2)); //$r1
                }
                else if (sp[2] == "$NONE" || sp[2] == "$BL" || sp[2] == "$B")
                {//ジャンプ命令ではなく 直地、BL , B
                    usereg = U.NOT_FOUND;
                }
                else
                {
                    throw new PatchException(R.Error("レジスタ指定が正しくありません.\r\nJUMP:addr:$r7=filename ように定義してください."));
                }
            }

            int addaddr = 0;
            if (sp.Length > 3)
            {
                if (sp[3][0] == '-')
                {
                    addaddr = -1 * (int)U.atoi0x(sp[3].Substring(1));
                }
                else if (sp[3][0] == '+')
                {
                    addaddr = (int)U.atoi0x(sp[3].Substring(1));
                }
                else if (sp[3][0] >= '0' && sp[3][0] <= '9')
                {
                    addaddr = (int)U.atoi0x(sp[3]);
                }
                else
                {
                    throw new PatchException(R.Error("アドレス補正値が正しくありません.\r\nJUMP:addr:$r7:+0x100=filename ように定義してください."));
                }
            }

            uint add_routine_addr = U.NOT_FOUND;
            foreach (BinBlock bb in binBlocks)
            {
                if (bb.filename == filename)
                {
                    add_routine_addr = (uint)(((int)bb.addr) + addaddr);
                    break;
                }
            }
            if (add_routine_addr == U.NOT_FOUND)
            {//ファイルがない
                add_routine_addr = ItJumpAddr(filename);
                if (!U.isSafetyOffset(add_routine_addr))
                {//数字としても解釈できない
                    throw new PatchException(R.Error("ジャンプ先のファイル名 {0} がありません", filename));
                }
            }

            byte[] b;
            if (usereg == U.NOT_FOUND)
            {//直地,BL,B
                if (sp[2] == "$NONE")
                {//ジャンプ命令ではなく
                    b = new byte[4];
                    U.write_p32(b, 0, add_routine_addr);
                }
                else if (sp[2] == "$B")
                {//B Jump
                    b = DisassemblerTrumb.MakeBJump(injection_addr, add_routine_addr);
                }
                else if (sp[2] == "$BL")
                {//BL Jump
                    b = DisassemblerTrumb.MakeBLJump(injection_addr, add_routine_addr);
                }
                else
                {
                    throw new PatchException(R.Error("不明な命令です"));
                }
            }
            else
            {//ジャンプコードを生成する
                b = DisassemblerTrumb.MakeInjectJump(injection_addr, add_routine_addr, usereg);
            }

            //割り当てるアドレスを求める
            uint addr = convertBinAddressString(sp[1], (uint)b.Length, 0x100 , basedir);
            if (!U.CheckZeroAddressWrite(addr))
            {
                throw new SyntaxException(R.Error("アドレス0番地-0x100番地には書き込むことができません。", U.To0xHexString(addr)));
            }
            WriteBB(addr, filename, b, binBlocks, undodata);
        }
        void BinWriteTEXT(string[] sp,string filename,List<BinBlock> binBlocks,Undo.UndoData undodata)
        {
            string basedir = Path.GetDirectoryName(filename);
            string addrstring = sp[1];
            uint id = convertBinAddressString(addrstring, 8, 0x100, basedir);
            if (!U.isSafetyOffset(id) )
            {
                id = U.toOffset(U.atoi0x(addrstring));
                if (id == U.NOT_FOUND)
                {
                    throw new PatchException(R.Error("アドレスの指定が正しくありません dest:{0}", addrstring));
                }
            }

            string writetext = File.ReadAllText(filename);
            if (sp[0] == "TEXTADV")
            {
                writetext = TextForm.ConvertFEditorToEscape(writetext);
            }

            uint MaxTextCount = TextForm.GetDataCount();
            uint TextBaseAddress = TextForm.GetBaseAddress();


            uint addr;
            if (id < MaxTextCount)
            {
                addr = TextForm.WriteText(TextBaseAddress
                    , MaxTextCount
                    , id
                    , writetext
                    , undodata);
            }
            else if (U.isSafetyPointer(id))
            {
                uint p_text_pointer = U.toOffset(id);
                uint text_pointer = Program.ROM.u32(p_text_pointer);
                if (!U.isSafetyPointer(text_pointer))
                {
                    throw new PatchException(R.Error("ポインタではありません" + sp[1]));
                }
                uint new_textpointer = CStringForm.WriteCString(text_pointer, writetext);
                if (new_textpointer == U.NOT_FOUND)
                {
                    throw new PatchException(R.Error("書き込みに失敗しました" + sp[1]));
                }

                Program.ROM.write_p32(p_text_pointer, new_textpointer, undodata);

                addr = U.toOffset(new_textpointer);
            }
            else
            {
                return;
            }

            binBlocks.Add(new BinBlock(addr, filename));
        }
        void BinWriteExtebds(string[] sp, string filename, List<BinBlock> binBlocks, Undo.UndoData undodata)
        {
            string basedir = Path.GetDirectoryName(filename);
            string addrstring = sp[1];
            uint id = convertBinAddressString(addrstring, 8, 0x100, basedir);
            if (!U.isSafetyOffset(id) )
            {
                id = U.toOffset(U.atoi0x(addrstring));
                if (id == U.NOT_FOUND)
                {
                    throw new PatchException(R.Error("アドレスの指定が正しくありません dest:{0}", addrstring));
                }
            }
            string typename = Path.GetFileName(filename);

            uint addr;
            if (typename == "TEXT")
            {
                addr = TextForm.ExpandsArea(this,id,undodata);
            }
            else
            {
                throw new PatchException(R.Error("拡張の指定が正しくありません type:{0}", typename));
            }
            if (addr == U.NOT_FOUND)
            {
                throw new PatchException(R.Error("{0}テーブルの拡張に失敗しました", typename));
            }

            binBlocks.Add(new BinBlock(addr, typename));
        }

        uint ItJumpAddr(string filename)
        {
            if (File.Exists(filename))
            {//同名のファイルがある場合、数字ではないだろう.
                return U.NOT_FOUND;
            }

            string name = Path.GetFileName(filename);

            if (name.IndexOf('.') >= 0)
            {//拡張子のピリオドがあるとアドレス値ではない。アドレスに小数点はないのだから
                return U.NOT_FOUND;
            }
            if (name.Length <= 2)
            {
                return U.NOT_FOUND;
            }

            //先頭に0xがあって数字として解釈できるか?
            if (name[0] == '0' && name[1] == 'x')
            {
                if (U.isHexString(name.Substring(2)))
                {
                    return U.atoi0x(name);
                }
            }

            return U.NOT_FOUND;
        }


        static bool[] MakeMaskAddress(byte[] original, uint base_address)
        {
            bool[] isSkip = new bool[original.Length];

            base_address = U.toOffset(base_address);
            if (!U.isSafetyOffset(base_address))
            {
                return isSkip;
            }

            List<DisassemblerTrumb.LDRPointer> ldr = DisassemblerTrumb.MakeLDRMap(original, 0);

            uint base_pointer = U.toPointer(base_address);
            for (int i = 0; i < ldr.Count; i++)
            {
                if (ldr[i].ldr_data >= base_pointer
                    && ldr[i].ldr_data <= base_pointer + original.Length)
                {
                    isSkip[ldr[i].ldr_data_address + 0] = true;
                    isSkip[ldr[i].ldr_data_address + 1] = true;
                    isSkip[ldr[i].ldr_data_address + 2] = true;
                    isSkip[ldr[i].ldr_data_address + 3] = true;
                }
            }
            return isSkip;
        }
        static bool[] MakeIFRMaskAddress(uint length,uint blocksize,uint[] pointerIndexes)
        {
            bool[] isSkip = new bool[length];

            for (uint i = 0; i < length; i+= blocksize)
            {
                foreach (uint pointerIndex in pointerIndexes)
                {
                    isSkip[i + pointerIndex + 0] = true;
                    isSkip[i + pointerIndex + 1] = true;
                    isSkip[i + pointerIndex + 2] = true;
                    isSkip[i + pointerIndex + 3] = true;
                }
            }
            return isSkip;
        }


        static byte[] ReadMod(string[] sp, string filename, out bool[] isSkip)
        {
            if (! File.Exists(filename))
            {
                isSkip = new bool[0]{};
                return new byte[0] {};
            }
            //ファイルを読み込んで、
            byte[] b = File.ReadAllBytes(filename);

            //アドレスの移動がある場合は、マスクパターンを作ります.
            uint chaddr = U.atoi0x(U.at(sp, 2));
            isSkip = MakeMaskAddress(b, chaddr);

            return b;
        }
        static byte[] ReadMod(string[] sp, byte[] b, out bool[] isSkip)
        {
            //アドレスの移動がある場合は、マスクパターンを作ります.
            uint chaddr = U.atoi0x(U.at(sp, 2));
            isSkip = MakeMaskAddress(b, chaddr);

            return b;
        }

        //lynによってインポートされるelfのマスクパターンを作ります。
        //
        static bool[] MakeLynMaskPattern(byte[] bin)
        {
            bool[] isSkip = new bool[bin.Length];
            for (int i = 0; i + 3 < bin.Length ; i += 4)
            {
                if (bin[i] == 0 && bin[i+1] == 0 && bin[i+2] == 0 && bin[i+3] == 0 )
                {
                    isSkip[i + 0] = true;
                    isSkip[i + 1] = true;
                    isSkip[i + 2] = true;
                    isSkip[i + 3] = true;
                }
            }
            return isSkip;
        }

        //本来配置する場所から移動した場合、
        //データ参照のLDR値を変更しないといけいない.
        byte[] ChangeAddress(byte[] original,uint alloc_address, uint base_address)
        {
            alloc_address = U.toOffset(alloc_address);
            base_address = U.toOffset(base_address);
            if (! U.isSafetyOffset(base_address) )
            {
                return original;
            }

            List<DisassemblerTrumb.LDRPointer> ldr = DisassemblerTrumb.MakeLDRMap(original,0);
            uint base_pointer = U.toPointer(base_address);
            uint alloc_pointer = U.toPointer(alloc_address);
            for (int i = 0; i < ldr.Count; i++)
            {
                if (ldr[i].ldr_data >= base_pointer 
                    && ldr[i].ldr_data <= base_pointer + original.Length)
                {
                    uint new_address = alloc_pointer + (ldr[i].ldr_data - base_pointer);
                    U.write_u32(original, ldr[i].ldr_data_address, new_address);
                }
            }
            return original;
        }
        static ConcurrentDictionary<string, string> CacheCheckIF = new ConcurrentDictionary<string, string>();

        public static void ClearCheckIF()
        {
            CacheCheckIF.Clear();
        }

        static string CheckIFFast(PatchSt patch)
        {
            string ret;
            if (CacheCheckIF.TryGetValue(patch.PatchFileName, out ret))
            {
                return ret;
            }
            return CheckIF(patch,false);
        }

        static string CheckIF(PatchSt patch,bool isDetail)
        {
            foreach (var pair in patch.Param)
            {
                string[] sp = pair.Key.Split(':');
                string key = sp[0];
                string addrstring = U.at(sp, 1);
                string value = pair.Value;

                bool isnot = false;
                if (key != "IF" && key != "PATCHED_IFNOT")
                {
                    if (key != "IFNOT" && key != "PATCHED_IF" && key != "CONFLICT_IF")
                    {
                        continue;
                    }
                    isnot = true;
                }

                string basedir = Path.GetDirectoryName(patch.PatchFileName);
                uint address = convertBinAddressString(addrstring, 0, 0x100, basedir);
                if (!U.isSafetyOffset(address) )
                {
                    if (!isnot)
                    {
                        CacheCheckIF[patch.PatchFileName] = "E";
                        if (!isDetail)
                        {
                            return "E";
                        }
                        else
                        {
                            return R._("前提条件となるパッチ等がないようです。\r\n{0}\r\n", pair.Value)
                                   + U.at(patch.Param, "IF_COMMENT")
                                   ;
                        }
                    }
                    continue;
                }

                if (!U.isSafetyOffset(address))
                {
                    if (!isnot)
                    {
                        CacheCheckIF[patch.PatchFileName] = "E";
                        if (!isDetail)
                        {
                            return "E";
                        }
                        else
                        {
                            throw new SyntaxException(R.Error("IFパースエラー、このアドレス({2})は危険です。\r\n{0}={1}", pair.Key, pair.Value, U.To0xHexString(address)));
                        }
                    }
                    continue;
                }

                string[] args = value.Split(' ');
                if (args.Length <= 1)
                {
                    CacheCheckIF[patch.PatchFileName] = "E";
                    if (!isDetail)
                    {
                        return "E";
                    }
                    throw new SyntaxException(R.Error("IFパースエラー、この値にはスペースがありません\r\n{0}={1}", pair.Key, pair.Value));
                }


                uint[] data = new uint[args.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    data[i] = Program.ROM.u8(address + (uint)i);
                }
                uint[] need = new uint[args.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    need[i] = U.atoi0x(args[i]);
                }

                bool notFound = false;
                for (int i = 0; i < args.Length; i++)
                {
                    if (data[i] != need[i])
                    {
                        notFound = true;
                        break;
                    }
                }

                if (isnot)
                {
                    if (notFound == false)
                    {
                        if (key == "PATCHED_IF")
                        {//インストール済み
                            CacheCheckIF[patch.PatchFileName] = "I";
                            if (!isDetail)
                            {
                                return "I";
                            }
                        }
                        else
                        {
                            CacheCheckIF[patch.PatchFileName] = "E";
                            if (!isDetail)
                            {
                                return "E";
                            }
                        }

                        return pair.Key + "=" + pair.Value + "\r\n"
                            + "Address:" + U.To0xHexString(address) + "\r\n"
                            + "Not Need:" + U.DumpByte(need) + "\r\n"
                            + "ROM Data:" + U.DumpByte(data) + "\r\n"
                            + U.at(patch.Param, "IF_COMMENT")
                                ;
                    }
                }
                else
                {
                    if (notFound == true)
                    {
                        if (key == "PATCHED_IF")
                        {//インストール済み
                            CacheCheckIF[patch.PatchFileName] = "I";
                            if (!isDetail)
                            {
                                return "I";
                            }
                        }
                        else
                        {
                            CacheCheckIF[patch.PatchFileName] = "E";
                            if (!isDetail)
                            {
                                return "E";
                            }
                        }

                        return pair.Key + "=" + pair.Value + "\r\n"
                            + "Address:" + U.To0xHexString(address) + "\r\n"
                            + "Need   :" + U.DumpByte(need) + "\r\n"
                            + "ROM Data:" + U.DumpByte(data) + "\r\n"
                            + U.at(patch.Param, "IF_COMMENT")
                                ;
                    }
                }
            }

            CacheCheckIF[patch.PatchFileName] = "";
            return "";
        }

        string CheckIF(PatchSt patch)
        {
            string error = CheckIF(patch,true);
            if (error != "")
            {
                return error;
            }

            //それぞれの動作に必要な項目のチェック
            string type = U.at(patch.Param, "TYPE");
            if (type == "EA")
            {
                string event_assembler = Program.Config.at("event_assembler", "");
                if (event_assembler == "")
                {
                    return R.Error("{0}の設定がありません。 設定->オプションから、{0}を設定してください。", "event_assembler");
                }
            }

            return "";
        }

        public class BinMapping
        {
            public string key;
            public string filename;
            public uint addr;
            public uint length;
            public Address.DataTypeEnum type;
            public byte[] bin;
            public bool[] mask;
        };

        //BINパッチがどこにマップされたのか追跡して表示する.
        static List<BinMapping> TracePatchedMapping(PatchSt patch)
        {
            List<BinMapping> binMappings = new List<BinMapping>();

            string type = U.at(patch.Param, "TYPE");
            if (type == "BIN")
            {//BINパッチ. 
                return TraceBINPatchedMapping(patch);
            }
            if (type == "EA")
            {//EAパッチ. 
                return TraceEAPatchedMapping(patch);
            }

            return binMappings;
        }

        static void TraceEditPatch(List<BinMapping> binMappings, PatchSt patchSt)
        {
            foreach (var pair in patchSt.Param)
            {
                string[] sp = pair.Key.Split(':');
                string key = sp[0];

                if (key == "EDIT_PATCH" && pair.Value != "")
                {
                    TraceEditPatchLow(pair.Value, binMappings, patchSt);
                }
            }
        }

        static void TraceEditPatchLow(string editpatch, List<BinMapping> binMappings, PatchSt patchSt)
        {
            string basedir = Path.GetDirectoryName(patchSt.PatchFileName);
            editpatch = Path.Combine(basedir, editpatch);

            if (!File.Exists(editpatch))
            {
                Debug.Assert(false);
                return;
            }
            PatchSt editpatchSt = LoadPatch(editpatch, true);
            if (editpatchSt == null)
            {
                Debug.Assert(false);
                return;
            }
            string type = U.at(editpatchSt.Param, "TYPE");
            if (type == "STRUCT")
            {
                TraceEditPatchStruct(binMappings, editpatchSt, basedir);
            }
            else if (type == "IMAGE")
            {//画像
                TraceEditPatchImage(binMappings, editpatchSt);
            }
            else if (type == "BIN" || type == "EA")
            {//BINパッチまたは、EAでネストしている場合
                TraceEditPatchNest(binMappings, editpatchSt);
            }
        }
        static void TraceEditPatchNest(List<BinMapping> binMappings, PatchSt editpatchSt)
        {
            List<BinMapping> nest = TracePatchedMapping(editpatchSt);
            binMappings.AddRange(nest);
        }
        static void TraceEditPatchImage(List<BinMapping> binMappings, PatchSt editpatchSt)
        {
            List<Address> list = new List<Address>();
            MakePatchStructDataListForIMAGE(list, false, editpatchSt);

            foreach(Address a in list)
            {
                BinMapping binmap = new BinMapping();
                binmap.addr = a.Addr;
                binmap.filename = a.Info;
                binmap.key = "DATA";
                binmap.length = a.Length;
                binmap.bin = Program.ROM.getBinaryData(binmap.addr, binmap.length);
                binmap.mask = new bool[binmap.addr]; //画像なのでマスクは不要
                binMappings.Add(binmap);
            }
        }

        static void TraceEditPatchStruct(List<BinMapping> binMappings, PatchSt editpatchSt, string basedir)
        {
            uint struct_address = 0x0;
            while (true)
            {//パッチを複数個インストールしている可能性の探索
                struct_address = TraceEditPatchStructInner(binMappings, editpatchSt, struct_address, basedir);

                if (struct_address == U.NOT_FOUND)
                {
                    break;
                }

                struct_address += 16;
            }
        }

        static uint TraceEditPatchStructInner(List<BinMapping> binMappings, PatchSt editpatchSt, uint search_start_addr, string basedir)
        {
            uint struct_address;
            string pointer_str = U.at(editpatchSt.Param, "POINTER");
            if (pointer_str == "")
            {
                string address_str = U.at(editpatchSt.Param, "ADDRESS");
                if (address_str == "")
                {
                    return U.NOT_FOUND;
                }
                struct_address = convertBinAddressString(pointer_str, 8, search_start_addr, basedir);
                if (!U.isSafetyOffset(struct_address))
                {
                    return U.NOT_FOUND;
                }
            }
            else
            {
                uint addr = convertBinAddressString(pointer_str, 8, search_start_addr, basedir);
                if (!U.isSafetyOffset(addr))
                {
                    return U.NOT_FOUND;
                }
                BinMapping binmap = new BinMapping();
                binmap.addr = addr;
                binmap.filename = "";
                binmap.key = "POINTER";
                binmap.length = 4;
                binmap.bin = Program.ROM.getBinaryData(addr, 4);
                binmap.mask = new bool[] { true, true, true, true };
                binMappings.Add(binmap);

                struct_address = Program.ROM.p32(addr);
                if (!U.isSafetyOffset(struct_address))
                {
                    return U.NOT_FOUND;
                }
            }

            if (struct_address < search_start_addr)
            {
                return U.NOT_FOUND;
            }

            uint datasize = U.atoi0x(U.at(editpatchSt.Param, "DATASIZE"));

            uint datacount;
            string datacount_str = U.at(editpatchSt.Param, "DATACOUNT");
            if (datacount_str.Length > 0 && datacount_str[0] == '$')
            {//grep等
                datacount = convertBinAddressString(datacount_str, 8, struct_address, basedir);
                if (datacount == U.NOT_FOUND)
                {
                    return U.NOT_FOUND;
                }
                if (datacount > struct_address)
                {
                    datacount = (uint)Math.Ceiling((datacount - struct_address) / (double)datasize);
                }
            }
            else
            {//直値
                datacount = U.atoi0x(datacount_str);
            }
            if (datacount <= 0)
            {
                return U.NOT_FOUND;
            }

            {
                BinMapping binmap = new BinMapping();
                binmap.addr = struct_address;
                binmap.filename = "";
                binmap.key = "DATA";
                binmap.length = datacount * datasize;
                binmap.bin = Program.ROM.getBinaryData(struct_address, binmap.length);
                binmap.mask = new bool[binmap.addr]; //データなのでマスクは不要
                binMappings.Add(binmap);
            }
            return struct_address;
        }

        //BINパッチがどこにマップされたのか追跡して表示する.
        static List<BinMapping> TraceBINPatchedMapping(PatchSt patch)
        {
            string type = U.at(patch.Param, "TYPE");
            Debug.Assert(type == "BIN");

            List<BinMapping> binMappings = new List<BinMapping>();
            uint lastMatchAddr = Program.ROM.RomInfo.compress_image_borderline_address();

            Dictionary<string, bool> jumpMatch = new Dictionary<string, bool>();
            foreach (var pair in patch.Param)
            {
                string[] sp = pair.Key.Split(':');
                string key = sp[0];
                string value = pair.Value;

                if (key != "JUMP")
                {
                    continue;
                }

                string basedir = Path.GetDirectoryName(patch.PatchFileName);
                uint addr = convertBinAddressString(sp[1], 0, 0x100, basedir);
                if (!U.isSafetyOffset(addr))
                {
                    continue;
                }

                uint length;
                Address.DataTypeEnum datatype;
                string sp2 = U.at(sp, 2);
                if (sp2 == "$NONE")
                {//ジャンプ命令ではなく
                    length = 4;
                    if (addr <= Program.ROM.RomInfo.compress_image_borderline_address())
                    {
                        datatype = Address.DataTypeEnum.POINTER_ASM;
                    }
                    else
                    {
                        datatype = Address.DataTypeEnum.POINTER;
                    }
                }
                else if (sp2 == "$B")
                {//B Jump
                    length = 2;
                    datatype = Address.DataTypeEnum.BIN;
                }
                else if (sp2 == "$BL")
                {//BL Jump
                    length = 2;
                    datatype = Address.DataTypeEnum.BIN;
                }
                else
                {//ジャンプコードを生成する
                    if (addr % 4 != 0)
                    {//4バイトアライメントをみたせない場合 NOPを追加
                        length = 10;
                    }
                    else
                    {
                        length = 8;
                    }
                    datatype = Address.DataTypeEnum.JUMPTOHACK;
                }

                if (addr != U.NOT_FOUND)
                {
                    BinMapping b = new BinMapping();
                    b.key = pair.Key;
                    b.filename = "$JUMP:" + pair.Value;
                    b.addr = addr;
                    b.length = length;
                    b.type = datatype;
                    b.bin = Program.ROM.getBinaryData(addr, length);
                    b.mask = new bool[b.length]; //all false

                    binMappings.Add(b);

                    jumpMatch[pair.Value] = true;
                }

            }

            foreach (var pair in patch.Param)
            {
                string[] sp = pair.Key.Split(':');
                string key = sp[0];
                string value = pair.Value;
                byte[] bin;
                bool[] mask;

                if (!(key == "BIN" || key == "BINP" || key == "BINAP" || key == "BINF"))
                {
                    continue;
                }
                if (sp.Length < 2)
                {
                    continue;
                }

                uint addr;
                uint length;
                FEBuilderGBA.Address.DataTypeEnum datatype = Address.DataTypeEnum.MIX;
                if (key == "BINP")
                {
                    datatype = Address.DataTypeEnum.POINTER;
                }
                else if (key == "BINAP")
                {
                    datatype = Address.DataTypeEnum.POINTER_ASM;
                }
                else if (key == "BINF")
                {
                    datatype = Address.DataTypeEnum.BIN;
                }

                if (sp[1] == "$FREEAREA")
                {//フリーエリア設置
                    //展開されるものを生成して、GREP検索する必要があります.
                    string basedir = Path.GetDirectoryName(patch.PatchFileName);
                    string filename = Path.Combine(basedir, value);
                    bool[] isSkip;
                    byte[] mod = ReadMod(sp, filename, out isSkip);

                    //ポインタ部は可変なので、maskパターンを作って検索します.
                    addr = U.GrepPatternMatch(Program.ROM.Data, mod, isSkip, lastMatchAddr);
                    if (addr == U.NOT_FOUND)
                    {
                        continue;
                    }
                    length = (uint)mod.Length;
                    bin = Program.ROM.getBinaryData(addr, length);
                    mask = isSkip;

                    if (jumpMatch.ContainsKey(value))
                    {
                        datatype = Address.DataTypeEnum.ASM;
                    }
                    //フリー領域は連続して配置されるので、最後に発見したもののアドレスを記録する.
                    lastMatchAddr = addr + length;
                }
                else
                {//位置が特定できる場合
                    string basedir = Path.GetDirectoryName(patch.PatchFileName);
                    addr = convertBinAddressString(sp[1], 0, 0x100, basedir);

                    //インストールされるファイルを特定する
                    string filename = Path.Combine(basedir, value);
                    bool[] isSkip;
                    byte[] mod = ReadMod(sp, filename, out isSkip);
                    length = (uint)mod.Length;

                    bin = mod;
                    mask = isSkip;
                }


                if (addr != U.NOT_FOUND)
                {
                    BinMapping b = new BinMapping();
                    b.key = pair.Key;
                    b.filename = pair.Value;
                    b.addr = addr;
                    b.length = length;
                    b.type = datatype;
                    b.bin = bin;
                    b.mask = mask;

                    binMappings.Add(b);
                }

            }

            foreach (var pair in patch.Param)
            {
                string[] sp = pair.Key.Split(':');
                string key = sp[0];
                string value = pair.Value;
                byte[] bin;
                bool[] mask;

                if (!(key == "SLIDE"))
                {
                    continue;
                }
                if (sp.Length < 3)
                {
                    continue;
                }

                FEBuilderGBA.Address.DataTypeEnum datatype = Address.DataTypeEnum.MIX;

                uint addr = U.atoi0x(sp[1]);
                uint dest_addr = U.atoi0x(value);
                if (!U.isSafetyOffset(addr)
                    || !U.isSafetyOffset(dest_addr))
                {
                    continue;
                }

                uint length = dest_addr - addr;
                bin = Program.ROM.getBinaryData(addr, length);
                mask = new bool[length];

                BinMapping b = new BinMapping();
                b.key = pair.Key;
                b.filename = pair.Value;
                b.addr = addr;
                b.length = length;
                b.type = datatype;
                b.bin = bin;
                b.mask = mask;

                binMappings.Add(b);
            }
            foreach (var pair in patch.Param)
            {
                string[] sp = pair.Key.Split(':');
                string key = sp[0];
                string value = pair.Value;
                byte[] bin;
                bool[] mask;

                if (!(key == "CLEAR"))
                {
                    continue;
                }
                if (sp.Length < 2)
                {
                    continue;
                }

                FEBuilderGBA.Address.DataTypeEnum datatype = Address.DataTypeEnum.BIN;

                uint addr = U.atoi0x(sp[1]);
                if (!U.isSafetyOffset(addr))
                {
                    continue;
                }

                uint length = U.atoi0x(sp[2]);
                bin = Program.ROM.getBinaryData(addr, length);
                mask = new bool[length];

                BinMapping b = new BinMapping();
                b.key = pair.Key;
                b.filename = pair.Value;
                b.addr = addr;
                b.length = length;
                b.type = datatype;
                b.bin = bin;
                b.mask = mask;

                binMappings.Add(b);
            }


            //データの位置を追跡
            {
                TraceEditPatch(binMappings, patch);
            }
            return binMappings;
        }
        //EAパッチがどこにマップされたのか追跡して表示する.
        static List<BinMapping> TraceEAPatchedMapping(PatchSt patch)
        {
            string type = U.at(patch.Param, "TYPE");
            Debug.Assert(type == "EA");

            List<BinMapping> binMappings = new List<BinMapping>();

            string dir = Path.GetDirectoryName(patch.PatchFileName);
            string[] files = Directory.GetFiles(dir, "*.event", SearchOption.AllDirectories);

            //メイン処理のファイルが.txtなどで発見できていない場合、追加してあげる.
            {
                string ea = U.at(patch.Param, "EA");
                if (U.GetFilenameExt(ea) != ".EVENT")
                {
                    ea = Path.Combine(dir, ea);
                    files = U.AddIfNotExist(files, ea);
                }
            }

            uint lastMatchAddr = Program.ROM.RomInfo.compress_image_borderline_address();

            foreach (string fullfilename in files)
            {
                if (EAUtil.IsFBGTemp(fullfilename))
                {
                    continue;
                }

                EAUtil ea = new EAUtil(fullfilename);
                for (int n = 0; n < ea.DataList.Count; n++)
                {
                    EAUtil.Data data = ea.DataList[n];
                    if (data.DataType == EAUtil.DataEnum.ORG)
                    {
                        uint addr = data.ORGAddr;

                        BinMapping b = new BinMapping();
                        b.key = "ORG";
                        b.filename = "";
                        b.addr = addr;
                        b.length = 0; //不明
                        b.type = Address.DataTypeEnum.MIX;

                        if (U.isSafetyOffset(addr + 64))
                        {//長さが不明なので比較するとき困るので適当に64バイトほど取得します.
                            b.bin = Program.ROM.getBinaryData(addr, 64);
                            b.mask = MakeMaskAddress(b.bin, addr);
                        }
                        else
                        {
                            b.bin = new byte[0] { };
                            b.mask = new bool[0] { };
                        }

                        binMappings.Add(b);
                    }
                    else if (data.DataType == EAUtil.DataEnum.ASM 
                        || data.DataType == EAUtil.DataEnum.MIX)
                    {
                        //展開されるものを生成して、GREP検索する必要があります.
                        string basedir = Path.GetDirectoryName(patch.PatchFileName);
                        string[] sp = new string[] { };
                        bool[] isSkip;
                        byte[] mod = ReadMod(sp, data.BINData, out isSkip);

                        //可変なので、maskパターンを作って検索します.
                        uint addr = U.GrepPatternMatch(Program.ROM.Data, mod, isSkip, lastMatchAddr);
                        if (addr == U.NOT_FOUND)
                        {
                            continue;
                        }

                        uint length = (uint)mod.Length;

                        BinMapping b = new BinMapping();
                        b.key = data.DataType.ToString();
                        b.filename = data.Name;
                        b.addr = addr;
                        b.length = length;
                        b.bin = Program.ROM.getBinaryData(addr, length);
                        b.mask = isSkip;
                        if (data.DataType == EAUtil.DataEnum.ASM)
                        {
                            b.type = Address.DataTypeEnum.PATCH_ASM;
                        }
                        else
                        {
                            b.type = Address.DataTypeEnum.MIX;
                        }

                        binMappings.Add(b);

                        //最後に発見したアドレスを追加
                        lastMatchAddr = addr + length;
                    }
                    else if (data.DataType == EAUtil.DataEnum.LYN)
                    {
                        //展開されるものを生成して、GREP検索する必要があります.
                        string basedir = Path.GetDirectoryName(patch.PatchFileName);
                        string[] sp = new string[] { };
                        bool[] isSkip = MakeLynMaskPattern(data.BINData);

                        //可変なので、maskパターンを作って検索します.
                        uint addr = U.GrepPatternMatch(Program.ROM.Data, data.BINData, isSkip, lastMatchAddr);
                        if (addr == U.NOT_FOUND)
                        {
                            continue;
                        }
                        uint length = (uint)data.BINData.Length;

                        BinMapping b = new BinMapping();
                        b.key = data.DataType.ToString();
                        b.filename = data.Name;
                        b.addr = addr;
                        b.length = length;
                        b.bin = Program.ROM.getBinaryData(addr, length);
                        b.mask = isSkip;
                        b.type = Address.DataTypeEnum.PATCH_ASM;

                        binMappings.Add(b);

                        //最後に発見したアドレスを追加
                        lastMatchAddr = addr + length;
                    }
                    else
                    {
                        //展開されるものを生成して、GREP検索する必要があります.
                        string basedir = Path.GetDirectoryName(patch.PatchFileName);

                        uint addr = U.Grep(Program.ROM.Data, data.BINData, lastMatchAddr);
                        if (addr == U.NOT_FOUND)
                        {
                            continue;
                        }
                        uint length = (uint)data.BINData.Length;

                        BinMapping b = new BinMapping();
                        b.key = data.DataType.ToString();
                        b.filename = data.Name;
                        b.addr = addr;
                        b.length = length;
                        b.bin = data.BINData;
                        b.mask = MakeMaskAddress(b.bin, addr);
                        b.type = Address.DataTypeEnum.BIN;

                        binMappings.Add(b);

                        //最後に発見したアドレスを追加
                        lastMatchAddr = addr + length;
                    }
                }
            }

            //データの位置を追跡
            TraceEditPatch(binMappings, patch);
            return binMappings;
        }

        static string atMultiLine(PatchSt patch,string keyword)
        {
            string r = U.at(patch.Param, keyword);
            r = r.Replace("\\r\\n", "\r\n");
            r = r.Replace("\n", "\r\n");
            return r;
        }

        string MakeConflictMessage(PatchSt patch)
        {
            string conflict_message = atMultiLine(patch, "CONFLICT_MESSAGE");

            if (conflict_message == "")
            {
                return "";
            }
            return R._("以下の競合するパッチのいずれかがインストールされているため、利用できません。") + "\r\n" + conflict_message;
        }

        string RepaceStringForBinMAP(string text, List<BinMapping> map)
        {
            if (text == "")
            {
                return text;
            }

            for (int i = 0; i < map.Count; i++)
            {
                text = ReplaceL1Macro(text, map[i].filename, map[i].addr);
            }
            return text;
        }

        string MakePatchedMessage(PatchSt patch)
        {

            string mapping = "";
            List<BinMapping> map = TracePatchedMapping(patch);
            for (int i = 0; i < map.Count; i++ )
            {
                mapping += map[i].key + "=" + map[i].filename + " -----> " + U.To0xHexString(map[i].addr) + "\r\n";
            }

            string patched_message = "";
            foreach (var pair in patch.Param)
            {
                if (pair.Key.IndexOf("EVENTSCRIPT") != 0)
                {
                    continue;
                }
                if (patched_message.Length <= 0)
                {
                    patched_message += "======== SCRIPT ==============";
                }
                patched_message += "\r\n" + pair.Value;
            }
            patched_message = RepaceStringForBinMAP(patched_message, map);

            if (mapping.Length > 0)
            {//マッピングを取得できるのであれば表示 BINのみ
                patched_message += "\r\n\r\n======== MAPPING ==============\r\n" + mapping;
            }

            return patched_message;
        }
        static string ReplaceL1Macro(string patched_message,string filename,uint addr)
        {
            uint p0 = U.toPointer(addr);
            uint p1 = U.toPointer(addr + 1);
            uint littleEndian = U.ChangeEndian32(p0);
            uint littleEndianPlus1 = U.ChangeEndian32(p1);

            patched_message = patched_message.Replace("{$B:" + filename + "}", p0.ToString("X08"));
            patched_message = patched_message.Replace("{$B1:" + filename + "}", p1.ToString("X08"));
            patched_message = patched_message.Replace("{$L:" + filename + "}", littleEndian.ToString("X08"));
            patched_message = patched_message.Replace("{$L1:" + filename + "}", littleEndianPlus1.ToString("X08"));
            return patched_message;
        }


        //EDIT_PATHを探す 
        private void PatchList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.PatchList.SelectedIndex < 0 || this.PatchList.SelectedIndex >= this.FiltedPatchs.Count)
            {
                return;
            }
            PatchSt patch = this.FiltedPatchs[this.PatchList.SelectedIndex];

            this.Text = R._("パッチ") + " " + patch.Name;
            PatchFilename.Text = patch.PatchFileName;


            try
            {
                string iferror = CheckIF(patch);
                if (iferror.Length > 0)
                {
                    string infomesg = MakeInfoAndAuthorString(patch);
                    if (iferror.IndexOf("PATCHED_IF") >= 0)
                    {
                        string mapmessage = MakePatchedMessage(patch);

                        PATCHED_TextBox.Visible = true;
                        PATCHED_TextBox.Text = iferror + "\r\n" + mapmessage + "\r\n\r\n" + infomesg;
                        TAB.SelectedTab = PatchedPage;
                    }
                    else if (iferror.IndexOf("CONFLICT_IF") >= 0)
                    {
                        string conflict_message = MakeConflictMessage(patch);

                        CONFLICT_TextBox.Visible = true;
                        CONFLICT_TextBox.Text = iferror + "\r\n" + conflict_message + "\r\n\r\n" + infomesg;
                        TAB.SelectedTab = ConflictPage;
                    }
                    else
                    {
                        ELSE_TextBox.Visible = true;
                        ELSE_TextBox.Text = iferror + "\r\n\r\n" + infomesg;
                        TAB.SelectedTab = ElsePage;
                    }
                }
                else
                {
                    LoadPatch(patch);
                    TAB.SelectedTab = PatchPage;
                }
            }
            catch (PatchException exception)
            {
                string infomesg = MakeInfoAndAuthorString(patch);

                ELSE_TextBox.Visible = true;
                ELSE_TextBox.Text = exception.Message + "\r\n"
                    + "\r\n" + infomesg;
                TAB.SelectedTab = ElsePage;
                this.ActiveControl = PatchList;
                return;
            }
            catch (SyntaxException exception)
            {
                string infomesg = MakeInfoAndAuthorString(patch);

                ERROR_TextBox.Visible = true;
                ERROR_TextBox.Text = exception.Message + "\r\n"
                    + "\r\n"
                    + exception.StackTrace
                    + "\r\n\r\n" + infomesg;
                TAB.SelectedTab = ErrorPage;
                this.ActiveControl = PatchList;
                return;
            }
            InputFormRef.ReColor(this);
            this.ActiveControl = PatchList;
        }


        private void PatchOpenButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(PatchFilename.Text);
        }

        private void RealodButton_Click(object sender, EventArgs e)
        {
            int index = this.PatchList.SelectedIndex;
            this.ReScan();
            U.SelectedIndexSafety(this.PatchList,index);
        }

        private void Filter_TextChanged(object sender, EventArgs e)
        {
            ReFilter();
            Filter.Focus();
        }

        bool checkPatchName(string patch,string PatchName, string PatchName2)
        {
            {
                string value = Path.GetFileNameWithoutExtension(patch);
                value = U.substr(value, 6); //skip "PATCH_"

                if (value == PatchName)
                {
                    return true;
                }
                if (PatchName2 != "" && value == PatchName2)
                {
                    return true;
                }
            }

            string[] lines = File.ReadAllLines(patch);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (U.IsComment(line) || U.OtherLangLine(line))
                {
                    continue;
                }
                line = U.ClipComment(line);
                line = line.Trim();

                int sep = line.IndexOf('=');
                if (sep < 0)
                {
                    continue;
                }
                string kkey = line.Substring(0, sep);
                if (!(kkey == "NAME" || kkey.IndexOf("NAME.") == 0))
                {
                    continue;
                }
                string value = line.Substring(sep + 1);
                if (value == "")
                {
                    continue;
                }

                if (value == PatchName)
                {
                    return true;
                }
                if (PatchName2 != "" && value == PatchName2)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ApplyPatch(string PatchName, string PatchName2 = "", string patchCombo = "")
        {
            //フィルターをしていたらやめさせる.
            Filter.Text = "";
            //パッチの再スキャン
            ReScan();

            //自動適応するパッチを検索
            int i = 0;
            for (; i < this.Patchs.Count; i++)
            {
                bool r = checkPatchName(this.Patchs[i].PatchFileName, PatchName, PatchName2);
                if (r)
                {
                    break;
                }
            }
            if (i >= this.Patchs.Count)
            {
                return false;
            }

            //パッチの選択
            PatchList.SelectedIndex = i;

            if (this.TAB.SelectedTab == this.PatchedPage)
            {//既に適応済み
                return true;
            }
            if (this.TAB.SelectedTab != this.PatchPage)
            {//パッチ画面ではない.エラーが発生している
                return false;
            }

            if (patchCombo != "")
            {//comboboxを選択する必要があるらしい.
                ApplyPatchSelectCombo(patchCombo);
            }

            //書き込みボタンを押す.
            return ApplyPatchWriteButton();
        }

        bool ApplyPatchSelectCombo(string patchCombo)
        {
            for (int n = 0; n < this.PatchPage.Controls.Count; n++)
            {
                Control c = this.PatchPage.Controls[n];
                if (!(c is ComboBox))
                {
                    continue;
                }
                if (c.Name != "PatchMainCombo")
                {
                    continue;
                }

                ComboBox combo = (ComboBox)c;
                combo.Text = patchCombo;
                return true;
            }
            return false;
        }
        bool ApplyPatchWriteButton()
        {
            for (int n = 0; n < this.PatchPage.Controls.Count; n++)
            {
                Control c = this.PatchPage.Controls[n];
                if (!(c is Button))
                {
                    continue;
                }
                if (c.Name != "WriteButton")
                {
                    continue;
                }

                //書き込みボタンを押す.
                Button button = (Button)c;
                button.PerformClick();

                R.ShowOK("パッチを適応しました");
                return true;
            }

            //書き込みボタンがない.
            return false;
        }


        public static bool ImportImageOneTime(string patch,Control parent,Form self, ref uint out_img, ref uint out_img2, ref uint out_tsa, ref uint out_pal)
        {
            string[] lines = patch.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            PatchSt st = LoadPatch(lines, "[Virtual]", false);
            if (st == null)
            {
                R.ShowStopError("{0}{1}\r\n{2}", "ImportImageOneTime",1,patch);
                return false;
            }

            string type = U.at(st.Param, "TYPE");
            if (type != "IMAGE")
            {
                R.ShowStopError("{0}{1}\r\n{2}", "ImportImageOneTime", 2, patch);
                return false;
            }
            LoadPatchImage(st,parent, self);

            string prefix = "PatchImage";
            int prefixLength = prefix.Length;

            Button importButton = null;
            for (int n = 0; n < parent.Controls.Count; n++)
            {
                Control c = parent.Controls[n];
                if (!(c is Button))
                {
                    continue;
                }
                string name = U.substr(c.Name,prefixLength);
                if (name.IndexOf("_Import") < 0)
                {
                    continue;
                }
                importButton = (Button)c;
                break;
            }

            if (importButton == null)
            {
                R.ShowStopError("{0}{1}\r\n{2}", "ImportImageOneTime", 3, patch);
                return false;
            }

            //インポートボタンをクリック
            importButton.PerformClick();

            //もし、書き込んだ場合は、ポインタを更新します.
            uint p;
            for (int n = 0; n < st.Param.Count; n++)
            {
                p = atOffset(st.Param, "IMAGE_POINTER");
                if (p > 0 && U.isSafetyOffset(p))
                {
                    out_img = Program.ROM.p32(p);
                }
                p = atOffset(st.Param, "ZIMAGE_POINTER");
                if (p > 0 && U.isSafetyOffset(p))
                {
                    out_img = Program.ROM.p32(p);
                }
                p = atOffset(st.Param, "Z2IMAGE_POINTER");
                if (p > 0 && U.isSafetyOffset(p))
                {
                    out_img2 = Program.ROM.p32(p);
                }

                p = atOffset(st.Param, "TSA_POINTER");
                if (U.isSafetyOffset(p))
                {
                    out_tsa = Program.ROM.p32(p);
                }
                p = atOffset(st.Param, "ZTSA_POINTER");
                if (U.isSafetyOffset(p))
                {
                    out_tsa = Program.ROM.p32(p);
                }
                p = atOffset(st.Param, "HEADERTSA_POINTER");
                if (U.isSafetyOffset(p))
                {
                    out_tsa = Program.ROM.p32(p);
                }
                p = atOffset(st.Param, "ZHEADERTSA_POINTER");
                if (U.isSafetyOffset(p))
                {
                    out_tsa = Program.ROM.p32(p);
                }

                p = atOffset(st.Param, "PALETTE_POINTER");
                if (U.isSafetyOffset(p))
                {
                    out_pal = Program.ROM.p32(p);
                }
            }

            return true;
        }
        public static bool TSAEditorImageOneTime(string patch,Control parent, Form self)
        {
            string[] lines = patch.Split(new string[]{"\r\n"},StringSplitOptions.None);
            PatchSt st = LoadPatch(lines,"[Virtual]" , false);
            if (st == null)
            {
                R.ShowStopError("{0}{1}\r\n{2}", "TSAEditorImageOneTime", 1, patch);
                return false;
            }

            string type = U.at(st.Param, "TYPE");
            if (type != "IMAGE")
            {
                R.ShowStopError("{0}{1}\r\n{2}", "TSAEditorImageOneTime", 2, patch);
                return false;
            }
            LoadPatchImage(st, parent , self);

            string prefix = "PatchImage";
            int prefixLength = prefix.Length;

            Button tsaEditorButton = null;
            for (int n = 0; n < parent.Controls.Count; n++)
            {
                Control c = parent.Controls[n];
                if (!(c is Button))
                {
                    continue;
                }
                string name = U.substr(c.Name, prefixLength);
                if (name.IndexOf("_TSAEditor") < 0)
                {
                    continue;
                }
                tsaEditorButton = (Button)c;
                break;
            }

            if (tsaEditorButton == null)
            {
                R.ShowStopError("{0}{1}\r\n{2}", "TSAEditorImageOneTime", 3, patch);
                return false;
            }

            //TSAEditorボタンをクリック
            tsaEditorButton.PerformClick();
            return true;
        }
        static uint CalcAddrLength(PatchSt st)
        {
            string combo = U.at(st.Param, "COMBO");
            if (combo == "")
            {
                return 1;
            }
            string[] sp = combo.Split('|');
            if (sp.Length < 2)
            {
                return 1;
            }
            string[] bytesSP = sp[1].Split(' ');
            return (uint)bytesSP.Length;
        }

        static void MakePatchStructDataListForADDR(List<Address> list, bool isPointerOnly, PatchSt patch)
        {
            string address_string = U.at(patch.Param, "ADDRESS");
            if (address_string.Length <= 0)
            {
                return;
            }

            string basedir = Path.GetDirectoryName(patch.PatchFileName);
            string[] address_sp;
            uint addr;
            if (address_string[0] == '$')
            {//マクロ展開.
                addr = convertBinAddressString(address_string, 0, 0x100, basedir); //check only.
                if (!U.isSafetyOffset(addr))
                {
                    return;
                }
                address_sp = new string[] { U.To0xHexString(addr) };
            }
            else
            {//直値か、直値が複数.
                address_sp = address_string.Split(' ');
            }

            uint length = 0;
            if (isPointerOnly == false)
            {
                length = CalcAddrLength(patch);
            }

            for (int n = 0; n < address_sp.Length; n++)
            {
                uint a = U.atoi0x(address_sp[n]);
                if (!U.isSafetyOffset(a)) continue;

                if (length < 4)
                {
                    FEBuilderGBA.Address.AddAddress(list, a, length, U.NOT_FOUND, patch.Name + "@ADDRESS", Address.DataTypeEnum.BIN);
                }
                else
                {
                    FEBuilderGBA.Address.AddAddress(list, a, length, U.NOT_FOUND, patch.Name + "@ADDRESS", Address.DataTypeEnum.MIX);
                }
            }
        }
        static void MakePatchStructDataListForEA(List<Address> list, bool isPointerOnly, PatchSt patch , bool isStoreSymbol)
        {
            string use_asmamp = U.at(patch.Param, "ASMMAP", "true");
            if (U.stringbool(use_asmamp) == false)
            {//ASMMAPに含めない
                return;
            }

            string basedir = Path.GetDirectoryName(patch.PatchFileName);
            List<BinMapping> map = TracePatchedMapping(patch);
            foreach(BinMapping m in map)
            {
                uint a = m.addr;
                if (!U.isSafetyOffset(a))
                {
                    continue;
                }
                uint length = isPointerOnly ? 0 : m.length;
                FEBuilderGBA.Address.AddAddress(list
                    , a
                    , length
                    , U.NOT_FOUND
                    , patch.Name + "@" + m.filename + "@EA"
                    , m.type);

                if (isStoreSymbol && m.filename != "")
                {
                    SymbolUtil.ProcessSymbol(list, basedir, m.filename, m.addr);
                }
            }
        }
        static void MakePatchStructDataListForBIN(List<Address> list, bool isPointerOnly, PatchSt patch, bool isStoreSymbol)
        {
            string use_asmamp = U.at(patch.Param, "ASMMAP","true");
            if (U.stringbool(use_asmamp) == false)
            {//ASMMAPに含めない
                return;
            }

            string basedir = Path.GetDirectoryName(patch.PatchFileName);
            List<BinMapping> map = TracePatchedMapping(patch);
            foreach (BinMapping m in map)
            {
                if (!U.isSafetyOffset(m.addr))
                {
                    continue;
                }
                uint length;
                if (isPointerOnly == false)
                {
                    length = m.length;
                    if (m.type == Address.DataTypeEnum.POINTER)
                    {
                        FEBuilderGBA.Address.AddPointer(list
                            , m.addr
                            , 0
                            , patch.Name + "@" + m.filename + "@BIN"
                            , Address.DataTypeEnum.POINTER);
                        FEBuilderGBA.Address.AddAddress(list
                            , m.addr
                            , 0
                            , U.NOT_FOUND
                            , "Pointer_" + patch.Name + "@" + m.filename + "@BIN"
                            , Address.DataTypeEnum.POINTER
                            );
                    }
                    else if (m.type == Address.DataTypeEnum.POINTER_ASM)
                    {
                        FEBuilderGBA.Address.AddPointer(list
                            , m.addr
                            , 0
                            , "ASM" + patch.Name + "@" + m.filename + "@BIN"
                            , Address.DataTypeEnum.PATCH_ASM);
                        FEBuilderGBA.Address.AddAddress(list
                            , m.addr
                            , 0
                            , U.NOT_FOUND
                            , "Pointer_" + patch.Name + "@" + m.filename + "@BIN"
                            , Address.DataTypeEnum.POINTER_ASM
                            );
                    }
                    else if (m.type == Address.DataTypeEnum.BIN)
                    {
                        FEBuilderGBA.Address.AddAddress(list
                            , m.addr
                            , (uint)m.length
                            , U.NOT_FOUND
                            , "Fixed " + patch.Name + "@" + m.filename + "@BIN"
                            , Address.DataTypeEnum.BIN
                            );
                    }
                    else if (m.type == Address.DataTypeEnum.JUMPTOHACK)
                    {
                        FEBuilderGBA.Address.AddAddress(list
                            , m.addr
                            , (uint)m.length - 4
                            , U.NOT_FOUND
                            , "Jump " + patch.Name + "@" + m.filename + "@BIN"
                            , Address.DataTypeEnum.BIN
                            );
                    }
                    else
                    {
                        FEBuilderGBA.Address.AddAddress(list
                            , m.addr
                            , 0
                            , U.NOT_FOUND
                            , patch.Name + "@" + m.filename + "@BIN"
                            , m.type);
                    }
                }
                else
                {
                    FEBuilderGBA.Address.AddAddress(list
                        , m.addr
                        , 0
                        , U.NOT_FOUND
                        , patch.Name + "@" + m.filename + "@BIN"
                        , m.type);
                }

                if (isStoreSymbol && m.filename != "")
                {
                    SymbolUtil.ProcessSymbol(list, basedir, m.filename, m.addr);
                }
            }
        }

        static void MakePatchStructDataListForSWITCH(List<Address> list, bool isPointerOnly, PatchSt patch)
        {
            string basedir = Path.GetDirectoryName(patch.PatchFileName);
            foreach (var pair in patch.Param)
            {
                if (pair.Key.IndexOf("ONN:") != 0)
                {
                    if (pair.Key.IndexOf("OFF:") != 0)
                    {
                        continue;
                    }
                }
                string[] op = pair.Key.Split(':');
                if (op.Length < 2)
                {
                    continue;
                }
                uint length = 0;
                if (isPointerOnly == false)
                {
                    length = (uint)pair.Value.Split(' ').Length;
                }

                uint a = convertBinAddressString(op[1], 8, 0, basedir);
                if (!U.isSafetyOffset(a)) continue;

                if (length < 4)
                {
                    FEBuilderGBA.Address.AddAddress(list, a, length, U.NOT_FOUND, patch.Name + "@SWITCH", Address.DataTypeEnum.BIN);
                }
                else
                {
                    FEBuilderGBA.Address.AddAddress(list, a, length, U.NOT_FOUND, patch.Name + "@SWITCH", Address.DataTypeEnum.MIX);
                }
            }
        }
        static void MakePatchStructDataListForSTRUCT(List<Address> list, bool isPointerOnly, PatchSt patch)
        {
            string basedir = Path.GetDirectoryName(patch.PatchFileName);
            uint struct_address = 0;
            uint struct_pointer = U.NOT_FOUND;
            string pointer_str = U.at(patch.Param, "POINTER");
            if (pointer_str != "")
            {
                struct_pointer = convertBinAddressString(pointer_str, 8, 0, basedir);
                if (!U.isSafetyOffset(struct_pointer))
                {
                    return;
                }
                struct_address = Program.ROM.p32(struct_pointer);
                if (!U.isSafetyOffset(struct_address))
                {
                    return;
                }
            }
            else
            {
                string address_str = U.at(patch.Param, "ADDRESS");
                if (address_str == "")
                {
                    return;
                }
                struct_address = convertBinAddressString(address_str, 8, 0, basedir);
                if (!U.isSafetyOffset(struct_address))
                {
                    return;
                }
                struct_pointer = U.NOT_FOUND;
            }

            uint datasize = U.atoi0x(U.at(patch.Param, "DATASIZE"));
            if (datasize <= 0)
            {
                return;
            }

            uint datacount;
            string datacount_str = U.at(patch.Param, "DATACOUNT");
            if (datacount_str.Length > 0 && datacount_str[0] == '$')
            {//grep等
                datacount = convertBinAddressString(datacount_str, 8, struct_address, basedir);
                if (datacount == U.NOT_FOUND)
                {
                    return;
                }
                if (datacount > struct_address)
                {
                    datacount = (uint)Math.Ceiling((datacount - struct_address) / (double)datasize);
                }
            }
            else
            {//直値
                datacount = U.atoi0x(datacount_str);
            }
            if (datacount <= 0)
            {
                if (datacount_str == "")
                {
                    return;
                }
            }
            string[] typeArray;
            Address.DataTypeEnum iftType;
            uint[] pointerIndexes = MakePointerIndexes(patch, out typeArray , out iftType);

            string patchname = patch.Name + "@STRUCT";
            list.Add(new Address(struct_address
                , datasize * (datacount+1)
                , struct_pointer
                , patchname
                , iftType
                , datasize
                , pointerIndexes));

            List<uint> tracelist = new List<uint>();
            uint addr = struct_address;
            for (int i = 0; i < datacount; i++ , addr += datasize)
            {
                for (int n = 0; n < pointerIndexes.Length; n++)
                {
                    uint p = addr + pointerIndexes[n];
                    string type = typeArray[n];
                    if (type == "EVENT")
                    {//イベント呼び出し
                        EventScriptForm.ScanScript(list, p, true, false
                            , patchname + " DATA " + n , tracelist);
                    }
                    else if (type == "BATTLEANIMEPOINTER")
                    {//戦闘アニメのデータ
                        ImageBattleAnimeForm.MakeBattleAnimeSettingDataLength(list, p
                            , patchname + " DATA " + n);
                    }
                    else if (type == "PatchImage_IMAGE")
                    {//画像
                        uint a = Program.ROM.p32(p);
                        if (U.isSafetyOffset(a))
                        {
                            uint width = atOffset(patch.Param, "WIDTH");
                            uint height = atOffset(patch.Param, "HEIGHT");
                            FEBuilderGBA.Address.AddAddress(list, a, width * height / 2, p
                                , patchname + " IMAGE " + n, Address.DataTypeEnum.IMG);
                        }
                    }
                    else if (type == "PatchImage_ZIMAGE")
                    {//画像
                        uint a = Program.ROM.p32(p);
                        if (U.isSafetyOffset(a))
                        {
                            uint len = LZ77.getCompressedSize(Program.ROM.Data, a);
                            FEBuilderGBA.Address.AddAddress(list, a, len, p
                                , patchname + " IMAGE ", Address.DataTypeEnum.LZ77IMG);
                        }
                    }
                    else if (type == "PatchImage_TSA")
                    {//画像TSA
                        uint a = Program.ROM.p32(p);
                        if (U.isSafetyOffset(a))
                        {
                            uint width = atOffset(patch.Param, "WIDTH");
                            uint height = atOffset(patch.Param, "HEIGHT");
                            FEBuilderGBA.Address.AddAddress(list, a, width * height / 32, p
                                , patchname + " TSA " + n, Address.DataTypeEnum.TSA);
                        }
                    }
                    else if (type == "PatchImage_ZTSA")
                    {//画像 ZTSA
                        uint a = Program.ROM.p32(p);
                        if (U.isSafetyOffset(a))
                        {
                            uint len = LZ77.getCompressedSize(Program.ROM.Data, a);
                            FEBuilderGBA.Address.AddAddress(list, a, len, p
                                , patchname + " ZTSA " + n, Address.DataTypeEnum.LZ77IMG);
                        }
                    }
                    else if (type == "PatchImage_HEADERTSA")
                    {//画像 HEADERTSA
                        uint a = Program.ROM.p32(p);
                        if (U.isSafetyOffset(a))
                        {
                            FEBuilderGBA.Address.AddHeaderTSAPointer(list, p
                                , patchname + " HEADERTSA " + n, false);
                        }
                    }
                    else if (type == "PatchImage_ZTSA")
                    {//画像 ZHEADERTSA
                        uint a = Program.ROM.p32(p);
                        if (U.isSafetyOffset(a))
                        {
                            uint len = LZ77.getCompressedSize(Program.ROM.Data, a);
                            FEBuilderGBA.Address.AddAddress(list, a, len, p
                                , patchname + " ZHEADERTSA " + n, Address.DataTypeEnum.LZ77TSA);
                        }
                    }
                    else if (type == "PatchImage_Palette")
                    {//画像 Palette
                        uint a = Program.ROM.p32(p);
                        if (U.isSafetyOffset(a))
                        {
                            uint len = atOffset(patch.Param, "PALETTE", "1") * 0x20;
                            FEBuilderGBA.Address.AddAddress(list, a, len, p
                                , patchname + " PALETTE " + n, Address.DataTypeEnum.LZ77PAL);
                        }
                    }
                    else if (type == "AP")
                    {//AP
                        uint a = Program.ROM.p32(p);
                        if (U.isSafetyOffset(a))
                        {
                            FEBuilderGBA.Address.AddAPPointer(list
                                , p
                                , patchname + " AP " + n
                                , isPointerOnly);
                        }
                    }
                    else if (type == "ROMTCS")
                    {//ROMTCS
                        uint a = Program.ROM.p32(p);
                        if (U.isSafetyOffset(a))
                        {
                            FEBuilderGBA.Address.AddROMTCSPointer(list
                                , p
                                , patchname + " ROMTCS " + n
                                , isPointerOnly);
                        }
                    }
                    else if (type == "PROCS")
                    {//PROCS
                        uint a = Program.ROM.p32(p);
                        if (U.isSafetyOffset(a))
                        {
                            FEBuilderGBA.Address.AddProcsPointer(list
                                , p
                                , patchname + " PROCS " + n
                                , isPointerOnly);
                        }
                    }
                    else if (type == "ASM")
                    {//ASM
                        uint a = Program.ROM.p32(p);
                        if (U.isSafetyOffset(a))
                        {
                            FEBuilderGBA.Address.AddFunction(list
                                , p
                                , patchname + " ASM " + n
                                );
                        }
                    }
                    else
                    {//不明なポインタ
                        FEBuilderGBA.Address.AddPointer(list
                            , p
                            , 0
                            , patchname + " DATA " + n
                            , Address.DataTypeEnum.MIX
                            );
                    }
                }
            }
        }
        static void MakePatchStructDataListForIMAGE(List<Address> list, bool isPointerOnly, PatchSt patch)
        {
            uint width = atOffset(patch.Param, "WIDTH", "8");
            uint height = atOffset(patch.Param, "HEIGHT", "8");
            uint p;
            p = atOffset(patch.Param, "IMAGE_POINTER");
            if (p > 0 && U.isSafetyOffset(p))
            {
                uint a = Program.ROM.p32(p);
                if (U.isSafetyOffset(a))
                {
                    FEBuilderGBA.Address.AddAddress(list, a, width * height / 2, p, patch.Name + "@IMAGE_POINTER", Address.DataTypeEnum.IMG);
                }
            }
            p = atOffset(patch.Param, "ZIMAGE_POINTER");
            if (p > 0 && U.isSafetyOffset(p))
            {
                uint a = Program.ROM.p32(p);
                if (U.isSafetyOffset(a))
                {
                    uint len = LZ77.getCompressedSize(Program.ROM.Data, a);
                    FEBuilderGBA.Address.AddAddress(list, a, len, p, patch.Name + "@ZIMAGE_POINTER", Address.DataTypeEnum.LZ77IMG);
                }
            }

            p = atOffset(patch.Param, "TSA_POINTER");
            if (U.isSafetyOffset(p))
            {
                uint a = Program.ROM.p32(p);
                if (U.isSafetyOffset(a))
                {
                    FEBuilderGBA.Address.AddAddress(list, a, width * height / 32, p, patch.Name + "@TSA_POINTER", Address.DataTypeEnum.TSA);
                }
            }
            p = atOffset(patch.Param, "ZTSA_POINTER");
            if (U.isSafetyOffset(p))
            {
                uint a = Program.ROM.p32(p);
                if (U.isSafetyOffset(a))
                {
                    uint len = LZ77.getCompressedSize(Program.ROM.Data, a);
                    FEBuilderGBA.Address.AddAddress(list, a, len, p, patch.Name + "@ZTSA_POINTER", Address.DataTypeEnum.LZ77TSA);
                }
            }
            p = atOffset(patch.Param, "HEADERTSA_POINTER");
            if (U.isSafetyOffset(p))
            {
                FEBuilderGBA.Address.AddHeaderTSAPointer(list, p, patch.Name + "@HEADERTSA_POINTER", false);
            }
            p = atOffset(patch.Param, "ZHEADERTSA_POINTER");
            if (U.isSafetyOffset(p))
            {
                uint a = Program.ROM.p32(p);
                if (U.isSafetyOffset(a))
                {
                    uint len = LZ77.getCompressedSize(Program.ROM.Data, a);
                    FEBuilderGBA.Address.AddAddress(list, a, len, p, patch.Name + "@ZHEADERTSA_POINTER", Address.DataTypeEnum.LZ77TSA);
                }
            }

            p = atOffset(patch.Param, "PALETTE_POINTER");
            if (U.isSafetyOffset(p))
            {
                uint a = Program.ROM.p32(p);
                if (U.isSafetyOffset(a))
                {
                    uint len = atOffset(patch.Param, "PALETTE", "1") * 0x20;
                    FEBuilderGBA.Address.AddAddress(list, a, len, p, patch.Name + "@PALETTE_POINTER", Address.DataTypeEnum.LZ77PAL);
                }
            }
            else
            {
                uint a = atOffset(patch.Param, "PALETTE_ADDRESS");
                if (U.isSafetyOffset(a))
                {
                    uint len = atOffset(patch.Param, "PALETTE", "1") * 0x20;
                    FEBuilderGBA.Address.AddAddress(list, a, len, U.NOT_FOUND, patch.Name + "@PALETTE_ADDRESS", Address.DataTypeEnum.PAL);
                }
            }
        }

        static bool IsMakePatchStructDataListTarget(string type, string checkIF, bool isInstallOnly, bool isStructOnly)
        {
            if (isStructOnly)
            {
                if (type != "STRUCT")
                {//構造体だけ
                    return false;
                }
            }

            if (isInstallOnly)
            {
                if (checkIF == "E")
                {//エラーがおきているので無視
                    return false;
                }
                else if (checkIF != "I")
                {//インストールされていないので無視したいのだが...
                    //構造体と画像は性質上インストールできない
                    if (type == "STRUCT" || type == "IMAGE")
                    {//構造体または画像
                        return true;
                    }
                    return false;
                }
            }

            return true;
        }

        static bool isCanonicalSkip(PatchSt patch)
        {
            string v = U.at(patch.Param, "CANONICAL_SKIP", "0");
            return U.stringbool(v);
        }
            

        //パッチが知っているアドレスをすべて取得します.
        public static void MakePatchStructDataList(List<Address> list, bool isPointerOnly, bool isInstallOnly, bool isStructOnly, bool isStoreSymbol)
        {
            List<PatchSt> patchs = ScanPatchs(GetPatchDirectory(),false);
            for (int i = 0; i < patchs.Count; i++)
            {
                PatchSt patch = patchs[i];

                if (isCanonicalSkip(patch))
                {
                    continue;
                }

                string type = U.at(patch.Param, "TYPE");
                string checkIF = CheckIFFast(patch);
                if (! IsMakePatchStructDataListTarget( type,  checkIF,  isInstallOnly,  isStructOnly) )
                {
                    continue;
                }

                if (type == "ADDR")
                {
                    MakePatchStructDataListForADDR(list, isPointerOnly, patch);
                }
                else if (type == "EA")
                {
                    MakePatchStructDataListForEA(list, isPointerOnly, patch, isStoreSymbol);
                }
                else if (type == "BIN")
                {
                    MakePatchStructDataListForBIN(list, isPointerOnly, patch, isStoreSymbol);
                }
                else if (type == "SWITCH")
                {
                    MakePatchStructDataListForSWITCH(list, isPointerOnly, patch);
                }
                else if (type == "STRUCT")
                {
                    MakePatchStructDataListForSTRUCT(list, isPointerOnly, patch);
                }
                else if (type == "IMAGE")
                {
                    MakePatchStructDataListForIMAGE(list, isPointerOnly, patch);
                }

                if (InputFormRef.DoEvents(null, "Check Patch " + patch.Name)) return;
            }
        }

        static uint[] MakePointerIndexes(PatchSt patch
            , out string[] out_typeArray
            , out Address.DataTypeEnum out_iftType
            )
        {
            bool hasASM = false;
            bool hasNoASM = false;
            List<string> typeArray = new List<string>();
            List<uint> pointerIndexes = new List<uint>();
            foreach (var pair in patch.Param)
            {
                string[] sp = pair.Key.Split(':');
                string key = sp[0];
                string type = U.at(sp, 1);
                string value = pair.Value;

                if (!U.isnum(key[1]))
                {
                    continue;
                }
                int datanum = (int)U.atoi(key.Substring(1));

                if (key[0] == 'P')
                {
                    if (type == "ASM")
                    {
                        hasASM = true;
                    }
                    else
                    {
                        hasNoASM = true;
                    }

                    pointerIndexes.Add((uint)datanum);
                    typeArray.Add(type);
                }
            }

            out_typeArray = typeArray.ToArray();

            if (hasASM)
            {
                if (hasNoASM == false)
                {//ASMだけ
                    out_iftType = Address.DataTypeEnum.InputFormRef_ASM;
                }
                else
                {//ASMとデータの混在
                    out_iftType = Address.DataTypeEnum.InputFormRef_MIX;
                }
            }
            else
            {//ASMを含まない
                out_iftType = Address.DataTypeEnum.InputFormRef; ;
            }

            return pointerIndexes.ToArray();
        }

        static void MakeEventScriptAddEventScript(PatchSt st, string eventscript 
            , List<EventScript.Script> scripts)
        {
            if (eventscript == "")
            {
                return;
            }

            string error = CheckIFFast(st);
            if (error != "I")
            {
                return;
            }

            string scriptbin = U.cut(eventscript, "{$", "}");
            scriptbin = U.skip(scriptbin, ":");
            if (scriptbin != "")
            {
                string basedir = Path.GetDirectoryName(st.PatchFileName);
                string filename = Path.Combine(basedir, scriptbin);
                if (!File.Exists(filename))
                {
                    Debug.Assert(false);
                    return;
                }

                uint FindStartAddress = Program.ROM.RomInfo.compress_image_borderline_address();

                byte[] grepdata = File.ReadAllBytes(filename);
                uint addr = U.Grep(Program.ROM.Data, grepdata, FindStartAddress, 0, 4);
                if (addr == U.NOT_FOUND)
                {
                    return;
                }
                //マクロを置換する.
                eventscript = ReplaceL1Macro(eventscript, scriptbin, addr);
            }
            EventScript.Script script = EventScript.ParseScriptLine(eventscript);
            if (script == null)
            {
                Debug.Assert(false);
                return;
            }
            script.IsExdends = true;
            scripts.Add(script);
        }
        static void MakeEventScriptAddUseFlag(PatchSt st, string useflag
            , Dictionary<uint, string> flags)
        {
            if (useflag == "")
            {
                return;
            }
            string error = CheckIFFast(st);
            if (error != "I")
            {
                return;
            }

            uint f = U.atoi0x(useflag);
            if (f <= 0)
            {
                return;
            }
            flags[f] = U.at(st.Param, "NAME");
        }

        public static void MakeEventScript(List<EventScript.Script> scripts, Dictionary<uint, string> flags)
        {
            List<PatchSt> patchs = ScanPatchs(GetPatchDirectory(),true);
            for (int i = 0; i < patchs.Count; i++)
            {
                PatchSt patch = patchs[i];
                if (isCanonicalSkip(patch))
                {
                    continue;
                }

                foreach (var pair in patch.Param)
                {
                    if (pair.Key.IndexOf("EVENTSCRIPT") == 0)
                    {
                        MakeEventScriptAddEventScript(patch, pair.Value, scripts);
                        continue;
                    }
                    if (pair.Key.IndexOf("USEFLAG") == 0)
                    {
                        MakeEventScriptAddUseFlag(patch, pair.Value, flags);
                        continue;
                    }
                }
            }
        }

        public class PatchException : Exception
        {
            public PatchException(string message)
                : base(message)
            {
            }
        }
        public class SyntaxException : Exception
        {
            public SyntaxException(string message)
                : base(message)
            {
            }
        }

        private void Filter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                U.SelectedIndexSafety(PatchList, 0, true);
                return;
            }
        }

        private void PatchList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up && this.PatchList.SelectedIndex == 0)
            {
                Filter.Focus();
                return;
            }
        }

        private void Filter_DoubleClick(object sender, EventArgs e)
        {
            this.Filter.Text = "";
            Filter.Focus();
        }

        public void JumpTo(string filter,int index)
        {
            this.Filter.Text = filter;
            U.SelectedIndexSafety(this.PatchList, index, true);
        }

#if DEBUG
        //重たいのでフルテストの時のみ実行
        public static void TESTFULL_PATCH()
        {
            List<PatchSt>  patchs = ScanPatchs(GetPatchDirectory(), false);
            for (int i = 0; i < patchs.Count; i++)
            {
                PatchSt patch = patchs[i];
                uint p;

                string basedir = Path.GetDirectoryName(patch.PatchFileName);
                string type = U.at(patch.Param, "TYPE");
                if (type == "ADDR")
                {
                    string address_string = U.at(patch.Param, "ADDRESS");
                    if (address_string.Length <= 0)
                    {
                        Debug.Assert(false);
                        continue;
                    }

                    string[] address_sp;
                    uint addr;
                    if (address_string[0] == '$')
                    {//マクロ展開.
                        addr = convertBinAddressString(address_string, 0, 0x100 , basedir); //check only.
                        if (!U.isSafetyOffset(addr))
                        {//IFの条件を満たしていないだけかもしれない.
                            continue;
                        }
                        address_sp = new string[] { U.To0xHexString(addr) };
                    }
                    else
                    {//直値か、直値が複数.
                        address_sp = address_string.Split(' ');
                    }


                    addr = U.toOffset(U.atoi0x(address_sp[0]));
                    if (!U.isSafetyOffset(addr))
                    {
                        Debug.Assert(false);
                        continue;
                    }
                    uint value = Program.ROM.u8(addr);
                    string default_string = U.at(patch.Param, "DEFAULT");
                    if (default_string.Length > 0)
                    {
                        uint default_value = U.atoi0x(default_string);
                        Debug.Assert(value == default_value);
                    }

                    for (int n = 1; n < address_sp.Length; n++)
                    {
                        uint a = U.toOffset(U.atoi0x(address_sp[n]));
                        if (!U.isSafetyOffset(a))
                        {
                            Debug.Assert(false);
                            continue;
                        }
                        uint r = Program.ROM.u8(a);
                        Debug.Assert(r == value);
                    }
                }
                else if (type == "EA")
                {
                    string EA = U.at(patch.Param, "EA");

                    string eaFilename = Path.Combine(basedir, EA);
                    Debug.Assert(File.Exists(eaFilename));

                    EAUtil eaU = new EAUtil(eaFilename);
                }
                else if (type == "BIN")
                {
                    List<BinMapping> map = TracePatchedMapping(patch);
                    for (int n = 0; n < map.Count; n++)
                    {
                        uint a = map[n].addr;
                        if (!U.isSafetyOffset(a))
                        {
                            Debug.Assert(false);
                        }
                    }
                }
                else if (type == "SWITCH")
                {
                    foreach (var pair in patch.Param)
                    {
                        if (pair.Key.IndexOf("ONN:") != 0)
                        {
                            if (pair.Key.IndexOf("OFF:") != 0)
                            {
                                continue;
                            }
                        }
                        string[] op = pair.Key.Split(':');
                        if (op.Length < 2)
                        {
                            Debug.Assert(false);
                            continue;
                        }

                        uint a = convertBinAddressString(op[1], 8, 0 , basedir);
                        if (!U.isSafetyOffset(a))
                        {
                            Debug.Assert(false);
                            continue;
                        }
                    }
                }
                else if (type == "STRUCT")
                {
                    uint struct_address = 0;

                    string pointer_str = U.at(patch.Param, "POINTER");
                    uint addr = convertBinAddressString(pointer_str, 8, 0 , basedir);
                    if (!U.isSafetyOffset(addr))
                    {
                        continue;
                    }
                    struct_address = Program.ROM.p32(addr);
                    if (!U.isSafetyOffset(struct_address))
                    {
                        Debug.Assert(false);
                        continue;
                    }

                    uint datasize = U.atoi0x(U.at(patch.Param, "DATASIZE"));
                    if (datasize <= 0)
                    {
                        Debug.Assert(false);
                        continue;
                    }

                    uint datacount;
                    string datacount_str = U.at(patch.Param, "DATACOUNT");
                    if (datacount_str.Length > 0 && datacount_str[0] == '$')
                    {//grep等
                        datacount = convertBinAddressString(datacount_str, 8, struct_address , basedir);
                        if (datacount == U.NOT_FOUND)
                        {
                            Debug.Assert(false);
                            return;
                        }
                        if (datacount > struct_address)
                        {
                            datacount = (uint)Math.Ceiling((datacount - struct_address) / (double)datasize);
                        }
                    }
                    else
                    {//直値
                        datacount = U.atoi0x(datacount_str);
                    }
                    if (datacount <= 0)
                    {
                        if (datacount_str == "")
                        {
                            Debug.Assert(false);
                            continue;
                        }
                    }
                }
                else if (type == "IMAGE")
                {
                    uint width = atOffset(patch.Param, "WIDTH", "8");
                    uint height = atOffset(patch.Param, "HEIGHT", "8");

                    p = atOffset(patch.Param, "IMAGE_POINTER");
                    if (p > 0 && U.isSafetyOffset(p))
                    {
                        uint a = Program.ROM.p32(p);
                        if (!U.isSafetyOffset(a))
                        {
                            Debug.Assert(false);
                        }
                    }
                    p = atOffset(patch.Param, "ZIMAGE_POINTER");
                    if (p > 0 && U.isSafetyOffset(p))
                    {
                        uint a = Program.ROM.p32(p);
                        if (!U.isSafetyOffset(a))
                        {
                            Debug.Assert(false);
                        }
                    }

                    p = atOffset(patch.Param, "TSA_POINTER");
                    if (U.isSafetyOffset(p))
                    {
                        uint a = Program.ROM.p32(p);
                        if (!U.isSafetyOffset(a) && a != 0)
                        {
                            Debug.Assert(false);
                        }
                    }
                    p = atOffset(patch.Param, "ZTSA_POINTER");
                    if (U.isSafetyOffset(p))
                    {
                        uint a = Program.ROM.p32(p);
                        if (!U.isSafetyOffset(a) && a != U.NOT_FOUND && a != 0)
                        {
                            Debug.Assert(false);
                        }
                    }
                    p = atOffset(patch.Param, "HEADERTSA_POINTER");
                    if (p > 0)
                    {
                        if (!U.isSafetyOffset(p) && p != U.NOT_FOUND && p != 0)
                        {
                            Debug.Assert(false);
                        }
                    }
                    p = atOffset(patch.Param, "ZHEADERTSA_POINTER");
                    if (U.isSafetyOffset(p))
                    {
                        uint a = Program.ROM.p32(p);
                        if (!U.isSafetyOffset(a) && a != U.NOT_FOUND && a != 0)
                        {
                            Debug.Assert(false);
                        }
                    }

                    p = atOffset(patch.Param, "PALETTE_POINTER");
                    if (U.isSafetyOffset(p))
                    {
                        uint a = Program.ROM.p32(p);
                        if (!U.isSafetyOffset(a) && a != U.NOT_FOUND && a != 0)
                        {
                            Debug.Assert(false);
                        }
                    }
                    else
                    {
                        uint a = atOffset(patch.Param, "PALETTE_ADDRESS");
                        if (!U.isSafetyOffset(a) && a != U.NOT_FOUND && a != 0)
                        {
                            Debug.Assert(false);
                        }
                    }
                }
                else
                {
                    Debug.Assert(false);
                }

                CheckEditPatch(basedir, patch);
            }
        }

        static void CheckEditPatch(string basedir , PatchSt patch)
        {
            foreach (var pair in patch.Param)
            {
                string[] sp = pair.Key.Split(':');
                string key = sp[0];

                if (key == "EDIT_PATCH" && pair.Value != "")
                {
                    string editpatch = pair.Value;

                    editpatch = Path.Combine(basedir, editpatch);
                    if (!File.Exists(editpatch))
                    {//EDIT_PATCHが存在しない
                        Debug.Assert(false);
                    }
                }
            }

        }
#endif

        private void UnInstallButton_Click(object sender, EventArgs e)
        {
            if (InputFormRef.IsPleaseWaitDialog(this))
            {//2重割り込み禁止
                return;
            }
            if (this.PatchList.SelectedIndex < 0 || this.PatchList.SelectedIndex >= this.FiltedPatchs.Count)
            {
                return;
            }

            PatchSt patch = this.FiltedPatchs[this.PatchList.SelectedIndex];
            string type = U.at(patch.Param, "TYPE");
            if (type != "BIN" && type != "EA")
            {
                R.ShowStopError("BINかEA以外のパッチはアンインストールできません。");
                return;
            }
            List<BinMapping> binmap = TracePatchedMapping(patch);

            //パッチを含んでいないファイルを提示してもらう.
            PatchFormUninstallDialogForm f = (PatchFormUninstallDialogForm)InputFormRef.JumpFormLow<PatchFormUninstallDialogForm>();
            f.Init(binmap);
            DialogResult dr = f.ShowDialog();
            if (dr != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            byte[] orignalROMData = f.GetOrignalROMData();
            if (orignalROMData.Length <= 0)
            {
                return;
            }

            //少し時間がかかるので、しばらくお待ちください表示.
            using (InputFormRef.AutoPleaseWait pleaseWait = new InputFormRef.AutoPleaseWait(this))
            {
                Undo.UndoData undodata = Program.Undo.NewUndoData(this, patch.Name);
                string error = UninstallPatch(pleaseWait, binmap, orignalROMData, undodata);
                if (error != "")
                {
                    Program.Undo.Rollback(undodata);  //操作の取り消し

                    R.ShowStopError("アンインストールに失敗しました.\r\n\r\n{0}", error);
                    return;
                }
                Program.Undo.Push(undodata);
                Program.ReLoadSetting();
                InputFormRef.ShowWriteNotifyAnimation(this, 0);

                U.ReSelectList(PatchList);
            }
        }

        static string UninstallPatch(InputFormRef.AutoPleaseWait pleaseWait 
            , List<BinMapping> binmap
            , byte[] orignalROM
            , Undo.UndoData undodata)
        {
            for (int n = 0; n < binmap.Count; n++)
            {
                BinMapping map = binmap[n];
                pleaseWait.DoEvents(R._("アンインストール中です。進捗:{1}/{2} アドレス:{0}", U.To0xHexString(map.addr), n, binmap.Count));

                if (map.length == 0)
                {//サイズがわからないので、自動的に求めます.
                    map.length = CalcAutoLength(map.addr, orignalROM);
                    map.bin = Program.ROM.getBinaryData(map.addr , map.length);
                }

                for (int i = 0; i < map.length; i++)
                {
                    uint addr = map.addr + (uint)i;
                    uint o = U.at(orignalROM, addr); //パッチを含んでいないROMの内容
                    uint x = U.at(map.bin, i);       //パッチで変更される内容
                    uint c = Program.ROM.u8(addr);   //現在のROMの内容

                    Program.CommentCache.Remove(addr);

                    if (c == x)
                    {//現在のROMの内容が、パッチの内容と同一の場合、含んでいないROMの内容で上書きして消去する
                        Program.ROM.write_u8(addr, o, undodata);
                    }
                    //   ori P1  P2  Cur  NOW
                    //    1  A            A
                    //    2  B   X        X
                    //    3      X        X
                    //    4      X   C    C
                    //                    ^------もしXXを消す場合 A B 3 C
                    //                    ^------もしABを消す場合 1 X X C
                }
            }

            pleaseWait.DoEvents(R._("ROM末尾の最適化をしています"));
            StripROM(binmap, undodata);

            pleaseWait.DoEvents(R._("完了"));
            return "";
        }
        static void StripROM(List<BinMapping> binmap, Undo.UndoData undodata)
        {
            uint extendsAddr = U.toOffset(Program.ROM.RomInfo.extends_address());
            int length = Program.ROM.Data.Length;

            //終端が0x00で埋まるならROMサイズを小さくする.
            uint stripSize = U.NOT_FOUND;
            for (int n = 0; n < binmap.Count; n++)
            {
                BinMapping map = binmap[n];
                uint addr = map.addr;
                if (addr < extendsAddr)
                {//拡張領域ではないのでstripできない.
                    continue;
                }

                for (int i = (int)addr; i < length; i++, addr++)
                {
                    if (Program.ROM.Data[i] != 0x00)
                    {
                        break;
                    }
                }
                if (addr == length)
                {
                    if (stripSize > map.addr)
                    {
                        stripSize = map.addr;
                    }
                }
            }
            if (Program.ROM.Data.Length > stripSize
                && stripSize >= extendsAddr)
            {
                undodata.list.Add(new Undo.UndoPostion(stripSize, (uint)Program.ROM.Data.Length - stripSize));
                Program.ROM.write_resize_data(stripSize);
            }

        }

        static uint CalcAutoLength(uint addr, byte[] other, int RecoverMissMatch = 10)
        {
            int length = Math.Min(Program.ROM.Data.Length, other.Length);
            int checkpoint;
            int i;
            for (i = (int)addr; i < length; i++)
            {
                if (Program.ROM.Data[i] != other[i])
                {
                    continue;
                }

                checkpoint = i;

                i++;
                int missCount = 0;
                for (; i < length; i++)
                {
                    if (Program.ROM.Data[i] != other[i])
                    {
                        i -= missCount;
                        break;
                    }

                    if (missCount >= RecoverMissMatch)
                    {
                        i -= missCount;

                        return ((uint)i) - addr;
                    }

                    missCount++;
                }
            }

            return ((uint)i) - addr;
        }

        private void FilterExLabel_Click(object sender, EventArgs e)
        {
            PatchFilterExForm f = (PatchFilterExForm)InputFormRef.JumpFormLow<PatchFilterExForm>();
            f.ShowDialog();

            if (f.DialogResult != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            this.Filter.Text = f.TagFilter;
            if (this.Filter.Text.Length > 1)
            {
                this.Filter.Select(this.Filter.Text.Length, 0);
            }
        }

        //FELint
        public static void MakeCheckError(List<FELint.ErrorSt> errors)
        {
            List<PatchSt> patchs = ScanPatchs(GetPatchDirectory(), false);
            for (int i = 0; i < patchs.Count; i++)
            {
                if (Program.AsmMapFileAsmCache.IsStopFlagOn()) return;

                PatchSt patch = patchs[i];
                if (isCanonicalSkip(patch))
                {
                    continue;
                }

                string type = U.at(patch.Param, "TYPE");
                if (type != "STRUCT")
                {
                    continue;
                }

                string checkIF = CheckIFFast(patch);
                if (checkIF == "E")
                {
                    continue;
                }

                CheckDoubleInstall(patch, (uint)i, errors);
            }
        }

        //二重にインストールしていないか確認します.
        static void CheckDoubleInstall(PatchSt patch, uint loopI , List<FELint.ErrorSt> errors)
        {
            string pointer = U.at(patch.Param, "POINTER");
            if (pointer == "")
            {//不明
                return;
            }

            byte[] need;
            if (pointer.IndexOf("$GREP4END")==0)
            {
                need = MakeGrepData(pointer);
            }
            else if (pointer.IndexOf("$FGREP4END")==0)
            {
                need = MakeGrepData(pointer, Path.GetDirectoryName(patch.PatchFileName));
            }
            else
            {//不明
                return;
            }

            uint p = U.Grep(Program.ROM.Data, need, 0x100, 0, 4);
            if (p == U.NOT_FOUND)
            {
                string name = U.at(patch.Param, "NAME");
                errors.Add(new FELint.ErrorSt(FELint.Type.PATCH, U.NOT_FOUND
                    , R._("「{0}」パッチがインストールされていますが、設定画面のポインタを取得できません。パッチが壊れていると思われます。", name), loopI));
                return;
            }

            uint p2 = U.Grep(Program.ROM.Data, need, p + 4 , 0, 4);
            if (p2 != U.NOT_FOUND)
            {
                string name = U.at(patch.Param, "NAME");
                errors.Add(new FELint.ErrorSt(FELint.Type.PATCH, p
                    , R._("「{0}」パッチを2重にインストールしています。\r\n1st({1}),2nd({2})"
                    , name, U.To0xHexString(p), U.To0xHexString(p2)), loopI));
                return;
            }

            //問題なし
            return;
        }

        public void SelectPatchByTag(uint loopI)
        {
            this.Filter.Text = "";
            U.SelectedIndexSafety(this.PatchList, loopI);
        }

        string UpdatePatchBySkillSystems(PatchSt patch, PatchSt new_patchSt)
        {
            using (U.MakeTempDirectory tempdir = new U.MakeTempDirectory())
            {
                //Export
                string SkillAssignmentClassSkillSystem = Path.Combine(tempdir.Dir, "SkillAssignmentClassSkillSystem.tsv");
                string SkillAssignmentUnitSkillSystem = Path.Combine(tempdir.Dir, "SkillAssignmentUnitSkillSystem.tsv");
                string SkillConfigSkillSystem = Path.Combine(tempdir.Dir, "SkillConfigSkillSystemForm.tsv");
                SkillAssignmentClassSkillSystemForm.ExportAllData(SkillAssignmentClassSkillSystem);
                SkillAssignmentUnitSkillSystemForm.ExportAllData(SkillAssignmentUnitSkillSystem);
                SkillConfigSkillSystemForm.ExportAllData(SkillConfigSkillSystem);

                //Uninstall
                UnInstallButton_Click(null, null);

                //Install
                bool r = ApplyPatch(new_patchSt.Name);
                if (!r)
                {
                    return R.Error("新しいパッチをインストールできませんでした。") + new_patchSt.PatchFileName;
                }

                //Import
                SkillAssignmentClassSkillSystemForm.ImportAllData(SkillAssignmentClassSkillSystem);
                SkillAssignmentUnitSkillSystemForm.ImportAllData(SkillAssignmentUnitSkillSystem);
                SkillConfigSkillSystemForm.ImportAllData(SkillConfigSkillSystem);
            }
            return "";
        }
        string UpdatePatchByNone(PatchSt patch, PatchSt new_patchSt)
        {
            //Uninstall
            UnInstallButton_Click(null, null);

            //Install
            bool r = ApplyPatch(new_patchSt.Name);
            if (!r)
            {
                return R.Error("新しいパッチをインストールできませんでした。") + new_patchSt.PatchFileName;
            }
            return "";
        }

        //バージョンアップデート
        string UpdatePatch(PatchSt patch)
        {
            string basedir = Path.GetDirectoryName(patch.PatchFileName);
            string update_patch = U.at(patch.Param, "UPDATE_PATCH");
            if (update_patch == "")
            {
                return R.Error("新しいパッチが存在しません。");
            }
            string new_patch = Path.Combine(basedir , update_patch);
            if (!File.Exists(new_patch))
            {
                return R.Error("新しいパッチが存在しません。") + new_patch;
            }
            PatchSt new_patchSt = LoadPatch(new_patch, true);
            if (new_patchSt == null)
            {
                return R.Error("新しいパッチが存在しません。") + new_patch;
            }
            string update_method = U.at(patch.Param, "UPDATE_METHOD");

            if (update_method == "SKILL")
            {
                return UpdatePatchBySkillSystems(patch , new_patchSt);
            }
            else if (update_method == "NONE")
            {
                return UpdatePatchByNone(patch , new_patchSt);
            }
            else
            {
                return "";
            }
        }

    }
}
