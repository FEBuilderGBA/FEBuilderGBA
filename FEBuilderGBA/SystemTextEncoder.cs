using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace FEBuilderGBA
{
    public interface SystemTextEncoderTBLEncodeInterface
    {
        string Decode(byte[] str);
        string Decode(byte[] str, int start, int len);
        byte[] Encode(string str);

        Dictionary<string, uint> GetEncodeDicLow();
    }
    public class SystemTextEncoder
    {
        Encoding Encoder;
        SystemTextEncoderTBLEncodeInterface TBLEncode;
        public SystemTextEncoder()
        {
            Build();
        }
        public SystemTextEncoder(OptionForm.textencoding_enum textencoding, ROM rom)
        {
            Build(textencoding, rom);
        }

        public void Build()
        {
            OptionForm.textencoding_enum textencoding = OptionForm.textencoding();
            Build(textencoding, Program.ROM);
        }

        public void Build(OptionForm.textencoding_enum textencoding,ROM rom)
        {
            bool r = LoadTBL(textencoding, rom);
            if (r)
            {//TBLを利用.
                return;
            }

            if (textencoding == OptionForm.textencoding_enum.Auto)
            {//自動選択
                PatchUtil.PRIORITY_CODE priorityCode = PatchUtil.SearchPriorityCode(rom);
                if (priorityCode == PatchUtil.PRIORITY_CODE.UTF8)
                {
                    this.Encoder = System.Text.Encoding.GetEncoding("UTF-8");
                }
                else if (priorityCode == PatchUtil.PRIORITY_CODE.LAT1)
                {
                    this.Encoder = System.Text.Encoding.GetEncoding("iso-8859-1");
                }
                else
                {
                    //ディフォルトは日本語.
                    this.Encoder = System.Text.Encoding.GetEncoding("Shift_JIS");
                }
            }
            else if (textencoding == OptionForm.textencoding_enum.LAT1)
            {
                this.Encoder = System.Text.Encoding.GetEncoding("iso-8859-1");
            }
            else if (textencoding == OptionForm.textencoding_enum.UTF8)
            {
                this.Encoder = System.Text.Encoding.GetEncoding("UTF-8");
            }
            else if (textencoding == OptionForm.textencoding_enum.Shift_JIS)
            {
                this.Encoder = System.Text.Encoding.GetEncoding("Shift_JIS");
            }

            this.TBLEncode = null;
        }
        bool LoadTBL(OptionForm.textencoding_enum textencoding, ROM rom)
        {
            if (rom == null)
            {
                return false;
            }

            if (textencoding == OptionForm.textencoding_enum.ZH_TBL)
            {
                string resoucefilename = System.IO.Path.Combine(Program.BaseDirectory, "config", "translate", "zh_tbl", rom.RomInfo.TitleToFilename() + ".tbl");
                if (! File.Exists(resoucefilename))
                {
                    Log.Error("tbl not found. filename:{0}", resoucefilename);
                    return false;
                }
                this.TBLEncode = new SystemTextEncoderTBLEncode(resoucefilename);
                this.Encoder = null;
                return true;
            }
            else if (textencoding == OptionForm.textencoding_enum.EN_TBL)
            {
                string resoucefilename = System.IO.Path.Combine(Program.BaseDirectory, "config", "translate", "en_tbl", rom.RomInfo.TitleToFilename() + ".tbl");
                if (! File.Exists(resoucefilename))
                {
                    Log.Error("tbl not found. filename:{0}", resoucefilename);
                    return false;
                }
                this.TBLEncode = new SystemTextEncoderTBLEncode(resoucefilename);
                this.Encoder = null;
                return true;
            }
            else if (textencoding == OptionForm.textencoding_enum.AR_TBL)
            {
                string resoucefilename = System.IO.Path.Combine(Program.BaseDirectory, "config", "translate", "ar_tbl", rom.RomInfo.TitleToFilename() + ".arabic_tbl");
                if (! File.Exists(resoucefilename))
                {
                    Log.Error("tbl not found. filename:{0}", resoucefilename);
                    return false;
                }
                SystemTextEncoderTBLEncode inner = null;
                if (Program.ROM.RomInfo.version() == 6)
                {
                    string resoucefilename_inner = System.IO.Path.Combine(Program.BaseDirectory, "config", "translate", "en_tbl", rom.RomInfo.TitleToFilename() + ".tbl");
                    if (! File.Exists(resoucefilename))
                    {
                        return false;
                    }
                    inner = new SystemTextEncoderTBLEncode(resoucefilename_inner);
                }
                this.TBLEncode = new SystemTextEncoderArabianTBLEncode(resoucefilename,inner);
                this.Encoder = null;
                return true;
            }
            else if (textencoding == OptionForm.textencoding_enum.KR_TBL)
            {
                string resoucefilename = System.IO.Path.Combine(Program.BaseDirectory, "config", "translate", "kr_tbl", rom.RomInfo.TitleToFilename() + ".tbl");
                if (!File.Exists(resoucefilename))
                {
                    Log.Error("tbl not found. filename:{0}", resoucefilename);
                    return false;
                }
                this.TBLEncode = new SystemTextEncoderTBLEncode(resoucefilename);
                this.Encoder = null;
                return true;
            }
            return false;
        }

        public string Decode(byte[] str)
        {
            if (this.Encoder == null)
            {
                return this.TBLEncode.Decode(str);
            }
            else
            {
                return this.Encoder.GetString(str);
            }
        }
        public string Decode(byte[] str,int start,int len)
        {
            if (this.Encoder == null)
            {
                return this.TBLEncode.Decode(str, start, len);
            }
            else
            {
                return this.Encoder.GetString(str, start,len);
            }
        }
        public byte[] Encode(string str)
        {
            if (this.Encoder == null)
            {
                return this.TBLEncode.Encode(str);
            }
            else
            {
                return this.Encoder.GetBytes(str);
            }
        }

        public Dictionary<string, uint> GetTBLEncodeDicLow()
        {
            if (this.Encoder == null)
            {
                return this.TBLEncode.GetEncodeDicLow();
            }
            else
            {
                return new Dictionary<string, uint>();
            }
        }
    }
}
