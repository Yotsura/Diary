using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using Dialy.Funcs;

namespace Dialy
{
    public class SearchWindowViewModel : INotifyPropertyChanged
    {
        public SortedDictionary<DateTime, string> _allDiaries;
        private int _indicateSize;
        public int IndicateSize
        {
            get => _indicateSize;
            set
            {
                _indicateSize = value;
                OnPropertyChanged(nameof(IndicateSize));
            }
        }

        private List<DateTime> _indicateList;
        public List<DateTime> IndicateList
        {
            get => _indicateList;
            set
            {
                _indicateList = value;
                OnPropertyChanged(nameof(IndicateList));
            }
        }

        private string _recordTxt;
        public string RecordTxt
        {
            get => _recordTxt;
            set
            {
                _recordTxt = value;
                OnPropertyChanged(nameof(RecordTxt));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SearchWindowViewModel(SortedDictionary<DateTime, string> allDiaries,int fontSize)
        {
            _allDiaries = allDiaries;
            IndicateSize = fontSize;
        }
        public void SearchFunc(String words, bool isRegSearch)
        {
            //除外検索　ORはできない　：AAA -AAAA -AAAB
            //()でくくらずにORと通常検索を併用しない
            //OR：A OR B
            //(A OR B OR C) D (E OR F)　半角()
            //(A B C) OR D
            //ワイルドカード	AA*BB

            var Searcher = new SearchFuncs(isRegSearch);

            words = words.Replace("　", " ");
            var kakkos = new Regex(@"(?<=\().*?(?=\))").Matches(words.Trim());
            var notkakkos = words;
            var result = _allDiaries.Select(x => (x.Key, x.Value));
            foreach(var kakko in kakkos)
            {
                notkakkos = notkakkos.Replace($"({kakko})", string.Empty);
                //()内の検索
                var word = kakko.ToString().Contains(" OR ") ?
                    kakko.ToString().Split(new string[] { " OR " }, StringSplitOptions.RemoveEmptyEntries) :
                    kakko.ToString().Split(new string[] { " "}, StringSplitOptions.RemoveEmptyEntries);
                if(kakko.ToString().Contains(" OR "))
                {
                    result = Searcher.OrSearcher(result,word);
                }
                else
                {
                    result = Searcher.AndSearcher(result, word.Where(x => !x.StartsWith("-")), word.Where(x => x.StartsWith("-")));
                }
            }

            var targetWords = notkakkos.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Where(x => !string.IsNullOrEmpty(x));
            var include2 = targetWords.Where(x => !x.StartsWith("-")).ToList();
            var exclude2 = targetWords.Where(x => x.StartsWith("-")).ToList();
            result = Searcher.AndSearcher(result, include2, exclude2);

            //IndicateList = orsearch ? OrSearcher(targetWords) : AndSearcher(targetWords);
            if (result.Count() > 0)
                IndicateList = result.Select(x => x.Key).OrderByDescending(x => x).ToList();
            else
                IndicateList = new List<DateTime>();
            //IndicateList.Reverse();
        }

    }
}
