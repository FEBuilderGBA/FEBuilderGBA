using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FEBuilderGBA
{
    //FE文字列データのように
    //ポインタとデータ領域がきれいに分かれて、線形に記録されている場合、空きがあればそこにデータシフトして、空きを確保する.
    public class MoveToUnuseSpace
    {
        public struct ADDR_AND_LENGTH
        {
    	    public uint addr;
    	    public uint length;
        };

        public static uint CheckMoveToUnuseSpace(
		     uint index_start_addr //indexの開始
		    ,uint index_end_addr   //indexの終端
		    ,uint data_start_addr  //データの開始
		    ,uint data_end_addr    //データの終端
		    ,uint now_index        //データを入れたいindex場所
		    ,Func<uint,bool,MoveToUnuseSpace.ADDR_AND_LENGTH> get_data_pos_callback //データサイズを求める.
        ){
            uint addr = index_start_addr + (now_index * 4);
            if (addr < index_start_addr)
		    {
			    return 0;
		    }
            if (addr >= index_end_addr)
		    {
			    return 0;
		    }
		    uint r = 0;

		    //上方向へ探索
		    r = CheckMoveToUnuseSpaceTop(
			     index_start_addr //indexの開始
                ,addr        //indexの終端
			    ,data_start_addr  //データの開始
			    ,data_end_addr    //データの終端
			    ,get_data_pos_callback //データサイズを求める.
		    );
		    if(U.NOT_FOUND == r)
		    {
			    return U.NOT_FOUND;
		    }
		    uint free_space = r;
		
		    //下方向へ探索
		    r = CheckMoveToUnuseSpaceBottom(
                 addr + 4      //indexの開始
			    ,index_end_addr   //indexの終端
			    ,data_start_addr  //データの開始
			    ,data_end_addr    //データの終端
			    ,get_data_pos_callback //データサイズを求める.
		    );
		    if(U.NOT_FOUND == r)
		    {
			    return U.NOT_FOUND;
		    }
		    free_space = free_space + r;

            return free_space;
	    }

        public static uint RunMoveToUnuseSpace(
		     uint index_start_addr //indexの開始
		    ,uint index_end_addr   //indexの終端
		    ,uint data_start_addr  //データの開始
		    ,uint data_end_addr    //データの終端
		    ,uint now_index        //データを入れたいindex場所
		    ,Func<uint,bool,MoveToUnuseSpace.ADDR_AND_LENGTH> get_data_pos_callback //データサイズを求める.
            , List<Undo.UndoPostion> undolist
        )
        {
            uint addr = index_start_addr + (now_index * 4);
            if (addr < index_start_addr)
		    {
			    return U.NOT_FOUND;
		    }
            if (addr >= index_end_addr)
		    {
                return U.NOT_FOUND;
            }

		    bool r;
		
		    //上方向へ探索
		    r = RunMoveToUnuseSpaceTop(
			     index_start_addr //indexの開始
                ,addr + 4        //indexの終端
			    ,data_start_addr  //データの開始
			    ,data_end_addr    //データの終端
			    ,get_data_pos_callback //データサイズを求める.
                ,undolist
		    );
		    if(!r)
		    {
                return U.NOT_FOUND;
            }

		    //下方向へ探索
		    r = RunMoveToUnuseSpaceBottom(
                 addr + 4      //indexの開始
			    ,index_end_addr   //indexの終端
			    ,data_start_addr  //データの開始
			    ,data_end_addr    //データの終端
			    ,get_data_pos_callback //データサイズを求める.
                , undolist
            );
		    if(!r)
		    {
                return U.NOT_FOUND;
            }

            return addr;
        }

        static uint CheckMoveToUnuseSpaceTop(
		     uint index_start_addr //indexの開始
		    ,uint index_end_addr   //indexの終端
		    ,uint data_start_addr  //データの開始
		    ,uint data_end_addr    //データの終端
		    ,Func<uint,bool,MoveToUnuseSpace.ADDR_AND_LENGTH> get_data_pos_callback //データサイズを求める.
        ){
		    uint free_size = 0;
		    for(uint addr = index_start_addr ; addr < index_end_addr ; addr += 4)
		    {
                //uHuffman patchを使っているかどうか.
                bool useUnHuffmanPatch = false;

                uint data_s = Program.ROM.u32(addr);
                if (!U.isPointer(data_s))
                {//ポインタではない
                    if (!FETextEncode.IsUnHuffmanPatchPointer(data_s))
                    {//不明
                        continue;
                    }
                    //unHuffman patch適応データ
                    useUnHuffmanPatch = true;
                    data_s = FETextEncode.ConvertUnHuffmanPatchToPointer(data_s);
                }
                data_s = U.toOffset(data_s);

                ADDR_AND_LENGTH aal = get_data_pos_callback(data_s, useUnHuffmanPatch);
                if (aal.addr < data_start_addr || aal.addr + aal.length > data_end_addr)
			    {//独自拡張され、データ領域以外に設置されている.
                    continue;
			    }
                if (aal.length >= 0x00200000)
                {//長すぎる.
                    continue;
                }
                uint aaladdr = aal.addr + aal.length;

                //次のデータのポインタを取得
                uint next_addr = Program.ROM.u32(addr+4);
                if (FETextEncode.IsUnHuffmanPatchPointer(next_addr))
                {
                    next_addr = FETextEncode.ConvertUnHuffmanPatchToPointer(next_addr);
                }
                next_addr = U.toOffset(next_addr);
                if (next_addr == aaladdr)
                {//ちょうど利用している.空き領域なし.次に行ってみよう.
				    continue;
			    }
                if (next_addr < aaladdr)
                {//アドレスが逆転している...放置
                    continue;
                }

			    //フリーサイズ
                free_size += (next_addr - aaladdr);
		    }
		    return free_size;
	    }
        static uint CheckMoveToUnuseSpaceBottom(
		     uint index_start_addr //indexの開始
		    ,uint index_end_addr   //indexの終端
		    ,uint data_start_addr  //データの開始
		    ,uint data_end_addr    //データの終端
		    ,Func<uint,bool,MoveToUnuseSpace.ADDR_AND_LENGTH> get_data_pos_callback //データサイズを求める.
        )
        {
            uint free_size = 0;
		    for(uint addr = index_end_addr - 8; addr > index_start_addr ; addr -= 4)
		    {
                //uHuffman patchを使っているかどうか.
                bool useUnHuffmanPatch = false;

                uint data_s = Program.ROM.u32(addr);
                if (!U.isPointer(data_s))
                {//ポインタではないので不明
                    if (!FETextEncode.IsUnHuffmanPatchPointer(data_s))
                    {//不明
                        continue;
                    }
                    //unHuffman patch適応データ
                    useUnHuffmanPatch = true;
                    data_s = FETextEncode.ConvertUnHuffmanPatchToPointer(data_s);
                }
                data_s = U.toOffset(data_s);

                ADDR_AND_LENGTH aal = get_data_pos_callback(data_s, useUnHuffmanPatch);
                if (aal.addr < data_start_addr || aal.addr + aal.length > data_end_addr)
			    {//独自拡張され、データ領域以外に設置されている.
				    continue;
			    }

                uint aaladdr = aal.addr + aal.length;

                //次のデータのポインタを取得
                uint next_addr = Program.ROM.u32(addr + 4);
                if (FETextEncode.IsUnHuffmanPatchPointer(next_addr))
                {
                    next_addr = FETextEncode.ConvertUnHuffmanPatchToPointer(next_addr);
                }
                next_addr = U.toOffset(next_addr);
                if (next_addr == aaladdr)
                {//ちょうど利用している.空き領域なし.次に行ってみよう.
                    continue;
                }
                if (next_addr < aaladdr)
                {//アドレスが逆転している...
                    continue;
                }
			
			    //フリーサイズ
                free_size += (next_addr - (aal.addr + aal.length));
		    }
		    return free_size;
	    }


        static bool RunMoveToUnuseSpaceTop(
             uint index_start_addr //indexの開始
            , uint index_end_addr   //indexの終端
            , uint data_start_addr  //データの開始
            , uint data_end_addr    //データの終端
            , Func<uint,bool,MoveToUnuseSpace.ADDR_AND_LENGTH> get_data_pos_callback //データサイズを求める.
            , List<Undo.UndoPostion> undolist
        )
        {
            uint use_data_addr = data_start_addr;
            uint addr;
            for (addr = index_start_addr; addr < index_end_addr; addr += 4)
            {
                //uHuffman patchを使っているかどうか.
                bool useUnHuffmanPatch = false;

                uint data_s = Program.ROM.u32(addr);
                if (!U.isPointer(data_s))
                {//ポインタではない
                    if (!FETextEncode.IsUnHuffmanPatchPointer(data_s))
                    {//不明
                        continue;
                    }
                    //unHuffman patch適応データ
                    useUnHuffmanPatch = true;
                    data_s = FETextEncode.ConvertUnHuffmanPatchToPointer(data_s);
                }
                data_s = U.toOffset(data_s);

                ADDR_AND_LENGTH aal = get_data_pos_callback(data_s, useUnHuffmanPatch);
                if (aal.addr < data_start_addr || aal.addr + aal.length > data_end_addr)
                {//独自拡張され、データ領域以外に設置されている.
                    //無視して次行ってみよう.
                    continue;
                }

                //空き領域を作るためにデータを移動 movemem
                byte[] original = Program.ROM.getBinaryData(aal.addr, aal.length);
                undolist.Add(new Undo.UndoPostion(aal.addr, original));
                Program.ROM.write_range(use_data_addr, original);

                //移動したので挿げ替え.
                undolist.Add(new Undo.UndoPostion(addr, 4));
                if (useUnHuffmanPatch)
                {
                    Program.ROM.write_u32(addr, FETextEncode.ConvertPointerToUnHuffmanPatchPointer(U.toPointer(use_data_addr)) );
                }
                else
                {
                    Program.ROM.write_p32(addr, use_data_addr);
                }

                use_data_addr += aal.length;
            }

            return true;
        }

        static bool RunMoveToUnuseSpaceBottom(
		     uint index_start_addr //indexの開始
		    ,uint index_end_addr   //indexの終端
		    ,uint data_start_addr  //データの開始
		    ,uint data_end_addr    //データの終端
		    ,Func<uint,bool,MoveToUnuseSpace.ADDR_AND_LENGTH> get_data_pos_callback //データサイズを求める.
            , List<Undo.UndoPostion> undolist
        )
        {
		    uint use_data_addr = data_end_addr;
		    for(uint addr = index_end_addr - 8; addr > index_start_addr ; addr -= 4)
		    {
                //uHuffman patchを使っているかどうか.
                bool useUnHuffmanPatch = false;

                uint data_s = Program.ROM.u32(addr);
                if (!U.isPointer(data_s))
                {//ポインタではない
                    if (!FETextEncode.IsUnHuffmanPatchPointer(data_s))
                    {//不明
                        continue;
                    }
                    //unHuffman patch適応データ
                    useUnHuffmanPatch = true;
                    data_s = FETextEncode.ConvertUnHuffmanPatchToPointer(data_s);
                }
                data_s = U.toOffset(data_s);

                ADDR_AND_LENGTH aal = get_data_pos_callback(data_s, useUnHuffmanPatch);
			    if(aal.addr < data_start_addr || aal.addr+aal.length > data_end_addr )
			    {//独自拡張され、データ領域以外に設置されている.
				    continue;
			    }

                //空き領域を作るためにデータを移動 movemem
                byte[] original = Program.ROM.getBinaryData(aal.addr, aal.length);
                undolist.Add(new Undo.UndoPostion(aal.addr, original));
                Program.ROM.write_range(use_data_addr - aal.length, original);

                //移動したので挿げ替え.
                undolist.Add(new Undo.UndoPostion(addr, 4));
                if (useUnHuffmanPatch)
                {
                    Program.ROM.write_u32(addr, FETextEncode.ConvertPointerToUnHuffmanPatchPointer(U.toPointer(use_data_addr - aal.length)));
                }
                else
                {
                    Program.ROM.write_p32(addr, use_data_addr - aal.length);
                }

                use_data_addr -= aal.length;
		    }
		    return true;
	    }


        //改造ROMだとデータを共有している場合があるので、本当にそのサイズは正しいのか、
        //すべての文字列データから再検証します。
        //遅くなるけど、これは必須です。
        static uint ConvertSafetyLength(
               uint length
             , uint index_start_addr //indexの開始
             , uint index_end_addr   //indexの終端
             , uint data_s           //入れたいデータのアドレス
            )
        {
            //念のため、その範囲からスタートするポインタがないか確認します. 
            //tlpとか、途中からアドレスが逆転していることがあるらしいので。
            for (uint p = index_start_addr; p < index_end_addr; p += 4)
            {
                uint data_s_dupcheck = Program.ROM.u32(p);
                if (!U.isPointer(data_s_dupcheck))
                {
                    if (!FETextEncode.IsUnHuffmanPatchPointer(data_s_dupcheck))
                    {//ポインタが不明
                        continue;
                    }
                    data_s_dupcheck = FETextEncode.ConvertUnHuffmanPatchToPointer(data_s_dupcheck);
                }
                data_s_dupcheck = U.toOffset(data_s_dupcheck);
                if (data_s_dupcheck == data_s)
                {//自分自身
                    continue;
                }

                if (data_s_dupcheck >= data_s && data_s_dupcheck < data_s + length)
                {//データを共有しているらしいので、サイズを切り詰めないとダメだ.
                    uint new_length = data_s_dupcheck - data_s;
                    if (new_length < length)
                    {//より小さくなる場合は、縮める.
                        length = new_length;
                    }
                }
            }

            return length;
        }

        //Text領域にASMを配置しているハックがあるらしい
        //そういうのがないかどうかチェックします。
        static uint ConvertSafetyLength2(uint length
            , uint aal_length
            , uint data_s)
        {
            if (length <= aal_length)
            {
                return length;
            }

            uint addr = data_s + aal_length;
            uint end = data_s + length;
            uint truelength = aal_length;
            for (; addr < end; addr++)
            {
                uint d = Program.ROM.u8(addr);
                if (d != 0x00)
                {
                    uint sub_padding_addr = U.SubPadding4(addr);
                    if (sub_padding_addr < data_s)
                    {
                        return truelength;
                    }
                    else
                    {
                        return sub_padding_addr - data_s;
                    }
                }
            }
            return truelength;
        }

        public static  uint OriginalDataSize(
               uint index_start_addr //indexの開始
             , uint index_end_addr   //indexの終端
             , uint data_start_addr  //データの開始
             , uint data_end_addr    //データの終端
             , uint now_index        //データを入れたいindex場所
             , Func<uint,bool,MoveToUnuseSpace.ADDR_AND_LENGTH> get_data_pos_callback //データサイズを求める.
             )
         {
            //uHuffman patchを使っているかどうか.
            bool useUnHuffmanPatch = false;

            uint addr = index_start_addr + (now_index * 4);
            uint data_s = Program.ROM.u32(addr);

            //他と共有していないか調べる.
            for (uint p = index_start_addr; p < index_end_addr; p += 4)
            {
                uint data_s2 = Program.ROM.u32(p);
                if (p == addr)
                {
                    continue;
                }
                if (data_s2 == data_s)
                {//他と共有しているのでリサイクルしてはいけません.
                    return 0;
                }
            }

            if (!U.isPointer(data_s))
            {//ポインタではない　単体データ
                if (!FETextEncode.IsUnHuffmanPatchPointer(data_s))
                {
                    MoveToUnuseSpace.ADDR_AND_LENGTH aal_untihuffman = get_data_pos_callback(data_s, false);
                    return ConvertSafetyLength(aal_untihuffman.length, index_start_addr, index_end_addr, data_s);
                }
                //unHuffman patch適応データ
                useUnHuffmanPatch = true;
                data_s = FETextEncode.ConvertUnHuffmanPatchToPointer(data_s);
            }

            data_s = U.toOffset(data_s);
            MoveToUnuseSpace.ADDR_AND_LENGTH aal = get_data_pos_callback(data_s, useUnHuffmanPatch);
            if (data_s < data_start_addr || data_s >= data_end_addr)
            {//拡張領域にあるらしいので基底サイズは不明. 自サイズしかわからない.
                return ConvertSafetyLength(aal.length, index_start_addr, index_end_addr, data_s);
            }

            uint i = 1;
            uint data_s_next;
            do
            {
                uint next_addr = index_start_addr + ((now_index+i) * 4);
                data_s_next = Program.ROM.u32(next_addr);

                if(! U.isPointer(data_s_next) )
                {
                    if (!FETextEncode.IsUnHuffmanPatchPointer(data_s_next))
                    {//次のポインタが不明なので自サイズしかわからない.
                        return ConvertSafetyLength(aal.length, index_start_addr, index_end_addr, data_s);
                    }
                    //unHuffman patch適応データ
                    useUnHuffmanPatch = true;
                    data_s_next = FETextEncode.ConvertUnHuffmanPatchToPointer(data_s_next);
                }
                data_s_next = U.toOffset(data_s_next);
                if (data_s_next < data_start_addr || data_s_next >= data_end_addr )
                {//拡張領域にあるらしいので次のデータを参照したい
                    i++;
                    continue;
                }
                if (data_s_next < data_s)
                {
                    //アドレスが逆転してます. 危険なので自分のデータの長さだけを求めます.input
                    return ConvertSafetyLength(aal.length, index_start_addr, index_end_addr, data_s);
                }
                break;
            }
            while(true);

            uint length = data_s_next - data_s;
            //共有の可能性を排除
            length = ConvertSafetyLength(length, index_start_addr, index_end_addr, data_s);
            //text領域にASMを設置している可能性を排除
            length = ConvertSafetyLength2(length, aal.length, data_s);
            return length;
         }
    }
}
