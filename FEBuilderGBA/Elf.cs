using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace FEBuilderGBA
{
    public class Elf
    {
        public enum ScanType
        {
             All
            ,SymOnly
        }
        public Elf(string filename)
        {
            this.SymList = new List<Sym>();
            this.ProgramBIN = new byte[0];

            byte[] bin = File.ReadAllBytes(filename);
            if (!CheckELF(bin))
            {
                return;
            }
            ParseSHDR(bin);
        }

        bool CheckELF(byte[] bin)
        {
            if (bin.Length <= 0x34)
            {
                return false;
            }
            return (bin[1] == 'E' && bin[2] == 'L' && bin[3] == 'F' );
        }

        public class Sym
        {
            public string name;
            public uint addr;
//            public uint length;
        }
        public List<Sym> SymList { get; private set; }
        public byte[] ProgramBIN { get; private set; }

/*
typedef struct elfhdr{
         char   e_ident[EI_NIDENT];   ELF Identification 
+0x10        Elf32_Half      e_type;               // object file type 
+0x12        Elf32_Half      e_machine;            // machine 
+0x14        Elf32_Word      e_version;            // object file version 
+0x18        Elf32_Addr      e_entry;              // virtual entry point 
+0x1C        Elf32_Off       e_phoff;              // program header table offset 
+0x20        Elf32_Off       e_shoff;              // section header table offset 
+0x24        Elf32_Word      e_flags;              // processor-specific flags 
+0x28        Elf32_Half      e_ehsize;             // ELF header size 
+0x2A        Elf32_Half      e_phentsize;          // program header entry size 
+0x2C        Elf32_Half      e_phnum;              // number of program header entries 
+0x2E        Elf32_Half      e_shentsize;          // section header entry size 
+0x30        Elf32_Half      e_shnum;              // number of section header entries 
+0x32        Elf32_Half      e_shstrndx;           // section header table's "section header string table" entry offset 
} Elf32_Ehdr;
+0x34 == sizeof()

typedef struct {
+0x00	Elf32_Word	sh_name;
+0x04	Elf32_Word	sh_type;
+0x08	Elf32_Word	sh_flags;
+0x0C	Elf32_Addr	sh_addr;
+0x10	Elf32_Off	sh_offset;
+0x14	Elf32_Word	sh_size;
+0x18	Elf32_Word	sh_link;
+0x1C	Elf32_Word	sh_info;
+0x20	Elf32_Word	sh_addralign;
+0x24	Elf32_Word	sh_entsize;
} Elf32_Shdr;
+0x28 == sizeof()

typedef struct {
+0x00	Elf32_Word		st_name;
+0x04	Elf32_Addr		st_value;
+0x08	Elf32_Word		st_size;
+0x0C	uint8_t			st_info;
+0x0D	uint8_t			st_other;
+0x0E	Elf32_Half		st_shndx;
} Elf32_Sym;
+0x10 == sizeof()
*/
        const uint Elf32_Shdr_Size = 0x28;

        void ParseSYM(byte[] bin, uint baseaddr, uint e_shoff)
        {
            if (baseaddr + 0x1C > bin.Length)
            {
                return;
            }

            uint addr = baseaddr;
            uint sh_link = U.u32(bin, addr + 0x18);
            uint sh_size = U.u32(bin, addr + 0x14);
            uint sh_offset = U.u32(bin, addr + 0x10);

            //sh_link番目にあるデータに文字列テーブルがあるらしい.
            addr = e_shoff + sh_link * Elf32_Shdr_Size;
            uint shdr_linksecsion_sh_offset = U.u32(bin, addr + 0x10);

            //現在のSHDRテーブルを読む
            const uint Elf32_Sym_Size = 0x10;
            uint nloop_count = sh_size / Elf32_Sym_Size;
            for (uint n = 0; n < nloop_count; n++)
            {
                addr = sh_offset + (n * Elf32_Sym_Size);
                if (addr + 0x08 > bin.Length)
                {
                    break;
                }

                uint st_name = U.u32(bin, addr + 0x00);
                uint st_value = U.u32(bin, addr + 0x04);
                uint st_shndx = U.u16(bin, addr + 0x0E);

                if (st_name == 0)
                {
                    continue;
                }
                if (st_value <= 1)
                {
                    continue;
                }
                if (st_value >= 0x02000000)
                {
                    continue;
                }
                if (st_shndx > 0x1)
                {//fff1 externか何か?
                    continue;
                }

                uint nameaddr = shdr_linksecsion_sh_offset + st_name;
                if (nameaddr > bin.Length)
                {
                    continue;
                }
                if (bin[nameaddr] == '$' || bin[nameaddr] == 0 || bin[nameaddr] == ' ')
                {//変な名前なのはいらん.
                    continue;
                }

                string name = U.getASCIIString(bin, nameaddr);

                //名前がある場合、[sh_link].sh_offset + sym.st_name に名前がある. 0終端
                Sym sym = new Sym();
                sym.name = name;
                sym.addr = st_value;

                this.SymList.Add(sym);
            }
        }

        void ParseProgbits(byte[] bin, uint baseaddr)
        {
            const uint SHF_EXECINSTR = 0x04;

            uint addr = baseaddr;
            uint sh_flags = U.u32(bin , addr + 0x08);
            uint sh_offset = U.u32(bin , addr + 0x10);
            uint sh_size = U.u32(bin , addr + 0x14);

            if ((sh_flags & SHF_EXECINSTR) != SHF_EXECINSTR)
            {
                return;
            }
            if (sh_offset > bin.Length)
            {
                return;
            }
            if (sh_size == 0)
            {
                return;
            }
            if (sh_offset + sh_size > bin.Length)
            {
                return;
            }

            this.ProgramBIN = U.getBinaryData(bin, sh_offset, sh_size);
        }

        void ParseSHDR(byte[] bin)
        {
            const uint SHT_PROGBITS = 0x01;
            const uint SHT_SYMTAB = 0x02;
            const uint SHT_DYNSYM = 0x04;

            uint e_shoff = U.u32(bin, 0x20);
            uint e_shnum = U.u16(bin, 0x30);

            for (uint i = 0; i < e_shnum; i++)
            {
                uint addr = e_shoff + (i * Elf32_Shdr_Size);

                uint sh_type = U.u32(bin, addr + 0x04);
                if (sh_type == SHT_SYMTAB || sh_type == SHT_DYNSYM)
                {//シンボルテーブル
                    ParseSYM(bin, addr, e_shoff);
                }
                else if (sh_type == SHT_PROGBITS)
                {//プログラム
                    ParseProgbits(bin, addr);
                }
            }
        }
        public string ToEASymbol()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < SymList.Count; i++)
            {
                Sym sym = SymList[i];

                sb.Append(sym.name);
                sb.Append("=$");
                sb.Append(U.ToHexString(sym.addr));
                sb.AppendLine();
            }
            return sb.ToString();
        }

#if DEBUG
        public static void TEST_ELF()
        {
            Elf elf = new Elf(Program.GetTestData("test.elf"));
            Debug.Assert(elf.ProgramBIN.Length == 0x88);
            Debug.Assert(elf.ProgramBIN[0] == 0x10);
            Debug.Assert(elf.ProgramBIN[1] == 0xB4);
            Debug.Assert(elf.SymList.Count == 0x08);
            Debug.Assert(elf.SymList[0].name == "Table");
            Debug.Assert(elf.SymList[0].addr == 0x88);
        }
#endif
    }
}
