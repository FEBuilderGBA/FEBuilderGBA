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
        private bool growthAsDecimal = false;
        private bool exportAsClass = false;
        
        public CsvManager(bool clipboard, bool useUID, bool header, bool unitName, bool stats, bool growths, bool growthDecimal, bool isClass) 
        {
            toClipboard = clipboard;
            includeUID = useUID;
            includeHeader = header;
            includeName = unitName;
            useStats = stats;
            useGrowths = growths;
            growthAsDecimal = growthDecimal;
            exportAsClass = isClass;
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
                if (MagicSplitUtil.SearchMagicSplit() == MagicSplitUtil.magic_split_enum.FE8UMAGIC)
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
                if (MagicSplitUtil.SearchMagicSplit() == MagicSplitUtil.magic_split_enum.FE8UMAGIC)
                {
                    output += ", MAG";
                }
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
                    output += (int)(sbyte)Program.ROM.u8(addr + 11) + ", "; // hp
                    output += (int)(sbyte)Program.ROM.u8(addr + 12) + ", "; // str
                    output += (int)(sbyte)Program.ROM.u8(addr + 13) + ", "; // skill
                    output += (int)(sbyte)Program.ROM.u8(addr + 14) + ", "; // spd
                    output += (int)(sbyte)Program.ROM.u8(addr + 15) + ", "; // def
                    output += (int)(sbyte)Program.ROM.u8(addr + 16) + ", "; // res
                    output += (int)(sbyte)Program.ROM.u8(addr + 17) + ", "; // con
                    output += (int)(sbyte)MagicSplitUtil.GetClassBaseMagicExtends(uid, addr); // mag
                }
                else 
                {
                    output += (int)(sbyte)Program.ROM.u8(addr + 12) + ", "; // hp
                    output += (int)(sbyte)Program.ROM.u8(addr + 13) + ", "; // str
                    output += (int)(sbyte)Program.ROM.u8(addr + 14) + ", "; // skill
                    output += (int)(sbyte)Program.ROM.u8(addr + 15) + ", "; // spd
                    output += (int)(sbyte)Program.ROM.u8(addr + 16) + ", "; // def
                    output += (int)(sbyte)Program.ROM.u8(addr + 17) + ", "; // res
                    output += (int)(sbyte)Program.ROM.u8(addr + 18) + ", "; // luck
                    output += (int)(sbyte)Program.ROM.u8(addr + 19) + ", "; // con
                    output += (int)(sbyte)MagicSplitUtil.GetUnitBaseMagicExtends(uid, addr); // mag
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
                    output += (float)(sbyte)Program.ROM.u8(addr + 27) / growthDivisor + ", "; // hp
                    output += (float)(sbyte)Program.ROM.u8(addr + 28) / growthDivisor + ", "; // str
                    output += (float)(sbyte)Program.ROM.u8(addr + 29) / growthDivisor + ", "; // skill
                    output += (float)(sbyte)Program.ROM.u8(addr + 30) / growthDivisor + ", "; // spd
                    output += (float)(sbyte)Program.ROM.u8(addr + 31) / growthDivisor + ", "; // def
                    output += (float)(sbyte)Program.ROM.u8(addr + 32) / growthDivisor + ", "; // res
                    output += (float)(sbyte)Program.ROM.u8(addr + 33) / growthDivisor + ", "; // luck
                    output += (float)(sbyte)MagicSplitUtil.GetClassGrowMagicExtends(uid, addr) / growthDivisor; // mag
                }
                else 
                {
                    output += (float)(sbyte)Program.ROM.u8(addr + 28) / growthDivisor + ", "; // hp
                    output += (float)(sbyte)Program.ROM.u8(addr + 29) / growthDivisor + ", "; // str
                    output += (float)(sbyte)Program.ROM.u8(addr + 30) / growthDivisor + ", "; // skill
                    output += (float)(sbyte)Program.ROM.u8(addr + 31) / growthDivisor + ", "; // spd
                    output += (float)(sbyte)Program.ROM.u8(addr + 32) / growthDivisor + ", "; // def
                    output += (float)(sbyte)Program.ROM.u8(addr + 33) / growthDivisor + ", "; // res
                    output += (float)(sbyte)Program.ROM.u8(addr + 34) / growthDivisor + ", "; // luck
                    output += (float)(sbyte)MagicSplitUtil.GetUnitGrowMagicExtends(uid, addr) / growthDivisor; // mag
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

                if (includeHeader) // if we include the header, we want to skip it since its not relevant
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
                    // Decode fieldRow[0] to make it into the address of the current unit
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
                            Program.ROM.write_u8(addr + 11, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata); // hp
                            rowIndex++;
                            Program.ROM.write_u8(addr + 12, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata); // str
                            rowIndex++;
                            Program.ROM.write_u8(addr + 13, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata); // skl
                            rowIndex++;
                            Program.ROM.write_u8(addr + 14, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata); // spd
                            rowIndex++;
                            Program.ROM.write_u8(addr + 15, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata); // def
                            rowIndex++;
                            Program.ROM.write_u8(addr + 16, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata); // res
                            rowIndex++;
                            Program.ROM.write_u8(addr + 17, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata); // con
                            rowIndex++;

                            if (MagicSplitUtil.SearchMagicSplit() == MagicSplitUtil.magic_split_enum.FE8UMAGIC)
                            {
                                MagicSplitUtil.WriteClassBaseMagicExtends(uid, addr, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata); // mag
                                rowIndex++;
                            }
                        }
                        else 
                        {
                            Program.ROM.write_u8(addr + 12, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata); // hp
                            rowIndex++;
                            Program.ROM.write_u8(addr + 13, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata); // str
                            rowIndex++;
                            Program.ROM.write_u8(addr + 14, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata); // skl
                            rowIndex++;
                            Program.ROM.write_u8(addr + 15, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata); // spd
                            rowIndex++;
                            Program.ROM.write_u8(addr + 16, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata); // def
                            rowIndex++;
                            Program.ROM.write_u8(addr + 17, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata); // res
                            rowIndex++;
                            Program.ROM.write_u8(addr + 18, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata); // luk
                            rowIndex++;
                            Program.ROM.write_u8(addr + 19, (uint)sbyte.Parse(fieldRow[rowIndex]), undodata); // con
                            rowIndex++;

                            if (MagicSplitUtil.SearchMagicSplit() == MagicSplitUtil.magic_split_enum.FE8UMAGIC)
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
                            Program.ROM.write_u8(addr + 27, (uint)(sbyte)Math.Round(float.Parse(fieldRow[rowIndex]) * growthDivisor), undodata); // hp
                            rowIndex++;
                            Program.ROM.write_u8(addr + 28, (uint)(sbyte)Math.Round(float.Parse(fieldRow[rowIndex]) * growthDivisor), undodata); // str
                            rowIndex++;
                            Program.ROM.write_u8(addr + 29, (uint)(sbyte)Math.Round(float.Parse(fieldRow[rowIndex]) * growthDivisor), undodata); // skl
                            rowIndex++;
                            Program.ROM.write_u8(addr + 30, (uint)(sbyte)Math.Round(float.Parse(fieldRow[rowIndex]) * growthDivisor), undodata); // spd
                            rowIndex++;
                            Program.ROM.write_u8(addr + 31, (uint)(sbyte)Math.Round(float.Parse(fieldRow[rowIndex]) * growthDivisor), undodata); // def
                            rowIndex++;
                            Program.ROM.write_u8(addr + 32, (uint)(sbyte)Math.Round(float.Parse(fieldRow[rowIndex]) * growthDivisor), undodata); // res
                            rowIndex++;
                            Program.ROM.write_u8(addr + 33, (uint)(sbyte)Math.Round(float.Parse(fieldRow[rowIndex]) * growthDivisor), undodata); // luk
                            rowIndex++;
                            if (MagicSplitUtil.SearchMagicSplit() == MagicSplitUtil.magic_split_enum.FE8UMAGIC)
                            {
                                MagicSplitUtil.WriteClassGrowMagicExtends(uid, addr, (uint)(sbyte)Math.Round(float.Parse(fieldRow[rowIndex]) * growthDivisor), undodata); // mag
                                rowIndex++;
                            }
                        }
                        else 
                        {

                            Program.ROM.write_u8(addr + 28, (uint)(sbyte)Math.Round(float.Parse(fieldRow[rowIndex]) * growthDivisor), undodata); // hp
                            rowIndex++;
                            Program.ROM.write_u8(addr + 29, (uint)(sbyte)Math.Round(float.Parse(fieldRow[rowIndex]) * growthDivisor), undodata); // str
                            rowIndex++;
                            Program.ROM.write_u8(addr + 30, (uint)(sbyte)Math.Round(float.Parse(fieldRow[rowIndex]) * growthDivisor), undodata); // skl
                            rowIndex++;
                            Program.ROM.write_u8(addr + 31, (uint)(sbyte)Math.Round(float.Parse(fieldRow[rowIndex]) * growthDivisor), undodata); // spd
                            rowIndex++;
                            Program.ROM.write_u8(addr + 32, (uint)(sbyte)Math.Round(float.Parse(fieldRow[rowIndex]) * growthDivisor), undodata); // def
                            rowIndex++;
                            Program.ROM.write_u8(addr + 33, (uint)(sbyte)Math.Round(float.Parse(fieldRow[rowIndex]) * growthDivisor), undodata); // res
                            rowIndex++;
                            Program.ROM.write_u8(addr + 34, (uint)(sbyte)Math.Round(float.Parse(fieldRow[rowIndex]) * growthDivisor), undodata); // luk
                            rowIndex++;
                            if (MagicSplitUtil.SearchMagicSplit() == MagicSplitUtil.magic_split_enum.FE8UMAGIC)
                            {
                                MagicSplitUtil.WriteUnitGrowMagicExtends(uid, addr, (uint)(sbyte)Math.Round(float.Parse(fieldRow[rowIndex]) * growthDivisor), undodata); // mag
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
