﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FEBuilderGBA
{
    public class RecycleAddress
    {
        List<Address> Recycle;
        public RecycleAddress()
        {
            this.Recycle = new List<Address>();
        }
        public RecycleAddress(List<Address> list)
        {
            this.Recycle = list;
            this.RecycleOptimize();
        }

        public bool AlreadyRecycled(uint addr)
        {
            //既に登録されている場合は登録しない.
            foreach (Address a in Recycle)
            {
                if (a.Addr == addr)
                {//すでにある.
                    return true;
                }
            }
            return false;
        }

        //別の領域で使われているので再利用してはいけない領域を消す.
        public void SubRecycle(List<Address> rlist)
        {
            foreach (Address a in rlist)
            {
                SubRecycle(a.Addr, a.Length);
            }
        }
        public bool SubRecycle(uint addr, int length)
        {
            return SubRecycle(addr, (uint)length);
        }
        public bool SubRecycle(uint addr, uint length)
        {
            bool ret = false;
            //既に登録されている場合は削除する
            for (int i = 0; i < this.Recycle.Count; )
            {
                Address a = this.Recycle[i];
                if (a.Addr >= addr
                    && a.Addr < addr + length)
                {//登録されているので解除する.
                    this.Recycle.RemoveAt(i);
                    ret = true;
                    continue;
                }
                if (a.Addr + a.Length > addr
                    && a.Addr + a.Length < addr + length)
                {//登録されているので解除する.
                    this.Recycle.RemoveAt(i);
                    ret = true;
                    continue;
                }
                i++;
            }
            return ret;
        }


        //追加で領域の指定
        public void AddRecycle(List<Address> rlist)
        {
            foreach(Address a in rlist)
            {
                AddRecycle(a);
            }
        }
        void AddRecycle(Address a)
        {
            //既に登録されている場合は無視する
            for (int i = 0; i < this.Recycle.Count; i++)
            {
                Address b = this.Recycle[i];
                if (b.Addr >= a.Addr
                    && b.Addr < a.Addr + a.Length)
                {//登録されているので無視する.
                    return;
                }
                if (b.Addr + b.Length > a.Addr
                    && b.Addr + b.Length < a.Addr + a.Length)
                {//登録されているので無視する.
                    return;
                }
            }
            this.Recycle.Add(a);
        }

        bool RecycleOptimize_List()
        {
            //まずアドレス順に昇順に並べる.
            this.Recycle.Sort((a, b) => { return (int)(((int)a.Addr) - ((int)b.Addr)); });

            bool conflict = false;

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
                uint p_end2 = p2.Addr + p2.Length;
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
                    Log.Error(R._("重複だが結合できる "), i.ToString(), "P:", U.ToHexString(p.Addr), " l:", U.ToHexString(p.Length), " P2:", U.ToHexString(p2.Addr), " l:", U.ToHexString(p.Length));

                    Debug.Assert((p_end - p2.Addr) >= 0);
                    Debug.Assert(p2.Length >= (p_end - p2.Addr));

                    uint length = p2.Length - (p_end - p2.Addr);
                    p.ResizeAddress(p.Addr, length);

                    this.Recycle.RemoveAt(i + 1);

                    //結合したリストの妥当性のテストのため再度ループを回す必要がある
                    conflict = true;
                    continue;
                }
                i++;
            }
            return conflict;
        }

        public void RecycleOptimize()
        {
            if (this.Recycle.Count <= 1)
            {
                return;
            }

            //矛盾点が無くなるまで、最適化ループを回します。
            //念のため1000回で諦めます
            for (int i = 0; i < 1000; i++)
            {
                bool conflict = RecycleOptimize_List();
                if (conflict == false)
                {
                    break;
                }
            }

            //探索しやすいように、サイズで昇順に並べる.
            this.Recycle.Sort((a, b) => { return (int)(((int)a.Length) - ((int)b.Length)); });
        }

        public uint WritePointerOnly(uint write_pointer, uint content_addr, Undo.UndoData undodata)
        {
            Program.ROM.write_p32(write_pointer, content_addr, undodata);
            return content_addr;
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
                    uint left_size = p.Length;
                    if (!U.isPadding4(use_addr))
                    {//padding4ではないと端数が出てしまうので補正する
                        if (left_size < 4)
                        {//空きがなさすぎるため利用しない
                            Log.Notify("アドレスが端数値なので補正しようとしましたが、サイズが4未満なので利用しません", U.To0xHexString(p.Addr));
                            continue;
                        }
                        uint diff = 4 - (use_addr % 4);
                        use_addr += diff;
                        left_size -= diff;
                        Debug.Assert(U.isPadding4(use_addr));
                        if (left_size < write_data.Length)
                        {//align 4補正したらサイズが足りん!
                            Log.Notify("アドレスが端数値なので補正しようとしたらサイズ不足になりました", U.To0xHexString(p.Addr));
                            continue;
                        }
                        Log.Notify("アドレスが端数値なので補正します。", U.To0xHexString(p.Addr));
                    }

                    //ちょうど良い領域があったので利用しよう
                    Program.ROM.write_range(use_addr, write_data, undodata);
                    uint next_addr = U.Padding4(use_addr + (uint)write_data.Length);
                    left_size = U.Sub(left_size, (next_addr - use_addr));

                    p.ResizeAddress(next_addr, left_size);
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
                int lasiI = this.Recycle.Count - 1;
                Address lastP = this.Recycle[lasiI];
                if (lastP.Addr + lastP.Length >= Program.ROM.Data.Length)
                {//自分が最後のデータだった場合
                    //ROMサイズを増設.
                    Program.ROM.write_resize_data(U.Padding4(lastP.Addr + (uint)write_data.Length));
                    Program.ROM.write_range(lastP.Addr, write_data, undodata);

                    this.Recycle.RemoveAt(lasiI);
                    return lastP.Addr;
                }

                //空き領域から利用.
                return InputFormRef.AppendBinaryData(write_data, undodata);
            }
        }


        //もし、リサイクルできない端数が残ったら、それらは0x00で総クリアする
        public void BlackOut(Undo.UndoData undodata)
        {
            foreach(Address p in Recycle)
            {
                Program.ROM.write_fill(p.Addr, p.Length, 0x00, undodata);
            }
            this.Recycle.Clear();
        }
    }
}
