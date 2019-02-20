using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialy
{
    public class TaskWindowViewModel : INotifyPropertyChanged
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

        private string _taskTxt;
        public string TaskTxt
        {
            get => _taskTxt;
            set
            {
                _taskTxt = value;
                OnPropertyChanged(nameof(TaskTxt));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TaskWindowViewModel(string taskTxt, int fontSize)
        {
            IndicateSize = fontSize;
            TaskTxt = taskTxt;
        }
    }
}
