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

        private TaskRecord _task;
        public TaskRecord Task
        {
            get => _task;
            set
            {
                _task = value;
                OnPropertyChanged(nameof(Task));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TaskWindowViewModel(string folderPath, int fontSize)
        {
            IndicateSize = fontSize;
            Task = new TaskRecord(folderPath);
            try
            {
                Task.OpenTaskFile();
            }
            catch
            {
                var oldfile = Task.Filepath.Replace("taskTxt.log", $"{DateTime.Now.ToString("yyyyMMdd")}taskTxt.log");
                System.IO.File.Copy(Task.Filepath, oldfile);
                Task.Txt = $"データファイルの展開に失敗。\r\n旧データを退避しました。\r\n＜ファイルパス＞\r\n{oldfile}";
            }
        }
    }
}
