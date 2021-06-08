using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Net;
using System.Runtime.CompilerServices;
using System.IO.Compression;
using System.Reflection;
using System.Collections;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;

namespace FEBuilderGBA
{
    //その他、雑多なもの.
    //名前タイプするのが面倒なので Util -> U とする.
    public static class U
    {
        //見つからない.
        public const uint NOT_FOUND = 0xFFFFFFFF;

        public static string at(string[] list, uint at, string def = "")
        {
            if (at >= list.Length)
            {
                return def;
            }
            return list[(int)at];
        }
        public static uint at(uint[] list, int at, uint def = 0)
        {
            if (at >= list.Length || at < 0)
            {
                return def;
            }
            return list[at];
        }
        public static string at(string[] list, int at, string def = "")
        {
            if (at >= list.Length || at < 0)
            {
                return def;
            }
            return list[at];
        }
        public static string at(List<string> list, uint at, string def = "")
        {
            if (at >= list.Count)
            {
                return def;
            }
            return list[(int)at];
        }
        public static string at(List<string> list, int at, string def = "")
        {
            if (at >= list.Count || at < 0)
            {
                return def;
            }
            return list[at];
        }

        public static string at(Dictionary<string,string> dic, string at, string def = "")
        {
            string a;
            if (!dic.TryGetValue(at, out a))
            {
                return def;
            }
            return a;
        }
        public static string at(Dictionary<uint, string> dic, uint at, string def = "")
        {
            string a;
            if (!dic.TryGetValue(at, out a))
            {
                return def;
            }
            return a;
        }
        public static string at(Dictionary<uint, string> dic, int at, string def = "")
        {
            return U.at(dic, (uint)at, def);
        }
        public static uint at(Dictionary<uint, uint> dic, uint at, uint def = 0)
        {
            uint a;
            if (!dic.TryGetValue(at, out a))
            {
                return def;
            }
            return a;
        }
        public static Bitmap at(Dictionary<string, Bitmap> dic, string at,Bitmap def = null)
        {
            Bitmap a;
            if (!dic.TryGetValue(at, out a))
            {
                return def;
            }
            return a;
        }
        //名前長いから短い奴に
        [MethodImpl(256)]
        public static bool IsEmpty(string str)
        {
            return string.IsNullOrEmpty(str);
        }

        [MethodImpl(256)]
        public static byte at(byte[] list, int at, byte def = 0)
        {
            if (at >= list.Length)
            {
                return def;
            }
            return list[at];
        }
        [MethodImpl(256)]
        public static byte at(byte[] list, uint at, byte def = 0)
        {
            if (at >= list.Length)
            {
                return def;
            }
            return list[at];
        }

        [MethodImpl(256)]
        public static Color at(Color[] list, int at, Color def)
        {
            if (at >= list.Length)
            {
                return def;
            }
            return list[at];
        }


        public static double atof(String a)
        {
            //C#のTryParseはC言語のatoiと違い、後ろに数字以外があると false が変えるので補正する.
            for (int i = 0; i < a.Length; i++)
            {
                if (!isnum_f(a[i]))
                {
                    a = a.Substring(0, i);
                    break;
                }
            }

            IFormatProvider ifp = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");
            System.Globalization.NumberStyles ns = System.Globalization.NumberStyles.Float;

            double ret = 0;
            if (!Double.TryParse(a,ns, ifp, out ret))
            {
                return 0;
            }
            return ret;
        }
#if DEBUG
        public static void TEST_atof()
        {
            double r;
            Debug.Assert( (r=atof("1.2")) == 1.2);
            Debug.Assert( (r=atof("12")) == 12);
            Debug.Assert( (r=atof("12A")) == 12);
            Debug.Assert( (r=atof("12.2A")) == 12.2);
        }

#endif 
        public static uint atoi(String a)
        {
            //C#のTryParseはC言語のatoiと違い、後ろに数字以外があると false が変えるので補正する.
            for (int i = 0; i < a.Length; i++)
            {
                if (!isnum(a[i]))
                {
                    a = a.Substring(0, i);
                    break;
                }
            }

            int ret = 0;
            if (!int.TryParse(a, out ret))
            {
                return 0;
            }
            return (uint)ret;
        }
        public static uint atou(String a)
        {
            //C#のTryParseはC言語のatoiと違い、後ろに数字以外があると false が変えるので補正する.
            for (int i = 0; i < a.Length; i++)
            {
                if (!isnum(a[i]))
                {
                    a = a.Substring(0, i);
                    break;
                }
            }

            uint ret = 0;
            if (!uint.TryParse(a, out ret))
            {
                return 0;
            }
            return ret;
        }
#if DEBUG
        public static void TEST_atou()
        {
            {
                uint r = atou("2147483812");
                Debug.Assert(r == 2147483812);
            }
        }
#endif
        public static uint atoh(String a)
        {
            //C#のTryParseはC言語のatoiと違い、後ろに数字以外があると false が変えるので補正する.
            for (int i = 0; i < a.Length; i++)
            {
                if (!ishex(a[i]))
                {
                    a = a.Substring(0, i);
                    break;
                }
            }

            int ret = 0;
            if (!int.TryParse(a, System.Globalization.NumberStyles.HexNumber, null, out ret))
            {
                return 0;
            }
            return (uint)ret;
        }
        public static uint atoi0x(String a)
        {
            if (a.Length >= 2 && a[0] == '0' && a[1] == 'x')
            {
                return atoh(a.Substring(2));
            }
            if (a.Length >= 1 && a[0] == '$')
            {
                return atoh(a.Substring(1));
            }
            return atoi(a);
        }
        //偶数か？
        public static bool isEven(int size)
        {
            return (size & 1) == 0;
        }
        public static bool isEven(uint size)
        {
            return (size & 1) == 0;
        }

        public class FunctionalComparer<T> : IComparer<T>
        {
            private Func<T, T, int> comparer;
            public FunctionalComparer(Func<T, T, int> comparer)
            {
                this.comparer = comparer;
            }
            public int Compare(T x, T y)
            {
                return comparer(x, y);
            }
        };
        public class FunctionalComparerOne<T> : IComparer<T>
        {
            private Func<T,int> toInt;
            public FunctionalComparerOne(Func<T, int> toInt)
            {
                this.toInt = toInt;
            }
            public int Compare(T x, T y)
            {
                return toInt(x) - toInt(y);
            }
        };

        //DICソート
        public static List<KeyValuePair<TKey, TValue>> OrderBy<TKey, TValue>
            (Dictionary<TKey, TValue> dic, Func<KeyValuePair<TKey, TValue>, int> toInt)
        {
            List<KeyValuePair<TKey, TValue>> list = new List<KeyValuePair<TKey, TValue>>();
            foreach(KeyValuePair<TKey, TValue> pair in dic)
            {
                list.Add(pair);
            }
            FunctionalComparerOne<KeyValuePair<TKey, TValue>> comp
                = new FunctionalComparerOne<KeyValuePair<TKey, TValue>>(toInt);
            list.Sort(comp);
            return list;
        }

        //キー割り当てを文字列から生成
        public static bool CheckKeys(string text, out Keys keys)
        {
            if (text.Length <= 0)
            {
                keys = new Keys(); //未割り当てと怒られないために.
                return false;
            }

            try
            {
                KeysConverter c = new KeysConverter();
                keys = (Keys)(c.ConvertFrom(text));
            }
            catch (Exception e)
            {
                if (CheckKeysCtrlFixed(text, out keys))
                {
                    return true;
                }

                Log.Error(e.ToString());

//                keys = new Keys(); //未割り当てと怒られないために.
                return false;
            }
            return true;
        }

        static bool CheckKeysCtrlFixed(string text, out Keys keys)
        {
            if (text.IndexOf("Ctrl+") >= 0)
            {
                text = text.Replace("Ctrl+", U.GetCtrlKeyName() + "+");
            }
            else
            {
                keys = new Keys();
                return false;
            }

            try
            {
                KeysConverter c = new KeysConverter();
                keys = (Keys)(c.ConvertFrom(text));
            }
            catch (Exception)
            {
                keys = new Keys(); //未割り当てと怒られないために.
                return false;
            }
            return true;
        }


        //タイムスタンプのコピー
        public static void CopyTimeStamp(string srcFilename,string destFilename)
        {
#if DEBUG
#else
            try
            {
#endif
            // 作成日時
            File.SetCreationTime(destFilename, File.GetCreationTime(srcFilename));

            // 更新日時
            File.SetLastWriteTime(destFilename, File.GetLastWriteTime(srcFilename));

            // アクセス日時
            File.SetLastAccessTime(destFilename, File.GetLastAccessTime(srcFilename));
#if DEBUG
#else
            }
            catch(Exception e)
            {
               Log.Error(e.ToString());
            }
#endif
        }

        //タイムスタンプのコピー
        public static void SetTimeStamp(string filename, DateTime datetime)
        {
#if DEBUG
#else
            try
            {
#endif
            // 作成日時
            File.SetCreationTime(filename, datetime);

            // 更新日時
            File.SetLastWriteTime(filename, datetime);

            // アクセス日時
            File.SetLastAccessTime(filename, datetime);
#if DEBUG
#else
            }
            catch(Exception e)
            {
               Log.Error(e.ToString());
            }
#endif
        }

        public static string MakeFilename(string addname,string override_ext=null)
        {
            String dir = Path.GetDirectoryName(Program.ROM.Filename);
            String filename = Path.GetFileNameWithoutExtension(Program.ROM.Filename);
            String ext;
            if (override_ext == null)
            {
                if (Program.ROM.IsVirtualROM)
                {//仮想ROMの場合、拡張子がないので、便宜上 gbaをつけます.
                    ext = ".gba";
                }
                else
                {
                    ext = Path.GetExtension(Program.ROM.Filename);
                }
            }
            else
            {
                ext = override_ext;
            }

            string ret = Path.Combine(dir, filename + "." + addname + ext);
            return ret;
        }
        //外部アプリで実行するため、一時的に出力します.
        public static string WriteTempROM(string addname)
        {
            string t = MakeFilename(addname);
            Program.ROM.Save(t, true);
            return t;
        }

        public static string escape_shell_args(string str)
        {
            if (str.Length > 0 && str[str.Length - 1] == '\\')
            {//最後に \ があれば \\ として逃げる. 
                str = str + "\\ ";
            }
            str = str.Replace("\"", "\\\"");
            return '"' + str + '"';
        }

        [MethodImpl(256)]
        public static bool isPadding4(uint a)
        {
            return a % 4 == 0;
        }
        public static uint SubPadding4(uint p)
        {
            uint mod = p % 4;
            if (mod == 0)
            {
                return p;
            }
            else
            {
                return p - mod;
            }
        }


        [MethodImpl(256)]
        public static uint Padding2(uint p)
        {
            if ((p & 0x01) == 0x01)
            {
                return p + 1;
            }
            return p;
        }
        [MethodImpl(256)]
        public static uint Padding2Before(uint p)
        {
            if ((p & 0x01) == 0x01)
            {
                return p - 1;
            }
            return p;
        }

        [MethodImpl(256)]
        public static uint Padding4(uint p)
        {
            uint mod = p % 4;
            if (mod == 0)
            {
                return p;
            }
            else
            {
                return p + (4 - mod);
            }
        }

        [MethodImpl(256)]
        public static int Padding4(int p)
        {
            int mod = p % 4;
            if (mod == 0)
            {
                return p;
            }
            else
            {
                return p + (4 - mod);
            }
        }
        public static uint Padding8(uint p)
        {
            uint mod = p % 8;
            if (mod == 0)
            {
                return p;
            }
            else
            {
                return p + (8 - mod);
            }
        }
        public static int Padding8(int p)
        {
            int mod = p % 8;
            if (mod == 0)
            {
                return p;
            }
            else
            {
                return p + (8 - mod);
            }
        }

        public static uint Padding16(uint p)
        {
            uint mod = p % 16;
            if (mod == 0)
            {
                return p;
            }
            else
            {
                return p + (16 - mod);
            }
        }
        public static int Padding16(int p)
        {
            int mod = p % 16;
            if (mod == 0)
            {
                return p;
            }
            else
            {
                return p + (16 - mod);
            }
        }
        public static byte[] FillArray(uint size, byte fill)
        {
            byte[] b = new byte[size];
            for (int i = 0; i < size; i++)
            {
                b[i] = fill;
            }
            return b;
        }
        public static void write_fill(byte[] data, uint addr, uint length, byte fill = 0x00)
        {
            for (uint i = 0; i < length; i++)
            {
                data[addr + i] = fill;
            }
        }

        public static bool isalhpa(byte a)
        {
            return isalhpa((char)a);
        }
        public static bool isalhpa(char a)
        {
            return ((a >= 'a' && a <= 'z')
                || (a >= 'A' && a <= 'Z')
                );
        }
        public static bool isalhpanum(byte a)
        {
            return isalhpanum((char)a);
        }
        public static bool isalhpanum(char a)
        {
            return (a >= 'a' && a <= 'z')
                || (a >= 'A' && a <= 'Z')
                || (a >= '0' && a <= '9')
                ;
        }
        public static bool isnum_f(byte a)
        {
            return isnum_f((char)a);
        }
        public static bool isnum_f(char a)
        {
            return ((a >= '0' && a <= '9') || a == '.');
        }
        public static bool isnum(byte a)
        {
            return isnum((char)a);
        }
        public static bool isnum(char a)
        {
            return (a >= '0' && a <= '9');
        }
        public static bool ishex(byte a)
        {
            return ishex((char)a);
        }
        public static bool ishex(char a)
        {
            return (a >= '0' && a <= '9') || (a >= 'a' && a <= 'f') || (a >= 'A' && a <= 'F');
        }
        public static bool isAlphaNumString(string str)
        {
            for(int i = 0 ; i < str.Length ; i ++ )
            {
                if (!
                    ((str[i] >= '0' && str[i] <= '9' )
                   ||(str[i] >= 'a' && str[i] <= 'z' )
                   ||(str[i] >= 'A' && str[i] <= 'Z' )
                   ||(str[i] == '\0')
                    ))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool isAsciiString(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (c >= 0x7f)
                {
                    return false;
                }
                if (c >= 0x01 && c <= 0x1f)
                {
                    if (c == 0x09 || c == 0x0a || c == 0x0d)
                    {//タブと改行を除く
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public static bool isAlphaString(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!
                     ((str[i] >= 'a' && str[i] <= 'z')
                   || (str[i] >= 'A' && str[i] <= 'Z')
                   || (str[i] == '\0')
                    ))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool isJAString(string str)
        {
            bool r = RegexCache.IsMatch(str, @"^[\p{IsHiragana}|\p{IsKatakana}|\p{IsCJKUnifiedIdeographs}]+$");
            return r;
        }
        public static bool isHexString(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!
                    ((str[i] >= '0' && str[i] <= '9')
                   || (str[i] >= 'a' && str[i] <= 'f')
                   || (str[i] >= 'A' && str[i] <= 'F')
                   || (str[i] == '\0')
                    ))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool isNumString(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!
                    ((str[i] >= '0' && str[i] <= '9')
                   || (str[i] == '\0')
                    ))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool isAscii(byte a)
        {
            return (a >= 0x20 && a <= 0x7e);
        }

        public static string ToHexString(decimal a)
        {
            return ToHexString((uint)a);
        }
        public static string ToHexString(int a)
        {
            if (a <= 0xff)
            {
                return a.ToString("X02");
            }
            if (a <= 0xffff)
            {
                return a.ToString("X04");
            }
            if (a <= 0x7fffffff)
            {
                return a.ToString("X08");
            }
            return "???";
        }
        public static string ToHexString8(int a)
        {
            return a.ToString("X08");
        }
        public static string ToHexString8(uint a)
        {
            return a.ToString("X08");
        }
        public static string ToHexString4(int a)
        {
            return a.ToString("X04");
        }
        public static string ToHexString4(uint a)
        {
            return a.ToString("X04");
        }
        public static string ToHexString2(int a)
        {
            return a.ToString("X02");
        }
        public static string ToHexString2(uint a)
        {
            return a.ToString("X02");
        }

        public static string To0xHexString(uint a)
        {
            return "0x" + ToHexString(a);
        }
        public static string To0xHexString(int a)
        {
            return "0x" + ToHexString(a);
        }
        public static string ToHexString(uint a)
        {
            if (a <= 0xff)
            {
                return a.ToString("X02");
            }
            if (a <= 0xffff)
            {
                return a.ToString("X04");
            }
            if (a <= 0xffffff)
            {
                return a.ToString("X06");
            }
            if (a <= 0xffffffff)
            {
                return a.ToString("X08");
            }
            return "???";
        }

        public class AddrResult
        {
            public uint addr;
            public string name;
            public uint tag;

            public bool isNULL()
            {
                return addr == 0 || name == null;
            }
            public AddrResult(uint addr, string name)
            {
                this.addr = addr;
                this.name = name;
            }
            public AddrResult(uint addr, string name,uint tag)
            {
                this.addr = addr;
                this.name = name;
                this.tag = tag;
            }
            public AddrResult()
            {
            }
        };

        public static void ConvertListBox(List<U.AddrResult> list, ref ListBoxEx listbox)
        {
            listbox.BeginUpdate();
            listbox.Items.Clear();
            foreach (U.AddrResult ar in list)
            {
                if (!ar.isNULL())
                {
                    listbox.Items.Add(ar.name);
                }
            }
            listbox.Tag = list;
            listbox.EndUpdate();
        }
        public static void ConvertComboBox(List<U.AddrResult> list,ref ComboBox listbox)
        {
            listbox.BeginUpdate();
            listbox.Items.Clear();
            foreach (U.AddrResult ar in list)
            {
                if (!ar.isNULL())
                {
                    listbox.Items.Add(ar.name);
                }
            }
            listbox.Tag = list;
            listbox.EndUpdate();
        }
        public static void ConvertComboBox(Dictionary<uint,string> list, ref ComboBox listbox, bool withNumber)
        {
            listbox.BeginUpdate();
            listbox.Items.Clear();
            if (withNumber)
            {
                foreach (var pair in list)
                {
                    listbox.Items.Add(U.ToHexString(pair.Key) + "=" + pair.Value);
                }
            }
            else
            {
                foreach (var pair in list)
                {
                    listbox.Items.Add(pair.Value);
                }
            }

            listbox.EndUpdate();
        }
        public static uint FindList(List<U.AddrResult> list, uint addr)
        {
            int max = list.Count;
            for (int i = 0; i < max; i++)
            {
                if (list[i].addr == addr)
                {
                    return (uint)i;
                }
            }
            return U.NOT_FOUND;
        }
        public static uint FindList(List<U.AddrResult> list, string name)
        {
            int max = list.Count;
            for (int i = 0; i < max; i++)
            {
                if (list[i].name == name)
                {
                    return (uint)i;
                }
            }
            return U.NOT_FOUND;
        }
        public static uint FindList(ComboBox list, string name)
        {
            int max = list.Items.Count;
            for (int i = 0; i < max; i++)
            {
                string a = list.Items[i].ToString();
                if (a.IndexOf(name) >= 0)
                {
                    return (uint)i;
                }
            }
            return U.NOT_FOUND;
        }
        public static uint FindList(ListBox list, string name)
        {
            int max = list.Items.Count;
            for (int i = 0; i < max; i++)
            {
                string a = list.Items[i].ToString();
                if (a.IndexOf(name) >= 0)
                {
                    return (uint)i;
                }
            }
            return U.NOT_FOUND;
        }
        public static bool FindListAndSelect(ComboBox list, string name)
        {
            uint id = FindList(list, name);
            if (id == U.NOT_FOUND)
            {
                list.SelectedIndex = -1;
                return false;
            }
            U.SelectedIndexSafety(list, id);
            return true;
        }
        public static bool FindListAndSelect(ListBox list, string name)
        {
            uint id = FindList(list, name);
            if (id == U.NOT_FOUND)
            {
                list.SelectedIndex = -1;
                return false;
            }
            U.SelectedIndexSafety(list, id);
            return true;
        }
        public static uint FindComboSelectHexFromValueWhereName(ComboBox list, string name)
        {
            int max = list.Items.Count;
            for (int i = 0; i < max; i++)
            {
                string a = list.Items[i].ToString();
                string[] sp = a.Split('=');
                if (sp.Length >= 2)
                {
                    if (sp[1] == name)
                    {
                        return U.atoh(sp[0]);
                    }
                }
            }
            return U.NOT_FOUND;
        }
        public static void Swap<T>(ref T indexA, ref T indexB)
        {
            T tmp = indexA;
            indexA = indexB;
            indexB = tmp;
        }

        public static void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }
        public static void SwapUp<T>(IList<T> list, ListBox listbox, int indexA)
        {
            if (indexA <= 0)
            {
                return;
            }
            Swap(list, indexA, indexA - 1);
            listbox.Items[indexA] = " "; //高さを再計算させるため ダミーの値を入れる.
            listbox.Items[indexA - 1] = " ";
        }
        public static void SwapDown<T>(IList<T> list,ListBox listbox, int indexA)
        {
            if (indexA+1 >= list.Count)
            {
                return;
            }
            Swap(list, indexA, indexA + 1);
            listbox.Items[indexA] = " "; //高さを再計算させるため ダミーの値を入れる.
            listbox.Items[indexA + 1] = " ";
        }

        public static int DrawText(String text, Graphics g, Font font, SolidBrush brush, bool isWithDraw, Rectangle bounds)
        {
            if (isWithDraw)
            {
                StringFormat format = new StringFormat();
                format.FormatFlags = StringFormatFlags.NoWrap;
                try
                {
                    g.DrawString(text, font, brush, bounds, format);
                }
                catch (ExternalException e)
                {//まれにGDI+内部でエラーが発生することがるらしい.原因不明
                    Log.Error(R.ExceptionToString(e));
                    Debug.Assert(false);
                }
            }

            try
            {

                SizeF bb = new SizeF(bounds.Width, bounds.Height);
                SizeF size = g.MeasureString(text, font, bb);
                return (int)size.Width;
            }
            catch (ExternalException e)
            {//まれにGDI+内部でエラーが発生することがるらしい.原因不明
                Log.Error(R.ExceptionToString(e));
                Debug.Assert(false);
                return 0;
            }
        }

        //エラーの赤枠を描画する.
        public static void DrawErrorRectangle(Graphics g,bool isWithDraw,Rectangle listbounds)
        {
            if ( !isWithDraw)
            {
                return;
            }

            Pen pen = new Pen(OptionForm.Color_Error_ForeColor(), 3);
            g.DrawRectangle(pen, listbounds);
            pen.Dispose();
        }

        public static Size DrawTextMulti(String text, Graphics g, Font font, SolidBrush brush, bool isWithDraw, Rectangle bounds)
        {
            Debug.Assert(bounds.Height > 0);

            if (isWithDraw)
            {
                StringFormat format = new StringFormat();
                format.FormatFlags = StringFormatFlags.NoWrap;
                try
                {
                    g.DrawString(text, font, brush, bounds, format);
                }
                catch (ExternalException e)
                {//まれにGDI+内部でエラーが発生することがるらしい.原因不明
                    Log.Error(R.ExceptionToString(e));
                    Debug.Assert(false);
                }
            }
            Debug.Assert(bounds.Height > 0);

            try
            {
                SizeF bb = new SizeF(bounds.Width, bounds.Height);
                Debug.Assert(bb.Height > 0);

                SizeF size = g.MeasureString(text, font, bb);

                return new Size((int)size.Width, (int)size.Height);
            }
            catch (ExternalException e)
            {//まれにGDI+内部でエラーが発生することがるらしい.原因不明
                Log.Error(R.ExceptionToString(e));
                Debug.Assert(false);
                return new Size(0, 0);
            }
        }
        public static int CountLines(string str)
        {
            return str.Split('\n').Length;
        }

        public static bool BitmapSave(Bitmap bitmap, string saveToFile)
        {
            string errorMessage;
            try
            {
                string ext = U.GetFilenameExt(saveToFile);
                if (ext == ".PNG")
                {
                    bitmap.Save(saveToFile, System.Drawing.Imaging.ImageFormat.Png);
                }
                else
                {
                    bitmap.Save(saveToFile);
                }
                return true;
            }
            catch (ExternalException e)
            {//まれにGDI+内部でエラーが発生することがるらしい.原因不明
                errorMessage = R.ExceptionToString(e);
            }
            catch (System.OutOfMemoryException e)
            {
                errorMessage = R.ExceptionToString(e);
            }
            catch (System.UnauthorizedAccessException e)
            {
                errorMessage = R.ExceptionToString(e);
            }
            catch (IOException e)
            {
                errorMessage = R.ExceptionToString(e);
            }
            R.ShowStopError(errorMessage);
            return false;
        }

        public static int DrawPicture(Bitmap pic, Graphics g, bool isWithDraw, Rectangle bounds)
        {
            if (isWithDraw)
            {
                try
                {
                    g.DrawImage(pic, bounds);
                }
                catch (ExternalException e)
                {//まれにGDI+内部でエラーが発生することがるらしい.原因不明
                    Log.Error(R.ExceptionToString(e));
                    Debug.Assert(false);
                }
                catch (System.OutOfMemoryException e)
                {//まれにGDI+内部でエラーが発生することがるらしい.原因不明
                    Log.Error("GDI+ OutOfMemoryException", e.ToString(), e.StackTrace);
                    Debug.Assert(false);
                }
            }
            return bounds.Width;
        }
        public static void DrawLineByBrush(Graphics g,SolidBrush brush, int x, int y, int x2, int y2, bool isWithDraw)
        {
            if (isWithDraw)
            {
                if (x > x2) U.Swap(ref x, ref x2);
                if (y > y2) U.Swap(ref y, ref y2);

                g.FillRectangle(brush, new Rectangle(x, y, x2 - x, y2 - y));
            }
        }

        public static void CopyCombo(ComboBox src, ref ComboBox dest)
        {
            dest.BeginUpdate();
            dest.Items.Clear();
            for (int i = 0; i < src.Items.Count; i ++ )
            {
                dest.Items.Add(src.Items[i]);
            }
            dest.Tag = src.Tag;
            dest.EndUpdate();
            dest.SelectedIndex = src.SelectedIndex;
        }
        public static List<string> ComboBoxToStringList(ComboBox combo)
        {
            List<string> ret = new List<string>();
            for (int i = 0; i < combo.Items.Count; i++)
            {
                ret.Add(combo.Items[i].ToString());
            }
            return ret;
        }

        static uint ConvertUTF8ToUTF32(byte[] moji)
        {
            if (moji.Length <= 0)
            {
                return 0;
            }
            if (moji[0] < 0x80)
            {
                return (uint)moji[0];
            }
            if (moji[0] >= 0xFC && moji.Length >= 6)
            {//6バイト
                uint code = (((uint)moji[0]) & 0x01);
                code = code << 6;
                code |= (((uint)moji[1]) & 0x3F);
                code = code << 6;
                code |= (((uint)moji[2]) & 0x3F);
                code = code << 6;
                code |= (((uint)moji[3]) & 0x3F);
                code = code << 6;
                code |= (((uint)moji[4]) & 0x3F);
                code = code << 6;
                code |= (((uint)moji[5]) & 0x3F);
                return code;
            }
            if (moji[0] >= 0xF8 && moji.Length >= 5)
            {//5バイト
                uint code = (((uint)moji[0]) & 0x03);
                code = code << 6;
                code |= (((uint)moji[1]) & 0x3F);
                code = code << 6;
                code |= (((uint)moji[2]) & 0x3F);
                code = code << 6;
                code |= (((uint)moji[3]) & 0x3F);
                code = code << 6;
                code |= (((uint)moji[4]) & 0x3F);
                return code;
            }
            if (moji[0] >= 0xF0 && moji.Length >= 4)
            {//4バイト
                uint code = (((uint)moji[0]) & 0x07);
                code = code << 6;
                code |= (((uint)moji[1]) & 0x3F);
                code = code << 6;
                code |= (((uint)moji[2]) & 0x3F);
                code = code << 6;
                code |= (((uint)moji[3]) & 0x3F);
                return code;
            }
            if (moji[0] >= 0xE0 && moji.Length >= 3)
            {//3バイト
                uint code = (((uint)moji[0]) & 0x0F);
                code = code << 6;
                code |= (((uint)moji[1]) & 0x3F);
                code = code << 6;
                code |= (((uint)moji[2]) & 0x3F);
                return code;
            }
            if (moji[0] >= 0xC0 && moji.Length >= 2)
            {//2バイト
                uint code = (((uint)moji[0]) & 0x1F);
                code = code << 6;
                code |= (((uint)moji[1]) & 0x3F);
                return code;
            }
            //壊れたコード
            return 0;
        }
        public static int AppendUTF8(List<byte> str, byte[] srcdata, int starti)
        {
            int i = starti;
            int copylen;
            byte c = srcdata[i];
            if (c >= 0xFC)
            {//6バイト
                copylen = 6;
            }
            else if (c >= 0xF8)
            {//5バイト
                copylen = 5;
            }
            else if (c >= 0xF0)
            {//4バイト
                copylen = 4;
            }
            else if (c >= 0xE0)
            {//3バイト
                copylen = 3;
            }
            else if (c >= 0xC0)
            {//2バイト
                copylen = 2;
            }
            else
            {//壊れたコード
                copylen = 1;
            }

            int limit = Math.Min(srcdata.Length, starti + copylen);
            for (; i < limit; i++)
            {
                c = srcdata[i];
                str.Add(c);
            }
            return i - starti;
        }

        public static bool IsUTF8_LAT1SpecialFont(byte code1, byte code2)
        {
            //see https://feuniverse.us/t/scraizas-crazy-asm/5624/2
            if (code1 >= 0x7b && code1 <= 0x7d)
            {
                return true;
            }
            if (code1 == 0x7f)
            {
                return true;
            }
            if (code1 == 0xC2)
            {//U+81 - 
                if (code2 >= 0x81 && code2 <= 0x90)
                {
                    return true;
                }
                if (code2 >= 0x95 && code2 <= 0xA9)
                {
                    return true;
                }
                if (code2 >= 0xAC && code2 <= 0xB9)
                {
                    return true;
                }
                if (code2 >= 0xBC && code2 <= 0xBE)
                {
                    return true;
                }
            }
            return false;
        }

        //一文字変換
        public static uint ConvertMojiCharToUnit(string one, PatchUtil.PRIORITY_CODE priorityCode)
        {
            one = FETextEncode.ConvertSPMoji(one);
            byte[] moji = Program.SystemTextEncoder.Encode(one);

            if (moji.Length >= 2 && moji[0] == '@')
            {//@1234
                int dummy;
                uint code = FETextEncode.at_code_to_binary(moji, 0, out dummy);
                return code;
            }
            else if (priorityCode == PatchUtil.PRIORITY_CODE.UTF8)
            {
                return ConvertUTF8ToUTF32(moji);
            }
            else if (moji.Length >= 4)
            {
                uint code = U.u32(moji, 0);
                return code;
            }
            else if (moji.Length >= 3)
            {
                uint code = U.u24(moji, 0);
                return code;
            }
            else if (moji.Length >= 2)
            {
                uint code = U.u16(moji, 0);
                return code;
            }
            else if (moji.Length >= 1)
            {
                uint code = U.u8(moji, 0);
                return code;
            }
            else
            {
                return 0;
            }
        }
        //一文字変換
        public static uint ConvertMojiCharToUnitFast(string one, PatchUtil.PRIORITY_CODE priorityCode)
        {
            if (one.Length <= 0)
            {
                return 0;
            }
            if (one.Length >= 3 && one[0] == '@')
            {
                byte[] data = System.Text.Encoding.ASCII.GetBytes(one);

                int nowi;
                uint code = FETextEncode.at_code_to_binary(data, 0, out nowi);
                return code;
            }

            //特殊文字はないことにして速度アップを図る.
            byte[] moji = Program.SystemTextEncoder.Encode(one);
            if (priorityCode == PatchUtil.PRIORITY_CODE.UTF8)
            {
                return ConvertUTF8ToUTF32(moji);
            }
            else if (moji.Length >= 4)
            {
                uint code = U.u32(moji, 0);
                return code;
            }
            else if (moji.Length >= 3)
            {
                uint code = U.u24(moji, 0);
                return code;
            }
            else if (moji.Length >= 2)
            {
                uint code = U.u16(moji, 0);
                return code;
            }
            else if (moji.Length >= 1)
            {
                uint code = U.u8(moji, 0);
                return code;
            }
            else
            {
                return 0;
            }
        }
        //一文字変換
        public static string ConvertUnitToMojiChar(uint one)
        {
            if (one <= 0xff)
            {
                byte[] moji = new byte[1];
                moji[0] = (byte)(one & 0xff);
                return Program.SystemTextEncoder.Decode(moji);
            }
            else if (one <= 0xffff)
            {
                byte[] moji = new byte[2];
                moji[0] = (byte)(one & 0xff);
                moji[1] = (byte)((one >> 8) & 0xff);
                return Program.SystemTextEncoder.Decode(moji);
            }
            else if (one <= 0xffffff)
            {
                byte[] moji = new byte[3];
                moji[0] = (byte)(one & 0xff);
                moji[1] = (byte)((one >> 8) & 0xff);
                moji[2] = (byte)((one >> 16) & 0xff);
                return Program.SystemTextEncoder.Decode(moji);
            }
            else
            {
                byte[] moji = new byte[4];
                moji[0] = (byte)(one & 0xff);
                moji[1] = (byte)((one >> 8) & 0xff);
                moji[2] = (byte)((one >> 16) & 0xff);
                moji[3] = (byte)((one >> 24) & 0xff);
                return Program.SystemTextEncoder.Decode(moji);
            }
        }
        public static void append_u32(List<byte> data, uint a)
        {
            data.Add((byte)((a & 0xFF)));
            data.Add((byte)((a & 0xFF00) >> 8));
            data.Add((byte)((a & 0xFF0000) >> 16));
            data.Add((byte)((a & 0xFF000000) >> 24));
        }
        public static void append_u24(List<byte> data, uint a)
        {
            data.Add((byte)((a & 0xFF)));
            data.Add((byte)((a & 0xFF00) >> 8));
            data.Add((byte)((a & 0xFF0000) >> 16));
        }
        public static void append_u16(List<byte> data, uint a)
        {
            data.Add((byte)((a & 0xFF)));
            data.Add((byte)((a & 0xFF00) >> 8));
        }
        public static void append_u8(List<byte> data, uint a)
        {
            data.Add((byte)a);
        }
        public static void append_big32(List<byte> data, uint a)
        {
            data.Add((byte)((a & 0xFF000000) >> 24));
            data.Add((byte)((a & 0xFF0000) >> 16));
            data.Add((byte)((a & 0xFF00) >> 8));
            data.Add((byte)((a & 0xFF)));
        }
        public static void append_big24(List<byte> data, uint a)
        {
            data.Add((byte)((a & 0xFF0000) >> 16));
            data.Add((byte)((a & 0xFF00) >> 8));
            data.Add((byte)((a & 0xFF)));
        }
        public static void append_big16(List<byte> data, uint a)
        {
            data.Add((byte)((a & 0xFF00) >> 8));
            data.Add((byte)((a & 0xFF)));
        }
        public static void append_big8(List<byte> data, uint a)
        {
            data.Add((byte)a);
        }
        public static void append_range(List<byte> data, string str)
        {
            byte[] b = System.Text.Encoding.GetEncoding("ASCII").GetBytes(str);
            data.AddRange(b);
        }
        public static String getASCIIString(byte[] data,uint addr, int length)
        {
            if (length <= 0) return "";
            byte[] d = U.getBinaryData(data,addr, length);
            return System.Text.Encoding.GetEncoding("ASCII").GetString(d);
        }
        public static string getASCIIString(byte[] data, uint addr)
        {
            for (uint i = addr; i < data.Length; i++)
            {
                if (data[i] == 0)
                {
                    uint length = i - addr;
                    return getASCIIString( data , addr , (int)length);
                }
            }

            return "";
        }

        public static uint ChangeEndian16(uint a)
        {
            return ((uint)(a & 0xFF) << 8) + ((a & 0xFF00) >> 8);
        }
        public static uint ChangeEndian32(uint a)
        {
            uint r = (((a & 0xFF) << 24)
                + ((a & 0xFF00) << 8)
                + ((a & 0xFF0000) >> 8)
                + ((a & 0xFF000000) >> 24));
            return r;
        }
#if DEBUG
        public static void TEST_ChangeEndian16()
        {
            uint a = 0x1234;
            uint r = U.ChangeEndian16(a);

            Debug.Assert(r == 0x3412);
        }
        public static void TEST_ChangeEndian32()
        {
            {
                uint a = 0x12345678;
                uint r = U.ChangeEndian32(a);

                Debug.Assert(r == 0x78563412);
            }
            {
                uint a = 0x08010203;
                uint r = U.ChangeEndian32(a);

                Debug.Assert(r == 0x03020108);
            }
        }
#endif

        public static string[] subrange(string[] data, int s, int e)
        {
            return subrange(data, (uint) s, (uint) e);
        }
        public static string[] subrange(string[] data, uint s, uint e)
        {
            s = Math.Min(s, (uint)data.Length);
            e = Math.Min(e, (uint)data.Length);
            if (e <= s)
            {
                return new string[0];
            }

            string[] d = new string[e - s];
            Array.Copy(data, s, d, 0, e - s);
            return d;
        }

        public static byte[] subrange(byte[] data, int s, int e)
        {
            return subrange(data, (uint)s, (uint)e);
        }
        public static byte[] subrange(byte[] data, uint s, uint e)
        {
            s = Math.Min(s, (uint)data.Length);
            e = Math.Min(e, (uint)data.Length);
            if (e <= s)
            {
                 return   new byte[0];
            }

            byte[] d = new byte[e - s];
            Array.Copy(data, s, d, 0, e - s);
            return d;
        }
        public static List<byte> subrangeToList(byte[] data, uint s, uint e)
        {
            s = Math.Min(s, (uint)data.Length);
            e = Math.Min(e, (uint)data.Length);
            if (e <= s)
            {
                return new List<byte>();
            }

            List<byte> ret = new List<byte>();
            for (uint i = s; i < e; i++)
            {
                ret.Add(data[i]);
            }
            return ret;
        }
        public static List<byte> subrange(List<byte> data, uint s, uint e)
        {
            s = Math.Min(s, (uint)data.Count);
            e = Math.Min(e, (uint)data.Count);
            if (e <= s)
            {
                return new List<byte>();
            }

            List<byte> ret = new List<byte>();
            for (uint i = s; i < e; i++)
            {
                ret.Add(data[(int)i]);
            }
            return ret;
        }
        public static byte[] del(byte[] data,uint s,uint e)
        {
            s = Math.Min(s, (uint)data.Length);
            e = Math.Min(e, (uint)data.Length);
            Debug.Assert(s < e);
            
            byte[] d = new byte[data.Length - (e - s)];
            Array.Copy(data, 0, d, 0, s);
            Array.Copy(data, e, d, s, data.Length - e);
            return d;
        }
#if DEBUG
        public static void TEST_del()
        {
            {
                byte[] data = new byte[] { 0, 1, 2, 3 };
                byte[] r = del(data, 1, 3);
                Debug.Assert(r[0] == 0);
                Debug.Assert(r[1] == 3);
                Debug.Assert(r.Length == 2);
            }
            {
                byte[] data = new byte[] { 0, 1, 2, 3 };
                byte[] r = del(data, 2, 3);
                Debug.Assert(r[0] == 0);
                Debug.Assert(r[1] == 1);
                Debug.Assert(r[2] == 3);
                Debug.Assert(r.Length == 3);
            }
        }
#endif


        public static List<TYPE> ListMarge<TYPE>(List<TYPE> a, List<TYPE> b)
        {
            List<TYPE> ret = new List<TYPE>();
            ret.AddRange(a);
            for(int i = 0 ; i < b.Count ; i++)
            {
                if ( ret.IndexOf(b[i]) < 0 )
                {
                    ret.Add(b[i]);
                }
            }
            return ret;
        }
        //配列の追加.
        public static byte[] ArrayAppend(byte[] a,byte[] b)
        {
            byte[] r = new byte[a.Length+b.Length];
            Array.Copy(a, 0, r, 0, a.Length);
            Array.Copy(b, 0, r, a.Length, b.Length);
            return r;
        }
        //配列に追加
        public static byte[] ArrayInsert(byte[] a,int pos, byte[] b)
        {
            Debug.Assert(pos < a.Length);

            byte[] r = new byte[a.Length + b.Length];
            Array.Copy(a, 0, r, 0, pos);
            Array.Copy(b, 0, r, pos, b.Length);
            Array.Copy(a, pos , r, pos + b.Length, a.Length - pos);
            return r;
        }

        public static bool CheckZeroAddressWriteHigh(uint addr, bool ShowMessage = true)
        {
            return CheckZeroAddressWrite(addr, ShowMessage, Program.ROM.RomInfo.compress_image_borderline_address() );
        }

        //0番地に書き込みを拒否する
        public static bool CheckZeroAddressWrite(uint addr,bool ShowMessage = true , uint ProtectedAddress = 0x100)
        {
            if (addr == U.NOT_FOUND || addr <= ProtectedAddress)
            {
                if (ShowMessage)
                {
                    R.ShowStopError("アドレス0番地-{1}番地には書き込むことができません。\r\nあなたが書き込もうとしたアドレス:{0}\r\nこの範囲への書き込みは危険です。", U.To0xHexString(addr), U.To0xHexString(ProtectedAddress));
                }
                return false;
            }
            return true;
        }
        //Check ALIGN 4
        public static bool CheckPaddingALIGN4(uint addr, bool ShowMessage = true)
        {
            if (! U.isPadding4(addr) )
            {
                if (ShowMessage)
                {
                    R.ShowStopError("書き込もうとしているアドレスが4の倍数でないので、書き込みを拒否します。\r\nあなたが書き込もうとしたアドレス:{0}\r\n4の倍数でないアドレスへの書き込みは、ほぼ間違っています。", U.To0xHexString(addr));
                }
                return false;
            }
            return true;
        }

        public static uint ParseUnitGrowAssign(uint unitgrow)
        {
            return (unitgrow >> 1) & 0x3;
        }
        public static uint ParseUnitGrowLV(uint unitgrow)
        {
            return (unitgrow >> 3) & 0x1F;
        }
        public static uint ParsePosY(uint unitpos)
        {
            return (unitpos >> 16) & 0xFFFF;
        }
        public static uint ParsePosX(uint unitpos)
        {
            return (unitpos) & 0xFFFF;
        }
        public static uint ParseFE8UnitPosX(uint unitpos)
        {
            Debug.Assert(Program.ROM.RomInfo.version() >= 8); //FE8限定
            return (unitpos) & 0x3F;
        }
        public static uint ParseFE8UnitPosY(uint unitpos)
        {
            Debug.Assert(Program.ROM.RomInfo.version() >= 8); //FE8限定
            return (unitpos >> 6) & 0x3F;
        }
        public static uint ParseFE8UnitPosExtraBit(uint unitpos)
        {
            Debug.Assert(Program.ROM.RomInfo.version() >= 8); //FE8限定
            return (unitpos >> 12) & 0x7;
        }
        public static uint MakeFe8UnitPos(uint x, uint y, uint ext)
        {
            return (x & 0x3F) | ((y & 0x3F) << 6) | ((ext & 0x7) << 12);
        }

        public static bool SelectedIndexSafety(ListBox list, decimal selectID, bool selectFocus = false)
        {
            return SelectedIndexSafety(list, (int)selectID, selectFocus);
        }
        public static bool SelectedIndexSafety(ListBox list, uint selectID, bool selectFocus = false)
        {
            return SelectedIndexSafety(list, (int)selectID, selectFocus);
        }
        public static bool SelectedIndexSafety(ListBox list, int selectID, bool selectFocus = false)
        {
            if (selectID < 0)
            {
                selectID = 0;
            }

            if (selectID < list.Items.Count)
            {
                list.SelectedIndex = selectID;
                if (selectFocus)
                {
                    list.Focus();
                }
                return true;
            }
            return false;
        }
        public static bool SelectedIndexSafety(ComboBox list, uint selectID, bool selectFocus = false)
        {
            return SelectedIndexSafety(list, (int)selectID, selectFocus);
        }
        public static bool SelectedIndexSafety(ComboBox list, int selectID, bool selectFocus = false)
        {
            if (selectID < 0)
            {
                selectID = 0;
            }

            if (list.Items.Count < 0)
            {//件数が0件
                Debug.Assert(false);
                list.SelectedIndex = -1;
                return false;
            }

            if (list.Items.Count > selectID)
            {
                list.SelectedIndex = selectID;
                if (selectFocus)
                {
                    list.Focus();
                }
                return true;
            }
            return false;
        }
        public static List<TYPE> Filter<TYPE>(List<TYPE> list, Func<TYPE, bool> callback)
        {
            List<TYPE> ret = new List<TYPE>();
            for (int i = 0; i < list.Count; i++)
            {
                if (callback(list[i]))
                {
                    ret.Add(list[i]);
                }
            }
            return ret;
        }

        public static string ConvertListBoxAllItemsToText(ListBox list)
        {
            if (list == null || list.Items == null)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Items.Count; i++)
            {
                sb.AppendLine(list.Items[i].ToString());
            }
            return sb.ToString();
        }

        public static void SetClipboardText(string text)
        {
            if (text == "")
            {
                return;
            }
            try
            {
                Clipboard.SetText(text);
            }
            catch (ExternalException e)
            {
                string str = R._("クリップボードにデータを保存できませんでした。");
                R.ShowStopError(str + "\r\n" + text + "\r\n" + e.ToString());
            }
        }

        //透明色を作る.
        //この関数はとてもくせがあるので、安全に実行できるようにラップする.
        public static void MakeTransparent(Bitmap bitmap)
        {
            if (bitmap != null && bitmap.Palette != null && bitmap.Palette.Entries != null)
            {
                if (bitmap.Palette.Entries.Length > 0)
                {
                    bitmap.MakeTransparent(bitmap.Palette.Entries[0]);
                }
                else
                {
                    bitmap.MakeTransparent();
                }
            }
        }
        public static byte[] getBinaryData(byte[] data, uint addr, int count)
        {
            if (count < 0)
            {
                R.Error("U.getBinaryData pointer:{0} count:{1}", U.To0xHexString(addr), count);
                Debug.Assert(false);
                return new byte[0];
            }
            return getBinaryData(data, addr, (uint)count);
        }


        public static byte[] getBinaryData(byte[] data, uint addr, uint count)
        {
            if (data.Length <= addr + count)
            {
                if (data.Length == 0)
                {
                    return  new byte[0];
                }
                if (addr >= data.Length - 1)
                {
                    addr = (uint)data.Length - 1;
                }
                count = (uint)(data.Length) - addr;
            }
            check_safety(data, addr + count);
            byte[] ret = new byte[count];

            Array.Copy(data, addr, ret, 0, count);
            return ret;
        }
        public static String convertByteToStringDump(byte[] data)
        {
            String bin = "";
            for (uint i = 0; i < data.Length; i++)
            {
                uint a = u8(data, i);
                bin += a.ToString("X02");
            }
            return bin;
        }
        public static byte[] convertStringDumpToByte(string d)
        {
            byte[] r = new byte[d.Length / 2];
            Array.Clear(r, 0, r.Length);

            int length = r.Length * 2;

            for (int len = 0; len < length; len++)
            {
                if ((d[len] >= '0' && d[len] <= '9'))
                {
                    U.write_u4(r, (uint)(len / 2), (uint)(d[len] - '0'), (len % 2) == 0);
                }
                else if ((d[len] >= 'a' && d[len] <= 'f'))
                {
                    U.write_u4(r, (uint)(len / 2), (uint)(d[len] - 'a' + 10), (len % 2) == 0);
                }
                else if ((d[len] >= 'A' && d[len] <= 'F'))
                {
                    U.write_u4(r, (uint)(len / 2), (uint)(d[len] - 'A' + 10), (len % 2) == 0);
                }
                else
                {
                    break;
                }
            }
            return r;
        }

        public static void write_range(byte[] data, uint addr, byte[] write_data)
        {
            check_safety(data, addr + (uint)write_data.Length);
            Array.Copy(write_data, 0, data, addr, write_data.Length);
        }

        public static uint big32(byte[] data, uint addr)
        {
            check_safety(data, addr + 4);
            return data[addr + 3] + ((uint)data[addr + 2] << 8) + ((uint)data[addr + 1] << 16) + ((uint)data[addr + 0] << 24);
        }
        public static uint big24(byte[] data, uint addr)
        {
            check_safety(data, addr + 3);
            return data[addr + 2] + ((uint)data[addr + 1] << 8) + ((uint)data[addr + 0] << 16);
        }
        public static uint big16(byte[] data, uint addr)
        {
            check_safety(data, addr + 2);
            return data[addr + 1] + ((uint)data[addr + 0] << 8);
        }
        public static uint big8(byte[] data, uint addr)
        {
            check_safety(data, addr);
            return data[addr];
        }

        [MethodImpl(256)]
        public static uint u32(byte[] data, uint addr)
        {
            check_safety(data, addr + 4);
            return data[addr] + ((uint)data[addr + 1] << 8) + ((uint)data[addr + 2] << 16) + ((uint)data[addr + 3] << 24);
        }

        public static uint u24(byte[] data, uint addr)
        {
            check_safety(data, addr + 3);
            return data[addr] + ((uint)data[addr + 1] << 8) + ((uint)data[addr + 2] << 16);
        }
 
        [MethodImpl(256)]
        public static uint u16(byte[] data, uint addr)
        {
            check_safety(data, addr + 2);
            return data[addr] + ((uint)data[addr + 1] << 8);
        }

        [MethodImpl(256)]
        public static uint u8(byte[] data, uint addr)
        {
            check_safety(data, addr + 1);
            return data[addr];
        }
        [MethodImpl(256)]
        public static uint u4(byte[] data, uint addr, bool isHigh)
        {
            check_safety(data, addr + 1);
            if (isHigh)
            {
                return (uint)((data[addr] >> 4) & 0xf);
            }
            else
            {
                return (uint)(data[addr] & 0xf);
            }
        }

        [MethodImpl(256)]
        public static uint p32(byte[] data, uint addr)
        {
            uint a = U.u32(data, addr);
            a = U.toOffset(a);
            return a;
        }
        public static void write_p32(byte[] data, uint addr, uint a)
        {
            write_u32(data, addr, U.toPointer(a));
        }
        public static void write_u32(byte[] data, uint addr, uint a)
        {
            check_safety(data, addr + 4);
            data[addr] = (byte)((a & 0xFF));
            data[addr + 1] = (byte)((a & 0xFF00) >> 8);
            data[addr + 2] = (byte)((a & 0xFF0000) >> 16);
            data[addr + 3] = (byte)((a & 0xFF000000) >> 24);
        }
        public static void write_u24(byte[] data, uint addr, uint a)
        {
            check_safety(data, addr + 3);
            data[addr] = (byte)((a & 0xFF));
            data[addr + 1] = (byte)((a & 0xFF00) >> 8);
            data[addr + 2] = (byte)((a & 0xFF0000) >> 16);
        }
        public static void write_u16(byte[] data, uint addr, uint a)
        {
            check_safety(data, addr+2);
            data[addr] = (byte)((a & 0xFF));
            data[addr + 1] = (byte)((a & 0xFF00) >> 8);
        }
        public static void write_u8(byte[] data, uint addr, uint a)
        {
            check_safety(data, addr + 1);
            data[addr] = (byte)a;
        }
        public static void write_u4(byte[] data, uint addr, uint a, bool isHigh)
        {
            check_safety(data, addr + 1);
            if (isHigh)
            {
                data[addr] = (byte)((byte)(data[addr] & 0xf) | (byte)((a & 0xf) << 4));
            }
            else
            {
                data[addr] = (byte)((byte)(data[addr] & 0xf0) | (byte)(a & 0xf));
            }
        }
        public static void write_big32(byte[] data, uint addr, uint a)
        {
            check_safety(data, addr + 4);
            data[addr + 0] = (byte)((a & 0xFF000000) >> 24);
            data[addr + 1] = (byte)((a & 0xFF0000) >> 16);
            data[addr + 2] = (byte)((a & 0xFF00) >> 8);
            data[addr + 3] = (byte)((a & 0xFF));
        }
        public static void write_big24(byte[] data, uint addr, uint a)
        {
            check_safety(data, addr + 3);
            data[addr + 0] = (byte)((a & 0xFF0000) >> 16);
            data[addr + 1] = (byte)((a & 0xFF00) >> 8);
            data[addr + 2] = (byte)((a & 0xFF));
        }
        public static void write_big16(byte[] data, uint addr, uint a)
        {
            check_safety(data, addr + 2);
            data[addr + 0] = (byte)((a & 0xFF00) >> 8);
            data[addr + 1] = (byte)((a & 0xFF));
        }

        //C#が仕事をさぼるので、我々が代わりに仕事をする.
        [MethodImpl(256)]
        static void check_safety(byte[] data, uint addr)
        {
            if (addr > data.Length)
            {
                throw new System.IndexOutOfRangeException(String.Format("Max length:{0}(0x{1}) Access:{2}(0x{3})", data.Length, U.ToHexString(data.Length), addr, U.ToHexString(addr) ));
            }
        }

        public static bool is_RAMPointer(uint a)
        {
            return is_03RAMPointer(a) || is_02RAMPointer(a);
        }
        public static bool is_ROMorRAMPointer(uint a)
        {
            return isPointer(a) || is_03RAMPointer(a) || is_02RAMPointer(a);
        }
        public static bool is_ROMorRAMPointerOrNULL(uint a)
        {
            return isPointerOrNULL(a) || is_03RAMPointer(a) || is_02RAMPointer(a);
        }

        public static bool is_03RAMPointer(uint a)
        {
            return (a >= 0x03000000 && a < 0x03007FFF);
        }

        public static bool is_02RAMPointer(uint a)
        {
            return (a >= 0x02000000 && a < 0x0203FFFF);
        }
        public static bool is_0EDiskPointer(uint a)
        {
            return (a >= 0x0E000000 && a < 0x0E008000);
        }
        public static bool isROMPointer(uint a)
        {
            return isPointer(a);
        }
        public static uint ConvertPointer(uint addr , bool isASM)
        {
            if (isASM)
            {
                return addr | 1;
            }
            else
            {
                return DisassemblerTrumb.ProgramAddrToPlain(addr);
            }
        }

        [MethodImpl(256)]
        public static bool isPointerASM(uint a)
        {
            return (a >= 0x08000000 && a < 0x0A000000) && U.IsValueOdd(a);
        }

        [MethodImpl(256)]
        public static bool isPointerASMOrNull(uint a)
        {
            if (a == 0) return true;
            return (a >= 0x08000000 && a < 0x0A000000) && U.IsValueOdd(a);
        }

        [MethodImpl(256)]
        public static bool isPointer(uint a)
        {
            return (a >= 0x08000000 && a < 0x0A000000);
        }
        [MethodImpl(256)]
        public static bool isPointerOrNULL(uint a)
        {
            return U.isPointer(a) || a == 0x0;
        }

        [MethodImpl(256)]
        public static bool isOffset(uint a)
        {
            return (a < 0x02000000 && a >= 0x00000000);
        }

        [MethodImpl(256)]
        public static uint toOffset(uint a)
        {
            if (a <= 1)
            {
                return a;
            }
            if (U.isPointer(a))
            {
                return a - 0x08000000;
            }
            return a;
        }

        [MethodImpl(256)]
        public static uint toPointer(uint a)
        {
            if (a <= 1)
            {
                return a;
            }
            if (U.isOffset(a))
            {
                return a + 0x08000000;
            }
            return a;
        }

        public static uint GrepEnablePointer(byte[] data, uint start = 0x100, uint end = 0)
        {
            if (end == 0 || end == U.NOT_FOUND)
            {//終端が明記されない場合は、自動的にデータの終端
                end = (uint)data.Length;
            }

            if (start > end)
            {//データ数が足りない
                return U.NOT_FOUND;
            }
            end -= 3;

            uint addr;
            for (addr = start; addr < end; addr += 4)
            {
                uint p = U.u32(data, addr);
                if (U.isPointer(p))
                {
                    if (U.toOffset(p) < data.Length)
                    {
                        continue;
                    }
                }
                break;
            }
            return addr;
        }

        public static uint GrepEnd(byte[] data, byte[] need, uint start = 0x100, uint end = 0, uint blocksize = 1, uint plus = 0, bool needPointer = false)
        {
            uint grepresult = U.Grep(Program.ROM.Data, need, start, end, blocksize);
            if (grepresult == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }
            uint resultAddr = grepresult + (uint)need.Length + plus;
            if (resultAddr > data.Length)
            {//データ終端を超えてしまった
                return U.NOT_FOUND;
            }

            if (needPointer)
            {//検索で見つけたものはポインタである必要がある.
                if (U.isPointerOrNULL(U.u32(data, resultAddr)))
                {
                    return resultAddr;
                }
                //どうやらマッチした場所は違うらしい? 
                //続きから再検索
                return GrepEnd(data, need, resultAddr, end , blocksize, plus , needPointer );
            }
            else
            {
                return resultAddr;
            }
        }

        public static uint Grep(byte[] data, byte[] need, uint start = 0x100,uint end = 0,uint blocksize = 1)
        {
            if (end == 0 || end == U.NOT_FOUND)
            {//終端が明記されない場合は、自動的にデータの終端
                end = (uint)data.Length;
            }

            if (need.Length <= 0)
            {
                return U.NOT_FOUND;
            }
            if (start > end)
            {//データ数が足りない
                return U.NOT_FOUND;
            }
            uint length = end;
            if (length < need.Length)
            {//検索する文字列より、検索されるデータのほうが短い
                return U.NOT_FOUND;
            }
            length -= (uint)need.Length;
            byte needfirst = need[0];

            for (uint i = start; i <= length; i += blocksize)
            {
                if (data[i] != needfirst)
                {
                    continue;
                }

                uint match = (uint)need.Length;
                uint n = 1;
                for (; n < match; n++)
                {
                    if (data[i + n] != need[n])
                    {
                        break;
                    }
                }
                if (n >= match)
                {
                    return i;
                }
            }
            return U.NOT_FOUND;
        }
        public static uint GrepPointer(byte[] data, uint needaddr, uint start = 0x100, uint end = 0)
        {
            if (needaddr == 0 || needaddr == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }
            if (end == 0 || end == U.NOT_FOUND)
            {
                end = (uint)data.Length;
            }
            else
            {
                end = (uint)Math.Min((uint)data.Length, end);
            }
            if (end < 4)
            {
                return U.NOT_FOUND;
            }
            end -= (uint)4;

            needaddr = U.toPointer(needaddr);

            for (uint i = start; i <= end; i+= 4)
            {
                if ( data[i+3] == 0x08 || data[i+3] == 0x09 )
                {
                    if (U.u32(data, i) == needaddr)
                    {
                        return i;
                    }
                }
            }
            return U.NOT_FOUND;
        }
        public static List<uint> GrepPointerAll(byte[] data, uint needaddr, uint start = 0x100, uint end = 0)
        {
            List<uint> ret = new List<uint>();
            if (needaddr == 0 || needaddr == U.NOT_FOUND)
            {
                return ret;
            }

            if (end == 0 || end == U.NOT_FOUND)
            {
                end = (uint)data.Length;
            }
            else
            {
                end = (uint)Math.Min((uint)data.Length, end);
            }
            if (end < 4)
            {
                return ret;
            }
            end -= (uint)4;

            byte[] matchData = new byte[4];
            U.write_u32(matchData,0, U.toPointer(needaddr));

            for (uint i = start; i <= end; i += 4)
            {
                if (data[i] != matchData[0])
                {
                    continue;
                }
                if (data[i+1] != matchData[1])
                {
                    continue;
                }
                if (data[i+2] != matchData[2])
                {
                    continue;
                }
                if (data[i+3] != matchData[3])
                {
                    continue;
                }
                ret.Add(i);
            }
            return ret;
        }
        public static List<uint> GrepPointerAllOnLDR(byte[] romdata, uint needaddr)
        {
            return DisassemblerTrumb.GrepLDRData(romdata, needaddr);
        }
        public static List<Address> MakeAllStructPointersList(bool isPointerOnly)
        {
            List<Address> list = new List<Address>();

            EventCondForm.MakeAllDataLength(list);
            if (InputFormRef.DoEvents(null, "MakeAllStructPointersList 1")) return list;
            ImageBattleAnimeForm.MakeAllDataLength(list, isPointerOnly);
            ImageBattleBGForm.MakeAllDataLength(list, isPointerOnly);
            ImageBattleTerrainForm.MakeAllDataLength(list, isPointerOnly);
            ImageBGForm.MakeAllDataLength(list, isPointerOnly);
            SongTableForm.MakeAllDataLength(list);
            ItemShopForm.MakeAllDataLength(list);
            MapChangeForm.MakeAllDataLength(list);
            FontForm.MakeAllDataLength(list);
            TextForm.MakeAllDataLength(list , isPointerOnly);
            MapPointerForm.MakeAllDataLength(list, isPointerOnly);
            MapExitPointForm.MakeAllDataLength(list);
            ItemForm.MakeAllDataLength(list);
            ImageMagicFEditorForm.MakeAllDataLength(list , isPointerOnly);
            ImageMagicCSACreatorForm.MakeAllDataLength(list, isPointerOnly);
            TextCharCodeForm.MakeAllDataLength(list);
            UnitActionPointerForm.MakeAllDataLength(list);
            if (InputFormRef.DoEvents(null, "MakeAllStructPointersList 2")) return list;

            if (Program.ROM.RomInfo.is_multibyte() == false)
            {
                SkillAssignmentClassSkillSystemForm.MakeAllDataLength(list);
                SkillAssignmentUnitSkillSystemForm.MakeAllDataLength(list);
                SkillConfigSkillSystemForm.MakeAllDataLength(list, isPointerOnly);
            }
            else
            {
                SkillConfigFE8NSkillForm.MakeAllDataLength(list);
                SkillConfigFE8NVer2SkillForm.MakeAllDataLength(list, isPointerOnly);
            }
            if (InputFormRef.DoEvents(null, "MakeAllStructPointersList 3")) return list;

            ImageBattleScreenForm.MakeAllDataLength(list, isPointerOnly);
            ImageItemIconForm.MakeAllDataLength(list);
            ImageUnitMoveIconFrom.MakeAllDataLength(list, isPointerOnly);
            ImageUnitWaitIconFrom.MakeAllDataLength(list, isPointerOnly);
            ImageUnitPaletteForm.MakeAllDataLength(list, isPointerOnly);
            ImageSystemIconForm.MakeAllDataLength(list, isPointerOnly);
            ItemPromotionForm.MakeAllDataLength(list);
            SoundBossBGMForm.MakeAllDataLength(list);
            SoundFootStepsForm.MakeAllDataLength(list);
            SupportAttributeForm.MakeAllDataLength(list);
            UnitPaletteForm.MakeAllDataLength(list);
            MantAnimationForm.MakeAllDataLength(list);
            MenuDefinitionForm.MakeAllDataLength(list);
            OtherTextForm.MakeAllDataLength(list);
            StatusRMenuForm.MakeAllDataLength(list);
            StatusParamForm.MakeAllDataLength(list);
            AIScriptForm.MakeAllDataLength(list , isPointerOnly);
            AITargetForm.MakeAllDataLength(list);
            ItemUsagePointerForm.MakeAllDataLength(list);
            ArenaClassForm.MakeAllDataLength(list);
            MapTileAnimation1Form.MakeAllDataLength(list);
            MapTileAnimation2Form.MakeAllDataLength(list);
            ItemWeaponEffectForm.MakeAllDataLength(list);
            ItemWeaponTriangleForm.MakeAllDataLength(list);
            AIStealItemForm.MakeAllDataLength(list);
            AIMapSettingForm.MakeAllDataLength(list);
            AIPerformStaffForm.MakeAllDataLength(list);
            AIPerformItemForm.MakeAllDataLength(list);
            ImageRomAnimeForm.MakeAllDataLength(list, isPointerOnly);
            ImageGenericEnemyPortraitForm.MakeAllDataLength(list, isPointerOnly);
            MapTerrainFloorLookupTableForm.MakeAllDataLength(list);
            MapTerrainBGLookupTableForm.MakeAllDataLength(list);
            ArenaEnemyWeaponForm.MakeAllDataLength(list);
            EventUnitForm.RecycleReserveUnits(ref list);
            if (InputFormRef.DoEvents(null, "MakeAllStructPointersList 4")) return list;

            if (Program.ROM.RomInfo.version() == 8)
            {
                ImageTSAAnime2Form.MakeAllDataLength(list, isPointerOnly);
                StatusOptionForm.MakeAllDataLength(list, isPointerOnly);
                StatusOptionOrderForm.MakeAllDataLength(list);
                StatusUnitsMenuForm.MakeAllDataLength(list);
                LinkArenaDenyUnitForm.MakeAllDataLength(list);
                TextDicForm.MakeAllDataLength(list);
                ImageTSAAnimeForm.MakeAllDataLength(list, isPointerOnly);
                MonsterItemForm.MakeAllDataLength(list);
                MonsterProbabilityForm.MakeAllDataLength(list);
                MonsterWMapProbabilityForm.MakeAllDataLength(list);
                EDForm.MakeAllDataLength(list);
                EventBattleTalkForm.MakeAllDataLength(list);
                ClassForm.MakeAllDataLength(list);
                CCBranchForm.MakeAllDataLength(list);
                OPClassAlphaNameForm.MakeAllDataLength(list);
                WorldMapPathForm.MakeAllDataLength(list);
                UnitForm.MakeAllDataLength(list);
                WorldMapEventPointerForm.MakeAllDataLength(list);
                ImagePortraitForm.MakeAllDataLength(list, isPointerOnly);
                EDStaffRollForm.MakeAllDataLength(list, isPointerOnly);
                OPPrologueForm.MakeAllDataLength(list, isPointerOnly);
                EventHaikuForm.MakeAllDataLength(list);
                EventForceSortieForm.MakeAllDataLength(list);
                ImageChapterTitleForm.MakeAllDataLength(list,isPointerOnly);
                SoundRoomForm.MakeAllDataLength(list);
                SupportTalkForm.MakeAllDataLength(list);
                SupportUnitForm.MakeAllDataLength(list);
                WorldMapImageForm.MakeAllDataLength(list, isPointerOnly);
                WorldMapPointForm.MakeAllDataLength(list);
                WorldMapBGMForm.MakeAllDataLength(list);
                SummonUnitForm.MakeAllDataLength(list, isPointerOnly);
                SummonsDemonKingForm.MakeAllDataLength(list, isPointerOnly);
                MapSettingForm.MakeAllDataLength(list);
                ImageCGForm.MakeAllDataLength(list, isPointerOnly);

                if (Program.ROM.RomInfo.is_multibyte())
                {
                    OPClassFontForm.MakeAllDataLength(list, isPointerOnly);
                    OPClassDemoForm.MakeAllDataLength(list);
                    ExtraUnitForm.MakeAllDataLength(list);
                }
                else
                {
                    OPClassFontFE8UForm.MakeAllDataLength(list, isPointerOnly);
                    OPClassDemoFE8UForm.MakeAllDataLength(list);
                    ExtraUnitFE8UForm.MakeAllDataLength(list);
                    FE8SpellMenuExtendsForm.MakeAllDataLength(list);
                }

            }
            else if (Program.ROM.RomInfo.version() == 7)
            {
                ImageTSAAnimeForm.MakeAllDataLength(list, isPointerOnly);
                EDFE7Form.MakeAllDataLength(list);
                EventBattleTalkFE7Form.MakeAllDataLength(list);
                EDSensekiCommentForm.MakeAllDataLength(list);
                ClassForm.MakeAllDataLength(list);
                UnitCustomBattleAnimeForm.MakeAllDataLength(list);
                UnitFE7Form.MakeAllDataLength(list);
                WorldMapEventPointerFE7Form.MakeAllDataLength(list);
                ImagePortraitForm.MakeAllDataLength(list, isPointerOnly);
                EventHaikuFE7Form.MakeAllDataLength(list);
                EventForceSortieFE7Form.MakeAllDataLength(list);
                SoundRoomForm.MakeAllDataLength(list);
                SupportTalkFE7Form.MakeAllDataLength(list);
                SupportUnitForm.MakeAllDataLength(list);
                WorldMapImageFE7Form.MakeAllDataLength(list, isPointerOnly);
                SoundRoomCGForm.MakeAllDataLength(list);
                TacticianAffinityFE7.MakeAllDataLength(list);
                StatusOptionForm.MakeAllDataLength(list, isPointerOnly);
                StatusOptionOrderForm.MakeAllDataLength(list);
                EventFinalSerifFE7Form.MakeAllDataLength(list);

                if (Program.ROM.RomInfo.is_multibyte())
                {
                    ImageChapterTitleFE7Form.MakeAllDataLength(list, isPointerOnly);
                    MapSettingFE7Form.MakeAllDataLength(list);
                    ImageCGForm.MakeAllDataLength(list, isPointerOnly);
                    OPClassDemoFE7Form.MakeAllDataLength(list , isPointerOnly);
                }
                else
                {
                    MapSettingFE7UForm.MakeAllDataLength(list);
                    ImageCGFE7UForm.MakeAllDataLength(list, isPointerOnly);
                    OPClassDemoFE7UForm.MakeAllDataLength(list);
                }
            }
            else if (Program.ROM.RomInfo.version() == 6)
            {
                EDFE6Form.MakeAllDataLength(list);
                EventBattleTalkFE6Form.MakeAllDataLength(list);
                EDSensekiCommentForm.MakeAllDataLength(list);
                ClassFE6Form.MakeAllDataLength(list);
                UnitFE6Form.MakeAllDataLength(list);
                WorldMapEventPointerFE6Form.MakeAllDataLength(list);
                ImagePortraitFE6Form.MakeAllDataLength(list, isPointerOnly);
                EventHaikuFE6Form.MakeAllDataLength(list);
                ImageChapterTitleFE7Form.MakeAllDataLength(list, isPointerOnly);
                SoundRoomFE6Form.MakeAllDataLength(list);
                SupportTalkFE6Form.MakeAllDataLength(list);
                SupportUnitFE6Form.MakeAllDataLength(list);
                WorldMapImageFE6Form.MakeAllDataLength(list, isPointerOnly);
                MapSettingFE6Form.MakeAllDataLength(list);
                OPClassAlphaNameFE6Form.MakeAllDataLength(list);
            }
            if (InputFormRef.DoEvents(null, "MakeAllStructPointersList 5")) return list;

            if (Program.ROM.RomInfo.is_multibyte())
            {
                MapTerrainNameForm.MakeAllDataLength(list);
            }
            else
            {
                MapTerrainNameEngForm.MakeAllDataLength(list);
            }
            return list;
        }
        public static void AppendAllASMStructPointersList(List<Address> structlist
            , List<DisassemblerTrumb.LDRPointer> ldrmap
            , bool isPatchInstallOnly
            , bool isPatchPointerOnly
            , bool isPatchStructOnly
            , bool isUseOtherGraphics
            , bool isUseOAMSP
            )
        {
            if (InputFormRef.DoEvents(null, "AppendAllASMStructPointersList PatchForm")) return;
            PatchForm.MakePatchStructDataList(structlist, isPatchPointerOnly, isPatchInstallOnly, isPatchStructOnly);

            if (ldrmap != null)
            {
                if (InputFormRef.DoEvents(null, "AppendAllASMStructPointersList ProcsScriptForm")) return;
                ProcsScriptForm.MakeAllDataLength(structlist, ldrmap);

                if (InputFormRef.DoEvents(null, "AppendAllASMStructPointersList EventScript")) return;
                EventScript.MakeEventASMMAPList(structlist); //イベントから呼び出される特殊指定の領域を調べます.

                if (InputFormRef.DoEvents(null, "AppendAllASMStructPointersList EventFunctionPointerForm")) return;
                EventFunctionPointerForm.MakeAllDataLength(structlist);

                if (InputFormRef.DoEvents(null, "AppendAllASMStructPointersList Command85PointerForm")) return;
                Command85PointerForm.MakeAllDataLength(structlist);

                if (InputFormRef.DoEvents(null, "AppendAllASMStructPointersList ItemEffectPointerForm")) return;
                ItemEffectPointerForm.MakeAllDataLength(structlist);

                if (InputFormRef.DoEvents(null, "AppendAllASMStructPointersList UnitIncreaseHeightForm")) return;
                UnitIncreaseHeightForm.MakeAllDataLength(structlist);

                if (InputFormRef.DoEvents(null, "AppendAllASMStructPointersList MapLoadFunctionForm")) return;
                MapLoadFunctionForm.MakeAllDataLength(structlist);
            }

            if (isUseOtherGraphics)
            {
                if (InputFormRef.DoEvents(null, "AppendAllASMStructPointersList GraphicsToolForm")) return;
                GraphicsToolForm.MakeLZ77DataList(structlist); //lz77で圧縮されたもの(主に画像)
            }
            if (isUseOAMSP)
            {
                Debug.Assert(ldrmap != null);
                if (InputFormRef.DoEvents(null, "AppendAllASMStructPointersList OAMSPForm")) return;
                OAMSPForm.MakeAllDataLength(structlist, structlist, ldrmap);
            }
        }

        public static List<UseValsID> MakeVarsIDArray()
        {
            List<UseValsID> list = new List<UseValsID>();
            if (InputFormRef.DoEvents(null, "MakeVarsIDArray 1")) return list;

            UnitForm.MakeVarsIDArray(list);
            ClassForm.MakeVarsIDArray(list);
            ItemForm.MakeVarsIDArray(list);
            EventCondForm.MakeVarsIDArray(list);

            if (InputFormRef.DoEvents(null, "MakeVarsIDArray 2")) return list;
            if (Program.ROM.RomInfo.is_multibyte())
            {
            }
            else
            {
                StatusParamForm.MakeVarsIDArray(list);
                MapTerrainNameEngForm.MakeVarsIDArray(list);
            }
            MenuDefinitionForm.MakeVarsIDArray(list);
            StatusRMenuForm.MakeVarsIDArray(list);
            SoundBossBGMForm.MakeVarsIDArray(list);

            if (InputFormRef.DoEvents(null, "MakeVarsIDArray 3")) return list;
            if (Program.ROM.RomInfo.version() == 8)
            {
                WorldMapEventPointerForm.MakeVarsIDArray(list);
                StatusOptionForm.MakeVarsIDArray(list);
                StatusUnitsMenuForm.MakeVarsIDArray(list);
                MapSettingForm.MakeVarsIDArray(list);
                SupportTalkForm.MakeVarsIDArray(list);
                EDForm.MakeVarsIDArray(list);
                EventHaikuForm.MakeVarsIDArray(list);
                EventBattleTalkForm.MakeVarsIDArray(list);
                SoundRoomForm.MakeVarsIDArray(list);
                WorldMapPointForm.MakeVarsIDArray(list);
                TextDicForm.MakeVarsIDArray(list);
                WorldMapBGMForm.MakeVarsIDArray(list);

                if (Program.ROM.RomInfo.is_multibyte())
                {
                    OPClassDemoForm.MakeVarsIDArray(list);
                    SkillConfigFE8NSkillForm.MakeVarsIDArray(list);
                    SkillConfigFE8NVer2SkillForm.MakeVarsIDArray(list);
                    SkillConfigFE8NVer3SkillForm.MakeVarsIDArray(list);
                }
                else
                {
                    OPClassDemoFE8UForm.MakeVarsIDArray(list);
                    SkillConfigSkillSystemForm.MakeVarsIDArray(list);
                }

            }
            else if (Program.ROM.RomInfo.version() == 7)
            {//7
                if (Program.ROM.RomInfo.is_multibyte())
                {
                    MapSettingFE7Form.MakeVarsIDArray(list);
                    OPClassDemoFE7Form.MakeVarsIDArray(list);
                }
                else
                {
                    MapSettingFE7UForm.MakeVarsIDArray(list);
                    OPClassDemoFE7UForm.MakeVarsIDArray(list);
                }
                StatusOptionForm.MakeVarsIDArray(list);
                StatusUnitsMenuForm.MakeVarsIDArray(list);
                SupportTalkFE7Form.MakeVarsIDArray(list);
                EDFE7Form.MakeVarsIDArray(list);
                EventHaikuFE7Form.MakeVarsIDArray(list);
                EventBattleTalkFE7Form.MakeVarsIDArray(list);
                SoundRoomForm.MakeVarsIDArray(list);
                EDSensekiCommentForm.MakeVarsIDArray(list);
                EventFinalSerifFE7Form.MakeVarsIDArray(list);
                WorldMapEventPointerFE7Form.MakeVarsIDArray(list);
            }
            else
            {//6
                MapSettingFE6Form.MakeVarsIDArray(list);
//                OPClassDemoFE6Form.MakeVarsIDArray(list);
                SupportTalkFE6Form.MakeVarsIDArray(list);
                EDFE6Form.MakeVarsIDArray(list);
                EventHaikuFE6Form.MakeVarsIDArray(list);
                EventBattleTalkFE6Form.MakeVarsIDArray(list);
                SoundRoomFE6Form.MakeVarsIDArray(list);
                WorldMapEventPointerFE6Form.MakeVarsIDArray(list);
            }
            if (InputFormRef.DoEvents(null, "MakeVarsIDArray 4")) return list;
            PatchForm.MakeVarsIDArray(list);

            Program.AsmMapFileAsmCache.MakeVarsIDArray(list);

            if (InputFormRef.DoEvents(null, "MakeVarsIDArray 5")) return list;
            UseValsID.RemoveDuplicates(list);

            return list;
        }

        //AB CD XX XX XX EE みたいな 一部スキップできる部分マッチ
        public static uint GrepPatternMatch(byte[] data, byte[] need,bool[] isSkip, uint start = 0x100, uint end = 0, uint blocksize = 1)
        {
            Debug.Assert(isSkip.Length >= need.Length);

            if (end == 0)
            {//終端が明記されない場合は、自動的にデータの終端
                end = (uint)data.Length;
            }

            if (start > end)
            {//データ数が足りない
                return U.NOT_FOUND;
            }
            uint length = end;
            if (length < need.Length)
            {//検索する文字列より、検索されるデータのほうが短い
                return U.NOT_FOUND;
            }
            if (need.Length <= 0)
            {//空配列と比較
                return U.NOT_FOUND;
            }
            length -= (uint)need.Length;
            if (data.Length < length)
            {
                length = (uint)data.Length;
            }

            byte needfirst = need[0];
            bool isSkipfirst = isSkip[0];

            for (uint i = start; i <= length; i += blocksize)
            {
                if (data[i] != needfirst)
                {
                    if (isSkipfirst == false)
                    {
                        continue;
                    }
                }

                uint match = (uint)need.Length;
                uint n = 1;
                for (; n < match; n++)
                {
                    if (data[i + n] != need[n])
                    {
                        if (isSkip[n] == false)
                        {
                            break;
                        }
                    }
                }
                if (n >= match)
                {
                    return i;
                }
            }
            return U.NOT_FOUND;
        }
        public static uint GrepPatternMatchEnd(byte[] data, byte[] need, bool[] isSkip, uint start = 0x100, uint end = 0, uint blocksize = 1, uint plus = 0, bool needPointer = false)
        {
            uint grepresult = U.GrepPatternMatch(Program.ROM.Data, need,isSkip, start, end, blocksize);
            if (grepresult == U.NOT_FOUND)
            {
                return U.NOT_FOUND;
            }
            uint resultAddr = grepresult + (uint)need.Length + plus;
            if (resultAddr > data.Length)
            {//データ終端を超えてしまった
                return U.NOT_FOUND;
            }

            if (needPointer)
            {//検索で見つけたものはポインタである必要がある.
                if (U.isPointerOrNULL(U.u32(data, resultAddr)))
                {
                    return resultAddr;
                }
                //どうやらマッチした場所は違うらしい? 
                //続きから再検索
                return GrepPatternMatchEnd(data, need,isSkip, resultAddr, end, blocksize, plus, needPointer);
            }
            else
            {
                return resultAddr;
            }
        }

        public static uint MatchExistingStructure(uint addr , List<Address> existingStructure)
        {
            int length = existingStructure.Count;
            for (int i = 0; i < length; i++)
            {
                Address a = existingStructure[i];
                if (addr >= a.Addr && addr < a.Addr + a.Length)
                {
                    return a.Addr + a.Length;
                }
            }
            return U.NOT_FOUND;
        }

        public static bool isSafetyLength(uint addr, uint length)
        {
            if (!U.isSafetyOffset(addr + length))
            {
                return false;
            }
            if (length >= 0x00200000)
            {
                return false;
            }
            return true;
        }

        [MethodImpl(256)]
        public static bool isSafetyOffset(uint a)
        {
            return (a < 0x02000000 && a >= 0x00000100 && a < Program.ROM.Data.Length);
        }

        [MethodImpl(256)]
        public static bool isSafetyPointer(uint a)
        {
            return (a < 0x0A000000 && a >= 0x08000100 && a - 0x08000000 < Program.ROM.Data.Length);
        }

        [MethodImpl(256)]
        public static bool isSafetyOffset(uint a,ROM rom)
        {
            return (a < 0x02000000 && a >= 0x00000100 && a < rom.Data.Length);
        }

        [MethodImpl(256)]
        public static bool isSafetyPointer(uint a,ROM rom)
        {
            return (a < 0x0A000000 && a >= 0x08000100 && a - 0x08000000 < rom.Data.Length);
        }

        [MethodImpl(256)]
        public static bool isExtrendsROMArea(uint a)
        {
            return (a >= U.toOffset(Program.ROM.RomInfo.extends_address()));
        }

        public static bool isSafetyZArray(uint a)
        {
            return (a < Program.ROM.Data.Length);
        }

        public static bool isSafetyZArray(uint a, byte[] array)
        {
            return (a < array.Length);
        }

        public static bool isSafetyZArray(uint a, List<byte> array)
        {
            return (a < array.Count);
        }

        public static bool isSafetyZArray(int a)
        {
            return (a < Program.ROM.Data.Length);
        }

        public static bool isSafetyZArray(int a, byte[] array)
        {
            return (a < array.Length);
        }

        [MethodImpl(256)]
        public static bool isSafetyZArray(int a, List<byte> array)
        {
            return (a < array.Count);
        }
        
        public static bool IsComment(string line)
        {
            if (line.Length < 1)
            {
                return true;
            }
            if (line[0] == '#')
            {
                return true;
            }
            if (line[0] == ';')
            {
                return true;
            }
            if (line.Length >= 2)
            {
                if (line[0] == '/' && line[1] == '/')
                {
                    return true;
                }
                if (line[0] == '-' && line[1] == '-')
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsCommentSlashOnly(string line)
        {
            if (line.Length < 1)
            {
                return true;
            }
            if (line.Length >= 2)
            {
                if (line[0] == '/' && line[1] == '/')
                {
                    return true;
                }
            }
            return false;
        }

        [MethodImpl(256)]
        public static bool OtherLangLine(string line)
        {
            return OtherLangLine(line,Program.ROM);
        }
        public static bool OtherLangLine(string line,ROM rom)
        {
            if (rom.RomInfo.is_multibyte())
            {
                if (line.IndexOf("\t{U}") >= 0)
                {//日本語版で英語版専用項目はいらない
                    return true;
                }
            }
            else
            {
                if (line.IndexOf("\t{J}") >= 0)
                {//英語版で日本語版専用項目はいらない
                    return true;
                }
            }
            return false;
        }
        static int ClipCommentIndexOf(string str, string need)
        {
            int index = str.IndexOf(need);
            if (index < 0)
            {
                return -1;
            }
            if (index == 0)
            {
                return 0;
            }
            if (str[index - 1] == ' ' || str[index - 1] == '\t')
            {
                return index - 1;
            }
            return -1;
        }
        public static string ClipComment(string str)
        {
            int term = ClipCommentIndexOf(str,"{J}");
            if (term >= 0)
            {//言語指定を飛ばす
                str = str.Substring(0, term);
            }
            term = ClipCommentIndexOf(str,"{U}");
            if (term >= 0)
            {//言語指定を飛ばす
                str = str.Substring(0, term);
            }
            term = ClipCommentIndexOf(str,"//");
            if (term >= 0)
            {//コメント
                str = str.Substring(0, term);
            }
            return str;
        }
        public static string ClipCommentWithCharpAndAtmark(string str)
        {
            int term = ClipCommentIndexOf(str, "{J}");
            if (term >= 0)
            {//言語指定を飛ばす
                str = str.Substring(0, term);
            }
            term = ClipCommentIndexOf(str, "{U}");
            if (term >= 0)
            {//言語指定を飛ばす
                str = str.Substring(0, term);
            }
            term = ClipCommentIndexOf(str, "//");
            if (term >= 0)
            {//コメント
                str = str.Substring(0, term);
            }
            term = ClipCommentIndexOf(str, "#");
            if (term >= 0)
            {//コメント
                str = str.Substring(0, term);
            }
            term = ClipCommentIndexOf(str, "@");
            if (term >= 0)
            {//コメント
                str = str.Substring(0, term);
            }
            return str;
        }
        //拡張子を取得 結果は必ず大文字 .PNG みたいに
        public static string GetFilenameExt(string filename)
        {
            return Path.GetExtension(filename).ToUpper();
        }

        public static long GetFileSize(string filename)
        {
            FileInfo info = new FileInfo(filename);
            return info.Length;
        }
        public static DateTime GetFileDateCreationTime(string filename)
        {
            FileInfo info = new FileInfo(filename);
            return info.CreationTime;
        }
        public static DateTime GetFileDateLastWriteTime(string filename)
        {
            FileInfo info = new FileInfo(filename);
            return info.LastWriteTime;
        }

        //https://stackoverflow.com/questions/146134/how-to-remove-illegal-characters-from-path-and-filenames
        readonly static string s_regexSearch = "[" + System.Text.RegularExpressions.Regex.Escape(
            new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars()))
            + "]";
        public static string escape_filename(string str)
        {
            str = RegexCache.Replace(str, s_regexSearch, "_");
            return str;
        }
        public static string escape_return(string str)
        {
            str = str.Replace("\r\n", " ");
            str = str.Replace("\n", " ");
            str = str.Replace("\r", " ");
            return str;
        }
        public static bool IsBadFilename(string str)
        {
            return (escape_filename(str) != str);
        }

        public static string ConfigDataFilename(string type)
        {
            return ConfigDataFilename(type,Program.ROM);
        }

        //第2言語として英語を利用できるか?
        public static bool CanSecondLanguageEnglish(string lang)
        {
            if (lang == "en") return false;
            if (lang == "ja") return false;
            return true;
        }
        public static string ConfigDataFilename(string type,ROM rom)
        {
            //
            //サーチ順番
            //aaaa_FE7.en.txt
            //aaaa_FE7.txt
            //aaaa_ALL.en.txt
            //aaaa_ALL.txt
            string lang = OptionForm.lang();
            bool canSecondLanguageEnglish = CanSecondLanguageEnglish(lang);
            string fullfilename;
            if (rom != null)
            {
                fullfilename = Path.Combine(Program.BaseDirectory, "config", "data", type + rom.RomInfo.TitleToFilename() + "." + lang + ".txt");
                if (File.Exists(fullfilename))
                {//言語指定がある
                    return fullfilename;
                }
                if (canSecondLanguageEnglish)
                {//第2言語を英語にできるなら英語リソースを検索
                    fullfilename = Path.Combine(Program.BaseDirectory, "config", "data", type + rom.RomInfo.TitleToFilename() + "." + "en" + ".txt");
                    if (File.Exists(fullfilename))
                    {//言語指定がある
                        return fullfilename;
                    }
                }

                //ないなら共通版
                fullfilename = Path.Combine(Program.BaseDirectory, "config", "data", type + rom.RomInfo.TitleToFilename() + ".txt");
                if (File.Exists(fullfilename))
                {//各FEごとの設定がある
                    return fullfilename;
                }
            }



            //すべてのバージョン共通版の言語版があるか?
            fullfilename = Path.Combine(Program.BaseDirectory, "config", "data", type + "ALL" + "." + lang + ".txt");
            if (File.Exists(fullfilename))
            {//すべてのバージョン共通版の言語版がある
                return fullfilename;
            }

            if (canSecondLanguageEnglish)
            {//第2言語を英語にできるなら英語リソースを検索
                fullfilename = Path.Combine(Program.BaseDirectory, "config", "data", type + "ALL" + "." + "en" + ".txt");
                if (File.Exists(fullfilename))
                {//言語指定がある
                    return fullfilename;
                }
            }

            fullfilename = Path.Combine(Program.BaseDirectory, "config", "data", type + "ALL" + ".txt");
            return fullfilename;
        }


        public static string ConfigEtcFilename(string type, ROM rom)
        {
            string romtitle = "";
            if (rom == null)
            {
                romtitle = "_";
            }
            else if (rom.IsVirtualROM)
            {//仮想ROMなのでファイル名はない
                romtitle = "_Virtial_" + rom.RomInfo.VersionToFilename();
            }
            else
            {
                romtitle = U.GetFirstPeriodFilename(rom.Filename);
            }
            return Path.Combine(Program.BaseDirectory, "config", "etc", romtitle, type + ".txt");
        }
        public static string ConfigEtcFilename(string type, string romBaseFilename)
        {
            string romtitle = U.GetFirstPeriodFilename(romBaseFilename);
            return Path.Combine(Program.BaseDirectory, "config", "etc", romtitle, type + ".txt");
        }

        public static string ConfigEtcFilename(string type)
        {
            return ConfigEtcFilename(type, Program.ROM );
        }

        //同じ場所を再選択して、再選択イベントを動かす.
        public static void ReSelectList(ListBox list)
        {
            U.ForceUpdate(list, list.SelectedIndex);
        }
        public static void ReSelectList(ComboBox list)
        {
            U.ForceUpdate(list, list.SelectedIndex);
        }
        public static void ReSelectList(ListBox list, ListBox list2)
        {
            int index = list.SelectedIndex;
            int index2 = list2.SelectedIndex;
            U.ForceUpdate(list, index);
            U.ForceUpdate(list2, index2);
        }
        public static void ReSelectList(ComboBox list, ComboBox list2)
        {
            int index = list.SelectedIndex;
            int index2 = list2.SelectedIndex;
            U.ForceUpdate(list, index);
            U.ForceUpdate(list2, index2);
        }
        public static uint ByteSwap16(uint a)
        {
            return ((a >> 8) & 0xFF) + ((a & 0xFF) << 8);
        }
        public static string skip(string text, string need)
        {
            int i = text.IndexOf(need);
            if (i < 0) return "";
            return text.Substring(i + need.Length);
        }
        public static string cut(string text, string need,string endneed)
        {
            int i = text.IndexOf(need);
            if (i < 0) return "";
            string a = text.Substring(i+need.Length);

            i = a.IndexOf(endneed);
            if (i < 0) return "";
            return a.Substring(0,i);
        }
        public static string cut(string text, string endneed)
        {
            int i = text.IndexOf(endneed);
            if (i < 0) return "";
            return text.Substring(0, i);
        }
        public static string term(string text, string term)
        {
            int i = text.IndexOf(term);
            if (i < 0) return text;
            return text.Substring(0, i);
        }

        public static string skipLine(StreamReader reader, string need)
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line.IndexOf(need) >= 0)
                {
                    return line;
                }
            }
            return "";
        }

        public static void MapMarge(ref Dictionary<uint, string>  a , Dictionary<uint, string>  b,bool isOverrade = true)
        {
            if (isOverrade)
            {
                foreach (var pair in b)
                {
                    a[pair.Key] = pair.Value;
                }
            }
            else
            {
                foreach (var pair in b)
                {
                    if (!a.ContainsKey(pair.Key))
                    {
                        a[pair.Key] = pair.Value;
                    }
                }
            }
        }

        public static Dictionary<uint, string> LoadDicResource(string fullfilename)
        {
            Dictionary<uint, string> dic = new Dictionary<uint, string>();
            if (!U.IsRequiredFileExist(fullfilename))
            {
                return dic;
            }

            try
            {
                using (StreamReader reader = File.OpenText(fullfilename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (U.IsComment(line) || U.OtherLangLine(line))
                        {
                            continue;
                        }
                        line = U.ClipComment(line);
                        if (line == "")
                        {
                            continue;
                        }
                        string[] sp = line.Split('=');
                        dic[U.atoh(sp[0])] = U.at(sp, 1);
                    }
                }
            }
            catch (Exception e)
            {
                R.ShowStopError("必須となる設定ファイルを読みこめません。\r\n設定ファイルが壊れている可能性があります。\r\n再ダウンロードしなおしてください。\r\n{0}\r\n{1}", fullfilename, e.ToString());
            }
            return dic;
        }

        public static void SaveTSVResource(string fullfilename,Dictionary<uint, string[]> data)
        {
            string dir = Path.GetDirectoryName(fullfilename);
            if (!Directory.Exists(dir))
            {
                U.mkdir(dir);
            }
            if (! U.CanWriteFileRetry(fullfilename) )
            {
                return;
            }
            try
            {
                using (StreamWriter w = new StreamWriter(fullfilename))
                {
                    foreach (var pair in data)
                    {
                        string line = U.ToHexString(pair.Key);
                        for(int i = 0 ; i < pair.Value.Length ; i++)
                        {
                            line += "\t" + pair.Value[i];
                        }
                   
                        w.WriteLine(line);
                    }
                }
            }
            catch (Exception e)
            {
                R.ShowStopError("ファイルに書き込めません。\r\n{0}\r\n{1}", fullfilename, e.ToString());
            }
            
        }

        public static Dictionary<uint, string[]> LoadTSVResource(string fullfilename, bool isRequired = true)
        {
            Dictionary<uint, string[]> dic = new Dictionary<uint, string[]>();
            if (isRequired)
            {
                if (!U.IsRequiredFileExist(fullfilename))
                {
                    return dic;
                }
            }
            else
            {
                if (! File.Exists(fullfilename))
                {
                    return dic;
                }
            }

            try
            {
                using (StreamReader reader = File.OpenText(fullfilename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (U.IsComment(line) || U.OtherLangLine(line))
                        {
                            continue;
                        }
                        line = U.ClipComment(line);
                        if (line == "")
                        {
                            continue;
                        }
                        string[] sp = line.Split('\t');
                        if (sp.Length < 1)
                        {
                            continue;
                        }
                        dic[U.atoh(sp[0])] = U.subrange(sp, 1, (uint)sp.Length);
                    }
                }
            }
            catch (Exception e)
            {
                R.ShowStopError("必須となる設定ファイルを読みこめません。\r\n設定ファイルが壊れている可能性があります。\r\n再ダウンロードしなおしてください。\r\n{0}\r\n{1}", fullfilename, e.ToString());
            }
            return dic;
        }

        public static void SaveTSVResource1(string fullfilename, Dictionary<uint, string> data)
        {
            string dir = Path.GetDirectoryName(fullfilename);
            if (!Directory.Exists(dir))
            {
                U.mkdir(dir);
            }
            if (!U.CanWriteFileRetry(fullfilename))
            {
                return;
            }
            try
            {
                using (StreamWriter w = new StreamWriter(fullfilename))
                {
                    foreach (var pair in data)
                    {
                        string line = U.ToHexString(pair.Key) + "\t" + pair.Value;
                        w.WriteLine(line);
                    }
                }
            }
            catch (Exception e)
            {
                R.ShowStopError("ファイルに書き込めません。\r\n{0}\r\n{1}", fullfilename, e.ToString());
            }
        }

        public static Dictionary<uint, string> LoadTSVResource1(string fullfilename, bool isRequired = true)
        {
            Dictionary<uint, string> dic = new Dictionary<uint, string>();
            if (isRequired)
            {
                if (!U.IsRequiredFileExist(fullfilename))
                {
                    return dic;
                }
            }
            else
            {
                if (!File.Exists(fullfilename))
                {
                    return dic;
                }
            }

            try
            {
                using (StreamReader reader = File.OpenText(fullfilename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (U.IsComment(line) || U.OtherLangLine(line))
                        {
                            continue;
                        }
                        line = U.ClipComment(line);
                        if (line == "")
                        {
                            continue;
                        }
                        string[] sp = line.Split('\t');
                        if (sp.Length < 2)
                        {
                            continue;
                        }
                        dic[U.atoh(sp[0])] = sp[1];
                    }
                }
            }
            catch (Exception e)
            {
                R.ShowStopError("必須となる設定ファイルを読みこめません。\r\n設定ファイルが壊れている可能性があります。\r\n再ダウンロードしなおしてください。\r\n{0}\r\n{1}", fullfilename, e.ToString());
            }
            return dic;
        }
        public static Dictionary<uint, string> LoadConfigEtcTSV1(string type)
        {
            return U.LoadTSVResource1(U.ConfigEtcFilename(type), false);
        }

        public static void SaveConfigEtcTSV1(string type, Dictionary<uint, string> dic,string romBaseFilename)
        {
            string fullfilename = U.ConfigEtcFilename(type, romBaseFilename);
            if (dic.Count <= 0)
            {//0件なら消す.
                if (File.Exists(fullfilename))
                {
                    File.Delete(fullfilename);
                }
                return;
            }
            U.SaveTSVResource1(fullfilename, dic);
        }

        //オプション引数 --mode=foo とかを、dic["--mode"]="foo" みたいに変換します. 
        public static Dictionary<string,string> OptionMap(string[] args,string defautFilenameOption)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Length <= 2)
                {
                    continue;
                }
                if (args[i][0] == '-' && args[i][1] == '-')
                {
                    int a = args[i].IndexOf('=');
                    if (a <= 0)
                    {//値なし 作る予定はないが...
                        dic[args[i]] = "";
                    }
                    else
                    {
                        dic[U.substr(args[i],0,a)] = args[i].Substring(a + 1);
                    }
                }
                else if (File.Exists(args[i]))
                {//引数がない けど、ファイル名ならば
                    dic[defautFilenameOption] = args[i];
                }
                else
                {
                    dic[""] = args[i];
                }
            }
            return dic;
        }

        //see http://dobon.net/vb/dotnet/programing/arraycompare.html
        [DllImport("msvcrt.dll",
            CallingConvention = CallingConvention.Cdecl)]
        public static extern int memcmp(byte[] b1, byte[] b2, IntPtr count);
        public static int memcmp(byte[] a, byte[] b)
        {
            if (object.ReferenceEquals(a, b))
            {
                return -1;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return -1;
            }

            return memcmp(a, b, (IntPtr)a.Length);
        }


        [DllImport("msvcrt.dll",
            CallingConvention = CallingConvention.Cdecl)]
        public static extern int memcmp(IntPtr b1, IntPtr b2, IntPtr size);

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        //一時的にカレントディレクトリを移動する.
        public class ChangeCurrentDirectory : IDisposable
        {
            string current_dir;
            public ChangeCurrentDirectory(string dir)
            {
                current_dir = Directory.GetCurrentDirectory();
                Directory.SetCurrentDirectory(Path.GetDirectoryName(dir));
            }
            public void Dispose()
            {
                Directory.SetCurrentDirectory(current_dir);
            }
        }

        public class ActiveControlSave : IDisposable
        {
            Form CurrentForm;
            Control CurrentControl;
            public ActiveControlSave(Form f)
            {
                CurrentForm = f;
                CurrentControl = CurrentForm.ActiveControl;
            }
            public void Dispose()
            {
                try
                {
                    CurrentForm.ActiveControl = CurrentControl;
                }
                catch(Exception e)
                {
                    Log.Error(R.ExceptionToString(e));
                }
            }
        }

        public static string GetRelativePath(string uri1, string uri2)
        {
            Uri u1 = new Uri(uri1);
            Uri u2 = new Uri(uri2);

            Uri relativeUri = u1.MakeRelativeUri(u2);

            string relativePath = relativeUri.ToString();

            relativePath = relativePath.Replace('/', '\\');
            relativePath = Uri.UnescapeDataString(relativePath);

            return (relativePath);
        }
        public static string UrlDecode(string urlString)
        {
            return Uri.UnescapeDataString(urlString);
        }

        public static string FindFileOne(string path, string toolname)
        {
            string[] files = U.Directory_GetFiles_Safe(path, toolname, SearchOption.AllDirectories);
            if (files.Length <= 0)
            {
                return "";
            }
            return files[0];
        }
        public static byte[] ResizeArray(byte[] src, uint datasize)
        {
            byte[] dest = new byte[datasize];
            int size;
            if (datasize < src.Length)
            {
                size = (int)datasize;
            }
            else
            {
                size = src.Length;
            }
            Array.Copy(src, dest, size);
            return dest;
        }
        public static string[] removeEmptyValue(string[] src,string removedata="")
        {
            List<string> dest = new List<string>(src.Length);
            for (int i = 0; i < src.Length; i++)
            {
                if (src[i] == removedata)
                {
                    continue;
                }
                dest.Add(src[i]);
            }
            return dest.ToArray();
        }
        public static uint ConvertCharToByte(string one)
        {
            uint search_char;
            byte[] sjisstr = Program.SystemTextEncoder.Encode(one);
            if (sjisstr.Length >= 2)
            {
                search_char = (uint)((sjisstr[0] << 8) + sjisstr[1]);
            }
            else
            {
                search_char = sjisstr[0];
            }
            return search_char;
        }
        public static Color ColorFromName(string name)
        {
            //ディフォルトの Color.FromName は16進数指定するとバグるので自分で作る... 
            if(U.isHexString(name))
            {
                uint colorcode = U.atoh(name);
                Color color = Color.FromArgb((int)colorcode);
                return color;
            }
            else
            {
                Color color = Color.FromName(name);
                return color;
            }
        }

        //まともなsubstr 長さがたりなかったときに例外ではなく、末尾までを返す.
        public static string substr(string str,int start,int length)
        {
            if (start < 0)
            {
                start = str.Length - start;
                if (start < 0)
                {
                    start = 0;
                }
            }
            if (start >= str.Length)
            {
                return "";
            }
            if (start + length >= str.Length)
            {
                if (str.Length < start)
                {
                    length = 0;
                }
                else
                {
                    length = str.Length - start;
                }
            }
            if (length < 0)
            {
                return "";
            }
            return str.Substring(start, length);
        }
        public static string substr(string str, int start)
        {
            if (start < 0)
            {
                start = str.Length + start;
                if (start < 0)
                {
                    start = 0;
                }
            }
            if (start >= str.Length)
            {
                return "";
            }
            return str.Substring(start);
        }
        public static string InnerSplit(string text,string split,int get_count)
        {
            string[] sp = text.Split(new string[]{ split },StringSplitOptions.None);
            if (get_count >= sp.Length)
            {
                return "";
            }
            return sp[get_count];
        }

        [DllImport("user32.dll")]
        public static extern int IsWindowEnabled(IntPtr hwnd);

        public static bool IsProcessExit(Process p)
        {
            if (p.HasExited)
            {
                return true;
            }
            //どうも、HasExited や Exitedイベントには数秒のタイムラグがあるらしい...
            if (IsWindowEnabled(p.MainWindowHandle) == 0)
            {//メインウィンドウが無効なので終了していると思われる.
                return true;
            }
            return false;
        }
        public static void CopyItem(ComboBox src,ComboBox dest)
        {
            dest.BeginUpdate();
            foreach (string s in src.Items)
            {
                dest.Items.Add(s);
            }
            dest.EndUpdate();
        }
        public static void SelectCombo(ComboBox combo, uint stat1)
        {
            for (int i = 0; i < combo.Items.Count; i++)
            {
                if (U.atoh((string)combo.Items[i]) == stat1)
                {
                    combo.SelectedIndex = i;
                    return;
                }
            }
            combo.SelectedIndex = -1;
            return;
        }

        public static string DumpByte(uint[] data)
        {
            string ret = "";
            for (int i = 0; i < data.Length; i++)
            {
                ret += " 0x" + data[i].ToString("X");
            }
            if (ret.Length > 0)
            {
                return ret.Substring(1);
            }
            return ret;
        }
        public static string DumpByte(byte[] data)
        {
            string ret = "";
            for (int i = 0; i < data.Length; i++)
            {
                ret += " 0x" + data[i].ToString("X");
            }
            if (ret.Length > 0)
            {
                return ret.Substring(1);
            }
            return ret;
        }
        public static bool stringbool(string s)
        {
            string ss = s.ToLower().Trim();
            if (ss == "false")
            {
                return false;
            }
            if (ss == "0")
            {
                return false;
            }
            if (ss == "no")
            {
                return false;
            }
            return true;
        }
        public static string SelectValueComboboxText(string str)
        {
            string[] sp = str.Split('=');
            return sp[0];
        }
        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
        //改行を消す.
        public static string nl2none(string lines)
        {
            return lines.Replace("\r\n", "");
        }
        //改行を\r\nに.
        public static string nl2br(string lines)
        {
            return lines.Replace("\\r\\n", "\r\n");
        }
        public static string br2nl(string lines)
        {
            return lines.Replace("\r\n", "\\r\\n");
        }
        //コントロールからFormまたは、ユーザコントロールへの逆変換
        public static Control ControlToParentFormOrUserControl(Control c)
        {
            if (c == null)
            {
                return null;
            }
            if (c is Form)
            {
                return c;
            }
            if (c is UserControl)
            {
                return c;
            }
            Control pc = c.Parent;
            return ControlToParentFormOrUserControl(pc);
        }
        //コントロールからFormへの逆変換
        public static Form ControlToParentForm(Control c)
        {
            if (c == null)
            {
                return null;
            }
            if (c is Form)
            {
                return (Form)c;
            }
            Control pc = c.Parent;
            return ControlToParentForm(pc);
        }
        //コントロールのRectをFormのRectにする.
        public static Rectangle ControlRectToFormRect(Control c,Rectangle rc)
        {
            if (c == null)
            {
                return rc;
            }
            if (c is Form)
            {
                return rc;
            }
            Point pt = c.Location;
            rc = new Rectangle(
                 rc.Left + pt.X
                , rc.Top + pt.Y
                , rc.Right + pt.X
                , rc.Bottom + pt.Y
                );

            Control pc = c.Parent;
            return ControlRectToFormRect(pc, rc);
        }
        //英語版FEにはUnicodeの1バイトだけ表記があるらしい
        [MethodImpl(256)]
        public static bool IsEnglishSPCode(byte code)
        {
            return (code >= 0x81 || code == 0x1f || code == 0x7B || code == 0x7C || code == 0x7D || code == 0x7F);
        }
        [MethodImpl(256)]
        public static bool isUTF8PreCode(byte code, byte code2)
        {
            return code >= 0xC0 && code2 >= 0x80;
        }

        [MethodImpl(256)]
        public static bool isSJIS1stCode(byte c)
        {
            if (((0x81 <= c && c <= 0x9f) || (0xe0 <= c && c <= 0xfc)))
            {
                return true;
            }
            return false;
        }
        [MethodImpl(256)]
        public static bool isSJIS2ndCode(byte c)
        {
            if (((0x40 <= c && c <= 0x7e) || (0x80 <= c && c <= 0xfc)))
            {
                return true;
            }
            return false;
        }

        //みんな大好きPHPのhtmlspecialchars
        //タグをエスケープ 基本的に PHP の htmlspecialchars と同じ.
        //http://search.net-newbie.com/php/function.htmlspecialchars.html
        public static string htmlspecialchars(string inStr)
        {
            string a = inStr; 
            a = a.Replace(">", "&gt;");
            a = a.Replace("<", "&lt;");
            a = a.Replace("\"", "&quot;");
            a = a.Replace("'", "&apos;");
            return a;
        }
        //htmlspecialcharsの逆変換
        public static string unhtmlspecialchars(string inStr)
        {
            string a = inStr;
            a = a.Replace("&gt;", ">");
            a = a.Replace("&lt;", "<");
            a = a.Replace("&quot;", "\"");
            a = a.Replace("&apos;", "'");

            a = a.Replace("&#45;", "-");
            
            return a;
        }

        public static Bitmap Zoom(Bitmap bitmap,int zoom = 0)
        {
            if (zoom <= 0)
            {//拡大しない
                return bitmap;
            }
            zoom += 1;

            try
            {
                int width = zoom * (int)bitmap.Width;
                int height = zoom * (int)bitmap.Height;

                Bitmap zoomPic = new Bitmap(width, height);
                using (Graphics g = Graphics.FromImage(zoomPic))
                {
                    g.DrawImage(bitmap, 0, 0, zoomPic.Width, zoomPic.Height);
                }
                return zoomPic;
            }
            catch (ExternalException e)
            {//まれにGDI+内部でエラーが発生することがるらしい.原因不明
                Log.Error(R.ExceptionToString(e));
                Debug.Assert(false);
                return bitmap;
            }
            catch (System.OutOfMemoryException e)
            {//まれにGDI+内部でエラーが発生することがるらしい.原因不明
                Log.Error("GDI+ OutOfMemoryException", e.ToString(), e.StackTrace);
                Debug.Assert(false);
                return bitmap;
            }
        }


        public static string getVersion()
        {
            //see   ビルド時間の取得
            //http://devlights.hatenablog.com/entry/2015/04/14/230841 

            var asm = typeof(U).Assembly;
            var ver = asm.GetName().Version;

            var build = ver.Build;
            var revision = ver.Revision;
            var baseDate = new DateTime(2000, 1, 1);

            return baseDate.AddDays(build).AddSeconds(revision * 2).ToString("yyyyMMdd.HH");
        }
        //NumUpDownControlの範囲を安全に再設定します
        public static void SelectedIndexSafety(NumericUpDown nud, int value)
        {
            if (value > nud.Maximum)
            {
                value = (int)nud.Maximum;
            }
            if (value < nud.Minimum)
            {
                value = (int)nud.Minimum;
            }
            SelectedIndexSafety(nud, (int)nud.Maximum, (int)nud.Minimum, value);
        }
        public static void SelectedIndexSafety(NumericUpDown nud, uint value)
        {
            if (value > nud.Maximum)
            {
                value = (uint)nud.Maximum;
            }
            if (value < nud.Minimum)
            {
                value = (uint)nud.Minimum;
            }
            SelectedIndexSafety(nud, (uint)nud.Maximum, (uint)nud.Minimum, value);
        }

        //NumUpDownControlの範囲を安全に再設定します
        public static void SelectedIndexSafety(NumericUpDown nud, int min, int max, int value)
        {
            if (min > max)
            {
                int temp;
                temp = min;
                min = max;
                max = temp;
            }
            if (min > value)
            {
                min = value;
            }
            if (max < value)
            {
                max = value;
            }

            if (nud.Maximum < min)
            {
                if (nud.Value > max)
                {
                    nud.Value = nud.Minimum;
                    nud.Maximum = max;
                }
                else
                {
                    nud.Maximum = max;
                }
                if (nud.Value < min)
                {
                    nud.Value = nud.Maximum;
                    nud.Minimum = min;
                }
                else
                {
                    nud.Minimum = min;
                }
            }
            else
            {
                if (nud.Value < min)
                {
                    nud.Value = nud.Maximum;
                    nud.Minimum = min;
                }
                else
                {
                    nud.Minimum = min;
                }
                if (nud.Value > max)
                {
                    nud.Value = nud.Minimum;
                    nud.Maximum = max;
                }
                else
                {
                    nud.Maximum = max;
                }
            }

            try
            {
                ForceUpdate(nud, value);
            }
            catch (ArgumentException e)
            {
                Log.Error(R.ExceptionToString(e));
                Debug.Assert(false);
            }
        }

        //NumUpDownControlの範囲を安全に再設定します uintバージョン
        public static void SelectedIndexSafety(NumericUpDown nud, uint min, uint max, uint value)
        {
            if (min > max)
            {
                uint temp;
                temp = min;
                min = max;
                max = temp;
            }
            if (min > value)
            {
                min = value;
            }
            if (max < value)
            {
                max = value;
            }

            if (nud.Maximum < min)
            {
                if (nud.Value > max)
                {
                    nud.Value = nud.Minimum;
                    nud.Maximum = max;
                }
                else
                {
                    nud.Maximum = max;
                }
                if (nud.Value < min)
                {
                    nud.Value = nud.Maximum;
                    nud.Minimum = min;
                }
                else
                {
                    nud.Minimum = min;
                }
            }
            else
            {
                if (nud.Value < min)
                {
                    nud.Value = nud.Maximum;
                    nud.Minimum = min;
                }
                else
                {
                    nud.Minimum = min;
                }
                if (nud.Value > max)
                {
                    nud.Value = nud.Minimum;
                    nud.Maximum = max;
                }
                else
                {
                    nud.Maximum = max;
                }
            }

            try
            {
                ForceUpdate(nud, value);
            }
            catch (ArgumentException e)
            {
                Log.Error(R.ExceptionToString(e));
                Debug.Assert(false);
            }
        }
        
        public static void append_vlength_code(List<byte> data, int time)
        {//gba_mus_riper_v24より
            char word1 = (char)(time & 0x7f);
            char word2 = (char)((time >> 7) & 0x7f);
            char word3 = (char)((time >> 14) & 0x7f);
            char word4 = (char)((time >> 21) & 0x7f);

            if (word4 != 0)
            {
                data.Add((byte)(word4 | 0x80));
                data.Add((byte)(word3 | 0x80));
                data.Add((byte)(word2 | 0x80));
            }
            else if (word3 != 0)
            {
                data.Add((byte)(word3 | 0x80));
                data.Add((byte)(word2 | 0x80));
            }
            else if (word2 != 0)
            {
                data.Add((byte)(word2 | 0x80));
            }
            data.Add((byte)(word1));
        }
        public static uint read_vlength_code(byte[] data, uint pos, out uint out_pos)
        {
            uint ret = (uint)(data[pos] & 0x7F);
            for (; (data[pos] & 0x80) == 0x80; )
            {
                pos++;
                ret = (ret << 7) | ((uint)(data[pos] & 0x7F)) ;
            }
            out_pos = pos + 1;
            return ret;
        }


        //CRC32計算
        //see http://kagasu.hatenablog.com/entry/2016/11/21/202302
        public class CRC32
        {
            private const int TABLE_LENGTH = 256;
            private uint[] crcTable;

            public CRC32()
            {
                BuildCRC32Table();
            }

            private void BuildCRC32Table()
            {
                crcTable = new uint[256];
                for (uint i = 0; i < 256; i++)
                {
                    var x = i;
                    for (var j = 0; j < 8; j++)
                    {
                        x = (uint)((x & 1) == 0 ? x >> 1 : -306674912 ^ x >> 1);
                    }
                    crcTable[i] = x;
                }
            }

            public uint Calc(byte[] buf)
            {
                uint num = uint.MaxValue;
                for (var i = 0; i < buf.Length; i++)
                {
                    num = crcTable[(num ^ buf[i]) & 255] ^ num >> 8;
                }

                return (uint)(num ^ -1);
            }
        }

        public static void SelectFileByExplorer(string path,bool isExport = true)
        {
            if (path == "" || File.Exists(path) == false)
            {
                return;
            }

            if (isExport)
            {//エクスポート時
                if (OptionForm.select_in_explorer_when_export_enum.None == OptionForm.select_in_explorer_when_export())
                {//エクスプローラーを開かない.
                    return;
                }
            }

            try
            {
                string filename = U.escape_shell_args(path);
                Process.Start("EXPLORER.EXE", "/select," + filename);
            }
            catch (Exception ee)
            {
                R.ShowStopError(ee.ToString());
            }
        }
        //ひらがな判定
        //see https://dobon.net/vb/dotnet/string/ishiragana.html
        public static bool isHiragana(char c)
        {
            //「ぁ」～「より」までと、「ー」「ダブルハイフン」をひらがなとする
            return ('\u3041' <= c && c <= '\u309F')
                    || c == '\u30FC' || c == '\u30A0';
        }

        static string GenUserAgent()
        {
            System.OperatingSystem os = System.Environment.OSVersion;

            uint seed = U.atoi(DateTime.Now.ToString("yyMMddHH"));

            Random rand = new Random((int)seed);
            int SafariMinorVersion = 537;
            int SafariMajorVersion = 36;
            int Chrome1Version = 65;
            int Chrome2Version = 0;
            int Chrome3Version = 2107;
            int Chrome4Version = 108;

            string UserAgent = string.Format("Mozilla/5.0 (Windows NT {0}.{1}; Win64; x64) AppleWebKit/{2}.{3} (KHTML, like Gecko) Chrome/{4}.{5}.{6}.{7} Safari/{2}.{3}"
                ,os.Version.Major//Windows 8では、「6」//OSのメジャーバージョン番号を表示する
                ,os.Version.Minor//Windows 8では、「2」//OSのマイナーバージョン番号を表示する
                , SafariMinorVersion
                , SafariMajorVersion
                , Chrome1Version
                , Chrome2Version
                , Chrome3Version
                , Chrome4Version
                );
            return UserAgent;
        }

        static HttpWebRequest HttpMakeRequest(string url, string referer, System.Net.CookieContainer cookie = null)
        {
            ServicePointManager.ServerCertificateValidationCallback = OnRemoteCertificateValidationCallback;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //TLS 1.2 

            string UserAgent = GenUserAgent(); //"Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            if (OptionForm.proxy_server_when_connecting() == OptionForm.proxy_server_when_connecting_enum.None)
            {//自動プロキシ検出を利用しない.
             //こちらの方が早くなります.
                request.Proxy = null;
            }

            //貴方の好きなUAを使ってね。
            request.UserAgent = UserAgent;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            if (referer != "")
            {
                request.Referer = referer;
            }
            if (cookie != null)
            {
                request.CookieContainer = new System.Net.CookieContainer();
                request.CookieContainer.Add(cookie.GetCookies(request.RequestUri));
            }
            return request;
        }

        public static string GetURLBaseServer(string baseurl)
        {
            Uri uri = new Uri(baseurl);
            return uri.Scheme + "://" + uri.Authority + "/";
        }
        public static string GetURLBaseDir(string baseurl)
        {
            Uri uri = new Uri(baseurl);
            return uri.Scheme + "://" + uri.Authority + Path.GetDirectoryName(uri.LocalPath).Replace("\\","/") + "/" ;
        }
        public static string GetURLFilename(string baseurl)
        {
            Uri uri = new Uri(baseurl);
            return Path.GetFileName(uri.LocalPath);
        }
#if DEBUG
        public static void TEST_GetURLBaseServer()
        {
            string r = GetURLBaseServer("http://foo.local/aaa/ddd/file.zip?dl=123");
            Debug.Assert(r == "http://foo.local/");
        }
        public static void TEST_GetURLBaseDir()
        {
            string r = GetURLBaseDir("http://foo.local/aaa/ddd/file.zip?dl=123");
            Debug.Assert(r == "http://foo.local/aaa/ddd/");
        }
        public static void TEST_GetURLFilename()
        {
            string r = GetURLFilename("http://foo.local/aaa/ddd/file.zip?dl=123");
            Debug.Assert(r == "file.zip");
        }
#endif

        //URLを整形してフルパスにします.
        public static string MakeFullURLPath(string baseurl,string targeturl)
        {
            if (targeturl.IndexOf("http") == 0)
            {//フルパス
                return targeturl;
            }
            Uri uri = new Uri(baseurl);
            if (targeturl.IndexOf("/") == 0)
            {
                return uri.Scheme + "://" + uri.Authority + targeturl;
            }
            return uri.Scheme + "://" + uri.Authority + Path.GetDirectoryName(uri.LocalPath) + "/" + targeturl;
        }

        //https://qiita.com/Takezoh/items/3eff6806a59152656ddc
        //MONOには証明書が入っていないので別処理
        private static bool OnRemoteCertificateValidationCallback(
          object sender,
          X509Certificate certificate,
          X509Chain chain,
          SslPolicyErrors sslPolicyErrors)
        {
            //危険だけど継続する
            return true;
        }

        //httpでそこそこ怪しまれずに通信する
        public static string HttpGet(string url, string referer = "", System.Net.CookieContainer cookie = null)
        {
            HttpWebRequest request = HttpMakeRequest(url, referer, cookie);
            string r = "";

            WebResponse rsp = request.GetResponse();
            Stream stm = rsp.GetResponseStream();
            if (stm != null)
            {
                StreamReader reader = new StreamReader(stm, Encoding.UTF8);
                r = reader.ReadToEnd();
                stm.Close();
            }
            rsp.Close();

            if (cookie != null)
            {
                System.Net.CookieCollection cookies = request.CookieContainer.GetCookies(request.RequestUri);
                cookie.Add(cookies);
            }

            return r;
        }

        public static void HttpDownload(string savefilename, string url, string referer = "", InputFormRef.AutoPleaseWait pleaseWait = null, System.Net.CookieContainer cookie = null)
        {
            HttpWebRequest request = HttpMakeRequest(url, referer, cookie);

            WebResponse rsp = request.GetResponse();
            using (Stream output = File.OpenWrite(savefilename))
            using (Stream input = rsp.GetResponseStream())
            {
                byte[] buffer = new byte[1024*8];
                int totalSize = (int)rsp.ContentLength;
                int readTotalSize = 0;
                int bytesRead;
                while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    output.Write(buffer, 0, bytesRead);

                    if (pleaseWait != null)
                    {
                        readTotalSize += bytesRead;
                        if (totalSize == -1)
                        {
                            pleaseWait.DoEvents("Download: " + readTotalSize + "/" + "???");
                        }
                        else
                        {
                            pleaseWait.DoEvents("Download: " + readTotalSize + "/" + totalSize);
                        }
                    }
                }
            }

            rsp.Close();

            if (cookie != null)
            {
                System.Net.CookieCollection cookies = request.CookieContainer.GetCookies(request.RequestUri);
                cookie.Add(cookies);
            }
        }
        public static string HttpPost(string url, Dictionary<string, string> args, string referer = "", System.Net.CookieContainer cookie = null)
        {
            bool isFirst = true;
            StringBuilder sb = new StringBuilder();
            foreach (var a in args)
            {
                if (!isFirst)
                {
                    sb.Append('&');
                }
                sb.Append(Uri.EscapeUriString(a.Key));
                sb.Append('=');
                sb.Append(Uri.EscapeUriString(a.Value));
                isFirst = false;
            }

            string postArgs = sb.ToString();
            byte[] data = Encoding.ASCII.GetBytes(postArgs);

            HttpWebRequest request = HttpMakeRequest(url, referer, cookie);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            Stream reqstm = request.GetRequestStream();
            //POST送信
            reqstm.Write(data, 0, data.Length);
            reqstm.Close();

            string r = "";
            WebResponse rsp = request.GetResponse();
            Stream stm = rsp.GetResponseStream();
            if (stm != null)
            {
                StreamReader reader = new StreamReader(stm, Encoding.UTF8);
                r = reader.ReadToEnd();
                stm.Close();
            }
            rsp.Close();

            if (cookie != null)
            {
                System.Net.CookieCollection cookies = request.CookieContainer.GetCookies(request.RequestUri);
                cookie.Add(cookies);
            }

            return r;
        }

        public static string HttpUpload(string url, Dictionary<string, string> args, string fileid, string filename, string referer = "")
        {
            //区切り文字列
            string boundary = System.Environment.TickCount.ToString();

            StringBuilder sb = new StringBuilder();
            foreach (var a in args)
            {
                sb.Append("--" + boundary + "\r\n");
                sb.Append(Uri.EscapeUriString(a.Key));
                sb.Append('=');
                sb.Append(Uri.EscapeUriString(a.Value));
            }
            sb.Append("--" + boundary + "\r\n");
            sb.Append("Content-Disposition: form-data; name=\"" + fileid  + "\"; filename=\"" + Path.GetExtension(filename) + "\"\r\n");
            sb.Append("Content-Type: application/octet-stream\r\n");
            sb.Append("Content-Transfer-Encoding: binary\r\n\r\n");
            //バイト型配列に変換
            byte[] startData = Encoding.ASCII.GetBytes(sb.ToString());
            byte[] fileData = File.ReadAllBytes(filename);
            byte[] endData = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

            HttpWebRequest request = HttpMakeRequest(url, referer);
            request.Method = "POST";
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.ContentLength = startData.Length + fileData.Length + endData.Length;
            Stream reqstm = request.GetRequestStream();
            //POST送信
            reqstm.Write(startData, 0, startData.Length);
            reqstm.Write(fileData, 0, fileData.Length);
            reqstm.Write(endData, 0, endData.Length);
            reqstm.Close();

            string r = "";
            WebResponse rsp = request.GetResponse();
            Stream stm = rsp.GetResponseStream();
            if (stm != null)
            {
                StreamReader reader = new StreamReader(stm, Encoding.UTF8);
                r = reader.ReadToEnd();
                stm.Close();
            }
            rsp.Close();

            return r;
        }

        public static string HttpUploadFirebase(string buket, Dictionary<string, string> args, string fileid, string filename)
        {
            string url = "https://firebasestorage.googleapis.com/v0/b/" + buket + ".appspot.com/o/" + fileid;
            string referer = "https://" + buket + ".firebaseapp.com/";

            //区切り文字列
            string boundary = System.Environment.TickCount.ToString();

            StringBuilder sb = new StringBuilder();
            foreach (var a in args)
            {
                sb.Append("--" + boundary + "\r\n");
                sb.Append(Uri.EscapeUriString(a.Key));
                sb.Append('=');
                sb.Append(Uri.EscapeUriString(a.Value));
            }

            sb.Append("--" + boundary + "\r\n");
            sb.Append("Content-Type: application/octet-stream\r\n\r\n");
            //バイト型配列に変換
            byte[] startData = Encoding.ASCII.GetBytes(sb.ToString());
            byte[] fileData = File.ReadAllBytes(filename);
            byte[] endData = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

            HttpWebRequest request = HttpMakeRequest(url, referer);
            request.Method = "POST";
            request.ContentType = "multipart/related; boundary=" + boundary;
            request.ContentLength = startData.Length + fileData.Length + endData.Length;
            Stream reqstm = request.GetRequestStream();
            //POST送信
            reqstm.Write(startData, 0, startData.Length);
            reqstm.Write(fileData, 0, fileData.Length);
            reqstm.Write(endData, 0, endData.Length);
            reqstm.Close();

            string r = "";
            WebResponse rsp = request.GetResponse();
            Stream stm = rsp.GetResponseStream();
            if (stm != null)
            {
                StreamReader reader = new StreamReader(stm, Encoding.UTF8);
                r = reader.ReadToEnd();
                stm.Close();
            }
            rsp.Close();

            return r;
        }

        public static WebResponse HttpHead(string url, string referer = "", System.Net.CookieContainer cookie = null)
        {
            HttpWebRequest request = HttpMakeRequest(url, referer, cookie);
            request.Method = "HEAD";
            WebResponse resp = request.GetResponse();
            resp.Close();

            return resp;
        }
        static void DownloadFileByDirect(string save_filename, string download_url, InputFormRef.AutoPleaseWait pleaseWait)
        {
            U.HttpDownload(save_filename, download_url, Path.GetDirectoryName(download_url), pleaseWait);
        }
        static void DownloadFileByDropbox(string save_filename, string download_url, InputFormRef.AutoPleaseWait pleaseWait)
        {
            if (download_url.IndexOf("dl=1") >= 0)
            {
                DownloadFileByDirect(save_filename, download_url,pleaseWait);
                return;
            }
            CookieContainer cookie = new CookieContainer();
            HttpGet(download_url, Path.GetDirectoryName(download_url), cookie);

            //dl=0があれば削る
            download_url = download_url.Replace("dl=0", "");
            //?がないなら追加
            if (download_url.IndexOf("?") < 0)
            {
                download_url = download_url + "?";
            }
            //dl=1をつけてダウンロードページへ
            string url = download_url + "dl=1";
            //ダウンロード開始
            U.HttpDownload(save_filename, url, download_url, pleaseWait, cookie);
        }

        static void DownloadFileByMediafire(string save_filename, string download_url, InputFormRef.AutoPleaseWait pleaseWait)
        {
            CookieContainer cookie = new CookieContainer();
            string html = HttpGet(download_url, Path.GetDirectoryName(download_url), cookie);

            string html2 = U.skip(html, "aria-label=\"Download file\"");
            string regex = "href=\"([^\"]+)\"";
            Match m = RegexCache.Match(html2, regex);
            if (m.Groups.Count < 2)
            {
                throw new Exception(R._("MediafireのDOWNLOADボタンが見つかりませんでした。\r\n仕様が変更されたようです。FEBuilderGBAの開発者に連絡してください。\r\n")+"\r\n" + html);
            }
            string url = m.Groups[1].ToString();
            U.HttpDownload(save_filename, url, download_url, pleaseWait);
        }
        
        static void DownloadFileByGoogleDrive(string save_filename, string download_url, InputFormRef.AutoPleaseWait pleaseWait)
        {
            string id;
            if (download_url.IndexOf("/file/d/") >= 0)
            {
                System.Text.RegularExpressions.Match m = RegexCache.Match(download_url, "/file/d/([^/]+)");
                id = m.Groups[1].ToString();
            }
            else if (download_url.IndexOf("/drive/folders") >= 0)
            {
                DownloadFoldersByGoogleDrive(save_filename, download_url, pleaseWait);
                return ;
            }
            else if (download_url.IndexOf("id=") >= 0)
            {
                System.Text.RegularExpressions.Match m = RegexCache.Match(download_url, "id=([^&]+)");
                id = m.Groups[1].ToString();
            }
            else
            {
                throw new Exception(R._("Google DriveのIDが見つかりません"));
            }

            string url = "https://drive.google.com/uc?export=download&id=" + id;
            U.HttpDownload(save_filename, url, download_url, pleaseWait);
        }
        static void DownloadFoldersByGoogleDrive(string save_filename, string download_url, InputFormRef.AutoPleaseWait pleaseWait)
        {
            using (U.MakeTempDirectory dir = new MakeTempDirectory())
            {
                pleaseWait.DoEvents("download folders.");
                string html = U.HttpGet(download_url);
                html = U.skip(html, "window['_DRIVE_ivd'] = ");
                string[] sp = html.Split(new string[] { "\\x5b\\x22" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 1; i + 1 < sp.Length; i += 2)
                {
                    string id = U.cut(sp[i], "\\x22");
                    string filename = U.cut(sp[i + 1], ",\\x22", "\\x22,");
                    string date = U.cut(sp[i + 1], ",0,0,0,", ",");

                    filename = U.escape_filename(filename);
                    filename = Path.Combine(dir.Dir, filename);


                    string url = "https://drive.google.com/uc?export=download&id=" + id;
                    U.HttpDownload(filename, url, download_url, pleaseWait);

                    //日付を設定
                    if (File.Exists(filename))
                    {
                        DateTime datetime;
                        if (TryParseUnitTime(date, out datetime))
                        {
                            U.SetTimeStamp(filename, datetime);
                        }
                    }
                }


                pleaseWait.DoEvents("compress");
                string save_filename_7z = save_filename + ".7z";
                ArchSevenZip.Compress(save_filename_7z, dir.Dir, 0);
                U.Move(save_filename_7z, save_filename);
            }

        }

        public static void Move(string src, string dest)
        {
            if (src == dest)
            {
                return;
            }
            File.Copy(src, dest,true);
            File.Delete(src);
        }

        public static bool TryParseUnitTime(string date, out DateTime retDateTime)
        {
            date = date.Trim();
            if (!U.isNumString(date))
            {
                retDateTime = DateTime.Now;
                return false;
            }
            if (date.Length >= 10 + 6)
            {//dropboxのミリ秒まで含めた時刻
                date = date.Substring(0, date.Length - 6);
            }
            else if (date.Length >= 10 + 3)
            {//googledriveの特殊な時刻
                date = date.Substring(0, date.Length - 3);
            }

            uint dateuint = U.atoi(date);
            if (dateuint < 1262271600)
            {//2010/1/1 以前
                retDateTime = DateTime.Now;
                return false;
            }
            DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            retDateTime = UNIX_EPOCH.AddSeconds(dateuint).ToLocalTime();
            return true;
        }

        static void DownloadFileByGetUploader(string save_filename, string download_url, InputFormRef.AutoPleaseWait pleaseWait)
        {
            string url = download_url;
            string contents = U.HttpGet(url);
            Log.Debug("front page:{0}", contents);
            contents = U.skip(contents, "name=\"token\"");
            string token = U.cut(contents, "value=\"", "\"");
            token = Uri.UnescapeDataString(token);
            token = U.unhtmlspecialchars(token);
            if (token.Length <= 8)
            {
                Log.Error("token NOT FOUND:", token);
                return;
            }

            Dictionary<string, string> args = new Dictionary<string, string>();
            args["token"] = token;
            contents = U.HttpPost(download_url, args, download_url);
            Log.Debug("download page:{0}", contents);
            contents = U.skip(contents, "http-equiv=\"refresh\"");
            string durl = U.cut(contents, "URL=", "\"");
            durl = Uri.UnescapeDataString(durl);
            durl = U.unhtmlspecialchars(durl);
            if (durl == "" && durl.IndexOf("http") < 0)
            {
                Log.Error("download url NOT FOUND:{0}", durl);
                return;
            }

            Log.Notify("download url:{0}", durl);
            U.HttpDownload(save_filename, durl, download_url, pleaseWait);
        }

        public static void DownloadFile(string save_filename, string download_url, InputFormRef.AutoPleaseWait pleaseWait)
        {
            if (download_url.IndexOf("getuploader") > 0)
            {
                U.DownloadFileByGetUploader(save_filename, download_url, pleaseWait);
            }
            else if (download_url.IndexOf("dropbox") > 0)
            {
                U.DownloadFileByDropbox(save_filename, download_url, pleaseWait);
            }
            else if (download_url.IndexOf("drive.google.com") > 0)
            {
                U.DownloadFileByGoogleDrive(save_filename, download_url, pleaseWait);
            }
            else if (download_url.IndexOf("mediafire.com") > 0)
            {
                U.DownloadFileByMediafire(save_filename, download_url, pleaseWait);
            }
            else
            {
                U.DownloadFileByDirect(save_filename, download_url, pleaseWait);
            }
        }

        public static string table_replace(string target, string[] table)
        {
            if (table == null)
            {
                Debug.Assert(false);
                return target;
            }

            string r = target;
            for (int i = 0; i < table.Length; i += 2)
            {
                r = r.Replace(table[i], table[i + 1]);
            }
            return r;
        }
        public static string table_replace_rev(string target, string[] table)
        {
            if (table == null)
            {
                Debug.Assert(false);
                return target;
            }

            string r = target;
            for (int i = 0; i < table.Length; i += 2)
            {
                r = r.Replace(table[i + 1], table[i]);
            }
            return r;
        }
        public static string table_replace_regex(string target, string[] table)
        {
            if (table == null)
            {
                Debug.Assert(false);
                return target;
            }

            string r = target;
            for (int i = 0; i < table.Length; i += 2)
            {
                r = RegexCache.Replace(r, table[i], table[i + 1]);
            }
            return r;
        }
        public static string table_replace_regex_rev(string target, string[] table)
        {
            if (table == null)
            {
                Debug.Assert(false);
                return target;
            }

            string r = target;
            for (int i = 0; i < table.Length; i += 2)
            {
                r = RegexCache.Replace(r, table[i + i], table[i]);
            }
            return r;
        }

        public static string table_replace(string target, List<string> table)
        {
            if (table == null)
            {
                Debug.Assert(false);
                return target;
            }

            string r = target;
            for (int i = 0; i < table.Count; i += 2)
            {
                r = r.Replace(table[i], table[i + 1]);
            }
            return r;
        }
        public static string table_replace_rev(string target, List<string> table)
        {
            if (table == null)
            {
                Debug.Assert(false);
                return target;
            }

            string r = target;
            for (int i = 0; i < table.Count; i += 2)
            {
                r = r.Replace(table[i + 1], table[i]);
            }
            return r;
        }
        public static string table_replace_regex(string target, List<string> table)
        {
            if (table == null)
            {
                Debug.Assert(false);
                return target;
            }

            string r = target;
            for (int i = 0; i < table.Count; i += 2)
            {
                r = RegexCache.Replace(r, table[i], table[i + 1]);
            }
            return r;
        }
        public static string table_replace_regex_rev(string target, List<string> table)
        {
            if (table == null)
            {
                Debug.Assert(false);
                return target;
            }

            string r = target;
            for (int i = 0; i < table.Count; i += 2)
            {
                r = RegexCache.Replace(r, table[i + i], table[i]);
            }
            return r;
        }

        [DllImport("user32.dll")]
        public static extern bool LockWindowUpdate(IntPtr hWndLock);

        public static bool mkdir(string dir)
        {
            if (Directory.Exists(dir))
            {
                try
                {
                    Directory.Delete(dir, true);
                }
                catch (Exception e)
                {
                    //ディレクトリがロックされていて消せない場合があるらしい
                    //その場合、作るという目的は達成しているので、まあいいかなあ。
                    Log.Error(e.ToString());
                    return false;
                }
            }
            Directory.CreateDirectory(dir);
            return true;
        }

        //Nビットで表現される マイナスありの数字にキャストします。
        public static int CastBit(uint a,int bitCount)
        {
            uint bit = (uint)(1 << bitCount);
            uint bitmask = bit - 1;
            int r;

            if ((a & bit) > 0)
            {//マイナス.
                r = -1 * (((int)(bitmask ^ (a & bitmask)) + 1));
            }
            else
            {
                r = (int)(a & bitmask);
            }
            return r;
        }
        public static string ToPlus(int a)
        {
            if (a < 0)
            {
                return "" + a;
            }
            return "+" + a;
        }

        //日本語探索
        static string ToMigemo(string str)
        {
            if (!U.isAsciiString(str))
            {
                return MultiByteJPUtil.mb_convert_kana(str, "HcasRX");
            }
            return str;
        }

        static string ToNarrowFontToAlpha(string str)
        {
            return MultiByteJPUtil.ConvertNarrowFontToAlpha(str);
        }


        static Dictionary<string, string> CleanupFindStringCache = new Dictionary<string, string>();
        public static string CleanupFindString(string str, bool isJP)
        {
            if (CleanupFindStringCache.ContainsKey(str))
            {
                return CleanupFindStringCache[str];
            }

            string s = CleanupFindStringLow(str , isJP);
            CleanupFindStringCache[str] = s;
            return s;
        }
#if DEBUG
        public static void TEST_CleanupFindStringLow_Narrow()
        {
            string r = "[_B][_r][_o]t[_h][_e][_r] [_S]w[_o][_r][_d]";
            r = CleanupFindString(r, isJP:false);
            Debug.Assert(r == "brother sword");
        }
#endif //DEBUG
        public static string ConvertNarrowFont(string str)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (U.isalhpa(c))
                {
                    sb.Append("[_");
                    sb.Append(c);
                    sb.Append("]");
                }
                else if (c == '[')
                {
                    for (; i < str.Length; i++)
                    {
                        sb.Append(str[i]);
                        if (str[i] == ']')
                        {
                            break;
                        }
                    }
                }
                else if (c == '@')
                {
                    sb.Append(c);
                    sb.Append(str[i++]);
                    sb.Append(str[i++]);
                    sb.Append(str[i++]);
                    sb.Append(str[i]);
                }
                else
                {
                    sb.Append(c);
                }

            }
            return sb.ToString();
        }
#if DEBUG
        public static void TEST_ConvertNarrowFont()
        {
            {
                string r = "foo";
                r = ConvertNarrowFont(r);
                Debug.Assert(r == "[_f][_o][_o]");
            }
            {
                string r = "[_f]oo";
                r = ConvertNarrowFont(r);
                Debug.Assert(r == "[_f][_o][_o]");
            }
            {
                string r = "あいうえお";
                r = ConvertNarrowFont(r);
                Debug.Assert(r == "あいうえお");
            }
        }
#endif //DEBUG

        static Dictionary<string, string> CleanupFindKanaInputStringCache = new Dictionary<string, string>();
        public static string KanaToNumber(string str)
        {
            if (CleanupFindKanaInputStringCache.ContainsKey(str))
            {
                return CleanupFindKanaInputStringCache[str];
            }

            string s = KanaToNumberLow(str);
            CleanupFindKanaInputStringCache[str] = s;
            return s;
        }

        static string CleanupFindStringLow(string str, bool isJP)
        {
            string a = MultiByteJPUtil.ConvertNarrowFontToAlpha(str);
            a = a.ToLower();
            if (isJP)
            {
                a = RegexCache.Replace(a, @"[\[\]、,，]", "");    ///No Translate
                //カタカナはすべてひらがなを経由してローマ字へ、全角英数字は、半角英数字に置き換えます.
                a = ToMigemo(a);
            }
            else
            {
                a = RegexCache.Replace(a, @"[\[\]、,，]", " ");   ///No Translate
            }

            a = RegexCache.Replace(a, @"\s+", " ");//連続するスペースを1つにする. ///No Translate
            return a;
        }


        static string KanaToNumberLow(string str)
        {
            return MultiByteJPUtil.ConvertKanaToNumber(str);
        }

        public static bool StrStrEx(string str,string need,bool isJP)
        {
            Debug.Assert(U.CleanupFindString(need,isJP) == need);
            if (str == null)
            {
                return false;
            }

            string t = U.CleanupFindString(str, isJP);
            if (t.IndexOf(need) >= 0)
            {
                return true;
            }

            string[] sp = need.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < sp.Length; i++)
            {
                if (t.IndexOf(sp[i]) < 0)
                {//ない
                    return false;
                }
            }
            return true;
        }

        //必ずアップデートイベントを発生させる.
        //C#のイベントはおかしい
        public static void ForceUpdate(NumericUpDown obj,decimal value )
        {
            if (obj.Value == value)
            {//同一の場合、発火しないのでわざと違う値にする.
             //どう考えてもこの方法はおかしいと思うのだけど、
             //どうやっても、イベント発火を手動でできない.
             //C#のイベントはおかしい.
                FireEvent(obj, "OnValueChanged");
                return;
            }

            //C#がエラーメッセージの仕事をさぼるので我々が代わりにやる.
            if (value < obj.Minimum)
            {
                Form f = ControlToParentForm(obj);
                string error = String.Format("The value is less than Minimum! value:({0}) NumericUpDown:({1}) current:{2} Minimum:({3}) Maximum:({4}) ParentForm:({5})", value, obj.Name, obj.Value, obj.Minimum, obj.Maximum, f == null ? "null" : f.Name);
#if DEBUG
                throw new Exception(error);
#else
                Log.Error(error);
                value = obj.Minimum;
#endif
            }
            if (value > obj.Maximum)
            {
                Form f = ControlToParentForm(obj);
                string error = String.Format("The value is greater than Maximum! value:({0}) NumericUpDown:({1}) current:{2} Minimum:({3}) Maximum:({4}) ParentForm:({5})", value, obj.Name, obj.Value, obj.Minimum, obj.Maximum, f == null ? "null" : f.Name);
#if DEBUG
                throw new Exception(error);
#else
                Log.Error(error);
                value = obj.Maximum;
#endif
            }
            obj.Value = value;
        }


        //必ずアップデートイベントを発生させる.
        //C#のイベントはおかしい
        public static void ForceUpdate(ComboBox obj, int value)
        {
            if (obj.SelectedIndex == value)
            {
                if (obj.SelectedIndex == -1)
                {
                    return;
                }
                FireEvent(obj, "OnSelectedValueChanged");
                FireEvent(obj, "OnSelectedIndexChanged");

                return;
            }

            if (value >= obj.Items.Count)
            {
                Form f = ControlToParentForm(obj);
#if DEBUG
                throw new Exception(String.Format("The value is greater than Items.Count! value:({0}) ComboBox:({1}) current:{2} ParentForm:({3}) Count:{4}", value, obj.Name, obj.SelectedIndex, f == null ? "null" : f.Name ,obj.Items.Count));
#else
                Log.Error(String.Format("The value is greater than Items.Count! value:({0}) ComboBox:({1}) current:{2} ParentForm:({3}) Count:{4}", value, obj.Name, obj.SelectedIndex, f == null ? "null" : f.Name, obj.Items.Count));
                if (obj.Items.Count == 0)
                {
                    value = -1;
                }
                else
                {
                    value = obj.Items.Count - 1;
                    if (value < 0)
                    {
                        value = 0;
                    }
                }
#endif
            }

            obj.SelectedIndex = value;
        }
        public static void ForceUpdate(ListBox obj, uint value)
        {
            ForceUpdate(obj,(int)value);
        }
        //必ずアップデートイベントを発生させる.
        //C#のイベントはおかしい
        public static void ForceUpdate(ListBox obj, int value)
        {
            if (obj.SelectedIndex == value)
            {
                if (obj.SelectedIndex == -1)
                {
                    return;
                }
                FireEvent(obj, "OnSelectedValueChanged");
                FireEvent(obj, "OnSelectedIndexChanged");
                return;
            }

            if (value >= obj.Items.Count)
            {
                Form f = ControlToParentForm(obj);
#if DEBUG
                throw new Exception(String.Format("The value is greater than Items.Count! value:({0}) ComboBox:({1}) current:{2} ParentForm:({3}) Count:{4}", value, obj.Name, obj.SelectedIndex, f == null ? "null" : f.Name, obj.Items.Count));
#else
                Log.Error(String.Format("The value is greater than Items.Count! value:({0}) ComboBox:({1}) current:{2} ParentForm:({3}) Count:{4}", value, obj.Name, obj.SelectedIndex, f == null ? "null" : f.Name, obj.Items.Count));
                if (obj.Items.Count == 0)
                {
                    value = -1;
                }
                else
                {
                    value = obj.Items.Count - 1;
                    if (value < 0)
                    {
                        value = 0;
                    }
                }
#endif
            }
            obj.SelectedIndex = value;
        }
        //aaa.bbb.ccc.gba -> aaa
        public static string GetFirstPeriodFilename(string fullfilename)
        {
            string filename = Path.GetFileName(fullfilename);
            int a = filename.IndexOf('.');
            if (a < 0)
            {
                return fullfilename;
            }
            string dir = Path.GetDirectoryName(filename);
            return Path.Combine(dir, filename.Substring(0, a));
        }

        public static Encoding GetSystemDefault()
        {
            if (OptionForm.lang() == "ja")
            {
                return System.Text.Encoding.GetEncoding("Shift_JIS");
            }
            else if (OptionForm.lang() == "zh")
            {
                return System.Text.Encoding.GetEncoding("gb2312");
            }
            return new UTF8Encoding(false);
        }

        //全部 0の空配列かどうか.
        public static bool IsNullArray(byte[] arr)
        {
            for (uint i = 0; i < arr.Length; i += 2)
            {
                if (arr[i] != 0)
                {
                    return false;
                }
            }
            return true;
        }
        //dest を src で置き換える ただし、あふれた分は無視
        public static void ArrayPatch(byte[] src, uint src_offset, byte[] dest, uint dest_offset)
        {
            if (src.Length < src_offset)
            {
                return;
            }
            if (dest.Length < dest_offset)
            {
                return;
            }
            uint src_length = (uint)src.Length - src_offset;
            if (src_length < src_offset)
            {
                return;
            }
            uint dest_length = (uint)dest.Length - dest_offset;
            uint copy_length = Math.Min(src_length, dest_length);
            Array.Copy( src, src_offset,dest, dest_offset, copy_length);
        }

        public static EventHandler FireKeyDown(Control listbox,KeyEventHandler func, Keys keys)
        {
            return
                (Object o, EventArgs ee) =>
                {
                    func(listbox, new KeyEventArgs(keys));
                };
        }
        //指定した幅で丸める
        public static string strimwidth(string str , int startMoji , int widthMoji ,string trimmarker = "...")
        {
	        int countMoji = 0;
	        string ret = "";

	        //先頭も省略する場合
	        if (startMoji >= 1)
	        {
		        ret = trimmarker;
	        }

            
	        //メインとなる文字列を取得
	        ret += U.substr(str,startMoji,widthMoji);
                        
	        //後ろを省略している場合
	        if (countMoji >= startMoji + widthMoji)
	        {
		        ret += trimmarker;
	        }
	        return ret;
        }

        public static string ChangeExtFilename(string filename,string ext,string appendname="")
        {
            string dir = Path.GetDirectoryName(filename);
            string name = Path.GetFileNameWithoutExtension(filename);

            return Path.Combine(dir, name + appendname + ext);
        }
        public static string HexDump(List<byte> bytes)
        {
            return HexDump(bytes.ToArray());
        }
        public static string HexDump(byte[] bytes)
        {
            StringBuilder r = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 1)
            {
                if ((i % 16) == 0)
                {
                    if (i != 0)
                    {
                        r.AppendLine();
                    }
                }
                r.Append(" ");
                r.Append(bytes[i].ToString("X02"));
            }
            r.AppendLine();
            return r.ToString();
        }
        public static string HexDumpLiner(byte[] bytes)
        {
            StringBuilder r = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 1)
            {
                r.Append(" ");
                r.Append(bytes[i].ToString("X02"));
            }
            return r.ToString();
        }
        public static string HexDumpLiner(byte[] bytes, uint addr, uint length)
        {
            StringBuilder r = new StringBuilder();
            uint max = Math.Min(addr + length, (uint)bytes.Length);
            for (uint i = addr; i < max; i += 1)
            {
                r.Append(" ");
                r.Append(bytes[i].ToString("X02"));
            }
            return r.ToString();
        }
        public static string HexDumpLiner0x(byte[] bytes)
        {
            string r = HexDumpLiner(bytes);
            return r.Replace(" ", " 0x");
        }
        public static string HexDumpLiner0x(byte[] bytes, uint addr, uint length)
        {
            string r = HexDumpLiner(bytes, addr, length);
            return r.Replace(" ", " 0x");
        }
        public static string HexDumpLinerDoll(byte[] bytes, uint addr, uint length)
        {
            string r = HexDumpLiner(bytes, addr, length);
            return r.Replace(" ", " $");
        }

        public static String var_dump(object obj,int nest=0)
        {
            if (obj == null)
            {
                return "null";
            }
            if (   obj is uint || obj is int
                || obj is ushort || obj is short
                || obj is byte || obj is byte
                || obj is float || obj is double || obj is bool
                || obj is UInt16 || obj is Int16
                || obj is UInt32 || obj is Int32
                || obj is UInt64 || obj is Int64
                ) 
            {
                return obj.ToString();
            }
            if (obj is string)
            {
                return "\"" + obj.ToString() + "\"";
            }

            if (nest >= 2)
            {
                return "...";
            }

            StringBuilder sb = new StringBuilder();
            IEnumerable ienum = obj as IEnumerable;
            if (ienum != null)
            {
                sb.Append("{");
                foreach (object o in ienum)
                {
                    sb.Append( var_dump(o,nest+1) + ",");
                }
                sb.Append("}");
                return sb.ToString();
            }

            sb.Append("{");
            const BindingFlags FINDS_FLAG = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo[] infoArray = obj.GetType().GetFields(FINDS_FLAG);
            foreach (FieldInfo info in infoArray)
            {
                object o = info.GetValue(obj);
                sb.Append( info.Name + ": " + var_dump(o, nest + 1) + ",");
            }
            sb.Append("}");
            return sb.ToString();
        }
        static public int GetCountOfLines(string lines)
        {
            return lines.Split(new string[]{"\r\n"},StringSplitOptions.None).Length;
        }

        public static bool CanWriteFileRetry(string path)
        {
            if (!File.Exists(path))
            {//存在しないので書き込めると思う.
                return true;
            }

            bool isRetry = true;
            do
            {
                try
                {
                    using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        isRetry = false;
                    }
                }
                catch (Exception e)
                {
                    DialogResult dr = R.ShowYesNo("ファイルに書き込めません。\r\n再試行しますか？\r\nファイル:{0}\r\n\r\n{1}", path, e.ToString());
                    if (dr == DialogResult.No)
                    {
                        //書き込めないので、ユーザキャンセル.
                        return false;
                    }
                }
            }
            while (isRetry);

            //書き込める
            return true;
        }

        public static bool CanReadFileRetry(OpenFileDialog open)
        {
            if (open.FileNames.Length <= 0)
            {
                return false;
            }
            if (!File.Exists(open.FileNames[0]))
            {
                return false;
            }
            if (!U.CanReadFileRetry(open.FileNames[0]))
            {
                return false;
            }
            return true;
        }

        public static bool CanReadFileRetry(string path)
        {
            Debug.Assert(path != "");

            bool isRetry = true;
            do
            {
                if (!File.Exists(path))
                {
                    DialogResult dr = R.ShowYesNo("ファイルがありません。\r\nファイル名:{0}\r\n再試行しますか？", path);
                    if (dr == DialogResult.No)
                    {
                        //ユーザキャンセル.
                        return false;
                    }
                }

                try
                {
                    using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        isRetry = false;
                    }
                }
                catch (Exception e)
                {
                    DialogResult dr = R.ShowYesNo("ファイルを読み込めません。\r\n再試行しますか？\r\nファイル:{0}\r\n\r\n{1}", path, e.ToString());
                    if (dr == DialogResult.No)
                    {
                        //ユーザキャンセル.
                        return false;
                    }
                }
            }
            while (isRetry);

            //読込める
            return true;
        }

        //サンプル用のバイナリデータ
        public static string MakeOPData(uint addr, uint length)
        {
            string opcode;
            if (length == 2)
            {
                opcode = Program.ROM.u8(addr + 1).ToString("X02") + Program.ROM.u8(addr).ToString("X02");
            }
            else
            {
                opcode = Program.ROM.u8(addr + 1).ToString("X02") + Program.ROM.u8(addr).ToString("X02") + " " + Program.ROM.u8(addr + 3).ToString("X02") + Program.ROM.u8(addr + 2).ToString("X02");
            }
            return opcode;
        }
        public static string MakeJisageSpace(int jisage)
        {
            if (jisage <= 0)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < jisage; i++)
            {
                sb.Append("    ");
            }
            return sb.ToString();
        }

        //奇数かどうか
        public static bool IsValueOdd(uint addr)
        {
            return (addr & 0x1) == 0x1;
        }


        //人間がこのコントロールに入力したものかどうか.
        public static bool IsOrderOfHuman(object sender)
        {
            if (!(sender is Control))
            {
                return false;
            }
            Control c = (Control)sender;
            return (c.Focused);
        }

        public static bool IsRequiredFileExist(string filename)
        {
            if (!File.Exists(filename))
            {
                if (Program.ROM.RomInfo.version() == 0)
                {//FEでないので読めなくてもいい
                    return false;
                }

                R.ShowStopError("必須となる設定ファイルを読みこめません。\r\n設定ファイルが壊れている可能性があります。\r\n再ダウンロードしなおしてください。\r\n{0}", filename.Replace("_ALL.txt", "_*.txt"));
                Debug.Assert(false);
                return false;
            }
            return true;
        }
        public static void DrawRectanglePopupX(PaintEventArgs e, Point pt, Font font, Brush backBrush, Brush foreBrush, string msg)
        {
            string[] lines = msg.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            Rectangle windowrc = new Rectangle();
            windowrc.X = (int)pt.X + 48 + 1;
            windowrc.Y = (int)pt.Y;
            windowrc.Width = (int)0;
            windowrc.Height = (int)font.Height * 4;

            //枠を描画する幅を特定します.
            SizeF textSize = new SizeF();
            for (int i = 0; i < lines.Length; i++)
            {
                textSize = e.Graphics.MeasureString(lines[i], font, 1000);
                if (windowrc.Width < textSize.Width) windowrc.Width = (int)textSize.Width;
            }

            if (windowrc.X + windowrc.Width > e.ClipRectangle.Right)
            {//画面右端の場合、左側に押し出す.
                windowrc.X = windowrc.X - windowrc.Width - 32;
                windowrc.Y = windowrc.Y + 16 + 32;
            }
            if (windowrc.Y + windowrc.Height > e.ClipRectangle.Bottom)
            {//画面下端の場合、上側に押し出す.
                windowrc.X = windowrc.X - 16 - 32;
                windowrc.Y = windowrc.Y - windowrc.Height - 32;
            }

            if (windowrc.X < 0)
            {
                windowrc.X = 0;
                while (true)
                {
                    if (pt.X < windowrc.Width && pt.Y >= windowrc.Y && pt.Y <= windowrc.Y + windowrc.Height)
                    {
                        windowrc.Y += 16;
                        continue;
                    }
                    break;
                }
            }
            pt.X = windowrc.X;
            pt.Y = windowrc.Y;

            e.Graphics.FillRectangle(backBrush, windowrc);

            for (int i = 0; i < lines.Length; i++)
            {
                e.Graphics.DrawString(lines[i], font, foreBrush, pt);
                pt.Y = pt.Y + font.Height;
            }
        }
        public static void DrawRectanglePopupY(PaintEventArgs e, Point pt, Font font, Brush backBrush, Brush foreBrush, string msg)
        {
            string[] lines = msg.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            Rectangle windowrc = new Rectangle();
            windowrc.X = (int)pt.X;
            windowrc.Y = (int)pt.Y + 16 + 1;
            windowrc.Width = (int)0;
            windowrc.Height = (int)font.Height * 4;

            //枠を描画する幅を特定します.
            SizeF textSize = new SizeF();
            for (int i = 0; i < lines.Length; i++)
            {
                textSize = e.Graphics.MeasureString(lines[i], font, 1000);
                if (windowrc.Width < textSize.Width) windowrc.Width = (int)textSize.Width;
            }

            if (windowrc.X + windowrc.Width > e.ClipRectangle.Right)
            {//画面右端の場合、左側に押し出す.
                windowrc.X = windowrc.X - windowrc.Width;
                windowrc.Y = windowrc.Y + 16;
            }
            if (windowrc.Y + windowrc.Height > e.ClipRectangle.Bottom)
            {//画面下端の場合、上側に押し出す.
                windowrc.X = windowrc.X - 16;
                windowrc.Y = windowrc.Y - 32 - windowrc.Height;
            }

            if (windowrc.X < 0)
            {
                windowrc.X = 0;
            }
            pt.X = windowrc.X;
            pt.Y = windowrc.Y;

            e.Graphics.FillRectangle(backBrush, windowrc);

            for (int i = 0; i < lines.Length; i++)
            {
                e.Graphics.DrawString(lines[i], font, foreBrush, pt);
                pt.Y = pt.Y + font.Height;
            }
        }

        //幅を推測する(ポインタ)
        public static int CalcLZ77LinerImagePointerToWidth(uint addr, int align = 8)
        {
            addr = U.toOffset(addr);
            if (!U.isSafetyOffset(addr))
            {
                return align;
            }
            uint a = Program.ROM.p32(addr);
            return CalcLZ77LinerImageToWidth(a, align);
        }

        //幅を推測する
        public static int CalcLZ77LinerImageToWidth(uint addr, int align = 8)
        {
            addr = U.toOffset(addr);
            if (! U.isSafetyOffset(addr ))
            {
                return align;
            }
            uint size = LZ77.getUncompressSize(Program.ROM.Data, addr);
            if (size <= 0)
            {
                return align;
            }

            int a = (int)size / 2 / 2 / align;
            if (a <= 0)
            {
                return align;
            }
            return a * align;
        }
        //幅と高さを推測する
        public static Size CalcLZ77ImageToSize(uint addr, int align = 8)
        {
            //高さ1だったとしたら幅はいくつか？
            int width = CalcLZ77LinerImageToWidth(addr, align);

            //綺麗に割り切れる幅を探す.
            for (int w = 32; w >= 1; w--)
            {
                if (width % (w * 8) == 0)
                {
                    return new Size(w * 8, width / (w * 8) * 8);
                }
            }
            Debug.Assert(false);
            return new Size(8, 8);
        }
        //幅と高さを推測する
        public static Size CalcLZ77ImageToSizePointer(uint pointer, int align = 8)
        {
            uint addr = U.toOffset(pointer);
            if (! U.isSafetyOffset(addr))
            {
                return new Size(align,align);
            }

            addr = Program.ROM.p32(addr);
            if (! U.isSafetyOffset(addr))
            {
                return new Size(align, align);
            }
            
            return CalcLZ77ImageToSize(addr);
        }

        //https://dobon.net/vb/dotnet/file/writecsvfile.html より
        /// <summary>
        /// 必要ならば、文字列をダブルクォートで囲む
        /// </summary>
        public static string EncloseDoubleQuotesIfNeed(string field)
        {
            if (NeedEncloseDoubleQuotes(field))
            {
                return EncloseDoubleQuotes(field);
            }
            return field;
        }

        /// <summary>
        /// 文字列をダブルクォートで囲む
        /// </summary>
        private static string EncloseDoubleQuotes(string field)
        {
            if (field.IndexOf('"') > -1)
            {
                //"を""とする
                field = field.Replace("\"", "\"\"");
            }
            return "\"" + field + "\"";
        }

        /// <summary>
        /// 文字列をダブルクォートで囲む必要があるか調べる
        /// </summary>
        private static bool NeedEncloseDoubleQuotes(string field)
        {
            return field.IndexOf('"') > -1 ||
                field.IndexOf(',') > -1 ||
                field.IndexOf('\r') > -1 ||
                field.IndexOf('\n') > -1 ||
                field.StartsWith(" ") ||
                field.StartsWith("\t") ||
                field.EndsWith(" ") ||
                field.EndsWith("\t");
        }

        //OAMREGSの長さを求める
        //
        public static uint OAMREGSLength(uint addr,ROM rom)
        {
            if (!U.isSafetyOffset(addr, rom))
            {
                return 0;
            }
            uint count = rom.u16(addr);
//            Debug.Assert(count <= 0x14);

            return 2 + count * 2 * 3;

            //[CC CC]  
            //oam1  oam0  oam2
            //XX ?? YY ?? ?? ??
            //CC=count この値だけOAMのデータが続きます。データは6バイトです。
            //XX=x     オブジェクトを表示するX座標
            //YY=x     オブジェクトを表示するY座標
        }
        //TextBatchの長さを求める
        //
        public static uint TextBatchLength(uint addr , ROM rom)
        {
            uint first = addr;

            uint length = (uint)rom.Data.Length - 8;
            for (; addr < length; addr += 8 )
            {
                uint addr02 = rom.u16(addr);
                if (addr02 == 0)
                {
                    addr += 8;
                    break;
                }
            }

            return (addr - first) ;
        }

        //TextBatchの長さを求める
        //
        public static uint TextBatchShortLength(uint addr, ROM rom)
        {
            uint first = addr;

            uint length = (uint)rom.Data.Length - 2;
            for (; addr < length; addr += 2)
            {
                uint addr02 = rom.u16(addr);
                if (addr02 == 0)
                {
                    addr += 2;
                    break;
                }
            }

            return (addr - first);
        }

        public static string ToCharOneHex(byte a)
        {
            return a.ToString("X02");
        }
        public static Dictionary<string, string> LoadTSVResourcePair(string fullfilename)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (!U.IsRequiredFileExist(fullfilename))
            {
                return dic;
            }

            try
            {
                using (StreamReader reader = File.OpenText(fullfilename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (U.IsComment(line) || U.OtherLangLine(line))
                        {
                            continue;
                        }
                        line = U.ClipComment(line);
                        if (line == "")
                        {
                            continue;
                        }
                        string[] sp = line.Split('\t');
                        if (sp.Length < 2)
                        {
                            continue;
                        }
                        dic[sp[1]] = sp[0];
                    }
                }
            }
            catch (Exception e)
            {
                R.ShowStopError("必須となる設定ファイルを読みこめません。\r\n設定ファイルが壊れている可能性があります。\r\n再ダウンロードしなおしてください。\r\n{0}\r\n{1}", fullfilename, e.ToString());
            }
            return dic;
        }
        public static void AddCancelButton(Form f)
        {
            if (f.CancelButton != null)
            {
                return ;
            }
            Button cancelButton = new Button ();
            cancelButton.Click += (Object Sender, EventArgs e) =>
            {
                f.DialogResult = DialogResult.Cancel;
                f.Close();
            };
            f.DialogResult = DialogResult.Cancel;
            f.CancelButton = cancelButton;
        }

        public static string[] AddIfNotExist(string[] data, string need)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == need)
                {
                    return data;
                }
            }

            string[] d = new string[data.Length + 1];
            Array.Copy(data, 0, d, 0,data.Length );
            d[data.Length] = need;
            return d;
        }

        public static string mktempdir()
        {
            for (int retry = 0; retry < 4; retry++)
            {
                string tempdir = Path.GetTempFileName();
                try
                {
                    File.Delete(tempdir);
                    U.mkdir(tempdir);
                }
                catch (Exception)
                {
                    continue;
                }
                return tempdir;
            }

            //最後にもう一回試す.
            //これは失敗した場合に例外を飛ばすためです。
            {
                string tempdir = Path.GetTempFileName();
                File.Delete(tempdir);
                U.mkdir(tempdir);
                return tempdir;
            }
        }
        public static string mktemp(string ext)
        {
            return  System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ext;
        }
        public class MakeTempDirectory : IDisposable
        {
            public string Dir { get; private set; }
            public MakeTempDirectory()
            {
                this.Dir = mktempdir();
            }
            public void Dispose()
            {
                try
                {
                    Directory.Delete(this.Dir, true);
                }
                catch (Exception e)
                {
                    R.ShowStopError(e.ToString());
                }
            }
        }

        //https://stackoverflow.com/questions/25772622/how-do-i-echo-into-an-existing-cmd-window
        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        static StreamWriter _stdOutWriter;
        public static void echo(string line)
        {
            if (! Program.IsCommandLine)
            {
                Log.Notify(line);
                return;
            }

            if (_stdOutWriter == null)
            {
                // this needs to happen before attachconsole.
                // If the output is not redirected we still get a valid stream but it doesn't appear to write anywhere
                // I guess it probably does write somewhere, but nowhere I can find out about
                var stdout = Console.OpenStandardOutput();
                _stdOutWriter = new StreamWriter(stdout);
                _stdOutWriter.AutoFlush = true;

                AttachConsole(ATTACH_PARENT_PROCESS);
            }

            _stdOutWriter.WriteLine(line);
            Console.WriteLine(line);
        }
        public static void FireOnClick(Object obj)
        {
            FireEvent(obj, "OnClick");
        }
        static void FireEvent(Object obj, string name, object arg = null)
        {
            if (arg == null)
            {
                arg = EventArgs.Empty;
            }
            try
            {
                MethodInfo m = obj.GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance);
                m.Invoke(obj, new object[] { arg });
            }
            catch (TargetInvocationException e)
            {
                Log.Error("FireEvent",name,R.ExceptionToString(e));
            }

        }
        public static void FireOnMouseDoubleClick(Object obj)
        {
            FireEvent(obj, "OnMouseDoubleClick", new MouseEventArgs(MouseButtons.Left,1,0,0,0));
        }


        //60fpsをgif FPSに変換
        //GAME 1/60  sec
        //GIF  1/100 sec
        public static ushort GameFrameSecToGifFrameSec(uint fps60)
        {
            return (ushort)Math.Round(((double)fps60) * 100.0D / 60.0D);
        }
#if DEBUG
        public static void TEST_GameFrameSecToGifFrameSec()
        {
            {
                ushort r = GameFrameSecToGifFrameSec(1);
                Debug.Assert(r == 2);
            }
        }
#endif
        //60fpsをPC用のTick秒(100ns)に変換
        //GAME 1/60  sec
        //PC   1/10000000 sec
        public static long GameFrameSecToTickSec(uint fps60)
        {
            return (long)Math.Round(((double)fps60) * 10000000.0D / 60.0D);
        }

        public static string md5(string filename)
        {
            return md5(File.ReadAllBytes(filename));
        }

        public static string md5(byte[] bin)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = md5.ComputeHash(bin);
            md5.Clear();

            System.Text.StringBuilder result = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                result.Append(b.ToString("x2"));
            }
            return result.ToString();
        }

        public static void AllowDropFilename(Form self
            , string[] allowExts
            , Action<string> callback )
        {
            self.AllowDrop = true;
            self.DragEnter += (sender, e) =>
            {
                //ファイルがドラッグされている場合、カーソルを変更する
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] fileName = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                    if (fileName.Length <= 0)
                    {
                        return;
                    }

                    string ext = U.GetFilenameExt(fileName[0]);
                    if (Array.IndexOf(allowExts, ext) < 0)
                    {
                        return;
                    }

                    e.Effect = DragDropEffects.Copy;
                }
            };
            self.DragDrop += (sender, e) =>
            {
                //ドロップされたファイルの一覧を取得
                string[] fileName = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                if (fileName.Length <= 0)
                {
                    return;
                }
                string ext = U.GetFilenameExt(fileName[0]);
                if (Array.IndexOf(allowExts, ext) < 0)
                {
                    return;
                }
                e.Effect = DragDropEffects.None;
                for (int i = 0; i < fileName.Length; i++)
                {
                    callback(fileName[i]);
                }
            };
        }

        //コントロールの位置を横方向に入れ替える
        public static void SwapControlPosition(Control a,Control b)
        {
            int margine = b.Location.X - (a.Location.X + a.Width);

            b.Location = new Point(a.Location.X, b.Location.Y);
            a.Location = new Point(b.Location.X + b.Width + margine, a.Location.Y);
        }
        //コントロールの位置を横方向にずらす
        public static void ShiftControlPosition(Control a, Control b)
        {
            int margine = b.Location.X - (a.Location.X + a.Width);

            a.Location = new Point(b.Location.X, a.Location.Y);
            b.Location = new Point(a.Location.X + a.Width + margine, b.Location.Y);
        }

        //改行をスペースに変換
        public static string nl2space(string str)
        {
            return str.Replace("\r\n", " ");
        }
        public static void SetIcon(Button button, Icon tempIcon)
        {
            if (button.Height <= 6)
            {
                return;
            }

            Bitmap icon = tempIcon.ToBitmap();
            U.MakeTransparent(icon);
            button.TextImageRelation = TextImageRelation.ImageBeforeText;
            Bitmap bitmap = ImageUtil.BitmapScale(icon, button.Height - 6, button.Height - 6);

            button.Image = icon;
        }
        public static void SetIcon(Button button, Bitmap icon)
        {
            if (icon == null || button.Height <= 6 || icon.Height <= 0)
            {
                return;
            }
            U.MakeTransparent(icon);
            button.TextImageRelation = TextImageRelation.ImageBeforeText;
            Bitmap bitmap = ImageUtil.BitmapScale(icon,button.Height - 6, button.Height - 6);

            button.Image = bitmap;
           
        }

        //個人情報の削除
        public static string TrimPersonalInfomation(string text)
        {
            //FEBuilderGBAのベースディレクトリ
            text = text.Replace(Program.BaseDirectory, "%BASEDIR%");
            //ROMがあるディレクトリ
            if (Program.ROM != null && !Program.ROM.IsVirtualROM)
            {
                text = text.Replace(Path.GetDirectoryName(Program.ROM.Filename), "%ROMDIR%");
            }
            //ユーザ名が入っているディレクトリを消す.
            string users = System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            text = text.Replace(users, "%USER%");

            return text;
        }

        //最大値を返す.
        public static uint GetMaxValue(List<uint> list)
        {
            if (list.Count == 0)
            {
                return 0;
            }
            int index = GetMaxIndex(list);
            return list[index];
        }
        public static int GetMaxIndex(List<uint> list)
        {
            uint max = 0;
            int maxIndex = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] > max)
                {
                    max = list[i];
                    maxIndex = i;
                }
            }
            return maxIndex;
        }
        public static int GetIndexOf(uint[] list , uint need)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == need)
                {
                    return i;
                }
            }
            return -1;
        }

        public static bool IsLinux()
        {
            //https://stackoverflow.com/questions/5116977/how-to-check-the-os-version-at-runtime-e-g-windows-or-linux-without-using-a-con
            int p = (int)Environment.OSVersion.Platform;
            return (p == 4) || (p == 6) || (p == 128);
        }

        //https://stackoverflow.com/questions/17212964/net-zlib-inflate-with-net-4-5
        public static byte[] ConvertZlibToByte(byte[] zlib)
        {
            using (MemoryStream ms = new MemoryStream(zlib))
            {
                MemoryStream msInner = new MemoryStream();

                // Read past the first two bytes of the zlib header
                ms.Seek(2, SeekOrigin.Begin);

                using (DeflateStream z = new DeflateStream(ms, CompressionMode.Decompress , false))
                {
                    z.CopyTo(msInner);
                }
                return msInner.ToArray();
            }
        }
        public static byte[] ConvertGzipToByte(byte[] gzip)
        {
            using (MemoryStream ms = new MemoryStream(gzip))
            {
                MemoryStream msInner = new MemoryStream();

                using (GZipStream z = new GZipStream(ms, CompressionMode.Decompress,false))
                {
                    z.CopyTo(msInner);
                }
                return msInner.ToArray();
            }
        }
#if DEBUG
        public static void TEST_BASE64ZLIB()
        {
            {
                string test = "eJx1VltrlUcU/eZ8eKkWz4nVWDVvIklbMadeQKN502pUvORAobaPp9YW26oH0QcVFMTgJWqCP0BMFG94ifoq4oP2oRQ1SW0fBIWiTxU0TdKK0LXYa/j2OerDYs98s2fNmtl79nxHc0nSCWxLk6QCFIHeJEnu49s52HZgLb6tA9YDZfTzsDsw3gDsBFqBs/g+TvPIdQJoFmdBnD8C54Gf0P8Z2Cq+b2XpQ97dwJfApcS4+tH+CHYItgu2O7U5XGsLfDrRPgYcdzxlaYntr3Jmp8DnJdol2Iew82H7UttTr9NyF+gC7gGLgMXATqA/QCPswpz1qYnzrwHbpbUibRPh+2EwvhNAazA8QrsBdmkw3sfAgEBtdZg/ObW9D0njvNR0/Ctdo8CI4ySeo/+x4xpQrOJZkJt83Hc7vu+Vz4yQ+ZNzFP0XsA9hW2r4OsW1HPOvAg+0/1o/jyXALmAW4wBM45oaawRKwFwXM2quR3uj42hwGslxEliNbxdgLwKvgs0pinOV+H5lzF0ceD5/0EdxmJ5YnH6B/VvxXwHbIU7O+Q6oAFc4jm8bgC+AScBnwBzgN8V4PNqbgLkhuyvndDdK0hT3WXZ98lAT548DrgNNWov8+TSbc1l8FRdfouBiPVk5TU1N0hM5CsqtfJr5NTst0a+ovPM59IN48qoVPleZCzH/i2lWCypCSXeE7c/Vn+c4YuzrXN43675vl88G3bVraXbv5utOd6fV5xR1F5Sr/aoffe7Ox31FDdzDFFe7OF7n1i+otlXkE8+vmFbHM68zKcmvWWvGPbbLvyi+yL/GzS+m1fEt6lu3uPy43/dVjTOPD9Vo+hPxe5C8fUarE1vb8yxz88akdg+OA21uPuvFG3Aeqcntg7rT5D2a2Pgpl/NP0D4NHFMud6i28H7vDqbztXLtjPw6tHaj9vU0l+nYn7P+Jd1Xcm4GhsXZEqzOLUR7KNidbHTgu/QX0JuzGs81Y5/ayXsYGHScxJ7E/MvyOat1j0g7OQLsbcWdNYXnMBbtnpzVI885HeODwOzUako8r2eJvUWxjvUIBeWNj3G941sM/75gb0a3Yh99WRsmuDrVoxxrAj4BPlUu8B2Ib8XvQFcw9ClG3P+B1GrDbOBwqK5xKzHWBqxKrdbyHbuVZO9N5COG3f6izjbV7/htM3i+53rAf46rVl+X9HHOgM71DjR9oPNYqRrOt/659E4NFo/6YLEYrdF4Q7l5M9h7cVdjnL9FNXyBanZZmqaG6veQGltDtU6C7xRjMuz26s+B+jj3keP6B/gmvH2OHm9CxtHj4sLc4VojwMxgXD4XZ7yHlznD8/xa3Ix/neoRz4Z5Gv9DBt9xZ1rEO+q0x3O+Ic4xqp8jik1LkukbfA/vu8D508CzL2d18X+OcF5C";
                byte[] data = System.Convert.FromBase64String(test);
                byte[] raw = U.ConvertZlibToByte(data);
            }
            {
                string test = "eJztzgEJAAAIA7CD8S1tDDlsCZYAAHTY+R7Q6ABGDACP";
                byte[] data = System.Convert.FromBase64String(test);
                byte[] raw = U.ConvertZlibToByte(data);
            }
        }
#endif
        //ROMファイル名を文字列から消去します.
        public static string StripROMFilename(string title)
        {
            string filename = Path.GetFileName(Program.ROM.Filename);
            title = title.Replace(filename, "");
            return title;
        }

        public static string ConvertFormName(Control f)
        {
            if (f == null)
            {
                return "";
            }
            else
            {
                return U.StripROMFilename(f.Text) + " (" + f.Name + ")";
            }
        }

        public static List<uint> Uniq(List<uint> list)
        {
            HashSet<uint> hs1 = new HashSet<uint>(list);
            return new List<uint>(hs1);
        }

        public static bool IsFillData(byte[] data, byte need)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] != need)
                {
                    return false;
                }
            }
            return true;
        }
        public static bool IsFillData00_Or_FF(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if ((data[i] == 0x00) && (data[i] == 0xFF) )
                {
                    return true;
                }
            }
            return false;
        }
        [MethodImpl(256)]
        public static string SA(string name)
        {
            if (name == "")
            {
                return "";
            }
            return " " + name;
        }

        [MethodImpl(256)]
        public static uint Sub(uint a, uint b)
        {
            if (a <= b)
            {
                return 0;
            }
            return a - b;
        }

        public static uint[] DicKeys(Dictionary<uint,string> dic)
        {
            var k = dic.Keys;
            uint[] keys = new uint[k.Count];
            k.CopyTo(keys, 0);
            return keys;
        }
        public static string[] DicKeys(Dictionary<string, string> dic)
        {
            var k = dic.Keys;
            string[] keys = new string[k.Count];
            k.CopyTo(keys, 0);
            return keys;
        }
        public static uint Grep4EndByDmp(string filename, uint start_offset, uint plus)
        {
            if (!File.Exists(filename))
            {
                return U.NOT_FOUND;
            }
            byte[] bin = File.ReadAllBytes(filename);
            return U.GrepEnd(Program.ROM.Data, bin, start_offset, 0, 4 ,plus, true);
        }
        public static double GetOtherProgramVersion(string filename)
        {
            if (! File.Exists(filename))
            {
                return 0;
            }

            try
            {
                System.Diagnostics.FileVersionInfo vi =
                    System.Diagnostics.FileVersionInfo.GetVersionInfo(
                        filename);
                return U.atof(vi.FileVersion);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return 0;
            }
        }

        /// 指定された拡張子に関連付けられた実行ファイルのパスを取得する。
        /// https://dobon.net/vb/dotnet/system/findassociatedexe.html
        /// <summary>
        /// 指定された拡張子に関連付けられた実行ファイルのパスを取得する。
        /// </summary>
        /// <param name="extName">".txt"などの拡張子。</param>
        /// <returns>見つかった時は、実行ファイルのパス。
        /// 見つからなかった時は、空の文字列。</returns>
        /// <example>
        /// 拡張子".txt"に関連付けられた実行ファイルのパスを取得する例
        /// <code>
        /// string exePath = FindAssociatedExecutable(".txt");
        /// </code>
        /// </example>
        public static string FindAssociatedExecutable(string extName)
        {
            //pszOutのサイズを取得する
            uint pcchOut = 0;
            //ASSOCF_INIT_IGNOREUNKNOWNで関連付けられていないものを無視
            //ASSOCF_VERIFYを付けると検証を行うが、パフォーマンスは落ちる
            AssocQueryString(AssocF.Init_IgnoreUnknown, AssocStr.Executable,
                extName, null, null, ref pcchOut);
            if (pcchOut == 0)
            {
                return string.Empty;
            }
            //結果を受け取るためのStringBuilderオブジェクトを作成する
            StringBuilder pszOut = new StringBuilder((int)pcchOut);
            //関連付けられた実行ファイルのパスを取得する
            AssocQueryString(AssocF.Init_IgnoreUnknown, AssocStr.Executable,
                extName, null, pszOut, ref pcchOut);
            //結果を返す
            return pszOut.ToString();
        }

        [DllImport("Shlwapi.dll",
            SetLastError = true,
            CharSet = CharSet.Auto)]
        private static extern uint AssocQueryString(AssocF flags,
            AssocStr str,
            string pszAssoc,
            string pszExtra,
            [Out] StringBuilder pszOut,
            [In][Out] ref uint pcchOut);

        [Flags]
        private enum AssocF
        {
            None = 0,
            Init_NoRemapCLSID = 0x1,
            Init_ByExeName = 0x2,
            Open_ByExeName = 0x2,
            Init_DefaultToStar = 0x4,
            Init_DefaultToFolder = 0x8,
            NoUserSettings = 0x10,
            NoTruncate = 0x20,
            Verify = 0x40,
            RemapRunDll = 0x80,
            NoFixUps = 0x100,
            IgnoreBaseClass = 0x200,
            Init_IgnoreUnknown = 0x400,
            Init_FixedProgId = 0x800,
            IsProtocol = 0x1000,
            InitForFile = 0x2000,
        }

        private enum AssocStr
        {
            Command = 1,
            Executable,
            FriendlyDocName,
            FriendlyAppName,
            NoOpen,
            ShellNewValue,
            DDECommand,
            DDEIfExec,
            DDEApplication,
            DDETopic,
            InfoTip,
            QuickTip,
            TileInfo,
            ContentType,
            DefaultIcon,
            ShellExtension,
            DropTarget,
            DelegateExecute,
            SupportedUriProtocols,
            Max,
        }

        public static bool Wordrap(char c)
        {
            return c == ' ' || c == '\t' || c == '\n';
        }
        public static uint[] ParseTSVLine(string line,bool skipFirst,char sep = '\t')
        {
            string[] arr = line.Split(sep);
            List<uint> list = new List<uint>();
            
            for (int i = 0 ; i < arr.Length; i++)
            {
                string s = arr[i];
                while (s.Length >= 1 && s[0] == '"')
                {
                    i++;
                    if (i >= arr.Length)
                    {
                        break;
                    }
                    s += arr[i];
                }

                if (skipFirst)
                {//最初のデータは名前なので捨てる
                    skipFirst = false;
                    continue;
                }

                uint v = U.atoi0x(s);
                list.Add(v);
            }
            return list.ToArray();
        }


        [DllImport("kernel32.dll")]
        static extern uint FormatMessage(
          uint dwFlags, IntPtr lpSource,
          uint dwMessageId, uint dwLanguageId,
          StringBuilder lpBuffer, int nSize,
          IntPtr Arguments);
        const uint FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
        const uint FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
        const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
        const uint FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x00002000;
        const uint FORMAT_MESSAGE_FROM_HMODULE = 0x00000800;
        const uint FORMAT_MESSAGE_FROM_STRING = 0x00000400;

        //https://www.atmarkit.co.jp/fdotnet/dotnettips/741win32errmsg/win32errmsg.html
        public static string HRESULTtoString(int errCode) 
        {
            StringBuilder message = new StringBuilder(1024);

            FormatMessage(
              FORMAT_MESSAGE_FROM_SYSTEM,
              IntPtr.Zero,
              (uint)errCode,
              0,
              message,
              message.Capacity,
              IntPtr.Zero);

            return message.ToString();
        }

        public static bool[] MakeMask2(byte[] bin, byte code, byte code2)
        {
            bool[] mask = new bool[bin.Length];
            for (int i = 0; i < bin.Length; i++)
            {
                if (bin[i] == code)
                {
                    mask[i] = true;
                }
                else if (bin[i] == code2)
                {
                    mask[i] = true;
                }
            }
            return mask;
        }
        public static byte[] PickupBinaryBattern(byte[] pickup,byte[] bin,byte code)
        {
            List<byte> match = new List<byte>();
            for (int i = 0; i < bin.Length; i++)
            {
                if (i >= pickup.Length)
                {
                    break;
                }
                if (bin[i] != code)
                {
                    continue;
                }
                match.Add(bin[i]);
            }
            return match.ToArray();
        }

        //U.Directory_GetFiles_Safe の安全な実装.
        public static string[] Directory_GetFiles_Safe(string path, string filter, SearchOption op = SearchOption.TopDirectoryOnly)
        {
            if (op == SearchOption.TopDirectoryOnly)
            {
                return Directory.GetFiles(path, filter);
            }

            List<string> files = new List<string>();
            Directory_GetFiles_AllFile_Safe_Low(files , path , filter);
            return files.ToArray();
        }
        static void Directory_GetFiles_AllFile_Safe_Low(List<string> files , string path, string filter)
        {
            try
            {
                string[] l = Directory.GetFiles(path , filter);
                foreach(string s in l)
                {
                    files.Add(s);
                }

                l = Directory.GetDirectories(path);
                foreach(string s in l)
                {
                    Directory_GetFiles_AllFile_Safe_Low(files , s , filter);
                }
            }
            catch (Exception e)
            {//Skip
                Log.Error(R.ExceptionToString(e));
                Debug.Assert(false);
            }
        }

        public static void OpenURLOrFile(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch (Exception ee)
            {
                R.ShowStopError(ee.ToString());
            }
        }
        public static bool WriteAllLinesInError(string filename, List<string> lines)
        {
            try
            {
                File.WriteAllLines(filename, lines);
            }
            catch (IOException ee)
            {
                R.ShowStopError(R.ExceptionToString(ee));
                return false;
            }
            return true;
        }

        static DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public static DateTime FromUnitTime(uint unixTime)
        {
            // UNIXエポックからの経過秒数で得られるローカル日付
            return UNIX_EPOCH.AddSeconds(unixTime).ToLocalTime();
        }
        //書き込み系でExceptionを出したくないので、エラーメッセージを表示に置き換える.

        //
        // 概要:
        //     新しいファイルを作成し、指定したバイト配列をそのファイルに書き込んだ後、ファイルを閉じます。既存のターゲット ファイルは上書きされます。
        //
        // パラメーター:
        //   path:
        //     書き込み先のファイル。
        //
        //   bytes:
        //     ファイルに書き込むバイト。
        //
        // 例外:
        //   System.ArgumentException:
        //     path が、長さが 0 の文字列であるか、空白しか含んでいないか、または System.IO.Path.InvalidPathChars で定義されている無効な文字を
        //     1 つ以上含んでいます。
        //
        //   System.ArgumentNullException:
        //     path が null か、バイト配列が空です。
        //
        //   System.IO.PathTooLongException:
        //     指定したパス、ファイル名、またはその両方がシステム定義の最大長を超えています。たとえば、Windows ベースのプラットフォームの場合、パスの長さは
        //     248 文字未満、ファイル名の長さは 260 文字未満である必要があります。
        //
        //   System.IO.DirectoryNotFoundException:
        //     指定したパスが無効です (割り当てられていないドライブであるなど)。
        //
        //   System.IO.IOException:
        //     ファイルを開くときに、I/O エラーが発生しました。
        //
        //   System.UnauthorizedAccessException:
        //     path によって、読み取り専用のファイルが指定されました。またはこの操作は、現在のプラットフォームではサポートされていません。またはpath によってディレクトリが指定されました。または呼び出し元に、必要なアクセス許可がありません。
        //
        //   System.IO.FileNotFoundException:
        //     path で指定されたファイルが見つかりませんでした。
        //
        //   System.NotSupportedException:
        //     path の形式が無効です。
        //
        //   System.Security.SecurityException:
        //     呼び出し元に、必要なアクセス許可がありません。
        [SecuritySafeCritical]
        public static void WriteAllBytes(string path, byte[] bytes)
        {
            try
            {
                File.WriteAllBytes(path, bytes);
            }
            catch (Exception e)
            {
                string error = R.ExceptionToString(e);
                R.ShowStopError(error);
            }
        }

        //
        // 概要:
        //     新しいファイルを作成し、文字列のコレクションをそのファイルに書き込んでから、そのファイルを閉じます。
        //
        // パラメーター:
        //   path:
        //     書き込み先のファイル。
        //
        //   contents:
        //     ファイルに書き込む行。
        //
        // 例外:
        //   System.ArgumentException:
        //     path が、長さが 0 の文字列であるか、空白しか含んでいないか、または System.IO.Path.GetInvalidPathChars()
        //     メソッドで定義されている無効な文字を 1 つ以上含んでいます。
        //
        //   System.ArgumentNullException:
        //      path  または contents が null です。
        //
        //   System.IO.DirectoryNotFoundException:
        //     pathが無効です (割り当てられていないドライブであるなど)。
        //
        //   System.IO.FileNotFoundException:
        //     path で指定されたファイルが見つかりませんでした。
        //
        //   System.IO.IOException:
        //     ファイルを開くときに、I/O エラーが発生しました。
        //
        //   System.IO.PathTooLongException:
        //     path がシステムで定義されている最大長を超えています。たとえば、Windows ベースのプラットフォームの場合、パスの長さは 248 文字未満、ファイル名の長さは
        //     260 文字未満である必要があります。
        //
        //   System.NotSupportedException:
        //     path の形式が無効です。
        //
        //   System.Security.SecurityException:
        //     呼び出し元に、必要なアクセス許可がありません。
        //
        //   System.UnauthorizedAccessException:
        //     path が、読み取り専用のファイルを指定します。またはこの操作は、現在のプラットフォームではサポートされていません。またはpath がディレクトリです。または呼び出し元に、必要なアクセス許可がありません。
        [SecuritySafeCritical]
        public static void WriteAllLines(string path, IEnumerable<string> contents)
        {
            try
            {
                File.WriteAllLines(path, contents);
            }
            catch (Exception e)
            {
                string error = R.ExceptionToString(e);
                R.ShowStopError(error);
            }
        }

        //
        // 概要:
        //     新しいファイルを作成し、指定した文字列配列をそのファイルに書き込んだ後、ファイルを閉じます。
        //
        // パラメーター:
        //   path:
        //     書き込み先のファイル。
        //
        //   contents:
        //     ファイルに書き込む文字列配列。
        //
        // 例外:
        //   System.ArgumentException:
        //     path が、長さが 0 の文字列であるか、空白しか含んでいないか、または System.IO.Path.InvalidPathChars で定義されている無効な文字を
        //     1 つ以上含んでいます。
        //
        //   System.ArgumentNullException:
        //     path または contents のいずれかが null です。
        //
        //   System.IO.PathTooLongException:
        //     指定したパス、ファイル名、またはその両方がシステム定義の最大長を超えています。たとえば、Windows ベースのプラットフォームの場合、パスの長さは
        //     248 文字未満、ファイル名の長さは 260 文字未満である必要があります。
        //
        //   System.IO.DirectoryNotFoundException:
        //     指定したパスが無効です (割り当てられていないドライブであるなど)。
        //
        //   System.IO.IOException:
        //     ファイルを開くときに、I/O エラーが発生しました。
        //
        //   System.UnauthorizedAccessException:
        //     path によって、読み取り専用のファイルが指定されました。またはこの操作は、現在のプラットフォームではサポートされていません。またはpath によってディレクトリが指定されました。または呼び出し元に、必要なアクセス許可がありません。
        //
        //   System.IO.FileNotFoundException:
        //     path で指定されたファイルが見つかりませんでした。
        //
        //   System.NotSupportedException:
        //     path の形式が無効です。
        //
        //   System.Security.SecurityException:
        //     呼び出し元に、必要なアクセス許可がありません。
        [SecuritySafeCritical]
        public static void WriteAllLines(string path, string[] contents)
        {
            try
            {
                File.WriteAllLines(path, contents);
            }
            catch (Exception e)
            {
                string error = R.ExceptionToString(e);
                R.ShowStopError(error);
            }
        }

        //
        // 概要:
        //     指定されたエンコーディングを使用して新しいファイルを作成し、文字列のコレクションをそのファイルに書き込んでから、そのファイルを閉じます。
        //
        // パラメーター:
        //   path:
        //     書き込み先のファイル。
        //
        //   contents:
        //     ファイルに書き込む行。
        //
        //   encoding:
        //     使用する文字エンコーディング。
        //
        // 例外:
        //   System.ArgumentException:
        //     path が、長さが 0 の文字列であるか、空白しか含んでいないか、または System.IO.Path.GetInvalidPathChars()
        //     メソッドで定義されている無効な文字を 1 つ以上含んでいます。
        //
        //   System.ArgumentNullException:
        //      path、 contents、または encoding が null です。
        //
        //   System.IO.DirectoryNotFoundException:
        //     pathが無効です (割り当てられていないドライブであるなど)。
        //
        //   System.IO.FileNotFoundException:
        //     path で指定されたファイルが見つかりませんでした。
        //
        //   System.IO.IOException:
        //     ファイルを開くときに、I/O エラーが発生しました。
        //
        //   System.IO.PathTooLongException:
        //     path がシステムで定義されている最大長を超えています。たとえば、Windows ベースのプラットフォームの場合、パスの長さは 248 文字未満、ファイル名の長さは
        //     260 文字未満である必要があります。
        //
        //   System.NotSupportedException:
        //     path の形式が無効です。
        //
        //   System.Security.SecurityException:
        //     呼び出し元に、必要なアクセス許可がありません。
        //
        //   System.UnauthorizedAccessException:
        //     path が、読み取り専用のファイルを指定します。またはこの操作は、現在のプラットフォームではサポートされていません。またはpath がディレクトリです。または呼び出し元に、必要なアクセス許可がありません。
        [SecuritySafeCritical]
        public static void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            try
            {
                File.WriteAllLines(path, contents,encoding);
            }
            catch (Exception e)
            {
                string error = R.ExceptionToString(e);
                R.ShowStopError(error);
            }
        }

        //
        // 概要:
        //     新しいファイルを作成し、指定されたエンコーディングで指定された文字列配列をそのファイルに書き込んでから、そのファイルを閉じます。
        //
        // パラメーター:
        //   path:
        //     書き込み先のファイル。
        //
        //   contents:
        //     ファイルに書き込む文字列配列。
        //
        //   encoding:
        //     文字列配列に適用された文字エンコーディングを表す System.Text.Encoding オブジェクト。
        //
        // 例外:
        //   System.ArgumentException:
        //     path が、長さが 0 の文字列であるか、空白しか含んでいないか、または System.IO.Path.InvalidPathChars で定義されている無効な文字を
        //     1 つ以上含んでいます。
        //
        //   System.ArgumentNullException:
        //     path または contents のいずれかが null です。
        //
        //   System.IO.PathTooLongException:
        //     指定したパス、ファイル名、またはその両方がシステム定義の最大長を超えています。たとえば、Windows ベースのプラットフォームの場合、パスの長さは
        //     248 文字未満、ファイル名の長さは 260 文字未満である必要があります。
        //
        //   System.IO.DirectoryNotFoundException:
        //     指定したパスが無効です (割り当てられていないドライブであるなど)。
        //
        //   System.IO.IOException:
        //     ファイルを開くときに、I/O エラーが発生しました。
        //
        //   System.UnauthorizedAccessException:
        //     path によって、読み取り専用のファイルが指定されました。またはこの操作は、現在のプラットフォームではサポートされていません。またはpath によってディレクトリが指定されました。または呼び出し元に、必要なアクセス許可がありません。
        //
        //   System.IO.FileNotFoundException:
        //     path で指定されたファイルが見つかりませんでした。
        //
        //   System.NotSupportedException:
        //     path の形式が無効です。
        //
        //   System.Security.SecurityException:
        //     呼び出し元に、必要なアクセス許可がありません。
        [SecuritySafeCritical]
        public static void WriteAllLines(string path, string[] contents, Encoding encoding)
        {
            try
            {
                File.WriteAllLines(path, contents,encoding);
            }
            catch (Exception e)
            {
                string error = R.ExceptionToString(e);
                R.ShowStopError(error);
            }
        }

        //
        // 概要:
        //     新しいファイルを作成し、指定した文字列をそのファイルに書き込んだ後、ファイルを閉じます。既存のターゲット ファイルは上書きされます。
        //
        // パラメーター:
        //   path:
        //     書き込み先のファイル。
        //
        //   contents:
        //     ファイルに書き込む文字列。
        //
        // 例外:
        //   System.ArgumentException:
        //     path が、長さが 0 の文字列であるか、空白しか含んでいないか、または System.IO.Path.InvalidPathChars で定義されている無効な文字を
        //     1 つ以上含んでいます。
        //
        //   System.ArgumentNullException:
        //     path が null であるか、contents が空です。
        //
        //   System.IO.PathTooLongException:
        //     指定したパス、ファイル名、またはその両方がシステム定義の最大長を超えています。たとえば、Windows ベースのプラットフォームの場合、パスの長さは
        //     248 文字未満、ファイル名の長さは 260 文字未満である必要があります。
        //
        //   System.IO.DirectoryNotFoundException:
        //     指定したパスが無効です (割り当てられていないドライブであるなど)。
        //
        //   System.IO.IOException:
        //     ファイルを開くときに、I/O エラーが発生しました。
        //
        //   System.UnauthorizedAccessException:
        //     path によって、読み取り専用のファイルが指定されました。またはこの操作は、現在のプラットフォームではサポートされていません。またはpath によってディレクトリが指定されました。または呼び出し元に、必要なアクセス許可がありません。
        //
        //   System.IO.FileNotFoundException:
        //     path で指定されたファイルが見つかりませんでした。
        //
        //   System.NotSupportedException:
        //     path の形式が無効です。
        //
        //   System.Security.SecurityException:
        //     呼び出し元に、必要なアクセス許可がありません。
        [SecuritySafeCritical]
        public static void WriteAllText(string path, string contents)
        {
            try
            {
                File.WriteAllText(path, contents);
            }
            catch (Exception e)
            {
                string error = R.ExceptionToString(e);
                R.ShowStopError(error);
            }
        }
        //
        // 概要:
        //     新しいファイルを作成し、指定したエンコーディングで指定の文字列をそのファイルに書き込んだ後、ファイルを閉じます。既存のターゲット ファイルは上書きされます。
        //
        // パラメーター:
        //   path:
        //     書き込み先のファイル。
        //
        //   contents:
        //     ファイルに書き込む文字列。
        //
        //   encoding:
        //     文字列に適用するエンコーディング。
        //
        // 例外:
        //   System.ArgumentException:
        //     path が、長さが 0 の文字列であるか、空白しか含んでいないか、または System.IO.Path.InvalidPathChars で定義されている無効な文字を
        //     1 つ以上含んでいます。
        //
        //   System.ArgumentNullException:
        //     path が null であるか、contents が空です。
        //
        //   System.IO.PathTooLongException:
        //     指定したパス、ファイル名、またはその両方がシステム定義の最大長を超えています。たとえば、Windows ベースのプラットフォームの場合、パスの長さは
        //     248 文字未満、ファイル名の長さは 260 文字未満である必要があります。
        //
        //   System.IO.DirectoryNotFoundException:
        //     指定したパスが無効です (割り当てられていないドライブであるなど)。
        //
        //   System.IO.IOException:
        //     ファイルを開くときに、I/O エラーが発生しました。
        //
        //   System.UnauthorizedAccessException:
        //     path によって、読み取り専用のファイルが指定されました。またはこの操作は、現在のプラットフォームではサポートされていません。またはpath によってディレクトリが指定されました。または呼び出し元に、必要なアクセス許可がありません。
        //
        //   System.IO.FileNotFoundException:
        //     path で指定されたファイルが見つかりませんでした。
        //
        //   System.NotSupportedException:
        //     path の形式が無効です。
        //
        //   System.Security.SecurityException:
        //     呼び出し元に、必要なアクセス許可がありません。
        [SecuritySafeCritical]
        public static void WriteAllText(string path, string contents, Encoding encoding)
        {
            try
            {
                File.WriteAllText(path,contents,encoding);
            }
            catch (Exception e)
            {
                string error = R.ExceptionToString(e);
                R.ShowStopError(error);
            }
        }

        //@1234 を解析.
        public static byte[] SkipAtMark(string str, uint pos, Encoding SJISEncoder)
        {
            Debug.Assert(str.Substring((int)pos, 1) == "@");
            uint len = (uint)str.Length;
            if (len - pos > 4)
            {
                len = 5 + pos;
            }

            uint i;
            for (i = pos + 1; i < len; i++)
            {
                char c = str[(int)i];
                if ((c >= '0' && c <= '9') || c >= 'a' && c <= 'f' || c >= 'A' && c <= 'F')
                {
                    continue;
                }

                break;
            }
            string key = str.Substring((int)pos, (int)(i - pos));
            byte[] sjisstr = SJISEncoder.GetBytes(key);
            return sjisstr;
        }
        public static string Reverse(string str)
        {
            char[] arr = str.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
        public static void Clipboard_SetText(string text)
        {
            string lastError = "";
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    Clipboard.SetText(text);
                    return;
                }
                catch (Exception e)
                {
                    lastError = R.ExceptionToString(e);
                    Log.Error(lastError); 
                }
            }
            R.ShowStopError(lastError);
        }
        public static void SetActiveControlSafety(Form f,Control c)
        {
            if (f.IsDisposed)
            {
                return;
            }
            if (!f.Visible)
            {
                return;
            }

            try
            {
                f.ActiveControl = c;
            }
            catch(Exception e)
            {
                Log.Error(R.ExceptionToString(e));
            }
        }

        //U+XXX
        public static string ToUnicode(uint code)
        {
            if (code == 0)
            {
                return "";
            }
            return ((char)code).ToString();
        }

        public static bool isURL(string text)
        {
            return RegexCache.IsMatch(text, "^https?://");
        }

        //https://dobon.net/vb/dotnet/file/copyfolder.html
        /// <summary>
        /// ディレクトリをコピーする
        /// </summary>
        /// <param name="sourceDirName">コピーするディレクトリ</param>
        /// <param name="destDirName">コピー先のディレクトリ</param>
        public static void CopyDirectory(
            string sourceDirName, string destDirName)
        {
            //コピー先のディレクトリがないときは作る
            if (!System.IO.Directory.Exists(destDirName))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(destDirName);
                    if (System.IO.Directory.Exists(sourceDirName))
                    {
                        //属性もコピー
                        System.IO.File.SetAttributes(destDirName,
                            System.IO.File.GetAttributes(sourceDirName));

                        //日付をコピー
                        CopyTimeStamp(sourceDirName, destDirName);
                    }
                }
                catch (Exception e)
                {
                    Log.Error("ディレクトリを作成できませんでした。\r\n{0}\r\n\r\n{1}", destDirName, R.ExceptionToString(e));
                }
            }

            //コピー先のディレクトリ名の末尾に"\"をつける
            if (destDirName[destDirName.Length - 1] !=
                    System.IO.Path.DirectorySeparatorChar)
            {
                destDirName = destDirName + System.IO.Path.DirectorySeparatorChar;
            }

            //コピー元のディレクトリにあるファイルをコピー
            string[] files = System.IO.Directory.GetFiles(sourceDirName);
            foreach (string file in files)
            {
                string destfilename = destDirName + System.IO.Path.GetFileName(file);
                try
                {
                    System.IO.File.Copy(file, destfilename, true);
                    //日付をコピー
                    CopyTimeStamp(file, destfilename);
                }
                catch (Exception e)
                {
                    Log.Error("ファイルをコピーできませんでした。\r\nSrc:{0}\r\nDest:{1}\r\n\r\n{2}", file, destfilename,R.ExceptionToString(e));
                }
            }

            //コピー元のディレクトリにあるディレクトリについて、再帰的に呼び出す
            string[] dirs = System.IO.Directory.GetDirectories(sourceDirName);
            foreach (string dir in dirs)
            {
                if (IsEmptyDirectory(dir))
                {
                    continue;
                }
                CopyDirectory(dir, destDirName + System.IO.Path.GetFileName(dir));
            }
        }
        //圧縮ファイルを展開した時に、単一ディレクトリを作られるなら、その内部だけをコピー
        public static void CopyDirectory1Trim(
            string sourceDirName, string destDirName)
        {
            string[] files = System.IO.Directory.GetFiles(sourceDirName);
            string[] dirs = System.IO.Directory.GetDirectories(sourceDirName);

            if (files.Length <= 0 && dirs.Length == 1)
            {//単一ディレクトリならば、その内部だけをコピーします.
                sourceDirName = Path.Combine(sourceDirName, dirs[0]);
            }
            CopyDirectory(sourceDirName,destDirName);
        }

        public static bool IsEmptyDirectory(string sourceDirName)
        {
            if (! Directory.Exists(sourceDirName))
            {
                return false;
            }
            string[] files = System.IO.Directory.GetFiles(sourceDirName);
            string[] dirs = System.IO.Directory.GetDirectories(sourceDirName);
            if (files.Length <= 0 && dirs.Length <= 0)
            {
                return true;
            }
            return false;
        }
        public static void DeleteFile(string dir , string filename, SearchOption option = SearchOption.TopDirectoryOnly)
        {
            string[] files = U.Directory_GetFiles_Safe(dir, filename, option);
            foreach (string name in files)
            {
                File.Delete(name);
            }
        }
        public static bool CopyFile(string src,string dest)
        {
            if (!File.Exists(src))
            {
                return false;
            }
            byte[] bin = File.ReadAllBytes(src);
            File.WriteAllBytes(dest, bin);
            return true;
        }
/*
        //高解像度対応
        [DllImport("gdi32.dll")]
        extern static int GetDeviceCaps(IntPtr hdc, int index);
        public static int GetSystemDpi()
        {
            const int HORZRES = 8;
            const int DESKTOPHORZRES = 118;

            try
            {
                using (Graphics screen = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hdc = screen.GetHdc();

                    int virtualWidth = GetDeviceCaps(hdc, HORZRES);
                    int physicalWidth = GetDeviceCaps(hdc, DESKTOPHORZRES);
                    screen.ReleaseHdc(hdc);

                    return (int)(96f * physicalWidth / virtualWidth);
                }
            }
            catch (Exception)
            {
                return 96;
            }
        }
*/
        public class FixDocsBugs
        {
            Size KeepSize;
            Form Form;
            public FixDocsBugs(Form f)
            {
                KeepSize = f.Size;
                foreach (Control c in f.Controls)
                {
                    int width = c.Left + c.Width;
                    int height = c.Top + c.Height;

                    if (KeepSize.Width < width)
                    {
                        KeepSize.Width = width;
                    }
                    if (KeepSize.Height < height)
                    {
                        KeepSize.Height = height;
                    }
                }
                Form = f;
            }
            public void AllowMaximizeBox()
            {
                this.Form.MaximizeBox = true;
                this.Form.Size = this.KeepSize;
            }
        }

        public static string GetCtrlKeyName()
        {
            return Keys.Control.ToString();
        }
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void SetFocusByProcess(Process p)
        {
            if (p == null)
            {
                return;
            }
            if (p.HasExited)
            {
                return;
            }
            if (p.MainWindowHandle == IntPtr.Zero)
            {
                return;
            }
            U.SetForegroundWindow(p.MainWindowHandle);
        }

        public static uint CalcCheckSUM(byte[] data)
        {
            uint result = 0;
            foreach (byte c in data)
            {
                result += c;
            }
            return result;
        }
        static public uint ConvertNUDToUint(NumericUpDown c)
        {
            if (c.Value >= 0)
            {
                return (uint)c.Value;
            }
            return (uint)((int)c.Value);
        }

        public static void SetActiveControl(Control c)
        {
            Form f = ControlToParentForm(c);
            if (f == null)
            {
                return;
            }
            f.ActiveControl = c;
        }
        public static List<string> DictionaryToValuesList(Dictionary<uint,string> d)
        {
            List<string> ret = new List<string>();
            var values = d.Values;
            foreach (var dd in values)
            {
                ret.Add(dd);
            }
            return ret;
        }
        public static string HexsToString(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(' ');
                sb.Append(data[i].ToString("X"));
            }
            if (sb.Length >= 1)
            {
                return sb.ToString(1,sb.Length - 1);
            }
            return sb.ToString();
        }
        public static byte[] StringToHexs(string text)
        {
            string[] sp = text.Split(' ');
            List<byte> data = new List<byte>();
            for (int i = 0; i < sp.Length; i++)
            {
                data.Add((byte)U.atoh(sp[i]));
            }
            return data.ToArray();
        }
        public static string ToString_StringBuilder(StringBuilder sb)
        {
            if (sb.Length <= 0)
            {
                return "";
            }
            return sb.ToString(1, sb.Length - 1);
        }
    }
}

