﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

namespace FEBuilderGBA
{
    public partial class VersionForm : Form
    {
        public VersionForm()
        {
            InitializeComponent();

            //see   ビルド時間の取得
            //http://devlights.hatenablog.com/entry/2015/04/14/230841 

            string ver;
#if DEBUG
            ver = "-Debug Build-";
#else
            ver = U.getVersion();
#endif

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(
                R._("{1} Version:{0}\r\nCopyright: 2017-\r\nLicense: GPLv3\r\n\r\nこのソフトウェアはオープンソースのフリーソフトウェアです。\r\nGPLv3に従ってご自由にお使いください。"
                , ver
                , typeof(U).Assembly.GetName().Name
                )
                );

            sb.AppendLine();
            if (Program.ROM != null)
            {
                string FEVersion = "";
                FEVersion = Program.ROM.RomInfo.VersionToFilename;
                FEVersion += " @ROMSize: " + Program.ROM.Data.Length;

                U.CRC32 crc32 = new U.CRC32();
                uint targetCRC32 = crc32.Calc(Program.ROM.Data);
                FEVersion += " @CRC32: " + U.ToHexString8(targetCRC32);
                sb.AppendLine("FEVersion:" + FEVersion);

                if (targetCRC32 == Program.ROM.RomInfo.orignal_crc32)
                {
                    sb.AppendLine(R._("[このROMは正規品の無改造ROMです。]"));
                }
            }

            if (IsOldVersion())
            {
                sb.AppendLine(MakeUpdateMessage());
            }


            Version.Text = sb.ToString();
            Version.Select(0, 0); //全選択解除.
#if DEBUG
            DevTranslateButton.Show();
#endif
        }

        public static bool IsOldVersion()
        {
            bool isDebug;
#if DEBUG
            isDebug = true;
#else
            isDebug = true;
#endif
            if (isDebug)
            {
                return false;
            }
            DateTime rdate;
            string ver = U.getVersion();
            bool r = DateTime.TryParseExact(ver, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out rdate);
            
            if (!r)
            {
                return false;
            }
            if (rdate.AddMonths(1) < DateTime.Now)
            {
                return true;
            }

            return false;
        }
        public static string MakeUpdateMessage()
        {
            string str = R._("現在利用しているバージョンから、1か月以上が経過しています。\r\n新しいバージョンが出ていないか確認してください。");

            string lang = OptionForm.lang();
            string url = MainFormUtil.GetManualURL();

            str += "\r\n" + "https://github.com/FEBuilderGBA/FEBuilderGBA/releases/latest";
            str += "\r\n" + url;

            return str;
        }


        private void VersionForm_Load(object sender, EventArgs e)
        {
        }

        private void DevTranslateButton_Click(object sender, EventArgs e)
        {
            InputFormRef.JumpForm<DevTranslateForm>();
        }

        int KonamiCommand_MatchCount = 0;
        Keys[] KonamiCommand = new Keys[] { Keys.Up, Keys.Down, Keys.Right, Keys.Right, Keys.B ,Keys.A };

        private void Version_KeyDown(object sender, KeyEventArgs e)
        {
            if (KonamiCommand[KonamiCommand_MatchCount] == e.KeyCode)
            {
                KonamiCommand_MatchCount++;
                if (KonamiCommand_MatchCount >= KonamiCommand.Length)
                {
                    KonamiCommand_MatchCount = 0;
                    DevTranslateButton.Show();
                }
            }

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
