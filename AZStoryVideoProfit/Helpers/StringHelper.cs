using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.Helpers
{
    public class StringHelper
    {
        public static int CountWords(string s)
        {
            MatchCollection collection = Regex.Matches(s, @"[\S]+");
            //MatchCollection collection = Regex.Matches(s, @"\w+");
            return collection.Count;
        }
    }
}
