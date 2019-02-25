using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Dialy
{
    public class SearchWindowViewModel : INotifyPropertyChanged
    {
        public SortedDictionary<DateTime, string> _allDiaries;
        public int _fontSize;

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
            _fontSize = fontSize;
        }

        public void SearchFunc(String words, bool orsearch)
        {
            var targetWords = words.Split(new string[] { " ", "　" }, StringSplitOptions.RemoveEmptyEntries);
            IndicateList = orsearch ? OrSearcher(targetWords) : AndSearcher(targetWords);
            IndicateList.Reverse();
        }

        private List<DateTime> OrSearcher(string[] targetWords)
        {
            var hitList = new SortedDictionary<DateTime, string>();
            foreach (var word in targetWords)
            {
                var temp = GetHitList(_allDiaries, word);
                temp.Keys.ToList().ForEach(key => hitList[key] = temp[key]);
            }
            return hitList.Keys.ToList();
        }

        private List<DateTime> AndSearcher(string[] targetWords)
        {
            var hitList = _allDiaries;
            foreach (var word in targetWords)
            {
                hitList = GetHitList(hitList, word);
            }
            return hitList.Keys.ToList();
        }

        private SortedDictionary<DateTime, string> GetHitList(SortedDictionary<DateTime, string> hitList, string word)
        {
            return new SortedDictionary<DateTime, string>(hitList
                .Where(date => date.Value.IndexOf(word, StringComparison.CurrentCultureIgnoreCase) != -1)
                .ToDictionary(x => x.Key, x => x.Value));
        }

    }
}
