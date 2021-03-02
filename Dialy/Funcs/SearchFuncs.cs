using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dialy.Funcs
{
    public static class SearchFuncs
    {
        public static IEnumerable<(DateTime date, string value)> Search(IEnumerable<(DateTime date, string value)> origlist, string txt, bool isRegSearch)
        {
            var word = SplitWords(txt);
            var result = txt.Contains(" OR ") ?
                OrSearcher(origlist, word, isRegSearch) : AndSearcher(origlist, word, isRegSearch);
            return result;
        }

        public static IEnumerable<string> GetAllSearchWords(string txt)
        {
            var kakkos = txt.GetKakkoWords();
            var notkakkos = txt;
            var words = new List<string>();
            foreach (var kakko in kakkos)
            {
                notkakkos = notkakkos.Replace($"({kakko})", string.Empty).Replace("  ", " ").Trim();
                words.AddRange(SearchFuncs.SplitWords(kakko.ToString()));
            }
            words.AddRange(SearchFuncs.SplitWords(notkakkos));

            var result = words.Where(x => !x.StartsWith("-"));
            return words;
        }

        public static IEnumerable<string> GetKakkoWords(this string origTxt)
        {
            //var reg = new Regex(@"(?<=\s\().*?\s+?.*?(?=\))");
            var reg = new Regex(@"((?<=\s\().*?\s+?.*?(?=\)))|((?<=\().*?\s+?.*?(?=\)\s))|((?<=\s\().*?\s+?.*?(?=\)\s))");
            //var test = reg.IsMatch(origTxt);
            var kakkos = reg.Matches(origTxt).Cast<Match>().Select(x => x.Value);
            return kakkos;
        }

        public static IEnumerable<string> SplitWords(string txt)
        {
            var word = txt.Contains(" OR ") ?
                    txt.Split(new string[] { " OR " }, StringSplitOptions.RemoveEmptyEntries) :
                    txt.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var result = word.Where(x => !string.IsNullOrEmpty(x));
            return result;
        }

        public static IEnumerable<(DateTime date, string value)> OrSearcher(IEnumerable<(DateTime date, string value)> origlist, IEnumerable<string> targetWords,bool isRegSearch)
        {
            var hitList = new List<(DateTime, string)>();
            foreach (var word in targetWords)
            {
                var temp = GetHitList(origlist, word, true, isRegSearch);
                hitList.AddRange(temp);
            }
            return hitList.Distinct().OrderBy(x => x.Item1);
        }

        public static IEnumerable<(DateTime date, string value)> AndSearcher(IEnumerable<(DateTime date, string value)> origlist, IEnumerable<string> targetWords,bool isRegSearch)
        {
            var include = targetWords.Where(x => !x.StartsWith("-")).ToList();
            var exclude = targetWords.Where(x => x.StartsWith("-")).ToList();

            var hitList = origlist;
            foreach (var word in include)
            {
                hitList = GetHitList(hitList, word, true,isRegSearch);
            }
            foreach (var word in exclude.Select(x => x.TrimStart('-')).Where(x => !string.IsNullOrEmpty(x)))
            {
                hitList = GetHitList(hitList, word, false,isRegSearch);
            }
            return hitList;
        }

        public static IEnumerable<(DateTime date, string value)> GetHitList(IEnumerable<(DateTime date, string value)> hitList, string word, bool isInclude,bool isRegSearch)
        {
            var result = isRegSearch ?
                hitList.Where(record => new Regex(word).IsMatch(record.value) == isInclude) :
                hitList.Where(record => Ishit(record.value, word) == isInclude);
            return result;
        }
        public static bool Ishit(string record, string word)
        {
            var hitIdx = record.IndexOf(word, StringComparison.CurrentCultureIgnoreCase);
            return hitIdx != -1;
        }
    }
}