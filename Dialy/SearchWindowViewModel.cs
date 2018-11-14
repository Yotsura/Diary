using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialy
{
    public class SearchWindowViewModel : INotifyPropertyChanged
    {
        SortedDictionary<DateTime, string> _allDiaries;

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

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SearchWindowViewModel(SortedDictionary<DateTime, string> allDiaries)
        {
            _allDiaries = allDiaries;
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

        private SortedDictionary<DateTime, string> GetHitList
            (SortedDictionary<DateTime, string> population, string target)
        {
            var result = new SortedDictionary<DateTime, string>();
            foreach (var date in population)
            {
                if (date.Value.IndexOf(target, StringComparison.CurrentCultureIgnoreCase) == -1) continue;
                result.Add(date.Key, date.Value);
            }
            return result;
        }

    }
}
