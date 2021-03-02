using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using System.Windows.Media;
using Dialy.Funcs;

namespace Dialy.Model
{
    public class SearchResult
    {
        string _origTxt;
        List<CharInfo> _recordTxtInfo;

        public SearchResult(bool isRegSearch, string searchWords, string origTxt)
        {
            //検索ワードで-になっていない物すべてハイライトする
            _origTxt = origTxt;
            _recordTxtInfo = _origTxt.Select(x => new CharInfo(x)).ToList();
            foreach (var word in SearchFuncs.GetAllSearchWords(searchWords))
            {
                if (isRegSearch)
                    RegHitCheck(word);
                else
                    HitCheck(word);
            }
        }

        void HitCheck(string word)
        {
            var hits = GethitIdxs(_origTxt, word);
            var len = word.Length;
            foreach (var idx in hits)
            {
                for (var i = idx; i < idx + len; i++)
                {
                    _recordTxtInfo[i].IsHit = true;
                }
            }
        }

        IEnumerable<int> GethitIdxs(string origTxt, string word)
        {
            var result = new List<int>();
            var searchRange = origTxt;
            if (string.IsNullOrEmpty(origTxt) || string.IsNullOrEmpty(word))
                return result;
            var hit = searchRange.IndexOf(word, StringComparison.CurrentCultureIgnoreCase);
            if (hit == -1)
                return result;

            result.Add(hit);
            var omitLength = hit + word.Length;
            searchRange = searchRange.Substring(omitLength);

            var temp = GethitIdxs(searchRange, word).Select(x => x += omitLength);
            result.AddRange(temp);
            return result;
        }

        void RegHitCheck(string word)
        {
            var hits = new Regex(word).Matches(_origTxt);
            foreach (Match hit in hits)
            {
                var len = hit.Value.Length;
                var idx = hit.Index;
                for (var i = idx; i < idx + len; i++)
                {
                    _recordTxtInfo[i].IsHit = true;
                }
            }
            return;
        }

        public List<Run> GetRuns()
        {
            var result = new List<Run>();
            var temp = string.Empty;
            var isHit = _recordTxtInfo.First().IsHit;
            foreach (var c in _recordTxtInfo)
            {
                if (isHit != c.IsHit)
                {
                    result.Add(isHit ?
                        new Run(temp) { Background = new SolidColorBrush(Colors.YellowGreen), Foreground = new SolidColorBrush(Colors.Black) } :
                        new Run(temp));
                    temp = c.Val.ToString();
                    isHit = c.IsHit;
                }
                else
                {
                    temp += c.Val;
                }
            }
            if (!temp.IsNullOrEmpty())
                result.Add(isHit ?
                    new Run(temp) { Background = new SolidColorBrush(Colors.YellowGreen), Foreground = new SolidColorBrush(Colors.Black) } :
                    new Run(temp));

            return result;
        }
    }
}
