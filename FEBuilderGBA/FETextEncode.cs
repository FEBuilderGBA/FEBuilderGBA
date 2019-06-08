using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FEBuilderGBA
{
    class FETextEncode
    {
        public int DataSize { get; private set; }
        public struct huffman_value_st{
            public uint bit;
            public byte bitcount;
            public huffman_value_st(uint bit, byte bitcount)
            {
                this.bit = bit;
                this.bitcount = bitcount;
            }
        };
        Dictionary<uint,huffman_value_st> huffman_map;
        uint tree_base;

        public FETextEncode()
        {
            RebuildHuffmanMap();
        }
        public void RebuildHuffmanMap()
        {
            this.huffman_map = new Dictionary<uint, huffman_value_st>();
            uint tree_data_base = Program.ROM.p32p(Program.ROM.RomInfo.mask_point_base_pointer());
            this.tree_base = Program.ROM.p32(Program.ROM.RomInfo.mask_pointer());
            make_huffman_map(tree_data_base, 1);

            if (! this.huffman_map.ContainsKey(0))
            {//終端文字列が辞書に存在しない.
                throw new FETextDecode.FETextException(R._("text decoder table broken 0x{0}.this is broken rom.", U.ToHexString(tree_base)));
            }

            //EOF
            huffman_value_st huffman_value = this.huffman_map[0];
            if (Program.ROM.RomInfo.is_multibyte())
            {
                if (huffman_value.bitcount < 8)
                {
                    huffman_value.bitcount = 8;
                    this.huffman_map[0] = huffman_value;
                }
            }
            else
            {
                if (huffman_value.bitcount < 7)
                {
                    huffman_value.bitcount = 7;
                    this.huffman_map[0] = huffman_value;
                }
            }
        }

		public String Encode(String str,out byte[] outByte) 
		{
            str = ConvertSPMoji(str);
            if (str == "")
            {
                outByte = new byte[0];
                return "";
            }


            uint bitcount = 0;

            byte[] sjisstr = Program.SystemTextEncoder.Encode(str); 
            int length = sjisstr.Length;

            List<byte> dest = new List<byte>(length*3);
            dest.Add(0);
            int writePoint = 0;
            
            for(int i = 0; i <= length ; )
			{
                huffman_value_st huffman_value;
                if (i == length)
                {
                    //終端コード
                    //FE6J   0x23 8ビット?
                    //FE7J   0x51 8ビット?
                    //FE8J   0x65 8ビット?
                    //FE7U   0x49 7ビット
                    //FE8U   0x5E 7ビット
                    huffman_value = this.huffman_map[0];
                    i = length + 1;
                }
                else
                {
                    uint code;
                    int nexti = string_to_code_next(sjisstr, i, out code);

                    if (!huffman_map.TryGetValue(code, out huffman_value))
                    {//辞書にない
                        outByte = null;
                        return show_sjis(sjisstr, i, nexti);
                    }
                    i = nexti;
                }

				uint shiftbit = (bitcount%8);
                dest[writePoint] |= (byte)(((huffman_value.bit & 0xFF) << (byte)shiftbit) & 0xFF);

                uint remainbit = 8 - shiftbit;
				if (remainbit < huffman_value.bitcount)
				{
					for( ; remainbit < huffman_value.bitcount ; remainbit += 8)
					{
						writePoint++;
                        dest.Add(0);
                        dest[writePoint] |= (byte)((huffman_value.bit >> (byte)remainbit) & 0xFF);
					}
				}

                bitcount += huffman_value.bitcount;

                if ( (bitcount % 8) == 0)
                {
                    writePoint++;
                    dest.Add(0);
                }
            }

			uint ret_array_size = (uint)(bitcount/8 + (bitcount%8 != 0 ? 1 : 0));
			outByte = new byte[ret_array_size];
			Array.Copy(dest.ToArray(),0,outByte,0,ret_array_size);
 
            return "";
		}

		String show_sjis(byte[] sjisstr,int nowi,int nexti)
		{
            return Program.SystemTextEncoder.Decode(sjisstr, nowi, nexti - nowi);
		}
		byte countBitMinus1(uint bit)
		{
			byte n = 31;
			for( ; n >= 0 ; n --)
			{
				if ( (bit & (1 << n)) > 0)
				{
					return n ;
				}
			}
			return 0;
		}

        int string_to_code_next(byte[] sjisstr,int nowi,out uint out_code)
		{
			if (sjisstr[nowi] == '@')
			{
                uint code = at_code_to_binary(sjisstr, nowi, out nowi);

                out_code = code;
                return nowi;
			}
            if (sjisstr[nowi] < 0x20)
            {//SPACE以下の制御コード
                out_code = (uint)sjisstr[nowi];
                return nowi + 1;
            }
			if ( sjisstr[nowi] <= 0x7e)
			{
                if (sjisstr.Length <= nowi + 1)
                {//NOP
                }
                else if (sjisstr[nowi + 1] <= 0x7e)
                {//連続して 0x7e以下の場合、連結できる可能性を検討.
                    out_code = ((uint)sjisstr[nowi]) | (((uint)sjisstr[nowi + 1]) << 8);

                    if (huffman_map.ContainsKey(out_code))
                    {//念のため辞書確認.
                        return nowi + 2;
                    }
                }

                out_code = (uint) sjisstr[nowi];
                return nowi + 1;
			}
            if (sjisstr.Length <= nowi + 1)
            {
                out_code = ((uint)sjisstr[nowi]);
                return nowi + 1;
            }
            out_code = ((uint)sjisstr[nowi]) | (((uint)sjisstr[nowi + 1]) << 8);
			return nowi + 2;
		}

        public static uint at_code_to_binary(byte[] sjisstr,int i,out int out_newI)
        {
            uint code = 0;

            int n = 1;
            for (; n < 5; n++)
            {
                if (sjisstr.Length <= i + n)
                {
                    break;
                }
                else if ((sjisstr[i + n] >= '0' && sjisstr[i + n] <= '9'))
                {
                    code = code * 16 + (uint)(sjisstr[i + n] - '0');
                }
                else if ((sjisstr[i + n] >= 'a' && sjisstr[i + n] <= 'f'))
                {
                    code = code * 16 + (uint)(sjisstr[i + n] - 'a' + 10);
                }
                else if ((sjisstr[i + n] >= 'A' && sjisstr[i + n] <= 'F'))
                {
                    code = code * 16 + (uint)(sjisstr[i + n] - 'A' + 10);
                }
                else
                {
                    break;
                }
            }
            out_newI = i + n;
            return code;
        }

        byte[] convert_unHuffman_code_to_binary(byte[] sjisstr)
        {
            InputFormRef.PRIORITY_CODE priorityCode = InputFormRef.SearchPriorityCode();

            List<byte> ret = new List<byte>();
            for (int i = 0; i < sjisstr.Length; )
            {
                if (sjisstr[i] == '@')
                {
                    uint code1 = at_code_to_binary(sjisstr, i , out i);

                    //unHuffman patchでは可変長デコードらしい.
                    //@0009 -> 0x09
                    //@0080@000d -> 0x80 0x0d
                    //@0010@0102 -> 0x10 0x02 0x01 または、0x10 0x02 0x01 0xFF(昔のルーチン?)
                    //
                    //0x10だけ別処理が必要.
                    //0x10の後、+2バイト後に 終端0xFFがあることがある.

                    if (code1 == 0x80)
                    {
                        uint code2 = 0x1C;
                        if (i < sjisstr.Length && sjisstr[i] == '@')
                        {
                            code2 = at_code_to_binary(sjisstr, i, out i);
                        }
                        else
                        {
                            //一番無害な、 0x1C 目を開ける で補正しよう
                            Log.Error("@0080の後に、@命令が続いていません。@001Cで補正します。");
                        }
                        ret.Add((byte)(code1));     //@0080
                        ret.Add((byte)(code2));    //@0000
                    }
                    else if (code1 == 0x10)
                    {
                        uint code2 = 0x0101;
                        if (i < sjisstr.Length && sjisstr[i] == '@')
                        {
                            code2 = at_code_to_binary(sjisstr, i, out i);
                            if (code2 < 0x100)
                            {//@0010 の次のコードが 0x0003 みたいに 0x100以下
                                Log.Error(R._("@0010の次のコードが 0x100以下です。危険なので、+0x100補正します。 code2:{0}", code2));
                                code2 += 0x100;
                            }
                        }
                        else
                        {
                            //一番 無害な 0x0101で補正.
                            Log.Error(R._("@0010の後に、@命令が続いていません。@0010@0101@0001で補正します。"));
                        }


                        uint code3 = 0x0;
                        if (i < sjisstr.Length && sjisstr[i] == '@')
                        {
                            code3 = at_code_to_binary(sjisstr, i, out i);
                        }
                        else
                        {//@0010@0101???
                            Log.Error(R._("@0010命令が途中で終わってしまいました。"));
                        }

                        ret.Add((byte)(code1));     //@0010

                        ret.Add((byte)(code2 & 0xFF));    //@0000       //@0102 みたいなのは、リトルエンディアン化する必要がある
                        ret.Add((byte)((code2 >> 8) & 0xFF));    //@0000

                        if (code3 > 0)
                        {
                            ret.Add((byte)(code3));     //@0010終端に謎のコード 大昔は 0xFFでも入れていたのかなあ・・・?
                        }
                    }
                    else
                    {//@0080 と @0010 以外は1バイトで格納
                        ret.Add((byte)(code1 & 0xFF));
                    }
                    continue;
                }

                if (i + 1 >= sjisstr.Length)
                {//もう余白がないので1バイト文字列
                    ret.Add(sjisstr[i]);   i++;
                }
                else
                {
                    byte code1 = sjisstr[i];
                    byte code2 = sjisstr[i + 1];
                    if (priorityCode == InputFormRef.PRIORITY_CODE.UTF8 
                        && code1 >= 0xC0 && code1 >= 0x80)
                    {
                        i += U.AppendUTF8(ret, sjisstr, i);
                    }
                    else if (priorityCode == InputFormRef.PRIORITY_CODE.SJIS
                        && U.isSJIS1stCode(code1) && U.isSJIS2ndCode(code2))
                    {
                        ret.Add(sjisstr[i]); i++;
                        ret.Add(sjisstr[i]); i++;
                    }
                    else
                    {//1バイト
                        ret.Add(sjisstr[i]); i++;
                    }
                }

            }
            //0終端
            ret.Add(0);
            ret.Add(0); //終端が2つないとダメ?らしい.

            return ret.ToArray();
        }



		void make_huffman_map(uint tree_data,uint bit_deps)
		{
            if (!U.isSafetyOffset(this.tree_base))
            {
                this.huffman_map[0] = new huffman_value_st();
                R.Error("make_huffman_map: Can not Load tree_base:{0}", U.ToHexString(this.tree_base));
                Debug.Assert(false);
                return;
            }
            if (!U.isSafetyOffset(tree_data))
            {
                this.huffman_map[0] = new huffman_value_st();
                R.Error("make_huffman_map: Can not Load tree_data:{0}", U.ToHexString(tree_data));
                Debug.Assert(false);
                return;
            }

            for (uint i = 0; i < 2; i++)
			{
		        uint tree_ = (uint) Program.ROM.u16(tree_data + (i*2) );
		        uint tree_next_data = this.tree_base	+	(tree_ * 4);
                uint bit = (bit_deps << 1) + i;

	            uint src_data = Program.ROM.u32(tree_next_data);
			    if( (src_data & 0x80000000) > 0)
	            {
	                uint code = src_data & 0xFFFF;
					byte  bitcount = (byte) countBitMinus1(bit);

					huffman_value_st huffman_value = new huffman_value_st();

                    huffman_value.bit = revBit(bit,(int)bitcount);
					huffman_value.bitcount = bitcount;
					this.huffman_map[code] = huffman_value;
			    }
			    else
			    {
					make_huffman_map(tree_next_data,bit);
			    }
			}
		}
        static uint revBit(uint bit, int n)
        {
            uint ret = 0;
            int i = 0;
            for (n = n - 1; n >= 0; n--, i++)
            {
                ret = ret << 1;
                if ( (bit & (1 << i)) > 0)
                {
                    ret += 1;
                }
            }
            return ret;
        }

        //iw-ramにデータをおいている人がいるらしい.
        public static bool IsUnHuffmanPatch_IW_RAMPointer(uint pointer)
        {
            return (pointer >= 0x83000000 && pointer <= 0x83007FFF);
        }
        //ew-ramにデータをおいている人がいるらしい.
        public static bool IsUnHuffmanPatch_EW_RAMPointer(uint pointer)
        {
            return (pointer >= 0x82000000 && pointer <= 0x8203FFFF);
        }

        public static bool IsUnHuffmanPatchPointer(uint pointer)
        {
            return (pointer >= 0x88000000 && pointer <= 0x8A000000);
        }
        public static uint ConvertUnHuffmanPatchToPointer(uint pointer)
        {
            if (!IsUnHuffmanPatchPointer(pointer))
            {
                return pointer;
            }
            return pointer - 0x80000000;
        }
        public static uint ConvertPointerToUnHuffmanPatchPointer(uint pointer)
        {
            if (!U.isPointer(pointer))
            {
                return pointer;
            }
            return pointer + 0x80000000;
        }


        //unhuffman path対応.
        public void UnHuffmanEncode(string str, out byte[] outByte)
        {
            //前処理 改行を@0001にするなど.
            str = ConvertSPMoji(str);
            if (str == "")
            {
                outByte = new byte[0];
                return;
            }

            byte[] sjisstr = Program.SystemTextEncoder.Encode(str);
            outByte = convert_unHuffman_code_to_binary(sjisstr);
        }


        public static string ConvertSPMoji(string str)
        {
            InputFormRef.PRIORITY_CODE priorityCode = InputFormRef.SearchPriorityCode();
            str = str.Replace("\r\n", "@0001");
            if (priorityCode == InputFormRef.PRIORITY_CODE.UTF8)
            {
                return str;
            }

            //日本語版は一部入りきらない文字を特殊フォント化しているので変換する.
            if (Program.ROM.RomInfo.version() >= 8)
            {
                str = str.Replace("(ｻﾝﾀﾞｰｽﾄｰﾑ)", "нопр");///No Translate
                str = str.Replace("(ﾌｫﾚｽﾄﾅｲﾄ)", "⊂⊃┌┐");///No Translate
                str = str.Replace("(ﾄﾞﾗｺﾞﾝﾏｽﾀｰ)", "абвг");///No Translate
                str = str.Replace("(ﾜｲﾊﾞｰﾝﾅｲﾄ)", "шщъы");///No Translate
                str = str.Replace("(ﾌｧﾙｺﾝﾅｲﾄ)", "жзий");///No Translate
                str = str.Replace("(ﾓｰｻﾄﾞｩｰｸﾞ)", "∞┴∠∧");///No Translate
                str = str.Replace("(ﾃﾞｽｶﾞｰｺﾞｲﾙ)", "∇∈∋├");///No Translate
                str = str.Replace("(ﾄﾞﾗｺﾞﾝｿﾞﾝﾋﾞ)", "≠≡≦≧");///No Translate
                str = str.Replace("(ｺﾞｰｺﾞﾝの卵)", "∪∫∬∴");///No Translate
            }
            else if (Program.ROM.RomInfo.version() >= 7)
            {//FE7
                str = str.Replace("(ｻﾝﾀﾞｰｽﾄｰﾑ)", "⑮⑯⑰⑱⑲⑳α");///No Translate
                str = str.Replace("(ﾌｧﾙｺﾝﾅｲﾄ)", "⑧⑨⑩⑪⑫⑬⑭");///No Translate
                str = str.Replace("(ﾄﾞﾗｺﾞﾝﾏｽﾀｰ)", "①②③④⑤⑥⑦");///No Translate
            }
            else if (Program.ROM.RomInfo.version() >= 6)
            {//FE6
                str = str.Replace("(ｻﾝﾀﾞｰｽﾄｰﾑ)", "①②③④⑤⑥⑦");///No Translate
                str = str.Replace("(ﾌｧﾙｺﾝﾅｲﾄ)", "ⅠⅡⅢⅣⅤⅥⅦ");///No Translate
                str = str.Replace("(ﾄﾞﾗｺﾞﾝﾏｽﾀｰ)", "⑪⑫⑬⑭⑮⑯⑰");///No Translate
            }
            //英語版FEにはUnicodeの1バイトだけ表記があるらしい.
            {
                for (int c = 0x82; c <= 0xff; c++)
                {
                    str = str.Replace(((char)c).ToString(), "@00" + c.ToString("X02"));
                }
            }
            return str;
        }
    }
}
