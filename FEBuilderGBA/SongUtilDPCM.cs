using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace FEBuilderGBA
{

    //うごかん そのうち消す
    class SongUtilDPCM
    {
        const uint DPCM_BLK_SIZE = 0x40;
        static int[] dpcmLookupTable = new int[]{0, 1, 4, 9, 16, 25, 36, 49, -64, -49, -36, -25, -16, -9, -4, -1};

        static int squared(int x) { return x * x; }

        static void dpcm_lookahead(
                out int minimumError, out uint minimumErrorIndex,
                int[] sampleBuf, uint startIndex, uint lookahead, int prevLevel)
        {
            if (lookahead == 0)
            {
                minimumError = 0;
                minimumErrorIndex = 0;
                return;
            }

            minimumError = int.MaxValue;
            minimumErrorIndex = 0; //一度も補正されない時があるので 0 を設定するべき
            for (uint i = 0; i < dpcmLookupTable.Length; i++)
            {
                int newLevel = prevLevel + dpcmLookupTable[i];

                // TODO apply dither noise
                int s = sampleBuf[startIndex];
                int errorEstimation = squared(s - newLevel);
                if (errorEstimation >= minimumError)
                {
                    continue;
                }

                int recMinimumError;
                uint recMinimumErrorIndex;
                dpcm_lookahead(out recMinimumError, out recMinimumErrorIndex,
                        sampleBuf, startIndex + 1, lookahead - 1, newLevel);

                // TODO weigh the error squared
                int error = squared(s - newLevel) + recMinimumError;
                if (error < minimumError)
                {
                    if (newLevel <= 127 && newLevel >= -128)
                    {
                        minimumError = error;
                        minimumErrorIndex = i;
                    }
                }
            }
        }

        static int[] Add0x80(byte[] s)
        {
            int[] ds = new int[DPCM_BLK_SIZE];
            for (int i = 0; i < s.Length; i++)
            {
                int dd = ((int)s[i]) + 0x80;
                ds[i] = (sbyte)dd;
            }
            return ds;
        }

        public static byte[] wavToDPCMByte(byte[] data, uint dpcm_enc_lookahead = 3, bool loopEnabled = false, int loopStart = 0, int loopEnd = 0)
        {
            List<byte> wave = new List<byte>();
            if (data[0] != 'R'
            || data[1] != 'I'
            || data[2] != 'F'
            || data[3] != 'F'
                )
            {
                R.ShowStopError("Waveファイルではありません. RIFFヘッダがありません");
                return null;
            }
            if (data.Length < (44 + 1))
            {
                R.ShowStopError("Waveファイルではありません. データが小さすぎます");
                return null;
            }

            uint riff_chunk_size = U.u32(data, 4);
            uint riff_form_type = U.u32(data, 8);
            uint fmt_chunk_ID = U.u32(data, 12);
            uint fmt_chunk_size = U.u32(data, 16);
            uint fmt_wave_format_type = U.u16(data, 20);
            uint fmt_channel = U.u16(data, 32);
            uint fmt_samples_per_sec = U.u32(data, 24);
            uint fmt_bytes_per_sec = U.u32(data, 28);
            uint fmt_block_size = U.u16(data, 32);
            uint fmt_bits_per_sample = U.u16(data, 34);
            uint data_chunk_ID = U.u32(data, 36);
            uint data_chunk_size = U.u32(data, 40);
            if (data_chunk_size > data.Length - (44))
            {//チャンクのデータサイズが不正だったら修正する.
                data_chunk_size = (uint)(data.Length - (44));
            }
            if (data_chunk_size <= 1)
            {
                R.ShowStopError("Waveファイルではありません. data_chunk_size({0})が小さすぎます", data_chunk_size);
                return null;
            }

            PatchUtil.ImprovedSoundMixer withImprovedSoundMixer = PatchUtil.SearchImprovedSoundMixer();
            if (fmt_bits_per_sample > 8)
            {//サンプルビット数が8ビットを超える
                R.ShowStopError("Waveファイルが高音質すぎます。{0}bit\r\n品質は、8bit 12khz monoぐらいにしてください。", fmt_bits_per_sample);
                return null;
            }

            U.append_u32(wave, 0x01); //圧縮フラグ 0x01 00 00 00
            U.append_u32(wave, fmt_samples_per_sec * 1024);//周波数*1024
            U.append_u32(wave, 0); //不明
            U.append_u32(wave, data_chunk_size); //元のデータ長

            if (loopEnabled)
            {
                if (loopEnd > data_chunk_size)
                {
                    loopEnd = (int)data_chunk_size;
                }
            }
            else
            {//ループしない場合終端になります
                loopEnd = (int)data_chunk_size;
            }

            int loop_sample = 0;
            for (uint i = 0; i < loopEnd; i += DPCM_BLK_SIZE) 
            {
                uint waveDataI = 44 + i;
                byte[] wavBin = U.getBinaryData(data, waveDataI, DPCM_BLK_SIZE);
                int[] ds = Add0x80(wavBin);

                // check if loop end is inside this block
                if (i + DPCM_BLK_SIZE > loopEnd)
                {
                    Debug.Assert(loopEnd - i < DPCM_BLK_SIZE);
                    ds[loopEnd - i] = (byte)loop_sample;
                }
                // TODO apply dither noise
                int s = ds[0];
                if (loopEnabled && i == loopStart)
                {
                    loop_sample = s;
                }

                U.append_u8(wave, (byte)(s & 0xFF));
                //data_write(ofs, block_pos, s, false);

                uint innerLoopCount = 1;
                byte outData = 0;
                while (innerLoopCount < DPCM_BLK_SIZE)
                {
                    uint sampleBufReadLen = Math.Min(dpcm_enc_lookahead, DPCM_BLK_SIZE - innerLoopCount);
                    int minimumError;
                    uint minimumErrorIndex;
                    if (innerLoopCount != 1)
                    {//どういうわけか、最初の1バイトは特殊な格納方法をする
                        dpcm_lookahead(
                                out minimumError, out minimumErrorIndex,
                                ds, innerLoopCount, sampleBufReadLen, s);
                        outData = (byte)((minimumErrorIndex & 0xF) << 4);
                        s += dpcmLookupTable[minimumErrorIndex];
                        innerLoopCount += 1;
                    }

                    sampleBufReadLen = Math.Min(dpcm_enc_lookahead, DPCM_BLK_SIZE - innerLoopCount);
                    dpcm_lookahead(
                            out minimumError, out minimumErrorIndex,
                            ds,innerLoopCount, sampleBufReadLen, s);
                    outData |= (byte)(minimumErrorIndex & 0xF);
                    s += dpcmLookupTable[minimumErrorIndex];
                    innerLoopCount += 1;

                    U.append_u8(wave, (byte)(outData & 0xFF));
                    //data_write(ofs, block_pos, outData, true);
                }
            }

            return wave.ToArray();
        }

    }
}
