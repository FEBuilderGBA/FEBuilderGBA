using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FEBuilderGBA
{
    class FETextDecode
    {
        ROM ROM = null;
        SystemTextEncoder SystemTextEncoder = null;
        public FETextDecode()
        {
            this.ROM = Program.ROM;
            this.SystemTextEncoder = Program.SystemTextEncoder;
        }
        public FETextDecode(ROM rom,SystemTextEncoder encoder)
        {
            this.ROM = rom;
            this.SystemTextEncoder = encoder;
        }


        //ワンライナー
        public static String Direct(uint id)
        {
            FETextDecode d = new FETextDecode();
            return d.Decode(id);
        }

        public String Decode(uint id)
        {
            int datasize;
            return Decode(id, out datasize);
        }
        public String Decode(uint id, out int out_DataSize)
        {
            if (U.isSafetyPointer(id))
            {//FE7だと曲名などにアドレス直指定 SJIS文字があるらしい...
                return CString( U.toOffset(id),out out_DataSize);
            }

            if (id < 0 || id >= TextForm.GetDataCount())
            {
                out_DataSize = 0;
                return "";
            }

            uint text_base = this.ROM.p32(this.ROM.RomInfo.text_pointer());
            if (!U.isSafetyOffset(text_base))
            {//テキストポインタを壊しているので復帰する.
                text_base = this.ROM.RomInfo.text_recover_address();
            }

            uint p = text_base + (id * 4);
            if (!U.isOffset(p))
            {//エラー
                out_DataSize = 0;
                return "";
            }
            return DecodeAddr(p,out out_DataSize);
        }
        public String DecodeAddr(uint addr, out int out_DataSize)
        {
            uint paddr = this.ROM.u32(U.toOffset(addr));

            if (FETextEncode.IsUnHuffmanPatchPointer(paddr))
            {//un-huffman patch?
                uint unhuffman_addr = U.toOffset(FETextEncode.ConvertUnHuffmanPatchToPointer(paddr));
                return UnHffmanPatchDecode(unhuffman_addr, out out_DataSize);
            }
            else if (U.isPointer(paddr))
            {
                return huffman_decode(U.toOffset(paddr), out out_DataSize);
            }
            out_DataSize = 0;
            return "";
        }

        public String UnHffmanPatchDecode(uint addr, out int out_DataSize)
        {
            if (!U.isSafetyOffset(addr))
            {
                Log.Error(R._("UnHffmanPatchDecode string addr:{0} is not safety", addr.ToString("X")));
                out_DataSize = 0;
                return "";
            }


            int length = (int)this.ROM.getBlockDataCount(addr, 1, (i, p) => { return this.ROM.u8(p) != 0; });
            if (length <= 0)
            {
                out_DataSize = 0;
                return "";
            }

            if (U.isSafetyOffset(addr + (uint)length + 2, this.ROM)
                && this.ROM.u8(addr + (uint)length + 2) == 0x00)
            {//anti-huffman は nullを2つ重ねるらしい
                out_DataSize = length + 2;
            }
            else if (U.isSafetyOffset(addr + (uint)length + 1, this.ROM))
            {//現在あるnullだけを含める.
                out_DataSize = length + 1;
            }
            else
            {//壊れたデータ nullがないよ
                out_DataSize = length;
            }

            byte[] srcdata = this.ROM.getBinaryData(addr, (uint)length);
            return UnHffmanPatchDecodeLow(srcdata);
        }
        int AppendSJIS(List<byte> str,byte code,byte code2,InputFormRef.PRIORITY_CODE priorityCode)
        {
            if (priorityCode == InputFormRef.PRIORITY_CODE.LAT1)
            {//SJISと 1バイトUnicodeは範囲が重複するので、どちらかを優先しないといけない.
                if (code >= 0x82)
                {//英語版FEにはUnicodeの1バイトだけ表記があるらしい.
                    AppendAtmarkCode(str, code); //@000Fとかのコード
                    return 1;
                }
            }
            str.Add(code);
            str.Add(code2);
            return 2;
        }

        public String UnHffmanPatchDecodeLow(byte[] srcdata)
        {
            InputFormRef.PRIORITY_CODE priorityCode = InputFormRef.SearchPriorityCode();
            List<byte> str = new List<byte>();
            int length = srcdata.Length;
            int i = 0;
            while( i < length )
            {
                byte code = srcdata[i];
                if (length > i + 1)
                {
                    byte code2 = srcdata[i + 1];
                    if (priorityCode == InputFormRef.PRIORITY_CODE.UTF8 && code >= 0xC0 && code2 >= 0x80)
                    {
                        i += U.AppendUTF8(str , srcdata , i);
                        continue;
                    }
                    else if (U.isSJIS1stCode(code) && U.isSJIS2ndCode(code2))
                    {//SJISコード 2バイト読み飛ばす.
                        i += AppendSJIS(str, code, code2, priorityCode);
                        continue;
                    }

                    //unHuffman patchでは可変長デコードらしい.
                    //@0009 -> 0x09
                    //@0080@000d -> 0x80 0x0d
                    //@0010@0102 -> 0x10 0x02 0x01 0x01 または、0x10 0x02 0x01 0xFF
                    //
                    //0x10だけ別処理が必要.
                    //0x10の後、+2バイト後に 終端0x01 か 0xFFがある

                    if (code == 0x80)
                    {
                        code2 = srcdata[i + 1];
                        AppendAtmarkCode(str, code); //@0080
                        AppendAtmarkCode(str, code2); //@000d
                        i+=2;
                        continue;
                    }
                    if (code == 0x10)
                    {
                        if (length <= i + 2)
                        {//もう文字列の容量がないので、 @0010@0102 じゃないと思う. 謎のコード.
                            str.Add(code);
                            str.Add(code2);
                            i += 2;
                            continue;
                        }
                        code2 = srcdata[i + 1];
                        byte code3 = srcdata[i + 2];

                        uint code23 = ((uint)code3 << 8) | (((uint)code2) & 0xFF);

                        AppendAtmarkCode(str,code); //@0010
                        AppendAtmarkCode(str,code23); //@0102
                        i += 3;
                        continue;
                    }
                }
                if (code < 0x20 || code == 0x40)
                {
                    AppendAtmarkCode(str, code); //@000Fとかのコード
                    i += 1;
                    continue;
                }
                if (code >= 0x82 && priorityCode == InputFormRef.PRIORITY_CODE.LAT1)
                {//英語版FEにはUnicodeの1バイトだけ表記があるらしい.
                    AppendAtmarkCode(str, code); //@000Fとかのコード
                    i += 1;
                    continue;
                }

                //アルファベット等だと思われる.
                str.Add(code);
                i += 1;
            }

            return listbyte_to_string_low(str.ToArray(), str.Count);
        }
        void AppendAtmarkCode(List<byte> str,uint code)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(code.ToString("X04"));

            str.Add((byte)'@');
            for (int i = 0; i < data.Length; i++ )
            {
                str.Add(data[i]);
            }
        }
        public String huffman_decode(uint addr, out int out_DataSize)
        {
            List<byte> str = new List<byte>(65535);

            uint tree_base = this.ROM.p32(this.ROM.RomInfo.mask_pointer());
            uint tree_data_base = this.ROM.p32p(this.ROM.RomInfo.mask_point_base_pointer());
            bool useSJIS = this.ROM.RomInfo.is_multibyte();

            if (!U.isSafetyOffset(addr))
            {
                Log.Error(R._("huffman_decode string addr:{0} is not safety", addr.ToString("X")));
                out_DataSize = 0;
                return "";
            }
            if (!U.isSafetyOffset(tree_base))
            {
                throw new Exception(R._("mask_pointer broken 0x{0}.this is broken rom.", U.ToHexString(tree_base)));
            }
            if (!U.isSafetyOffset(tree_data_base))
            {
                throw new Exception(R._("tree_data_base broken 0x{0}.this is broken rom.", U.ToHexString(tree_data_base)));
            }

            uint rom_length = (uint)this.ROM.Data.Length;
            uint tree_data = tree_data_base;
		    uint beforecode = 0;
            uint addr_start = addr;
		    do
            {
                if (addr + 4 > rom_length)
                {
                    Log.Error("String Tree Broken: {0}/{1} addr_start:{2}", U.ToHexString8(addr), U.ToHexString8(rom_length), U.ToHexString8(addr_start));
                    out_DataSize = 0;
                    return "";
                }
                uint bit = this.ROM.u32(addr);
                if (bit == 0x0)
                {
                    if (addr + 4 + 4 > rom_length)
                    {
                        Log.Error("String Tree Broken2: {0}/{1} addr_start:{2}", U.ToHexString8(addr), U.ToHexString8(rom_length), U.ToHexString8(addr_start));
                        out_DataSize = 0;
                        return "";
                    }
                    if (this.ROM.u32(addr + 4) == 0x0)
                    {
                        out_DataSize = 0;
                        return "";
                    }
                }

                addr++;

			    for(int bit_index=7;bit_index>=0;bit_index--)
                {
                    uint tree_addr = tree_data + ((bit & 1) * 2);
                    if (tree_addr + 2 > rom_length)
                    {
                        Log.Error("String Tree Broken3: {0}/{1} addr_start:{2}", U.ToHexString8(tree_addr), U.ToHexString8(rom_length), U.ToHexString8(addr_start));
                        out_DataSize = 0;
                        return "";
                    }

                    uint tree_ = this.ROM.u16(tree_addr);
                    tree_data = tree_base	+	(tree_ * 4);

				    bit	>>=	1;

                    uint old = tree_data;

                    if (tree_data + 4 > rom_length)
                    {
                        Log.Error("String Data Broken: {0}/{1} addr_start:{2}", U.ToHexString8(tree_data), U.ToHexString8(rom_length), U.ToHexString8(addr_start));
                        out_DataSize = 0;
                        return "";
                    }

                    uint data = this.ROM.u32(tree_data);
				    if( (data & 0x80000000) <= 0)
                    {
					    continue;
				    }
				    tree_data	=	tree_data_base;
				
                    uint code = data & 0xFFFF;
                    if (code <= 0 || (code & 0xFF) == 0) 
				    {//終端
                        //Debug.Assert(code <= 0);
                        out_DataSize = (int)(addr - addr_start);
					    return listbyte_to_string(str.ToArray(), str.Count);
				    }

				    if (
					    useSJIS
					    || beforecode == 0x10
					    || beforecode == 0x80
					    || (code & 0xFF00) >= 0x8100
					    || (code & 0xFF00) == 0x0000
					    || (code & 0xFF00) == 0x1F00
					    )
				    {
                        append_string(str, code, beforecode);
					    beforecode =  code;
				    }
				    else
				    {
                        append_string(str, code & 0xFF, beforecode);
					    beforecode =  (code&0xFF);

                        uint nextCode = (code >> 8) & 0xFF;
                        append_string(str, nextCode, beforecode);
                        beforecode = nextCode;
				    }
			    }
		    }
            while(true);
        }

        //出現数カウント
        public class huffman_count_st
        {
            public uint count = 0;
            public List<uint> unsing_text_addr = new List<uint>();
            public int code_number = -1;
        };
        public void huffman_count(uint addr, ref Dictionary<uint, huffman_count_st> dic)
        {
            uint tree_base = this.ROM.p32(this.ROM.RomInfo.mask_pointer());
            uint tree_data_base = this.ROM.p32p(this.ROM.RomInfo.mask_point_base_pointer());
            bool useSJIS = this.ROM.RomInfo.is_multibyte();

            uint tree_data = tree_data_base;
            uint addr_start = addr;
            do
            {
                uint bit = this.ROM.u32(addr);
                if (bit == 0x0 && this.ROM.u32(addr + 4) == 0x0)
                {
                    return;
                }
                addr++;

                for (int bit_index = 7; bit_index >= 0; bit_index--)
                {
                    uint tree_ = this.ROM.u16(tree_data + ((bit & 1) * 2));
                    tree_data = tree_base + (tree_ * 4);

                    bit >>= 1;

                    uint data = this.ROM.u32(tree_data);
                    if ((data & 0x80000000) <= 0)
                    {
                        continue;
                    }
                    tree_data = tree_data_base;

                    uint code = data & 0xFFFF;
                    if (code <= 0)
                    {//終端
                        return;
                    }

                    huffman_count_st st;
                    if (! dic.TryGetValue(code,out st) )
                    {
                        st = new huffman_count_st();
                        dic[code] = st; 
                    }
                    st.count++;
                    st.unsing_text_addr.Add(addr_start);
                }
            }
            while (true);
        }


        public void append_string(List<byte> str,uint code,uint beforecode)
    	{
		    if (code == 0x00)
		    {
                return;
		    }
		    if (beforecode == 0x0010
                || beforecode == 0x0011
			    || beforecode == 0x0080
			    || code <= 0x001F 
			    || (code >= 0x0080 && code <= 0x0F00 )
			    )
		    {
                byte[] data = System.Text.Encoding.ASCII.GetBytes(code.ToString("X04"));

                str.Add((byte)'@');
                for (int i = 0; i < data.Length; i++ )
                {
                    str.Add(data[i]);
                }
                return;
		    }
            if ( (code & 0xFF00) <= 0)
            {
                str.Add((byte)code);
	            return;
            }
            Debug.Assert((byte)(code & 0xFF) != 0);
            Debug.Assert((byte)((code >> 8) & 0xFF) != 0);

            //XXYY
            str.Add((byte)(code&0xFF));
            str.Add((byte)( (code >> 8) & 0xFF));
            return;
        }

        public String listbyte_to_string(byte[] srcdata,int length)
        {
            List<byte> str = new List<byte>();

            InputFormRef.PRIORITY_CODE priorityCode = InputFormRef.SearchPriorityCode();

            length = Math.Min(srcdata.Length,length);
            int len = 0;
            while( len < length )
            {
                byte code = srcdata[len];
                if (length > len + 1)
                {
                    byte code2 = srcdata[len + 1];
                    if (priorityCode == InputFormRef.PRIORITY_CODE.UTF8 && code >= 0xC0 && code2 >= 0x80)
                    {
                        len += U.AppendUTF8(str, srcdata, len);
                        continue;
                    }
                    else if (U.isSJIS1stCode(code) && U.isSJIS2ndCode(code2))
                    {//SJISコード 2バイト読み飛ばす.
                        len += AppendSJIS(str, code, code2, priorityCode);
                        continue;
                    }
                }
                if (code >= 0x82 && priorityCode == InputFormRef.PRIORITY_CODE.LAT1)
                {//英語版FEにはUnicodeの1バイトだけ表記があるらしい.
                    AppendAtmarkCode(str, code);
                    len += 1;
                    continue;
                }

                //それ以外の文字
                str.Add(code);
                len += 1;
            }
            return listbyte_to_string_low(str.ToArray(), str.Count);
        }
        String listbyte_to_string_low(byte[] str, int len)
        {
            string r = this.SystemTextEncoder.Decode(str, 0, len);
            return ConvertSPMoji(r);
        }
        String CString(uint p, out int out_DataSize)
        {
            string str = this.ROM.getString(p, out out_DataSize);
            return ConvertSPMoji(str);
        }

        string ConvertSPMoji(string str)
        {
            return ConvertSPMoji(this.ROM, str);
        }

        public static string ConvertSPMoji(ROM rom,string str)
        {
            InputFormRef.PRIORITY_CODE priorityCode = InputFormRef.SearchPriorityCode();
            str = str.Replace("@0001", "\r\n");
            if (priorityCode == InputFormRef.PRIORITY_CODE.UTF8)
            {
                return str;
            }

            //日本語版は一部入りきらない文字を特殊フォント化しているので変換する.
            if (rom.RomInfo.version() >= 8)
            {
                str = str.Replace("нопр", "(ｻﾝﾀﾞｰｽﾄｰﾑ)"); ///No Translate
                str = str.Replace("⊂⊃┌┐", "(ﾌｫﾚｽﾄﾅｲﾄ)");  ///No Translate
                str = str.Replace("абвг", "(ﾄﾞﾗｺﾞﾝﾏｽﾀｰ)");///No Translate
                str = str.Replace("шщъы", "(ﾜｲﾊﾞｰﾝﾅｲﾄ)");///No Translate
                str = str.Replace("жзий", "(ﾌｧﾙｺﾝﾅｲﾄ)");///No Translate
                str = str.Replace("∞┴∠∧", "(ﾓｰｻﾄﾞｩｰｸﾞ)");///No Translate
                str = str.Replace("∇∈∋├", "(ﾃﾞｽｶﾞｰｺﾞｲﾙ)");///No Translate
                str = str.Replace("≠≡≦≧", "(ﾄﾞﾗｺﾞﾝｿﾞﾝﾋﾞ)");///No Translate
                str = str.Replace("∪∫∬∴", "(ｺﾞｰｺﾞﾝの卵)");///No Translate
            }
            else if (rom.RomInfo.version() >= 7)
            {//FE7
                str = str.Replace("⑮⑯⑰⑱⑲⑳α", "(ｻﾝﾀﾞｰｽﾄｰﾑ)");///No Translate
                str = str.Replace("⑧⑨⑩⑪⑫⑬⑭", "(ﾌｧﾙｺﾝﾅｲﾄ)");///No Translate
                str = str.Replace("①②③④⑤⑥⑦", "(ﾄﾞﾗｺﾞﾝﾏｽﾀｰ)");///No Translate
            }
            else if (rom.RomInfo.version() >= 6)
            {//FE6
                str = str.Replace("①②③④⑤⑥⑦", "(ｻﾝﾀﾞｰｽﾄｰﾑ)");///No Translate
                str = str.Replace("ⅠⅡⅢⅣⅤⅥⅦ", "(ﾌｧﾙｺﾝﾅｲﾄ)");///No Translate
                str = str.Replace("⑪⑫⑬⑭⑮⑯⑰", "(ﾄﾞﾗｺﾞﾝﾏｽﾀｰ)");///No Translate
            }
            //英語版FEにはUnicodeの1バイトだけ表記があるらしい.
            {
                for (int c = 0x82; c <= 0xff; c++)
                {
                    str = str.Replace("@00" + c.ToString("X02"), ((char)c).ToString());
                }
            }
            return str;
        }
    }
}
