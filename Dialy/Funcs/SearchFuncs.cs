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

        public IEnumerable<(DateTime date, string value)> Search(IEnumerable<(DateTime date, string value)> origlist, string txt)
        {
            var word = GetSearchWords(txt);
            var result = txt.Contains(" OR ") ?
                OrSearcher(origlist, word) : AndSearcher(origlist, word);
            return result;
        }
        static IEnumerable<string> GetSearchWords(string txt)
        {
            var word = txt.Contains(" OR ") ?
                    txt.Split(new string[] { " OR " }, StringSplitOptions.RemoveEmptyEntries) :
                    txt.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var result = word.Where(x => !string.IsNullOrEmpty(x));
            return result;
        }

        public IEnumerable<(DateTime date, string value)> OrSearcher(IEnumerable<(DateTime date, string value)> origlist, IEnumerable<string> targetWords)
        {
            var hitList = new List<(DateTime, string)>();
            foreach (var word in targetWords)
            {
                var temp = GetHitList(origlist, word, true);
                hitList.AddRange(temp);
            }
            return hitList.Distinct().OrderBy(x => x.Item1);
        }

        public IEnumerable<(DateTime date, string value)> AndSearcher(IEnumerable<(DateTime date, string value)> origlist, IEnumerable<string> targetWords)
        {
            var include = targetWords.Where(x => !x.StartsWith("-")).ToList();
            var exclude = targetWords.Where(x => x.StartsWith("-")).ToList();

            var hitList = origlist;
            foreach (var word in include)
            {
                hitList = GetHitList(hitList, word, true);
            }
            foreach (var word in exclude.Select(x => x.TrimStart('-')).Where(x => !string.IsNullOrEmpty(x)))
            {
                hitList = GetHitList(hitList, word, false);
            }
            return hitList;
        }

        public IEnumerable<(DateTime date, string value)> GetHitList(IEnumerable<(DateTime date, string value)> hitList, string word, bool isInclude)
        {
            var result = _isRegSearch ?
                hitList.Where(record => new Regex(word).IsMatch(record.value) == isInclude) :
                hitList.Where(record => Ishit(record.value, word) == isInclude);
            return result;
        }
        public bool Ishit(string record, string word)
        {
            var hitIdx = record.IndexOf(word, StringComparison.CurrentCultureIgnoreCase);
            return hitIdx != -1;
        }
    }
}