using System.Text.RegularExpressions;

namespace AZBinaryProfit.MainApi.Helpers
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
