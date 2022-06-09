using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class ToolExportEAEventForm : Form
    {
        public ToolExportEAEventForm()
        {
            InitializeComponent();

            if (Program.ROM.RomInfo.version < 8)
            {
                ExportWMAPEvent2EAButton.Hide();
            }
        }

        private void ToolExportEAEventForm_Load(object sender, EventArgs e)
        {
            //マップIDリストを作る.
            U.ConvertListBox(MapSettingForm.MakeMapIDList(), ref  this.MAP_LISTBOX);
        }

        public void JumpToMAPID(uint mapid)
        {
            U.SelectedIndexSafety(this.MAP_LISTBOX, mapid);
        }

        string GetMapName(uint appendAddr,string type)
        {
            return U.escape_filename(MAP_LISTBOX.Text + "_" + type + "_" + U.To0xHexString(appendAddr));
        }

        private void SaveAS(uint addr,string eatype,string eaOption,string filetype)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("EA|*.event|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.AddExtension = true;
            Program.LastSelectedFilename.Load(this, "", save, GetMapName(addr, filetype));

            save.ShowDialog();
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            string filename = save.FileName;
            Program.LastSelectedFilename.Save(this, "", save);

            string ext = U.GetFilenameExt(filename);
            string errorOutput;
            bool r;
            try
            {
                r = MainFormUtil.DisasembleEventAssembler(addr
                    , filename
                    , eatype
                    , eaOption
                    , addEndGuardsCheckBox.Checked
                    , out errorOutput);
            }
            catch (Win32Exception e)
            {
                r = false;
                errorOutput = R._("プロセスを実行できません。\r\nfilename:{0}\r\n{1}", filename, e.ToString());
            }
            if (!r)
            {
                R.ShowStopError("EAでエクスポートできませんでした。\r\n{0}", errorOutput);
                return;
            }

            //エクスプローラで選択しよう
            U.SelectFileByExplorer(filename);
        }

        private void ExportEAButton_Click(object sender, EventArgs e)
        {
            uint mapid = (uint)MAP_LISTBOX.SelectedIndex;
            if (mapid == U.NOT_FOUND)
            {
                return ;
            }
            uint addr = MapSettingForm.GetEventAddrWhereMapID(mapid);
            if (!U.isSafetyOffset(addr))
            {
                return ;
            }
            if (addr == U.NOT_FOUND)
            {
                return;
            }
            SaveAS(addr, "Structure", "", "Event");
        }

        private void ExportWMAPEventEAButton_Click(object sender, EventArgs e)
        {
            uint mapid = (uint)MAP_LISTBOX.SelectedIndex;
            if (mapid == U.NOT_FOUND)
            {
                return;
            }
            uint addr;
            if (Program.ROM.RomInfo.version == 8)
            {
                addr = WorldMapEventPointerForm.GetEventByMapID(mapid, false);
            }
            else if (Program.ROM.RomInfo.version == 7)
            {
                addr = WorldMapEventPointerFE7Form.GetEventByMapID(mapid);
            }
            else
            {//6
                addr = WorldMapEventPointerFE6Form.GetEventByMapID(mapid);
            }
            if (!U.isSafetyOffset(addr))
            {
                return;
            }
            if (addr == U.NOT_FOUND)
            {
                return;
            }
            SaveAS(addr, "ToEnd", "none", "WorldMap");
        }

        private void ExportWMAPEvent2EAButton_Click(object sender, EventArgs e)
        {
            uint mapid = (uint)MAP_LISTBOX.SelectedIndex;
            if (mapid == U.NOT_FOUND)
            {
                return;
            }
            uint addr;
            if (Program.ROM.RomInfo.version == 8)
            {
                addr = WorldMapEventPointerForm.GetEventByMapID(mapid, true);
            }
            else if (Program.ROM.RomInfo.version == 7)
            {
                return;
            }
            else
            {//6
                return;
            }
            if (!U.isSafetyOffset(addr))
            {
                return;
            }
            if (addr == U.NOT_FOUND)
            {
                return;
            }
            SaveAS(addr, "ToEnd", "none", "WorldMapSelect");
        }

        private void ExportMainTableButton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("EA|*.event|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.AddExtension = true;

            save.ShowDialog();
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            string filename = save.FileName;
            Program.LastSelectedFilename.Save(this, "", save);

            using (InputFormRef.AutoPleaseWait wait = new InputFormRef.AutoPleaseWait())
            {
                ExportMainTable(filename);
            }
            U.SelectFileByExplorer(filename);
        }

        void ExportMainTable(string filename)
        {
            string saveDir = Path.GetDirectoryName(filename);

            StringBuilder sb = new StringBuilder();
            if (Program.ROM.RomInfo.version == 8)
            {//FE8
                ((UnitForm)InputFormRef.JumpFormLow<UnitForm>()).InputFormRef.SaveDumpAutomatic(sb,saveDir);
                ((ClassForm)InputFormRef.JumpFormLow<ClassForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((ItemForm)InputFormRef.JumpFormLow<ItemForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((MapSettingForm)InputFormRef.JumpFormLow<MapSettingForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((SoundRoomForm)InputFormRef.JumpFormLow<SoundRoomForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((SupportUnitForm)InputFormRef.JumpFormLow<SupportUnitForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((SupportTalkForm)InputFormRef.JumpFormLow<SupportTalkForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((EventBattleTalkForm)InputFormRef.JumpFormLow<EventBattleTalkForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((EventHaikuForm)InputFormRef.JumpFormLow<EventHaikuForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((WorldMapPointForm)InputFormRef.JumpFormLow<WorldMapPointForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((WorldMapPathForm)InputFormRef.JumpFormLow<WorldMapPathForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((UnitPaletteForm)InputFormRef.JumpFormLow<UnitPaletteForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
            }
            else if (Program.ROM.RomInfo.version == 7)
            {//FE7
                ((UnitFE7Form)InputFormRef.JumpFormLow<UnitFE7Form>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((ClassForm)InputFormRef.JumpFormLow<ClassForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((ItemForm)InputFormRef.JumpFormLow<ItemForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                if (Program.ROM.RomInfo.is_multibyte)
                {
                    ((MapSettingFE7Form)InputFormRef.JumpFormLow<MapSettingFE7Form>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                }
                else
                {
                    ((MapSettingFE7UForm)InputFormRef.JumpFormLow<MapSettingFE7UForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                }
                ((SoundRoomForm)InputFormRef.JumpFormLow<SoundRoomForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((SupportUnitForm)InputFormRef.JumpFormLow<SupportUnitForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((EventBattleTalkFE7Form)InputFormRef.JumpFormLow<EventBattleTalkFE7Form>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((EventBattleTalkFE7Form)InputFormRef.JumpFormLow<EventBattleTalkFE7Form>()).N1_InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((EventHaikuFE7Form)InputFormRef.JumpFormLow<EventHaikuFE7Form>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
            }
            else
            {//FE6
                ((UnitFE6Form)InputFormRef.JumpFormLow<UnitFE6Form>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((ClassFE6Form)InputFormRef.JumpFormLow<ClassFE6Form>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((ItemFE6Form)InputFormRef.JumpFormLow<ItemFE6Form>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((MapSettingFE6Form)InputFormRef.JumpFormLow<MapSettingFE6Form>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((SoundRoomFE6Form)InputFormRef.JumpFormLow<SoundRoomFE6Form>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((SupportUnitFE6Form)InputFormRef.JumpFormLow<SupportUnitFE6Form>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((SupportTalkFE6Form)InputFormRef.JumpFormLow<SupportTalkFE6Form>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((EventBattleTalkFE6Form)InputFormRef.JumpFormLow<EventBattleTalkFE6Form>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((EventBattleTalkFE6Form)InputFormRef.JumpFormLow<EventBattleTalkFE6Form>()).N_InputFormRef.SaveDumpAutomatic(sb, saveDir);
                ((EventHaikuFE6Form)InputFormRef.JumpFormLow<EventHaikuFE6Form>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
            }
            ((SongTableForm)InputFormRef.JumpFormLow<SongTableForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
            ((ItemWeaponEffectForm)InputFormRef.JumpFormLow<ItemWeaponEffectForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
            ((ItemWeaponTriangleForm)InputFormRef.JumpFormLow<ItemWeaponTriangleForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
            ((SupportAttributeForm)InputFormRef.JumpFormLow<SupportAttributeForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
            ((AITargetForm)InputFormRef.JumpFormLow<AITargetForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);
            ((AIStealItemForm)InputFormRef.JumpFormLow<AIStealItemForm>()).InputFormRef.SaveDumpAutomatic(sb, saveDir);

            U.WriteAllText(filename, sb.ToString());
        }

        private void ExportUndoDataButton_Click(object sender, EventArgs e)
        {
            string title = R._("保存するファイル名を選択してください");
            string filter = R._("EA|*.event|All files|*");

            SaveFileDialog save = new SaveFileDialog();
            save.Title = title;
            save.Filter = filter;
            save.AddExtension = true;

            save.ShowDialog();
            if (save.FileNames.Length <= 0 || !U.CanWriteFileRetry(save.FileNames[0]))
            {
                return;
            }
            string filename = save.FileName;
            Program.LastSelectedFilename.Save(this, "", save);
            ExportUndoDataTable(filename);

            U.SelectFileByExplorer(filename);
        }
        void ExportUndoDataTable(string filename)
        {
            Dictionary<string, bool> checkDupMap = new Dictionary<string, bool>();

            //新しい方から逆順に探索します
            //同じデータを書かないためです.
            List<string> revLines = new List<string>();
            for (int i = Program.Undo.UndoBuffer.Count - 1; i >= 0; i--)
            {
                Undo.UndoData undodata = Program.Undo.UndoBuffer[i];

                bool isOutput = false;
                for (int n = undodata.list.Count - 1; n >= 0 ; n--)
                {
                    Undo.UndoPostion ud = undodata.list[n];
                    string key = ud.addr + "@" + ud.data.Length;
                    if (checkDupMap.ContainsKey(key))
                    {//既に出力済み
                        continue;
                    }
                    checkDupMap[key] = true;

                    if (ud.data.Length >= 1024 * 1024 * 2)
                    {
                        revLines.Add("//SKIP THIS DATA 2M OVER");
                        continue;
                    }
                    revLines.Add("BYTE " + U.HexDumpLinerDoll(Program.ROM.Data, ud.addr, (uint)ud.data.Length) + ";");
                    revLines.Add("ORG $" +  U.ToHexString(ud.addr));
                    isOutput = true;
                }
                if (isOutput)
                {
                    revLines.Add("//" + undodata.name + undodata.time);
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int i = revLines.Count - 1; i >= 0; i--)
            {
                sb.AppendLine(revLines[i]);
            }
            U.WriteAllText(filename, sb.ToString());
        }
    }
}
