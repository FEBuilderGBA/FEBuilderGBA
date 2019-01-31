using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;

namespace FEBuilderGBA
{
    public partial class ToolDecompileResultInnerControl : UserControl
    {
        public ToolDecompileResultInnerControl()
        {
            InitializeComponent();
        }
        public class NavigationEventArgs : EventArgs
        {
            public bool IsNewTab; //新しいタブを開いてほしい時
            public uint Address;  //表示するアドレス
            public uint Size;     //表示するサイズ
        };

        //変更があったとき
        public EventHandler Navigation;

        public void JumpTo(uint addr, uint size)
        {
            if (addr == 0 || size == 0)
            {
                return;
            }
            DelayDecompile(addr , size);
        }

        public void SetFocus()
        {
            this.Code.Focus();
        }

        EventHandler DelayEventHandler;
        class DecomplieResult : EventArgs
        {
            public string Result;
        }

        void DelayDecompile(uint addr, uint size)
        {
            string retdec = OptionForm.GetRetDec();
            if (retdec == "")
            {
                R.ShowStopError("RetDec逆コンパイラが設定されていません。\r\n設定のパス2の画面から、RetDecを動作させるためにRetDecの設定をしてください。");
                return;
            }
            string python3 = OptionForm.GetPython3();
            if (python3 == "")
            {
                R.ShowStopError("python3が設定されていません。\r\n設定のパス2の画面から、RetDecを動作させるために必要なpythonの設定をしてください。");
                return;
            }

            this.Code.Text = R._("逆コンパイル中です。しばらくお待ちください。");

            //逆コンパイル結果が代入される
            string output_c = Path.GetTempFileName();

            System.Threading.Thread s1 = new System.Threading.Thread(t =>
            {
                // 双引号中可以包含空格，区分绝对路径和相对路径，参数解析太麻烦了，所以先假定没有设置输出文件
                string retdec_option = "-a thumb -e little -k -l c -m raw --raw-section-vma 0x8000000 --select-decode-only --cleanup --backend-no-opts --backend-no-debug";
                retdec_option += " --raw-entry-point " + U.To0xHexString(U.toPointer(addr)) + " --select-ranges " + U.To0xHexString(U.toPointer(addr)) + "-" + U.To0xHexString(U.toPointer(addr + size));
                string args = U.escape_shell_args(retdec) + " " + retdec_option + " -o " + U.escape_shell_args(output_c) + " " + U.escape_shell_args(Program.ROM.Filename);
                Log.Debug(args);
                string stdout = MainFormUtil.ProgramRunAsAndEndWait(python3,args);
                Log.Debug(stdout);

                DecomplieResult ee = new DecomplieResult();
                if (!File.Exists(output_c))
                {
                    ee.Result = R._("ファイルを読めませんでした。") +  "\r\n" + output_c + "\r\n" + stdout;
                }
                else
                {
                    ee.Result =File.ReadAllText(output_c);
                    File.Delete(output_c);
                }

                this.Invoke(DelayEventHandler, new object[] { this,ee });
            });

            //逆コンパイラの完了を受けとる
            DelayEventHandler = (ss,ee) =>
            {
                DecomplieResult dr = (DecomplieResult)ee;
                this.Code.Text = dr.Result;
            };

            //スレッドを利用して、逆コンパイラを実行
            s1.Start();
        }

    }
}
