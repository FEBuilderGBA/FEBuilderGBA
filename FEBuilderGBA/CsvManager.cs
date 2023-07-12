using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Windows.Forms;
using System.Diagnostics;

namespace FEBuilderGBA
{
    class CsvManager
    {
        private bool toClipboard = false;
        private bool includeHeader = false;
        private bool includeName = false;
        private bool useStats = false;
        private bool useGrowths = false;
        private bool growthAsDecimal = false;
        private bool includeUID = false;

        CsvManager(bool clipboard, bool useUID, bool header, bool unitName, bool stats, bool growths, bool growthDecimal) 
        {
            toClipboard = clipboard;
            includeHeader = header;
            includeName = unitName;
            useStats = stats;
            useGrowths = growths;
            growthAsDecimal = growthDecimal;
            includeUID = useUID;
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

        private string GetUnitDataFromROM(int index, InputFormRef InputFormRef) 
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

            // Growths
            if (useGrowths)
            {
                if (useGrowths)
                {
                    output += ", ";
                }
                output += (float)(sbyte)Program.ROM.u8(addr + 28) / growthDivisor + ", "; // hp
                output += (float)(sbyte)Program.ROM.u8(addr + 29) / growthDivisor + ", "; // str
                output += (float)(sbyte)Program.ROM.u8(addr + 30) / growthDivisor + ", "; // skill
                output += (float)(sbyte)Program.ROM.u8(addr + 31) / growthDivisor + ", "; // spd
                output += (float)(sbyte)Program.ROM.u8(addr + 32) / growthDivisor + ", "; // def
                output += (float)(sbyte)Program.ROM.u8(addr + 33) / growthDivisor + ", "; // res
                output += (float)(sbyte)Program.ROM.u8(addr + 34) / growthDivisor + ", "; // luck
                output += (float)(sbyte)MagicSplitUtil.GetUnitGrowMagicExtends(uid, addr) / growthDivisor; // mag
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
    }
}
