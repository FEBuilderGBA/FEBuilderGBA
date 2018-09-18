using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FEBuilderGBA
{
    //正規表現のキャッシュ
    //コンパイルして永続的にキャッシュします.
    static class RegexCache
    {
        static Dictionary<string,Regex> Cache = new Dictionary<string,Regex>();
        public static Regex Regex(string pattern)
        {
            Regex r;
            if(Cache.TryGetValue(pattern,out r))
            {
                return r;
            }
            r = new Regex(pattern, RegexOptions.Compiled);
            Cache[pattern] =  r;
            return r;
        }
        public static string Replace(string str, string pattern, string replace)
        {
            return Regex(pattern).Replace(str, replace);
        }
        public static Match Match(string str, string pattern)
        {
            return Regex(pattern).Match(str);
        }
    }
}
