using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace FEBuilderGBA
{
    public static class MultiByteJPUtil
    {

        static string[] replaceTableAplha = new string[]{
		         "Ａ","A"    ///No Translate
		        ,"Ｂ","B"    ///No Translate
		        ,"Ｃ","C"    ///No Translate
		        ,"Ｄ","D"    ///No Translate
		        ,"Ｅ","E"    ///No Translate
		        ,"Ｆ","F"    ///No Translate
		        ,"Ｇ","G"    ///No Translate
		        ,"Ｈ","H"    ///No Translate
		        ,"Ｉ","I"    ///No Translate
		        ,"Ｊ","J"    ///No Translate
		        ,"Ｋ","K"    ///No Translate
		        ,"Ｌ","L"    ///No Translate
		        ,"Ｍ","M"    ///No Translate
		        ,"Ｎ","N"    ///No Translate
		        ,"Ｏ","O"    ///No Translate
		        ,"Ｐ","P"    ///No Translate
		        ,"Ｑ","Q"    ///No Translate
		        ,"Ｒ","R"    ///No Translate
		        ,"Ｓ","S"    ///No Translate
		        ,"Ｔ","T"    ///No Translate
		        ,"Ｕ","U"    ///No Translate
		        ,"Ｖ","V"    ///No Translate
		        ,"Ｗ","W"    ///No Translate
		        ,"Ｘ","X"    ///No Translate
		        ,"Ｙ","Y"    ///No Translate
		        ,"Ｚ","Z"    ///No Translate
		        ,"ａ","a"    ///No Translate
		        ,"ｂ","b"    ///No Translate
		        ,"ｃ","c"    ///No Translate
		        ,"ｄ","d"    ///No Translate
		        ,"ｅ","e"    ///No Translate
		        ,"ｆ","f"    ///No Translate
		        ,"ｇ","g"    ///No Translate
		        ,"ｈ","h"    ///No Translate
		        ,"ｉ","i"    ///No Translate
		        ,"ｊ","j"    ///No Translate
		        ,"ｋ","k"    ///No Translate
		        ,"ｌ","l"    ///No Translate
		        ,"ｍ","m"    ///No Translate
		        ,"ｎ","n"    ///No Translate
		        ,"ｏ","o"    ///No Translate
		        ,"ｐ","p"    ///No Translate
		        ,"ｑ","q"    ///No Translate
		        ,"ｒ","r"    ///No Translate
		        ,"ｓ","s"    ///No Translate
		        ,"ｔ","t"    ///No Translate
		        ,"ｕ","u"    ///No Translate
		        ,"ｖ","v"    ///No Translate
		        ,"ｗ","w"    ///No Translate
		        ,"ｘ","x"    ///No Translate
		        ,"ｙ","y"    ///No Translate
		        ,"ｚ","z"    ///No Translate
		        ,"ｰ","ー"    ///No Translate
		        ,"‘","'"    ///No Translate
		        ,"’","'"    ///No Translate
		        ,"“","\""    ///No Translate
		        ,"”","\""    ///No Translate
		        ,"（","("    ///No Translate
		        ,"）",")"    ///No Translate
		        ,"〔","["    ///No Translate
		        ,"〕","]"    ///No Translate
		        ,"［","["    ///No Translate
		        ,"］","]"    ///No Translate
		        ,"｛","{"    ///No Translate
		        ,"｝","}"    ///No Translate
		        ,"〈","<"    ///No Translate
		        ,"〉",">"    ///No Translate
		        ,"《","<"    ///No Translate
		        ,"》",">"    ///No Translate
		        ,"「","{"    ///No Translate
		        ,"」","}"    ///No Translate
		        ,"『","{"    ///No Translate
		        ,"』","}"    ///No Translate
		        ,"【","["    ///No Translate
		        ,"】","]"    ///No Translate
		        ,"・","･"    ///No Translate
		        ,"！","!"    ///No Translate
		        ,"♯","#"    ///No Translate
		        ,"＆","&"    ///No Translate
		        ,"＄","$"    ///No Translate
		        ,"？","?"    ///No Translate
		        ,"：",":"    ///No Translate
		        ,"；",";"    ///No Translate
		        ,"／","/"    ///No Translate
		        ,"＼","\\"    ///No Translate
		        ,"＠","@"    ///No Translate
		        ,"｜","|"    ///No Translate
		        ,"－","-"    ///No Translate
		        ,"＝","="    ///No Translate
		        ,"≒","="    ///No Translate
		        ,"％","%"    ///No Translate
		        ,"＋","+"    ///No Translate
		        ,"－","-"    ///No Translate
		        ,"÷","/"    ///No Translate
		        ,"＊","*"    ///No Translate
		        ,"～","~"     ///No Translate
                //UTF-8だと別の～もあるから判断が難しい・・・
	        };
        static string[] replaceTableNum = new string[]{
		         "１","1"    ///No Translate
		        ,"２","2"    ///No Translate
		        ,"３","3"    ///No Translate
		        ,"４","4"    ///No Translate
		        ,"５","5"    ///No Translate
		        ,"６","6"    ///No Translate
		        ,"７","7"    ///No Translate
		        ,"８","8"    ///No Translate
		        ,"９","9"    ///No Translate
		        ,"０","0"    ///No Translate
	        };
        static string[] replaceTableHankanaToHiragana = new string[]{
		         "ｳﾞ","う゛"    ///No Translate
		        ,"ｶﾞ","が"    ///No Translate
		        ,"ｷﾞ","ぎ"    ///No Translate
		        ,"ｸﾞ","ぐ"    ///No Translate
		        ,"ｹﾞ","げ"    ///No Translate
		        ,"ｺﾞ","ご"    ///No Translate
		        ,"ｻﾞ","ざ"    ///No Translate
		        ,"ｼﾞ","じ"    ///No Translate
		        ,"ｽﾞ","ず"    ///No Translate
		        ,"ｾﾞ","ぜ"    ///No Translate
		        ,"ｿﾞ","ぞ"    ///No Translate
		        ,"ﾀﾞ","だ"    ///No Translate
		        ,"ﾁﾞ","ぢ"    ///No Translate
		        ,"ﾂﾞ","づ"    ///No Translate
		        ,"ｾﾞ","ぜ"    ///No Translate
		        ,"ｿﾞ","ぞ"    ///No Translate
		        ,"ﾊﾞ","ば"    ///No Translate
		        ,"ﾋﾞ","び"    ///No Translate
		        ,"ﾌﾞ","ぶ"    ///No Translate
		        ,"ﾍﾞ","べ"    ///No Translate
		        ,"ﾎﾞ","ぼ"    ///No Translate
		        ,"ﾊﾟ","ぱ"    ///No Translate
		        ,"ﾋﾟ","ぴ"    ///No Translate
		        ,"ﾌﾟ","ぷ"    ///No Translate
		        ,"ﾍﾟ","ぺ"    ///No Translate
		        ,"ﾎﾟ","ぽ"    ///No Translate
		        ,"ｱ","あ"    ///No Translate
		        ,"ｲ","い"    ///No Translate
		        ,"ｳ","う"    ///No Translate
		        ,"ｴ","え"    ///No Translate
		        ,"ｵ","お"    ///No Translate
		        ,"ｶ","か"    ///No Translate
		        ,"ｷ","き"    ///No Translate
		        ,"ｸ","く"    ///No Translate
		        ,"ｹ","け"    ///No Translate
		        ,"ｺ","こ"    ///No Translate
		        ,"ｻ","さ"    ///No Translate
		        ,"ｼ","し"    ///No Translate
		        ,"ｽ","す"    ///No Translate
		        ,"ｾ","せ"    ///No Translate
		        ,"ｿ","そ"    ///No Translate
		        ,"ﾀ","た"    ///No Translate
		        ,"ﾁ","ち"    ///No Translate
		        ,"ﾂ","つ"    ///No Translate
		        ,"ﾃ","て"    ///No Translate
		        ,"ﾄ","と"    ///No Translate
		        ,"ﾅ","な"    ///No Translate
		        ,"ﾆ","に"    ///No Translate
		        ,"ﾇ","ぬ"    ///No Translate
		        ,"ﾈ","ね"    ///No Translate
		        ,"ﾉ","の"    ///No Translate
		        ,"ﾊ","は"    ///No Translate
		        ,"ﾋ","ひ"    ///No Translate
		        ,"ﾌ","ふ"    ///No Translate
		        ,"ﾍ","へ"    ///No Translate
		        ,"ﾎ","ほ"    ///No Translate
		        ,"ﾏ","ま"    ///No Translate
		        ,"ﾐ","み"    ///No Translate
		        ,"ﾑ","む"    ///No Translate
		        ,"ﾒ","め"    ///No Translate
		        ,"ﾓ","も"    ///No Translate
		        ,"ﾔ","や"    ///No Translate
		        ,"ﾕ","ゆ"    ///No Translate
		        ,"ﾖ","よ"    ///No Translate
		        ,"ﾗ","ら"    ///No Translate
		        ,"ﾘ","り"    ///No Translate
		        ,"ﾙ","る"    ///No Translate
		        ,"ﾚ","れ"    ///No Translate
		        ,"ﾛ","ろ"    ///No Translate
		        ,"ｦ","を"    ///No Translate
		        ,"ﾜ","わ"    ///No Translate
		        ,"ﾝ","ん"    ///No Translate
		        ,"ｧ","ぁ"    ///No Translate
		        ,"ｨ","ぃ"    ///No Translate
		        ,"ｩ","ぅ"    ///No Translate
		        ,"ｪ","ぇ"    ///No Translate
		        ,"ｫ","ぉ"    ///No Translate
		        ,"ｬ","ゃ"    ///No Translate
		        ,"ｭ","ゅ"    ///No Translate
		        ,"ｮ","ょ"    ///No Translate
		        ,"ｯ","っ"    ///No Translate
		        ,"ｰ","ー"    ///No Translate
	        };
        static string[] replaceTableHankanaToKatakana = new string[]{
		         "ｳﾞ","ヴ"    ///No Translate
		        ,"ｶﾞ","ガ"    ///No Translate
		        ,"ｷﾞ","ギ"    ///No Translate
		        ,"ｸﾞ","グ"    ///No Translate
		        ,"ｹﾞ","ゲ"    ///No Translate
		        ,"ｺﾞ","ゴ"    ///No Translate
		        ,"ｻﾞ","ザ"    ///No Translate
		        ,"ｼﾞ","ジ"    ///No Translate
		        ,"ｽﾞ","ズ"    ///No Translate
		        ,"ｾﾞ","ゼ"    ///No Translate
		        ,"ｿﾞ","ゾ"    ///No Translate
		        ,"ﾀﾞ","ダ"    ///No Translate
		        ,"ﾁﾞ","ヂ"    ///No Translate
		        ,"ﾂﾞ","ヅ"    ///No Translate
		        ,"ｾﾞ","ゼ"    ///No Translate
		        ,"ｿﾞ","ゾ"    ///No Translate
		        ,"ﾊﾞ","バ"    ///No Translate
		        ,"ﾋﾞ","ビ"    ///No Translate
		        ,"ﾌﾞ","ブ"    ///No Translate
		        ,"ﾍﾞ","ベ"    ///No Translate
		        ,"ﾎﾞ","ボ"    ///No Translate
		        ,"ﾊﾟ","パ"    ///No Translate
		        ,"ﾋﾟ","ピ"    ///No Translate
		        ,"ﾌﾟ","プ"    ///No Translate
		        ,"ﾍﾟ","ペ"    ///No Translate
		        ,"ﾎﾟ","ポ"    ///No Translate
		        ,"ｱ","ア"    ///No Translate
		        ,"ｲ","イ"    ///No Translate
		        ,"ｳ","ウ"    ///No Translate
		        ,"ｴ","エ"    ///No Translate
		        ,"ｵ","オ"    ///No Translate
		        ,"ｶ","カ"    ///No Translate
		        ,"ｷ","キ"    ///No Translate
		        ,"ｸ","ク"    ///No Translate
		        ,"ｹ","ケ"    ///No Translate
		        ,"ｺ","コ"    ///No Translate
		        ,"ｻ","サ"    ///No Translate
		        ,"ｼ","シ"    ///No Translate
		        ,"ｽ","ス"    ///No Translate
		        ,"ｾ","セ"    ///No Translate
		        ,"ｿ","ソ"    ///No Translate
		        ,"ﾀ","タ"    ///No Translate
		        ,"ﾁ","チ"    ///No Translate
		        ,"ﾂ","ツ"    ///No Translate
		        ,"ﾃ","テ"    ///No Translate
		        ,"ﾄ","ト"    ///No Translate
		        ,"ﾅ","ナ"    ///No Translate
		        ,"ﾆ","ニ"    ///No Translate
		        ,"ﾇ","ヌ"    ///No Translate
		        ,"ﾈ","ネ"    ///No Translate
		        ,"ﾉ","ノ"    ///No Translate
		        ,"ﾊ","ハ"    ///No Translate
		        ,"ﾋ","ヒ"    ///No Translate
		        ,"ﾌ","フ"    ///No Translate
		        ,"ﾍ","ヘ"    ///No Translate
		        ,"ﾎ","ホ"    ///No Translate
		        ,"ﾏ","マ"    ///No Translate
		        ,"ﾐ","ミ"    ///No Translate
		        ,"ﾑ","ム"    ///No Translate
		        ,"ﾒ","メ"    ///No Translate
		        ,"ﾓ","モ"    ///No Translate
		        ,"ﾔ","ヤ"    ///No Translate
		        ,"ﾕ","ユ"    ///No Translate
		        ,"ﾖ","ヨ"    ///No Translate
		        ,"ﾗ","リ"    ///No Translate
		        ,"ﾘ","リ"    ///No Translate
		        ,"ﾙ","ル"    ///No Translate
		        ,"ﾚ","レ"    ///No Translate
		        ,"ﾛ","ロ"    ///No Translate
		        ,"ｦ","ヲ"    ///No Translate
		        ,"ﾜ","ワ"    ///No Translate
		        ,"ﾝ","ン"    ///No Translate
		        ,"ｧ","ァ"    ///No Translate
		        ,"ｨ","ィ"    ///No Translate
		        ,"ｩ","ゥ"    ///No Translate
		        ,"ｪ","ェ"    ///No Translate
		        ,"ｫ","ォ"    ///No Translate
		        ,"ｬ","ャ"    ///No Translate
		        ,"ｭ","ュ"    ///No Translate
		        ,"ｮ","ョ"    ///No Translate
		        ,"ｯ","ッ"    ///No Translate
		        ,"ｰ","ー"    ///No Translate
	        };
        static string[] replaceTableKatakanaToHiragana = new string[]{
		         "ヴ","う゛"    ///No Translate
		        ,"ア","あ"    ///No Translate
		        ,"イ","い"    ///No Translate
		        ,"ウ","う"    ///No Translate
		        ,"エ","え"    ///No Translate
		        ,"オ","お"    ///No Translate
		        ,"カ","か"    ///No Translate
		        ,"キ","き"    ///No Translate
		        ,"ク","く"    ///No Translate
		        ,"ケ","け"    ///No Translate
		        ,"コ","こ"    ///No Translate
		        ,"サ","さ"    ///No Translate
		        ,"シ","し"    ///No Translate
		        ,"ス","す"    ///No Translate
		        ,"セ","せ"    ///No Translate
		        ,"ソ","そ"    ///No Translate
		        ,"タ","た"    ///No Translate
		        ,"チ","ち"    ///No Translate
		        ,"ツ","つ"    ///No Translate
		        ,"テ","て"    ///No Translate
		        ,"ト","と"    ///No Translate
		        ,"ナ","な"    ///No Translate
		        ,"ニ","に"    ///No Translate
		        ,"ヌ","ぬ"    ///No Translate
		        ,"ネ","ね"    ///No Translate
		        ,"ノ","の"    ///No Translate
		        ,"ハ","は"    ///No Translate
		        ,"ヒ","ひ"    ///No Translate
		        ,"フ","ふ"    ///No Translate
		        ,"ヘ","へ"    ///No Translate
		        ,"ホ","ほ"    ///No Translate
		        ,"マ","ま"    ///No Translate
		        ,"ミ","み"    ///No Translate
		        ,"ム","む"    ///No Translate
		        ,"メ","め"    ///No Translate
		        ,"モ","も"    ///No Translate
		        ,"ヤ","や"    ///No Translate
		        ,"ユ","ゆ"    ///No Translate
		        ,"ヨ","よ"    ///No Translate
		        ,"ラ","ら"    ///No Translate
		        ,"リ","り"    ///No Translate
		        ,"ル","る"    ///No Translate
		        ,"レ","れ"    ///No Translate
		        ,"ロ","ろ"    ///No Translate
		        ,"ワ","わ"    ///No Translate
		        ,"ヲ","を"    ///No Translate
		        ,"ン","ん"    ///No Translate
		        ,"ァ","ぁ"    ///No Translate
		        ,"ィ","ぃ"    ///No Translate
		        ,"ゥ","ぅ"    ///No Translate
		        ,"ェ","ぇ"    ///No Translate
		        ,"ォ","ぉ"    ///No Translate
		        ,"ガ","が"    ///No Translate
		        ,"ギ","ぎ"    ///No Translate
		        ,"グ","ぐ"    ///No Translate
		        ,"ゲ","げ"    ///No Translate
		        ,"ゴ","ご"    ///No Translate
		        ,"ザ","ざ"    ///No Translate
		        ,"ジ","じ"    ///No Translate
		        ,"ズ","ず"    ///No Translate
		        ,"ゼ","ぜ"    ///No Translate
		        ,"ゾ","ぞ"    ///No Translate
		        ,"ダ","だ"    ///No Translate
		        ,"ヂ","ぢ"    ///No Translate
		        ,"ヅ","づ"    ///No Translate
		        ,"デ","で"    ///No Translate
		        ,"ド","ど"    ///No Translate
		        ,"バ","ば"    ///No Translate
		        ,"ビ","び"    ///No Translate
		        ,"ブ","ぶ"    ///No Translate
		        ,"ベ","べ"    ///No Translate
		        ,"ボ","ぼ"    ///No Translate
		        ,"パ","ぱ"    ///No Translate
		        ,"ピ","ぴ"    ///No Translate
		        ,"プ","ぷ"    ///No Translate
		        ,"ペ","ぺ"    ///No Translate
		        ,"ポ","ぽ"    ///No Translate
		        ,"ャ","ゃ"    ///No Translate
		        ,"ュ","ゅ"    ///No Translate
		        ,"ョ","ょ"    ///No Translate
		        ,"ッ","っ"    ///No Translate
		        ,"ヮ","ゎ"    ///No Translate
	        };
        //拡張typo修正
        //r	 「ローマ字」を「ひらがな」に変換します。
        //R	 「ひらがな」を「ローマ字」に変換します。
        static string[] replaceTableRomaNobasu = new string[]{
		         "aa","aー"    ///No Translate
		        ,"ii","iー"    ///No Translate
		        ,"uu","uー"    ///No Translate
		        ,"ee","eー"    ///No Translate
		        ,"oo","oー"    ///No Translate
		        ,"","ー"    ///No Translate
	        };
        static string[] replaceTableRoma = new string[]{
		         "tsu","っ"    ///No Translate
		        ,"match","まっ"    ///No Translate
		        ,"ltsu","っ"    ///No Translate
		        ,"u","う"    ///No Translate
		        ,"lyi","ぃ"    ///No Translate
		        ,"xyi","ぃ"    ///No Translate
		        ,"lye","ぇ"    ///No Translate
		        ,"xye","ぇ"    ///No Translate
		        ,"wha","あぁ"    ///No Translate
		        ,"wha","うぁ"    ///No Translate
		        ,"whi","うぃ"    ///No Translate
		        ,"whe","うぇ"    ///No Translate
		        ,"who","うぉ"    ///No Translate
		        ,"kyi","きぃ"    ///No Translate
		        ,"kye","きぇ"    ///No Translate
		        ,"kya","きゃ"    ///No Translate
		        ,"kyu","きゅ"    ///No Translate
		        ,"kyo","きょ"    ///No Translate
		        ,"kwa","くぁ"    ///No Translate
		        ,"qwa","くぁ"    ///No Translate
		        ,"qwi","くぃ"    ///No Translate
		        ,"qyi","くぃ"    ///No Translate
		        ,"qwu","くぅ"    ///No Translate
		        ,"qwe","くぇ"    ///No Translate
		        ,"qye","くぇ"    ///No Translate
		        ,"qwo","くぉ"    ///No Translate
		        ,"qya","くゃ"    ///No Translate
		        ,"qyu","くゅ"    ///No Translate
		        ,"qyo","くょ"    ///No Translate
		        ,"syi","しぃ"    ///No Translate
		        ,"swi","しぇ"    ///No Translate
		        ,"sha","しゃ"    ///No Translate
		        ,"shu","しゅ"    ///No Translate
		        ,"sho","しょ"    ///No Translate
		        ,"syi","しぇ"    ///No Translate
		        ,"sya","しゃ"    ///No Translate
		        ,"syu","しゅ"    ///No Translate
		        ,"syo","しょ"    ///No Translate
		        ,"si","し"    ///No Translate
		        ,"shi","し"    ///No Translate
		        ,"swa","すぁ"    ///No Translate
		        ,"swi","すぃ"    ///No Translate
		        ,"swu","すぅ"    ///No Translate
		        ,"swe","すぇ"    ///No Translate
		        ,"swo","すぉ"    ///No Translate
		        ,"cyi","ちぃ"    ///No Translate
		        ,"tyi","ちぃ"    ///No Translate
		        ,"che","ちぇ"    ///No Translate
		        ,"cye","ちぇ"    ///No Translate
		        ,"tye","ちぇ"    ///No Translate
		        ,"cha","ちゃ"    ///No Translate
		        ,"cya","ちゃ"    ///No Translate
		        ,"tya","ちゃ"    ///No Translate
		        ,"chu","ちゅ"    ///No Translate
		        ,"cyu","ちゅ"    ///No Translate
		        ,"tyu","ちゅ"    ///No Translate
		        ,"cho","ちょ"    ///No Translate
		        ,"cyo","ちょ"    ///No Translate
		        ,"tyo","ちょ"    ///No Translate
		        ,"ci","ち"    ///No Translate
		        ,"chi","ち"    ///No Translate
		        ,"tsa","つぁ"    ///No Translate
		        ,"tsi","つぃ"    ///No Translate
		        ,"tse","つぇ"    ///No Translate
		        ,"tso","つぉ"    ///No Translate
		        ,"tu","つ"    ///No Translate
		        ,"tsu","つ"    ///No Translate
		        ,"ltu","っ"    ///No Translate
		        ,"xtu","っ"    ///No Translate
		        ,"thi","てぃ"    ///No Translate
		        ,"the","てぇ"    ///No Translate
		        ,"tha","てゃ"    ///No Translate
		        ,"thu","てゅ"    ///No Translate
		        ,"tho","てょ"    ///No Translate
		        ,"twa","とぁ"    ///No Translate
		        ,"twi","とぃ"    ///No Translate
		        ,"twu","とぅ"    ///No Translate
		        ,"twe","とぇ"    ///No Translate
		        ,"two","とぉ"    ///No Translate
		        ,"nyi","にぃ"    ///No Translate
		        ,"nye","にぇ"    ///No Translate
		        ,"nya","にゃ"    ///No Translate
		        ,"nyu","にゅ"    ///No Translate
		        ,"nyo","にょ"    ///No Translate
		        ,"hyi","ひぃ"    ///No Translate
		        ,"hye","ひぇ"    ///No Translate
		        ,"hya","ひゃ"    ///No Translate
		        ,"hyu","ひゅ"    ///No Translate
		        ,"hyo","ひょ"    ///No Translate
		        ,"fwa","ふぁ"    ///No Translate
		        ,"fwi","ふぃ"    ///No Translate
		        ,"fyi","ふぃ"    ///No Translate
		        ,"fwu","ふぅ"    ///No Translate
		        ,"few","ふぇ"    ///No Translate
		        ,"fye","ふぇ"    ///No Translate
		        ,"fwo","ふぉ"    ///No Translate
		        ,"fya","ふゃ"    ///No Translate
		        ,"fyu","ふゅ"    ///No Translate
		        ,"fyo","ふょ"    ///No Translate
		        ,"myi","みぃ"    ///No Translate
		        ,"mye","みぇ"    ///No Translate
		        ,"mya","みゃ"    ///No Translate
		        ,"myu","みゅ"    ///No Translate
		        ,"myo","みょ"    ///No Translate
		        ,"lya","ゃ"    ///No Translate
		        ,"xya","ゃ"    ///No Translate
		        ,"lyu","ゅ"    ///No Translate
		        ,"xyu","ゅ"    ///No Translate
		        ,"lyo","ょ"    ///No Translate
		        ,"xyo","ょ"    ///No Translate
		        ,"ryi","りぃ"    ///No Translate
		        ,"rye","りぇ"    ///No Translate
		        ,"rya","りゃ"    ///No Translate
		        ,"ryu","りゅ"    ///No Translate
		        ,"ryo","りょ"    ///No Translate
		        ,"gyi","ぎぃ"    ///No Translate
		        ,"gye","ぎぇ"    ///No Translate
		        ,"gya","ぎゃ"    ///No Translate
		        ,"gyu","ぎゅ"    ///No Translate
		        ,"gyo","ぎょ"    ///No Translate
		        ,"gwa","ぐぁ"    ///No Translate
		        ,"gwi","ぐぃ"    ///No Translate
		        ,"gwu","ぐぅ"    ///No Translate
		        ,"gwe","ぐぇ"    ///No Translate
		        ,"gwo","ぐぉ"    ///No Translate
		        ,"jyi","じぃ"    ///No Translate
		        ,"zyi","じぃ"    ///No Translate
		        ,"jye","じぇ"    ///No Translate
		        ,"zye","じぇ"    ///No Translate
		        ,"jya","じゃ"    ///No Translate
		        ,"zya","じゃ"    ///No Translate
		        ,"lwa","ゎ"    ///No Translate
		        ,"xwa","ゎ"    ///No Translate
		        ,"jyu","じゅ"    ///No Translate
		        ,"zyu","じゅ"    ///No Translate
		        ,"jyo","じょ"    ///No Translate
		        ,"zyo","じょ"    ///No Translate
		        ,"dyi","ぢぃ"    ///No Translate
		        ,"dye","ぢぇ"    ///No Translate
		        ,"dya","ぢゃ"    ///No Translate
		        ,"dyu","ぢゅ"    ///No Translate
		        ,"dyo","ぢょ"    ///No Translate
		        ,"dhi","でぃ"    ///No Translate
		        ,"dhe","でぇ"    ///No Translate
		        ,"dha","でゃ"    ///No Translate
		        ,"dhu","でゅ"    ///No Translate
		        ,"dho","でょ"    ///No Translate
		        ,"dwa","どぁ"    ///No Translate
		        ,"dwi","どぃ"    ///No Translate
		        ,"dwu","どぅ"    ///No Translate
		        ,"dwe","どぇ"    ///No Translate
		        ,"dwo","どぉ"    ///No Translate
		        ,"byi","びぃ"    ///No Translate
		        ,"bye","びぇ"    ///No Translate
		        ,"bya","びょ"    ///No Translate
		        ,"byu","びゅ"    ///No Translate
		        ,"byo","びょ"    ///No Translate
		        ,"pyi","ぴぃ"    ///No Translate
		        ,"pye","ぴぇ"    ///No Translate
		        ,"pya","ぴゃ"    ///No Translate
		        ,"pyu","ぴゅ"    ///No Translate
		        ,"pyo","ぴょ"    ///No Translate
		        ,"vyi","う゛ぃ"    ///No Translate
		        ,"vye","う゛ぇ"    ///No Translate
		        ,"vya","う゛ゃ"    ///No Translate
		        ,"vyu","う゛ゅ"    ///No Translate
		        ,"vyo","う゛ょ"    ///No Translate
		        ,"wu","う"    ///No Translate
		        ,"la","ぁ"    ///No Translate
		        ,"li","ぃ"    ///No Translate
		        ,"xi","ぃ"    ///No Translate
		        ,"lu","ぅ"    ///No Translate
		        ,"xu","ぅ"    ///No Translate
		        ,"le","ぇ"    ///No Translate
		        ,"xe","ぇ"    ///No Translate
		        ,"lo","ぉ"    ///No Translate
		        ,"xo","ぉ"    ///No Translate
		        ,"ye","いぇ"    ///No Translate
		        ,"ka","か"    ///No Translate
		        ,"ca","か"    ///No Translate
		        ,"lka","か" //ヵ    ///No Translate
		        ,"xka","か" //ヵ    ///No Translate
		        ,"ki","き"    ///No Translate
		        ,"qa","くぁ"    ///No Translate
		        ,"qi","くぃ"    ///No Translate
		        ,"qe","くぇ"    ///No Translate
		        ,"qo","くぉ"    ///No Translate
		        ,"ku","く"    ///No Translate
		        ,"cu","く"    ///No Translate
		        ,"qu","く"    ///No Translate
		        ,"ke","け"    ///No Translate
		        ,"lke","け" //ヶ    ///No Translate
		        ,"xke","け" //ヶ    ///No Translate
		        ,"ko","こ"    ///No Translate
		        ,"co","こ"    ///No Translate
		        ,"sa","さ"    ///No Translate
		        ,"si","し"    ///No Translate
		        ,"ci","し"    ///No Translate
		        ,"su","す"    ///No Translate
		        ,"se","せ"    ///No Translate
		        ,"ce","せ"    ///No Translate
		        ,"so","そ"    ///No Translate
		        ,"ta","た"    ///No Translate
		        ,"ti","ち"    ///No Translate
		        ,"tu","つ"    ///No Translate
		        ,"te","て"    ///No Translate
		        ,"to","と"    ///No Translate
		        ,"na","な"    ///No Translate
		        ,"ni","に"    ///No Translate
		        ,"nu","ぬ"    ///No Translate
		        ,"ne","ね"    ///No Translate
		        ,"no","の"    ///No Translate
		        ,"ha","は"    ///No Translate
		        ,"hi","ひ"    ///No Translate
		        ,"fa","ふぁ"    ///No Translate
		        ,"fa","ふぁ"    ///No Translate
		        ,"fi","ふぃ"    ///No Translate
		        ,"fe","ふぇ"    ///No Translate
		        ,"fo","ふぉ"    ///No Translate
		        ,"hu","ふ"    ///No Translate
		        ,"fu","ふ"    ///No Translate
		        ,"he","へ"    ///No Translate
		        ,"ho","ほ"    ///No Translate
		        ,"ma","ま"    ///No Translate
		        ,"mi","み"    ///No Translate
		        ,"mu","む"    ///No Translate
		        ,"me","め"    ///No Translate
		        ,"mo","も"    ///No Translate
		        ,"ya","や"    ///No Translate
		        ,"yu","ゆ"    ///No Translate
		        ,"yo","よ"    ///No Translate
		        ,"ra","ら"    ///No Translate
		        ,"ri","り"    ///No Translate
		        ,"ru","る"    ///No Translate
		        ,"re","れ"    ///No Translate
		        ,"wa","わ"    ///No Translate
		        ,"wo","を"    ///No Translate
		        ,"nn","ん"    ///No Translate
		        ,"xn","ん"    ///No Translate
		        ,"ga","が"    ///No Translate
		        ,"gi","ぎ"    ///No Translate
		        ,"gu","ぐ"    ///No Translate
		        ,"ge","げ"    ///No Translate
		        ,"go","ご"    ///No Translate
		        ,"za","ざ"    ///No Translate
		        ,"je","じぇ"    ///No Translate
		        ,"ja","じゃ"    ///No Translate
		        ,"ju","じゅ"    ///No Translate
		        ,"jo","じょ"    ///No Translate
		        ,"zi","じ"    ///No Translate
		        ,"ji","じ"    ///No Translate
		        ,"zu","ず"    ///No Translate
		        ,"ze","ぞ"    ///No Translate
		        ,"zo","ぞ"    ///No Translate
		        ,"da","だ"    ///No Translate
		        ,"di","ぢ"    ///No Translate
		        ,"ji","ぢ"    ///No Translate
		        ,"du","づ"    ///No Translate
		        ,"de","で"    ///No Translate
		        ,"do","ど"    ///No Translate
		        ,"ba","ば"    ///No Translate
		        ,"bi","び"    ///No Translate
		        ,"bu","ぶ"    ///No Translate
		        ,"be","べ"    ///No Translate
		        ,"bo","ぼ"    ///No Translate
		        ,"pa","ぱ"    ///No Translate
		        ,"pi","ぴ"    ///No Translate
		        ,"pu","ぷ"    ///No Translate
		        ,"pe","ぺ"    ///No Translate
		        ,"po","ぽ"    ///No Translate
		        ,"va","う゛ぁ"    ///No Translate
		        ,"vi","う゛ぃ"    ///No Translate
		        ,"ve","う゛ぇ"    ///No Translate
		        ,"vo","う゛ぉ"    ///No Translate
		        ,"vu","う゛"    ///No Translate
		        ,"a","あ"    ///No Translate
		        ,"i","い"    ///No Translate
		        ,"u","う"    ///No Translate
		        ,"e","え"    ///No Translate
		        ,"o","お"    ///No Translate
	        };

        //みんな大好き PHPのmb_convert_kanaの移植
        //n	 「全角」数字を「半角」に変換します。
        //N	 「半角」数字を「全角」に変換します。
        //a	 「全角」英数字を「半角」に変換します。
        //A	 「半角」英数字を「全角」に変換します 
        //s	 「全角」スペースを「半角」に変換します
        //S	 「半角」スペースを「全角」に変換します（U+0020 -> U+3000）。
        //k	 「全角カタカナ」を「半角カタカナ」に変換します。
        //K	 「半角カタカナ」を「全角カタカナ」に変換します。
        //h	 「全角ひらがな」を「半角カタカナ」に変換します。
        //H	 「半角カタカナ」を「全角ひらがな」に変換します。
        //c	 「全角カタカナ」を「全角ひらがな」に変換します。
        //C	 「全角ひらがな」を「全角カタカナ」に変換します。
        //拡張typo修正
        //r	 「ローマ字」を「ひらがな」に変換します。
        //R	 「ひらがな」を「ローマ字」に変換します。
        public static string mb_convert_kana(string inTarget, string option)
        {
	        string ret = inTarget;
            //r	 「全角」英字を「半角」に変換します。
            //R	 「半角」英字を「全角」に変換します。
            //a	 「全角」英数字を「半角」に変換します。
            //A	 「半角」英数字を「全角」に変換します 
	        if ( option.IndexOf("r") != -1 ||   option.IndexOf("a") != -1 )
	        {
		        ret = U.table_replace(ret ,replaceTableAplha );
	        }
	        else if ( option.IndexOf("R") != -1 ||  option.IndexOf("A") != -1 )
	        {
                ret = U.table_replace_rev(ret, replaceTableAplha);
	        }

            //n	 「全角」数字を「半角」に変換します。
            //N	 「半角」数字を「全角」に変換します。
            //a	 「全角」英数字を「半角」に変換します。
            //A	 「半角」英数字を「全角」に変換します 
	        if ( option.IndexOf("n") != -1 ||  option.IndexOf("a") != -1 )
	        {
                ret = U.table_replace(ret, replaceTableNum);
	        }
	        else if ( option.IndexOf("N") != -1 ||  option.IndexOf("A") != -1)
	        {
                ret = U.table_replace_rev(ret, replaceTableNum);
	        }

            string[] replaceTableSpace = new string[]{
		         "　"," "
	        };
            //s	 「全角」スペースを「半角」に変換します
            //S	 「半角」スペースを「全角」に変換します
	        if ( option.IndexOf("s") != -1 )
	        {
                ret = U.table_replace(ret, replaceTableSpace);
	        }
	        else if ( option.IndexOf("S") != -1)
	        {
                ret = U.table_replace_rev(ret, replaceTableSpace);
	        }

            //r	 「ローマ字」を「ひらがな」に変換します。
            if (option.IndexOf("r") != -1)
            {
                ret = U.table_replace(ret, replaceTableRoma);
            }
            
            //k	 「全角カタカナ」を「半角カタカナ」に変換します。
            //K	 「半角カタカナ」を「全角カタカナ」に変換します。
	        if ( option.IndexOf("k") != -1 )
	        {
                ret = U.table_replace_rev(ret, replaceTableHankanaToKatakana);
	        }
	        else if ( option.IndexOf("K") != -1)
	        {
                ret = U.table_replace(ret, replaceTableHankanaToKatakana);
	        }

            //c	 「全角カタカナ」を「全角ひらがな」に変換します。
            //C	 「全角ひらがな」を「全角カタカナ」に変換します。
	        if ( option.IndexOf("c") != -1 )
	        {
                ret = U.table_replace(ret, replaceTableKatakanaToHiragana);
	        }
	        else if ( option.IndexOf("C") != -1)
	        {
                ret = U.table_replace_rev(ret, replaceTableKatakanaToHiragana);
	        }

            //h	 「全角ひらがな」を「半角カタカナ」に変換します。
            //H	 「半角カタカナ」を「全角ひらがな」に変換します。
	        if ( option.IndexOf("h") != -1 )
	        {
                ret = U.table_replace_rev(ret, replaceTableHankanaToHiragana);
	        }
	        else if ( option.IndexOf("H") != -1)
	        {
                ret = U.table_replace(ret, replaceTableHankanaToHiragana);
	        }

            //R	 「ひらがな」を「ローマ字」に変換します。
            if (option.IndexOf("R") != -1)
            {
                ret = U.table_replace_rev(ret, replaceTableRoma);
                ret = U.table_replace_rev(ret, replaceTableRomaNobasu);
            }
            return ret;
        }

    
    }
}
