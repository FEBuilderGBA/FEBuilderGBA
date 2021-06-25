using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FEBuilderGBA
{
    //ARM7-TDMI-manual-pt3.pdf
    public class DisassemblerTrumb
    {
        public DisassemblerTrumb()
        {

        }
        public DisassemblerTrumb(AsmMapFile asmMapFile)
        {
            this.ASMMapFile = asmMapFile;
        }

        //MAPファイルを共有します.
        //C++でいうところのconstつけて参照させたい.
        AsmMapFile ASMMapFile;

        public enum CodeType
        {
             Unknown //不明
            ,LDR     //LDR代入
            ,STR     //STR書込
            ,MOV  
            ,EXPR
            ,CMP
            ,JMP        //B jump
            ,BXJMP      //BX r2 みたいなレジスタジャンプ(関数終端でよく見られる)
            ,CONDJMP    //BEQ みたいな条件ジャンプ
            ,CALL       //BL
            ,PUSH
            ,POP
            ,BIOSCALL
            ,OTHER
        };
        public class VM
        {
            public uint[] r = new uint[14];
            //r14 SP  r15 PCは省略. 静的解析なんだし、正確さを求めてはいけない...
            public string[] type = new string[14];

            public void reset()
            {
                Array.Clear(r,0,r.Length);
                Array.Clear(type, 0, type.Length);
            }
            public void MoveType(uint dest, uint src)
            {
                Debug.Assert(dest < 14);
                Debug.Assert(src  < 14);
                if (type[src] != null)
                {
                    if (type[src].Length > 0)
                    {
                        type[dest] = type[src];
                    }
                }
            }
            public void MoveType(uint dest, uint src,uint src2)
            {
                Debug.Assert(dest < 14);
                Debug.Assert(src < 14);
                Debug.Assert(src2 < 14);
                if (type[src] != null)
                {
                    if (type[src].Length > 0)
                    {
                        type[dest] = type[src];
                    }
                }
                else if (type[src2] != null)
                {
                    if (type[src2].Length > 0)
                    {
                        type[dest] = type[src2];
                    }
                }
            }
            public string GetType(uint a)
            {
                if (a == U.NOT_FOUND)
                {
                    return "";
                }

                Debug.Assert(a < 14);
                if (type[a] != null)
                {
                    if (type[a].Length > 0)
                    {
                        return type[a];
                    }
                }
                return "";
            }

            public string CommentType(uint a1)
            {
                string type1 = GetType(a1);
                if (type1.Length > 0)
                {
                    return " r" + a1 + "=" + type1; 
                }
                return "";
            }
        };

        public class Code
        {
            public string ASM; //アセンブラ
            public string Comment; //コメント
            public CodeType Type; //種類
            public uint Data;     //付属するデータ
            public uint Data2;     //付属するデータ

            public Code(string asm, CodeType type, uint data, uint data2, string comment)
            {
                this.ASM = asm;
                this.Type = type;
                this.Data = data;
                this.Data2 = data2;
                this.Comment = comment;
            }
            public uint GetLength()
            {
                if (Type == CodeType.CALL) return 4; //CALLだけ4バイト
                return 2; //他は2バイト
            }
            public Code Clone()
            {
                Code c = new Code(this.ASM,this.Type,this.Data,this.Data2,this.Comment);
                return c;
            }
        };

        public Code Disassembler(byte[] bin, uint pos, uint maxlength, VM vm)
        {
            uint length = Math.Min((uint)bin.Length, maxlength);

            uint a = U.u16(bin, pos);
            switch (a & 0xF800)
            {//上位5ビットで命令分岐
                case (0x0 << 11): //LSL Format1
                    return Format1(bin, pos, a, vm, 0);
                case (0x1 << 11): //LSR Format1
                    return Format1(bin, pos, a, vm, 1);
                case (0x2 << 11): //ASR Format1
                    return Format1(bin, pos, a, vm, 2);
                case (0x3 << 11): //Format2
                    return Format2(bin, pos, a, vm);
                case (0x4 << 11): //MOV Format3
                    return Format3(bin, pos, a, vm, 0);
                case (0x5 << 11): //CMP Format3
                    return Format3(bin, pos, a, vm, 1);
                case (0x6 << 11): //ADD Format3
                    return Format3(bin, pos, a, vm, 2);
                case (0x7 << 11): //SUB Format3
                    return Format3(bin, pos, a, vm, 3);

                case (0x8 << 11): //Format4 or Format5
                    if ((a & (0x1 << 10)) <= 0)
                    {
                        return Format4(bin, pos, a, vm);
                    }
                    return Format5(bin, pos, a, vm);

                case (0x9 << 11): //LDR Format6
                    return Format6(bin, pos, a, vm);

                case (0xA << 11): //STR Format7 or Format8
                case (0xB << 11): //LDR Format7 or Format8
                    if ((a & (0x1 << 9)) <= 0)
                    {
                        return Format7(bin, pos, a, vm);
                    }
                    return Format8(bin, pos, a, vm);

                case (0xC << 11): //Format9
                case (0xD << 11): //Format9
                case (0xE << 11): //Format9
                case (0xF << 11): //Format9
                    return Format9(bin, pos, a, vm);

                case (0x10 << 11): //Format10
                case (0x11 << 11): //Format10
                    return Format10(bin, pos, a, vm);

                case (0x12 << 11): //Format11
                case (0x13 << 11): //Format11
                    return Format11(bin, pos, a, vm);

                case (0x14 << 11): //Format12
                case (0x15 << 11): //Format12
                    return Format12(bin, pos, a, vm);

                case (0x16 << 11): //Format13 or Format14
                case (0x17 << 11): //Format14
                    if ((a & (0x1 << 10)) <= 0)
                    {
                        return Format13(bin, pos, a, vm);
                    }
                    return Format14(bin, pos, a, vm);

                case (0x18 << 11): //Format15
                case (0x19 << 11): //Format15
                    return Format15(bin, pos, a);

                case (0x1A << 11): //Format16
                case (0x1B << 11): //Format16 or Format17
                    if ((a & (0xf << 8)) >= (0xf << 8))
                    {
                        return Format17(bin, pos, a);
                    }
                    return Format16(bin, pos, a);

                case (0x1C << 11): //Format18
                    return Format18(bin, pos, a);

                case (0x1D << 11): //Format19
                case (0x1E << 11): //Format19
                    return Format19(bin, pos, a, vm );
            }

            return new Code("@dcw	$" + a.ToString("X04"), CodeType.Unknown, a, 0, CommentFunction(pos));
        }



        DisassemblerTrumb.Code Format1(byte[] bin, uint pos, uint a,VM vm, uint hint)
        {
            if (a == 0)
            {//NOP
                return new Code("NOP", CodeType.Unknown, 0, 0, CommentFunction(pos));
            }

            uint Rd = ((a) & 0x7);
            uint Rs = ((a >> 3) & 0x7);
            uint imm = ((a >> 6) & 0x1F);

            string d = "r" + Rd + " ,r" + Rs + " ,#0x" + imm.ToString("X");

            if (hint == 0)
            {
                vm.r[Rd] = vm.r[Rs] << (int)imm;
                vm.MoveType(Rd, Rs);
                return new Code("LSL " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
            }
            else if (hint == 1)
            {
                vm.r[Rd] = vm.r[Rs] >> (int)imm;
                vm.MoveType(Rd, Rs);
                return new Code("LSR " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
            }
            else //if (hint == 2)
            {
                vm.r[Rd] = (uint)(((int)vm.r[Rs]) >> (int)imm);
                vm.MoveType(Rd, Rs);
                return new Code("ASR " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
            }
        }
        Code Format2(byte[] bin, uint pos, uint a, VM vm)
        {
            uint Rd = ((a) & 0x7);
            uint Rs = ((a >> 3) & 0x7);

            string d = "r" + Rd + " ,r" + Rs;
            if ((a & (1 << 10)) <= 0)
            {//rg
                uint Rn = ((a >> 6) & 0x7);
                d = d + ", R" + Rn;

                if ((a & (1 << 9)) <= 0)
                {
                    vm.r[Rd] = vm.r[Rs] + vm.r[Rn];
                    vm.MoveType(Rd, Rs, Rn);
                    return new Code("ADD " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
                }
                else
                {
                    vm.r[Rd] = vm.r[Rs] - vm.r[Rn];
                    vm.MoveType(Rd, Rs, Rn);
                    return new Code("SUB " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
                }
            }
            else
            {//imm
                uint imm = ((a >> 6) & 0x7);
                if (imm == 0)
                {
                    vm.r[Rd] = vm.r[Rs];
                    vm.MoveType(Rd, Rs);
                    return new Code("MOV " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
                }
                d = d + ", #0x" + imm.ToString("X");

                if ((a & (1 << 9)) <= 0)
                {
                    vm.r[Rd] = vm.r[Rs] + imm;
                    vm.MoveType(Rd, Rs);
                    return new Code("ADD " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
                }
                else
                {
                    vm.r[Rd] = vm.r[Rs] - imm;
                    vm.MoveType(Rd, Rs);
                    return new Code("SUB " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
                }
            }

        }
        Code Format3(byte[] bin, uint pos, uint a, VM vm, uint hint)
        {
            uint Rd = ((a >> 8) & 0x7);
            uint imm = (a & 0xFF);

            string d = "r" + Rd + ", #0x" + imm.ToString("X");
            if (hint == 0)
            {
                vm.r[Rd] = imm;
                return new Code("MOV " + d, CodeType.MOV, 0, 0, CommentFunction(pos));
            }
            else if (hint == 1)
            {
                return new Code("CMP " + d, CodeType.CMP, 0, 0, CommentFunction(pos));
            }
            else if (hint == 2)
            {
                vm.r[Rd] += imm;
                return new Code("ADD " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
            }
            else //if (hint == 3)
            {
                vm.r[Rd] -= imm;
                return new Code("SUB " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
            }
        }
        Code Format4(byte[] bin, uint pos, uint a, VM vm)
        {
            uint Rd = ((a) & 0x7);
            uint Rs = ((a >> 3) & 0x7);

            string d = "r" + Rd + " ,r" + Rs;
            switch (((a >> 6) & 0xF))
            {
                case 0:
                    vm.r[Rd] = vm.r[Rd] & Rs;
                    vm.MoveType(Rd, Rs);
                    return new Code("AND " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
                case 1:
                    return new Code("EOR " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
                case 2:
                    vm.r[Rd] = vm.r[Rd] << (int)vm.r[Rs];
                    vm.MoveType(Rd, Rs);
                    return new Code("LSL " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
                case 3:
                    vm.r[Rd] = vm.r[Rd] >> (int)vm.r[Rs];
                    vm.MoveType(Rd, Rs);
                    return new Code("LSR " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
                case 4:
                    vm.r[Rd] = (uint)(((int)vm.r[Rd]) >> (int)vm.r[Rs]);
                    return new Code("ASR " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
                case 5:
                    return new Code("ADC " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
                case 6:
                    return new Code("SBC " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
                case 7:
                    return new Code("ROR " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
                case 8:
                    return new Code("TST " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
                case 9:
                    vm.r[Rd] = 0 - vm.r[Rs];
                    vm.MoveType(Rd, Rs);
                    return new Code("NEG " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
                case 0xA:
                    return new Code("CMP " + d, CodeType.CMP, 0, 0, CommentFunction(pos));
                case 0xB:
                    return new Code("CMN " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
                case 0xC:
                    vm.r[Rd] = vm.r[Rd] | Rs;
                    vm.MoveType(Rd, Rs);
                    return new Code("ORR " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
                case 0xD:
                    vm.r[Rd] = vm.r[Rd] * Rs;
                    vm.MoveType(Rd, Rs);
                    return new Code("MUL " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
                case 0xE:
                    return new Code("BIC " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
                case 0xF:
                default:
                    return new Code("MVN " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
            }
        }
        Code Format5(byte[] bin, uint pos, uint a, VM vm)
        {
            uint Rd = ((a) & 0x7);
            uint Rs = ((a >> 3) & 0x7);

            string d;
            if (((a >> 7) & 0x1) <= 0)
            {
                d = "r" + Rd;
            }
            else
            {
                Rd += 8; //use r8-
                if (Rd == 13)
                {
                    d = "SP";
                }
                else if (Rd == 14)
                {
                    d = "LR";
                }
                else if (Rd == 15)
                {
                    d = "PC";
                }
                else
                {
                    d = "r" + Rd;
                }
            }

            string s;
            if (((a >> 6) & 0x1) <= 0)
            {
                s = "r" + Rs;
            }
            else
            {
                Rs += 8; //use r8-
                if (Rs == 13)
                {
                    s = "SP";
                }
                else if (Rs == 14)
                {
                    s = "LR";
                }
                else if (Rs == 15)
                {
                    s = "PC";
                }
                else
                {
                    s = "r" + Rs;
                }
            }

            switch (((a >> 8) & 0x3))
            {
                case 0:
                    if (Rd < 14 && Rs < 14)
                    {
                        vm.r[Rd] += vm.r[Rs];
                    }
                    return new Code("ADD " + d + ", " + s, CodeType.EXPR, 0, 0, CommentFunction(pos));
                case 1:
                    return new Code("CMP " + d + ", " + s, CodeType.CMP, 0, 0, CommentFunction(pos));
                case 2:
                    if (Rd < 14 && Rs < 14)
                    {
                        vm.r[Rd] = vm.r[Rs];
                    }
                    return new Code("MOV " + d + ", " + s, CodeType.MOV, 0, 0, CommentFunction(pos));
                case 3:
                default:
                    return new Code("BX " + s, CodeType.BXJMP, 0, 0, CommentFunction(pos));
            }
        }

        public static void TEST_Format5()
        {
            uint pos = 0x004fa8;
            byte[] d = new byte[] { 0x87, 0x46 };
            uint a = 0x4687;

            DisassemblerTrumb dis = new DisassemblerTrumb();
            
            DisassemblerTrumb.VM vm = new DisassemblerTrumb.VM();
            Code c = dis.Format5(d, pos, a, vm);
            Debug.Assert(c.ASM == "MOV PC, r0");
        }

        [MethodImpl(256)]
        static uint ParseLDRPointer(uint pos, uint a)
        {
            uint imm = (a & 0xff) << 2;
            uint pointer = U.toPointer(pos) + 2 + (imm);
            return pointer;
        }

        Code Format6(byte[] bin, uint pos, uint a, VM vm)
        {
            uint Rd = ((a >> 8) & 0x7);
            uint data = 0;

            string d = "r" + Rd;
            uint pointer = ParseLDRPointer(pos, a);
            pointer = U.Padding4(pointer);
            
            if (isSafetyPointer(bin, pointer))
            {
                pointer = U.toOffset(pointer);
                data = U.u32(bin, pointer);
            }

            string type = TypeFunction(pointer, data);
            if (type.Length < 0)
            {
                type = CommentFunction(pos);
            }
            string comment = MakeLDRComment(pointer, data, pos, Rd, U.NOT_FOUND, vm);
            uint imm = LDR_IMM(a);

            vm.r[Rd] = data;
            vm.type[Rd] = type;
            return new Code("LDR " + d + ", [PC, #0x" + imm.ToString("X") + "]", CodeType.LDR, data, pointer, comment);
        }

        Code Format7(byte[] bin, uint pos, uint a, VM vm)
        {
            uint Rd = ((a) & 0x7);
            uint Rs = ((a >> 3) & 0x7);
            uint offsetR = ((a >> 6) & 0x7);

            uint pointer = vm.r[Rs] + vm.r[offsetR];
            uint data = 0;
            if (isSafetyPointer(bin, pointer))
            {
                data = U.u32(bin, U.toOffset(pointer));
            }
            string type = TypeFunction(pointer, data);
            if (type.Length < 0)
            {
                type = CommentFunction(pos);
            }


            string d = "r" + Rd + ", [r" + Rs + ", r" + offsetR + "]";

            if ((a & (1 << 11)) <= 0)
            {
                //STRの時は、書き込まれる方の中身は出さないどうせ間違っているから
                string comment = MakeSTRComment(pointer, data, pos, Rd, Rs, vm);

                if ((a & (1 << 10)) <= 0)
                {
                    return new Code("STR " + d, CodeType.STR, data, pointer, comment);
                }
                else
                {
                    return new Code("STRB " + d, CodeType.STR, data, pointer, comment);
                }
            }
            else
            {
                string comment = MakeLDRComment(pointer, data, pos, Rd, Rs, vm);

                if ((a & (1 << 10)) <= 0)
                {
                    vm.r[Rd] = data;
                    vm.type[Rd] = type;
                    return new Code("LDR " + d, CodeType.LDR, data, pointer, comment);
                }
                else
                {
                    data = data & 0xff;
                    vm.r[Rd] = data ;
                    vm.type[Rd] = type;
                    return new Code("LDRB " + d, CodeType.LDR, data, pointer, comment);
                }
            }
        }
        public static void TEST_Format7_2()
        {
            byte[] d = new byte[] { 0x8D, 0x55 };
            DisassemblerTrumb dis = new DisassemblerTrumb();
            DisassemblerTrumb.VM vm = new VM();

            Code c = dis.Disassembler(d, 0, (uint)d.Length, vm);
            Debug.Assert(c.ASM == "STRB r5, [r1, r6]");
        }

      

        Code Format8(byte[] bin, uint pos, uint a, VM vm)
        {
            uint Rd = ((a) & 0x7);
            uint Rs = ((a >> 3) & 0x7);
            uint offsetR = ((a >> 6) & 0x7);

            uint pointer = vm.r[Rs] + vm.r[offsetR];
            uint data = 0;
            if (isSafetyPointer(bin, pointer))
            {
                data = U.u32(bin, U.toOffset(pointer));
            }
            string type = TypeFunction(pointer, data);
            if (type.Length < 0)
            {
                type = CommentFunction(pos);
            }


            string d = "r" + Rd + ", [r" + Rs + ", r" + offsetR + "]";

            if ((a & (1 << 11)) <= 0)
            {
                if ((a & (1 << 10)) <= 0)
                {
                    string comment = MakeSTRComment(pointer, data, pos, Rd, Rs, vm);

                    return new Code("STRH " + d, CodeType.STR, 0, 0, comment);
                }
                else
                {
                    string comment = MakeLDRComment(pointer, data, pos, Rd, Rs, vm);

                    data = 0xff;
                    vm.r[Rd] = data;
                    vm.type[Rd] = type;
                    return new Code("LDSB " + d, CodeType.LDR, 0, 0, comment);
                }
            }
            else
            {
                string comment = MakeLDRComment(pointer, data, pos, Rd, Rs, vm);

                if ((a & (1 << 10)) <= 0)
                {
                    data = 0xffff;
                    vm.r[Rd] = data;
                    vm.type[Rd] = type;
                    return new Code("LDRH " + d, CodeType.LDR, 0, 0, comment);
                }
                else
                {
                    data = 0xffff;
                    vm.r[Rd] = data;
                    vm.type[Rd] = type;
                    return new Code("LDSH " + d, CodeType.LDR, 0, 0, comment);
                }
            }
        }

        public static void TEST_Format8()
        {
            byte[] d = new byte[] { 0x40, 0x5E };
            
            DisassemblerTrumb dis = new DisassemblerTrumb();
            DisassemblerTrumb.VM vm = new DisassemblerTrumb.VM();
            Code c = dis.Disassembler(d, 0, 2, vm);
            Debug.Assert(c.ASM == "LDSH r0, [r0, r1]");
        }

        Code Format9(byte[] bin, uint pos, uint a, VM vm)
        {
            uint Rd = ((a) & 0x7);
            uint Rs = ((a >> 3) & 0x7);
            uint imm = ((a >> 6) & 0x1F);

            if ((a & (1 << 12)) <= 0)
            {
                imm = imm << 2;
            }
            uint pointer = vm.r[Rs] + imm;
            uint data = 0;
            if (isSafetyPointer(bin, pointer + 4))
            {
                pointer = U.toOffset(pointer);
                data = U.u32(bin, pointer);
            }
            string type = TypeFunction(pointer, data);
            if (type.Length < 0)
            {
                type = CommentFunction(pos);
            }

            string d = "r" + Rd + ", [r" + Rs + ", #0x" + imm.ToString("X") + "]";

            if ((a & (1 << 12)) <= 0)
            {
                if ((a & (1 << 11)) <= 0)
                {
                    string comment = MakeSTRComment(pointer, data, pos, Rd, Rs, vm);
                    return new Code("STR " + d, CodeType.STR, data, pointer, comment);
                }
                else
                {
                    string comment = MakeLDRComment(pointer, data, pos, Rd, Rs, vm);
                    vm.r[Rd] = data;
                    vm.type[Rd] = type;
                    return new Code("LDR " + d, CodeType.LDR, data, pointer, comment);
                }
            }
            else
            {
                if ((a & (1 << 11)) <= 0)
                {
                    string comment = MakeSTRComment(pointer, data, pos, Rd, Rs, vm);
                    return new Code("STRB " + d, CodeType.STR, data, pointer, comment);
                }
                else
                {
                    string comment = MakeLDRComment(pointer,data, pos, Rd, Rs, vm);
                    vm.r[Rd] = data;
                    vm.type[Rd] = type;
                    return new Code("LDRB " + d, CodeType.LDR, data, pointer, comment);
                }
            }
        }

        public static void TEST_Format9()
        {
            byte[] d = new byte[] { 0x9A, 0x68 };

            DisassemblerTrumb dis = new DisassemblerTrumb();
            DisassemblerTrumb.VM vm = new DisassemblerTrumb.VM();
            Code c2 = dis.Disassembler(d, 0, (uint)d.Length, vm);
            Debug.Assert(c2.ASM == "LDR r2, [r3, #0x8]");
        }

        string MakeLDRComment(uint pointer, uint data, uint pos, uint Rd, uint Rs, VM vm)
        {
            return CommentFunction(pointer, data) + CommentFunction(pos) + vm.CommentType(Rd) + vm.CommentType(Rs);
        }
        string MakeSTRComment(uint pointer, uint data, uint pos, uint Rd, uint Rs, VM vm)
        {
            string comment = "";
            if (pointer != U.NOT_FOUND && pointer > 0x01000000 && !U.isROMPointer(pointer))
            {
                if (data != U.NOT_FOUND && data > 0x01000000 && !U.isROMPointer(data))
                {
                    comment = CommentFunction(pointer, data);
                }
                else
                {
                    comment = CommentFunction(pointer);
                }
            }
            if (pos != U.NOT_FOUND && pos > 0x01000000 && !U.isROMPointer(pos))
            {
                comment += CommentFunction(pos);
            }
            return comment;
        }

        public static void TEST_Format9_2()
        {
            byte[] d = new byte[] { 0x80, 0x79 };

            DisassemblerTrumb dis = new DisassemblerTrumb();
            DisassemblerTrumb.VM vm = new DisassemblerTrumb.VM();
            Code c = dis.Disassembler(d, 0, 2, vm);
            Debug.Assert(c.ASM == "LDRB r0, [r0, #0x6]");
        }

        Code Format10(byte[] bin, uint pos, uint a, VM vm)
        {
            uint Rd = ((a) & 0x7);
            uint Rs = ((a >> 3) & 0x7);
            uint imm = ((a >> 6) & 0x1F);
            imm = imm * 2; //immのポインタは2倍する必要があるらしい..?

            uint pointer = vm.r[Rs] + imm;

            uint data = 0;
            if (isSafetyPointer(bin, pointer))
            {
                pointer = U.toOffset(pointer);
                data = U.u32(bin, pointer);
            }

            string d = "r" + Rd + ", [r" + Rs + ", #0x" + imm.ToString("X") + "]";

            if ((a & (1 << 11)) <= 0)
            {
                string comment = MakeSTRComment(pointer, data, pos, Rd, Rs, vm);
                return new Code("STRH " + d, CodeType.STR, data, pointer, comment);
            }
            else
            {
                string type = TypeFunction(pointer, data);
                if (type.Length < 0)
                {
                    type = CommentFunction(pos);
                }

                string comment = MakeLDRComment(pointer, data, pos, Rd, Rs, vm);

                data = data & 0xffff;
                vm.r[Rd] = data;
                vm.type[Rd] = type;
                return new Code("LDRH " + d, CodeType.LDR, data, pointer, comment);
            }
        }

#if DEBUG
        public static void TEST_Format10()
        {
            uint pos = 0x20ca4;
            byte[] d = new byte[] { 0x01, 0x8d };

            DisassemblerTrumb dis = new DisassemblerTrumb();
            DisassemblerTrumb.VM vm = new VM();
            uint a = 0x8d01;
            Code c = dis.Format10(d, pos, a, vm);
            Debug.Assert(c.ASM == "LDRH r1, [r0, #0x28]");
        }
#endif

        Code Format11(byte[] bin, uint pos, uint a, VM vm)
        {
            uint Rd = ((a >> 8) & 0x7);
            string d = "r" + Rd;
            uint imm = (a & 0xff);
            imm = imm * 4;

            if ((a & (1 << 11)) <= 0)
            {
                string comment = MakeSTRComment(U.NOT_FOUND, U.NOT_FOUND, pos, Rd, U.NOT_FOUND, vm);
                return new Code("STR " + d + ",[SP, #0x" + imm.ToString("X") + "]", CodeType.STR, 0, 0, comment);
            }
            else
            {
                string comment = MakeLDRComment(U.NOT_FOUND, U.NOT_FOUND, pos, Rd, U.NOT_FOUND, vm);
                return new Code("LDR " + d + ",[SP, #0x" + imm.ToString("X") + "]", CodeType.LDR, 0, 0, comment);
            }
        }
        Code Format12(byte[] bin, uint pos, uint a, VM vm)
        {
            string d = "r" + ((a >> 8) & 0x7);
            uint imm = (a & 0xff);

            if ((a & (1 << 11)) <= 0)
            {
                return new Code("ADD " + d + ",PC, #0x" + imm.ToString("X"), CodeType.EXPR,0, 0, CommentFunction(pos));
            }
            else
            {
                return new Code("ADD " + d + ",SP, #0x" + imm.ToString("X"), CodeType.EXPR, 0, 0, CommentFunction(pos));
            }
        }
        Code Format13(byte[] bin, uint pos, uint a, VM vm)
        {
            uint imm = (a & 0x7f);
            imm = imm * 4;

            if ((a & (1 << 7)) <= 0)
            {
                return new Code("ADD SP, #0x" + imm.ToString("X"), CodeType.EXPR, 0, 0, CommentFunction(pos));
            }
            else
            {
                return new Code("SUB SP, #0x" + imm.ToString("X"), CodeType.EXPR, 0, 0, CommentFunction(pos));
            }
        }
#if DEBUG

        public static void TEST_Format13()
        {
            uint pos = 0x988c6;
            byte[] d = new byte[] { 0x01, 0xb0 };

            DisassemblerTrumb dis = new DisassemblerTrumb();
            DisassemblerTrumb.VM vm = new VM();
            uint a = 0xb001;
            Code c = dis.Format13(d, pos, a, vm);
            Debug.Assert(c.ASM == "ADD SP, #0x4");
        }
        public static void TEST_Format13_2()
        {
            uint pos = 0x07AE80;
            byte[] d = new byte[] { 0x81, 0xb0 };

            DisassemblerTrumb dis = new DisassemblerTrumb();
            DisassemblerTrumb.VM vm = new VM();
            uint a = 0xb081;
            Code c = dis.Format13(d, pos, a, vm);
            Debug.Assert(c.ASM == "SUB SP, #0x4");
        }
#endif

        Code Format14(byte[] bin, uint pos, uint a, VM vm)
        {
            string d = "";
            if ((a & (1)) > 0)
            {
                d += ",r0";
            }
            if ((a & (1<<1)) > 0)
            {
                d += ",r1";
            }
            if ((a & (1<<2)) > 0)
            {
                d += ",r2";
            }
            if ((a & (1<<3)) > 0)
            {
                d += ",r3";
            }
            if ((a & (1<<4)) > 0)
            {
                d += ",r4";
            }
            if ((a & (1<<5)) > 0)
            {
                d += ",r5";
            }
            if ((a & (1<<6)) > 0)
            {
                d += ",r6";
            }
            if ((a & (1<<7)) > 0)
            {
                d += ",r7";
            }
            if (d.Length > 0)
            {
                d = d.Substring(1);
            }
            //PUSH POP には対応できないので VMの値は全部リセットする.
            //完ぺきではないがそこそこ正しいを目指す.
            vm.reset();

            if ((a & (1 << 11)) <= 0)
            {
                if ((a & (1 << 8)) <= 0)
                {
                    return new Code("PUSH {" + d + "}", CodeType.PUSH, 0, 0, CommentFunction(pos));
                }
                else
                {
                    if (d != "")
                    {
                        d = d + ",";
                    }
                    return new Code("PUSH {" + d + "lr}", CodeType.PUSH, 0, 0, CommentFunction(pos));
                }
            }
            else
            {
                if ((a & (1 << 8)) <= 0)
                {
                    return new Code("POP {" + d + "}", CodeType.POP, 0, 0, CommentFunction(pos));
                }
                else
                {
                    if (d != "")
                    {
                        d = d + ",";
                    }
                    return new Code("POP {" + d + "pc}", CodeType.POP, 0, 0, CommentFunction(pos));
                }
            }
        }

#if DEBUG
        public static void TEST_Format14()
        {
            uint pos = 0x0988d4;
            byte[] d = new byte[] { 0x10, 0xb5 };

            DisassemblerTrumb dis = new DisassemblerTrumb();
            DisassemblerTrumb.VM vm = new VM();
            uint a = 0xb510;
            Code c = dis.Format14(d,pos,a,vm);
            Debug.Assert(c.ASM == "PUSH {r4,lr}");
        }
        public static void TEST_Format14_2()
        {
            uint pos = 0x0988d4;
            byte[] d = new byte[] { 0x70, 0xBD };

            DisassemblerTrumb dis = new DisassemblerTrumb();
            DisassemblerTrumb.VM vm = new VM();
            uint a = 0xBD70;
            Code c = dis.Format14(d, pos, a, vm);
            Debug.Assert(c.ASM == "POP {r4,r5,r6,pc}");
        }
#endif
        Code Format15(byte[] bin, uint pos, uint a)
        {
            string d = "";
            if ((a & (1)) > 0)
            {
                d += ",r0";
            }
            if ((a & (1 << 1)) > 0)
            {
                d += ",r1";
            }
            if ((a & (1 << 2)) > 0)
            {
                d += ",r2";
            }
            if ((a & (1 << 3)) > 0)
            {
                d += ",r3";
            }
            if ((a & (1 << 4)) > 0)
            {
                d += ",r4";
            }
            if ((a & (1 << 5)) > 0)
            {
                d += ",r5";
            }
            if ((a & (1 << 6)) > 0)
            {
                d += ",r6";
            }
            if ((a & (1 << 7)) > 0)
            {
                d += ",r7";
            }
            if (d.Length > 0)
            {
                d = d.Substring(1);
            }

            d = "r" + ((a >> 8) & 0x7) + ",{" + d + "}";

            if ((a & (1 << 11)) <= 0)
            {
                return new Code("STMIA " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
            }
            else
            {
                return new Code("LDMIA " + d, CodeType.EXPR, 0, 0, CommentFunction(pos));
            }
        }
        Code Format16(byte[] bin, uint pos, uint a)
        {
            short imm = ((sbyte)(a & 0xff));
            uint addr = (uint)( U.toPointer(pos) + 4 + (imm<<1));

            switch (((a >> 8) & 0xF))
            {
                case 0:
                    return new Code("BEQ #0x" + addr.ToString("X"), CodeType.CONDJMP, addr, 0, CommentFunction(pos));
                case 1:
                    return new Code("BNE #0x" + addr.ToString("X"), CodeType.CONDJMP, addr, 0, CommentFunction(pos));
                case 2:
                    return new Code("BCS #0x" + addr.ToString("X"), CodeType.CONDJMP, addr, 0, CommentFunction(pos));
                case 3:
                    return new Code("BCC #0x" + addr.ToString("X"), CodeType.CONDJMP, addr, 0, CommentFunction(pos));
                case 4:
                    return new Code("BMI #0x" + addr.ToString("X"), CodeType.CONDJMP, addr, 0, CommentFunction(pos));
                case 5:
                    return new Code("BPL #0x" + addr.ToString("X"), CodeType.CONDJMP, addr, 0, CommentFunction(pos));
                case 6:
                    return new Code("BVS #0x" + addr.ToString("X"), CodeType.CONDJMP, addr, 0, CommentFunction(pos));
                case 7:
                    return new Code("BVC #0x" + addr.ToString("X"), CodeType.CONDJMP, addr, 0, CommentFunction(pos));
                case 8:
                    return new Code("BHI #0x" + addr.ToString("X"), CodeType.CONDJMP, addr, 0, CommentFunction(pos));
                case 9:
                    return new Code("BLS #0x" + addr.ToString("X"), CodeType.CONDJMP, addr, 0, CommentFunction(pos));
                case 0xA:
                    return new Code("BGE #0x" + addr.ToString("X"), CodeType.CONDJMP, addr, 0, CommentFunction(pos));
                case 0xB:
                    return new Code("BLT #0x" + addr.ToString("X"), CodeType.CONDJMP, addr, 0, CommentFunction(pos));
                case 0xC:
                    return new Code("BGT #0x" + addr.ToString("X"), CodeType.CONDJMP, addr, 0, CommentFunction(pos));
                case 0xD:
                    return new Code("BLE #0x" + addr.ToString("X"), CodeType.CONDJMP, addr, 0, CommentFunction(pos));
                case 0xE:
                    return new Code("??? #0x" + addr.ToString("X"), CodeType.CONDJMP, addr, 0, CommentFunction(pos));
                case 0xF:
                default:
                    return new Code("??? #0x" + addr.ToString("X"), CodeType.CONDJMP, addr, 0, CommentFunction(pos));
            }
        }

        public static void TEST_Format16()
        {
            uint pos = 0x04f56;
            byte[] d = new byte[] { 0x00, 0xDB };
            uint a = 0xDB00;

            DisassemblerTrumb dis = new DisassemblerTrumb();
            Code c = dis.Format16(d, pos, a);
            Debug.Assert(c.Data == 0x8004f5a);
        }
        public static void TEST_Format16_2()
        {
            uint pos = 0x04f62;
            byte[] d = new byte[] { 0x17, 0xD0 };
            uint a = 0xD017;

            DisassemblerTrumb dis = new DisassemblerTrumb();
            Code c = dis.Format16(d, pos, a);
            Debug.Assert(c.Data == 0x8004f94);
        }
        public static void TEST_Format16_3()
        {
            uint pos = 0x04f6c;
            byte[] d = new byte[] { 0x02, 0xD0 };
            uint a = 0xD002;

            DisassemblerTrumb dis = new DisassemblerTrumb();
            Code c = dis.Format16(d, pos, a);
            Debug.Assert(c.Data == 0x8004f74);
        }
        public static void TEST_Format16_4()
        {
            uint pos = 0x05B8E;
            byte[] d = new byte[] { 0xEB, 0xD8 };
            uint a = 0xD8EB;

            DisassemblerTrumb dis = new DisassemblerTrumb();
            Code c = dis.Format16(d, pos, a);
            Debug.Assert(c.Data == 0x8005B68);
        }
       

        Code Format17(byte[] bin, uint pos, uint a)
        {
            uint imm = (a & 0x1F);
            return new Code("SWI #x" + imm.ToString("X"), CodeType.BIOSCALL, 0, 0, BIOSCommentFunction(imm));
        }

        [MethodImpl(256)]
        static int ParseJumpIM(uint a)
        {
            int im = 0;
            if ((a & (1 << 10)) > 0)
            {//マイナス.
                im = -1 * (((int)(0x3FF ^ (a & 0x3FF)) + 1));
            }
            else
            {
                im = (int)(a & 0x3FF);
            }
            return im;
        }
        static uint ParseJumpIMM(uint pos, uint a)
        {
            int im = ParseJumpIM(a);
            uint imm = (uint)(U.toPointer(pos) + 4 + (im << 1));
            return imm;
        }

        Code Format18(byte[] bin, uint pos, uint a)
        {
            uint imm = ParseJumpIMM(pos, a);
            return new Code("B 0x" + imm.ToString("X"), CodeType.JMP, imm, 0, CommentFunction(imm) + CommentFunction(pos));
        }

        [MethodImpl(256)]
        static uint ParseCallIMM(byte[] bin, uint pos, uint a)
        {
            int im = 0;
            if ((a & (1 << 10)) > 0)
            {//マイナス.
                im = -1 * (((int)(0x3FF ^ (a & 0x3FF)) + 1));
            }
            else
            {
                im = (int)(a & 0x3FF);
            }

            uint b = U.u16(bin, pos + 2);
            int im2 = (int)(b & 0x7FF);
            uint imm = (uint)(U.toPointer(pos) + 4 + (im << 12) + (im2 << 1));
            return imm;
        }
        Code Format19(byte[] bin, uint pos, uint a, VM vm)
        {
            uint imm = ParseCallIMM(bin, pos, a);
            string type = TypeFunction(pos,imm);
            vm.type[0] = type;

            return new Code("BL 0x" + imm.ToString("X08"), CodeType.CALL, imm, 0, CommentFunction(imm) + CommentFunction(pos));
        }

        [MethodImpl(256)]
        static bool isSafetyPointer(byte[] bin, uint a)
        {
            return (a < 0x0A000000 && a >= 0x08000200 && a - 0x08000000 < bin.Length);
        }
        string CommentFunction(uint addr)
        {
            if (this.ASMMapFile == null)
            {
                return "";
            }

            uint pointer = U.toPointer(addr);

            AsmMapFile.AsmMapSt p;
            if (this.ASMMapFile.TryGetValue(pointer, out p))
            {
                return "   //" + p.Name + U.SA(p.ResultAndArgs)+ Program.CommentCache.S_At(addr);
            }
            string comment;
            if (Program.CommentCache.TryGetValue(addr, out comment))
            {
                return "  //" + comment;
            }

            return "";
        }
        string BIOSCommentFunction(uint swi)
        {
            return "   //BIOS: " + AsmMapFile.GetSWI_GBA_BIOS_CALL(swi);
        }

        string TypeFunction(uint addr,uint data)
        {
            addr = U.toPointer(addr);
            if (this.ASMMapFile == null)
            {
                return "";
            }

            AsmMapFile.AsmMapSt p;

            if (addr < 0x08000000)
            {//ROMデータでないので参照が取れない.
                if (this.ASMMapFile.TryGetValue(addr, out p))
                {
                    return p.TypeName;
                }
                return "";
            }

            if (this.ASMMapFile.TryGetValue(data, out p))
            {//データ参照先からデータが取れるなら参照先だけ明記する.
                return p.TypeName;
            }
            else
            {//参照先から取れないなら、参照元から名前を取ってみる
                if (this.ASMMapFile.TryGetValue(addr, out p))
                {
                    return p.TypeName;
                }
                return "";
            }

        }

        string CommentFunction(uint addr, uint data = 0)
        {
            if (addr == U.NOT_FOUND)
            {
                return "";
            }

            addr = U.toPointer(addr);
            if (addr < 0x02000000)
            {
                return "";
            }
            if (addr >= 0x08000000 && addr <= 0x08000500 )
            {
                return "";
            }
            if (addr > 0x0F000000)
            {
                return "";
            }
            if (this.ASMMapFile == null)
            {
                return "";
            }

            string comment = " # pointer:" + addr.ToString("X08");
            AsmMapFile.AsmMapSt p;

            if (addr < 0x08000000)
            {//ROMデータでないので参照が取れない.
                if (this.ASMMapFile.TryGetValue(addr, out p))
                {
                    comment += " (" + p.Name + " " + p.ResultAndArgs + ")";
                }
                return comment;
            }

            if (this.ASMMapFile.TryGetValue(data, out p))
            {//データ参照先からデータが取れるなら参照先だけ明記する.
                comment += " -> " + data.ToString("X08");
                comment += " (" + p.Name + " " + p.ResultAndArgs + ")";
                return comment;
            }
            if (U.isPointerASM(data))
            {//LYNでは、アドレス+1のデータが使われるので、それ用にパースしてみる.
                if (this.ASMMapFile.TryGetValue(data - 1, out p))
                {
                    comment += " -> " + data.ToString("X08");
                    comment += " (" + p.Name + " " + p.ResultAndArgs + ")";
                    return comment;
                }
            }

            {//参照先から取れないなら、参照元から名前を取ってみる
                if (this.ASMMapFile.TryGetValue(addr, out p))
                {
                    comment += " (" + p.Name + " " + p.ResultAndArgs + ")";
                }
                comment += " -> " + data.ToString("X08");
            }
            return comment;
        }

        


        public static void TEST_Format9_4()
        {
            uint pos = 0x00498E;
            byte[] d = new byte[] { 0xC8,0x60 };
            uint a = 0x60C8;

            DisassemblerTrumb dis = new DisassemblerTrumb();
            DisassemblerTrumb.VM vm = new DisassemblerTrumb.VM();
            Code c = dis.Format9(d, pos, a, vm);
            Debug.Assert(c.ASM == "STR r0, [r1, #0xC]");
        }

        public static void TEST_Format9_3()
        {
            uint pos = 0x004990;
            byte[] d = new byte[] { 0x48,0x60 };
            uint a = 0x6848;

            DisassemblerTrumb dis = new DisassemblerTrumb();
            DisassemblerTrumb.VM vm = new DisassemblerTrumb.VM();
            Code c = dis.Format9(d, pos, a, vm);
            Debug.Assert(c.ASM == "LDR r0, [r1, #0x4]");
        }

        //長さを求めるため、長さに関係する項目だけを取得する
        static CodeType DisassemblerFastForLength(byte[] bin, uint pos, out uint out_data)
        {
            out_data = 0;

            uint a = U.u16(bin, pos);
            switch (a & 0xF800)
            {//上位5ビットで命令分岐
                case (0x0 << 11): //LSL Format1
                    if (a == 0)
                    {
                        return CodeType.Unknown;
                    }
                    return CodeType.OTHER;
                case (0x1 << 11): //LSR Format1
                case (0x2 << 11): //ASR Format1
                case (0x3 << 11): //Format2
                case (0x4 << 11): //MOV Format3
                case (0x5 << 11): //CMP Format3
                case (0x6 << 11): //ADD Format3
                case (0x7 << 11): //SUB Format3
                    return CodeType.OTHER;

                case (0x8 << 11): //Format4 or Format5
                    if ((a & (0x1 << 10)) <= 0)
                    {
                        return CodeType.OTHER; //format4
                    }
                    if (((a >> 8) & 0x3) == 0x3)
                    {//Format5 の BX
                        return CodeType.BXJMP;
                    }
                    return CodeType.OTHER;
                case (0x9 << 11): //LDR Format6
                    {
                        uint pointer = ParseLDRPointer(pos, a);
                        pointer = U.toOffset(pointer);
                        out_data = U.Padding4(pointer);
                    }
                    return CodeType.LDR;
                case (0xA << 11): //STR Format7 or Format8
                case (0xB << 11): //LDR Format7 or Format8
                case (0xC << 11): //Format9
                case (0xD << 11): //Format9
                case (0xE << 11): //Format9
                case (0xF << 11): //Format9
                case (0x10 << 11): //Format10
                case (0x11 << 11): //Format10
                case (0x12 << 11): //Format11
                case (0x13 << 11): //Format11
                case (0x14 << 11): //Format12
                case (0x15 << 11): //Format12
                    return CodeType.OTHER;
                case (0x16 << 11): //Format13 or Format14
                case (0x17 << 11): //Format14
                    if ((a & (0x1 << 10)) <= 0)
                    {
                        return CodeType.OTHER; //format13
                    }

                    if ((a & (1 << 11)) <= 0)
                    {
                        return CodeType.PUSH;
                    }
                    else
                    {
                        return CodeType.POP;
                    }
                case (0x18 << 11): //Format15
                case (0x19 << 11): //Format15
                    return CodeType.OTHER;
                case (0x1A << 11): //Format16
                case (0x1B << 11): //Format16 or Format17
                    if ((a & (0xf << 8)) >= (0xf << 8))
                    {//format 17
                        return CodeType.OTHER;
                    }
                    short imm = ((sbyte)(a & 0xff));
                    out_data = (uint)(U.toPointer(pos) + 4 + (imm << 1));
                    return CodeType.CONDJMP;
                case (0x1C << 11): //Format18

                    out_data = ParseJumpIMM(pos,a);
                    return CodeType.JMP;
                case (0x1D << 11): //Format19
                case (0x1E << 11): //Format19

                    out_data = ParseCallIMM(bin, pos, a);
                    return CodeType.CALL;
            }

            return CodeType.Unknown;
        }

        public static bool IsCodeArea(uint addr)
        {
            return (addr < 0x100000 || (addr >= 0xFE0000 && addr <= 0xFE3000)) ;
        }

        public static uint CalcLength(byte[] prog,uint addr, uint limit, List<uint> out_ldrtable)
        {
            addr = DisassemblerTrumb.ProgramAddrToPlain(addr);
            return CalcLengthByHack(prog, addr, limit, out_ldrtable);
        }

        //長さを計算する. Cで書かれた関数用
        static uint CalcLengthByC(byte[] prog, uint addr, uint limit, List<uint> ldrtable)
        {
            uint unknown_start_addr=0;
            uint unknown = 0;
            uint startaddr = addr;
            while ( addr < limit )
            {
                if (ldrtable.IndexOf(addr) >= 0)
                {//LDR参照のポインタデータが入っている
                    addr += 4;
                    continue;
                }

                //長さを求めるため、長さに関係する項目だけを取得する
                uint data;
                CodeType type = DisassemblerFastForLength(prog, addr, out data);

                if (type == DisassemblerTrumb.CodeType.Unknown)
                {
                    unknown++;
                    if (unknown == 1)
                    {
                        unknown_start_addr = addr;
                    }
                    else if (unknown >= 0x1f)
                    {//不明が連続したら止める
                        return unknown_start_addr - startaddr;
                    }
                }
                else
                {//ちゃんとした命令
                    unknown = 0;
                }


                if (type == CodeType.CALL)
                {//CALLだけ32ビットなので4バイト
                    addr += 4;
                }
                else
                {//他の命令は2バイト
                    addr += 2;
                }

                if (type == CodeType.LDR)
                {//LDRならばデータ参照を記録.
                    ldrtable.Add(data);
                }

                if (type == CodeType.BXJMP)
                {//関数終端 LDR参照があるので終了.

                    //しかし、LDR参照のデータが関数の後ろにあることがあるので、調べる.
                    while (addr < limit)
                    {
                        if (ldrtable.IndexOf(addr) >= 0)
                        {//LDR参照のポインタデータが入っている
                            addr += 4;
                            continue;
                        }
                        break;
                    }
                    break;
                }
            }

            return addr - startaddr;
        }

#if DEBUG
        public static void TEST_CalcLengthByHack1()
        {
            byte[] bin = File.ReadAllBytes(Program.GetTestData("MMBPrepStats.dmp"));
            List<uint> ldrtable = new List<uint>();
            uint length = CalcLengthByHack(bin, 0, (uint)bin.Length, ldrtable);
            Debug.Assert(length == 0x3C);
        }
#endif


        //フック処理
        //LDR参照のデータが末尾に登場するのでそれを見つける.
        public static uint CalcLengthByHack(byte[] prog
            ,uint addr, uint limit, List<uint> ldrtable)
        {
            uint unknown_start_addr = 0;
            uint unknown = 0;
            uint startaddr = addr;
            uint lastJumpAddr = 0;
            while (addr < limit)
            {
                if (ldrtable.IndexOf(addr) >= 0)
                {//LDR参照のポインタデータが入っている
                    addr += 4;
                    continue;
                }
                if (ldrtable.Count != 0)
                {//LDR参照テーブルが存在していて、
                    uint maxldr = U.GetMaxValue(ldrtable);
                    if (addr > maxldr)
                    {//LDR参照値で一番大きい値をすぎているので終了するべき
                        break;
                    }
                }

                //長さを求めるため、長さに関係する項目だけを取得する
                uint data;
                CodeType type = DisassemblerFastForLength(prog, addr, out data);

                if (type == DisassemblerTrumb.CodeType.Unknown)
                {
                    unknown++;
                    if (unknown == 1)
                    {
                        unknown_start_addr = addr;
                    }
                    else if (unknown >= 0x1f)
                    {//不明が連続したら止める
                        return unknown_start_addr - startaddr;
                    }
                }
                else
                {//ちゃんとした命令
                    unknown = 0;
                }

                if (type == CodeType.CALL)
                {//CALLだけ32ビットなので4バイト
                    addr += 4;
                }
                else
                {//他の命令は2バイト
                    addr += 2;
                }

                if (type == CodeType.LDR)
                {//LDRならばデータ参照を記録.
                    ldrtable.Add(data);
                }
                else if (type == CodeType.JMP 
                    || type == CodeType.CALL 
                    || type == CodeType.CONDJMP)
                {
                    data = U.toOffset(data);
                    if (data > addr              //現在のaddrより後方へ参照していて
                        && data < limit          //0xffffffみたいに外へ参照しているわけでもなく
                        && data < addr + 0x200   //ありえないぐらい遠くなく
                        && lastJumpAddr < data   //既知の最後の参照エリアより後ろにあること
                        )
                    {
                        lastJumpAddr = data;
                    }
                }
                else if (type == CodeType.BXJMP)
                {//関数終端? LDR参照があるので終了.
                    if (ldrtable.Count == 0)
                    {//LDRテーブルが空なら終了させるべき
                        break;
                    }
                }
            }

            uint retlength = addr - startaddr;
            if (lastJumpAddr >= addr)
            {//ジャンプで、もっと後ろへのとび先が登録されている.
                List<uint> nextLdrTable = new List<uint>();
                uint nestLength = CalcLengthByHack(prog,lastJumpAddr, limit, nextLdrTable);
                //ネストの結果をマージする.
                ldrtable.AddRange(nextLdrTable);
                retlength = (lastJumpAddr + nestLength) - startaddr;
            }
            return retlength;
        }




        //LDRで指されるデータの検索
        public static List<uint> GrepLDRData(byte[] romdata,uint need, uint start = 0x100, uint limit = 0)
        {
            need = U.toPointer(need);

            List<uint> ret = new List<uint>();

            uint addr = U.Padding2(start);
            if (limit == 0)
            {
                limit = (uint)romdata.Length - 4;
            }
            limit = U.Padding2(limit);

            while (addr < limit)
            {
                uint a = U.u16(romdata, addr);
                switch (a & 0xF800)
                {//上位5ビットで命令分岐
                    case (0x0 << 11): //LSL Format1
                    case (0x1 << 11): //LSR Format1
                    case (0x2 << 11): //ASR Format1
                    case (0x3 << 11): //Format2
                    case (0x4 << 11): //MOV Format3
                    case (0x5 << 11): //CMP Format3
                    case (0x6 << 11): //ADD Format3
                    case (0x7 << 11): //SUB Format3
                    case (0x8 << 11): //Format4 or Format5
                        addr += 2;
                        break;
                    case (0x9 << 11): //LDR Format6
                        {
                            uint pointer = ParseLDRPointer(addr, a);
                            pointer = U.toOffset(pointer);
                            pointer = U.Padding4(pointer);
                            if (pointer < limit)
                            {
                                uint d = U.u32(romdata, pointer);
                                if (need == d)
                                {
                                    ret.Add(pointer);
                                }
                            }
                        }
                        addr += 2;
                        break;
                    case (0xA << 11): //STR Format7 or Format8
                    case (0xB << 11): //LDR Format7 or Format8
                    case (0xC << 11): //Format9
                    case (0xD << 11): //Format9
                    case (0xE << 11): //Format9
                    case (0xF << 11): //Format9
                    case (0x10 << 11): //Format10
                    case (0x11 << 11): //Format10
                    case (0x12 << 11): //Format11
                    case (0x13 << 11): //Format11
                    case (0x14 << 11): //Format12
                    case (0x15 << 11): //Format12
                    case (0x16 << 11): //Format13 or Format14
                    case (0x17 << 11): //Format14
                    case (0x18 << 11): //Format15
                    case (0x19 << 11): //Format15
                    case (0x1A << 11): //Format16
                    case (0x1B << 11): //Format16 or Format17
                    case (0x1C << 11): //Format18
                        addr += 2;
                        break;
                    case (0x1D << 11): //Format19
                    case (0x1E << 11): //Format19
                        addr += 4;
                        break;
                    default:
                        addr += 2;
                        break;
                }
            }

            return ret;
        }

        //LDRで指されるデータのマップを作成.
        public class LDRPointer{
            public uint ldr_address; //LDRが書いてあったプログラムのアドレス
            public uint ldr_data_address; //LDRが指しているポインタが書いてあるアドレス
            public uint ldr_data; //そこに書かれているポインタ。つまり、 ldr r1, [ldr_data]の値.
        };
        public static List<LDRPointer> MakeLDRMap(byte[] prog, uint start , uint limit = 0,bool isPointerOnly = false)
        {
            List<LDRPointer> ret = new List<LDRPointer>();

            uint addr = U.Padding2(start);
            if (limit == 0)
            {
                if (prog.Length < 4)
                {//短すぎてリストを作れない.
                    return ret;
                }
                limit = (uint)prog.Length;
            }
            limit = Math.Min(limit, (uint)prog.Length);
            while (addr + 1 < limit)
            {
                uint a = U.u16(prog,addr);
                switch (a & 0xF800)
                {//上位5ビットで命令分岐
                    case (0x0 << 11): //LSL Format1
                    case (0x1 << 11): //LSR Format1
                    case (0x2 << 11): //ASR Format1
                    case (0x3 << 11): //Format2
                    case (0x4 << 11): //MOV Format3
                    case (0x5 << 11): //CMP Format3
                    case (0x6 << 11): //ADD Format3
                    case (0x7 << 11): //SUB Format3
                    case (0x8 << 11): //Format4 or Format5
                        addr += 2;
                        break;
                    case (0x9 << 11): //LDR Format6
                        {
                            uint pointer = ParseLDRPointer(addr, a);
                            pointer = U.toOffset(pointer);
                            pointer = U.Padding4(pointer);
                            if (pointer < limit)
                            {
                                uint data = U.u32(prog,pointer);
                                if (isPointerOnly == false
                                    || (U.isPointer(data) && U.toOffset(data) < prog.Length) )
                                {
                                    LDRPointer p = new LDRPointer();
                                    p.ldr_address = addr;
                                    p.ldr_data_address = pointer;
                                    p.ldr_data = data;
                                    ret.Add(p);
                                }
                            }
                        }
                        addr += 2;
                        break;
                    case (0xA << 11): //STR Format7 or Format8
                    case (0xB << 11): //LDR Format7 or Format8
                    case (0xC << 11): //Format9
                    case (0xD << 11): //Format9
                    case (0xE << 11): //Format9
                    case (0xF << 11): //Format9
                    case (0x10 << 11): //Format10
                    case (0x11 << 11): //Format10
                    case (0x12 << 11): //Format11
                    case (0x13 << 11): //Format11
                    case (0x14 << 11): //Format12
                    case (0x15 << 11): //Format12
                    case (0x16 << 11): //Format13 or Format14
                    case (0x17 << 11): //Format14
                    case (0x18 << 11): //Format15
                    case (0x19 << 11): //Format15
                    case (0x1A << 11): //Format16
                    case (0x1B << 11): //Format16 or Format17
                    case (0x1C << 11): //Format18
                        addr += 2;
                        break;
                    case (0x1D << 11): //Format19
                    case (0x1E << 11): //Format19
                        addr += 4;
                        break;
                    default:
                        addr += 2;
                        break;
                }
            }

            return ret;
        }


        public static uint LDR_IMM(uint a)
        {
            uint imm = (a & 0xff) << 2;
            return imm;
        }

        //BLで指されるデータのマップを作成.
        public class BLPointer
        {
            public uint bl_address; //LDRが書いてあったプログラムのアドレス
            public uint bl_function; //呼ばれる関数
        };
        public static List<BLPointer> MakeBLMap(byte[] prog, uint start, uint limit = 0, bool isOutRangeOnly = true)
        {
            List<BLPointer> ret = new List<BLPointer>();

            uint addr = U.Padding2(start);
            if (limit == 0)
            {
                if (prog.Length < 4)
                {//短すぎてリストを作れない.
                    return ret;
                }
                limit = (uint)prog.Length;
            }
            limit = Math.Min(limit, (uint)prog.Length);
            limit = U.Padding2(limit);
            while (addr + 1 < limit)
            {
                uint a = U.u16(prog, addr);
                switch (a & 0xF800)
                {//上位5ビットで命令分岐
                    case (0x0 << 11): //LSL Format1
                    case (0x1 << 11): //LSR Format1
                    case (0x2 << 11): //ASR Format1
                    case (0x3 << 11): //Format2
                    case (0x4 << 11): //MOV Format3
                    case (0x5 << 11): //CMP Format3
                    case (0x6 << 11): //ADD Format3
                    case (0x7 << 11): //SUB Format3
                    case (0x8 << 11): //Format4 or Format5
                    case (0x9 << 11): //LDR Format6
                    case (0xA << 11): //STR Format7 or Format8
                    case (0xB << 11): //LDR Format7 or Format8
                    case (0xC << 11): //Format9
                    case (0xD << 11): //Format9
                    case (0xE << 11): //Format9
                    case (0xF << 11): //Format9
                    case (0x10 << 11): //Format10
                    case (0x11 << 11): //Format10
                    case (0x12 << 11): //Format11
                    case (0x13 << 11): //Format11
                    case (0x14 << 11): //Format12
                    case (0x15 << 11): //Format12
                    case (0x16 << 11): //Format13 or Format14
                    case (0x17 << 11): //Format14
                    case (0x18 << 11): //Format15
                    case (0x19 << 11): //Format15
                    case (0x1A << 11): //Format16
                    case (0x1B << 11): //Format16 or Format17
                    case (0x1C << 11): //Format18
                        addr += 2;
                        break;
                    case (0x1D << 11): //Format19
                    case (0x1E << 11): //Format19
                        uint data = ParseCallIMM(prog, addr, a);
                        addr += 4;

                        if (! U.isSafetyPointer(data))
                        {
                            break;
                        }
                        if (isOutRangeOnly)
                        {
                            if (data >= start && data < limit)
                            {
                                break;
                            }
                        }

                        BLPointer bl = new BLPointer();
                        bl.bl_address = addr - 4;
                        bl.bl_function = data;
                        ret.Add(bl);

                        break;
                    default:
                        addr += 2;
                        break;
                }
            }

            return ret;
        }

        //プロセスにルーチンを追加するために割り込むフックを生成する.
        //壊すサイズは 6バイトから8バイトです. inject_addr%4==0の場合 6バイト  そうでなければ 8バイトです.
        public static byte[] MakeInjectJump(uint inject_addr,uint add_routine_addr,uint usereg)
        {
            List<byte> asm = new List<byte>();
            if(inject_addr % 4 != 0)
            {//4バイトアライメントをみたせない場合 NOPを追加.
                asm.Add(0x0); //NOP
                asm.Add(0x0);
                inject_addr += 2;
            }

            //アドレスを使うので4バイト
            Debug.Assert(inject_addr % 4 == 0);

            //ldr usereg,imm
            //mov pc,usereg
            uint ldr = (0x9 << 11); //LDR Format6
            ldr |= (usereg & 0x7) << 8;
            //ldr |= 0; //immは、常に 0
            U.append_u16(asm,ldr);

            //mov pc,usereg
            uint mov = 0x4000; //Mov Format5
            mov |= 0x0687;     //mov pc,
            mov |= (usereg & 0x7) << 3;
            U.append_u16(asm, mov);

            //飛ばすルーチン
            U.append_u32(asm, U.toPointer(add_routine_addr));

            return asm.ToArray();
        }
#if DEBUG
        public static void TEST_MakeInjectJump()
        {
            {
                byte[] r = MakeInjectJump(0xbb180, 0xe4fd50, 4);
                //00 4C A7 46 50 FD E4 08
                Debug.Assert(r[0] == 0x00);
                Debug.Assert(r[1] == 0x4C);
                Debug.Assert(r[2] == 0xA7);
                Debug.Assert(r[3] == 0x46);
                Debug.Assert(r[4] == 0x50);
                Debug.Assert(r[5] == 0xFD);
                Debug.Assert(r[6] == 0xE4);
                Debug.Assert(r[7] == 0x08);
            }
            {
                byte[] r = MakeInjectJump(0xbb182, 0xe4fd50, 4);
                //00 00 00 4C A7 46 50 FD E4 08
                Debug.Assert(r[0] == 0x00);
                Debug.Assert(r[1] == 0x00);
                Debug.Assert(r[2] == 0x00);
                Debug.Assert(r[3] == 0x4C);
                Debug.Assert(r[4] == 0xA7);
                Debug.Assert(r[5] == 0x46);
                Debug.Assert(r[6] == 0x50);
                Debug.Assert(r[7] == 0xFD);
                Debug.Assert(r[8] == 0xE4);
                Debug.Assert(r[9] == 0x08);
            }
            {
                byte[] r = MakeInjectJump(0xbb180, 0xe4fd50, 0);
                //00 48 87 46 50 FD E4 08
                Debug.Assert(r[0] == 0x00);
                Debug.Assert(r[1] == 0x48);
                Debug.Assert(r[2] == 0x87);
                Debug.Assert(r[3] == 0x46);
                Debug.Assert(r[4] == 0x50);
                Debug.Assert(r[5] == 0xFD);
                Debug.Assert(r[6] == 0xE4);
                Debug.Assert(r[7] == 0x08);
            }
            {
                byte[] r = MakeInjectJump(0xbb184, 0xe4fd50, 0);
                //00 48 87 46 50 FD E4 08
                Debug.Assert(r[0] == 0x00);
                Debug.Assert(r[1] == 0x48);
                Debug.Assert(r[2] == 0x87);
                Debug.Assert(r[3] == 0x46);
                Debug.Assert(r[4] == 0x50);
                Debug.Assert(r[5] == 0xFD);
                Debug.Assert(r[6] == 0xE4);
                Debug.Assert(r[7] == 0x08);
            }
        }
#endif
        //B Jumpを生成します 2バイト
        public static byte[] MakeBJump(uint inject_addr, uint add_routine_addr)
        {
            byte[] asm = new byte[2];
            int im = (((int)add_routine_addr) - ((int)(inject_addr + 4))) / 2;
            uint imm;
            if (im < 0)
            {
                imm = (uint)im & 0x7FF; //マイナスビット分1ビット増えます.
            }
            else
            {
                imm = (uint)im & 0x3FF;
            }
            U.write_u16(asm, 0, (uint)imm | (0x1C << 11) );
            return asm;
        }
        public static uint ProgramAddrToPlain(uint addr)
        {
            return U.Padding2Before(addr);
        }
#if DEBUG
        public static void TEST_MakeBJumpMinus()
        {
            byte[] a = MakeBJump(0x004a0c, 0x49dc);
            Debug.Assert(a[0] == 0xE6);
            Debug.Assert(a[1] == 0xE7);
        }
        public static void TEST_MakeBJump()
        {
            byte[] a = MakeBJump(0x002694, 0x26a4);
            Debug.Assert(a[0] == 0x06);
            Debug.Assert(a[1] == 0xE0);
        }
#endif
        //BL Jumpを生成します 4バイト
        public static byte[] MakeBLJump(uint inject_addr, uint add_routine_addr)
        {
            byte[] asm = new byte[4];

            int a = ((int)add_routine_addr) - ((int)inject_addr + 4);
            int im =  (a >> 12);
            int im2 = (a >> 1 );

            if (a < 0)
            {
                im = im & 0x7FF; //マイナスビット分1ビット増えます.
            }
            else
            {
                im = im & 0x3FF;
            }

            int b = (im         )  | (0x1E << 11);
            int c = (im2 & 0xFFF) | (0x1F << 11);
            U.write_u16(asm, 0, (uint)(b) );
            U.write_u16(asm, 2, (uint)(c));

            return asm;
        }

#if DEBUG
        public static void TEST_MakeBLJumpMinus()
        {
            byte[] a = MakeBLJump(0x4a2c, 0x49d4);
            Debug.Assert(a[0] == 0xff);
            Debug.Assert(a[1] == 0xf7);
            Debug.Assert(a[2] == 0xd2);
            Debug.Assert(a[3] == 0xff);
        }
        public static void TEST_MakeBLJump()
        {
            {
                byte[] a = MakeBLJump(0x4520, 0xd65c0);
                Debug.Assert(a[0] == 0xd2);
                Debug.Assert(a[1] == 0xf0);
                Debug.Assert(a[2] == 0x4e);
                Debug.Assert(a[3] == 0xf8);
            }
            {
                byte[] a = MakeBLJump(0x452C, 0xd636c);
                Debug.Assert(a[0] == 0xd1);
                Debug.Assert(a[1] == 0xf0);
                Debug.Assert(a[2] == 0x1e);
                Debug.Assert(a[3] == 0xff);
            }
        }
#endif

        //ジャンプコードのとび先を取得.
        public static uint JumpCodeToAddr(byte[] bin, uint pos)
        {
            DisassemblerTrumb trumb = new DisassemblerTrumb();
            VM vm = new VM();
            Code c = trumb.Disassembler(bin, pos, (uint)bin.Length, vm);
            if (c.Type == CodeType.CONDJMP || c.Type == CodeType.JMP)
            {
                return c.Data;
            }
            return U.NOT_FOUND;
        }

        //コードかどうかの判定
        public static bool IsCode(uint pos)
        {
            return IsCode(Program.ROM.Data, pos, Program.ROM);
        }
        public static bool IsCode(byte[] prog, uint pos)
        {
            return IsCode(prog ,  pos, Program.ROM);
        }
        public static void TEST_IsCode_FE8J()
        {
            if (Program.ROM.RomInfo.version() == 8 && Program.ROM.RomInfo.is_multibyte() )
            {
                Debug.Assert( true == IsCode(0x194BC));
            }
        }

        
        public static bool IsCode(byte[] prog , uint pos, ROM currentROM)
        {
            //先頭にpushがあるか?
            {
                uint a0 = U.u8(prog,pos);
                uint a1 = U.u8(prog,pos+1);
                if (a1 == 0xB5 && ((a0 & 0xf) == 0 ) )
                {
                    return true;
                }
            }

            uint limit = (uint)prog.Length;

            List<uint> ldrdata = new List<uint>();
            uint length = CalcLength(prog, pos, limit, ldrdata);

            uint correctLDR = 0;
            for(int i = 0 ; i < ldrdata.Count ; i ++)
            {
                uint v = ldrdata[i];
                if (!U.isSafetyPointer(v, currentROM))
                {
                    continue;
                }

                uint p = U.u32(prog, v);
                if ( ! U.isSafetyPointer(p, currentROM))
                {
                    continue;
                }
                correctLDR++;
            }

            return (correctLDR >= 1) ;
        }
        public static bool IsCallBX(uint a)
        {
            if ((a & 0xF800) != (0x8 << 11))
            {
                return false;
            }
            //Format4 or Format5
            if ((a & (0x1 << 10)) <= 0)
            {//Format4なので対象外
                return false;
            }

            uint b = (((a >> 8) & 0x3));
            if (b == 0x3)
            {//bx r3 などの bx call
                return true;
            }
            //それ以外のコード
            return false;
        }
    }
}
