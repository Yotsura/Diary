using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using System.Windows.Media;
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

        private string _searchWords = string.Empty;
        public string SearchWords
        {
            get => _searchWords;
            set
            {
                _searchWords = value;
                OnPropertyChanged(nameof(SearchWords));
            }
        }

        private bool _isRegSearch;
        public bool IsRegSearch
        {
            get => _isRegSearch;
            set
            {
                _isRegSearch = value;
                OnPropertyChanged(nameof(IsRegSearch));
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

        private FlowDocument _Document = CreateFlowDoc("検索値をハイライト表示します。");
        public FlowDocument Document
        {
            get => _Document;
            set
            {
                _Document = value;
                OnPropertyChanged(nameof(Document));
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

        public void SearchFunc()
        {
            var searcher = new SearchFuncs(IsRegSearch);
            SearchWords = string.Join(" ", SearchWords.Split(new string[] { " ", "　", "\t" }, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrEmpty(x)));
            
            var kakkos = new Regex(@"((?<=\s\().*?\s+?.*?(?=\)))|((?<=\().*?\s+?.*?(?=\)\s))|((?<=\s\().*?\s+?.*?(?=\)\s))").Matches(SearchWords);
            var notkakkos = SearchWords;
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
            AddSearchLog(SearchWords);
        }

        public void IndicateRecord(DateTime date)
        {
            var data = _allDiaries[date];
            if (date == null)
                Document = new FlowDocument();
            else
            {
                if (SearchWords == string.Empty)
                    Document = CreateFlowDoc(data);
                else
                {
                    var test = new Model.SearchResult(_isRegSearch,_searchWords,data).GetRuns();
                    Document = CreateFlowDoc(test);
                }
            }
        }

        private static FlowDocument CreateFlowDoc(string innerText)
        {
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run(innerText));
            var result = new FlowDocument(paragraph);
            result.PageWidth = 2000;
            return result;
        }
        private static FlowDocument CreateFlowDoc(List<Run> runs)
        {
            var paragraph = new Paragraph();
            paragraph.Inlines.AddRange(runs);
            var result = new FlowDocument(paragraph);
            result.PageWidth = 2000;
            return result;
        }
    }
}
