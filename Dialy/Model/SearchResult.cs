using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace Dialy.Model
{
    public class CharInfo
    {
        public char Val { get; set; }
        public bool IsHit { get; set; } = false;
        public CharInfo(char val)
        {
            Val = val;
        }
    }
    public class SearchResult
    {
        bool _isRegSearch;
        string _origTxt;
        List<CharInfo> _origTxtInfo;

        IEnumerable<string> _searchWords;
        DateTime Date { get; set; }
        FlowDocument Val { get; set; }

        public SearchResult(bool isRegSearch, string searchWords, string origTxt)
        {
            //検索ワードで-になっていない物すべてハイライトする
            _isRegSearch = isRegSearch;
            _origTxt = origTxt;
            _origTxtInfo = _origTxt.Select(x => new CharInfo(x)).ToList();
            _searchWords = GetSearchWords(searchWords);

            foreach(var word in _searchWords)
            {
                if (_isRegSearch)
                    RegHitCheck(word);
                else
                    HitCheck(word);

            }
        }

        static IEnumerable<string> GetSearchWords(string txt)
        {
            var kakkos = new Regex(@"((?<=\s\().*?\s+?.*?(?=\)))|((?<=\().*?\s+?.*?(?=\)\s))|((?<=\s\().*?\s+?.*?(?=\)\s))").Matches(txt);
            var notkakkos = txt;
            var words = SplitWords(notkakkos).ToList();
            foreach (var kakko in kakkos)
            {
                notkakkos = notkakkos.Replace($"({kakko})", string.Empty).Replace("  ", " ").Trim();
                words.AddRange(SplitWords(kakko.ToString()));
            }
            var result = words.Where(x => !x.StartsWith("-"));
            return words;
        }

        static IEnumerable<string> SplitWords(string txt)
        {
            var word = txt.Contains(" OR ") ?
                    txt.Split(new string[] { " OR " }, StringSplitOptions.RemoveEmptyEntries) :
                    txt.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            var result = word.Where(x => !string.IsNullOrEmpty(x));
            return result;
        }

        public void HitCheck(string word)
        {

        }

        public void RegHitCheck(string word)
        {
            var hits = new Regex(word).Matches(_origTxt);

            foreach(Match hit in hits)
            {
                var idx = hit.Index;
                var len = hit.Value.Length;
                for(var i=idx;i<idx+len;i++)
                {
                    _origTxtInfo[i].IsHit = true;
                }
            }
            return;
        }

        public List<Run> GetRuns()
        {
            var result = new List<Run>();
            var temp = string.Empty;
            var isHit = _origTxtInfo.First().IsHit;
            foreach (var c in _origTxtInfo)
            {
                if (isHit != c.IsHit)
                {
                    result.Add(isHit ?
                        new Run(temp) { Background = new SolidColorBrush(Colors.YellowGreen), Foreground = new SolidColorBrush(Colors.Black) } :
                        new Run(temp)
                        );
                    temp = c.Val.ToString();
                    isHit = c.IsHit;
                }
                else
                {
                    temp += c.Val;
                }
            }
            if (temp != string.Empty)
                result.Add(isHit ?
                    new Run(temp) { Background = new SolidColorBrush(Colors.YellowGreen), Foreground = new SolidColorBrush(Colors.Black) } :
                    new Run(temp)
                    );

            return result;
        }
    }
}
