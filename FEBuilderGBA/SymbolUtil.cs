using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace FEBuilderGBA
{
    public static class SymbolUtil
    {
        public enum DebugSymbol
        {
            None = 0
            ,SaveSymTxt = 1
            ,SaveComment = 2
            ,SaveBoth = 3
        };

        static void RegistSymbol(string basefilename, string symbol, uint baseaddr )
        {
            string basename = "@" + Path.GetFileName(basefilename);

            string[] lines = symbol.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                //EA形式
                //MMBDrawInventoryObjs=$100109D
                string[] sp = line.Split('=');
                if (sp.Length < 2)
                {
                    continue;
                }
                string name = sp[0];
                uint addr = U.atoi0x(sp[1]);
                addr += baseaddr;
                if (name.Length <= 0 || addr <= 0x100)
                {
                    continue;
                }

                addr = DisassemblerTrumb.ProgramAddrToPlain(addr);
                Program.CommentCache.Update(addr, name + basename);
            }
        }

        static string GetSymTxt(string basefilename)
        {
            return
                Path.Combine(
                     Path.GetDirectoryName(basefilename)
                    , Path.GetFileNameWithoutExtension(basefilename) + ".sym.txt"
                    );
        }
        static bool StoreSymbol(string basefilename, string symbol)
        {
            string path = GetSymTxt(basefilename);
            if (!U.CanWriteFileRetry(path))
            {
                return false;
            }
            if (symbol == "")
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
            }
            File.WriteAllText(path, symbol);
            return true;
        }

        public static void ProcessSymbol(string basefilename, string symbol, SymbolUtil.DebugSymbol storeSymbol , uint baseaddr)
        {
            if (storeSymbol == SymbolUtil.DebugSymbol.None)
            {//シンボルを利用しない
            }
            else if (storeSymbol == SymbolUtil.DebugSymbol.SaveSymTxt)
            {//シンボルをファイルに保存
                SymbolUtil.StoreSymbol(basefilename, symbol);
            }
            else if (storeSymbol == SymbolUtil.DebugSymbol.SaveComment)
            {//シンボルをコメントとして保存
                SymbolUtil.RegistSymbol(basefilename, symbol, baseaddr);
            }
            else if (storeSymbol == SymbolUtil.DebugSymbol.SaveBoth)
            {//両方
                SymbolUtil.StoreSymbol(basefilename, symbol); //まずは保存.
                SymbolUtil.RegistSymbol(basefilename, symbol, baseaddr);
            }
        }

        static void ProcessSymbol(string basefilename, List<Address> list)
        {
            foreach (Address a in list)
            {
                if (a.DataType != Address.DataTypeEnum.Comment)
                {
                    continue;
                }
                uint addr = a.Addr;
                addr = DisassemblerTrumb.ProgramAddrToPlain(addr);
                Program.CommentCache.Update(addr, a.Info);
            }
        }

        static void MakeAllDataLengthSub(List<Address> list, string symbolfilename, uint baseaddr)
        {
            string basename = "@" + Path.GetFileName(symbolfilename);

            string[] lines = File.ReadAllLines(symbolfilename);
            foreach (string line in lines)
            {
                //EA形式
                //MMBDrawInventoryObjs=$100109D
                string[] sp = line.Split('=');
                if (sp.Length < 2)
                {
                    continue;
                }
                string name = sp[0];
                uint addr = U.atoi0x(sp[1]);
                addr += baseaddr;
                if (name.Length <= 0 || addr <= 0x100)
                {
                    continue;
                }

                addr = DisassemblerTrumb.ProgramAddrToPlain(addr);
                Program.CommentCache.Update(addr, name + basename);
            }
        }

        public static void MakeAllDataLength(List<Address> list, string basefilename)
        {
            string[] files;
            try
            {
                files = Directory.GetFiles(basefilename, "*.sym.txt", SearchOption.AllDirectories);
            }
            catch (System.IO.IOException e)
            {
                R.Error("シンボル探索中にエラーが発生しました。\r\n{0}" , e.ToString());
                return ;
            }
            foreach (string fullpath in files)
            {
                string dir = Path.GetDirectoryName(fullpath);
                string filename = Path.GetFileNameWithoutExtension(fullpath);

                string binfilename;
                binfilename = Path.Combine(dir, filename, ".DMP");
                if (File.Exists(binfilename))
                {
                    uint baseaddr = U.Grep(Program.ROM.Data, File.ReadAllBytes(binfilename), Program.ROM.RomInfo.compress_image_borderline_address(), 0, 4);
                    if (baseaddr == U.NOT_FOUND)
                    {
                        continue;
                    }
                    MakeAllDataLengthSub(list, fullpath, baseaddr);
                    continue;
                }
                binfilename = Path.Combine(dir, filename, ".BIN");
                if (File.Exists(binfilename))
                {
                    uint baseaddr = U.Grep(Program.ROM.Data, File.ReadAllBytes(binfilename), Program.ROM.RomInfo.compress_image_borderline_address(), 0, 4);
                    if (baseaddr == U.NOT_FOUND)
                    {
                        continue;
                    }
                    MakeAllDataLengthSub(list, fullpath, baseaddr);
                    continue;
                }
            }
        }
    }
}
