using System;
using System.Collections.Generic;
using System.Text;

namespace FEBuilderGBA
{
    public class ToolTextCharRecreate
    {
        Dictionary<uint, CharCounter> CharCounterMap = new Dictionary<uint, CharCounter>();
	    public class CharCounter
	    {
		    public uint mojiBin; //keyと同じ
		    public int count;
		    public int length;
	    }

	    public void Add(string str)
	    {
            Program.FETextEncoder.StringCount(str, CharCounterMap);
	    }
        public void AddEN(string str)
        {
            Program.FETextEncoder.StringCountEN(str, CharCounterMap);
        }

        public List<CharCounter> GetSortedList()
	    {
            List<CharCounter> list = new List<CharCounter>(CharCounterMap.Values);
            list.Sort((a, b) => { return b.count != a.count ? (b.count) - (a.count) : (b.length) - (a.length); });
            return list;
	    }
    }
}
