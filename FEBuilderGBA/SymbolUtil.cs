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

        //readelf の symbol情報をEAのシンボル情報に変換します
        public static string ReadElfToEASymbol(string readelf)
        {
            StringBuilder sb = new StringBuilder();

            readelf = U.skip(readelf, "0: 00000000");
            string[] lines = readelf.Split(':');
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string name = U.cut(line, "DEFAULT    1 ", " ");
                if (name == "" || name[0] == ' ' || name[0] == '$')
                {
                    continue;
                }
                uint addr = U.atoh(line.Substring(1));
                if (addr <= 1 || addr >= 0x02000000)
                {
                    continue;
                }
                sb.Append(name);
                sb.Append("=$");
                sb.Append(U.ToHexString(addr));
                sb.AppendLine();
            }
            return sb.ToString();
        }

    }
}
