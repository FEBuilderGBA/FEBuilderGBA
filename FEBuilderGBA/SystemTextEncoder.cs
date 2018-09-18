using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace FEBuilderGBA
{
    public class SystemTextEncoder
    {
        Encoding Encoder;
        SystemTextEncoderTBLEncodeClass TBLEncode;
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
            if (textencoding == OptionForm.textencoding_enum.ZH_TBL && rom != null)
            {
                string resoucefilename = System.IO.Path.Combine(Program.BaseDirectory, "config", "translate", "zh_tbl", rom.TitleToFilename() + ".tbl");
                if (File.Exists(resoucefilename))
                {
                    this.TBLEncode = new SystemTextEncoderTBLEncodeClass(resoucefilename);
                    this.Encoder = null;
                    return;
                }
                Log.Error("tbl not found. filename:{0}",resoucefilename);
            }
            else if (textencoding == OptionForm.textencoding_enum.EN_TBL && rom != null)
            {
                string resoucefilename = System.IO.Path.Combine(Program.BaseDirectory, "config", "translate", "en_tbl", rom.TitleToFilename() + ".tbl");
                if (File.Exists(resoucefilename))
                {
                    this.TBLEncode = new SystemTextEncoderTBLEncodeClass(resoucefilename);
                    this.Encoder = null;
                    return;
                }
                Log.Error("tbl not found. filename:{0}", resoucefilename);
            }

            InputFormRef.PRIORITY_CODE priorityCode = InputFormRef.SearchPriorityCode(rom);
            if (priorityCode == InputFormRef.PRIORITY_CODE.UTF8)
            {
                this.Encoder = System.Text.Encoding.GetEncoding("UTF-8");
            }
            else
            {
                //ディフォルトは日本語.
                this.Encoder = System.Text.Encoding.GetEncoding("Shift_JIS");
            }

            this.TBLEncode = null;
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

    }
}
