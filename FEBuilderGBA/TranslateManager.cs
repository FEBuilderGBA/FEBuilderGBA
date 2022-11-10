using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEBuilderGBA
{
    public enum TranslateEngineEnum
    {
         Google
        ,Google2
    }

    public class TranslateManage
    {
        TranslateEngineEnum TranslateEngine = TranslateEngineEnum.Google2;

        void ChangeTranslateAPI(TranslateEngineEnum api)
        {
            TranslateEngine = api;
        }

        //複数行の翻訳
        public string TransMulti(string src, string from, string to)
        {
            if (TranslateEngine == TranslateEngineEnum.Google)
            {
                TranslateGoogle trans = new TranslateGoogle();
                return trans.TransMulti(src, from, to);
            }
            else if (TranslateEngine == TranslateEngineEnum.Google2)
            {
                TranslateGoogle2 trans = new TranslateGoogle2();
                return trans.TransMulti(src, from, to);
            }
            return "";
        }

        //1行の翻訳
        public string Trans(string src, string from, string to)
        {
            if (TranslateEngine == TranslateEngineEnum.Google)
            {
                TranslateGoogle trans = new TranslateGoogle();
                return trans.Trans(src, from, to);
            }
            else if (TranslateEngine == TranslateEngineEnum.Google2)
            {
                TranslateGoogle2 trans = new TranslateGoogle2();
                return trans.Trans(src, from, to);
            }
            return "";
        }

        //1行の翻訳
        public string TransOne(string src, string from, string to)
        {
            if (TranslateEngine == TranslateEngineEnum.Google)
            {
                TranslateGoogle trans = new TranslateGoogle();
                return trans.TransOne(src, from, to);
            }
            else if (TranslateEngine == TranslateEngineEnum.Google2)
            {
                TranslateGoogle2 trans = new TranslateGoogle2();
                return trans.TransOne(src, from, to);
            }
            return "";
        }
    }
}
