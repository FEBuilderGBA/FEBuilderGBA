using System;
using System.Text;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Windows.Forms;
using System.Diagnostics;

// DeepLを使用して翻訳された日本語コメント

namespace FEBuilderGBA
{
    /// <summary>
    /// A class designed to import and export CSV to files or clipboard.
    /// CSVをファイルやクリップボードにインポート/エクスポートするためのクラスです。
    /// </summary>
    class CsvManager
    {
        private bool toClipboard = false;
        private bool includeUID = false;
        private bool includeHeader = false;
        private bool includeName = false;
        private bool useStats = false;
        private bool useGrowths = false;
        private bool useWepLevel = false;
        private bool growthAsDecimal = false;
        private bool exportAsClass = false;
        private bool isUsingMagicSplit = false;
        
        public CsvManager(bool clipboard, bool useUID, bool header, bool unitName, bool stats, bool growths, bool wepLevel, bool growthDecimal, bool isClass) 
        {
            toClipboard = clipboard;
            includeUID = useUID;
            includeHeader = header;
            includeName = unitName;
            useStats = stats;
            useGrowths = growths;
            useWepLevel = wepLevel;
            growthAsDecimal = growthDecimal;
            exportAsClass = isClass;
            isUsingMagicSplit = MagicSplitUtil.SearchMagicSplit() == MagicSplitUtil.magic_split_enum.FE8UMAGIC;
        }

        public static String GetUnitNameByAddr(uint addr)
        {
            uint id = Program.ROM.u16(addr);
            return TextForm.Direct(id).Trim();
        }

        private string SetupHeader()
        {
            string output = "";
            if (!includeHeader)
            {
                return output;
            }

            if (includeName)
            {
                output += "Name, ";
            }

            if (useStats)
            {
                output += "HP, STR, SKL, SPD, DEF, RES, LUCK, CON";
                if (isUsingMagicSplit)
                {
                    output += ", MAG";
                }
            }

            if (useGrowths)
            {
                if (useStats) // add an extra comma here because we want to allow this to be the start if we don't have the stats
                {
                    output += ", ";
                }
                output += "HP, STR, SKL, SPD, DEF, RES, LUCK";
                if (isUsingMagicSplit)
                {
                    output += ", MAG";
                }
            }

            if (useWepLevel) 
            {
                if (useStats || useGrowths) 
                {
                    output += ", ";
                }
                output += "Sword, Lance, Axe, Bow, Staff, Anima, Light, Dark";
            }

            output += "\n";

            return output;
        }

        private string GetDataFromROM(int index, InputFormRef InputFormRef) 
        {
            int growthDivisor = growthAsDecimal == true ? 100 : 1;

            string output = "";
            uint uid = (uint)index;
            uint addr = InputFormRef.IDToAddr(uid);
            if (!U.isSafetyOffset(addr))
            {
                return "";
            }

            if (includeName)
            {
                output += GetUnitNameByAddr(addr);
                if (includeUID)
                {
                    output += "(" + uid + ")"; // export the UID in brackets
                    output += ", ";
                }
            }
            else if (includeUID)
            {
                output += uid + ", ";
            }

            // Base Stats
            if (useStats)
            {
                if (exportAsClass) 
                {
                    // hp 11, str 12, skl 13, spd 14, def 15, res 16, con 17
                    for (uint offset = 11; offset <= 17; offset++)
                    {
                        output += (int)(sbyte)Program.ROM.u8(addr + offset) + ", ";
                    }
                    if (isUsingMagicSplit) 
                    {
                        output += (int)(sbyte)MagicSplitUtil.GetClassBaseMagicExtends(uid, addr); // mag
                    }
                }
                else 
                {
                    // hp 12, str 13, skl 14, spd 15, def 16, res 17, luck 18, con 19
                    for (uint offset = 12; offset <= 19; offset++)
                    {
                        output += (int)(sbyte)Program.ROM.u8(addr + offset) + ", ";
                    }
                    if (isUsingMagicSplit) 
                    {
                        output += (int)(sbyte)MagicSplitUtil.GetUnitBaseMagicExtends(uid, addr); // mag
                    }
                }
            }

            // Growths
            if (useGrowths)
            {
                if (useGrowths)
                {
                    output += ", ";
                }
                if (exportAsClass) 
                {
                    // hp 27, str 28, skl 29, spd 30, def 31, res 32, luck 33
                    for (uint offset = 27; offset <= 33; offset++)
                    {
                        output += (float)(sbyte)Program.ROM.u8(addr + offset) / growthDivisor + ", ";
                    }
                    if (isUsingMagicSplit) 
                    {
                        output += (float)(sbyte)MagicSplitUtil.GetClassGrowMagicExtends(uid, addr) / growthDivisor; // mag
                    }
                }
                else 
                {
                    // hp 28, str 29, skl 30, spd 31, def 32, res 33, luck 34
                    for (uint offset = 28; offset <= 34; offset++)
                    {
                        output += (float)(sbyte)Program.ROM.u8(addr + offset) / growthDivisor + ", ";
                    }
                    if (isUsingMagicSplit) 
                    {
                        output += (float)(sbyte)MagicSplitUtil.GetUnitGrowMagicExtends(uid, addr) / growthDivisor; // mag
                    }
                }
            }

            if (useWepLevel) 
            {
                if (useStats || useGrowths)
                {
                    output += ", ";
                }
                if (exportAsClass) 
                {
                    for (uint offset = 44; offset <= 51; offset++)
                    {
                        output += (float)(sbyte)Program.ROM.u8(addr + offset) + ", "; 
                    }
                }
                else 
                {
                    for (uint offset = 20; offset <= 27; offset++)
                    {
                        output += (float)(sbyte)Program.ROM.u8(addr + offset) + ", ";
                    }
                }
            }

            output += "\n";
            return output;
        }

        private void WriteToFile(string output)
        {
            if (toClipboard)
            {
                Clipboard.SetText(output);
                MessageBox.Show("Copied Stats to Clipboard");
            }
            else
            {
                SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
                SaveFileDialog1.InitialDirectory = @"C:\";
                SaveFileDialog1.RestoreDirectory = true;
                SaveFileDialog1.DefaultExt = "csv";
                SaveFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                SaveFileDialog1.FilterIndex = 1;
                SaveFileDialog1.CheckFileExists = false;
                SaveFileDialog1.CheckPathExists = true;

                if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileStream fs = File.OpenWrite(SaveFileDialog1.FileName);
                    byte[] bytes = Encoding.UTF8.GetBytes(output);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();

                    string argument = "/select, \"" + SaveFileDialog1.FileName + "\"";
                    Process.Start("explorer.exe", argument);
                }
            }
        }
        private bool WriteDataToROM(InputFormRef InputFormRef, int selectedIndex = -1)
        {
            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
            OpenFileDialog1.InitialDirectory = @"C:\";
            OpenFileDialog1.RestoreDirectory = true;
            OpenFileDialog1.DefaultExt = "csv";
            OpenFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            OpenFileDialog1.FilterIndex = 1;
            OpenFileDialog1.CheckFileExists = true;
            OpenFileDialog1.CheckPathExists = true;

            uint index = 0;

            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Sanitize the data input to make sure it's all good
                TextFieldParser parser = new TextFieldParser(OpenFileDialog1.FileName);
                parser.TrimWhiteSpace = true;
                parser.Delimiters = new string[] { ", " };

                if (includeHeader) // if we included the header, we want to skip here since its not relevant
                {
                    parser.ReadLine();
                }

                while (parser.PeekChars(1) != null)
                {
                    string[] fieldRow = parser.ReadFields();

                    uint uid;
                    if (fieldRow[0].Length < 1)
                    {
                        break;
                    }

                    if (selectedIndex >= 0)
                    {
                        uid = (uint)selectedIndex;
                    }
                    // Decode fieldRow[0] to make it into the UID for the current unit
                    else if (includeName)
                    {
                        string[] name = fieldRow[0].Split('(', ')');
                        if (name.Length <= 2)
                        {
                            MessageBox.Show("Error: Missing UID at Index " + index + ". Aborting...");
                            return false;
                        }
                        uid = uint.Parse(name[1]);
                    }
                    else
                    {
                        uid = uint.Parse(fieldRow[0]);
                    }

                    uint addr = InputFormRef.IDToAddr(uid);
                    if (!U.isSafetyOffset(addr))
                    {
                        MessageBox.Show("Error: Outside Safety Offset at Index " + index + ". Aborting...");
                        return false;
                    }

                    Undo.UndoData undodata = Program.Undo.NewUndoData("ImportCharacterStats", U.ToHexString(uid), U.ToHexString(addr));

                    // we use a row index here so we can optionally skip over some functionality
                    // its a little messy but it lets us skip magic if the ROM doesn't support it
                    // this is also useful for using growths if base stats aren't included
                    // ここでは行インデックスを使用しているので、オプションでいくつかの機能をスキップすることができる。
                    // ROM がマジックをサポートしていない場合、マジックをスキップすることができます。
                    // 基本ステータスが含まれていない場合、成長を使用するのにも便利です。
                    uint rowIndex = 0;
                    if (includeName)
                    {
                        rowIndex++;
                    }
                    if (useStats)
                    {
                        // Write the stats to the ROM
                        if (exportAsClass) 
                        {
                            for (uint offset = 11; offset <= 17; offset++)
                            {
                                Program.ROM.write_u8(addr + offset, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata);
                                rowIndex++;
                            }

                            if (isUsingMagicSplit)
                            {
                                MagicSplitUtil.WriteClassBaseMagicExtends(uid, addr, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata); // mag
                                rowIndex++;
                            }
                        }
                        else 
                        {
                            for (uint offset = 12; offset <= 19; offset++) 
                            {
                                Program.ROM.write_u8(addr + offset, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata);
                                rowIndex++;
                            }

                            if (isUsingMagicSplit)
                            {
                                MagicSplitUtil.WriteUnitBaseMagicExtends(uid, addr, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata); // mag
                                rowIndex++;
                            }
                        }
                    }

                    if (useGrowths)
                    {
                        int growthDivisor = 1;
                        if (growthAsDecimal)
                        {
                            growthDivisor = 100;
                        }

                        if (exportAsClass) 
                        {
                            for (uint offset = 27; offset <= 33; offset++) 
                            {
                                Program.ROM.write_u8(addr + offset, (uint)(sbyte)Math.Round(float.Parse(fieldRow[rowIndex]) * growthDivisor), undodata); // hp
                                rowIndex++;
                            }

                            if (isUsingMagicSplit)
                            {
                                MagicSplitUtil.WriteClassGrowMagicExtends(uid, addr, (uint)(sbyte)Math.Round(float.Parse(fieldRow[rowIndex]) * growthDivisor), undodata); // mag
                                rowIndex++;
                            }
                        }
                        else 
                        {
                            for (uint offset = 28; offset <= 34; offset++) 
                            {
                                Program.ROM.write_u8(addr + offset, (uint)(sbyte)Math.Round(float.Parse(fieldRow[rowIndex]) * growthDivisor), undodata); // hp
                                rowIndex++;
                            }

                            if (isUsingMagicSplit)
                            {
                                MagicSplitUtil.WriteUnitGrowMagicExtends(uid, addr, (uint)(sbyte)Math.Round(float.Parse(fieldRow[rowIndex]) * growthDivisor), undodata); // mag
                                rowIndex++;
                            }
                        }
                    }

                    if (useWepLevel)
                    {
                        if (exportAsClass)
                        {
                            for (uint offset = 44; offset <= 51; offset++)
                            {
                                Program.ROM.write_u8(addr + offset, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata);
                                rowIndex++;
                            }
                        }
                        else
                        {
                            for (uint offset = 20; offset <= 27; offset++)
                            {
                                Program.ROM.write_u8(addr + offset, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata);
                                rowIndex++;
                            }
                        }
                    }
                    index++;
                    Program.Undo.Push(undodata);
                }
            }

            return true;
        }

        public string ExportSingle(InputFormRef InputFormRef, int index) 
        {
            string output = "";

            if (includeHeader)
            {
                output += SetupHeader();
            }

            output += GetDataFromROM(index, InputFormRef);

            WriteToFile(output);
            return output;
        }

        public string ExportList(InputFormRef InputFormRef, int count) 
        {
            string output = "";
            if (includeHeader)
            {
                output += SetupHeader();
            }

            for (int i = 0; i < count; i++)
            {
                output += GetDataFromROM(i, InputFormRef);
            }

            WriteToFile(output);

            return output;
        }

        public bool ImportSingle(InputFormRef InputFormRef, int selectedIndex) 
        {
            WriteDataToROM(InputFormRef, selectedIndex);
            return true;
        }

        public bool ImportList(InputFormRef InputFormRef) 
        {
            WriteDataToROM(InputFormRef);
            return true;
        }
    }
}
