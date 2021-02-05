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

        private List<string> _searchLog;
        public List<string> SearchLog
        {
            get
            {
                var temp = new List<string>(_searchLog);
                temp.Reverse();
                return temp;
            }
            set
            {
                _searchLog = value;
                Settings.Default.SearchLog = _searchLog;
                Settings.Default.Save();
                OnPropertyChanged(nameof(SearchLog));
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
            _searchLog = Settings.Default.SearchLog != null ? Settings.Default.SearchLog : new List<string>();
        }

        public void AddSearchLog(string word)
        {
            if (string.IsNullOrEmpty(word)) return;
            var temp = new List<string>(_searchLog.Where(x => x != word));
            temp.Add(word);
            if (temp.Count > Settings.Default.SearchLogLimit)
                temp.RemoveFirst(temp.Count - Settings.Default.SearchLogLimit);
            SearchLog = temp;
        }

        public void SearchFunc(string origwords, bool isRegSearch)
        {
            var searcher = new SearchFuncs(isRegSearch);
            var words = string.Join(" ", origwords.Split(new string[] { " ", "　", "\t" }, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrEmpty(x)));
            
            var kakkos = new Regex(@"(?<=\().*?(?=\))").Matches(words);
            var notkakkos = words;
            var result = _allDiaries.Select(x => (x.Key, x.Value));
            foreach(var kakko in kakkos)
            {
                notkakkos = notkakkos.Replace($"({kakko})", string.Empty).Replace("  ", " ").Trim();
                //()内の検索
                result = searcher.Search(result, kakko.ToString());
            }

            result = searcher.Search(result, notkakkos);

            IndicateList = result.Count() > 0 ?
                result.Select(x => x.Key).OrderByDescending(x => x).ToList() :
                new List<DateTime>();
            AddSearchLog(words);
        }
    }
}
