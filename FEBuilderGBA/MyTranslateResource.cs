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
        public static void LoadResource(string fullfilename)
        {
            Resource.LoadResource(fullfilename);
        }
    }
}
