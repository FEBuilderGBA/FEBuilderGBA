using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace FEBuilderGBA
{
    //C#のリソースはなんだかいけていないので、
    //自前リソースを作る.
    //
    //:文字列
    //翻訳
    //
    //:文字列
    //翻訳
    //
    //
    public static class MyTranslateResource
    {
        static MyTranslateResourceLow Resource = new MyTranslateResourceLow();

        //翻訳文字列の取得
        public static string str(string src)
        {
            return Resource.str(src);
        }
        public static string str(string src, params object[] args)
        {
            string trans = str(src);

#if !DEBUG
            try
            {
#endif
                if (args.Length <= 0)
                {
                    return trans;
                }
                return string.Format(trans, args);
#if !DEBUG
            }
            catch (FormatException e)
            {
                Log.Error("Translate Error! {0}->{1} @@ {2}" , src,trans , e.ToString() );
                try
                {
                    return string.Format(src, args);
                }
                catch (FormatException e2)
                {
                    Log.Error("Translate Error2! {0} @@ {1}", src, e2.ToString());
                    return src;
                }
            }
#endif
        }
        public static void LoadResource(string fullfilename)
        {
            Resource.LoadResource(fullfilename);
        }
    }
}
