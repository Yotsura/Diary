using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dialy.Funcs
{
    public class SearchFuncs
    {
        bool _isRegSearch;
        public SearchFuncs(bool isRegSearch)
        {
            _isRegSearch = isRegSearch;
        }
        public IEnumerable<(DateTime date, string value)> OrSearcher(IEnumerable<(DateTime date, string value)> origlist, IEnumerable<string> targetWords)
        {
            var hitList = new List<(DateTime, string)>();
            foreach (var word in targetWords)
            {
                var temp = GetHitList(origlist, word);
                //temp.Keys.ToList().ForEach(key => hitList[key] = temp[key]);
                //temp.Key.AsParallel().ForAll(key => hitList[key] = temp[key]);
                hitList.AddRange(temp);
            }
            return hitList.Distinct().OrderBy(x=>x.Item1);
        }

        //public static IEnumerable<(DateTime, string)> AndSearcher(this IEnumerable<(DateTime, string)> origlist, IEnumerable<string> targetWords)
        //{
        //    var hitList = origlist.AndSearcher(targetWords, null);
        //    return hitList;
        //}
        public IEnumerable<(DateTime date, string value)> AndSearcher(IEnumerable<(DateTime date, string value)> origlist,
            IEnumerable<string> targetWords, IEnumerable<string> excludeWords)
        {
            var hitList = origlist;
            foreach (var word in targetWords)
            {
                hitList = GetHitList(hitList, word);
            }
            foreach(var word in excludeWords.Select(x=>x.TrimStart('-')).Where(x=>!string.IsNullOrEmpty(x)))
            {
                hitList = hitList.Where(date => date.value.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) == -1);
                //hitList = hitList.Where(x => !x.Item2.Contains(word));
            }
            return hitList;
        }

        public IEnumerable<(DateTime date, string value)> GetHitList(IEnumerable<(DateTime date, string value)> hitList, string word)
        {
            List<(DateTime, string)> result;
            if (_isRegSearch)
            {
                var regTxt = (word.Contains("*") && !word.Contains(".*?")) ? word.Replace("*", ".*?") : word;
                result = new List<(DateTime, string)>(hitList.Where(drecordte => new Regex(regTxt).IsMatch(drecordte.value)));
            }
            else
            {
                result = new List<(DateTime, string)>(hitList
                    .Where(date => date.value.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) != -1));
            }
            return result;
        }
    }
}
