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

        static void ProcessSymbol(List<Address> list , string basefilename)
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

        public static void ProcessSymbol(List<Address> list 
            ,string basedir, string binfilename, uint baseaddr)
        {
            string ext = U.GetFilenameExt(binfilename);
            if (ext == ".ELF")
            {//elfだと自身にシンボルがある
                string elfFilename = Path.Combine(basedir, binfilename);
                if (File.Exists(elfFilename))
                {
                    ProcessSymbolElf(list, elfFilename, baseaddr);
                }
            }
            if (ext == ".BIN" || ext == ".DMP")
            {
                string symtxt = Path.Combine(basedir, Path.GetFileNameWithoutExtension(binfilename) + ".sym.txt");
                if (File.Exists(symtxt))
                {
                    ProcessSymbolSymTxt(list, symtxt, baseaddr, binfilename);
                }
            }
        }
        static void ProcessSymbolSymTxt(List<Address> list, string symtxt, uint baseaddr, string binfilename)
        {
            string basename = "@" + Path.GetFileName(binfilename);

            string[] lines = File.ReadAllLines(symtxt);
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

        static void ProcessSymbolElf(List<Address> list, string elffilename, uint baseaddr)
        {
            Elf elf = new Elf(elffilename);
            if (elf.ProgramBIN.Length < 0)
            {
                return;
            }

            string basename = "@" + Path.GetFileName(elffilename);
            foreach (Elf.Sym line in elf.SymList)
            {
                string name = line.name;
                uint addr = line.addr;
                addr += baseaddr;
                if (name.Length <= 0 || addr <= 0x100)
                {
                    continue;
                }

                addr = DisassemblerTrumb.ProgramAddrToPlain(addr);
                Program.CommentCache.Update(addr, name + basename);
            }
        }
    }
}
