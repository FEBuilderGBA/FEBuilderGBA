using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FEBuilderGBA
{
    public class RecycleAddress
    {
        List<Address> Recycle;
        public RecycleAddress(List<Address> list)
        {
            this.Recycle = list;
            this.RecycleOptimize();
        }

        public bool AlreadyRecycled(uint addr)
        {
            //既に登録されている場合は登録しない.
            for (int i = 0; i < this.Recycle.Count; i++)
            {
                if (this.Recycle[i].Addr == addr)
                {//すでにある.
                    return true;
                }
            }
            return false;
        }

        //別の領域で使われているので再利用してはいけない領域を消す.
        public void SubRecycle(List<Address> rlist)
        {
            for (int i = 0; i < rlist.Count; i++)
            {
                SubRecycle(rlist[i].Addr, rlist[i].Length);
            }
        }
        public bool SubRecycle(uint addr, int length)
        {
            return SubRecycle(addr, (uint)length);
        }
        public bool SubRecycle(uint addr, uint length)
        {
            //既に登録されている場合は登録しない.
            for (int i = 0; i < this.Recycle.Count; i++)
            {
                if (this.Recycle[i].Addr >= addr
                    && this.Recycle[i].Addr < addr + length)
                {//登録されているので解除する.
                    this.Recycle.RemoveAt(i);
                    return true;
                }
                if (this.Recycle[i].Addr + this.Recycle[i].Length >= addr
                    && this.Recycle[i].Addr + this.Recycle[i].Length < addr + length)
                {//登録されているので解除する.
                    this.Recycle.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }


        public void RecycleOptimize()
        {
            if (this.Recycle.Count <= 1)
            {
                return;
            }

            //まずアドレス順に昇順に並べる.
            this.Recycle.Sort((a, b) => { return (int)(a.Addr - b.Addr); });

            //重複する部分アドレスが含まれている場合除外する
            for (int i = 0; i < this.Recycle.Count - 1; )
            {
                Address p = this.Recycle[i];
                Address p2 = this.Recycle[i + 1];

                if (p.Addr == p2.Addr && p.Length == p2.Length)
                {//完全に一致
                    this.Recycle.RemoveAt(i + 1);
                    continue;
                }

                uint p_end = p.Addr + p.Length;
                uint p_end2 =  p2.Addr + p2.Length;
                if (p_end > p2.Addr && p_end2 <= p_end)
                {//重複している
                    //p |---------------------|
                    //p2      |------------|
                    Log.Notify("重複 ", i.ToString(), "P:", U.ToHexString(p.Addr), " l:", U.ToHexString(p.Length), " P2:", U.ToHexString(p2.Addr), " l:", U.ToHexString(p.Length));
                    this.Recycle.RemoveAt(i + 1);
                    continue;
                }
                if (p_end > p2.Addr && p_end2 > p_end)
                {//重複している
                    //p |---------------------|
                    //p2                   |------------|
                    Log.Error("重複だが結合できる ", i.ToString(), "P:", U.ToHexString(p.Addr), " l:", U.ToHexString(p.Length), " P2:", U.ToHexString(p2.Addr), " l:", U.ToHexString(p.Length));

                    Debug.Assert((p_end - p2.Addr) >= 0);
                    Debug.Assert(p2.Length >= (p_end - p2.Addr));

                    uint length = p2.Length - (p_end - p2.Addr);
                    p.ResizeAddress(p.Addr, length);

                    this.Recycle.RemoveAt(i + 1);
                    continue;
                }
                i++;
            }

            //探索しやすいように、昇順に並べる.
            this.Recycle.Sort((a, b) => { return (int)(a.Length - b.Length); });
        }

        public uint WriteAndWritePointer(uint write_pointer, byte[] write_data, Undo.UndoData undodata)
        {
            uint use_addr = Write(write_data, undodata);
            if (use_addr == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }
            //ポインタ先に書き込んで領域を入れる
            Program.ROM.write_p32(write_pointer, use_addr, undodata);

            return use_addr;
        }
        public uint Write(byte[] write_data, Undo.UndoData undodata)
        {
            for (int i = 0; i < this.Recycle.Count; i++)
            {
                Address p = this.Recycle[i];
                if (p.Length >= write_data.Length)
                {
                    uint use_addr = p.Addr;

                    //ちょうど良い領域があったので利用しよう
                    Program.ROM.write_range(use_addr, write_data, undodata);
                    uint addr = U.Padding4(p.Addr + (uint)write_data.Length);
                    uint length = U.Sub(p.Length, (addr - use_addr));

                    p.ResizeAddress(addr, length);
                    if (p.Length < 4)
                    {//もう空きがない.
                        this.Recycle.RemoveAt(i);
                    }

                    return use_addr;
                }
            }

            if (this.Recycle.Count <= 0)
            {
                //空き領域から利用.
                return InputFormRef.AppendBinaryData(write_data, undodata);
            }
            else
            {
                Address lastP = this.Recycle[this.Recycle.Count - 1];
                if (lastP.Addr + lastP.Length >= Program.ROM.Data.Length)
                {//自分が最後のデータだった場合
                    //ROMサイズを増設.
                    Program.ROM.write_resize_data(U.Padding4(lastP.Addr + (uint)write_data.Length));

                    Program.ROM.write_range(lastP.Addr, write_data, undodata);
                    return lastP.Addr;
                }

                //空き領域から利用.
                return InputFormRef.AppendBinaryData(write_data, undodata);
            }
        }


        //もし、リサイクルできない端数が残ったら、それらは0x00で総クリアする
        public void BlackOut(Undo.UndoData undodata)
        {
            for (int i = 0; i < this.Recycle.Count; i++)
            {
                Address p = this.Recycle[i];
                Program.ROM.write_fill(p.Addr, p.Length, 0x00, undodata);
            }
            this.Recycle.Clear();
        }
    }
}
