using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialy
{
    class SecondWindowViewModel : INotifyPropertyChanged
    {
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

        private DateTime _indicateDate;
        public DateTime IndicateDate
        {
            get => _indicateDate;
            set
            {
                _indicateDate = value;
                OnPropertyChanged(nameof(IndicateDate));
            }
        }

        private string _dialyTxt;
        public string DiaryTxt
        {
            get => _dialyTxt;
            set
            {
                _dialyTxt = value;
                OnPropertyChanged(nameof(DiaryTxt));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SecondWindowViewModel(DateTime date, string diaryTxt, int fontSize)
        {
            IndicateSize = fontSize;
            IndicateDate = date;
            DiaryTxt = diaryTxt;
        }
    }
}
