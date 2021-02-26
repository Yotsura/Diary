using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dialy.Funcs
{
    public static class StringExtension
    {
        public static IEnumerable<string> GetKakkoWords(this string origTxt)
        {
            //var reg = new Regex(@"(?<=\s\().*?\s+?.*?(?=\))");
            var reg = new Regex(@"((?<=\s\().*?\s+?.*?(?=\)))|((?<=\().*?\s+?.*?(?=\)\s))|((?<=\s\().*?\s+?.*?(?=\)\s))");
            //var test = reg.IsMatch(origTxt);
            var kakkos = reg.Matches(origTxt).Cast<Match>().Select(x => x.Value);
            return kakkos;
        }
    }
}
