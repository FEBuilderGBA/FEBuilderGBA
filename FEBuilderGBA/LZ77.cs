using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

//この関数は、Nintenlord's lz77 および FE Editor Adv を参考にしています.
//
//追加参考文献 Algorithms with Python LZ77 符号 (LZSS 符号)
// see http://www.geocities.jp/m_hiroi/light/pyalgo32.html
//
namespace FEBuilderGBA
{
    class LZ77
    {
        const int WINDOW_SIZE = 4096;
        const int READ_AHEAD_BUFFER_SIZE = 18;  //18がGBAの上限
        const int BLOCK_SIZE = 8;
        const int SLIDING_WINDOW_SIZE = 0x1000;

        const int MAX_UNCOMP_DATA_LIMIT = 0x14000;

        //圧縮されてそうか？
        public static bool iscompress(byte[] input,uint offset)
        {
            return getCompressedSize(input, offset) > 0; 
        }
        //解凍したときのサイズ.
        public static uint getUncompressSize(byte[] input, uint offset)
        {
            if (input.Length <= offset)
            {
                return 0;
            }
            if (input.Length - offset < 3)
            {
                return 0;
            }
            if (input[0 + offset] != 0x10)
            {
                return 0;
            }

            int size = input[1 + offset] + (input[2 + offset] << 8) + (input[3 + offset] << 16);
            if (size < 3)
            {
                return 0;
            }

            if (size > MAX_UNCOMP_DATA_LIMIT)
            {
                return 0;
            }

            return (uint)size;
        }

        //圧縮されているデータ長を求めます.
        public static uint getCompressedSize(byte[] input, uint offset)
        {
            if (input.Length <= offset)
            {
                return 0;
            }
            if (input.Length - offset < 3)
            {
                return 0;
            }
            if (input[0 + offset] != 0x10)
            {
                return 0;
            }

            int size = input[1 + offset] + (input[2 + offset] << 8) + (input[3 + offset] << 16);
            if (size < 3)
            {
                return 0;
            }
            if (size > MAX_UNCOMP_DATA_LIMIT)
            {
                //Debug.Assert(false);
                return 0;
            }

            int writeAddress = 0;
            uint inputlimit = (uint)input.Length;
            uint index = 4 + offset;

            while (writeAddress < size)
            {
                if (index >= inputlimit)
                {//データ終端を超えている!
                    return 0;
                }

                //ブロックの圧縮状況を取得.
                byte isCompressed = input[index];
                index++;

                for (int blockCount = 1 << 7; blockCount > 0; blockCount = blockCount >> 1)
                {
                    if (writeAddress >= size)
                    {//変なデータがあったときに落ちないように、常に境界チェックする.
                        break;
                    }
                    if (index >= inputlimit)
                    {//データ終端を超えている!
                        break;
                    }

                    if ((isCompressed & blockCount) == 0)
                    {//無圧縮
                        writeAddress++;
                        index++;
                    }
                    else
                    {//圧縮されている
                        int first = input[index];
                        index++;
                        if (index >= inputlimit)
                        {//データ終端を超えている!
                            return 0;
                        }

                        int second = input[index];
                        index++;

                        int amountToCopy = 3 + (first >> 4); //match_count
                        int copyOffset = ((first & 0x0F) << 8) | (second & 0xFF); //match_pos

                        if (writeAddress + amountToCopy >= size)
                        {//別ツールでやったときに壊れたデータが格納されるときがある.
                            //最大値を超える場合は補正する.
                            amountToCopy = size - writeAddress;
                        }
                        if (writeAddress <= copyOffset)
                        {//どうしようもない壊れ方をしている場合、無視するしかない.
                            amountToCopy = 0;
                        }

                        writeAddress += amountToCopy;
                    }
                }
            }
            if (writeAddress != size)
            {//予定しているサイズと違う.
                return 0;
            }

            return (uint)(index - offset);
        }
        //圧縮されているデータをそのまま取得する
        public static byte[] GetCompressDataLow(byte[] input, uint offset)
        {
            uint image_size = LZ77.getCompressedSize(input, offset);
            if (image_size <= 0)
            {
                return new byte[0] ;
            }
            return U.getBinaryData(input, offset, image_size);
        }

        //解凍する.
        public static byte[] decompress(byte[] input, uint offset)
        {
            if (input.Length <= offset)
            {
                return new byte[0];
            }
            if (input.Length - offset < 4)
            {
                return new byte[0];
            }
            if (input[0 + offset] != 0x10)
            {
                return new byte[0];
            }

            int size = input[1 + offset] + (input[2 + offset] << 8) + (input[3 + offset] << 16);
            if (size < 3)
            {
                return new byte[0];
            }
//            Debug.Assert(size <= MAX_UNCOMP_DATA_LIMIT);

            int writeAddress = 0;
            uint inputlimit = (uint)input.Length;
            uint index = 4 + offset;
            byte[] output = new byte[size];

            while (writeAddress < size)
            {
                if (index >= inputlimit)
                {//データ終端を超えている!
                    break;
                }

                //ブロックの圧縮状況を取得.
                byte isCompressed = input[index];
                index++;

                for (int blockCount = 1 << 7; blockCount > 0; blockCount = blockCount >> 1)
                {
                    if (writeAddress >= size)
                    {//変なデータがあったときに落ちないように、常に境界チェックする.
                        break;
                    }
                    if (index >= inputlimit)
                    {//データ終端を超えている!
                        break;
                    }

                    if ((isCompressed & blockCount) == 0)
                    {//無圧縮
                        output[writeAddress++] = input[index];
                        index++;
                    }
                    else
                    {//圧縮されている
                        int first = input[index];
                        index++;
                        if (index >= inputlimit)
                        {//データ終端を超えている!
                            break;
                        }

                        int second = input[index];
                        index++;

                        int amountToCopy = 3 + (first >> 4); //match_count
                        int copyOffset = ((first & 0x0F) << 8) | (second & 0xFF); //match_pos

                        if (writeAddress + amountToCopy >= size)
                        {//別ツールでやったときに壊れたデータが格納されるときがある.
                            //最大値を超える場合は補正する.
                            amountToCopy = size - writeAddress;
                        }
                        if (writeAddress <= copyOffset )
                        {//どうしようもない壊れ方をしている場合、無視するしかない.
                            amountToCopy = 0;
                        }

                        for (int i = 0; i < amountToCopy; i++, writeAddress++)
                        {
                            output[writeAddress] = output[writeAddress - copyOffset - 1];
                        }
                    }
                }
            }
            return output;
        }
        public static int search(byte[] input, int position, int length, out int out_match_pos)
        {
            List<int> results = new List<int>(READ_AHEAD_BUFFER_SIZE);

            if (!(position < length))
            {
                out_match_pos = 0;
                return 0;
            }
            if ((position < 3) || ((length - position) < 3))
            {
                out_match_pos = 0;
                return 0;
            }

            for (int i = 1; ((i < SLIDING_WINDOW_SIZE) && (i < position)); i++)
            {
                if (
                    input[position - i - 1]
                    == input[position]
                )
                {
                    results.Add(i + 1);
                }
            }
            if (results.Count == 0)
            {
                out_match_pos = 0;
                return 0;
            }

            int amountOfBytes = 0;

            while (amountOfBytes < READ_AHEAD_BUFFER_SIZE)
            {
                amountOfBytes++;
                if (position + amountOfBytes >= length)
                {
                    out_match_pos = 0;
                    return 0;
                }
                bool searchComplete = false;
                for (int i = results.Count - 1; i >= 0; i--)
                {
                    if (
                        input[position + amountOfBytes]
                        !=
                        input[
                            position - results[i]
                            + (
                                amountOfBytes
                                % results[i]
                            )
                        ]
                    )
                    {
                        if (results.Count > 1)
                        {
                            results.RemoveAt(i);
                        }
                        else
                        {
                            searchComplete = true;
                        }
                    }
                }
                if (searchComplete)
                {
                    break;
                }
            }

            //Length of data is first, then position
            out_match_pos = results[0];
            return amountOfBytes;
        }
        // search method; FE EDitor からもってきました。 自分で作った奴はバグがあるみたいなので・・・


        //圧縮
        public static byte[] compress(byte[] data)
        {
            Debug.Assert(data.Length <= MAX_UNCOMP_DATA_LIMIT);

            List<byte> compressedData = new List<byte>(MAX_UNCOMP_DATA_LIMIT);

            int length = data.Length;
            int position = 0;

            //header
            compressedData.Add((byte)0x10);
            compressedData.Add((byte)length);
            compressedData.Add((byte)(length >> 8));
            compressedData.Add((byte)(length >> 16));

            while (position < length)
            {
                //8ブロック単位で圧縮.
                byte isCompressed = 0;
                List<byte> tempVector = new List<byte>(BLOCK_SIZE);

                for (int i = 0; i < BLOCK_SIZE; i++)
                {
                    if (position >= length)
                    {
                        break;
                    }

                    int match_count ;
                    int match_pos ;
                    match_count = search(data, position, length, out match_pos);
                    if (match_count > 2)
                    {//圧縮できた
                        Debug.Assert(match_count - 3 <= 0xF);
                        Debug.Assert(match_pos - 1 <= 0xFFF);
                        int add = (((match_count - 3) & 0xF) << 4)
                                + (((match_pos-1) >> 8) & 0xF);
                        tempVector.Add((byte)add);

                        add = (match_pos-1) & 0xFF;
                        tempVector.Add((byte)add);

                        position += match_count;

                        int bit = 1 << (BLOCK_SIZE - (i + 1));
                        isCompressed |= (byte)bit;
                    }
                    else
                    {//圧縮できない
                        tempVector.Add( (byte)data[position]);
                        position++;
                    }
                }
                compressedData.Add(isCompressed);
                compressedData.AddRange(tempVector);
            }
		    while (compressedData.Count % 4 != 0)
			    compressedData.Add((byte) 0);

#if DEBUG
            //デバッグ時は常に圧縮チェックします.
            byte[] ret = compressedData.ToArray();
            DEBUG_LZ77CHECK(data, ret);
            return ret;
#else
            return compressedData.ToArray();
#endif
        }


#if DEBUG
        public static void TEST_LZ77()
        {
            byte[] a = new byte[15];
            a[0] = (byte)'a';
            a[1] = (byte)'b';
            a[2] = (byte)'r';
            a[3] = (byte)'a';
            a[4] = (byte)'k';
            a[5] = (byte)'a';
            a[6] = (byte)'t';
            a[7] = (byte)'a';
            a[8] = (byte)'b';
            a[9] = (byte)'r';
            a[10] = (byte)'a';
            a[11] = (byte)'x';
            a[12] = (byte)'y';
            a[13] = (byte)'z';
            a[14] = (byte)'a';

            //圧縮した結果
            byte[] z_ans = new byte[20];
            z_ans[0] = (byte)0x10;
            z_ans[1] = (byte)0x0F;
            z_ans[2] = (byte)0x00;
            z_ans[3] = (byte)0x00;
            z_ans[4] = (byte)0x01;
            z_ans[5] = (byte)0x61;
            z_ans[6] = (byte)0x62;
            z_ans[7] = (byte)0x72;
            z_ans[8] = (byte)0x61;
            z_ans[9] = (byte)0x6B;
            z_ans[10] = (byte)0x61;
            z_ans[11] = (byte)0x74;
            z_ans[12] = (byte)0x10;
            z_ans[13] = (byte)0x06;
            z_ans[14] = (byte)0x00;
            z_ans[15] = (byte)0x78;
            z_ans[16] = (byte)0x79;
            z_ans[17] = (byte)0x7A;
            z_ans[18] = (byte)0x61;
            z_ans[19] = (byte)0x00;

            //圧縮
            byte[] z = compress(a);
            System.Diagnostics.Debug.Assert(z.Length == z_ans.Length);
            for (int i = 0; i < z.Length; i++)
            {
                System.Diagnostics.Debug.Assert(z[i] == z_ans[i]);
            }
           
            //複合
            byte[] o = decompress(z,0);
            System.Diagnostics.Debug.Assert(a.Length == o.Length);
            for (int i = 0; i < a.Length; i++)
            {
                System.Diagnostics.Debug.Assert(a[i] == o[i]);
            }
        }
        public static void TEST_LZ77_bin()
        {

            byte[] a = File.ReadAllBytes(Program.GetTestData("TEST_GrepTileBitmap.png")); 
            byte[] z = compress(a);

            DEBUG_LZ77CHECK(a, z);
        }

        public static void DEBUG_LZ77CHECK(byte[] original_data, byte[] lz77data)
        {
            byte[] uncomp = LZ77.decompress(lz77data, 0);
            Debug.Assert(original_data.Length == uncomp.Length);

            for (int i = 0; i < uncomp.Length; i++)
            {
                Debug.Assert(original_data[i] == uncomp[i]);
            }
        }
#endif
    }
}
