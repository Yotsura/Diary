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
        SortedDictionary<DateTime, String> _hitList;

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

        public void SearchFunc(String words, bool perfectmatch)
        {
            var targetWords = words.Split(new string[] { " ", "　" }, StringSplitOptions.RemoveEmptyEntries);
            _hitList = _allDiaries;
            foreach (var word in targetWords)
            {
                _hitList = GetHitList(_hitList, word);
            }
            IndicateList = _hitList.Keys.ToList();
            IndicateList.Reverse();
        }

        public SortedDictionary<DateTime, string> GetHitList
            (SortedDictionary<DateTime, string> population, string target)
        {
            var result = new SortedDictionary<DateTime, string>();
            foreach (var date in population)
            {
                if (date.Value.IndexOf(target, StringComparison.CurrentCultureIgnoreCase) == -1) continue;
                result.Add(date.Key, date.Value);
            }
            return result;
            //return population.Where(x => x.Value.IndexOf(target) != -1);
        }

    }
}
